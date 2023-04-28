
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            LogopictureBox = new System.Windows.Forms.PictureBox();
            TableLayoutPanelAbout = new System.Windows.Forms.TableLayoutPanel();
            AboutrichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)LogopictureBox).BeginInit();
            TableLayoutPanelAbout.SuspendLayout();
            SuspendLayout();
            // 
            // LogopictureBox
            // 
            LogopictureBox.BackgroundImage = Properties.Resources.omegaLogo;
            LogopictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            LogopictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            LogopictureBox.InitialImage = null;
            LogopictureBox.Location = new System.Drawing.Point(3, 3);
            LogopictureBox.Name = "LogopictureBox";
            LogopictureBox.Size = new System.Drawing.Size(349, 269);
            LogopictureBox.TabIndex = 2;
            LogopictureBox.TabStop = false;
            // 
            // TableLayoutPanelAbout
            // 
            TableLayoutPanelAbout.ColumnCount = 1;
            TableLayoutPanelAbout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelAbout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            TableLayoutPanelAbout.Controls.Add(AboutrichTextBox, 0, 1);
            TableLayoutPanelAbout.Controls.Add(LogopictureBox, 0, 0);
            TableLayoutPanelAbout.Location = new System.Drawing.Point(7, 6);
            TableLayoutPanelAbout.Margin = new System.Windows.Forms.Padding(2);
            TableLayoutPanelAbout.Name = "TableLayoutPanelAbout";
            TableLayoutPanelAbout.RowCount = 2;
            TableLayoutPanelAbout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            TableLayoutPanelAbout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            TableLayoutPanelAbout.Size = new System.Drawing.Size(355, 393);
            TableLayoutPanelAbout.TabIndex = 7;
            // 
            // AboutrichTextBox
            // 
            AboutrichTextBox.BackColor = System.Drawing.Color.White;
            AboutrichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            AboutrichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            AboutrichTextBox.Font = new System.Drawing.Font("Segoe UI", 8.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            AboutrichTextBox.Location = new System.Drawing.Point(2, 277);
            AboutrichTextBox.Margin = new System.Windows.Forms.Padding(2);
            AboutrichTextBox.MaxLength = 1000;
            AboutrichTextBox.MinimumSize = new System.Drawing.Size(354, 100);
            AboutrichTextBox.Name = "AboutrichTextBox";
            AboutrichTextBox.ReadOnly = true;
            AboutrichTextBox.Size = new System.Drawing.Size(354, 114);
            AboutrichTextBox.TabIndex = 8;
            AboutrichTextBox.Text = resources.GetString("AboutrichTextBox.Text");
            // 
            // AboutDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(372, 411);
            Controls.Add(TableLayoutPanelAbout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(388, 450);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(388, 450);
            Name = "AboutDialog";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "About";
            ((System.ComponentModel.ISupportInitialize)LogopictureBox).EndInit();
            TableLayoutPanelAbout.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.PictureBox LogopictureBox;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanelAbout;
        private System.Windows.Forms.RichTextBox AboutrichTextBox;
    }
}