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
        public IDisposable reverbEffect;

        public void Initialize()
        {
            if (xaudio2 != null) return;

            enumerator = new MMDeviceEnumerator();
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            xaudio2 = XAudio2.XAudio2Create(ProcessorSpecifier.UseDefaultProcessor);
            masteringVoice = xaudio2.CreateMasteringVoice(2, 48000);
            xaudio2.StartEngine();
        }

        public float GetSessionPeak()
        {
            if (device == null) return 0;

            try
            {
                var sessions = device.AudioSessionManager.Sessions;
                int currentProcessId = Environment.ProcessId;
                float peak = 0;

                for (int i = 0; i < sessions.Count; i++)
                {
                    var session = sessions[i];
                    if (session.GetProcessID == currentProcessId)
                    {
                        float sessionPeak = session.AudioMeterInformation.MasterPeakValue;
                        if (sessionPeak > peak) peak = sessionPeak;
                    }
                }
                return peak;
            }
            catch
            {
                // COM-calls could sometimes fail
                return 0;
            }
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

                        // Only samplerates from 22kHz o 48kHz
                        if (reader.WaveFormat.SampleRate < 22000 || reader.WaveFormat.SampleRate > 48000)
                        {
                            MessageBox.Show(
                                $"The file \"{Path.GetFileName(file)}\" can´t be opened due to its samplerate: {reader.WaveFormat.SampleRate} Hz.\n" +
                                "Valid samplerates are between 22kHz - 48kHz. Please convert the file first.",
                                "Samplerate not supported",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            continue;
                        }

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
                            // add effects items
                            fileInfos.SubItems.Add("1");
                            fileInfos.SubItems.Add("1000");
                            fileInfos.SubItems.Add(f1.DopplertrackBar.Minimum.ToString());
                            fileInfos.SubItems.Add("1");
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

                using (var reader = new WaveFileReader(soundFilepath))
                {
                    audioDataSize = (uint)reader.Length;
                    audioDataPtr = Marshal.AllocHGlobal((int)audioDataSize);

                    // In kleinen Chunks lesen, um LOH-Allokationen zu vermeiden
                    byte[] chunk = new byte[8192];
                    int totalRead = 0;
                    int bytesRead;
                    while ((bytesRead = reader.Read(chunk, 0, chunk.Length)) > 0)
                    {
                        Marshal.Copy(chunk, 0, audioDataPtr + totalRead, bytesRead);
                        totalRead += bytesRead;
                    }

                    readerFormat = reader.WaveFormat;
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
                bool reverbEnabled = roomAssigned && f1.RoomenableButton.BackColor == Color.LightGreen;

                // Build effect chain
                (eqEffect, reverbEffect) = RoomCreationEffects.SetEffectChain(sourceVoice);

                // Set eq parameters
                EQCreationEffect.SetEqualizer(sourceVoice);

                // Set room parameters
                if (roomAssigned)
                {
                    RoomCreationEffects.SetRoomFilter(sourceVoice);
                    RoomCreationEffects.SetRoomReverb(sourceVoice);
                }

                // Initial effects states due to buttons
                if (eqEnabled)
                    sourceVoice.EnableEffect(0);
                else
                    sourceVoice.DisableEffect(0);

                if (reverbEnabled)
                    sourceVoice.EnableEffect(1);
                else
                    sourceVoice.DisableEffect(1);

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

            reverbEffect?.Dispose();
            reverbEffect = null;

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