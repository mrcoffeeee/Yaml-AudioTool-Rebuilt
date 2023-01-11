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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.WaveviewerhScrollBar = new System.Windows.Forms.HScrollBar();
            this.customWaveViewer1 = new Yaml_AudioTool_Rebuilt.CustomWaveViewer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.NormalizeButton = new System.Windows.Forms.Button();
            this.channeltableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RightChannelLabel = new System.Windows.Forms.Label();
            this.MonoChannelLabel = new System.Windows.Forms.Label();
            this.LeftChannelLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.channeltableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.WaveviewerhScrollBar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.customWaveViewer1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(54, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(918, 337);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // WaveviewerhScrollBar
            // 
            this.WaveviewerhScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WaveviewerhScrollBar.LargeChange = 1;
            this.WaveviewerhScrollBar.Location = new System.Drawing.Point(0, 285);
            this.WaveviewerhScrollBar.Maximum = 0;
            this.WaveviewerhScrollBar.Name = "WaveviewerhScrollBar";
            this.WaveviewerhScrollBar.Size = new System.Drawing.Size(734, 31);
            this.WaveviewerhScrollBar.TabIndex = 2;
            this.WaveviewerhScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.WaveviewerhScrollBar_Scroll);
            // 
            // customWaveViewer1
            // 
            this.customWaveViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customWaveViewer1.Enabled = false;
            this.customWaveViewer1.Location = new System.Drawing.Point(3, 3);
            this.customWaveViewer1.Name = "customWaveViewer1";
            this.customWaveViewer1.PenColor = System.Drawing.Color.DodgerBlue;
            this.customWaveViewer1.PenWidth = 1F;
            this.customWaveViewer1.SamplesPerPixel = 128;
            this.customWaveViewer1.Size = new System.Drawing.Size(728, 279);
            this.customWaveViewer1.StartPosition = ((long)(0));
            this.customWaveViewer1.TabIndex = 3;
            this.customWaveViewer1.WaveStream = null;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.NormalizeButton, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(737, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(178, 279);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // NormalizeButton
            // 
            this.NormalizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NormalizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NormalizeButton.Location = new System.Drawing.Point(4, 4);
            this.NormalizeButton.Margin = new System.Windows.Forms.Padding(4);
            this.NormalizeButton.Name = "NormalizeButton";
            this.NormalizeButton.Size = new System.Drawing.Size(170, 32);
            this.NormalizeButton.TabIndex = 28;
            this.NormalizeButton.Text = "Normalize";
            this.NormalizeButton.UseVisualStyleBackColor = true;
            this.NormalizeButton.Click += new System.EventHandler(this.NormalizeButton_Click);
            // 
            // channeltableLayoutPanel
            // 
            this.channeltableLayoutPanel.ColumnCount = 1;
            this.channeltableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.channeltableLayoutPanel.Controls.Add(this.RightChannelLabel, 0, 2);
            this.channeltableLayoutPanel.Controls.Add(this.MonoChannelLabel, 0, 1);
            this.channeltableLayoutPanel.Controls.Add(this.LeftChannelLabel, 0, 0);
            this.channeltableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.channeltableLayoutPanel.Name = "channeltableLayoutPanel";
            this.channeltableLayoutPanel.RowCount = 3;
            this.channeltableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.channeltableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.channeltableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.channeltableLayoutPanel.Size = new System.Drawing.Size(36, 282);
            this.channeltableLayoutPanel.TabIndex = 2;
            // 
            // RightChannelLabel
            // 
            this.RightChannelLabel.AutoSize = true;
            this.RightChannelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightChannelLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.RightChannelLabel.Location = new System.Drawing.Point(3, 188);
            this.RightChannelLabel.Name = "RightChannelLabel";
            this.RightChannelLabel.Size = new System.Drawing.Size(30, 94);
            this.RightChannelLabel.TabIndex = 2;
            this.RightChannelLabel.Text = "\r\nR";
            this.RightChannelLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RightChannelLabel.Visible = false;
            // 
            // MonoChannelLabel
            // 
            this.MonoChannelLabel.AutoSize = true;
            this.MonoChannelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MonoChannelLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.MonoChannelLabel.Location = new System.Drawing.Point(3, 94);
            this.MonoChannelLabel.Name = "MonoChannelLabel";
            this.MonoChannelLabel.Size = new System.Drawing.Size(30, 94);
            this.MonoChannelLabel.TabIndex = 1;
            this.MonoChannelLabel.Text = "M";
            this.MonoChannelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MonoChannelLabel.Visible = false;
            // 
            // LeftChannelLabel
            // 
            this.LeftChannelLabel.AutoSize = true;
            this.LeftChannelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftChannelLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LeftChannelLabel.Location = new System.Drawing.Point(3, 0);
            this.LeftChannelLabel.Name = "LeftChannelLabel";
            this.LeftChannelLabel.Size = new System.Drawing.Size(30, 94);
            this.LeftChannelLabel.TabIndex = 0;
            this.LeftChannelLabel.Text = "\r\n\r\n\r\nL";
            this.LeftChannelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LeftChannelLabel.Visible = false;
            // 
            // DestructiveEffectsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 361);
            this.Controls.Add(this.channeltableLayoutPanel);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(1000, 400);
            this.MinimumSize = new System.Drawing.Size(1000, 400);
            this.Name = "DestructiveEffectsEditor";
            this.Text = "Destructive Effects Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DestructiveEffectsEditor_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.channeltableLayoutPanel.ResumeLayout(false);
            this.channeltableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.HScrollBar WaveviewerhScrollBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button NormalizeButton;
        public CustomWaveViewer customWaveViewer1;
        private System.Windows.Forms.TableLayoutPanel channeltableLayoutPanel;
        public System.Windows.Forms.Label RightChannelLabel;
        public System.Windows.Forms.Label MonoChannelLabel;
        public System.Windows.Forms.Label LeftChannelLabel;
    }
}