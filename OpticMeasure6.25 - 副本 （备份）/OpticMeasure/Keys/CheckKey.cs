using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZHCLNEW.TeaSec;
using SenseLockApp;
using System.Windows.Forms;

namespace ZHCLNEW.MatchClassZT
{
    public class CheckKey
    {
        private TeaKeySec teasec;
        private SenseLockClass senselock;

        public CheckKey()
        {
            teasec = new TeaKeySec();
            senselock = new SenseLockClass();
        }
        public void FindKey()
        {
            if (!teasec.FindKey())
            {
                if (!senselock.FindKey())
                {
                    MessageBox.Show("未找到加密锁,请插入加密锁后，再进行操作。");
                    Application.Exit();
                }
            }
        }
        public void isKeySecOK()
        {
            if (teasec.isKeySecOK() == false)
                if (senselock.isKeySecOK() == false)
                {
                    MessageBox.Show("加密锁不是正版，请更换加密锁！");
                    Application.Exit();
                }
        }
    }
}
