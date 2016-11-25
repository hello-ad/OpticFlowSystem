using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;

namespace OpticMeasure.OptiClass
{
    public struct  deformationCaliberation // 变形标定板点结构
    {
       public int orgCode;//赵涛程序识别出的点的 code 值；
       public int typeCode; // 0 编码点； 1：非编码点
       public Vector2  orgCoordinate; //赵涛程序识别出的图像坐标；
       public Vector2  threeDdemention;//给定输入的平面坐标
       public int iCode ;//选择直线时指定的行号
       public int jCode ;// 选择直线时，按X/Y排序时产生的列号
    }

    public struct UncodeBoundaryStruct  //非编码点包围盒数据结构
    { 
        public Vector2  APointCoordinate;  //包围盒上边中心A的二维坐标
        public Vector2  BPointCoordinate; //包围盒左边中心B的二维坐标
        public Vector2  CPointCoordinate;  //包围盒下边中心C的二维坐标
        public Vector2  DPointCoordinate;  //包围盒右边中心D的二维坐标
    }

  
}
