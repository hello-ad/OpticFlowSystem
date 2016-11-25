using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;
//using ZH2DCL.ZtClass;

namespace ZH2DCL._2DClass
{

    public class ImageCommon
    {
        //Struct 收集系统信息
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public uint dwOemId;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
        }
        //struct 收集内存情况
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORYSTATUS
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public uint dwTotalPhys;
            public uint dwAvailPhys;
            public uint dwTotalPageFile;
            public uint dwAvailPageFile;
            public uint dwTotalVirtual;
            public uint dwAvailVirtual;
        }
        //获取系统信息
        [DllImport("kernel32")]
        static extern void GetSystemInfo(ref SYSTEM_INFO pSI);
        //获取内存信息 
        [DllImport("kernel32")]
        static extern void GlobalMemoryStatus(ref MEMORYSTATUS buf);

        #region // ************   获取 CPU 核心数量 ---支持 64位操作系统
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESSORCORE
        {
            public byte Flags;
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct NUMANODE
        {
            public uint NodeNumber;
        }

        private enum PROCESSOR_CACHE_TYPE
        {
            CacheUnified,
            CacheInstruction,
            CacheData,
            CacheTrace
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CACHE_DESCRIPTOR
        {
            public byte Level;
            public byte Associativity;
            public ushort LineSize;
            public uint Size;
            public PROCESSOR_CACHE_TYPE Type;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION_UNION
        {
            [FieldOffset(0)]
            public PROCESSORCORE ProcessorCore;
            [FieldOffset(0)]
            public NUMANODE NumaNode;
            [FieldOffset(0)]
            public CACHE_DESCRIPTOR Cache;
            [FieldOffset(0)]
            private UInt64 Reserved1;
            [FieldOffset(8)]
            private UInt64 Reserved2;
        }


        private enum LOGICAL_PROCESSOR_RELATIONSHIP
        {
            RelationProcessorCore,
            RelationNumaNode,
            RelationCache,
            RelationProcessorPackage,
            RelationGroup,
            RelationAll = 0xffff
        }

        private struct SYSTEM_LOGICAL_PROCESSOR_INFORMATION
        {
            public UIntPtr ProcessorMask;
            public LOGICAL_PROCESSOR_RELATIONSHIP Relationship;
            public SYSTEM_LOGICAL_PROCESSOR_INFORMATION_UNION ProcessorInformation;
        }

        [DllImport(@"kernel32.dll", SetLastError = true)]
        private static extern bool GetLogicalProcessorInformation(
            IntPtr Buffer,
            ref uint ReturnLength
        );

        private const int ERROR_INSUFFICIENT_BUFFER = 122;

        private static SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] MyGetLogicalProcessorInformation()
        {
            uint ReturnLength = 0;
            GetLogicalProcessorInformation(IntPtr.Zero, ref ReturnLength);
            if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
            {
                IntPtr Ptr = Marshal.AllocHGlobal((int)ReturnLength);
                try
                {
                    if (GetLogicalProcessorInformation(Ptr, ref ReturnLength))
                    {
                        int size = Marshal.SizeOf(typeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION));
                        int len = (int)ReturnLength / size;
                        SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] Buffer = new SYSTEM_LOGICAL_PROCESSOR_INFORMATION[len];
                        IntPtr Item = Ptr;
                        for (int i = 0; i < len; i++)
                        {
                            Buffer[i] = (SYSTEM_LOGICAL_PROCESSOR_INFORMATION)Marshal.PtrToStructure(Item, typeof(SYSTEM_LOGICAL_PROCESSOR_INFORMATION));
                            //Item = new IntPtr((Int32)Item + size);
                            Item = new IntPtr(Item.ToInt64() + size);
                        }
                        return Buffer;
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(Ptr);
                }
            }
            return null;
        }

        public int getCpuCores()
        {
            SYSTEM_LOGICAL_PROCESSOR_INFORMATION[] Buffer = MyGetLogicalProcessorInformation();
            int coreCount = 0;
            for (int i = 0; i < Buffer.Length; i++)
            {
                //listBox1.Items.Add(Buffer[i].ToString() + ":" + Buffer[i].ProcessorMask);
                if (Buffer[i].Relationship == LOGICAL_PROCESSOR_RELATIONSHIP.RelationProcessorCore)
                {
                    coreCount += 1;
                }
            }
            return coreCount;
        }
        #endregion // ***************************************************

        public ImageCommon()
        {
            // 构造函数
        }

        public int availMemory()
        {
            MEMORYSTATUS memSt = new MEMORYSTATUS();
            GlobalMemoryStatus(ref memSt);
            int availMem = (int)memSt.dwAvailPhys;
            return availMem;
        }
        public int availCPUCore(int usedBytePerImage)
        {
            int num = 0;
            int cpuCore = 0;
            cpuCore = Environment.ProcessorCount;
            if (cpuCore <= 0)
            {
                SYSTEM_INFO pSI = new SYSTEM_INFO();
                GetSystemInfo(ref pSI);
                cpuCore = (int)pSI.dwNumberOfProcessors;
            }

            if (cpuCore <= 0)
            {
                cpuCore = getCpuCores();
            }

            MEMORYSTATUS memSt = new MEMORYSTATUS();
            GlobalMemoryStatus(ref memSt);
            int availMem = (int)memSt.dwAvailPhys;

            if (cpuCore * usedBytePerImage < availMem)
                num = cpuCore;
            else
            {
                for (int i = cpuCore - 1; i > 0; i--)
                {
                    if (i * usedBytePerImage < availMem)
                    {
                        num = i;
                        break;
                    }
                }
            }
            return num;
        }
        public int availCPUCore(string filename)
        {
            int num = 0;
            int cpuCore = 0;
            cpuCore = Environment.ProcessorCount;
            
            //ManagementClass c = new ManagementClass(new ManagementPath("Win32_Processor"));
            //// Get the properties in the class  
            //ManagementObjectCollection moc = c.GetInstances();
            //if (moc.Count > 0)
            //{
            //    cpuCore = moc.Count;
            //}
            //else
            if (cpuCore <= 0)
            {
                SYSTEM_INFO pSI = new SYSTEM_INFO();
                GetSystemInfo(ref pSI);
                cpuCore = (int)pSI.dwNumberOfProcessors;
            }

            if (cpuCore <= 0)
            {
                cpuCore = getCpuCores();
            }
            MEMORYSTATUS memSt = new MEMORYSTATUS();
            GlobalMemoryStatus(ref memSt);
            long availMem = (long)memSt.dwAvailPhys;

            Bitmap curBitmap = (Bitmap)Image.FromFile(filename);
  
            GlobalMemoryStatus(ref memSt);
            long availMemUsed = (long)memSt.dwAvailPhys;
            long usedBytePerImage = availMem - availMemUsed;
            long usedByte = curBitmap.Width * curBitmap.Height * 3 ;
            if (usedBytePerImage < usedByte)
                usedBytePerImage = Convert.ToInt64(usedByte * 1.05);

            curBitmap.Dispose();
            GC.Collect();

            GlobalMemoryStatus(ref memSt);
            availMem = (long)memSt.dwAvailPhys;

            //if (Convert.ToInt32(usedBytePerImage / 1024 / 1024) >= 100)
            //    cpuCore = 1;

            // 每张照片在 读入过程中， 其可能使用的内存为其本身所占内存大小的 8 位左右
            // 故以 8 倍来检测可能并行的 核心数
            if ( cpuCore * (usedBytePerImage/1024) * 10 < availMem/1024 )
            {
                num = cpuCore;
            }
            else
            {
                for (int i = cpuCore - 1; i > 0; i--)
                {
                    if (i * (usedBytePerImage/1024) * 10 < availMem/1024 )
                    {
                        num = i;
                        break;
                    }
                }
            }
            if (num == 0)
                num = 8;
            return num;
        }
        public void TransTo24JpgAndSave(string filename, string targetFilename)
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

        public Bitmap transformPictureinto24Jepg(Bitmap curBitmap)
        {
            //计算24位图像的字节数 
            int bytes = curBitmap.Width * curBitmap.Height * 3;

            //定义位图数组 
            byte[] grayValues = new byte[bytes];

            //复制被锁定的图像到该数组 
            //System.Runtime.InteropServices.Marshal.Copy(ptr2, grayValues, 0, bytes);

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

            //System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, ptr2, bytes);

            //解锁 
            curBitmap.UnlockBits(data);

            return curBitmap;
        } // 只能将 8 位的照片转换为 24 位

        public string getFileNameOnly(string filename)
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
        //public void refreshFiletree(TreeView filetree,MatchClass currentOMC, int start, int end, int except)
        //{
        //    string filename; // 读入有效图片文件，且正常进行了灰度化处理
        //    TreeNode node;
        //    filetree.Nodes.Clear();

        //    if (start != 0)
        //    {
        //        node = new TreeNode();
        //        node.Text = "查看上一组";
        //        node.Name = (start - currentOMC.numInProBatch).ToString();
        //        node.Tag = "-1";
        //        filetree.Nodes.Add(node);
        //    }
        //    for (int i = start; i < end; i++)
        //    {
        //        // 逐个文件建立 图片文件 树，以便于进行查看
        //        if (i != except)
        //        {
        //            filename = currentOMC.readFilesindex[i].FileName; // .readFileNames[i]; // 当前文件名
        //            node = new TreeNode();
        //            node.Text = getFileNameOnly(filename);
        //            node.Name = filename;
        //            //node.Tag = (i-start).ToString();  // 该照片 在数组中的序号
        //            node.Tag = i.ToString();  // 该照片 在数组中的序号
        //            filetree.Nodes.Add(node);
        //            // 由于 文件树 节点与 readFileNames 数组一一对应，故： Node.Index 也可以视作 该照片 在 readFileNames 中的序号
        //        }
        //    } // 建立照片树

        //    // 从临时文件中刷新数据，将新的一组数据写入到 picturecodedataArray数组中
        //    int batNo = Convert.ToInt32(Math.Round(start * 1.0 / currentOMC.numInProBatch));
        //    currentOMC.RecoveryTmpData(start, end, batNo, 0);

        //    if (end != currentOMC.readFileNames.Length)
        //    {
        //        node = new TreeNode();
        //        node.Text = "查看下一组";
        //        node.Name = end.ToString();
        //        node.Tag = "-9";
        //        filetree.Nodes.Add(node);
        //    }

        //    if (filetree.Nodes.Count > 0)
        //        if (start == 0)
        //            filetree.SelectedNode = filetree.Nodes[0];
        //        else
        //            filetree.SelectedNode = filetree.Nodes[1];

        //    // 默认情况下，第一张照片被选中
        //}

    }
}
