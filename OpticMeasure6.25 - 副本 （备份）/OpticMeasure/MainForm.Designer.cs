namespace OpticMeasure
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读入照片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读取工程文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存梯度量ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图片编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.八邻域函数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.灰度差叠加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.匹配ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单母板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二维矢量图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.动画生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.模型变形测量ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.计算旋转矩阵ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.光流计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.图片编辑ToolStripMenuItem,
            this.匹配ToolStripMenuItem,
            this.视图ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.动画生成ToolStripMenuItem,
            this.模型变形测量ToolStripMenuItem,
            this.计算旋转矩阵ToolStripMenuItem,
            this.光流计算ToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(750, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.读入照片ToolStripMenuItem,
            this.读取工程文件ToolStripMenuItem,
            this.保存梯度量ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 读入照片ToolStripMenuItem
            // 
            this.读入照片ToolStripMenuItem.Name = "读入照片ToolStripMenuItem";
            this.读入照片ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.读入照片ToolStripMenuItem.Text = "读入照片";
            this.读入照片ToolStripMenuItem.Click += new System.EventHandler(this.读入照片ToolStripMenuItem_Click);
            // 
            // 读取工程文件ToolStripMenuItem
            // 
            this.读取工程文件ToolStripMenuItem.Name = "读取工程文件ToolStripMenuItem";
            this.读取工程文件ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.读取工程文件ToolStripMenuItem.Text = "读取工程文件";
            this.读取工程文件ToolStripMenuItem.Click += new System.EventHandler(this.读取工程文件ToolStripMenuItem_Click);
            // 
            // 保存梯度量ToolStripMenuItem
            // 
            this.保存梯度量ToolStripMenuItem.Name = "保存梯度量ToolStripMenuItem";
            this.保存梯度量ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.保存梯度量ToolStripMenuItem.Text = "保存梯度量";
            this.保存梯度量ToolStripMenuItem.Click += new System.EventHandler(this.保存梯度量ToolStripMenuItem_Click);
            // 
            // 图片编辑ToolStripMenuItem
            // 
            this.图片编辑ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.八邻域函数ToolStripMenuItem,
            this.灰度差叠加ToolStripMenuItem});
            this.图片编辑ToolStripMenuItem.Name = "图片编辑ToolStripMenuItem";
            this.图片编辑ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.图片编辑ToolStripMenuItem.Text = "图片编辑";
            // 
            // 八邻域函数ToolStripMenuItem
            // 
            this.八邻域函数ToolStripMenuItem.Name = "八邻域函数ToolStripMenuItem";
            this.八邻域函数ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.八邻域函数ToolStripMenuItem.Text = "八邻域函数";
            this.八邻域函数ToolStripMenuItem.Click += new System.EventHandler(this.八邻域函数ToolStripMenuItem_Click);
            // 
            // 灰度差叠加ToolStripMenuItem
            // 
            this.灰度差叠加ToolStripMenuItem.Name = "灰度差叠加ToolStripMenuItem";
            this.灰度差叠加ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.灰度差叠加ToolStripMenuItem.Text = "灰度差叠加";
            this.灰度差叠加ToolStripMenuItem.Click += new System.EventHandler(this.灰度差叠加ToolStripMenuItem_Click);
            // 
            // 匹配ToolStripMenuItem
            // 
            this.匹配ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.单母板ToolStripMenuItem});
            this.匹配ToolStripMenuItem.Name = "匹配ToolStripMenuItem";
            this.匹配ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.匹配ToolStripMenuItem.Text = "匹配";
            // 
            // 单母板ToolStripMenuItem
            // 
            this.单母板ToolStripMenuItem.Name = "单母板ToolStripMenuItem";
            this.单母板ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.单母板ToolStripMenuItem.Text = "单母板";
            this.单母板ToolStripMenuItem.Click += new System.EventHandler(this.单母板ToolStripMenuItem_Click);
            // 
            // 视图ToolStripMenuItem
            // 
            this.视图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.二维矢量图ToolStripMenuItem});
            this.视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            this.视图ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.视图ToolStripMenuItem.Text = "视图";
            // 
            // 二维矢量图ToolStripMenuItem
            // 
            this.二维矢量图ToolStripMenuItem.Name = "二维矢量图ToolStripMenuItem";
            this.二维矢量图ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.二维矢量图ToolStripMenuItem.Text = "二维矢量图";
            this.二维矢量图ToolStripMenuItem.Click += new System.EventHandler(this.二维矢量图ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 动画生成ToolStripMenuItem
            // 
            this.动画生成ToolStripMenuItem.Name = "动画生成ToolStripMenuItem";
            this.动画生成ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.动画生成ToolStripMenuItem.Text = "动画生成";
            this.动画生成ToolStripMenuItem.Click += new System.EventHandler(this.动画生成ToolStripMenuItem_Click);
            // 
            // 模型变形测量ToolStripMenuItem
            // 
            this.模型变形测量ToolStripMenuItem.Name = "模型变形测量ToolStripMenuItem";
            this.模型变形测量ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.模型变形测量ToolStripMenuItem.Text = "模型变形测量";
            this.模型变形测量ToolStripMenuItem.Click += new System.EventHandler(this.模型变形测量ToolStripMenuItem_Click);
            // 
            // 计算旋转矩阵ToolStripMenuItem
            // 
            this.计算旋转矩阵ToolStripMenuItem.Name = "计算旋转矩阵ToolStripMenuItem";
            this.计算旋转矩阵ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.计算旋转矩阵ToolStripMenuItem.Text = "计算旋转矩阵";
            this.计算旋转矩阵ToolStripMenuItem.Click += new System.EventHandler(this.计算旋转矩阵ToolStripMenuItem_Click);
            // 
            // 光流计算ToolStripMenuItem
            // 
            this.光流计算ToolStripMenuItem.Name = "光流计算ToolStripMenuItem";
            this.光流计算ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.光流计算ToolStripMenuItem.Text = "光流计算";
            this.光流计算ToolStripMenuItem.Click += new System.EventHandler(this.光流计算ToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(41, 21);
            this.testToolStripMenuItem.Text = "test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 384);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图片编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 匹配ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 动画生成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读入照片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 模型变形测量ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读取工程文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 单母板ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 八邻域函数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 灰度差叠加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 计算旋转矩阵ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二维矢量图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存梯度量ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 光流计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
    }
}