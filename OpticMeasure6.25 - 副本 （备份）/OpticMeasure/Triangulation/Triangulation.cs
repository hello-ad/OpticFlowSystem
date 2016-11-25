using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using OpticMeasure;
using Microsoft.DirectX;

namespace TriangulationFun
{
    class Triangulation
    {
        public int StepValue1 =1;//两点间隔值
        public int V1Row , V2Row , V3Row, V4Row, V1Col, V2Col , V3Col , V4Col;//顶点V1、V2、V3的行和列号
        public int V1IndexInTriangleVertexArray;//顶点V1在顶点数组里的索引
        public int V2IndexInTriangleVertexArray;//顶点V2在顶点数组里的索引
        public int V3IndexInTriangleVertexArray;//顶点V3在顶点数组里的索引
        public int V4IndexInTriangleVertexArray;//顶点V4在顶点数组里的索引
        public int PictureHight;//照片高度
        public int PictureWidth ;//照片宽度
        public TriangleVertex[] TriangleVertexArray;//三角形顶点数组
        public Triangle[] TriangleArray;//三角形表数组

        /*
        函数一：得到三角形顶点表
        功能：将像素点（阵列小圆点）装入顶点表TriangleVertexArray[] //（静态数组）中；
        输入：照片的宽度 PictureWidth，照片高度PictureHight
        输出：TriangleVertexArray
        */
        public void GetTrianglerVertexArrayFunc(int ReadPictureHight, int ReadPictureWidth, int StepValue)
        {
            int k = 0;//第k个顶点
            PictureHight = ReadPictureHight;
            PictureWidth = ReadPictureWidth;
            TriangleVertexArray = new TriangleVertex[0];//创建三角形顶点表数组
  
            // ----i、j 循环，将三角形顶点表装起------
            //  边界点没装

            for (int i = 1; i < PictureHight - 1; i = i + StepValue)
            {
                for (int j = 1; j < PictureWidth - 1; j = j + StepValue)
                {
                    Array.Resize(ref TriangleVertexArray, TriangleVertexArray.Length + 1);
                    TriangleVertexArray[k].D2V1.X = (float)(j + 0.5);//像素的中心横坐标
                    TriangleVertexArray[k].D2V1.Y = (float)(i + 0.5);//像素的中心列坐标
                    TriangleVertexArray[k].Index = i * PictureWidth + j;//像素在灰度数组中的索引
                    // 判断 该点是否是边界点
                    if ((i == 0) || (j == 0) || (i == PictureHight - 1) || (j == PictureWidth - 1))
                        TriangleVertexArray[k].isBoundaryPoint = true;
                    else
                        TriangleVertexArray[k].isBoundaryPoint = false;
                    k = k + 1;


                }

            }
            
        }


        /*
         函数二：得到三角形表函数
         函数名：GetTrianglerArrayFunc()
         功能：得到三角形表的数组，知道每个三角形顶点 V1、V2、V3在三角形顶点表里面的索引
         输入：TriangleVertexArray，照片的宽度 PictureWidth，照片高度PictureHight，点间距步长StepValue1 （不需要时候可以设为0）
         * 每4个点，剖分两个三角形
         * V1  V2  ...V2
         * V4  V3  ...V3
        */


