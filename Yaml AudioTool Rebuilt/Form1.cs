using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vortice.XAudio2;

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

        private bool stopFlag = false;
        private string fileTime = "";
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
            SetWindowsSize();
            PopulateComboboxes();
            RoomListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            SetDoubleBuffering(MainVolumeMeter, true);
            SetDoubleBuffering(MainVolumeSlider, true);
            SetDoubleBuffering(FilelistView, true);
            SetDoubleBuffering(RoomListView, true);
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
                FilterComboBox.Items.Add(item);
            }
            FilterComboBox.SelectedIndex = 0;

            // Reverbpresets Combobox
            var r = typeof(Vortice.XAudio2.Fx.Presets);
            List<string> reverbPresetNames = new(r.GetFields().Select(x => x.Name));
            foreach (var item in reverbPresetNames)
            {
                ReverbpresetComboBox.Items.Add(item);
            }
            ReverbpresetComboBox.SelectedIndex = 1;
        }

        private void ResetMainFormValues()
        {
            ap.StopPlayback();
            playbackTimer.Stop();
            RemoveButtonsEnabled(false, false);
            if (FilelistView.Items.Count == 0)
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
                //this.Size = new Size(980, 599);
            }
            else if (value >= 1900)
            {
                this.Size = new Size(1600, 900);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            long samplePosition = (long)ap.sourceVoice.State.SamplesPlayed;
            int sampleRate = ap.sourceVoice.VoiceDetails.InputSampleRate;
            double totalplaybackSeconds = (double)samplePosition / sampleRate;
            int minutes = (int)(totalplaybackSeconds / 60);
            int seconds = (int)(totalplaybackSeconds % 60);
            timeLabel.Text = $"{minutes:D2}:{seconds:D2}";

            MainVolumeMeter.Amplitude = ap.device.AudioMeterInformation.MasterPeakValue;
            MainVolumeMeter.Refresh();

            if (LoopButton.BackColor == Color.Salmon)
            {
                ap.sourceVoice.ExitLoop(0);
            }

            if (ap.sourceVoice.State.BuffersQueued == 0)
            {
                if (timeLabel.Text == fileTime)
                {
                    stopFlag = true;
                }

                if (stopFlag == true && MainVolumeMeter.Amplitude < 0.00001)
                {
                    StopPlayback();
                }
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

        public static void SetDoubleBuffering(Control control, bool value)
        {
            System.Reflection.PropertyInfo controlProperty = typeof(Control)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            controlProperty.SetValue(control, value, null);
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

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsDialog formSettings = new();
            if (FilelistView.Items.Count > 0)
            {
                formSettings.audiofoldersetButton.Enabled = false;
            }
            else
            {
                formSettings.audiofoldersetButton.Enabled = true;
            }
            formSettings.ShowDialog();
        }

        private void AboutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AboutDialog formAbout = new();
            formAbout.ShowDialog();
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo processInfo = new("Yaml Audio Tool Rebuilt - Manual.pdf")
                {
                    UseShellExecute = true
                };
                Process.Start(processInfo);
            }
            catch (Exception)
            {
                MessageBox.Show("Error: Could not find the help manual.");
            }
        }

        #endregion MenuestripSection

        #region ListviewSection

        //######################################################
        //###                                                ###
        //### Elements for "ListView" section                ###
        //###                                                ###
        //######################################################

        private void FilelistView_SelectedIndexChanged(object sender, EventArgs e)
        {
            formDestructiveEffectsEditor = (DestructiveEffectsEditor)Application.OpenForms["DestructiveEffectseditor"];

            StopPlayback();
            if (FilelistView.SelectedItems.Count == 0)
            {
                ResetMainFormValues();
                RemoveButtonsEnabled(false, true);

                if (GetOpenForm("DestructiveEffectsEditor"))
                {
                    formDestructiveEffectsEditor.ResetDestructiveEffectsEditorValues();
                }
            }

            if (FilelistView.SelectedItems.Count > 0)
            {
                EnumtextBox.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(titleHeader)].Text;
                timeLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(durationHeader)].Text;
                selectedsoundLabel.Text = "Filename: " + GetFilenameFromPath(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filenameHeader)].Text);
                MainVolumeSlider.Volume = Convert.ToSingle(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(volumeHeader)].Text);
                PrioritytrackBar.Value = Convert.ToInt32(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(priorityHeader)].Text);
                priorityvalueLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(priorityHeader)].Text;
                if (FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(loopHeader)].Text == "true")
                    LoopButton.BackColor = Color.LightGreen;
                else if (FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(loopHeader)].Text == "false")
                    LoopButton.BackColor = Color.Salmon;
                MinDistancenumericUpDown.Value = Convert.ToInt32(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(mindistanceHeader)].Text);
                MaxDistancenumericUpDown.Value = Convert.ToInt32(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(maxdistanceHeader)].Text);
                double dopplerValue = Convert.ToDouble(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(dopplerHeader)].Text);
                DopplertrackBar.Value = Convert.ToInt32(dopplerValue * 100);
                dopplervalueLabel.Text = Convert.ToString(dopplerValue);
                LocalizecheckBox.Checked = Convert.ToBoolean(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(localizeHeader)].Text);
                StreamcheckBox.Checked = Convert.ToBoolean(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(streamHeader)].Text);
                TypecomboBox.SelectedItem = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(typeHeader)].Text;
                FalloffcomboBox.SelectedIndex = Convert.ToInt32(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(falloffHeader)].Text);
                StackcomboBox.SelectedIndex = Convert.ToInt32(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(stackHeader)].Text);
                ChangeFilelabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filepathHeader)].Text;
                //PitchShifterValues
                PitchPot.Value = Convert.ToDouble(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(pitchHeader)].Text) * 100;
                PitchvalueLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(pitchHeader)].Text;
                PitrandPot.Value = Convert.ToDouble(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(pitchrandHeader)].Text) * 100;
                PitchrandvalueLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(pitchrandHeader)].Text;
                PitchenableButton.Enabled = true;
                RemoveButtonsEnabled(true, true);
                if (FilelistView.SelectedItems.Count == 1)
                {
                    if (GetOpenForm("DestructiveEffectsEditor") && formDestructiveEffectsEditor.Visible == true)
                    {
                        formDestructiveEffectsEditor.Text = Text + ": Destructive Effects Editor -> " + FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filenameHeader)].Text + ".wav";
                        if (!formDestructiveEffectsEditor.DEEBackgroundWorker.IsBusy)
                        {
                            string bwString = "LOADAUDIO|" + FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filepathHeader)].Text;
                            formDestructiveEffectsEditor.DEEBackgroundWorker.RunWorkerAsync(bwString);
                        }                        
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
            StopPlayback();
            FilelistView.Focus();
            if (FilelistView.Items.Count > 1 &&
                FilelistView.SelectedItems.Count == 1 &&
                FilelistView.Items.IndexOf(FilelistView.SelectedItems[0]) > 0)
            {
                int a = FilelistView.Items.IndexOf(FilelistView.SelectedItems[0]);
                FilelistView.Items[a].Selected = false;
                FilelistView.Items[a - 1].Selected = true;
                timeLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(durationHeader)].Text;
                RemoveButtonsEnabled(true, true);
            }
            else if (FilelistView.SelectedItems.Count > 0)
            {
                timeLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(durationHeader)].Text;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (FilelistView.SelectedItems.Count > 0 &&
                FilelistView.SelectedItems[0].SubItems[3] != null)
            {
                timeLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(durationHeader)].Text;
            }
            StopPlayback();
        }

        private void LoopButton_Click(object sender, EventArgs e)
        {
            if (LoopButton.BackColor == Color.Salmon)
            {
                LoopButton.BackColor = Color.LightGreen;

                if (FilelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                    {
                        FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(loopHeader)].Text = "true";
                    }
                }

            }
            else if (LoopButton.BackColor == Color.LightGreen)
            {
                LoopButton.BackColor = Color.Salmon;

                if (FilelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                    {
                        FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(loopHeader)].Text = "false";
                    }
                }
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (FilelistView.SelectedItems.Count == 1)
            {
                stopFlag = false;
                fileTime = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(durationHeader)].Text;
                FilelistView.Focus();
                if (FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filepathHeader)].Text.StartsWith('\\'))
                {
                    string filepathTemp;
                    SettingsDialog sd = new();
                    filepathTemp = sd.audiofolderLabel.Text +
                        FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filepathHeader)].Text +
                        FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filenameHeader)].Text +
                        ".wav";
                    ap.GetSoundFromList(filepathTemp);
                }
                else
                {
                    ap.GetSoundFromList(FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filepathHeader)].Text);
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
                        MainVolumeMeter.Amplitude = 0;
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
            StopPlayback();
            FilelistView.Focus();
            if (FilelistView.Items.Count > 1 &&
                FilelistView.SelectedItems.Count == 1 &&
                FilelistView.Items.IndexOf(FilelistView.SelectedItems[0]) < FilelistView.Items.Count - 1)
            {
                int a = FilelistView.Items.IndexOf(FilelistView.SelectedItems[0]);
                FilelistView.Items[a].Selected = false;
                FilelistView.Items[a + 1].Selected = true;
                timeLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(durationHeader)].Text;
                RemoveButtonsEnabled(true, true);
            }
            else if (FilelistView.SelectedItems.Count > 0)
            {
                timeLabel.Text = FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(durationHeader)].Text;
            }
        }

        private void StopPlayback()
        {
            ap.StopPlayback();
            ap.waveFileReader?.Close();
            playbackTimer.Stop();
            MainVolumeMeter.Amplitude = 0;
        }

        #endregion PlaybackSection

        #region YAMLEditorSection

        //######################################################
        //###                                                ###
        //### Elements for "YAML Editor" section             ###
        //###                                                ###
        //######################################################

        private void AddfileButton_Click(object sender, EventArgs e)
        {
            ap.StopPlayback();
            playbackTimer.Stop();
            ap.OpenFile(false);
            if (FilelistView.Items.Count > 0 &&
                SaveYamlButton.Enabled == false)
            {
                SaveYamlButton.Enabled = true;
                FilelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                FilelistView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
                FilelistView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                RemoveButtonsEnabled(false, true);
            }
        }

        public void RemoveButtonsEnabled(bool value1, bool value2)
        {
            RemoveButton.Enabled = value1;
            RemoveallButton.Enabled = value2;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (var item in FilelistView.SelectedItems)
            {
                FilelistView.SelectedItems[0].Remove();
            }
            ResetMainFormValues();
            formDestructiveEffectsEditor?.ResetDestructiveEffectsEditorValues();
        }

        private void RemoveallButton_Click(object sender, EventArgs e)
        {
            FilelistView.Items.Clear();
            RoomListView.Items.Clear();
            ResetMainFormValues();
            formDestructiveEffectsEditor?.ResetDestructiveEffectsEditorValues();
        }

        private void OpenyamlButton_Click(object sender, EventArgs e)
        {
            SettingsDialog sd = new();
            if (sd.audiofolderLabel.Text == "NONE")
            {
                MessageBox.Show("No Audio folder in \"Settings\" selected.");
                sd.ShowDialog();
                return;
            }
            YamlExportImport.ImportYAML();
            if (RoomListView.Items.Count > 0)
            {
                RoommapButton.Enabled = true;
                RoomunmapButton.Enabled = true;
                RoomremoveButton.Enabled = true;
            }
            if (FilelistView.Items.Count > 0 &&
                SaveYamlButton.Enabled == false)
            {
                SaveYamlButton.Enabled = true;
                RemoveButtonsEnabled(false, true);
            }
            FilelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void SaveYamlButton_Click(object sender, EventArgs e)
        {
            YamlExportImport.ExportYAML();
        }

        #endregion YAMLEditorSection

        #region DestructiveEffectsSection

        //######################################################
        //###                                                ###
        //### Elements for "Destructive Effects" section     ###
        //###                                                ###
        //######################################################


        private void DestructiveEffectsButton_Click(object sender, EventArgs e)
        {
            if (formDestructiveEffectsEditor != null)
            {
                formDestructiveEffectsEditor.Visible = true;
                if (FilelistView.SelectedItems.Count == 1)
                {
                    formDestructiveEffectsEditor.Text = Text + ": Destructive Effects Editor -> " + FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filenameHeader)].Text + ".wav";
                    if (!formDestructiveEffectsEditor.DEEBackgroundWorker.IsBusy)
                    {
                        string bwString = "LOADAUDIO|" + FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(filepathHeader)].Text;
                        formDestructiveEffectsEditor.DEEBackgroundWorker.RunWorkerAsync(bwString);
                    }                    
                }
            }
            else
            {
                DestructiveEffectsEditor editorForm = new()
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                editorForm.Show();
            }

            DestructiveEffectsButton.Enabled = false;
        }

        #endregion DestructiveEffectsSection

        #region Property-Playback

        //######################################################
        //###                                                ###
        //### Elements for property editor "Playback"        ###
        //###                                                ###
        //######################################################

        private void MainVolumeSlider_VolumeChanged(object sender, EventArgs e)
        {
            double value = MainVolumeSlider.Volume;
            ap.SetVolume(value, FilelistView.SelectedItems.Count);

            if (FilelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(volumeHeader)].Text = Convert.ToString(value);
                }
            }
        }

        private void PrioritytrackBar_Scroll(object sender, EventArgs e)
        {
            priorityvalueLabel.Text = Convert.ToString(PrioritytrackBar.Value);

            if (FilelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(priorityHeader)].Text = Convert.ToString(PrioritytrackBar.Value);
                }
            }
        }

        private void DopplertrackBar_Scroll(object sender, EventArgs e)
        {
            dopplervalueLabel.Text = Convert.ToString(DopplertrackBar.Value / 100.0);

            if (FilelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(dopplerHeader)].Text = Convert.ToString(DopplertrackBar.Value / 100.0);
                }
            }
        }

        private void LocalizecheckBox_CheckedChanged_1(object sender, EventArgs e)
        {
            if (LocalizecheckBox.Text == "Disabled")
            {
                LocalizecheckBox.Checked = true;
                LocalizecheckBox.Text = "Enabled";

                if (FilelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                    {
                        FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(localizeHeader)].Text = "true";
                    }
                }

            }
            else if (LocalizecheckBox.Text == "Enabled")
            {
                LocalizecheckBox.Checked = false;
                LocalizecheckBox.Text = "Disabled";

                if (FilelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                    {
                        FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(localizeHeader)].Text = "false";
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

                if (FilelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                    {
                        FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(streamHeader)].Text = "true";
                    }
                }

            }
            else if (StreamcheckBox.Text == "Enabled")
            {
                StreamcheckBox.Checked = false;
                StreamcheckBox.Text = "Disabled";

                if (FilelistView.SelectedItems.Count > 0)
                {
                    for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                    {
                        FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(streamHeader)].Text = "false";
                    }
                }
            }
        }

        private void TypecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(typeHeader)].Text = Convert.ToString(TypecomboBox.SelectedItem);
                }
            }
        }

        private void FalloffcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(falloffHeader)].Text = Convert.ToString(FalloffcomboBox.SelectedIndex);
                }
            }
        }

        private void MaxDistancenumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (MaxDistancenumericUpDown.Value <= MinDistancenumericUpDown.Value)
            {
                MaxDistancenumericUpDown.Value = MinDistancenumericUpDown.Value;
            }

            if (FilelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(maxdistanceHeader)].Text = MaxDistancenumericUpDown.Value.ToString();
                }
            }
        }

        private void MinDistancenumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (MinDistancenumericUpDown.Value >= MaxDistancenumericUpDown.Value)
            {
                MinDistancenumericUpDown.Value = MaxDistancenumericUpDown.Value;
            }

            if (FilelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(mindistanceHeader)].Text = MinDistancenumericUpDown.Value.ToString();
                }
            }
        }

        private void StackcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < FilelistView.SelectedItems.Count; a++)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(stackHeader)].Text = Convert.ToString(StackcomboBox.SelectedIndex);
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
            if (FilelistView.SelectedItems.Count == 1)
            {
                FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(titleHeader)].Text = EnumtextBox.Text;
                FilelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void ChangeFilebutton_Click(object sender, EventArgs e)
        {
            if (FilelistView.SelectedItems.Count == 1)
            {
                ap.StopPlayback();
                playbackTimer.Stop();
                ap.OpenFile(true);
                if (FilelistView.Items.Count > 0 &&
                    SaveYamlButton.Enabled == false)
                {
                    SaveYamlButton.Enabled = true;
                    FilelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    FilelistView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
                    FilelistView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            if (FindButton.Text == "Find Title" &&
                FindtextBox.Text != "" &&
                FilelistView.Items.Count > 0)
            {
                foreach (ListViewItem item in FilelistView.Items)
                {
                    if (item.SubItems[FilelistView.Columns.IndexOf(titleHeader)].Text.Contains(FindtextBox.Text) ||
                        item.SubItems[FilelistView.Columns.IndexOf(filenameHeader)].Text.Contains(FindtextBox.Text))
                    {
                        item.BackColor = Color.Salmon;
                    }
                }
                FindButton.Text = "Reset";
            }

            else if (FindButton.Text == "Reset")
            {
                foreach (ListViewItem item in FilelistView.Items)
                {
                    item.BackColor = SystemColors.Window;
                }
                FindButton.Text = "Find Title";
                FindtextBox.Text = "";
            }
        }

        private void SortcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilelistView.Items.Count > 1)
            {
                ListViewColumnSorter lvwColumnSorter = new();
                FilelistView.ListViewItemSorter = lvwColumnSorter;

                int[] Columns =
                [
                    FilelistView.Columns.IndexOf(titleHeader),
                    FilelistView.Columns.IndexOf(filenameHeader),
                    FilelistView.Columns.IndexOf(filepathHeader),
                    FilelistView.Columns.IndexOf(roommapHeader),
                    FilelistView.Columns.IndexOf(typeHeader)
                ];

                lvwColumnSorter.SortColumn = Columns[SortcomboBox.SelectedIndex];
                lvwColumnSorter.Order = SortOrder.Ascending;

                FilelistView.Sort();

            }
        }

        #endregion Property-Organize

        #region Property-RoomCreation

        //######################################################
        //###                                                ###
        //### Elements for property editor "Room Creation"   ###
        //###                                                ###
        //######################################################

        private void FiltercomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RoomListView.SelectedItems.Count == 1)
            {
                RoomCreationEffects.UpdateFilterSettings(RoomListView.SelectedItems.Count, ap.sourceVoice);
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filtertypeHeader)].Text = FilterComboBox.SelectedIndex.ToString();
            }
        }

        private void FrequencyPot_ValueChanged(object sender, EventArgs e)
        {
            RoomCreationEffects.UpdateFilterSettings(RoomListView.SelectedItems.Count, ap.sourceVoice);
            float value = Convert.ToSingle(Math.Round(FilterfrequencyPot.Value, 1));
            FilterfrequencyvalueLabel.Text = IXAudio2.RadiansToCutoffFrequency(value, 48000f).ToString("0.0") + " Hz";


            if (RoomListView.SelectedItems.Count == 1)
            {
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filterfrequencyHeader)].Text = value.ToString("0.0");
            }
        }

        private void OneoverqPot_ValueChanged(object sender, EventArgs e)
        {
            RoomCreationEffects.UpdateFilterSettings(RoomListView.SelectedItems.Count, ap.sourceVoice);
            double value = Math.Round(FilteroneoverqPot.Value, 1);
            FilteroneoverqvalueLabel.Text = value.ToString("0.0");

            if (RoomListView.SelectedItems.Count == 1)
            {
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filteroneoverqHeader)].Text = value.ToString("0.0");
            }
        }

        private void RoomListView_Click(object sender, EventArgs e)
        {
            FilternameTextBox.Text = RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filternameHeader)].Text;
            FilterComboBox.Text = FilterComboBox.Items[Convert.ToInt32(RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filtertypeHeader)].Text)].ToString();
            FilterfrequencyPot.Value = Math.Round(Convert.ToDouble(RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filterfrequencyHeader)].Text), 1);
            FilterfrequencyvalueLabel.Text = IXAudio2.RadiansToCutoffFrequency(Convert.ToSingle(RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filterfrequencyHeader)].Text), 48000f).ToString("0.0") + " Hz";
            FilteroneoverqPot.Value = Convert.ToDouble(RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filteroneoverqHeader)].Text);
            FilteroneoverqvalueLabel.Text = RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(filteroneoverqHeader)].Text;
            ReverbpresetComboBox.SelectedIndex = Convert.ToInt32(RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbidHeader)].Text);
            ReverbwetdryPot.Value = Convert.ToDouble(RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbwetdryHeader)].Text);
            ReverbwetdryvalueLabel.Text = RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbwetdryHeader)].Text + " %";
        }

        private void RoomstoreButton_Click(object sender, EventArgs e)
        {
            if (CheckRoomDuplicates() == true)
            {
                MessageBox.Show("Room creation impossible!\n\n" +
                    "Please use a unique room/filter name.");
            }
            else if (CheckRoomDuplicates() == false)
            {
                var actualReverbParameter = Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(RoomCreationEffects.ReverbPresets[ReverbpresetComboBox.SelectedIndex], false);
                ListViewItem roomitem = new(RoomnameTextBox.Text);
                //Filter Items
                roomitem.SubItems.Add(FilternameTextBox.Text);
                roomitem.SubItems.Add(FilterComboBox.SelectedIndex.ToString());
                roomitem.SubItems.Add(Math.Round(FilterfrequencyPot.Value, 1).ToString("0.0"));
                roomitem.SubItems.Add(Math.Round(FilteroneoverqPot.Value, 1).ToString("0.0"));
                //reverb Items
                roomitem.SubItems.Add(ReverbpresetComboBox.SelectedItem.ToString());
                roomitem.SubItems.Add(ReverbpresetComboBox.SelectedIndex.ToString());
                roomitem.SubItems.Add(ReverbwetdryPot.Value.ToString("0.0"));
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
                RoomListView.Items.Add(roomitem);

                if (RoomListView.Items.Count > 0)
                {
                    RoommapButton.Enabled = true;
                    RoomunmapButton.Enabled = true;
                    RoomremoveButton.Enabled = true;
                }
            }
        }

        private void RoomremoveButton_Click(object sender, EventArgs e)
        {
            if (RoomListView.SelectedItems.Count == 1)
            {
                int a = 0;
                foreach (var item in FilelistView.Items)
                {
                    if (FilelistView.Items[a].SubItems[FilelistView.Columns.IndexOf(roommapHeader)].Text == RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(roomnameHeader)].Text)
                    {
                        FilelistView.Items[a].SubItems[FilelistView.Columns.IndexOf(roommapHeader)].Text = "";
                        FilelistView.Items[a].SubItems[FilelistView.Columns.IndexOf(roomidHeader)].Text = "";
                    }
                    a++;
                }
                RoomListView.SelectedItems[0].Remove();

                if (RoomListView.Items.Count == 0)
                {
                    RoommapButton.Enabled = false;
                    RoomunmapButton.Enabled = false;
                    RoomremoveButton.Enabled = false;
                }
            }
        }

        private bool CheckRoomDuplicates()
        {
            int a = 0;
            foreach (var item in RoomListView.Items)
            {
                if (RoomListView.Items[a].SubItems[RoomListView.Columns.IndexOf(roomnameHeader)].Text == RoomnameTextBox.Text ||
                    RoomListView.Items[a].SubItems[RoomListView.Columns.IndexOf(filternameHeader)].Text == FilternameTextBox.Text)
                    return true;
                a++;
            }
            return false;
        }

        private void ReverbwetdryPot_ValueChanged(object sender, EventArgs e)
        {
            RoomCreationEffects.UpdateReverbSettings(FilelistView.SelectedItems.Count, ap.sourceVoice);
            ReverbwetdryvalueLabel.Text = Math.Round(ReverbwetdryPot.Value, 1).ToString("0.0") + " %";

            if (RoomListView.SelectedItems.Count == 1)
            {
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbwetdryHeader)].Text = Math.Round(ReverbwetdryPot.Value, 1).ToString("0.0");
            }
        }

        private void RoommapButton_Click(object sender, EventArgs e)
        {
            if (FilelistView.SelectedItems.Count > 0 &&
                RoomListView.SelectedItems.Count == 1)
            {
                int a = 0;
                foreach (var item in FilelistView.SelectedItems)
                {
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(roommapHeader)].Text = RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(roomnameHeader)].Text;
                    FilelistView.SelectedItems[a].SubItems[FilelistView.Columns.IndexOf(roomidHeader)].Text = RoomListView.SelectedItems[0].Index.ToString();
                    a++;
                }
            }
        }

        private void RoomunmapButton_Click(object sender, EventArgs e)
        {
            if (RoomListView.SelectedItems.Count == 1)
            {
                int a = 0;
                foreach (var item in FilelistView.Items)
                {
                    if (FilelistView.Items[a].SubItems[FilelistView.Columns.IndexOf(roommapHeader)].Text == RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(roomnameHeader)].Text)
                    {
                        FilelistView.Items[a].SubItems[FilelistView.Columns.IndexOf(roommapHeader)].Text = "";
                        FilelistView.Items[a].SubItems[FilelistView.Columns.IndexOf(roomidHeader)].Text = "";
                    }
                    a++;
                }
            }
        }

        private void RoomenableButton_Click(object sender, EventArgs e)
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

        private void ReverbpresetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var actualReverbParameter = Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(RoomCreationEffects.ReverbPresets[ReverbpresetComboBox.SelectedIndex], false);

            RoomCreationEffects.UpdateReverbSettings(FilelistView.SelectedItems.Count, ap.sourceVoice);
            ReverbwetdryPot.Value = Math.Round(actualReverbParameter.WetDryMix, 1);
            ReverbwetdryvalueLabel.Text = Math.Round(actualReverbParameter.WetDryMix, 1).ToString("0.0") + " %";

            if (RoomListView.SelectedItems.Count == 1)
            {
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbpresetHeader)].Text = ReverbpresetComboBox.SelectedItem.ToString();
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbwetdryHeader)].Text = actualReverbParameter.WetDryMix.ToString("0.0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbreflectionsdelayHeader)].Text = actualReverbParameter.ReflectionsDelay.ToString("0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbroomfrequencyHeader)].Text = actualReverbParameter.RoomFilterFreq.ToString("0.0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbdelayHeader)].Text = actualReverbParameter.ReverbDelay.ToString("0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbroomfiltermainHeader)].Text = actualReverbParameter.RoomFilterMain.ToString("0.0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbroomfilterhfHeader)].Text = actualReverbParameter.RoomFilterHF.ToString("0.0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbreflectionsgainHeader)].Text = actualReverbParameter.ReflectionsGain.ToString("0.0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbgainHeader)].Text = actualReverbParameter.ReverbGain.ToString("0.0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbdecaytimeHeader)].Text = actualReverbParameter.DecayTime.ToString("0.0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbdensityHeader)].Text = actualReverbParameter.Density.ToString("0.0");
                RoomListView.SelectedItems[0].SubItems[RoomListView.Columns.IndexOf(reverbroomsizeHeader)].Text = actualReverbParameter.RoomSize.ToString("0.0");
            }
        }

        #endregion Property-RoomCreation

        #region Property-Effects

        //######################################################
        //###                                                ###
        //### Elements for property editor "Effects"         ###
        //###                                                ###
        //######################################################


        private void PitchenableButton_Click(object sender, EventArgs e)
        {
            if (PitchenableButton.Text == "Off")
            {
                double soundPitchFactor = Math.Round(PitchPot.Value / 100.00, 2);
                double soundPitchRand = Math.Round(PitrandPot.Value / 100.00, 2);
                ap.SetPitch(soundPitchFactor, soundPitchRand, FilelistView.SelectedItems.Count);
                PitchenableButton.Text = "On";
                PitchenableButton.BackColor = Color.LightGreen;
            }

            else if (PitchenableButton.Text == "On")
            {
                ap.SetPitch(1, 0, FilelistView.SelectedItems.Count);
                PitchenableButton.Text = "Off";
                PitchenableButton.BackColor = Color.Salmon;
            }
        }

        private void PitchPot_ValueChanged(object sender, EventArgs e)
        {
            double soundPitchFactor = Math.Round(PitchPot.Value / 100.00, 2);
            double soundPitchRand = Math.Round(PitrandPot.Value / 100.00, 2);

            if (PitchenableButton.BackColor == Color.LightGreen)
            {
                ap.SetPitch(soundPitchFactor, soundPitchRand, FilelistView.SelectedItems.Count);
            }

            PitchvalueLabel.Text = soundPitchFactor.ToString();

            if (FilelistView.SelectedItems.Count == 1)
            {
                FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(pitchHeader)].Text = soundPitchFactor.ToString("");
            }
        }

        private void PitrandPot_ValueChanged(object sender, EventArgs e)
        {
            double soundPitchRand = Math.Round(PitrandPot.Value / 100.00, 2);
            PitchrandvalueLabel.Text = soundPitchRand.ToString("");

            if (FilelistView.SelectedItems.Count == 1)
            {
                FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(pitchrandHeader)].Text = soundPitchRand.ToString("");
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            PitchvalueLabel.Text = "1";
            PitchPot.Value = 100;
            PitchrandvalueLabel.Text = "0";
            PitrandPot.Value = 0;

            if (PitchenableButton.BackColor == Color.LightGreen)
            {
                ap.SetPitch(Convert.ToSingle(PitchvalueLabel.Text), Convert.ToSingle(PitchrandvalueLabel.Text), FilelistView.SelectedItems.Count);
            }

            if (FilelistView.SelectedItems.Count == 1)
            {
                FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(pitchHeader)].Text = "1";
                FilelistView.SelectedItems[0].SubItems[FilelistView.Columns.IndexOf(pitchrandHeader)].Text = "0";
            }
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
        private readonly CaseInsensitiveComparer ObjectCompare;

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
