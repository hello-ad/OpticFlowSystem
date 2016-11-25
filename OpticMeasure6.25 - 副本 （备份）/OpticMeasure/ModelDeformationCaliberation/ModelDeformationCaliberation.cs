using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpticMeasure.OptiClass;
using Microsoft.DirectX;
using System.Collections;
using OpticMeasure;
using TriangulationFun;
using System.Windows.Forms;
using OpticMeasure.Class2D;

namespace OpticMeasure.Model
{
    class ModelDeformationCaliberation
    {
        public deformationCaliberation[] DefCalRow;// 变形标定点行数组内容
        public int[] PointSetInUncodeBoundary;//包围盒里面点数组
        public Vector2 UncodePointCoordinate;// 非编码点的P的坐标
        public Vector2 UncodePoint3DCoordinate;// 非编码点的P的世界坐标
        public int IndexOfUncodePointTriangle;//包含非编码点的P三角形编号
        public double S_T;//剖分三角形的面积
        public double TriangleArea1, TriangleArea2, TriangleArea3;//P点构成的三个三角形面积
        public int Rowlength;//行的长度
        public int Collength; //列的长度
        public Vector2 MeanDistance;//标定点X、Y方向的平均距离
        public TriangleVertex[] TriangleVertexArray;
        public Triangle[] TriangleArray;
        public UncodeBoundaryStruct UncodeBoundary;//包围盒


        /*
         *函数一：读图片待测标记点函数 函数名：ReadPointsFun()
         *功能：读入照片待测点P，并将其显示在当前照片上，将P点的像素坐标赋值给UncodePointCoordinate
         */
        public void ReadPicturesFun()
        {
            // ReferencePictureDataStruct[] ReferencePictureDataArray = new ReferencePictureDataStruct[2];
            // 自己手动赋值
            UncodePoint3DCoordinate = new Vector2();
            UncodePointCoordinate = new Vector2();
            ReadProject read = new ReadProject();// 选点


            //UncodePointCoordinate.X = number[0];//需要插值的像素坐标1171.279,959.0428
            //UncodePointCoordinate.Y = number[1];



        }


        /*
         *函数三：赋世界坐标函数 函数名：GetThreeDdementionFunc()
         *功能：对选择的点赋世界坐标 
         */
        public void GetThreeDdementionFunc()
        {


            //Rowlength = 5;//行的长度 
            //Collength = 17;//列的长度
            //int i = 1;// 行系数变量
            //int k = 0;//点索引号
            int a = 13; //每个点之间距离
            int kk = 0;//

            float m = DefCalRow[0].threeDdemention.X = 0;//X轴坐标 (手动赋值进去)
            float n = DefCalRow[0].threeDdemention.Y = 0;//Y轴坐标 (手动赋值进去)
            //循环赋世界坐标
            for (int i = 0; i < Rowlength; i++)
            {
                for (int j = 0; j < Collength; j++)
                {
                    DefCalRow[kk].threeDdemention.X = i * a;
                    DefCalRow[kk].threeDdemention.Y = j * a;
                    kk++;
                }

            }

        }

        /*
          函数四：得到三角形表函数 函数名：CalculateTrianglerArrayFunc()
          功能：调用三角剖分函数，对选择的点进行三角剖分，得到三角形表TrianglarArray
         * 输入：标定点的像素坐标orgCoordinate
         * 输出：三角形表TrianglarArray  
         */
        public void CalculateTrianglerArrayFunc()
        {
            TriangleVertexArray = new TriangleVertex[DefCalRow.Length];
            int i = 0;// 点的编号
            int k = 0;//数组序号

            for (; i < DefCalRow.Length; i++)
            {
                TriangleVertexArray[k].D2V1 = DefCalRow[i].orgCoordinate;
                k++;
            }

            // 调用三角剖分函数
            Triangulation trian = new Triangulation();
            trian.PictureHight = Rowlength;
            trian.PictureWidth = Collength;
            trian.TriangleVertexArray = TriangleVertexArray;
            trian.GetTrianglerArrayFunc();
            TriangleArray = trian.TriangleArray;

            //消除对象
            trian = null;
        }

