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

        public double awLength = 0.2; // ��ͷ�ߵĳ��ȣ� > 1.0 Ϊ�̶����ȣ� <= 1.0 Ϊ ֱ�߳��ȵı�������ͷ�߳��� = awLength * ֱ�߳���
        public double awAngle = 20.0; // ��ͷ��ֱ�ߵļнǣ�Ĭ��ֵΪ 15��
        public int awColor = Color.Blue.ToArgb(); //  ���ߵ���ɫ��Ĭ��Ϊ Blue
        public int awWidth = 2; // ���ߵĿ�ȣ�Ĭ��Ϊ 1 �� 1������
        public float arrowScale = 1.0f; // ��ͷ����
       
        public lineTypeEnum LineType = lineTypeEnum.Arrow;//0�������ͷֱ�ߣ�1��������޼�ͷ�߶�
        public int LeftMargin =50;//�ڲ��߾�Internal margin,��߾࣬��λ����
        public int RightMargin = 25;//�ڲ��߾�Internal margin,�ұ߾࣬��λ����
        public int TopMargin = 50;//�ڲ��߾�Internal margin,�ϱ߾࣬��λ����
        public int BottomMargin = 25;//�ڲ��߾�Internal margin,�±߾࣬��λ����
        public float Percent = 50f;//����ڻ�����������
        public bool ShowEnable = false;//��ͼʹ�ܱ���
        public bool NodesShowEnable = false;//�ڵ���ʾʹ��
        public PointF MinPoint = new PointF();//����������С������
        public PointF MaxPoint = new PointF();//����������������
        public Point CanvasSize = new Point(1000, 1000);
        public float VectorLength = 1;//�����Ŵ�����С����

        private RectangleF _zbBound = new RectangleF(0, 0, 1, 1);
        private bool _zbdisplay = false;
        private PointF _zbStart = new PointF(50, 50);
        public  PamaterSetting zbDrawPara =new PamaterSetting ();
      
        public bool zbDisplay
        {
            get { return _zbdisplay; }
            set { _zbdisplay = value;}
        }

        //��ʼ������
        public DrawFlow()
        {
            listFlow.Clear();
        }

        //��������
        ~DrawFlow()
        {
            listFlow.Clear();            
        }

        //��ȡ��̬���鳤��
        public int Length
        {
            get
            {
                return listFlow.Count;
            }
        }

        //������պ���
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

            //////��ʾ����
            //////showContour(ref rgbValues, AllpicturelunkuoArray[index].lunkuoArray);
            //////��ʾ������
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
            Image imageTemp = Image.FromFile(Application.StartupPath + @"\Cursor\�հ�.BMP");
            Bitmap curBitmap = new Bitmap(imageTemp,imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(curBitmap);

            DrawGraphics(g);

            picture.Image = curBitmap;

        }
        public void DrawFlowGraphicsF(PictureBox picture)
        {                  
            int imgWidth = CanvasSize.X, imgHeight = CanvasSize.Y;
            //GetCoordinateMinAndMaxFunc(ref MinPoint, ref MaxPoint);//�����Сֵ��ȡ����

            AutoSetImageSizeFunc(MinPoint, MaxPoint, ref imgWidth, ref imgHeight);//���û�����С

            //Image imageTemp = Image.FromFile(Application.StartupPath + @"\Cursor\�հ�.BMP");//�������ͼ��
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
        private PointF calcNewPointByLength(PointF p0, PointF p1)  // ��ָ���ļ�ͷ���ȱ���������ԭֱ�ߵ� �յ� λ������
        {
            double len = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // ֱ�ߵĳ���
            len = len * arrowScale;  // ��ͷ���Ȱ�ָ���ȱ����
            double x, y;
            if (p1.X != p0.X)
            {
                double k = ((double)(p1.Y - p0.Y)) / (p1.X - p0.X);  // ֱ�ߵ�б��

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
            Pen pen = new Pen(Color.FromArgb(awColor), awWidth); // ���� ��ע��ɫ����ͷ�߿��
            FlowData fd;
            float lengthtemp=0f;
            //Font FontTemp = new Font (SystemFonts.DefaultFont,System.Drawing.FontStyle.Regular);
            System.Drawing.Font FontTemp = new System.Drawing.Font("����", 9);
            Brush BrushTemp = pen.Brush;

            for (int i = 0; i < listFlow.Count; i++)
            {
                fd = (FlowData)listFlow[i];
                if (LineType == lineTypeEnum.Arrow)
                {
                    DrawAnyArrow(g, pen, fd.p0, fd.p1, awLength, awAngle, awColor, awWidth);
                }

                g.DrawLine(pen, fd.p0, fd.p1); // ��ֱ��;

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
            Pen pen = new Pen(Color.FromArgb(awColor), awWidth); // ���� ��ע��ɫ����ͷ�߿��
            FlowDataF fd;
            System.Drawing.Font FontTemp = new System.Drawing.Font("����", 3);
            Brush BrushTemp = pen.Brush;
            
            SizeF OffsetSize = new SizeF(MinPoint.X - LeftMargin, MinPoint.Y - TopMargin);

             for (int i = 0; i < listFlow.Count; i++)
            {
                fd = (FlowDataF)listFlow[i];

                //����ƽ��
                fd.p0 -= OffsetSize; fd.p1 -= OffsetSize;
                PointF newPos = calcNewPointByLength(fd.p0, fd.p1);

                fd.p1.X = newPos.X;
                fd.p1.Y = newPos.Y;

                if (LineType == lineTypeEnum.Arrow)
                {
                    DrawAnyArrow(g, pen, fd.p0, fd.p1, awLength, awAngle, awColor, awWidth);//���Ƽ�ͷ 
                    g.DrawLine(pen, fd.p0, fd.p1); // ��ֱ��;

                }

                if (LineType == lineTypeEnum.Line)
                {                    
                    g.DrawLine(pen, fd.p0, fd.p1); // ��ֱ��;
                }

                if (NodesShowEnable == true)
                {
                    RectangleF rect = new RectangleF(fd.p0 - new SizeF(2, 2), new SizeF(4, 4));
                    g.FillRectangle(BrushTemp, rect);//���������
                }               
                
            }
            BrushTemp.Dispose();
            FontTemp.Dispose();
            pen.Dispose();
        }

        private void DrawAnyArrow(Graphics g, Point p0, Point p1, double arrowLength, double arrowAngle, int arrowColor, int arrowWidth)
        {
            // p0,p1 ��Ҫ����ֱ�ߵ���������
            // arrowLength ����ͷ�ߵĳ��ȣ� > 1.0 Ϊ�̶����ȣ� <= 1.0 Ϊ ֱ�߳��ȵı�������ͷ�߳��� = arrowLength * ֱ�߳���
            // arrowAngle ����ͷ��ֱ�ߵĽǶ�
            // arrowColor �����ߵ���ɫ
            // arrowWidth �����ߵĿ��

            ////���ڵ�(x0,y0)ָ��(x1,y1)�ļ�ͷ�ߣ���(x1,y1)Ϊԭ�㣬�ȼ����(x1,y1) 
            ////ָ��(x0,y0)�ĵ�λ��������ͷ����ֱ�ߵĽǶ�Ϊd����ô���ǽ�������� 
            ////��ת�Ƕ�d�ͣ�d��������ת�Ĺ�ʽΪ�� 
            ////(a+b*i)*(cos(d)+i*sin(d))   =   a*cos(d)-b*sin(d)   +   i*(a*sin(d)+b*cos(d)) 
            ////��Ϊ�����������(x1,y1)Ϊԭ��ĵ�λ���������Ի���Ҫ����һ�����ȣ� 
            ////��ͷ�ߵĳ��ȣ���Ȼ�����ƫ�ƾͿ����ˡ�Ҳ���ǻ��ļ�ͷ�ߵĶ��������Ϊ 
            ////x=x1+L*(a*cos(d)-b*sin(d)),   y=y1+L*(a*sin(d)+b*cos(d))�� 
            ////��һ����������꣬�������sin(d)����   -sin(d)   �Ϳ����ˡ� 
            ////���湫ʽ�е�aΪ(x0-x1)/L0,   bΪ(y0-y1)/L0��L0��(x0,y0)��(x1,y1)�ĳ��ȣ� 
            ////L�Ǽ�ͷ�ߵĳ���(�Լ��趨)�� 

            Pen pen = new Pen(Color.FromArgb(arrowColor), arrowWidth); // ���� ��ע��ɫ����ͷ�߿��
            Brush brush = pen.Brush;

            double L0 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // ����ֱ�ߵĳ���
            double a = (p0.X - p1.X) / L0;
            double b = (p0.Y - p1.Y) / L0; // ��λ��������������
            double d = arrowAngle / 180 * PI; // ��ͷ����ֱ�ߵĽǶ�Ϊ arrowAngle
            double length = 0.0;
  
            if (arrowLength > 1.0)
                length = arrowLength;
            else
                length = arrowLength * L0;
            // ��������Ĳ��� arrowLength > 1.0����Ϊ�ǹ̶����ȵļ�ͷ����
            // ��� arrowLength <= 1.0�� ���ͷ����Ϊ ֱ�߳��� * arrowLength��

            int P2x = (int)(p1.X + arrowLength * (a * Math.Cos(d) - b * Math.Sin(d)));
            int P2y = (int)(p1.Y + arrowLength * (a * Math.Sin(d) + b * Math.Cos(d)));

            int P3x = (int)(p1.X + arrowLength * (a * Math.Cos(d) - b * Math.Sin(-d)));
            int P3y = (int)(p1.Y + arrowLength * (a * Math.Sin(-d) + b * Math.Cos(d)));

            g.DrawLine(pen, p0, p1); // ��ֱ��;
            g.DrawPolygon(pen, new Point[] { new Point(P2x, P2y), new Point(P3x, P3y), p1 });
            g.FillPolygon(brush, new Point[] { new Point(P2x, P2y), new Point(P3x, P3y), p1 });
            //g.DrawLine(pen, new Point(P2x, P2y), p1); // ����ͷ��һ����
            //g.DrawLine(pen, new Point(P3x, P3y), p1); // ����ͷ����һ����

            brush.Dispose();
            pen.Dispose();

        }
        private void DrawAnyArrow(Graphics g, PointF p0, PointF p1, double arrowLength, double arrowAngle, int arrowColor, int arrowWidth)
        {            
            Pen pen = new Pen(Color.FromArgb(arrowColor), arrowWidth); // ���� ��ע��ɫ����ͷ�߿��
            Brush brush = pen.Brush;

            double L0 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // ����ֱ�ߵĳ���
            double a = (p0.X - p1.X) / L0;
            double b = (p0.Y - p1.Y) / L0; // ��λ��������������
            double d = arrowAngle / 180 * PI; // ��ͷ����ֱ�ߵĽǶ�Ϊ arrowAngle
            double length = 0.0;

            if (arrowLength > 1.0)
                length = arrowLength;
            else
                length = arrowLength * L0;
            // ��������Ĳ��� arrowLength > 1.0����Ϊ�ǹ̶����ȵļ�ͷ����
            // ��� arrowLength <= 1.0�� ���ͷ����Ϊ ֱ�߳��� * arrowLength��

            int P2x = (int)(p1.X + arrowLength * (a * Math.Cos(d) - b * Math.Sin(d)));
            int P2y = (int)(p1.Y + arrowLength * (a * Math.Sin(d) + b * Math.Cos(d)));

            int P3x = (int)(p1.X + arrowLength * (a * Math.Cos(d) - b * Math.Sin(-d)));
            int P3y = (int)(p1.Y + arrowLength * (a * Math.Sin(-d) + b * Math.Cos(d)));

            g.DrawLine(pen, p0, p1); // ��ֱ��;
            g.DrawPolygon(pen, new PointF[] { new PointF(P2x, P2y), new PointF(P3x, P3y), p1 });
            g.FillPolygon(brush, new PointF[] { new PointF(P2x, P2y), new PointF(P3x, P3y), p1 });
            //g.DrawLine(pen, new Point(P2x, P2y), p1); // ����ͷ��һ����
            //g.DrawLine(pen, new Point(P3x, P3y), p1); // ����ͷ����һ����

            brush.Dispose();
            pen.Dispose();

        }
        
        private void DrawAnyArrow(Graphics g,Pen pen, Point p0, Point p1, double arrowLength, double arrowAngle, int arrowColor, int arrowWidth)
        {
            // p0,p1 ��Ҫ����ֱ�ߵ���������
            // arrowLength ����ͷ�ߵĳ���
            // arrowAngle ����ͷ��ֱ�ߵĽǶ�
            // arrowColor �����ߵ���ɫ
            // arrowWidth �����ߵĿ��

            ////���ڵ�(x0,y0)ָ��(x1,y1)�ļ�ͷ�ߣ���(x1,y1)Ϊԭ�㣬�ȼ����(x1,y1) 
            ////ָ��(x0,y0)�ĵ�λ��������ͷ����ֱ�ߵĽǶ�Ϊd����ô���ǽ�������� 
            ////��ת�Ƕ�d�ͣ�d��������ת�Ĺ�ʽΪ�� 
            ////(a+b*i)*(cos(d)+i*sin(d))   =   a*cos(d)-b*sin(d)   +   i*(a*sin(d)+b*cos(d)) 
            ////��Ϊ�����������(x1,y1)Ϊԭ��ĵ�λ���������Ի���Ҫ����һ�����ȣ� 
            ////��ͷ�ߵĳ��ȣ���Ȼ�����ƫ�ƾͿ����ˡ�Ҳ���ǻ��ļ�ͷ�ߵĶ��������Ϊ 
            ////x=x1+L*(a*cos(d)-b*sin(d)),   y=y1+L*(a*sin(d)+b*cos(d))�� 
            ////��һ����������꣬�������sin(d)����   -sin(d)   �Ϳ����ˡ� 
            ////���湫ʽ�е�aΪ(x0-x1)/L0,   bΪ(y0-y1)/L0��L0��(x0,y0)��(x1,y1)�ĳ��ȣ� 
            ////L�Ǽ�ͷ�ߵĳ���(�Լ��趨)�� 
            double length = 0;

 
            //Pen pen = new Pen(Color.FromArgb(arrowColor), arrowWidth); // ���� ��ע��ɫ����ͷ�߿��  ��ʹ��ǰ���� Pen ����
            Brush brush = pen.Brush;
            double L0 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // ����ֱ�ߵĳ���

            if (!((p0.Equals(new Point(0, 0))) || (p1.Equals(new Point(0, 0)))) & (L0 != 0.0))
            {
                double a = (p0.X - p1.X) / L0;
                double b = (p0.Y - p1.Y) / L0; // ��λ��������������
                double d = arrowAngle / 180 * PI; // ��ͷ����ֱ�ߵĽǶ�Ϊ arrowAngle

                if (arrowLength > 1.0)
                    length = arrowLength;
                else
                    length = arrowLength * L0;
                // ��������Ĳ��� arrowLength > 1.0����Ϊ�ǹ̶����ȵļ�ͷ����
                // ��� arrowLength <= 1.0�� ���ͷ����Ϊ ֱ�߳��� * arrowLength��

                int P2x = (int)(p1.X + length * (a * Math.Cos(d) - b * Math.Sin(d)));
                int P2y = (int)(p1.Y + length * (a * Math.Sin(d) + b * Math.Cos(d)));

                int P3x = (int)(p1.X + length * (a * Math.Cos(d) - b * Math.Sin(-d)));
                int P3y = (int)(p1.Y + length * (a * Math.Sin(-d) + b * Math.Cos(d)));

                g.DrawLine(pen, p0, p1); // ��ֱ��;
                g.DrawPolygon(pen, new Point[] { new Point(P2x, P2y), new Point(P3x, P3y), p1 });
                g.FillPolygon(brush, new Point[] { new Point(P2x, P2y), new Point(P3x, P3y), p1 });
                //g.DrawLine(pen, new Point(P2x, P2y), p1); // ����ͷ��һ����
                //g.DrawLine(pen, new Point(P3x, P3y), p1); // ����ͷ����һ����
            }
            else
            {
                //System.Drawing.Font font = new System.Drawing.Font("Arial", 10); // �� Arial ���壬10��������б�ע
                //if (p0.Equals(new Point(0, 0)))
                //{
                //    g.DrawString("��", font, brush, p1);
                //    //g.DrawLine(pen,new Point(p1.X,p1.Y-1),new Point(p1.X,p1.Y+1));
                //    //g.DrawLine(pen,new Point(p1.X-1,p1.Y),new Point(p1.X+1,p1.Y));
                //}
                //if (p1.Equals(new Point(0, 0)))
                //{
                //    g.DrawString("��", font, brush, p0);
                //    //g.DrawLine(pen, new Point(p0.X, p0.Y - 1), new Point(p0.X, p0.Y + 1));
                //    //g.DrawLine(pen, new Point(p0.X - 1, p0.Y), new Point(p0.X + 1, p0.Y));
                //}
            }
            brush.Dispose();
            //pen.Dispose();  Pen ������ʹ��ǰ����

        }
        private void DrawAnyArrow(Graphics g, Pen pen, PointF p0, PointF p1, double arrowLength, double arrowAngle, int arrowColor, int arrowWidth)
        { 
            double length = 0;


            //Pen pen = new Pen(Color.FromArgb(arrowColor), arrowWidth); // ���� ��ע��ɫ����ͷ�߿��  ��ʹ��ǰ���� Pen ����
            Brush brush = pen.Brush;
            double L0 = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2)); // ����ֱ�ߵĳ���

            if (!((p0.Equals(new PointF(0, 0))) || (p1.Equals(new PointF(0, 0)))) & (L0 != 0.0))
            {
                double a = (p0.X - p1.X) / L0;
                double b = (p0.Y - p1.Y) / L0; // ��λ��������������
                double d = arrowAngle / 180 * PI; // ��ͷ����ֱ�ߵĽǶ�Ϊ arrowAngle

                if (arrowLength > 1.0)
                    length = arrowLength;
                else
                    length = arrowLength * L0;
                // ��������Ĳ��� arrowLength > 1.0����Ϊ�ǹ̶����ȵļ�ͷ����
                // ��� arrowLength <= 1.0�� ���ͷ����Ϊ ֱ�߳��� * arrowLength��

                //length = length * arrowScale;

                float P2x = (float)(p1.X + length * (a * Math.Cos(d) - b * Math.Sin(d)));
                float P2y = (float)(p1.Y + length * (a * Math.Sin(d) + b * Math.Cos(d)));

                float P3x = (float)(p1.X + length * (a * Math.Cos(d) - b * Math.Sin(-d)));
                float P3y = (float)(p1.Y + length * (a * Math.Sin(-d) + b * Math.Cos(d)));

                g.DrawLine(pen, p0, p1); // ����ͷֱ��;
                g.DrawPolygon(pen, new PointF[] { new PointF(P2x, P2y), new PointF(P3x, P3y), p1 });
                g.FillPolygon(brush, new PointF[] { new PointF(P2x, P2y), new PointF(P3x, P3y), p1 });
                //g.DrawLine(pen, new Point(P2x, P2y), p1); // ����ͷ��һ����
                //g.DrawLine(pen, new Point(P3x, P3y), p1); // ����ͷ����һ����
            }
            else
            {
                
            }
            brush.Dispose();
            //pen.Dispose();  Pen ������ʹ��ǰ����

        }



        //�õ����꼯�ϵ����ֵ����Сֵ
        public bool GetCoordinateMinAndMaxFunc(ref Point MinPoint, ref Point MaxPoint)
        //��������bool GetCoordinateMinAndMaxFunc(ref point MinPoint,ref point MaxPoint )
        //���룺ListFlow������ͼ��������
        //������������꼯�ϵ����ֵ����Сֵ����MinPoint��MaxPoint
        //������������ȡListFlow���ֵ����Сֵ
        {

            //�㷨
            //step1��������Ч�Լ��
            //�� ListFlow==null ��
            if (listFlow == null)
                return false;
            //�� ListFlow ==0�� 
            if (listFlow.Count == 0)
                return false;

            //step2��MinPoint��MaxPoint��ʼ��
            //MinPoint��MaxPoint��X���긳ֵΪListFlow[0]��p0����X
            MinPoint.X = ((FlowData)(listFlow[0])).p0.X;
            MaxPoint.X = ((FlowData)(listFlow[0])).p0.X;

            //MinPoint��MaxPoint��Y���긳ֵΪListFlow[0]��p0����Y
            MinPoint.Y = ((FlowData)(listFlow[0])).p0.Y;
            MaxPoint.Y = ((FlowData)(listFlow[0])).p0.Y;

            FlowData FlowDataTemp = new FlowData();//

            //Step3������ѭ��
            //for i=0 to ListFlow.Length
            for (int i = 0; i < listFlow.Count; i++)
            {
                FlowDataTemp = (FlowData)(listFlow[i]);

                //step4��X������������Сֵ�Ƚ�
                //�� ��i��ListFlow����X����MaxPoint����X ��
                if (FlowDataTemp.p0.X > MaxPoint.X)
                {
                    //MaxPoint����X��ֵΪ��i��ListFlow����X
                    MaxPoint.X = FlowDataTemp.p0.X;
                }

                //�� ��i��ListFlow����XС��MinPoint����X ��
                if (FlowDataTemp.p0.X < MinPoint.X)
                {
                    //MinPoint����X��ֵΪ��i��ListFlow����X
                    MinPoint.X = FlowDataTemp.p0.X;
                }


                //Y������������Сֵ�Ƚ�
                //�� ��i��ListFlow����Y����MaxPoint����Y ��
                if (FlowDataTemp.p0.Y > MaxPoint.Y)
                {
                    //MaxPoint����Y��ֵΪ��i��ListFlow����Y
                    MaxPoint.Y = FlowDataTemp.p0.Y;
                }
                //�� ��i��ListFlow����YС��MinPoint����Y ��
                if (FlowDataTemp.p0.Y < MinPoint.Y)
                {
                    //MinPoint����Y��ֵΪ��i��ListFlow����Y
                    MinPoint.Y = FlowDataTemp.p0.Y;
                }
            }//step5������step3

            //step6������true
            return true;
        }
        public bool GetCoordinateMinAndMaxFunc(ref PointF MinPoint, ref PointF MaxPoint)
        //��������bool GetCoordinateMinAndMaxFunc(ref point MinPoint,ref point MaxPoint )
        //���룺ListFlow������ͼ��������
        //������������꼯�ϵ����ֵ����Сֵ����MinPoint��MaxPoint
        //������������ȡListFlow���ֵ����Сֵ
        {

            //�㷨
            //step1��������Ч�Լ��
            //�� ListFlow==null ��
            if (listFlow == null)
                return false;
            //�� ListFlow ==0�� 
            if (listFlow.Count == 0)
                return false;

            //step2��MinPoint��MaxPoint��ʼ��
            //MinPoint��MaxPoint��X���긳ֵΪListFlow[0]��p0����X
            MinPoint.X = ((FlowDataF)(listFlow[0])).p0.X;
            MaxPoint.X = ((FlowDataF)(listFlow[0])).p0.X;

            //MinPoint��MaxPoint��Y���긳ֵΪListFlow[0]��p0����Y
            MinPoint.Y = ((FlowDataF)(listFlow[0])).p0.Y;
            MaxPoint.Y = ((FlowDataF)(listFlow[0])).p0.Y;

            FlowDataF FlowDataTemp = new FlowDataF();//

            //Step3������ѭ��
            //for i=0 to ListFlow.Length
            for (int i = 0; i < listFlow.Count; i++)
            {
                FlowDataTemp = (FlowDataF)(listFlow[i]);

                //step4��X������������Сֵ�Ƚ�
                //�� ��i��ListFlow����X����MaxPoint����X ��
                if (FlowDataTemp.p0.X > MaxPoint.X)
                {
                    //MaxPoint����X��ֵΪ��i��ListFlow����X
                    MaxPoint.X = FlowDataTemp.p0.X;
                }

                //�� ��i��ListFlow����XС��MinPoint����X ��
                if (FlowDataTemp.p0.X < MinPoint.X)
                {
                    //MinPoint����X��ֵΪ��i��ListFlow����X
                    MinPoint.X = FlowDataTemp.p0.X;
                }


                //Y������������Сֵ�Ƚ�
                //�� ��i��ListFlow����Y����MaxPoint����Y ��
                if (FlowDataTemp.p0.Y > MaxPoint.Y)
                {
                    //MaxPoint����Y��ֵΪ��i��ListFlow����Y
                    MaxPoint.Y = FlowDataTemp.p0.Y;
                }
                //�� ��i��ListFlow����YС��MinPoint����Y ��
                if (FlowDataTemp.p0.Y < MinPoint.Y)
                {
                    //MinPoint����Y��ֵΪ��i��ListFlow����Y
                    MinPoint.Y = FlowDataTemp.p0.Y;
                }
            }//step5������step3

            //step6������true
            return true;
        }


        //��������ƽ�Ƽ���
        bool CoordinateTranslationFunc(Point ShiftSize)
        //��������CoordinateTranslationFunc(point ShiftSize)
        //���룺ListFlow������ͼ�������飬�ƶ���СSize
        //��������º��ListFlow������ͼ��������
        //�����������������������ݰ�����Size����ƽ��
        {
            //�㷨
            //�㷨
            //step1��������Ч�Լ��
            //�� ListFlow==null ��
            if (listFlow == null)
                return false;
            //�� ListFlow ==0�� 
            if (listFlow.Count == 0)
                return false;

            ArrayList ListFlowBackup = (ArrayList)listFlow.Clone();
            listFlow.Clear();
            FlowData FlowDataTemp = new FlowData();//

            //Step2������ѭ��
            //for i=0 to ListFlow.Length
            for (int i = 0; i < ListFlowBackup.Count; i++)
            {
                FlowDataTemp = (FlowData)(ListFlowBackup[i]);

                //step3�������ƶ�
                //P0��ֵp0��ȥShiftSize
                FlowDataTemp.p0.Offset(ShiftSize);

                //P1��ֵp1��ȥShiftSize
                FlowDataTemp.p1.Offset(ShiftSize);

                listFlow.Add(FlowDataTemp);
            }//step4������step2

            //step5������true
            return true;
        }


        bool getScalingOfCanvasFunc(PointF p)
        {

            SizeF SizeActual = new SizeF(MaxPoint.X - MinPoint.X, MaxPoint.Y - MinPoint.Y);
            SizeF Scaling =new SizeF(SizeActual.Width/CanvasSize.X,SizeActual.Width/CanvasSize.Y);
            
            return true;
        }

       
        // ��������ʾ����
        private void DrawXYkd(Graphics dc)
        {
            int lineWidth, fontSize;
            lineWidth = zbDrawPara.zbDrawParamater.zbLineWidth;//��ȡ�������
            fontSize = zbDrawPara.zbDrawParamater.zbFontSize;//�����ע�����С
            //zbStart = new PointF(MinPoint.X,MinPoint.Y);
            //zbBound = new RectangleF(MinPoint, new SizeF(MaxPoint.X - MinPoint.X,MaxPoint.Y -MinPoint.Y));
            SizeF OffsetSize = new SizeF(MinPoint.X - LeftMargin, MinPoint.Y - TopMargin);//ƽ����
            PointF ZeroPoint = new PointF(LeftMargin,TopMargin);//����ԭ��λ��
            SizeF BoxSize = new SizeF(MaxPoint.X - MinPoint.X, MaxPoint.Y - MinPoint.Y);//������Ч��ͼ����С

            //Graphics dc = Graphics.FromImage(picture.Image);
            Pen pen = new Pen(Color.FromArgb(zbDrawPara.zbDrawParamater.zbcolor), lineWidth);
            System.Drawing.Font font = new System.Drawing.Font("Arial", fontSize); // �� Arial ���壬10��������б�ע

            //if ((zbDrawPara.zbDrawParamater.Xenable == true) | (zbDrawPara.zbDrawParamater.Yenable == true))
            //{
            //    dc.DrawLine(pen, new Point(zbStart.X, zbStart.Y), new Point(zbStart.X,zbBound.Height));
            //    dc.DrawLine(pen, new Point(zbStart.X, zbStart.Y), new Point(zbBound.Width,zbStart.Y));
            //} // ��Ҫ��ʾ X �� Y ���꣬��Ҫ��ʾ��Ӧ�� ����

            if (zbDrawPara.zbDrawParamater.Xenable == true)
            {
                int widthStart = Convert.ToInt32(Math.Ceiling(MinPoint.X / zbDrawPara.zbDrawParamater.zbkdXAux));
                int widthN = Convert.ToInt32(MaxPoint.X / zbDrawPara.zbDrawParamater.zbkdXAux) ;//����̶ȸ���

                dc.DrawLine(pen, ZeroPoint, new PointF(ZeroPoint.X +BoxSize.Width, ZeroPoint.Y));// ������������

                for (int i = widthStart; i <= widthN; i++)
                {
                    PointF WN1 = new PointF( i * zbDrawPara.zbDrawParamater.zbkdXAux, TopMargin ) ;

                    if ((WN1.X % zbDrawPara.zbDrawParamater.zbkdXMain) == 0)//���̶����ж�
                    {
                        if (zbDrawPara.zbDrawParamater.zbIsGrid == true)//�������ж�
                            dc.DrawLine(pen, new PointF(WN1.X - OffsetSize.Width, WN1.Y), new PointF(WN1.X - OffsetSize.Width, WN1.Y + MaxPoint.Y - MinPoint.Y));//��������
                        else
                            dc.DrawLine(pen, new PointF(WN1.X - OffsetSize.Width, WN1.Y), new PointF(WN1.X - OffsetSize.Width, WN1.Y+zbDrawPara.zbDrawParamater.zbkdLabelLengthMain ));//�����̶���
                        if (i != widthStart)
                        {
                            string KeDuStr = WN1.X.ToString();
                            dc.DrawString(KeDuStr, font, pen.Brush, new PointF(WN1.X - OffsetSize.Width, WN1.Y));//��ע���̶�ֵ
                        }
                    }
                    else
                    {
                        if (i != widthStart)
                        {
                            dc.DrawLine(pen, new PointF(WN1.X - OffsetSize.Width, WN1.Y), new PointF(WN1.X - OffsetSize.Width, WN1.Y + zbDrawPara.zbDrawParamater.zbkdLabelLengthAux));//���ο̶���
                        }
                    }
                }
                    
            }
            if (zbDrawPara.zbDrawParamater.Yenable == true)
            {
                int heightStart = Convert.ToInt32(Math.Ceiling(MinPoint.Y / zbDrawPara.zbDrawParamater.zbkdYAux));
                int heightN = Convert.ToInt32(MaxPoint.Y/ zbDrawPara.zbDrawParamater.zbkdYAux);

                dc.DrawLine(pen, new PointF(LeftMargin, TopMargin), new PointF(LeftMargin, TopMargin + MaxPoint.Y-MinPoint .Y));// ������������

                for (int i = heightStart; i <= heightN; i++)
                {
                    PointF HN1 = new PointF(LeftMargin, i * zbDrawPara.zbDrawParamater.zbkdYAux);

                    if ((HN1.Y % zbDrawPara.zbDrawParamater.zbkdYMain) == 0)//���̶����ж�
                    {
                        if (zbDrawPara.zbDrawParamater.zbIsGrid == true)//���������ж�
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
