namespace Yaml_AudioTool_Rebuilt
{
    partial class SettingsDialog
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
            this.audiofoldergroupBox = new System.Windows.Forms.GroupBox();
            this.audiofolderLabel = new System.Windows.Forms.Label();
            this.audiofoldersetButton = new System.Windows.Forms.Button();
            this.audiofoldergroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // audiofoldergroupBox
            // 
            this.audiofoldergroupBox.AutoSize = true;
            this.audiofoldergroupBox.Controls.Add(this.audiofolderLabel);
            this.audiofoldergroupBox.Controls.Add(this.audiofoldersetButton);
            this.audiofoldergroupBox.Location = new System.Drawing.Point(14, 15);
            this.audiofoldergroupBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.audiofoldergroupBox.Name = "audiofoldergroupBox";
            this.audiofoldergroupBox.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.audiofoldergroupBox.Size = new System.Drawing.Size(654, 142);
            this.audiofoldergroupBox.TabIndex = 0;
            this.audiofoldergroupBox.TabStop = false;
            this.audiofoldergroupBox.Text = "Audio Folder Path";
            // 
            // audiofolderLabel
            // 
            this.audiofolderLabel.AutoSize = true;
            this.audiofolderLabel.Location = new System.Drawing.Point(118, 58);
            this.audiofolderLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.audiofolderLabel.Name = "audiofolderLabel";
            this.audiofolderLabel.Size = new System.Drawing.Size(72, 30);
            this.audiofolderLabel.TabIndex = 1;
            this.audiofolderLabel.Text = "NONE";
            // 
            // audiofoldersetButton
            // 
            this.audiofoldersetButton.Location = new System.Drawing.Point(10, 44);
            this.audiofoldersetButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.audiofoldersetButton.Name = "audiofoldersetButton";
            this.audiofoldersetButton.Size = new System.Drawing.Size(98, 58);
            this.audiofoldersetButton.TabIndex = 0;
            this.audiofoldersetButton.Text = "Set";
            this.audiofoldersetButton.UseVisualStyleBackColor = true;
            this.audiofoldersetButton.Click += new System.EventHandler(this.audiofoldersetButton_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(682, 171);
            this.Controls.Add(this.audiofoldergroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(706, 235);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(706, 235);
            this.Name = "SettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.audiofoldergroupBox.ResumeLayout(false);
            this.audiofoldergroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox audiofoldergroupBox;
        public System.Windows.Forms.Label audiofolderLabel;
        public System.Windows.Forms.Button audiofoldersetButton;
    }
}