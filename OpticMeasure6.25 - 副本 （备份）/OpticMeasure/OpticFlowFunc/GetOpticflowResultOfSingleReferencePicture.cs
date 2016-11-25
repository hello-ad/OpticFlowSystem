using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpticMeasure;
using System.Windows.Forms;
using TriangulationFun;
using Microsoft.DirectX;
using System.Collections;
using OpticMeasure.OpticFlowFunc;
using System.Drawing;
using CSharpAlgorithm.Algorithm;
using System.IO;


namespace OpticMeasure.OpticFlowFunc
{
    public class GetOpticflowResultOfSingleReferencePicture
    {
        public DisplacementsForHomologousPointStruct[] HomologousPointsDisplacementsArray;//要显示的灰度梯度数组

     
        int length;//第i张母板待匹配照片数组长度
        int InReferencePictureDataArrayIndex; //第i张母板索引号为输入量
        //int j = 0; // 第j个像素
        // int k = 0; // 第k张待匹配照片
       
        float Greyvalue; // 灰度差值
        Vector2 Grad;//光流点结果
        int CurPictureIndex;//待匹配照片在结果数组中的索引

        Apicturegrayinformation[] CurrentPictureGreyArray;//当前照片灰度数组
        Apicturegrayinformation[] ReferencePictureGreyArray; //参考照片（即母版）的灰度数组
        float[] PictureDlateGreyArray;// 参考照片和匹配照片灰度差数组
        System.Drawing.Bitmap ReferencePictureBitmap;
        System.Drawing.Bitmap CurrentPictureBitmap;

        int RefPictureHight;//母板照片高度
        int RefPictureWidth;//母板照片宽度

        int RefPicturePointArrayHight;//母板照片点数组高度
        int RefPicturePointArrayWidth;//母板照片点数组宽度

        CSharpAlgorithm.Algorithm.MatrixOfAlgorithm Mt;//旋转矩阵
        double T = 0;//平移向量

       
        Vector2[] InReferencePicturePixXYArray;//像素经过旋转平移后在母板坐标系下的坐标数组

        FunctionFuncCalss functionFun;// 功能函数对象
        Triangulation trian; //三角剖分对象
        CreatArray CreatArray =new CreatArray();//创建数组对象
        ArrayList CurrentPixtNeighbourArray = new ArrayList(); // 当前照片的八邻域数组

        int StepValue; //像素点间间隔
        bool IsStep;//=true表示需要跳点，跳点间隔=StepValue
        bool IsVibrationcorrection;//是否去震动

        //单母板光流计算函数
        public void GetOpticflowResultOfSingleReferencePictureFunc(OpticMeasureClass opticFlowFunc, int _InReferencePictureDataArrayIndex)
        {
            int CalcuateModer;//计算模式

            InReferencePictureDataArrayIndex = _InReferencePictureDataArrayIndex; //第i张母板索引号为输入量
            length = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexOfPictureForMachArray.Length; //第i张母板待匹配照片数组长度
           
            StepValue = opticFlowFunc.StepValue ; //像素点间间隔
            IsStep = opticFlowFunc.IsStep ;//=true表示需要跳点，跳点间隔=StepValue
            IsVibrationcorrection = opticFlowFunc.IsVibrationcorrection;//是否去震动
            

            //----修改--switch case 判断四种情况---

            //判断计算模型
            if (IsStep == false & IsVibrationcorrection == false)
            {
                CalcuateModer = 0;
            }
            else if (IsStep == false & IsVibrationcorrection == true)
            {
                CalcuateModer = 1;
            }
            else if (IsStep == true & IsVibrationcorrection == false)
            {
                CalcuateModer = 2;
            }
            else
            {
                CalcuateModer = 3;
            }

            if (opticFlowFunc.ReferencePictureDataArray == null)
            {
                MessageBox.Show("还没有读入照片！");

            }
            else
            {
                //装母板的灰度数组

                functionFun = new FunctionFuncCalss();

                ReferencePictureBitmap = (Bitmap)Image.FromFile(opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].ReferencePictureLuJin);//得到母板点的Bitmap
                RefPictureHight = ReferencePictureBitmap.Height;
                RefPictureWidth = ReferencePictureBitmap.Width;
                
                CreatArray.creatPictureGreyArray(ReferencePictureBitmap, ref ReferencePictureGreyArray);//创建母板灰度数组
                functionFun.LoadgrayValuesArrayForMatch(ReferencePictureBitmap, ref ReferencePictureGreyArray);//装载母板灰度数组

                              
                //根据计算模板选择相应的光流计算
                switch (CalcuateModer)
                {
                    case 0://逐像素不去震动

                        pixelNoVibrationcorrection( opticFlowFunc);

                        break;

                    case 1://逐像素去震动

                        pixelWithVibrationcorrection(opticFlowFunc);

                        break;

                    case 2://跳点不去震动

                        piontsNoVibrationcorrection(opticFlowFunc);

                        break;

                    case 3://跳点去震动

                        piontsWithVibrationcorrection(opticFlowFunc);
                      
                        break;
                }

            }

        }



