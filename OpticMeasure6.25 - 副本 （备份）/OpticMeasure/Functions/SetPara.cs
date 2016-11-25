using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HL2D.View2D;

namespace ZH2DCL.Setting
{
    public partial class SetPara : Form
    {
        private PamaterSetting pamater;

        public SetPara(PamaterSetting _pamater)
        {
            InitializeComponent();
            pamater = _pamater;
        }

        private void SetPara_Load(object sender, EventArgs e)
        {
            Display();
        }

        private void Display()
        {
            this.ck_Xenable.Checked = pamater.zbDrawParamater.Xenable;
            this.ck_Yenable.Checked = pamater.zbDrawParamater.Yenable;
            this.ck_IsGrid.Checked  = pamater.zbDrawParamater.zbIsGrid;

            this.nu_XkdM.Value = (decimal)pamater.zbDrawParamater.zbkdXMain;
            this.nu_YkdM.Value = (decimal)pamater.zbDrawParamater.zbkdYMain;
            this.nu_XkdA.Value = (decimal)pamater.zbDrawParamater.zbkdXAux;
            this.nu_YkdA.Value = (decimal)pamater.zbDrawParamater.zbkdYAux;
            this.nu_MkdLength.Value = (decimal)pamater.zbDrawParamater.zbkdLabelLengthMain;
            this.nu_kdALength.Value = (decimal)pamater.zbDrawParamater.zbkdLabelLengthAux;

            this.nu_font.Value = (decimal)pamater.zbDrawParamater.zbFontSize;
            this.panel2.BackColor = Color.FromArgb(pamater.zbDrawParamater.zbcolor);
            this.nu_lineWidth.Value = (decimal)pamater.zbDrawParamater.zbLineWidth;
        }

        private void SetPamater()
        {
            pamater.zbDrawParamater.Xenable = this.ck_Xenable.Checked;
            pamater.zbDrawParamater.Yenable = this.ck_Yenable.Checked;
            pamater.zbDrawParamater.zbIsGrid = this.ck_IsGrid.Checked;

            pamater.zbDrawParamater.zbkdXMain = Convert.ToInt32(this.nu_XkdM.Value);
            pamater.zbDrawParamater.zbkdYMain = Convert.ToInt32(this.nu_YkdM.Value);
            pamater.zbDrawParamater.zbkdXAux = Convert.ToInt32(this.nu_XkdA.Value);
            pamater.zbDrawParamater.zbkdYAux = Convert.ToInt32(this.nu_YkdA.Value);
            pamater.zbDrawParamater.zbkdLabelLengthMain = Convert.ToInt32(this.nu_MkdLength.Value);
            pamater.zbDrawParamater.zbkdLabelLengthAux = Convert.ToInt32(this.nu_kdALength.Value);

            pamater.zbDrawParamater.zbFontSize = Convert.ToInt32(this.nu_font.Value);
            pamater.zbDrawParamater.zbcolor = this.panel2.BackColor.ToArgb();
            pamater.zbDrawParamater.zbLineWidth = Convert.ToInt32(this.nu_lineWidth.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetPamater();
            pamater.SetDefaultSetting(pamater.zbDrawParamater);
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            button1_Click(sender, new EventArgs());
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            pamater.zbDrawParamater.Init();
            Display();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            button2_Click(sender, new EventArgs());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog coldiglog = new ColorDialog();
            if (coldiglog.ShowDialog() == DialogResult.OK)
            {
                this.panel2.BackColor = coldiglog.Color;
            }
            coldiglog.Dispose();
        }

    }
}