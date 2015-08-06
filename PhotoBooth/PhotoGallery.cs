using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;
using AForge.Imaging.Filters;
namespace PhotoBooth
{
    public partial class PhotoGallery : Form
    {
        public static Bitmap img = null;
        private BrightnessCorrection filter = new BrightnessCorrection();
        private ContrastCorrection filtercontrast = new ContrastCorrection();
        private string ImageFilePath = null;
        Boolean bHaveMouse;
        Point ptOriginal = new Point();
        Point ptLast = new Point();
        Rectangle rectCropArea;
        Image srcImage = null;
        Point positionPanel = new Point(340, 530);
        public PhotoGallery()
        {
            InitializeComponent();
            bHaveMouse = false;
        }

        private void btnBrowerImage_Click(object sender, EventArgs e)
        {
            DialogResult result = openBrownImage.ShowDialog();
            if (result == DialogResult.OK)
            {
                ImageFilePath = openBrownImage.FileName;
                panelImage.BackgroundImage = Image.FromFile(ImageFilePath);
                img = new Bitmap(Image.FromFile(openBrownImage.FileName));
                srcImage = img;
                Effect();
            }
        }

        private void btnProps_Click(object sender, EventArgs e)
        {
            HidePanel();
            panelEffect.Location = positionPanel;
            panelEffect.Visible = true;
        }

        private void btnEffect_Click(object sender, EventArgs e)
        {
            HidePanel();
            btnMuchh.Location = positionPanel;
            btnMuchh.HorizontalScroll.Visible = true;
            btnMuchh.Visible = true;
        }
        public void HidePanel()
        {
            panelEffect.Visible = false;
            btnMuchh.Visible = false;
            panelEffect3.Visible = false;
            panelEffect4.Visible = false;
            panelMenu.Visible = false;
            bHaveMouse = false;
            btnCrop.Enabled = false;
            panelImage.Refresh();
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            HidePanel();
            panelEffect3.Location = positionPanel;
            panelEffect3.HorizontalScroll.Visible = true;
            panelEffect3.Visible = true;
        }

        private void btnGallery_Click(object sender, EventArgs e)
        {
            HidePanel();
            panelEffect4.Location = positionPanel;
            panelEffect4.HorizontalScroll.Visible = true;
            panelEffect4.Visible = true;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            panelMenu.Visible = true;
        }

        private void PhotoGallery_Click(object sender, EventArgs e)
        {
            HidePanel();
        }

        private void sliderControl1_ValuesChanged(object sender, EventArgs e)
        {
            if (img != null)
            {
                var value = double.Parse(Convert.ToString(sliderControl1.Value)) * Convert.ToDouble((double)5 / (double)100);
                filtercontrast.Factor = value;
                panelImage.BackgroundImage = filtercontrast.Apply(img);
            }
        }

        private void sliderControl2_ValuesChanged(object sender, EventArgs e)
        {
            if (img != null)
            {
                var value = double.Parse(Convert.ToString(sliderControl2.Value));
                if (value < 50)
                {
                    value = (value - 50) * 0.02;
                }
                else
                {
                    value = (value - 50) * 0.02;
                }
                filter.AdjustValue = value;
                panelImage.BackgroundImage = filter.Apply(img);
            }
        }

        #region "panelImage"
        private void panelImage_MouseDown(object sender, MouseEventArgs e)
        {
            bHaveMouse = true;
            ptOriginal.X = e.X;
            ptOriginal.Y = e.Y;
            ptLast.X = -1;
            ptLast.Y = -1;
            rectCropArea = new Rectangle(new Point(e.X, e.Y), new Size());
        }

