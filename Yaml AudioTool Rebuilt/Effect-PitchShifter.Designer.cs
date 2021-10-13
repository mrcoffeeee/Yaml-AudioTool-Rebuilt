
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
            this.resetButton = new System.Windows.Forms.Button();
            this.PitchShifterResetLayoutPanel = new System.Windows.Forms.Panel();
            this.PitchShifterPotsLayoutPanel.SuspendLayout();
            this.PitchShifterValuesLayoutPanel.SuspendLayout();
            this.PitchShifterResetLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PitchPot
            // 
            this.PitchPot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PitchPot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PitchPot.Location = new System.Drawing.Point(7, 6);
            this.PitchPot.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.PitchPot.Maximum = 200D;
            this.PitchPot.Minimum = 50D;
            this.PitchPot.Name = "PitchPot";
            this.PitchPot.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.PitchPot.Size = new System.Drawing.Size(96, 93);
            this.PitchPot.TabIndex = 26;
            this.PitchPot.Value = 100D;
            this.PitchPot.ValueChanged += new System.EventHandler(this.PitchPot_ValueChanged);
            // 
            // PitchrandvalueLabel
            // 
            this.PitchrandvalueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PitchrandvalueLabel.AutoSize = true;
            this.PitchrandvalueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PitchrandvalueLabel.Location = new System.Drawing.Point(7, 168);
            this.PitchrandvalueLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.PitchrandvalueLabel.Name = "PitchrandvalueLabel";
            this.PitchrandvalueLabel.Size = new System.Drawing.Size(58, 29);
            this.PitchrandvalueLabel.TabIndex = 22;
            this.PitchrandvalueLabel.Text = "0,00";
            this.PitchrandvalueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PitchrandLabel
            // 
            this.PitchrandLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PitchrandLabel.AutoSize = true;
            this.PitchrandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PitchrandLabel.Location = new System.Drawing.Point(7, 115);
            this.PitchrandLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.PitchrandLabel.Name = "PitchrandLabel";
            this.PitchrandLabel.Size = new System.Drawing.Size(141, 29);
            this.PitchrandLabel.TabIndex = 7;
            this.PitchrandLabel.Text = "Randomize:";
            this.PitchrandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PitchvalueLabel
            // 
            this.PitchvalueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PitchvalueLabel.AutoSize = true;
            this.PitchvalueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PitchvalueLabel.Location = new System.Drawing.Point(7, 63);
            this.PitchvalueLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.PitchvalueLabel.Name = "PitchvalueLabel";
            this.PitchvalueLabel.Size = new System.Drawing.Size(58, 29);
            this.PitchvalueLabel.TabIndex = 21;
            this.PitchvalueLabel.Text = "1,00";
            this.PitchvalueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PitchLabel
            // 
            this.PitchLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PitchLabel.AutoSize = true;
            this.PitchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PitchLabel.Location = new System.Drawing.Point(7, 11);
            this.PitchLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.PitchLabel.Name = "PitchLabel";
            this.PitchLabel.Size = new System.Drawing.Size(66, 29);
            this.PitchLabel.TabIndex = 57;
            this.PitchLabel.Text = "Shift:";
            this.PitchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PitrandPot
            // 
            this.PitrandPot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PitrandPot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PitrandPot.Location = new System.Drawing.Point(7, 111);
            this.PitrandPot.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.PitrandPot.Maximum = 150D;
            this.PitrandPot.Minimum = 0D;
            this.PitrandPot.Name = "PitrandPot";
            this.PitrandPot.Size = new System.Drawing.Size(96, 93);
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
            this.PitchShifterPotsLayoutPanel.Size = new System.Drawing.Size(110, 210);
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
            this.PitchShifterValuesLayoutPanel.Size = new System.Drawing.Size(175, 210);
            this.PitchShifterValuesLayoutPanel.TabIndex = 60;
            // 
            // resetButton
            // 
            this.resetButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.resetButton.Location = new System.Drawing.Point(180, 5);
            this.resetButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(107, 46);
            this.resetButton.TabIndex = 61;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // PitchShifterResetLayoutPanel
            // 
            this.PitchShifterResetLayoutPanel.Controls.Add(this.resetButton);
            this.PitchShifterResetLayoutPanel.Location = new System.Drawing.Point(21, 243);
            this.PitchShifterResetLayoutPanel.Name = "PitchShifterResetLayoutPanel";
            this.PitchShifterResetLayoutPanel.Size = new System.Drawing.Size(290, 55);
            this.PitchShifterResetLayoutPanel.TabIndex = 62;
            // 
            // Effect_PitchShifter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(331, 316);
            this.Controls.Add(this.PitchShifterResetLayoutPanel);
            this.Controls.Add(this.PitchShifterValuesLayoutPanel);
            this.Controls.Add(this.PitchShifterPotsLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(355, 380);
            this.MinimumSize = new System.Drawing.Size(355, 380);
            this.Name = "Effect_PitchShifter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PitchShifter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Effect_PitchShifter_FormClosed);
            this.PitchShifterPotsLayoutPanel.ResumeLayout(false);
            this.PitchShifterValuesLayoutPanel.ResumeLayout(false);
            this.PitchShifterValuesLayoutPanel.PerformLayout();
            this.PitchShifterResetLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Panel PitchShifterResetLayoutPanel;
    }
}