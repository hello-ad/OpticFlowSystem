using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OpticMeasure.OpticFlowFunc
{
    class newOpticFlow 
    {
        Apicturegrayinformation[] CurrentPictureGreyArray;//当前照片灰度数组
        Apicturegrayinformation[] ReferencePictureGreyArray; //参考照片（即母版）的灰度数组
        
        Apicturegrayinformation[] Im1;//模板照片拷贝数组
        Apicturegrayinformation[] Im2; //当前照片拷贝数组
        
        System.Drawing.Bitmap ReferencePictureBitmap;
        System.Drawing.Bitmap CurrentPictureBitmap;
        FunctionFuncCalss functionFun=new FunctionFuncCalss();// 功能函数对象
        OpticMeasureClass opticFlowFunc;//
        CreatArray CreatArray = new CreatArray();//创建数组对象
       

        public void Flow_Diagnostics_Run_Refine(OpticMeasureClass currentOMC)
        {
            //------定义局部变量-------------------------------
            int[] window_shifting = new int[4];//装载矩形窗的左上角和右下角点的坐标 数组
            int size_average =3;// 卷积核大小，即邻域大小
            byte thresholding =20;//得到阈值
            byte dwtSeries =2;//得到小波分解级数
            byte flagFilter =0 ;//得到应用何种小波基和何种阈值法（0-5）

                     
            //------装待匹配照片灰度数组-------------------------------------------
            for (int i = 0; i < currentOMC.readFileNames.Length-1;i++ )
            {
                //模板照片灰度数组
                ReferencePictureBitmap = (Bitmap)Image.FromFile(currentOMC.readFileNames[i]);
                CreatArray.creatPictureGreyArray(ReferencePictureBitmap, ref ReferencePictureGreyArray);
                functionFun.LoadgrayValuesArrayForMatch(ReferencePictureBitmap, ref ReferencePictureGreyArray);
                
                //带匹配照片灰度数组              
                CurrentPictureBitmap = (Bitmap)Image.FromFile(currentOMC.readFileNames[i+1]);
                CreatArray.creatPictureGreyArray(CurrentPictureBitmap, ref CurrentPictureGreyArray);//创建待匹配照片灰度数组
                functionFun.LoadgrayValuesArrayForMatch(CurrentPictureBitmap, ref CurrentPictureGreyArray);//装载待匹配照片灰度数组
            }

            //-------------选择需要处理的区域(功能空缺)-------------------------------------
           
            
            //---------------------光强修正-------------------------------------------------
            //先将原始数组拷贝
            Im1 = new Apicturegrayinformation[ReferencePictureGreyArray.Length];
            Im2 = new Apicturegrayinformation[CurrentPictureGreyArray.Length];
             
            Array.Copy(ReferencePictureGreyArray, Im1,ReferencePictureGreyArray.Length);
            Array.Copy(CurrentPictureGreyArray,Im2, CurrentPictureGreyArray.Length);

            //functionFun.correction_illumination(ReferencePictureBitmap, ReferencePictureGreyArray, CurrentPictureGreyArray, window_shifting, size_average, ref Im2);

            //------------图片预处理（小波变换、高斯低通滤波）-----------------------------------

              //---先 小波变换-------------
            
            Apicturegrayinformation[] Im1WaveletArray = new Apicturegrayinformation[Im1.Length ];//Im1 小波变换后的数组（大小减半）
            Apicturegrayinformation[] Im2WaveletArray = new Apicturegrayinformation[Im2.Length ];//Im2 小波变换后的数组（大小减半）
           
            functionFun.WaveletFun(ReferencePictureBitmap, Im1, thresholding, dwtSeries, flagFilter,ref Im1WaveletArray);// 模板照片小波变换
            functionFun.WaveletFun(CurrentPictureBitmap, Im2, thresholding, dwtSeries, flagFilter, ref Im2WaveletArray);// 匹配照片小波变换

            //--再 高斯低通滤波-----------------
            double sigma = 1.5;//均方值
            Apicturegrayinformation[] Im1GaussrasultGrayArray = new Apicturegrayinformation[Im1.Length];//Im1 高斯滤波后数组
            Apicturegrayinformation[] Im2GaussrasultGrayArray = new Apicturegrayinformation[Im2.Length];//Im2 高斯滤波后数组
            
            //调用高斯滤波函数
            functionFun.GaussFunc(Im1WaveletArray, sigma, ref Im1GaussrasultGrayArray);
            functionFun.GaussFunc(Im2WaveletArray, sigma, ref Im2GaussrasultGrayArray);

            //------------光流计算-----------------------------





        }




    }


}
