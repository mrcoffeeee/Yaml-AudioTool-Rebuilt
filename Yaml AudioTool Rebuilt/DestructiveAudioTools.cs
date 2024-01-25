using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Yaml_AudioTool_Rebuilt
{
    internal static class DestructiveAudioTools
    {
        private static (int startSample, int endSample) GetSamplePostitions(double start, double end, int sampleRate, int channels)
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

            return (startSample, endSample);
        }

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

            return Math.Round(max, 3).ToString();
        }

        public static float[] Normalize(float[] audioData, double start, double end, int sampleRate, int channels)
        {
            (int startSample, int endSample) = GetSamplePostitions(start, end, sampleRate, channels);
            float max = 0;

            // Do Max Peak Detection
            for (int i = 0; i < audioData.Length; i++)
            {
                if (i >= startSample && i <= endSample)
                {
                    var abs = Math.Abs(audioData[i]);
                    if (abs > max) max = abs;
                }
            }

            // Do Normalization
            if (max != 0 || max < 1.0f)
            {
                for (int i = 0; i < audioData.Length; i++)
                {
                    if (i >= startSample && i <= endSample)
                    {
                        audioData[i] *= 1f / max;
                        if (audioData[i] > 1)
                            audioData[i] = 1;
                    }
                }
            }
            return audioData;
        }

        public static float[] VolumeUp(float[] audioData, double start, double end, int sampleRate, int channels)
        {
            (int startSample, int endSample) = GetSamplePostitions(start, end, sampleRate, channels);
            float max = 0;

            // Do Max Peak Detection
            for (int i = 0; i < audioData.Length; i++)
            {
                if (i >= startSample && i <= endSample)
                {
                    var abs = Math.Abs(audioData[i]);
                    if (abs > max) max = abs;
                }
            }

            // Do Volume Up
            float volumeUp = (max + 0.1f) / max;

            if (max * volumeUp <= 1)
            {
                for (int i = 0; i < audioData.Length; i++)
                {
                    if (i >= startSample && i <= endSample)
                    {
                        audioData[i] *= volumeUp;
                    }
                }
            }
            return audioData;
        }

        public static float[] VolumeDown(float[] audioData, double start, double end, int sampleRate, int channels)
        {
            (int startSample, int endSample) = GetSamplePostitions(start, end, sampleRate, channels);
            float max = 0;

            // Do Max Peak Detection
            for (int i = 0; i < audioData.Length; i++)
            {
                if (i >= startSample && i <= endSample)
                {
                    var abs = Math.Abs(audioData[i]);
                    if (abs > max) max = abs;
                }
            }

            // Do Volume Down
            float volumeDown = (max - 0.1f) / max;

            if (volumeDown > 0)
            {
                for (int i = 0; i < audioData.Length; i++)
                {
                    if (i >= startSample && i <= endSample)
                    {
                        audioData[i] *= volumeDown;
                    }
                }
            }
            return audioData;
        }

        public static float[] Trim(float[] audioData, double start, double end, int sampleRate, int channels)
        {
            (int startSample, int endSample) = GetSamplePostitions(start, end, sampleRate, channels);
            List<float> trimmedAudioList = [];

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
            return [.. trimmedAudioList];
        }

        public static float[] Fade(float[] audioData, double start, double end, int sampleRate, int channels, int fadeIndex)
        {
            (int startSample, int endSample) = GetSamplePostitions(start, end, sampleRate, channels);
            float fadeRatio;
            List<float> fadeAudioList = [];

            // Linear FadeIn
            if (fadeIndex == 0)
            {
                int fadeIn = 0;
                for (int i = 0; i < audioData.Length; i++)
                {
                    if (i >= startSample && i <= endSample)
                    {
                        fadeRatio = (float)fadeIn / (endSample - startSample);
                        fadeAudioList.Add(audioData[i] *= fadeRatio);
                        fadeIn++;
                    }
                    else
                    {
                        fadeAudioList.Add(audioData[i]);
                    }
                }
            }

            // Exponential FadeIn
            if (fadeIndex == 1)
            {
                int fadeIn = 0;
                for (int i = 0; i < audioData.Length; i++)
                {
                    if (i >= startSample && i <= endSample)
                    {
                        fadeRatio = (float)Math.Pow(2, (float)fadeIn / (endSample - startSample)) - 1;
                        fadeAudioList.Add(audioData[i] *= fadeRatio);
                        fadeIn++;
                    }
                    else
                    {
                        fadeAudioList.Add(audioData[i]);
                    }
                }
            }

            // Linear FadeOut
            else if (fadeIndex == 2)
            {
                int fadeOut = endSample - startSample;
                for (int i = 0; i < audioData.Length; i++)
                {
                    if (i >= startSample && i <= endSample)
                    {
                        fadeRatio = (float)fadeOut / (endSample - startSample);
                        fadeAudioList.Add(audioData[i] *= fadeRatio);
                        fadeOut--;
                    }
                    else
                    {
                        fadeAudioList.Add(audioData[i]);
                    }
                }
            }

            // Exponential FadeOut
            else if (fadeIndex == 3)
            {
                int fadeOut = endSample - startSample;
                for (int i = 0; i < audioData.Length; i++)
                {
                    if (i >= startSample && i <= endSample)
                    {
                        fadeRatio = (float)Math.Pow(2, (float)fadeOut / (endSample - startSample)) - 1;
                        fadeAudioList.Add(audioData[i] *= fadeRatio);
                        fadeOut--;
                    }
                    else
                    {
                        fadeAudioList.Add(audioData[i]);
                    }
                }
            }
            return [.. fadeAudioList];
        }
    }
}
