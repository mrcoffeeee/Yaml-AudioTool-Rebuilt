using NAudio.Gui;
using NAudio.Wave;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Vortice.Mathematics;
using Vortice.Multimedia;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class DestructiveEffectsEditor : Form
    {
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        private bool mouseDown = false;
        private readonly double scrollScaler = 1.000;
        private float[] audioDataM, audioDataL, audioDataR, audioData_BackupM, audioData_BackupL, audioData_BackupR;
        private double mousePositionX = 0;
        private double xLimit = 0;
        private AxisLimits limits;
        private AxisLine PlottableBeingDragged;
        private readonly VerticalLine[] markerLines = new VerticalLine[10];
        private HorizontalLine stereoLine = new();
        private HorizontalSpan waveformSpan;
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
            List<float> wholeFile = new(Convert.ToInt32(audioReader.Length / 4));
            var readBuffer = new float[WaveFormat.SampleRate * WaveFormat.Channels];
            int samplesRead;
            while ((samplesRead = audioReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                wholeFile.AddRange(readBuffer.Take(samplesRead));
            }
            audioDataM = [.. wholeFile];
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
                    markerLines[i].Text = cues[i].Label;
                    markerLines[i].LabelOppositeAxis = true;
                    markerLines[i].IsVisible = true;
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
            //PlotHScrollBar.Maximum = Convert.ToInt32(scrollScaler * limits.XRange.Max);
            //PlotHScrollBar.LargeChange = PlotHScrollBar.Maximum;
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
                var limits = WaveformsPlot.Plot.Axes.GetLimits();
                startPoint = limits.XRange.Min;
                endPoint = xLimit;
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
                var limits = WaveformsPlot.Plot.Axes.GetLimits();
                startPoint = limits.XRange.Min;
                endPoint = xLimit;
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
                var limits = WaveformsPlot.Plot.Axes.GetLimits();
                startPoint = limits.XRange.Min;
                endPoint = xLimit;
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
                var limits = WaveformsPlot.Plot.Axes.GetLimits();
                startPoint = limits.XRange.Min;
                endPoint = xLimit;
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

        private void AddMarker(int i, double posX)
        {
            markerLines[i].X = posX;
            markerLines[i].Text = "M " + (i + 1);
            markerLines[i].IsVisible = true;
            markerLines[i].IsDraggable = true;
            WaveformsPlot.Plot.MoveToFront(markerLines[i]);

            if (!Text.EndsWith('*'))
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = System.Drawing.Color.LightGreen;
                Text += "*";
            }
            WaveformsPlot.Refresh();
        }

        private void RemoveMarkerButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < markerLines.Length; i++)
            {
                if (markerLines[i].Color == ScottPlot.Colors.LimeGreen)
                {
                    markerLines[i].Color = ScottPlot.Colors.Red;
                    markerLines[i].IsVisible = false;
                    markerLines[i].X = 0;
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
            WaveformsPlot.Plot.Axes.AutoScaleX();
            WaveformsPlot.Refresh();
            //PlotHScrollBar.LargeChange = PlotHScrollBar.Maximum;
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
                        writer.AddCue((int)(markerLines[i].X * WaveFormat.SampleRate), markerLines[i].Text);
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
                        writer.AddCue((int)(markerLines[i].X * WaveFormat.SampleRate), markerLines[i].Text);
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
                    var chM = WaveformsPlot.Plot.Add.Signal(audioDataM, 1 / Convert.ToDouble(WaveFormat.SampleRate));
                    chM.Color = ScottPlot.Colors.DarkRed;
                    
                    WaveformsPlot.Plot.Axes.SetLimitsY(-1, 1);
                    limits = WaveformsPlot.Plot.Axes.GetLimits();
                    xLimit = audioDataM.Length / Convert.ToDouble(WaveFormat.SampleRate);
                    WaveformsPlot.Plot.Axes.SetLimitsX(0, xLimit);
                }
            }

            else if (WaveFormat.Channels == 2)
            {
                //MessageBox.Show(audioDataL.Length.ToString() + "--" + audioDataR.Length.ToString());
                if (audioDataL.Length > 0 && audioDataR.Length > 0)
                {
                    var chL = WaveformsPlot.Plot.Add.Signal(audioDataL, 1 / Convert.ToDouble(WaveFormat.SampleRate));
                    chL.Color = ScottPlot.Colors.DarkRed;
                    chL.Data.YOffset = 1;

                    var chR = WaveformsPlot.Plot.Add.Signal(audioDataR, 1 / Convert.ToDouble(WaveFormat.SampleRate));
                    chR.Color = ScottPlot.Colors.ForestGreen;
                    chR.Data.YOffset = -1;

                    WaveformsPlot.Plot.Axes.SetLimitsY(-2, 2);
                    limits = WaveformsPlot.Plot.Axes.GetLimits();
                    xLimit = audioDataL.Length / Convert.ToDouble(WaveFormat.SampleRate);
                    WaveformsPlot.Plot.Axes.SetLimitsX(0, xLimit);
                    stereoLine.IsVisible = true;
                }
            }

            //SET SCROLL BOUNDARIES & ZOOM
            ScottPlot.AxisRules.LockedVertical vlockRule = new(WaveformsPlot.Plot.Axes.Left, limits.Bottom, limits.Top);
            ScottPlot.AxisRules.MaximumBoundary boundaryRule = new(xAxis: WaveformsPlot.Plot.Axes.Bottom, yAxis: WaveformsPlot.Plot.Axes.Left, limits: new AxisLimits(0, xLimit, limits.Bottom, limits.Top));
            ScottPlot.AxisRules.MinimumSpan minspanRule = new(xAxis: WaveformsPlot.Plot.Axes.Bottom, yAxis: WaveformsPlot.Plot.Axes.Left, xSpan: .001, ySpan: 1);
            WaveformsPlot.Plot.Axes.Rules.Clear();
            WaveformsPlot.Plot.Axes.Rules.Add(vlockRule);
            WaveformsPlot.Plot.Axes.Rules.Add(boundaryRule);
            WaveformsPlot.Plot.Axes.Rules.Add(minspanRule);
            WaveformsPlot.Refresh();
        }

        private static void PlotConfiguration()
        {
            //WaveformsPlot.Configuration.AllowDroppedFramesWhileDragging = true;
            //WaveformsPlot.Configuration.DoubleClickBenchmark = false;
            //WaveformsPlot.Configuration.EnablePlotObjectEditor = false;
            //WaveformsPlot.Configuration.LockVerticalAxis = true;
            //WaveformsPlot.Configuration.Quality = ScottPlot.Control.QualityMode.Low;

        }

        private void InitialPlotSetup()
        {
            WaveformsPlot.Reset();
            WaveformsPlot.Plot.Title("");
            WaveformsPlot.Plot.XLabel("Time (seconds)");
            WaveformsPlot.Plot.YLabel("Audio level");
            WaveformsPlot.Plot.Axes.SetLimitsX(0, 1);
            WaveformsPlot.Plot.Axes.SetLimitsY(-1, 1);
            WaveformsPlot.Plot.Grid.XAxisStyle.IsVisible = false;
            waveformSpan = WaveformsPlot.Plot.Add.HorizontalSpan(0, 0, ScottPlot.Colors.LightGray);
            waveformSpan.IsVisible = false;

            for (int i = 0; i < markerLines.Length; i++)
            {
                markerLines[i] = WaveformsPlot.Plot.Add.VerticalLine(0, 3, ScottPlot.Colors.Red, LinePattern.Solid);
                markerLines[i].LabelFontSize = 11;
                markerLines[i].LabelOppositeAxis = true;
                markerLines[i].IsVisible = false;
            }
            WaveformsPlot.Plot.Axes.Top.MinimumSize = 30;

            stereoLine = WaveformsPlot.Plot.Add.HorizontalLine(0, 1, ScottPlot.Colors.Black, LinePattern.Solid);
            stereoLine.Y = 0;
            stereoLine.IsVisible = false;

            WaveformsPlot.Plot.HideLegend();

            // OPTIONS FOR DISPLAYING THE MARKER LEGEND
            /*ScottPlot.Panels.LegendPanel panel = new(WaveformsPlot.Plot.Legend)
            {
                Edge = Edge.Right,
                Alignment = Alignment.UpperCenter
            };
            WaveformsPlot.Plot.Axes.AddPanel(panel);*/

            WaveformsPlot.Refresh();
        }

        private void WaveformsPlot_MouseMove(object sender, MouseEventArgs e)
        {
            WaveformsPlot.MouseMove += (s, e) =>
            {
                Pixel mousePixel = new(e.X, e.Y);
                Coordinates mouseCoordinates = WaveformsPlot.Plot.GetCoordinates(mousePixel);

                limits = WaveformsPlot.Plot.Axes.GetLimits();
                double x = mouseCoordinates.X;
                if (x >= limits.XRange.Min && x <= xLimit)
                {
                    mousePositionX = x;
                }
                else if (x > xLimit)
                {
                    mousePositionX = xLimit;
                }
                else if (x < limits.XRange.Min)
                    mousePositionX = limits.XRange.Min;

                if (mouseDown && Cursor != Cursors.Hand)
                {
                    waveformSpan.X2 = mousePositionX;
                }

                //PlotHScrollBar.LargeChange = Convert.ToInt32(scrollScaler * (limits.XRange.Max - limits.XRange.Min));
                //PlotHScrollBar.Value = Convert.ToInt32(limits.XRange.Min * scrollScaler);
                PositionLabel.Text = "Position (sec): " + mousePositionX.ToString("0.000");
                            
            };

            // this rectangle is the area around the mouse in coordinate units
            CoordinateRect rect = WaveformsPlot.Plot.GetCoordinateRect(e.X, e.Y, radius: 1);

            if (PlottableBeingDragged is null)
            {
                // set cursor based on what's beneath the plottable
                var lineUnderMouse = GetLineUnderMouse(e.X, e.Y);
                if (lineUnderMouse is null) Cursor = Cursors.Default;
                else if (lineUnderMouse.IsDraggable && lineUnderMouse is VerticalLine) Cursor = Cursors.Hand;
            }
            else
            {
                if (PlottableBeingDragged is VerticalLine vl)
                {                    
                    vl.X = rect.HorizontalCenter;
                    if (vl.X < limits.XRange.Min)
                    {
                        vl.X = limits.XRange.Min;
                    }

                    else if (vl.X > xLimit)
                    {
                        vl.X = xLimit;
                    }
                }
                WaveformsPlot.Refresh();
            }
        }

        private void WaveformsPlot_MouseWheel(object sender, MouseEventArgs e)
        {

        }

        private void WaveformsPlot_DoubleClick(object sender, EventArgs e)
        {
            for (int i = 0; i < markerLines.Length; i++)
            {
                if (markerLines[i].IsVisible == false)
                {
                    AddMarker(i, mousePositionX);
                    return;
                }
            }
        }

        private void WaveformsPlot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mouseDown == false)
            {
                if (mousePositionX >= limits.XRange.Min && mousePositionX <= xLimit)
                {
                    waveformSpan.X1 = mousePositionX;
                    waveformSpan.X2 = mousePositionX;
                }

                else if (mousePositionX < limits.XRange.Min)
                {
                    waveformSpan.X1 = limits.XRange.Min;
                    waveformSpan.X2 = limits.XRange.Min;
                }

                else if (mousePositionX > xLimit)
                {
                    waveformSpan.X1 = xLimit;
                    waveformSpan.X2 = xLimit;
                }

                /*for (int i = 0; i < markerLines.Length; i++)
                {
                    markerLines[i].Color = ScottPlot.Colors.LimeGreen;
                }*/
                RemoveMarkerButton.Enabled = false;
                waveformSpan.IsVisible = true;
                mouseDown = true;
            }

            var lineUnderMouse = GetLineUnderMouse(e.X, e.Y);
            if (lineUnderMouse is not null)
            {
                PlottableBeingDragged = lineUnderMouse;

                if (e.Button == MouseButtons.Right)
                {
                    if (PlottableBeingDragged.Color == ScottPlot.Colors.LimeGreen)
                    {
                        PlottableBeingDragged.Color = ScottPlot.Colors.Red;
                        foreach (var ml in markerLines)
                        {
                            if (ml.Color == ScottPlot.Colors.LimeGreen)
                            {
                                RemoveMarkerButton.Enabled = true;
                            }
                            else
                            {
                                RemoveMarkerButton.Enabled = false;
                            }
                        }
                    }

                    else if (PlottableBeingDragged.Color == ScottPlot.Colors.Red)
                    {
                        PlottableBeingDragged.Color = ScottPlot.Colors.LimeGreen;
                        RemoveMarkerButton.Enabled = true;
                    }
                }
                
   //             WaveformsPlot.Interaction.Disable(); // disable panning while dragging
            }
        }

        private void WaveformsPlot_MouseUp(object sender, MouseEventArgs e)
        {
            PlottableBeingDragged = null;
            mouseDown = false;
 //           WaveformsPlot.Interaction.Enable(); // enable panning again
            WaveformsPlot.Refresh();
        }

        private void PlotHScrollBar_Scroll(object sender, ScrollEventArgs e)
        {

        }


        /*private void WaveformsPlot_MouseMove(object sender, MouseEventArgs e)
        {
            //(double x, _) = WaveformsPlot.GetMouseCoordinates();
            WaveformsPlot.MouseMove += (s, e) =>
            {
                Pixel mousePixel = new(e.X, e.Y);
                Coordinates mouseCoordinates = WaveformsPlot.Plot.GetCoordinates(mousePixel);
                this.Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3}";
                double x = mouseCoordinates.X;
                limits = WaveformsPlot.Plot.Axes.GetLimits();
                double position = 0;
                if (x >= limits.XRange.Min && x <= limits.XRange.Max)
                {
                    position = x;
                }
                else if (x > limits.XRange.Max)
                {
                    position = limits.XRange.Max;
                }
                else if (x < limits.XRange.Min)
                    position = limits.XRange.Min;

                if (mouseDown)
                {
                    waveformSpan.X2 = position;
                }
                PlotHScrollBar.LargeChange = Convert.ToInt32(scrollScaler * (limits.XRange.Max - limits.XRange.Min));
                PlotHScrollBar.Value = Convert.ToInt32(limits.XRange.Min * scrollScaler);
                PositionLabel.Text = "Position (sec): " + position.ToString("0.000");
                WaveformsPlot.Refresh();
            };            
        }*/

        /*private void WaveformsPlot_MouseWheel(object sender, MouseEventArgs e)
        {
            limits = WaveformsPlot.Plot.Axes.GetLimits();
            PlotHScrollBar.LargeChange = Convert.ToInt32(scrollScaler * (limits.XRange.Max - limits.XRange.Min));
            PlotHScrollBar.Value = Convert.ToInt32(limits.XRange.Min * scrollScaler);
            WaveformsPlot.Refresh();
        }*/

        /*private void WaveformsPlot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mouseDown == false)
            {
                //(double x, _) = WaveformsPlot.GetMouseCoordinates();
                WaveformsPlot.MouseMove += (s, e) =>
                {
                    Pixel mousePixel = new(e.X, e.Y);
                    Coordinates mouseCoordinates = WaveformsPlot.Plot.GetCoordinates(mousePixel);
                    this.Text = $"X={mouseCoordinates.X:N3}, Y={mouseCoordinates.Y:N3}";
                    double x = mouseCoordinates.X;
                    limits = WaveformsPlot.Plot.Axes.GetLimits();

                    if (x >= limits.XRange.Min && x <= limits.XRange.Max)
                    {
                        waveformSpan.X1 = x;
                        waveformSpan.X2 = x;
                    }

                    else if (x < limits.XRange.Min)
                    {
                        waveformSpan.X1 = limits.XRange.Min;
                        waveformSpan.X2 = limits.XRange.Min;
                    }

                    else if (x > limits.XRange.Max)
                    {
                        waveformSpan.X1 = limits.XRange.Max;
                        waveformSpan.X2 = limits.XRange.Max;
                    }

                    for (int i = 0; i < markerLines.Length; i++)
                    {
                        markerLines[i].Color = Colors.Red;
                    }
                    RemoveMarkerButton.Enabled = false;
                    waveformSpan.IsVisible = true;
                    mouseDown = true;
                };
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
                                markerLines[i].Color = Colors.LightGreen;
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
        }*/

        /*private void PlotHScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            double xMin = PlotHScrollBar.Value / scrollScaler;
            double xMax = (PlotHScrollBar.Value + PlotHScrollBar.LargeChange) / scrollScaler;
            if (xMax > xMin)
            {
                if (xMax > PlotHScrollBar.Maximum)
                    xMax = PlotHScrollBar.Maximum;
                WaveformsPlot.Plot.Axes.SetLimitsX(xMin, xMax);
                WaveformsPlot.Refresh();
            }
        }*/

        private AxisLine GetLineUnderMouse(float x, float y)
        {
            CoordinateRect rect = WaveformsPlot.Plot.GetCoordinateRect(x, y, radius: 10);

            foreach (AxisLine axLine in WaveformsPlot.Plot.GetPlottables<AxisLine>().Reverse())
            {
                if (axLine.IsUnderMouse(rect))
                    return axLine;
            }

            return null;
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
            WaveformsPlot.Refresh();
            Text = temp;
            e.Result = choice;
        }

        private void DEEBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString() != "REVERTAUDIO" && e.Result.ToString() != "SAVEAUDIO")
            {
                if (e.Result.ToString() != "LOADAUDIO")
                {
                    if (!Text.EndsWith('*'))
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
                    //PlotHScrollBar.Maximum = Convert.ToInt32(scrollScaler * limits.XRange.Max);
                    //PlotHScrollBar.LargeChange = PlotHScrollBar.Maximum;
                }

                TableLayoutPanelChanges.Enabled = true;
                SaveButton.BackColor = System.Drawing.Color.LightGreen;
            }
            else if (e.Result.ToString() == "REVERTAUDIO")
            {
                if (Text.Contains(".wav*"))
                    Text = Text.Replace(".wav*", ".wav");
                TableLayoutPanelChanges.Enabled = false;
                SaveButton.BackColor = SystemColors.Control;
                InitialPlotSetup();
                PlotWaveform();
                //PlotHScrollBar.Maximum = Convert.ToInt32(scrollScaler * limits.XRange.Max);
                //PlotHScrollBar.LargeChange = PlotHScrollBar.Maximum;
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
