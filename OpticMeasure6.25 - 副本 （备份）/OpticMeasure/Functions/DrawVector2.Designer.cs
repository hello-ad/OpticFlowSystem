namespace ZHCLNEW.DrawCloud
{
    partial class DrawVector2
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.zuobiaoLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSaveArea = new System.Windows.Forms.CheckBox();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filetree = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCurFile = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panelVector2 = new System.Windows.Forms.Panel();
            this.pictureVector2 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.图像放大ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图像缩小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图像复原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图像查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelVector2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureVector2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zuobiaoLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(713, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // zuobiaoLabel1
            // 
            this.zuobiaoLabel1.Name = "zuobiaoLabel1";
            this.zuobiaoLabel1.Size = new System.Drawing.Size(56, 22);
            this.zuobiaoLabel1.Text = "坐标参数";
            this.zuobiaoLabel1.Click += new System.EventHandler(this.zuobiaoLabel1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numericUpDown2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.chkSaveArea);
            this.panel1.Controls.Add(this.chkSave);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 448);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(713, 35);
            this.panel1.TabIndex = 1;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 1;
            this.numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown2.Location = new System.Drawing.Point(589, 9);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(52, 21);
            this.numericUpDown2.TabIndex = 46;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown2.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(554, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 45;
            this.label3.Text = "比例";
            // 
            // chkSaveArea
            // 
            this.chkSaveArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSaveArea.AutoSize = true;
            this.chkSaveArea.Enabled = false;
            this.chkSaveArea.Location = new System.Drawing.Point(441, 11);
            this.chkSaveArea.Name = "chkSaveArea";
            this.chkSaveArea.Size = new System.Drawing.Size(102, 16);
            this.chkSaveArea.TabIndex = 44;
            this.chkSaveArea.Text = "选择保存区域 ";
            this.chkSaveArea.UseVisualStyleBackColor = true;
            // 
            // chkSave
            // 
            this.chkSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSave.AutoSize = true;
            this.chkSave.Location = new System.Drawing.Point(363, 12);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(72, 16);
            this.chkSave.TabIndex = 43;
            this.chkSave.Text = "保存图片";
            this.chkSave.UseVisualStyleBackColor = true;
            this.chkSave.CheckedChanged += new System.EventHandler(this.chkSave_CheckedChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(309, 9);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "播放频率";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(171, 11);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(71, 16);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "多次循环";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(95, 11);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(71, 16);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "一次循环";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "动画显示";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.filetree);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 423);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图片选择";
            // 
            // filetree
            // 
            this.filetree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filetree.Location = new System.Drawing.Point(3, 17);
            this.filetree.Name = "filetree";
            this.filetree.Size = new System.Drawing.Size(194, 374);
            this.filetree.TabIndex = 2;
            this.filetree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.filetree_BeforeSelect);
            this.filetree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.filetree_AfterSelect);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtCurFile);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 391);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 29);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "当前：";
            // 
            // txtCurFile
            // 
            this.txtCurFile.Location = new System.Drawing.Point(50, 4);
            this.txtCurFile.Name = "txtCurFile";
            this.txtCurFile.Size = new System.Drawing.Size(136, 21);
            this.txtCurFile.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panelVector2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(200, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(513, 423);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "位移场显示区";
            // 
            // panelVector2
            // 
            this.panelVector2.Controls.Add(this.pictureVector2);
            this.panelVector2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVector2.Location = new System.Drawing.Point(3, 17);
            this.panelVector2.Name = "panelVector2";
            this.panelVector2.Size = new System.Drawing.Size(507, 403);
            this.panelVector2.TabIndex = 0;
            // 
            // pictureVector2
            // 
            this.pictureVector2.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureVector2.Location = new System.Drawing.Point(0, 4);
            this.pictureVector2.Name = "pictureVector2";
            this.pictureVector2.Size = new System.Drawing.Size(211, 156);
            this.pictureVector2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureVector2.TabIndex = 0;
            this.pictureVector2.TabStop = false;
            this.pictureVector2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureVector2_MouseClick);
            this.pictureVector2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureVector2_MouseDown);
            this.pictureVector2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureVector2_MouseMove);
            this.pictureVector2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureVector2_MouseUp);
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
            this.图像查看ToolStripMenuItem.Text = "光标还原";
            this.图像查看ToolStripMenuItem.Click += new System.EventHandler(this.图像查看ToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DrawVector2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "DrawVector2";
            this.Size = new System.Drawing.Size(713, 483);
            this.Load += new System.EventHandler(this.DrawVector2_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panelVector2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureVector2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panelVector2;
        private System.Windows.Forms.PictureBox pictureVector2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 图像放大ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图像缩小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图像复原ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图像查看ToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel zuobiaoLabel1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView filetree;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCurFile;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.CheckBox chkSaveArea;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
    }
}
