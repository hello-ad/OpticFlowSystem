using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using OpticMeasure.OpticFlowFunc;


namespace OpticMeasure
{
    public class FetchImage
    {
        private OpticMeasureClass opc;
        public FetchImage(OpticMeasureClass _opc) // 构造函数
        {
            opc = _opc;
        }

        ~FetchImage() // 析构函数
        { 
        }
        public void Destroy()
        { 
        }

        public void ReadPicture(string filename, int curFileIndex)
        {
            // 对照片的处理过程

        }
    }

    public class OpticMeasureClass
    {
        #region // 全局变量定义

        public string[] readFileNames = new string[0]; // 读入的图片文件名数组
        public int numInProBatch = 1000;  // 每组内部的 分组数量
        public int currentPictureIndex = -1; // 当前正显示的照片编号

        public  Apicturegrayinformation[] CurrentPictureGreyArray;  //当前照片的灰度数组
        public  Apicturegrayinformation[] ReferencePictureGreyArray;  //参考照片（即母版）的灰度数组
        public  PictureOpticflowDataResultStruct[] PictureOpticflowDataResultArray; //光流法结果数组，长度等于采集照片数
        public  ReferencePictureDataStruct[] ReferencePictureDataArray;  //参考照片（即母版）数组，长度等于所选择的参考照片（即母版）总数

        public  float FactorOfGreygrad = 1.0f; //灰度差缩放因子
        public  bool  graydeltaAdd = false; // 叠加灰度差
        public  bool  forwardAdd = true;  // 正向叠加
        public  bool  grayed = false; //灰度化显示
        public  bool  biaodingDsp = true; // 显示模板照片
        public float maxValue = 0, minValue = 0; // 最大、最小的灰度差值  --- 灰度差 的极值


        public  byte TypeOfGreygradComputation; //拟用的灰度梯度模板类型，用于Switch Case判断。
        public  int StepValue;       //像素点间间隔
        public  bool  IsStep;       //=true表示需要跳点，跳点间隔=StepValue
        public  int  StepOfReferencePicture; //母板间隔张数
        public  bool  IsVibrationcorrection;//是否去震动
        public  int TypeOfTemplate; // 像素灰度差计算，即 灰度值直接相减还是求八邻域灰度差

        private Bitmap curBitmap;

        #endregion

        public OpticMeasureClass() // 构造函数
        {

            PictureOpticflowDataResultArray = new PictureOpticflowDataResultStruct[2];
            ReferencePictureDataArray = new ReferencePictureDataStruct[1];

            ReferencePictureDataArray[0].ReferencePictureLuJin = PictureOpticflowDataResultArray[0].PictureLujin;
            ReferencePictureDataArray[0].IndexOfPictureForMachArray = new int[1];
            ReferencePictureDataArray[0].IndexOfPictureForMachArray[0] = PictureOpticflowDataResultArray[1].PictureIndex;
 
        }
        ~OpticMeasureClass() // 析构函数
        { 

        }
        //====光流函数=======================
        public void OpticFlowFun(OpticMeasureClass opticFlowFunc, GetOpticflowResultOfSingleReferencePicture _singleOpticFlowFun)
        {
           
            _singleOpticFlowFun.GetOpticflowResultOfSingleReferencePictureFunc(opticFlowFunc, 0);

        }

       

        public void ClearTmpData() // 清理临时数据，可以随时调用
        { 

        }
        public void InitReadPicturesParameters(int filenumber)
        {
            readFileNames = new string[filenumber];
        }
        public void SaveTmpData(int start, int end, int a)
        {
            //throw new NotImplementedException();
        }

        public  void ClearDataArray(int p, int p_2)
        {
            //throw new NotImplementedException();
        }

