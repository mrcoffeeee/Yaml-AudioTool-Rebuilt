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
        private readonly OpenFileDialog openfiledialog = new();

        

        public IWaveSource OpenFile()
        {
            openfiledialog.Multiselect = true;
            StopPlayback();

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
                            MessageBox.Show(Path.GetFileNameWithoutExtension(file) + ":\n\n" +
                                "Samplerate unsupported yet!\n" +
                                "Please convert your file to 48kHz.");
                            return soundSource = null;
                        }                        

                        Form1 f1 = (Form1)Application.OpenForms["Form1"];
                        ListViewItem fileInfos = new(Path.GetFileNameWithoutExtension(file));
                        // add general fileinfos
                        fileInfos.SubItems.Add(file);
                        fileInfos.SubItems.Add("");
                        fileInfos.SubItems.Add("");
                        fileInfos.SubItems.Add((soundSource.Length / 1000).ToString());
                        fileInfos.SubItems.Add(soundSource.GetLength().ToString(@"mm\:ss"));
                        fileInfos.SubItems.Add(soundSource.WaveFormat.Channels.ToString());
                        fileInfos.SubItems.Add((soundSource.WaveFormat.SampleRate / 1000).ToString("0.0"));
                        fileInfos.SubItems.Add((soundSource.WaveFormat.BitsPerSample * soundSource.WaveFormat.SampleRate / 1000).ToString());
                        fileInfos.SubItems.Add(soundSource.WaveFormat.BitsPerSample.ToString());
                        fileInfos.SubItems.Add((f1.VolumetrackBar.Maximum / 100.0).ToString("0.0"));
                        fileInfos.SubItems.Add(Convert.ToString(128));
                        fileInfos.SubItems.Add("false");
                        // add effects items
                        fileInfos.SubItems.Add("1");
                        fileInfos.SubItems.Add("1000");
                        fileInfos.SubItems.Add((f1.DopplertrackBar.Maximum / 100.0).ToString("0.0"));
                        fileInfos.SubItems.Add("1,00");
                        fileInfos.SubItems.Add("0,00");
                        // add misc items
                        fileInfos.SubItems.Add("false");
                        fileInfos.SubItems.Add("false");
                        fileInfos.SubItems.Add("1");
                        fileInfos.SubItems.Add("0");
                        // add fileinfos to listview
                        f1.filelistView.Items.Add(fileInfos);
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

        public void GetSoundFromList(string listItem)
        {
            soundSource = CodecFactory.Instance.GetCodec(listItem);
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
                string soundFilepath = f1.filelistView.SelectedItems[0].SubItems[1].Text;
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

                // Set VolumeMeter
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