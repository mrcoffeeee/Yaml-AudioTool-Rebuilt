using System;
using System.IO;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Vortice.XAudio2;
using NAudio.Gui;
using NAudio.Wave;
using System.Drawing.Text;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class Form1 : Form
    {
        #region VariablesObjects

        //######################################################
        //###                                                ###
        //### Variables & Objects                            ###
        //###                                                ###
        //######################################################

        private int baseWindowWidth = 0;
        private int baseWindowHeight = 0;
        private int baseListViewWidth = 0;
        private int baseListViewHeight = 0;
        private int basefilterlistviewHeight = 0;

        private readonly AudioPlayback ap = new();

        private static DestructiveEffectsEditor formDestructiveEffectsEditor;

        #endregion VariablesObjects

        #region Form-Related

        //######################################################
        //###                                                ###
        //### Form related events                            ###
        //###                                                ###
        //######################################################

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetWindowsSize();
            PopulateComboboxes();
            roomlistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void PopulateComboboxes()
        {
            // Filter Combobox
            var f = typeof(FilterType);
            List<string> filterNames = new(f.GetFields().Select(x => x.Name));
            foreach (var item in filterNames)
            {
                if (item.Contains("value__"))
                    continue;
                filtercomboBox.Items.Add(item);
            }
            filtercomboBox.SelectedIndex = 0;

            // Reverbpresets Combobox
            var r = typeof(Vortice.XAudio2.Fx.Presets);
            List<string> reverbPresetNames = new(r.GetFields().Select(x => x.Name));
            foreach (var item in reverbPresetNames)
            {
                reverbpresetcomboBox.Items.Add(item);
            }
            reverbpresetcomboBox.SelectedIndex = 1;
        }

        private void ResetMainFormValues()
        {
            ap.StopPlayback();
            playbackTimer.Stop();
            removeButtonsEnabled(false, false);
            if (filelistView.Items.Count == 0)
                SaveYamlButton.Enabled = false;
            PitchenableButton.Enabled = false;
            PitchenableButton.Text = "Off";
            PitchenableButton.BackColor = Color.Salmon;
            timeLabel.Text = "00:00";
            EnumtextBox.Text = "";
            selectedsoundLabel.Text = "Selection: NONE";
            ChangeFilelabel.Text = "Filepath:";
        }

        private void SetWindowsSize()
        {
            int value = Screen.PrimaryScreen.Bounds.Width;

            if (value < 1900)
            {
                this.Size = new Size(980, 599);
            }
            else if (value >= 1900)
            {
                this.Size = new Size(1600, 900);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            ap.timerCount += playbackTimer.Interval;
            var timeSpan = TimeSpan.FromMilliseconds(ap.timerCount);
            timeLabel.Text = timeSpan.ToString(@"mm\:ss");
            //ap.sourceVoice.GetEffectParameters(0);
            //  meterLabel.Text = ap.peakLevel.ToString();

            //float[] peakLevel = new float[2];
            //float[] rmsLevel = new float[2];

            //Vortice.XAudio2.Fx.VolumeMeterLevels volumeLevel = new(peakLevel, rmsLevel, 2);

            //ap.sourceVoice.GetEffectParameters(0, volumeLevel, sizeof(volumeLevel));

            if (LoopButton.BackColor == Color.Salmon)
            {
                ap.sourceVoice.ExitLoop(0);
            }

            if (ap.sourceVoice.State.BuffersQueued == 0)
            {
                ap.StopPlayback();
                ap.waveFileReader.Close();
                playbackTimer.Stop();
            }
        }

        private static string GetFilenameFromPath(string filePath)
        {
            string fileName = "";
            char[] tempArray = filePath.ToCharArray();
            Array.Reverse(tempArray);
            foreach (char c in tempArray)
            {
                if (c.ToString() == "/")
                {
                    break;
                }
                fileName += c.ToString();
            }
            tempArray = fileName.ToCharArray();
            Array.Reverse(tempArray);
            return new string(tempArray);
        }

        private void ExitApplication()
        {
            try
            {
                ap.StopPlayback();
                File.Delete("Temp.wav");
            }
            catch
            {

            }
            Application.Exit();
        }

        private static bool GetOpenForm(string formName)
        {
            FormCollection formCollection = Application.OpenForms;

            foreach (Form frm in formCollection)
            {
                if (frm.Name == formName)
                {
                    return true;
                }
            }
            return false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExitApplication();
        }

        #endregion Form-Related

        #region MenuestripSection

        //######################################################
        //###                                                ###
        //### Elements for MenueStrip section                ###
        //###                                                ###
        //######################################################

        private void SettingstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialog formSettings = new();
            if (filelistView.Items.Count > 0)
            {
                formSettings.audiofoldersetButton.Enabled = false;
            }
            else
            {
                formSettings.audiofoldersetButton.Enabled = true;
            }
            formSettings.ShowDialog();
        }

        private void AbouttoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AboutDialog formAbout = new();
            formAbout.ShowDialog();
        }

        #endregion MenuestripSection

        #region ListviewSection

        //######################################################
        //###                                                ###
        //### Elements for "ListView" section                ###
        //###                                                ###
        //######################################################

        private void filelistView_SelectedIndexChanged(object sender, EventArgs e)
        {
            formDestructiveEffectsEditor = (DestructiveEffectsEditor)Application.OpenForms["DestructiveEffectseditor"];

            if (filelistView.SelectedItems.Count == 0)
            {
                ResetMainFormValues();
                removeButtonsEnabled(false, true);

                if (GetOpenForm("DestructiveEffectsEditor"))
                {
                    formDestructiveEffectsEditor.ResetDestructiveEffectsEditorValues();
                }
            }

            if (filelistView.SelectedItems.Count > 0)
            {
                EnumtextBox.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(titleHeader)].Text;
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
                selectedsoundLabel.Text = "Filename: " + GetFilenameFromPath(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filenameHeader)].Text);
                double volumeValue = Convert.ToDouble(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(volumeHeader)].Text) * 100;
                VolumetrackBar.Value = Convert.ToInt32(volumeValue);
                VolumevaluenumericUpDown.Value = Convert.ToInt32(volumeValue);
                PrioritytrackBar.Value = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(priorityHeader)].Text);
                priorityvalueLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(priorityHeader)].Text;
                if (filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(loopHeader)].Text == "true")
                    LoopButton.BackColor = Color.LightGreen;
                else if (filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(loopHeader)].Text == "false")
                    LoopButton.BackColor = Color.Salmon;
                MinDistancenumericUpDown.Value = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(mindistanceHeader)].Text);
                MaxDistancenumericUpDown.Value = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(maxdistanceHeader)].Text);
                double dopplerValue = Convert.ToDouble(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(dopplerHeader)].Text);
                DopplertrackBar.Value = Convert.ToInt32(dopplerValue * 100);
                dopplervalueLabel.Text = Convert.ToString(dopplerValue);
                LocalizecheckBox.Checked = Convert.ToBoolean(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(localizeHeader)].Text);
                StreamcheckBox.Checked = Convert.ToBoolean(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(streamHeader)].Text);
                TypecomboBox.SelectedIndex = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(typeHeader)].Text);
                FalloffcomboBox.SelectedIndex = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(falloffHeader)].Text);
                StackcomboBox.SelectedIndex = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(stackHeader)].Text);
                ChangeFilelabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text;
                PitchenableButton.Enabled = true;
                removeButtonsEnabled(true, true);
                if (filelistView.SelectedItems.Count == 1)
                {
                    if (GetOpenForm("DestructiveEffectsEditor") && formDestructiveEffectsEditor.Visible == true)
                    {
                        formDestructiveEffectsEditor.Text = "Destructive Effects Editor -> " + filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filenameHeader)].Text + ".wav";
                        formDestructiveEffectsEditor.LoadAudioWaveform(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text);
                    }
                }

            }
        }

        #endregion ListviewSection

        #region PlaybackSection

        //######################################################
        //###                                                ###
        //### Elements for "Playback" section                ###
        //###                                                ###
        //######################################################

        private void BackButton_Click(object sender, EventArgs e)
        {
            playbackTimer.Stop();
            ap.StopPlayback();
            filelistView.Focus();
            if (filelistView.Items.Count > 1 &&
                filelistView.SelectedItems.Count == 1 &&
                filelistView.Items.IndexOf(filelistView.SelectedItems[0]) > 0)
            {
                int a = filelistView.Items.IndexOf(filelistView.SelectedItems[0]);
                filelistView.Items[a].Selected = false;
                filelistView.Items[a - 1].Selected = true;
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
                removeButtonsEnabled(true, true);
            }
            else if (filelistView.SelectedItems.Count > 0)
            {
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count > 0 &&
                filelistView.SelectedItems[0].SubItems[3] != null)
            {
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
            }
            ap.StopPlayback();
            ap.waveFileReader.Close();
            playbackTimer.Stop();
        }

        private void LoopButton_Click(object sender, EventArgs e)
        {
            if (LoopButton.BackColor == Color.Salmon)
            {
                LoopButton.BackColor = Color.LightGreen;

                if (filelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                    {
                        filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(loopHeader)].Text = "true";
                    }
                }

            }
            else if (LoopButton.BackColor == Color.LightGreen)
            {
                LoopButton.BackColor = Color.Salmon;

                if (filelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                    {
                        filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(loopHeader)].Text = "false";
                    }
                }
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count == 1)
            {
                filelistView.Focus();
                if (filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text.StartsWith("\\"))
                {
                    string filepathTemp;
                    SettingsDialog sd = new();
                    filepathTemp = sd.audiofolderLabel.Text +
                        filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text +
                        filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filenameHeader)].Text +
                        ".wav";
                    ap.GetSoundFromList(filepathTemp);
                }
                else
                {
                    ap.GetSoundFromList(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text);
                }

                try
                {
                    ap.StartPlayback();

                    if (ap.xaudio2 != null && ap.playbackPause == false)
                    {
                        playbackTimer.Start();
                    }
                    else if (ap.xaudio2 != null && ap.playbackPause == true)
                    {
                        playbackTimer.Stop();
                    }
                }

                catch (Exception)
                {
                    MessageBox.Show("Please check your selected file.", "Playback impossible! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            playbackTimer.Stop();
            ap.StopPlayback();
            filelistView.Focus();
            if (filelistView.Items.Count > 1 &&
                filelistView.SelectedItems.Count == 1 &&
                filelistView.Items.IndexOf(filelistView.SelectedItems[0]) < filelistView.Items.Count - 1)
            {
                int a = filelistView.Items.IndexOf(filelistView.SelectedItems[0]);
                filelistView.Items[a].Selected = false;
                filelistView.Items[a + 1].Selected = true;
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
                removeButtonsEnabled(true, true);
            }
            else if (filelistView.SelectedItems.Count > 0)
            {
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
            }
        }

        #endregion PlaybackSection

        #region YAMLEditorSection

        //######################################################
        //###                                                ###
        //### Elements for "YAML Editor" section             ###
        //###                                                ###
        //######################################################

        private void addfileButton_Click(object sender, EventArgs e)
        {
            ap.StopPlayback();
            playbackTimer.Stop();
            ap.OpenFile(false);
            if (filelistView.Items.Count > 0 &&
                SaveYamlButton.Enabled == false)
            {
                SaveYamlButton.Enabled = true;
                filelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                filelistView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
                filelistView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                removeButtonsEnabled(false, true);
            }
        }

        public void removeButtonsEnabled(bool value1, bool value2)
        {
            removeButton.Enabled = value1;
            removeallButton.Enabled = value2;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            foreach (var item in filelistView.SelectedItems)
            {
                filelistView.SelectedItems[0].Remove();
            }
            ResetMainFormValues();
            formDestructiveEffectsEditor?.ResetDestructiveEffectsEditorValues();
        }

        private void removeallButton_Click(object sender, EventArgs e)
        {
            filelistView.Items.Clear();
            roomlistView.Items.Clear();
            ResetMainFormValues();
            formDestructiveEffectsEditor?.ResetDestructiveEffectsEditorValues();
        }

        private void openyamlButton_Click(object sender, EventArgs e)
        {
            SettingsDialog sd = new();
            if (sd.audiofolderLabel.Text == "NONE")
            {
                MessageBox.Show("No Audio folder in \"Settings\" selected.");
                sd.ShowDialog();
                return;
            }
            YamlExportImport.ImportYAML();
            if (roomlistView.Items.Count > 0)
            {
                roommapButton.Enabled = true;
                roomunmapButton.Enabled = true;
                filterremoveButton.Enabled = true;
            }
            if (filelistView.Items.Count > 0 &&
                SaveYamlButton.Enabled == false)
            {
                SaveYamlButton.Enabled = true;
                removeButtonsEnabled(false, true);
            }
            filelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void SaveYamlButton_Click(object sender, EventArgs e)
        {
            YamlExportImport.ExportYAML();
        }

        #endregion YAMLEditorSection

        #region Property-Playback

        //######################################################
        //###                                                ###
        //### Elements for property editor "Playback"        ###
        //###                                                ###
        //######################################################

        private void VolumetrackBar_Scroll_1(object sender, EventArgs e)
        {
            double value = ap.SetVolume(VolumetrackBar.Value, filelistView.SelectedItems.Count);
            VolumevaluenumericUpDown.Value = Convert.ToInt32(value * 100);

            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(volumeHeader)].Text = Convert.ToString(value);
                }
            }
        }

        private void VolumevaluenumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            VolumetrackBar.Value = Convert.ToInt32(VolumevaluenumericUpDown.Value);
            double value = ap.SetVolume(VolumetrackBar.Value, filelistView.SelectedItems.Count);

            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(volumeHeader)].Text = Convert.ToString(value);
                }
            }
        }

        private void PrioritytrackBar_Scroll(object sender, EventArgs e)
        {
            priorityvalueLabel.Text = Convert.ToString(PrioritytrackBar.Value);

            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(priorityHeader)].Text = Convert.ToString(PrioritytrackBar.Value);
                }
            }
        }

        private void DopplertrackBar_Scroll(object sender, EventArgs e)
        {
            dopplervalueLabel.Text = Convert.ToString(DopplertrackBar.Value / 100.0);

            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(dopplerHeader)].Text = Convert.ToString(DopplertrackBar.Value / 100.0);
                }
            }
        }

        private void LocalizecheckBox_CheckedChanged_1(object sender, EventArgs e)
        {
            if (LocalizecheckBox.Text == "Disabled")
            {
                LocalizecheckBox.Checked = true;
                LocalizecheckBox.Text = "Enabled";

                if (filelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                    {
                        filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(localizeHeader)].Text = "true";
                    }
                }

            }
            else if (LocalizecheckBox.Text == "Enabled")
            {
                LocalizecheckBox.Checked = false;
                LocalizecheckBox.Text = "Disabled";

                if (filelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                    {
                        filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(localizeHeader)].Text = "false";
                    }
                }
            }
        }

        private void StreamcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StreamcheckBox.Text == "Disabled")
            {
                StreamcheckBox.Checked = true;
                StreamcheckBox.Text = "Enabled";

                if (filelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                    {
                        filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(streamHeader)].Text = "true";
                    }
                }

            }
            else if (StreamcheckBox.Text == "Enabled")
            {
                StreamcheckBox.Checked = false;
                StreamcheckBox.Text = "Disabled";

                if (filelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                    {
                        filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(streamHeader)].Text = "false";
                    }
                }
            }
        }

        private void TypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(typeHeader)].Text = Convert.ToString(TypecomboBox.SelectedIndex);
                }
            }
        }

        private void FalloffcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(falloffHeader)].Text = Convert.ToString(FalloffcomboBox.SelectedIndex);
                }
            }
        }

        private void MaxDistancenumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (MaxDistancenumericUpDown.Value <= MinDistancenumericUpDown.Value)
            {
                MaxDistancenumericUpDown.Value = MinDistancenumericUpDown.Value;
            }

            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(maxdistanceHeader)].Text = MaxDistancenumericUpDown.Value.ToString();
                }
            }
        }

        private void MinDistancenumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (MinDistancenumericUpDown.Value >= MaxDistancenumericUpDown.Value)
            {
                MinDistancenumericUpDown.Value = MaxDistancenumericUpDown.Value;
            }

            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(mindistanceHeader)].Text = MinDistancenumericUpDown.Value.ToString();
                }
            }
        }

        private void StackcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(stackHeader)].Text = Convert.ToString(StackcomboBox.SelectedIndex);
                }
            }
        }


        #endregion Property-Playback

        #region Property-Organize

        //######################################################
        //###                                                ###
        //### Elements for property editor "Organize"        ###
        //###                                                ###
        //######################################################

        private void EnumButton_Click(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count == 1)
            {
                filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(titleHeader)].Text = EnumtextBox.Text;
                filelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void ChangeFilebutton_Click(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count == 1)
            {
                ap.StopPlayback();
                playbackTimer.Stop();
                ap.OpenFile(true);
                if (filelistView.Items.Count > 0 &&
                    SaveYamlButton.Enabled == false)
                {
                    SaveYamlButton.Enabled = true;
                    filelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    filelistView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
                    filelistView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            if (FindButton.Text == "Find Title" &&
                FindtextBox.Text != "" &&
                filelistView.Items.Count > 0)
            {
                foreach (ListViewItem item in filelistView.Items)
                {
                    if (item.SubItems[filelistView.Columns.IndexOf(titleHeader)].Text.Contains(FindtextBox.Text) ||
                        item.SubItems[filelistView.Columns.IndexOf(filenameHeader)].Text.Contains(FindtextBox.Text))
                    {
                        item.BackColor = Color.Salmon;
                    }
                }
                FindButton.Text = "Reset";
            }

            else if (FindButton.Text == "Reset")
            {
                foreach (ListViewItem item in filelistView.Items)
                {
                    item.BackColor = SystemColors.Window;
                }
                FindButton.Text = "Find Title";
                FindtextBox.Text = "";
            }
        }

        private void SortcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filelistView.Items.Count > 1)
            {
                ListViewColumnSorter lvwColumnSorter = new();
                filelistView.ListViewItemSorter = lvwColumnSorter;

                int[] Columns = new int[5]
                {
                    filelistView.Columns.IndexOf(titleHeader),
                    filelistView.Columns.IndexOf(filenameHeader),
                    filelistView.Columns.IndexOf(filepathHeader),
                    filelistView.Columns.IndexOf(roommapHeader),
                    filelistView.Columns.IndexOf(typeHeader)
                };

                lvwColumnSorter.SortColumn = Columns[SortcomboBox.SelectedIndex];
                lvwColumnSorter.Order = SortOrder.Ascending;

                filelistView.Sort();

            }
        }

        #endregion Property-Organize

        #region Property-RoomCreation

        //######################################################
        //###                                                ###
        //### Elements for property editor "Room Creation"   ###
        //###                                                ###
        //######################################################

        private void filtercomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (roomlistView.SelectedItems.Count == 1)
            {
                RoomCreationEffects.UpdateFilterSettings(filelistView.SelectedItems.Count, ap.sourceVoice);
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filtertypeHeader)].Text = filtercomboBox.SelectedIndex.ToString();
            }
        }

        private void frequencyPot_ValueChanged(object sender, EventArgs e)
        {
            RoomCreationEffects.UpdateFilterSettings(filelistView.SelectedItems.Count, ap.sourceVoice);
            float value = Convert.ToSingle(Math.Round(filterfrequencyPot.Value, 1));
            filterfrequencyvalueLabel.Text = IXAudio2.RadiansToCutoffFrequency(value, 48000f).ToString("0.0") + " Hz";


            if (roomlistView.SelectedItems.Count == 1)
            {
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filterfrequencyHeader)].Text = value.ToString("0.0");
            }
        }

        private void oneoverqPot_ValueChanged(object sender, EventArgs e)
        {
            RoomCreationEffects.UpdateFilterSettings(filelistView.SelectedItems.Count, ap.sourceVoice);
            double value = Math.Round(filteroneoverqPot.Value, 1);
            filteroneoverqvalueLabel.Text = value.ToString("0.0");

            if (roomlistView.SelectedItems.Count == 1)
            {
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filteroneoverqHeader)].Text = value.ToString("0.0");
            }
        }

        private void filterlistView_Click(object sender, EventArgs e)
        {
            filternametextBox.Text = roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filternameHeader)].Text;
            filtercomboBox.Text = filtercomboBox.Items[Convert.ToInt32(roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filtertypeHeader)].Text)].ToString();
            filterfrequencyPot.Value = Math.Round(Convert.ToDouble(roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filterfrequencyHeader)].Text), 1);
            filterfrequencyvalueLabel.Text = IXAudio2.RadiansToCutoffFrequency(Convert.ToSingle(roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filterfrequencyHeader)].Text), 48000f).ToString("0.0") + " Hz";
            filteroneoverqPot.Value = Convert.ToDouble(roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filteroneoverqHeader)].Text);
            filteroneoverqvalueLabel.Text = roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filteroneoverqHeader)].Text;
            reverbpresetcomboBox.SelectedIndex = Convert.ToInt32(roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbidHeader)].Text);
            reverbwetdryPot.Value = Convert.ToDouble(roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbwetdryHeader)].Text);
            reverbwetdryvalueLabel.Text = roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbwetdryHeader)].Text + " %";
        }

        private void roomstoreButton_Click(object sender, EventArgs e)
        {
            if (checkRoomDuplicates() == true)
            {
                MessageBox.Show("Room creation impossible!\n\n" +
                    "Please use a unique room/filter name.");
            }
            else if (checkRoomDuplicates() == false)
            {
                var actualReverbParameter = Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(RoomCreationEffects.ReverbPresets[reverbpresetcomboBox.SelectedIndex], false);
                ListViewItem roomitem = new(roomnametextBox.Text);
                //Filter Items
                roomitem.SubItems.Add(filternametextBox.Text);
                roomitem.SubItems.Add(filtercomboBox.SelectedIndex.ToString());
                roomitem.SubItems.Add(Math.Round(filterfrequencyPot.Value, 1).ToString("0.0"));
                roomitem.SubItems.Add(Math.Round(filteroneoverqPot.Value, 1).ToString("0.0"));
                //reverb Items
                roomitem.SubItems.Add(reverbpresetcomboBox.SelectedItem.ToString());
                roomitem.SubItems.Add(reverbpresetcomboBox.SelectedIndex.ToString());
                roomitem.SubItems.Add(reverbwetdryPot.Value.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.ReflectionsDelay.ToString("0"));
                roomitem.SubItems.Add(actualReverbParameter.RoomFilterFreq.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.ReverbDelay.ToString("0"));
                roomitem.SubItems.Add(actualReverbParameter.RoomFilterMain.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.RoomFilterHF.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.ReflectionsGain.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.ReverbGain.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.DecayTime.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.Density.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.RoomSize.ToString("0.0"));
                roomlistView.Items.Add(roomitem);

                if (roomlistView.Items.Count > 0)
                {
                    roommapButton.Enabled = true;
                    roomunmapButton.Enabled = true;
                    filterremoveButton.Enabled = true;
                }
            }
        }

        private void roomremoveButton_Click(object sender, EventArgs e)
        {
            if (roomlistView.SelectedItems.Count == 1)
            {
                int a = 0;
                foreach (var item in filelistView.Items)
                {
                    if (filelistView.Items[a].SubItems[filelistView.Columns.IndexOf(roommapHeader)].Text == roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(roomnameHeader)].Text)
                    {
                        filelistView.Items[a].SubItems[filelistView.Columns.IndexOf(roommapHeader)].Text = "";
                        filelistView.Items[a].SubItems[filelistView.Columns.IndexOf(roomidHeader)].Text = "";
                    }
                    a++;
                }
                roomlistView.SelectedItems[0].Remove();

                if (roomlistView.Items.Count == 0)
                {
                    roommapButton.Enabled = false;
                    roomunmapButton.Enabled = false;
                    filterremoveButton.Enabled = false;
                }
            }
        }

        private bool checkRoomDuplicates()
        {
            int a = 0;
            foreach (var item in roomlistView.Items)
            {
                if (roomlistView.Items[a].SubItems[roomlistView.Columns.IndexOf(roomnameHeader)].Text == roomnametextBox.Text ||
                    roomlistView.Items[a].SubItems[roomlistView.Columns.IndexOf(filternameHeader)].Text == filternametextBox.Text)
                    return true;
                a++;
            }
            return false;
        }

        private void reverbwetdryPot_ValueChanged(object sender, EventArgs e)
        {
            RoomCreationEffects.UpdateReverbSettings(filelistView.SelectedItems.Count, ap.sourceVoice);
            reverbwetdryvalueLabel.Text = Math.Round(reverbwetdryPot.Value, 1).ToString("0.0") + " %";

            if (roomlistView.SelectedItems.Count == 1)
            {
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbwetdryHeader)].Text = Math.Round(reverbwetdryPot.Value, 1).ToString("0.0");
            }
        }

        private void roommapButton_Click(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count > 0 &&
                roomlistView.SelectedItems.Count == 1)
            {
                int a = 0;
                foreach (var item in filelistView.SelectedItems)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(roommapHeader)].Text = roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(roomnameHeader)].Text;
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(roomidHeader)].Text = roomlistView.SelectedItems[0].Index.ToString();
                    a++;
                }
            }
        }

        private void roomunmapButton_Click(object sender, EventArgs e)
        {
            if (roomlistView.SelectedItems.Count == 1)
            {
                int a = 0;
                foreach (var item in filelistView.Items)
                {
                    if (filelistView.Items[a].SubItems[filelistView.Columns.IndexOf(roommapHeader)].Text == roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(roomnameHeader)].Text)
                    {
                        filelistView.Items[a].SubItems[filelistView.Columns.IndexOf(roommapHeader)].Text = "";
                        filelistView.Items[a].SubItems[filelistView.Columns.IndexOf(roomidHeader)].Text = "";
                    }
                    a++;
                }
            }
        }
        #endregion Property-RoomCreation

        #region Property-Effects

        //######################################################
        //###                                                ###
        //### Elements for property editor "Effects"         ###
        //###                                                ###
        //######################################################

        private void PitchshifterButton_Click(object sender, EventArgs e)
        {
            Effect_PitchShifter pitchshifterForm = new()
            {
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };
            pitchshifterForm.Show();
            PitchshifterButton.Enabled = false;
        }

        private void PitchenableButton_Click(object sender, EventArgs e)
        {
            if (PitchenableButton.Text == "Off")
            {
                PitchenableButton.Text = "On";
                PitchenableButton.BackColor = Color.LightGreen;
            }

            else if (PitchenableButton.Text == "On")
            {
                PitchenableButton.Text = "Off";
                PitchenableButton.BackColor = Color.Salmon;
            }
        }

        private void RoomenableButton_Click_1(object sender, EventArgs e)
        {
            if (RoomenableButton.Text == "Off")
            {
                RoomenableButton.Text = "On";
                RoomenableButton.BackColor = Color.LightGreen;
            }

            else if (RoomenableButton.Text == "On")
            {
                RoomenableButton.Text = "Off";
                RoomenableButton.BackColor = Color.Salmon;
            }
        }

        private void reverbpresetcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var actualReverbParameter = Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(RoomCreationEffects.ReverbPresets[reverbpresetcomboBox.SelectedIndex], false);

            RoomCreationEffects.UpdateReverbSettings(filelistView.SelectedItems.Count, ap.sourceVoice);
            reverbwetdryPot.Value = Math.Round(actualReverbParameter.WetDryMix, 1);
            reverbwetdryvalueLabel.Text = Math.Round(actualReverbParameter.WetDryMix, 1).ToString("0.0") + " %";

            if (roomlistView.SelectedItems.Count == 1)
            {
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbpresetHeader)].Text = reverbpresetcomboBox.SelectedItem.ToString();
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbwetdryHeader)].Text = actualReverbParameter.WetDryMix.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbreflectionsdelayHeader)].Text = actualReverbParameter.ReflectionsDelay.ToString("0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbroomfrequencyHeader)].Text = actualReverbParameter.RoomFilterFreq.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbdelayHeader)].Text = actualReverbParameter.ReverbDelay.ToString("0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbroomfiltermainHeader)].Text = actualReverbParameter.RoomFilterMain.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbroomfilterhfHeader)].Text = actualReverbParameter.RoomFilterHF.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbreflectionsgainHeader)].Text = actualReverbParameter.ReflectionsGain.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbgainHeader)].Text = actualReverbParameter.ReverbGain.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbdecaytimeHeader)].Text = actualReverbParameter.DecayTime.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbdensityHeader)].Text = actualReverbParameter.Density.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbroomsizeHeader)].Text = actualReverbParameter.RoomSize.ToString("0.0");
            }
        }

        private void DestructiveEffectsButton_Click(object sender, EventArgs e)
        {
            if (formDestructiveEffectsEditor != null)
            {
                formDestructiveEffectsEditor.Visible = true;
                if (filelistView.SelectedItems.Count == 1)
                {
                    formDestructiveEffectsEditor.Text = "Destructive Effects Editor -> " + filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filenameHeader)].Text + ".wav";
                    formDestructiveEffectsEditor.LoadAudioWaveform(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text);
                }
            }
            else
            {
                DestructiveEffectsEditor editorForm = new()
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    TopMost = true
                };
                editorForm.Show();
            }


            DestructiveEffectsButton.Enabled = false;
        }


        #endregion Property-Effects


    }

    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;

        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;

        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor. Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface. It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