        public void DeletedFileAllAfterRead()
        {
            //throw new NotImplementedException();
            DeleteTmpData(); // 删除所有的临时数据，以备照片读取
            if (readFileNames.Length > 0)
                Array.Resize(ref readFileNames, 0);
        }
        public void DeleteTmpData()
        {
            string path = Application.StartupPath;
            path = path + @"\TmpData";
            if (System.IO.Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                FileInfo[] files = dirInfo.GetFiles();   // 获取该目录下的所有文件
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
            }

        }
        public bool RecoveryTmpData(int start, int end, int batchNo, int retain)
        {
            return true;
        }
        //---7.3----
        public void getMaxMin()
        {
            float min = 0, max = 0;
            if (PictureOpticflowDataResultArray != null)
                for (int i = 0; i < PictureOpticflowDataResultArray.Length; i++)
                {
                    if (PictureOpticflowDataResultArray[i].OpticflowResultFOfPixelArray != null)
                    {
                        for (int j = 0; j < PictureOpticflowDataResultArray[i].OpticflowResultFOfPixelArray.Length; j++)
                        {
                            if (PictureOpticflowDataResultArray[i].OpticflowResultFOfPixelArray[i].Greyvalue > max)
                                max = PictureOpticflowDataResultArray[i].OpticflowResultFOfPixelArray[i].Greyvalue;
                            if (PictureOpticflowDataResultArray[i].OpticflowResultFOfPixelArray[i].Greyvalue < min)
                                min = PictureOpticflowDataResultArray[i].OpticflowResultFOfPixelArray[i].Greyvalue;
                        }
                    }
                }
            maxValue = max;
            minValue = min; // 获得 灰度差 的 最大最小值
        }

          private int valuesToArgb(float min, float max, float value, Color C1, Color C2)
        {
            if (Math.Abs(max - min) > 0)
            {
                int left, right;
                left = C1.R; right = C2.R;
                float r = right * (value - min) / (max - min) + left * (max - value) / (max - min);

                if (r < 0) r = 0;
                if (r > 255) r = 255;

                left = C1.G; right = C2.G;
                float g = right * (value - min) / (max - min) + left * (max - value) / (max - min);
                if (g < 0) g = 0;
                if (g > 255) g = 255;

                left = C1.B; right = C2.B;
                float b = right * (value - min) / (max - min) + left * (max - value) / (max - min);
                if (b < 0) b = 0;
                if (b > 255) b = 255;
                Color C = Color.FromArgb((int)r, (int)g, (int)b);
                return C.ToArgb();
            }
            else
            {
                return C1.ToArgb();
            }

        }
        


