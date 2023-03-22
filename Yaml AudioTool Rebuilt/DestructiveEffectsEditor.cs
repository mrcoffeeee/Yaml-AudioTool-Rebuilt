using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using NAudio.Wave;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vortice.Multimedia;
using Vortice.XAudio2;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class DestructiveEffectsEditor : Form
    {
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        bool mouseDown = false;
        float[] audioData, audioData_Backup;
        //readonly ScottPlot.Plottable.VLine verticalLine;
        AxisLimits limits;
        ScottPlot.Plottable.HSpan waveformSpan;
        NAudio.Wave.WaveFormat WaveFormat;

        public DestructiveEffectsEditor()
        {
            InitializeComponent();
            PlotConfiguration();
            InitialPlotSetup();
            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                LoadAudioWaveform(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text);
            }
            //verticalLine = WaveformsPlot.Plot.AddVerticalLine(0, Color.Red, 2);            
        }

        public void CachedSound(string filePath)
        {
            using var audioReader = new AudioFileReader(filePath);
            WaveFormat = audioReader.WaveFormat;
            var wholeFile = new List<float>((int)(audioReader.Length / 4));
            var readBuffer = new float[WaveFormat.SampleRate * WaveFormat.Channels];
            int samplesRead;
            while ((samplesRead = audioReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                wholeFile.AddRange(readBuffer.Take(samplesRead));
            }
            audioData = wholeFile.ToArray();
            audioData_Backup = wholeFile.ToArray();
        }

        public void LoadAudioWaveform(string filePath)
        {
            CachedSound(filePath);
            this.Text = "Destructive Effects Editor -> " + filePath;
            FilenameLabel.Text = filePath;
            PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioData);
            if (WaveFormat.Channels == 1)
                ChannelsLabel.Text = "Mono";
            else if (WaveFormat.Channels == 2)
                ChannelsLabel.Text = "Stereo";
            else
                ChannelsLabel.Text = "Unsupported Channels #";
            SamplerateLabel.Text = WaveFormat.SampleRate + " kHz";
            PlotWaveform(audioData);
            TableLayoutPanelFD.Visible = true;
            NormalizeButton.Enabled = true;
            VolumeUpButton.Enabled = true;
            VolumeDownButton.Enabled = true;
            TrimButton.Enabled = true;
        }

        public void ResetDestructiveEffectsEditorValues()
        {
            this.Text = "Destructive Effects Editor";
            InitialPlotSetup();
            TableLayoutPanelFD.Visible = false;
            NormalizeButton.Enabled = false;
            VolumeUpButton.Enabled = false;
            VolumeDownButton.Enabled = false;
            TrimButton.Enabled = false;
            RevertButton.Visible = false;
            SaveButton.Enabled = false;
        }

        private void NormalizeButton_Click(object sender, EventArgs e)
        {
            if (TableLayoutPanelFD.Visible == true)
            {
                audioData.CopyTo(audioData_Backup, 0);
                audioData = DestructiveAudioTools.Normalize(audioData, PeakLabel.Text);
                PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioData);
                RevertButton.Visible = true;
                SaveButton.Enabled = true;
                if (!this.Text.EndsWith("*"))
                    this.Text = this.Text + "*";
                InitialPlotSetup();
                PlotWaveform(audioData);
            }
        }

        private void VolumeUpButton_Click(object sender, EventArgs e)
        {
            audioData.CopyTo(audioData_Backup, 0);
            audioData = DestructiveAudioTools.VolumeUp(audioData, PeakLabel.Text);
            PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioData);
            RevertButton.Visible = true;
            SaveButton.Enabled = true;
            if (!this.Text.EndsWith("*"))
                this.Text = this.Text + "*";
            InitialPlotSetup();
            PlotWaveform(audioData);
        }

        private void VolumeDownButton_Click(object sender, EventArgs e)
        {
            audioData.CopyTo(audioData_Backup, 0);
            audioData = DestructiveAudioTools.VolumeDown(audioData, PeakLabel.Text);
            PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioData);
            RevertButton.Visible = true;
            SaveButton.Enabled = true;
            if (!this.Text.EndsWith("*"))
                this.Text = this.Text + "*";
            InitialPlotSetup();
            PlotWaveform(audioData);
        }

        private void TrimButton_Click(object sender, EventArgs e)
        {
            if (waveformSpan.X1 != waveformSpan.X2)
            {
                audioData.CopyTo(audioData_Backup, 0);
                audioData = DestructiveAudioTools.Trim(audioData, waveformSpan.X1, waveformSpan.X2, WaveFormat.SampleRate,WaveFormat.Channels);
                PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioData);
                RevertButton.Visible = true;
                SaveButton.Enabled = true;
                if (!this.Text.EndsWith("*"))
                    this.Text = this.Text + "*";
                InitialPlotSetup();
                PlotWaveform(audioData);
            }            
        }

        private void RevertButton_Click(object sender, EventArgs e)
        {            
            Array.Resize(ref audioData, audioData_Backup.Length);
            audioData_Backup.CopyTo(audioData, 0);
            PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioData);
            RevertButton.Visible = false;
            SaveButton.Enabled = false;
            if (this.Text.Contains(".wav*"))
                this.Text = this.Text.Replace(".wav*", ".wav");
            InitialPlotSetup();
            PlotWaveform(audioData);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            RevertButton.Visible = false;
            SaveButton.Enabled = false;
            string backupPath = FilenameLabel.Text.Replace(".wav", "");

            DialogResult dialogResult = MessageBox.Show("Do you want to replace the current file?", "Save Changes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Move(FilenameLabel.Text, backupPath + "_BACKUP.wav");
                using (WaveFileWriter writer = new WaveFileWriter(FilenameLabel.Text, WaveFormat))
                {
                    writer.WriteSamples(audioData, 0, audioData.Length);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                using (WaveFileWriter writer = new WaveFileWriter(backupPath + "_EDIT.wav", WaveFormat))
                {
                    writer.WriteSamples(audioData, 0, audioData.Length);
                }
            }
            this.Text = "Destructive Effects Editor";
            InitialPlotSetup();
            FilenameLabel.Text = "";
            PositionLabel.Text = "Position (sec): ";
            PeakLabel.Text = "Peak: ";
            ChannelsLabel.Text = "";
            SamplerateLabel.Text = "";
        }

        public void PlotWaveform(float[] audio)
        {
            if (WaveFormat.Channels == 1)
            {
                var chM = WaveformsPlot.Plot.AddSignal(audio, WaveFormat.SampleRate);
                chM.Color = Color.DarkRed;
                WaveformsPlot.Plot.SetAxisLimitsY(-1, 1);
                WaveformsPlot.Plot.AxisAutoX(0);
                limits = WaveformsPlot.Plot.GetAxisLimits();
                WaveformsPlot.Plot.SetOuterViewLimits(0, limits.XMax, -1, 1);
            }

            else if (WaveFormat.Channels == 2)
            {
                int length = audio.Length / 2;
                float[] audioL = new float[length];
                float[] audioR = new float[length];

                for (int i = 0; i < length; i++)
                {
                    audioL[i] = audio[2 * i];
                    audioR[i] = audio[2 * i + 1];
                }

                var chL = WaveformsPlot.Plot.AddSignal(audioL, WaveFormat.SampleRate);
                chL.Color = Color.DarkRed;
                chL.OffsetY = 1;

                var chR = WaveformsPlot.Plot.AddSignal(audioR, WaveFormat.SampleRate);
                chR.Color = Color.ForestGreen;
                chR.OffsetY = -1;
                WaveformsPlot.Plot.SetAxisLimitsY(-2, 2);
                WaveformsPlot.Plot.AxisAutoX(0);
                limits = WaveformsPlot.Plot.GetAxisLimits();
                WaveformsPlot.Plot.SetOuterViewLimits(0, limits.XMax, -2, 2);
            }
            WaveformsPlot.Refresh();
        }

        private void PlotConfiguration()
        {
            WaveformsPlot.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
            WaveformsPlot.Configuration.AllowDroppedFramesWhileDragging = true;
            WaveformsPlot.Configuration.LockVerticalAxis = true;
            WaveformsPlot.Configuration.EnablePlotObjectEditor = false;
        }

        private void InitialPlotSetup()
        {
            WaveformsPlot.Reset();
            WaveformsPlot.Plot.Title("");
            WaveformsPlot.Plot.XLabel("Time (seconds)");
            WaveformsPlot.Plot.YLabel("Audio level");
            WaveformsPlot.Plot.SetAxisLimitsY(-2, 2);
            WaveformsPlot.Plot.YAxis.Grid(false);
            waveformSpan = WaveformsPlot.Plot.AddHorizontalSpan(0, 0, Color.LightGray);
            waveformSpan.IsVisible = false;
            WaveformsPlot.Refresh();
        }

        private void WaveformsPlot_MouseMove(object sender, MouseEventArgs e)
        {
            (double x, double y) = WaveformsPlot.GetMouseCoordinates();
            limits = WaveformsPlot.Plot.GetAxisLimits();
            double position = 0;
            if (x >= limits.XMin && x <= limits.XMax)
            {
                position = x;
            }
            else if (x > limits.XMax)
            {
                position = limits.XMax;
            }

            if (mouseDown)
            {
                waveformSpan.X2 = position;
                WaveformsPlot.Render();
            }
            PositionLabel.Text = "Position (sec): " + position.ToString("0.00");
        }

        private void WaveformsPlot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mouseDown == false)
            {
                (double x, double y) = WaveformsPlot.GetMouseCoordinates();
                limits = WaveformsPlot.Plot.GetAxisLimits();

                if (x >= limits.XMin && x <= limits.XMax)
                {
                    waveformSpan.X1 = x;
                    waveformSpan.X2 = x;
                }

                else if (x < limits.XMin)
                {
                    waveformSpan.X1 = limits.XMin;
                    waveformSpan.X2 = limits.XMin;
                }

                else if (x > limits.XMax)
                {
                    waveformSpan.X1 = limits.XMax;
                    waveformSpan.X2 = limits.XMax;
                }

                waveformSpan.IsVisible = true;
                mouseDown = true;
            }

            else if (e.Button == MouseButtons.Left && mouseDown == true)
            {
                mouseDown = false;
            }

            else if (e.Button == MouseButtons.Right && waveformSpan.IsVisible)
            {
                waveformSpan.IsVisible = false;
                mouseDown = false;
            }
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
