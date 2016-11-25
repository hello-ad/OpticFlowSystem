using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using System.Collections;
using System.Drawing;
using OpticMeasure.OpticFlowFunc;
using CSharpAlgorithm.Algorithm;
using System.Windows.Forms;


namespace OpticMeasure.OpticFlowFunc
{
    class FunctionFuncCalss
    {

        //public ArrayList NeighbourPointValueAndIndexOfPixArray; //八邻域数组

        public Vector2[] MarkPointsArray;// 标记点数组
        Vector2[] PiontImageCoorditionArray;//母板点数组
        public int CurrentPixIndex;//像素（点）在数组中的序号 
        public byte Greyvalue;//光流像素结果
        public Vector2 Grad;//光流点结果


        //----读标记点函数-----
        public void ReadMarkPointsFunc()
        {
            MarkPointsArray = new Vector2[4];

        }

        //-----------得到旋转矩阵和平移向量函数----
        public void GetRotationMatrixAndTranslationFunc(ref CSharpAlgorithm.Algorithm.MatrixOfAlgorithm Mt)
        {
            //// 系数矩阵数据
            //double[] mtxDataCoef14 = {
            //                           1.0,1.0,-1.0,1.0,
            //                           2.0,1.0,0.0,1.0,
            //                           1.0,-1.0,0.0,2.0,
            //                           -1.0,2.0,1.0,1.0,};
            //// 常数矩阵数据
            //double[] mtxDataConst14 = {
            //                           3.0,
            //                            2.0,
            //                           1.0,
            //                           0.0};

            //====测试======================
            // 系数矩阵数据
            double[] mtxDataCoef14 = {
                                         1345.729,0.0,698.7874,0.0,
                                         0.0,1345.729,0.0,698.7874,
                                         3161.341,0.0,559.1614,0.0,
                                         0.0,3161.341,0.0,559.1614,
                                         1366.758,0,1679.83,0,
                                         0,1366.758,0,1679.83,
                                         3166.972,0,1658.789,0,
                                         0,3166.972,0,1658.789,
                                       };
            // 常数矩阵数据
            double[] mtxDataConst14 = {
                                         698.7809,
                                         2525.117,
                                         559.1558,
                                         709.4886,
                                         1679.782,
                                         2504.123,
                                         1658.81,
                                         703.8783,
                                       };
            //1,698.7809,2525.117
            //2,559.1558,709.4886
            //3,1679.782,2504.123
            //4,1658.81,703.8783


            // 构造系数矩阵
            CSharpAlgorithm.Algorithm.MatrixOfAlgorithm mtxCoef14 = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm(8, 4, mtxDataCoef14);
            // 构造常数矩阵
            CSharpAlgorithm.Algorithm.MatrixOfAlgorithm mtxConst14 = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm(8, 1, mtxDataConst14);

            // 构造线性方程组
            LEquations leqs14 = new LEquations(mtxCoef14, mtxConst14);

            // 求解线性最小二乘问题的广义逆法
            Mt = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm();
            CSharpAlgorithm.Algorithm.MatrixOfAlgorithm mtxResultAP = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm();
            CSharpAlgorithm.Algorithm.MatrixOfAlgorithm mtxResultU = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm();
            CSharpAlgorithm.Algorithm.MatrixOfAlgorithm mtxResultV = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm();

            if (!leqs14.GetRootsetGinv(Mt, mtxResultAP, mtxResultU, mtxResultV, 0.0001))
            {
                MessageBox.Show("计算旋转矩阵失败");
            }

        }

        #region //----八邻域函数-------


        public void racepossibleEdgePointIndexFunc(int PictureHight, int PictureWidth, int CurrentPixIndex, ref ArrayList NeighbourPointValueAndIndexOfPixArray)
        {
            if (NeighbourPointValueAndIndexOfPixArray != null)
            {
                NeighbourPointValueAndIndexOfPixArray.Clear();
            }

            int k = (int)(CurrentPixIndex / PictureWidth);
            int l = CurrentPixIndex - k * PictureWidth;

            int[] kOffset = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] lOffset = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
            int kk, ll;
            int NextPointIndex;

            for (int p = 0; p < 8; p++)
            {
                kk = k + kOffset[p];
                ll = l + lOffset[p];

                if (kk >= 0 && kk < PictureHight && ll >= 0 && ll < PictureWidth)
                {
                    NextPointIndex = kk * PictureWidth + ll;
                    NeighbourPointValueAndIndexOfPixArray.Add(NextPointIndex);

                }
            }


        }
        #endregion

        #region  // ---装灰度数组函数-------

        public void LoadgrayValuesArrayForMatch(System.Drawing.Bitmap curBitmap, ref Apicturegrayinformation[] ApicturegrayinformationArray)
        {
            int PictureHight = curBitmap.Height;
            int PictureWidth = curBitmap.Width;
            int strideValue;
            int lenth = PictureWidth * PictureHight;
            double max = 0, min = 255;

            if (ApicturegrayinformationArray == null)
            {
                ApicturegrayinformationArray = new Apicturegrayinformation[lenth];
            }
            else
            {
                Array.Resize(ref ApicturegrayinformationArray, lenth);
            }

            Rectangle rect = new Rectangle(0, 0, PictureWidth, PictureHight);
            System.Drawing.Imaging.BitmapData bmpData = curBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, curBitmap.PixelFormat);
            IntPtr ptr = bmpData.Scan0;


