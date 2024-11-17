using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maptool
{
    class Address
    {
        public maptool.Form1 f1;

        public void Search(int idx,ComboBox cb,int Prif,int City,int Town1,int Town2)
        {
            String db = f1.datapath + "\\code\\FIIM_ADDR.mdb";

            try
            {
                OleDbConnection conn = new OleDbConnection();
                OleDbCommand comm = new OleDbCommand();

                conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + db;

                // 接続します。
                conn.Open();

                // SELECT文を設定します。

                if(idx == 0)
                {
                    comm.CommandText = "SELECT * FROM 住所テーブル WHERE [都道府県]>" + Prif + " AND [市町村]=" + City + " AND [大字]=" + Town1 + " AND [丁目]=" + Town2;
                }
                if (idx == 1)
                {
                    comm.CommandText = "SELECT * FROM 住所テーブル WHERE [都道府県]=" + Prif + " AND [市町村]>" + City + " AND [大字]=" + Town1 + " AND [丁目]=" + Town2;
                }
                if (idx == 2)
                {
                    comm.CommandText = "SELECT * FROM 住所テーブル WHERE [都道府県]=" + Prif + " AND [市町村]=" + City + " AND [大字]>" + Town1 + " AND [丁目]=" + Town2;
                }
                if (idx == 3)
                {
                    comm.CommandText = "SELECT * FROM 住所テーブル WHERE [都道府県]=" + Prif + " AND [市町村]=" + City + " AND [大字]=" + Town1 + " AND [丁目]>" + Town2;
                }

                comm.Connection = conn;
                OleDbDataReader reader = comm.ExecuteReader();

                int reccnt = 0;

                List<ItemSet> src = new List<ItemSet>();

                // 結果を表示します。
                while (reader.Read())
                {
                    short prif = (short)reader.GetValue(0);
                    short city = (short)reader.GetValue(1);
                    short town1 = (short)reader.GetValue(2);
                    short town2 = (short)reader.GetValue(3);
                    String name = (string)reader.GetValue(4);
                    String kana = "";

                    int zx = (int)reader.GetValue(12);
                    int zy = (int)reader.GetValue(13);

                    if (idx == 0)
                    {
                        src.Add(new ItemSet(prif, name));/// 1つでItem１つ分となる
                    }
                    if (idx == 1)
                    {
                        src.Add(new ItemSet(city, name));/// 1つでItem１つ分となる
                    }
                    if (idx == 2)
                    {
                        src.Add(new ItemSet(town1, name));/// 1つでItem１つ分となる
                    }
                    if (idx == 3)
                    {
                        src.Add(new ItemSet(town2, name));/// 1つでItem１つ分となる
                    }

                }

                // ComboBoxに表示と値をセット
                cb.DataSource = src;
                cb.DisplayMember = "ItemDisp";
                cb.ValueMember = "ItemValue";

                conn.Close();
                //msg = "Import終了";
                //                Invoke(new UpdateMessage(MsgOut), msg);
            }
            catch (Exception ex)
            {
            }

        }
        public void GetLonLat(int Prif, int City, int Town1, int Town2, String number,ref double lon, ref double lat,ref double lx,ref double ly)
        {
            String db = f1.datapath + "\\code\\FIIM_ADDR.mdb";

            int mode = 0;
            if (number.Length > 0)
            {
                mode = 1;
            }
                lon = 0.0;
            lat = 0.0;

            try
            {
                OleDbConnection conn = new OleDbConnection();
                OleDbCommand comm = new OleDbCommand();

                conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + db; // MDB名など

                // 接続します。
                conn.Open();

                // SELECT文を設定します。
                
                if(mode == 0)
                {
                    comm.CommandText = "SELECT * FROM 住所テーブル WHERE [都道府県]=" + Prif + " AND [市町村]=" + City + " AND [大字]=" + Town1 + " AND [丁目]=" + Town2;
                }
                else
                {
                    comm.CommandText = "SELECT * FROM 地番テーブル WHERE [都道府県]=" + Prif + " AND [市町村]=" + City + " AND [大字]=" + Town1 + " AND [丁目]=" + Town2+ " AND [名称]='" + number+"'";
                }

                comm.Connection = conn;
                OleDbDataReader reader = comm.ExecuteReader();

                int reccnt = 0;

                //List<ItemSet> src = new List<ItemSet>();

                // 結果を表示します。
                while (reader.Read())
                {
                    short prif = 0;
                    short city = 0;
                    short town1 = 0;
                    short town2 = 0;
                    String name = "";
                    String kana = "";

                    int zx = 0;
                    int zy = 0;
                    if (mode== 0)
                    {

                        zx = (int)reader.GetValue(12);
                        zy = (int)reader.GetValue(13);
                    }
                    else
                    {

                        zx = (int)reader.GetValue(13);
                        zy = (int)reader.GetValue(14);
                    }

                    //MapComLib.Convert cs = new MapComLib.Convert();

                    long pCx = 0;
                    long pCy = 0;
                    double m_keido = (double)zx / 1000;
                    double m_ido = (double)zy / 1000;
                    int kijyunkei = 6;
                    lx = 0.0;
                    ly = 0.0;

                    //日本測地系→世界測地系
                    MapComLib.Convert.ConvJ2W(zx, zy, ref pCx, ref pCy);

                    //経緯度→正規化座標
                    MapComLib.Convert.gpconv(m_keido, m_ido, kijyunkei, ref lx, ref ly);


                    zx = (int)(lx * 1000.0);// m -> mm
                    zy = (int)(ly * 1000.0);// m -> mm

                    lon = (double)pCx / (3600 * 1000);
                    lat = (double)pCy / (3600 * 1000);

                }

                conn.Close();
            }
            catch (Exception ex)
            {
            }
        }
        public void GetAddrName(int Prif, int City, int Town1, int Town2,ref String addrname)
        {
            String db = f1.datapath + "\\code\\FIIM_ADDR.mdb";
            try
            {
                OleDbConnection conn = new OleDbConnection();
                OleDbCommand comm = new OleDbCommand();

                conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + db; // MDB名など

                // 接続します。
                conn.Open();

                // SELECT文を設定します。

                comm.CommandText = "SELECT * FROM 住所テーブル WHERE [都道府県]=" + Prif + " AND [市町村]=" + City + " AND [大字]=" + Town1 + " AND [丁目]=" + Town2;

                comm.Connection = conn;
                OleDbDataReader reader = comm.ExecuteReader();

                if (reader.Read())
                {
                    String name = (String)reader["名称"];

                    addrname = name;

                }

                conn.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
    public class ItemSet
    {
        // DisplayMemberとValueMemberにはプロパティで指定する仕組み
        public String ItemDisp { get; set; }
        public int ItemValue { get; set; }

        // プロパティをコンストラクタでセット
        public ItemSet(int v, String s)
        {
            ItemDisp = s;
            ItemValue = v;
        }
    }

}
