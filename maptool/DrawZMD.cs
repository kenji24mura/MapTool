using com;
using COM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ZMDCom
{
    class DrawZMD
    {
        public maptool.Form1 f1;

        string DATA_PATH = System.Configuration.ConfigurationManager.AppSettings["DATA_PATH"];

        string mappath;
        string symbolpath;

        //リソース
        Pen[] pe = new Pen[16];
        Font[] f = new Font[16];
        SolidBrush[] b = new SolidBrush[16];

        Image[] SymbolImage = new Image[96];

        int mask = 0;
        Point Offset;

        int maxlay = 0;

        public bool[] layer_disp = new bool[16];

        //Thread Parameters
        Graphics t_g;
        double t_lx;
        double t_ly;
        double t_rate;
        int t_offset_w;
        int t_offset_h;

        public PointInfo pInfo = new PointInfo();

        public DrawZMD()
        {
            mappath = DATA_PATH + "\\ZENRIN";
            symbolpath = mappath + "\\ZmdIcon";


            pe[0] = new Pen(Color.Black, 1);
            pe[1] = new Pen(Color.Gray, 1);
            pe[2] = new Pen(Color.Green, 1);
            pe[3] = new Pen(Color.Magenta, 1);
            pe[4] = new Pen(Color.Red, 1);
            pe[5] = new Pen(Color.Orange, 1);
            pe[6] = new Pen(Color.Pink, 1);

            f[0] = new Font("ＭＳ Ｐゴシック", 10);
            //f[1] = new Font("@ＭＳ Ｐゴシック", 10);

            b[0] = new SolidBrush(Color.Black);
            b[1] = new SolidBrush(Color.LightGray);
            b[2] = new SolidBrush(Color.DarkGray);
            b[3] = new SolidBrush(Color.SkyBlue);

            for (int i = 0; i < SymbolImage.Length; i++)
            {
                string fname = symbolpath + "\\" + "zmd_" + i.ToString() + ".ico";
                string default_fname = symbolpath + "\\" + "zmd_default.ico";

                if (File.Exists(fname) == true)
                {
                    SymbolImage[i] = Image.FromFile(fname);
                }
                else
                {
                    SymbolImage[i] = Image.FromFile(default_fname);
                }
            }

        }


        public void SetThreadParams(Graphics g, double lx, double ly, double rate, int offset_w, int offset_h)
        {
            t_g = g;
            t_lx = lx;
            t_ly = ly;
            t_rate = rate;
            t_offset_w = offset_w;
            t_offset_h = offset_h;

        }
        public void DrawThread()
        {
            //Console.WriteLine("Thread");
            List<String> meshList = new List<String>();

            DrawZenrin(t_g, t_lx, t_ly, t_rate, t_offset_w, t_offset_h, ref meshList);
        }
        public void DrawZenrin(Graphics g, double lx, double ly, double rate, int offset_w, int offset_h,ref  List<String> meshList)
        {
            //List<String> meshList = new List<String>();

            long Lx, Ly;
            long nZx, nZy;

            int dirX = 0;
            int dirY = 0;

            long MBase_X, MBase_Y;

            string strtbl1 = "0123456789abcdef";
            string strtbl2 = "abcdefghijklmnop";

            Lx = (long)(lx * 100.0) - zmd_def.ZMDORGX;
            Ly = (long)(ly * 100.0) - zmd_def.ZMDORGY;

            // Lx, Ly は１０センチ単位なので
            Lx = (Lx + 5) / 10;
            Ly = (Ly + 5) / 10;

            //描画範囲のrectangle

            Rectangle canvas_r = new Rectangle();

            canvas_r.X = (int)Lx - (int)(offset_w * rate);
            canvas_r.Y = (int)Ly - (int)(offset_h * rate);
            canvas_r.Width = (int)(offset_w * 2 * rate);
            canvas_r.Height = (int)(offset_h * 2 * rate);


            int meshMax = 3;

            if (rate > 100)
            {
                meshMax = 99;
            }

            for (int m = 0; m < meshMax; m++)
            {
                for (int n = 0; n < meshMax; n++)
                {
                    //DateTime dt = DateTime.Now;

                    nZx = Lx + zmd_def.MAP_W * (m - (meshMax - 1) / 2);
                    nZy = Ly + zmd_def.MAP_H * (n - (meshMax - 1) / 2);


                    // センターメッシュ取得
                    long MeshX = nZx / zmd_def.MAP_W;
                    if (nZx >= 0) MeshX++;

                    long MeshY = nZy / zmd_def.MAP_H;
                    if (nZy >= 0) MeshY++;

                    //メッシュからファイル名を算出
                    string WkBuf = MeshX.ToString("x4") + MeshY.ToString("x4");

                    string FileName = "";
                    string DirNameX = "";
                    string DirNameY = "";


                    for (int i = 0; i < WkBuf.Length; i++)
                    {

                        for (int j = 0; j < strtbl1.Length; j++)
                        {
                            if (WkBuf.Substring(i, 1) == strtbl1.Substring(j, 1))
                            {
                                FileName += strtbl2.Substring(j, 1);
                            }
                        }
                    }

                    dirX = (int)(MeshX / 5);
                    WkBuf = dirX.ToString("x4");
                    for (int i = 0; i < WkBuf.Length; i++)
                    {

                        for (int j = 0; j < strtbl1.Length; j++)
                        {
                            if (WkBuf.Substring(i, 1) == strtbl1.Substring(j, 1))
                            {
                                DirNameX += strtbl2.Substring(j, 1);
                            }
                        }
                    }

                    dirY = (int)(MeshY / 5);
                    WkBuf = dirY.ToString("x4");
                    for (int i = 0; i < WkBuf.Length; i++)
                    {

                        for (int j = 0; j < strtbl1.Length; j++)
                        {
                            if (WkBuf.Substring(i, 1) == strtbl1.Substring(j, 1))
                            {
                                DirNameY += strtbl2.Substring(j, 1);
                            }
                        }
                    }

                    //
                    //Console.WriteLine("m=" + m.ToString() + ",n=" + n.ToString() + ",FileName=" + FileName);


                    //MsgOut("DirNameX=" + DirNameX);
                    //MsgOut("DirNameY=" + DirNameY);


                    MBase_X = (MeshX - 1) * zmd_def.MAP_W;
                    MBase_Y = (MeshY - 1) * zmd_def.MAP_H;

                    //
                    //メッシュの矩形
                    //
                    Rectangle mesh_r = new Rectangle();

                    mesh_r.X = (int)MBase_X;
                    mesh_r.Y = (int)MBase_Y;
                    mesh_r.Width = (int)zmd_def.MAP_W;
                    mesh_r.Height = (int)zmd_def.MAP_H;

                    //
                    //描画範囲にない場合は描画しない
                    //
                    Rectangle intersectRect = Rectangle.Intersect(canvas_r, mesh_r);
                    if (intersectRect.IsEmpty == true)
                    {
                        MsgOut("メッシュ[" + FileName + "]は対象外");

                    }
                    else
                    {

                        meshList.Add(FileName);


                        MsgOut("メッシュ[" + FileName + "]は対象");


                        Offset.X = (int)(nZx - MBase_X - zmd_def.MAP_W * (m - (meshMax - 1) / 2));
                        Offset.Y = (int)(nZy - MBase_Y - zmd_def.MAP_H * (n - (meshMax - 1) / 2));

                        //MsgOut("Offset.X=" + Offset.X.ToString());
                        //MsgOut("Offset.Y=" + Offset.Y.ToString());

                        string ZmdFile = "";


                        //該当メッシュファイルを検索
                        string[] files = Directory.GetFiles(
                            mappath, FileName + ".baf", SearchOption.AllDirectories);

                        foreach (string s in files)
                        {
                            //Console.WriteLine(s);
                            ZmdFile = s;
                        }

                        if (ZmdFile.Length > 1)
                        {
                            string DirName = System.IO.Path.GetDirectoryName(ZmdFile);

                            //MsgOut(ZmdFile);

                            //上位ディレクトリを検索
                            DirectoryInfo diParent = Directory.GetParent(DirName);
                            DirectoryInfo diParent2 = Directory.GetParent(diParent.FullName);
                            DirectoryInfo diParent3 = Directory.GetParent(diParent2.FullName);
                            DirectoryInfo diParent4 = Directory.GetParent(diParent3.FullName);


                            string PasswordFile = diParent4.FullName + "\\PASSWORD.DAT";

                            //                        g.Clear(Color.White);

                            DrawZMDData(g, ZmdFile, PasswordFile, rate, offset_w, offset_h);
                        }
                        else
                        {
                            //MsgOut("Not Founf[" + FileName + "]");
                        }
                    }
                }
            }

            f1.PaintView();
        }

        public int DrawZMDData(Graphics g, string ZmdFile, string PasswordFile, double rate, int offset_w, int offset_h)
        {

            //if(mask != 0)
            //{
            mask = GetMask(PasswordFile);
            //}

            ReadZMD(g, ZmdFile, rate, offset_w, offset_h);

            return 0;
        }

        public int GetMask(string PasswordFile)
        {
            int m_mask = 0;

            System.UInt32[] data = new System.UInt32[18];

            char[] passwd = new char[256];

            string str_passwd = "";


            StreamReader sr = new StreamReader(PasswordFile, Encoding.GetEncoding("Shift_JIS"));

            while (sr.Peek() != -1)
            {
                string header = "PassWord1=";

                string line = sr.ReadLine();

                if (line.IndexOf(header) == 0)
                {
                    str_passwd = line.Substring(header.Length);
                }
            }
            sr.Close();

            //MsgOut(str_passwd);

            passwd = str_passwd.ToCharArray();

            int j = 0;
            for (int i = 0; i < 18; i++)
            {

                if (passwd[i] >= 'a' && passwd[i] <= 'z')
                {
                    data[j] = (System.UInt32)(122 - passwd[i]);
                }
                else if (passwd[i] == '#')
                {
                    data[j] = 26;
                }
                else if (passwd[i] >= '0' && passwd[i] <= '9')
                {
                    data[j] = (System.UInt32)(passwd[i] - 21);
                }
                else if (passwd[i] == '%')
                {
                    data[j] = 37;
                }
                if (passwd[i] >= 'A' && passwd[i] <= 'Z')
                {
                    data[j] = (System.UInt32)(passwd[i] - 27);
                }
                j++;
            }
            m_mask = (int)(data[7] << 6 | ((data[8] << 2) >> 2));

            return m_mask;
        }

        public void ReadZMD(Graphics g, string zmdfile, double rate, int offset_w, int offset_h)
        {
            string fname = Path.GetFileNameWithoutExtension(zmdfile);

            if (File.Exists(zmdfile) == false) return;


            FileStream fs = new FileStream(zmdfile, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);


            FRM_HEAD frm_head = new FRM_HEAD();
            frm_head.pickind = new PicKind_Code();
            frm_head.date = new Date();
            frm_head.zno = new ZNO();
            FRM_BRK frm_brk = new FRM_BRK();
            FRM_INF frm_inf = new FRM_INF();
            FRM_LAY frm_lay = new FRM_LAY();

            char[] s_fname = new char[256];

            s_fname = fname.ToCharArray();

            byte[] s1 = new byte[4];
            byte[] s2 = new byte[4];
            byte[] s3 = new byte[4];
            byte[] s4 = new byte[4];

            System.UInt32[] brkptr = new System.UInt32[99];
            System.UInt32[] brksize = new System.UInt32[99];


            int valx = 0;
            int valy = 0;



            for (int j = 0; j < 4; j++)
            {
                if (s_fname[j] >= 'A' && s_fname[j] <= 'P')
                {
                    valx += (int)(s_fname[j] - 'A') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                else
                {
                    valx += (int)(s_fname[j] - 'a') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                if (s_fname[j + 4] >= 'A' && s_fname[j + 4] <= 'P')
                {
                    valy += (int)(s_fname[j + 4] - 'A') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                else
                {
                    valy += (int)(s_fname[j + 4] - 'a') * (int)Math.Pow((double)16, (double)(3 - j));
                }
            }

            s1 = BitConverter.GetBytes(valx);
            s2 = BitConverter.GetBytes(valy);

            //
            //ヘッダ部
            //


            frm_head.pickind.pic1 = br.ReadByte();
            frm_head.pickind.pic2 = br.ReadByte();
            frm_head.pickind.pic3 = br.ReadByte();
            frm_head.pickind.pic4 = br.ReadByte();

            frm_head.date.y = br.ReadUInt16();
            frm_head.date.m = br.ReadByte();
            frm_head.date.d = br.ReadByte();

            frm_head.zno.x = br.ReadUInt16();
            frm_head.zno.y = br.ReadUInt16();

            frm_head.maxlay = br.ReadUInt16();           // 最大レイヤ番号
            frm_head.rsv = br.ReadUInt16();              // リザーブ
            frm_head.brk_ptr = br.ReadUInt32();          // ブロック管理部アドレス
            frm_head.brk_cnt = br.ReadUInt32();          // ブロック情報部レコード数
            frm_head.inf_ptr = br.ReadUInt32();          // 属性対応管理部アドレス
            frm_head.inf_cnt = br.ReadUInt32();          // 属性対応情報部レコード数
            frm_head.brk_str = br.ReadUInt32();          // ブロック部開始アドレス
            frm_head.ref_ptr = br.ReadUInt32();          // 関連図管理部アドレス
            frm_head.ref_cnt = br.ReadUInt32();          // 関連図情報部レコード数
            frm_head.rsv_ptr = br.ReadUInt32();          // 予約領域部アドレス
            frm_head.file_sz = br.ReadUInt32();          // ファイル容量

            maxlay = frm_head.maxlay;

            //
            //ブロック管理部
            //

            fs.Seek(frm_head.brk_ptr, SeekOrigin.Begin);

            for (int i = 0; i < frm_head.brk_cnt; i++)
            {
                frm_brk.brk_ptr = br.ReadUInt32();
                frm_brk.brk_sz = br.ReadUInt32();

                s3 = BitConverter.GetBytes(frm_brk.brk_ptr);


                s3[0] = (byte)(s3[0] ^ s1[0]);
                s3[1] = (byte)(s3[1] ^ s1[1]);
                s3[2] = (byte)(s3[2] ^ s2[0]);
                s3[3] = (byte)(s3[3] ^ s2[1]);
                //マスクとのXOR
                s3[1] = (byte)(s3[1] ^ mask);
                s3[2] = (byte)(s3[2] ^ mask);


                frm_brk.brk_ptr = BitConverter.ToUInt32(s3, 0);

                brkptr[i] = frm_brk.brk_ptr;
                brksize[i] = frm_brk.brk_sz;
            }


            //
            //属性対応情報管理部
            //

            fs.Seek(frm_head.inf_ptr, SeekOrigin.Begin);


            for (int i = 0; i < frm_head.inf_cnt; i++)
            {
                frm_inf.inf_num = br.ReadUInt32();          // 図内属性番号
                frm_inf.inf_z_brk = br.ReadUInt16();        // 図形形状所属ブロック
                frm_inf.inf_m_brk = br.ReadUInt16();        // 文字形状所属ブロック
                frm_inf.inf_z_off = br.ReadUInt32();        // 図形形状ブロック内オフセット
                frm_inf.inf_m_off = br.ReadUInt32();        // 文字形状ブロック内オフセット
                frm_inf.inf_recnum = br.ReadUInt32();       // 属性情報部レコード番号
                frm_inf.rsv = br.ReadUInt32();			   // リザーブ
            }

            //
            //ブロック部
            //

            for (int i = 0; i < frm_head.brk_cnt; i++)
            {
                if (brksize[i] > 0) {
                    fs.Seek(brkptr[i], SeekOrigin.Begin);

                    FrmSub(g, i, fs, br, brksize[i], rate, offset_w, offset_h);

                    //f1.PaintView();
                }
            }

            //
            //関連情報管理部
            //

            //
            //予約領域部
            //

            fs.Close();
            br.Close();

        }

        public void FrmSub(Graphics g, int brkno, FileStream fs, BinaryReader br, System.UInt32 brksize, double rate, int offset_w, int offset_h)
        {

            FRM_FRMCOM frm_com = new FRM_FRMCOM();

            //形状情報部

            Boolean EOF = false;
            byte[] buf = new byte[2048];


            UInt32[] lay_off = new UInt32[maxlay];
            UInt32[] lay_size = new UInt32[maxlay];
            UInt32[] lay_frm_off = new UInt32[maxlay];


            int flg = 0;
            int newbufcnt = 0;

            //
            //ブロックの先頭ポインタ
            //
            int brkstart = (int)fs.Seek(0, SeekOrigin.Current);


            //if ((UInt32)brkstart == 0xffffffff) return;

            int size = 0;

            //レイヤ情報部
            for (int j = 0; j < maxlay; j++)
            {

                //try
                //{
                lay_off[j] = br.ReadUInt32();
                lay_size[j] = br.ReadUInt32();
                lay_frm_off[j] = br.ReadUInt32();
                size += 12;
                //}
                //catch (Exception e)
                // {

                // }


                //Console.WriteLine(j.ToString() + "," + lay_off[j].ToString() + "," + lay_size[j].ToString() + "," + lay_frm_off[j].ToString());
            }


            while (EOF != true)
            {
                int cur = (int)fs.Seek(0, SeekOrigin.Current) - brkstart;

                //
                //レイヤ番号判定
                //
                int layno = -1;

                for (int k = 0; k < maxlay; k++)
                {
                    if (lay_frm_off[k] != 0xffffffff && cur >= lay_off[k] && cur < lay_off[k] + lay_size[k])
                    {
                        layno = k + 1;
                        break;
                    }
                }

                //Console.WriteLine(layno);

                frm_com.frmcom_recnum = br.ReadUInt32();
                frm_com.frmcom_size = br.ReadUInt16();
                frm_com.frmcom_type = br.ReadByte();
                frm_com.frmcom_status = br.ReadByte();

                size += 8;

                int type = (int)frm_com.frmcom_type;
                int len = frm_com.frmcom_size - (8);

                buf = br.ReadBytes(len);

                FRM_AREAPOLY frm_areapoly = new FRM_AREAPOLY();
                frm_areapoly.ldxy = new XY();
                frm_areapoly.ruxy = new XY();
                FRM_STRINF frm_strinf = new FRM_STRINF();
                frm_strinf.ldxy = new XY();
                frm_strinf.ruxy = new XY();
                FRM_STRLINE frm_strline = new FRM_STRLINE();
                frm_strline.kxy = new XY();
                FRM_SYMBOL frm_symbol = new FRM_SYMBOL();
                frm_symbol.ldxy = new XY();
                frm_symbol.ruxy = new XY();
                frm_symbol.kxy = new XY();
                FRM_LINES frm_lines = new FRM_LINES();
                frm_lines.ldxy = new XY();
                frm_lines.ruxy = new XY();

                FRM_POLYGON frm_polygon = new FRM_POLYGON();
                frm_polygon.ldxy = new XY();
                frm_polygon.ruxy = new XY();

                FRM_OUTPOLY frm_outpoly = new FRM_OUTPOLY();
                frm_outpoly.ldxy = new XY();
                frm_outpoly.ruxy = new XY();

                FRM_INPOLY frm_inpoly = new FRM_INPOLY();
                FRM_INPOLY frm_areainpoly = new FRM_INPOLY();

                FRM_STRSYM frm_strsym = new FRM_STRSYM();
                frm_strsym.ldxy = new XY();
                frm_strsym.ruxy = new XY();
                frm_strsym.kxy = new XY();


                int offset = 0;

                int wx = 0;
                int wy = 0;

                switch (type)
                {
                    case 1://文字

                        if (layer_disp[type - 1] == false) break;
                        //if (checkBox1.Checked == false) break;

                        if (rate < 10)//rate 10以上は文字処理しない

                        {
                            frm_strinf.ldxy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;
                            frm_strinf.ldxy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;
                            frm_strinf.ruxy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;
                            frm_strinf.ruxy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;
                            frm_strinf.height = BitConverter.ToUInt16(buf, offset);
                            offset += 2;
                            frm_strinf.angle = BitConverter.ToUInt16(buf, offset);
                            offset += 2;

                            frm_strinf.c1 = (byte)BitConverter.ToChar(buf, offset);
                            offset += 1;
                            frm_strinf.c2 = (byte)BitConverter.ToChar(buf, offset);
                            offset += 1;
                            frm_strinf.form = (byte)BitConverter.ToChar(buf, offset);
                            offset += 1;
                            frm_strinf.line = (byte)BitConverter.ToChar(buf, offset);
                            offset += 1;

                            for (int k = 0; k < frm_strinf.line; k++)
                            {
                                frm_strline.kxy.x = BitConverter.ToInt16(buf, offset);
                                offset += 2;
                                frm_strline.kxy.y = BitConverter.ToInt16(buf, offset);
                                offset += 2;
                                frm_strline.width = BitConverter.ToUInt16(buf, offset);
                                offset += 2;
                                frm_strline.height = BitConverter.ToUInt16(buf, offset);
                                offset += 2;
                                frm_strline.cnt = BitConverter.ToUInt16(buf, offset);
                                offset += 2;

                                byte[] strbuf1 = new byte[frm_strline.cnt * 2];
                                Array.Copy(buf, offset, strbuf1, 0, frm_strline.cnt * 2);

                                byte[] strbuf2 = jis2sj.Jis2Sjis(strbuf1);

                                string text = System.Text.Encoding.GetEncoding("shift_jis").GetString(strbuf2);

                                //Console.WriteLine(text);

                                wx = frm_strline.kxy.x - Offset.X;
                                wy = frm_strline.kxy.y - Offset.Y;

                                //                          int str_sx = (int)((double)(frm_strline.kxy.x) / rate);
                                //                          int str_sy = (int)((double)(meshH - (frm_strline.kxy.y)) / rate);
                                int str_sx = (int)((double)wx / rate) + offset_w;
                                int str_sy = (int)((double)(-wy) / rate) + offset_h;



                                int font_size = (int)((double)frm_strline.height / rate);

                                //★計算は暫定

                                Font s_font;

                                SolidBrush s_br;//文字用

                                if(layno >= 96 && layno <= 99)
                                {
                                    font_size = (int)(30 / rate);
                                    s_br = new SolidBrush(Color.Red);
                                }
                                else
                                {
                                    font_size = (int)(20 / rate);
                                    s_br = new SolidBrush(Color.Black);
                                }

                                FontFamily fontFamily = new FontFamily("ＭＳ Ｐゴシック");
                                s_font = new Font(
                                fontFamily,
                                    font_size,
                                   FontStyle.Regular,
                                   GraphicsUnit.Pixel);


                                var format = new StringFormat();
                                format.Alignment = StringAlignment.Near;      // 左右方向は中心寄せ
                                format.LineAlignment = StringAlignment.Near;  // 上下方向は中心寄せ

                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                if (frm_strinf.form == 2)
                                {


                                    format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                                    StringCOM.DrawString(g, text, s_font, s_br, str_sx, str_sy - font_size, -frm_strinf.angle, format);
                                }
                                else
                                {

                                    format.FormatFlags = StringFormatFlags.DirectionVertical;
                                    StringCOM.DrawString(g, text, s_font, s_br, str_sx - font_size, str_sy, -frm_strinf.angle, format);
                                }
                                s_br.Dispose();
                                s_font.Dispose();


                                offset += frm_strline.cnt * 2;
                            }

                        }


                        break;
                    case 2://部品

                        if (layer_disp[type - 1] == false) break;
                        //if (checkBox2.Checked == false) break;

                        frm_symbol.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_symbol.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_symbol.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_symbol.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_symbol.width = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_symbol.angle = BitConverter.ToUInt16(buf, offset);//角度
                        offset += 2;
                        frm_symbol.kxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_symbol.kxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_symbol.symno = BitConverter.ToUInt16(buf, offset);//部品番号
                        offset += 2;

                        wx = frm_symbol.kxy.x - Offset.X;
                        wy = frm_symbol.kxy.y - Offset.Y;

                        int symbol_sx = (int)((double)wx / rate) + offset_w;
                        int symbol_sy = (int)((double)(-wy) / rate) + offset_h;

                        //MsgOut("symno=" + frm_symbol.symno.ToString());

                        //角度を考慮するとどうなるか？
                        Image image = SymbolImage[frm_symbol.symno];
                        double deg = frm_symbol.angle;
                        Point iconP = new Point(symbol_sx, symbol_sy);
                        int iconSize = 32;

                        DrawIcon(g, image,deg, iconP, iconSize);
//                        g.DrawImage(SymbolImage[frm_symbol.symno], symbol_sx - 16, symbol_sy - 16, 32, 32);

                        break;
                    case 3://折れ線

                        if (layer_disp[type - 1] == false) break;
                        //if (checkBox3.Checked == false) break;

                        frm_lines.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_lines.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_lines.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_lines.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_lines.cnt = BitConverter.ToUInt16(buf, offset);
                        offset += 2;

                        XY lines_xy = new XY();
                        Point[] lines_pnt = new Point[frm_lines.cnt];

                        for (int k = 0; k < frm_lines.cnt; k++)
                        {
                            lines_xy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            lines_xy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            wx = lines_xy.x - Offset.X;
                            wy = lines_xy.y - Offset.Y;

                            lines_pnt[k].X = (int)((double)wx / rate) + offset_w;
                            lines_pnt[k].Y = (int)((double)(-wy) / rate) + offset_h;

                        }

                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pe[0], lines_pnt);

                        break;
                    case 4://単純ポリゴン
                        if (layer_disp[type - 1] == false) break;

                        //if (checkBox4.Checked == false) break;

                        frm_polygon.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_polygon.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_polygon.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_polygon.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_polygon.cnt = BitConverter.ToUInt16(buf, offset);
                        offset += 2;

                        XY polygon_xy = new XY();
                        Point[] polygon_pnt = new Point[frm_polygon.cnt];

                        for (int k = 0; k < frm_polygon.cnt; k++)
                        {
                            polygon_xy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;
                            polygon_xy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            wx = polygon_xy.x - Offset.X;
                            wy = polygon_xy.y - Offset.Y;

                            polygon_pnt[k].X = (int)((double)wx / rate) + offset_w;
                            polygon_pnt[k].Y = (int)((double)(-wy) / rate) + offset_h;

                        }

                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        if (layno == 8 || layno == 18)
                        {
                            g.FillPolygon(b[3], polygon_pnt);
                        }
                        else
                        {
                            g.FillPolygon(b[1], polygon_pnt);
                        }

                        g.DrawPolygon(pe[1], polygon_pnt);

                        break;
                    case 5://中抜きポリゴン
                        if (layer_disp[type - 1] == false) break;

                        //if (checkBox5.Checked == false) break;

                        frm_outpoly.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.inpoly = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.cnt = BitConverter.ToUInt16(buf, offset);
                        offset += 2;

                        flg = frm_outpoly.cnt;

                        if (flg < 16)
                        {
                            flg = 16;
                        }
                        else
                        {
                            int s = flg / 16; //16の倍数
                            int a = flg % 16; //16で割ったあまり

                            if (a > 0)
                            {
                                flg = 16 * (s + 1);
                            }
                            else
                            {
                                flg = 16 * s;
                            }

                        }

                        newbufcnt = flg / 8;

                        byte[] outpolybuf = new byte[newbufcnt];
                        Array.Copy(buf, offset, outpolybuf, 0, newbufcnt);

                        offset += newbufcnt;//バイト分シフト

                        XY outpoly_xy = new XY();
                        Point[] outpoly_pnt = new Point[frm_outpoly.cnt];


                        for (int k = 0; k < frm_outpoly.cnt; k++)
                        {
                            outpoly_xy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;


                            outpoly_xy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            wx = outpoly_xy.x - Offset.X;
                            wy = outpoly_xy.y - Offset.Y;


                            outpoly_pnt[k].X = (int)((double)wx / rate) + offset_w;
                            outpoly_pnt[k].Y = (int)((double)(-wy) / rate) + offset_h;

                        }

                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        if (layno == 8 || layno == 18)
                        {
                            g.FillPolygon(b[3], outpoly_pnt);
                        }
                        else
                        {
                            g.FillPolygon(b[1], outpoly_pnt);
                        }

                        g.DrawPolygon(pe[2], outpoly_pnt);

                        //中抜きポリゴン
                        for (int m = 0; m < frm_outpoly.inpoly; m++)
                        {
                            frm_inpoly.cnt = BitConverter.ToUInt16(buf, offset);
                            offset += 2;

                            flg = frm_inpoly.cnt;

                            if (flg < 16)
                            {
                                flg = 16;
                            }
                            else
                            {
                                int s = flg / 16; //16の倍数
                                int a = flg % 16; //16で割ったあまり

                                if (a > 0)
                                {
                                    flg = 16 * (s + 1);
                                }
                                else
                                {
                                    flg = 16 * s;
                                }

                            }

                            newbufcnt = flg / 8;

                            byte[] inpolybuf = new byte[newbufcnt];
                            Array.Copy(buf, offset, inpolybuf, 0, newbufcnt);

                            offset += newbufcnt;//バイト分シフト

                            XY inpoly_xy = new XY();
                            Point[] inpoly_pnt = new Point[frm_inpoly.cnt];


                            for (int k = 0; k < frm_inpoly.cnt; k++)
                            {
                                inpoly_xy.x = BitConverter.ToInt16(buf, offset);
                                offset += 2;


                                inpoly_xy.y = BitConverter.ToInt16(buf, offset);
                                offset += 2;

                                wx = inpoly_xy.x - Offset.X;
                                wy = inpoly_xy.y - Offset.Y;

                                inpoly_pnt[k].X = (int)((double)wx / rate) + offset_w;
                                inpoly_pnt[k].Y = (int)((double)(-wy) / rate) + offset_h;

                            }
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            if (layno == 8 || layno == 18)
                            {
                                g.FillPolygon(b[3], inpoly_pnt);
                            }
                            else
                            {
                                g.FillPolygon(b[1], inpoly_pnt);
                            }
                            g.DrawPolygon(pe[2], inpoly_pnt);


                        }



                        break;
                    case 6://行政ポリゴン
                        if (layer_disp[type - 1] == false) break;

                        //if (checkBox6.Checked == false) break;

                        frm_areapoly.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.addr1 = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.addr2 = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.addr3 = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.addr4 = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.inpoly = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.cnt = BitConverter.ToUInt16(buf, offset);
                        offset += 2;


                        flg = frm_areapoly.cnt;

                        if (flg < 16)
                        {
                            flg = 16;
                        }
                        else
                        {
                            int s = flg / 16; //16の倍数
                            int a = flg % 16; //16で割ったあまり

                            if (a > 0)
                            {
                                flg = 16 * (s + 1);
                            }
                            else
                            {
                                flg = 16 * s;
                            }

                        }

                        newbufcnt = flg / 8;

                        byte[] areapolybuf = new byte[newbufcnt];
                        Array.Copy(buf, offset, areapolybuf, 0, newbufcnt);

                        offset += newbufcnt;//バイト分シフト

                        XY areapoly_xy = new XY();
                        Point[] areapoly_pnt = new Point[frm_areapoly.cnt];


                        for (int k = 0; k < frm_areapoly.cnt; k++)
                        {
                            areapoly_xy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            areapoly_xy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            wx = areapoly_xy.x - Offset.X;
                            wy = areapoly_xy.y - Offset.Y;

                            areapoly_pnt[k].X = (int)((double)wx / rate) + offset_w;
                            areapoly_pnt[k].Y = (int)((double)(-wy) / rate) + offset_h;

                        }

                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawPolygon(pe[1], areapoly_pnt);

                        //中抜きポリゴン（行政界）
                        for (int m = 0; m < frm_areapoly.inpoly; m++)
                        {
                            frm_areainpoly.cnt = BitConverter.ToUInt16(buf, offset);
                            offset += 2;

                            flg = frm_areainpoly.cnt;

                            if (flg < 16)
                            {
                                flg = 16;
                            }
                            else
                            {
                                int s = flg / 16; //16の倍数
                                int a = flg % 16; //16で割ったあまり

                                if (a > 0)
                                {
                                    flg = 16 * (s + 1);
                                }
                                else
                                {
                                    flg = 16 * s;
                                }

                            }

                            newbufcnt = flg / 8;

                            byte[] areainpolybuf = new byte[newbufcnt];
                            Array.Copy(buf, offset, areainpolybuf, 0, newbufcnt);

                            offset += newbufcnt;//バイト分シフト

                            XY areainpoly_xy = new XY();
                            Point[] areainpoly_pnt = new Point[frm_areainpoly.cnt];


                            for (int k = 0; k < frm_areainpoly.cnt; k++)
                            {
                                areainpoly_xy.x = BitConverter.ToInt16(buf, offset);
                                offset += 2;

                                areainpoly_xy.y = BitConverter.ToInt16(buf, offset);
                                offset += 2;

                                wx = areainpoly_xy.x - Offset.X;
                                wy = areainpoly_xy.y - Offset.Y;

                                areainpoly_pnt[k].X = (int)((double)wx / rate) + offset_w;
                                areainpoly_pnt[k].Y = (int)((double)(-wy) / rate) + offset_h;

                            }

                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.DrawPolygon(pe[2], areainpoly_pnt);
                        }

                        break;
                    case 7://文字付部品
                        if (layer_disp[type - 1] == false) break;

                        //if (checkBox7.Checked == false) break;
                        frm_strsym.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_strsym.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_strsym.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_strsym.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_strsym.width = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_strsym.angle = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_strsym.kxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_strsym.kxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_strsym.symno = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_strsym.cnt = BitConverter.ToUInt16(buf, offset);
                        offset += 2;

                        wx = frm_strsym.kxy.x - Offset.X;
                        wy = frm_strsym.kxy.y - Offset.Y;


                        int strsymbol_sx = (int)((double)wx / rate) + offset_w;
                        int strsymbol_sy = (int)((double)(-wy) / rate) + offset_h;

                        //g.DrawString("☆", f[0], b[0], strsymbol_sx, strsymbol_sy);

                        byte[] strsymbuf1 = new byte[frm_strsym.cnt * 2];
                        Array.Copy(buf, offset, strsymbuf1, 0, frm_strsym.cnt * 2);

                        byte[] strsymbuf2 = jis2sj.Jis2Sjis(strsymbuf1);

                        string strtext = System.Text.Encoding.GetEncoding("shift_jis").GetString(strsymbuf2);


                        Point p = new Point(strsymbol_sx, strsymbol_sy);

                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        if (layno == 100)
                        {
                            DrawGaiku(g, p, strtext);
                        }
                        else
                        {
                            g.DrawString(strtext, f[0], b[0], strsymbol_sx - 10, strsymbol_sy - 10);
                        }

                        break;
                    default:
                        break;

                }

                size += len;

                if (size >= brksize)
                {
                    EOF = true;
                }
            }

        }
        private void DrawGaiku(Graphics g,Point p, String num)
        {
            Font f;
            Brush s_br;//文字用
            Brush g_br;//図形用
            Pen pe;

            int font_size = 12;
            FontFamily fontFamily = new FontFamily("Meiryo UI");
            f = new Font(
                fontFamily,
                    font_size,
                   FontStyle.Regular,
                                      GraphicsUnit.Pixel);

            pe = new Pen(Color.Gray, 2);

            s_br = new SolidBrush(Color.White);
            g_br = new SolidBrush(Color.FromArgb(128, 255, 0, 0));

            int w = 30;
            int h = 30;

            Rectangle r = new Rectangle(p.X - w / 2, p.Y - h / w, w, h);

            g.DrawEllipse(pe, r);
            g.FillEllipse(g_br, r);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            g.DrawString(num, f, s_br, r, stringFormat);

            pe.Dispose();
            f.Dispose();
            s_br.Dispose();
            g_br.Dispose();
        }
        public void DrawIcon(Graphics g, Image image, double deg, Point p, int size)
        {
            // 度をラジアン値に変換する
            double rad = Math.PI * deg / 180.0;

            Rectangle rt = new Rectangle();
            rt.X = p.X;
            rt.Y = p.Y;
            rt.Width = size;
            rt.Height = size;


            Point[] destinationPoints = {
//new Point(p.X,p.Y),   // destination for upper-left point of
//new Point(p.X+ (int)(size * Math.Cos(rad)),p.Y - (int)(size*Math.Sin(rad))),  // destination for upper-right point of
//new Point(p.X+ (int)(size*Math.Sin(rad)),p.Y + (int)(size*Math.Cos(rad)))};  // destination for lower-left point of

new Point(p.X - (int)(size * Math.Sin(rad)), p.Y-(int)(size * Math.Cos(rad))),   // destination for upper-left point of
new Point(p.X + (int)(size * Math.Cos(rad)) - (int)(size * Math.Sin(rad)), p.Y - (int)(size * Math.Cos(rad)) - (int)(size * Math.Sin(rad))),  // destination for upper-right point of
new Point(p.X, p.Y)};  // destination for lower-left point of

        g.DrawImage(image, destinationPoints);
        }


        public void GetPointInfo(double lx, double ly)
        {
            pInfo.lx = lx;
                pInfo.ly = ly;
                    pInfo.meshX=0;
            pInfo.meshY =0;
            pInfo.meshfname = "";
            pInfo.addr1 =0;
            pInfo.addr2 =0;
            pInfo.addr3 =0;
            pInfo.addr4 =0;
            pInfo.addrname = "";
            pInfo.zcity =0;//拡張市町村
            pInfo.ztown1 =0;//大字
            pInfo.ztown2 =0;//丁目
            pInfo.zgcode =0;//街区
            pInfo.number ="";
            pInfo.name ="";
            if(pInfo.ext_info != null)
            {
                pInfo.ext_info.Clear();
            }


            //座標からメッシュを算出
            long Lx, Ly;
            long nZx, nZy;

            int dirX = 0;
            int dirY = 0;

            //long MBase_X, MBase_Y;

            string strtbl1 = "0123456789abcdef";
            string strtbl2 = "abcdefghijklmnop";

            Lx = (long)(lx * 100.0) - zmd_def.ZMDORGX;
            Ly = (long)(ly * 100.0) - zmd_def.ZMDORGY;

            // Lx, Ly は１０センチ単位なので
            Lx = (Lx + 5) / 10;
            Ly = (Ly + 5) / 10;

            nZx = Lx;
            nZy = Ly;

            // センターメッシュ取得
            long MeshX = nZx / zmd_def.MAP_W;
            if (nZx >= 0) MeshX++;

            long MeshY = nZy / zmd_def.MAP_H;
            if (nZy >= 0) MeshY++;


            pInfo.meshX = MeshX;
            pInfo.meshY = MeshY;

            long px = nZx % zmd_def.MAP_W;
            long py = nZy % zmd_def.MAP_H;

            string WkBuf = MeshX.ToString("x4") + MeshY.ToString("x4");

            string FileName = "";
            string DirNameX = "";
            string DirNameY = "";


            for (int i = 0; i < WkBuf.Length; i++)
            {

                for (int j = 0; j < strtbl1.Length; j++)
                {
                    if (WkBuf.Substring(i, 1) == strtbl1.Substring(j, 1))
                    {
                        FileName += strtbl2.Substring(j, 1);
                    }
                }
            }

            pInfo.meshfname = FileName;

            dirX = (int)(MeshX / 5);
            WkBuf = dirX.ToString("x4");
            for (int i = 0; i < WkBuf.Length; i++)
            {

                for (int j = 0; j < strtbl1.Length; j++)
                {
                    if (WkBuf.Substring(i, 1) == strtbl1.Substring(j, 1))
                    {
                        DirNameX += strtbl2.Substring(j, 1);
                    }
                }
            }

            dirY = (int)(MeshY / 5);
            WkBuf = dirY.ToString("x4");
            for (int i = 0; i < WkBuf.Length; i++)
            {

                for (int j = 0; j < strtbl1.Length; j++)
                {
                    if (WkBuf.Substring(i, 1) == strtbl1.Substring(j, 1))
                    {
                        DirNameY += strtbl2.Substring(j, 1);
                    }
                }
            }

            //該当メッシュファイルを検索
            string[] files = Directory.GetFiles(
                mappath, FileName + ".baf", SearchOption.AllDirectories);

            string ZmdFile = "";

            foreach (string s in files)
            {
                ZmdFile = s;
            }


            string[] files2 = Directory.GetFiles(
    mappath, FileName + ".baa", SearchOption.AllDirectories);

            string BaaFile = "";

            foreach (string s in files2)
            {
                BaaFile = s;
            }


            if (ZmdFile.Length > 1)
            {
                string DirName = System.IO.Path.GetDirectoryName(ZmdFile);

                //MsgOut(ZmdFile);

                //上位ディレクトリを検索
                DirectoryInfo diParent = Directory.GetParent(DirName);
                DirectoryInfo diParent2 = Directory.GetParent(diParent.FullName);
                DirectoryInfo diParent3 = Directory.GetParent(diParent2.FullName);
                DirectoryInfo diParent4 = Directory.GetParent(diParent3.FullName);


                string PasswordFile = diParent4.FullName + "\\PASSWORD.DAT";

                ReadZMD2(ZmdFile, BaaFile,PasswordFile, px, py);
            }
            else
            {
            }
        }
        public void ReadZMD2(string zmdfile, string BaaFile, string PasswordFile, long px, long py)
        {
            mask = GetMask(PasswordFile);

            string fname = Path.GetFileNameWithoutExtension(zmdfile);

            if (File.Exists(zmdfile) == false) return;


            FileStream fs = new FileStream(zmdfile, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);


            FRM_HEAD frm_head = new FRM_HEAD();
            frm_head.pickind = new PicKind_Code();
            frm_head.date = new Date();
            frm_head.zno = new ZNO();
            FRM_BRK frm_brk = new FRM_BRK();
            FRM_INF frm_inf = new FRM_INF();
            FRM_LAY frm_lay = new FRM_LAY();

            char[] s_fname = new char[256];

            s_fname = fname.ToCharArray();

            byte[] s1 = new byte[4];
            byte[] s2 = new byte[4];
            byte[] s3 = new byte[4];
            byte[] s4 = new byte[4];

            System.UInt32[] brkptr = new System.UInt32[99];
            System.UInt32[] brksize = new System.UInt32[99];


            int valx = 0;
            int valy = 0;



            for (int j = 0; j < 4; j++)
            {
                if (s_fname[j] >= 'A' && s_fname[j] <= 'P')
                {
                    valx += (int)(s_fname[j] - 'A') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                else
                {
                    valx += (int)(s_fname[j] - 'a') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                if (s_fname[j + 4] >= 'A' && s_fname[j + 4] <= 'P')
                {
                    valy += (int)(s_fname[j + 4] - 'A') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                else
                {
                    valy += (int)(s_fname[j + 4] - 'a') * (int)Math.Pow((double)16, (double)(3 - j));
                }
            }

            s1 = BitConverter.GetBytes(valx);
            s2 = BitConverter.GetBytes(valy);

            //
            //ヘッダ部
            //


            frm_head.pickind.pic1 = br.ReadByte();
            frm_head.pickind.pic2 = br.ReadByte();
            frm_head.pickind.pic3 = br.ReadByte();
            frm_head.pickind.pic4 = br.ReadByte();

            frm_head.date.y = br.ReadUInt16();
            frm_head.date.m = br.ReadByte();
            frm_head.date.d = br.ReadByte();

            frm_head.zno.x = br.ReadUInt16();
            frm_head.zno.y = br.ReadUInt16();

            frm_head.maxlay = br.ReadUInt16();           // 最大レイヤ番号
            frm_head.rsv = br.ReadUInt16();              // リザーブ
            frm_head.brk_ptr = br.ReadUInt32();          // ブロック管理部アドレス
            frm_head.brk_cnt = br.ReadUInt32();          // ブロック情報部レコード数
            frm_head.inf_ptr = br.ReadUInt32();          // 属性対応管理部アドレス
            frm_head.inf_cnt = br.ReadUInt32();          // 属性対応情報部レコード数
            frm_head.brk_str = br.ReadUInt32();          // ブロック部開始アドレス
            frm_head.ref_ptr = br.ReadUInt32();          // 関連図管理部アドレス
            frm_head.ref_cnt = br.ReadUInt32();          // 関連図情報部レコード数
            frm_head.rsv_ptr = br.ReadUInt32();          // 予約領域部アドレス
            frm_head.file_sz = br.ReadUInt32();          // ファイル容量

            maxlay = frm_head.maxlay;

            //
            //ブロック管理部
            //

            fs.Seek(frm_head.brk_ptr, SeekOrigin.Begin);

            for (int i = 0; i < frm_head.brk_cnt; i++)
            {
                frm_brk.brk_ptr = br.ReadUInt32();
                frm_brk.brk_sz = br.ReadUInt32();

                s3 = BitConverter.GetBytes(frm_brk.brk_ptr);


                s3[0] = (byte)(s3[0] ^ s1[0]);
                s3[1] = (byte)(s3[1] ^ s1[1]);
                s3[2] = (byte)(s3[2] ^ s2[0]);
                s3[3] = (byte)(s3[3] ^ s2[1]);
                //マスクとのXOR
                s3[1] = (byte)(s3[1] ^ mask);
                s3[2] = (byte)(s3[2] ^ mask);


                frm_brk.brk_ptr = BitConverter.ToUInt32(s3, 0);

                brkptr[i] = frm_brk.brk_ptr;
                brksize[i] = frm_brk.brk_sz;
            }


            //
            //属性対応情報管理部
            //

            fs.Seek(frm_head.inf_ptr, SeekOrigin.Begin);


            for (int i = 0; i < frm_head.inf_cnt; i++)
            {
                frm_inf.inf_num = br.ReadUInt32();          // 図内属性番号
                frm_inf.inf_z_brk = br.ReadUInt16();        // 図形形状所属ブロック
                frm_inf.inf_m_brk = br.ReadUInt16();        // 文字形状所属ブロック
                frm_inf.inf_z_off = br.ReadUInt32();        // 図形形状ブロック内オフセット
                frm_inf.inf_m_off = br.ReadUInt32();        // 文字形状ブロック内オフセット
                frm_inf.inf_recnum = br.ReadUInt32();       // 属性情報部レコード番号
                frm_inf.rsv = br.ReadUInt32();			   // リザーブ
            }

            //
            //ブロック部
            //

            for (int i = 0; i < frm_head.brk_cnt; i++)
            {
                if (brksize[i] > 0)
                {
                    fs.Seek(brkptr[i], SeekOrigin.Begin);

                    FrmSub2(BaaFile,PasswordFile, i, fs, br, brksize[i], px, py);

                }
            }

            //
            //関連情報管理部
            //

            //
            //予約領域部
            //

            fs.Close();
            br.Close();

        }

        public void FrmSub2(string BaaFile,string PasswordFile,int brkno, FileStream fs, BinaryReader br, System.UInt32 brksize, long px, long py)
        {
            PointF chk_point = new PointF();
            chk_point.X = (float)px;
            chk_point.Y = (float)py;

            FRM_FRMCOM frm_com = new FRM_FRMCOM();

            //形状情報部

            Boolean EOF = false;
            byte[] buf = new byte[2048];


            UInt32[] lay_off = new UInt32[maxlay];
            UInt32[] lay_size = new UInt32[maxlay];
            UInt32[] lay_frm_off = new UInt32[maxlay];


            int flg = 0;
            int newbufcnt = 0;

            //
            //ブロックの先頭ポインタ
            //
            int brkstart = (int)fs.Seek(0, SeekOrigin.Current);


            //if ((UInt32)brkstart == 0xffffffff) return;

            int size = 0;

            //レイヤ情報部
            for (int j = 0; j < maxlay; j++)
            {

                //try
                //{
                lay_off[j] = br.ReadUInt32();
                lay_size[j] = br.ReadUInt32();
                lay_frm_off[j] = br.ReadUInt32();
                size += 12;
                //}
                //catch (Exception e)
                // {

                // }


                //Console.WriteLine(j.ToString() + "," + lay_off[j].ToString() + "," + lay_size[j].ToString() + "," + lay_frm_off[j].ToString());
            }


            while (EOF != true)
            {
                int cur = (int)fs.Seek(0, SeekOrigin.Current) - brkstart;

                //
                //レイヤ番号判定
                //
                int layno = -1;

                for (int k = 0; k < maxlay; k++)
                {
                    if (lay_frm_off[k] != 0xffffffff && cur >= lay_off[k] && cur < lay_off[k] + lay_size[k])
                    {
                        layno = k + 1;
                        break;
                    }
                }

                frm_com.frmcom_recnum = br.ReadUInt32();
                frm_com.frmcom_size = br.ReadUInt16();
                frm_com.frmcom_type = br.ReadByte();
                frm_com.frmcom_status = br.ReadByte();

                size += 8;

                int type = (int)frm_com.frmcom_type;
                int len = frm_com.frmcom_size - (8);

                buf = br.ReadBytes(len);

                FRM_AREAPOLY frm_areapoly = new FRM_AREAPOLY();
                frm_areapoly.ldxy = new XY();
                frm_areapoly.ruxy = new XY();
                FRM_STRINF frm_strinf = new FRM_STRINF();
                frm_strinf.ldxy = new XY();
                frm_strinf.ruxy = new XY();
                FRM_STRLINE frm_strline = new FRM_STRLINE();
                frm_strline.kxy = new XY();
                FRM_SYMBOL frm_symbol = new FRM_SYMBOL();
                frm_symbol.ldxy = new XY();
                frm_symbol.ruxy = new XY();
                frm_symbol.kxy = new XY();
                FRM_LINES frm_lines = new FRM_LINES();
                frm_lines.ldxy = new XY();
                frm_lines.ruxy = new XY();

                FRM_POLYGON frm_polygon = new FRM_POLYGON();
                frm_polygon.ldxy = new XY();
                frm_polygon.ruxy = new XY();

                FRM_OUTPOLY frm_outpoly = new FRM_OUTPOLY();
                frm_outpoly.ldxy = new XY();
                frm_outpoly.ruxy = new XY();

                FRM_INPOLY frm_inpoly = new FRM_INPOLY();
                FRM_INPOLY frm_areainpoly = new FRM_INPOLY();

                FRM_STRSYM frm_strsym = new FRM_STRSYM();
                frm_strsym.ldxy = new XY();
                frm_strsym.ruxy = new XY();
                frm_strsym.kxy = new XY();


                int offset = 0;

                int wx = 0;
                int wy = 0;

                switch (type)
                {
                    case 1://文字
                        break;
                    case 2://シンボル
                        break;
                    case 3://折れ線
                        break;
                    case 4://単純ポリゴン
                        frm_polygon.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_polygon.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_polygon.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_polygon.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_polygon.cnt = BitConverter.ToUInt16(buf, offset);
                        offset += 2;

                        XY polygon_xy = new XY();
                        Point[] polygon_pnt = new Point[frm_polygon.cnt];

                        List<PointF> chk_polygon = new List<PointF>();

                        for (int k = 0; k < frm_polygon.cnt; k++)
                        {
                            polygon_xy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;
                            polygon_xy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            polygon_pnt[k].X = polygon_xy.x;
                            polygon_pnt[k].Y = polygon_xy.y;

                            PointF pf = new PointF();
                            pf.X = (float)polygon_xy.x;
                            pf.Y = (float)polygon_xy.y;
                            chk_polygon.Add(pf);
                        }
                        if (CheckInOut(chk_point, chk_polygon) == true)
                        {
                            ReadBAA(BaaFile,PasswordFile, (int)frm_com.frmcom_recnum);
                            //f1.MsgOut("frm_com.frmcom_recnum=" + frm_com.frmcom_recnum);
                        }

                        break;
                    case 5://中抜きポリゴン
                        frm_outpoly.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.inpoly = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_outpoly.cnt = BitConverter.ToUInt16(buf, offset);
                        offset += 2;

                        flg = frm_outpoly.cnt;

                        if (flg < 16)
                        {
                            flg = 16;
                        }
                        else
                        {
                            int s = flg / 16; //16の倍数
                            int a = flg % 16; //16で割ったあまり

                            if (a > 0)
                            {
                                flg = 16 * (s + 1);
                            }
                            else
                            {
                                flg = 16 * s;
                            }

                        }

                        newbufcnt = flg / 8;

                        byte[] outpolybuf = new byte[newbufcnt];
                        Array.Copy(buf, offset, outpolybuf, 0, newbufcnt);

                        offset += newbufcnt;//バイト分シフト

                        XY outpoly_xy = new XY();
                        Point[] outpoly_pnt = new Point[frm_outpoly.cnt];


                        for (int k = 0; k < frm_outpoly.cnt; k++)
                        {
                            outpoly_xy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;


                            outpoly_xy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            outpoly_pnt[k].X = outpoly_xy.x;
                            outpoly_pnt[k].Y = outpoly_xy.y;

                        }

                        //中抜きポリゴン
                        for (int m = 0; m < frm_outpoly.inpoly; m++)
                        {
                            frm_inpoly.cnt = BitConverter.ToUInt16(buf, offset);
                            offset += 2;

                            flg = frm_inpoly.cnt;

                            if (flg < 16)
                            {
                                flg = 16;
                            }
                            else
                            {
                                int s = flg / 16; //16の倍数
                                int a = flg % 16; //16で割ったあまり

                                if (a > 0)
                                {
                                    flg = 16 * (s + 1);
                                }
                                else
                                {
                                    flg = 16 * s;
                                }

                            }

                            newbufcnt = flg / 8;

                            byte[] inpolybuf = new byte[newbufcnt];
                            Array.Copy(buf, offset, inpolybuf, 0, newbufcnt);

                            offset += newbufcnt;//バイト分シフト

                            XY inpoly_xy = new XY();
                            Point[] inpoly_pnt = new Point[frm_inpoly.cnt];


                            for (int k = 0; k < frm_inpoly.cnt; k++)
                            {
                                inpoly_xy.x = BitConverter.ToInt16(buf, offset);
                                offset += 2;


                                inpoly_xy.y = BitConverter.ToInt16(buf, offset);
                                offset += 2;

                                inpoly_pnt[k].X = inpoly_xy.x;
                                inpoly_pnt[k].Y = inpoly_xy.y;

                            }
                        }

                        break;
                    case 6://行政ポリゴン
                        frm_areapoly.ldxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.ldxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.ruxy.x = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.ruxy.y = BitConverter.ToInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.addr1 = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.addr2 = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.addr3 = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.addr4 = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.inpoly = BitConverter.ToUInt16(buf, offset);
                        offset += 2;
                        frm_areapoly.cnt = BitConverter.ToUInt16(buf, offset);
                        offset += 2;


                        flg = frm_areapoly.cnt;

                        if (flg < 16)
                        {
                            flg = 16;
                        }
                        else
                        {
                            int s = flg / 16; //16の倍数
                            int a = flg % 16; //16で割ったあまり

                            if (a > 0)
                            {
                                flg = 16 * (s + 1);
                            }
                            else
                            {
                                flg = 16 * s;
                            }

                        }

                        newbufcnt = flg / 8;

                        byte[] areapolybuf = new byte[newbufcnt];
                        Array.Copy(buf, offset, areapolybuf, 0, newbufcnt);

                        offset += newbufcnt;//バイト分シフト

                        XY areapoly_xy = new XY();
                        Point[] areapoly_pnt = new Point[frm_areapoly.cnt];

                        List<PointF> chk_areapolygon = new List<PointF>();

                        for (int k = 0; k < frm_areapoly.cnt; k++)
                        {
                            areapoly_xy.x = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            areapoly_xy.y = BitConverter.ToInt16(buf, offset);
                            offset += 2;

                            areapoly_pnt[k].X = areapoly_xy.x;
                            areapoly_pnt[k].Y = areapoly_xy.y;

                            PointF pf = new PointF();
                            pf.X = (float)areapoly_xy.x;
                            pf.Y = (float)areapoly_xy.y;
                            chk_areapolygon.Add(pf);

                        }

                        if (CheckInOut(chk_point, chk_areapolygon) == true)
                        {
                            pInfo.addr1 = frm_areapoly.addr1;
                            pInfo.addr2 = frm_areapoly.addr2;
                            pInfo.addr3 = frm_areapoly.addr3;
                            pInfo.addr4 = frm_areapoly.addr4;

                            int zcity = frm_areapoly.addr1;
                            int ztown1 = frm_areapoly.addr2;
                            int ztown2 = frm_areapoly.addr3;

                            maptool.Address pAddr = new maptool.Address();
                            pAddr.f1 = f1;

                            String area_addrName = "";

                            pAddr.GetAddrName(zcity / 1000, 0, 0, 0, ref area_addrName);
                            pInfo.area_addrname = area_addrName.Trim();

                            pAddr.GetAddrName(zcity / 1000, zcity % 1000, 0, 0, ref area_addrName);
                            pInfo.area_addrname += area_addrName.Trim();

                            if (ztown1 > 0)
                            {
                                pAddr.GetAddrName(zcity / 1000, zcity % 1000, ztown1, 0, ref area_addrName);
                                pInfo.area_addrname += area_addrName.Trim();
                            }

                            if (ztown2 > 0)
                            {
                                pAddr.GetAddrName(zcity / 1000, zcity % 1000, ztown1, ztown2, ref area_addrName);
                                pInfo.area_addrname += area_addrName.Trim();
                            }


                        }

                        //中抜きポリゴン（行政界）
                        for (int m = 0; m < frm_areapoly.inpoly; m++)
                        {
                            frm_areainpoly.cnt = BitConverter.ToUInt16(buf, offset);
                            offset += 2;

                            flg = frm_areainpoly.cnt;

                            if (flg < 16)
                            {
                                flg = 16;
                            }
                            else
                            {
                                int s = flg / 16; //16の倍数
                                int a = flg % 16; //16で割ったあまり

                                if (a > 0)
                                {
                                    flg = 16 * (s + 1);
                                }
                                else
                                {
                                    flg = 16 * s;
                                }

                            }

                            newbufcnt = flg / 8;

                            byte[] areainpolybuf = new byte[newbufcnt];
                            Array.Copy(buf, offset, areainpolybuf, 0, newbufcnt);

                            offset += newbufcnt;//バイト分シフト

                            XY areainpoly_xy = new XY();
                            Point[] areainpoly_pnt = new Point[frm_areainpoly.cnt];


                            for (int k = 0; k < frm_areainpoly.cnt; k++)
                            {
                                areainpoly_xy.x = BitConverter.ToInt16(buf, offset);
                                offset += 2;

                                areainpoly_xy.y = BitConverter.ToInt16(buf, offset);
                                offset += 2;

                                areainpoly_pnt[k].X = areainpoly_xy.x;
                                areainpoly_pnt[k].Y = areainpoly_xy.y;

                            }
                        }

                        break;
                    case 7:
                        break;
                    default:
                        break;
                }

                size += len;

                if (size >= brksize)
                {
                    EOF = true;
                }
            }

        }

        private void ReadBAA(string BaaFile, string PasswordFile, int recnum)
        {

            mask = GetMask(PasswordFile);

            string baa_fname = BaaFile;

            FileStream fs = new FileStream(baa_fname, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            byte[] buf = new byte[2048];
            byte[] wbuf16 = new byte[16];
            byte[] wbuf62 = new byte[62];


            buf = br.ReadBytes(4);//図種別
            UInt16 zno_x = br.ReadUInt16();//図番号X
            UInt16 zno_y = br.ReadUInt16();//図番号Y

            uint ptr1 = br.ReadUInt32();//住所索引管理部アドレス
            uint rec1 = br.ReadUInt32();//住所索引情報部レコード数
            uint ptr2 = br.ReadUInt32();//属性情報管理部アドレス
            uint rec2 = br.ReadUInt32();//属性情報部レコード数
            uint ptr3 = br.ReadUInt32();//別記属性管理部アドレス
            uint rec3 = br.ReadUInt32();//別記属性管理部容量

            fs.Seek(ptr1, SeekOrigin.Begin);

            for (int i = 0; i < rec1; i++)
            {
                UInt16 city = br.ReadUInt16();
                UInt16 town1 = br.ReadUInt16();
                UInt16 town2 = br.ReadUInt16();
                UInt16 gcode = br.ReadUInt16();
                uint z_ptr = br.ReadUInt32();
                uint z_cnt = br.ReadUInt32();
            }

            byte[] s1 = new byte[4];
            byte[] s2 = new byte[4];
            byte[] s3 = new byte[4];
            byte[] s4 = new byte[4];

            int valx = 0;
            int valy = 0;

            string fname = Path.GetFileNameWithoutExtension(baa_fname);

            char[] s_fname = new char[256];
            s_fname = fname.ToCharArray();


            for (int j = 0; j < 4; j++)
            {
                if (s_fname[j] >= 'A' && s_fname[j] <= 'P')
                {
                    valx += (int)(s_fname[j] - 'A') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                else
                {
                    valx += (int)(s_fname[j] - 'a') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                if (s_fname[j + 4] >= 'A' && s_fname[j + 4] <= 'P')
                {
                    valy += (int)(s_fname[j + 4] - 'A') * (int)Math.Pow((double)16, (double)(3 - j));
                }
                else
                {
                    valy += (int)(s_fname[j + 4] - 'a') * (int)Math.Pow((double)16, (double)(3 - j));
                }
            }

            s1 = BitConverter.GetBytes(valx);
            s2 = BitConverter.GetBytes(valy);

            s3 = BitConverter.GetBytes(ptr2);

            s3[0] = (byte)(s3[0] ^ s1[0]);
            s3[1] = (byte)(s3[1] ^ s1[1]);
            s3[2] = (byte)(s3[2] ^ s2[0]);
            s3[3] = (byte)(s3[3] ^ s2[1]);
            //マスクとのXOR
            s3[1] = (byte)(s3[1] ^ mask);
            s3[2] = (byte)(s3[2] ^ mask);


            ptr2 = BitConverter.ToUInt32(s3, 0);
            fs.Seek(ptr2, SeekOrigin.Begin);

            for (int i = 0; i < rec2; i++)
            {
                uint zno = br.ReadUInt32();//図番号
                UInt16 ztype = br.ReadUInt16();//属性種別
                UInt16 zcity = br.ReadUInt16();//拡張市町村
                UInt16 ztown1 = br.ReadUInt16();//大字
                UInt16 ztown2 = br.ReadUInt16();//丁目
                UInt16 zgcode = br.ReadUInt16();//街区

                buf = br.ReadBytes(2);

                wbuf16 = br.ReadBytes(16);//地番
                byte[] number_buf = jis2sj.Jis2Sjis(wbuf16);
                string number = System.Text.Encoding.GetEncoding("shift_jis").GetString(number_buf);

                byte septype = br.ReadByte();
                byte seppos = br.ReadByte();

                wbuf62 = br.ReadBytes(62);//名称
                byte[] name_buf = jis2sj.Jis2Sjis(wbuf62);
                string name = System.Text.Encoding.GetEncoding("shift_jis").GetString(name_buf);

                Int16 floor = br.ReadInt16();//建物階数

                uint frm_zno = br.ReadUInt32();//形状対応情報部のレコード番号
                uint frm_zoffset = br.ReadUInt32();//別記属性管理部からのオフセット

                if (frm_zno == recnum)
                {
                    //Console.WriteLine(name);
                    pInfo.zcity = zcity;
                    pInfo.ztown1 = ztown1;
                    pInfo.ztown2 = ztown2;
                    pInfo.zgcode = zgcode;
                    pInfo.number = number;
                    pInfo.name = name;

                    maptool.Address pAddr = new maptool.Address();
                    pAddr.f1 = f1;
                    String addrName = "";

                    pAddr.GetAddrName(zcity / 1000, 0, 0,0, ref addrName);
                    pInfo.addrname = addrName.Trim();

                    pAddr.GetAddrName(zcity / 1000, zcity % 1000,0,0, ref addrName);
                    pInfo.addrname += addrName.Trim();

                    if (ztown1 > 0)
                    {
                        pAddr.GetAddrName(zcity / 1000, zcity % 1000, ztown1, 0, ref addrName);
                        pInfo.addrname += addrName.Trim();
                    }

                    if (ztown2 > 0)
                    {
                        pAddr.GetAddrName(zcity / 1000, zcity % 1000, ztown1, ztown2, ref addrName);
                        pInfo.addrname += addrName.Trim();
                    }

                    if (frm_zoffset != 0xffffffff)
                    {
                        fs.Seek(ptr3 + frm_zoffset, SeekOrigin.Begin);
                        uint ext_cnt = br.ReadUInt32();//別記属性レコード数

                        pInfo.ext_info = new List<String>();

                        for(int m = 0; m < ext_cnt; m++)
                        {
                            uint ext_no = br.ReadUInt32();
                            uint ext_code = br.ReadUInt16();
                            char ctype = br.ReadChar();

                            byte rsv = br.ReadByte();
                            int ext_floor = br.ReadInt16();

                            byte[] wfloor_name = new byte[10];
                            wfloor_name = br.ReadBytes(10);//地番
                            byte[] floor_name_buf = jis2sj.Jis2Sjis(wfloor_name);
                            string floor_name = System.Text.Encoding.GetEncoding("shift_jis").GetString(floor_name_buf);

                            byte sep_type = br.ReadByte();
                            byte sep_pos = br.ReadByte();

                            byte[] wext_name = new byte[50];
                            wext_name = br.ReadBytes(50);//地番
                            byte[] ext_name_buf = jis2sj.Jis2Sjis(wext_name);
                            string ext_name = System.Text.Encoding.GetEncoding("shift_jis").GetString(ext_name_buf);

                            pInfo.ext_info.Add(ctype+" "+ext_floor.ToString("D2")+" "+ext_name);
                        }
                    }

                    break;
                }
            }

            fs.Close();
            br.Close();
        }

        public void PointToMesh(int wx, int wy,ref String meshfname)
        {
            double lx;
            double ly;

            lx = (double)wx / 1000.0;
            ly = (double)wy / 1000.0;

            //座標からメッシュを算出
            long Lx, Ly;
            long nZx, nZy;

            int dirX = 0;
            int dirY = 0;

            string strtbl1 = "0123456789abcdef";
            string strtbl2 = "abcdefghijklmnop";

            Lx = (long)(lx * 100.0) - zmd_def.ZMDORGX;
            Ly = (long)(ly * 100.0) - zmd_def.ZMDORGY;

            // Lx, Ly は１０センチ単位なので
            Lx = (Lx + 5) / 10;
            Ly = (Ly + 5) / 10;

            nZx = Lx;
            nZy = Ly;

            // センターメッシュ取得
            long MeshX = nZx / zmd_def.MAP_W;
            if (nZx >= 0) MeshX++;

            long MeshY = nZy / zmd_def.MAP_H;
            if (nZy >= 0) MeshY++;


            pInfo.meshX = MeshX;
            pInfo.meshY = MeshY;

            long px = nZx % zmd_def.MAP_W;
            long py = nZy % zmd_def.MAP_H;

            string WkBuf = MeshX.ToString("x4") + MeshY.ToString("x4");

            string FileName = "";
            string DirNameX = "";
            string DirNameY = "";


            for (int i = 0; i < WkBuf.Length; i++)
            {

                for (int j = 0; j < strtbl1.Length; j++)
                {
                    if (WkBuf.Substring(i, 1) == strtbl1.Substring(j, 1))
                    {
                        FileName += strtbl2.Substring(j, 1);
                    }
                }
            }

            meshfname = FileName;

        }

        public Boolean CheckInOut(PointF point, List<PointF> polygon)
        {
            Boolean ret = false;

            int count = 0;
            for (int n = 0; n < polygon.Count(); n++)
            {
                PointF pt1;
                PointF pt2;

                if (n == (polygon.Count() - 1))
                {
                    pt1 = polygon[n];
                    pt2 = polygon[0];
                }
                else
                {
                    pt1 = polygon[n];
                    pt2 = polygon[n + 1];
                }

                SizeF sizef = new SizeF(point.X, point.Y);

                PointF vector1 = PointF.Subtract(pt1, sizef);
                PointF vector2 = PointF.Subtract(pt2, sizef);

                // 外積(分母省略)を算出する.
                float ext = vector1.X * vector2.Y - vector1.Y * vector2.X;

                // ポイントからの半直線（Ｘ軸正の方向）と辺が重なる場合はカウントアップする.
                if ((vector1.Y >= 0) && (vector2.Y < 0) && (ext > 0))
                {
                    count++;
                }
                else if ((vector1.Y < 0) && (vector2.Y >= 0) && (ext < 0))
                {
                    count++;
                }
            }
            // カウントの値が奇数ならば内側と判定する.
            if ((count % 2) == 1)
            {
                // 内側と判定する.
                ret = true;
            }
            else
            {
                // 外側と判定する.
                ret = false;
            }
            return ret;
        }
        public void MsgOut(string msg)
        {
            f1.MsgOut(msg);
        }
    }

    //地点情報
    public class PointInfo{
        public double lx;
        public double ly;
        public long meshX;
        public long meshY;
        public String area_addrname;
        public String meshfname;
        public ushort addr1;
        public ushort addr2;
        public ushort addr3;
        public ushort addr4;
        public String addrname;
        public UInt16 zcity;//拡張市町村
        public UInt16 ztown1;//大字
        public UInt16 ztown2;//丁目
        public UInt16 zgcode;//街区
        public String number;
        public String name;
        public List<String> ext_info;
    }
}
