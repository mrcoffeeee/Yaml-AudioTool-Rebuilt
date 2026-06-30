using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NAudio.Wave;
using Vortice.XAudio2;
using NAudio.CoreAudioApi;

namespace Yaml_AudioTool_Rebuilt
{
    public class AudioPlayback
    {
        public bool playbackPause = false;
        public bool playbackStop = true;
        public IntPtr audioDataPtr = IntPtr.Zero;

        public MMDeviceEnumerator enumerator;
        public MMDevice device;

        public IXAudio2 xaudio2;
        public AudioBuffer audioBuffer;        
        public IXAudio2SourceVoice sourceVoice;
        public IXAudio2MasteringVoice masteringVoice;
        public IDisposable eqEffect;
        public IDisposable echoEffect;
        public IDisposable reverbEffect;
        public IDisposable limiterEffect;

        public IDisposable masterVolumeMeter;
        private float[] peakLevels;
        private float[] rmsLevels;
        const float rmsSmoothFactor = 0.07f; // lower values for more smoothing, higher for more direct showing
        const float peakSmoothFactor = 0.4f;
        private float smoothedRms = 0;
        private float smoothedPeak = 0;
        public uint masterChannelCount = 2;

        public void Initialize()
        {
            if (xaudio2 != null) return;

            enumerator = new MMDeviceEnumerator();
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            xaudio2 = XAudio2.XAudio2Create(ProcessorSpecifier.UseDefaultProcessor);
            masteringVoice = xaudio2.CreateMasteringVoice(2, 48000);
            xaudio2.StartEngine();

            // Bind VolumeMeter to MasteringVoice (Index 0 of chain)
            Vortice.XAudio2.Fx.Fx.XAudio2CreateVolumeMeter(out var meterUnknown);
            masterVolumeMeter = meterUnknown;

            var meterDescriptor = new EffectDescriptor(meterUnknown, masterChannelCount);
            masteringVoice.SetEffectChain(meterDescriptor);
            masteringVoice.EnableEffect(0);

            peakLevels = new float[masterChannelCount];
            rmsLevels = new float[masterChannelCount];
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct VolumeMeterLevelsNative
        {
            public IntPtr PeakLevels;
            public IntPtr RMSLevels;
            public uint ChannelCount;
        }

        public (float peak, float rms) GetMasterLevels()
        {
            if (masteringVoice == null || peakLevels == null || rmsLevels == null)
                return (0, 0);

            GCHandle peakHandle = default;
            GCHandle rmsHandle = default;

            try
            {
                // pin float-arrays to make xaudio to write into unmanaged memory
                peakHandle = GCHandle.Alloc(peakLevels, GCHandleType.Pinned);
                rmsHandle = GCHandle.Alloc(rmsLevels, GCHandleType.Pinned);

                var native = new VolumeMeterLevelsNative
                {
                    PeakLevels = peakHandle.AddrOfPinnedObject(),
                    RMSLevels = rmsHandle.AddrOfPinnedObject(),
                    ChannelCount = masterChannelCount
                };

                // Hand over native struct as Span<byte>
                Span<byte> buffer = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref native, 1));
                masteringVoice.GetEffectParameters(0, buffer);

                // Fill peakLevels and rmsLevels with actual values
                float maxPeak = 0;
                float maxRms = 0;
                for (int i = 0; i < masterChannelCount; i++)
                {
                    if (peakLevels[i] > maxPeak) maxPeak = peakLevels[i];
                    if (rmsLevels[i] > maxRms) maxRms = rmsLevels[i];
                }

                smoothedPeak += peakSmoothFactor * (maxPeak - smoothedPeak);
                smoothedRms += rmsSmoothFactor * (maxRms - smoothedRms);

                if (smoothedPeak < 0.001f) smoothedPeak = 0;
                if (smoothedRms < 0.001f) smoothedRms = 0;

                return (smoothedPeak, smoothedRms);
            }
            catch
            {
                return (0, 0);
            }
            finally
            {
                // Free pins always
                if (peakHandle.IsAllocated) peakHandle.Free();
                if (rmsHandle.IsAllocated) rmsHandle.Free();
            }
        }

        public void MeterDecaying()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            smoothedRms += rmsSmoothFactor * (0 - smoothedRms);
            smoothedPeak += peakSmoothFactor * (0 - smoothedPeak);

            if (smoothedRms < 0.001f) smoothedRms = 0;
            if (smoothedPeak < 0.001f) smoothedPeak = 0;

            f1.MainVolumePeakMeter.Amplitude = smoothedPeak;
            f1.MainVolumeRMSMeter.Amplitude = smoothedRms;

