namespace Yaml_AudioTool_Rebuilt
{
    partial class Effect_Normalize
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
            this.NormalizeValuesLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.NormalizeLabel = new System.Windows.Forms.Label();
            this.SaveNormalizedAudioButton = new System.Windows.Forms.Button();
            this.NormalizeFileButton = new System.Windows.Forms.Button();
            this.NormalizeValuesLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // NormalizeValuesLayoutPanel
            // 
            this.NormalizeValuesLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.NormalizeValuesLayoutPanel.ColumnCount = 1;
            this.NormalizeValuesLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.NormalizeValuesLayoutPanel.Controls.Add(this.NormalizeLabel, 0, 2);
            this.NormalizeValuesLayoutPanel.Controls.Add(this.SaveNormalizedAudioButton, 0, 1);
            this.NormalizeValuesLayoutPanel.Controls.Add(this.NormalizeFileButton, 0, 0);
            this.NormalizeValuesLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.NormalizeValuesLayoutPanel.Name = "NormalizeValuesLayoutPanel";
            this.NormalizeValuesLayoutPanel.RowCount = 3;
            this.NormalizeValuesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.NormalizeValuesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.NormalizeValuesLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.NormalizeValuesLayoutPanel.Size = new System.Drawing.Size(174, 121);
            this.NormalizeValuesLayoutPanel.TabIndex = 61;
            // 
            // NormalizeLabel
            // 
            this.NormalizeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NormalizeLabel.AutoSize = true;
            this.NormalizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NormalizeLabel.Location = new System.Drawing.Point(4, 94);
            this.NormalizeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NormalizeLabel.Name = "NormalizeLabel";
            this.NormalizeLabel.Size = new System.Drawing.Size(79, 13);
            this.NormalizeLabel.TabIndex = 81;
            this.NormalizeLabel.Text = "Normalize Infos";
            this.NormalizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SaveNormalizedAudioButton
            // 
            this.SaveNormalizedAudioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SaveNormalizedAudioButton.Location = new System.Drawing.Point(3, 43);
            this.SaveNormalizedAudioButton.Name = "SaveNormalizedAudioButton";
            this.SaveNormalizedAudioButton.Size = new System.Drawing.Size(168, 30);
            this.SaveNormalizedAudioButton.TabIndex = 80;
            this.SaveNormalizedAudioButton.Text = "Save Normalized Audio";
            this.SaveNormalizedAudioButton.UseVisualStyleBackColor = true;
            this.SaveNormalizedAudioButton.Click += new System.EventHandler(this.SaveNormalizedAudioButton_Click);
            // 
            // NormalizeFileButton
            // 
            this.NormalizeFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NormalizeFileButton.Location = new System.Drawing.Point(3, 3);
            this.NormalizeFileButton.Name = "NormalizeFileButton";
            this.NormalizeFileButton.Size = new System.Drawing.Size(168, 30);
            this.NormalizeFileButton.TabIndex = 79;
            this.NormalizeFileButton.Text = "Normalize Audiofile";
            this.NormalizeFileButton.UseVisualStyleBackColor = true;
            this.NormalizeFileButton.Click += new System.EventHandler(this.NormalizeFileButton_Click);
            // 
            // Effect_Normalize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 151);
            this.Controls.Add(this.NormalizeValuesLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(214, 190);
            this.MinimumSize = new System.Drawing.Size(214, 190);
            this.Name = "Effect_Normalize";
            this.Text = "Normalize";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Effect_Normalize_FormClosed);
            this.NormalizeValuesLayoutPanel.ResumeLayout(false);
            this.NormalizeValuesLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel NormalizeValuesLayoutPanel;
        private System.Windows.Forms.Button NormalizeFileButton;
        private System.Windows.Forms.Button SaveNormalizedAudioButton;
        private System.Windows.Forms.Label NormalizeLabel;
    }
}