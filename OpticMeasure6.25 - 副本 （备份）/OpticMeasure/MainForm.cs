using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using View3D.Animate;
using TriangulationFun;
using OpticMeasure;
using System.Collections;
using OpticMeasure.Model;
using Microsoft.DirectX;
using OpticMeasure.OptiClass;
using OpticMeasure.Class2D;
using OpticMeasure.OpticFlowFunc;
using System.IO;
using OpticMeasure.OpticForm;
using ZHCLNEW.DrawCloud;
using System.Data.OleDb;

namespace OpticMeasure
{
    public partial class MainForm : Form
    {
        private Form FunFrm;
        private OpticMeasureClass currentOMC = new OpticMeasureClass();
        private ModelDeformationCaliberation model = new ModelDeformationCaliberation();
        private ReadProject readproject = new ReadProject();
        private GetOpticflowResultOfSingleReferencePicture SingleOpticFlow = new GetOpticflowResultOfSingleReferencePicture();
        private FunctionFuncCalss functionFun = new FunctionFuncCalss();
        public ReadPictures readpictures;
        private System.Drawing.Bitmap curBitmap;
        private newOpticFlow newOpticFun ;

        public MainForm()
        {
            InitializeComponent();
        }

        private void 动画生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnimatedForm frm = new AnimatedForm();
            frm.ShowDialog();
        }