        //像素不去震动函数：pixelNoVibrationcorrection()
        public void pixelNoVibrationcorrection(OpticMeasureClass opticFlowFunc)
        {
            //=============待匹配照片循环==================
            for (int k = 0; k < length; k++)
            {

                CurPictureIndex = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexOfPictureForMachArray[k];//待匹配照片在结果数组中的索引
                CurrentPictureBitmap = (Bitmap)Image.FromFile(opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].PictureLujin);

                CreatArray.creatPictureGreyArray(CurrentPictureBitmap, ref CurrentPictureGreyArray);// 创建匹配照片的灰度数组
                functionFun.LoadgrayValuesArrayForMatch(CurrentPictureBitmap, ref CurrentPictureGreyArray); // 装待匹配照片的灰度数组

                CreatArray.creatPixelResultArray(opticFlowFunc, CurPictureIndex, RefPictureHight, RefPictureWidth);//像素结果数组创建

                //=============照片像素循环 像素流计算（像素直接相减）==================
                for (int j = 0; j < CurrentPictureGreyArray.Length; j++)
                {
                    //Greyvalue = CurrentPictureGreyArray[j].OrigingrayValue - ReferencePictureGreyArray[j].OrigingrayValue;

                    //Greyvalue = CurrentPictureGreyArray[j].OrigingrayValue / ReferencePictureGreyArray[j].OrigingrayValue - ReferencePictureGreyArray[j].OrigingrayValue/ReferencePictureGreyArray[j].OrigingrayValue;
                    //Greyvalue = CurrentPictureGreyArray[j].OrigingrayValue / ReferencePictureGreyArray[j].OrigingrayValue - CurrentPictureGreyArray[j+1].OrigingrayValue / ReferencePictureGreyArray[j].OrigingrayValue;
                   
                    //将结果存到结果数组中
                    opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPixelArray[j].Greyvalue = Greyvalue;
                }

                //清空数组
                Array.Resize(ref CurrentPictureGreyArray, 0);
                

            }

            //清数数组
            Array.Resize(ref ReferencePictureGreyArray, 0);
            
