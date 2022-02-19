using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();

            if (!File.Exists("Settings.txt"))
            {
                audiofolderLabel.Text = "NONE";
            }
            else
            {
                TextReader tr = new StreamReader("Settings.txt");
                audiofolderLabel.Text = tr.ReadLine();
                tr.Close();
            }
        }

        private void audiofoldersetButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new ();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                audiofolderLabel.Text = folderBrowserDialog.SelectedPath;
            }

            TextWriter tw = new StreamWriter("Settings.txt");
            tw.WriteLine(audiofolderLabel.Text);
            tw.Close();
        }
    }
}
