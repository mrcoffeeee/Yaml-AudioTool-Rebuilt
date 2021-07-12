
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.aboutinfotextBox = new System.Windows.Forms.TextBox();
            this.abouttitletextBox = new System.Windows.Forms.TextBox();
            this.aboutauthortextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Yaml_AudioTool_Rebuilt.Properties.Resources.omegaLogo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(385, 345);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // aboutinfotextBox
            // 
            this.aboutinfotextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.aboutinfotextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.aboutinfotextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.aboutinfotextBox.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.aboutinfotextBox.Location = new System.Drawing.Point(13, 368);
            this.aboutinfotextBox.Name = "aboutinfotextBox";
            this.aboutinfotextBox.ReadOnly = true;
            this.aboutinfotextBox.Size = new System.Drawing.Size(360, 18);
            this.aboutinfotextBox.TabIndex = 3;
            this.aboutinfotextBox.Text = "YAML AudioTool Rebuilt v0.2 for";
            this.aboutinfotextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // abouttitletextBox
            // 
            this.abouttitletextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.abouttitletextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.abouttitletextBox.Font = new System.Drawing.Font("Segoe UI Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.abouttitletextBox.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.abouttitletextBox.Location = new System.Drawing.Point(13, 400);
            this.abouttitletextBox.Name = "abouttitletextBox";
            this.abouttitletextBox.Size = new System.Drawing.Size(360, 29);
            this.abouttitletextBox.TabIndex = 4;
            this.abouttitletextBox.Text = "Wing Commander IV - Remastered";
            this.abouttitletextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // aboutauthortextBox
            // 
            this.aboutauthortextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.aboutauthortextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.aboutauthortextBox.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.aboutauthortextBox.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.aboutauthortextBox.Location = new System.Drawing.Point(13, 442);
            this.aboutauthortextBox.Name = "aboutauthortextBox";
            this.aboutauthortextBox.Size = new System.Drawing.Size(359, 16);
            this.aboutauthortextBox.TabIndex = 5;
            this.aboutauthortextBox.Text = "2021: mrcoffeeee";
            this.aboutauthortextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(390, 480);
            this.Controls.Add(this.aboutauthortextBox);
            this.Controls.Add(this.abouttitletextBox);
            this.Controls.Add(this.aboutinfotextBox);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(406, 519);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(406, 519);
            this.Name = "AboutDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox aboutinfotextBox;
        private System.Windows.Forms.TextBox abouttitletextBox;
        private System.Windows.Forms.TextBox aboutauthortextBox;
    }
}