using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDXCom
{
    class DrawMDX
    {
        string DATA_PATH = System.Configuration.ConfigurationManager.AppSettings["DATA_PATH"];
        string BASE_AREA = System.Configuration.ConfigurationManager.AppSettings["BASE_AREA"];

        public string MDX_PATH;
        //シンボル定義テーブル

        List<SymbolDefTable> ssd = new List<SymbolDefTable>();
        List<SymbolFnameTable> spd = new List<SymbolFnameTable>();
        List<PolygonDefTable> psd = new List<PolygonDefTable>();
        List<LineDefTable> lsd = new List<LineDefTable>();
        List<StringDefTable> tsd = new List<StringDefTable>();

        List<SymbolDefTable> ssd_200000 = new List<SymbolDefTable>();
        List<SymbolFnameTable> spd_200000 = new List<SymbolFnameTable>();
        List<PolygonDefTable> psd_200000 = new List<PolygonDefTable>();
        List<LineDefTable> lsd_200000 = new List<LineDefTable>();
        List<StringDefTable> tsd_200000 = new List<StringDefTable>();

        bool checkPolygon = false;
        bool checkLine= true;
        bool checkString= false;
        bool checkSymbol= false;

        public DrawMDX()
        {
            MDX_PATH = DATA_PATH + @"\MDX";

            string deffname = "";

            deffname = MDX_PATH + @"\10000\表示定義\10000\10000_ssd.csv";
            LoadSymbolDef(deffname, ssd);

            deffname = MDX_PATH + @"\10000\表示定義\10000\10000_spd.csv";
            LoadSymbolFname(deffname, spd);

            deffname = MDX_PATH + @"\10000\表示定義\10000\10000_psd.csv";
            LoadPolygonDef(deffname, psd);

            deffname = MDX_PATH + @"\10000\表示定義\10000\10000_lsd.csv";
            LoadLineDef(deffname, lsd);

            deffname = MDX_PATH + @"\10000\表示定義\10000\10000_tsd.csv";
            LoadStringDef(deffname,tsd);


            deffname = MDX_PATH + @"\200000\表示定義\200000\200000_ssd.csv";
            LoadSymbolDef(deffname, ssd_200000);

            deffname = MDX_PATH + @"\200000\表示定義\200000\200000_spd.csv";
            LoadSymbolFname(deffname, spd_200000);

            deffname = MDX_PATH + @"\200000\表示定義\200000\200000_psd.csv";
            LoadPolygonDef(deffname, psd_200000);

            deffname = MDX_PATH + @"\200000\表示定義\200000\200000_lsd.csv";
            LoadLineDef(deffname, lsd_200000);

            deffname = MDX_PATH + @"\200000\表示定義\200000\200000_tsd.csv";
            LoadStringDef(deffname, tsd_200000);
        }

        public int DrawMAPPLE(Graphics g, int maptype,double zx,double zy,Double rate, int offset_w, int offset_h,ref  List<String> meshList)
        {
            g.Clear(Color.White);

            //int ret = 0;

            double lx;
            double ly;
            double m_keido =0;
            double m_ido = 0;

            int offsetx;
            int offsety;

            lx = zx;//m
            ly = zy;//m

            int kijyunkei = 6;

            MapComLib.Convert.gpconv2(lx, ly, kijyunkei, ref m_keido, ref m_ido);

            long pCx = 0;
            long pCy = 0;

            pCx = (long)(m_keido*1000);//中心座標X
            pCy = (long)(m_ido*1000);//中心座標Y

            string meshname = "";
            int sx=0;
            int sy = 0;

            String filename = "";

            switch (maptype){
                case MDX.MDX_10000:

                    for(int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {

                            //
                            //いまのところ不要なメッシュははぶいていないよ
                            //
                            GetMesh(maptype, pCx+(i-1)* MDX.MDX_10000_WIDTH, pCy+(j-1)* MDX.MDX_10000_HEIGHT, rate, ref meshname, ref sx, ref sy);

                            filename = MDX_PATH + "\\10000\\MDX\\" + meshname + ".mdx";
                            offsetx = -offset_w + sx-(int)((double)(i-1)*MDX.MDX_10000_WIDTH/rate);
                            offsety = offset_h + sy- (int)((double)(j-1)*MDX.MDX_10000_HEIGHT / rate);

                            LoadMdx(g, maptype, filename, rate, offsetx, offsety);
                        }
                    }


                    break;
                case MDX.MDX_25000:
                    break;
                case MDX.MDX_50000:
                    break;
                case MDX.MDX_200000:


                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            //
                            //いまのところ不要なメッシュははぶいていないよ
                            //

                            GetMesh(maptype, pCx + (i - 1) * MDX.MDX_200000_WIDTH, pCy + (j - 1) * MDX.MDX_200000_HEIGHT, rate, ref meshname, ref sx, ref sy);

                            filename = MDX_PATH + "\\200000\\MDX\\" + meshname + ".mdx";
                            offsetx = -offset_w + sx - (int)((double)(i - 1) * MDX.MDX_200000_WIDTH / rate);
                            offsety = offset_h + sy - (int)((double)(j - 1) * MDX.MDX_200000_HEIGHT / rate);

                            LoadMdx(g, maptype, filename, rate, offsetx, offsety);
                        }
                    }


                    break;

            }
            return 0;

        }

        public void GetMesh(int maptype,long pCx,long pCy,double rate,ref string meshname,ref int sx,ref int sy)
        {
            double mx1;
            double my1;

            double mx2;
            double my2;

            double mx3;
            double my3;

            double mx4;
            double my4;

            int mesh1;
            int mesh2;
            int mesh3;

            int offsetx;
            int offsety;

            switch (maptype)
            {
                case MDX.MDX_10000:

                    mx1 = (double)pCx / (3600 * 1000);
                    my1 = (double)pCy / (2400 * 1000);

                    mx2 = ((double)pCx % (3600 * 1000)) / (450 * 1000);
                    my2 = ((double)pCy % (2400 * 1000)) / (300 * 1000);

                    mx3 = ((double)pCx % (450 * 1000)) / (225 * 1000);
                    my3 = ((double)pCy % (300 * 1000)) / (150 * 1000);

                    mx4 = (double)pCx % (225 * 1000);
                    my4 = (double)pCy % (150 * 1000);

                    mesh1 = (int)my1 * 100 + (int)mx1 - 100;
                    mesh2 = (int)my2 * 10 + (int)mx2;
                    mesh3 = (int)my3 * 2 + (int)mx3 + 1;

                    sx = (int)(mx4 / rate);
                    sy = (int)(my4 / rate);

                    meshname = mesh1.ToString("D4") + mesh2.ToString("D2") + mesh3;

                    break;
                case MDX.MDX_25000:
                    break;
                case MDX.MDX_50000:
                    break;
                case MDX.MDX_200000:

                    mx1 = (double)pCx / (3600 * 1000);
                    my1 = (double)pCy / (2400 * 1000);

                    mx2 = (double)pCx % (3600 * 1000);
                    my2 = (double)pCy % (2400 * 1000);

                    mesh1 = (int)my1 * 100 + (int)mx1 - 100;

                    sx = (int)(mx2 / rate);
                    sy = (int)(my2 / rate);

                    meshname = mesh1.ToString("D4");

                    break;

            }
        }

        public void LoadMdx(Graphics g, int maptype,String filename, Double val, int offsetx, int offsety)
        {
            if (File.Exists(filename) == false) return;

            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                //ヘッダ
                Byte[] header = new Byte[256];
                header = br.ReadBytes(256);

                int line_start = 0;
                int line_cnt = 0;
                int poly_start = 0;
                int poly_cnt = 0;
                int string_start = 0;
                int string_cnt = 0;
                int symbol_start = 0;
                int symbol_cnt = 0;

                long lonlat1 = 0;//南西端経度
                long lonlat2 = 0;//北東端経度
                long lonlat3 = 0;//南西端緯度
                long lonlat4 = 0;//北東端緯度


                ReadHeader(header, ref line_start, ref line_cnt, ref poly_start, ref poly_cnt, ref string_start, ref string_cnt, ref symbol_start, ref symbol_cnt, ref lonlat1, ref lonlat2, ref lonlat3, ref lonlat4);

//                g.Clear(Color.White);

                if (checkPolygon == true)
                {
                    ReadPolygon(g, maptype,fs, br, poly_start, poly_cnt, lonlat1, lonlat2, lonlat3, lonlat4, val, offsetx, offsety);
                }
                if (checkLine == true)
                {
                    ReadLine(g, maptype,fs, br, line_start, line_cnt, lonlat1, lonlat2, lonlat3, lonlat4, val, offsetx, offsety);
                }
                if (checkString == true)
                {
                    ReadString(g, maptype,fs, br, string_start, string_cnt, lonlat1, lonlat2, lonlat3, lonlat4, val, offsetx, offsety);
                }
                if (checkSymbol == true)
                {
                    ReadSymbol(g, maptype,fs, br, symbol_start, symbol_cnt, lonlat1, lonlat2, lonlat3, lonlat4, val, offsetx, offsety);
                }

                //DrawView(0, 0);

                fs.Close();
                br.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }

        }
        public void ReadHeader(Byte[] header, ref int line_start, ref int line_cnt, ref int poly_start, ref int poly_cnt, ref int string_start, ref int string_cnt, ref int symbol_start, ref int symbol_cnt, ref long lonlat1, ref long lonlat2, ref long lonlat3, ref long lonlat4)
        {
            int offset = 0;

            //listBox1.Items.Clear();

            //ファイルフォーマットバージョン char[8] 8 MDX0100 "MDXvvrr" vv：バージョン、rr：リビジョン(ver1.00ならば"MDX0100")
            Byte[] version = new Byte[8];
            Array.Copy(header, offset, version, 0, 8);
            string str_version = System.Text.Encoding.GetEncoding("shift_jis").GetString(version);

            //MsgOut1("ファイルフォーマットバージョン:" + str_version);

            offset += 8;
            //            測地系コード short 2 1,2 1:旧日本測地系（Tokyo Datum）、2:世界測地系（JGD2000） １）
            short kei = BitConverter.ToInt16(header, offset);
            //MsgOut1("測地系コード:" + kei);
            offset += 2;
            //(予備コード)short 2 ※未整備
            offset += 2;
            //基準データ縮尺 long 4 基準縮尺の分母（分子は1とする）
            long scale = BitConverter.ToInt32(header, offset);
            //MsgOut1("基準データ縮尺:" + scale);
            offset += 4;
            //メッシュ番号 char[16] 16 当該縮尺管理上のメッシュ番号
            Byte[] meshno = new Byte[16];
            Array.Copy(header, offset, meshno, 0, 16);
            string str_meshno = System.Text.Encoding.GetEncoding("shift_jis").GetString(meshno);
            //MsgOut1("メッシュ番号:" + str_meshno);
            offset += 16;

            //図郭情報－南西端経度 long 4 単位[1 / 1000秒]
            lonlat1 = BitConverter.ToInt32(header, offset);
            //MsgOut1("南西端経度:" + lonlat1);
            offset += 4;
            //図郭情報－北東端経度 long 4 単位[1 / 1000秒]/
            lonlat2 = BitConverter.ToInt32(header, offset);
            //MsgOut1("北東端経度:" + lonlat2);
            offset += 4;
            //図郭情報－南西端緯度 long 4 単位[1 / 1000秒]
            lonlat3 = BitConverter.ToInt32(header, offset);
            //MsgOut1("南西端緯度:" + lonlat3);
            offset += 4;
            //図郭情報－北東端緯度 long 4 単位[1 / 1000秒]
            lonlat4 = BitConverter.ToInt32(header, offset);
            //MsgOut1("北東端緯度:" + lonlat4);
            offset += 4;
            //最終更新日 char[16] 16 "yyyymmdd" yyyy: 西暦年、mm：月、dd: 日
            Byte[] update = new Byte[16];
            Array.Copy(header, offset, update, 0, 16);
            string str_update = System.Text.Encoding.GetEncoding("shift_jis").GetString(update);
            //MsgOut1("メッシュ番号:" + str_update);
            offset += 16;
            //  リリース識別情報 char[16] 16 ”rryymms” rr: リリース、yy: 奥付年、mm: 奥付月、s: 管理番号
            Byte[] release = new Byte[16];
            Array.Copy(header, offset, release, 0, 16);
            string str_release = System.Text.Encoding.GetEncoding("shift_jis").GetString(release);
            //MsgOut1("メッシュ番号:" + str_release);
            offset += 16;
            //    (予備) - 48 ※未整備
            offset += 48;
            //  線レコード開始位置 unsigned long 4 線レコードへのファイル先頭からのオフセット
            line_start = BitConverter.ToInt32(header, offset);
            //MsgOut1("線レコード開始位置:" + line_start);
            offset += 4;
            // 線レコードの数 unsigned long 4
            line_cnt = BitConverter.ToInt32(header, offset);
            //MsgOut1("線レコードの数:" + line_cnt);
            offset += 4;
            //ポリゴンレコード開始位置 unsigned long 4 ポリゴンレコードへのファイル先頭からのオフセット
            poly_start = BitConverter.ToInt32(header, offset);
            //MsgOut1("ポリゴンレコード開始位置:" + poly_start);
            offset += 4;
            //ポリゴンレコードの数 unsigned long 4
            poly_cnt = BitConverter.ToInt32(header, offset);
            //MsgOut1("ポリゴンレコードの数:" + poly_cnt);
            offset += 4;
            //シンボルレコード開始位置 unsigned long 4 シンボルレコードへのファイル先頭からのオフセット
            symbol_start = BitConverter.ToInt32(header, offset);
            //MsgOut1("シンボルレコード開始位置:" + symbol_start);
            offset += 4;
            //シンボルレコードの数 unsigned long 4
            symbol_cnt = BitConverter.ToInt32(header, offset);
            //MsgOut1("シンボルレコードの数:" + symbol_cnt);
            offset += 4;
            //注記レコード開始位置 unsigned long 4 注記レコードへのファイル先頭からのオフセット
            string_start = BitConverter.ToInt32(header, offset);
            //MsgOut1("注記レコード開始位置:" + string_start);
            offset += 4;
            //注記レコードの数 unsigned long 4
            string_cnt = BitConverter.ToInt32(header, offset);
            //MsgOut1("注記レコードの数:" + string_cnt);
            offset += 4;
            //グループ線開始位置 unsigned long 4 グループ情報レコードへのファイル先頭からのオフセット
            long grpline_start = BitConverter.ToInt32(header, offset);
            //MsgOut1("グループ線開始位置:" + grpline_start);
            offset += 4;
            //グループ線の数 unsigned long 4
            long grpline_cnt = BitConverter.ToInt32(header, offset);
            //MsgOut1("グループ線の数:" + grpline_cnt);
            offset += 4;
            //くくり属性付きポリゴンレコード開始位置 unsigned long 4 くくり属性付きポリゴンレコードへのファイル先頭からのオフセット
            long mergeattrpoly_start = BitConverter.ToInt32(header, offset);
            //MsgOut1("くくり属性付きポリゴンレコード開始位置:" + mergeattrpoly_start);
            offset += 4;
            //くくり属性付きポリゴンレコードの数 unsigned long 4
            long mergeattrpoly_cnt = BitConverter.ToInt32(header, offset);
            //MsgOut1("くくり属性付きポリゴンレコードの数:" + mergeattrpoly_cnt);
            offset += 4;
            //(予備) - 80 ※未整備

        }

        public void ReadLine(Graphics g, int maptype,FileStream fs, BinaryReader br, int line_start, int line_cnt, long lonlat1, long lonlat2, long lonlat3, long lonlat4, Double val, int offsetx, int offsety)
        {
            //線データの先頭へ
            fs.Seek(line_start, SeekOrigin.Begin);

            for (int i = 0; i < line_cnt; i++)
            {
                //MsgOut1("----線データ" + (i + 1) + "件目----");
                //
                //           オブジェクトコード short 2 40～44,140 40:その他の線、41:道路、42:鉄道、43:行政界、44:等高線、140:建物形状
                short objcode = br.ReadInt16();
                //MsgOut1("オブジェクトコード:" + objcode);
                //識別番号 unsigned long 4 右欄参照
                //オブジェクト固有の識別番号
                //※(オブジェクトコード)×10,000,000＋(管理番号)
                long obj_number = br.ReadInt32();
                //MsgOut1("オブジェクト固有の識別番号:" + obj_number);

                //図式分類コード char[8] 8 図式(描画特性)を分類するコード
                Byte[] Kind1 = new Byte[8];
                Kind1 = br.ReadBytes(8);
                string str_Kind1 = System.Text.Encoding.GetEncoding("shift_jis").GetString(Kind1);
                //MsgOut1("図式分類コード:" + str_Kind1);

                //属性分類コード char[8] 8 ※未整備
                Byte[] Kind2 = new Byte[8];
                Kind2 = br.ReadBytes(8);
                string str_Kind2 = System.Text.Encoding.GetEncoding("shift_jis").GetString(Kind2);
                //MsgOut1("属性分類コード:" + str_Kind2);

                //エレメントラベル char[16] 16 属性データにも持つ地物固有のID（属性情報を持たない場合: NULL）１）
                Byte[] el_label = new Byte[16];
                el_label = br.ReadBytes(16);
                string str_el_label = System.Text.Encoding.GetEncoding("shift_jis").GetString(el_label);
                //MsgOut1("エレメントラベル:" + str_el_label);

                //描画順 short 2 1～255 描画順序を表す。数字が小さい方を後から描画
                short draw_seq = br.ReadInt16();
                //MsgOut1("描画順:" + draw_seq);

                //状態種別コード short 2 0,1 0:通常、1:非表示(隠線)
                short status_code = br.ReadInt16();
                //MsgOut1("状態種別コード:" + status_code);

                //H値 long 4
                long h_value = br.ReadInt32();
                //[41:道路]道路幅員(幅員区分における代表値)[1 / 10m]
                //[44:等高線]標高値[m] ※等高線の標高値はここでのみ持ち、形状点座標の標高値は0
                //※MAPPLE10000の等高線のH値は全て0
                //MsgOut1("H値:" + h_value);

                //形状点総数 long 4 以下に続く形状点の総数n
                long points = br.ReadInt32();
                //MsgOut1("形状点総数:" + points);


                Point[] point_buf = new Point[points];
                //Pen w_pe = new Pen(Color.Green, 1);

                for (int j = 0; j < points; j++)
                {
                    //形状点座標1 long[3] 12 1番目形状点座標(経度[1 / 1000秒], 緯度[1 / 1000秒], 標高値[1 / 10m]) ※標高値0
                    long zx = br.ReadInt32();
                    //MsgOut1("経度[1 / 1000秒]:" + zx);
                    long zy = br.ReadInt32();
                    //MsgOut1("緯度[1 / 1000秒]:" + zy);
                    long zw = br.ReadInt32();
                    //MsgOut1("標高値[1 / 10m]:" + zw);

                    point_buf[j].X = (int)((double)(zx - lonlat1) / val) - offsetx;
                    point_buf[j].Y = offsety - (int)((double)(zy - lonlat3) / val);
                }

                Color w_linecolor = Color.Black;
                int w_linestyle = 0;
                float w_linewidth = 0;

                GetLineDef(maptype,str_Kind1, ref w_linecolor, ref w_linestyle, ref w_linewidth);
                if (maptype == MDX.MDX_200000)
                {
                    w_linewidth = w_linewidth / 100;
                }


                Pen w_pe = new Pen(w_linecolor, w_linewidth);

                //0 実線
                //1 破線
                //2 点線
                //3 一点鎖線
                //4 二点鎖線
                //5 隠線
                //6 長破線
                //7 長一点鎖線
                //8 長二点
                switch (w_linestyle)
                {
                    case 0:
                        w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        break;
                    case 1:
                        w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        break;
                    case 2:
                        w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        break;
                    case 3:
                        w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                        break;
                    case 4:
                        w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                        break;
                    default:
                        w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        break;

                }

                g.DrawLines(w_pe, point_buf);
            }

            //pictureBox1.Image = canvas;
        }
        public void ReadPolygon(Graphics g, int maptype,FileStream fs, BinaryReader br, int poly_start, int poly_cnt, long lonlat1, long lonlat2, long lonlat3, long lonlat4, Double val, int offsetx, int offsety)
        {
            //ポリゴンデータの先頭へ
            fs.Seek(poly_start, SeekOrigin.Begin);

            for (int i = 0; i < poly_cnt; i++)
            {
                //MsgOut1("----ポリゴンデータ" + (i + 1) + "件目----");
                //
                //           オブジェクトコード short 2 40～44,140 40:その他の線、41:道路、42:鉄道、43:行政界、44:等高線、140:建物形状
                short objcode = br.ReadInt16();
                //MsgOut1("オブジェクトコード:" + objcode);
                //識別番号 unsigned long 4 右欄参照
                //オブジェクト固有の識別番号
                //※(オブジェクトコード)×10,000,000＋(管理番号)
                long obj_number = br.ReadInt32();
                //MsgOut1("オブジェクト固有の識別番号:" + obj_number);

                //図式分類コード char[8] 8 図式(描画特性)を分類するコード
                Byte[] Kind1 = new Byte[8];
                Kind1 = br.ReadBytes(8);
                string str_Kind1 = System.Text.Encoding.GetEncoding("shift_jis").GetString(Kind1);
                //MsgOut1("図式分類コード:" + str_Kind1);

                //属性分類コード char[8] 8 ※未整備
                Byte[] Kind2 = new Byte[8];
                Kind2 = br.ReadBytes(8);
                string str_Kind2 = System.Text.Encoding.GetEncoding("shift_jis").GetString(Kind2);
                //MsgOut1("属性分類コード:" + str_Kind2);

                //エレメントラベル char[16] 16 属性データにも持つ地物固有のID（属性情報を持たない場合: NULL）１）
                Byte[] el_label = new Byte[16];
                el_label = br.ReadBytes(16);
                string str_el_label = System.Text.Encoding.GetEncoding("shift_jis").GetString(el_label);
                //MsgOut1("エレメントラベル:" + str_el_label);

                //描画順 short 2 1～255 描画順序を表す。数字が小さい方を後から描画
                short draw_seq = br.ReadInt16();
                //MsgOut1("描画順:" + draw_seq);

                //状態種別コード short 2 0,1 0:通常、1:非表示(隠線)
                short status_code = br.ReadInt16();
                //MsgOut1("状態種別コード:" + status_code);

                //H値 long 4
                long h_value = br.ReadInt32();
                //[41:道路]道路幅員(幅員区分における代表値)[1 / 10m]
                //[44:等高線]標高値[m] ※等高線の標高値はここでのみ持ち、形状点座標の標高値は0
                //※MAPPLE10000の等高線のH値は全て0
                //MsgOut1("H値:" + h_value);

                //形状点総数 long 4 以下に続く形状点の総数n
                long points = br.ReadInt32();
                //MsgOut1("形状点総数:" + points);


                Point[] point_buf = new Point[points];
                //                Pen w_pe = new Pen(Color.Black, 1);

                for (int j = 0; j < points; j++)
                {
                    //形状点座標1 long[3] 12 1番目形状点座標(経度[1 / 1000秒], 緯度[1 / 1000秒], 標高値[1 / 10m]) ※標高値0
                    long zx = br.ReadInt32();
                    //MsgOut1("経度[1 / 1000秒]:" + zx);
                    long zy = br.ReadInt32();
                    //MsgOut1("緯度[1 / 1000秒]:" + zy);
                    long zw = br.ReadInt32();
                    //MsgOut1("標高値[1 / 10m]:" + zw);

                    point_buf[j].X = (int)((double)(zx - lonlat1) / val) - offsetx;
                    point_buf[j].Y = offsety - (int)((double)(zy - lonlat3) / val);
                }

                Color w_linecolor = Color.Black;
                int w_linestyle = 0;
                float w_linewidth = 0;
                Color w_brcolor = Color.Black;
                int w_brstyle = 0;

                GetPolygonDef(maptype,str_Kind1, ref w_linecolor, ref w_linestyle, ref w_linewidth, ref w_brcolor, ref w_brstyle);

                if (maptype == MDX.MDX_200000)
                {
                    w_linewidth = w_linewidth / 100;
                }


                //0 ソリッド（ベタ）
                //1 塗りなし
                //2 ハッチング・水平
                //3 ハッチング・垂直
                //4 ハッチング・左上がり45度
                //5 ハッチング・右上がり45度
                //6 ハッチング・水平クロス
                //7 ハッチング・45度クロス

                if (w_brstyle == 0)
                {
                    SolidBrush w_br = new SolidBrush(w_brcolor);
                        g.FillPolygon(w_br, point_buf);
                }


                if (w_linewidth > 0)
                {
                    Pen w_pe = new Pen(w_linecolor, w_linewidth);

                    //0 実線
                    //1 破線
                    //2 点線
                    //3 一点鎖線
                    //4 二点鎖線
                    //5 隠線
                    //6 長破線
                    //7 長一点鎖線
                    //8 長二点
                    switch (w_linestyle)
                    {
                        case 0:
                            w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            break;
                        case 1:
                            w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            break;
                        case 2:
                            w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                            break;
                        case 3:
                            w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                            break;
                        case 4:
                            w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                            break;
                        default:
                            w_pe.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            break;

                    }

                    g.DrawPolygon(w_pe, point_buf);
                }
            }
        }

        public void ReadString(Graphics g, int maptype,FileStream fs, BinaryReader br, int string_start, int string_cnt, long lonlat1, long lonlat2, long lonlat3, long lonlat4, Double val, int offsetx, int offsety)
        {
            //線データの先頭へ
            fs.Seek(string_start, SeekOrigin.Begin);

            //MsgOut2("" + string_cnt);

            for (int i = 0; i < string_cnt; i++)
            {

                //
                //           オブジェクトコード short 2 40～44,140 40:その他の線、41:道路、42:鉄道、43:行政界、44:等高線、140:建物形状
                short objcode = br.ReadInt16();
                //MsgOut1("オブジェクトコード:" + objcode);
                //識別番号 unsigned long 4 右欄参照
                //オブジェクト固有の識別番号
                //※(オブジェクトコード)×10,000,000＋(管理番号)
                long obj_number = br.ReadInt32();
                //MsgOut1("オブジェクト固有の識別番号:" + obj_number);

                //図式分類コード char[8] 8 図式(描画特性)を分類するコード
                Byte[] Kind1 = new Byte[8];
                Kind1 = br.ReadBytes(8);
                string str_Kind1 = System.Text.Encoding.GetEncoding("shift_jis").GetString(Kind1);
                //MsgOut1("図式分類コード:" + str_Kind1);

                //属性分類コード char[8] 8 ※未整備
                Byte[] Kind2 = new Byte[8];
                Kind2 = br.ReadBytes(8);
                string str_Kind2 = System.Text.Encoding.GetEncoding("shift_jis").GetString(Kind2);
                //MsgOut1("属性分類コード:" + str_Kind2);

                //エレメントラベル char[16] 16 属性データにも持つ地物固有のID（属性情報を持たない場合: NULL）１）
                Byte[] el_label = new Byte[16];
                el_label = br.ReadBytes(16);
                string str_el_label = System.Text.Encoding.GetEncoding("shift_jis").GetString(el_label);
                //MsgOut1("エレメントラベル:" + str_el_label);

                //描画順 short 2 1～255 描画順序を表す。数字が小さい方を後から描画
                short draw_seq = br.ReadInt16();
                //MsgOut1("描画順:" + draw_seq);

                //状態種別コード short 2 0,1 0:通常、1:非表示(隠線)
                short status_code = br.ReadInt16();
                //MsgOut1("状態種別コード:" + status_code);

                //H値 long 4
                long h_value = br.ReadInt32();
                //[41:道路]道路幅員(幅員区分における代表値)[1 / 10m]
                //[44:等高線]標高値[m] ※等高線の標高値はここでのみ持ち、形状点座標の標高値は0
                //※MAPPLE10000の等高線のH値は全て0
                //MsgOut1("H値:" + h_value);

                /*
                配置状態コード short 2 1,2,3
                1:注記表示・シンボル非表示
                2:シンボル表示・注記非表示 3:注記・シンボルとも表示
                */
                short statuscode = br.ReadInt16();

                /*
                シンボルX座標 long 4 経度[1 / 1000秒] （シンボル非表示の場合は注記原点と同じ値がセットされる）
                
                シンボルY座標 long 4 緯度[1 / 1000秒] （シンボル非表示の場合は注記原点と同じ値がセットされる）
                
                シンボルZ座標 long 4 ※未整備
                */
                long sym_x = br.ReadInt32();
                long sym_y = br.ReadInt32();
                long sym_z = br.ReadInt32();

                /*
                シンボル配置角度情報 long[2] 8
                配置角度を示すベクトル（X,Y） 緯度経度による正規化直交座標におけるベクトル
                ※ベクトルの向きが(1, 0)のとき角度0

                */
                long sym_range1 = br.ReadInt32();
                long sym_range2 = br.ReadInt32();

                /*
                
                注記原点X座標 long 4 経度[1 / 1000秒]
                
                注記原点Y座標 long 4 緯度[1 / 1000秒]
                
                */

                long str_x = br.ReadInt32();
                long str_y = br.ReadInt32();

                /*
                注記配置角度情報 long[2] 8
                配置角度を示すベクトル（X,Y） 緯度経度による正規化直交座標におけるベクトル
                ※ベクトルの向きが(1, 0)のとき角度0
                */

                long str_range1 = br.ReadInt32();
                long str_range2 = br.ReadInt32();


                /*
                 注記原点位置コード short 2 1～9 別図「★注記原点位置について」参照
                 注記配置方向コード short 2 0,1 0:横組み、1:縦組み
                */
                short pos_code = br.ReadInt16();
                short range_code = br.ReadInt16();


                /*
                文字列 char[160] 160 ASCII,Shift - JIS
                */
                Byte[] byte_string = new Byte[160];
                byte_string = br.ReadBytes(160);
                string str = System.Text.Encoding.GetEncoding("shift_jis").GetString(byte_string);

                // //MsgOut2(str);
                /*
                文字配置制御情報総数 short 2 0～ 以下に続く付加的な文字配置制御情報の総数m
                */
                short str_control_cnt = br.ReadInt16();

                for (int m = 0; m < str_control_cnt; m++)
                {
                    short str_control_pos = br.ReadInt16();
                    short str_control_code = br.ReadInt16();
                }

                short line_str_cnt = br.ReadInt16();

                for (int n = 0; n < line_str_cnt; n++)
                {
                    long line_str_x = br.ReadInt32();
                    long line_str_y = br.ReadInt32();
                    long line_str_range1 = br.ReadInt32();
                    long line_str_range2 = br.ReadInt32();
                }

                double px = (double)(str_x - lonlat1) / val - offsetx;
                double py = offsety - (double)(str_y - lonlat3) / val;

                int w_sym_ptncode = 0;
                double w_sym_size = 0;
                Color w_str_color = Color.Black;
                double w_str_size = 0;
                String w_str_font = "";
                int w_str_style = 0;

                GetStringDef(maptype,str_Kind1, ref w_sym_ptncode, ref w_sym_size, ref w_str_color, ref w_str_size, ref w_str_font, ref w_str_style);

                if (maptype == MDX.MDX_200000)
                {
                    w_str_size = w_str_size / 100;
                }

                try
                {
                    if (w_str_size > 0)
                    {
                        Font w_f = new Font(w_str_font, (float)w_str_size);
                        Brush w_fb = new SolidBrush(w_str_color);
                        g.DrawString(str, w_f, w_fb, (int)px, (int)py);
                    }

                }
                catch (Exception e)
                {
                    //MsgOut2(e.ToString());
                }

            }
        }
        public void ReadSymbol(Graphics g, int maptype,FileStream fs, BinaryReader br, int symbol_start, int symbol_cnt, long lonlat1, long lonlat2, long lonlat3, long lonlat4, Double val, int offsetx, int offsety)
        {
            //線データの先頭へ
            fs.Seek(symbol_start, SeekOrigin.Begin);

            for (int i = 0; i < symbol_cnt; i++)
            {
                //MsgOut1("----注記データ" + (i + 1) + "件目----");
                //
                //           オブジェクトコード short 2 40～44,140 40:その他の線、41:道路、42:鉄道、43:行政界、44:等高線、140:建物形状
                short objcode = br.ReadInt16();
                //MsgOut1("オブジェクトコード:" + objcode);
                //識別番号 unsigned long 4 右欄参照
                //オブジェクト固有の識別番号
                //※(オブジェクトコード)×10,000,000＋(管理番号)
                long obj_number = br.ReadInt32();
                //MsgOut1("オブジェクト固有の識別番号:" + obj_number);

                //図式分類コード char[8] 8 図式(描画特性)を分類するコード
                Byte[] Kind1 = new Byte[8];
                Kind1 = br.ReadBytes(8);
                string str_Kind1 = System.Text.Encoding.GetEncoding("shift_jis").GetString(Kind1);
                //MsgOut1("図式分類コード:" + str_Kind1);

                //属性分類コード char[8] 8 ※未整備
                Byte[] Kind2 = new Byte[8];
                Kind2 = br.ReadBytes(8);
                string str_Kind2 = System.Text.Encoding.GetEncoding("shift_jis").GetString(Kind2);
                //MsgOut1("属性分類コード:" + str_Kind2);

                //エレメントラベル char[16] 16 属性データにも持つ地物固有のID（属性情報を持たない場合: NULL）１）
                Byte[] el_label = new Byte[16];
                el_label = br.ReadBytes(16);
                string str_el_label = System.Text.Encoding.GetEncoding("shift_jis").GetString(el_label);
                //MsgOut1("エレメントラベル:" + str_el_label);

                //描画順 short 2 1～255 描画順序を表す。数字が小さい方を後から描画
                short draw_seq = br.ReadInt16();
                //MsgOut1("描画順:" + draw_seq);

                //状態種別コード short 2 0,1 0:通常、1:非表示(隠線)
                short status_code = br.ReadInt16();
                //MsgOut1("状態種別コード:" + status_code);

                //H値 long 4
                long h_value = br.ReadInt32();
                //[41:道路]道路幅員(幅員区分における代表値)[1 / 10m]
                //[44:等高線]標高値[m] ※等高線の標高値はここでのみ持ち、形状点座標の標高値は0
                //※MAPPLE10000の等高線のH値は全て0
                //MsgOut1("H値:" + h_value);

                /*
                シンボルX座標 long 4 経度[1 / 1000秒] （シンボル非表示の場合は注記原点と同じ値がセットされる）
                
                シンボルY座標 long 4 緯度[1 / 1000秒] （シンボル非表示の場合は注記原点と同じ値がセットされる）
                
                シンボルZ座標 long 4 ※未整備
                */
                long sym_x = br.ReadInt32();
                long sym_y = br.ReadInt32();
                long sym_z = br.ReadInt32();

                /*
                シンボル配置角度情報 long[2] 8
                配置角度を示すベクトル（X,Y） 緯度経度による正規化直交座標におけるベクトル
                ※ベクトルの向きが(1, 0)のとき角度0

                */
                long sym_range1 = br.ReadInt32();
                long sym_range2 = br.ReadInt32();


                double range = Math.Atan((double)sym_range2 / (double)sym_range1);

                double angle = range * (180 / Math.PI);

                // //MsgOut2(""+angle);

                int w_sym_ptncode = 0;
                int w_sym_size = 0;

                GetSymbolDef(maptype,str_Kind1, ref w_sym_ptncode, ref w_sym_size);

                string w_sym_fname = "";

                GetSymbolFname(maptype,w_sym_ptncode, ref w_sym_fname);


                //486332349　123355777

                string symbol_path = "";
                switch (maptype)
                {
                    case MDX.MDX_10000:
                        symbol_path = MDX_PATH + @"\10000\表示定義\symbol\mdx_bmp";
                        break;
                    case MDX.MDX_25000:
                        break;
                    case MDX.MDX_50000:
                        break;
                    case MDX.MDX_200000:
                        symbol_path = MDX_PATH + @"\200000\表示定義\symbol\mdx_bmp";
                        break;
                }

                Bitmap wIcon = new Bitmap(symbol_path + "\\" + w_sym_fname + ".bmp");

                double px = (double)(sym_x - lonlat1) / val - offsetx;
                double py = offsety - (double)(sym_y - lonlat3) / val;

                Point p = new Point((int)px + 16, (int)py + 16);

                DrawIcon(g, wIcon, angle, p, wIcon.Width/2, wIcon.Height/2);
                wIcon.Dispose();

            }

        }

        public void DrawIcon(Graphics g, System.Drawing.Image image, double deg, Point p, int width,int height)
        {
            // 度をラジアン値に変換する
            double rad = Math.PI * deg / 180.0;

            Rectangle rt = new Rectangle();
            rt.X = p.X;
            rt.Y = p.Y;
            rt.Width = width;
            rt.Height = height;


            Point[] destinationPoints = {

new Point(p.X - (int)(width * Math.Sin(rad)), p.Y-(int)(height * Math.Cos(rad))),   // destination for upper-left point of
new Point(p.X + (int)(width * Math.Cos(rad)) - (int)(height * Math.Sin(rad)), p.Y - (int)(height * Math.Cos(rad)) - (int)(width * Math.Sin(rad))),  // destination for upper-right point of
new Point(p.X, p.Y)};  // destination for lower-left point of

            g.DrawImage(image, destinationPoints);
        }

        //--------------------定義の取得---------------------------
        public void GetLineDef(int maptype,string kind, ref Color line_color, ref int line_style, ref float line_width)
        {
            List<LineDefTable> list = new List<LineDefTable>();
            switch (maptype)
            {
                case MDX.MDX_10000:
                    list = lsd;
                    break;
                case MDX.MDX_25000:
                    break;
                case MDX.MDX_50000:
                    break;
                case MDX.MDX_200000:
                    list = lsd_200000;
                    break;
            }

            kind = kind.TrimEnd('\0');
            var findVal = list.Find(x => x.kind == kind);
            line_color = findVal.line_color;
            line_style = findVal.line_style;
            line_width = findVal.line_width;
        }
        public void GetPolygonDef(int maptype, string kind, ref Color line_color, ref int line_style, ref float line_width, ref Color br_color, ref int br_style)
        {
            List<PolygonDefTable> list = new List<PolygonDefTable>();

            switch (maptype)
            {
                case MDX.MDX_10000:
                    list = psd;
                    break;
                case MDX.MDX_25000:
                    break;
                case MDX.MDX_50000:
                    break;
                case MDX.MDX_200000:
                    list = psd_200000;
                    break;
            }

            kind = kind.TrimEnd('\0');
            var findVal = list.Find(x => x.kind == kind);

            kind = findVal.kind;
            line_color = findVal.line_color;
            line_style = findVal.line_style;
            line_width = findVal.line_width;
            br_color = findVal.br_color;
            br_style = findVal.br_style;


        }
        public void GetSymbolDef(int maptype, string kind, ref int sym_ptncode, ref int sym_size)
        {
            List<SymbolDefTable> list = new List<SymbolDefTable>();
            switch (maptype)
            {
                case MDX.MDX_10000:
                    list = ssd;
                    break;
                case MDX.MDX_25000:
                    break;
                case MDX.MDX_50000:
                    break;
                case MDX.MDX_200000:
                    list = ssd_200000;
                    break;
            }
            kind = kind.TrimEnd('\0');

            var findVal = list.Find(x => x.kind == kind);
            sym_ptncode = findVal.sym_ptncode;
            sym_size = findVal.sym_size;

        }
        public void GetStringDef(int maptype, string kind, ref int sym_ptncode, ref double sym_size, ref Color str_color, ref double str_size, ref string str_font, ref int str_style)
        {
            List<StringDefTable> list = new List<StringDefTable>();
            switch (maptype)
            {
                case MDX.MDX_10000:
                    list = tsd;
                    break;
                case MDX.MDX_25000:
                    break;
                case MDX.MDX_50000:
                    break;
                case MDX.MDX_200000:
                    list = tsd_200000;
                    break;
            }

            kind = kind.TrimEnd('\0');
            var findVal = list.Find(x => x.kind == kind);

            sym_ptncode = findVal.sym_ptncode;
            sym_size = findVal.sym_size;
            str_color = findVal.str_color;
            str_size = findVal.sym_size;
            str_style = findVal.str_style;

        }

        public void GetSymbolFname(int maptype, int sym_ptncode, ref string sym_fname)
        {
            List<SymbolFnameTable> list = new List<SymbolFnameTable>();
            switch (maptype)
            {
                case MDX.MDX_10000:
                    list = spd;
                    break;
                case MDX.MDX_25000:
                    break;
                case MDX.MDX_50000:
                    break;
                case MDX.MDX_200000:
                    list = spd_200000;
                    break;
            }
            var findVal = list.Find(x => x.sym_ptncode == sym_ptncode);
            sym_fname = findVal.sym_fname;
        }


        //--------------定義をロードする-------------------------------------------
        public void LoadStringDef(string deffile,List<StringDefTable> list)
        {

            //String file = MDX_PATH + @"\10000\表示定義\10000\10000_tsd.csv";
            using (StreamReader sr = new StreamReader(deffile, Encoding.GetEncoding("SHIFT_JIS")))
            {
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        string[] param = line.Split(',');

                        string kind = param[0];
                        int sym_ptncode = 0;
                        double sym_size = 0;
                        Color str_color = Color.Black;
                        double str_size = 0;
                        string str_font = "";
                        int str_style = 0;

                        try
                        {
                            //                                図式分類コード テキスト 図式分類定義ファイルに定義されているコード

                            //                                描画順（デフォルト値） 整数 0固定

                            int seq = Int32.Parse(param[1]);

                            //                                [シンボル]表示縮尺範囲最小値 整数
                            //1以上
                            //またはNULL
                            //表示縮尺範囲における縮尺分母の最小値（最も拡大した場合の縮尺）
                            //NULLの時下限なし
                            int sym_min_scle = 0;
                            if (param[2].Length > 0)
                            {
                                sym_min_scle = Int32.Parse(param[2]);
                            }
                            //[シンボル]表示縮尺範囲最大値 整数
                            //1以上
                            //またはNULL
                            //表示縮尺範囲における縮尺分母の最大値（最も縮小した場合の縮尺）
                            //NULLの時上限なし
                            int sym_max_scle = 0;
                            if (param[3].Length > 0)
                            {
                                sym_max_scle = Int32.Parse(param[3]);
                            }
                            //[シンボル]シンボルパターンコード 整数 シンボルパターンのコード（シンボルパターン定義を参照）
                            if (param[4].Length > 0)
                            {
                                sym_ptncode = Int32.Parse(param[4]);
                            }

                            //[シンボル] シンボルサイズ 長整数 シンボルの大きさ（単位：基準縮尺における測地m）

                            if (param[5].Length > 0)
                            {
                                sym_size = Double.Parse(param[5]);
                            }
                            //[注記] 表示縮尺範囲最小値 整数
                            //1以上
                            //またはNULL
                            //表示縮尺範囲における縮尺分母の最小値（最も拡大した場合の縮尺）
                            //NULLの時下限なし
                            int min_scale = 0;
                            if (param[6].Length > 0)
                            {
                                min_scale = Int32.Parse(param[6]);
                            }
                            //[注記]表示縮尺範囲最大値 整数
                            //1以上
                            //またはNULL
                            //表示縮尺範囲における縮尺分母の最大値（最も縮小した場合の縮尺）
                            //NULLの時上限なし
                            int max_scale = 0;
                            if (param[7].Length > 0)
                            {
                                max_scale = Int32.Parse(param[7]);
                            }
                            //[注記]色（Ｒ） 整数 0～255 文字の色（赤）
                            //[注記] 色（Ｇ） 整数 0～255 文字の色（緑）
                            //[注記] 色（Ｂ） 整数 0～255 文字の色（青）

                            int R = 0;
                            int G = 0;
                            int B = 0;
                            if (param[8].Length > 0)
                            {
                                R = Int32.Parse(param[8]);
                            }
                            if (param[9].Length > 0)
                            {
                                G = Int32.Parse(param[9]);
                            }
                            if (param[10].Length > 0)
                            {
                                B = Int32.Parse(param[10]);
                            }

                            str_color = Color.FromArgb(R, G, B);

                            //[注記] 文字サイズ 長整数 文字の高さ（単位：測地m）

                            if (param[11].Length > 0)
                            {
                                str_size = Double.Parse(param[11]);
                            }
                            //[注記] 文字幅変形率 整数 文字の横方向変形率（単位：%）、100の時は正体
                            if (param[12].Length > 0)
                            {
                                int str_rate1 = Int32.Parse(param[12]);
                            }
                            //[注記]文字送り率 整数 文字送り率（単位：%）、100の時は文字間が0となる状態
                            int str_rate2 = Int32.Parse(param[13]);
                            //[注記]フォント テキスト フォント名称（Windowsフォント名）
                            if (param[14].Length > 0)
                            {
                                str_font = param[14];
                            }
                            //[注記] 文字スタイルコード 整数 文字のスタイル（補足を参照）
                            if (param[15].Length > 0)
                            {
                                str_style = Int32.Parse(param[15]);
                            }
                        }
                        catch (Exception)
                        {

                        }

                        list.Add(new StringDefTable() { kind = kind, sym_ptncode = sym_ptncode, sym_size = sym_size, str_color = str_color, str_size = str_size, str_font = str_font, str_style = str_style });


                    }
                    sr.Close();
                }

            }
        }

        public void LoadSymbolDef(string deffile, List<SymbolDefTable> list)
        {

            //String file = MDX_PATH + @"\10000\表示定義\10000\10000_ssd.csv";
            using (StreamReader sr = new StreamReader(deffile, Encoding.GetEncoding("SHIFT_JIS")))
            {
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        string[] param = line.Split(',');

                        try
                        {

                            //図式分類コード テキスト 図式分類定義ファイルに定義されているコード
                            //描画順（デフォルト値） 整数 0固定
                            //表示縮尺範囲最小値 整数
                            //1以上
                            //                                またはNULL
                            //                                表示縮尺範囲における縮尺分母の最小値（最も拡大した場合の縮尺）
                            //NULLの時下限なし
                            //表示縮尺範囲最大値 整数
                            //1以上
                            //またはNULL
                            //表示縮尺範囲における縮尺分母の最大値（最も縮小した場合の縮尺）
                            //NULLの時上限なし
                            //シンボルパターンコード 整数 シンボルパターンのコード（シンボルパターン定義を参照）
                            //シンボルサイズ 長整数 シンボルの大きさ（単位：基準縮尺における測地m）


                            string kind = param[0];

                            int min_scale = Int32.Parse(param[2]);
                            int max_scale = Int32.Parse(param[3]);
                            int sym_ptncode = Int32.Parse(param[4]);
                            int sym_size = Int32.Parse(param[5]);

                            list.Add(new SymbolDefTable() { kind = kind, sym_ptncode = sym_ptncode, sym_size = sym_size });

                        }
                        catch (Exception)
                        {

                        }

                    }
                    sr.Close();
                }

            }
        }
        public void LoadSymbolFname(string deffile, List<SymbolFnameTable> list)
        {

            //String file = MDX_PATH + @"\10000\表示定義\10000\10000_spd.csv";
            using (StreamReader sr = new StreamReader(deffile, Encoding.GetEncoding("SHIFT_JIS")))
            {
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        string[] param = line.Split(',');

                        try
                        {
                            int sym_ptncode = Int32.Parse(param[0]);
                            string sym_fname = param[1];

                            list.Add(new SymbolFnameTable() { sym_ptncode = sym_ptncode, sym_fname = sym_fname });

                        }
                        catch (Exception)
                        {
                        }
                    }
                    sr.Close();
                }

            }
        }
        public void LoadLineDef(string deffile, List<LineDefTable> list)
        {

            //String file = MDX_PATH + @"\10000\表示定義\10000\10000_lsd.csv";
            using (StreamReader sr = new StreamReader(deffile, Encoding.GetEncoding("SHIFT_JIS")))
            {
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        string[] param = line.Split(',');

                        string kind = param[0];
                        Color line_color = Color.Black;
                        int line_style = 0;
                        float line_width = 0;

                        try
                        {
                            int R = Int32.Parse(param[5]);
                            int G = Int32.Parse(param[6]);
                            int B = Int32.Parse(param[7]);
                            line_color = Color.FromArgb(R, G, B);
                            line_style = Int32.Parse(param[8]);
                            line_width = float.Parse(param[9]);
                        }
                        catch (Exception)
                        {

                        }
                        list.Add(new LineDefTable() { kind = kind, line_color = line_color, line_style = line_style, line_width = line_width });

                    }
                    sr.Close();
                }

            }
        }

        public void LoadPolygonDef(string deffile, List<PolygonDefTable> list)
        {

            //kind = kind.TrimEnd('\0');
            //String file = MDX_PATH + @"\10000\表示定義\10000\10000_psd.csv";
            using (StreamReader sr = new StreamReader(deffile, Encoding.GetEncoding("SHIFT_JIS")))
            {
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        string[] param = line.Split(',');

                        //if (param[0].IndexOf(kind) == 0)
                        //{

                        string kind = param[0];

                        Color line_color = Color.Black;
                        int line_style = 0;
                        float line_width = 0;
                        Color br_color = Color.Black;
                        int br_style = 0;

                        try
                        {
                            int R = Int32.Parse(param[5]);
                            int G = Int32.Parse(param[6]);
                            int B = Int32.Parse(param[7]);
                            line_color = Color.FromArgb(R, G, B);
                            line_style = Int32.Parse(param[8]);
                            line_width = float.Parse(param[9]);
                        }
                        catch (Exception)
                        {

                        }


                        try
                        {
                            int R = Int32.Parse(param[10]);
                            int G = Int32.Parse(param[11]);
                            int B = Int32.Parse(param[12]);

                            br_color = Color.FromArgb(R, G, B);
                            br_style = Int32.Parse(param[13]);
                        }
                        catch (Exception)
                        {

                        }
                        list.Add(new PolygonDefTable() { kind = kind, line_color = line_color, line_style = line_style, line_width = line_width, br_color = br_color, br_style = br_style });
                        //break;
                        //}

                    }
                    sr.Close();
                }

            }
        }

    }
}
