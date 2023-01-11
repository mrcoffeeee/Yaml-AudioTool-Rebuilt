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

namespace Yaml_AudioTool_Rebuilt
{
    public partial class DestructiveEffectsEditor : Form
    {
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        public DestructiveEffectsEditor()
        {
            InitializeComponent();
            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                LoadAudioWaveform();
            }
        }

        public void LoadAudioWaveform()
        {
            this.Text = "Destructive Effects Editor -> " + formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text;
            customWaveViewer1.Enabled = true;
            customWaveViewer1.BackColor = Color.White;
            customWaveViewer1.WaveStream = new WaveFileReader(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.filepathHeader)].Text);
            customWaveViewer1.FitToScreen();
            WaveviewerhScrollBar.Minimum = customWaveViewer1.HorizontalScroll.Minimum;
            WaveviewerhScrollBar.Maximum = customWaveViewer1.HorizontalScroll.Maximum;
            WaveviewerhScrollBar.Maximum = 0;
        }

        public void ResetDestructiveEffectsEditorValues()
        {
            this.Text = "Destructive Effects Editor";
            customWaveViewer1.WaveStream = null;
            MonoChannelLabel.Visible = false;
            LeftChannelLabel.Visible = false;
            RightChannelLabel.Visible = false;
            customWaveViewer1.Enabled = false;
            customWaveViewer1.BackColor = SystemColors.Control;
            WaveviewerhScrollBar.Minimum = 0;
            WaveviewerhScrollBar.Maximum = 0;
        }

        private void WaveviewerhScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            customWaveViewer1.HorizontalScroll.Value = WaveviewerhScrollBar.Value;
            customWaveViewer1.Update();
        }

        private void NormalizeButton_Click(object sender, EventArgs e)
        {
            Effect_Normalize normalizeForm = new();
            normalizeForm.StartPosition = FormStartPosition.CenterScreen;
            normalizeForm.TopMost = true;
            normalizeForm.Show();
            NormalizeButton.Enabled = false;
        }

        private void DestructiveEffectsEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            formMain.DestructiveEffectsButton.Enabled = true;
        }
    }
}
