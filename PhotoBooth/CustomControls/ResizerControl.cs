using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhotoBooth.CustomControls
{
    public partial class ResizerControl : Control
    {
        private Bitmap _imgResizer;
        private bool blResize = false;
        private bool blDrag = false;
        Rectangle rect;
        private int xPos;
        private int yPos;
        private bool blResizerOrDragMode = false;
        public ResizerControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            InitializeComponent();
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ResizerControl_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResizerControl_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ResizerControl_MouseUp);
            this.Click += new EventHandler(this.ResizerControl_Click);
            this.Leave += new EventHandler(this.ResizerControl_Leave);
            this.BackColor = Color.Transparent;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                return cp;
            }
        }
        public Bitmap ResizerImage
        {
            get { return _imgResizer; }
            set
            {
                _imgResizer = value;
                this.Size = new Size(this.ClientRectangle.Width + 1, this.ClientRectangle.Height + 1);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (_imgResizer != null)
            {
                Graphics g = pe.Graphics;
                rect = new Rectangle(this.ClientRectangle.X + 5, this.ClientRectangle.Y + 5, this.ClientRectangle.Width - 10, this.ClientRectangle.Height - 10);
                g.DrawImage(_imgResizer, rect);
                if (blResizerOrDragMode)
                {
                    Pen drawLine = new Pen(Color.Black);
                    drawLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawRectangle(drawLine, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                    g.FillRectangle(Brushes.Black, this.ClientRectangle.X + this.ClientRectangle.Width - 10, this.ClientRectangle.Y + this.ClientRectangle.Height - 10, 10, 10);
                }
                else
                {
                }
            }
            base.OnPaint(pe);
        }
        private void ResizerControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            #region X=0 && X=10
            if (blResizerOrDragMode)
            {
                if (e.X >= this.ClientRectangle.X + this.ClientRectangle.Width - 10 && e.X <= (this.ClientRectangle.X + this.ClientRectangle.Width) && e.Y >= this.ClientRectangle.Y + this.ClientRectangle.Height - 10 && e.Y <= this.ClientRectangle.Y + this.ClientRectangle.Height)
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
            #endregion
                else
                {
                    this.Cursor = Cursors.Arrow;
                }
                if (blResize)
                {
                    this.Size = new Size(e.X, e.Y);
                    Invalidate();
                }
                else if (blDrag)
                {
                    this.Top += (e.Y - yPos);
                    this.Left += (e.X - xPos);
                    Invalidate();
                }
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void ResizerControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (blResizerOrDragMode)
            {

                if (e.X >= this.ClientRectangle.X + this.ClientRectangle.Width - 10 && e.X <= (this.ClientRectangle.X + this.ClientRectangle.Width) && e.Y >= this.ClientRectangle.Y + this.ClientRectangle.Height - 10 && e.Y <= this.ClientRectangle.Y + this.ClientRectangle.Height)
                {
                    this.Cursor = Cursors.SizeNWSE;
                    blResize = true;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    blDrag = true;
                    xPos = e.X;
                    yPos = e.Y;
                }
            }
            else
            {
            }
        }
        private void ResizerControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (blResize)
            {
                blResize = false;
            }
            if (blDrag)
            {
                blDrag = false;
                this.Size = new Size(this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
                Invalidate();
            }

        }
        private void ResizerControl_Click(object sender, EventArgs e)
        {
            blResizerOrDragMode = true;
            this.Focus();
            Invalidate();
        }
        private void ResizerControl_Leave(object sender, EventArgs e)
        {
            blResizerOrDragMode = false;
            Invalidate();
        }
        protected override void OnBackColorChanged(EventArgs e)
        {
            if (this.Parent != null)
            {
                Parent.Invalidate(this.Bounds, true);
            }
            base.OnBackColorChanged(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnParentBackColorChanged(e);
        }

        public ResizerControl Clear()
        {
            ResizerImage = PhotoBooth.Properties.Resources.tranparent;
            return this;
        }
    }
}

