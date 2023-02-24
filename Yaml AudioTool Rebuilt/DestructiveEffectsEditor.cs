using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class DestructiveEffectsEditor : Form
    {
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        public DestructiveEffectsEditor()
        {
            InitializeComponent();
            InitialPlotSetup();
            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                LoadAudioWaveform(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text);
            }
        }

        public void LoadAudioWaveform(string filePath)
        {
            this.Text = "Destructive Effects Editor -> " + filePath;
            FilepathLabel.Text = filePath;
            if (formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.channelsHeader)].Text == "1")
                ChannelsLabel.Text = "Mono";
            else if (formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.channelsHeader)].Text == "2")
                ChannelsLabel.Text = "Stereo";
            else
                ChannelsLabel.Text = "Unsupported Channels #";
            SamplerateLabel.Text = formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.samplerateHeader)].Text + " kHz";
            BitsizeLabel.Text = formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.bitsizeHeader)].Text + " Bit";
            TableLayoutPanelFD.Visible = true;
            PlotWaveform(filePath);
        }

        public void ResetDestructiveEffectsEditorValues()
        {
            this.Text = "Destructive Effects Editor";
            InitialPlotSetup();
            TableLayoutPanelFD.Visible = false;
        }

        private void NormalizeButton_Click(object sender, EventArgs e)
        {
            if (TableLayoutPanelFD.Visible == true)
            {
                InitialPlotSetup();
                string normalizedPath = DestructiveAudioTools.Normalization(FilepathLabel.Text);
                NormalizeRevertButton.Visible = true;
                SaveButton.Enabled = true;
                this.Text = "Destructive Effects Editor -> " + FilepathLabel.Text + "*";
                PlotWaveform(normalizedPath);
            }
        }

        private void NormalizeRevertButton_Click(object sender, EventArgs e)
        {
            InitialPlotSetup();
            NormalizeRevertButton.Visible = false;
            SaveButton.Enabled = false;
            this.Text = "Destructive Effects Editor -> " + FilepathLabel.Text;
            PlotWaveform(FilepathLabel.Text);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            NormalizeRevertButton.Visible = false;
            SaveButton.Enabled = false;

            DialogResult dialogResult = MessageBox.Show("Do you want to replace the current file?", "Save Changes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string backupFile = FilepathLabel.Text.Replace(".wav", "");
                backupFile += "_BACKUP.wav";
                File.Replace(@"normalized_temp.wav", FilepathLabel.Text, backupFile);
            }
            else if (dialogResult == DialogResult.No)
            {
                FilepathLabel.Text = FilepathLabel.Text.Replace(".wav", "");
                FilepathLabel.Text += "_EDIT.wav";                
                File.Copy(@"normalized_temp.wav", FilepathLabel.Text);
            }
            this.Text = "Destructive Effects Editor -> " + FilepathLabel.Text;
        }

        static (double[] audioL, double[] audioR, int sampleRate, int channelCount) ReadWavefile(string filePath)
        {
            using var audioFile = new WaveFileReader(filePath);
            int sampleRate = audioFile.WaveFormat.SampleRate;
            int sampleCount = (int)(audioFile.Length / audioFile.WaveFormat.BitsPerSample / 8);
            int channelCount = audioFile.WaveFormat.Channels;
            var audioL = new List<double>(sampleCount);
            var audioR = new List<double>(sampleCount);
            float[] buffer;

            if (channelCount == 1)
            {
                while ((buffer = audioFile.ReadNextSampleFrame())?.Length > 0)
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        // write one sample for each channel (i is the channelNumber
                        audioL.Add(buffer[i]);
                    }
                }
            }

            if (channelCount == 2)
            {
                while ((buffer = audioFile.ReadNextSampleFrame())?.Length > 0)
                {
                    for (int i = 0; i < buffer.Length - 1; i++)
                    {
                        // write one sample for each channel (i is the channelNumber
                        audioL.Add(buffer[i]);
                        audioR.Add(buffer[i + 1]);
                    }
                }
            }
            return (audioL.ToArray(), audioR.ToArray(), sampleRate, channelCount);
        }

        public void PlotWaveform(string filePath)
        {
            (double[] audioL, double[] audioR, int sampleRate, int channelCount) = ReadWavefile(filePath);
            WaveformsPlot.Plot.Title(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filenameHeader)].Text + ".wav");

            if (channelCount == 1)
            {
                var chM = WaveformsPlot.Plot.AddSignal(audioL, sampleRate);
                chM.Color = Color.DarkRed;
                WaveformsPlot.Plot.SetAxisLimitsY(-1, 1);
                WaveformsPlot.Plot.AxisAutoX(0);
                var limits = WaveformsPlot.Plot.GetAxisLimits();
                WaveformsPlot.Plot.SetOuterViewLimits(0, limits.XMax, -1, 1);
            }

            else if (channelCount == 2)
            {
                var chL = WaveformsPlot.Plot.AddSignal(audioL, sampleRate);
                chL.Color = Color.DarkRed;
                chL.OffsetY = 1;

                var chR = WaveformsPlot.Plot.AddSignal(audioR, sampleRate);
                chR.Color = Color.ForestGreen;
                chR.OffsetY = -1;
                WaveformsPlot.Plot.SetAxisLimitsY(-2, 2);
                WaveformsPlot.Plot.AxisAutoX(0);
                var limits = WaveformsPlot.Plot.GetAxisLimits();
                WaveformsPlot.Plot.SetOuterViewLimits(0, limits.XMax, -2, 2);
            }
            WaveformsPlot.Refresh();
        }

        private void InitialPlotSetup()
        {
            WaveformsPlot.Reset();
            WaveformsPlot.Plot.Title("");
            WaveformsPlot.Plot.XLabel("Time (seconds)");
            WaveformsPlot.Plot.YLabel("Audio level");
            WaveformsPlot.Plot.SetAxisLimitsY(-2, 2);
            WaveformsPlot.Configuration.Quality = ScottPlot.Control.QualityMode.High;
            WaveformsPlot.Configuration.LockVerticalAxis = true;
            WaveformsPlot.Refresh();
        }

        private void DestructiveEffectsEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                InitialPlotSetup();
                e.Cancel = true;
                Hide();
            }
            formMain.DestructiveEffectsButton.Enabled = true;
        }
    }
}
