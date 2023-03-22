using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yaml_AudioTool_Rebuilt
{
    internal static class DestructiveAudioTools
    {

        public static string GetPeakVolume(float[] audioData)
        {
            //audioStream.Position = 0;
            float max = 0;

            // find the max peak
            for (int n = 0; n < audioData.Length; n++)
            {
                var abs = Math.Abs(audioData[n]);
                if (abs > max) max = abs;
            }

            return Math.Round(max,3).ToString();
        }

        public static float[] Normalize(float[] audioData, string peakVolume)
        {
            peakVolume = peakVolume.Replace("Peak: ", "");
            float max = Convert.ToSingle(peakVolume);

            // do normalization
            if (max != 0 || max < 1.0f)
            {
                for (int i = 0; i < audioData.Length; i++)
                {
                    audioData[i] *= 1f / max;
                    if (audioData[i] > 1)
                        audioData[i] = 1;
                }
            }
            return audioData;
        }

        public static float[] VolumeUp(float[] audioData, string peakVolume)
        {
            peakVolume = peakVolume.Replace("Peak: ", "");
            float peakValue = Convert.ToSingle(peakVolume);
            float volumeUp = (peakValue + 0.1f) / peakValue;
            // Do Volume Up
            if (peakValue * volumeUp <= 1)
            {
                for (int i = 0; i < audioData.Length; i++)
                {
                    audioData[i] *= volumeUp;
                }
            }
            return audioData;
        }

        public static float[] VolumeDown(float[] audioData, string peakVolume)
        {
            peakVolume = peakVolume.Replace("Peak: ", "");
            float peakValue = Convert.ToSingle(peakVolume);
            float volumeDown = (peakValue - 0.1f) / peakValue;
            // Do Volume Down
            if (volumeDown > 0)
            {
                for (int i = 0; i < audioData.Length; i++)
                {
                    audioData[i] *= volumeDown;
                }
            }
            return audioData;
        }

        public static float[] Trim(float[] audioData, double start, double end, int sampleRate, int channels)
        {
            double startTime = start;
            double endTime = end;

            if (start > end)
            {
                startTime = end;
                endTime = start;
            }

            int startSample = (int)(startTime * sampleRate * channels);
            int endSample = (int)(endTime * sampleRate * channels);
            //MessageBox.Show(startSample.ToString() + " : " + endSample.ToString());
            //int tempLength = audioData.Length - (endSample - startSample);
            //MessageBox.Show(tempLength.ToString() + " : " + audioData.Length.ToString());
            List<float> trimmedAudioList = new();

            for (int i = 0; i < audioData.Length; i++)
            {
                if (i >= startSample && i <= endSample)
                {
                    continue;
                }
                else
                {
                    trimmedAudioList.Add(audioData[i]);
                }
            }
            return trimmedAudioList.ToArray();
        }
    }   
}
