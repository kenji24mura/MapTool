using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maptool
{
    public partial class FormPointSearch : Form
    {
        public maptool.Form1 f1;
        public FormPointSearch()
        {
            InitializeComponent();
        }
        private void FormPointSearch_Load(object sender, EventArgs e)
        {
            textZX.Text = "0.0";
            textZY.Text = "0.0";
            textRate.Text = "1.0";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            double lx;
            double ly;
            double rate;

            lx = Double.Parse(textZX.Text);
            ly = Double.Parse(textZY.Text);
            rate = Double.Parse(textRate.Text);

            f1.CallDrawCanvas2(lx, ly,rate);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            string point = Clipboard.GetText();

            string[] param = point.Split(',');

            if (param.Length>=2)
            {
                textZX.Text = param[0];
                textZY.Text = param[1];

            }

        }
    }
}
