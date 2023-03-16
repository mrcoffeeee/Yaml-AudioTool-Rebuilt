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
            TableLayoutPanelDEE = new System.Windows.Forms.TableLayoutPanel();
            SaveButton = new System.Windows.Forms.Button();
            TableLayoutPanelA = new System.Windows.Forms.TableLayoutPanel();
            NormalizeButton = new System.Windows.Forms.Button();
            TrimButton = new System.Windows.Forms.Button();
            FadeButton = new System.Windows.Forms.Button();
            TableLayoutPanelVolume = new System.Windows.Forms.TableLayoutPanel();
            VolumeDownButton = new System.Windows.Forms.Button();
            VolumeUpButton = new System.Windows.Forms.Button();
            RevertButton = new System.Windows.Forms.Button();
            WaveformsPlot = new ScottPlot.FormsPlot();
            TableLayoutPanelFD = new System.Windows.Forms.TableLayoutPanel();
            FilenameLabel = new System.Windows.Forms.Label();
            SamplerateLabel = new System.Windows.Forms.Label();
            PeakLabel = new System.Windows.Forms.Label();
            ChannelsLabel = new System.Windows.Forms.Label();
            PositionLabel = new System.Windows.Forms.Label();
            TableLayoutPanelDEE.SuspendLayout();
            TableLayoutPanelA.SuspendLayout();
            TableLayoutPanelVolume.SuspendLayout();
            TableLayoutPanelFD.SuspendLayout();
            SuspendLayout();
            // 
            // TableLayoutPanelDEE
            // 
            TableLayoutPanelDEE.ColumnCount = 2;
            TableLayoutPanelDEE.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            TableLayoutPanelDEE.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            TableLayoutPanelDEE.Controls.Add(SaveButton, 1, 1);
            TableLayoutPanelDEE.Controls.Add(TableLayoutPanelA, 1, 0);
            TableLayoutPanelDEE.Controls.Add(WaveformsPlot, 0, 0);
            TableLayoutPanelDEE.Controls.Add(TableLayoutPanelFD, 0, 1);
            TableLayoutPanelDEE.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelDEE.Location = new System.Drawing.Point(0, 0);
            TableLayoutPanelDEE.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            TableLayoutPanelDEE.Name = "TableLayoutPanelDEE";
            TableLayoutPanelDEE.RowCount = 2;
            TableLayoutPanelDEE.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            TableLayoutPanelDEE.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            TableLayoutPanelDEE.Size = new System.Drawing.Size(1673, 672);
            TableLayoutPanelDEE.TabIndex = 1;
            // 
            // SaveButton
            // 
            SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            SaveButton.Enabled = false;
            SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            SaveButton.Location = new System.Drawing.Point(1345, 612);
            SaveButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(321, 52);
            SaveButton.TabIndex = 30;
            SaveButton.Text = "Save Changes";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // TableLayoutPanelA
            // 
            TableLayoutPanelA.ColumnCount = 1;
            TableLayoutPanelA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelA.Controls.Add(NormalizeButton, 0, 0);
            TableLayoutPanelA.Controls.Add(TrimButton, 0, 2);
            TableLayoutPanelA.Controls.Add(FadeButton, 0, 3);
            TableLayoutPanelA.Controls.Add(TableLayoutPanelVolume, 0, 1);
            TableLayoutPanelA.Controls.Add(RevertButton, 0, 7);
            TableLayoutPanelA.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelA.Location = new System.Drawing.Point(1343, 6);
            TableLayoutPanelA.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            TableLayoutPanelA.Name = "TableLayoutPanelA";
            TableLayoutPanelA.RowCount = 8;
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            TableLayoutPanelA.Size = new System.Drawing.Size(325, 592);
            TableLayoutPanelA.TabIndex = 4;
            // 
            // NormalizeButton
            // 
            NormalizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            NormalizeButton.Enabled = false;
            NormalizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NormalizeButton.Location = new System.Drawing.Point(7, 8);
            NormalizeButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            NormalizeButton.Name = "NormalizeButton";
            NormalizeButton.Size = new System.Drawing.Size(311, 64);
            NormalizeButton.TabIndex = 28;
            NormalizeButton.Text = "Normalize";
            NormalizeButton.UseVisualStyleBackColor = true;
            NormalizeButton.Click += NormalizeButton_Click;
            // 
            // TrimButton
            // 
            TrimButton.Dock = System.Windows.Forms.DockStyle.Fill;
            TrimButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TrimButton.Location = new System.Drawing.Point(7, 168);
            TrimButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            TrimButton.Name = "TrimButton";
            TrimButton.Size = new System.Drawing.Size(311, 64);
            TrimButton.TabIndex = 30;
            TrimButton.Text = "Trim";
            TrimButton.UseVisualStyleBackColor = true;
            TrimButton.Visible = false;
            // 
            // FadeButton
            // 
            FadeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            FadeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FadeButton.Location = new System.Drawing.Point(7, 248);
            FadeButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            FadeButton.Name = "FadeButton";
            FadeButton.Size = new System.Drawing.Size(311, 64);
            FadeButton.TabIndex = 31;
            FadeButton.Text = "Fade";
            FadeButton.UseVisualStyleBackColor = true;
            FadeButton.Visible = false;
            // 
            // TableLayoutPanelVolume
            // 
            TableLayoutPanelVolume.ColumnCount = 2;
            TableLayoutPanelVolume.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            TableLayoutPanelVolume.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            TableLayoutPanelVolume.Controls.Add(VolumeDownButton, 1, 0);
            TableLayoutPanelVolume.Controls.Add(VolumeUpButton, 0, 0);
            TableLayoutPanelVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelVolume.Location = new System.Drawing.Point(3, 83);
            TableLayoutPanelVolume.Name = "TableLayoutPanelVolume";
            TableLayoutPanelVolume.RowCount = 1;
            TableLayoutPanelVolume.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelVolume.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            TableLayoutPanelVolume.Size = new System.Drawing.Size(319, 74);
            TableLayoutPanelVolume.TabIndex = 33;
            // 
            // VolumeDownButton
            // 
            VolumeDownButton.Dock = System.Windows.Forms.DockStyle.Fill;
            VolumeDownButton.Enabled = false;
            VolumeDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            VolumeDownButton.Location = new System.Drawing.Point(166, 8);
            VolumeDownButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            VolumeDownButton.Name = "VolumeDownButton";
            VolumeDownButton.Size = new System.Drawing.Size(146, 58);
            VolumeDownButton.TabIndex = 33;
            VolumeDownButton.Text = "Volume  ˅";
            VolumeDownButton.UseVisualStyleBackColor = true;
            VolumeDownButton.Click += VolumeDownButton_Click;
            // 
            // VolumeUpButton
            // 
            VolumeUpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            VolumeUpButton.Enabled = false;
            VolumeUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            VolumeUpButton.Location = new System.Drawing.Point(7, 8);
            VolumeUpButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            VolumeUpButton.Name = "VolumeUpButton";
            VolumeUpButton.Size = new System.Drawing.Size(145, 58);
            VolumeUpButton.TabIndex = 32;
            VolumeUpButton.Text = "Volume  ˄";
            VolumeUpButton.UseVisualStyleBackColor = true;
            VolumeUpButton.Click += VolumeUpButton_Click;
            // 
            // RevertButton
            // 
            RevertButton.BackColor = System.Drawing.SystemColors.Control;
            RevertButton.BackgroundImage = Properties.Resources.Undo;
            RevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            RevertButton.Dock = System.Windows.Forms.DockStyle.Fill;
            RevertButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            RevertButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            RevertButton.Location = new System.Drawing.Point(7, 528);
            RevertButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            RevertButton.Name = "RevertButton";
            RevertButton.Size = new System.Drawing.Size(311, 56);
            RevertButton.TabIndex = 29;
            RevertButton.UseVisualStyleBackColor = false;
            RevertButton.Visible = false;
            RevertButton.Click += RevertButton_Click;
            // 
            // WaveformsPlot
            // 
            WaveformsPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            WaveformsPlot.Location = new System.Drawing.Point(7, 6);
            WaveformsPlot.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            WaveformsPlot.Name = "WaveformsPlot";
            WaveformsPlot.Size = new System.Drawing.Size(1324, 592);
            WaveformsPlot.TabIndex = 5;
            WaveformsPlot.MouseDown += WaveformsPlot_MouseDown;
            WaveformsPlot.MouseMove += WaveformsPlot_MouseMove;
            // 
            // TableLayoutPanelFD
            // 
            TableLayoutPanelFD.ColumnCount = 5;
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            TableLayoutPanelFD.Controls.Add(FilenameLabel, 0, 0);
            TableLayoutPanelFD.Controls.Add(SamplerateLabel, 4, 0);
            TableLayoutPanelFD.Controls.Add(PeakLabel, 2, 0);
            TableLayoutPanelFD.Controls.Add(ChannelsLabel, 3, 0);
            TableLayoutPanelFD.Controls.Add(PositionLabel, 1, 0);
            TableLayoutPanelFD.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelFD.Location = new System.Drawing.Point(5, 610);
            TableLayoutPanelFD.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            TableLayoutPanelFD.Name = "TableLayoutPanelFD";
            TableLayoutPanelFD.RowCount = 1;
            TableLayoutPanelFD.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelFD.Size = new System.Drawing.Size(1328, 56);
            TableLayoutPanelFD.TabIndex = 6;
            TableLayoutPanelFD.Visible = false;
            // 
            // FilenameLabel
            // 
            FilenameLabel.AutoSize = true;
            FilenameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            FilenameLabel.Location = new System.Drawing.Point(5, 0);
            FilenameLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            FilenameLabel.Name = "FilenameLabel";
            FilenameLabel.Size = new System.Drawing.Size(684, 56);
            FilenameLabel.TabIndex = 0;
            FilenameLabel.Text = "Filename";
            FilenameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SamplerateLabel
            // 
            SamplerateLabel.AutoSize = true;
            SamplerateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            SamplerateLabel.Location = new System.Drawing.Point(1196, 0);
            SamplerateLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            SamplerateLabel.Name = "SamplerateLabel";
            SamplerateLabel.Size = new System.Drawing.Size(127, 56);
            SamplerateLabel.TabIndex = 2;
            SamplerateLabel.Text = "Samplerate";
            SamplerateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PeakLabel
            // 
            PeakLabel.AutoSize = true;
            PeakLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            PeakLabel.Location = new System.Drawing.Point(949, 0);
            PeakLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            PeakLabel.Name = "PeakLabel";
            PeakLabel.Size = new System.Drawing.Size(127, 56);
            PeakLabel.TabIndex = 4;
            PeakLabel.Text = "Peak: ";
            PeakLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChannelsLabel
            // 
            ChannelsLabel.AutoSize = true;
            ChannelsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            ChannelsLabel.Location = new System.Drawing.Point(1086, 0);
            ChannelsLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            ChannelsLabel.Name = "ChannelsLabel";
            ChannelsLabel.Size = new System.Drawing.Size(100, 56);
            ChannelsLabel.TabIndex = 1;
            ChannelsLabel.Text = "Channels";
            ChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PositionLabel
            // 
            PositionLabel.AutoSize = true;
            PositionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            PositionLabel.Location = new System.Drawing.Point(699, 0);
            PositionLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            PositionLabel.Name = "PositionLabel";
            PositionLabel.Size = new System.Drawing.Size(240, 56);
            PositionLabel.TabIndex = 3;
            PositionLabel.Text = "Position (sec): ";
            PositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DestructiveEffectsEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1673, 672);
            Controls.Add(TableLayoutPanelDEE);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            MaximumSize = new System.Drawing.Size(1697, 736);
            MinimumSize = new System.Drawing.Size(1697, 736);
            Name = "DestructiveEffectsEditor";
            Text = "Destructive Effects Editor";
            FormClosing += DestructiveEffectsEditor_FormClosing;
            TableLayoutPanelDEE.ResumeLayout(false);
            TableLayoutPanelA.ResumeLayout(false);
            TableLayoutPanelVolume.ResumeLayout(false);
            TableLayoutPanelFD.ResumeLayout(false);
            TableLayoutPanelFD.PerformLayout();
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
    }
}