        /*整个程序总的流程函数
           函数五：计算待测点P世界坐标函数，函数名：CalcuateUncodePoint3DCoordinateFunc()
         * 功能：计算待测点P点的世界坐标
         
         */
        public void CalcuateUncodePointCoordinateFunc(int hang, int lie)
        {
            Rowlength = hang;
            Collength = lie;
            float[] number = { 


900.5765f,1096.195f,
709.568f,	1498.468f





                               };

            //入读照片(包括选点)
            //ReadPicturesFun();

            //赋世界坐标
            GetThreeDdementionFunc();
            if (number.Length != 2)
            {
                for (int i = 0; i < number.Length / 2; i++)
                {
                    UncodePointCoordinate.X = number[i * 2];
                    UncodePointCoordinate.Y = number[i * 2 + 1];
                    IndexOfUncodePointTriangle = 0;

                    //得到三角形表函数
                    CalculateTrianglerArrayFunc();

                    
                    //计算包围盒大小
                    CalcuateUncodeBoundaryFunc();

                    //计算哪些点在包围盒里面，存入点的编号到PointSetInUncodeBoundary数组中
                    FindPointInUncodeBoundaryFunc();

                    //判断待测点落在哪个三角形里面函数
                    GetPointInTrianglarArrayIndexFunc();

                    //计算P点三维插值坐标
                    CalcuateUncodePoint3DCoordinateFunc();
                }

                //清除number 数组
                Array.Resize(ref number, 0);
                Array.Resize(ref TriangleVertexArray, 0);
                Array.Resize(ref TriangleArray, 0);
            }
            else
            {
                UncodePointCoordinate.X = number[0];//需要插值的像素坐标1171.279,959.0428
                UncodePointCoordinate.Y = number[1];

                IndexOfUncodePointTriangle = 0;
                //得到三角形表函数
                CalculateTrianglerArrayFunc();

                //计算包围盒大小
                CalcuateUncodeBoundaryFunc();

                //计算哪些点在包围盒里面，存入点的编号到PointSetInUncodeBoundary数组中
                FindPointInUncodeBoundaryFunc();

                //判断待测点落在哪个三角形里面函数
                GetPointInTrianglarArrayIndexFunc();

                //计算P点三维插值坐标
                CalcuateUncodePoint3DCoordinateFunc();

                //清除number 数组
                Array.Resize(ref number, 0);
                Array.Resize(ref TriangleVertexArray, 0);
                Array.Resize(ref TriangleArray, 0);
            }

            ////得到三角形表函数
            //CalculateTrianglerArrayFunc();

            ////计算包围盒大小
            //CalcuateUncodeBoundaryFunc();

            ////计算哪些点在包围盒里面，存入点的编号到PointSetInUncodeBoundary数组中
            //FindPointInUncodeBoundaryFunc();

            ////判断待测点落在哪个三角形里面函数
            //GetPointInTrianglarArrayIndexFunc();

            ////计算P点三维插值坐标
            //CalcuateUncodePoint3DCoordinateFunc();

        }

        /*
         * 确定包围盒大小函数：函数名是CalcuateUncodeBoundaryFunc()
         */
        public void CalcuateUncodeBoundaryFunc()
        {
            float m = 1.50f, n = 1.50f; // m为竖向放大倍数，n为横向放大倍数（自己定义）
            //创建包围盒对象UncodeBoundary
            UncodeBoundary = new UncodeBoundaryStruct();

            // 计算MeanDistance：
            MeanDistance = new Vector2();
            MeanDistance.X = (DefCalRow[Collength - 1].orgCoordinate.X - DefCalRow[0].orgCoordinate.X) / (Collength - 1); //标定点X方向的平均距离
            MeanDistance.Y = (DefCalRow[Rowlength * Collength - 1].orgCoordinate.Y - DefCalRow[Collength].orgCoordinate.Y) / (Rowlength - 1); //标定点Y方向的平均距离
            //判断 n m 是否超出范围
            if (m > Rowlength || n > Collength)
            {
                MessageBox.Show("包围盒放大系数过大！");

            }
            else if (m < 0 || n < 0)
            {
                MessageBox.Show("包围盒放大系数过小！");
            }
            else
            {
                // 算包围盒边界4点的坐标
                UncodeBoundary.APointCoordinate.X = UncodePointCoordinate.X;
                UncodeBoundary.APointCoordinate.Y = UncodePointCoordinate.Y - m * MeanDistance.Y;
                UncodeBoundary.BPointCoordinate.X = UncodePointCoordinate.X - n * MeanDistance.X;
                UncodeBoundary.BPointCoordinate.Y = UncodePointCoordinate.Y;
                UncodeBoundary.CPointCoordinate.X = UncodePointCoordinate.X;
                UncodeBoundary.CPointCoordinate.Y = UncodePointCoordinate.Y + m * MeanDistance.Y;
                UncodeBoundary.DPointCoordinate.X = UncodePointCoordinate.X + n * MeanDistance.X;
                UncodeBoundary.DPointCoordinate.Y = UncodePointCoordinate.Y;

            }



        }

