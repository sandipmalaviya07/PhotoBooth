using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PhotoBooth.CustomControls
{
    public partial class SliderControl : Control
    {
        private Pen blackPen = new Pen(Color.Transparent, 0);
        private int width, height = 5;
        private int min = 0, max = 100;
        private Bitmap circle;
        private int dx;
        private bool trackMode = false;
        private bool _textVisible = true;
        private int _value = 0;
        private Color _fillSlidingColor = Color.Black;
        private Color _fontSliding = Color.Black;
        private Color _fillInnerColor = Color.White;
        System.Drawing.Font _fontSlidingFamily = new System.Drawing.Font("Arial", 10);
        private int _valueMin = 0;
        private bool _displayToolTip = false;
        // values changed event
        // Min property
        [DefaultValue(0)]
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                min = (int)((float)(this.ClientRectangle.Width - 20) * ((float)(_value - _valueMin) / (float)(max - _valueMin)));
                Invalidate();
            }
        }
        [DefaultValue(0)]
        public int Min
        {
            get { return _valueMin; }
            set
            {
                _valueMin = value;
                Invalidate();
            }
        }
        [DefaultValue(100)]
        public int Max
        {
            get { return max; }
            set
            {
                max = value;
                Invalidate();
            }
        }
        [DefaultValue(true)]
        public bool TextVisible
        {
            get { return _textVisible; }
            set
            {
                _textVisible = value;
                Invalidate();
            }
        }
        [DefaultValue(typeof(Color), "black")]
        public Color FillSlidingColor
        {
            get { return _fillSlidingColor; }
            set
            {
                _fillSlidingColor = value;
                Invalidate();
            }
        }
        [DefaultValue(typeof(Color), "black")]
        public Color FontSlidingColor
        {
            get { return _fontSliding; }
            set
            {
                _fontSliding = value;
                Invalidate();
            }
        }
        [DefaultValue(typeof(System.Drawing.Font), "Arial")]
        public System.Drawing.Font FontSlidingFamily
        {
            get { return _fontSlidingFamily; }
            set
            {
                _fontSlidingFamily = value;
                Invalidate();
            }
        }
        [DefaultValue(typeof(Color), "black")]
        public Color FillInnerColor
        {
            get { return _fillInnerColor; }
            set
            {
                _fillInnerColor = value;
                Invalidate();
            }
        }
        [DefaultValue(typeof(Bitmap), "")]
        public Bitmap SlidingImage
        {
            get { return circle; }
            set
            {
                circle = value;
                Invalidate();
            }
        }
        public event EventHandler ValuesChanged;
        public SliderControl()
        {
            InitializeComponent();
            Image img = Base64ToImage();
            circle = new Bitmap(img);
            circle.MakeTransparent(Color.FromArgb(255, 255, 255));
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            Rectangle rc = this.ClientRectangle;
            width = rc.Width - 20;
            int x = 8;
            int y = 5;
            g.DrawRectangle(blackPen, x - 1, y + 2, rc.Width, height);
            g.FillRectangle(new SolidBrush(_fillInnerColor), x, y + 2, rc.Width - 15, height);
            g.FillRectangle(new SolidBrush(_fillSlidingColor), x, y + 2, min, height);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            _value = (int)Math.Min((min * ((float)(max - _valueMin) / (float)width)) + _valueMin, max);
            if (_textVisible && _displayToolTip)
            {
                if (width - 30 < min)
                {
                    g.DrawString(_value.ToString(), _fontSlidingFamily, new SolidBrush(_fontSliding), x + min - 30, y + 5, drawFormat);
                }
                else
                {
                    g.DrawString(_value.ToString(), _fontSlidingFamily, new SolidBrush(_fontSliding), x + min + 10, y + 5, drawFormat);
                }
                Console.Write(min + "/");

            }
            g.DrawImage(circle, x + min - 7, y - 5, 20, 20);
            base.OnPaint(pe);
        }
        private void SliderControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (trackMode)
            {
                // release capture
                this.Capture = false;
                trackMode = false;
                _displayToolTip = false;
                Invalidate();
                if (ValuesChanged != null)
                    ValuesChanged(this, new EventArgs());
            }
        }
        private void SliderControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (trackMode)
            {
                min = e.X - 5;
                min = Math.Max(min, 0);
                min = Math.Min(min, ClientRectangle.Width - 20);
                _displayToolTip = true;
                Invalidate();

            }

        }
        private void SliderControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int x = (ClientRectangle.Right - width) / 2 - 4;
            int y = 0;
            if ((e.Y >= y) && (e.Y < y + 20))
            {
                if ((e.X >= x + min - 25))
                {
                    dx = e.X;
                    trackMode = true;
                }
            }

        }
        private void SliderControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //_displayToolTip = true;
            //if (e.Delta > 0)
            //{
            //    min = min + 1;
            //    min = Math.Min(min, ClientRectangle.Width - 50);
            //}
            //else
            //{
            //    if (_valueMin < _valueMin + min)
            //        min = min - 1;
            //}
            //Invalidate();
            //if (ValuesChanged != null)
            //    ValuesChanged(this, new EventArgs());
        }
        public Image Base64ToImage()
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String("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");
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}
