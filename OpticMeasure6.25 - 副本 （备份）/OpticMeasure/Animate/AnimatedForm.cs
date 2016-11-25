using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LanMsg.Gif.Components;
using AForge.Video.VFW;
using System.IO;
//using AForge.Video.FFMPEG;

namespace View3D.Animate
{
    public partial class AnimatedForm : Form
    {
        private string gifTarget = "";

        public AnimatedForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            selectpictures();
        }
        public void selectpictures()
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
            }
            opnDlg.Dispose();
            //this.toolStripProgressBar1.Maximum = this.tVLeft.Nodes.Count;
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

        private void jpgToGif(String[] pic, String newPic,int delayTime)
        {
            Image im = null;
            try
            {
                AnimatedGifEncoder e = new AnimatedGifEncoder();  //网上可以找到此类
                if (this.checkBox1.Checked == true)
                    e.SetRepeat(0);
                else
                    e.SetRepeat(-1);  // -1 : 不循环， 0：循环

                e.Start(newPic);
                progressBar1.Maximum = pic.Length;

                for (int i = 0; i < pic.Length; i++)
                {
                    e.SetDelay(delayTime);
                    im = Image.FromFile(pic[i]);
                    e.AddFrame(im);
                    curFile.Text = i.ToString() + ">>> " + getFileNameOnly(pic[i]);

                    im.Dispose();
                    progressBar1.Value += 1;

                    panel2.Refresh();
                }
                e.Finish();
                MessageBox.Show("GIF 动画文件转换完毕！");
            }
            catch (Exception e)
            {
                MessageBox.Show("jpgToGif Failed:");
                //e.printStackTrace();
            }

        }
        //private void jpgToAVIByFFpeg(String[] pic, String newPic, int delayTime)
        //{
        //    int frameRate = 25;

        //    if (delayTime > 0)
        //        frameRate = Convert.ToInt32(1000f / delayTime);

        //    VideoFileWriter writer = new VideoFileWriter();


        //    Image im;
        //    if (tVLeft.Nodes.Count > 0)
        //    {
        //        im = Image.FromFile(tVLeft.Nodes[0].Name.ToString());
        //        int w = im.Width;
        //        int h = im.Height;
        //        progressBar1.Maximum = tVLeft.Nodes.Count;
        //        if (w % 2 != 0)
        //            w += 1;
        //        if (h % 2 != 0)
        //            h += 1;

        //        writer.Open(newPic, w, h, frameRate, VideoCodec.MPEG4);

        //        for (int i = 0; i < tVLeft.Nodes.Count; i++)
        //        {
        //            im.Dispose();
        //            im = Image.FromFile(tVLeft.Nodes[i].Name.ToString());
        //            im = BitmapToBlowUp(im, w, h);

        //            this.pictureBox1.Image = im;
        //            this.pictureBox1.Refresh();
        //            if ((im.Height != h) | (im.Width != w))
        //                MessageBox.Show(tVLeft.Nodes[i].Name.ToString() + " 文件大小与首张不符！");
        //            writer.WriteVideoFrame((Bitmap)im);

        //            progressBar1.Value += 1;

        //            curFile.Text = i.ToString() + ">>> " + getFileNameOnly(pic[i]);
        //            curFile.Refresh();
        //        }
        //        writer.Close();
        //        writer.Dispose();

        //        MessageBox.Show("AVI 动画文件转换完毕！");
        //    }
        //} // FFPEG 类生成AVI，使用 MPG4 格式压缩
        // 如果使用 FFPEG 类，则需要： 
        // 1、引用如下类库：
        //  AForge.Video.FFMPEG
        // 2、将如下类库 Copy 到目标程度所在文件夹，如 bin\debug 等。
        // AForge.Net FrameWork \Externals\FFMPEG\BIN文件夹下的所有 DLL 文件
        // avcodec-53.dll,avdevice-53.dll,avfilter-2.dll,avformat-53.dll,avutil-51.dll,postproc-52.dll,swresample-0.dll,swscale-2.dll
        private void jpgToAVI(String[] pic, String newPic, int delayTime)
        {
            int frameRate = 25;

            if (delayTime > 0)
                frameRate = Convert.ToInt32(1000f / delayTime);

            AVIWriter aviWriter = new AVIWriter("wmv3");
            aviWriter.FrameRate = frameRate;

            Image im;
            if (tVLeft.Nodes.Count > 0)
            {
                im = Image.FromFile(tVLeft.Nodes[0].Name.ToString());
                int w = im.Width;
                int h = im.Height;
                progressBar1.Maximum = tVLeft.Nodes.Count;

                if (w % 2 != 0)
                    w += 1;
                if (h % 2 != 0)
                    h += 1;

                aviWriter.Open(newPic, w, h);  // 输出到指定的文件

                for (int i = 0; i < tVLeft.Nodes.Count; i++)
                {
                    im.Dispose();
                    im = Image.FromFile(tVLeft.Nodes[i].Name.ToString());
                    im = BitmapToBlowUp(im, w, h);

                    this.pictureBox1.Image = im;
                    this.pictureBox1.Refresh();
                    if ((im.Height != h) | (im.Width != w))
                        MessageBox.Show(tVLeft.Nodes[i].Name.ToString() + " 文件大小与首张不符！");
                    aviWriter.AddFrame((Bitmap)im);
                    progressBar1.Value += 1;

                    curFile.Text = i.ToString() + ">>> " + getFileNameOnly(pic[i]);
                    curFile.Refresh();
                }
                aviWriter.Close();
                aviWriter.Dispose();
                MessageBox.Show("AVI 动画文件转换完毕！");
            }
        } // WMV3 格式压缩

        private Image BitmapToBlowUp(Image p_Bitmap, int p_Width, int p_Height)//, bool p_ZoomType)
        {
            Bitmap _ZoomBitmap = new Bitmap(p_Width, p_Height);

            Graphics _Graphics = Graphics.FromImage(_ZoomBitmap);

            //if (!p_ZoomType)
            {
                _Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                _Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            }

            _Graphics.DrawImage(p_Bitmap, 0, 0, _ZoomBitmap.Width, _ZoomBitmap.Height);

            _Graphics.Dispose();
            return _ZoomBitmap;
        } // 对图像按指定的大小进行缩放

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // 生成 GIF 动画
            curFile.Text = "";
            progressBar1.Value = 0;

            if (gifTarget != "")
            {
                string[] filenames = new string[this.tVLeft.Nodes.Count];
                for (int i = 0; i < tVLeft.Nodes.Count; i++)
                {
                    filenames[i] = tVLeft.Nodes[i].Name;
                }
                int delaytime = (int)this.numericUpDown1.Value;

                if (raAVI.Checked == false)
                    jpgToGif(filenames, gifTarget, delaytime);
                else
                    jpgToAVI(filenames, gifTarget, delaytime);

            }
            else
                MessageBox.Show("目标动画文件名为空，请指定目标文件名！");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // 选择生成的 GIF 目标文件
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;

            if (raAVI.Checked == true)
            {
                saveFileDialog.Filter = "AVI文件|*.*|AVI(.AVI)|*.AVI|所有文件|*.*";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.Title = "AVI文件另存为";
            }
            else
            {
                saveFileDialog.Filter = "GIF文件|*.*|GIF(.GIF)|*.GIF|所有文件|*.*";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.Title = "GIF文件另存为";
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                gifTarget = saveFileDialog.FileName;
                if (File.Exists(gifTarget))  // 如果办理出文件已存在，则事先删除
                    File.Delete(gifTarget);
            }
            else
                gifTarget = "";
            saveFileDialog.Dispose();
            this.textBox1.Text = gifTarget;
        }

        private void raGIF_CheckedChanged(object sender, EventArgs e)
        {
            if (raGIF.Checked == true)
            {
                this.panel4.Enabled = true;

                if (gifTarget != "")
                    gifTarget = getFileNameNonExtend(gifTarget) + ".GIF";

                this.textBox1.Text = gifTarget;
            }
        }

        private void raAVI_CheckedChanged(object sender, EventArgs e)
        {
            if (raAVI.Checked == true)
            {
                this.panel4.Enabled = false;

                if (gifTarget != "")
                    gifTarget = getFileNameNonExtend(gifTarget) + ".AVI";

                this.textBox1.Text = gifTarget;
            }
        }
    }
}
