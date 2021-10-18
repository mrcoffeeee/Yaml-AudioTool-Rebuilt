
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
            this.LogopictureBox.Location = new System.Drawing.Point(3, 3);
            this.LogopictureBox.Name = "LogopictureBox";
            this.LogopictureBox.Size = new System.Drawing.Size(362, 322);
            this.LogopictureBox.TabIndex = 2;
            this.LogopictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AboutrichTextBox);
            this.panel1.Controls.Add(this.LogopictureBox);
            this.panel1.Location = new System.Drawing.Point(7, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(368, 443);
            this.panel1.TabIndex = 6;
            // 
            // AboutrichTextBox
            // 
            this.AboutrichTextBox.Font = new System.Drawing.Font("Segoe UI", 9.85F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.AboutrichTextBox.Location = new System.Drawing.Point(4, 300);
            this.AboutrichTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.AboutrichTextBox.Name = "AboutrichTextBox";
            this.AboutrichTextBox.ReadOnly = true;
            this.AboutrichTextBox.Size = new System.Drawing.Size(363, 144);
            this.AboutrichTextBox.TabIndex = 4;
            this.AboutrichTextBox.Text = "\t\t\t\n\t\tYAML AudioTool Rebuilt\n\nVersion: 0.3 - 211018\n\nNET5 Remake of the Vitei Aud" +
    "io Tool by Alex Miyamoto\nCode: Johannes Wronka\nLogo: Owen Davis";
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(390, 470);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(406, 509);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(406, 509);
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