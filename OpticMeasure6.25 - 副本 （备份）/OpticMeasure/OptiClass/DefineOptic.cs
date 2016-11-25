using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using System.Collections;
using System.Drawing;

namespace OpticMeasure
{
    public struct PictureOpticflowDataResultStruct  //光流法结果数据结构
    {
        public string PictureLujin; //当前照片路径
        public int PictureIndex; //当前照片编号
        public int ReferencePictureIndexInReferencePictureDataArray; //参考照片（即母版）数组中的编号
        public OpticflowResultForPixelGrey[]  OpticflowResultFOfPixelArray; //像素的光流结果数组
        public OpticflowResultForPointsGrey[] OpticflowResultFOfPointsArray; //点的光流结果数组；
        public Vector2 [] PictureMarkpointsArray; //照片标记点坐标数组，用来旋转平移参考照片（即母版），以消除振动
    }

    public struct OpticflowResultForPixelGrey  //光流结果像素灰度数据结构
    {
        public int PixIndexInReferencePicture; //在参考照片（即母版）灰度数组中的编号
        public float  Greyvalue; // 显示的灰度值结果
        public Vector2  Grad;  //灰度梯度
    }
    public struct OpticflowResultForPointsGrey//光流结果像点灰度数据结构
    {
        public int PointIndexInReferencePicture; //参考照片（即母版）点数组中编号
        public  Vector2  Grad;  //灰度梯度
        public Vector2 pointCoordinate;//点坐标
    }
    public struct Apicturegrayinformation
    {
        public byte currentgrayValue; //当前灰度(拉伸)
        public float OrigingrayValue; //原始灰度
    }
    public struct ReferencePictureDataStruct  //参考照片（即母版）数据的数据结构
    {
        public int ReferencePictureIndex; //参考照片（即母版）的编号
        public String ReferencePictureLuJin;  //参考照片（即母版）的文件路径
        public Vector2[]PiontImageCoorditionArray; //参考照片（即母版）点坐标的数组或模型变形测量的标定板上标定点
        public int[] IndexArray;//点在灰度数组中的位置
        public Triangle[] TriangleArray ; //PiontImageCoorditionArray中点三角剖分后的三角形数组
        public int[] IndexOfPictureForMachArray;  //待匹配照片在结果数组中的编号数组
    }
     public struct Triangle //三角形数据结构
    {
        public int pictureIndex;   //照片序号
        public int D2V1index;      //顶点1在顶点表中的索引号
        public int D2V2index;      //顶点2在顶点表中的索引号
        public int D2V3index;      //顶点3在顶点表中的索引号
   }

    public struct TriangleVertex    //三角形顶点数据结构
    {
        public Vector2 D2V1;        //顶点1的坐标
        public int Code;            //顶点1的编码
        public bool isBoundaryPoint; //顶点1是边界点
        public int Index;//顶点在灰度数组中的索引
        public ArrayList TriangleIndexArrayList ;//包含顶点1的三角形编号集合
        
    }
     

    public struct TriangleEdge      //三角形的边的数据结构
    {
        public int V1Index;         //边的顶点1在三角形顶点表中的索引
        public int V2Index;         //边的顶点2在三角形顶点表中的索引
    }
    
    
    
    //=========================绘图数据结构=======================
    
    //2 同名点偏移量数据结构
    public struct DisplacementsForHomologousPointStruct
    {
        public bool isAbsence;          //当前点是否缺省
        public int code;                //编码号
        public Vector2 pointCoordinate; // 参考照片特征同名点坐标
        public float deltaX;//t1相对t0时刻坐标系中斑点X轴偏移量
        public float deltaY;// t1相对t0时刻坐标系中斑点Y轴偏移量
        public float deltaL;//
        public override String ToString()
        {
            String StringTemp = pointCoordinate.X.ToString() + "," + pointCoordinate.Y.ToString() + "," + deltaX.ToString() + "," + deltaY.ToString();
            return StringTemp;
        }
    }

    //public struct FlowData
    //{
    //    public Point p0;
    //    public Point p1;
    //    public bool isExit;
    //}

  


 }
