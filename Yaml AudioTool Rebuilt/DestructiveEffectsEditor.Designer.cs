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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.NormalizeButton = new System.Windows.Forms.Button();
            this.WaveformsPlot = new ScottPlot.FormsPlot();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.WaveformsPlot, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(959, 337);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.NormalizeButton, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(770, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(186, 331);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // NormalizeButton
            // 
            this.NormalizeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NormalizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NormalizeButton.Location = new System.Drawing.Point(4, 4);
            this.NormalizeButton.Margin = new System.Windows.Forms.Padding(4);
            this.NormalizeButton.Name = "NormalizeButton";
            this.NormalizeButton.Size = new System.Drawing.Size(178, 32);
            this.NormalizeButton.TabIndex = 28;
            this.NormalizeButton.Text = "Normalize";
            this.NormalizeButton.UseVisualStyleBackColor = true;
            this.NormalizeButton.Click += new System.EventHandler(this.NormalizeButton_Click);
            // 
            // WaveformsPlot
            // 
            this.WaveformsPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WaveformsPlot.Location = new System.Drawing.Point(4, 3);
            this.WaveformsPlot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WaveformsPlot.Name = "WaveformsPlot";
            this.WaveformsPlot.Size = new System.Drawing.Size(759, 331);
            this.WaveformsPlot.TabIndex = 5;
            // 
            // DestructiveEffectsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(1000, 400);
            this.MinimumSize = new System.Drawing.Size(1000, 400);
            this.Name = "DestructiveEffectsEditor";
            this.Text = "Destructive Effects Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DestructiveEffectsEditor_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button NormalizeButton;
        public ScottPlot.FormsPlot WaveformsPlot;
    }
}