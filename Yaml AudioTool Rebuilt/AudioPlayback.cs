using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using Vortice.XAudio2;
using NAudio.CoreAudioApi;

namespace Yaml_AudioTool_Rebuilt
{
    public class AudioPlayback
    {
        public bool playbackPause = false;
        public bool playbackStop = true;

        public MMDeviceEnumerator enumerator;
        public MMDevice device;

        public IXAudio2 xaudio2;
        public IXAudio2SourceVoice sourceVoice;

        public WaveFileReader waveFileReader;

        private static string CalculateAudiolength(WaveFileReader waveFileReader)
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
                        using (waveFileReader = new WaveFileReader(file))
                        {
                            Form1 f1 = (Form1)Application.OpenForms["Form1"];

                            if (clickFlag == true)
                            {
                                string tempString = sd.audiofolderLabel.Text + "\\";
                                f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.filenameHeader)].Text = file.Replace(tempString, "").Replace("\\", "/").Replace(".wav", "");
                                f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.filepathHeader)].Text = file;
                                f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.sizeHeader)].Text = (waveFileReader.Length / 1000).ToString();
                                f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.durationHeader)].Text = CalculateAudiolength(waveFileReader);
                                f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.channelsHeader)].Text = waveFileReader.WaveFormat.Channels.ToString();
                                f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.samplerateHeader)].Text = Math.Round(waveFileReader.WaveFormat.SampleRate / 1000.0, 3).ToString();
                                f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.bitrateHeader)].Text = (waveFileReader.WaveFormat.BitsPerSample * waveFileReader.WaveFormat.SampleRate / 1000).ToString();
                                f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.bitsizeHeader)].Text = waveFileReader.WaveFormat.BitsPerSample.ToString();

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
                                fileInfos.SubItems.Add((waveFileReader.Length / 1000).ToString());
                                fileInfos.SubItems.Add(CalculateAudiolength(waveFileReader));
                                fileInfos.SubItems.Add(waveFileReader.WaveFormat.Channels.ToString());
                                fileInfos.SubItems.Add(Math.Round(waveFileReader.WaveFormat.SampleRate / 1000.0, 3).ToString());
                                fileInfos.SubItems.Add((waveFileReader.WaveFormat.BitsPerSample * waveFileReader.WaveFormat.SampleRate / 1000).ToString());
                                fileInfos.SubItems.Add(waveFileReader.WaveFormat.BitsPerSample.ToString());
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
                                fileInfos.SubItems.Add("1");
                                fileInfos.SubItems.Add("0");
                                fileInfos.SubItems.Add("0");
                                // add fileinfos to listview
                                f1.FilelistView.Items.Add(fileInfos);
                            }
                        }                        
                    }

                    catch (Exception)
                    {
                        MessageBox.Show("File not supported!");
                    }                    
                }
            }
        }

        public void GetSoundFromList(string filePath)
        {
            try
            {
                waveFileReader = new WaveFileReader(filePath);
            }

            catch (Exception)
            {
                MessageBox.Show("Please check if its filepath is still correct.", "File not found! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        public void StartPlayback()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            //Get active soundcard
            enumerator = new();            
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            if (xaudio2 != null && playbackPause == false)
            {
                sourceVoice.Stop();
                f1.PlayButton.Text = "▶";
                playbackPause = true;
            }
            else if (xaudio2 != null && playbackPause == true)
            {
                sourceVoice.Start();
                f1.PlayButton.Text = "| |";
                playbackPause = false;
            }
            else if (waveFileReader != null && playbackStop == true)
            {                
                string soundFilepath = f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.filepathHeader)].Text;
                xaudio2 = XAudio2.XAudio2Create(ProcessorSpecifier.UseDefaultProcessor);
                Vortice.Multimedia.WaveFormat waveFormat;
                AudioBuffer audioBuffer;
                var masteringVoice = xaudio2.CreateMasteringVoice(2, 48000);
                xaudio2.StartEngine();

                byte[] array;

                AudioFileReader reader;

                using (reader = new AudioFileReader(soundFilepath))
                {
                    array = new byte[reader.Length];

                    reader.Read(array, 0, array.Length);
                }

                audioBuffer = new AudioBuffer(array, BufferFlags.None);

                int encoding_ = (int)reader.WaveFormat.Encoding;

                waveFormat = Vortice.Multimedia.WaveFormat.CreateCustomFormat((Vortice.Multimedia.WaveFormatEncoding)encoding_,
                    reader.WaveFormat.SampleRate,
                    reader.WaveFormat.Channels,
                    reader.WaveFormat.AverageBytesPerSecond,
                    reader.WaveFormat.BlockAlign,
                    reader.WaveFormat.BitsPerSample);

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

                // Set Room                              
                if (f1.RoomenableButton.BackColor == Color.LightGreen)
                {
                    if (f1.FilelistView.SelectedItems[0].SubItems[f1.FilelistView.Columns.IndexOf(f1.roommapHeader)].Text != "")
                    {
                        RoomCreationEffects.SetRoomFilter(sourceVoice);
                        RoomCreationEffects.SetRoomReverb(sourceVoice);
                    }
                }

                sourceVoice.SubmitSourceBuffer(audioBuffer);
                sourceVoice.Start();
                //RoomCreationEffects.UpdateReverbSettings(f1.filelistView.SelectedItems.Count, sourceVoice);

                f1.PlayButton.Text = "| |";
                playbackStop = false;
            }     
        }

        public void StopPlayback()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (xaudio2 != null)
            {
                xaudio2.StopEngine();
                xaudio2.Dispose();
                xaudio2 = null;
            }
            if (sourceVoice != null)
            {
                sourceVoice.Dispose();
                sourceVoice = null;
            }
            if (f1 != null)
                f1.PlayButton.Text = "▶";
            playbackStop = true;
        }

        public void SetVolume(double sliderValue, int filelistValue)
        {
            if (xaudio2 != null &&
                filelistValue == 1)
            {
                sourceVoice.SetVolume(Convert.ToSingle(sliderValue));
            }
        }

        public void SetPitch(double pitchValue, double pitchrandValue, int filelistValue)
        {
            
            if (xaudio2 != null &&
                filelistValue == 1)
            {
                sourceVoice.SetFrequencyRatio(Effects.PitchRandomizer(Convert.ToSingle(pitchValue), Convert.ToSingle(pitchrandValue)), operationSet: 0);
            }
        }
    }
}