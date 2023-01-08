using NAudio.Wave;
using System;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Windows.Forms;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class Effect_Normalize : Form
    {
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        public Effect_Normalize()
        {
            InitializeComponent();
            SaveNormalizedAudioButton.Enabled = false;
            NormalizeLabel.Text = "";
        }        

        private void NormalizeFileButton_Click(object sender, EventArgs e)
        {
            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                NormalizeLabel.Text = "";
                NormalizeLabel.Text = "Processing ... Please wait.";
                string normalizeOutput = NormalizeEffect.ProcessNormalization(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text);
                NormalizeLabel.Text = normalizeOutput;
                SaveNormalizedAudioButton.Enabled = true;
            }

            else
            {
                MessageBox.Show("Please select only one file for normalization.");
            }
        }

        private void SaveNormalizedAudioButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to overwrite the existing file? \nIf not a new file will be created in the same folder of the original file.", "Confirmation", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                string tempOriginal = formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text;
                if (File.Exists(tempOriginal))
                {
                    File.Delete(tempOriginal);
                }
                File.Move(@"normalized_temp.wav", tempOriginal);
                NormalizeLabel.Text = "Normalized file saved! \nOriginal has been replaced.";
            }

            else if (result == DialogResult.No)
            {
                string tempNormalized = formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text;
                tempNormalized = tempNormalized.Replace(".wav", "_NORMALIZED.wav");
                if (File.Exists(tempNormalized))
                {
                    File.Delete(tempNormalized);
                }
                File.Move(@"normalized_temp.wav", tempNormalized);
                NormalizeLabel.Text = "Normalized file saved! \nNew file with label \"_NORMALIZED\" generated.";
            }

            else if (result == DialogResult.Cancel)
            {
                
                NormalizeLabel.Text = "Normalization canceled!";
            }


            if (File.Exists(@"normalized_temp.wav"))
            {
                File.Delete(@"normalized_temp.wav");
            }
            SaveNormalizedAudioButton.Enabled = false;
        }

        private void Effect_Normalize_FormClosed(object sender, FormClosedEventArgs e)
        {
            formMain.NormalizeButton.Enabled = true;
        }
    }

    public class NormalizeEffect
    {
        public static string ProcessNormalization(string inputFile)
        {
            var outPath = @"normalized_temp.wav";
            float max = 0;

            using (var reader = new AudioFileReader(inputFile))
            {
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
                return "Normalization processed: " + inputFile;
            }
        }
    }
}
