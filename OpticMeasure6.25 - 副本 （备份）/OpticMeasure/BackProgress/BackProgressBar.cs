using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using UserProgressBar;

namespace UserProgressBar
{
    public class BackProgressBar
    {
        private Form frm = null; // new Form();
        private ProgressBarUI ui = null; //new ProgressBarUI();
        public BackgroundWorker backWorker = null;
        //private delegate bool FunctionRun();
        //private FunctionRun backFunc = null;
        public delegate void backFunction();
        private backFunction backFunc= null;
 
        public BackProgressBar(backFunction _back)
        {
            frm = new Form();
            ui = new ProgressBarUI();

            frm.ClientSize = new System.Drawing.Size(511, 71);
            frm.ControlBox = false;
            frm.Name = "ProgressBar";
            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            frm.Text = "²Ù×÷½ø¶È";
            frm.TopMost = true;

            ui.Width = frm.ClientSize.Width;
            frm.Controls.Add(ui);

            backFunc = new backFunction(_back);
            backWorker = new BackgroundWorker();
            InitializeBackgoundWorker();
        }
        ~BackProgressBar()
        {
            if (!ui.IsDisposed)
                ui.Dispose();
            if (!frm.IsDisposed)
                frm.Dispose();
            if (backWorker != null)
                backWorker.Dispose();
        }
        public void RunBackWorker(bool isSubBar)
        {
            frm.Show();
            ui.SetTimerEnable(isSubBar);
            backWorker.RunWorkerAsync();
        }
        public void ReportProgress(int percentComplete)
        {
            backWorker.ReportProgress(percentComplete);
        }
        public void ReportProgress(int Current, int Total)
        {
            int percentComplete = (int)((float)Current / (float)Total * 100);
            backWorker.ReportProgress(percentComplete);
        }
        private void InitializeBackgoundWorker()
        {
            backWorker.WorkerReportsProgress = true;
            backWorker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
             backFunc();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ui.Dispose();
            frm.Close();
            frm.Dispose();
         }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ui.SetProgress(e.ProgressPercentage);
        }

    }
}
