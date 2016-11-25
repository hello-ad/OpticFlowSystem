using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;

namespace OpticMeasure.OpticFlowFunc
{
    class CreatArray
    {
        float[] PictureDlateGreyArray;// 参考照片和匹配照片灰度差数组
        Apicturegrayinformation[] PictureGreyArray; //参考照片（即母版）的灰度数组
        Vector2[] InReferencePicturePixXYArray;//像素经过旋转平移后在母板坐标系下的坐标数组
        DisplacementsForHomologousPointStruct[] HomologousPointsDisplacementsArray;//位移场数组
      
        //创建照片灰度数组
        public void creatPictureGreyArray(System.Drawing.Bitmap PictureBitmap, ref Apicturegrayinformation[] PictureGreyArray)
        {
            
            if (PictureGreyArray == null)
            {
               PictureGreyArray = new Apicturegrayinformation[PictureBitmap.Width * PictureBitmap.Height];
            }
            else
            {
                Array.Resize(ref PictureGreyArray, PictureBitmap.Width * PictureBitmap.Height);
            }

        }

        
        //创建灰度差值数组
        public void creatPictureDlateGreyArray(System.Drawing.Bitmap CurrentPictureBitmap, ref float[] PictureDlateGreyArray)
        {
           
            if (PictureDlateGreyArray == null)
            {
                PictureDlateGreyArray = new float[CurrentPictureBitmap.Width * CurrentPictureBitmap.Height];

            }
            else
            {
                Array.Resize(ref PictureDlateGreyArray, CurrentPictureBitmap.Width * CurrentPictureBitmap.Height);
            }
        }

        //创建像素光流结果数组
        public void creatPixelResultArray(OpticMeasureClass opticFlowFunc, int CurPictureIndex, int RefPictureHight, int RefPictureWidth)
        {
            

            if (opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPixelArray == null)
            {
                opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPixelArray = new OpticflowResultForPixelGrey[RefPictureWidth * RefPictureHight];
            }
            else
            {
                Array.Resize(ref opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPixelArray, RefPictureWidth * RefPictureHight);
            }
        }

        //创建点光流结果数组
        public void creatPointsResultArray(OpticMeasureClass opticFlowFunc, int CurPictureIndex, int RefPictureHight, int RefPictureWidth)
        {


            if (opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray == null)
            {
                opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray = new OpticflowResultForPointsGrey[RefPictureWidth * RefPictureHight];
            }
            else
            {
                Array.Resize(ref opticFlowFunc.PictureOpticflowDataResultArray[CurPictureIndex].OpticflowResultFOfPointsArray, RefPictureWidth * RefPictureHight);
            }
        }

        //创建像素（点）经过旋转平移后在母板坐标系下的坐标数组
        public void creatInReferencePicturePixXYArray(int PointArraylength,ref Vector2[] InReferencePicturePixXYArray )
        {
            if (InReferencePicturePixXYArray == null)
            {
                InReferencePicturePixXYArray = new Vector2[PointArraylength];
            }
            else
            {
                Array.Resize(ref InReferencePicturePixXYArray, PointArraylength);
            }
            
        }

        //创建位移场数组 HomologousPointsDisplacementsArray
        public void creatHomologousPointsDisplacementsArray(int PointArraylength, ref DisplacementsForHomologousPointStruct[] HomologousPointsDisplacementsArray)
        {
            if (HomologousPointsDisplacementsArray ==null)
            {
                HomologousPointsDisplacementsArray = new DisplacementsForHomologousPointStruct[PointArraylength];
            }
            else
            {
                Array.Resize(ref HomologousPointsDisplacementsArray, PointArraylength);
            }
        }

    }
}
