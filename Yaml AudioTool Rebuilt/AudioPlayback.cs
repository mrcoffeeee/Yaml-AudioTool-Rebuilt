using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NAudio.Wave;

using Vortice.XAudio2;
using Vortice.Multimedia;


namespace Yaml_AudioTool_Rebuilt
{
    public class AudioPlayback
    {
        public int timerCount = 0;

        public bool playbackPause = false;
        public bool playbackStop = true;

        public IXAudio2 xaudio2;
        public IXAudio2SourceVoice sourceVoice;
        //public IXAudio2SubmixVoice submixVoice;

        public WaveFileReader waveFileReader;      

        public NAudio.Wave.WaveFileReader OpenFile(bool clickFlag)
        {            
            StopPlayback();

            SettingsDialog sd = new();
            OpenFileDialog openfiledialog = new();
            openfiledialog.InitialDirectory = sd.audiofolderLabel.Text;
            openfiledialog.Multiselect = true;
            openfiledialog.Filter = "WAV Files|*.wav";

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openfiledialog.FileNames)
                {
                    try
                    {
                        waveFileReader = new WaveFileReader(file);

                        if (waveFileReader.WaveFormat.SampleRate != 48000)
                        {
                            MessageBox.Show("File: " + Path.GetFileName(file) + "\n\n" +
                                "WARNING:\n" +
                                "Samplerate should be 48kHz!\n" +
                                "Please provide new hq-file or convert the existing.");
                        }

                        

                        Form1 f1 = (Form1)Application.OpenForms["Form1"];

                        if (clickFlag == true)
                        {
                          //  MessageBox.Show(f1.filelistView.SelectedItems[0].Index.ToString());
                            string tempString = sd.audiofolderLabel.Text + "\\";
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.filenameHeader)].Text = file.Replace(tempString, "").Replace("\\", "/").Replace(".wav", "");
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.filepathHeader)].Text = file;
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.sizeHeader)].Text = (waveFileReader.Length / 1000).ToString(); 
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.durationHeader)].Text = waveFileReader.TotalTime.ToString(@"mm\:ss");
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.channelsHeader)].Text = waveFileReader.WaveFormat.Channels.ToString();
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.samplerateHeader)].Text = Math.Round(waveFileReader.WaveFormat.SampleRate / 1000.0, 3).ToString();
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.bitrateHeader)].Text = (waveFileReader.WaveFormat.BitsPerSample * waveFileReader.WaveFormat.SampleRate / 1000).ToString();
                            f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.bitsizeHeader)].Text = waveFileReader.WaveFormat.BitsPerSample.ToString();

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
                            fileInfos.SubItems.Add((waveFileReader.Length / 1000).ToString());
                            fileInfos.SubItems.Add(waveFileReader.TotalTime.ToString(@"mm\:ss"));
                            fileInfos.SubItems.Add(waveFileReader.WaveFormat.Channels.ToString());
                            fileInfos.SubItems.Add(Math.Round(waveFileReader.WaveFormat.SampleRate / 1000.0, 3).ToString());
                            fileInfos.SubItems.Add((waveFileReader.WaveFormat.BitsPerSample * waveFileReader.WaveFormat.SampleRate / 1000).ToString());
                            fileInfos.SubItems.Add(waveFileReader.WaveFormat.BitsPerSample.ToString());
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
                            fileInfos.SubItems.Add("0");
                            // add fileinfos to listview
                            f1.filelistView.Items.Add(fileInfos);
                        }
                        
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("File not supported!");
                        return waveFileReader = null;
                    }                    
                }
            }
            return waveFileReader;
        }

        public void GetSoundFromList(string filePath)
        {
            waveFileReader = new WaveFileReader(filePath);
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
            else if (waveFileReader != null && playbackStop == true)
            {                
                string soundFilepath = f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.filepathHeader)].Text;
                xaudio2 = XAudio2.XAudio2Create(ProcessorSpecifier.UseDefaultProcessor);
                Vortice.Multimedia.WaveFormat waveFormat;
                AudioBuffer audioBuffer;
                var masteringVoice = xaudio2.CreateMasteringVoice(2, 48000);
                xaudio2.StartEngine();

                byte[] array;

                AudioFileReader reader;

                // Offline Effects Apart From XAudio2
                // Set Pitchshifter
                /*if (f1.PitchenableButton.BackColor == Color.LightGreen)
                {                    
                    float pitchValue = Convert.ToSingle(f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.pitchHeader)].Text);
                    float pitchrandValue = Convert.ToSingle(f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.pitchrandHeader)].Text);
                    using (var readers = new AudioFileReader(soundFilepath))
                    {
                        float[] buffer = ReadAllAudioSamples(soundFilepath);
                        buffer = NAudioEffects.LowerVolume(buffer, 0.5f);                        
                       /* PitchShifter.PitchShift(
                            PitchShifter_Helper.PitchRandomizer(pitchValue, pitchrandValue), 
                            buffer.Length,                             
                            readers.WaveFormat.SampleRate, 
                            buffer);
                        buffer = NAudioEffects.Normalize(buffer);

                        NAudio.Wave.WaveFormat wwaveFormat = new NAudio.Wave.WaveFormat(48000, 24, 2);
                        using (WaveFileWriter writer = new WaveFileWriter("temp.wav", wwaveFormat))
                        {
                            writer.WriteSamples(buffer, 0, buffer.Length);
                        }                        
                    }
                    using (reader = new AudioFileReader("temp.wav"))
                    {
                        array = new byte[reader.Length];

                        reader.Read(array, 0, array.Length);
                    }
                }
                else
                {
                    using (reader = new AudioFileReader(soundFilepath))
                    {
                        array = new byte[reader.Length];

                        reader.Read(array, 0, array.Length);
                    }
                }*/

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
                if (f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.loopHeader)].Text == "true")
                {
                    audioBuffer.LoopCount = XAudio2.LoopInfinite;
                }
                
                //sourceVoice = Effects.SetVolumeMeter(sourceVoice);                

                // Set Volume
                double volumeValue = f1.VolumetrackBar.Value / 100.0;
                sourceVoice.SetVolume(Convert.ToSingle(volumeValue));

                // Set Pitch
                if (f1.PitchenableButton.BackColor == Color.LightGreen)
                {
                    sourceVoice.SetFrequencyRatio(2.0f, operationSet: 0);
                }

                // Set Room                              
                if (f1.RoomenableButton.BackColor == Color.LightGreen)
                {
                    if (f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.roommapHeader)].Text != "")
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

        public static float[] ReadAllAudioSamples(string filePath)
        {
            var readers = new AudioFileReader(filePath);
            List<float> allSamples = new List<float>();
            float[] samples = new float[16384];

            while(readers.Read(samples, 0, samples.Length) > 0)
            {
                for(int a = 0; a < samples.Length; a++)
                {
                    allSamples.Add(samples[a]);
                }
            }

            samples = new float[allSamples.Count];
            for(int a = 0; a < samples.Length; a++)
            {
                samples[a] = allSamples[a];
            }            
            return samples;
        }

        public static byte[] ConvertFloatToByteArray(float[] samples, int samplesCount)
        {
            var pcm = new byte[samplesCount * 2];
            int sampleIndex = 0;
            int pcmIndex = 0;

            while(sampleIndex < samplesCount)
            {
                var outsample = (short)(samples[sampleIndex] * short.MaxValue);
                pcm[pcmIndex] = (byte)(outsample & 0xff);
                pcm[pcmIndex + 1] = (byte)((outsample >> 8) & 0xff);

                sampleIndex++;
                pcmIndex += 2;
            }

            return pcm;
        }
    }
}