using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpticMeasure
{
    public partial class PictureTo24bitJpg : Form
    {
        public PictureTo24bitJpg()
        {
            InitializeComponent();
        }

        private void PictureTo24bitJpg_Resize(object sender, EventArgs e)
        {
            this.textBox1.Width = this.panel6.Width - 10;
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
        private void button1_Click(object sender, EventArgs e)
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
                    tvSource.Nodes.Add(node);
                }
            }

            opnDlg.Dispose();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tvSource.Nodes.Clear();
            tvTarget.Nodes.Clear();
        }

        private void PictureTo24bitJpg_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // 默认 目标目录为 桌面
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                // 已选择目标文件夹
                this.textBox1.Text = folderBrowserDialog1.SelectedPath;
                this.button2.Enabled = true;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            button1_Click(sender, new EventArgs());
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            button4_Click(sender, new EventArgs());
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            button3_Click(sender, new EventArgs());
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            button2_Click(sender, new EventArgs());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sfilename, tfilename;
            TreeNode node = null;

            string path = this.textBox1.Text;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            } // 如果指定的 路径 不存在，则创建该路径


            for (int i = 0; i < tvSource.Nodes.Count; i++)
            {
                // 逐一图片进行转换，转换完成一个图片时，将之存储入 
                sfilename = tvSource.Nodes[i].Name; // 源图片文件名
                tfilename = this.textBox1.Text + "\\" + getFileNameNonExtend(tvSource.Nodes[i].Text) + ".JPG";
                if (sfilename != tfilename)
                    TransTo24JpgAndSave(sfilename, tfilename);
                else
                    MessageBox.Show("转换目标文件 " + tfilename + " 已存在！" );
                node = new TreeNode();
                node.Text = getFileNameNonExtend(tvSource.Nodes[i].Text) + ".JPG";
                node.Name = tfilename;
                tvTarget.Nodes.Add(node);
                tvTarget.Refresh();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TransTo24JpgAndSave(string filename,string targetFilename)
        {  
            // filename : 源图片文件名
            // targetFilename : 目标图片文件名

            Bitmap curBitmap = (Bitmap)Image.FromFile(filename);
            int PictureFormat = 0;
            String curbitmapFormat = curBitmap.PixelFormat.ToString();
            PictureFormat = JugePictureFormat(curbitmapFormat);//读取字符串的方式识别图像

            if (PictureFormat == 24)
            {
                // 已经是 24 位 的 JPG 图像
                curBitmap.Save(targetFilename, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //新的我要的24位图像容器 
                Bitmap bit = new Bitmap(curBitmap.Width, curBitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                //可读写的方式锁定全部容器 
                System.Drawing.Imaging.BitmapData data2 = bit.LockBits(new Rectangle(0, 0, bit.Width, bit.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bit.PixelFormat);
                //得到首地址 
                IntPtr ptr2 = data2.Scan0;
                //计算24位图像的字节数 
                int bytes = curBitmap.Width * curBitmap.Height * 3;

                //定义位图数组 
                byte[] grayValues = new byte[bytes];

                //复制被锁定的图像到该数组 
                System.Runtime.InteropServices.Marshal.Copy(ptr2, grayValues, 0, bytes);

                //可读写的方式锁定当前全部8位原图像 
                System.Drawing.Imaging.BitmapData data = curBitmap.LockBits(new Rectangle(0, 0, curBitmap.Width, curBitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, curBitmap.PixelFormat);

                //得到首地址 
                IntPtr ptr1 = data.Scan0;

                //同上 
                int byte8 = curBitmap.Width * curBitmap.Height;
                byte[] gray8Values = new byte[byte8]; System.Runtime.InteropServices.Marshal.Copy(ptr1, gray8Values, 0, byte8);
                for (int i = 0, n = 0; i < bytes; i += 3, n++)
                {

                    //灰度变换 
                    double colorTemp = gray8Values[n];

                    //这个是测试用的可以忽略 这是原图24位 

                    grayValues[i + 2] = grayValues[i + 1] = grayValues[i] = (byte)colorTemp;

                } //送回数组 
                System.Runtime.InteropServices.Marshal.Copy(gray8Values, 0, ptr1, byte8);
                System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, ptr2, bytes);

                //解锁 
                curBitmap.UnlockBits(data); bit.UnlockBits(data2);

                bit.Save(targetFilename, System.Drawing.Imaging.ImageFormat.Jpeg); // 保存 24 位 的Jpeg图片
                bit.Dispose();

            }
            curBitmap.Dispose(); // 释放资源

        } // 2011.1.5 He


        public byte JugePictureFormat(int PictureFormatHashCode)
        {
            byte Temp = 0;
            if (PictureFormatHashCode == 137224)
            {
                Temp = 24;
            }
            if (PictureFormatHashCode == 198659)
            {
                Temp = 8;
            }
            return Temp;
        }
        public byte JugePictureFormat(string curbitmapFormat)
        {
            byte Temp = 0;
            String string1gray = "Format1bppIndexed";//1位灰度图
            String string4gray = "Format4bppIndexed";//4位灰度图
            String string8gray = "Format8bppIndexed";//8位灰度图
            String string16gray = "Format16bppGrayScale";//16位灰度图
            String string24RGB = "Format24bppRgb";//24位RGB图
            String string32RGB = "Format32bppArgb";//32位RGB图
            String string64RGB = "Format64bppArgb";//64位RGB图

            if (curbitmapFormat.CompareTo(string1gray) == 0)//比较是否是1位灰度图
            {
                Temp = 1;//1位灰度图
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string4gray) == 0)//比较是否是4位灰度图
            {
                Temp = 4;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string8gray) == 0)//比较是否是8位灰度图
            {
                Temp = 8;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string16gray) == 0)//比较是否是16位灰度图
            {
                Temp = 16;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string24RGB) == 0)//比较是否是24位RGB图
            {
                Temp = 24;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string32RGB) == 0) //比较是否是32位RGB图
            {
                Temp = 32;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string64RGB) == 0)  //比较是否是64位RGB图     
            {
                Temp = 64;
                return Temp;
            }
            else
            {
                MessageBox.Show("未知格式的照片");
                Temp = 0;
            }
            return Temp;
        }


    }
}