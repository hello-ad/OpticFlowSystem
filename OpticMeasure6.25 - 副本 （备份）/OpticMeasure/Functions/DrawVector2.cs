using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZH2DCL.ZtClass;
using HL2D.View2D;
using ZH2DCL._2DClass;
//using ZH2DCL.Setting;
using OpticMeasure;
using OpticMeasure.OpticFlowFunc;
using Microsoft.DirectX;
using ZH2DCL.Setting;

namespace ZHCLNEW.DrawCloud
{
    public partial class DrawVector2 : UserControl
    {
        private GetOpticflowResultOfSingleReferencePicture compVector2;
        private OpticMeasureClass opticflow;
        private DrawFlow drawVector2 = new DrawFlow();
        private Image2DView viewImage;
        private PamaterSetting pamater = new PamaterSetting();
        private OpticMeasureClass currentEdit;
        private ImageCommon imageCommon = new ImageCommon();
        private int _mouseOper = -1;    // Mouse 操作控制， 0 :放大   1：缩小
        public int mouseOper
        {
            get { return _mouseOper; }
            set
            {
                _mouseOper = value;
                Cursor cur;

                switch (_mouseOper)
                {
                    case 1: // 图像放大
                        cur = new Cursor(Application.StartupPath + @"\Cursor\cursor1.cur");
                        this.Cursor = cur;
                        break;
                    case 2: // 图像缩小
                        cur = new Cursor(Application.StartupPath + @"\Cursor\magnify.cur");
                        this.Cursor = cur;
                        break;
                    default: // 其他
                        this.Cursor = Cursors.Default;
                        break;

                }

            }
        }
        private string savePath;
        private Rectangle saveArea = new Rectangle();//
        private bool DragFlag = false;

        public DrawVector2()
        {
            InitializeComponent();
        }
        public void Init(OpticMeasureClass _opmc, GetOpticflowResultOfSingleReferencePicture _singleOpticFlow)
        {
            compVector2 = _singleOpticFlow;
            currentEdit = _opmc;
            pictureVector2.Size = panelVector2.Size;

            drawVector2.CanvasSize = new Point(this.panelVector2.Width, panelVector2.Height);
            bool succ;
            succ = pamater.GetDefaultSetting(ref pamater.zbDrawParamater);
            if (succ == false)
                pamater.zbDrawParamater.Init();
            // 如果参数表读取错误，则：取参数的初始值
            viewImage = new Image2DView(pictureVector2, panelVector2, pamater);

            TreeNode node;
            string filename;
            int ppindex = -1;

            if (currentEdit.readFileNames != null)
            {
                for (int i = 0; i < currentEdit.readFileNames.Length; i++)
                {
                    ppindex = i;//Convert.ToInt32(currentEdit.WaitMatchPictureIndexArray[i].ToString());
                    filename = currentEdit.readFileNames[ppindex]; 
                    node = new TreeNode();
                    node.Name = filename;
                    node.Text = imageCommon.getFileNameOnly(filename);
                    node.Tag = ppindex.ToString();  // 该照片 在数组中的序号
                    filetree.Nodes.Add(node);
                }
                if (filetree.Nodes.Count > 0)
                {
                    filetree.SelectedNode = filetree.Nodes[0];
                }
            }

        }

