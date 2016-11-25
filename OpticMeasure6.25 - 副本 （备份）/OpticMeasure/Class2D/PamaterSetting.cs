using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace HL2D.View2D
{
    public struct ZBpara
    {
        //public bool zbdisplayX; //= false; ��������ʾ
        //public bool zbdisplayY;
        public Point zbStart; //= new Point(0, 0); ���������
        public Rectangle zbBound; //= new Rectangle(0, 0, 1, 1); ͼ���Χ��
        public int zbkdXMain; // = 100; X����̶�
        public int zbkdYMain; //= 100;  Y����̶�
        public int zbkdXAux;   // X ������̶�
        public int zbkdYAux;   // Y ������̶�
        public bool Xenable; //= true; X������ʾ
        public bool Yenable; //= true;Y������ʾ
        public int zbkdLabelLengthMain; // = 50; ������̶���ʾ����
        public int zbkdLabelLengthAux; // ������̶���ʾ����
        public bool zbIsGrid; // = true; �Ƿ�������ʾ
        public int zbFontSize; // ������ʾ�����С
        public int zbcolor;    // ������ʾ��ɫ
        public int zbLineWidth;
        public void Init()
        {
            //zbdisplayX = false;
            //zbdisplayY = false;
            zbStart = new Point(0, 0);
            zbBound = new Rectangle(0, 0, 1, 1);
            zbkdXMain = 100;
            zbkdYMain = 100;
            zbkdXAux = 50;
            zbkdYAux = 50;
            Xenable = false;
            Yenable = false;
            zbkdLabelLengthMain = 50;
            zbkdLabelLengthAux = 30;
            zbIsGrid = true;
            zbFontSize = 10;
            zbcolor = Color.Red.ToArgb();
            zbLineWidth = 2;
        }
    }
    public class PamaterSetting
    {
        public ZBpara zbDrawParamater = new ZBpara();

        public PamaterSetting()
        {
            // ���캯��
            ZbDrawInit();
        }
        public void ZbDrawInit()
        {
            zbDrawParamater.Init();
        }
        private ZBpara xamBytesToStructB(byte[] arr)
        {
            ZBpara struReturn = new ZBpara();
            int size = Marshal.SizeOf(struReturn);
            System.IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(arr, 0, ptr, arr.Length);

            struReturn = (ZBpara)Marshal.PtrToStructure(ptr, struReturn.GetType());
            Marshal.FreeHGlobal(ptr);
            return struReturn;
        }
        private byte[] xamStructToBytesB(ZBpara s)
        {
            int size = Marshal.SizeOf(s);
            System.IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(s, ptr, true);
            byte[] ba = new byte[size];

            Marshal.Copy(ptr, ba, 0, ba.Length);
            return ba;
        }
        public bool GetDefaultSetting(ref ZBpara mpara)
        {
            bool error = false;
            string path = Application.StartupPath;
            path = path + @"\DefaultData";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            if (error == false)
            {
                path = path + @"\DefaultSetting.ini";
                if (System.IO.File.Exists(path))  // ���������ļ����ڣ�
                {
                    ZBpara mp = new ZBpara(); // ����һ���洢 ������ �����ݽṹʵ��
                    byte[] readblock = new byte[Marshal.SizeOf(mp)];

                    FileStream afile = new FileStream(path, FileMode.Open);
                    BinaryReader r = new BinaryReader(afile); // ����һ�� ������ �ļ��� 

                    try // ��ʼ��ȡ �����ļ� �ṹ�����ڱ�������ֻ��һ�� �ṹ����ֻ����һ���ṹ���ݼ���
                    {

                        readblock = r.ReadBytes(Marshal.SizeOf(mp)); // �Ӷ���Ķ������ļ��� �ж�ȡ MatchPixel�ṹ �Ķ��������� ��������
                        mp = xamBytesToStructB(readblock);

                        mpara = mp; // ����ȡ��ֵ ������Ҫ�� MatchPixelʵ����
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        mpara.Init(); // �ó�ʼ������
                        MessageBox.Show("�������ļ�����" + ex.Message);
                    }
                    finally
                    {
                        r.Close();
                        afile.Close();
                        afile.Dispose();
                    }
                }
                else
                {
                    mpara.Init();
                    error = true;
                }
            }
            return !error;
        }
        public bool SetDefaultSetting(ZBpara mpara) // ���������ṹΪ Ĭ�ϲ�����
        {
            bool error = false;
            string path = Application.StartupPath;
            path = path + @"\DefaultData";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            if (error == false)
            {
                path = path + @"\DefaultSetting.ini";
                if (System.IO.File.Exists(path))
                {
                    File.Delete(path); // �������ԭ���� ini �ļ���ɾ����д��
                }
                FileStream afile = new FileStream(path, FileMode.Append);
                BinaryWriter r = new BinaryWriter(afile); // ����һ�� ������ �ļ��� 

                byte[] readblock = new byte[Marshal.SizeOf(mpara)]; // ��������ṹת��Ϊ �������� ����
                readblock = xamStructToBytesB(mpara);
                try
                {
                    r.Write(readblock);
                }
                catch (Exception ex)
                {
                    error = true;
                    MessageBox.Show("�����ļ��������" + ex.Message);
                }
                finally
                {
                    r.Close();
                    afile.Close();
                    afile.Dispose();
                }
            }
            return !error;

        }

        //public bool GetUserSetting(string path, ref MatchPixel mpara)
        //{
        //    bool error = false;
        //    if (error == false)
        //    {
        //        if (System.IO.File.Exists(path))  // ���������ļ����ڣ�
        //        {
        //            MatchPixel mp = new MatchPixel(); // ����һ���洢 ������ �����ݽṹʵ��
        //            byte[] readblock = new byte[Marshal.SizeOf(mp)];

        //            FileStream afile = new FileStream(path, FileMode.Open);
        //            BinaryReader r = new BinaryReader(afile); // ����һ�� ������ �ļ��� 

        //            try // ��ʼ��ȡ �����ļ� �ṹ�����ڱ�������ֻ��һ�� �ṹ����ֻ����һ���ṹ���ݼ���
        //            {

        //                readblock = r.ReadBytes(Marshal.SizeOf(mp)); // �Ӷ���Ķ������ļ��� �ж�ȡ MatchPixel�ṹ �Ķ��������� ��������
        //                mp = xamBytesToStructB(readblock);

        //                mpara = mp; // ����ȡ��ֵ ������Ҫ�� MatchPixelʵ����
        //            }
        //            catch (Exception ex)
        //            {
        //                error = true;
        //                MessageBox.Show("�������ļ�����" + ex.Message);
        //            }
        //            finally
        //            {
        //                r.Close();
        //                afile.Close();
        //                afile.Dispose();
        //            }
        //        }
        //        else
        //            error = true;
        //    }
        //    return !error;
        //}
        //public bool SetUserSetting(string path, MatchPixel mpara) // ���������ṹΪ Ĭ�ϲ�����
        //{
        //    bool error = false;
        //    if (error == false)
        //    {
        //        if (System.IO.File.Exists(path))
        //        {
        //            File.Delete(path); // �������ԭ���� ini �ļ���ɾ����д��
        //        }
        //        FileStream afile = new FileStream(path, FileMode.Append);
        //        BinaryWriter r = new BinaryWriter(afile); // ����һ�� ������ �ļ��� 

        //        byte[] readblock = new byte[Marshal.SizeOf(mpara)]; // ��������ṹת��Ϊ �������� ����
        //        try
        //        {
        //            readblock = xamStructToBytesB(mpara);
        //            r.Write(readblock);
        //        }
        //        catch (Exception ex)
        //        {
        //            error = true;
        //            MessageBox.Show("�����ļ��������" + ex.Message);
        //        }
        //        finally
        //        {
        //            r.Close();
        //            afile.Close();
        //            afile.Dispose();
        //        }
        //    }
        //    return !error;

        //}

    }
}
