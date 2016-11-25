using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HL2D.View2D;
using OpticMeasure.OpticFlowFunc;

namespace OpticMeasure.Function
{
    public partial class imageDsp : UserControl
    {
        public Image2DView viewImage;
        private int _mouseOper = -1;    // Mouse 操作控制， 7 ：新增识别点  9： 识别点提取
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

        private OpticMeasureClass currentOMC;
        private GetOpticflowResultOfSingleReferencePicture singleOpticFlow; // 灰度差计算结果对象
        private int primitivePictureIndex;  // 原始图片编号
        public PamaterSetting pamater = new PamaterSetting();

        public imageDsp()
        {
            InitializeComponent();
        }

        public void Init(OpticMeasureClass _currentOMC, int _primitivePictureIndex)
        {
            currentOMC = _currentOMC;
            primitivePictureIndex = _primitivePictureIndex;
        }

        private void imageDsp_Load(object sender, EventArgs e)
        {
            this.pictureView.Size = this.panelView.Size;

            bool succ;
            succ = pamater.GetDefaultSetting(ref pamater.zbDrawParamater);
            if (succ == false)
                pamater.zbDrawParamater.Init();
            // 如果参数表读取错误，则：取参数的初始值

        }
        private void RefreshPictures()
        {
            //int rpmodel = 2; // 只进行图片叠加 方式显示

            // 首先清除原来所有的图片显示内容
            if (pictureView.Image != null)
                pictureView.Image.Dispose();
            pictureView.Image = null;
            GC.Collect(); // 释放原来显示所占用的资源
            string referenceFilename = currentOMC.ReferencePictureDataArray[0].ReferencePictureLuJin; // 单模板的模板照片文件名
            currentOMC.ViewPicture(this.pictureView, primitivePictureIndex, referenceFilename, currentOMC.PictureOpticflowDataResultArray); // 显示图片
        }

        public void DisplayImage()
        {
            RefreshPictures();
        }

        private void 图像放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // toolStrip 功能 ： 图像放大
            if (viewImage == null)
            {
                viewImage = new Image2DView(this.pictureView, this.panelView, pamater);
            }
            viewImage.viewImage = this.pictureView.Image;
            mouseOper = 1;  // 图像放大
        }

        private void 图像缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewImage == null)
            {
                viewImage = new Image2DView(this.pictureView, this.panelView, pamater);
            }
            viewImage.viewImage = this.pictureView.Image;
            mouseOper = 2;  // 图像缩小

        }

        private void 图像复原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewImage == null)
            {
                viewImage = new Image2DView(this.pictureView, this.panelView, pamater);
            }
            viewImage.viewImage = this.pictureView.Image;
            mouseOper = 0;  // 图像复原
            viewImage.ImageReset();

        }

        private void pictureView_MouseClick(object sender, MouseEventArgs e)
        {
            switch (mouseOper)
            {
                case 1: // 图像放大
                    viewImage.ImageZoom(e.Location, 1100);
                    break;
                case 2: // 图像缩小
                    viewImage.ImageZoom(e.Location, 900);
                    break;
            }
        }

        private void imageDsp_Resize(object sender, EventArgs e)
        {
            this.pictureView.Size = this.panelView.Size;
        }

        private void 图像查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            mouseOper = 0;
        }

        private void nuScale_ValueChanged(object sender, EventArgs e)
        {
            currentOMC.FactorOfGreygrad = (float)nuScale.Value;
            DisplayImage();
        }

        private void chkGrayAdd_CheckedChanged(object sender, EventArgs e)
        {
            currentOMC.graydeltaAdd = this.chkGrayAdd.Checked;
            DisplayImage();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            currentOMC.grayed = this.checkBox1.Checked;
            DisplayImage();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            currentOMC.biaodingDsp = this.checkBox2.Checked;
            DisplayImage();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            currentOMC.forwardAdd = this.checkBox3.Checked; // 正向叠加
            DisplayImage();
        }

    }
}
