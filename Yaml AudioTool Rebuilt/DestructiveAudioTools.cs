using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaml_AudioTool_Rebuilt
{
    internal static class DestructiveAudioTools
    {
        public static string Normalization(string inputFile)
        {
            var outPath = @"normalized_temp.wav";
            float max = 0;

            using var reader = new AudioFileReader(inputFile);
            // find the max peak
            float[] buffer = new float[reader.WaveFormat.SampleRate];
            int read;
            do
            {
                read = reader.Read(buffer, 0, buffer.Length);
                for (int n = 0; n < read; n++)
                {
                    var abs = Math.Abs(buffer[n]);
                    if (abs > max) max = abs;
                }
            } while (read > 0);
            Console.WriteLine($"Max sample value: {max}");

            if (max == 0 || max > 1.0f)
                return "File cannot be normalized.";

            // do normalization
            reader.Position = 0;
            reader.Volume = 1.0f / max;

            // write to a temp audio file
            WaveFileWriter.CreateWaveFile(outPath, reader);
            return outPath;
        }
    }
}
