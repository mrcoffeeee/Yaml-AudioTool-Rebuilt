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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            audiofoldergroupBox = new System.Windows.Forms.GroupBox();
            audiofolderLabel = new System.Windows.Forms.Label();
            audiofoldersetButton = new System.Windows.Forms.Button();
            audiofoldergroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // audiofoldergroupBox
            // 
            audiofoldergroupBox.AutoSize = true;
            audiofoldergroupBox.Controls.Add(audiofolderLabel);
            audiofoldergroupBox.Controls.Add(audiofoldersetButton);
            audiofoldergroupBox.Location = new System.Drawing.Point(12, 12);
            audiofoldergroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            audiofoldergroupBox.Name = "audiofoldergroupBox";
            audiofoldergroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            audiofoldergroupBox.Size = new System.Drawing.Size(545, 119);
            audiofoldergroupBox.TabIndex = 0;
            audiofoldergroupBox.TabStop = false;
            audiofoldergroupBox.Text = "Audio Folder Path";
            // 
            // audiofolderLabel
            // 
            audiofolderLabel.AutoSize = true;
            audiofolderLabel.Location = new System.Drawing.Point(98, 48);
            audiofolderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            audiofolderLabel.Name = "audiofolderLabel";
            audiofolderLabel.Size = new System.Drawing.Size(61, 25);
            audiofolderLabel.TabIndex = 1;
            audiofolderLabel.Text = "NONE";
            // 
            // audiofoldersetButton
            // 
            audiofoldersetButton.Location = new System.Drawing.Point(8, 37);
            audiofoldersetButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            audiofoldersetButton.Name = "audiofoldersetButton";
            audiofoldersetButton.Size = new System.Drawing.Size(82, 48);
            audiofoldersetButton.TabIndex = 0;
            audiofoldersetButton.Text = "Set";
            audiofoldersetButton.UseVisualStyleBackColor = true;
            audiofoldersetButton.Click += audiofoldersetButton_Click;
            // 
            // SettingsDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(570, 149);
            Controls.Add(audiofoldergroupBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(592, 205);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(592, 205);
            Name = "SettingsDialog";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Settings";
            audiofoldergroupBox.ResumeLayout(false);
            audiofoldergroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox audiofoldergroupBox;
        public System.Windows.Forms.Label audiofolderLabel;
        public System.Windows.Forms.Button audiofoldersetButton;
    }
}