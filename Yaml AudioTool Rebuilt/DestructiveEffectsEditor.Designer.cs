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
            NormalizeRevertButton = new System.Windows.Forms.Button();
            NormalizeButton = new System.Windows.Forms.Button();
            WaveformsPlot = new ScottPlot.FormsPlot();
            TableLayoutPanelFD = new System.Windows.Forms.TableLayoutPanel();
            BitsizeLabel = new System.Windows.Forms.Label();
            SamplerateLabel = new System.Windows.Forms.Label();
            ChannelsLabel = new System.Windows.Forms.Label();
            FilepathLabel = new System.Windows.Forms.Label();
            TableLayoutPanelDEE.SuspendLayout();
            TableLayoutPanelA.SuspendLayout();
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
            TableLayoutPanelDEE.Location = new System.Drawing.Point(12, 12);
            TableLayoutPanelDEE.Name = "TableLayoutPanelDEE";
            TableLayoutPanelDEE.RowCount = 2;
            TableLayoutPanelDEE.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            TableLayoutPanelDEE.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            TableLayoutPanelDEE.Size = new System.Drawing.Size(959, 337);
            TableLayoutPanelDEE.TabIndex = 1;
            // 
            // SaveButton
            // 
            SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            SaveButton.Enabled = false;
            SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            SaveButton.Location = new System.Drawing.Point(771, 307);
            SaveButton.Margin = new System.Windows.Forms.Padding(4);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(184, 26);
            SaveButton.TabIndex = 30;
            SaveButton.Text = "Save Changes";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // TableLayoutPanelA
            // 
            TableLayoutPanelA.ColumnCount = 2;
            TableLayoutPanelA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            TableLayoutPanelA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            TableLayoutPanelA.Controls.Add(NormalizeRevertButton, 1, 0);
            TableLayoutPanelA.Controls.Add(NormalizeButton, 0, 0);
            TableLayoutPanelA.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelA.Location = new System.Drawing.Point(770, 3);
            TableLayoutPanelA.Name = "TableLayoutPanelA";
            TableLayoutPanelA.RowCount = 3;
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            TableLayoutPanelA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            TableLayoutPanelA.Size = new System.Drawing.Size(186, 297);
            TableLayoutPanelA.TabIndex = 4;
            // 
            // NormalizeRevertButton
            // 
            NormalizeRevertButton.BackColor = System.Drawing.SystemColors.Control;
            NormalizeRevertButton.BackgroundImage = Properties.Resources.Undo;
            NormalizeRevertButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            NormalizeRevertButton.Dock = System.Windows.Forms.DockStyle.Fill;
            NormalizeRevertButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NormalizeRevertButton.Location = new System.Drawing.Point(152, 4);
            NormalizeRevertButton.Margin = new System.Windows.Forms.Padding(4);
            NormalizeRevertButton.Name = "NormalizeRevertButton";
            NormalizeRevertButton.Size = new System.Drawing.Size(30, 32);
            NormalizeRevertButton.TabIndex = 29;
            NormalizeRevertButton.UseVisualStyleBackColor = false;
            NormalizeRevertButton.Visible = false;
            NormalizeRevertButton.Click += NormalizeRevertButton_Click;
            // 
            // NormalizeButton
            // 
            NormalizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            NormalizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            NormalizeButton.Location = new System.Drawing.Point(4, 4);
            NormalizeButton.Margin = new System.Windows.Forms.Padding(4);
            NormalizeButton.Name = "NormalizeButton";
            NormalizeButton.Size = new System.Drawing.Size(140, 32);
            NormalizeButton.TabIndex = 28;
            NormalizeButton.Text = "Normalize";
            NormalizeButton.UseVisualStyleBackColor = true;
            NormalizeButton.Click += NormalizeButton_Click;
            // 
            // WaveformsPlot
            // 
            WaveformsPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            WaveformsPlot.Location = new System.Drawing.Point(4, 3);
            WaveformsPlot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            WaveformsPlot.Name = "WaveformsPlot";
            WaveformsPlot.Size = new System.Drawing.Size(759, 297);
            WaveformsPlot.TabIndex = 5;
            // 
            // TableLayoutPanelFD
            // 
            TableLayoutPanelFD.ColumnCount = 4;
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelFD.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            TableLayoutPanelFD.Controls.Add(BitsizeLabel, 3, 0);
            TableLayoutPanelFD.Controls.Add(SamplerateLabel, 2, 0);
            TableLayoutPanelFD.Controls.Add(ChannelsLabel, 1, 0);
            TableLayoutPanelFD.Controls.Add(FilepathLabel, 0, 0);
            TableLayoutPanelFD.Dock = System.Windows.Forms.DockStyle.Fill;
            TableLayoutPanelFD.Location = new System.Drawing.Point(3, 306);
            TableLayoutPanelFD.Name = "TableLayoutPanelFD";
            TableLayoutPanelFD.RowCount = 1;
            TableLayoutPanelFD.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelFD.Size = new System.Drawing.Size(761, 28);
            TableLayoutPanelFD.TabIndex = 6;
            TableLayoutPanelFD.Visible = false;
            // 
            // BitsizeLabel
            // 
            BitsizeLabel.AutoSize = true;
            BitsizeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            BitsizeLabel.Location = new System.Drawing.Point(684, 0);
            BitsizeLabel.Name = "BitsizeLabel";
            BitsizeLabel.Size = new System.Drawing.Size(74, 28);
            BitsizeLabel.TabIndex = 3;
            BitsizeLabel.Text = "Bit Size";
            BitsizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SamplerateLabel
            // 
            SamplerateLabel.AutoSize = true;
            SamplerateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            SamplerateLabel.Location = new System.Drawing.Point(604, 0);
            SamplerateLabel.Name = "SamplerateLabel";
            SamplerateLabel.Size = new System.Drawing.Size(74, 28);
            SamplerateLabel.TabIndex = 2;
            SamplerateLabel.Text = "Samplerate";
            SamplerateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChannelsLabel
            // 
            ChannelsLabel.AutoSize = true;
            ChannelsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            ChannelsLabel.Location = new System.Drawing.Point(524, 0);
            ChannelsLabel.Name = "ChannelsLabel";
            ChannelsLabel.Size = new System.Drawing.Size(74, 28);
            ChannelsLabel.TabIndex = 1;
            ChannelsLabel.Text = "Channels";
            ChannelsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FilepathLabel
            // 
            FilepathLabel.AutoSize = true;
            FilepathLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            FilepathLabel.Location = new System.Drawing.Point(3, 0);
            FilepathLabel.Name = "FilepathLabel";
            FilepathLabel.Size = new System.Drawing.Size(515, 28);
            FilepathLabel.TabIndex = 0;
            FilepathLabel.Text = "Filepath";
            FilepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DestructiveEffectsEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(984, 361);
            Controls.Add(TableLayoutPanelDEE);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            MaximumSize = new System.Drawing.Size(1000, 400);
            MinimumSize = new System.Drawing.Size(1000, 400);
            Name = "DestructiveEffectsEditor";
            Text = "Destructive Effects Editor";
            FormClosing += DestructiveEffectsEditor_FormClosing;
            TableLayoutPanelDEE.ResumeLayout(false);
            TableLayoutPanelA.ResumeLayout(false);
            TableLayoutPanelFD.ResumeLayout(false);
            TableLayoutPanelFD.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelDEE;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelA;
        public ScottPlot.FormsPlot WaveformsPlot;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelFD;
        private System.Windows.Forms.Label BitsizeLabel;
        private System.Windows.Forms.Label SamplerateLabel;
        private System.Windows.Forms.Label ChannelsLabel;
        private System.Windows.Forms.Label FilepathLabel;
        private System.Windows.Forms.Button NormalizeButton;
        private System.Windows.Forms.Button NormalizeRevertButton;
        private System.Windows.Forms.Button SaveButton;
    }
}