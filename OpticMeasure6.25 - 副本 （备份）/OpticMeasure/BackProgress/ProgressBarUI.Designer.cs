namespace UserProgressBar
{
    partial class ProgressBarUI
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
            this.prcBar = new System.Windows.Forms.ProgressBar();
            this.subBar = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // prcBar
            // 
            this.prcBar.ForeColor = System.Drawing.Color.Lime;
            this.prcBar.Location = new System.Drawing.Point(15, 13);
            this.prcBar.Name = "prcBar";
            this.prcBar.Size = new System.Drawing.Size(451, 23);
            this.prcBar.Step = 1;
            this.prcBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prcBar.TabIndex = 50;
            // 
            // subBar
            // 
            this.subBar.Location = new System.Drawing.Point(15, 43);
            this.subBar.Name = "subBar";
            this.subBar.Size = new System.Drawing.Size(451, 23);
            this.subBar.Step = 2;
            this.subBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.subBar.TabIndex = 51;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ProgressBarUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.subBar);
            this.Controls.Add(this.prcBar);
            this.Name = "ProgressBarUI";
            this.Size = new System.Drawing.Size(480, 78);
            this.Load += new System.EventHandler(this.ProgressBarUI_Load);
            this.SizeChanged += new System.EventHandler(this.ProgressBarUI_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar prcBar;
        private System.Windows.Forms.ProgressBar subBar;
        private System.Windows.Forms.Timer timer1;
    }
}
