using NAudio.Wave;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vortice.Multimedia;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class DestructiveEffectsEditor : Form
    {
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        bool mouseDown = false;
        float[] audioDataM, audioDataL, audioDataR, audioData_BackupM, audioData_BackupL, audioData_BackupR;
        AxisLimits limits;
        ScottPlot.Plottable.VLine[] markerLines = new ScottPlot.Plottable.VLine[10];
        ScottPlot.Plottable.MarkerPlot[] markerLabels = new ScottPlot.Plottable.MarkerPlot[10];
        ScottPlot.Plottable.HSpan waveformSpan;
        NAudio.Wave.WaveFormat WaveFormat;

        public DestructiveEffectsEditor()
        {
            InitializeComponent();
            PlotConfiguration();
            InitialPlotSetup();
            FadeComboBox.SelectedIndex = 0;
            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                LoadAudioWaveform(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text);
            }
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
            audioDataM = wholeFile.ToArray();
            audioData_BackupM = new float[audioDataM.Length];
            audioDataM.CopyTo(audioData_BackupM, 0);

            if (WaveFormat.Channels == 2)
            {
                int length = audioDataM.Length / 2;
                audioDataL = new float[length];
                audioDataR = new float[length];

                for (int i = 0; i < length; i++)
                {
                    audioDataL[i] = audioDataM[2 * i];
                    audioDataR[i] = audioDataM[2 * i + 1];
                }
                audioData_BackupL = new float[audioDataL.Length];
                audioData_BackupR = new float[audioDataR.Length];
                audioDataL.CopyTo(audioData_BackupL, 0);
                audioDataR.CopyTo(audioData_BackupR, 0);
            }

            using var cueReader = new CueWaveFileReader(filePath);
            CueList cues = cueReader.Cues;
            for (int i = 0; i < cues.Count; i++)
            {
                if (i == 10)
                {
                    MessageBox.Show("Sorry, only 10 markers per file are supported.");
                    break;
                }
                markerLines[i].X = Convert.ToDouble(cues[i].Position) / Convert.ToDouble(WaveFormat.SampleRate);
                markerLabels[i].X = Convert.ToDouble(cues[i].Position) / Convert.ToDouble(WaveFormat.SampleRate);
                markerLabels[i].Y = WaveFormat.Channels;
                markerLabels[i].Text = cues[i].Label;
                markerLines[i].IsVisible = true;
                markerLabels[i].IsVisible = true;
            }
        }

        public void LoadAudioWaveform(string filePath)
        {
            CachedSound(filePath);
            this.Text = "Destructive Effects Editor -> " + filePath;
            FilenameLabel.Text = filePath;
            if (WaveFormat.Channels == 1)
            {
                ChannelsLabel.Text = "Mono";
                PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioDataM);
            }
            else if (WaveFormat.Channels == 2)
            {
                ChannelsLabel.Text = "Stereo";
                PeakLabel.Text = "Peak: " +
                        DestructiveAudioTools.GetPeakVolume(audioDataL) +
                        " : " +
                        DestructiveAudioTools.GetPeakVolume(audioDataR);
            }

            else
                ChannelsLabel.Text = "Unsupported Channels #";
            SamplerateLabel.Text = WaveFormat.SampleRate + " kHz";
            WaveformsPlot.Enabled = true;
            PlotWaveform();
            TableLayoutPanelFD.Visible = true;
            NormalizeButton.Enabled = true;
            VolumeUpButton.Enabled = true;
            VolumeDownButton.Enabled = true;
            TrimButton.Enabled = true;
            FadeButton.Enabled = true;
            FadeComboBox.Enabled = true;
        }

        public void ResetDestructiveEffectsEditorValues()
        {
            this.Text = "Destructive Effects Editor";
            InitialPlotSetup();
            WaveformsPlot.Enabled = false;
            TableLayoutPanelFD.Visible = false;
            NormalizeButton.Enabled = false;
            VolumeUpButton.Enabled = false;
            VolumeDownButton.Enabled = false;
            TrimButton.Enabled = false;
            FadeButton.Enabled = false;
            FadeComboBox.Enabled = false;
            RevertButton.Visible = false;
            SaveButton.Enabled = false;
            SaveButton.BackColor = SystemColors.Control;
        }

        private void NormalizeButton_Click(object sender, EventArgs e)
        {
            if (TableLayoutPanelFD.Visible == true)
            {
                double startPoint;
                double endPoint;

                if (waveformSpan.X1 == waveformSpan.X2)
                {
                    var limits = WaveformsPlot.Plot.GetAxisLimits();
                    startPoint = limits.XMin;
                    endPoint = limits.XMax;
                }

                else
                {
                    startPoint = waveformSpan.X1;
                    endPoint = waveformSpan.X2;
                }

                audioDataM.CopyTo(audioData_BackupM, 0);

                if (WaveFormat.Channels == 1)
                {
                    DestructiveAudioTools.Normalize(audioDataM, startPoint, endPoint, WaveFormat.SampleRate, WaveFormat.Channels).CopyTo(audioDataM, 0);
                    PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioDataM);
                }

                else if (WaveFormat.Channels == 2)
                {
                    DestructiveAudioTools.Normalize(audioDataM, startPoint, endPoint, WaveFormat.SampleRate, WaveFormat.Channels).CopyTo(audioDataM, 0);
                    int length = audioDataM.Length / 2;
                    for (int i = 0; i < length; i++)
                    {
                        audioDataL[i] = audioDataM[2 * i];
                        audioDataR[i] = audioDataM[2 * i + 1];
                    }
                    PeakLabel.Text =
                        "Peak: " +
                        DestructiveAudioTools.GetPeakVolume(audioDataL) +
                        " : " +
                        DestructiveAudioTools.GetPeakVolume(audioDataR);
                }

                if (!this.Text.EndsWith("*"))
                {
                    this.Text += "*";
                }
                RevertButton.Visible = true;
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LightGreen;
                WaveformsPlot.Render();
            }
        }

        private void VolumeUpButton_Click(object sender, EventArgs e)
        {
            double startPoint;
            double endPoint;

            if (waveformSpan.X1 == waveformSpan.X2)
            {
                var limits = WaveformsPlot.Plot.GetAxisLimits();
                startPoint = limits.XMin;
                endPoint = limits.XMax;
            }

            else
            {
                startPoint = waveformSpan.X1;
                endPoint = waveformSpan.X2;
            }

            audioDataM.CopyTo(audioData_BackupM, 0);

            if (WaveFormat.Channels == 1)
            {
                DestructiveAudioTools.VolumeUp(audioDataM, startPoint, endPoint, WaveFormat.SampleRate, WaveFormat.Channels).CopyTo(audioDataM, 0);
                PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioDataM);
            }

            else if (WaveFormat.Channels == 2)
            {
                DestructiveAudioTools.VolumeUp(audioDataM, startPoint, endPoint, WaveFormat.SampleRate, WaveFormat.Channels).CopyTo(audioDataM, 0);
                int length = audioDataM.Length / 2;
                for (int i = 0; i < length; i++)
                {
                    audioDataL[i] = audioDataM[2 * i];
                    audioDataR[i] = audioDataM[2 * i + 1];
                }
                PeakLabel.Text =
                    "Peak: " +
                    DestructiveAudioTools.GetPeakVolume(audioDataL) +
                    " : " +
                    DestructiveAudioTools.GetPeakVolume(audioDataR);
            }

            if (!this.Text.EndsWith("*"))
            {
                this.Text += "*";
            }
            RevertButton.Visible = true;
            SaveButton.Enabled = true;
            SaveButton.BackColor = Color.LightGreen;
            WaveformsPlot.Render();
        }

        private void VolumeDownButton_Click(object sender, EventArgs e)
        {
            double startPoint;
            double endPoint;

            if (waveformSpan.X1 == waveformSpan.X2)
            {
                var limits = WaveformsPlot.Plot.GetAxisLimits();
                startPoint = limits.XMin;
                endPoint = limits.XMax;
            }

            else
            {
                startPoint = waveformSpan.X1;
                endPoint = waveformSpan.X2;
            }

            audioDataM.CopyTo(audioData_BackupM, 0);

            if (WaveFormat.Channels == 1)
            {
                DestructiveAudioTools.VolumeDown(audioDataM, startPoint, endPoint, WaveFormat.SampleRate, WaveFormat.Channels).CopyTo(audioDataM, 0);
                PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioDataM);
            }

            else if (WaveFormat.Channels == 2)
            {
                DestructiveAudioTools.VolumeDown(audioDataM, startPoint, endPoint, WaveFormat.SampleRate, WaveFormat.Channels).CopyTo(audioDataM, 0);
                int length = audioDataM.Length / 2;
                for (int i = 0; i < length; i++)
                {
                    audioDataL[i] = audioDataM[2 * i];
                    audioDataR[i] = audioDataM[2 * i + 1];
                }
                PeakLabel.Text =
                    "Peak: " +
                    DestructiveAudioTools.GetPeakVolume(audioDataL) +
                    " : " +
                    DestructiveAudioTools.GetPeakVolume(audioDataR);
            }

            if (!this.Text.EndsWith("*"))
            {
                this.Text += "*";
            }
            RevertButton.Visible = true;
            SaveButton.Enabled = true;
            SaveButton.BackColor = Color.LightGreen;
            WaveformsPlot.Render();
        }

        private void TrimButton_Click(object sender, EventArgs e)
        {
            if (waveformSpan.X1 != waveformSpan.X2)
            {
                for (int i = 0; i < markerLines.Length; i++)
                {
                    if (waveformSpan.X1 < markerLines[i].X && markerLines[i].X < waveformSpan.X2 ||
                        waveformSpan.X2 < markerLines[i].X && markerLines[i].X < waveformSpan.X1)
                    {
                        markerLines[i].IsVisible = false;
                        markerLines[i].X = 0;
                        markerLabels[i].IsVisible = false;
                        markerLabels[i].X = 0;
                        RemoveMarkerButton.Enabled = false;
                    }
                }

                Array.Resize(ref audioData_BackupM, audioDataM.Length);
                audioDataM.CopyTo(audioData_BackupM, 0);

                if (WaveFormat.Channels == 1)
                {
                    audioDataM = DestructiveAudioTools.Trim(audioDataM, waveformSpan.X1, waveformSpan.X2, WaveFormat.SampleRate, WaveFormat.Channels);
                    PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioDataM);
                }
                else if (WaveFormat.Channels == 2)
                {
                    audioDataM = DestructiveAudioTools.Trim(audioDataM, waveformSpan.X1, waveformSpan.X2, WaveFormat.SampleRate, WaveFormat.Channels);
                    int length = audioDataM.Length / 2;
                    Array.Resize(ref audioDataL, length);
                    Array.Resize(ref audioDataR, length);
                    for (int i = 0; i < length; i++)
                    {
                        audioDataL[i] = audioDataM[2 * i];
                        audioDataR[i] = audioDataM[2 * i + 1];
                    }
                    PeakLabel.Text = "Peak: " +
                        DestructiveAudioTools.GetPeakVolume(audioDataL) +
                        " : " +
                        DestructiveAudioTools.GetPeakVolume(audioDataR);
                }

                if (!this.Text.EndsWith("*"))
                {
                    this.Text += "*";
                }
                RevertButton.Visible = true;
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LightGreen;
                InitialPlotSetup();
                PlotWaveform();
            }
        }

        private void FadeButton_Click(object sender, EventArgs e)
        {
            double startPoint;
            double endPoint;

            if (waveformSpan.X1 == waveformSpan.X2)
            {
                var limits = WaveformsPlot.Plot.GetAxisLimits();
                startPoint = limits.XMin;
                endPoint = limits.XMax;
            }

            else
            {
                startPoint = waveformSpan.X1;
                endPoint = waveformSpan.X2;
            }

            audioDataM.CopyTo(audioData_BackupM, 0);


            if (WaveFormat.Channels == 1)
            {
                DestructiveAudioTools.Fade(audioDataM, startPoint, endPoint, WaveFormat.SampleRate, WaveFormat.Channels, FadeComboBox.SelectedIndex).CopyTo(audioDataM, 0);
                PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioDataM);
            }

            else if (WaveFormat.Channels == 2)
            {
                DestructiveAudioTools.Fade(audioDataM, startPoint, endPoint, WaveFormat.SampleRate, WaveFormat.Channels, FadeComboBox.SelectedIndex).CopyTo(audioDataM, 0);
                int length = audioDataM.Length / 2;
                for (int i = 0; i < length; i++)
                {
                    audioDataL[i] = audioDataM[2 * i];
                    audioDataR[i] = audioDataM[2 * i + 1];
                }
                PeakLabel.Text =
                    "Peak: " +
                    DestructiveAudioTools.GetPeakVolume(audioDataL) +
                    " : " +
                    DestructiveAudioTools.GetPeakVolume(audioDataR);
            }

            if (!this.Text.EndsWith("*"))
            {
                this.Text += "*";
            }
            RevertButton.Visible = true;
            SaveButton.Enabled = true;
            SaveButton.BackColor = Color.LightGreen;
            WaveformsPlot.Render();
        }

        private void AddMarker(int i)
        {
            markerLines[i].IsVisible = true;
            markerLabels[i].IsVisible = true;
            markerLines[i].X = waveformSpan.X1;
            markerLabels[i].X = waveformSpan.X1;
            markerLabels[i].Text = "Marker" + (i + 1);
            markerLabels[i].TextFont.Size = 14;
            markerLabels[i].Y = WaveFormat.Channels;

            if (!this.Text.EndsWith("*"))
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LightGreen;
                this.Text += "*";
            }
            WaveformsPlot.Render();
        }

        private void RemoveMarkerButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < markerLines.Length; i++)
            {
                if (waveformSpan.X1 < markerLines[i].X && markerLines[i].X < waveformSpan.X2 ||
                    waveformSpan.X2 < markerLines[i].X && markerLines[i].X < waveformSpan.X1)
                {
                    markerLines[i].IsVisible = false;
                    markerLines[i].X = 0;
                    markerLabels[i].IsVisible = false;
                    markerLabels[i].X = 0;
                    RemoveMarkerButton.Enabled = false;
                }
            }

            int count = 0;
            foreach (var line in markerLines)
            {
                if (line.IsVisible == true)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }

            if (count == 10)
            {
                if (this.Text.Contains(".wav*"))
                    this.Text = this.Text.Replace(".wav*", ".wav");
                SaveButton.Enabled = false;
                SaveButton.BackColor = SystemColors.Control;
            }
            WaveformsPlot.Refresh();
        }

        private void RevertButton_Click(object sender, EventArgs e)
        {
            if (WaveFormat.Channels == 1)
            {
                Array.Resize(ref audioDataM, audioData_BackupM.Length);
                audioData_BackupM.CopyTo(audioDataM, 0);
                PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioDataM);
            }
            else if (WaveFormat.Channels == 2)
            {
                Array.Resize(ref audioDataM, audioData_BackupM.Length);
                Array.Resize(ref audioDataL, audioData_BackupL.Length);
                Array.Resize(ref audioDataR, audioData_BackupR.Length);
                audioData_BackupM.CopyTo(audioDataM, 0);
                audioData_BackupL.CopyTo(audioDataL, 0);
                audioData_BackupR.CopyTo(audioDataR, 0);
                PeakLabel.Text = "Peak: " +
                    DestructiveAudioTools.GetPeakVolume(audioDataL) +
                    " : " +
                    DestructiveAudioTools.GetPeakVolume(audioDataR);
            }

            RevertButton.Visible = false;
            SaveButton.Enabled = false;
            SaveButton.BackColor = SystemColors.Control;
            if (this.Text.Contains(".wav*"))
                this.Text = this.Text.Replace(".wav*", ".wav");
            InitialPlotSetup();
            PlotWaveform();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            RevertButton.Visible = false;
            SaveButton.Enabled = false;
            SaveButton.BackColor = SystemColors.Control;
            string backupPath = FilenameLabel.Text.Replace(".wav", "");

            DialogResult dialogResult = MessageBox.Show("Do you want to replace the current file?", "Save Changes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Move(FilenameLabel.Text, backupPath + "_BACKUP.wav");
                using (CueWaveFileWriter writer = new(FilenameLabel.Text, WaveFormat))
                {
                    writer.WriteSamples(audioDataM, 0, audioDataM.Length);
                    for (int i = 0; i < markerLines.Length; i++)
                    {
                        if (markerLines[i].IsVisible == true)
                        {
                            writer.AddCue((int)(markerLines[i].X * WaveFormat.SampleRate), markerLabels[i].Text);
                        }
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                using (CueWaveFileWriter writer = new(backupPath + "_EDIT.wav", WaveFormat))
                {
                    writer.WriteSamples(audioDataM, 0, audioDataM.Length);
                    for (int i = 0; i < markerLines.Length; i++)
                    {
                        if (markerLines[i].IsVisible == true)
                        {
                            writer.AddCue((int)(markerLines[i].X * WaveFormat.SampleRate), markerLabels[i].Text);
                        }
                    }
                }
            }
            this.Text = "Destructive Effects Editor";
            InitialPlotSetup();
            ResetDestructiveEffectsEditorValues();
            FilenameLabel.Text = "";
            PositionLabel.Text = "Position (sec): ";
            PeakLabel.Text = "Peak: ";
            ChannelsLabel.Text = "";
            SamplerateLabel.Text = "";
        }

        public void PlotWaveform()
        {
            if (WaveFormat.Channels == 1)
            {
                var chM = WaveformsPlot.Plot.AddSignal(audioDataM, WaveFormat.SampleRate);
                chM.Color = Color.DarkRed;
                WaveformsPlot.Plot.SetAxisLimitsY(-1, 1);
                WaveformsPlot.Plot.AxisAutoX(0);
                limits = WaveformsPlot.Plot.GetAxisLimits();
                WaveformsPlot.Plot.SetOuterViewLimits(0, limits.XMax, -1, 1);
            }

            else if (WaveFormat.Channels == 2)
            {
                var chL = WaveformsPlot.Plot.AddSignal(audioDataL, WaveFormat.SampleRate);
                chL.Color = Color.DarkRed;
                chL.OffsetY = 1;

                var chR = WaveformsPlot.Plot.AddSignal(audioDataR, WaveFormat.SampleRate);
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

            for (int i = 0; i < markerLines.Length; i++)
            {
                markerLines[i] = WaveformsPlot.Plot.AddVerticalLine(0, Color.Red, 3, LineStyle.Solid);
                markerLabels[i] = WaveformsPlot.Plot.AddMarker(0, 2, MarkerShape.filledCircle, 10, Color.Red);
                markerLines[i].IsVisible = false;
                markerLabels[i].IsVisible = false;
            }
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

                for (int i = 0; i < markerLines.Length; i++)
                {
                    markerLines[i].Color = Color.Red;
                    markerLabels[i].Color = Color.Red;
                }
                RemoveMarkerButton.Enabled = false;
                waveformSpan.IsVisible = true;
                mouseDown = true;
            }

            else if (e.Button == MouseButtons.Left && mouseDown == true)
            {
                if (waveformSpan.X1 == waveformSpan.X2)
                {
                    for (int i = 0; i < markerLines.Length; i++)
                    {
                        if (markerLines[i].IsVisible == false)
                        {
                            AddMarker(i);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < markerLines.Length; i++)
                    {
                        if (markerLines[i].IsVisible == true)
                        {
                            if (waveformSpan.X1 < markerLines[i].X && markerLines[i].X < waveformSpan.X2 ||
                                waveformSpan.X2 < markerLines[i].X && markerLines[i].X < waveformSpan.X1)
                            {
                                markerLines[i].Color = Color.LightGreen;
                                markerLabels[i].Color = Color.LightGreen;
                                RemoveMarkerButton.Enabled = true;
                            }
                        }
                    }
                }
                mouseDown = false;
            }

            else if (e.Button == MouseButtons.Right && waveformSpan.IsVisible)
            {
                waveformSpan.IsVisible = false;
                waveformSpan.X1 = waveformSpan.X2;
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
