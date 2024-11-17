using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maptool
{ 
    public partial class DataList : Form
    {
        public maptool.Form1 f1;
        string ulaypath;

        String[] LayTypeName = { "線", "ポリゴン", "シンボル", "文字", "自由線" }; 
        private ColumnHeader[] columnData = new ColumnHeader[20];
        public DataList()
        {
            InitializeComponent();
        }

        private void DataList_Load(object sender, EventArgs e)
        {
            Left = (Screen.GetBounds(this).Width - Width) / 2;
            Top = (Screen.GetBounds(this).Height - Height) / 2;

            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            //listView1.Sorting = SortOrder.None;
            listView1.View = View.Details;

            // 列（コラム）ヘッダの作成
            for (int i = 0; i < 20; i++)
            {
                columnData[i] = new ColumnHeader();
            }
            columnData[0].Text = "登録年月日";
            columnData[0].Width = 250;
            columnData[1].Text = "種類";
            columnData[1].Width = 150;
            columnData[2].Text = "住所";
            columnData[2].Width = 300;
            columnData[3].Text = "座標X";
            columnData[3].Width = 200;
            columnData[4].Text = "座標Y";
            columnData[4].Width = 200;
            columnData[5].Text = "メッシュ";
            columnData[5].Width = 100;
            columnData[6].Text = "";
            columnData[6].Width = 0;

            ColumnHeader[] colHeaderRegValue =
              { this.columnData[0],
                this.columnData[1],
                this.columnData[2],
                this.columnData[3],
                this.columnData[4],
                this.columnData[5],
                this.columnData[6]};
            listView1.Columns.AddRange(colHeaderRegValue);

            listView1.HideSelection = false;

            ulaypath = f1.datapath + "\\ULAY";

            DirectoryInfo di = new DirectoryInfo(ulaypath);

            // ディレクトリ直下のすべてのファイル一覧を取得する
            FileInfo[] fiAlls = di.GetFiles();
            foreach (FileInfo f in fiAlls)
            {
                string file = ulaypath +"\\" +f.Name;
                SetData(file);
            }

        }

        private void SetData(String file)
        {
            string[] item = new string[20];

//            ulaypath = f1.datapath + "\\ULAY";

//            string file = ulaypath + "\\layer.txt";

            if (File.Exists(file) == false)
            {
                return;
            }

            int linecnt = 0;

            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("SHIFT_JIS")))
            {
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        linecnt++;

                        string line = sr.ReadLine();

                        string[] param = line.Split(',');


                        int laytype = Int32.Parse(param[0]);

                        int point_start = 1;

                        int cnt = Int32.Parse(param[point_start]);
                        Point[] points = new Point[cnt];

                        for (int i = 0; i < cnt; i++)
                        {
                            double mx = double.Parse(param[i * 2 + point_start + 1]) / 1000;
                            double my = double.Parse(param[i * 2 + point_start + 2]) / 1000;
                        }

                        int attr_start = 2 + cnt * 2;

                        String StringText = param[attr_start];
                        String IconName = param[attr_start + 1];
                        int LineWidth = Int32.Parse(param[attr_start + 2]);
                        String LineCol = param[attr_start + 3];
                        int LineType = Int32.Parse(param[attr_start + 4]);
                        int PolyBrushType = Int32.Parse(param[attr_start + 5]);
                        String PolyBrush = param[attr_start + 6];
                        String PolyBrush2 = param[attr_start + 7];
                        int PolyHatchStyle = Int32.Parse(param[attr_start + 8]);
                        int StringSize = Int32.Parse(param[attr_start + 9]);
                        String StringBrush = param[attr_start + 10];
                        int StringType = Int32.Parse(param[attr_start + 11]);
                        int StringAngle = Int32.Parse(param[attr_start + 12]);
                        int StringFontType = Int32.Parse(param[attr_start + 13]);
                        Double SymbolAngle = Double.Parse(param[attr_start + 14]);

                        String meshname = param[attr_start + 15];
                        String addrname = param[attr_start + 16];
                        String entrydate = param[attr_start + 17];

                        item[0] = entrydate;
                        item[1] = LayTypeName[Int32.Parse(param[0]) - 1];
                        item[2] = addrname;
                        item[3] = param[2];
                        item[4] = param[3];
                        item[5] = meshname;

                        listView1.Items.Add(new ListViewItem(item));



                    }
                    sr.Close();
                }
            }




        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count < 1)//
            {
                return;
                //何らかの処理
            }

            ListViewItem itemx = listView1.SelectedItems[0];

            int zx = Int32.Parse(itemx.SubItems[3].Text);
            int zy = Int32.Parse(itemx.SubItems[4].Text);

            double lx = (double)zx / 1000;
            double ly = (double)zy / 1000;

            f1.CallDrawCanvas(lx, ly);
        }
    }
}