        private void panelImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (img != null)
            {
                Point ptCurrent = new Point(e.X, e.Y);
                if (bHaveMouse)
                {
                    int width = srcImage.Width - 4;
                    int height = srcImage.Height - 2;
                    ptLast = ptCurrent;
                    if (e.X > ptOriginal.X && e.Y > ptOriginal.Y)
                    {
                        rectCropArea.Width = Math.Min(e.X, width) - ptOriginal.X;
                        rectCropArea.Height = Math.Min(e.Y, height) - ptOriginal.Y;

                    }
                    else if (e.X < ptOriginal.X && e.Y > ptOriginal.Y)
                    {
                        rectCropArea.Width = ptOriginal.X - Math.Max(e.X, 0);
                        rectCropArea.Height = Math.Min(e.Y, height) - ptOriginal.Y;
                        rectCropArea.X = Math.Max(e.X, 0);
                        rectCropArea.Y = ptOriginal.Y;
                    }
                    else if (e.X > ptOriginal.X && e.Y < ptOriginal.Y)
                    {
                        rectCropArea.Width = Math.Min(e.X, width) - ptOriginal.X;
                        rectCropArea.Height = ptOriginal.Y - Math.Max(e.Y, 0);
                        rectCropArea.X = ptOriginal.X;
                    }
                    else
                    {
                        rectCropArea.Width = ptOriginal.X - Math.Max(e.X, 0);
                        rectCropArea.Height = ptOriginal.Y - Math.Max(e.Y, 0);
                        rectCropArea.X = Math.Max(e.X, 0);
                        rectCropArea.Y = Math.Max(e.Y, 0);

                    }
                    panelImage.Refresh();
                }
            }
        }

        private void panelImage_MouseUp(object sender, MouseEventArgs e)
        {
            bHaveMouse = false;
            if (ptLast.X != -1)
            {
                Point ptCurrent = new Point(e.X, e.Y);
            }
            ptLast.X = -1;
            ptLast.Y = -1;
            ptOriginal.X = -1;
            ptOriginal.Y = -1;
            btnCrop.Enabled = true;

        }
        private void panelImage_Paint(object sender, PaintEventArgs e)
        {

            if (bHaveMouse)
            {
                Pen drawLine = new Pen(Color.Black);
                drawLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                e.Graphics.DrawRectangle(drawLine, rectCropArea);
                SolidBrush _color = new SolidBrush(Color.FromArgb(80, Color.Black));
                //e.Graphics.DrawLine(drawLine, rectCropArea.X, 0, rectCropArea.X, srcImage.Height);
                e.Graphics.FillRectangle(_color, 0, 0, rectCropArea.X, srcImage.Height);
                e.Graphics.FillRectangle(_color, rectCropArea.X, 0, srcImage.Width, rectCropArea.Y);
                e.Graphics.FillRectangle(_color, rectCropArea.X + rectCropArea.Width, rectCropArea.Y, srcImage.Width - (rectCropArea.Width + rectCropArea.X), srcImage.Height);
                e.Graphics.FillRectangle(_color, rectCropArea.X, rectCropArea.Y + rectCropArea.Height, rectCropArea.Width, srcImage.Height - (rectCropArea.Height + rectCropArea.Y));
            }
        }
        #endregion

