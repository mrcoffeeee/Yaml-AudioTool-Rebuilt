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
            this.audiofoldergroupBox.Controls.Add(this.audiofolderLabel);
            this.audiofoldergroupBox.Controls.Add(this.audiofoldersetButton);
            this.audiofoldergroupBox.Location = new System.Drawing.Point(12, 12);
            this.audiofoldergroupBox.Name = "audiofoldergroupBox";
            this.audiofoldergroupBox.Size = new System.Drawing.Size(382, 57);
            this.audiofoldergroupBox.TabIndex = 0;
            this.audiofoldergroupBox.TabStop = false;
            this.audiofoldergroupBox.Text = "Audio Folder Path";
            // 
            // audiofolderLabel
            // 
            this.audiofolderLabel.AutoSize = true;
            this.audiofolderLabel.Location = new System.Drawing.Point(69, 29);
            this.audiofolderLabel.Name = "audiofolderLabel";
            this.audiofolderLabel.Size = new System.Drawing.Size(40, 15);
            this.audiofolderLabel.TabIndex = 1;
            this.audiofolderLabel.Text = "NONE";
            // 
            // audiofoldersetButton
            // 
            this.audiofoldersetButton.Location = new System.Drawing.Point(6, 22);
            this.audiofoldersetButton.Name = "audiofoldersetButton";
            this.audiofoldersetButton.Size = new System.Drawing.Size(57, 29);
            this.audiofoldersetButton.TabIndex = 0;
            this.audiofoldersetButton.Text = "Set";
            this.audiofoldersetButton.UseVisualStyleBackColor = true;
            this.audiofoldersetButton.Click += new System.EventHandler(this.audiofoldersetButton_Click);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 83);
            this.Controls.Add(this.audiofoldergroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(422, 122);
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.audiofoldergroupBox.ResumeLayout(false);
            this.audiofoldergroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox audiofoldergroupBox;
        public System.Windows.Forms.Label audiofolderLabel;
        public System.Windows.Forms.Button audiofoldersetButton;
    }
}