namespace ZHCLNEW.DrawCloud
{
    partial class DrawVector2Frm
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
            this.drawVector21 = new ZHCLNEW.DrawCloud.DrawVector2();
            this.SuspendLayout();
            // 
            // drawVector21
            // 
            this.drawVector21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawVector21.Location = new System.Drawing.Point(0, 0);
            this.drawVector21.Name = "drawVector21";
            this.drawVector21.Size = new System.Drawing.Size(544, 444);
            this.drawVector21.TabIndex = 0;
            // 
            // DrawVector2Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 444);
            this.Controls.Add(this.drawVector21);
            this.Name = "DrawVector2Frm";
            this.Text = "像素梯度显示";
            this.Load += new System.EventHandler(this.DrawVector2Frm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DrawVector2 drawVector21;
    }
}