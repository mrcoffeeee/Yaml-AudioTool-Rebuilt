using NAudio.Gui;
using NAudio.Wave;
using ScottPlot;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Vortice.Multimedia;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class DestructiveEffectsEditor : Form
    {
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        private bool mouseDown = false;
        private double scrollScaler = 1000.0;
        private float[] audioDataM, audioDataL, audioDataR, audioData_BackupM, audioData_BackupL, audioData_BackupR;
        private AxisLimits limits;
        private readonly ScottPlot.Plottable.VLine[] markerLines = new ScottPlot.Plottable.VLine[10];
        private readonly ScottPlot.Plottable.MarkerPlot[] markerLabels = new ScottPlot.Plottable.MarkerPlot[10];
        private ScottPlot.Plottable.HSpan waveformSpan;
        private NAudio.Wave.WaveFormat WaveFormat;

        public DestructiveEffectsEditor()
        {
            InitializeComponent();
            Text = formMain.Text + ": Destructive Effects Editor";
            EnableGuiElements(false);
            PlotConfiguration();
            InitialPlotSetup();
            FadeComboBox.SelectedIndex = 0;
            if (formMain.FilelistView.SelectedItems.Count == 1)
            {
                string bwString = "LOADAUDIO|" + formMain.FilelistView.SelectedItems[0].SubItems[formMain.FilelistView.Columns.IndexOf(formMain.filepathHeader)].Text;
                DEEBackgroundWorker.RunWorkerAsync(bwString);
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

            if (cueReader.Cues != null)
            {
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
        }

        public void LoadAudioWaveform(string filePath)
        {
            CachedSound(filePath);
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
            PlotHScrollBar.Maximum = Convert.ToInt32(scrollScaler * limits.XMax);
            PlotHScrollBar.LargeChange = PlotHScrollBar.Maximum;
        }

        private void EnableGuiElements(bool flag)
        {
            if (flag)
            {
                TableLayoutPanelDEE.Enabled = true;
            }
            else
            {
                TableLayoutPanelDEE.Enabled = false;
            }
        }

        public void ResetDestructiveEffectsEditorValues()
        {
            Text = formMain.Text + ": Destructive Effects Editor";
            InitialPlotSetup();
            WaveformsPlot.Enabled = false;
            EnableGuiElements(false);
            TableLayoutPanelChanges.Enabled = false;
            SaveButton.BackColor = SystemColors.Control;
        }

        private void NormalizeButton_Click(object sender, EventArgs e)
        {            
            string bwString = "NORMALIZEAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioNormalization()
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
        }

        private void VolumeUpButton_Click(object sender, EventArgs e)
        {
            string bwString = "VOLUPAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioVolumeUp()
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
        }

        private void VolumeDownButton_Click(object sender, EventArgs e)
        {
            string bwString = "VOLDOWNAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioVolumeDown()
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
        }

        private void TrimButton_Click(object sender, EventArgs e)
        {
            string bwString = "TRIMAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioTrim()
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
            }
        }

        private void FadeButton_Click(object sender, EventArgs e)
        {
            string bwString = "FADEAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioFade()
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

            if (!Text.EndsWith("*"))
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LightGreen;
                Text += "*";
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
                if (Text.Contains(".wav*"))
                    Text = Text.Replace(".wav*", ".wav");
                SaveButton.Enabled = false;
                SaveButton.BackColor = SystemColors.Control;
            }
            WaveformsPlot.Refresh();
        }

        private void ZoomResetButton_Click(object sender, EventArgs e)
        {
            WaveformsPlot.Plot.AxisAutoX();
            WaveformsPlot.Render();
            PlotHScrollBar.LargeChange = PlotHScrollBar.Maximum;
        }

        private void RevertButton_Click(object sender, EventArgs e)
        {
            string bwString = "REVERTAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioRevert()
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
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string bwString = "SAVEAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioSave()
        {
            TableLayoutPanelChanges.Enabled = false;
            string backupPath = FilenameLabel.Text.Replace(".wav", "");
            TopMost = false;

            DialogResult dialogResult = MessageBox.Show("Do you want to replace the current file?", "Save Changes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                TopMost = true;
                File.Move(FilenameLabel.Text, backupPath + "_BACKUP.wav");
                using CueWaveFileWriter writer = new(FilenameLabel.Text, WaveFormat);
                writer.WriteSamples(audioDataM, 0, audioDataM.Length);
                for (int i = 0; i < markerLines.Length; i++)
                {
                    if (markerLines[i].IsVisible == true)
                    {
                        writer.AddCue((int)(markerLines[i].X * WaveFormat.SampleRate), markerLabels[i].Text);
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                TopMost = true;
                using CueWaveFileWriter writer = new(backupPath + "_EDIT.wav", WaveFormat);
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

        public void PlotWaveform()
        {
            if (WaveFormat.Channels == 1)
            {
                //MessageBox.Show(audioDataM.Length.ToString());
                if (audioDataM.Length > 0)
                {
                    var chM = WaveformsPlot.Plot.AddSignal(audioDataM, WaveFormat.SampleRate);
                    chM.Color = Color.DarkRed;

                    WaveformsPlot.Plot.SetAxisLimitsY(-1, 1);
                    WaveformsPlot.Plot.AxisAutoX(0);
                    limits = WaveformsPlot.Plot.GetAxisLimits();
                    WaveformsPlot.Plot.XAxis.SetBoundary(0, limits.XMax);
                    WaveformsPlot.Plot.YAxis.SetBoundary(-1, 1);
                }
            }

            else if (WaveFormat.Channels == 2)
            {
                //MessageBox.Show(audioDataL.Length.ToString() + "--" + audioDataR.Length.ToString());
                if (audioDataL.Length > 0 && audioDataR.Length > 0)
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
                    WaveformsPlot.Plot.XAxis.SetBoundary(0, limits.XMax);
                    WaveformsPlot.Plot.YAxis.SetBoundary(-2, 2);
                }
            }
            WaveformsPlot.Plot.XAxis.SetZoomInLimit(.001);
            WaveformsPlot.Refresh();
        }

        private void PlotConfiguration()
        {
            WaveformsPlot.Configuration.AllowDroppedFramesWhileDragging = true;
            WaveformsPlot.Configuration.DoubleClickBenchmark = false;
            WaveformsPlot.Configuration.EnablePlotObjectEditor = false;
            WaveformsPlot.Configuration.LockVerticalAxis = true;
            WaveformsPlot.Configuration.Quality = ScottPlot.Control.QualityMode.Low;
        }

        private void InitialPlotSetup()
        {
            WaveformsPlot.Reset();
            WaveformsPlot.Plot.Title("");
            WaveformsPlot.Plot.XLabel("Time (seconds)");
            WaveformsPlot.Plot.YLabel("Audio level");
            WaveformsPlot.Plot.SetAxisLimitsX(0, 1);
            WaveformsPlot.Plot.SetAxisLimitsY(-1, 1);
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
            (double x, _) = WaveformsPlot.GetMouseCoordinates();
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
            else if (x < limits.XMin)
                position = limits.XMin;

            if (mouseDown)
            {
                waveformSpan.X2 = position;
            }
            PlotHScrollBar.LargeChange = Convert.ToInt32(scrollScaler * (limits.XMax - limits.XMin));
            PlotHScrollBar.Value = Convert.ToInt32(limits.XMin * scrollScaler);
            WaveformsPlot.Render();
            PositionLabel.Text = "Position (sec): " + position.ToString("0.000");
        }

        private void WaveformsPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            limits = WaveformsPlot.Plot.GetAxisLimits();
            PlotHScrollBar.LargeChange = Convert.ToInt32(scrollScaler * (limits.XMax - limits.XMin));
            PlotHScrollBar.Value = Convert.ToInt32(limits.XMin * scrollScaler);
            WaveformsPlot.Render();
        }

        private void WaveformsPlot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mouseDown == false)
            {
                (double x, _) = WaveformsPlot.GetMouseCoordinates();
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

        private void PlotHScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            double xMin = PlotHScrollBar.Value / scrollScaler;
            double xMax = (PlotHScrollBar.Value + PlotHScrollBar.LargeChange) / scrollScaler;
            if (xMax > xMin)
            {
                if (xMax > PlotHScrollBar.Maximum)
                    xMax = PlotHScrollBar.Maximum;
                WaveformsPlot.Plot.SetAxisLimitsX(xMin, xMax);
                WaveformsPlot.Render();
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

        private void DEEBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            EnableGuiElements(false);
            string temp = Text;
            string choice = e.Argument.ToString().Split('|').First();
            string argument = e.Argument.ToString().Split('|').Last();

            if (choice == "LOADAUDIO")
            {
                temp = formMain.Text + ": Destructive Effects Editor -> " + argument;
                Text = "PLEASE WAIT - CALCULATING AUDIO DATA ...";
                LoadAudioWaveform(argument);                
            }
            else if (choice == "NORMALIZEAUDIO")
            {
                Text = "PLEASE WAIT - PROCESSING AUDIO NORMALIZATION ...";
                AudioNormalization();
            }
            else if (choice == "VOLUPAUDIO")
            {
                Text = "PLEASE WAIT - PROCESSING AUDIO VOLUME UP ...";
                AudioVolumeUp();
            }
            else if (choice == "VOLDOWNAUDIO")
            {
                Text = "PLEASE WAIT - PROCESSING AUDIO VOLUME DOWN ...";
                AudioVolumeDown();
            }
            else if (choice == "TRIMAUDIO")
            {
                Text = "PLEASE WAIT - PROCESSING AUDIO TRIM ...";
                AudioTrim();
            }
            else if (choice == "FADEAUDIO")
            {
                Text = "PLEASE WAIT - PROCESSING AUDIO FADE ...";
                AudioFade();
            }
            else if (choice == "REVERTAUDIO")
            {
                Text = "PLEASE WAIT - UNDOING AUDIO PROCESSING ...";
                AudioRevert();
            }
            else if (choice == "SAVEAUDIO")
            {
                Text = "PLEASE WAIT - SAVING AUDIO FILE ...";
                AudioSave();
            }
            WaveformsPlot.Render();
            Text = temp;
            e.Result = choice;
        }

        private void DEEBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString() != "REVERTAUDIO" && e.Result.ToString() != "SAVEAUDIO")
            {
                if (e.Result.ToString() != "LOADAUDIO")
                {
                    if (!Text.EndsWith("*"))
                    {
                        Text += "*";
                    }
                }
                else
                {
                    EnableGuiElements(true);
                    return;
                }
                if (e.Result.ToString() == "TRIMAUDIO")
                {
                    InitialPlotSetup();
                    PlotWaveform();
                    PlotHScrollBar.Maximum = Convert.ToInt32(scrollScaler * limits.XMax);
                    PlotHScrollBar.LargeChange = PlotHScrollBar.Maximum;
                }

                TableLayoutPanelChanges.Enabled = true;
                SaveButton.BackColor = Color.LightGreen;                
            }
            else if (e.Result.ToString() == "REVERTAUDIO")
            {
                if (Text.Contains(".wav*"))
                    Text = Text.Replace(".wav*", ".wav");
                TableLayoutPanelChanges.Enabled = false;
                SaveButton.BackColor = SystemColors.Control;
                InitialPlotSetup();
                PlotWaveform();
                PlotHScrollBar.Maximum = Convert.ToInt32(scrollScaler * limits.XMax);
                PlotHScrollBar.LargeChange = PlotHScrollBar.Maximum;
            }
            else if (e.Result.ToString() == "SAVEAUDIO")
            {
                Text = formMain.Text + ": Destructive Effects Editor";
                InitialPlotSetup();
                ResetDestructiveEffectsEditorValues();
                FilenameLabel.Text = "";
                PositionLabel.Text = "Position (sec): ";
                PeakLabel.Text = "Peak: ";
                ChannelsLabel.Text = "";
                SamplerateLabel.Text = "";
            }

            EnableGuiElements(true);
        }
    }
}
