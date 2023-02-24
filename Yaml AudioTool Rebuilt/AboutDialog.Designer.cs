
namespace Yaml_AudioTool_Rebuilt
{
    partial class AboutDialog
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
            LogopictureBox = new System.Windows.Forms.PictureBox();
            panel1 = new System.Windows.Forms.Panel();
            AboutrichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)LogopictureBox).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // LogopictureBox
            // 
            LogopictureBox.BackgroundImage = Properties.Resources.omegaLogo;
            LogopictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            LogopictureBox.InitialImage = null;
            LogopictureBox.Location = new System.Drawing.Point(4, 3);
            LogopictureBox.Name = "LogopictureBox";
            LogopictureBox.Size = new System.Drawing.Size(353, 320);
            LogopictureBox.TabIndex = 2;
            LogopictureBox.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(AboutrichTextBox);
            panel1.Controls.Add(LogopictureBox);
            panel1.Location = new System.Drawing.Point(7, 6);
            panel1.Margin = new System.Windows.Forms.Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(368, 416);
            panel1.TabIndex = 6;
            // 
            // AboutrichTextBox
            // 
            AboutrichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            AboutrichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            AboutrichTextBox.Location = new System.Drawing.Point(3, 300);
            AboutrichTextBox.Margin = new System.Windows.Forms.Padding(2);
            AboutrichTextBox.MaximumSize = new System.Drawing.Size(355, 102);
            AboutrichTextBox.MaxLength = 1000;
            AboutrichTextBox.MinimumSize = new System.Drawing.Size(355, 102);
            AboutrichTextBox.Name = "AboutrichTextBox";
            AboutrichTextBox.ReadOnly = true;
            AboutrichTextBox.Size = new System.Drawing.Size(355, 102);
            AboutrichTextBox.TabIndex = 4;
            AboutrichTextBox.Text = "YAML AudioTool Rebuilt\n\nVersion: 0.7 - 230224\n\nNET6 Remake of the Vitei Audio Tool by Alex Miyamoto\nCode: Johannes Wronka\nLogo: Owen Davis";
            // 
            // AboutDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(377, 430);
            Controls.Add(panel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(393, 469);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(393, 469);
            Name = "AboutDialog";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "About";
            ((System.ComponentModel.ISupportInitialize)LogopictureBox).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.PictureBox LogopictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox AboutrichTextBox;
    }
}