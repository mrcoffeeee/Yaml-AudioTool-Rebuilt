using NAudio.Wave;
using ScottPlot;
using ScottPlot.Plottables;
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

        private bool mouseDown = false;
        private string currentFilePath;
        private string saveTargetPath;
        private (double X, string Text)[] saveMarkerSnapshot;
        private (double X, string Label)[] loadedCues;
        private bool cueLimitExceeded;
        private float[] audioDataM, audioDataL, audioDataR, audioData_BackupM, audioData_BackupL, audioData_BackupR;
        private double mousePositionX = 0;
        private double xLimit = 0;
        private AxisLimits limits;
        private AxisLine PlottableBeingDragged;
        private readonly VerticalLine[] markerLines = new VerticalLine[10];
        private HorizontalLine stereoLine = new();
        private HorizontalSpan waveformSpan;
        private NAudio.Wave.WaveFormat WaveFormat;
        private double operationStart;
        private double operationEnd;
        private int operationFadeIndex;
        private double trimSpanX1;
        private double trimSpanX2;
        private bool[] trimMarkersToHide;
        private (double X, string Text)[] trimMarkerSnapshot;

        private readonly ScottPlot.Color leftChannelColor = new(13, 148, 136);      // Teal;
        private readonly ScottPlot.Color rightChannelColor = new(244, 63, 94);      // Coral;
        private readonly ScottPlot.Color markerColorNormal = new(55, 65, 75);       // Anthrazit
        private readonly ScottPlot.Color markerColorSelected = new(245, 158, 11);   // Amber

        public DestructiveEffectsEditor()
        {
            InitializeComponent();
            Text = formMain.Text + ": Destructive Effects Editor";
            EnableGuiElements(false);
            InitialPlotSetup();
            FadeComboBox.SelectedIndex = 0;
            if (formMain.FilelistView.SelectedItems.Count == 1)
            {
                string filepath = formMain.FilelistView.SelectedItems[0].SubItems[formMain.FilelistView.Columns.IndexOf(formMain.filepathHeader)].Text;
                string bwString = "LOADAUDIO|" + filepath;
                Text = "PLEASE WAIT - CALCULATING AUDIO DATA ...";
                EnableGuiElements(false);
                DEEBackgroundWorker.RunWorkerAsync(bwString);
            }
        }

        public void CachedSound(string filePath)
        {
            using var audioReader = new AudioFileReader(filePath);
            WaveFormat = audioReader.WaveFormat;

            // AudioFileReader liefert 32-Bit IEEE Float, also 4 Bytes pro Sample
            int totalSamples = (int)(audioReader.Length / 4);

            if (WaveFormat.Channels == 1)
            {
                audioDataM = new float[totalSamples];
                int offset = 0;
                int samplesRead;
                while ((samplesRead = audioReader.Read(audioDataM, offset, audioDataM.Length - offset)) > 0)
                {
                    offset += samplesRead;
                }
                audioData_BackupM = new float[audioDataM.Length];
                audioDataM.CopyTo(audioData_BackupM, 0);

                audioDataL = null;
                audioDataR = null;
                audioData_BackupL = null;
                audioData_BackupR = null;
            }
            else if (WaveFormat.Channels == 2)
            {
                int length = totalSamples / 2;
                audioDataL = new float[length];
                audioDataR = new float[length];

                // In Chunks lesen und direkt nach L/R splitten, kein großer Zwischen-Buffer
                float[] chunk = new float[8192]; // muss gerade Zahl sein für sauberes Stereo-Splitten
                int sampleIndex = 0;
                int samplesRead;
                while ((samplesRead = audioReader.Read(chunk, 0, chunk.Length)) > 0)
                {
                    for (int i = 0; i < samplesRead; i += 2)
                    {
                        audioDataL[sampleIndex] = chunk[i];
                        audioDataR[sampleIndex] = chunk[i + 1];
                        sampleIndex++;
                    }
                }

                audioData_BackupL = new float[length];
                audioData_BackupR = new float[length];
                audioDataL.CopyTo(audioData_BackupL, 0);
                audioDataR.CopyTo(audioData_BackupR, 0);

                audioDataM = null;
                audioData_BackupM = null;
            }

            // Cue-Daten einsammeln
            using var cueReader = new CueWaveFileReader(filePath);
            cueLimitExceeded = false;

            if (cueReader.Cues != null)
            {
                CueList cues = cueReader.Cues;
                int cueCount = Math.Min(cues.Count, markerLines.Length);
                if (cues.Count > markerLines.Length)
                {
                    cueLimitExceeded = true;
                }

                var cueList = new List<(double, string)>();
                for (int i = 0; i < cueCount; i++)
                {
                    double position = Convert.ToDouble(cues[i].Position) / Convert.ToDouble(WaveFormat.SampleRate);
                    cueList.Add((position, cues[i].Label));
                }
                loadedCues = [.. cueList];
            }
            else
            {
                loadedCues = [];
            }
        }

        public void LoadAudioWaveform(string filePath)
        {
            currentFilePath = filePath;
            CachedSound(filePath);
        }

        private void DisplayLoadedAudio()
        {
            FilenameLabel.Text = currentFilePath;

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
            {
                ChannelsLabel.Text = "Unsupported Channels #";
            }

            SamplerateLabel.Text = WaveFormat.SampleRate + " kHz";
            WaveformsPlot.Enabled = true;
            PlotWaveform();

            // Cues jetzt auf die markerLines übertragen (auf dem UI-Thread!)
            if (loadedCues != null)
            {
                for (int i = 0; i < loadedCues.Length; i++)
                {
                    markerLines[i].X = loadedCues[i].X;
                    markerLines[i].Text = loadedCues[i].Label;
                    markerLines[i].LabelOppositeAxis = true;
                    markerLines[i].IsVisible = true;
                    WaveformsPlot.Plot.MoveToFront(markerLines[i]);
                }
            }

            if (cueLimitExceeded)
            {
                MessageBox.Show("Sorry, only 10 markers per file are supported.");
            }
        }

        private void UpdatePeakLabel()
        {
            if (WaveFormat.Channels == 1)
            {
                PeakLabel.Text = "Peak: " + DestructiveAudioTools.GetPeakVolume(audioDataM);
            }
            else if (WaveFormat.Channels == 2)
            {
                PeakLabel.Text = "Peak: " +
                    DestructiveAudioTools.GetPeakVolume(audioDataL) +
                    " : " +
                    DestructiveAudioTools.GetPeakVolume(audioDataR);
            }
        }

        private float[] InterleaveStereo()
        {
            int length = audioDataL.Length;
            float[] interleaved = new float[length * 2];
            for (int i = 0; i < length; i++)
            {
                interleaved[2 * i] = audioDataL[i];
                interleaved[2 * i + 1] = audioDataR[i];
            }
            return interleaved;
        }

        private void SplitStereo(float[] interleaved)
        {
            int length = interleaved.Length / 2;
            if (audioDataL == null || audioDataL.Length != length)
            {
                audioDataL = new float[length];
                audioDataR = new float[length];
            }
            for (int i = 0; i < length; i++)
            {
                audioDataL[i] = interleaved[2 * i];
                audioDataR[i] = interleaved[2 * i + 1];
            }
        }

        private void ApplyInPlaceEffect(Func<float[], float[]> effect)
        {
            if (WaveFormat.Channels == 1)
            {
                audioDataM.CopyTo(audioData_BackupM, 0);
                effect(audioDataM).CopyTo(audioDataM, 0);
            }
            else if (WaveFormat.Channels == 2)
            {
                audioDataL.CopyTo(audioData_BackupL, 0);
                audioDataR.CopyTo(audioData_BackupR, 0);

                float[] interleaved = InterleaveStereo();
                effect(interleaved).CopyTo(interleaved, 0);
                SplitStereo(interleaved);
            }
        }

        private void EnableGuiElements(bool flag)
        {
            if (flag)
            {
                TableLayoutPanelDEE.Enabled = true;
                UseWaitCursor = false;
            }
            else
            {
                TableLayoutPanelDEE.Enabled = false;
                UseWaitCursor = true;
            }
        }

        private (double start, double end) CalculateOperationRange()
        {
            if (waveformSpan.X1 == waveformSpan.X2)
            {
                var currentLimits = WaveformsPlot.Plot.Axes.GetLimits();
                return (currentLimits.XRange.Min, xLimit);
            }
            else
            {
                return (waveformSpan.X1, waveformSpan.X2);
            }
        }

        public void ResetDestructiveEffectsEditorValues()
        {
            Text = formMain.Text + ": Destructive Effects Editor";

            // Plot aufräumen und alte Audio-Daten freigeben
            WaveformsPlot.Plot.Clear();
            audioDataM = null;
            audioDataL = null;
            audioDataR = null;
            audioData_BackupM = null;
            audioData_BackupL = null;
            audioData_BackupR = null;
            saveMarkerSnapshot = null;
            trimMarkersToHide = null;
            WaveFormat = null;
            currentFilePath = null;

            InitialPlotSetup();
            WaveformsPlot.Enabled = false;
            EnableGuiElements(false);
            TableLayoutPanelChanges.Enabled = false;
            SaveButton.BackColor = SystemColors.Control;
            GC.Collect();
        }

        private void NormalizeButton_Click(object sender, EventArgs e)
        {
            (operationStart, operationEnd) = CalculateOperationRange();
            Text = "PLEASE WAIT - PROCESSING AUDIO NORMALIZATION ...";
            EnableGuiElements(false);
            string bwString = "NORMALIZEAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioNormalization()
        {
            ApplyInPlaceEffect(data => DestructiveAudioTools.Normalize(
                audioData: data, operationStart, operationEnd, WaveFormat.SampleRate, WaveFormat.Channels));
        }

        private void VolumeUpButton_Click(object sender, EventArgs e)
        {
            (operationStart, operationEnd) = CalculateOperationRange();
            Text = "PLEASE WAIT - PROCESSING AUDIO VOLUME UP ...";
            EnableGuiElements(false);
            string bwString = "VOLUPAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioVolumeUp()
        {
            ApplyInPlaceEffect(data => DestructiveAudioTools.VolumeUp(
                audioData: data, operationStart, operationEnd, WaveFormat.SampleRate, WaveFormat.Channels));
        }

        private void VolumeDownButton_Click(object sender, EventArgs e)
        {
            (operationStart, operationEnd) = CalculateOperationRange();
            Text = "PLEASE WAIT - PROCESSING AUDIO VOLUME DOWN ...";
            EnableGuiElements(false);
            string bwString = "VOLDOWNAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioVolumeDown()
        {
            ApplyInPlaceEffect(data => DestructiveAudioTools.VolumeDown(
                audioData: data, operationStart, operationEnd, WaveFormat.SampleRate, WaveFormat.Channels));
        }

        private void TrimButton_Click(object sender, EventArgs e)
        {
            trimSpanX1 = waveformSpan.X1;
            trimSpanX2 = waveformSpan.X2;

            // Pre-Check: which marker is where?
            trimMarkersToHide = new bool[markerLines.Length];
            for (int i = 0; i < markerLines.Length; i++)
            {
                if (markerLines[i].IsVisible &&
                    ((trimSpanX1 < markerLines[i].X && markerLines[i].X < trimSpanX2) ||
                     (trimSpanX2 < markerLines[i].X && markerLines[i].X < trimSpanX1)))
                {
                    trimMarkersToHide[i] = true;
                }
            }

            // Snapshot of all visible markers for restauration
            var snapshot = new (double X, string Text)[markerLines.Length];
            for (int i = 0; i < markerLines.Length; i++)
            {
                if (markerLines[i].IsVisible && !trimMarkersToHide[i])
                {
                    snapshot[i] = (markerLines[i].X, markerLines[i].Text);
                }
                else
                {
                    snapshot[i] = (-1, null); // -1 marks "no marker to be trimmed"
                }
            }
            trimMarkerSnapshot = snapshot;

            Text = "PLEASE WAIT - PROCESSING AUDIO TRIM ...";
            EnableGuiElements(false);
            string bwString = "TRIMAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioTrim()
        {
            if (trimSpanX1 == trimSpanX2) return;

            if (WaveFormat.Channels == 1)
            {
                Array.Resize(ref audioData_BackupM, audioDataM.Length);
                audioDataM.CopyTo(audioData_BackupM, 0);
                audioDataM = DestructiveAudioTools.Trim(audioDataM, trimSpanX1, trimSpanX2, WaveFormat.SampleRate, WaveFormat.Channels);
            }
            else if (WaveFormat.Channels == 2)
            {
                // Backups vor Trim auf aktuelle Größe bringen und Daten kopieren
                Array.Resize(ref audioData_BackupL, audioDataL.Length);
                Array.Resize(ref audioData_BackupR, audioDataR.Length);
                audioDataL.CopyTo(audioData_BackupL, 0);
                audioDataR.CopyTo(audioData_BackupR, 0);

                // M ad-hoc, trimmen, zurück nach L+R splitten (SplitStereo resized automatisch)
                float[] interleaved = InterleaveStereo();
                interleaved = DestructiveAudioTools.Trim(interleaved, trimSpanX1, trimSpanX2, WaveFormat.SampleRate, WaveFormat.Channels);
                SplitStereo(interleaved);
            }
        }

        private void FadeButton_Click(object sender, EventArgs e)
        {
            (operationStart, operationEnd) = CalculateOperationRange();
            operationFadeIndex = FadeComboBox.SelectedIndex;
            Text = "PLEASE WAIT - PROCESSING AUDIO FADE ...";
            EnableGuiElements(false);
            string bwString = "FADEAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioFade()
        {
            ApplyInPlaceEffect(data => DestructiveAudioTools.Fade(
                audioData: data, operationStart, operationEnd, WaveFormat.SampleRate, WaveFormat.Channels, operationFadeIndex));
        }

        private void AddMarker(int i, double posX)
        {
            markerLines[i].X = posX;
            markerLines[i].Text = "M " + (i + 1);
            markerLines[i].IsVisible = true;
            WaveformsPlot.Plot.MoveToFront(markerLines[i]);

            if (!Text.EndsWith('*'))
            {
                TableLayoutPanelChanges.Enabled = true;
                SaveButton.BackColor = System.Drawing.Color.LightGreen;
                Text += "*";
            }
            WaveformsPlot.Refresh();
        }

        private void RemoveMarkerButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < markerLines.Length; i++)
            {
                if (markerLines[i].Color == markerColorSelected)
                {
                    markerLines[i].Color = markerColorNormal;
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
            Text = "PLEASE WAIT - UNDOING AUDIO PROCESSING ...";
            EnableGuiElements(false);
            string bwString = "REVERTAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioRevert()
        {
            if (WaveFormat.Channels == 1)
            {
                Array.Resize(ref audioDataM, audioData_BackupM.Length);
                audioData_BackupM.CopyTo(audioDataM, 0);
            }
            else if (WaveFormat.Channels == 2)
            {
                Array.Resize(ref audioDataL, audioData_BackupL.Length);
                Array.Resize(ref audioDataR, audioData_BackupR.Length);
                audioData_BackupL.CopyTo(audioDataL, 0);
                audioData_BackupR.CopyTo(audioDataR, 0);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            TableLayoutPanelChanges.Enabled = false;
            string backupPath = FilenameLabel.Text.Replace(".wav", "");
            TopMost = false;

            DialogResult dialogResult = MessageBox.Show("Do you want to replace the current file?", "Save Changes", MessageBoxButtons.YesNo);
            TopMost = true;

            if (dialogResult == DialogResult.Yes)
            {
                string backupTargetPath = backupPath + "_BACKUP.wav";
                if (File.Exists(backupTargetPath))
                {
                    File.Delete(backupTargetPath);
                }
                File.Move(FilenameLabel.Text, backupTargetPath);
                saveTargetPath = FilenameLabel.Text;
            }
            else if (dialogResult == DialogResult.No)
            {
                saveTargetPath = backupPath + "_EDIT.wav";
            }
            else
            {
                // User hat den Dialog abgebrochen
                TableLayoutPanelChanges.Enabled = true;
                return;
            }

            // Marker-Snapshot anlegen, damit der BG-Thread nicht auf die UI zugreifen muss
            var snapshot = new System.Collections.Generic.List<(double, string)>();
            for (int i = 0; i < markerLines.Length; i++)
            {
                if (markerLines[i].IsVisible)
                {
                    snapshot.Add((markerLines[i].X, markerLines[i].Text));
                }
            }
            saveMarkerSnapshot = [.. snapshot];

            Text = "PLEASE WAIT - SAVING AUDIO FILE ...";
            EnableGuiElements(false);
            string bwString = "SAVEAUDIO|";
            DEEBackgroundWorker.RunWorkerAsync(bwString);
        }

        private void AudioSave()
        {
            using CueWaveFileWriter writer = new(saveTargetPath, WaveFormat);

            if (WaveFormat.Channels == 1)
            {
                writer.WriteSamples(audioDataM, 0, audioDataM.Length);
            }
            else if (WaveFormat.Channels == 2)
            {
                // M ad-hoc bauen für den Writer
                float[] interleaved = InterleaveStereo();
                writer.WriteSamples(interleaved, 0, interleaved.Length);
            }

            foreach (var marker in saveMarkerSnapshot)
            {
                writer.AddCue((int)(marker.X * WaveFormat.SampleRate), marker.Text);
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
                    chM.Color = leftChannelColor;

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
                    chL.Color = leftChannelColor;
                    chL.Data.YOffset = 1;

                    var chR = WaveformsPlot.Plot.Add.Signal(audioDataR, 1 / Convert.ToDouble(WaveFormat.SampleRate));
                    chR.Color = rightChannelColor;
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
                markerLines[i] = WaveformsPlot.Plot.Add.VerticalLine(0, 3, markerColorNormal, LinePattern.Solid);
                markerLines[i].LabelFontSize = 11;
                markerLines[i].LabelOppositeAxis = true;
                markerLines[i].IsDraggable = true;
                markerLines[i].IsVisible = false;
            }
            WaveformsPlot.Plot.Axes.Top.MinimumSize = 30;

            stereoLine = WaveformsPlot.Plot.Add.HorizontalLine(0, 1, ScottPlot.Colors.Black, LinePattern.Solid);
            stereoLine.Y = 0;
            stereoLine.IsVisible = false;

            WaveformsPlot.Plot.HideLegend();
            WaveformsPlot.Refresh();

            FadeComboBox.SelectedIndex = 0;
        }

        private void WaveformsPlot_MouseMove(object sender, MouseEventArgs e)
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
            {
                mousePositionX = limits.XRange.Min;
            }

            if (mouseDown && Cursor != Cursors.Hand)
            {
                waveformSpan.X2 = mousePositionX;
            }

            PositionLabel.Text = "Position (sec): " + mousePositionX.ToString("0.000");

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
            var lineUnderMouse = GetLineUnderMouse(e.X, e.Y);

            if (e.Button == MouseButtons.Left)
            {
                if (lineUnderMouse is VerticalLine vl)
                {
                    // Klick auf Marker: alle anderen Marker zurück auf Rot, diesen auf Grün
                    foreach (var ml in markerLines)
                    {
                        if (ml.IsVisible)
                        {
                            ml.Color = ReferenceEquals(ml, vl) ? markerColorSelected : markerColorNormal;
                        }
                    }
                    PlottableBeingDragged = vl;
                    RemoveMarkerButton.Enabled = true;
                    WaveformsPlot.Refresh();
                }
                else if (mouseDown == false)
                {
                    // Klick ins Leere: alle Marker zurück auf Rot, Span-Selektion starten
                    foreach (var ml in markerLines)
                    {
                        if (ml.IsVisible)
                        {
                            ml.Color = markerColorNormal;
                        }
                    }
                    RemoveMarkerButton.Enabled = false;

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

                    waveformSpan.IsVisible = true;
                    mouseDown = true;
                }
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
                ResetDestructiveEffectsEditorValues();
                InitialPlotSetup();
                e.Cancel = true;
                GC.Collect();
                Hide();
            }
            formMain.DestructiveEffectsButton.Enabled = true;
        }

        private void DEEBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string choice = e.Argument.ToString().Split('|').First();
            string argument = e.Argument.ToString().Split('|').Last();

            if (choice == "LOADAUDIO")
            {
                LoadAudioWaveform(argument);
            }
            else if (choice == "NORMALIZEAUDIO")
            {
                AudioNormalization();
            }
            else if (choice == "VOLUPAUDIO")
            {
                AudioVolumeUp();
            }
            else if (choice == "VOLDOWNAUDIO")
            {
                AudioVolumeDown();
            }
            else if (choice == "TRIMAUDIO")
            {
                AudioTrim();
            }
            else if (choice == "FADEAUDIO")
            {
                AudioFade();
            }
            else if (choice == "REVERTAUDIO")
            {
                AudioRevert();
            }
            else if (choice == "SAVEAUDIO")
            {
                AudioSave();
            }

            e.Result = choice;
        }

        private void DEEBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            // Fehlerbehandlung: falls in DoWork eine Exception geflogen ist
            if (e.Error != null)
            {
                MessageBox.Show("Error during audio operation: " + e.Error.Message);
                EnableGuiElements(true);
                return;
            }

            string result = e.Result?.ToString() ?? "";

            if (result == "LOADAUDIO")
            {
                Text = formMain.Text + ": Destructive Effects Editor -> " + currentFilePath;
                DisplayLoadedAudio();
                EnableGuiElements(true);
                return;
            }

            // Title vom "PLEASE WAIT..."-Status zurücksetzen
            Text = formMain.Text + ": Destructive Effects Editor -> " + currentFilePath;

            if (result != "REVERTAUDIO" && result != "SAVEAUDIO")
            {
                if (!Text.EndsWith('*'))
                {
                    Text += "*";
                }

                if (result == "TRIMAUDIO")
                {
                    RemoveMarkerButton.Enabled = false;

                    // build plot from scratch
                    InitialPlotSetup();
                    PlotWaveform();

                    double trimStart = Math.Min(trimSpanX1, trimSpanX2);
                    double trimEnd = Math.Max(trimSpanX1, trimSpanX2);
                    double removedDuration = trimEnd - trimStart;

                    // recover markers
                    if (trimMarkerSnapshot != null)
                    {
                        for (int i = 0; i < trimMarkerSnapshot.Length && i < markerLines.Length; i++)
                        {
                            if (trimMarkerSnapshot[i].X < 0) continue;

                            double newX = trimMarkerSnapshot[i].X;

                            // put markers to the left after trim
                            if (newX >= trimEnd)
                            {
                                newX -= removedDuration;
                            }

                            markerLines[i].X = newX;
                            markerLines[i].Text = trimMarkerSnapshot[i].Text;
                            markerLines[i].LabelOppositeAxis = true;
                            markerLines[i].IsVisible = true;
                            WaveformsPlot.Plot.MoveToFront(markerLines[i]);
                        }
                    }
                }

                UpdatePeakLabel();
                TableLayoutPanelChanges.Enabled = true;
                SaveButton.BackColor = System.Drawing.Color.LightGreen;
            }
            else if (result == "REVERTAUDIO")
            {
                if (Text.Contains(".wav*"))
                    Text = Text.Replace(".wav*", ".wav");
                TableLayoutPanelChanges.Enabled = false;
                SaveButton.BackColor = SystemColors.Control;
                InitialPlotSetup();
                PlotWaveform();

                // get original file markers back
                if (loadedCues != null)
                {
                    for (int i = 0; i < loadedCues.Length; i++)
                    {
                        markerLines[i].X = loadedCues[i].X;
                        markerLines[i].Text = loadedCues[i].Label;
                        markerLines[i].LabelOppositeAxis = true;
                        markerLines[i].IsVisible = true;
                        WaveformsPlot.Plot.MoveToFront(markerLines[i]);
                    }
                }

                UpdatePeakLabel();
            }
            else if (result == "SAVEAUDIO")
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

            WaveformsPlot.Refresh();
            EnableGuiElements(true);
        }
    }
}
