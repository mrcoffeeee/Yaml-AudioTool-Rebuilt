using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using CSCore;
using CSCore.Codecs;
using CSCore.Codecs.WAV;

using Vortice;
using Vortice.XAudio2;
using Vortice.Multimedia;
using NAudio;

namespace Yaml_AudioTool_Rebuilt
{
    public class AudioPlayback
    {
        public int timerCount = 0;

        public bool playbackPause = false;
        public bool playbackStop = true;

        public IXAudio2 xaudio2;
        public IXAudio2SourceVoice sourceVoice;
      //  public IXAudio2SubmixVoice submixVoice;

        public IWaveSource soundSource;

        private readonly Effects ef = new();        

        public IWaveSource OpenFile(bool clickFlag)
        {            
            StopPlayback();

            SettingsDialog sd = new();
            OpenFileDialog openfiledialog = new();
            openfiledialog.InitialDirectory = sd.audiofolderLabel.Text;
            openfiledialog.Multiselect = true;
            openfiledialog.Filter = CodecFactory.SupportedFilesFilterEn;

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                //IWaveSource source;

                foreach (string file in openfiledialog.FileNames)
                {
                    try
                    {
                        soundSource = CodecFactory.Instance.GetCodec(file);

                        if (soundSource.WaveFormat.SampleRate != 48000)
                        {
                            MessageBox.Show("File: " + Path.GetFileName(file) + "\n\n" +
                                "WARNING:\n" +
                                "Samplerate should be 48kHz!\n" +
                                "Please provide new hq-file or convert the existing.");
                           // return soundSource = null;
                        }

                        Form1 f1 = (Form1)Application.OpenForms["Form1"];

                        if (clickFlag == true)
                        {
                          //  MessageBox.Show(f1.filelistView.SelectedItems[0].Index.ToString());
                            string tempString = sd.audiofolderLabel.Text + "\\";
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.filenameHeader)].Text = file.Replace(tempString, "").Replace("\\", "/").Replace(".wav", "");
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.filepathHeader)].Text = file;
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.sizeHeader)].Text = (soundSource.Length / 1000).ToString();
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.durationHeader)].Text = soundSource.GetLength().ToString(@"mm\:ss");
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.channelsHeader)].Text = soundSource.WaveFormat.Channels.ToString();
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.samplerateHeader)].Text = Math.Round(soundSource.WaveFormat.SampleRate / 1000.0, 3).ToString();
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.bitrateHeader)].Text = (soundSource.WaveFormat.BitsPerSample * soundSource.WaveFormat.SampleRate / 1000).ToString();
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.bitsizeHeader)].Text = soundSource.WaveFormat.BitsPerSample.ToString();

                        }

                        else if  (clickFlag == false)
                        {
                            // add general fileinfos
                            ListViewItem fileInfos = new(Path.GetFileNameWithoutExtension(file));
                            string tempString = sd.audiofolderLabel.Text + "\\";
                            fileInfos.SubItems.Add(file.Replace(tempString, "").Replace("\\", "/").Replace(".wav", ""));
                            fileInfos.SubItems.Add(file);
                            fileInfos.SubItems.Add("");
                            fileInfos.SubItems.Add("");
                            fileInfos.SubItems.Add((soundSource.Length / 1000).ToString());
                            fileInfos.SubItems.Add(soundSource.GetLength().ToString(@"mm\:ss"));
                            fileInfos.SubItems.Add(soundSource.WaveFormat.Channels.ToString());
                            fileInfos.SubItems.Add(Math.Round(soundSource.WaveFormat.SampleRate / 1000.0, 3).ToString());
                            fileInfos.SubItems.Add((soundSource.WaveFormat.BitsPerSample * soundSource.WaveFormat.SampleRate / 1000).ToString());
                            fileInfos.SubItems.Add(soundSource.WaveFormat.BitsPerSample.ToString());
                            fileInfos.SubItems.Add((f1.VolumetrackBar.Maximum / 100.0).ToString(""));
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
                            // add fileinfos to listview
                            f1.filelistView.Items.Add(fileInfos);
                        }
                        
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("File not supported!");
                        return soundSource = null;
                    }                    
                }
            }
            return soundSource;
        }

        public void GetSoundFromList(string filePath)
        {          
            soundSource = CodecFactory.Instance.GetCodec(filePath);
        }

        public void StartPlayback()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

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
            else if (soundSource != null && playbackStop == true)
            {                
                string soundFilepath = f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.filepathHeader)].Text;
                xaudio2 = XAudio2.XAudio2Create(ProcessorSpecifier.UseDefaultProcessor);
                Vortice.Multimedia.WaveFormat waveFormat;
                AudioBuffer audioBuffer;
                var masteringVoice = xaudio2.CreateMasteringVoice(2, 48000);
                xaudio2.StartEngine();

                // Set Pitchshifter                                
                if (f1.PitchenableButton.BackColor == Color.LightGreen)
                {
                    soundSource = CodecFactory.Instance.GetCodec(soundFilepath);
                    soundSource = ef.SetPitchshift(soundSource);
                    CSCore.Extensions.WriteToFile(soundSource, "Temp.wav");
                    soundFilepath = "Temp.wav";
                }

                NAudio.Wave.WaveFileReader reader;

                byte[] array;
                using (reader = new NAudio.Wave.WaveFileReader(soundFilepath))
                {
                    array = new byte[reader.Length];

                    reader.Read(array, 0, array.Length);

                    audioBuffer = new AudioBuffer(array, BufferFlags.None);

                    int encoding_ = (int)reader.WaveFormat.Encoding;

                    waveFormat = Vortice.Multimedia.WaveFormat.CreateCustomFormat((Vortice.Multimedia.WaveFormatEncoding)encoding_,
                        reader.WaveFormat.SampleRate,
                        reader.WaveFormat.Channels,
                        reader.WaveFormat.AverageBytesPerSecond,
                        reader.WaveFormat.BlockAlign,
                        reader.WaveFormat.BitsPerSample);
                }

                sourceVoice = xaudio2.CreateSourceVoice(waveFormat, VoiceFlags.UseFilter);
                //submixVoice = xaudio2.CreateSubmixVoice(waveFormat.Channels, 48000, SubmixVoiceFlags.UseFilter, 0); 

                // Set Loop
                if (f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.loopHeader)].Text == "true")
                {
                    audioBuffer.LoopCount = IXAudio2.LoopInfinite;
                }

                // Set Effects
                // Set Room                              
                if (f1. RoomenableButton.BackColor == Color.LightGreen)
                {
                    if (f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.roommapHeader)].Text != "")
                    {
                        Effects.SetRoomFilter(sourceVoice);
                        Effects.SetRoomReverb(sourceVoice);
                    }

                }

                // Set Volume
                double volumeValue = f1.VolumetrackBar.Value / 100.0;
                sourceVoice.SetVolume(Convert.ToSingle(volumeValue));

                ///DO IT HERE!

                // Set Brickwall Limiter
                //ef.SetBrickwallLimiter(xaudio2, waveFormat, masteringVoice);

                sourceVoice.SubmitSourceBuffer(audioBuffer);
                sourceVoice.Start();

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
          //      submixVoice.Dispose();
            //    submixVoice = null;
            }
            timerCount = 0;
            if (f1 != null)
                f1.PlayButton.Text = "▶";
            playbackStop = true;
        }

        public double SetVolume(int trackbarValue, int filelistValue)
        {
            double value = trackbarValue / 100.0;
            if (xaudio2 != null &&
                filelistValue == 1)
            {
                sourceVoice.SetVolume(Convert.ToSingle(value));
            }
            return value;
        }
    }
}