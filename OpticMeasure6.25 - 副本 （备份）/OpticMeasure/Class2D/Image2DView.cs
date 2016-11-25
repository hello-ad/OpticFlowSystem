using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HL2D.View2D
{
    public class Image2DView
    {
        private Panel _panel;
        private PictureBox _picture;
        public enum ViewModel { ImageAndLunKuo, ImageOnly, LunKuoOnly };
        private Image _viewImage;
        private ViewModel _viewImageModel;
        private Size _panelSize;
        private string  _orgImagePath; // 原始图像保存
        public int scaleModel = 1 ;  // 0 : 图像放大   1 : 调整图像容器缩放
        private Size _orgImageSize;
        private float _scaleMax = 3.0f;
        private float _scaleMin = 0.5f;
        private Rectangle _zbBound = new Rectangle(0, 0, 1, 1);
        private bool _zbdisplay = false;
        private Point _zbStart = new Point(0, 0);

        public Point pos = new Point(0, 0);
        // 下述 2 个参数，在使用图像容缩放时，可以不必使用。
        private float _aspectRate;
        private float _scaleRate = 1;  // 缩放比例，按原始图片大小，以控件显示范围为标准，计算相应的缩放比例，该数据主要针对于 大纵横比 的图片的首次显示用
        // End 

        // 参数表值
        private PamaterSetting zbDrawPara;

        public Image2DView(PictureBox _pict, Panel _pan, PamaterSetting _zbdraw)
        {
            picture = _pict;
            containPanel = _pan;
            zbDrawPara = _zbdraw;
        }
        public Image2DView(PamaterSetting _zbdraw)
        {
            // 构造函数
            zbDrawPara = _zbdraw;
        }
        #region
        public Rectangle zbBound
        {
            get { return _zbBound; }
            set { _zbBound = value; }
        } // 设置图像边界
        public Point zbStart
        {
            get { return _zbStart; }
            set { _zbStart = value; }
        }
        public bool zbDisplay
        {
            get { return _zbdisplay; }
            set
            {
                if (OrgImagePath != null)
                {
                    viewImage = Image.FromFile(OrgImagePath);
                    picture.Image = viewImage;
                    _zbdisplay = value;
                    if (_zbdisplay == true)
                        DrawXYkd();
                }
            }
        }
        public float scaleMax
        {
            get { return _scaleMax; }
            set { _scaleMax = value; }
        }
        public float scaleMin
        {
            get { return _scaleMin; }
            set { _scaleMin = value; }
        }
        private float scaleRate
        {
            get { return _scaleRate; }
            set { _scaleRate = value; }
        }
        public PictureBox picture
        {
            get { return _picture; }
            set
            { 
                _picture = value;
                _picture.SizeMode = PictureBoxSizeMode.StretchImage;

                if ((_panel != null) &(viewImage != null))
                {
                    ImageInit();
                }
            }
        }
        public Image viewImage
        {
            get { return _viewImage; }
            set 
            {
                _viewImage = value;
                zbBound = new Rectangle(zbBound.X, zbBound.Y, _viewImage.Width, _viewImage.Height);
                if (zbDisplay == true)
                    DrawXYkd();
            }
        }
        public ViewModel viewModel
        {
            get { return _viewImageModel; }
            set { _viewImageModel = value; }
        }
        public Panel containPanel
        {
            get { return _panel; }
            set 
            { 
                _panel = value; 
                _panelSize = _panel.Size;
                _panel.AutoScroll = true;
                _panel.AutoSize = true;
                _panel.Dock = DockStyle.Fill;
                if ((_picture != null) & (viewImage != null))
                {
                    ImageInit();
                }
            }
        }
        public Size OrgImageSize
        {
            get { return _orgImageSize; }
            set { _orgImageSize = value; }
        }
        public Size containPanelSize
        {
            get { return _panelSize; }
            set { _panelSize = value; }
        }
        public string OrgImagePath
        {
            get { return _orgImagePath; }
            set 
            {
                _orgImagePath = value;
                ImageInit();
            }
        }

        private float aspectRate
        {
            get { return _aspectRate; }
            set { _aspectRate = value; }
        }

        #endregion
        private  void zoom(Point center, int ZoomIndexBy1000)
        {
            int w, h;
            Point oldsize = new Point(picture.Width, picture.Height);

            if (scaleModel == 0)
            {
                w = Convert.ToInt32(viewImage.Width * ZoomIndexBy1000  / 1000);
                h = Convert.ToInt32(viewImage.Height* ZoomIndexBy1000  / 1000);
                Image orgimage = Image.FromFile(OrgImagePath);
                if (!((w < orgimage.Width ) | (h < orgimage.Height )))
                {
                    viewImage = BitmapToBlowUp(orgimage, w, h, true);
                    picture.Image = viewImage;

                    if (w > containPanelSize.Width)
                        picture.Width = w;
                    else
                        picture.Width = containPanelSize.Width;

                    if (h > containPanelSize.Height)
                        picture.Height = h;
                    else
                        picture.Height = containPanelSize.Height;
                }
                else
                {
                    picture.Image = Image.FromFile(OrgImagePath);
                    viewImage = picture.Image;
                    picture.Width = containPanelSize.Width;
                    picture.Height = containPanelSize.Height;

                }
                orgimage.Dispose();

            }
            else
            {
                w = Convert.ToInt32(oldsize.X * ZoomIndexBy1000 / 1000);
                h = Convert.ToInt32(oldsize.Y * ZoomIndexBy1000 / 1000);
                //picture.SizeMode = PictureBoxSizeMode.Zoom;

                if (w > containPanelSize.Width)
                    picture.Width = w;
                else
                    picture.Width = containPanelSize.Width;

                if (h > containPanelSize.Height)
                    picture.Height = h;
                else
                    picture.Height = containPanelSize.Height;
            }
        }  // 暂不使用
        private  Image BitmapToBlowUp(Image p_Bitmap, int p_Width, int p_Height, bool p_ZoomType)
        {
            Bitmap _ZoomBitmap = new Bitmap(p_Width, p_Height);

            Graphics _Graphics = Graphics.FromImage(_ZoomBitmap);

            if (!p_ZoomType)
            {
                _Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                _Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            }

            _Graphics.DrawImage(p_Bitmap, 0, 0, _ZoomBitmap.Width, _ZoomBitmap.Height);

            _Graphics.Dispose();
            return _ZoomBitmap;
        } // 对图像进行缩放，暂不使用

        public void ImageZoom(Point center, int ZoomIndexBy1000)
        {

            Point oldsize = new Point(picture.Width, picture.Height);
           
            int  w = Convert.ToInt32(oldsize.X * ZoomIndexBy1000 / 1000);
            int  h = Convert.ToInt32(oldsize.Y * ZoomIndexBy1000 / 1000);

            if (((w <= containPanelSize.Width * scaleMax) & (w >= containPanelSize.Width * scaleMin)) | ((h <= containPanelSize.Height * scaleMax) & (h >= containPanelSize.Height * scaleMin)))
            {
                picture.Width = w;
                picture.Height = h;

                //picture.Top = (containPanelSize.Height >= picture.Height ? Convert.ToInt32((containPanelSize.Height - picture.Height) * 1.0 / 2) : 0);
                //picture.Left = (containPanelSize.Width >= picture.Width ? Convert.ToInt32((containPanelSize.Width - picture.Width) * 1.0 / 2) : 0);

                //if (picture.Width <= containPanel.Width)
                //    if (picture.Left > 0) picture.Left = 0;
                //if (picture.Height <= containPanel.Height)
                //    if (picture.Top < 0) picture.Top = 0;

                // 缩放后，以 center 为中心重新定位
                Point p1 = new Point(center.X,center.Y);  // 进行缩放的 图像位置
                Point p1Client = containPanel.PointToClient(p1);  // 将屏幕位置转换为 客户区 位置

                // 屏幕中心位置
                Point c1 = new Point(Convert.ToInt32(containPanel.Width / 2), Convert.ToInt32(containPanel.Height / 2));
                Point c1Client = containPanel.PointToClient(c1);

                // 缩放后 图像中心 位置 
                Point p2 = new Point(Convert.ToInt32(center.X * ZoomIndexBy1000/1000), Convert.ToInt32(center.Y * ZoomIndexBy1000/1000));
                Point p2Client = containPanel.PointToClient(p2);

                Point distance = new Point(p2Client.X - c1Client.X, p2Client.Y - c1Client.Y);

                containPanel.AutoScrollPosition = distance;

                //picture.Top = (containPanelSize.Height >= picture.Height ? Convert.ToInt32((containPanelSize.Height - picture.Height) * 1.0 / 2) : 0);
                //picture.Left = (containPanelSize.Width >= picture.Width ? Convert.ToInt32((containPanelSize.Width - picture.Width) * 1.0 / 2) : 0);

            } // 缩放后的 图像大小 在缩放的最大倍数与最小倍数之间， 有效

        }
        private void ImageInit()
        {
            if (viewImage != null)
            {
                if ((containPanelSize.Width > viewImage.Width) & (containPanelSize.Height > viewImage.Height))
                {
                    if (viewImage.Width > viewImage.Height)
                    {
                        picture.Width = containPanelSize.Width;
                        picture.Height = Convert.ToInt32(containPanelSize.Height * viewImage.Height * 1.0 / viewImage.Width);
                    }
                    else
                    {
                        picture.Height = containPanelSize.Height;
                        picture.Width = Convert.ToInt32(containPanelSize.Width * viewImage.Width * 1.0 / viewImage.Height);
                    }
                }

                if ((containPanelSize.Width <= viewImage.Width) & (containPanelSize.Height > viewImage.Height))
                {
                    picture.Width = containPanelSize.Width;
                    picture.Height = Convert.ToInt32(containPanelSize.Width * viewImage.Height * 1.0 / viewImage.Width);
                }

                if ((containPanelSize.Width > viewImage.Width) & (containPanelSize.Height <= viewImage.Height))
                {
                    picture.Height = containPanelSize.Height;
                    picture.Width = Convert.ToInt32(containPanelSize.Height * viewImage.Width * 1.0 / viewImage.Height);
                }

                if ((containPanelSize.Width <= viewImage.Width) & (containPanelSize.Height <= viewImage.Height))
                {
                    if (viewImage.Width > viewImage.Height)
                    {
                        picture.Width = containPanelSize.Width;
                        picture.Height = Convert.ToInt32(containPanelSize.Height * viewImage.Height * 1.0 / viewImage.Width);
                    }
                    else
                    {
                        picture.Height = containPanelSize.Height;
                        picture.Width = Convert.ToInt32(containPanelSize.Height * viewImage.Width * 1.0 / viewImage.Height);
                    }
                }

                picture.Top = (containPanelSize.Height >= picture.Height ? Convert.ToInt32((containPanelSize.Height - picture.Height) * 1.0 / 2) : 0);
                picture.Left = (containPanelSize.Width >= picture.Width ? Convert.ToInt32((containPanelSize.Width - picture.Width) * 1.0 / 2) : 0);

                picture.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        public void ImageReset()
        {
            picture.Top = 0;
            picture.Left = 0;
            ImageInit();

            //MessageBox.Show( "picture H " + picture.Height.ToString() + " W " + picture.Width.ToString() + " panel h " + containPanel.Height.ToString() + " w " + containPanel.Width.ToString() );
        }

        #region
        // 坐标轴显示处理
        private void DrawXYkd()
        {
            int lineWidth, fontSize;
            lineWidth = zbDrawPara.zbDrawParamater.zbLineWidth ;
            fontSize  = zbDrawPara.zbDrawParamater.zbFontSize;

            Graphics dc = Graphics.FromImage(picture.Image);
            Pen pen = new Pen(Color.FromArgb(zbDrawPara.zbDrawParamater.zbcolor), lineWidth);
            System.Drawing.Font font = new System.Drawing.Font("Arial", fontSize); // 用 Arial 字体，10号字体进行标注

            //if ((zbDrawPara.zbDrawParamater.Xenable == true) | (zbDrawPara.zbDrawParamater.Yenable == true))
            //{
            //    dc.DrawLine(pen, new Point(zbStart.X, zbStart.Y), new Point(zbStart.X,zbBound.Height));
            //    dc.DrawLine(pen, new Point(zbStart.X, zbStart.Y), new Point(zbBound.Width,zbStart.Y));
            //} // 如要显示 X 或 Y 坐标，则要显示相应的 轴线

            if (zbDrawPara.zbDrawParamater.Xenable == true)
            {
                int widthN = Convert.ToInt32(zbBound.Width / zbDrawPara.zbDrawParamater.zbkdXAux);
                for (int i = 0; i <= widthN; i++)
                {
                    Point WN1 = new Point(zbStart.X + i * zbDrawPara.zbDrawParamater.zbkdXAux, zbStart.Y);
                    Point WN2 = new Point(zbStart.X + i * zbDrawPara.zbDrawParamater.zbkdXAux, zbBound.Height);

                    if (i == 0)
                        dc.DrawLine(pen, new Point(zbStart.X, zbStart.Y), new Point(zbBound.Width, zbStart.Y));
                    // 画坐标轴线

                    if (((WN1.X - zbStart.X) % zbDrawPara.zbDrawParamater.zbkdXMain) == 0)
                    {
                        if (zbDrawPara.zbDrawParamater.zbIsGrid == true)
                            dc.DrawLine(pen, WN1, WN2);
                        else
                            dc.DrawLine(pen, WN1, new Point(WN2.X, zbDrawPara.zbDrawParamater.zbkdLabelLengthMain));
                        if (i != 0)
                            dc.DrawString((WN1.X+zbBound.X).ToString(), font, pen.Brush, WN1);
                    }
                    else
                        dc.DrawLine(pen, WN1, new Point(WN2.X, zbDrawPara.zbDrawParamater.zbkdLabelLengthAux));

                }
            }
            if (zbDrawPara.zbDrawParamater.Yenable == true)
            {
                int heightN = Convert.ToInt32(zbBound.Height / zbDrawPara.zbDrawParamater.zbkdYAux);
                for (int i = 0; i <= heightN; i++)
                {
                    Point HN1 = new Point(zbStart.X, zbStart.Y + i * zbDrawPara.zbDrawParamater.zbkdYAux);
                    Point HN2 = new Point(zbBound.Width, zbStart.Y + i * zbDrawPara.zbDrawParamater.zbkdYAux);
                    if (i == 0)
                       dc.DrawLine(pen, new Point(zbStart.X, zbStart.Y), new Point(zbStart.X, zbBound.Height));
                    // 画坐标轴线

                    if (((HN1.Y - zbStart.Y) % zbDrawPara.zbDrawParamater.zbkdYMain) == 0)
                    {
                        if (zbDrawPara.zbDrawParamater.zbIsGrid == true)
                            dc.DrawLine(pen, HN1, HN2);
                        else
                            dc.DrawLine(pen, HN1, new Point(zbDrawPara.zbDrawParamater.zbkdLabelLengthMain, HN2.Y));
                        if (i != 0)
                            dc.DrawString((HN1.Y +zbBound.Y ).ToString(), font, pen.Brush, HN1);
                    }
                    else
                        dc.DrawLine(pen, HN1, new Point(zbDrawPara.zbDrawParamater.zbkdLabelLengthAux, HN2.Y));
                }
            }
            font.Dispose();
            pen.Dispose();
            dc.Dispose();
        }
        #endregion

        #region
        //public void SetViewImage(PictureBox _picture,Image _viewImage)
        //{
        //    if (_picture != null)
        //        picture = _picture;
        //    if (_viewImage != null)
        //        viewImage = _viewImage;
        //}

        //public void SetViewImage(Image _viewImage)
        //{
        //    viewImage = _viewImage;
        //}

        //public void SetViewMode(viewModel _viewmodel)
        //{
        //    viewImageModel = _viewmodel;
        //}

        #endregion

    }
}
