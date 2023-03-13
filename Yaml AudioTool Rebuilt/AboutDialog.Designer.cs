
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
            LogopictureBox.Location = new System.Drawing.Point(5, 6);
            LogopictureBox.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            LogopictureBox.Name = "LogopictureBox";
            LogopictureBox.Size = new System.Drawing.Size(599, 538);
            LogopictureBox.TabIndex = 2;
            LogopictureBox.TabStop = false;
            // 
            // TableLayoutPanelAbout
            // 
            TableLayoutPanelAbout.ColumnCount = 1;
            TableLayoutPanelAbout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            TableLayoutPanelAbout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            TableLayoutPanelAbout.Controls.Add(AboutrichTextBox, 0, 1);
            TableLayoutPanelAbout.Controls.Add(LogopictureBox, 0, 0);
            TableLayoutPanelAbout.Location = new System.Drawing.Point(12, 12);
            TableLayoutPanelAbout.Name = "TableLayoutPanelAbout";
            TableLayoutPanelAbout.RowCount = 2;
            TableLayoutPanelAbout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            TableLayoutPanelAbout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            TableLayoutPanelAbout.Size = new System.Drawing.Size(609, 786);
            TableLayoutPanelAbout.TabIndex = 7;
            // 
            // AboutrichTextBox
            // 
            AboutrichTextBox.BackColor = System.Drawing.Color.White;
            AboutrichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            AboutrichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            AboutrichTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            AboutrichTextBox.Location = new System.Drawing.Point(3, 554);
            AboutrichTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            AboutrichTextBox.MaxLength = 1000;
            AboutrichTextBox.MinimumSize = new System.Drawing.Size(606, 200);
            AboutrichTextBox.Name = "AboutrichTextBox";
            AboutrichTextBox.ReadOnly = true;
            AboutrichTextBox.Size = new System.Drawing.Size(606, 228);
            AboutrichTextBox.TabIndex = 8;
            AboutrichTextBox.Text = resources.GetString("AboutrichTextBox.Text");
            // 
            // AboutDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(633, 810);
            Controls.Add(TableLayoutPanelAbout);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(657, 874);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(657, 874);
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