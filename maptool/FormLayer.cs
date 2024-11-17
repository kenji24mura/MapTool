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
    public partial class FormLayer : Form
    {
        public Form1 f1;
        public Boolean[] layer_disp = new Boolean[7];
        public FormLayer()
        {
            InitializeComponent();
        }

        private void FormLayer_Load(object sender, EventArgs e)
        {
            Left = (Screen.GetBounds(this).Width - Width) / 2;
            Top = (Screen.GetBounds(this).Height - Height) / 2;

            checkBox1.Checked = layer_disp[0];
            checkBox2.Checked = layer_disp[1];
            checkBox3.Checked = layer_disp[2];
            checkBox4.Checked = layer_disp[3];
            checkBox5.Checked = layer_disp[4];
            checkBox6.Checked = layer_disp[5];
            checkBox7.Checked = layer_disp[6];
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
//            Boolean[] layer_disp=new Boolean[7];

            layer_disp[0] = checkBox1.Checked;
            layer_disp[1] = checkBox2.Checked;
            layer_disp[2] = checkBox3.Checked;
            layer_disp[3] = checkBox4.Checked;
            layer_disp[4] = checkBox5.Checked;
            layer_disp[5] = checkBox6.Checked;
            layer_disp[6] = checkBox7.Checked;

            f1.SetLayerCheck(layer_disp);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
