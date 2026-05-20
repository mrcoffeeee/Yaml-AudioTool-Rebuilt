using System;

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
            if (max > 0 && max < 1.0f)
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

            // Clamp auf Array-Grenzen, um Edge Cases abzufangen
            if (startSample < 0) startSample = 0;
            if (endSample >= audioData.Length) endSample = audioData.Length - 1;
            if (endSample < startSample) return audioData;

            int removedCount = endSample - startSample + 1;
            float[] result = new float[audioData.Length - removedCount];

            // Alles vor dem zu entfernenden Bereich kopieren
            Array.Copy(audioData, 0, result, 0, startSample);

            // Alles nach dem Bereich kopieren
            int tailLength = audioData.Length - (endSample + 1);
            if (tailLength > 0)
            {
                Array.Copy(audioData, endSample + 1, result, startSample, tailLength);
            }

            return result;
        }

        public static float[] Fade(float[] audioData, double start, double end, int sampleRate, int channels, int fadeIndex)
        {
            (int startSample, int endSample) = GetSamplePostitions(start, end, sampleRate, channels);

            // Clamp auf Array-Grenzen
            if (startSample < 0) startSample = 0;
            if (endSample >= audioData.Length) endSample = audioData.Length - 1;
            if (endSample <= startSample) return audioData;

            int fadeLength = endSample - startSample;
            float[] result = new float[audioData.Length];

            // Bereiche vor und nach dem Fade unverändert übernehmen
            Array.Copy(audioData, 0, result, 0, startSample);
            int tailStart = endSample + 1;
            if (tailStart < audioData.Length)
            {
                Array.Copy(audioData, tailStart, result, tailStart, audioData.Length - tailStart);
            }

            // Fade-Bereich verarbeiten
            for (int i = startSample; i <= endSample; i++)
            {
                float fadeRatio;
                int relativePosition = i - startSample;

                switch (fadeIndex)
                {
                    case 0: // Linear FadeIn
                        fadeRatio = (float)relativePosition / fadeLength;
                        break;
                    case 1: // Exponential FadeIn
                        {
                            float t = (float)relativePosition / fadeLength;
                            fadeRatio = t * t;
                        }
                        break;
                    case 2: // Linear FadeOut
                        fadeRatio = (float)(fadeLength - relativePosition) / fadeLength;
                        break;
                    case 3: // Exponential FadeOut
                        {
                            float t = (float)(fadeLength - relativePosition) / fadeLength;
                            fadeRatio = t * t;
                        }
                        break;
                    default:
                        fadeRatio = 1f;
                        break;
                }

                result[i] = audioData[i] * fadeRatio;
            }

            return result;
        }
    }
}
