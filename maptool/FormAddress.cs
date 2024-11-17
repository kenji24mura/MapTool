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
    public partial class FormAddress : Form
    {
        public maptool.Form1 f1;
        //
        //Addressクラスのインスタンス
        //
        Address address = new Address();

        public FormAddress()
        {
            InitializeComponent();
        }

        private void FormAddress_Load(object sender, EventArgs e)
        {
            Left = (Screen.GetBounds(this).Width - Width) / 2;
            Top = (Screen.GetBounds(this).Height - Height) / 2;

            address.f1 = f1;
            button1.Text = "1";
            button2.Text = "2";
            button3.Text = "3";
            button4.Text = "4";
            button5.Text = "5";
            button6.Text = "6";
            button7.Text = "7";
            button8.Text = "8";
            button9.Text = "9";
            button10.Text = "0";
            button11.Text = "-";
            button12.Text = "BS";
            button13.Text = "CLS";

        }

        private void btnAddrSearch_Click(object sender, EventArgs e)
        {
            SearchAddr();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrif_Click(object sender, EventArgs e)
        {
            int prif = 0;
            int city = 0;
            int town1 = 0;
            int town2 = 0;

            int idx = 0;

            comboBox2.DataSource = null;
            comboBox3.DataSource = null;
            comboBox4.DataSource = null;

            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            address.Search(idx, comboBox1, prif, city, town1, town2);
        }

        private void btnCity_Click(object sender, EventArgs e)
        {
            int prif = 0;
            int city = 0;
            int town1 = 0;
            int town2 = 0;

            if (comboBox1.SelectedIndex >= 0)
            {
                ItemSet tmp1 = ((ItemSet)comboBox1.SelectedItem);
                prif = tmp1.ItemValue;
            }

            int idx = 1;

            comboBox3.DataSource = null;
            comboBox4.DataSource = null;

            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            address.Search(idx, comboBox2, prif, city, town1, town2);
        }

        private void btnTown1_Click(object sender, EventArgs e)
        {
            int prif = 0;
            int city = 0;
            int town1 = 0;
            int town2 = 0;

            if (comboBox1.SelectedIndex >= 0)
            {
                ItemSet tmp1 = ((ItemSet)comboBox1.SelectedItem);
                prif = tmp1.ItemValue;
            }
            if (comboBox2.SelectedIndex >= 0)
            {
                ItemSet tmp2 = ((ItemSet)comboBox2.SelectedItem);
                city = tmp2.ItemValue;
            }

            int idx = 2;

            comboBox4.DataSource = null;
            comboBox4.Items.Clear();

            address.Search(idx, comboBox3, prif, city, town1, town2);

        }

        private void btnTown2_Click(object sender, EventArgs e)
        {
            int prif = 0;
            int city = 0;
            int town1 = 0;
            int town2 = 0;

            if (comboBox1.SelectedIndex >= 0)
            {
                ItemSet tmp1 = ((ItemSet)comboBox1.SelectedItem);
                prif = tmp1.ItemValue;
            }
            if (comboBox2.SelectedIndex >= 0)
            {
                ItemSet tmp2 = ((ItemSet)comboBox2.SelectedItem);
                city = tmp2.ItemValue;
            }
            if (comboBox3.SelectedIndex >= 0)
            {
                ItemSet tmp3 = ((ItemSet)comboBox3.SelectedItem);
                town1 = tmp3.ItemValue;
            }

            int idx = 3;

            address.Search(idx, comboBox4, prif, city, town1, town2);

        }

        private void SearchAddr()
        {
            int prif = 0;
            int city = 0;
            int town1 = 0;
            int town2 = 0;
            String number = textNumber.Text;

            if (comboBox1.SelectedIndex >= 0)
            {
                ItemSet tmp1 = ((ItemSet)comboBox1.SelectedItem);
                prif = tmp1.ItemValue;
            }
            if (comboBox2.SelectedIndex >= 0)
            {
                ItemSet tmp2 = ((ItemSet)comboBox2.SelectedItem);
                city = tmp2.ItemValue;
            }
            if (comboBox3.SelectedIndex >= 0)
            {
                ItemSet tmp3 = ((ItemSet)comboBox3.SelectedItem);
                town1 = tmp3.ItemValue;
            }
            if (comboBox4.SelectedIndex >= 0)
            {
                ItemSet tmp4 = ((ItemSet)comboBox4.SelectedItem);
                town2 = tmp4.ItemValue;
            }

            double lon = 0.0;
            double lat = 0.0;
            double lx = 0.0;
            double ly = 0.0;

            address.GetLonLat(prif, city, town1, town2, number, ref lon, ref lat, ref lx, ref ly);

            //MsgOut("lon=" + lon + ",lat=" + lat);
            //MsgOut("lx=" + lx + ",ly=" + ly);

            f1.CallDrawCanvas(lx, ly);

           // lx_b = lx;
           // ly_b = ly;

        //    start.X = 0;
        //    start.Y = 0;


          //  DrawCanvas(lx, ly);


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.DataSource = null;
            comboBox3.DataSource = null;
            comboBox4.DataSource = null;

            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            textNumber.Text = "";

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //       comboBox2.DataSource = null;
            comboBox3.DataSource = null;
            comboBox4.DataSource = null;

            //     comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            textNumber.Text = "";

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //            comboBox2.DataSource = null;
            //            comboBox3.DataSource = null;
            comboBox4.DataSource = null;

            //           comboBox2.Items.Clear();
            //         comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            textNumber.Text = "";

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textNumber.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b.Text == "CLS")
            {
                textNumber.Text = "";
            }
            else
            {
                if (b.Text == "BS")
                {
                    int len = textNumber.Text.Length;
                    if (len > 0)
                    {
                        textNumber.Text = textNumber.Text.Substring(0, len - 1);
                    }
                }
                else
                {
                    textNumber.Text = textNumber.Text + b.Text;
                }
            }
        }
    }
}
