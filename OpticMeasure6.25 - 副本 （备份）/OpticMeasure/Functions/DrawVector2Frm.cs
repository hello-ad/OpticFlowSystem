using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZH2DCL.ZtClass;
using OpticMeasure;
using OpticMeasure.OpticFlowFunc;

namespace ZHCLNEW.DrawCloud
{
    public partial class DrawVector2Frm : Form
    {
        private GetOpticflowResultOfSingleReferencePicture compVector2;
        private OpticMeasureClass currentEdit;

        public DrawVector2Frm(OpticMeasureClass _opmc, GetOpticflowResultOfSingleReferencePicture _singleOpticFlow)
        {
            InitializeComponent();
            currentEdit = _opmc;
            compVector2 = _singleOpticFlow;
        }

        private void DrawVector2Frm_Load(object sender, EventArgs e)
        {
            drawVector21.Init(currentEdit, compVector2);
            
        }
    }
}