        /*
         * 此函数是判断哪些点落在包围盒里面的功能函数，
         * 函数名是FindPointInUncodeBoundaryFunc()，得到包围盒标定点集合数组Vector2[]PointSetInUncodeBoundary
         * 输入：标定点的像素坐标orgCoordinate
         * 输出：包围盒标定点集合Vector2[]PointSetInUncodeBoundary 
         */

        public void FindPointInUncodeBoundaryFunc()
        {

            if (PointSetInUncodeBoundary == null)
            {
                PointSetInUncodeBoundary = new int[0];
            }

            for (int i = 0; i < DefCalRow.Length; i++)
            {
                // 判断点是否在包围盒内
                if ((UncodeBoundary.BPointCoordinate.X <= DefCalRow[i].orgCoordinate.X) && (DefCalRow[i].orgCoordinate.X <= UncodeBoundary.DPointCoordinate.X) && (UncodeBoundary.APointCoordinate.Y <= DefCalRow[i].orgCoordinate.Y) && (DefCalRow[i].orgCoordinate.Y <= UncodeBoundary.CPointCoordinate.Y))
                {
                    //将点编号存到PointSetInUncodeBoundary数组中：

                    Array.Resize(ref PointSetInUncodeBoundary, (PointSetInUncodeBoundary.Length + 1));
                    PointSetInUncodeBoundary[PointSetInUncodeBoundary.Length - 1] = i;

                }

            }
            if (PointSetInUncodeBoundary.Length < 1)
            {
                MessageBox.Show(" 包围盒太小，请重新调整n,m 大小！");

            }

        }