        public bool ViewPicture(PictureBox picture, int index,string referenceFilename, PictureOpticflowDataResultStruct[] PictureOpticflowDataResultArray)
        {
            if (System.IO.File.Exists(referenceFilename) == false)
            {
                MessageBox.Show(" 模板文件：" + referenceFilename + " 文件不存在！");
                return false;
            }
            else
            {
                if (readFileNames != null)
                {
                    if (PictureOpticflowDataResultArray != null)
                    {
                        if (PictureOpticflowDataResultArray[index].OpticflowResultFOfPixelArray != null)
                        {
                            curBitmap = GrayPicture(referenceFilename, index, PictureOpticflowDataResultArray);
                            picture.Image = curBitmap;
                        }
                        else
                            MessageBox.Show("尚未进行像素灰度差计算！！！");
                    }
                    else
                        MessageBox.Show("尚未进行光流计算！！！");
                }
                return true;
            }
        }
        private Bitmap GrayPicture(string filename, int index, PictureOpticflowDataResultStruct[] PictureOpticflowDataResultArray)
        {
            Bitmap bmp = (Bitmap)Image.FromFile(filename);
            int PictureWidth = bmp.Width;
            int PictureHeight = bmp.Height;

            int strideValue;
            double max = 0, min = 255;
            float AverageOrigingray = 0;
            double SumCurentgray = 0;
            int lenth = PictureWidth * PictureHeight;
            if (lenth == PictureOpticflowDataResultArray[index].OpticflowResultFOfPixelArray.Length)
            {
                Apicturegrayinformation[] ApicturegrayinformationArray = new Apicturegrayinformation[lenth];

                Rectangle rect = new Rectangle(0, 0, PictureWidth, PictureHeight);
                System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                IntPtr ptr = bmpData.Scan0;

                if (grayed == true) // 灰度化显示
                {
                    if (PictureWidth % 4 == 0)
                    {
                        strideValue = PictureWidth * 3;

                        byte[] rgbValues = new byte[lenth * 3];
                        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, lenth * 3);
                        //灰度化
                        Dogray(ref ApicturegrayinformationArray, rgbValues, strideValue, PictureWidth, PictureHeight, false, ref max, ref min, ref AverageOrigingray);

                        //灰度拉伸
                        graystretch(ref ApicturegrayinformationArray, max, min, ref SumCurentgray);
                    }
                    else
                    {
                        strideValue = bmpData.Stride;
                        int bytes = strideValue * PictureHeight;
                        byte[] rgbValues = new byte[bytes];
                        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                        //灰度化
                        Dogray(ref ApicturegrayinformationArray, rgbValues, strideValue, PictureWidth, PictureHeight, false, ref max, ref min, ref AverageOrigingray);
                        //灰度拉伸
                        graystretch(ref ApicturegrayinformationArray, max, min, ref SumCurentgray);
                        //光滑去噪
                        //showgussSmooth(ref ApicturegrayinformationArray);
                    }

                    bmp.UnlockBits(bmpData);
                    bmp.Dispose();
                    GC.Collect();

                    curBitmap = CreateBmpByGrays(ApicturegrayinformationArray, index, PictureWidth, PictureHeight, PictureOpticflowDataResultArray);
                    return curBitmap;
                } // 灰度化显示
                else // 彩色显示
                {
                    float tmp = 0;

                    if (PictureWidth % 4 == 0)
                    {
                        strideValue = PictureWidth * 3;

                        byte[] rgbValues = new byte[lenth * 3];
                        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, lenth * 3);

                        for (int i = 0; i < lenth; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                if (biaodingDsp == true)
                                    tmp = rgbValues[i * 3 + j];
                                else
                                    tmp = 0;
                                if (graydeltaAdd == true) // 灰度叠加
                                {
                                    if (forwardAdd == true) // 正向叠加
                                    {
                                        tmp += PictureOpticflowDataResultArray[index].OpticflowResultFOfPixelArray[i].Greyvalue * FactorOfGreygrad;
                                    }
                                    else
                                    {
                                        tmp -= PictureOpticflowDataResultArray[index].OpticflowResultFOfPixelArray[i].Greyvalue * FactorOfGreygrad;
                                    }
                                }
                                if (tmp > 255)
                                    rgbValues[i * 3 + j] = 255;
                                if (tmp < 0)
                                    rgbValues[i * 3 + j] = 0;
                                if ((tmp >= 0) & (tmp <= 255))
                                    rgbValues[i * 3 + j] = (byte)tmp;
                            }
                        }
                        System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, lenth * 3);
                    }
                    else
                    {
                        strideValue = bmpData.Stride;
                        int bytes = strideValue * PictureHeight;
                        byte[] rgbValues = new byte[bytes];
                        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                        for (int i = 0; i < rgbValues.Length; i++)
                        {
                            if (biaodingDsp == true)
                                tmp = rgbValues[i];
                            else
                                tmp = 0;
                            if (graydeltaAdd == true) // 灰度叠加
                            {
                                if (forwardAdd == true) // 正向叠加
                                {
                                    tmp += PictureOpticflowDataResultArray[index].OpticflowResultFOfPixelArray[i].Greyvalue * FactorOfGreygrad;
                                }
                                else
                                {
                                    tmp -= PictureOpticflowDataResultArray[index].OpticflowResultFOfPixelArray[i].Greyvalue * FactorOfGreygrad;
                                }
                            }
                            if (tmp > 255)
                                rgbValues[i] = 255;
                            if (tmp < 0)
                                rgbValues[i] = 0;
                            if ((tmp >= 0) & (tmp <= 255))
                                rgbValues[i] = (byte)tmp;
                        }
                        System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                    }

                    bmp.UnlockBits(bmpData);

