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
    public partial class Effect_PitchShifter : Form
    {
        public Effect_PitchShifter()
        {
            InitializeComponent();
        }

        private void PitchPot_ValueChanged(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            double soundPitchFactor = PitchPot.Value / 100.00;
            PitchvalueLabel.Text = soundPitchFactor.ToString("0.00");

            if (f1.filelistView.SelectedItems.Count == 1)
            {
                f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.pitchHeader)].Text = soundPitchFactor.ToString("0.00");
            }
        }

        private void PitrandPot_ValueChanged(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            double soundPitchRand = PitrandPot.Value / 100.00;
            PitchrandvalueLabel.Text = soundPitchRand.ToString("0.00");

            if (f1.filelistView.SelectedItems.Count == 1)
            {
                f1.filelistView.SelectedItems[0].SubItems[f1.filelistView.Columns.IndexOf(f1.pitchrandHeader)].Text = soundPitchRand.ToString("0.00");
            }
        }
    }
}
