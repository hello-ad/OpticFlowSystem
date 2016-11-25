using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Drawing.Imaging;
using HL2D.View2D;
using System.IO;

namespace ZH2DCL.ZtClass
{
    public struct FlowData
    {
       public  Point p0;
       public  Point p1;
       public bool isExit;
   }
    public struct FlowDataF
    {
        public PointF p0;
        public PointF p1;
        public bool isExit;
    }
    public struct FlowSizeF
    {
        public PointF p0;
        public SizeF p1;
        public bool isExit;
    }

   
    public enum lineTypeEnum
    {
        Arrow =0,
        Line=1,
    }

    public class DrawFlow
    {
        private double PI = 3.1415926;
        private ArrayList listFlow = new ArrayList();       

        public double awLength = 0.2; // 箭头线的长度， > 1.0 为固定长度， <= 1.0 为 直线长度的比例，箭头线长度 = awLength * 直线长度
        public double awAngle = 20.0; // 箭头与直线的夹角，默认值为 15度
        public int awColor = Color.Blue.ToArgb(); //  画线的颜色，默认为 Blue
        public int awWidth = 2; // 画线的宽度，默认为 1 ： 1个像素
        public float arrowScale = 1.0f; // 简头比例
       
        public lineTypeEnum LineType = lineTypeEnum.Arrow;//0代表带箭头直线，1代表绘制无箭头线段
        public int LeftMargin =50;//内部边距Internal margin,左边距，单位像素
        public int RightMargin = 25;//内部边距Internal margin,右边距，单位像素
        public int TopMargin = 50;//内部边距Internal margin,上边距，单位像素
        public int BottomMargin = 25;//内部边距Internal margin,下边距，单位像素
        public float Percent = 50f;//相对于画布放缩比例
        public bool ShowEnable = false;//绘图使能变量
        public bool NodesShowEnable = false;//节点显示使能
        public PointF MinPoint = new PointF();//数据区域最小点坐标
        public PointF MaxPoint = new PointF();//数据区域最大点坐标
        public Point CanvasSize = new Point(1000, 1000);
        public float VectorLength = 1;//向量放大与缩小比率

        private RectangleF _zbBound = new RectangleF(0, 0, 1, 1);
        private bool _zbdisplay = false;
        private PointF _zbStart = new PointF(50, 50);
        public  PamaterSetting zbDrawPara =new PamaterSetting ();
      
        public bool zbDisplay
        {
            get { return _zbdisplay; }
            set { _zbdisplay = value;}
        }

        //初始化函数
        public DrawFlow()
        {
            listFlow.Clear();
        }

        //析构函数
        ~DrawFlow()
        {
            listFlow.Clear();            
        }

        //获取动态数组长度
        public int Length
        {
            get
            {
                return listFlow.Count;
            }
        }

        //数组清空函数
        public void ClearFlow()
        {
            listFlow.Clear();
            GC.Collect();
        }

        public void AddFlow(Point pointStart, Point pointEnd, bool isexit)
        {
            FlowData fd = new FlowData();
            fd.p0 = pointStart;
            fd.p1 = pointEnd;
            fd.isExit = isexit;
            listFlow.Add(fd);
        }
        public void AddFlow(PointF pointStart, PointF pointEnd, bool isexit)
        {
            FlowDataF fd = new FlowDataF();
            fd.p0 = pointStart;
            fd.p1 = pointEnd;
            fd.isExit = isexit;
            listFlow.Add(fd);
        }


        public void AddFlow(FlowData fdata)
        {
            listFlow.Add(fdata);
        }
        public void AddFlow(FlowDataF fdata)
        {
            listFlow.Add(fdata);
        }


