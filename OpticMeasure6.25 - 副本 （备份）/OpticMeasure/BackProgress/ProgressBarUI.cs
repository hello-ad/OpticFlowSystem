using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UserProgressBar
{
    public partial class ProgressBarUI : UserControl
    {
        public ProgressBarUI()
        {
            InitializeComponent();
        }

        private void ProgressBarUI_Load(object sender, EventArgs e)
        {
            this.prcBar.Width = this.Width - 20;
        }

        public bool Increase(int nValue)
        {
            if (nValue > 0)
            {
                if (prcBar.Value + nValue < prcBar.Maximum)
                {
                    prcBar.Value += nValue;
                    return true;
                }
                else
                {
                    prcBar.Value = prcBar.Maximum;
                    this.ParentForm.Close();
                    return false;
                }
            }
            this.Refresh();
            return false;
        }
        public void SetProgress(int Nvalue)
        {
            prcBar.Value = Nvalue;
        }
        public void SetTimerEnable(bool isEnable)
        {
            this.timer1.Enabled = isEnable;
            if (!isEnable)
                this.subBar.Value = subBar.Maximum;
        }
        private void ProgressBarUI_SizeChanged(object sender, EventArgs e)
        {
            prcBar.Width = this.Width - 20;
            subBar.Width = this.Width - 20;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (subBar.Value + subBar.Step <= subBar.Maximum)
            {
                subBar.Value += subBar.Step;
            }
            else
            {
                subBar.Value = 0;
            }
        }

    }
}
