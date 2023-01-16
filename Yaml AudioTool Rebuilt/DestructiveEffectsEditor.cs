using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using ScottPlot;
using NAudio.Gui;
using Microsoft.VisualBasic.Devices;
using Vortice.XAudio2;

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
                LoadAudioWaveform();                
            }
        }

        public void LoadAudioWaveform()
        {
            this.Text = "Destructive Effects Editor -> " + formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text;
            PlotWaveform();
        }

        public void ResetDestructiveEffectsEditorValues()
        {
            this.Text = "Destructive Effects Editor";
            InitialPlotSetup();
        }

        private void NormalizeButton_Click(object sender, EventArgs e)
        {
            Effect_Normalize normalizeForm = new();
            normalizeForm.StartPosition = FormStartPosition.CenterScreen;
            normalizeForm.TopMost = true;
            normalizeForm.Show();
            NormalizeButton.Enabled = false;
        }

        static (double[] audioL, double[] audioR, int sampleRate, int channelCount) ReadWavefile(string filePath)
        {
            using (var audioFile = new WaveFileReader(filePath))
            {
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
        }

        public void PlotWaveform()
        {
            (double[] audioL, double[] audioR, int sampleRate, int channelCount) = ReadWavefile(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text);
            string channels = "Unsupported Channels #";
            if (channelCount == 1)
            {
                channels = "Mono";
                WaveformsPlot.Plot.AddSignal(audioL, sampleRate);
                WaveformsPlot.Plot.Title(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filenameHeader)].Text +
                    ".wav" +
                    " | Channels: " + channels);
                WaveformsPlot.Plot.SetAxisLimitsY(-1, 1);
                WaveformsPlot.Plot.AxisAutoX(0);
                var limits = WaveformsPlot.Plot.GetAxisLimits();
                WaveformsPlot.Plot.SetOuterViewLimits(0, limits.XMax, -1, 1);
            }

            else if(channelCount == 2)
            {
                channels = "Stereo";
                var ch1 = WaveformsPlot.Plot.AddSignal(audioL, sampleRate);
                ch1.OffsetY = 1;

                var ch2 = WaveformsPlot.Plot.AddSignal(audioR, sampleRate);
                ch2.OffsetY = -1;

                WaveformsPlot.Plot.Title(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filenameHeader)].Text +
                    ".wav" +
                    " | Channels: " + channels);
                WaveformsPlot.Plot.SetAxisLimitsY(-2, 2);
                WaveformsPlot.Plot.AxisAutoX(0);
                var limits = WaveformsPlot.Plot.GetAxisLimits();
                WaveformsPlot.Plot.SetOuterViewLimits(0, limits.XMax, -2, 2);
            }
            else
            {
                WaveformsPlot.Plot.Title(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filenameHeader)].Text +
                    ".wav" +
                    " | Channels: " + channels);
            }
            WaveformsPlot.Refresh();
        }

        private void InitialPlotSetup()
        {
            WaveformsPlot.Reset();
            WaveformsPlot.Plot.Title("Channels:");
            WaveformsPlot.Plot.XLabel("Time (seconds)");
            WaveformsPlot.Plot.YLabel("Audio level");
            WaveformsPlot.Plot.SetAxisLimitsX(0, 1);
            WaveformsPlot.Plot.SetAxisLimitsY(-2, 2);
            WaveformsPlot.Plot.SetOuterViewLimits(0, 1, -2, 2);
            WaveformsPlot.Configuration.Quality = ScottPlot.Control.QualityMode.High;
            WaveformsPlot.Configuration.LockVerticalAxis = true;
            WaveformsPlot.Refresh();
        }

        private void DestructiveEffectsEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            formMain.DestructiveEffectsButton.Enabled = true;
        }
    }
}
