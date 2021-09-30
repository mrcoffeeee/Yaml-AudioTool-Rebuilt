using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CSCore;
using CSCore.SoundOut;
using CSCore.Codecs;
using Vortice.XAudio2;

namespace Yaml_AudioTool_Rebuilt
{
    public partial class Form1 : Form
    {
        //###############################################################################################
        //###############################################################################################
        //###                                                                                         ###
        //### Variables & Objects                                                                     ###
        //###                                                                                         ###
        //###############################################################################################
        //###############################################################################################


        private int baseWindowWidth = 0;
        private int baseWindowHeight = 0;
        private int baseListViewWidth = 0;
        private int baseListViewHeight = 0;
        private int basefilterlistviewHeight = 0;

        private readonly AudioPlayback ap = new();
      //  private readonly Effect_PitchShifter formPitchshifter = new();
        

        //###############################################################################################
        //###############################################################################################
        //###                                                                                         ###
        //### MainForm events                                                                         ###
        //###                                                                                         ###
        //###############################################################################################
        //###############################################################################################


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


        //######################################################
        //###                                                ###
        //### Elements for MenuStrip section                 ###
        //###                                                ###
        //######################################################


        private void AbouttoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AboutDialog f2 = new();
            f2.ShowDialog();
        }


        //######################################################
        //###                                                ###
        //### Elements for "ListView" section                ###
        //###                                                ###
        //######################################################