            if (smoothedRms == 0 && smoothedPeak == 0)
            {
                f1.meterDecaying = false;
                f1.playbackTimer.Stop();
            }
            return;
        }

        public static string CalculateAudiolength(WaveFileReader waveFileReader)
        {
            int hours = waveFileReader.TotalTime.Hours;
            if (hours > 0)
            {
                hours *= 60;
            }
            int minutes = waveFileReader.TotalTime.Minutes;
            minutes += hours;
            int seconds = waveFileReader.TotalTime.Seconds;
            string time = minutes.ToString("D2") + ":" + seconds.ToString("D2");
            return time;
        }

        private static Vortice.Multimedia.WaveFormatEncoding MapEncoding(NAudio.Wave.WaveFormatEncoding naudioEncoding)
        {
            return naudioEncoding switch
            {
                NAudio.Wave.WaveFormatEncoding.Pcm => Vortice.Multimedia.WaveFormatEncoding.Pcm,
                NAudio.Wave.WaveFormatEncoding.IeeeFloat => Vortice.Multimedia.WaveFormatEncoding.IeeeFloat,
                NAudio.Wave.WaveFormatEncoding.Extensible => Vortice.Multimedia.WaveFormatEncoding.Extensible,
                NAudio.Wave.WaveFormatEncoding.Adpcm => Vortice.Multimedia.WaveFormatEncoding.Adpcm,
                _ => throw new NotSupportedException($"Audio-Encoding '{naudioEncoding}' wird nicht unterstützt.")
            };
        }

        public void OpenFile(bool clickFlag)
        {            
            StopPlayback();

            SettingsDialog sd = new();
            OpenFileDialog openfiledialog = new()
            {
                InitialDirectory = sd.audiofolderLabel.Text,
                Multiselect = true,
                Filter = "WAV Files|*.wav"
            };

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openfiledialog.FileNames)
                {
                    try
                    {
                        using var reader = new WaveFileReader(file);

                        Form1 f1 = (Form1)Application.OpenForms["Form1"];

                        if (clickFlag == true)
                        {
                            string tempString = sd.audiofolderLabel.Text + "\\";
                            f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.filenameHeader)].Text = file.Replace(tempString, "").Replace("\\", "/").Replace(".wav", "");
                            f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.filepathHeader)].Text = file;
                            f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.sizeHeader)].Text = (reader.Length / 1000).ToString();
                            f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.durationHeader)].Text = CalculateAudiolength(reader);
                            f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.channelsHeader)].Text = reader.WaveFormat.Channels.ToString();
                            f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.samplerateHeader)].Text = Math.Round(reader.WaveFormat.SampleRate / 1000.0, 3).ToString();
                            f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.bitrateHeader)].Text = (reader.WaveFormat.BitsPerSample * reader.WaveFormat.SampleRate / 1000).ToString();
                            f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.bitsizeHeader)].Text = reader.WaveFormat.BitsPerSample.ToString();

                        }

                        else if (clickFlag == false)
                        {
                            // add general fileinfos
                            ListViewItem fileInfos = new(Path.GetFileNameWithoutExtension(file));
                            string tempString = sd.audiofolderLabel.Text + "\\";
                            fileInfos.SubItems.Add(file.Replace(tempString, "").Replace("\\", "/").Replace(".wav", ""));
                            fileInfos.SubItems.Add(file);
                            fileInfos.SubItems.Add("");
                            fileInfos.SubItems.Add("");
                            fileInfos.SubItems.Add("SFX");
                            fileInfos.SubItems.Add((reader.Length / 1000).ToString());
                            fileInfos.SubItems.Add(CalculateAudiolength(reader));
                            fileInfos.SubItems.Add(reader.WaveFormat.Channels.ToString());
                            fileInfos.SubItems.Add(Math.Round(reader.WaveFormat.SampleRate / 1000.0, 3).ToString());
                            fileInfos.SubItems.Add((reader.WaveFormat.BitsPerSample * reader.WaveFormat.SampleRate / 1000).ToString());
                            fileInfos.SubItems.Add(reader.WaveFormat.BitsPerSample.ToString());
                            fileInfos.SubItems.Add(f1.MainVolumeSlider.Volume.ToString(""));
                            fileInfos.SubItems.Add(Convert.ToString(128));
                            fileInfos.SubItems.Add("false");
                            // add effect items
                            fileInfos.SubItems.Add("1");
                            fileInfos.SubItems.Add("1000");
                            fileInfos.SubItems.Add(f1.DopplertrackBar.Minimum.ToString());
                            // add pitch effect items
                            fileInfos.SubItems.Add("1");
                            fileInfos.SubItems.Add("0");
                            // add eq effect items
                            fileInfos.SubItems.Add("1,00");
                            fileInfos.SubItems.Add("1,00");
                            fileInfos.SubItems.Add("1,00");
                            fileInfos.SubItems.Add("1,00");
                            fileInfos.SubItems.Add("1,00");
                            fileInfos.SubItems.Add("1,00");
                            fileInfos.SubItems.Add("1,00");
                            fileInfos.SubItems.Add("1,00");
                            fileInfos.SubItems.Add("0");
                            // add echo effect items
                            fileInfos.SubItems.Add("500");
                            fileInfos.SubItems.Add("0,50");
                            fileInfos.SubItems.Add("0,50");
                            fileInfos.SubItems.Add("0");
                            // add misc items
                            fileInfos.SubItems.Add("false");
                            fileInfos.SubItems.Add("false");
                            fileInfos.SubItems.Add("LINEAR");
                            fileInfos.SubItems.Add("MANY");
                            // add fileinfos to listview
                            f1.FilelistView.Items.Add(fileInfos);
                        }
                    }

                    catch (Exception)
                    {
                        MessageBox.Show("File not supported!");
                    }                    
                }
            }
        }

        public void StartPlayback()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (sourceVoice != null && playbackPause == false)
            {
                sourceVoice.Stop();
                f1.PlayButton.Text = "▶";
                playbackPause = true;
            }
            else if (sourceVoice != null && playbackPause == true)
            {
                sourceVoice.Start();
                f1.PlayButton.Text = "| |";
                playbackPause = false;
            }
            else if (playbackStop == true && f1.FilelistView.SelectedItems.Count > 0)
            {
                string soundFilepath = f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.filepathHeader)].Text;
                Vortice.Multimedia.WaveFormat waveFormat;

                WaveFormat readerFormat;
                uint audioDataSize;

                // Check waveformat
                using (var probe = new WaveFileReader(soundFilepath))
                {
                    readerFormat = probe.WaveFormat;
                }

                bool is24BitPcm = readerFormat.Encoding == WaveFormatEncoding.Pcm
                                  && readerFormat.BitsPerSample == 24;

                if (is24BitPcm)
                {
                    // Convert 24Bit to 32Bit float to enable playback
                    using var floatReader = new AudioFileReader(soundFilepath);
                    audioDataSize = (uint)floatReader.Length;
                    audioDataPtr = Marshal.AllocHGlobal((int)audioDataSize);

                    byte[] chunk = new byte[8192];
                    int totalRead = 0;
                    int bytesRead;
                    while ((bytesRead = floatReader.Read(chunk, 0, chunk.Length)) > 0)
                    {
                        Marshal.Copy(chunk, 0, audioDataPtr + totalRead, bytesRead);
                        totalRead += bytesRead;
                    }

                    readerFormat = floatReader.WaveFormat;  // jetzt IeeeFloat, 32 Bit
                }
                else
                {
                    // Read 16Bit or 32Bit data
                    using var reader = new WaveFileReader(soundFilepath);
                    audioDataSize = (uint)reader.Length;
                    audioDataPtr = Marshal.AllocHGlobal((int)audioDataSize);

                    byte[] chunk = new byte[8192];
                    int totalRead = 0;
                    int bytesRead;
                    while ((bytesRead = reader.Read(chunk, 0, chunk.Length)) > 0)
                    {
                        Marshal.Copy(chunk, 0, audioDataPtr + totalRead, bytesRead);
                        totalRead += bytesRead;
                    }
                }

                // Samplerate check
                if (readerFormat.SampleRate < 22000 || readerFormat.SampleRate > 48000)
                {
                    MessageBox.Show(
                        $"This file has a samplerate of {readerFormat.SampleRate} Hz and can´t be played.\n" +
                        "Please resample it to 48000 Hz first:" +
                        "\n[Destructive Effects → \"Resample to 48kHz\"]",
                        "Samplerate not supported",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // Free used space
                    if (audioDataPtr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(audioDataPtr);
                        audioDataPtr = IntPtr.Zero;
                    }
                    return;
                }

                audioBuffer = new AudioBuffer(audioDataPtr, audioDataSize, BufferFlags.None);

                waveFormat = Vortice.Multimedia.WaveFormat.CreateCustomFormat(
                    MapEncoding(readerFormat.Encoding),
                    readerFormat.SampleRate,
                    readerFormat.Channels,
                    readerFormat.AverageBytesPerSecond,
                    readerFormat.BlockAlign,
                    readerFormat.BitsPerSample);

                // Effects XAudio2
                sourceVoice = xaudio2.CreateSourceVoice(waveFormat, VoiceFlags.UseFilter, 10);

                // Set Loop
                if (f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.loopHeader)].Text == "true")
                {
                    audioBuffer.LoopCount = XAudio2.LoopInfinite;
                }

                // Set Volume
                sourceVoice.SetVolume(f1.MainVolumeSlider.Volume);

                // Set Pitch
                if (f1.PitchenableButton.BackColor == Color.LightGreen)
                {
                    float pitchValue = Convert.ToSingle(f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.pitchHeader)].Text);
                    float pitchrandValue = Convert.ToSingle(f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.pitchrandHeader)].Text);
                    sourceVoice.SetFrequencyRatio(Effects.PitchRandomizer(pitchValue, pitchrandValue), operationSet: 0);
                }

                // Set Effect Chain
                bool roomAssigned = f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.roommapHeader)].Text != "";
                bool eqEnabled = f1.EQenableButton.BackColor == Color.LightGreen;
                bool echoEnabled = f1.EchoenableButton.BackColor == Color.LightGreen;
                bool reverbEnabled = roomAssigned && f1.RoomenableButton.BackColor == Color.LightGreen;

                // Build effect chain
                (eqEffect, echoEffect, reverbEffect, limiterEffect) = RoomCreationEffects.SetEffectChain(sourceVoice);

                // Deactivate all effects by default
                sourceVoice.DisableEffect(0);  // EQ
                sourceVoice.DisableEffect(1);  // Echo
                sourceVoice.DisableEffect(2);  // Reverb
                sourceVoice.DisableEffect(3);  // Limiter

                // Set parameters
                Effects.SetEqualizer(sourceVoice);
                Effects.SetEcho(sourceVoice);
                if (roomAssigned)
                {
                    RoomCreationEffects.SetRoomFilter(sourceVoice);
                    RoomCreationEffects.SetRoomReverb(sourceVoice);
                }
                Effects.SetLimiter(sourceVoice);

                // Activate effects dependend to gui buttons
                if (eqEnabled) sourceVoice.EnableEffect(0);
                if (echoEnabled) sourceVoice.EnableEffect(1);
                if (reverbEnabled) sourceVoice.EnableEffect(2);
                //sourceVoice.EnableEffect(3);  // Limiter always on as master peak protection

                sourceVoice.SubmitSourceBuffer(audioBuffer);
                sourceVoice.Start();

                f1.PlayButton.Text = "| |";
                playbackStop = false;
            }     
        }

        public void StopPlayback()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (sourceVoice != null)
            {
                sourceVoice.Stop();
                sourceVoice.FlushSourceBuffers();
                sourceVoice.DestroyVoice();
                sourceVoice.Dispose();
                sourceVoice = null;
            }

            eqEffect?.Dispose();
            eqEffect = null;

            echoEffect?.Dispose();
            echoEffect = null;

            reverbEffect?.Dispose();
            reverbEffect = null;

            limiterEffect?.Dispose();
            limiterEffect = null;

            audioBuffer?.Dispose();
            audioBuffer = null;
            if (audioDataPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(audioDataPtr);
                audioDataPtr = IntPtr.Zero;
            }

            f1?.PlayButton.Text = "▶";
            
            playbackStop = true;
            playbackPause = false;
        }

        public void SetVolume(double sliderValue, int filelistValue)
        {
            if (sourceVoice != null &&
                filelistValue == 1)
            {
                sourceVoice.SetVolume(Convert.ToSingle(sliderValue));
            }
        }

        public void SetPitch(double pitchValue, double pitchrandValue, int filelistValue)
        {
            if (sourceVoice != null &&
                filelistValue == 1)
            {
                sourceVoice.SetFrequencyRatio(Effects.PitchRandomizer(Convert.ToSingle(pitchValue), Convert.ToSingle(pitchrandValue)), operationSet: 0);
            }
        }        

        public void Cleanup()
        {
            StopPlayback();

            if (masteringVoice != null)
            {
                masteringVoice.DestroyVoice();
                masteringVoice.Dispose();
                masteringVoice = null;
            }

            if (xaudio2 != null)
            {
                xaudio2.StopEngine();
                xaudio2.Dispose();
                xaudio2 = null;
            }

            device?.Dispose();
            device = null;

            enumerator?.Dispose();
            enumerator = null;
        }
    }
}