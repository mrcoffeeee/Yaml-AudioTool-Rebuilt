
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
            this.LogopictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AboutrichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.LogopictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogopictureBox
            // 
            this.LogopictureBox.BackgroundImage = global::Yaml_AudioTool_Rebuilt.Properties.Resources.omegaLogo;
            this.LogopictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.LogopictureBox.InitialImage = null;
            this.LogopictureBox.Location = new System.Drawing.Point(5, 6);
            this.LogopictureBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.LogopictureBox.Name = "LogopictureBox";
            this.LogopictureBox.Size = new System.Drawing.Size(621, 644);
            this.LogopictureBox.TabIndex = 2;
            this.LogopictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AboutrichTextBox);
            this.panel1.Controls.Add(this.LogopictureBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(631, 886);
            this.panel1.TabIndex = 6;
            // 
            // AboutrichTextBox
            // 
            this.AboutrichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AboutrichTextBox.Font = new System.Drawing.Font("Segoe UI", 9.85F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AboutrichTextBox.Location = new System.Drawing.Point(7, 600);
            this.AboutrichTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AboutrichTextBox.Name = "AboutrichTextBox";
            this.AboutrichTextBox.ReadOnly = true;
            this.AboutrichTextBox.Size = new System.Drawing.Size(619, 284);
            this.AboutrichTextBox.TabIndex = 4;
            this.AboutrichTextBox.Text = "\t\t\t\n\t\tYAML AudioTool Rebuilt\n\nVersion: 0.4 - 220308\n\nNET6 Remake of the Vitei Aud" +
    "io Tool by Alex Miyamoto\nCode: Johannes Wronka\nLogo: Owen Davis";
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(655, 906);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(679, 970);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(679, 970);
            this.Name = "AboutDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.LogopictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox LogopictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox AboutrichTextBox;
    }
}