            // 销毁functionFun
            functionFun = null;

        }
       
        //像素去震动函数：pixelWithVibrationcorrection()
        public void pixelWithVibrationcorrection(OpticMeasureClass opticFlowFunc)
        {
            int InReferencePictureIndex = 0;//在母板系照片灰度中的索引
            Mt = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm();
            //call读识标记点函数，将母板的标记点识取出来
            functionFun.ReadMarkPointsFunc();

            //将第i张母板标记点存到PictureOpticflowDataResultArray[ReferencePictureDataArray[i].ReferencePictureIndex].PictureMarkpointsArray
            opticFlowFunc.PictureOpticflowDataResultArray[opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].ReferencePictureIndex].PictureMarkpointsArray = functionFun.MarkPointsArray;

            CreatArray.creatInReferencePicturePixXYArray(RefPictureWidth * RefPictureHight, ref InReferencePicturePixXYArray);//像素经过旋转平移后在母板坐标系下的坐标数组
          
            //=============待匹配照片循环================
            for (int k = 0; k < length; k++)
            {

                CurPictureIndex = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexOfPictureForMachArray[k];//待匹配照片在结果数组中的索引
              
                CreatArray.creatPixelResultArray(opticFlowFunc, CurPictureIndex, RefPictureHight, RefPictureWidth);//创建像素光流结果数组

                //装待匹配照片灰度数组
                CurrentPictureBitmap = (Bitmap)Image.FromFile(opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].PictureLujin);
                CreatArray.creatPictureGreyArray(CurrentPictureBitmap, ref CurrentPictureGreyArray);//创建待匹配照片灰度数组
                functionFun.LoadgrayValuesArrayForMatch(CurrentPictureBitmap, ref CurrentPictureGreyArray);//装载待匹配照片灰度数组
                
                functionFun.ReadMarkPointsFunc();//call识标记点函数

                //将第K张待匹配照片标记点到光流结果数组中
                opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].PictureMarkpointsArray = functionFun.MarkPointsArray;

                // call 求旋转平移矩阵函数，得到母板照片旋转平移到当前待匹配照片上的旋转平移矩阵函数
                functionFun.GetRotationMatrixAndTranslationFunc(ref Mt);

                //=============待匹配照片像素循环 像素流计算（去震动像素直接相减）==================

                for (int j = 0; j < CurrentPictureGreyArray.Length; j++)
                {

                    // 得到旋转平移后的坐标
                    functionFun.Get2DzuobiaoInReferencePictureFunc(opticFlowFunc, CurrentPictureBitmap, Mt, T, InReferencePictureDataArrayIndex, j, RefPictureWidth * RefPictureHight, ref InReferencePicturePixXYArray);

                    //计算编号
                    functionFun.CalculateIndexOfInReferencePictureFunc(RefPictureHight, RefPictureWidth, j, InReferencePicturePixXYArray, ref InReferencePictureIndex);

                    //求灰度差 直接相减
                    Greyvalue = CurrentPictureGreyArray[j].OrigingrayValue - ReferencePictureGreyArray[InReferencePictureIndex].OrigingrayValue;

                    //将结果存到结果数组中
                    opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPixelArray[j].Greyvalue = Greyvalue;
                }


                //清空待匹配照片灰度数组
                Array.Resize(ref CurrentPictureGreyArray, 0);

            }

            //销毁对象
            functionFun = null;
            //清空母版照片灰度数组
            Array.Resize(ref ReferencePictureGreyArray, 0);


        }

        
        // 跳点不去震动函数：piontsNoVibrationcorrection()
        public void piontsNoVibrationcorrection(OpticMeasureClass opticFlowFunc)
        {
            int PointArraylength;//第i张母板点坐标数组长度
           
            CalculateWidthAndHightOfPointImage();//计算母板点数组的长和宽
           
            trian = new Triangulation(); // 创建三角剖分对象

            //call装顶点表函数，装母板顶点表
            trian.GetTrianglerVertexArrayFunc(ReferencePictureBitmap.Height, ReferencePictureBitmap.Width, StepValue);

            PointArraylength = trian.TriangleVertexArray.Length;
            packedPiontImageCoorditionArrayFunction(opticFlowFunc, PointArraylength);//装母板点数组
           
            //销毁trian对象
            trian.Destroy();
            GC.Collect();
 
          
            //=============待匹配照片循环 计算==================
            for (int k = 0; k < length; k++)
            {
                CurPictureIndex = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexOfPictureForMachArray[k];//待匹配照片在结果数组中的索引

                CurrentPictureBitmap = (Bitmap)Image.FromFile(opticFlowFunc.PictureOpticflowDataResultArray[opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexOfPictureForMachArray[k]].PictureLujin);
                
                CreatArray.creatPictureGreyArray(CurrentPictureBitmap, ref CurrentPictureGreyArray); //创建 匹配照片灰度数组

                CreatArray.creatPictureDlateGreyArray(CurrentPictureBitmap, ref PictureDlateGreyArray);//创建灰度差值数组

                functionFun.LoadgrayValuesArrayForMatch(CurrentPictureBitmap, ref CurrentPictureGreyArray);//装载 匹配照片灰度数组

                //CreatArray.creatHomologousPointsDisplacementsArray(PointArraylength,ref HomologousPointsDisplacementsArray);//创建位移场数组

                CreatArray.creatPointsResultArray(opticFlowFunc, CurPictureIndex, RefPicturePointArrayHight, RefPicturePointArrayWidth);//创建结果数组
               
                // 求母板照片和待匹配照片灰度差
                for (int j = 0; j < CurrentPictureGreyArray.Length; j++)
                {
                    PictureDlateGreyArray[j] = CurrentPictureGreyArray[j].OrigingrayValue - ReferencePictureGreyArray[j].OrigingrayValue;

                }

                //清数组
                Array.Resize(ref CurrentPictureGreyArray, 0);
                

                //====母板点数组循环计算=========
                for (int j = 0; j < PointArraylength;j++ )
                {
                   
                    //装载点坐标数组
                    opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray[j].pointCoordinate.X = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray[j].X;
                    opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray[j].pointCoordinate.Y = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray[j].Y;
                   
                    //装八邻域数组
                    functionFun.racepossibleEdgePointIndexFunc(ReferencePictureBitmap.Height, ReferencePictureBitmap.Width, opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexArray[j], ref CurrentPixtNeighbourArray);

                    //soble算子
                    Grad.X = 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[1]] + 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[2]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[3]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[5]] - 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[6]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[7]];

                    Grad.Y = -2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[0]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[1]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[3]] + 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[4]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[5]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[7]];

                  //Prewitt算子
                    //Grad.X = 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[1]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[2]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[3]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[5]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[6]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[7]];

                    //Grad.Y = -1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[0]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[1]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[3]] +1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[4]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[5]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[7]];
                    
                    opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray[j].Grad = Grad;

                }
              
                // 清除数组
                Array.Resize(ref PictureDlateGreyArray, 0);
                CurrentPixtNeighbourArray.Clear();

            }
            // 清除数组
            Array.Resize(ref ReferencePictureGreyArray, 0);

        }

        
        //跳点去震动函数：piontsWithVibrationcorrection()
        public void piontsWithVibrationcorrection(OpticMeasureClass opticFlowFunc)
        {

            int PointArraylength;//第i张母板点坐标数组长度
            int InReferencePictureIndex = 0;//在母板系照片灰度中的索引
            int s = 0;//点序号
            Mt = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm();
            trian = new Triangulation();  // 创建三角剖分对象
           
            CalculateWidthAndHightOfPointImage(); //计算母板点数组的长和宽
          
            //call装顶点表函数，装母板顶点表
            trian.GetTrianglerVertexArrayFunc(ReferencePictureBitmap.Height, ReferencePictureBitmap.Width, StepValue);

            PointArraylength = trian.TriangleVertexArray.Length;//母板点数组长度
            //PointArraylength = RefPicturePointArrayWidth * RefPicturePointArrayHight;//母板点数组长度
            packedPiontImageCoorditionArrayFunction(opticFlowFunc, PointArraylength);//装载到母板点数组中
           
            //销毁trian对象
            trian = null;//可以这么销毁

            functionFun.ReadMarkPointsFunc(); //call读识标记点函数，将母板的标记点识取出来
            //将第i张母板标记点存到PictureOpticflowDataResultArray[ReferencePictureDataArray[i].ReferencePictureIndex].PictureMarkpointsArray
            opticFlowFunc.PictureOpticflowDataResultArray[opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].ReferencePictureIndex].PictureMarkpointsArray = functionFun.MarkPointsArray;

            CreatArray.creatInReferencePicturePixXYArray(RefPictureHight*RefPictureWidth, ref InReferencePicturePixXYArray); //创建像素经过旋转平移后在母板坐标系下的坐标数组

          
            //=======匹配照片 循环计算=====
            for (int k = 0; k < length; k++)
            {

                CurPictureIndex = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexOfPictureForMachArray[k];//待匹配照片在结果数组中的索引

                CreatArray.creatPointsResultArray(opticFlowFunc, CurPictureIndex, RefPicturePointArrayHight, RefPicturePointArrayWidth);//创建点光流结果数组
                
                CurrentPictureBitmap = (Bitmap)Image.FromFile(opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].PictureLujin);
                CreatArray.creatPictureGreyArray(CurrentPictureBitmap, ref CurrentPictureGreyArray);//创建 匹配照片灰度数组
                functionFun.LoadgrayValuesArrayForMatch(CurrentPictureBitmap, ref CurrentPictureGreyArray); //装待匹配照片灰度数组
               
                CreatArray.creatPictureDlateGreyArray(CurrentPictureBitmap, ref PictureDlateGreyArray);//创建灰度差数组
                CreatArray.creatHomologousPointsDisplacementsArray(PointArraylength,ref HomologousPointsDisplacementsArray);//创建位移场数组
                
                functionFun.ReadMarkPointsFunc();//call识标记点函数
                opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].PictureMarkpointsArray = functionFun.MarkPointsArray; //将第K张待匹配照片标记点到光流结果数组中

                // call 求旋转平移矩阵函数，得到母板照片旋转平移到当前待匹配照片上的旋转平移矩阵函数
                functionFun.GetRotationMatrixAndTranslationFunc(ref Mt);

               // 求母板照片和待匹配照片灰度差
                for (int j = 0; j < CurrentPictureGreyArray.Length; j++)
                {

                    // 得到旋转平移后的坐标
                    functionFun.Get2DzuobiaoInReferencePictureFunc(opticFlowFunc, CurrentPictureBitmap, Mt, T, InReferencePictureDataArrayIndex, j, CurrentPictureGreyArray.Length, ref InReferencePicturePixXYArray);

                    //计算编号
                    functionFun.CalculateIndexOfInReferencePictureFunc(RefPictureHight, RefPictureWidth, j, InReferencePicturePixXYArray, ref InReferencePictureIndex);

                    //求灰度差 直接相减
                    PictureDlateGreyArray[j] = CurrentPictureGreyArray[j].OrigingrayValue - ReferencePictureGreyArray[InReferencePictureIndex].OrigingrayValue;

                }
                //清数组
                Array.Resize(ref CurrentPictureGreyArray, 0);
             
               
             //=============待匹配照片 点循环 计算（去震动）================== 
                //for (int j = 0; j < PointArraylength; j++)
                //{
                    
                //    //将点坐标存起
                //    opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray[j].pointCoordinate.X = opticFlowFunc.ReferencePictureDataArray[i].PiontImageCoorditionArray[j].X;
                //    opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray[j].pointCoordinate.Y = opticFlowFunc.ReferencePictureDataArray[i].PiontImageCoorditionArray[j].Y;
                    
                //    // call求灰度差函数
                //    functionFun.racepossibleEdgePointIndexFunc(RefPicturePointArrayHight, RefPicturePointArrayWidth, opticFlowFunc.ReferencePictureDataArray[i].IndexArray[j], ref CurrentPixtNeighbourArray);


                //    //判断 八邻域数组长度是否为8，不为8的话 梯度赋0
                //    if (CurrentPixtNeighbourArray.Count == 8)
                //    {
                //        //soble算子
                //        Grad.X = 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[1]] + 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[2]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[3]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[5]] - 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[6]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[7]];

                //        Grad.Y = -2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[0]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[1]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[3]] + 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[4]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[5]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[7]];
                //    }
                //    else
                //    {
                //        Grad.X = 0;
                //        Grad.Y = 0;
                //    }

                //    //将灰度差存到结果数组里面

                //    opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray[j].Grad = Grad;
                //}

                //=====存到赵涛数组里===========

                //装载显示点的坐标
                for (int j = 0; j < PointArraylength; j++)
                {

                    HomologousPointsDisplacementsArray[j].pointCoordinate.X = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray[j].X;
                    HomologousPointsDisplacementsArray[j].pointCoordinate.Y = opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray[j].Y;

                }

                //====母板点数组循环计算=========
                for (int j = 0; j < PointArraylength; j++)
                {
                    //装八邻域数组
                    functionFun.racepossibleEdgePointIndexFunc(ReferencePictureBitmap.Height, ReferencePictureBitmap.Width, opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexArray[j], ref CurrentPixtNeighbourArray);

                    //判断 八邻域数组长度是否为8，不为8的话 梯度赋0
                    if (CurrentPixtNeighbourArray.Count == 8)
                    {
                        //soble算子
                        HomologousPointsDisplacementsArray[s].deltaX = 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[1]] + 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[2]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[3]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[5]] - 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[6]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[7]];

                        HomologousPointsDisplacementsArray[s].deltaY = -2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[0]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[1]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[3]] + 2 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[4]] + 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[5]] - 1 * PictureDlateGreyArray[(int)CurrentPixtNeighbourArray[7]];
                    }
                    else
                    {
                        HomologousPointsDisplacementsArray[s].deltaX = 0;
                        HomologousPointsDisplacementsArray[s].deltaY = 0;
                    }

                    s++;

                }
               
                //销毁对象
                functionFun = null;
                // 清除数组
                Array.Resize(ref PictureDlateGreyArray, 0);
                CurrentPixtNeighbourArray.Clear();


            }
            // 清除数组
            Array.Resize(ref ReferencePictureGreyArray, 0);
        }


        //将母板点坐标(x,y)存到母板点数组中函数: packedPiontImageCoorditionArrayFunction（）
        public void packedPiontImageCoorditionArrayFunction(OpticMeasureClass opticFlowFunc, int PointArraylength)
        {
           //判断母板点数组是否建立
            if (opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray == null)
            {
                opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray = new Vector2[PointArraylength];
            }
            else
            {
                Array.Resize(ref opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray, PointArraylength);
            }
            //判断母板点索引数组是否建立
            if (opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexArray == null)
            {
                opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexArray = new int[PointArraylength];
            }
            else
            {
                Array.Resize(ref opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexArray, PointArraylength);
            }

            //将母板点坐标(x,y)存到母板点数组中
            for (int n = 0; n < PointArraylength; n++)
            {
                opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray[n].X = trian.TriangleVertexArray[n].D2V1.X;
                opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].PiontImageCoorditionArray[n].Y = trian.TriangleVertexArray[n].D2V1.Y;
                opticFlowFunc.ReferencePictureDataArray[InReferencePictureDataArrayIndex].IndexArray[n] = trian.TriangleVertexArray[n].Index;
            }

        }

        //计算母板跳点后的长和宽函数
        public void CalculateWidthAndHightOfPointImage()
        {

            if ((ReferencePictureBitmap.Height - 1) % StepValue == 0)
            {
                RefPicturePointArrayHight = (ReferencePictureBitmap.Height - 1) / StepValue ;
            }
            else
            {
                RefPicturePointArrayHight = (ReferencePictureBitmap.Height - 1) / StepValue + 1;
            }

            if ((ReferencePictureBitmap.Width - 1) % StepValue == 0)
            {
                RefPicturePointArrayWidth = (ReferencePictureBitmap.Width - 1) / StepValue ;
            }
            else
            {
                RefPicturePointArrayWidth = (ReferencePictureBitmap.Width - 1) / StepValue + 1;
            }
        }
     
           

    }
}
