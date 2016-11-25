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
            opnDlg.Filter = "����ͼ���ļ�|*.bmp;*.pcx;*.png;*.jpg;*.gif;" + "*.tif;*.ico;*.dxf;*.cgm;*.cdr;*.wmf;*.eps;*.emf|" + "λͼ��*.bmp;*.jpg;*.png;........)|*.bmp;*.pcx;*.png;*.jpg;*.gif;*.tif;*ico|" + "ʸ��ͼ��*.wmf;*.eps;*.emf;....)|*.dxf;*.cgm;*.cdr;*.wmf;*.eps;*.emf";
            //���öԻ������
            opnDlg.Title = "��ͼ���ļ�";
            opnDlg.Multiselect = true;
            //���á���������ť
            opnDlg.ShowHelp = true;
            TreeNode node = null;

            if (opnDlg.ShowDialog() == DialogResult.OK)
            {
                // ѡ�����ļ���
                string[] files = opnDlg.FileNames; // ѡ���ͼ���ļ����ļ��������ļ�·��
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
            this.textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Ĭ�� Ŀ��Ŀ¼Ϊ ����
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                // ��ѡ��Ŀ���ļ���
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
            } // ���ָ���� ·�� �����ڣ��򴴽���·��


            for (int i = 0; i < tvSource.Nodes.Count; i++)
            {
                // ��һͼƬ����ת����ת�����һ��ͼƬʱ����֮�洢�� 
                sfilename = tvSource.Nodes[i].Name; // ԴͼƬ�ļ���
                tfilename = this.textBox1.Text + "\\" + getFileNameNonExtend(tvSource.Nodes[i].Text) + ".JPG";
                if (sfilename != tfilename)
                    TransTo24JpgAndSave(sfilename, tfilename);
                else
                    MessageBox.Show("ת��Ŀ���ļ� " + tfilename + " �Ѵ��ڣ�" );
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
            // filename : ԴͼƬ�ļ���
            // targetFilename : Ŀ��ͼƬ�ļ���

            Bitmap curBitmap = (Bitmap)Image.FromFile(filename);
            int PictureFormat = 0;
            String curbitmapFormat = curBitmap.PixelFormat.ToString();
            PictureFormat = JugePictureFormat(curbitmapFormat);//��ȡ�ַ����ķ�ʽʶ��ͼ��

            if (PictureFormat == 24)
            {
                // �Ѿ��� 24 λ �� JPG ͼ��
                curBitmap.Save(targetFilename, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //�µ���Ҫ��24λͼ������ 
                Bitmap bit = new Bitmap(curBitmap.Width, curBitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                //�ɶ�д�ķ�ʽ����ȫ������ 
                System.Drawing.Imaging.BitmapData data2 = bit.LockBits(new Rectangle(0, 0, bit.Width, bit.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bit.PixelFormat);
                //�õ��׵�ַ 
                IntPtr ptr2 = data2.Scan0;
                //����24λͼ����ֽ��� 
                int bytes = curBitmap.Width * curBitmap.Height * 3;

                //����λͼ���� 
                byte[] grayValues = new byte[bytes];

                //���Ʊ�������ͼ�񵽸����� 
                System.Runtime.InteropServices.Marshal.Copy(ptr2, grayValues, 0, bytes);

                //�ɶ�д�ķ�ʽ������ǰȫ��8λԭͼ�� 
                System.Drawing.Imaging.BitmapData data = curBitmap.LockBits(new Rectangle(0, 0, curBitmap.Width, curBitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, curBitmap.PixelFormat);

                //�õ��׵�ַ 
                IntPtr ptr1 = data.Scan0;

                //ͬ�� 
                int byte8 = curBitmap.Width * curBitmap.Height;
                byte[] gray8Values = new byte[byte8]; System.Runtime.InteropServices.Marshal.Copy(ptr1, gray8Values, 0, byte8);
                for (int i = 0, n = 0; i < bytes; i += 3, n++)
                {

                    //�Ҷȱ任 
                    double colorTemp = gray8Values[n];

                    //����ǲ����õĿ��Ժ��� ����ԭͼ24λ 

                    grayValues[i + 2] = grayValues[i + 1] = grayValues[i] = (byte)colorTemp;

                } //�ͻ����� 
                System.Runtime.InteropServices.Marshal.Copy(gray8Values, 0, ptr1, byte8);
                System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, ptr2, bytes);

                //���� 
                curBitmap.UnlockBits(data); bit.UnlockBits(data2);

                bit.Save(targetFilename, System.Drawing.Imaging.ImageFormat.Jpeg); // ���� 24 λ ��JpegͼƬ
                bit.Dispose();

            }
            curBitmap.Dispose(); // �ͷ���Դ

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
            String string1gray = "Format1bppIndexed";//1λ�Ҷ�ͼ
            String string4gray = "Format4bppIndexed";//4λ�Ҷ�ͼ
            String string8gray = "Format8bppIndexed";//8λ�Ҷ�ͼ
            String string16gray = "Format16bppGrayScale";//16λ�Ҷ�ͼ
            String string24RGB = "Format24bppRgb";//24λRGBͼ
            String string32RGB = "Format32bppArgb";//32λRGBͼ
            String string64RGB = "Format64bppArgb";//64λRGBͼ

            if (curbitmapFormat.CompareTo(string1gray) == 0)//�Ƚ��Ƿ���1λ�Ҷ�ͼ
            {
                Temp = 1;//1λ�Ҷ�ͼ
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string4gray) == 0)//�Ƚ��Ƿ���4λ�Ҷ�ͼ
            {
                Temp = 4;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string8gray) == 0)//�Ƚ��Ƿ���8λ�Ҷ�ͼ
            {
                Temp = 8;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string16gray) == 0)//�Ƚ��Ƿ���16λ�Ҷ�ͼ
            {
                Temp = 16;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string24RGB) == 0)//�Ƚ��Ƿ���24λRGBͼ
            {
                Temp = 24;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string32RGB) == 0) //�Ƚ��Ƿ���32λRGBͼ
            {
                Temp = 32;
                return Temp;
            }
            else if (curbitmapFormat.CompareTo(string64RGB) == 0)  //�Ƚ��Ƿ���64λRGBͼ     
            {
                Temp = 64;
                return Temp;
            }
            else
            {
                MessageBox.Show("δ֪��ʽ����Ƭ");
                Temp = 0;
            }
            return Temp;
        }


    }
}