            if (PictureWidth % 4 == 0)
            {
                strideValue = PictureWidth * 3;
                byte[] rgbValues = new byte[lenth * 3];
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, lenth * 3);
                //灰度化
                Dogray(PictureHight, PictureWidth, ref ApicturegrayinformationArray, rgbValues, strideValue);
                //灰度拉伸
                graystretch(ref ApicturegrayinformationArray, max, min);
            }
            else
            {
                strideValue = bmpData.Stride;
                int bytes = strideValue * PictureHight;
                byte[] rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                //灰度化
                Dogray(PictureHight, PictureWidth, ref ApicturegrayinformationArray, rgbValues, strideValue);
                //灰度拉伸
                graystretch(ref ApicturegrayinformationArray, max, min);
            }
            curBitmap.UnlockBits(bmpData);

        }

        //灰度化
        public void Dogray(int PictureHight, int PictureWidth, ref Apicturegrayinformation[] ApicturegrayinformationArray, byte[] rgbValues, int strideValue)
        {
            double colorTemp;

            if ((strideValue - PictureWidth * 3) == 0)
            {
                for (int i = 0; i < rgbValues.Length; i += 3)
                {

                    colorTemp = rgbValues[i + 2] * 0.299 + rgbValues[i + 1] * 0.587 + rgbValues[i] * 0.114;

                    ApicturegrayinformationArray[i / 3].OrigingrayValue = (float)colorTemp;

                }
            }
            else
            {
                float[] TemprgbValuesArray = new float[rgbValues.Length];

                for (int i = 0; i < PictureHight; i++)
                {
                    for (int j = 0; j < PictureWidth * 3; j += 3)
                    {

                        colorTemp = rgbValues[i * strideValue + j + 2] * 0.299 + rgbValues[i * strideValue + j + 1] * 0.587 + rgbValues[i * strideValue + j] * 0.114;
                        TemprgbValuesArray[i * strideValue + j + 2] = TemprgbValuesArray[i * strideValue + j + 1] = TemprgbValuesArray[i * strideValue + j] = (float)colorTemp;

                    }
                }
                for (int i = 0; i < PictureHight; i++)
                {
                    for (int j = 0; j < PictureWidth * 3; j += 3)
                    {
                        ApicturegrayinformationArray[i * PictureWidth + j / 3].OrigingrayValue = TemprgbValuesArray[i * strideValue + j];
                    }
                }

            }

        }

        //灰度拉伸
        private void graystretch(ref Apicturegrayinformation[] ApicturegrayinformationArray, double max, double min)
        {
            double Curentgraytemp;
            double p;

            float maxFz = 255f;
            float minFz = 0f;


            if (max > (double)maxFz) max = (double)maxFz;
            if (min < (double)minFz) min = (double)minFz;

            p = 255.0 / (max - min);

            for (int i = 0; i < ApicturegrayinformationArray.Length; i++)
            {
                if (ApicturegrayinformationArray[i].OrigingrayValue > maxFz)
                {
                    Curentgraytemp = 255;
                }
                else if (ApicturegrayinformationArray[i].OrigingrayValue < minFz)
                {
                    Curentgraytemp = 0;
                }
                else
                {
                    Curentgraytemp = (p * ((double)ApicturegrayinformationArray[i].OrigingrayValue - min) + 0.5);
                }
                //ApicturegrayinformationArray[i].currentgrayValue = (byte)Curentgraytemp;

            }

        }



        #endregion

        #region //---得到旋转平移后的坐标函数------

        public void Get2DzuobiaoInReferencePictureFunc(OpticMeasureClass opticFlowFunc, System.Drawing.Bitmap CurrentPictureBitmap, CSharpAlgorithm.Algorithm.MatrixOfAlgorithm Mt, double T, int _InReferencePictureDataArrayIndex, int CurrentPixIndex, int Arraylength, ref Vector2[] InReferencePicturePixXYArray)
        {
            int i = _InReferencePictureDataArrayIndex; //第i张母板
            bool IsStep = opticFlowFunc.IsStep;
            Vector2[] CurrentPicturePixXYArray;//当前照片像素坐标数组
            CurrentPicturePixXYArray = new Vector2[Arraylength];//创建待匹配照片坐标数组

            //根据像素索引算出像素坐标：
            CurrentPicturePixXYArray[CurrentPixIndex].X = (float)(CurrentPixIndex / CurrentPictureBitmap.Width);//算出横坐标
            CurrentPicturePixXYArray[CurrentPixIndex].Y = (float)(CurrentPixIndex % CurrentPictureBitmap.Width); //算出纵坐标

            //像素坐标平移旋转后的坐标
            InReferencePicturePixXYArray[CurrentPixIndex].X = (float)(CurrentPicturePixXYArray[CurrentPixIndex].X + T);//为了便于编译，Mt去掉
            InReferencePicturePixXYArray[CurrentPixIndex].Y = (float)(CurrentPicturePixXYArray[CurrentPixIndex].Y + T);

        }
        #endregion



        //得到在当前点在母板坐标系下的索引

        public void CalculateIndexOfInReferencePictureFunc(int PictureHight, int PictureWidth, int CurrentPixIndex, Vector2[] InReferencePicturePixXYArray, ref int InReferencePixIndex)
        {
            InReferencePixIndex = (int)(InReferencePicturePixXYArray[CurrentPixIndex].X * PictureWidth + InReferencePicturePixXYArray[CurrentPixIndex].Y);


        }

        // 释放内存函数
        public void Desopse()
        {
            // public Vector2[] MarkPointsArray;// 标记点数组
            //Vector2[] PiontImageCoorditionArray;//母板点数组
            if (MarkPointsArray != null)
            {
                Array.Resize(ref MarkPointsArray, 0);
            }

            if (PiontImageCoorditionArray != null)
            {
                Array.Resize(ref PiontImageCoorditionArray, 0);
            }

        }

        #region //修正光照强度函数

        public void correction_illumination(Bitmap PictureBitmap, Apicturegrayinformation[] Im1, Apicturegrayinformation[] Im2, /*int[] window_shifting, int size_average,*/ ref Apicturegrayinformation[] _Im2)
        {

            float tempGrayValueForIm1 = 0f;
            float tempGrayValueForIm2 = 0f;
            float I1_mean;//Im1的灰度均值
            float I2_mean;//IM2的灰度均值
            ArrayList NeighbourPointValueArray = new ArrayList();//存放 点八邻域下标的 数组
            Apicturegrayinformation[] Im1juanji = new Apicturegrayinformation[Im1.Length];//Im1 存放卷积的数组
            Apicturegrayinformation[] Im2juanji = new Apicturegrayinformation[Im2.Length];//Im2 存放卷积的数组


            //求出Im1、Im2指定区域的灰度平均值I1_mean和I2_mean;
            for (int i = 0; i < Im1.Length; i++)
            {
                tempGrayValueForIm1 += Im1[i].OrigingrayValue;
            }
            I1_mean = tempGrayValueForIm1 / Im1.Length;

            for (int i = 0; i < Im2.Length; i++)
            {
                tempGrayValueForIm2 += Im2[i].OrigingrayValue;
            }
            I2_mean = tempGrayValueForIm2 / Im2.Length;

            //------以Im1的光亮修正Im2的光亮,注意不要超出255--------
            for (int i = 0; i < Im1.Length; i++)
            {
                _Im2[i].OrigingrayValue = (I2_mean / I1_mean) * Im2[i].OrigingrayValue;
            }
            //----------再进行卷积运算------------
            //先求点的八邻域，在求和
            //Im1求卷积
            for (int i = 0; i < Im1.Length; i++)
            {
                if (NeighbourPointValueArray != null)
                {
                    NeighbourPointValueArray.Clear();
                }
                racepossibleEdgePointIndexFunc(PictureBitmap.Height, PictureBitmap.Width, i, ref NeighbourPointValueArray);
                for (int j = 0; j < NeighbourPointValueArray.Count; j++)
                {
                    Im1juanji[i].OrigingrayValue += Im1[(int)NeighbourPointValueArray[j]].OrigingrayValue;
                }
            }
            //Im2求卷积
            for (int i = 0; i < Im2.Length; i++)
            {
                if (NeighbourPointValueArray != null)
                {
                    NeighbourPointValueArray.Clear();
                }
                racepossibleEdgePointIndexFunc(PictureBitmap.Height, PictureBitmap.Width, i, ref NeighbourPointValueArray);
                for (int j = 0; j < NeighbourPointValueArray.Count; j++)
                {
                    Im2juanji[i].OrigingrayValue += Im2[(int)NeighbourPointValueArray[j]].OrigingrayValue;
                }
            }

            //------修正光强-------------------------

            Apicturegrayinformation[] I12F = new Apicturegrayinformation[Im1.Length];// 存放卷积差的 数组

            for (int i = 0; i < Im1.Length; i++)
            {
                I12F[i].OrigingrayValue = Im1juanji[i].OrigingrayValue - Im2juanji[i].OrigingrayValue;
            }
            for (int i = 0; i < Im1.Length; i++)
            {
                _Im2[i].OrigingrayValue = _Im2[i].OrigingrayValue + I12F[i].OrigingrayValue;
            }

        }
        #endregion


        #region //-------小波变换-------------------------------------

        // ----------小波变换总的流程函数-----------------------
        public void WaveletFun(Bitmap curBitmap, Apicturegrayinformation[] grayinformationArray, byte _thresholding, byte _dwtSeries, byte _flagFilter, ref Apicturegrayinformation[] _resultGrayArray)
        {
            double[] tempA = new double[grayinformationArray.Length];
            double[] tempB = new double[grayinformationArray.Length];

            for (int i = 0; i < grayinformationArray.Length; i++)
            {
                tempA[i] = grayinformationArray[i].OrigingrayValue;

            }


            byte thresholding = _thresholding;//得到阈值
            byte dwtSeries = _dwtSeries;//得到小波分解级数
            byte flagFilter = _flagFilter;//得到应用何种小波基和何种阈值法
            double[] lowFilter = null;
            double[] highFilter = null;

            switch (flagFilter & 0x0f)
            {
                case 0://haar
                    lowFilter = new double[] { 0.70710678118655, 0.70710678118655 };
                    break;
                case 1://daubechies2
                    lowFilter = new double[] { 0.48296291314453, 0.83651630373780, 0.22414386804201, -0.12940952255126 };
                    break;
                case 2://daubechies3
                    lowFilter = new double[] { 0.33267055295008, 0.80689150931109, 0.45987750211849, -0.13501102001025, -0.08544127388203, 0.03522629188571 };
                    break;
                case 3://daubechies4
                    lowFilter = new double[] { 0.23037781330889, 0.71484657055291, 0.63088076792986, -0.02798376941686, -0.18703481171909, 0.03084138183556, 0.03288301166689, -0.01059740178507 };
                    break;
                case 4://daubechies5
                    lowFilter = new double[] { 0.16010239797419, 0.60382926979719, 0.72430852843778, 0.13842814590132, -0.24229488706638, -0.03224486958464, 0.07757149384005, -0.00624149021280, -0.01258075199908, 0.00333572528547 };
                    break;
                case 5://daubechies6
                    lowFilter = new double[] { 0.11154074335011, 0.49462389039845, 0.75113390802110, 0.31525035170920, -0.22626469396544, -0.12976686756726, 0.09750160558732, 0.02752286553031, -0.03158203931849, 0.00055384220116, 0.00477725751195, -0.00107730108531 };
                    break;
                default:
                    MessageBox.Show("无效！");
                    break;
            }

            highFilter = new double[lowFilter.Length];
            for (int i = 0; i < lowFilter.Length; i++)
            {
                highFilter[i] = Math.Pow(-1, i) * lowFilter[lowFilter.Length - 1 - i];// 高通滤波器和低通滤波器关系（-1）的i次方
            }

            for (int k = 0; k < dwtSeries; k++)
            {
                int coef = (int)Math.Pow(2, k);//系数，2的K次方
                for (int i = 0; i < curBitmap.Height; i++)
                {
                    if (i < curBitmap.Height / coef)
                    {
                        for (int j = 0; j < curBitmap.Width; j++)
                        {
                            if (j < curBitmap.Width / coef)
                            {
                                tempB[i * curBitmap.Width / coef + j] = tempA[i * curBitmap.Width + j];
                            }
                        }
                    }
                }
                wavelet2D(ref tempB, lowFilter, highFilter, coef, curBitmap);
                for (int i = 0; i < curBitmap.Height; i++)
                {
                    if (i < curBitmap.Height / coef)
                    {
                        for (int j = 0; j < curBitmap.Width; j++)
                        {
                            if (j < curBitmap.Width / coef)
                            {
                                tempA[i * curBitmap.Width + j] = tempB[i * curBitmap.Width / coef + j];
                            }
                        }
                    }
                }

                if ((flagFilter & 0xf0) == 0x10)
                {
                    for (int l = 0; l < grayinformationArray.Length; l++)
                    {
                        if (tempA[l] < thresholding && tempA[l] > -thresholding)
                            tempA[l] = 0;
                    }
                }
                else
                {
                    for (int l = 0; l < grayinformationArray.Length; l++)
                    {
                        if (tempA[l] >= thresholding)
                            tempA[l] = tempA[l] - thresholding;
                        else
                        {
                            if (tempA[l] <= -thresholding)
                                tempA[l] = tempA[l] + thresholding;
                            else
                                tempA[l] = 0;
                        }
                    }
                }
            }

            for (int k = dwtSeries - 1; k >= 0; k--)
            {
                int coef = (int)Math.Pow(2, k);
                for (int i = 0; i < curBitmap.Height; i++)
                {
                    if (i < curBitmap.Height / coef)
                    {
                        for (int j = 0; j < curBitmap.Width; j++)
                        {
                            if (j < curBitmap.Width / coef)
                            {
                                tempB[i * curBitmap.Width / coef + j] = tempA[i * curBitmap.Width + j];
                            }
                        }
                    }
                }
                iwavelet2D(ref tempB, lowFilter, highFilter, coef, curBitmap);
                for (int i = 0; i < curBitmap.Height; i++)
                {
                    if (i < curBitmap.Height / coef)
                    {
                        for (int j = 0; j < curBitmap.Width; j++)
                        {
                            if (j < curBitmap.Width / coef)
                            {
                                tempA[i * curBitmap.Width + j] = tempB[i * curBitmap.Width / coef + j];
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < grayinformationArray.Length; i++)
            {
                if (tempA[i] >= 255)
                    tempA[i] = 255;
                if (tempA[i] <= 0)
                    tempA[i] = 0;
                _resultGrayArray[i].OrigingrayValue = (float)tempA[i];
            }




        }

        // -----功能函数 一维离散小波变换-----------------------
        private void wavelet1D(double[] scl0, double[] p, double[] q, out  double[] scl1, out double[] wvl1)
        {
            int temp;
            int sclLen = scl0.Length;
            int pLen = p.Length;
            scl1 = new double[sclLen / 2];
            wvl1 = new double[sclLen / 2];

            for (int i = 0; i < sclLen / 2; i++)
            {
                scl1[i] = 0.0;
                wvl1[i] = 0.0;
                for (int j = 0; j < pLen; j++)
                {
                    temp = (j + i * 2) % sclLen;
                    scl1[i] += p[j] * scl0[temp];
                    wvl1[i] += q[j] * scl0[temp];
                }
            }
        }
        // -----功能函数 一维离散小波逆变换-----------------------
        private void iwavelet1D(out double[] scl0, double[] p, double[] q, double[] scl1, double[] wvl1)
        {
            int temp;
            int sclLen = scl1.Length;
            int pLen = p.Length;
            scl0 = new double[sclLen * 2];

            for (int i = 0; i < sclLen; i++)
            {
                scl0[2 * i + 1] = 0.0;
                scl0[2 * i] = 0.0;
                for (int j = 0; j < pLen / 2; j++)
                {
                    temp = (i - j + sclLen) % sclLen;
                    scl0[2 * i + 1] += p[2 * j + 1] * scl1[temp] + q[2 * j + 1] * wvl1[temp];
                    scl0[2 * i] += p[2 * j] * scl1[temp] + q[2 * j] * wvl1[temp];
                }
            }
        }
        // -----功能函数 二维离散小波变换-----------------------
        public void wavelet2D(ref double[] dataImage, double[] p, double[] q, int series, Bitmap curBitmap)
        {
            double[] s = new double[curBitmap.Width / series];
            double[] s1 = new double[curBitmap.Width / (2 * series)];
            double[] w1 = new double[curBitmap.Width / (2 * series)];
            for (int i = 0; i < curBitmap.Height / series; i++)
            {
                for (int j = 0; j < curBitmap.Width / series; j++)
                {
                    s[j] = dataImage[i * curBitmap.Width / series + j];
                }
                wavelet1D(s, p, q, out s1, out w1);

                for (int j = 0; j < curBitmap.Width / series; j++)
                {
                    if (j < curBitmap.Width / (2 * series))
                        dataImage[i * curBitmap.Width / series + j] = s1[j];
                    else
                        dataImage[i * curBitmap.Width / series + j] = w1[j - curBitmap.Width / (2 * series)];
                }
            }

            for (int i = 0; i < curBitmap.Width / series; i++)
            {
                for (int j = 0; j < curBitmap.Height / series; j++)
                {
                    s[j] = dataImage[j * curBitmap.Width / series + i];
                }
                wavelet1D(s, p, q, out s1, out w1);
                for (int j = 0; j < curBitmap.Height / series; j++)
                {
                    if (j < curBitmap.Height / (2 * series))
                        dataImage[j * curBitmap.Width / series + i] = s1[j];
                    else
                        dataImage[j * curBitmap.Width / series + i] = w1[j - curBitmap.Height / (2 * series)];
                }
            }
        }
        // -----功能函数 二维离散小波逆变换-----------------------
        public void iwavelet2D(ref double[] dataImage, double[] p, double[] q, int series, Bitmap curBitmap)
        {
            double[] s = new double[curBitmap.Width / series];
            double[] s1 = new double[curBitmap.Width / (2 * series)];
            double[] w1 = new double[curBitmap.Width / (2 * series)];
            for (int i = 0; i < curBitmap.Width / series; i++)
            {
                for (int j = 0; j < curBitmap.Height / series; j++)
                {
                    if (j < curBitmap.Height / (2 * series))
                        s1[j] = dataImage[j * curBitmap.Width / series + i];
                    else
                        w1[j - curBitmap.Height / (2 * series)] = dataImage[j * curBitmap.Width / series + i];
                }
                iwavelet1D(out s, p, q, s1, w1);
                for (int j = 0; j < curBitmap.Height / series; j++)
                {
                    dataImage[j * curBitmap.Width / series + i] = s[j];
                }
            }
            for (int i = 0; i < curBitmap.Height / series; i++)
            {
                for (int j = 0; j < curBitmap.Width / series; j++)
                {
                    if (j < curBitmap.Width / (2 * series))
                        s1[j] = dataImage[i * curBitmap.Width / series + j];
                    else
                        w1[j - curBitmap.Width / (2 * series)] = dataImage[i * curBitmap.Width / series + j];
                }
                iwavelet1D(out s, p, q, s1, w1);
                for (int j = 0; j < curBitmap.Width / series; j++)
                {
                    dataImage[i * curBitmap.Width / series + j] = s[j];
                }
            }
        }


        #endregion


        #region //-----高斯低通滤波函数--------------------
        //----高斯滤波函数总流程函数--------------------
        public void GaussFunc(Apicturegrayinformation[] grayinformationArray, double _sigma, ref Apicturegrayinformation[] rasultGrayArray)
        {
            double[] tempArray;
            double[] tempImage = new double[grayinformationArray.Length];
            double sigma = _sigma;//得到均方差

            for (int i = 0; i < grayinformationArray.Length; i++)
            {
                tempImage[i] = grayinformationArray[i].OrigingrayValue;

            }
            //调用 高斯滤波方法
            gaussSmooth(tempImage, out tempArray, sigma);
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] > 255)
                    rasultGrayArray[i].OrigingrayValue = 255;
                else if (tempArray[i] < 0)
                    rasultGrayArray[i].OrigingrayValue = 0;
                else
                    rasultGrayArray[i].OrigingrayValue = (float)tempArray[i];
            }


        }

        // -------功能函数 二维高斯卷积----------------
        private void gaussSmooth(double[] inputImage, out double[] outputImage, double sigma)
        {
            double std2 = 2 * sigma * sigma;
            int radius = Convert.ToInt16(Math.Ceiling(3 * sigma));
            int filterWidth = 2 * radius + 1;
            double[] filter = new double[filterWidth];
            outputImage = new double[inputImage.Length];
            int length = Convert.ToInt16(Math.Sqrt(inputImage.Length));
            double[] tempImage = new double[inputImage.Length];

            double sum = 0;
            for (int i = 0; i < filterWidth; i++)
            {
                int xx = (i - radius) * (i - radius);
                filter[i] = Math.Exp(-xx / std2);
                sum += filter[i];
            }
            for (int i = 0; i < filterWidth; i++)
            {
                filter[i] = filter[i] / sum;
            }

            //水平方向滤波
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    double temp = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        int rem = (Math.Abs(j + k)) % length;
                        temp += inputImage[i * length + rem] * filter[k + radius];
                    }
                    tempImage[i * length + j] = temp;
                }
            }

            // 垂直方向滤波
            for (int j = 0; j < length; j++)
            {
                for (int i = 0; i < length; i++)
                {
                    double temp = 0;
                    for (int k = -radius; k <= radius; k++)
                    {
                        int rem = (Math.Abs(i + k)) % length;
                        temp += tempImage[rem * length + j] * filter[k + radius];
                    }
                    outputImage[i * length + j] = temp;
                }
            }
        }


        #endregion

        #region// --------光流计算函数-----------------------------

        //-----HS算法-----------
        public void horn_schunk_estimator(Bitmap curBitmap, Apicturegrayinformation[] Im1, Apicturegrayinformation[] Im2, ref float[] unew, ref float[] vnew)
        {
            //------定义局部变量
            float[] Ix = new float[Im1.Length];//存X方向的偏导数信息  大小为m/2*n/2
            float[] Iy = new float[Im1.Length]; //存Y方向的偏导数信
            float[] It = new float[Im1.Length]; //存T方向的偏导数信
            float[] uv = new float[Im1.Length];// 公式 3-14 UV相乘部分
            float[] u1 = new float[Im1.Length]; // 公式 3-14 U的第一项
            float[] u2 = new float[Im1.Length]; // 公式 3-14 U的第三项
            float[] v1 = new float[Im1.Length]; // 公式 3-14 V的第一项
            float[] v2 = new float[Im1.Length]; // 公式 3-14 V的第三项
            //float[] unew = new float[Im1.Length]; // u的结果数组
            //float[] vnew = new float[Im1.Length]; // v的结果数组
            int maxnum_1 = 500;//最大迭代次数
            float tol = 10 ^ (-12);//最大误差
            int lambda = 200;//拉格朗日乘子
            float tempu = 0f;//u的临时变量
            float tempv = 0f;//v的临时变量
            float[] u = new float[Im1.Length];//u的临时数组，开始初始化为0
            float[] v = new float[Im1.Length];//v的临时数组，开始初始化为0
            float fenmu = 0f;// 公式的分母 临时变量

            //---分别计算Ix、Iy、It采用自己给的算子

            for (int i = 0; i < Im1.Length; i++)
            {
                Ix[i] = (Im1[i].OrigingrayValue + Im2[i].OrigingrayValue) / 2;
                Iy[i] = (Im1[i].OrigingrayValue + Im2[i].OrigingrayValue) / 2;
                It[i] = Im2[i].OrigingrayValue - Im1[i].OrigingrayValue;

            }

            for (int i = 0; i < curBitmap.Height; i++)
            {
                for (int j = 0; j < curBitmap.Width; j++)
                {
                    Ix[i] = (-Ix[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] + Ix[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)] +
                        Ix[((i + 1) % curBitmap.Height) * curBitmap.Width + j] - Ix[i * curBitmap.Width + j]) / 2;
                    Iy[i] = (Iy[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] + Iy[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)] -
                        Iy[((i + 1) % curBitmap.Height) * curBitmap.Width + j] - Iy[i * curBitmap.Width + j]) / 2;
                    It[i] = (It[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] + It[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)] +
                        It[((i + 1) % curBitmap.Height) * curBitmap.Width + j] + It[i * curBitmap.Width + j]) / 4;
                }
            }

            //-----根据HS 公式算得：UV、U1、U2、V1、V1 ;for循环
            //((Ix^2+Iy^2)+lambda^2)
            // u1 = Ix ^ 2 / ((Ix ^ 2 + Iy ^ 2) + lambda ^ 2);
            //u2 = (Ix * It) / ((Ix ^ 2 + Iy ^ 2) + lambda ^ 2);
            //v1 = Iy ^ 2 / ((Ix ^ 2 + Iy ^ 2) + lambda ^ 2);
            //v2 = (Iy*It)/ ((Ix^2+Iy^2)+lambda^2);

            for (int i = 0; i < Im1.Length; i++)
            {
                fenmu = (float)(Math.Pow(Ix[i], 2) + Math.Pow(Iy[i], 2) + Math.Pow(lambda, 2));
                uv[i] = (Ix[i] + Iy[i]) / fenmu;
                u1[i] = (float)(Math.Pow(Ix[i], 2) / fenmu);
                u2[i] = (float)((Ix[i] * It[i]) / fenmu);
                v1[i] = (float)(Math.Pow(Iy[i], 2) / fenmu);
                v2[i] = (float)((Iy[i] * It[i]) / fenmu);
            }

            //------进行迭代求解 
            //边界判断; 行、列 while ,for循环
            double total_error = 100000000;//误差变量
            int k = 0;// 循环变量 
            while (total_error > tol & k < maxnum_1)
            {
                for (int n = 0; n < curBitmap.Height; n++)
                {
                    for (int m = 0; m < curBitmap.Width; m++)
                    {
                        if (n == 0)
                        {
                            if (m == 0)
                            {
                                tempu = u[(m + 1) * curBitmap.Width + n] + u[m * curBitmap.Width + (n + 1)] + u[(m + 1) * curBitmap.Width + (n + 1)];
                                tempv = v[(m + 1) * curBitmap.Width + n] + v[m * curBitmap.Width + (n + 1)] + v[(m + 1) * curBitmap.Width + (n + 1)];
                            }
                            else if (m == curBitmap.Width - 1)
                            {
                                tempu = u[(m - 1) * curBitmap.Width + n] + u[m * curBitmap.Width + (n + 1)] + u[(m - 1) * curBitmap.Width + (n + 1)];
                                tempv = v[(m - 1) * curBitmap.Width + n] + v[m * curBitmap.Width + (n + 1)] + v[(m - 1) * curBitmap.Width + (n + 1)];

                            }
                            else
                            {
                                tempu = u[(m - 1) * curBitmap.Width + n] + u[(m + 1) * curBitmap.Width + n] + u[(m - 1) * curBitmap.Width + (n + 1)] + u[m * curBitmap.Width + (n + 1)] + u[(m + 1) * curBitmap.Width + (n + 1)];
                                tempv = v[(m - 1) * curBitmap.Width + n] + v[(m + 1) * curBitmap.Width + n] + v[(m - 1) * curBitmap.Width + (n + 1)] + v[m * curBitmap.Width + (n + 1)] + v[(m + 1) * curBitmap.Width + (n + 1)];

                            }
                        }
                        else if (n == curBitmap.Height - 1)
                        {
                            if (m == 0)
                            {
                                tempu = u[(m + 1) * curBitmap.Width + n] + u[m * curBitmap.Width + (n - 1)] + u[(m + 1) * curBitmap.Width + (n - 1)];
                                tempv = v[(m + 1) * curBitmap.Width + n] + v[m * curBitmap.Width + (n - 1)] + v[(m + 1) * curBitmap.Width + (n - 1)];
                            }
                            else if (m == curBitmap.Width - 1)
                            {
                                tempu = u[(m - 1) * curBitmap.Width + n] + u[m * curBitmap.Width + (n - 1)] + u[(m - 1) * curBitmap.Width + (n - 1)];
                                tempv = v[(m - 1) * curBitmap.Width + n] + v[m * curBitmap.Width + (n - 1)] + v[(m - 1) * curBitmap.Width + (n - 1)];

                            }
                            else
                            {
                                tempu = u[(m - 1) * curBitmap.Width + n] + u[(m + 1) * curBitmap.Width + n] + u[(m - 1) * curBitmap.Width + (n - 1)] + u[m * curBitmap.Width + (n - 1)] + u[(m + 1) * curBitmap.Width + (n - 1)];
                                tempv = v[(m - 1) * curBitmap.Width + n] + v[(m + 1) * curBitmap.Width + n] + v[(m - 1) * curBitmap.Width + (n - 1)] + v[m * curBitmap.Width + (n - 1)] + v[(m + 1) * curBitmap.Width + (n - 1)];

                            }

                        }
                        else
                        {
                            if (m == 0)
                            {
                                tempu = u[m * curBitmap.Width + (n - 1)] + u[m * curBitmap.Width + (n + 1)] + u[(m + 1) * curBitmap.Width + (n - 1)] + u[(m + 1) * curBitmap.Width + n] + u[(m + 1) * curBitmap.Width + (n + 1)];
                                tempv = v[m * curBitmap.Width + (n - 1)] + v[m * curBitmap.Width + (n + 1)] + v[(m + 1) * curBitmap.Width + (n - 1)] + v[(m + 1) * curBitmap.Width + n] + v[(m + 1) * curBitmap.Width + (n + 1)];
                            }
                            else if (m == curBitmap.Width - 1)
                            {
                                tempu = u[m * curBitmap.Width + (n - 1)] + u[m * curBitmap.Width + (n + 1)] + u[(m - 1) * curBitmap.Width + (n - 1)] + u[(m - 1) * curBitmap.Width + n] + u[(m - 1) * curBitmap.Width + (n + 1)];
                                tempv = v[m * curBitmap.Width + (n - 1)] + v[m * curBitmap.Width + (n + 1)] + v[(m - 1) * curBitmap.Width + (n - 1)] + v[(m - 1) * curBitmap.Width + n] + v[(m - 1) * curBitmap.Width + (n + 1)];

                            }
                            else
                            {
                                tempu = u[(m - 1) * curBitmap.Width + (n - 1)] + u[(m - 1) * curBitmap.Width + n] + u[(m - 1) * curBitmap.Width + (n + 1)] + u[m * curBitmap.Width + (n - 1)] +
                                        u[m * curBitmap.Width + (n + 1)] + u[(m + 1) * curBitmap.Width + (n - 1)] + u[(m + 1) * curBitmap.Width + n] + u[(m + 1) * curBitmap.Width + (n + 1)];
                                tempv = v[(m - 1) * curBitmap.Width + (n - 1)] + v[(m - 1) * curBitmap.Width + n] + v[(m - 1) * curBitmap.Width + (n + 1)] + v[m * curBitmap.Width + (n - 1)] +
                                        v[m * curBitmap.Width + (n + 1)] + v[(m + 1) * curBitmap.Width + (n - 1)] + v[(m + 1) * curBitmap.Width + n] + v[(m + 1) * curBitmap.Width + (n + 1)];

                            }
                        }

                        //newu、newv
                        unew[m * curBitmap.Width + n] = u1[m * curBitmap.Width + n] * tempu - uv[m * curBitmap.Width + n] * tempv - u2[m * curBitmap.Width + n];
                        vnew[m * curBitmap.Width + n] = v1[m * curBitmap.Width + n] * tempv - uv[m * curBitmap.Width + n] * tempu - v2[m * curBitmap.Width + n];
                    }
                }

                // 求误差泛函

                for (int i = 0; i < Im1.Length; i++)
                {
                    total_error = total_error + Math.Pow(unew[i] - u[i], 2) + Math.Pow(vnew[i] - v[i], 2);
                }

                total_error = Math.Sqrt(total_error) / Im1.Length;
               
                //将数组替换 进行迭代
                Array.Copy(unew, u, unew.Length);
                Array.Copy(vnew, v, vnew.Length);


                k++;
            }

            //---hs 算法 算结束了----

        }

        //------刘天舒 算法-------
        public void liu_shen_estimator(Bitmap curBitmap, Apicturegrayinformation[] Im1, Apicturegrayinformation[] Im2, float[] u, float[] v, ref float[] unew, ref float[] vnew)
        {
            float[] Ix = new float[Im1.Length];//存X方向的偏导数信息  大小为m/2*n/2
            float[] Iy = new float[Im1.Length]; //存Y方向的偏导数信
            float[] It = new float[Im1.Length]; //存T方向的偏导数信
            float[] Itx = new float[Im1.Length]; //存X在T方向的偏导数信
            float[] Ity = new float[Im1.Length]; //存Y在T方向的偏导数信


            float[] IIx = new float[Im1.Length];//存X方向偏导和照片灰度乘积 大小为m/2*n/2
            float[] IIy = new float[Im1.Length];//存Y方向偏导和照片灰度乘积
            float[] II = new float[Im1.Length]; //存照片对应点灰度平方
            float[] Ixt = new float[Im1.Length]; //存照片灰度关于X、T的二次偏导
            float[] Iyt = new float[Im1.Length]; //存照片灰度关于Y、T的二次偏导

            //float[] u = new float[Im1.Length];//u的临时数组，开始初始化为HS光流
            //float[] v = new float[Im1.Length];//v的临时数组，开始初始化为HS光流

            float[] A11 = new float[Im1.Length];//论文41页公式 矩阵系数 A11
            float[] A12 = new float[Im1.Length];//论文41页公式 矩阵系数 A12
            float[] A22 = new float[Im1.Length];//论文41页公式 矩阵系数 A22
            float[] B11 = new float[Im1.Length];//论文41页公式 逆矩阵系数 B11
            float[] B12 = new float[Im1.Length];//论文41页公式 逆矩阵系数 B12
            float[] B22 = new float[Im1.Length];//论文41页公式 逆矩阵系数 B22
            float[] bu = new float[Im1.Length];//论文41页公式3-29 
            float[] bv = new float[Im1.Length];//论文41页公式3-29
            //float[] unew = new float[Im1.Length]; // u的结果数组
            //float[] vnew = new float[Im1.Length]; // v的结果数组
            int maxnum = 500;//最大迭代次数
            float tol = 10 ^ (-12);//最大误差
            int lambda = 200;//拉格朗日乘子
            //--------------新增---------------------
            float[] Ax = new float[Im1.Length];//
            float[] Ay = new float[Im1.Length];
            float[] Axy = new float[Im1.Length];
            float[] DetA = new float[Im1.Length];
            //---------------------------------
            float[] ux = new float[Im1.Length];//公式（3.29中）中间变量
            float[] uy = new float[Im1.Length];
            float[] uf = new float[Im1.Length];
            float[] um = new float[Im1.Length];
            float[] uh = new float[Im1.Length];

            float[] vx = new float[Im1.Length];//
            float[] vy = new float[Im1.Length];
            float[] vf = new float[Im1.Length];
            float[] vm = new float[Im1.Length];
            float[] vh = new float[Im1.Length];



            // -----计算梯度IIx、IIy、II、Ixt、Iyt
            for (int i = 0; i < Im1.Length; i++)
            {
                Ix[i] = Im1[i].OrigingrayValue;
                Iy[i] = Im1[i].OrigingrayValue;
                It[i] = Im2[i].OrigingrayValue - Im1[i].OrigingrayValue;
                Ax[i] = Im1[i].OrigingrayValue;
                Ay[i] = Im1[i].OrigingrayValue;
                Axy[i] = Im1[i].OrigingrayValue;
            }

            for (int i = 0; i < curBitmap.Height; i++)
            {
                for (int j = 0; j < curBitmap.Width; j++)
                {
                    Ix[i] = (-Ix[((i - 1) % curBitmap.Height) * curBitmap.Width + j] + Ix[((i + 1) % curBitmap.Height) * curBitmap.Width + j]) / 2;
                    Iy[i] = (Iy[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] - Iy[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]) / 2;

                    Itx[i] = (-It[((i - 1) % curBitmap.Height) * curBitmap.Width + j] + It[((i + 1) % curBitmap.Height) * curBitmap.Width + j]) / 2;
                    Ity[i] = (It[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] - It[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]) / 2;

                }
            }

            for (int i = 0; i < Im1.Length; i++)
            {
                IIx[i] = Im1[i].OrigingrayValue * Ix[i];
                IIy[i] = Im1[i].OrigingrayValue * Iy[i];
                II[i] = Im1[i].OrigingrayValue * Im1[i].OrigingrayValue;

                Ixt[i] = Im1[i].OrigingrayValue * Itx[i];
                Iyt[i] = Im1[i].OrigingrayValue * Ity[i];
            }

            //---求公式（3.28）中矩阵A中产生逆矩阵----
            //---产生A11、A12、A22、B11、B12、B22----
            //--A11 = I.*(imfilter(I, D2/(h*h), 'replicate',  'same')-2*I/(h*h)) - alpha*cmtx;
            //--A22 = I.*(imfilter(I, D2'/(h*h), 'replicate',  'same')-2*I/(h*h)) - alpha*cmtx;
            //--A12 = I.*imfilter(I, M/(h*h), 'replicate',  'same');
            //--DetA = A11.*A22-A12.*A12; 
            //B11 = A22./DetA;   %求2*2矩阵A的逆矩阵的各个B11
            //B12 = -A12./DetA;  %求2*2矩阵A的逆矩阵的各个B12
            //B22 = A11./DetA;   %求2*2矩阵A的逆矩阵的各个B22
            for (int i = 0; i < curBitmap.Height; i++)
            {
                for (int j = 0; j < curBitmap.Width; j++)
                {
                    Ax[i] = (Ax[((i - 1) % curBitmap.Height) * curBitmap.Width + j] + Ax[((i + 1) % curBitmap.Height) * curBitmap.Width + j] - 2 * Ax[i * curBitmap.Width + j]) / 2;
                    Ay[i] = (Iy[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] - Iy[i * curBitmap.Width + ((j - 1) % curBitmap.Width)] - 2 * Ay[i * curBitmap.Width + j]) / 2;

                    Axy[i] = (Axy[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)] - Axy[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]
                                - Axy[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)] + Axy[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]) / 4;

                }
            }


            for (int i = 0; i < Im1.Length; i++)
            {
                A11[i] = Im1[i].OrigingrayValue * Ax[i];
                A22[i] = Im1[i].OrigingrayValue * Ay[i];
                A12[i] = Im1[i].OrigingrayValue * Axy[i];

                DetA[i] = A11[i] * A22[i] - A12[i] * A12[i];

                B11[i] = A22[i] / DetA[i];
                B12[i] = A12[i] / DetA[i];
                B22[i] = A11[i] / DetA[i];

            }

            //-------------【迭代计算】---------------------

            //----产生中间变量--- Ix、Iy、Itx、Ity---
            for (int i = 0; i < curBitmap.Height; i++)
            {
                for (int j = 0; j < curBitmap.Width; j++)
                {
                    Ix[i] = (-Ix[((i - 1) % curBitmap.Height) * curBitmap.Width + j] + Ix[((i + 1) % curBitmap.Height) * curBitmap.Width + j]) / 2;
                    Iy[i] = (Iy[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] - Iy[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]) / 2;

                    Itx[i] = (-It[((i - 1) % curBitmap.Height) * curBitmap.Width + j] + It[((i + 1) % curBitmap.Height) * curBitmap.Width + j]) / 2;
                    Ity[i] = (It[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] - It[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]) / 2;

                }
            }
          //------进行迭代求解-------------------------------  
            double total_error = 100000000000;
            int k = 0;

            while (total_error > tol & k < maxnum)
            {

                //----产生中间变量----Ux Uy Uf Um Uh 、Vx Vy Vm Vh ------
                for (int i = 0; i < curBitmap.Height; i++)
                {

                    for (int j = 0; j < curBitmap.Width; j++)
                    {
                        ux[i] = (-u[((i - 1) % curBitmap.Height) * curBitmap.Width + j] + u[((i + 1) % curBitmap.Height) * curBitmap.Width + j]) / 2;
                        vx[i] = (-v[((i - 1) % curBitmap.Height) * curBitmap.Width + j] + v[((i + 1) % curBitmap.Height) * curBitmap.Width + j]) / 2;

                        uy[i] = (u[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] - u[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]) / 2;
                        vy[i] = (v[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] - v[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]) / 2;

                        uf[i] = (-u[((i - 1) % curBitmap.Height) * curBitmap.Width + j] + u[((i + 1) % curBitmap.Height) * curBitmap.Width + j]) / 2;
                        vf[i] = (v[i * curBitmap.Width + ((j + 1) % curBitmap.Width)] + v[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]) / 2;

                        um[i] = (u[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                - u[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]
                                - u[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                + u[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]) / 4;

                        vm[i] = (v[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                - v[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]
                                - v[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                + v[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]) / 4;

                        uh[i] = (u[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                + u[((i - 1) % curBitmap.Height) * curBitmap.Width + j]
                                + u[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]
                                + u[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                + u[i * curBitmap.Width + ((j + 1) % curBitmap.Width)]
                                + u[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                + u[((i + 1) % curBitmap.Height) * curBitmap.Width + j]
                                + u[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]);

                        vh[i] = (v[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                + v[((i - 1) % curBitmap.Height) * curBitmap.Width + j]
                                + v[((i - 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]
                                + v[i * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                + v[i * curBitmap.Width + ((j + 1) % curBitmap.Width)]
                                + v[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j - 1) % curBitmap.Width)]
                                + v[((i + 1) % curBitmap.Height) * curBitmap.Width + j]
                                + v[((i + 1) % curBitmap.Height) * curBitmap.Width + ((j + 1) % curBitmap.Width)]);

                    }

                }

       
                //---进行 bu bv  求解------------------------------------
                for (int i = 0; i < Im1.Length; i++)
                {
                    bu[i] = 2 * IIx[i] * ux[i] + IIx[i] * vy[i] + IIy[i] * vx[i] + II[i] * uf[i] + II[i] * vm[i] + lambda * uh[i] + Ixt[i];

                    bv[i] = IIy[i] * ux[i] + IIx[i] * uy[i] + 2 * IIy[i] * vy[i] + II[i] * um[i] + II[i] * vf[i] + lambda * vh[i] + Iyt[i];

                    unew[i] = -(B11[i] * bu[i] + B12[i] * bv[i]);
                    vnew[i] = -(B12[i] * bu[i] + B12[i] * bv[i]);
                }

                // 泛函求解 计算误差 total_error
                total_error = 0;

                for (int i = 0; i < Im1.Length; i++)
                {
                    total_error = total_error + Math.Pow(unew[i] - u[i], 2) + Math.Pow(vnew[i] - v[i], 2);
                }

                total_error = Math.Sqrt(total_error) / Im1.Length;


                //将数组替换 进行迭代
                Array.Copy(unew, u, unew.Length);
                Array.Copy(vnew, v, vnew.Length);

                k++;
            }

        }



  #endregion


    }



      
}
