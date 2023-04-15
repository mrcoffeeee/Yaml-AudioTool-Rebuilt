namespace Yaml_AudioTool_Rebuilt
{
    partial class DestructiveEffectsEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DestructiveEffectsEditor));
            TableLayoutPanelDEE = new System.Windows.Forms.TableLayoutPanel();
            TableLayoutPanelA = new System.Windows.Forms.TableLayoutPanel();
            NormalizeButton = new System.Windows.Forms.Button();
            FadeComboBox = new System.Windows.Forms.ComboBox();
            TrimButton = new System.Windows.Forms.Button();
            TableLayoutPanelVolume = new System.Windows.Forms.TableLayoutPanel();
            VolumeDownButton = new System.Windows.Forms.Button();
            VolumeUpButton = new System.Windows.Forms.Button();
            RemoveMarkerButton = new System.Windows.Forms.Button();
            FadeButton = new System.Windows.Forms.Button();
            WaveformsPlot = new ScottPlot.FormsPlot();
            TableLayoutPanelFD = new System.Windows.Forms.TableLayoutPanel();
            FilenameLabel = new System.Windows.Forms.Label();
            SamplerateLabel = new System.Windows.Forms.Label();
            PeakLabel = new System.Windows.Forms.Label();
            ChannelsLabel = new System.Windows.Forms.Label();
            PositionLabel = new System.Windows.Forms.Label();
            TableLayoutPanelChanges = new System.Windows.Forms.TableLayoutPanel();
            SaveButton = new System.Windows.Forms.Button();
            RevertButton = new System.Windows.Forms.Button();
            TableLayoutPanelDEE.SuspendLayout();
            TableLayoutPanelA.SuspendLayout();
            TableLayoutPanelVolume.SuspendLayout();
            TableLayoutPanelFD.SuspendLayout();
            TableLayoutPanelChanges.SuspendLayout();
            SuspendLayout();
            // 
            // TableLayoutPanelDEE
            // 
            TableLayoutPanelDEE.ColumnCount = 2;
            TableLayoutPanelDEE.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
            TableLayoutPanelDEE.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            TableLayoutPanelDEE.Controls.Add(TableLayoutPanelA, 1, 0);
            TableLayoutPanelDEE.Controls.Add(WaveformsPlot, 0, 0);
            TableLayoutPanelDEE.Controls.Add(TableLayoutPanelFD, 0, 1);
            TableLayoutPanelDEE.Controls.Add(TableLayoutPanelChanges, 1, 1);
            TableLayoutPanelDEE.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelDEE.Location = new System.Drawing.Point(0, 0);
            TableLayoutPanelDEE.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            TableLayoutPanelDEE.Name = "TableLayoutPanelDEE";
            TableLayoutPanelDEE.RowCount = 2;
            TableLayoutPanelDEE.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            TableLayoutPanelDEE.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            TableLayoutPanelDEE.Size = new System.Drawing.Size(1401, 580);
            TableLayoutPanelDEE.TabIndex = 1;
            // 
            // TableLayoutPanelA
            // 
            TableLayoutPanelA.ColumnCount = 1;
            TableLayoutPanelA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelA.Controls.Add(NormalizeButton, 0, 0);
            TableLayoutPanelA.Controls.Add(FadeComboBox, 0, 4);
            TableLayoutPanelA.Controls.Add(TrimButton, 0, 2);
            TableLayoutPanelA.Controls.Add(TableLayoutPanelVolume, 0, 1);
            TableLayoutPanelA.Controls.Add(RemoveMarkerButton, 0, 5);
            TableLayoutPanelA.Controls.Add(FadeButton, 0, 3);
            TableLayoutPanelA.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelA.Location = new System.Drawing.Point(1194, 5);
            TableLayoutPanelA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            TableLayoutPanelA.Name = "TableLayoutPanelA";
            TableLayoutPanelA.RowCount = 7;
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            TableLayoutPanelA.Size = new System.Drawing.Size(203, 512);
            TableLayoutPanelA.TabIndex = 4;
            // 
            // NormalizeButton
            // 
            NormalizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            NormalizeButton.Enabled = false;
            NormalizeButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NormalizeButton.Location = new System.Drawing.Point(6, 7);
            NormalizeButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            NormalizeButton.Name = "NormalizeButton";
            NormalizeButton.Size = new System.Drawing.Size(191, 44);
            NormalizeButton.TabIndex = 28;
            NormalizeButton.Text = "Normalize";
            NormalizeButton.UseVisualStyleBackColor = true;
            NormalizeButton.Click += NormalizeButton_Click;
            // 
            // FadeComboBox
            // 
            FadeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            FadeComboBox.Enabled = false;
            FadeComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FadeComboBox.FormattingEnabled = true;
            FadeComboBox.Items.AddRange(new object[] { "Linear IN", "Exponential IN", "Linear OUT", "Exponential OUT" });
            FadeComboBox.Location = new System.Drawing.Point(3, 235);
            FadeComboBox.Name = "FadeComboBox";
            FadeComboBox.Size = new System.Drawing.Size(197, 33);
            FadeComboBox.TabIndex = 32;
            // 
            // TrimButton
            // 
            TrimButton.Dock = System.Windows.Forms.DockStyle.Fill;
            TrimButton.Enabled = false;
            TrimButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TrimButton.Location = new System.Drawing.Point(6, 123);
            TrimButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            TrimButton.Name = "TrimButton";
            TrimButton.Size = new System.Drawing.Size(191, 44);
            TrimButton.TabIndex = 30;
            TrimButton.Text = "Trim";
            TrimButton.UseVisualStyleBackColor = true;
            TrimButton.Click += TrimButton_Click;
            // 
            // TableLayoutPanelVolume
            // 
            TableLayoutPanelVolume.ColumnCount = 2;
            TableLayoutPanelVolume.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            TableLayoutPanelVolume.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            TableLayoutPanelVolume.Controls.Add(VolumeDownButton, 1, 0);
            TableLayoutPanelVolume.Controls.Add(VolumeUpButton, 0, 0);
            TableLayoutPanelVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelVolume.Location = new System.Drawing.Point(3, 61);
            TableLayoutPanelVolume.Name = "TableLayoutPanelVolume";
            TableLayoutPanelVolume.RowCount = 1;
            TableLayoutPanelVolume.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelVolume.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            TableLayoutPanelVolume.Size = new System.Drawing.Size(197, 52);
            TableLayoutPanelVolume.TabIndex = 33;
            // 
            // VolumeDownButton
            // 
            VolumeDownButton.Dock = System.Windows.Forms.DockStyle.Fill;
            VolumeDownButton.Enabled = false;
            VolumeDownButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            VolumeDownButton.Location = new System.Drawing.Point(104, 7);
            VolumeDownButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            VolumeDownButton.Name = "VolumeDownButton";
            VolumeDownButton.Size = new System.Drawing.Size(87, 38);
            VolumeDownButton.TabIndex = 33;
            VolumeDownButton.Text = "Vol  ˅";
            VolumeDownButton.UseVisualStyleBackColor = true;
            VolumeDownButton.Click += VolumeDownButton_Click;
            // 
            // VolumeUpButton
            // 
            VolumeUpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            VolumeUpButton.Enabled = false;
            VolumeUpButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            VolumeUpButton.Location = new System.Drawing.Point(6, 7);
            VolumeUpButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            VolumeUpButton.Name = "VolumeUpButton";
            VolumeUpButton.Size = new System.Drawing.Size(86, 38);
            VolumeUpButton.TabIndex = 32;
            VolumeUpButton.Text = "Vol  ˄";
            VolumeUpButton.UseVisualStyleBackColor = true;
            VolumeUpButton.Click += VolumeUpButton_Click;
            // 
            // RemoveMarkerButton
            // 
            RemoveMarkerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            RemoveMarkerButton.Enabled = false;
            RemoveMarkerButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            RemoveMarkerButton.Location = new System.Drawing.Point(6, 281);
            RemoveMarkerButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            RemoveMarkerButton.Name = "RemoveMarkerButton";
            RemoveMarkerButton.Size = new System.Drawing.Size(191, 44);
            RemoveMarkerButton.TabIndex = 35;
            RemoveMarkerButton.Text = "Remove Marker";
            RemoveMarkerButton.UseVisualStyleBackColor = true;
            RemoveMarkerButton.Click += RemoveMarkerButton_Click;
            // 
            // FadeButton
            // 
            FadeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            FadeButton.Enabled = false;
            FadeButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FadeButton.Location = new System.Drawing.Point(6, 181);
            FadeButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            FadeButton.Name = "FadeButton";
            FadeButton.Size = new System.Drawing.Size(191, 44);
            FadeButton.TabIndex = 31;
            FadeButton.Text = "Fade";
            FadeButton.UseVisualStyleBackColor = true;
            FadeButton.Click += FadeButton_Click;
            // 
            // WaveformsPlot
            // 
            WaveformsPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            WaveformsPlot.Enabled = false;
            WaveformsPlot.Location = new System.Drawing.Point(6, 5);
            WaveformsPlot.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            WaveformsPlot.Name = "WaveformsPlot";
            WaveformsPlot.Size = new System.Drawing.Size(1178, 512);
            WaveformsPlot.TabIndex = 5;
            WaveformsPlot.MouseDown += WaveformsPlot_MouseDown;
            WaveformsPlot.MouseMove += WaveformsPlot_MouseMove;
            // 
            // TableLayoutPanelFD
            // 
            TableLayoutPanelFD.ColumnCount = 5;
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 209F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 167F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            TableLayoutPanelFD.Controls.Add(FilenameLabel, 0, 0);
            TableLayoutPanelFD.Controls.Add(SamplerateLabel, 4, 0);
            TableLayoutPanelFD.Controls.Add(PeakLabel, 2, 0);
            TableLayoutPanelFD.Controls.Add(ChannelsLabel, 3, 0);
            TableLayoutPanelFD.Controls.Add(PositionLabel, 1, 0);
            TableLayoutPanelFD.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelFD.Location = new System.Drawing.Point(4, 527);
            TableLayoutPanelFD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            TableLayoutPanelFD.Name = "TableLayoutPanelFD";
            TableLayoutPanelFD.RowCount = 1;
            TableLayoutPanelFD.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelFD.Size = new System.Drawing.Size(1182, 48);
            TableLayoutPanelFD.TabIndex = 6;
            TableLayoutPanelFD.Visible = false;
            // 
            // FilenameLabel
            // 
            FilenameLabel.AutoSize = true;
            FilenameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            FilenameLabel.Location = new System.Drawing.Point(4, 0);
            FilenameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            FilenameLabel.Name = "FilenameLabel";
            FilenameLabel.Size = new System.Drawing.Size(593, 48);
            FilenameLabel.TabIndex = 0;
            FilenameLabel.Text = "Filename";
            FilenameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SamplerateLabel
            // 
            SamplerateLabel.AutoSize = true;
            SamplerateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            SamplerateLabel.Location = new System.Drawing.Point(1072, 0);
            SamplerateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            SamplerateLabel.Name = "SamplerateLabel";
            SamplerateLabel.Size = new System.Drawing.Size(106, 48);
            SamplerateLabel.TabIndex = 2;
            SamplerateLabel.Text = "Samplerate";
            SamplerateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PeakLabel
            // 
            PeakLabel.AutoSize = true;
            PeakLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            PeakLabel.Location = new System.Drawing.Point(814, 0);
            PeakLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PeakLabel.Name = "PeakLabel";
            PeakLabel.Size = new System.Drawing.Size(159, 48);
            PeakLabel.TabIndex = 4;
            PeakLabel.Text = "Peak: ";
            PeakLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChannelsLabel
            // 
            ChannelsLabel.AutoSize = true;
            ChannelsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            ChannelsLabel.Location = new System.Drawing.Point(981, 0);
            ChannelsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ChannelsLabel.Name = "ChannelsLabel";
            ChannelsLabel.Size = new System.Drawing.Size(83, 48);
            ChannelsLabel.TabIndex = 1;
            ChannelsLabel.Text = "Channels";
            ChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PositionLabel
            // 
            PositionLabel.AutoSize = true;
            PositionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            PositionLabel.Location = new System.Drawing.Point(605, 0);
            PositionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PositionLabel.Name = "PositionLabel";
            PositionLabel.Size = new System.Drawing.Size(201, 48);
            PositionLabel.TabIndex = 3;
            PositionLabel.Text = "Position (sec): ";
            PositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TableLayoutPanelChanges
            // 
            TableLayoutPanelChanges.ColumnCount = 2;
            TableLayoutPanelChanges.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            TableLayoutPanelChanges.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            TableLayoutPanelChanges.Controls.Add(SaveButton, 1, 0);
            TableLayoutPanelChanges.Controls.Add(RevertButton, 0, 0);
            TableLayoutPanelChanges.Location = new System.Drawing.Point(1193, 525);
            TableLayoutPanelChanges.Name = "TableLayoutPanelChanges";
            TableLayoutPanelChanges.RowCount = 1;
            TableLayoutPanelChanges.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelChanges.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            TableLayoutPanelChanges.Size = new System.Drawing.Size(204, 52);
            TableLayoutPanelChanges.TabIndex = 7;
            // 
            // SaveButton
            // 
            SaveButton.BackColor = System.Drawing.SystemColors.Control;
            SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            SaveButton.Enabled = false;
            SaveButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            SaveButton.Location = new System.Drawing.Point(87, 7);
            SaveButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(111, 38);
            SaveButton.TabIndex = 30;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = false;
            SaveButton.Click += SaveButton_Click;
            // 
            // RevertButton
            // 
            RevertButton.BackColor = System.Drawing.SystemColors.Control;
            RevertButton.BackgroundImage = Properties.Resources.Undo;
            RevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            RevertButton.Dock = System.Windows.Forms.DockStyle.Fill;
            RevertButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            RevertButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            RevertButton.Location = new System.Drawing.Point(6, 7);
            RevertButton.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            RevertButton.Name = "RevertButton";
            RevertButton.Size = new System.Drawing.Size(69, 38);
            RevertButton.TabIndex = 29;
            RevertButton.UseVisualStyleBackColor = false;
            RevertButton.Visible = false;
            RevertButton.Click += RevertButton_Click;
            // 
            // DestructiveEffectsEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1401, 580);
            Controls.Add(TableLayoutPanelDEE);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MinimumSize = new System.Drawing.Size(1415, 608);
            Name = "DestructiveEffectsEditor";
            Text = "Destructive Effects Editor";
            FormClosing += DestructiveEffectsEditor_FormClosing;
            TableLayoutPanelDEE.ResumeLayout(false);
            TableLayoutPanelA.ResumeLayout(false);
            TableLayoutPanelVolume.ResumeLayout(false);
            TableLayoutPanelFD.ResumeLayout(false);
            TableLayoutPanelFD.PerformLayout();
            TableLayoutPanelChanges.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelDEE;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelA;
        public ScottPlot.FormsPlot WaveformsPlot;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelFD;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.Label SamplerateLabel;
        private System.Windows.Forms.Label ChannelsLabel;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.Button NormalizeButton;
        private System.Windows.Forms.Button RevertButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button TrimButton;
        private System.Windows.Forms.Button VolumeUpButton;
        private System.Windows.Forms.Button FadeButton;
        private System.Windows.Forms.Label PeakLabel;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelVolume;
        private System.Windows.Forms.Button VolumeDownButton;
        private System.Windows.Forms.ComboBox FadeComboBox;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelChanges;
        private System.Windows.Forms.Button RemoveMarkerButton;
    }
}