        private void TransDataToDrawFlow(int pictureindex)
        {
            // 补充代码
            // DensityGenerationFromDisplacements 中指定图片 pictureindex 的位侈数据 传递到 DrawFlow 中
            //compVector2.loadDisplacementToDrawFlowFunc(pictureindex, ref drawVector2);

            drawVector2.ClearFlow();//清空原数据
            drawVector2.LineType = lineTypeEnum.Arrow;//设置绘图模式
            drawVector2.awColor = Color.Blue.ToArgb();//

            Vector2 min = new Vector2();
            Vector2 max = new Vector2();

            //string Filenametmp = @"d:\tmp\" + Index.ToString() + ".txt";
            //StreamWriter sw = File.CreateText(Filenametmp);

            //for (int i = 0; i < compVector2.HomologousPointsDisplacementsArray.Length; i++)
            //{
            //    FlowDataF temp = new FlowDataF();
            //    temp.p0.X = compVector2.HomologousPointsDisplacementsArray[i].pointCoordinate.X;
            //    temp.p0.Y = compVector2.HomologousPointsDisplacementsArray[i].pointCoordinate.Y;
            //    temp.p1.X = temp.p0.X + compVector2.HomologousPointsDisplacementsArray[i].deltaX;
            //    temp.p1.Y = temp.p0.Y + compVector2.HomologousPointsDisplacementsArray[i].deltaY;
            //    drawVector2.AddFlow(temp);

            //    if (min.X > temp.p0.X) min.X = temp.p0.X;
            //    if (min.Y > temp.p0.Y) min.Y = temp.p0.Y;
            //    if (max.X < temp.p0.X) max.X = temp.p0.X;
            //    if (max.Y < temp.p0.Y) max.Y = temp.p0.Y;
            //}
            //===修改 6、27=====
            if (pictureindex > 0)
            {
                FlowDataF temp = new FlowDataF();
                for (int i = 0; i < opticflow.PictureOpticflowDataResultArray[pictureindex].OpticflowResultFOfPointsArray.Length; i++)
                {
                    if (i==7459)
                    {
                        temp.p0.X = opticflow.PictureOpticflowDataResultArray[pictureindex].OpticflowResultFOfPointsArray[i].pointCoordinate.X;
                        temp.p0.Y = opticflow.PictureOpticflowDataResultArray[pictureindex].OpticflowResultFOfPointsArray[i].pointCoordinate.Y;

                    }
                   
                    temp.p0.X = opticflow.PictureOpticflowDataResultArray[pictureindex].OpticflowResultFOfPointsArray[i].pointCoordinate.X;
                    temp.p0.Y = opticflow.PictureOpticflowDataResultArray[pictureindex].OpticflowResultFOfPointsArray[i].pointCoordinate.Y;


                    temp.p1.X = temp.p0.X + opticflow.PictureOpticflowDataResultArray[pictureindex].OpticflowResultFOfPointsArray[i].Grad.X;
                    temp.p1.Y = temp.p0.Y + opticflow.PictureOpticflowDataResultArray[pictureindex].OpticflowResultFOfPointsArray[i].Grad.Y;

                    drawVector2.AddFlow(temp);

                    if (min.X > temp.p0.X) min.X = temp.p0.X;
                    if (min.Y > temp.p0.Y) min.Y = temp.p0.Y;
                    if (max.X < temp.p0.X) max.X = temp.p0.X;
                    if (max.Y < temp.p0.Y) max.Y = temp.p0.Y;

                }
                //=======
                drawVector2.MinPoint = new PointF(min.X, min.Y);
                drawVector2.MaxPoint = new PointF(max.X, max.Y);
            }



        }
        private void filetree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int ppindex = -1;
            if (filetree.SelectedNode != null)
            {
                // 当前 已选择了 某一张 匹配图片
                ppindex = filetree.SelectedNode.Index;//Convert.ToInt32(filetree.SelectedNode.Tag.ToString());
                //compVector2.currentIndex = ppindex;

                TransDataToDrawFlow(ppindex);
                drawVector2.zbDrawPara =pamater;

                if ((pamater.zbDrawParamater.Xenable == true) | (pamater.zbDrawParamater.Yenable == true))
                    drawVector2.zbDisplay = true;
                else
                    drawVector2.zbDisplay = false;

                drawVector2.DrawFlowGraphicsF(this.pictureVector2);
                viewImage.viewImage = pictureVector2.Image;
                viewImage.OrgImagePath = Application.StartupPath + @"\kk.jpg";
                this.pictureVector2.Image.Save(Application.StartupPath + @"\kk.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                e.Node.BackColor = Color.Blue;
                this.txtCurFile.Text = e.Node.Text;

                pictureVector2.Size = panelVector2.Size;
                pictureVector2.Top = 0;
                pictureVector2.Left = 0;

                pictureVector2.Refresh();

                if (chkSave.Checked)
                {
                    // 保存结果图片
                    string filename2 = savePath + @"\v2" + filetree.SelectedNode.Text;
                    int ys = SystemInformation.MenuHeight * 2 + SystemInformation.CaptionHeight;
                    int xs = this.groupBox1.Width;

                    Bitmap bmp ;
                    Graphics g;
                    if (chkSaveArea.Checked == false)
                    {
                        bmp = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width,Screen.PrimaryScreen.WorkingArea.Height);
                        g = Graphics.FromImage(bmp);

                        g.CopyFromScreen(0, 0, 0, 0, new Size(bmp.Width, bmp.Height));
                        bmp.Save(filename2, System.Drawing.Imaging.ImageFormat.Jpeg);
                        bmp.Dispose();
                    }
                    else
                    {
                        Point x = new Point(saveArea.X, saveArea.Y);
                        Point y = PointToScreen(x);
                        //MessageBox.Show(y.X.ToString() + " Y = " + y.Y.ToString());

                        bmp = new Bitmap(saveArea.Width,saveArea.Height);
                        g = Graphics.FromImage(bmp);
                        g.CopyFromScreen(y.X + xs, y.Y + ys, 0, 0, new Size(bmp.Width, bmp.Height));
                        //g.CopyFromScreen(saveArea.X + xs, saveArea.Y + ys, 0, 0, new Size(bmp.Width, bmp.Height));
                        bmp.Save(filename2, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }

            }
        }

        private void zuobiaoLabel1_Click(object sender, EventArgs e)
        {
            SetPara param = new SetPara(pamater);
            param.ShowDialog();
            drawVector2.zbDrawPara = pamater;
            if ((pamater.zbDrawParamater.Xenable == true) | (pamater.zbDrawParamater.Yenable == true))
                drawVector2.zbDisplay = true;
            else
                drawVector2.zbDisplay = false;
            drawVector2.DrawFlowGraphicsF(this.pictureVector2);
            viewImage.viewImage = pictureVector2.Image;
            viewImage.OrgImagePath = Application.StartupPath + @"\kk.jpg";
            this.pictureVector2.Image.Save(Application.StartupPath + @"\kk.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            
            pictureVector2.Refresh();
        }

        private void pictureVector2_MouseClick(object sender, MouseEventArgs e)
        {
            switch (mouseOper)
            {
                case 1 : // 图像放大
                    viewImage.ImageZoom(e.Location, 1100);
                    break;
                case 2 : // 图像缩小
                    viewImage.ImageZoom(e.Location, 900);
                    break;
            }

        }
        private void 图像放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewImage == null)
            {
                viewImage = new Image2DView(pictureVector2, panelVector2, pamater);
            }
            viewImage.viewImage = this.pictureVector2.Image;
            mouseOper = 1;  // 图像放大
        }