        private void filelistView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count == 0)
            {
                ResetMainFormValues();
            }
            if (filelistView.SelectedItems.Count > 0)
            {
                
                
                ap.GetSoundFromList(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text);
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
                selectedsoundLabel.Text = "Selection: " + filelistView.SelectedItems[0].Text;
                double volumeValue = Convert.ToDouble(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(volumeHeader)].Text) * 100;
                VolumetrackBar.Value = Convert.ToInt32(volumeValue);
                volumevalueLabel.Text = Convert.ToString(Convert.ToInt32(volumeValue)) + " %";
                PrioritytrackBar.Value = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(priorityHeader)].Text);
                priorityvalueLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(priorityHeader)].Text;
                if(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(loopHeader)].Text == "true")
                    LoopButton.BackColor = Color.LightGreen;
                else if (filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(loopHeader)].Text == "false")
                    LoopButton.BackColor = Color.Salmon;
                MindistancetextBox.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(mindistanceHeader)].Text;
                MaxdistancetextBox.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(maxdistanceHeader)].Text;
                double dopplerValue = Convert.ToDouble(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(dopplerHeader)].Text);
                DopplertrackBar.Value = Convert.ToInt32(dopplerValue * 100);
                dopplervalueLabel.Text = Convert.ToString(dopplerValue);
                LocalizecheckBox.Checked = Convert.ToBoolean(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(localizeHeader)].Text);
                StreamcheckBox.Checked = Convert.ToBoolean(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(streamHeader)].Text);
                TypecomboBox.SelectedIndex = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(typeHeader)].Text);
                FalloffcomboBox.SelectedIndex = Convert.ToInt32(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(falloffHeader)].Text);

                PitchenableButton.Enabled = true;
                removeButtonEnabled(true);
            }
        }

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
            if (filelistView.Items.Count > 0 &&
                filelistView.SelectedItems.Count > 0 &&
                filelistView.Items.IndexOf(filelistView.SelectedItems[0]) > 0)
            {
                int a = filelistView.Items.IndexOf(filelistView.SelectedItems[0]);
                filelistView.Items[a - 1].Selected = true;
                ap.GetSoundFromList(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text);
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
                removeButtonEnabled(true);
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
            playbackTimer.Stop();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count == 1)
            {
                filelistView.Focus();
                ap.soundSource = CodecFactory.Instance.GetCodec(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text);
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
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            playbackTimer.Stop();
            ap.StopPlayback();
            filelistView.Focus();
            if (filelistView.Items.Count > 0 &&
                filelistView.SelectedItems.Count > 0 &&
                filelistView.Items.IndexOf(filelistView.SelectedItems[0]) < filelistView.Items.Count - 1)
            {
                int a = filelistView.Items.IndexOf(filelistView.SelectedItems[0]);
                filelistView.Items[a + 1].Selected = true;
                ap.GetSoundFromList(filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(filepathHeader)].Text);
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
                removeButtonEnabled(true);
            }
            else if (filelistView.SelectedItems.Count > 0)
            {
                timeLabel.Text = filelistView.SelectedItems[0].SubItems[filelistView.Columns.IndexOf(durationHeader)].Text;
            }
        }

        #endregion PlaybackSection

        #region YAMLEditor

        //######################################################
        //###                                                ###
        //### Elements for "YAML Editor" section             ###
        //###                                                ###
        //######################################################


        private void addfileButton_Click(object sender, EventArgs e)
        {
            ap.OpenFile();
            if (filelistView.Items.Count > 0 &&
                saveyamlButton.Enabled == false)
            {
                saveyamlButton.Enabled = true;
                filelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                filelistView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
                filelistView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        public void removeButtonEnabled(bool value)
        {
            removeButton.Enabled = value;
            removeallButton.Enabled = value;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            foreach (var item in filelistView.SelectedItems)
            {
                filelistView.SelectedItems[0].Remove();
            }            
            ResetMainFormValues();
        }

        private void removeallButton_Click(object sender, EventArgs e)
        {
            filelistView.Items.Clear();
            ResetMainFormValues();
        }

        private void openyamlButton_Click(object sender, EventArgs e)
        {
            YamlExportImport.ImportYAML();
            if (roomlistView.Items.Count > 0)
            {
                roommapButton.Enabled = true;
                roomunmapButton.Enabled = true;
                filterremoveButton.Enabled = true;
            }
            if (filelistView.Items.Count > 0 &&
                saveyamlButton.Enabled == false)
            {
                saveyamlButton.Enabled = true;
            }
        }

        private void saveyamlButton_Click(object sender, EventArgs e)
        {
            YamlExportImport.ExportYAML();
        }

        #endregion YAMLEditor

        #region Property-Playback

        //######################################################
        //###                                                ###
        //### Elements for property editor "Playback"        ###
        //###                                                ###
        //######################################################


        private void VolumetrackBar_Scroll_1(object sender, EventArgs e)
        {
            double value = ap.SetVolume(VolumetrackBar.Value, filelistView.SelectedItems.Count);
            volumevalueLabel.Text = Convert.ToString(Convert.ToInt32(value * 100)) + " %";

            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(volumeHeader)].Text = Convert.ToString(value);
                }                
            }
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

        private void MaxdistancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(maxdistanceHeader)].Text = MaxdistancetextBox.Text;
                }                
            }
        }

        private void MindistancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (filelistView.SelectedItems.Count > 0)
            {
                for (int a = 0; a < filelistView.SelectedItems.Count; a++)
                {
                    filelistView.SelectedItems[a].SubItems[filelistView.Columns.IndexOf(mindistanceHeader)].Text = MindistancetextBox.Text;
                }
            }
        }


        #endregion Property-Playback

        #region Property-Effects

        //######################################################
        //###                                                ###
        //### Elements for property editor "Effects"         ###
        //###                                                ###
        //######################################################


        private void PitchshifterButton_Click(object sender, EventArgs e)
        {
            Effect_PitchShifter pitchshifterForm = new();            
            pitchshifterForm.TopMost = true;
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

        private void reverbpresetcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var actualReverbParameter = Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(Effects.ReverbPresets[reverbpresetcomboBox.SelectedIndex], false);

            if (roomlistView.SelectedItems.Count == 1)
            {
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbpresetHeader)].Text = reverbpresetcomboBox.SelectedItem.ToString();
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbwetdryHeader)].Text = actualReverbParameter.WetDryMix.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbreflectionsdelayHeader)].Text = actualReverbParameter.ReflectionsDelay.ToString();
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbroomfrequencyHeader)].Text = actualReverbParameter.RoomFilterFreq.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbdelayHeader)].Text = actualReverbParameter.ReverbDelay.ToString();
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbroomfiltermainHeader)].Text = actualReverbParameter.RoomFilterMain.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbroomfilterhfHeader)].Text = actualReverbParameter.RoomFilterHF.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbreflectionsgainHeader)].Text = actualReverbParameter.ReflectionsGain.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbgainHeader)].Text = actualReverbParameter.ReverbGain.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbdecaytimeHeader)].Text = actualReverbParameter.DecayTime.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbdensityHeader)].Text = actualReverbParameter.Density.ToString("0.0");
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(reverbroomsizeHeader)].Text = actualReverbParameter.RoomSize.ToString("0.0");
            }

        }


        #endregion Property-Effects


        //######################################################
        //###                                                ###
        //### Misc form events                               ###
        //###                                                ###
        //######################################################


        private void Form1_Resize(object sender, EventArgs e)
        {
            filelistView.Width = baseListViewWidth + this.Width - baseWindowWidth;
            filelistView.Height = baseListViewHeight + this.Height - baseWindowHeight;
            roomlistView.Height = basefilterlistviewHeight + this.Height - baseWindowHeight;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ExitApplication();
        }


        //###############################################################################################
        //###############################################################################################
        //###                                                                                         ###
        //### Form- related functions                                                                 ###
        //###                                                                                         ###
        //###############################################################################################
        //###############################################################################################


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
            reverbpresetcomboBox.Text = reverbpresetcomboBox.Items[0].ToString();
        }

        private void ResetMainFormValues()
        {
            ap.StopPlayback();
            playbackTimer.Stop();
            removeButtonEnabled(false);
            if (filelistView.Items.Count == 0)
                saveyamlButton.Enabled = false;
            PitchenableButton.Enabled = false;
            PitchenableButton.Text = "Off";
            PitchenableButton.BackColor = Color.Salmon;
            timeLabel.Text = "00:00";
            selectedsoundLabel.Text = "Selection: NONE";
        }

        private void SetWindowsSize()
        {
            int value = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;

            if (value < 1900)
            {
                filelistView.Size = new Size(691, 836);
                roomlistView.Size = new Size(595, 180);
            }
            else if (value >= 1900)
            {
                this.Size = new Size(1600, 900);
                filelistView.Size = new Size(1200, 750);
                roomlistView.Size = new Size(350, 422);
            }
            baseWindowWidth = this.Width;
            baseWindowHeight = this.Height;
            baseListViewWidth = filelistView.Width;
            baseListViewHeight = filelistView.Height;
            basefilterlistviewHeight = roomlistView.Height;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ap.timerCount += playbackTimer.Interval;
            var timeSpan = TimeSpan.FromMilliseconds(ap.timerCount);
            timeLabel.Text = timeSpan.ToString(@"mm\:ss");
          //  meterLabel.Text = ap.peakLevel.ToString();

            if (ap.sourceVoice.State.BuffersQueued == 0)
            {
                if (LoopButton.BackColor == Color.Salmon)
                {
                    ap.StopPlayback();
                    playbackTimer.Stop();
                }
            }
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
                var actualReverbParameter = Vortice.XAudio2.Fx.Fx.ReverbConvertI3DL2ToNative(Effects.ReverbPresets[reverbpresetcomboBox.SelectedIndex], false);
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
                roomitem.SubItems.Add(actualReverbParameter.ReflectionsDelay.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.RoomFilterFreq.ToString("0.0"));
                roomitem.SubItems.Add(actualReverbParameter.ReverbDelay.ToString("0.0"));
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

        private void filtercomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (roomlistView.SelectedItems.Count == 1)
            {
                ap.SetFilterFreq(filelistView.SelectedItems.Count);
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filtertypeHeader)].Text = filtercomboBox.SelectedIndex.ToString();
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

        private void frequencyPot_ValueChanged(object sender, EventArgs e)
        {
            ap.SetFilterFreq(filelistView.SelectedItems.Count);
            float value = Convert.ToSingle(Math.Round(filterfrequencyPot.Value, 1));
            filterfrequencyvalueLabel.Text = IXAudio2.RadiansToCutoffFrequency(value, 48000f).ToString("0.0") + " Hz";


            if (roomlistView.SelectedItems.Count == 1)
            {
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filterfrequencyHeader)].Text = value.ToString("0.0");
            }
        }

        private void oneoverqPot_ValueChanged(object sender, EventArgs e)
        {
            ap.SetFilterFreq(filelistView.SelectedItems.Count);
            double value = Math.Round(filteroneoverqPot.Value, 1);
            filteroneoverqvalueLabel.Text = value.ToString("0.0");

            if (roomlistView.SelectedItems.Count == 1)
            {
                roomlistView.SelectedItems[0].SubItems[roomlistView.Columns.IndexOf(filteroneoverqHeader)].Text = value.ToString("0.0");
            }
        }

        private void reverbwetdryPot_ValueChanged(object sender, EventArgs e)
        {
            ap.SetReverbWetDry(filelistView.SelectedItems.Count);
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
    }
}
