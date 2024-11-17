using COM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maptool
{
     class DrawUlay
    {
        public maptool.Form1 f1;
        string ulaypath;
        string pngpath;

        Bitmap[] uIcon = new Bitmap[5];

        public DashStyle[] LINETYPE_TBL = { DashStyle.Solid, DashStyle.Dash, DashStyle.Dot, DashStyle.DashDot };
        public HatchStyle[] POLYHATCHSTYLE_TBL = { HatchStyle.Vertical, HatchStyle.Horizontal, HatchStyle.Cross };
        public FontStyle[] FONTSTYLE_TBL = { FontStyle.Regular, FontStyle.Bold,FontStyle.Italic,FontStyle.Strikeout,FontStyle.Underline };
        public int[] STRINGTYPE_TBL = { 0, 1 };
        public Color SELECTED_LINECOL = Color.Magenta;
        public Color SELECTED_STRINGBRUSH = Color.Magenta;
        public int SELECTED_LINEWIDTH = 5;


        public void DrawUserLayer(String mesh,System.Drawing.Graphics g, double lx, double ly, double rate, int offset_w, int offset_h)
        {

            ulaypath = f1.datapath+"\\ULAY";
            pngpath = f1.datapath+"\\icon\\png";

          //  for(int i = 0; i < f1.ICON_TBL.Count; i++)
            //{
              //  uIcon[i] = new Bitmap(pngpath + "\\"+f1.ICON_TBL[i]);
            //}

            string file = ulaypath + "\\layer_"+mesh+".txt";

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

                            points[i].X = offset_w + (int)((mx - lx) * (10 / rate));
                            points[i].Y = offset_h - (int)((my - ly) * (10 / rate));

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

                        Pen u_pe = new Pen(ColorTranslator.FromHtml(LineCol), LineWidth);
                        DashStyle u_LineType = LINETYPE_TBL[LineType];
                        u_pe.DashStyle = u_LineType;

                        HatchStyle u_PolyHatchStyle = POLYHATCHSTYLE_TBL[PolyHatchStyle];
                        SolidBrush u_pbr_s = new SolidBrush(ColorTranslator.FromHtml(PolyBrush));
                        HatchBrush u_pbr_h = new HatchBrush(u_PolyHatchStyle, ColorTranslator.FromHtml(PolyBrush), ColorTranslator.FromHtml(PolyBrush2));

                        SolidBrush u_sbr = new SolidBrush(ColorTranslator.FromHtml(StringBrush));
                        Font u_font = new Font("ＭＳ Ｐゴシック", StringSize,FONTSTYLE_TBL[StringFontType]);
                        int u_StringType = STRINGTYPE_TBL[StringType];
                        int u_StringAngle = StringAngle;

                        switch (laytype)
                        {
                            case ConstDef.Mode_Line://line
                                if (cnt > 1)
                                {
                                    g.DrawLines(u_pe, points);
                                }
                                break;
                            case ConstDef.Mode_Polygon://polygon
                                if (cnt > 1)
                                {
                                    g.DrawPolygon(u_pe, points);
                                    if (PolyBrushType == 0)
                                    {
                                        g.FillPolygon(u_pbr_s, points);
                                    }
                                    else
                                    {
                                        g.FillPolygon(u_pbr_h, points);
                                    }
                                }
                                break;
                            case ConstDef.Mode_Symbol:

                                pngpath = f1.datapath + "\\icon\\png";
                                Bitmap wkIcon = new Bitmap(pngpath + "\\" + IconName);

                                //g.DrawImage(wkIcon, points[0].X - 16, points[0].Y - 16);

                                //Double SymbolAngle = Double.Parse(comboSymbolAngle.Text);

                                double deg = SymbolAngle;
                                Point iconP = new Point(points[0].X, points[0].Y);
                                int iconSize = 32;

                                DrawIcon(g, wkIcon, deg, iconP, iconSize);

                                wkIcon.Dispose();

                                //g.DrawImage(uIcon[IconIdx], points[0].X - 16, points[0].Y - 16);

                                break;
                            case ConstDef.Mode_String://
//                                g.DrawString(StringText, u_font, u_sbr, points[0].X, points[0].Y);

                                var format = new StringFormat();
                                format.Alignment = StringAlignment.Near;      // 左右方向は中心寄せ
                                format.LineAlignment = StringAlignment.Near;  // 上下方向は中心寄せ

                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                if (u_StringType == 0)
                                {
                                    format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                                    StringCOM.DrawString(g, StringText, u_font, u_sbr, points[0].X, points[0].Y, u_StringAngle, format);
                                }
                                else
                                {
                                    format.FormatFlags = StringFormatFlags.DirectionVertical;
                                    StringCOM.DrawString(g, StringText, u_font, u_sbr, points[0].X, points[0].Y, u_StringAngle, format);
                                }

                                break;
                            case ConstDef.Mode_Freeline://
                                if (cnt > 1)
                                {
                                    g.DrawLines(u_pe, points);
                                }
                                break;
                        }

                        u_pe.Dispose();
                        u_pbr_s.Dispose();
                        u_pbr_h.Dispose();
                        u_sbr.Dispose();
                        u_font.Dispose();

                    }
                    sr.Close();
                }
            }

        }
        public void DrawIcon(Graphics g, System.Drawing.Image image, double deg, Point p, int size)
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


        public void SaveUserlayer(int laytype, List<Point> points, String textBuf, String IconName, int LineWidth, String LineCol, int LineType,int PolyBrushType,String PolyBrush, String PolyBrush2,int PolyHatchStyle,int StringSize, String StringBrush, int StringType, int StringAngle,int StringFontType,Double SymbolAngle,String meshfname,String addrname)
        {
            ulaypath = f1.datapath + "\\ULAY";
            string file = ulaypath + "\\layer_"+ meshfname+ ".txt";

            DateTime dt = DateTime.Now;

            string arg = "";

            arg += laytype;
            arg += ",";

            arg += points.Count;

            for (int i = 0; i < points.Count; i++)
            {
                arg += ",";
                arg += points[i].X;
                arg += ",";
                arg += points[i].Y;
            }

            arg += ",";
            arg += textBuf;
            arg += ",";
            arg += IconName;
            arg += ",";
            arg += LineWidth;
            arg += ",";
            arg += LineCol;
            arg += ",";
            arg += LineType;
            arg += ",";
            arg += PolyBrushType;
            arg += ",";
            arg += PolyBrush;
            arg += ",";
            arg += PolyBrush2;
            arg += ",";
            arg += PolyHatchStyle;
            arg += ",";
            arg += StringSize;
            arg += ",";
            arg += StringBrush;
            arg += ",";
            arg += StringType;
            arg += ",";
            arg += StringAngle;
            arg += ",";
            arg += StringFontType;
            arg += ",";
            arg += SymbolAngle;
            arg += ",";
            arg += meshfname;
            arg += ",";
            arg += addrname;
            arg += ",";
            arg += dt.ToString();

            Encoding enc = Encoding.GetEncoding("Shift_JIS");
            using (StreamWriter writer = new StreamWriter(file, true, enc))
            {
                writer.WriteLine(arg);
            }

        }

        public void DeleteUserLayer(String SelectedMesh,int SelectedLineCnt)
        {
            ulaypath = f1.datapath + "\\ULAY";

            string file = ulaypath + "\\layer_"+ SelectedMesh+ ".txt";
            string tmpfile = ulaypath + "\\layer_"+ SelectedMesh + ".tmp";

            if (File.Exists(file) == false)
            {
                return;
            }
            if (File.Exists(tmpfile) == true)
            {
                File.Delete(tmpfile);
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

                        if (linecnt != SelectedLineCnt)
                        {

                            Encoding enc = Encoding.GetEncoding("Shift_JIS");
                            using (StreamWriter writer = new StreamWriter(tmpfile, true, enc))
                            {
                                writer.WriteLine(line);
                            }
                        }

                    }
                    sr.Close();
                }
            }

            if (File.Exists(tmpfile) == true)
            {
                FileInfo fileInfo = new FileInfo(tmpfile);
                File.Delete(file);
                fileInfo.MoveTo(file);
            }
            else
            {
                File.Delete(file);
            }
        }

        public  void SelectUserLayer(String mesh,double lx,double ly,ref int LineCnt,ref String LineBuf)
        {

//            String meshfname;

  //          f1.drawZMD.PointToMesh(lx, ly, ref meshfname);

            ulaypath = f1.datapath + "\\ULAY";

            f1.MsgOut("SelectUserLayer");

            string file = ulaypath + "\\layer_"+mesh+".txt";

            if (File.Exists(file) == false)
            {
                return;
            }

            int linecnt = 0;

            Boolean IsHit = false;

            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("SHIFT_JIS")))
            {
                if (sr != null)
                {
                    while (sr.EndOfStream == false && IsHit==false)
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

                            double len = Math.Sqrt(Math.Abs(lx - mx) * Math.Abs(lx - mx) + Math.Abs(ly - my) * Math.Abs(ly - my));

                            if(len < 5.0)
                            {
                                f1.MsgOut("★HIT!!! linecnt:" + linecnt + ",len=" + len);

                                LineCnt = linecnt;
                                LineBuf = line;

                                IsHit = true;
                                break;
                            }

                        }
                    }
                    sr.Close();
                }
            }

        }
    }
}