        public bool DeleteFlow(FlowData fdata)
        {
            FlowData fd;
            bool succ = false;

            for (int i = 0; i < listFlow.Count; i++)
            {
                fd = (FlowData)listFlow[i];
                if (fd.Equals(fdata))
                {
                    listFlow.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            return succ;
        }
        public bool DeleteFlow(FlowDataF fdata)
        {
            FlowDataF fd;
            bool succ = false;

            for (int i = 0; i < listFlow.Count; i++)
            {
                fd = (FlowDataF)listFlow[i];
                if (fd.Equals(fdata))
                {
                    listFlow.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            return succ;
        }


        public bool DeleteFlow(Point pointStart, Point pointEnd,bool isexit)
        {
            FlowData fd = new FlowData();
            FlowData fdata;
            fd.p0 = pointStart;
            fd.p1 = pointEnd;
            fd.isExit = isexit;
            bool succ = false;

            for (int i = 0; i < listFlow.Count; i++)
            {
                fdata = (FlowData)listFlow[i];
                if (fd.Equals(fdata))
                {
                    listFlow.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            return succ;
        }
        public bool DeleteFlow(PointF pointStart, PointF pointEnd, bool isexit)
        {
            FlowDataF fd = new FlowDataF();
            FlowDataF fdata;
            fd.p0 = pointStart;
            fd.p1 = pointEnd;
            fd.isExit = isexit;
            bool succ = false;

            for (int i = 0; i < listFlow.Count; i++)
            {
                fdata = (FlowDataF)listFlow[i];
                if (fd.Equals(fdata))
                {
                    listFlow.RemoveAt(i);
                    succ = true;
                    break;
                }
            }
            return succ;
        }

        public bool isExist(FlowData fdata)
        {
            FlowData fd;
            bool succ = false;

            for (int i = 0; i < listFlow.Count; i++)
            {
                fd = (FlowData)listFlow[i];
                if (fd.Equals(fdata))
                {
                    succ = true;
                    break;
                }
            }
            return succ;
        }
        public bool isExist(FlowDataF fdata)
        {
            FlowDataF fd;
            bool succ = false;

            for (int i = 0; i < listFlow.Count; i++)
            {
                fd = (FlowDataF)listFlow[i];
                if (fd.Equals(fdata))
                {
                    succ = true;
                    break;
                }
            }
            return succ;
        }


        public bool isExist(Point pointStart, Point pointEnd,bool isexit)
        {
            FlowData fd = new FlowData();
            fd.p0 = pointStart;
            fd.p1 = pointEnd;
            fd.isExit = isexit;
            return listFlow.Contains(fd);
        }
        public bool isExist(PointF pointStart, PointF pointEnd, bool isexit)
        {
            FlowDataF fd = new FlowDataF();
            fd.p0 = pointStart;
            fd.p1 = pointEnd;
            fd.isExit = isexit;
            return listFlow.Contains(fd);
        }

        public FlowData getElement(int index)
        {
            FlowData fd = new FlowData() ;
            if (index <= listFlow.Count)
            {
                fd = (FlowData)listFlow[index];
            }
             return fd;
       }
        public FlowDataF getElementF(int index)
        {
            FlowDataF fd = new FlowDataF();
            if (index <= listFlow.Count)
            {
                fd = (FlowDataF)listFlow[index];
            }
            return fd;
        }

        public void DrawFlowGraphics(PictureBox picture,int imgWidth, int imgHeight)
        {
            Bitmap curBitmap = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(curBitmap);

            DrawGraphics(g);

            //Rectangle rect = new Rectangle(0, 0, imgWidth, imgHeight);
            //BitmapData bmpData = curBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, curBitmap.PixelFormat);
            ////IntPtr ptr = bmpData.Scan0;

            ////byte[] rgbValues = new byte[1];

            //////显示轮廓
            //////showContour(ref rgbValues, AllpicturelunkuoArray[index].lunkuoArray);
            //////显示点中心
            //////showPointCenter(ref rgbValues,PictureCodeDataArray[index]);
            ////System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, rgbValues.Length);
            //curBitmap.UnlockBits(bmpData);
            picture.Image = curBitmap;

        }

        public void DrawFlowGraphics(PictureBox picture)
        {
            Point MinPoint = new Point();
            Point MaxPoint = new Point();
            Point ShiftSize = new Point();

            GetCoordinateMinAndMaxFunc(ref MinPoint, ref MaxPoint);

            ShiftSize.X = MinPoint.X - LeftMargin;
            ShiftSize.Y = MinPoint.Y - TopMargin;

            CoordinateTranslationFunc(ShiftSize);

            int imgWidth = MaxPoint.X - MinPoint.X + LeftMargin + RightMargin;
            int imgHeight = MaxPoint.Y - MinPoint.Y + TopMargin + BottomMargin;
            Image imageTemp = Image.FromFile(Application.StartupPath + @"\Cursor\空白.BMP");
            Bitmap curBitmap = new Bitmap(imageTemp,imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(curBitmap);

            DrawGraphics(g);

            picture.Image = curBitmap;

        }
        public void DrawFlowGraphicsF(PictureBox picture)
        {                  
            int imgWidth = CanvasSize.X, imgHeight = CanvasSize.Y;
            //GetCoordinateMinAndMaxFunc(ref MinPoint, ref MaxPoint);//最大最小值获取函数

            AutoSetImageSizeFunc(MinPoint, MaxPoint, ref imgWidth, ref imgHeight);//设置画布大小

            //Image imageTemp = Image.FromFile(Application.StartupPath + @"\Cursor\空白.BMP");//画布填充图案
            Bitmap curBitmap = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(curBitmap);

            DrawGraphicsF(g);
            if (zbDisplay == true)
            {
                DrawXYkd(g);
            }
            if (picture.Image != null)
                picture.Image.Dispose();
            
            picture.Image = curBitmap;
        }

        private void AutoSetImageSizeFunc(PointF MinPoint, PointF MaxPoint, ref int imgWidth, ref int imgHeight)
        {
            imgWidth = (int)(MaxPoint.X - MinPoint.X) + LeftMargin + RightMargin;
            imgHeight = (int)(MaxPoint.Y - MinPoint.Y) + TopMargin + BottomMargin;
            //if (imgHeight > imgWidth)
            //{
            //    imgWidth = imgHeight;
            //}
            //else
            //{
            //    imgHeight = imgWidth;
            //}            
        }
        private PointF calcNewPointByLength(PointF p0, PointF p1)  // 按指定的箭头长度比例，调整原直线的 终点 位置坐标
        {
            double len = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // 直线的长度
            len = len * arrowScale;  // 箭头长度按指定比便调整
            double x, y;
            if (p1.X != p0.X)
            {
                double k = ((double)(p1.Y - p0.Y)) / (p1.X - p0.X);  // 直线的斜率

                if (p1.X > p0.X)
                    x = Math.Sqrt((len * len) / (k * k + 1)) + p0.X;
                else
                    x = -Math.Sqrt((len * len) / (k * k + 1)) + p0.X;

                y = k * (x - p0.X) + p0.Y;
            }
            else
            {
                x = p1.X;
                y =p0.Y+(p1.Y-p0.Y)* arrowScale;
            }
            PointF rnt = new PointF((float)x, (float)y);
            return rnt;
        }

        private void DrawGraphics(Graphics g)
        {
            Pen pen = new Pen(Color.FromArgb(awColor), awWidth); // 定义 标注颜色、箭头线宽度
            FlowData fd;
            float lengthtemp=0f;
            //Font FontTemp = new Font (SystemFonts.DefaultFont,System.Drawing.FontStyle.Regular);
            System.Drawing.Font FontTemp = new System.Drawing.Font("宋体", 9);
            Brush BrushTemp = pen.Brush;

            for (int i = 0; i < listFlow.Count; i++)
            {
                fd = (FlowData)listFlow[i];
                if (LineType == lineTypeEnum.Arrow)
                {
                    DrawAnyArrow(g, pen, fd.p0, fd.p1, awLength, awAngle, awColor, awWidth);
                }

                g.DrawLine(pen, fd.p0, fd.p1); // 画直线;

                lengthtemp=(float)Math.Sqrt((fd.p0.X-fd.p1.X)*(fd.p0.X-fd.p1.X)+(fd.p0.Y-fd.p1.Y)*(fd.p0.Y-fd.p1.Y));
                if (lengthtemp > 20)
                    g.DrawString(fd.p0.X.ToString() + ',' + fd.p0.Y.ToString(), FontTemp, BrushTemp, fd.p0);
            }
            BrushTemp.Dispose();
            FontTemp.Dispose();
            pen.Dispose();
        }
        private void DrawGraphicsF(Graphics g)
        {
            Pen pen = new Pen(Color.FromArgb(awColor), awWidth); // 定义 标注颜色、箭头线宽度
            FlowDataF fd;
            System.Drawing.Font FontTemp = new System.Drawing.Font("宋体", 3);
            Brush BrushTemp = pen.Brush;
            
            SizeF OffsetSize = new SizeF(MinPoint.X - LeftMargin, MinPoint.Y - TopMargin);

             for (int i = 0; i < listFlow.Count; i++)
            {
                fd = (FlowDataF)listFlow[i];

                //向量平移
                fd.p0 -= OffsetSize; fd.p1 -= OffsetSize;
                PointF newPos = calcNewPointByLength(fd.p0, fd.p1);

                fd.p1.X = newPos.X;
                fd.p1.Y = newPos.Y;

                if (LineType == lineTypeEnum.Arrow)
                {
                    DrawAnyArrow(g, pen, fd.p0, fd.p1, awLength, awAngle, awColor, awWidth);//绘制箭头 
                    g.DrawLine(pen, fd.p0, fd.p1); // 画直线;

                }

                if (LineType == lineTypeEnum.Line)
                {                    
                    g.DrawLine(pen, fd.p0, fd.p1); // 画直线;
                }

                if (NodesShowEnable == true)
                {
                    RectangleF rect = new RectangleF(fd.p0 - new SizeF(2, 2), new SizeF(4, 4));
                    g.FillRectangle(BrushTemp, rect);//绘制坐标点
                }               
                
            }
            BrushTemp.Dispose();
            FontTemp.Dispose();
            pen.Dispose();
        }

        private void DrawAnyArrow(Graphics g, Point p0, Point p1, double arrowLength, double arrowAngle, int arrowColor, int arrowWidth)
        {
            // p0,p1 ：要画的直线的两个顶点
            // arrowLength ：箭头线的长度， > 1.0 为固定长度， <= 1.0 为 直线长度的比例，箭头线长度 = arrowLength * 直线长度
            // arrowAngle ：箭头与直线的角度
            // arrowColor ：画线的颜色
            // arrowWidth ：画线的宽度

            ////对于点(x0,y0)指向(x1,y1)的箭头线，以(x1,y1)为原点，先计算出(x1,y1) 
            ////指向(x0,y0)的单位向量，箭头线与直线的角度为d，那么就是将这个向量 
            ////旋转角度d和－d，向量旋转的公式为： 
            ////(a+b*i)*(cos(d)+i*sin(d))   =   a*cos(d)-b*sin(d)   +   i*(a*sin(d)+b*cos(d)) 
            ////因为上面的向量以(x1,y1)为原点的单位向量，所以还需要乘以一个长度（ 
            ////箭头线的长度），然后加上偏移就可以了。也就是画的箭头线的顶点的坐标为 
            ////x=x1+L*(a*cos(d)-b*sin(d)),   y=y1+L*(a*sin(d)+b*cos(d))。 
            ////另一个顶点的坐标，将上面的sin(d)换成   -sin(d)   就可以了。 
            ////上面公式中的a为(x0-x1)/L0,   b为(y0-y1)/L0，L0是(x0,y0)与(x1,y1)的长度， 
            ////L是箭头线的长度(自己设定)。 

            Pen pen = new Pen(Color.FromArgb(arrowColor), arrowWidth); // 定义 标注颜色、箭头线宽度
            Brush brush = pen.Brush;

            double L0 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // 画的直线的长度
            double a = (p0.X - p1.X) / L0;
            double b = (p0.Y - p1.Y) / L0; // 单位向量的两个分量
            double d = arrowAngle / 180 * PI; // 箭头线与直线的角度为 arrowAngle
            double length = 0.0;
  
            if (arrowLength > 1.0)
                length = arrowLength;
            else
                length = arrowLength * L0;
            // 如果给定的参数 arrowLength > 1.0，认为是固定长度的箭头长度
            // 如果 arrowLength <= 1.0， 则箭头长度为 直线长度 * arrowLength。

            int P2x = (int)(p1.X + arrowLength * (a * Math.Cos(d) - b * Math.Sin(d)));
            int P2y = (int)(p1.Y + arrowLength * (a * Math.Sin(d) + b * Math.Cos(d)));

            int P3x = (int)(p1.X + arrowLength * (a * Math.Cos(d) - b * Math.Sin(-d)));
            int P3y = (int)(p1.Y + arrowLength * (a * Math.Sin(-d) + b * Math.Cos(d)));

            g.DrawLine(pen, p0, p1); // 画直线;
            g.DrawPolygon(pen, new Point[] { new Point(P2x, P2y), new Point(P3x, P3y), p1 });
            g.FillPolygon(brush, new Point[] { new Point(P2x, P2y), new Point(P3x, P3y), p1 });
            //g.DrawLine(pen, new Point(P2x, P2y), p1); // 画箭头的一条边
            //g.DrawLine(pen, new Point(P3x, P3y), p1); // 画箭头的另一条边

            brush.Dispose();
            pen.Dispose();

        }
        private void DrawAnyArrow(Graphics g, PointF p0, PointF p1, double arrowLength, double arrowAngle, int arrowColor, int arrowWidth)
        {            
            Pen pen = new Pen(Color.FromArgb(arrowColor), arrowWidth); // 定义 标注颜色、箭头线宽度
            Brush brush = pen.Brush;

            double L0 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // 画的直线的长度
            double a = (p0.X - p1.X) / L0;
            double b = (p0.Y - p1.Y) / L0; // 单位向量的两个分量
            double d = arrowAngle / 180 * PI; // 箭头线与直线的角度为 arrowAngle
            double length = 0.0;

            if (arrowLength > 1.0)
                length = arrowLength;
            else
                length = arrowLength * L0;
            // 如果给定的参数 arrowLength > 1.0，认为是固定长度的箭头长度
            // 如果 arrowLength <= 1.0， 则箭头长度为 直线长度 * arrowLength。

            int P2x = (int)(p1.X + arrowLength * (a * Math.Cos(d) - b * Math.Sin(d)));
            int P2y = (int)(p1.Y + arrowLength * (a * Math.Sin(d) + b * Math.Cos(d)));

            int P3x = (int)(p1.X + arrowLength * (a * Math.Cos(d) - b * Math.Sin(-d)));
            int P3y = (int)(p1.Y + arrowLength * (a * Math.Sin(-d) + b * Math.Cos(d)));

            g.DrawLine(pen, p0, p1); // 画直线;
            g.DrawPolygon(pen, new PointF[] { new PointF(P2x, P2y), new PointF(P3x, P3y), p1 });
            g.FillPolygon(brush, new PointF[] { new PointF(P2x, P2y), new PointF(P3x, P3y), p1 });
            //g.DrawLine(pen, new Point(P2x, P2y), p1); // 画箭头的一条边
            //g.DrawLine(pen, new Point(P3x, P3y), p1); // 画箭头的另一条边

            brush.Dispose();
            pen.Dispose();

        }
        
        private void DrawAnyArrow(Graphics g,Pen pen, Point p0, Point p1, double arrowLength, double arrowAngle, int arrowColor, int arrowWidth)
        {
            // p0,p1 ：要画的直线的两个顶点
            // arrowLength ：箭头线的长度
            // arrowAngle ：箭头与直线的角度
            // arrowColor ：画线的颜色
            // arrowWidth ：画线的宽度

            ////对于点(x0,y0)指向(x1,y1)的箭头线，以(x1,y1)为原点，先计算出(x1,y1) 
            ////指向(x0,y0)的单位向量，箭头线与直线的角度为d，那么就是将这个向量 
            ////旋转角度d和－d，向量旋转的公式为： 
            ////(a+b*i)*(cos(d)+i*sin(d))   =   a*cos(d)-b*sin(d)   +   i*(a*sin(d)+b*cos(d)) 
            ////因为上面的向量以(x1,y1)为原点的单位向量，所以还需要乘以一个长度（ 
            ////箭头线的长度），然后加上偏移就可以了。也就是画的箭头线的顶点的坐标为 
            ////x=x1+L*(a*cos(d)-b*sin(d)),   y=y1+L*(a*sin(d)+b*cos(d))。 
            ////另一个顶点的坐标，将上面的sin(d)换成   -sin(d)   就可以了。 
            ////上面公式中的a为(x0-x1)/L0,   b为(y0-y1)/L0，L0是(x0,y0)与(x1,y1)的长度， 
            ////L是箭头线的长度(自己设定)。 
            double length = 0;

 
            //Pen pen = new Pen(Color.FromArgb(arrowColor), arrowWidth); // 定义 标注颜色、箭头线宽度  在使用前定义 Pen 对象。
            Brush brush = pen.Brush;
            double L0 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // 画的直线的长度

            if (!((p0.Equals(new Point(0, 0))) || (p1.Equals(new Point(0, 0)))) & (L0 != 0.0))
            {
                double a = (p0.X - p1.X) / L0;
                double b = (p0.Y - p1.Y) / L0; // 单位向量的两个分量
                double d = arrowAngle / 180 * PI; // 箭头线与直线的角度为 arrowAngle

                if (arrowLength > 1.0)
                    length = arrowLength;
                else
                    length = arrowLength * L0;
                // 如果给定的参数 arrowLength > 1.0，认为是固定长度的箭头长度
                // 如果 arrowLength <= 1.0， 则箭头长度为 直线长度 * arrowLength。

                int P2x = (int)(p1.X + length * (a * Math.Cos(d) - b * Math.Sin(d)));
                int P2y = (int)(p1.Y + length * (a * Math.Sin(d) + b * Math.Cos(d)));

                int P3x = (int)(p1.X + length * (a * Math.Cos(d) - b * Math.Sin(-d)));
                int P3y = (int)(p1.Y + length * (a * Math.Sin(-d) + b * Math.Cos(d)));

                g.DrawLine(pen, p0, p1); // 画直线;
                g.DrawPolygon(pen, new Point[] { new Point(P2x, P2y), new Point(P3x, P3y), p1 });
                g.FillPolygon(brush, new Point[] { new Point(P2x, P2y), new Point(P3x, P3y), p1 });
                //g.DrawLine(pen, new Point(P2x, P2y), p1); // 画箭头的一条边
                //g.DrawLine(pen, new Point(P3x, P3y), p1); // 画箭头的另一条边
            }
            else
            {
                //System.Drawing.Font font = new System.Drawing.Font("Arial", 10); // 用 Arial 字体，10号字体进行标注
                //if (p0.Equals(new Point(0, 0)))
                //{
                //    g.DrawString("●", font, brush, p1);
                //    //g.DrawLine(pen,new Point(p1.X,p1.Y-1),new Point(p1.X,p1.Y+1));
                //    //g.DrawLine(pen,new Point(p1.X-1,p1.Y),new Point(p1.X+1,p1.Y));
                //}
                //if (p1.Equals(new Point(0, 0)))
                //{
                //    g.DrawString("●", font, brush, p0);
                //    //g.DrawLine(pen, new Point(p0.X, p0.Y - 1), new Point(p0.X, p0.Y + 1));
                //    //g.DrawLine(pen, new Point(p0.X - 1, p0.Y), new Point(p0.X + 1, p0.Y));
                //}
            }
            brush.Dispose();
            //pen.Dispose();  Pen 对象在使用前定义

        }
        private void DrawAnyArrow(Graphics g, Pen pen, PointF p0, PointF p1, double arrowLength, double arrowAngle, int arrowColor, int arrowWidth)
        { 
            double length = 0;


            //Pen pen = new Pen(Color.FromArgb(arrowColor), arrowWidth); // 定义 标注颜色、箭头线宽度  在使用前定义 Pen 对象。
            Brush brush = pen.Brush;
            double L0 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // 画的直线的长度

            if (!((p0.Equals(new PointF(0, 0))) || (p1.Equals(new PointF(0, 0)))) & (L0 != 0.0))
            {
                double a = (p0.X - p1.X) / L0;
                double b = (p0.Y - p1.Y) / L0; // 单位向量的两个分量
                double d = arrowAngle / 180 * PI; // 箭头线与直线的角度为 arrowAngle

                if (arrowLength > 1.0)
                    length = arrowLength;
                else
                    length = arrowLength * L0;
                // 如果给定的参数 arrowLength > 1.0，认为是固定长度的箭头长度
                // 如果 arrowLength <= 1.0， 则箭头长度为 直线长度 * arrowLength。

                //length = length * arrowScale;

                float P2x = (float)(p1.X + length * (a * Math.Cos(d) - b * Math.Sin(d)));
                float P2y = (float)(p1.Y + length * (a * Math.Sin(d) + b * Math.Cos(d)));

                float P3x = (float)(p1.X + length * (a * Math.Cos(d) - b * Math.Sin(-d)));
                float P3y = (float)(p1.Y + length * (a * Math.Sin(-d) + b * Math.Cos(d)));

                g.DrawLine(pen, p0, p1); // 画箭头直线;
                g.DrawPolygon(pen, new PointF[] { new PointF(P2x, P2y), new PointF(P3x, P3y), p1 });
                g.FillPolygon(brush, new PointF[] { new PointF(P2x, P2y), new PointF(P3x, P3y), p1 });
                //g.DrawLine(pen, new Point(P2x, P2y), p1); // 画箭头的一条边
                //g.DrawLine(pen, new Point(P3x, P3y), p1); // 画箭头的另一条边
            }
            else
            {
                
            }
            brush.Dispose();
            //pen.Dispose();  Pen 对象在使用前定义

        }



        //得到坐标集合的最大值和最小值
        public bool GetCoordinateMinAndMaxFunc(ref Point MinPoint, ref Point MaxPoint)
        //函数名：bool GetCoordinateMinAndMaxFunc(ref point MinPoint,ref point MaxPoint )
        //输入：ListFlow待绘制图坐标数组
        //输出：上述坐标集合的最大值和最小值坐标MinPoint，MaxPoint
        //功能描述：获取ListFlow最大值和最小值
        {

            //算法
            //step1、数据有效性检查
            //若 ListFlow==null 则
            if (listFlow == null)
                return false;
            //若 ListFlow ==0则 
            if (listFlow.Count == 0)
                return false;

            //step2、MinPoint，MaxPoint初始化
            //MinPoint，MaxPoint的X坐标赋值为ListFlow[0]的p0坐标X
            MinPoint.X = ((FlowData)(listFlow[0])).p0.X;
            MaxPoint.X = ((FlowData)(listFlow[0])).p0.X;

            //MinPoint，MaxPoint的Y坐标赋值为ListFlow[0]的p0坐标Y
            MinPoint.Y = ((FlowData)(listFlow[0])).p0.Y;
            MaxPoint.Y = ((FlowData)(listFlow[0])).p0.Y;

            FlowData FlowDataTemp = new FlowData();//

            //Step3、数组循环
            //for i=0 to ListFlow.Length
            for (int i = 0; i < listFlow.Count; i++)
            {
                FlowDataTemp = (FlowData)(listFlow[i]);

                //step4、X坐标与最大和最小值比较
                //若 第i个ListFlow坐标X大于MaxPoint坐标X 则
                if (FlowDataTemp.p0.X > MaxPoint.X)
                {
                    //MaxPoint坐标X赋值为第i个ListFlow坐标X
                    MaxPoint.X = FlowDataTemp.p0.X;
                }

                //若 第i个ListFlow坐标X小于MinPoint坐标X 则
                if (FlowDataTemp.p0.X < MinPoint.X)
                {
                    //MinPoint坐标X赋值为第i个ListFlow坐标X
                    MinPoint.X = FlowDataTemp.p0.X;
                }


                //Y坐标与最大和最小值比较
                //若 第i个ListFlow坐标Y大于MaxPoint坐标Y 则
                if (FlowDataTemp.p0.Y > MaxPoint.Y)
                {
                    //MaxPoint坐标Y赋值为第i个ListFlow坐标Y
                    MaxPoint.Y = FlowDataTemp.p0.Y;
                }
                //若 第i个ListFlow坐标Y小于MinPoint坐标Y 则
                if (FlowDataTemp.p0.Y < MinPoint.Y)
                {
                    //MinPoint坐标Y赋值为第i个ListFlow坐标Y
                    MinPoint.Y = FlowDataTemp.p0.Y;
                }
            }//step5、跳到step3

            //step6、返回true
            return true;
        }
        public bool GetCoordinateMinAndMaxFunc(ref PointF MinPoint, ref PointF MaxPoint)
        //函数名：bool GetCoordinateMinAndMaxFunc(ref point MinPoint,ref point MaxPoint )
        //输入：ListFlow待绘制图坐标数组
        //输出：上述坐标集合的最大值和最小值坐标MinPoint，MaxPoint
        //功能描述：获取ListFlow最大值和最小值
        {

            //算法
            //step1、数据有效性检查
            //若 ListFlow==null 则
            if (listFlow == null)
                return false;
            //若 ListFlow ==0则 
            if (listFlow.Count == 0)
                return false;

            //step2、MinPoint，MaxPoint初始化
            //MinPoint，MaxPoint的X坐标赋值为ListFlow[0]的p0坐标X
            MinPoint.X = ((FlowDataF)(listFlow[0])).p0.X;
            MaxPoint.X = ((FlowDataF)(listFlow[0])).p0.X;

            //MinPoint，MaxPoint的Y坐标赋值为ListFlow[0]的p0坐标Y
            MinPoint.Y = ((FlowDataF)(listFlow[0])).p0.Y;
            MaxPoint.Y = ((FlowDataF)(listFlow[0])).p0.Y;

            FlowDataF FlowDataTemp = new FlowDataF();//

            //Step3、数组循环
            //for i=0 to ListFlow.Length
            for (int i = 0; i < listFlow.Count; i++)
            {
                FlowDataTemp = (FlowDataF)(listFlow[i]);

                //step4、X坐标与最大和最小值比较
                //若 第i个ListFlow坐标X大于MaxPoint坐标X 则
                if (FlowDataTemp.p0.X > MaxPoint.X)
                {
                    //MaxPoint坐标X赋值为第i个ListFlow坐标X
                    MaxPoint.X = FlowDataTemp.p0.X;
                }

                //若 第i个ListFlow坐标X小于MinPoint坐标X 则
                if (FlowDataTemp.p0.X < MinPoint.X)
                {
                    //MinPoint坐标X赋值为第i个ListFlow坐标X
                    MinPoint.X = FlowDataTemp.p0.X;
                }


                //Y坐标与最大和最小值比较
                //若 第i个ListFlow坐标Y大于MaxPoint坐标Y 则
                if (FlowDataTemp.p0.Y > MaxPoint.Y)
                {
                    //MaxPoint坐标Y赋值为第i个ListFlow坐标Y
                    MaxPoint.Y = FlowDataTemp.p0.Y;
                }
                //若 第i个ListFlow坐标Y小于MinPoint坐标Y 则
                if (FlowDataTemp.p0.Y < MinPoint.Y)
                {
                    //MinPoint坐标Y赋值为第i个ListFlow坐标Y
                    MinPoint.Y = FlowDataTemp.p0.Y;
                }
            }//step5、跳到step3

            //step6、返回true
            return true;
        }


        //所有坐标平移计算
        bool CoordinateTranslationFunc(Point ShiftSize)
        //函数名：CoordinateTranslationFunc(point ShiftSize)
        //输入：ListFlow待绘制图坐标数组，移动大小Size
        //输出：更新后的ListFlow待绘制图坐标数组
        //功能描述：将数据坐标数据按向量Size进行平移
        {
            //算法
            //算法
            //step1、数据有效性检查
            //若 ListFlow==null 则
            if (listFlow == null)
                return false;
            //若 ListFlow ==0则 
            if (listFlow.Count == 0)
                return false;

            ArrayList ListFlowBackup = (ArrayList)listFlow.Clone();
            listFlow.Clear();
            FlowData FlowDataTemp = new FlowData();//

            //Step2、数组循环
            //for i=0 to ListFlow.Length
            for (int i = 0; i < ListFlowBackup.Count; i++)
            {
                FlowDataTemp = (FlowData)(ListFlowBackup[i]);

                //step3、坐标移动
                //P0赋值p0减去ShiftSize
                FlowDataTemp.p0.Offset(ShiftSize);

                //P1赋值p1减去ShiftSize
                FlowDataTemp.p1.Offset(ShiftSize);

                listFlow.Add(FlowDataTemp);
            }//step4、跳到step2

            //step5、返回true
            return true;
        }


        bool getScalingOfCanvasFunc(PointF p)
        {

            SizeF SizeActual = new SizeF(MaxPoint.X - MinPoint.X, MaxPoint.Y - MinPoint.Y);
            SizeF Scaling =new SizeF(SizeActual.Width/CanvasSize.X,SizeActual.Width/CanvasSize.Y);
            
            return true;
        }

       
        // 坐标轴显示处理
        private void DrawXYkd(Graphics dc)
        {
            int lineWidth, fontSize;
            lineWidth = zbDrawPara.zbDrawParamater.zbLineWidth;//获取线条宽度
            fontSize = zbDrawPara.zbDrawParamater.zbFontSize;//坐标标注字体大小
            //zbStart = new PointF(MinPoint.X,MinPoint.Y);
            //zbBound = new RectangleF(MinPoint, new SizeF(MaxPoint.X - MinPoint.X,MaxPoint.Y -MinPoint.Y));
            SizeF OffsetSize = new SizeF(MinPoint.X - LeftMargin, MinPoint.Y - TopMargin);//平移量
            PointF ZeroPoint = new PointF(LeftMargin,TopMargin);//画布原点位置
            SizeF BoxSize = new SizeF(MaxPoint.X - MinPoint.X, MaxPoint.Y - MinPoint.Y);//画布有效绘图区大小

            //Graphics dc = Graphics.FromImage(picture.Image);
            Pen pen = new Pen(Color.FromArgb(zbDrawPara.zbDrawParamater.zbcolor), lineWidth);
            System.Drawing.Font font = new System.Drawing.Font("Arial", fontSize); // 用 Arial 字体，10号字体进行标注

            //if ((zbDrawPara.zbDrawParamater.Xenable == true) | (zbDrawPara.zbDrawParamater.Yenable == true))
            //{
            //    dc.DrawLine(pen, new Point(zbStart.X, zbStart.Y), new Point(zbStart.X,zbBound.Height));
            //    dc.DrawLine(pen, new Point(zbStart.X, zbStart.Y), new Point(zbBound.Width,zbStart.Y));
            //} // 如要显示 X 或 Y 坐标，则要显示相应的 轴线

            if (zbDrawPara.zbDrawParamater.Xenable == true)
            {
                int widthStart = Convert.ToInt32(Math.Ceiling(MinPoint.X / zbDrawPara.zbDrawParamater.zbkdXAux));
                int widthN = Convert.ToInt32(MaxPoint.X / zbDrawPara.zbDrawParamater.zbkdXAux) ;//计算刻度个数

                dc.DrawLine(pen, ZeroPoint, new PointF(ZeroPoint.X +BoxSize.Width, ZeroPoint.Y));// 画横坐标轴线

                for (int i = widthStart; i <= widthN; i++)
                {
                    PointF WN1 = new PointF( i * zbDrawPara.zbDrawParamater.zbkdXAux, TopMargin ) ;

                    if ((WN1.X % zbDrawPara.zbDrawParamater.zbkdXMain) == 0)//主刻度线判断
                    {
                        if (zbDrawPara.zbDrawParamater.zbIsGrid == true)//网格线判断
                            dc.DrawLine(pen, new PointF(WN1.X - OffsetSize.Width, WN1.Y), new PointF(WN1.X - OffsetSize.Width, WN1.Y + MaxPoint.Y - MinPoint.Y));//画网格线
                        else
                            dc.DrawLine(pen, new PointF(WN1.X - OffsetSize.Width, WN1.Y), new PointF(WN1.X - OffsetSize.Width, WN1.Y+zbDrawPara.zbDrawParamater.zbkdLabelLengthMain ));//画主刻度线
                        if (i != widthStart)
                        {
                            string KeDuStr = WN1.X.ToString();
                            dc.DrawString(KeDuStr, font, pen.Brush, new PointF(WN1.X - OffsetSize.Width, WN1.Y));//标注主刻度值
                        }
                    }
                    else
                    {
                        if (i != widthStart)
                        {
                            dc.DrawLine(pen, new PointF(WN1.X - OffsetSize.Width, WN1.Y), new PointF(WN1.X - OffsetSize.Width, WN1.Y + zbDrawPara.zbDrawParamater.zbkdLabelLengthAux));//画次刻度线
                        }
                    }
                }
                    
            }
            if (zbDrawPara.zbDrawParamater.Yenable == true)
            {
                int heightStart = Convert.ToInt32(Math.Ceiling(MinPoint.Y / zbDrawPara.zbDrawParamater.zbkdYAux));
                int heightN = Convert.ToInt32(MaxPoint.Y/ zbDrawPara.zbDrawParamater.zbkdYAux);

                dc.DrawLine(pen, new PointF(LeftMargin, TopMargin), new PointF(LeftMargin, TopMargin + MaxPoint.Y-MinPoint .Y));// 画纵坐标轴线

                for (int i = heightStart; i <= heightN; i++)
                {
                    PointF HN1 = new PointF(LeftMargin, i * zbDrawPara.zbDrawParamater.zbkdYAux);

                    if ((HN1.Y % zbDrawPara.zbDrawParamater.zbkdYMain) == 0)//主刻度线判断
                    {
                        if (zbDrawPara.zbDrawParamater.zbIsGrid == true)//主网格线判断
                            dc.DrawLine(pen, new PointF(HN1.X, HN1.Y - OffsetSize.Height), new PointF(HN1.X + MaxPoint.X - MinPoint.X, HN1.Y - OffsetSize.Height));
                        else
                            dc.DrawLine(pen, new PointF(HN1.X, HN1.Y - OffsetSize.Height), new PointF(HN1.X + zbDrawPara.zbDrawParamater.zbkdLabelLengthMain, HN1.Y - OffsetSize.Height));
                        if (i != heightStart)
                        {
                            string KeDuStr = HN1.Y.ToString();
                            dc.DrawString(KeDuStr, font, pen.Brush, new PointF(HN1.X, HN1.Y - OffsetSize.Height));
                        }
                    }
                    else
                    {
                        if (i != heightStart)
                        {
                            dc.DrawLine(pen, new PointF(HN1.X, HN1.Y - OffsetSize.Height), new PointF(HN1.X + zbDrawPara.zbDrawParamater.zbkdLabelLengthAux, HN1.Y - OffsetSize.Height));
                        }
                    }
                }
            }
            font.Dispose();
            pen.Dispose();
            dc.Dispose();
        }
       



    }
}
