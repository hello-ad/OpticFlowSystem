using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HL2D;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using ZH2DCL._2DClass;
using UserProgressBar;
using OpticMeasure;
using OpticMeasure.OpticFlowFunc;

namespace OpticMeasure
{
    public partial class ReadPictures : Form
    {
        private  bool autoTrans = true;
        private OpticMeasureClass currentOMC;
      
        //private DensityGenerationFromDisplacements computerVector2;
        BackProgressBar backThread = null;
       
        //OpticFlow opticFlowFunc = new OpticFlow();//创建光流结果对象
       // PictureOpticflowDataResultStruct[] PictureOpticflowDataResultArray;// 光流结果数组，长度等于采集照片数


        public ReadPictures(OpticMeasureClass _currentOMC)
        {
            InitializeComponent();
            currentOMC = _currentOMC;
            //computerVector2 = _computerVector2;
        }

        public ReadPictures()
        {
            // TODO: Complete member initialization
        }

        public void selectpictures( )
        {
       
            OpenFileDialog opnDlg = new OpenFileDialog();
            opnDlg.Filter = "所有图像文件|*.bmp;*.pcx;*.png;*.jpg;*.gif;" + "*.tif;*.ico;*.dxf;*.cgm;*.cdr;*.wmf;*.eps;*.emf|" + "位图（*.bmp;*.jpg;*.png;........)|*.bmp;*.pcx;*.png;*.jpg;*.gif;*.tif;*ico|" + "矢量图（*.wmf;*.eps;*.emf;....)|*.dxf;*.cgm;*.cdr;*.wmf;*.eps;*.emf";
            //设置对话框标题
            opnDlg.Title = "打开图像文件";
            opnDlg.Multiselect = true;
            //启用“帮助”按钮
            opnDlg.ShowHelp = true;
            TreeNode node = null;

            if (opnDlg.ShowDialog() == DialogResult.OK)
            {
               
                // 选择了文件！
                string[] files = opnDlg.FileNames; // 选择的图像文件的文件名，含文件路径
                for (int i = 0; i < files.Length; i++)
                {
                    node = new TreeNode();
                    node.Text = getFileNameOnly(files[i]);
                    node.Name = files[i];
                    this.tVLeft.Nodes.Add(node);
                }
                //将图片的路径信息，编号存到光流数组中
                if (currentOMC.PictureOpticflowDataResultArray == null)
                {
                    currentOMC.PictureOpticflowDataResultArray = new PictureOpticflowDataResultStruct[files.Length];
                }
                else
                {
                    Array.Resize(ref  currentOMC.PictureOpticflowDataResultArray, files.Length);
                }
                for (int i = 0; i < files.Length; i++)
                {
                    Array.Resize(ref currentOMC.PictureOpticflowDataResultArray, (currentOMC.PictureOpticflowDataResultArray.Length + 1));
                    currentOMC.PictureOpticflowDataResultArray[i].PictureLujin = files[i];
                    
                }
            }
            opnDlg.Dispose();
            this.toolStripProgressBar1.Maximum = this.tVLeft.Nodes.Count ;
            // 设置进度条的最大数目
        }

