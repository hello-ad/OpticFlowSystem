using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using OpticMeasure.Model;
using OpticMeasure.OptiClass;

namespace OpticMeasure.Class2D
{
    class ReadProject
    {
        public deformationCaliberation[] DefCalRow;
        public int maxhang;//行数
        public int maxlie; //列数

        //# region -----以前格式------
        //public void ReadFromProject()
        //{
          

        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "文本文件|*.*|TEXT(.txt)|*.txt|所有文件|*.*";
        //    openFileDialog.FilterIndex = 2;
        //    openFileDialog.Title = "像点坐标文件打开";
        //    bool succ = false;
        //    string readline;
        //    int pictSum = 0; // 图片总数
        //    int pointSum = 0;
        //    string[] sval;
        //    Value2DPoint v2d;
        //    lunkuo lko;
        //    int num3D = -1;
        //    int codenum = 0, uncodenum = 0, num = 0;

        //    if (openFileDialog.ShowDialog() == DialogResult.OK) // 打开了一个文件
        //    {
        //        string filename = openFileDialog.FileName;
        //        if (File.Exists(filename)) // 文件存在
        //        {
        //            StreamReader sreader = new StreamReader(filename);
        //            readline = sreader.ReadLine();
                   
        //            if (readline == "0x0x0x0x0x0x0x")
        //            {
        //                 文件首行特征符合，方可继续前行，读取以下的数据
        //                for (int i = 0; i > -1; i++)
        //                {
        //                    readline = sreader.ReadLine();
        //                    if (readline.Trim() != "")
        //                        break;
        //                } // 清除之间的空行
        //                string[] cts = new string[0];
        //                readline = sreader.ReadLine();
        //                readline = sreader.ReadLine();//少读一行
        //                if (readline.Contains("No"))
        //                {
        //                     每个照片数据的首行
        //                    sval = readline.Split(':');
        //                    uncodenum = Convert.ToInt32(sval[sval.Length - 1].ToString()); // 非编码点数量
        //                    codenum = Convert.ToInt32(sval[sval.Length - 2].ToString());  // 编码点数量
        //                    num = codenum + uncodenum + 4;  // 本照片数据的数据行数
        //                    Array.Resize(ref cts, num);
        //                    cts[0] = readline;
        //                    cts[1] = sreader.ReadLine();
        //                    cts[2] = sreader.ReadLine();
        //                    cts[3] = sreader.ReadLine();

        //                    for (int k = 0; k < codenum; k++)
        //                    {
        //                        cts[4 + k] = sreader.ReadLine();
        //                    }
        //                    DefCalRow = new deformationCaliberation[num - 4];
        //                    for (int k = 0; k < uncodenum; k++)
        //                    {
        //                        cts[4 + codenum + k] = sreader.ReadLine();
        //                       string[] numbers = cts[4 + codenum + k].Split(',');
                              
        //                       Array.Resize(ref DefCalRow, num - 4);
        //                        numbers = new string[3];
        //                       DefCalRow[k].orgCoordinate.X = float.Parse(numbers[1]);
        //                       DefCalRow[k].orgCoordinate.Y = float.Parse(numbers[2]);
                               

        //                    }
        //                    PictureCodeDataArray[i].ConverToStruct(cts);

        //                    readFileNames[i] = PictureCodeDataArray[i].PictureLujin;
        //                    readFilesindex[i].FileName = PictureCodeDataArray[i].PictureLujin;
        //                    将字符串转化为坐标存起来

        //                    MessageBox.Show("读入完成！");
        //                }
        //            }
        //        }
        //    }

        //}
        //#endregion

    #region  ---现在标定板格式-----
        public void ReadFromProject()
        {


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "文本文件|*.*|TEXT(.txt)|*.txt|所有文件|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.Title = "像点坐标文件打开";
            //bool succ = false;
            string readline;
            //int pictSum = 0; // 图片总数
            //int pointSum = 0;
            ////string[] sval;
            //Value2DPoint v2d;
            //lunkuo lko;
            //int num3D = -1;
            //int codenum = 0, uncodenum = 0, num = 0;

            if (openFileDialog.ShowDialog() == DialogResult.OK) // 打开了一个文件
            {
                string filename = openFileDialog.FileName;
                string[] cts = new string[0];
                DefCalRow = new deformationCaliberation[0];// 装载选定点的数组

                maxhang = 0;
                maxlie = 0;

                if (File.Exists(filename)) // 文件存在
                {
                    StreamReader sreader = new StreamReader(filename);

                    readline = sreader.ReadLine();

                    for (int k = 0; readline != null; k++)
                    {
                        Array.Resize(ref cts, cts.Length + 1);
                        Array.Resize(ref DefCalRow, DefCalRow.Length + 1);

                        cts[k] = readline;
                        string[] numbers = cts[k].Split(',');
                        int index = int.Parse(numbers[0]);

                        int hang = index / 100;
                        int lie = index % 100;
                        if (hang > maxhang)
                        {
                            maxhang = hang;
                        }
                        if (lie > maxlie)
                        {
                            maxlie = lie;
                        }
                        DefCalRow[k].orgCoordinate.X = float.Parse(numbers[1]);
                        DefCalRow[k].orgCoordinate.Y = float.Parse(numbers[2]);
                        readline = sreader.ReadLine();
                    }


                    MessageBox.Show("读入完成！");


                }
            }

        }
#endregion 
        
    }
}
    

