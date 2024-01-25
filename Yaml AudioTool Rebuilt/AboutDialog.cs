using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class AboutDialog : Form
    {
        private readonly string spacer = "  ";
        private readonly string version = "Version: 1.3.0";
        private readonly string buildDate = "Build date: 240125";
        private readonly string netVersion = "NET Version: " + Environment.Version;
        private readonly string author = "Author: Johannes Wronka";
        private readonly string additionalInfo = "Alex Miyamotos Vitei Audio Tool Remake\n  Logo by Owen Davis";
        readonly Form1 formMain = (Form1)Application.OpenForms["Form1"];

        public AboutDialog()
        {
            InitializeComponent();
            Text = formMain.Text + ": About";
            TitleLabel.Text = spacer + formMain.Text;
            InfoLabel.Text =
                spacer + additionalInfo + "\n" +
                spacer + version + "\n" +
                spacer + buildDate + "\n" +
                spacer + netVersion + "\n" +
                spacer + author + "\n";
        }

        private void FirstLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer", "https://github.com/force-net/Crc32.NET/blob/develop/LICENSE");
        }

        private void SecondLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer", "https://github.com/naudio/NAudio/blob/master/license.txt");
        }

        private void ThirdLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer", "https://github.com/ScottPlot/ScottPlot/blob/main/LICENSE");
        }

        private void FourthLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer", "https://github.com/amerkoleci/Vortice.Windows/blob/main/LICENSE");
        }
    }
}