        private string getFileNameOnly(string filename)
        {
            string nameOnly = "";
            for (int i = filename.Length; i > 0; i--)
            {
                if (filename[i - 1] != '\\')
                    nameOnly = filename[i - 1] + nameOnly;
                else
                    break;
            }
            return nameOnly;
        }
        private string getFileNameNonExtend(string filenameNonPath)
        {
            string name = "";
            for (int i = 0; i < filenameNonPath.Length; i++)
            {
                if (filenameNonPath[i] != '.')
                    name += filenameNonPath[i];
                else
                    break;
            }
            return name;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            selectpictures();
            // 选择照片
        }
        private void 删除单张照片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tVLeft.SelectedNode != null)
            if (MessageBox.Show("确认删除图片：" + this.tVLeft.SelectedNode.Text, "删除确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.tVLeft.Nodes.Remove(this.tVLeft.SelectedNode);
            }
        }
        private void 删除所有照片ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除所有图片", "删除确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.tVLeft.Nodes.Clear();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除所有图片", "删除确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.tVLeft.Nodes.Clear();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // 退出
            this.pictureBox1.Dispose();
            this.Close();
        }


        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            // 照片格式转换
            PictureTo24bitJpg jpgfrm = new PictureTo24bitJpg();
            jpgfrm.ShowDialog();
        }

        private void readImageTask(int index,int start)
        {
            FetchImage fi = new FetchImage(currentOMC);
            fi.ReadPicture(tVLeft.Nodes[index].Name, index );
            // 当前照片 读入到 到 pictureCodedataArray 数组中

            fi.Destroy();

            GC.Collect();
        }
        private void readBackWork()
        {
            backThread = new BackProgressBar(readImagesUseBatch);
            backThread.RunBackWorker(true);
        }
        //private void readImages()
        //{
        //    //currentOMC.InitReadPicturesParameters(tVLeft.Nodes.Count, (float)-0.45, (float)-0.5);
        //    int total = tVLeft.Nodes.Count;

        //    ImageCommon imagecommon = new ImageCommon();
        //    int cpucore = imagecommon.availCPUCore(tVLeft.Nodes[0].Name);

        //    if ((cpucore > 1) & (cpucore <= 3))
        //        cpucore = 2;
        //    if ((cpucore >= 4) & (cpucore <= 7))
        //        cpucore = 4;
        //    if (cpucore >= 8)
        //        cpucore = 8;

        //    int num = 0;
        //    //FetchImage fim1; // fim2, fim3, fim4;

        //    var watch = Stopwatch.StartNew();
        //    watch.Start();
        //    if (tVLeft.Nodes.Count > 0)
        //    {
        //        this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[0].Name);
        //        this.toolStripStatusLabel1.Text = "";
        //        if (this.checkBox1.Checked)
        //            backThread.ReportProgress(1, total); // 第一个进度，10%
        //        else
        //        {
        //            this.toolStripProgressBar1.Maximum = tVLeft.Nodes.Count;
        //            this.toolStripProgressBar1.Value = 1;
        //            this.pictureBox1.Refresh();
        //        }

        //        switch (cpucore)
        //        {
        //            case 1:  // 单核处理器
        //                //fim1 = new FetchImage(currentOMC);
                        
        //                for (int i = 0; i < tVLeft.Nodes.Count; i++)
        //                {
        //                    var curwatch = Stopwatch.StartNew();
        //                    curwatch.Start();

        //                    currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                    //fim1.ReadPicturesByFileName(tVLeft.Nodes[i].Name, i);

        //                    var t1 = new Task(() =>
        //                    {
        //                        readImageTask(num,0);
        //                    });
        //                    t1.Start();

        //                    Task.WaitAll(t1);
        //                    num += 1;

        //                    curwatch.Stop();
        //                    this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 1))).ToString("0.0") + " 秒";

        //                    this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[i].Name);
        //                    if (this.checkBox1.Checked)
        //                        backThread.ReportProgress(i+1, total); // 第一个进度，10%
        //                    else
        //                    {
        //                        this.toolStripProgressBar1.Value = i+1;
        //                        this.pictureBox1.Refresh();
        //                    }

        //                }
        //                //fim1.Destroy();
        //                GC.Collect();
        //                break;
        //            case 2:  // 2 核处理器
        //                #region
        //                for (int i = 0; i < tVLeft.Nodes.Count; )
        //                {
        //                    var curwatch = Stopwatch.StartNew();
        //                    curwatch.Start();

        //                    currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                    var t1 = new Task(() =>
        //                    {
        //                        readImageTask(num,0);
        //                    });
        //                    t1.Start();
        //                    i++;

        //                    if (i < tVLeft.Nodes.Count)
        //                    {
        //                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                        var t2 = new Task(() =>
        //                        {
        //                            readImageTask(num + 1,0);
        //                        });
        //                        t2.Start();
        //                        try
        //                        {
        //                            Task.WaitAll(t1, t2);
        //                        }
        //                        catch
        //                        {
        //                            MessageBox.Show("并行计算出错！");
        //                        }
        //                        num += 2;
        //                        curwatch.Stop();
        //                        this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 2))).ToString("0.0") + " 秒";

        //                        i++;
        //                    }
        //                    else
        //                    {
        //                        Task.WaitAll(t1);

        //                        num += 1;
        //                        curwatch.Stop();
        //                        this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 1))).ToString("0.0") + " 秒";
        //                    }

        //                   this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[i - 1].Name);
        //                   if (this.checkBox1.Checked)
        //                        backThread.ReportProgress(i, total); // 第一个进度，10%
        //                    else
        //                    {
        //                        this.toolStripProgressBar1.Value = i;
        //                        this.Refresh();
        //                     }

        //                    GC.Collect();
        //                }
        //                #endregion
        //                break;
        //            case 4:  // 4 核处理器
        //                #region
        //                for (int i = 0; i < tVLeft.Nodes.Count; )
        //                {
        //                    var curwatch = Stopwatch.StartNew();
        //                    curwatch.Start();
                            
        //                    currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                    var t1 = new Task(() =>
        //                    {
        //                        readImageTask(num,0);
        //                    });
        //                    t1.Start();
        //                    i++;

        //                    if (i < tVLeft.Nodes.Count)  // 启动 2 号任务
        //                    {
        //                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                        var t2 = new Task(() =>
        //                        {
        //                            readImageTask(num + 1,0);
        //                        });
        //                        t2.Start();
        //                        i++;

        //                        if (i < tVLeft.Nodes.Count)  // 启动 3 号任务
        //                        {
        //                            currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                            var t3 = new Task(() =>
        //                            {
        //                                readImageTask(num + 2,0);
        //                            });
        //                            t3.Start();
        //                            i++;

        //                            if (i < tVLeft.Nodes.Count) //启动 4 号任务
        //                            {
        //                                currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                                var t4 = new Task(() =>
        //                                {
        //                                    readImageTask(num + 3,0);
        //                                });
        //                                t4.Start();
        //                                i++;

        //                                Task.WaitAll(t1, t2, t3, t4);

        //                                num += 4;
        //                                curwatch.Stop();
        //                                this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 4))).ToString("0.0") + " 秒";

        //                            }
        //                            else
        //                            {
        //                                Task.WaitAll(t1, t2, t3);

        //                                num += 3;
        //                                curwatch.Stop();
        //                                this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 3))).ToString("0.0") + " 秒";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Task.WaitAll(t1, t2);

        //                            num += 2;
        //                            curwatch.Stop();
        //                            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 2))).ToString("0.0") + " 秒";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Task.WaitAll(t1);

        //                        num += 1;
        //                        curwatch.Stop();
        //                        this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 1))).ToString("0.0") + " 秒";
        //                    }

        //                    this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[i - 1].Name);
        //                    if (this.checkBox1.Checked)
        //                        backThread.ReportProgress(i, total); // 第一个进度，10%
        //                    else
        //                    {
        //                        this.toolStripProgressBar1.Value = i;
        //                        this.pictureBox1.Refresh();
        //                    }

        //                    GC.Collect();
        //                }
        //                #endregion
        //                break;
        //            case 8: // 8 核处理器
        //                #region
        //                for (int i = 0; i < tVLeft.Nodes.Count; )
        //                {
        //                    var curwatch = Stopwatch.StartNew();
        //                    curwatch.Start();

        //                    currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字

        //                    var t1 = new Task(() =>
        //                    {
        //                        readImageTask(num,0);
        //                    });
        //                    t1.Start();
        //                    i++;

        //                    if (i < tVLeft.Nodes.Count)  // 启动 2 号任务
        //                    {
        //                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                        var t2 = new Task(() =>
        //                        {
        //                            readImageTask(num + 1,0);
        //                        });
        //                        t2.Start();
        //                        i++;

        //                        if (i < tVLeft.Nodes.Count)  // 启动 3 号任务
        //                        {
        //                            currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                            var t3 = new Task(() =>
        //                            {
        //                                readImageTask(num + 2,0);
        //                            });
        //                            t3.Start();
        //                            i++;

        //                            if (i < tVLeft.Nodes.Count) //启动 4 号任务
        //                            {
        //                                currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                                var t4 = new Task(() =>
        //                                {
        //                                    readImageTask(num + 3,0);
        //                                });
        //                                t4.Start();
        //                                i++;

        //                                if (i < tVLeft.Nodes.Count) // 启动 5 号任务
        //                                {
        //                                    currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                                    var t5 = new Task(() =>
        //                                    {
        //                                        readImageTask(num + 4,0);
        //                                    });
        //                                    t5.Start();
        //                                    i++;

        //                                    if (i < tVLeft.Nodes.Count)  // 启动 6 号任务
        //                                    {
        //                                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                                        var t6 = new Task(() =>
        //                                        {
        //                                            readImageTask(num + 5,0);
        //                                        });
        //                                        t6.Start();
        //                                        i++;

        //                                        if (i < tVLeft.Nodes.Count) // 启动 7 号任务
        //                                        {
        //                                            currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                                            var t7 = new Task(() =>
        //                                            {
        //                                                readImageTask(num + 6,0);
        //                                            });
        //                                            t7.Start();
        //                                            i++;

        //                                            if (i < tVLeft.Nodes.Count) //启动 8 号任务
        //                                            {
        //                                                currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
        //                                                var t8 = new Task(() =>
        //                                                {
        //                                                    readImageTask(num + 7,0);
        //                                                });
        //                                                t8.Start();
        //                                                i++;

        //                                                Task.WaitAll(t1, t2, t3, t4, t5, t6, t7, t8);

        //                                                num += 8;
        //                                                curwatch.Stop();
        //                                                this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 8))).ToString("0.0") + " 秒";

        //                                            }
        //                                            else
        //                                            {
        //                                                Task.WaitAll(t1, t2, t3, t4, t5, t6, t7);

        //                                                num += 7;
        //                                                curwatch.Stop();
        //                                                this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 7))).ToString("0.0") + " 秒";
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            Task.WaitAll(t1, t2, t3, t4, t5, t6);

        //                                            num += 6;
        //                                            curwatch.Stop();
        //                                            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 6))).ToString("0.0") + " 秒";
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        Task.WaitAll(t1, t2, t3, t4, t5);

        //                                        num += 5;
        //                                        curwatch.Stop();
        //                                        this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 5))).ToString("0.0") + " 秒";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Task.WaitAll(t1, t2, t3, t4);

        //                                    num += 4;
        //                                    curwatch.Stop();
        //                                    this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 4))).ToString("0.0") + " 秒";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                Task.WaitAll(t1, t2, t3);

        //                                num += 3;
        //                                curwatch.Stop();
        //                                this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 3))).ToString("0.0") + " 秒";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Task.WaitAll(t1, t2);

        //                            num += 2;
        //                            curwatch.Stop();
        //                            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 2))).ToString("0.0") + " 秒";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Task.WaitAll(t1);

        //                        num += 1;
        //                        curwatch.Stop();
        //                        this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 1))).ToString("0.0") + " 秒";
        //                    }

        //                    this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[i - 1].Name);
        //                    if (this.checkBox1.Checked)
        //                        backThread.ReportProgress(i, total); // 第一个进度，10%
        //                    else
        //                    {
        //                        this.toolStripProgressBar1.Value = i;
        //                        this.pictureBox1.Refresh();
        //                    }

        //                    GC.Collect();
        //                }
        //                #endregion

        //                break;
        //        }
        //    }
        //    watch.Stop();
        //    this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  总用时：" + ((float)(watch.ElapsedMilliseconds / 1000 * 1.0)).ToString("0.0") + " 秒，平均用时： " + ((float)(watch.ElapsedMilliseconds / (1000 * total * 1.0))).ToString("0.0") + " 秒, 照片总数：" + total.ToString();  

        //}
        private void readImagesUseBatch()
        {
            // 按照片处理批次数量 进行照片读取
            //currentOMC.InitReadPicturesParameters(tVLeft.Nodes.Count, (float)-0.45, (float)-0.5);
            int total = tVLeft.Nodes.Count;

            ImageCommon imagecommon = new ImageCommon();
            int cpucore = imagecommon.availCPUCore(tVLeft.Nodes[0].Name);

            if ((cpucore > 1) & (cpucore <= 3))
                cpucore = 2;
            if ((cpucore >= 4) & (cpucore <= 7))
                cpucore = 4;
            if (cpucore >= 8)
                cpucore = 8;

            if (tVLeft.Nodes.Count > 0)
            {
                this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[0].Name);
                this.toolStripStatusLabel1.Text = "";
                if (this.checkBox1.Checked)
                    backThread.ReportProgress(1, total); // 第一个进度，10%
                else
                {
                    this.toolStripProgressBar1.Maximum = tVLeft.Nodes.Count;
                    this.toolStripProgressBar1.Value = 1;
                    this.pictureBox1.Refresh();
                }
            }

            int nper = currentOMC.numInProBatch;
            int k = this.tVLeft.Nodes.Count;
            int nb = 0;
            if (Math.Round((float)k / nper) * nper == k)
            {
                nb = (int)Math.Round((float)k / nper);
            }
            else
                nb = (int)Math.Round((float)k / nper + 0.5);
            // 定义 按批量数量 读取照片时，可以分为 多少 批次 读取

            int start = 0, end = 0;

            var watch = Stopwatch.StartNew();
            watch.Start();
            for (int a = 0; a < nb; a++)
            {
                start = a * nper;
                if (a * nper + nper < total)
                    end = a * nper + nper;
                else
                    end = total;
                readBatch(start, end, cpucore, total); // 每批次读取照片
                currentOMC.SaveTmpData(start, end, a);
                if (a>0)
                  currentOMC.ClearDataArray((a-1)*nper , (a-1)*nper + nper); // 把上一批次读取的 pictureCodeDataArray 数组内容失效
            }
            watch.Stop();
            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  总用时：" + ((float)(watch.ElapsedMilliseconds / 1000 * 1.0)).ToString("0.0") + " 秒，平均用时： " + ((float)(watch.ElapsedMilliseconds / (1000 * total * 1.0))).ToString("0.0") + " 秒, 照片总数：" + total.ToString();

        }
        private void readBatch(int start, int end, int cpucore,int total)
        {
            int num = start;

            switch (cpucore)
            {
                case 1:  // 单核处理器

                    for (int i = start; i < end ; i++)
                    {
                        var curwatch = Stopwatch.StartNew();
                        curwatch.Start();

                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                        var t1 = new Task(() =>
                        {
                            readImageTask(num,start);
                        });
                        t1.Start();

                        Task.WaitAll(t1);
                        num += 1;

                        curwatch.Stop();
                        this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 1))).ToString("0.0") + " 秒";

                        this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[i].Name);
                        if (this.checkBox1.Checked)
                            backThread.ReportProgress(i + 1, total); // 第一个进度，10%
                        else
                        {
                            this.toolStripProgressBar1.Value = i + 1;
                            this.pictureBox1.Refresh();
                        }

                    }
                    //fim1.Destroy();
                    GC.Collect();
                    break;
                case 2:  // 2 核处理器
                    #region
                    for (int i = start; i < end; )
                    {
                        var curwatch = Stopwatch.StartNew();
                        curwatch.Start();

                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                        var t1 = new Task(() =>
                        {
                            readImageTask(num,start);
                        });
                        t1.Start();
                        i++;

                        if (i < end )
                        {
                            currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                            var t2 = new Task(() =>
                            {
                                readImageTask(num + 1,start);
                            });
                            t2.Start();
                            try
                            {
                                Task.WaitAll(t1, t2);
                            }
                            catch
                            {
                                MessageBox.Show("并行计算出错！");
                            }
                            num += 2;
                            curwatch.Stop();
                            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 2))).ToString("0.0") + " 秒";

                            i++;
                        }
                        else
                        {
                            Task.WaitAll(t1);

                            num += 1;
                            curwatch.Stop();
                            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 1))).ToString("0.0") + " 秒";
                        }

                        this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[i - 1].Name);
                        if (this.checkBox1.Checked)
                            backThread.ReportProgress(i, total); // 第一个进度，10%
                        else
                        {
                            this.toolStripProgressBar1.Value = i;
                            this.Refresh();
                        }

                        GC.Collect();
                    }
                    #endregion
                    break;
                case 4:  // 4 核处理器
                    #region
                    for (int i = start; i < end; )
                    {
                        var curwatch = Stopwatch.StartNew();
                        curwatch.Start();

                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                        var t1 = new Task(() =>
                        {
                            readImageTask(num, start);
                        });
                        t1.Start();
                        i++;

                        if (i < end )  // 启动 2 号任务
                        {
                            currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                            var t2 = new Task(() =>
                            {
                                readImageTask(num + 1, start);
                            });
                            t2.Start();
                            i++;

                            if (i < end )  // 启动 3 号任务
                            {
                                currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                                var t3 = new Task(() =>
                                {
                                    readImageTask(num + 2, start);
                                });
                                t3.Start();
                                i++;

                                if (i < end ) //启动 4 号任务
                                {
                                    currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                                    var t4 = new Task(() =>
                                    {
                                        readImageTask(num + 3, start);
                                    });
                                    t4.Start();
                                    i++;

                                    Task.WaitAll(t1, t2, t3, t4);

                                    num += 4;
                                    curwatch.Stop();
                                    this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 4))).ToString("0.0") + " 秒";

                                }
                                else
                                {
                                    Task.WaitAll(t1, t2, t3);

                                    num += 3;
                                    curwatch.Stop();
                                    this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 3))).ToString("0.0") + " 秒";
                                }
                            }
                            else
                            {
                                Task.WaitAll(t1, t2);

                                num += 2;
                                curwatch.Stop();
                                this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 2))).ToString("0.0") + " 秒";
                            }
                        }
                        else
                        {
                            Task.WaitAll(t1);

                            num += 1;
                            curwatch.Stop();
                            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 1))).ToString("0.0") + " 秒";
                        }

                        this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[i - 1].Name);
                        if (this.checkBox1.Checked)
                            backThread.ReportProgress(i, total); // 第一个进度，10%
                        else
                        {
                            this.toolStripProgressBar1.Value = i;
                            this.pictureBox1.Refresh();
                        }

                        GC.Collect();
                    }
                    #endregion
                    break;
                case 8: // 8 核处理器
                    #region
                    for (int i = start; i < end ; )
                    {
                        var curwatch = Stopwatch.StartNew();
                        curwatch.Start();

                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字

                        var t1 = new Task(() =>
                        {
                            readImageTask(num, start);
                        });
                        t1.Start();
                        i++;

                        if (i < end )  // 启动 2 号任务
                        {
                            currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                            var t2 = new Task(() =>
                            {
                                readImageTask(num + 1, start);
                            });
                            t2.Start();
                            i++;

                            if (i < end )  // 启动 3 号任务
                            {
                                currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                                var t3 = new Task(() =>
                                {
                                    readImageTask(num + 2, start);
                                });
                                t3.Start();
                                i++;

                                if (i < end ) //启动 4 号任务
                                {
                                    currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                                    var t4 = new Task(() =>
                                    {
                                        readImageTask(num + 3, start);
                                    });
                                    t4.Start();
                                    i++;

                                    if (i < end ) // 启动 5 号任务
                                    {
                                        currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                                        var t5 = new Task(() =>
                                        {
                                            readImageTask(num + 4, start);
                                        });
                                        t5.Start();
                                        i++;

                                        if (i < end )  // 启动 6 号任务
                                        {
                                            currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                                            var t6 = new Task(() =>
                                            {
                                                readImageTask(num + 5, start);
                                            });
                                            t6.Start();
                                            i++;

                                            if (i < end ) // 启动 7 号任务
                                            {
                                                currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                                                var t7 = new Task(() =>
                                                {
                                                    readImageTask(num + 6, start);
                                                });
                                                t7.Start();
                                                i++;

                                                if (i < end ) //启动 8 号任务
                                                {
                                                    currentOMC.readFileNames[i] = tVLeft.Nodes[i].Name; // 保存读入的文件名字
                                                    var t8 = new Task(() =>
                                                    {
                                                        readImageTask(num + 7, start);
                                                    });
                                                    t8.Start();
                                                    i++;

                                                    Task.WaitAll(t1, t2, t3, t4, t5, t6, t7, t8);

                                                    num += 8;
                                                    curwatch.Stop();
                                                    this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 8))).ToString("0.0") + " 秒";

                                                }
                                                else
                                                {
                                                    Task.WaitAll(t1, t2, t3, t4, t5, t6, t7);

                                                    num += 7;
                                                    curwatch.Stop();
                                                    this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 7))).ToString("0.0") + " 秒";
                                                }
                                            }
                                            else
                                            {
                                                Task.WaitAll(t1, t2, t3, t4, t5, t6);

                                                num += 6;
                                                curwatch.Stop();
                                                this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 6))).ToString("0.0") + " 秒";
                                            }
                                        }
                                        else
                                        {
                                            Task.WaitAll(t1, t2, t3, t4, t5);

                                            num += 5;
                                            curwatch.Stop();
                                            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 5))).ToString("0.0") + " 秒";
                                        }
                                    }
                                    else
                                    {
                                        Task.WaitAll(t1, t2, t3, t4);

                                        num += 4;
                                        curwatch.Stop();
                                        this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 4))).ToString("0.0") + " 秒";
                                    }
                                }
                                else
                                {
                                    Task.WaitAll(t1, t2, t3);

                                    num += 3;
                                    curwatch.Stop();
                                    this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 3))).ToString("0.0") + " 秒";
                                }
                            }
                            else
                            {
                                Task.WaitAll(t1, t2);

                                num += 2;
                                curwatch.Stop();
                                this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 2))).ToString("0.0") + " 秒";
                            }
                        }
                        else
                        {
                            Task.WaitAll(t1);

                            num += 1;
                            curwatch.Stop();
                            this.toolStripStatusLabel1.Text = "多核心运行：" + cpucore.ToString() + "  当前照片用时：" + ((float)(curwatch.ElapsedMilliseconds / (1000 * 1.0 * 1))).ToString("0.0") + " 秒";
                        }

                        this.pictureBox1.Image = Image.FromFile(tVLeft.Nodes[i - 1].Name);
                        if (this.checkBox1.Checked)
                            backThread.ReportProgress(i, total); // 第一个进度，10%
                        else
                        {
                            this.toolStripProgressBar1.Value = i;
                            this.pictureBox1.Refresh();
                        }

                        GC.Collect();
                    }
                    #endregion

                    break;
            }

        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            currentOMC.DeletedFileAllAfterRead();
            currentOMC.InitReadPicturesParameters(this.tVLeft.Nodes.Count); 
            // 按照所选取的照片数量，创建 照片 数组
            if (this.checkBox1.Checked)
                readBackWork(); // 有进度条
            else
                readImagesUseBatch();  // 无进度条
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "";
            if (this.checkBox1.Checked)
                this.toolStripProgressBar1.Visible = false;
            else
                this.toolStripProgressBar1.Visible = true;
        }

        private void ReadPictures_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

    }
}
