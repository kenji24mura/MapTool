using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZMDCom;

namespace maptool
{
    public partial class FormPointInfo : Form
    {
        public PointInfo pf;
        
        public FormPointInfo()
        {
            InitializeComponent();
        }

        private void FormPointInfo_Load(object sender, EventArgs e)
        {
            Left = (Screen.GetBounds(this).Width - Width) / 2;
            Top = (Screen.GetBounds(this).Height - Height) / 2;

            listBox1.Items.Clear();
            listBox2.Items.Clear();

            listBox1.Items.Add("正規化座標X:"+pf.lx);
            listBox1.Items.Add("正規化座標Y:" + pf.ly);
            /*
            listBox1.Items.Add("メッシュ座標X:" + pf.meshX);
            listBox1.Items.Add("メッシュ座標Y:" + pf.meshY);
            listBox1.Items.Add("メッシュファイル名:" + pf.meshfname);
            listBox1.Items.Add("（行政界ポリゴン）住所:" + pf.area_addrname);//
            listBox1.Items.Add("（行政界ポリゴン）拡張市町村:" + pf.addr1);
            listBox1.Items.Add("（行政界ポリゴン）大字:" + pf.addr2);
            listBox1.Items.Add("（行政界ポリゴン）丁目:" + pf.addr3);
            listBox1.Items.Add("（行政界ポリゴン）街区:" + pf.addr4);
            */
            listBox1.Items.Add("住所:" + pf.addrname);//
            listBox1.Items.Add("拡張市町村:" + pf.zcity);//拡張市町村
            listBox1.Items.Add("大字:" + pf.ztown1);//大字
            listBox1.Items.Add("丁目:" + pf.ztown2);//丁目
            listBox1.Items.Add("街区:" + pf.zgcode);//街区
            listBox1.Items.Add("地番:" + pf.number);
            listBox1.Items.Add("名称:" + pf.name);

            if (pf.ext_info != null)
            {
                for (int i = 0; i < pf.ext_info.Count(); i++)
                {
                    listBox2.Items.Add(pf.ext_info[i]);
                }
            }

        }

    private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(pf.lx+","+pf.ly);
        }
    }
}
