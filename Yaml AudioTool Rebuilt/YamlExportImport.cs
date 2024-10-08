﻿using Force.Crc32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Yaml_AudioTool_Rebuilt
{
    class YamlExportImport
    {
        //######################################################
        //###                                                ###
        //### Export Functions                               ###
        //###                                                ###
        //######################################################        

        private static string CreateStringCRC32(string fileName)
        {            
            byte[] data = Encoding.ASCII.GetBytes(fileName);
            uint crcValue = Crc32Algorithm.Compute(data, 0, fileName.Length);
            return crcValue.ToString();
        }

        private static List<string> GenerateFilterEntries(List<string> lines)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int a = 0;
            lines.Add("  filters:");

            foreach (var item in f1.RoomListView.Items)
            {
                lines.Add("  - enumName: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.filternameHeader)].Text);
                lines.Add("    crc: " + CreateStringCRC32(f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.filternameHeader)].Text));
                lines.Add("    eFilter: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.filtertypeHeader)].Text);
                lines.Add("    fFrequency: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.filterfrequencyHeader)].Text.Replace(",", "."));
                lines.Add("    fOneOverQ: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.filteroneoverqHeader)].Text.Replace(",", "."));
                a++;
            }
            return lines;
        }

        private static List<string> GenerateReverbEntries(List<string> lines)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int a = 0;
            lines.Add("  reverbs:");

            foreach (var item in f1.RoomListView.Items)
            {
                lines.Add("  - effectDef:");
                lines.Add("      effectType: " + "0");
                lines.Add("      enumName: " + "Reverb-" + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbpresetHeader)].Text);
                lines.Add("      crc: " + CreateStringCRC32("Reverb-" + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbpresetHeader)].Text));
                lines.Add("    wetDryMix: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbwetdryHeader)].Text.Replace(",", "."));
                lines.Add("    reflectionsDelay: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbreflectionsdelayHeader)].Text.Replace(",", "."));
                lines.Add("    reverbDelay: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbdelayHeader)].Text.Replace(",", "."));
                lines.Add("    roomFilterFreq: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbroomfrequencyHeader)].Text.Replace(",", "."));
                lines.Add("    roomFilterMain: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbroomfiltermainHeader)].Text.Replace(",", "."));
                lines.Add("    roomFilterHF: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbroomfilterhfHeader)].Text.Replace(",", "."));
                lines.Add("    reflectionsGain: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbreflectionsgainHeader)].Text.Replace(",", "."));
                lines.Add("    reverbGain: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbgainHeader)].Text.Replace(",", "."));
                lines.Add("    decayTime: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbdecaytimeHeader)].Text.Replace(",", "."));
                lines.Add("    density: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbdensityHeader)].Text.Replace(",", "."));
                lines.Add("    roomSize: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbroomsizeHeader)].Text.Replace(",", "."));
                a++;
            }
            return lines;
        }

        private static List<string> GenerateRoomEntries(List<string> lines)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int a = 0;
            lines.Add("  rooms:");

            foreach (var item in f1.RoomListView.Items)
            {
                lines.Add("  - roomName: " + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.roomnameHeader)].Text);
                lines.Add("    crc: " + CreateStringCRC32(f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.roomnameHeader)].Text));
                lines.Add("    filterCRC: " + CreateStringCRC32(f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.filternameHeader)].Text));
                lines.Add("    effectCRCs:");
                lines.Add("    - " + CreateStringCRC32("Reverb-" + f1.RoomListView.Items[a].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbpresetHeader)].Text));
                a++;
            }
            return lines;
        }

        private static List<string> GenerateAudioEntries(List<string> lines)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            int a = 0;
            string eTypeFirst = "<%= ::AudioType::AUDIO_TYPE_";
            string eFalloffFirst = "<%= ::AudioFalloff::AUDIO_FALLOFF_";
            string eStackFirst = "<%= ::AudioStacking::AUDIO_STACK_";
            string eLast = " %>";
            lines.Add("  soundFiles:");
            foreach (var item in f1.FilelistView.Items)
            {
                lines.Add("  - enumName: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.titleHeader)].Text);
                lines.Add("    filename: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.filenameHeader)].Text);
                lines.Add("    stream: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.streamHeader)].Text);
                lines.Add("    loop: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.loopHeader)].Text);
                lines.Add("    volume: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.volumeHeader)].Text.Replace(",", "."));
                lines.Add("    minDistance: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.mindistanceHeader)].Text);
                lines.Add("    maxDistance: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.maxdistanceHeader)].Text);
                lines.Add("    eType: " + eTypeFirst + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.typeHeader)].Text + eLast);
                lines.Add("    eFalloff: " + eFalloffFirst + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.falloffHeader)].Text + eLast);
                lines.Add("    pitchRandomisation: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.pitchrandHeader)].Text.Replace(",", "."));
                lines.Add("    priority: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.priorityHeader)].Text);
                lines.Add("    crossfade: ");
                lines.Add("    basePitch: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.pitchHeader)].Text.Replace(",", "."));
                lines.Add("    dopplerFactor: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.dopplerHeader)].Text.Replace(",", "."));
                lines.Add("    localized: " + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.localizeHeader)].Text);
                lines.Add("    eStacking: " + eStackFirst + f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.stackHeader)].Text + eLast);
                lines.Add("    crc: " + CreateStringCRC32(f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.titleHeader)].Text));
                // ADD ROOM CRC ENTRY
                if (f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.roommapHeader)].Text != "")
                    lines.Add("    roomNameCRC: " + CreateStringCRC32(f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.roommapHeader)].Text));
                else
                    lines.Add("    roomNameCRC: " + "0");
                a++;
            }
            return lines;
        }

        public static void ExportYAML()
        {
            SaveFileDialog saveFileDialog = new()
            {
                Title = "Save your Yaml File",
                DefaultExt = "yaml",
                Filter = "Yaml files (*.yml)|*.yml|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Form1 f1 = (Form1)Application.OpenForms["Form1"];
                string savePath = saveFileDialog.FileName;
                List<string> lines =
                [
                    "AudioBank:"
                ];
                if (f1.RoomListView.Items.Count > 0)
                {
                    GenerateFilterEntries(lines);
                    GenerateReverbEntries(lines);
                    GenerateRoomEntries(lines);
                }
                GenerateAudioEntries(lines);

                File.WriteAllLines(savePath, lines);
            }
        }

        //######################################################
        //###                                                ###
        //### Import Functions                               ###
        //###                                                ###
        //######################################################

        private static void ReadSoundfileEntries(string yamlPath, int listOffset)
        {
            var fileStream = new FileStream(yamlPath, FileMode.Open, FileAccess.Read);
            using StreamReader StreamReader = new(fileStream, Encoding.UTF8);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string eTypeFirst = "<%= ::AudioType::AUDIO_TYPE_";
            string eFalloffFirst = "<%= ::AudioFalloff::AUDIO_FALLOFF_";
            string eStackFirst = "<%= ::AudioStacking::AUDIO_STACK_";
            string eLast = " %>";
            string line = "";
            SettingsDialog sd = new();
            string folderPath = sd.audiofolderLabel.Text;
            //string folderPath = Path.GetDirectoryName(yamlPath);
            // listOffset = f1.filelistView.Items.Count;
            int a = 0;

            while (true)
            {
                line = StreamReader.ReadLine();
                if (line == null)
                {
                    return;
                }
                else if (line.Contains("  soundFiles:"))
                {
                    break;
                }
            }

            line = StreamReader.ReadLine();

            while (line != null)
            {
                if (line.Contains("  - enumName: "))
                {
                    //Create audiofile entry in listview
                    ListViewItem fileitem = new(line.Replace("  - enumName: ", ""));
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    fileitem.SubItems.Add("");
                    f1.FilelistView.Items.Add(fileitem);
                    int index = fileitem.Index;

                    try
                    {
                        //Fill audiofile entry from yaml
                        line = StreamReader.ReadLine().Replace("    filename: ", "");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.filenameHeader)].Text = line;
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.filepathHeader)].Text = folderPath + "\\" + line.Replace("/", "\\") + ".wav";

                        //Fill missing audiofile entries from file
                        try
                        {
                            string filePath = f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.filepathHeader)].Text;
                            using var waveFileReader = new WaveFileReader(filePath);
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.sizeHeader)].Text = (waveFileReader.Length / 1000).ToString();
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.durationHeader)].Text = AudioPlayback.CalculateAudiolength(waveFileReader);
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.channelsHeader)].Text = (waveFileReader.WaveFormat.Channels).ToString();
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.samplerateHeader)].Text = (waveFileReader.WaveFormat.SampleRate / 1000.0).ToString();
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.bitrateHeader)].Text = (waveFileReader.WaveFormat.BitsPerSample * waveFileReader.WaveFormat.SampleRate / 1000).ToString();
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.bitsizeHeader)].Text = (waveFileReader.WaveFormat.BitsPerSample).ToString();
                        }
                        catch (Exception)
                        {
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.titleHeader)].Text = "--> FILE IS MISSING OR CORRUPT";
                        }

                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.streamHeader)].Text = StreamReader.ReadLine().Replace("    stream: ", "");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.loopHeader)].Text = StreamReader.ReadLine().Replace("    loop: ", "");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.volumeHeader)].Text = StreamReader.ReadLine().Replace("    volume: ", "").Replace(".", ",");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.mindistanceHeader)].Text = StreamReader.ReadLine().Replace("    minDistance: ", "");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.maxdistanceHeader)].Text = StreamReader.ReadLine().Replace("    maxDistance: ", "");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.typeHeader)].Text = StreamReader.ReadLine().Replace("    eType: ", "").Replace(eTypeFirst, "").Replace(eLast, "");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.falloffHeader)].Text = StreamReader.ReadLine().Replace("    eFalloff: ", "").Replace(eFalloffFirst, "").Replace(eLast, ""); ;
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.pitchrandHeader)].Text = StreamReader.ReadLine().Replace("    pitchRandomisation: ", "").Replace(".", ",");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.priorityHeader)].Text = StreamReader.ReadLine().Replace("    priority: ", "");
                        StreamReader.ReadLine();
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.pitchHeader)].Text = StreamReader.ReadLine().Replace("    basePitch: ", "").Replace(".", ",");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.dopplerHeader)].Text = StreamReader.ReadLine().Replace("    dopplerFactor: ", "").Replace(".", ",");
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.localizeHeader)].Text = StreamReader.ReadLine().Replace("    localized: ", "");
                        line = StreamReader.ReadLine();
                        if (line.Contains("    eStacking: "))
                        {
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.stackHeader)].Text = line.Replace("    eStacking: ", "").Replace(eStackFirst, "").Replace(eLast, ""); ;
                        }
                        else
                        {
                            f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.stackHeader)].Text = "MANY";
                        }

                        line = StreamReader.ReadLine();// Read crc:
                        line = StreamReader.ReadLine();// Read roomNameCRC: or enumName:
                        if (line == null)
                        {
                            return;
                        }
                        else if (line.Contains("    roomNameCRC: "))
                        {
                            if (line != "    roomNameCRC: 0")
                                f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.roommapHeader)].Text = line.Replace("    roomNameCRC: ", "");
                            line = StreamReader.ReadLine();
                        }
                    }
                    catch (Exception)
                    {
                        f1.FilelistView.Items[index].SubItems[f1.FilelistView.Columns.IndexOf(f1.titleHeader)].Text = "--> YAML ENTRY IS CORRUPT";
                        return;
                    }

                    a++;
                }
            }

            f1.FilelistView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            f1.FilelistView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            f1.FilelistView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private static int ReadRoomEntries(string yamlPath)
        {
            var fileStream = new FileStream(yamlPath, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(fileStream, Encoding.UTF8);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            string line = "";
            string roomName = "";
            string roomIndex = "";
            int listOffset = f1.RoomListView.Items.Count;

            while (true)
            {
                line = StreamReader.ReadLine();
                if (line == null)
                {
                    return listOffset;
                }
                else if (line.Contains("  rooms:"))
                {
                    break;
                }
            }

            while (!(line = StreamReader.ReadLine()).Contains("  soundFiles:"))
            {
                if (line.Contains("  - roomName: "))
                {
                    ListViewItem roomitem = new(line.Replace("  - roomName: ", ""));
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    roomitem.SubItems.Add("");
                    f1.RoomListView.Items.Add(roomitem);
                    roomName = roomitem.Text.ToString();
                    roomIndex = roomitem.Index.ToString();
                }

                if (line.Contains("    crc: "))
                {
                    line = line.Replace("    crc: ", "");
                    int a = 0;
                    foreach (var item in f1.FilelistView.Items)
                    {
                        if (f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.roommapHeader)].Text.Contains(line))
                        {
                            f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.roommapHeader)].Text = roomName;
                            f1.FilelistView.Items[a].SubItems[f1.FilelistView.Columns.IndexOf(f1.roomidHeader)].Text = roomIndex;
                        }
                        a++;
                    }
                }
            }
            return listOffset;
        }

        private static void ReadFilterEntries(string yamlPath, int listOffset)
        {
            var fileStream = new FileStream(yamlPath, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(fileStream, Encoding.UTF8);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string line = "";
            int a = 0;

            while (true)
            {
                line = StreamReader.ReadLine();
                if (line == null)
                {
                    return;
                }
                else if (line.Contains("  filters:"))
                {
                    break;
                }
            }

            while (!(line = StreamReader.ReadLine()).Contains("  reverbs:"))
            {
                if (line.Contains("  - enumName:"))
                {
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.filternameHeader)].Text = line.Replace("  - enumName: ", "");
                    StreamReader.ReadLine();
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.filtertypeHeader)].Text = StreamReader.ReadLine().Replace("    eFilter: ", "");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.filterfrequencyHeader)].Text = StreamReader.ReadLine().Replace("    fFrequency: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.filteroneoverqHeader)].Text = StreamReader.ReadLine().Replace("    fOneOverQ: ", "").Replace(".", ",");
                    a++;
                }
            }
        }

        private static void ReadReverbEntries(string yamlPath, int listOffset)
        {
            var fileStream = new FileStream(yamlPath, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(fileStream, Encoding.UTF8);
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            string line = StreamReader.ReadLine();
            int a = 0;

            while (true)
            {
                line = StreamReader.ReadLine();
                if (line == null)
                {
                    return;
                }
                else if (line.Contains("  reverbs:"))
                {
                    break;
                }
            }

            while (!(line = StreamReader.ReadLine()).Contains("  rooms:"))
            {
                if (line.Contains("      enumName: "))
                {
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbpresetHeader)].Text = line.Replace("      enumName: Reverb-", "");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbidHeader)].Text = f1.ReverbpresetComboBox.FindString(line.Replace("      enumName: Reverb-", "")).ToString();
                    StreamReader.ReadLine();
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbwetdryHeader)].Text = StreamReader.ReadLine().Replace("    wetDryMix: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbreflectionsdelayHeader)].Text = StreamReader.ReadLine().Replace("    reflectionsDelay: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbdelayHeader)].Text = StreamReader.ReadLine().Replace("    reverbDelay: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbroomfrequencyHeader)].Text = StreamReader.ReadLine().Replace("    roomFilterFreq: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbroomfiltermainHeader)].Text = StreamReader.ReadLine().Replace("    roomFilterMain: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbroomfilterhfHeader)].Text = StreamReader.ReadLine().Replace("    roomFilterHF: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbreflectionsgainHeader)].Text = StreamReader.ReadLine().Replace("    reflectionsGain: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbgainHeader)].Text = StreamReader.ReadLine().Replace("    reverbGain: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbdecaytimeHeader)].Text = StreamReader.ReadLine().Replace("    decayTime: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbdensityHeader)].Text = StreamReader.ReadLine().Replace("    density: ", "").Replace(".", ",");
                    f1.RoomListView.Items[a + listOffset].SubItems[f1.RoomListView.Columns.IndexOf(f1.reverbroomsizeHeader)].Text = StreamReader.ReadLine().Replace("    roomSize: ", "").Replace(".", ",");
                    a++;
                }
            }
        }

        public static void ImportYAML()
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Open your Yaml File",
                DefaultExt = "yaml",
                Filter = "Yaml files (*.yaml; *.yml)|*.yaml; *.yml|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                int listOffset = 0;
                try
                {
                    ReadSoundfileEntries(filePath, listOffset);

                    listOffset = ReadRoomEntries(filePath);
                    ReadFilterEntries(filePath, listOffset);
                    ReadReverbEntries(filePath, listOffset);
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to load YAML file correct!");
                }
            }
        }
    }
}