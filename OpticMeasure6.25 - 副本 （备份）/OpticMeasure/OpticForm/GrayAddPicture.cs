using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpticMeasure.Function;
using OpticMeasure.OpticFlowFunc;

namespace OpticMeasure.OpticForm
{
    public partial class GrayAddPicture : Form
    {
        private imageDsp imd;
        private OpticMeasureClass opmc;
        private GetOpticflowResultOfSingleReferencePicture singleOpticFlow;
        private int curStart;
        private int FileIndexSelected = -1;

        public GrayAddPicture(OpticMeasureClass _opmc)
        {
            InitializeComponent();
            opmc = _opmc;
            
        }

        private void EditPicture_Load(object sender, EventArgs e)
        {
            imd = new imageDsp();
            imd.Dock = DockStyle.Fill;
            this.panelView.Controls.Add(imd);

            if (opmc.readFileNames.Length >= opmc.numInProBatch)
                refreshFiletree(0, opmc.numInProBatch);
            else
                refreshFiletree(0, opmc.readFileNames.Length);

            //bool succ;
            //succ = pamater.GetDefaultSetting(ref pamater.zbDrawParamater);
            //if (succ == false)
            //    pamater.zbDrawParamater.Init();
            // 如果参数表读取错误，则：取参数的初始值

        }
        private void refreshFiletree(int start, int end)
        {
            string filename; // 读入有效图片文件，且正常进行了灰度化处理
            TreeNode node;
             curStart = start;

            filetree.Nodes.Clear();
            if (start != 0)
            {
                node = new TreeNode();
                node.Text = "查看上一组";
                node.Name = (start - opmc.numInProBatch).ToString();
                node.Tag = "-1";
                filetree.Nodes.Add(node);
            }
            for (int i = start; i < end; i++)
            {
                // 逐个文件建立 图片文件 树，以便于进行查看
                filename = opmc.readFileNames[i]; // .readFileNames[i]; // 当前文件名
                node = new TreeNode();
                node.Text = getFileNameOnly(filename);
                node.Name = filename;
                //node.Tag = (i-start).ToString();  // 该照片 在数组中的序号
                node.Tag = i.ToString();  // 该照片 在数组中的序号
                filetree.Nodes.Add(node);
                // 由于 文件树 节点与 readFileNames 数组一一对应，故： Node.Index 也可以视作 该照片 在 readFileNames 中的序号
            } // 建立照片树

            // 从临时文件中刷新数据，将新的一组数据写入到 picturecodedataArray数组中
            int batNo = Convert.ToInt32(Math.Round(start * 1.0 / opmc.numInProBatch));
            opmc.RecoveryTmpData(start, end, batNo, 0);

            if (end != opmc.readFileNames.Length)
            {
                node = new TreeNode();
                node.Text = "查看下一组";
                node.Name = end.ToString();
                node.Tag = "-9";
                filetree.Nodes.Add(node);
            }

            if (filetree.Nodes.Count > 0)
                if (start == 0)
                    filetree.SelectedNode = filetree.Nodes[0];
                else
                    filetree.SelectedNode = filetree.Nodes[1];

            // 默认情况下，第一张照片被选中
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
        private void RefreshPictures()
        {
            int index = Convert.ToInt32(filetree.SelectedNode.Tag.ToString());
            
        }
        private void filetree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (filetree.SelectedNode != null)
            {
                int index = Convert.ToInt32(filetree.SelectedNode.Tag);
                int n = Convert.ToInt32(filetree.SelectedNode.Tag);
                if ((index != FileIndexSelected) & (n >= 0))
                {
                    // 选择的照片不是原来正在显示的照片

                    RefreshPictures();

                    groupboxView.Text = "显示区 ：" + filetree.SelectedNode.Text;
                    //e.Node.BackColor = Color.Blue;

                    FileIndexSelected = index;
                    opmc.currentPictureIndex = index;

                    imd.Init(opmc, index);
                    imd.DisplayImage();

                    //if (this.button2.Text == "像点收缩")
                    //{
                    //    CreateCodeTree(index, codePP); // 重建该照片所对应的 编码点/非编码点 等图像识别信息
                    //}
                }
                if (n == -1)
                {
                    // 当前选择为 上一组
                    int s = Convert.ToInt32(this.filetree.SelectedNode.Name.ToString());
                    refreshFiletree(s, s + opmc.numInProBatch);
                }
                if (n == -9)
                {
                    // 当前选择为 下一组
                    int si = Convert.ToInt32(this.filetree.SelectedNode.Name.ToString());
                    if (si + opmc.numInProBatch >= opmc.readFileNames.Length)
                        refreshFiletree(si, opmc.readFileNames.Length);
                    else
                        refreshFiletree(si, si + opmc.numInProBatch);
                }
            }

        }

    }
}
