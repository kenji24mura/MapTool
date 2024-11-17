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
    public partial class FormIconList : Form
    {
        public Form1 f1;
        public String IconName;
        public int SelectedIdx = 0;

        public FormIconList()
        {
            InitializeComponent();
        }


        private void FormIconList_Load(object sender, EventArgs e)
        {
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 48;

            foreach (string item in f1.ICON_TBL)
            {
                listBox1.Items.Add(item);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult= DialogResult.Cancel;
            Close();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
              if (e.Index == -1) return;

            String path =f1.pngpath+"\\"+ f1.ICON_TBL[e.Index];

            Pen p = new Pen(Color.AliceBlue,5);
            SolidBrush b = new SolidBrush(Color.AliceBlue);
            if (e.Index == SelectedIdx)
            {
                e.Graphics.FillRectangle(b, e.Bounds.X, e.Bounds.Y,listBox1.Width-2,48);
            }
            e.Graphics.DrawImage(Image.FromFile(path), e.Bounds.X, e.Bounds.Y);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIdx = listBox1.SelectedIndex;
            IconName = f1.ICON_TBL[SelectedIdx];

            //            listBox1.Items.Clear();
            //           foreach (string item in f1.ICON_TBL)
            //          {
            //            listBox1.Items.Add(item);//
            //      }
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