        public void GetTrianglerArrayFunc()
        {
            int k = 0;//第k个三角形
            int i = 0;//第i行
            int j = 0;//第j列
            //int StepValue1 = 1;
            TriangleArray = new Triangle[0];//创建三角形表数组，定义长度有问题
            // i、j 循环，将三角形表装起
            for (; i < PictureHight - StepValue1; i = i + StepValue1)
            {
                V1Row = i;//
		        V2Row = V1Row;
		        V3Row = i + StepValue1;
                // 如果V3Row超出照片，把最后一行赋给V3Row
                if (V3Row >= PictureHight - 1)
                {
                    V3Row = PictureHight - 1;
                }
                    V4Row = V3Row;
                for (; j < PictureWidth - StepValue1; j = j + StepValue1)
                {
                    V1Col = j; V4Col = V1Col;
                    V2Col = j + StepValue1;
                    // 如果V2Col超出照片，把最后一行赋给V2Col
                    if (V2Col >= PictureWidth - StepValue1)
                    {
                        V2Col = PictureWidth - 1; 
                    }
                    V3Col = V2Col;

                    //根据V1、V2、V3和V4的行列计算V1IndexInTriangleVertexArray的函数，得到
                   V1IndexInTriangleVertexArray = V1Row * PictureWidth + V1Col;
                   V2IndexInTriangleVertexArray = V2Row * PictureWidth + V2Col;
                   V3IndexInTriangleVertexArray = V3Row * PictureWidth + V3Col;
                   V4IndexInTriangleVertexArray = V4Row * PictureWidth + V4Col;
                    //计算三角形表TriangleArray
                   Array.Resize(ref TriangleArray, TriangleArray.Length + 1);
                   TriangleArray[k].D2V1index = V1IndexInTriangleVertexArray;
                   TriangleArray[k].D2V2index = V3IndexInTriangleVertexArray;
                   TriangleArray[k].D2V3index = V2IndexInTriangleVertexArray;
                  
                   k++;
                   Array.Resize(ref TriangleArray, TriangleArray.Length+ 1);
                   TriangleArray[k].D2V1index = V1IndexInTriangleVertexArray;
                   TriangleArray[k].D2V2index = V4IndexInTriangleVertexArray;
                   TriangleArray[k].D2V3index = V3IndexInTriangleVertexArray;
                   
                  

                    //调用函数三
                   GenerateIndexOf4Points(i,j);
                   k++;

                }
                j = 0; 

            }
            //对TriangleVertexArray里每个TriangleIndexArrayList进行实例化
            for (int z = 0; z < TriangleVertexArray.Length; z++)
            {
                TriangleVertexArray[z].TriangleIndexArrayList = new ArrayList();
            }
            //循环对TriangleIndexArrayList加值
            for (int z=0; z < TriangleArray.Length; z++)
            {
                TriangleVertexArray[TriangleArray[z].D2V1index].TriangleIndexArrayList.Add(z);
                TriangleVertexArray[TriangleArray[z].D2V2index].TriangleIndexArrayList.Add(z);
                TriangleVertexArray[TriangleArray[z].D2V3index].TriangleIndexArrayList.Add(z);
            }

        }

        /*
            函数三：得到相邻4点的在TriangleVertexArray中的索引函数
            函数名：GenerateIndexOf4Points ()
            功能：得到相邻4点的在TriangleVertexArray中的索引
            输入：TriangleVertexArray，照片的宽度 PictureWidth，照片高度PictureHight，点间距步长StepValue1，
                  V1IndexInTriangleVertexArray，V2IndexInTriangleVertexArray，
                  V3IndexInTriangleVertexArray，V4IndexInTriangleVertexArray，i，j
            输出：V1IndexInTriangleVertexArray，V2IndexInTriangleVertexArray，       
         */

        public void GenerateIndexOf4Points(int i,int j)
        {
            // V1和V2（V4和V3）直接交换，无需重复计算
            V4IndexInTriangleVertexArray = V2IndexInTriangleVertexArray;
            V4IndexInTriangleVertexArray = V3IndexInTriangleVertexArray;
            //计算新V2IndexInTriangleVertexArray和V3IndexInTriangleVertexArray
            if (i <(PictureHight-StepValue1))
            {
                V2Row = i;
                V3Row = i+StepValue1;
            }
            if (V3Row>=PictureHight-1)
            {
                V3Row = PictureHight-1;
            }
            if (j<(PictureWidth-StepValue1))
            {
                V2Col = j+StepValue1;
            }
            if (V2Col >= PictureWidth-StepValue1)
            {
                V2Col = PictureWidth-1;
                V3Col = V2Col;
            }
            // 根据V2、V3的行和列算索引
            V2IndexInTriangleVertexArray= V2Row*PictureWidth+V2Col;
		    V3IndexInTriangleVertexArray= V3Row*PictureWidth+V3Col;
        }


        // destroy函数
        public void Destroy()
        {
           // TriangleVertexArray;//三角形顶点数组
        // TriangleArray;//三角形表数组
            if (TriangleVertexArray!=null)
            {
                if (TriangleVertexArray.Length>0)
                {
                    Array.Resize(ref TriangleVertexArray, 0);
                }
            }

            if (TriangleArray!=null)
            {
                if (TriangleArray.Length>0)
                {
                    Array.Resize(ref TriangleArray,0);
                }
            }
        }


    }
}