        private void 图像缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewImage == null)
            {
                viewImage = new Image2DView(pictureVector2, panelVector2, pamater);
            }
            viewImage.viewImage = this.pictureVector2.Image;
            mouseOper = 2;  // 图像放大

        }

        private void 图像复原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewImage == null)
            {
                viewImage = new Image2DView(pictureVector2, panelVector2, pamater);
            }
            viewImage.viewImage = this.pictureVector2.Image;
            viewImage.ImageReset();
            mouseOper = 0;

        }

        private void 图像查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mouseOper = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int index ;
            bool stop = false;

            if (filetree.Nodes.Count >= 1)
            {
                if (filetree.SelectedNode == null)
                    index = 0;
                else
                    index = filetree.SelectedNode.Index;

                if (this.radioButton1.Checked == true) // 一次循环
                {
                    if (index + 1 < filetree.Nodes.Count)
                        index++;
                    else
                        stop = true;
                }
                else  // 多次循环
                {
                    if (index + 1 < filetree.Nodes.Count)
                        index++;
                    else
                        index = 0;
                }
                if (stop == true)
                {
                    timer1.Stop();
                    this.panel2.Visible = false;
                    this.filetree.Visible = true;
                    button1.Text = "动画显示";
                }
                else
                    filetree.SelectedNode = filetree.Nodes[index];
                this.Refresh();
            }
            else
            {
                timer1.Stop();
                this.panel2.Visible = false;
                this.filetree.Visible = true;
                button1.Text = "动画显示";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "动画显示")
            {
                timer1.Start();
                this.panel2.Visible = true;
                this.filetree.Visible = false;
                button1.Text = "动画停止";
            }
            else
            {
                timer1.Stop();
                this.panel2.Visible = false;
                this.filetree.Visible = true;
                button1.Text = "动画显示";
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(this.numericUpDown1.Value);
            timer1.Interval = (int)(1000 / n);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filetree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.filetree.SelectedNode != null)
                this.filetree.SelectedNode.BackColor = filetree.BackColor;
        }

        private void DrawVector2_Load(object sender, EventArgs e)
        {
            pictureVector2.Size = panelVector2.Size;
        }

        private void pictureVector2_MouseDown(object sender, MouseEventArgs e)
        {


            if ((e.Button == MouseButtons.Left) & (this.chkSaveArea.Checked == true))
            {
                // 进行 多点提取 操作
                DragFlag = true; // 正进行 Mouse 移动操作
                saveArea.X = e.X;
                saveArea.Y = e.Y;

            }

        }

        private void pictureVector2_MouseMove(object sender, MouseEventArgs e)
        {
            if ((chkSaveArea.Checked == true) & (DragFlag == true))
            {
                int x = e.X;
                int y = e.Y;
                saveArea.Width = x - saveArea.X;
                saveArea.Height = y - saveArea.Y;

                pictureVector2.Refresh();
                Graphics g = this.pictureVector2.CreateGraphics(); // Graphics.FromImage(pBoxControl.Image);
                Pen pen = new Pen(Color.White, 1);

                g.DrawRectangle(pen, saveArea.X, saveArea.Y, saveArea.Width, saveArea.Height);
                g.Dispose();
            }

        }

        private void pictureVector2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (DragFlag == true)
                    DragFlag = false;
                pictureVector2.Refresh();
                //MessageBox.Show( saveArea.X.ToString() + "y " + saveArea.Y.ToString() + "w " + saveArea.Width.ToString() + "h " + saveArea.Height.ToString());
            }
        }

        private void chkSave_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSave.Checked == true)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    savePath = fbd.SelectedPath;
                }
                else
                    chkSave.Checked = false;

            }
            chkSaveArea.Enabled = chkSave.Checked;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            // 修改正 箭头的大小比例
            drawVector2.arrowScale = Convert.ToSingle(this.numericUpDown2.Value);
            drawVector2.DrawFlowGraphicsF(this.pictureVector2);

        }

        private void numericUpDown2_Leave(object sender, EventArgs e)
        {
            pictureVector2.Refresh();
        }

       
    }
}
