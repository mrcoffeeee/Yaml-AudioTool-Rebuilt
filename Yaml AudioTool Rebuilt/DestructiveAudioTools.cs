using NAudio.Wave;
using NAudio.Wave.SampleProviders;
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
                // t läuft von 0 (Bereichsanfang) bis 1 (Bereichsende)
                float t = (float)(i - startSample) / fadeLength;
                float fadeRatio;

                switch (fadeIndex)
                {
                    case 0: // Linear IN
                        fadeRatio = t;
                        break;
                    case 1: // Exponential IN (concave) – startet flach, schwillt spät an
                        fadeRatio = t * t;
                        break;
                    case 2: // Exponential IN (convex) – schwillt früh an, flacht oben ab
                        fadeRatio = 1f - (1f - t) * (1f - t);
                        break;
                    case 3: // S-Curve IN – flach, steile Mitte, flach
                        fadeRatio = t * t * (3f - 2f * t);
                        break;
                    case 4: // Linear OUT
                        fadeRatio = 1f - t;
                        break;
                    case 5: // Exponential OUT (concave)
                        {
                            float to = 1f - t;
                            fadeRatio = to * to;
                        }
                        break;
                    case 6: // Exponential OUT (convex)
                        fadeRatio = 1f - t * t;
                        break;
                    case 7: // S-Curve OUT
                        {
                            float to = 1f - t;
                            fadeRatio = to * to * (3f - 2f * to);
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

        public static void ResampleTo48kHz(string inputPath, string outputPath)
        {
            // Read original wavefile specs
            int originalBits;
            NAudio.Wave.WaveFormatEncoding originalEncoding;
            using (var probe = new WaveFileReader(inputPath))
            {
                originalBits = probe.WaveFormat.BitsPerSample;
                originalEncoding = probe.WaveFormat.Encoding;
            }

            using var reader = new AudioFileReader(inputPath);
            var resampler = new WdlResamplingSampleProvider(reader, 48000);

            // Reconvert float o original bitrate, 32bit stays as it is
            if (originalEncoding == NAudio.Wave.WaveFormatEncoding.IeeeFloat)
            {
                WaveFileWriter.CreateWaveFile(outputPath, new SampleToWaveProvider(resampler));
            }
            else if (originalBits == 24)
            {
                WaveFileWriter.CreateWaveFile(outputPath, new SampleToWaveProvider24(resampler));
            }
            else
            {
                // 16-Bit
                WaveFileWriter.CreateWaveFile16(outputPath, resampler);
            }
        }
    }
}