        /*
           判断待测点落在哪个三角形里面函数
         * 该函数是判断P点落在哪个三角形里的功能函数，函数名：GetPointInTrianglarArrayIndexFunc ()，
         * 最后 得到落P点三角形的编号IndexOfUncodePointTriangle 
         * 输入：母板点数组PiontImageCoorditionArray、三角形表数组TriangleArray
         * 输出：三角形的编号IndexOfUncodePointTriangle 、P点构成的三个三角形面积public double TriangleArea1、TriangleArea2、TriangleArea3  
         */
        public void GetPointInTrianglarArrayIndexFunc()
        {

            //通过顶点找三角形编号
            ArrayList TriangleIndexList = new ArrayList();//三角形序号数组
            int length = PointSetInUncodeBoundary.Length;
            float x1, x2, x3, y1, y2, y3; //三角形三点的x y坐标
            double S;//计算P分别与三角形S_T三边构成三角形的面积和

            //根据PointSetInUncodeBoundary里点找第一个三角形序号：
            for (int k = 0; k < TriangleVertexArray[PointSetInUncodeBoundary[0]].TriangleIndexArrayList.Count; k++)
            {
                TriangleIndexList.Add(TriangleVertexArray[PointSetInUncodeBoundary[0]].TriangleIndexArrayList[k]);
            }

            for (int i = 1; i < length; i++)
            {
                for (int k = 0; k < TriangleVertexArray[PointSetInUncodeBoundary[i]].TriangleIndexArrayList.Count; k++)
                {
                    if (!(TriangleIndexList.Contains(TriangleVertexArray[PointSetInUncodeBoundary[i]].TriangleIndexArrayList[k])))
                    {
                        TriangleIndexList.Add(TriangleVertexArray[PointSetInUncodeBoundary[i]].TriangleIndexArrayList[k]);
                    }

                }

            }


            for (int j = 0; j < TriangleIndexList.Count; j++)
            {
                //计算剖分的三角形面积S_T，根据公式S=(1/2)*(x1y2+x2y3+x3y1-x1y3-x2y1-x3y2)，其中：S=(1/2)*(x1y2+x2y3+x3y1-x1y3-x2y1-x3y2)
                x1 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V1index].D2V1.X;
                x2 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V2index].D2V1.X;
                x3 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V3index].D2V1.X;
                y1 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V1index].D2V1.Y;
                y2 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V2index].D2V1.Y;
                y3 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V3index].D2V1.Y;
                S_T = 0.5 * System.Math.Abs(x1 * y2 + x2 * y3 + x3 * y1 - x1 * y3 - x2 * y1 - x3 * y2);//剖分的三角形面积

                //计算P分别与三角形S_T三边构成三角形的面积和S（S=S1+S2+S3）

                x1 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V1index].D2V1.X;
                x2 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V2index].D2V1.X;
                x3 = UncodePointCoordinate.X;
                y1 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V1index].D2V1.Y;
                y2 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V2index].D2V1.Y;
                y3 = UncodePointCoordinate.Y;
                TriangleArea1 = 0.5 * System.Math.Abs(x1 * y2 + x2 * y3 + x3 * y1 - x1 * y3 - x2 * y1 - x3 * y2);

                x1 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V1index].D2V1.X;
                x2 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V3index].D2V1.X;
                x3 = UncodePointCoordinate.X;
                y1 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V1index].D2V1.Y;
                y2 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V3index].D2V1.Y;
                y3 = UncodePointCoordinate.Y;
                TriangleArea2 = 0.5 * System.Math.Abs(x1 * y2 + x2 * y3 + x3 * y1 - x1 * y3 - x2 * y1 - x3 * y2);

                x1 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V2index].D2V1.X;
                x2 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V3index].D2V1.X;
                x3 = UncodePointCoordinate.X;
                y1 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V2index].D2V1.Y;
                y2 = TriangleVertexArray[TriangleArray[(int)TriangleIndexList[j]].D2V3index].D2V1.Y;
                y3 = UncodePointCoordinate.Y;
                TriangleArea3 = 0.5 * System.Math.Abs(x1 * y2 + x2 * y3 + x3 * y1 - x1 * y3 - x2 * y1 - x3 * y2);

                S = TriangleArea1 + TriangleArea2 + TriangleArea3;
                if (Math.Abs(S - S_T) < Math.Pow(10, -1))//面积误差精度不能给太大
                {
                    IndexOfUncodePointTriangle = (int)TriangleIndexList[j];
                    break;
                }

            }

        }

        /*
         该函数是计算P点三维插值坐标函数，函数名：CalcuateUncodePoint3DCoordinateFunc()
         * 最后 得到P点的三维坐标UncodePoint3DCoordinate
         * 输入：P点落在三角形的编号IndexOfUncodePointTriangle、母板点数组PiontImageCoorditionArray、三角形表数组TriangleArray
         * 输出：P点的三维坐标UncodePoint3DCoordinate    
         */
        public void CalcuateUncodePoint3DCoordinateFunc()
        {

            int k = IndexOfUncodePointTriangle;//P点落在三角形序号
            Vector2 V1, V2, V3;//三角形三个点的世界坐标
            double S1, S2, S3;//计算P分别与三角形S_T三边构成三角形的面积
            //判断k 是否为零，如果为零判断面积是否相等，如果相等，则可以继续计算，不等，不计算
            if (k == 0)
            {
                if (Math.Abs(TriangleArea1 + TriangleArea2 + TriangleArea3 - S_T) < Math.Pow(10, -1))
                {
                    //得到三点的三维坐标
                    V1 = DefCalRow[TriangleArray[k].D2V1index].threeDdemention;
                    V2 = DefCalRow[TriangleArray[k].D2V2index].threeDdemention;
                    V3 = DefCalRow[TriangleArray[k].D2V3index].threeDdemention;
                    //根据公式：(注意顺序，顺时针，V1、V2、V3顶点)
                    //OX=S3/S_T*1X+S1/S_T*2X+S2/S_T*3X
                    //OY=S3/S_T*1Y+S1/S_T*2Y+S2/S_T*3Y
                    S1 = TriangleArea1; S2 = TriangleArea2; S3 = TriangleArea3;

                    //计算待测点P的插值坐标
                    UncodePoint3DCoordinate.X = (float)((S3 / S_T) * V1.X + (S1 / S_T) * V3.X + (S2 / S_T) * (V2.X));
                    UncodePoint3DCoordinate.Y = (float)((S3 / S_T) * V1.Y + (S1 / S_T) * V3.Y + (S2 / S_T) * (V2.Y));

                }
                else
                {
                    MessageBox.Show("点超出模板范围！");
                }
            }
            if (k != 0)
            {
                //得到三点的三维坐标
                V1 = DefCalRow[TriangleArray[k].D2V1index].threeDdemention;
                V2 = DefCalRow[TriangleArray[k].D2V2index].threeDdemention;
                V3 = DefCalRow[TriangleArray[k].D2V3index].threeDdemention;
                //根据公式：(注意顺序，顺时针，V1、V2、V3顶点)
                //OX=S3/S_T*1X+S1/S_T*2X+S2/S_T*3X
                //OY=S3/S_T*1Y+S1/S_T*2Y+S2/S_T*3Y
                S1 = TriangleArea1; S2 = TriangleArea2; S3 = TriangleArea3;

                //计算待测点P的插值坐标
                UncodePoint3DCoordinate.X = (float)((S3 / S_T) * V1.X + (S1 / S_T) * V3.X + (S2 / S_T) * (V2.X));
                UncodePoint3DCoordinate.Y = (float)((S3 / S_T) * V1.Y + (S1 / S_T) * V3.Y + (S2 / S_T) * (V2.Y));
            }


        }



    }
}
