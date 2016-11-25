namespace OpticMeasure.Function
{
    partial class imageDsp
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelControl = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.nuScale = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkGrayAdd = new System.Windows.Forms.CheckBox();
            this.panelView = new System.Windows.Forms.Panel();
            this.pictureView = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.图像放大ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图像缩小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图像复原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图像查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelControl.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuScale)).BeginInit();
            this.panelView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.panel3);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl.Location = new System.Drawing.Point(0, 413);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(730, 25);
            this.panelControl.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, -2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(730, 27);
            this.panel3.TabIndex = 5;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.checkBox3);
            this.panel6.Controls.Add(this.checkBox2);
            this.panel6.Controls.Add(this.checkBox1);
            this.panel6.Controls.Add(this.nuScale);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.chkGrayAdd);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(273, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(457, 27);
            this.panel6.TabIndex = 4;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(216, 7);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(72, 16);
            this.checkBox3.TabIndex = 14;
            this.checkBox3.Text = "正向叠加";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(380, 7);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(72, 16);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "模板显示";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(290, 7);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "灰度化显示";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // nuScale
            // 
            this.nuScale.DecimalPlaces = 1;
            this.nuScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nuScale.Location = new System.Drawing.Point(74, 5);
            this.nuScale.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nuScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nuScale.Name = "nuScale";
            this.nuScale.Size = new System.Drawing.Size(46, 21);
            this.nuScale.TabIndex = 11;
            this.nuScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuScale.ValueChanged += new System.EventHandler(this.nuScale_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "灰度差增强";
            // 
            // chkGrayAdd
            // 
            this.chkGrayAdd.AutoSize = true;
            this.chkGrayAdd.Location = new System.Drawing.Point(133, 7);
            this.chkGrayAdd.Name = "chkGrayAdd";
            this.chkGrayAdd.Size = new System.Drawing.Size(84, 16);
            this.chkGrayAdd.TabIndex = 9;
            this.chkGrayAdd.Text = "叠加灰度差";
            this.chkGrayAdd.UseVisualStyleBackColor = true;
            this.chkGrayAdd.CheckedChanged += new System.EventHandler(this.chkGrayAdd_CheckedChanged);
            // 
            // panelView
            // 
            this.panelView.Controls.Add(this.pictureView);
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(0, 0);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(730, 413);
            this.panelView.TabIndex = 1;
            // 
            // pictureView
            // 
            this.pictureView.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureView.Location = new System.Drawing.Point(4, 4);
            this.pictureView.Name = "pictureView";
            this.pictureView.Size = new System.Drawing.Size(254, 228);
            this.pictureView.TabIndex = 0;
            this.pictureView.TabStop = false;
            this.pictureView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureView_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.图像放大ToolStripMenuItem,
            this.图像缩小ToolStripMenuItem,
            this.图像复原ToolStripMenuItem,
            this.图像查看ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 92);
            // 
            // 图像放大ToolStripMenuItem
            // 
            this.图像放大ToolStripMenuItem.Name = "图像放大ToolStripMenuItem";
            this.图像放大ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.图像放大ToolStripMenuItem.Text = "图像放大";
            this.图像放大ToolStripMenuItem.Click += new System.EventHandler(this.图像放大ToolStripMenuItem_Click);
            // 
            // 图像缩小ToolStripMenuItem
            // 
            this.图像缩小ToolStripMenuItem.Name = "图像缩小ToolStripMenuItem";
            this.图像缩小ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.图像缩小ToolStripMenuItem.Text = "图像缩小";
            this.图像缩小ToolStripMenuItem.Click += new System.EventHandler(this.图像缩小ToolStripMenuItem_Click);
            // 
            // 图像复原ToolStripMenuItem
            // 
            this.图像复原ToolStripMenuItem.Name = "图像复原ToolStripMenuItem";
            this.图像复原ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.图像复原ToolStripMenuItem.Text = "图像复原";
            this.图像复原ToolStripMenuItem.Click += new System.EventHandler(this.图像复原ToolStripMenuItem_Click);
            // 
            // 图像查看ToolStripMenuItem
            // 
            this.图像查看ToolStripMenuItem.Name = "图像查看ToolStripMenuItem";
            this.图像查看ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.图像查看ToolStripMenuItem.Text = "图像查看";
            this.图像查看ToolStripMenuItem.Click += new System.EventHandler(this.图像查看ToolStripMenuItem_Click);
            // 
            // imageDsp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelView);
            this.Controls.Add(this.panelControl);
            this.Name = "imageDsp";
            this.Size = new System.Drawing.Size(730, 438);
            this.Load += new System.EventHandler(this.imageDsp_Load);
            this.Resize += new System.EventHandler(this.imageDsp_Resize);
            this.panelControl.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuScale)).EndInit();
            this.panelView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Panel panelView;
        private System.Windows.Forms.PictureBox pictureView;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox chkGrayAdd;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 图像放大ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图像缩小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图像复原ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图像查看ToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nuScale;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}
