using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NAudio.Wave;

namespace Yaml_AudioTool_Rebuilt
{
    /// <summary>
    /// Control for viewing waveforms
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        public Color PenColor { get; set; }
        public float PenWidth { get; set; }

        public void FitToScreen()
        {
            if (WaveStream == null) return;

            int samples = (int)(waveStream.Length / bytesPerSample);
            startPosition = 0;
            SamplesPerPixel = samples / this.Width;
        }

        private Point mousePos, startPos;
        private bool mouseDrag = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                startPos = e.Location;
                mousePos = new Point(-1, -1);
                mouseDrag = true;
                DrawVerticalLine(e.X);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDrag)
            {
                DrawVerticalLine(e.X);
                if (mousePos.X != -1) DrawVerticalLine(mousePos.X);
                mousePos = e.Location;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (mouseDrag && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDrag = false;
                DrawVerticalLine(startPos.X);

                if (mousePos.X == -1) return;
                DrawVerticalLine(mousePos.X);

                int leftSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Min(startPos.X, mousePos.X));
                int rightSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Max(startPos.X, mousePos.X));
                Zoom(leftSample, rightSample);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                FitToScreen();
                DestructiveEffectsEditor formDestructiveEffectsEditor = (DestructiveEffectsEditor)Application.OpenForms["DestructiveEffectseditor"];
                formDestructiveEffectsEditor.WaveviewerhScrollBar.Minimum = 0;
                formDestructiveEffectsEditor.WaveviewerhScrollBar.Maximum = 0;
            }
            base.OnMouseUp(e);
        }

        public void Zoom(int leftSample, int rightSample)
        {
            startPosition = leftSample * bytesPerSample;
            SamplesPerPixel = (rightSample - leftSample) / this.Width;
            DestructiveEffectsEditor formDestructiveEffectsEditor = (DestructiveEffectsEditor)Application.OpenForms["DestructiveEffectseditor"];
            formDestructiveEffectsEditor.WaveviewerhScrollBar.Minimum = formDestructiveEffectsEditor.customWaveViewer1.HorizontalScroll.Minimum;
            formDestructiveEffectsEditor.WaveviewerhScrollBar.Maximum = formDestructiveEffectsEditor.customWaveViewer1.HorizontalScroll.Maximum;
        }

        private void DrawVerticalLine(int x)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, Height)), Color.Black);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            FitToScreen();
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private WaveStream waveStream;
        private int samplesPerPixel = 128;
        private long startPosition;
        private int bytesPerSample;
        /// <summary>
        /// Creates a new WaveViewer control
        /// </summary>
        public CustomWaveViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.DoubleBuffered = true;

            this.PenColor = Color.DodgerBlue;
            this.PenWidth = 1;

        }

        /// <summary>
        /// sets the associated wavestream
        /// </summary>
        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                waveStream = value;
                if (waveStream != null)
                {
                    bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// The zoom level, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                samplesPerPixel = Math.Max(1, value);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Start position (currently in bytes)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = value;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (waveStream != null)
            {
                waveStream.Position = 0;
                int bytesRead;
                if (waveStream.WaveFormat.BitsPerSample == 24)
                {

                }
                byte[] waveData = new byte[samplesPerPixel * bytesPerSample];
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);

                using (Pen linePen = new Pen(PenColor, PenWidth))
                {
                    for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                    {
                        short lowL = 0;
                        short highL = 0;
                        short lowR = 0;
                        short highR = 0;
                        bytesRead = waveStream.Read(waveData, 0, samplesPerPixel * bytesPerSample);
                        if (bytesRead == 0)
                            break;
                        // Draw Mono
                        if (waveStream.WaveFormat.Channels == 1)
                        {
                            for (int n = 0; n < bytesRead; n += 2)
                            {
                                short sample = BitConverter.ToInt16(waveData, n);
                                if (sample < lowL) lowL = sample;
                                if (sample > highL) highL = sample;
                            }
                            float lowPercent = ((((float)lowL) - short.MinValue) / ushort.MaxValue);
                            float highPercent = ((((float)highL) - short.MinValue) / ushort.MaxValue);
                            DestructiveEffectsEditor formDestructiveEffectsEditor = (DestructiveEffectsEditor)Application.OpenForms["DestructiveEffectseditor"];
                            formDestructiveEffectsEditor.MonoChannelLabel.Visible = true;
                            formDestructiveEffectsEditor.LeftChannelLabel.Visible = false;
                            formDestructiveEffectsEditor.RightChannelLabel.Visible = false;
                            e.Graphics.DrawLine(linePen, x, this.Height * lowPercent, x, this.Height * highPercent);
                        }
                        // Draw Stereo
                        else if (waveStream.WaveFormat.Channels == 2)
                        {
                            for (int n = 0; n < bytesRead; n += 4)
                            {
                                short sampleL = BitConverter.ToInt16(waveData, n);
                                short sampleR = BitConverter.ToInt16(waveData, n+2);
                                if (sampleL < lowL) lowL = sampleL;
                                if (sampleL > highL) highL = sampleL;
                                if (sampleR < lowR) lowR = sampleR;
                                if (sampleR > highR) highR = sampleR;
                            }
                            float lowPercentL = ((((float)lowL) - short.MinValue) / ushort.MaxValue);
                            float highPercentL = ((((float)highL) - short.MinValue) / ushort.MaxValue);
                            float lowPercentR = ((((float)lowR) - short.MinValue) / ushort.MaxValue);
                            float highPercentR = ((((float)highR) - short.MinValue) / ushort.MaxValue);
                            DestructiveEffectsEditor formDestructiveEffectsEditor = (DestructiveEffectsEditor)Application.OpenForms["DestructiveEffectseditor"];
                            formDestructiveEffectsEditor.MonoChannelLabel.Visible = false;
                            formDestructiveEffectsEditor.LeftChannelLabel.Visible = true;
                            formDestructiveEffectsEditor.RightChannelLabel.Visible = true;
                            Pen dividerPen = new Pen(Color.Black);
                            e.Graphics.DrawLine(dividerPen, 0, this.Height / 2, this.Width, this.Height / 2);
                            e.Graphics.DrawLine(linePen, x, this.Height / 2 * lowPercentL, x, this.Height / 2 * highPercentL);                            
                            e.Graphics.DrawLine(linePen, x, this.Height - this.Height / 2 * lowPercentR, x, this.Height - this.Height / 2 * highPercentR);
                        }
                    }
                }
            }

            base.OnPaint(e);
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}