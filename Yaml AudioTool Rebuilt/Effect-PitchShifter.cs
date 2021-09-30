using System;
using System.Windows.Forms;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class Effect_PitchShifter : Form
    {
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        public Effect_PitchShifter()
        {
            InitializeComponent();
            //Form1 formMain = (Form1)Application.OpenForms["Form1"];
            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                PitchPot.Value = Convert.ToDouble(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.pitchHeader)].Text) * 100;
                PitchvalueLabel.Text = formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.pitchHeader)].Text;
                PitrandPot.Value = Convert.ToDouble(formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.pitchrandHeader)].Text) * 100;
                PitchrandvalueLabel.Text = formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.pitchrandHeader)].Text;
            }
        }

        private void PitchPot_ValueChanged(object sender, EventArgs e)
        {
            double soundPitchFactor = PitchPot.Value / 100.00;
            PitchvalueLabel.Text = soundPitchFactor.ToString("0.00");

            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.pitchHeader)].Text = soundPitchFactor.ToString("0.00");
            }
        }

        private void PitrandPot_ValueChanged(object sender, EventArgs e)
        {
            double soundPitchRand = PitrandPot.Value / 100.00;
            PitchrandvalueLabel.Text = soundPitchRand.ToString("0.00");

            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.pitchrandHeader)].Text = soundPitchRand.ToString("0.00");
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            PitchvalueLabel.Text = "1.00";
            PitchPot.Value = 100;
            PitchrandvalueLabel.Text = "0.00";
            PitrandPot.Value = 0;
            if (formMain.filelistView.SelectedItems.Count == 1)
            {
                formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.pitchHeader)].Text = "1.00";
                formMain.filelistView.SelectedItems[0].SubItems[formMain.filelistView.Columns.IndexOf(formMain.pitchrandHeader)].Text = "0.00";
            }
        }

        private void Effect_PitchShifter_FormClosed(object sender, FormClosedEventArgs e)
        {
            formMain.PitchshifterButton.Enabled = true;
        }
    }
}