                    return bmp;
                }
            }
            else
            {
                MessageBox.Show("计算灰度差的照片大小与当前照片大小不一致！！！");
                return bmp; 
            }

        }
        private Bitmap CreateBmpByGrays(Apicturegrayinformation[] ApicturegrayinformationArray,int index, int PictureWidth, int PictureHeight,PictureOpticflowDataResultStruct[] PictureOpticflowDataResultArray)
        {
                Bitmap bit = new Bitmap(PictureWidth, PictureHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                //可读写的方式锁定全部容器 
                System.Drawing.Imaging.BitmapData data2 = bit.LockBits(new Rectangle(0, 0, bit.Width, bit.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bit.PixelFormat);
                //得到首地址 
                IntPtr ptr2 = data2.Scan0;
                //计算24位图像的字节数 
                int bytes = PictureWidth * PictureHeight;

                //定义位图数组 
                byte[] grayValues = new byte[bytes];
                //复制被锁定的图像到该数组 
                System.Runtime.InteropServices.Marshal.Copy(ptr2, grayValues, 0, bytes);
                float tmp;

                for (int i = 0; i < bytes; i++)
                {
                    if (biaodingDsp == true) // 显示模板数据
                        tmp = (byte)ApicturegrayinformationArray[i].OrigingrayValue;
                    else
                        tmp = 0;

                    if (graydeltaAdd == true) // 灰度差叠加
                    {
                        if (forwardAdd == true) // 正向叠加
                        {
                            tmp += PictureOpticflowDataResultArray[index].OpticflowResultFOfPixelArray[i].Greyvalue * FactorOfGreygrad;
                        }
                        else
                        {
                            tmp -= PictureOpticflowDataResultArray[index].OpticflowResultFOfPixelArray[i].Greyvalue * FactorOfGreygrad;
                        }
                    }
                    if (tmp > 255)
                        grayValues[i] = 255;
                    if (tmp < 0)
                        grayValues[i] = 0;
                    if ((tmp >= 0) & (tmp <= 255))
                        grayValues[i] = (byte)tmp;
                } //送回数组 
                System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, ptr2, bytes);

                //解锁 
                bit.UnlockBits(data2);

                ColorPalette tempPalette;
                using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
                {
                    tempPalette = tempBmp.Palette;
                }
                for (int i = 0; i < 256; i++)
                {
                    tempPalette.Entries[i] = Color.FromArgb(i, i, i);
                }

                bit.Palette = tempPalette;

                return bit;
        }
        private void Dogray(ref Apicturegrayinformation[] ApicturegrayinformationArray, byte[] rgbValues, int strideValue, int PictureWidth, int PictureHeight, bool isBlack, ref double max, ref double min, ref float AverageOrigingray) // 灰度化处理
        {
            double colorTemp;
            max = 0;
            min = 255;
            double AverageOriginValue = 0;

            if ((strideValue - PictureWidth * 3) == 0)
            {
                for (int i = 0; i < rgbValues.Length; i += 3)
                {
                    if (isBlack == true)
                    {
                        colorTemp = (255 - rgbValues[i + 2]) * 0.299 + (255 - rgbValues[i + 1]) * 0.587 + (255 - rgbValues[i]) * 0.114;
                    }
                    else
                        colorTemp = rgbValues[i + 2] * 0.299 + rgbValues[i + 1] * 0.587 + rgbValues[i] * 0.114;
  
                    ApicturegrayinformationArray[i / 3].OrigingrayValue = (float)colorTemp;

                    if (max < colorTemp)
                    {
                        max = colorTemp;
                    }
                    if (min > colorTemp)
                    {
                        min = colorTemp;
                    }
                    AverageOriginValue += colorTemp;
                }
            }
            else
            {
                float[] TemprgbValuesArray = new float[rgbValues.Length];

                for (int i = 0; i < PictureHeight; i++)
                {
                    for (int j = 0; j < PictureWidth * 3; j += 3)
                    {
                        if (isBlack == true)
                            colorTemp = (255 - rgbValues[i * strideValue + j + 2]) * 0.299 + (255 - rgbValues[i * strideValue + j + 1]) * 0.587 + (255 - rgbValues[i * strideValue + j]) * 0.114;
                        else
                            colorTemp = rgbValues[i * strideValue + j + 2] * 0.299 + rgbValues[i * strideValue + j + 1] * 0.587 + rgbValues[i * strideValue + j] * 0.114;
                        TemprgbValuesArray[i * strideValue + j + 2] = TemprgbValuesArray[i * strideValue + j + 1] = TemprgbValuesArray[i * strideValue + j] = (float)colorTemp;
                        if (max < colorTemp)
                        {
                            max = colorTemp;
                        }
                        if (min > colorTemp)
                        {
                            min = colorTemp;
                        }
                        AverageOriginValue += colorTemp;
                    }
                }

                for (int i = 0; i < PictureHeight; i++)
                {
                    for (int j = 0; j < PictureWidth * 3; j += 3)
                    {
                        ApicturegrayinformationArray[i * PictureWidth + j / 3].OrigingrayValue = TemprgbValuesArray[i * strideValue + j];
                    }
                }

            }
            
            AverageOrigingray = (float)(AverageOriginValue / ApicturegrayinformationArray.Length);
        }
        private void graystretch(ref Apicturegrayinformation[] ApicturegrayinformationArray, double max, double min, ref double SumCurentgray) // 灰度拉伸
        {
            double Curentgraytemp;
            SumCurentgray = 0;
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
                SumCurentgray += Curentgraytemp;
                ApicturegrayinformationArray[i].currentgrayValue = (byte)Curentgraytemp;

            }
            SumCurentgray = SumCurentgray / ApicturegrayinformationArray.Length;

        }

    }
}
