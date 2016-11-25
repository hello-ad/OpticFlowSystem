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
        //public bool zbdisplayX; //= false; 坐标轴显示
        //public bool zbdisplayY;
        public Point zbStart; //= new Point(0, 0); 坐标轴起点
        public Rectangle zbBound; //= new Rectangle(0, 0, 1, 1); 图像包围盒
        public int zbkdXMain; // = 100; X坐标刻度
        public int zbkdYMain; //= 100;  Y坐标刻度
        public int zbkdXAux;   // X 次坐标刻度
        public int zbkdYAux;   // Y 次坐标刻度
        public bool Xenable; //= true; X坐标显示
        public bool Yenable; //= true;Y坐标显示
        public int zbkdLabelLengthMain; // = 50; 主坐标刻度显示长度
        public int zbkdLabelLengthAux; // 次坐标刻度显示长度
        public bool zbIsGrid; // = true; 是否网格显示
        public int zbFontSize; // 坐标显示字体大小
        public int zbcolor;    // 坐标显示颜色
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
            // 构造函数
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
                if (System.IO.File.Exists(path))  // 参数定义文件存在！
                {
                    ZBpara mp = new ZBpara(); // 定义一个存储 参数表 的数据结构实例
                    byte[] readblock = new byte[Marshal.SizeOf(mp)];

                    FileStream afile = new FileStream(path, FileMode.Open);
                    BinaryReader r = new BinaryReader(afile); // 定义一个 二进制 文件流 

                    try // 开始读取 参数文件 结构，由于本参数表只有一个 结构，故只读入一个结构数据即可
                    {

                        readblock = r.ReadBytes(Marshal.SizeOf(mp)); // 从定义的二进制文件流 中读取 MatchPixel结构 的二进制数据 长度数据
                        mp = xamBytesToStructB(readblock);

                        mpara = mp; // 将读取的值 置入需要的 MatchPixel实例中
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        mpara.Init(); // 置初始化数据
                        MessageBox.Show("读参数文件出错！" + ex.Message);
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
        public bool SetDefaultSetting(ZBpara mpara) // 保存参数表结构为 默认参数表
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
                    File.Delete(path); // 如果存在原来的 ini 文件，删除重写！
                }
                FileStream afile = new FileStream(path, FileMode.Append);
                BinaryWriter r = new BinaryWriter(afile); // 定义一个 二进制 文件流 

                byte[] readblock = new byte[Marshal.SizeOf(mpara)]; // 将参数表结构转换为 二进制流 数据
                readblock = xamStructToBytesB(mpara);
                try
                {
                    r.Write(readblock);
                }
                catch (Exception ex)
                {
                    error = true;
                    MessageBox.Show("参数文件保存出错！" + ex.Message);
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
        //        if (System.IO.File.Exists(path))  // 参数定义文件存在！
        //        {
        //            MatchPixel mp = new MatchPixel(); // 定义一个存储 参数表 的数据结构实例
        //            byte[] readblock = new byte[Marshal.SizeOf(mp)];

        //            FileStream afile = new FileStream(path, FileMode.Open);
        //            BinaryReader r = new BinaryReader(afile); // 定义一个 二进制 文件流 

        //            try // 开始读取 参数文件 结构，由于本参数表只有一个 结构，故只读入一个结构数据即可
        //            {

        //                readblock = r.ReadBytes(Marshal.SizeOf(mp)); // 从定义的二进制文件流 中读取 MatchPixel结构 的二进制数据 长度数据
        //                mp = xamBytesToStructB(readblock);

        //                mpara = mp; // 将读取的值 置入需要的 MatchPixel实例中
        //            }
        //            catch (Exception ex)
        //            {
        //                error = true;
        //                MessageBox.Show("读参数文件出错！" + ex.Message);
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
        //public bool SetUserSetting(string path, MatchPixel mpara) // 保存参数表结构为 默认参数表
        //{
        //    bool error = false;
        //    if (error == false)
        //    {
        //        if (System.IO.File.Exists(path))
        //        {
        //            File.Delete(path); // 如果存在原来的 ini 文件，删除重写！
        //        }
        //        FileStream afile = new FileStream(path, FileMode.Append);
        //        BinaryWriter r = new BinaryWriter(afile); // 定义一个 二进制 文件流 

        //        byte[] readblock = new byte[Marshal.SizeOf(mpara)]; // 将参数表结构转换为 二进制流 数据
        //        try
        //        {
        //            readblock = xamStructToBytesB(mpara);
        //            r.Write(readblock);
        //        }
        //        catch (Exception ex)
        //        {
        //            error = true;
        //            MessageBox.Show("参数文件保存出错！" + ex.Message);
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