        private void button28_Click(object sender, EventArgs e)
        {
            panelMenu.Visible = false;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ImageFilePath))
            {
                panelImage.BackgroundImage = Image.FromFile(ImageFilePath);
                img = new Bitmap(Image.FromFile(openBrownImage.FileName));

            }
            panelMenu.Visible = false;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            IFilter imgeFilter = new ConservativeSmoothing();
            img = imgeFilter.Apply(img);
            panelImage.BackgroundImage = img;
            panelMenu.Visible = false;
        }
        private void Effect()
        {
            IFilter imgeFilter = new BurkesDithering();
            Bitmap bitimg = new Bitmap(imgeFilter.Apply(img), 90, 111);
            btnCommon.Image = bitimg;
            imgeFilter = new GrayscaleBT709();
            btnGray.Image = new Bitmap(imgeFilter.Apply(img), 90, 111);
            imgeFilter = new Sepia();
            bitimg = new Bitmap(imgeFilter.Apply(img), 90, 111);
            btnSeperia.Image = bitimg;
            imgeFilter = new Invert();
            bitimg = new Bitmap(imgeFilter.Apply(img), 90, 111);
            btnInvert.Image = bitimg;
            imgeFilter = new Blur();
            bitimg = new Bitmap(imgeFilter.Apply(img), 90, 111);
            btnBlur.Image = bitimg;
            imgeFilter = new Texturer(new AForge.Imaging.Textures.MarbleTexture(10, 11), 0.7f, 0.3f);
            bitimg = new Bitmap(imgeFilter.Apply(img), 90, 111);
            btnJitter.Image = bitimg;
            imgeFilter = new ChannelFiltering(new IntRange(0, 0), new IntRange(0, 255), new IntRange(0, 255));
            bitimg = new Bitmap(imgeFilter.Apply(img), 90, 111);
            btnCyan.Image = bitimg;
            imgeFilter = new YCbCrExtractChannel(AForge.Imaging.YCbCr.CrIndex);
            bitimg = new Bitmap(imgeFilter.Apply(img), 90, 111);
            btnBlackWhite.Image = bitimg;
        }

        private void btnEffectApply_Click(object sender, EventArgs e)
        {
            resizerControl.Visible = true;
            IFilter imgeFilter = default(IFilter);
            Button effect = (Button)sender;
            Bitmap imgEffect = img;
            if (effect.Name == "btnGray")
            {
                imgeFilter = new GrayscaleBT709();
            }
            else if (effect.Name == "btnSeperia")
            {
                imgeFilter = new Sepia();
            }
            else if (effect.Name == "btnInvert")
            {
                imgeFilter = new Invert();
            }
            else if (effect.Name == "btnCommon")
            {
                imgeFilter = new BurkesDithering();
            }
            else if (effect.Name == "btnBlur")
            {
                imgeFilter = new Blur();
            }
            else if (effect.Name == "btnJitter")
            {
                imgeFilter = new Texturer(new AForge.Imaging.Textures.MarbleTexture(10, 11), 0.7f, 0.3f);
            }
            else if (effect.Name == "btnCyan")
            {
                imgeFilter = new ChannelFiltering(new IntRange(0, 0), new IntRange(0, 255), new IntRange(0, 255));
            }
            else if (effect.Name == "btnBlackWhite")
            {
                imgeFilter = new YCbCrExtractChannel(AForge.Imaging.YCbCr.CrIndex);
            }
            imgEffect = imgeFilter.Apply(img);
            panelImage.BackgroundImage = imgEffect;
            //img = imgEffect;
        }

        private void btnCrop_Click(object sender, EventArgs e)
        {
            IFilter imgeFilter = new Crop(new Rectangle(rectCropArea.X, rectCropArea.Y, rectCropArea.Width, rectCropArea.Height));
            Bitmap imgEffect = new Bitmap(img, panelImage.Width, panelImage.Height);
            img = imgeFilter.Apply(imgEffect);
            panelImage.BackgroundImage = img;
        }

        private void btnProp_Click(object sender, EventArgs e)
        {
            Button btnProp = (Button)sender;
            //resizerControl.Clear();
            Bitmap imgProp = default(Bitmap);
            switch (btnProp.Name)
            {
                case "btnYellowHair":
                    imgProp = PhotoBooth.Properties.Resources.Prop_02;
                    break;
                case "btnOranceMuchh":
                    imgProp = PhotoBooth.Properties.Resources.Prop_06;
                    break;
                case "btnBlueHair":
                    imgProp = PhotoBooth.Properties.Resources.Prop_07;
                    break;
                case "btnOrangeHair":
                    imgProp = PhotoBooth.Properties.Resources.Prop_05;
                    break;
                case "btnBlackMuchh":
                    imgProp = PhotoBooth.Properties.Resources.prop_01;
                    break;
                case "btnBlackHair":
                    imgProp = PhotoBooth.Properties.Resources.prop_03;
                    break;
                default:
                    break;

            }
            resizerControl.ResizerImage = imgProp;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
             DialogResult result = saveImage.ShowDialog();
             if (result == DialogResult.OK)
             {
                 panelImage.BackgroundImage.Save(saveImage.FileName);
             }
        }


    }
}
