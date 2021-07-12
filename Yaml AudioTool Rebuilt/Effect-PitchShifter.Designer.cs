
namespace Yaml_AudioTool_Rebuilt
{
    partial class Effect_PitchShifter
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
            this.PitchPot = new NAudio.Gui.Pot();
            this.PitchrandvalueLabel = new System.Windows.Forms.Label();
            this.PitchrandLabel = new System.Windows.Forms.Label();
            this.PitchvalueLabel = new System.Windows.Forms.Label();
            this.PitchLabel = new System.Windows.Forms.Label();
            this.PitrandPot = new NAudio.Gui.Pot();
            this.PitchShifterPotsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PitchShifterValuesLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PitchShifterPotsLayoutPanel.SuspendLayout();
            this.PitchShifterValuesLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PitchPot
            // 
            this.PitchPot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PitchPot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PitchPot.Location = new System.Drawing.Point(7, 6);
            this.PitchPot.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.PitchPot.Maximum = 200D;
            this.PitchPot.Minimum = 50D;
            this.PitchPot.Name = "PitchPot";
            this.PitchPot.Size = new System.Drawing.Size(96, 104);
            this.PitchPot.TabIndex = 26;
            this.PitchPot.Value = 100D;
            this.PitchPot.ValueChanged += new System.EventHandler(this.PitchPot_ValueChanged);
            // 
            // PitchrandvalueLabel
            // 
            this.PitchrandvalueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PitchrandvalueLabel.AutoSize = true;
            this.PitchrandvalueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PitchrandvalueLabel.Location = new System.Drawing.Point(7, 188);
            this.PitchrandvalueLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.PitchrandvalueLabel.Name = "PitchrandvalueLabel";
            this.PitchrandvalueLabel.Size = new System.Drawing.Size(67, 31);
            this.PitchrandvalueLabel.TabIndex = 22;
            this.PitchrandvalueLabel.Text = "0,00";
            this.PitchrandvalueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PitchrandLabel
            // 
            this.PitchrandLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PitchrandLabel.AutoSize = true;
            this.PitchrandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PitchrandLabel.Location = new System.Drawing.Point(7, 129);
            this.PitchrandLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.PitchrandLabel.Name = "PitchrandLabel";
            this.PitchrandLabel.Size = new System.Drawing.Size(159, 31);
            this.PitchrandLabel.TabIndex = 7;
            this.PitchrandLabel.Text = "Randomize:";
            this.PitchrandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PitchvalueLabel
            // 
            this.PitchvalueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PitchvalueLabel.AutoSize = true;
            this.PitchvalueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PitchvalueLabel.Location = new System.Drawing.Point(7, 71);
            this.PitchvalueLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.PitchvalueLabel.Name = "PitchvalueLabel";
            this.PitchvalueLabel.Size = new System.Drawing.Size(67, 31);
            this.PitchvalueLabel.TabIndex = 21;
            this.PitchvalueLabel.Text = "1,00";
            this.PitchvalueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PitchLabel
            // 
            this.PitchLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PitchLabel.AutoSize = true;
            this.PitchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PitchLabel.Location = new System.Drawing.Point(7, 13);
            this.PitchLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.PitchLabel.Name = "PitchLabel";
            this.PitchLabel.Size = new System.Drawing.Size(77, 31);
            this.PitchLabel.TabIndex = 57;
            this.PitchLabel.Text = "Shift:";
            this.PitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PitrandPot
            // 
            this.PitrandPot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PitrandPot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PitrandPot.Location = new System.Drawing.Point(7, 122);
            this.PitrandPot.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.PitrandPot.Maximum = 150D;
            this.PitrandPot.Minimum = 0D;
            this.PitrandPot.Name = "PitrandPot";
            this.PitrandPot.Size = new System.Drawing.Size(96, 105);
            this.PitrandPot.TabIndex = 58;
            this.PitrandPot.Value = 0D;
            this.PitrandPot.ValueChanged += new System.EventHandler(this.PitrandPot_ValueChanged);
            // 
            // PitchShifterPotsLayoutPanel
            // 
            this.PitchShifterPotsLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PitchShifterPotsLayoutPanel.ColumnCount = 1;
            this.PitchShifterPotsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PitchShifterPotsLayoutPanel.Controls.Add(this.PitchPot, 0, 0);
            this.PitchShifterPotsLayoutPanel.Controls.Add(this.PitrandPot, 0, 1);
            this.PitchShifterPotsLayoutPanel.Location = new System.Drawing.Point(201, 24);
            this.PitchShifterPotsLayoutPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.PitchShifterPotsLayoutPanel.Name = "PitchShifterPotsLayoutPanel";
            this.PitchShifterPotsLayoutPanel.RowCount = 2;
            this.PitchShifterPotsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PitchShifterPotsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PitchShifterPotsLayoutPanel.Size = new System.Drawing.Size(110, 233);
            this.PitchShifterPotsLayoutPanel.TabIndex = 59;
            // 
            // PitchShifterValuesLayoutPanel
            // 
            this.PitchShifterValuesLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PitchShifterValuesLayoutPanel.ColumnCount = 1;
            this.PitchShifterValuesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PitchShifterValuesLayoutPanel.Controls.Add(this.PitchrandvalueLabel, 0, 3);
            this.PitchShifterValuesLayoutPanel.Controls.Add(this.PitchrandLabel, 0, 2);
            this.PitchShifterValuesLayoutPanel.Controls.Add(this.PitchvalueLabel, 0, 1);
            this.PitchShifterValuesLayoutPanel.Controls.Add(this.PitchLabel, 0, 0);
            this.PitchShifterValuesLayoutPanel.Location = new System.Drawing.Point(21, 24);
            this.PitchShifterValuesLayoutPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.PitchShifterValuesLayoutPanel.Name = "PitchShifterValuesLayoutPanel";
            this.PitchShifterValuesLayoutPanel.RowCount = 4;
            this.PitchShifterValuesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.PitchShifterValuesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.PitchShifterValuesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.PitchShifterValuesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.PitchShifterValuesLayoutPanel.Size = new System.Drawing.Size(175, 233);
            this.PitchShifterValuesLayoutPanel.TabIndex = 60;
            // 
            // Effect_PitchShifter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(326, 272);
            this.Controls.Add(this.PitchShifterValuesLayoutPanel);
            this.Controls.Add(this.PitchShifterPotsLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(350, 336);
            this.MinimumSize = new System.Drawing.Size(350, 336);
            this.Name = "Effect_PitchShifter";
            this.Text = "PitchShifter";
            this.PitchShifterPotsLayoutPanel.ResumeLayout(false);
            this.PitchShifterValuesLayoutPanel.ResumeLayout(false);
            this.PitchShifterValuesLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label PitchLabel;
        private System.Windows.Forms.Label PitchrandLabel;
        public NAudio.Gui.Pot PitchPot;
        public NAudio.Gui.Pot PitrandPot;
        public System.Windows.Forms.Label PitchvalueLabel;
        public System.Windows.Forms.Label PitchrandvalueLabel;
        private System.Windows.Forms.TableLayoutPanel PitchShifterPotsLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel PitchShifterValuesLayoutPanel;
    }
}