        private void 读入照片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FunFrm != null)
                FunFrm.Dispose();
            FunFrm = new ReadPictures(currentOMC);
            ((ReadPictures)FunFrm).selectpictures();
            FunFrm.ShowDialog();
        }

        

        private void 模型变形测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            readproject.ReadFromProject();

            model.ReadPicturesFun();
            model.DefCalRow = readproject.DefCalRow;
            //判断DefCalRow 数组是否为空，为空的话提示“没有读入照片！”
            if (model.DefCalRow == null)
            {
                MessageBox.Show("还没有读入照片！");

            }
            else
            {
                model.CalcuateUncodePointCoordinateFunc(readproject.maxhang, readproject.maxlie);
            }
           

            
            
        }

        private void 读取工程文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            readproject.ReadFromProject();


        }

        private void 单母板ToolStripMenuItem_Click(object sender, EventArgs e)
        {


            currentOMC.PictureOpticflowDataResultArray = new PictureOpticflowDataResultStruct[currentOMC.readFileNames.Length];
            //将结果数组的图片路径和照片编号装起
            for (int i = 0; i < currentOMC.readFileNames.Length; i++)
            {
                currentOMC.PictureOpticflowDataResultArray[i].PictureLujin = currentOMC.readFileNames[i];
                currentOMC.PictureOpticflowDataResultArray[i].PictureIndex = i;

            }
            //装母板数组和待匹配数组 测试用的，真正的装母板数组在界面处
            currentOMC.ReferencePictureDataArray = new ReferencePictureDataStruct[1];
            currentOMC.ReferencePictureDataArray[0].ReferencePictureLuJin = currentOMC.PictureOpticflowDataResultArray[0].PictureLujin;
            currentOMC.ReferencePictureDataArray[0].IndexOfPictureForMachArray = new int[currentOMC.PictureOpticflowDataResultArray.Length - 1];

            for (int i = 0; i < currentOMC.PictureOpticflowDataResultArray.Length - 1; i++)
            {
                currentOMC.ReferencePictureDataArray[0].IndexOfPictureForMachArray[i] = currentOMC.PictureOpticflowDataResultArray[i + 1].PictureIndex;
            }


            currentOMC.OpticFlowFun(currentOMC, SingleOpticFlow);
           
            MessageBox.Show("计算完毕！");
        }



        private void 八邻域函数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Apicturegrayinformation[] ApicturegrayinformationArray;
            ArrayList neighbourhood = new ArrayList();
            //ApicturegrayinformationArray = new Apicturegrayinformation[0];
            //StreamReader rd = File.OpenText("data.txt");
            //string s = rd.ReadLine();
            //string[] ss = s.Split(',');


            //int row = int.Parse(ss[0]); //行数
            //int col = int.Parse(ss[1]);  //每行数据的个数

            //double[,] p1 = new double[row, col]; //数组

            //for (int i = 0; i < row; i++)  //读入数据并赋予数组
            //{
            //    string line = rd.ReadLine();
            //    string[] data = line.Split(',');
            //    for (int j = 0; j < col; j++)
            //    {
            //        Array.Resize(ref ApicturegrayinformationArray, ApicturegrayinformationArray.Length + 1);
            //        ApicturegrayinformationArray[i].currentgrayValue = byte.Parse(data[j]);
            //    }
            //}
            functionFun.racepossibleEdgePointIndexFunc(5120,5120,11884921,ref neighbourhood);


        }

        private void 灰度差叠加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FunFrm != null)
                FunFrm.Dispose();
            FunFrm = new GrayAddPicture(currentOMC);
            FunFrm.MdiParent = this;
            FunFrm.Dock = DockStyle.Fill;
            FunFrm.Show();
            FunFrm.WindowState = FormWindowState.Maximized; // 最大化窗体
        }

        private void 计算旋转矩阵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CSharpAlgorithm.Algorithm.MatrixOfAlgorithm Mt;
            Mt = new CSharpAlgorithm.Algorithm.MatrixOfAlgorithm();
            int i = 0;
            double x1,y1,x2,y2,z;
            //---1.2点算出来的旋转矩阵---------
            //x1 = 1345.742 * 0.980203477978675 + 0 * 0.021337356029396348 + 698.7499 * 0.10131380503641996 + 0 * 1.098189530500544;

            //y1 = 0 * 0.980203477978675 + 1345.742 * 0.021337356029396348 + 0 * 0.10131380503641996 + 698.7499 * 1.098189530500544;

            //x2 = 3161.329 * 0.980203477978675 + 0 * 0.021337356029396348 + 559.1767 * 0.10131380503641996 + 0 * 1.098189530500544;

            //y2 = 0 * 0.980203477978675 + 3161.329 * 0.021337356029396348 + 0 * 0.10131380503641996 + 559.1767 * 1.098189530500544;
            //---------1,2,3,4点算出来的旋转矩阵----

            x1 = 1366.758 * 0.00000853799846170511 + 0 * (-0.11570209301565185) + 1679.83 * 0.99997814805227869 + 0 * 1.3683102952151762;

            y1 = 0 * 0.00000853799846170511 + 1366.758 * (-0.11570209301565185) + 0 * 0.99997814805227869 + 1679.83 * 1.3683102952151762;

            x2 = 3166.972 * 0.00000853799846170511 + 0 * (-0.11570209301565185) + 1658.789 * 0.99997814805227869 + 0 * 1.3683102952151762;

            y2 = 0 * 0.00000853799846170511 + 3166.972 * (-0.11570209301565185) + 0 * 0.99997814805227869 + 1658.789 * 1.3683102952151762;

            z = x1 + y1 + x2 + y2;
            while (i<2)
            {
                functionFun.GetRotationMatrixAndTranslationFunc(ref Mt);
                
                i++;
            }
        }

        private void 二维矢量图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FunFrm != null)
                FunFrm.Dispose();
            FunFrm = new DrawVector2Frm(currentOMC, SingleOpticFlow);
            FunFrm.MdiParent = this;
            FunFrm.Dock = DockStyle.Fill;
            FunFrm.Show();
            FunFrm.WindowState = FormWindowState.Maximized;
        }

        private void 保存梯度量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentOMC.PictureOpticflowDataResultArray[1].OpticflowResultFOfPointsArray != null)
            {
                //定义
                String FileName = "";//文件名

                //算法
                //Step1、创建一个保存文件对话框
                SaveFileDialog SaveDlg = new SaveFileDialog();

                //Step2、设置打开文件筛选器
                SaveDlg.Filter = "文本格式文件|*.txt";

                //Step3、设置对话框标题
                SaveDlg.Title = "保存梯度量";// "保存文件";

                //Step4、判断 结果为打开，选定文件
                //若 SaveDlg.ShowDialog()==DialogResult.OK 
                if (SaveDlg.ShowDialog() == DialogResult.OK)
                {
                    FileName = SaveDlg.FileName;
                }

                string FileNameTemp = FileName;
                //Step4、定义输出流
                StreamWriter FileStream = new StreamWriter(FileNameTemp); // 定义工程文件的输出流
                //Step5、写标记行头
                FileStream.WriteLine("TITLE=\"DisplacementsFor2D\""); //文件标识 第一行

                for (int i = 0; i < currentOMC.PictureOpticflowDataResultArray[1].OpticflowResultFOfPointsArray.Length; i++)
                {
                    string temp = currentOMC.PictureOpticflowDataResultArray[1].OpticflowResultFOfPointsArray[i].pointCoordinate.X.ToString() + "," + currentOMC.PictureOpticflowDataResultArray[1].OpticflowResultFOfPointsArray[i].pointCoordinate.Y.ToString() + "," + currentOMC.PictureOpticflowDataResultArray[1].OpticflowResultFOfPointsArray[i].Grad.X.ToString() + "," + currentOMC.PictureOpticflowDataResultArray[1].OpticflowResultFOfPointsArray[i].Grad.Y.ToString();

                    FileStream.WriteLine(temp);
                }

                FileStream.Close();   // 关闭输出流
                FileStream.Dispose(); // 释放输出流所占用的资源
            }
        }

        private void 光流计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newOpticFun = new newOpticFlow();
            newOpticFun.Flow_Diagnostics_Run_Refine( currentOMC);

        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //读入Excel数据
            #region   测试读入Excel数据

            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //DataTable ExcelTable;
            //openFileDialog1.ShowDialog();
            //string MyFileName = openFileDialog1.FileName;
            //if (MyFileName.Trim() == "")
            //{
            //    return;
            //}
            //string MyFilePath = Path.GetFullPath(MyFileName);
            //string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + MyFilePath + ";Extended Properties=Excel 8.0;";
            //OleDbConnection myConn = new OleDbConnection(strCon);
            //string strCom = " SELECT * FROM [Sheet1$] ";
            //myConn.Open();
            //OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            //DataSet myDataSet = new DataSet();
            //myCommand.Fill(myDataSet, "[Sheet1$]");
            //myConn.Close();
            //ExcelTable = myDataSet.Tables[0];

            //int iRows = ExcelTable.Rows.Count;
            //int iColums = ExcelTable.Columns.Count;
            ////string[,] StoreData = new string[iRows, iColums];
            //string[] StoreData = new string[iRows * iColums];
            //float[] Data = new float[iRows * iColums];
            //int k = 0;
            //for (int i = 0; i < ExcelTable.Rows.Count; i++)
            //    for (int j = 0; j < ExcelTable.Columns.Count; j++)
            //    {
            //        //将Excel表中的数据存储到数组
            //        StoreData[k] = ExcelTable.Rows[i][j].ToString();
            //        k++;
            //    }
            //for (int i = 0; i < StoreData.Length;i++ )
            //{
            //    //Data[i] =(float) StoreData[i];
            //}
            
            
            #endregion 

            Bitmap ReferencePictureBitmap;// 图像对象
            Bitmap CurPictureBitmap;// 图像对象
            CreatArray CreatArray = new CreatArray();//创建数组对象

            //得到bitmap
            ReferencePictureBitmap = (Bitmap)Image.FromFile(currentOMC.readFileNames[0]);//得到母板点的Bitmap
            CurPictureBitmap = (Bitmap)Image.FromFile(currentOMC.readFileNames[1]);
               
            Apicturegrayinformation[] Im1=new Apicturegrayinformation[ReferencePictureBitmap.Height*ReferencePictureBitmap.Width];
            Apicturegrayinformation[] Im2=new Apicturegrayinformation[CurPictureBitmap.Width*CurPictureBitmap.Height];
            Apicturegrayinformation[] Im2correction = new Apicturegrayinformation[CurPictureBitmap.Width * CurPictureBitmap.Height];
            // 装载灰度数组
              
            //CreatArray.creatPictureGreyArray(ReferencePictureBitmap, ref Im1);
            //CreatArray.creatPictureGreyArray(CurPictureBitmap, ref Im2);

            functionFun.LoadgrayValuesArrayForMatch(ReferencePictureBitmap, ref Im1);//装载母板灰度数组
            functionFun.LoadgrayValuesArrayForMatch(CurPictureBitmap, ref Im2);//装载后一帧图片灰度数组

            float[] unew = new float[Im1.Length];
            float[] vnew = new float[Im1.Length];
            // 光强修正

            functionFun.correction_illumination(CurPictureBitmap, Im1, Im2, ref Im2correction);
            //预处理


            
            functionFun.horn_schunk_estimator(CurPictureBitmap, Im1, Im2, ref unew, ref vnew);

        }

        
        
        

       
    }
}
