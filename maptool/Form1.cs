using com;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZMDCom;
using MapComLib;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Data.OleDb;
using COM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Mime.MediaTypeNames;
using MDXCom;

namespace maptool
{

    public partial class Form1 : Form
    {
        string DATA_PATH = System.Configuration.ConfigurationManager.AppSettings["DATA_PATH"];
        string BASE_AREA = System.Configuration.ConfigurationManager.AppSettings["BASE_AREA"];

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("User32.Dll")]
        static extern int GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        extern static IntPtr GetForegroundWindow();

        public string datapath;
        public string mappath;
        public string pngpath;

        public List<String> ICON_TBL=new List<string>();

        int BaseArea = 0;

        Double Init_ZX = -43380.024092448708;
        Double Init_ZY = -145071.41296201688;

        Bitmap canvas;//地図描画用canvas
        Graphics g;
        Bitmap canvas2;//画面描画用canvas
        Graphics g2;
        Bitmap canvas3;//アイコン用canvas
        Graphics g3;

        Bitmap hbmp;
        Graphics hg;

        //ガイド用リソース
        Pen g_pe;
        SolidBrush g_br;
        Font g_font;

        //描画作業用
        //Bitmap[] wkIcon = new Bitmap[5];

        double rate = 1.0;

        Point start;
        Point startbk;
        Point ScrollStart;
        Point ScrollEnd;

        bool IsDown = false;

        int MoveEvent = 0;

        int sc_lx = 0;
        int sc_ly = 0;

        double lx_b;
        double ly_b;

        int ClickMode = 0;

        int WorkPointCnt = 0;

        List<Point> points = new List<Point>();
        List<Point> wk_points = new List<Point>();

        List<List<Point>> points_free = new List<List<Point>>();
        List<List<Point>> wk_points_free = new List<List<Point>>();

        String textBuf = "";

        //選択レイヤ
        int SelectedLineCnt = 0;

        //編集中状態
        Boolean IsEdit=false;

        //
        Boolean IsPointSelect = false;

        //選択状態
        Boolean IsSelected = false;
        String SelectedString = "";
        String SelectedMesh="";

        //アイコン名
        String IconName;

        //
        //DrawZMDクラスのインスタンス
        //
        DrawZMD drawZMD = new DrawZMD();
        //
        //DrawUlayクラスのインスタンス
        //
        DrawUlay drawUlay = new DrawUlay();
        //
        //Addressクラスのインスタンス
        //
        Address address = new Address();
        //DrawMDXクラスのインスタンス
        //
        DrawMDX drawMDX = new DrawMDX();

        //
        //ログクラスのインスタンス
        //
        Log lg = new Log();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Left = (Screen.GetBounds(this).Width - Width) / 2;
            Top = (Screen.GetBounds(this).Height - Height) / 2;

            datapath = DATA_PATH;
            mappath = DATA_PATH + "\\ZENRIN";
            pngpath = DATA_PATH + "\\icon\\png";

            lg.LOGFNAME = DATA_PATH + "\\log\\maptool.log";
            lg.loglimit = 1000000;
            lg.logmax = 10;


            BaseArea = Int32.Parse(BASE_AREA);


            Left = (Screen.GetBounds(this).Width - Width) / 2;
            Top = (Screen.GetBounds(this).Height - Height) / 2;

            canvas = new Bitmap(pictureBox1.Width * 3, pictureBox1.Height * 3);
            g = Graphics.FromImage(canvas);

            canvas2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g2 = Graphics.FromImage(canvas2);

            canvas3 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g3 = Graphics.FromImage(canvas3);

            g.Clear(Color.White);
            g2.Clear(Color.White);
            g3.Clear(Color.White);

            g_pe = new Pen(Color.Green, 1);
            g_br = new SolidBrush(Color.Black);

            int font_size = 15;

            FontFamily fontFamily = new FontFamily("Meiryo UI");
            g_font = new Font(
                fontFamily,
                    font_size,
                   FontStyle.Regular,
                                      GraphicsUnit.Pixel);

            checkGuide.Checked = false;
            checkPictureBoxOnly.Checked = true;

            //ペン選択
            comboLineWidth.Items.Add("1");
            comboLineWidth.Items.Add("2");
            comboLineWidth.Items.Add("3");
            comboLineWidth.Items.Add("4");
            comboLineWidth.Items.Add("5");
            comboLineWidth.Items.Add("6");
            comboLineWidth.Items.Add("7");
            comboLineWidth.Items.Add("8");
            comboLineWidth.Items.Add("9");
            comboLineWidth.Items.Add("10");
            comboLineWidth.SelectedIndex = 0;

            comboLineType.Items.Add("実線");
            comboLineType.Items.Add("点線");
            comboLineType.Items.Add("ドット");
            comboLineType.Items.Add("一点鎖線");
            comboLineType.SelectedIndex = 0;

            comboPolyBrushType.Items.Add("ソリッド");
            comboPolyBrushType.Items.Add("ハッチ");
            comboPolyBrushType.SelectedIndex = 0;

            comboPolyHatchType.Items.Add("Vertical");
            comboPolyHatchType.Items.Add("Horizonta");
            comboPolyHatchType.Items.Add("Cross");
            comboPolyHatchType.SelectedIndex = 0;

            comboStringSize.Items.Add("16");
            comboStringSize.Items.Add("24");
            comboStringSize.Items.Add("48");
            comboStringSize.SelectedIndex = 0;

            comboStringType.Items.Add("横書き");
            comboStringType.Items.Add("縦書き");
            comboStringType.SelectedIndex = 0;

            comboStringAngle.Items.Add("-80");
            comboStringAngle.Items.Add("-70");
            comboStringAngle.Items.Add("-60");
            comboStringAngle.Items.Add("-50");
            comboStringAngle.Items.Add("-40");
            comboStringAngle.Items.Add("-30");
            comboStringAngle.Items.Add("-20");
            comboStringAngle.Items.Add("-10");
            comboStringAngle.Items.Add("0");
            comboStringAngle.Items.Add("10");
            comboStringAngle.Items.Add("20");
            comboStringAngle.Items.Add("30");
            comboStringAngle.Items.Add("40");
            comboStringAngle.Items.Add("50");
            comboStringAngle.Items.Add("60");
            comboStringAngle.Items.Add("70");
            comboStringAngle.Items.Add("80");
            comboStringAngle.SelectedIndex = 8;

            comboFontStyle.Items.Add("標準");
            comboFontStyle.Items.Add("太字");
            comboFontStyle.Items.Add("斜体");
            comboFontStyle.Items.Add("中央に線");
            comboFontStyle.Items.Add("下線付き");
            comboFontStyle.SelectedIndex = 0;

            comboSymbolAngle.Items.Add("-80");
            comboSymbolAngle.Items.Add("-70");
            comboSymbolAngle.Items.Add("-60");
            comboSymbolAngle.Items.Add("-50");
            comboSymbolAngle.Items.Add("-40");
            comboSymbolAngle.Items.Add("-30");
            comboSymbolAngle.Items.Add("-20");
            comboSymbolAngle.Items.Add("-10");
            comboSymbolAngle.Items.Add("0");
            comboSymbolAngle.Items.Add("10");
            comboSymbolAngle.Items.Add("20");
            comboSymbolAngle.Items.Add("30");
            comboSymbolAngle.Items.Add("40");
            comboSymbolAngle.Items.Add("50");
            comboSymbolAngle.Items.Add("60");
            comboSymbolAngle.Items.Add("70");
            comboSymbolAngle.Items.Add("80");
            comboSymbolAngle.SelectedIndex = 8;


            for(int i = 1; i <10; i++)
            {
                comboScale.Items.Add(i.ToString());
            }
            for (int i = 0; i < 100; i++)
            {
                comboScale.Items.Add(((i + 1) * 10).ToString());
            }

            btnLineColor.BackColor = Color.Black;
            btnBrushColorBack.BackColor = Color.Blue;
            btnBrushColorFore.BackColor = Color.Red;
            btnFontColor.BackColor = Color.Black;

            ResetButtonColor();

            textVal.Text = rate.ToString("N1");

            drawZMD.f1 = this;
            drawZMD.layer_disp[0] = true;
            drawZMD.layer_disp[1] = true;
            drawZMD.layer_disp[2] = true;
            drawZMD.layer_disp[3] = true;
            drawZMD.layer_disp[4] = true;
            drawZMD.layer_disp[5] = true;
            drawZMD.layer_disp[6] = true;

            drawUlay.f1 = this;

            address.f1 = this;

            LoadIconList();

            IconName = ICON_TBL[0];
            pictureBox1.Image = canvas2;

            pngpath = DATA_PATH + "\\icon\\png";
            Bitmap wIcon = new Bitmap(pngpath + "\\" + ICON_TBL[0]);

            g3.DrawImage(wIcon, 0, 0, pictureBox2.Width, pictureBox2.Height);
            wIcon.Dispose();
            pictureBox2.Image = canvas3;


            ClickMode = ConstDef.Mode_Default;

            pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);

            DrawCanvas(Init_ZX, Init_ZY);

        }

        void LoadIconList()
        {

            DirectoryInfo di = new DirectoryInfo(pngpath);

            // ディレクトリ直下のすべてのファイル一覧を取得する
            FileInfo[] fiAlls = di.GetFiles("*.png");
            foreach (FileInfo f in fiAlls)
            {
                ICON_TBL.Add(f.Name);
            }
        }

        public void ResetButtonColor()
        {
            btnEditLine.BackColor = Color.White;
            btnEditPolygon.BackColor = Color.White;
            btnEditSymbol.BackColor = Color.White;
            btnEditString.BackColor = Color.White;
            btnEditFreeline.BackColor = Color.White;
            btnSelect.BackColor = Color.White;

            PaintView();
        }
        public void SetButtonColor(System.Windows.Forms.Button btn,Color color)
        {
            ResetButtonColor();
            btn.BackColor = color;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            g_pe.Dispose();
            g_br.Dispose();
            g_font.Dispose();

//            wkIcon[0].Dispose();
  //          wkIcon[1].Dispose();
    //        wkIcon[2].Dispose();
      //      wkIcon[3].Dispose();
        //    wkIcon[4].Dispose();

            g.Dispose();
            g2.Dispose();
            g3.Dispose();
        }

        //
        //中心位置をリセットする
        //
        private void RedrawCanvas()
        {
            double lx = 0;
            double ly = 0;

            lx_b -= (double)start.X / 10.0 * rate;
            ly_b += (double)start.Y / 10.0 * rate;


            start.X = 0;
            start.Y = 0;

            lx = lx_b;
            ly = ly_b;

            DrawCanvas(lx, ly);

        }
        //
        //バッファ用Canvasに、ZMDとユーザレイヤを描画する
        //

        private void DrawCanvas(double lx, double ly)
        {
            g.Clear(Color.White);

            //rate = Double.Parse(textVal.Text);

            int offset_w = canvas.Width / 2;
            int offset_h = canvas.Height / 2;


            drawZMD.SetThreadParams(g, lx, ly, rate, offset_w, offset_h);

            // 元のカーソルを保持
            Cursor preCursor = Cursor.Current;

            // カーソルを待機カーソルに変更
            Cursor.Current = Cursors.WaitCursor;

            // 少し時間のかかる処理

            List<String> meshList = new List<String>();

            if(rate < 10)
            {
                drawZMD.DrawZenrin(g, lx, ly, rate, offset_w, offset_h,ref meshList);
            }
            else
            {

                int maptype;
                if (rate < 200)
                {
                    maptype = MDX.MDX_10000;
                    drawMDX.DrawMAPPLE(g, maptype, lx, ly, rate, offset_w, offset_h, ref meshList);
                }
                else
                {
                    maptype = MDX.MDX_200000;
                    drawMDX.DrawMAPPLE(g, maptype, lx, ly, rate, offset_w, offset_h, ref meshList);
                }

            }


            for (int i = 0; i < meshList.Count; i++)
            {
                drawUlay.DrawUserLayer(meshList[i],g, lx, ly, rate, offset_w, offset_h);
            }

            PaintView();

            // カーソルを元に戻す
            Cursor.Current = preCursor;

            lx_b = lx;
            ly_b = ly;

        }

        //
        //バッファ用Canvas のビットマップをpictureboxに描画する
        //
        public void PaintView()
        {
            try
            {
                g2.Clear(Color.White);

                Point pos = new Point();

                //pos.X = start.X - canvas.Width / 3;
                //pos.Y = start.Y - canvas.Height / 3;
                pos.X = start.X - canvas.Width / 3;
                pos.Y = start.Y - canvas.Height / 3;

                g2.DrawImage(canvas, pos);


                if (checkGuide.Checked == true)
                {
                    DrawGuide();
                }

                pictureBox1.Image = canvas2;
            }
            catch (Exception ex)
            {

            }

            //
            //編集中のアイテム描画
            //
            if (IsEdit == true)
            {
                DrawWork();
            }

            //
            //選択中のアイテム描画
            //
            if (IsSelected)
            {
                DrawWork2();
            }

        }

        public void DrawGuide()
        {
            Rectangle rect = new Rectangle();
            Rectangle rect2 = new Rectangle();

            rect.X = 0;
            rect.Y = 0;
            rect.Width = canvas2.Width;
            rect.Height = canvas2.Height;

            rect2.X = canvas2.Width / 2 - 20;
            rect2.Y = canvas2.Height / 2 - 20;
            rect2.Width = 40;
            rect2.Height = 40;

            g2.DrawLine(g_pe, 0, rect.Height / 2, rect.Width, rect.Height / 2);
            g2.DrawLine(g_pe, rect.Width / 2, 0, rect.Width / 2, rect.Height);


            double dx = lx_b - (double)start.X / (10 * rate);
            double dy = ly_b + (double)start.Y / (10 * rate);


            var format = new StringFormat();
            format.Alignment = StringAlignment.Near;      // 左右方向は中心寄せ
            format.LineAlignment = StringAlignment.Near;  // 上下方向は中心寄せ

            g2.SmoothingMode = SmoothingMode.AntiAlias;
            g2.DrawString("[" + dx.ToString("N3") + "]-[" + dy.ToString("N3") + "]", g_font, g_br, 10, canvas2.Height - 20);

            g2.DrawRectangle(g_pe, rect2);
        }

        public void GetClickPoint(int sx, int sy)
        {
            Rectangle rect = new Rectangle();

            rect.Width = canvas2.Width;
            rect.Height = canvas2.Height;

            int wx = (rect.Width / 2) - sx + start.X;
            int wy = (rect.Height / 2) - sy + start.Y;

            double dx = lx_b - (double)wx / (10 * rate);
            double dy = ly_b + (double)wy / (10 * rate);

            MsgOut("sx=" + sx + ".y=" + sy);
            MsgOut("dx=" + dx + ".dy=" + dy);
            MsgOut("rate=" +rate);
        }

        public void GetPointInfo(int sx, int sy)
        {
            Rectangle rect = new Rectangle();

            rect.Width = canvas2.Width;
            rect.Height = canvas2.Height;

            int wx = (rect.Width / 2) - sx + start.X;
            int wy = (rect.Height / 2) - sy + start.Y;


            double dx = lx_b - (double)wx / (10 / rate);
            double dy = ly_b + (double)wy / (10 / rate);

            MsgOut("x=" + dx + ".y=" + dy);

            drawZMD.GetPointInfo(dx, dy);

            FormPointInfo fpi = new FormPointInfo();
            fpi.pf = drawZMD.pInfo;

            fpi.ShowDialog();

        }
        public void SelectInfo(int sx, int sy)
        {

            Rectangle rect = new Rectangle();

            rect.Width = canvas2.Width;
            rect.Height = canvas2.Height;

            int wx = (rect.Width / 2) - sx + start.X;
            int wy = (rect.Height / 2) - sy + start.Y;


            double dx = lx_b - (double)wx / (10 / rate);
            double dy = ly_b + (double)wy / (10 / rate);

            MsgOut("select x=" + dx + ".y=" + dy);

            int LineCnt = 0;
            String LineBuf = "";

            String meshfname = "";
            drawZMD.PointToMesh((int)(dx*1000), (int)(dy*1000), ref meshfname);

            drawUlay.SelectUserLayer(meshfname,dx, dy, ref LineCnt, ref LineBuf);
            if (LineCnt > 0)
            {
                SelectedLineCnt = LineCnt;
                SelectedString = LineBuf;
                SelectedMesh = meshfname;

                IsSelected = true;

                ClickMode = ConstDef.Mode_Default;
                PaintView();
            }
        }


        public void DrawWork()
        {
            //線
            int LineWidth = Int32.Parse(comboLineWidth.Text);
            Color LineColor = btnLineColor.BackColor;
            DashStyle LineType = drawUlay.LINETYPE_TBL[comboLineType.SelectedIndex];

            //ポリゴン
            int alpha = 255;
            if (checkTransParent.Checked == true)
            {
                alpha = 128;
            }

            Color PolyBrush = Color.FromArgb(alpha, btnBrushColorFore.BackColor);
            Color PolyBrush2 = Color.FromArgb(alpha, btnBrushColorBack.BackColor);

            int PolyBrushType = comboPolyBrushType.SelectedIndex;
            HatchStyle PolyHatchStyle = drawUlay.POLYHATCHSTYLE_TBL[comboPolyHatchType.SelectedIndex];

            //文字
            int StringSize = Int32.Parse(comboStringSize.Text);
            Color StringBrush = btnFontColor.BackColor;
            int StringType = drawUlay.STRINGTYPE_TBL[comboStringType.SelectedIndex];
            int StringAngle = Int32.Parse(comboStringAngle.Text);
            FontStyle StringFontStyle = drawUlay.FONTSTYLE_TBL[comboFontStyle.SelectedIndex];

            //シンボル
            Double SymbolAngle = Double.Parse(comboSymbolAngle.Text);

            Pen w_pe = new Pen(LineColor, LineWidth);
            //w_pe.EndCap=LineCap.ArrowAnchor; 
            w_pe.DashStyle = LineType;

            Font w_font = new Font("ＭＳ Ｐゴシック", StringSize, StringFontStyle);

            SolidBrush w_br_s = new SolidBrush(PolyBrush);
            HatchBrush w_br_h = new HatchBrush(PolyHatchStyle, PolyBrush, PolyBrush2);

            SolidBrush w_sbr = new SolidBrush(StringBrush);

            Point[] points = new Point[wk_points.Count];
            String text = textString.Text;
            textBuf = "";

            for (int i = 0; i < wk_points.Count; i++)
            {
                points[i].X = wk_points[i].X;
                points[i].Y = wk_points[i].Y;
            }

            switch (ClickMode)
            {
                case ConstDef.Mode_Line:
                    if (wk_points.Count > 1)
                    {
                        g2.DrawLines(w_pe, points);
                    }
                    break;
                case ConstDef.Mode_Polygon:
                    if (wk_points.Count > 1)
                    {
                        g2.DrawPolygon(w_pe, points);

                        if (PolyBrushType == 0)
                        {
                            g2.FillPolygon(w_br_s, points);
                        }
                        else
                        {
                            g2.FillPolygon(w_br_h, points);
                        }

                    }
                    break;
                case ConstDef.Mode_Symbol:
                    if (wk_points.Count > 0)
                    {
                        pngpath = DATA_PATH + "\\icon\\png";
                        Bitmap wkIcon = new Bitmap(pngpath + "\\" + IconName);

                        double deg = SymbolAngle;
                        Point iconP = new Point(points[0].X, points[0].Y);
                        int iconSize = 32;

                       DrawIcon(g2,wkIcon, deg, iconP, iconSize);

                        //g2.DrawImage(wkIcon, points[0].X - 16, points[0].Y - 16);
                        wkIcon.Dispose();
                    }
                    break;
                case ConstDef.Mode_String:
                    if (wk_points.Count > 0)
                    {
                        textBuf = text;
                        var format = new StringFormat();
                        format.Alignment = StringAlignment.Near;      // 左右方向は中心寄せ
                        format.LineAlignment = StringAlignment.Near;  // 上下方向は中心寄せ

                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        if (StringType == 0)
                        {


                            format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                            StringCOM.DrawString(g2, text, w_font, w_sbr, points[0].X, points[0].Y, StringAngle, format);
                        }
                        else
                        {
                            format.FormatFlags = StringFormatFlags.DirectionVertical;
                            StringCOM.DrawString(g2, text, w_font, w_sbr, points[0].X, points[0].Y, StringAngle, format);
                        }
                    }
                    break;
                case ConstDef.Mode_Freeline:

                    //
                    for(int j = 0; j < wk_points_free.Count(); j++)
                    {
                        Point[] m_points = new Point[wk_points_free[j].Count];

                        for (int k=0;k< wk_points_free[j].Count; k++)
                        {

                            m_points[k].X = wk_points_free[j][k].X;
                            m_points[k].Y = wk_points_free[j][k].Y;
                        }
                        g2.DrawLines(w_pe, m_points);
                    }

                    if (wk_points.Count > 1)
                    {
                        g2.DrawLines(w_pe, points);
                    }
                    break;
            }
            
            w_pe.Dispose();
            w_br_s.Dispose();
            w_br_h.Dispose();
            w_font.Dispose();
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

new Point(p.X - (int)(size * Math.Sin(rad)), p.Y-(int)(size * Math.Cos(rad))),   // destination for upper-left point of
new Point(p.X + (int)(size * Math.Cos(rad)) - (int)(size * Math.Sin(rad)), p.Y - (int)(size * Math.Cos(rad)) - (int)(size * Math.Sin(rad))),  // destination for upper-right point of
new Point(p.X, p.Y)};  // destination for lower-left point of

            g.DrawImage(image, destinationPoints);
        }


        public void DrawWork2()
        {
            String LineBuf = SelectedString;

            string[] param = LineBuf.Split(',');

            int offset_w = canvas2.Width / 2;
            int offset_h = canvas2.Height / 2;

            double lx = 0;
            double ly = 0;

            lx = lx_b;
            ly = ly_b;


            int laytype = Int32.Parse(param[0]);

            int point_start = 1;

            int cnt = Int32.Parse(param[point_start]);
            Point[] points = new Point[cnt];

            for (int i = 0; i < cnt; i++)
            {
                double mx = double.Parse(param[i * 2 + point_start + 1]) / 1000;
                double my = double.Parse(param[i * 2 + point_start + 2]) / 1000;

                points[i].X = offset_w + (int)((mx - lx) * (10 / rate)) + start.X;
                points[i].Y = offset_h - (int)((my - ly) * (10 / rate)) + start.Y;

            }

            int attr_start = 2 + cnt * 2;

            String StringText = param[attr_start];
//            int IconIdx = Int32.Parse(param[attr_start + 1]);
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

            Pen u_pe = new Pen(ColorTranslator.FromHtml(LineCol), LineWidth);
            DashStyle u_LineType = drawUlay.LINETYPE_TBL[LineType];
            u_pe.DashStyle = u_LineType;

            HatchStyle u_PolyHatchStyle = drawUlay.POLYHATCHSTYLE_TBL[PolyHatchStyle];


            //Color PolyBrush = Color.FromArgb(alpha, btnBrushColorFore.BackColor);
            //Color PolyBrush2 = Color.FromArgb(alpha, btnBrushColorBack.BackColor);

            SolidBrush u_pbr_s = new SolidBrush(ColorTranslator.FromHtml(PolyBrush));
            HatchBrush u_pbr_h = new HatchBrush(u_PolyHatchStyle, ColorTranslator.FromHtml(PolyBrush), ColorTranslator.FromHtml(PolyBrush2));

            SolidBrush u_sbr = new SolidBrush(ColorTranslator.FromHtml(StringBrush));
            Font u_font = new Font("ＭＳ Ｐゴシック", StringSize);
            int u_StringType = drawUlay.STRINGTYPE_TBL[StringType];
            int u_StringAngle = StringAngle;

            //if (linecnt == SelectedLineCnt)
            //{
                u_pe = new Pen(drawUlay.SELECTED_LINECOL, drawUlay.SELECTED_LINEWIDTH);
                u_sbr = new SolidBrush(drawUlay.SELECTED_STRINGBRUSH);
            //}

            switch (laytype)
            {
                case ConstDef.Mode_Line://line
                    if (cnt > 1)
                    {
                        g2.DrawLines(u_pe, points);
                    }
                    break;
                case ConstDef.Mode_Polygon://polygon
                    if (cnt > 1)
                    {
                        g2.DrawPolygon(u_pe, points);
                        if (PolyBrushType == 0)
                        {
                            g2.FillPolygon(u_pbr_s, points);
                        }
                        else
                        {
                            g2.FillPolygon(u_pbr_h, points);
                        }
                    }
                    break;
                case ConstDef.Mode_Symbol:

                    g2.DrawRectangle(u_pe, points[0].X, points[0].Y-32, 32, 32);

                    break;
                case ConstDef.Mode_String:

                    var format = new StringFormat();
                    format.Alignment = StringAlignment.Near;      // 左右方向は中心寄せ
                    format.LineAlignment = StringAlignment.Near;  // 上下方向は中心寄せ

                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    if (u_StringType == 0)
                    {
                        format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                        StringCOM.DrawString(g2, StringText, u_font, u_sbr, points[0].X, points[0].Y, u_StringAngle, format);
                    }
                    else
                    {
                        format.FormatFlags = StringFormatFlags.DirectionVertical;
                        StringCOM.DrawString(g2, StringText, u_font, u_sbr, points[0].X, points[0].Y, u_StringAngle, format);
                    }

                    break;
                case ConstDef.Mode_Freeline://
                    if (cnt > 1)
                    {
                        g2.DrawLines(u_pe, points);
                    }
                    break;
            }

            u_pe.Dispose();
            u_pbr_s.Dispose();
            u_pbr_h.Dispose();
            u_sbr.Dispose();
            u_font.Dispose();
        }


        public void AddData(int sx, int sy)
        {
            Rectangle rect = new Rectangle();

            rect.Width = canvas2.Width;
            rect.Height = canvas2.Height;

            int wx = (rect.Width / 2) - sx + start.X;
            int wy = (rect.Height / 2) - sy + start.Y;


            double dx = lx_b - (double)wx / (10 / rate);
            double dy = ly_b + (double)wy / (10 / rate);

            MsgOut("LINE :" + WorkPointCnt + " x=" + dx + ",y=" + dy);

            Point wp = new Point();

            wp.X = sx;
            wp.Y = sy;

            wk_points.Add(wp);

            Point p = new Point();
            p.X = (int)(dx * 1000);//(mm)
            p.Y = (int)(dy * 1000);//(mm)

            points.Add(p);

            WorkPointCnt++;

        }
        private void SaveLayer()
        {

            if (MessageBox.Show("保存しますか？", "確認", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                ClickMode = ConstDef.Mode_Default;
                SelectedLineCnt = 0;
                IsSelected = false;
                ResetButtonColor();

                return;
            }
            int alpha = 255;

            //線
            int LineWidth = Int32.Parse(comboLineWidth.Text);

            //ポリゴン
            alpha = 255;
            String LineCol = "0x"+Color.FromArgb(alpha, btnLineColor.BackColor).Name;
            
            int LineType = comboLineType.SelectedIndex;
            int PolyBrushType = comboPolyBrushType.SelectedIndex;
            alpha = 255;
            if (checkTransParent.Checked == true)
            {
                alpha = 128;
            }
            String PolyBrush = "0x" + Color.FromArgb(alpha, btnBrushColorFore.BackColor).Name;
            String PolyBrush2 = "0x" + Color.FromArgb(alpha, btnBrushColorBack.BackColor).Name;
            
            int PolyHatchStyle = comboPolyHatchType.SelectedIndex;
            int StringSize = Int32.Parse(comboStringSize.Text);
 
            alpha = 255;
            //文字
            String StringBrush = "0x" + Color.FromArgb(alpha, btnFontColor.BackColor).Name;
            int StringType = comboStringType.SelectedIndex;
            int StringAngle = Int32.Parse(comboStringAngle.Text);
            int StringFontType = comboFontStyle.SelectedIndex;

            //シンボル
            Double SymbolAngle = Double.Parse(comboSymbolAngle.Text);

            String meshfname = "";

            switch (ClickMode)
            {
                case ConstDef.Mode_Line:
                    drawZMD.PointToMesh(points[0].X, points[0].Y, ref meshfname);
                    drawZMD.GetPointInfo((double)points[0].X/1000, (double)points[0].Y/1000);
                    drawUlay.SaveUserlayer(ClickMode, points, textBuf, IconName, LineWidth, LineCol, LineType, PolyBrushType,PolyBrush, PolyBrush2, PolyHatchStyle,StringSize, StringBrush,StringType,StringAngle, StringFontType, SymbolAngle,meshfname, drawZMD.pInfo.area_addrname);
                    break;
                case ConstDef.Mode_Polygon:
                    drawZMD.PointToMesh(points[0].X, points[0].Y, ref meshfname);
                    drawZMD.GetPointInfo((double)points[0].X / 1000, (double)points[0].Y / 1000);
                    drawUlay.SaveUserlayer(ClickMode, points, textBuf, IconName, LineWidth, LineCol, LineType, PolyBrushType, PolyBrush, PolyBrush2, PolyHatchStyle, StringSize, StringBrush, StringType, StringAngle, StringFontType,SymbolAngle, meshfname, drawZMD.pInfo.area_addrname);
                    break;
                case ConstDef.Mode_Symbol:
                    drawZMD.PointToMesh(points[0].X, points[0].Y, ref meshfname);
                    drawZMD.GetPointInfo((double)points[0].X / 1000, (double)points[0].Y / 1000);
                    drawUlay.SaveUserlayer(ClickMode, points, textBuf, IconName, LineWidth, LineCol, LineType, PolyBrushType, PolyBrush, PolyBrush2, PolyHatchStyle, StringSize, StringBrush, StringType, StringAngle, StringFontType,SymbolAngle, meshfname,drawZMD.pInfo.area_addrname);
                    break;
                case ConstDef.Mode_String:
                    drawZMD.PointToMesh(points[0].X, points[0].Y, ref meshfname);
                    drawZMD.GetPointInfo((double)points[0].X / 1000, (double)points[0].Y / 1000);
                    drawUlay.SaveUserlayer(ClickMode, points, textBuf, IconName, LineWidth, LineCol, LineType, PolyBrushType, PolyBrush, PolyBrush2, PolyHatchStyle, StringSize, StringBrush, StringType, StringAngle, StringFontType,SymbolAngle, meshfname, drawZMD.pInfo.area_addrname);
                    break;
                case ConstDef.Mode_Freeline:

                    for (int j = 0; j < points_free.Count(); j++)
                    {
                        List<Point> m_points = new List<Point>();

                        for (int k = 0; k < points_free[j].Count; k++)
                        {
                            Point p = new Point();
                            p.X = points_free[j][k].X;
                            p.Y = points_free[j][k].Y;
                            m_points.Add(p);
                        }
                        drawZMD.PointToMesh(m_points[0].X, m_points[0].Y, ref meshfname);
                        drawZMD.GetPointInfo((double)m_points[0].X / 1000, (double)m_points[0].Y / 1000);
                        drawUlay.SaveUserlayer(ClickMode, m_points, textBuf, IconName, LineWidth, LineCol, LineType, PolyBrushType, PolyBrush, PolyBrush2, PolyHatchStyle, StringSize, StringBrush, StringType, StringAngle, StringFontType, SymbolAngle,meshfname, drawZMD.pInfo.area_addrname);
                    }


                    break;
            }
            ClickMode = ConstDef.Mode_Default;

            WorkPointCnt = 0;
            points.Clear();
            wk_points.Clear();
            wk_points_free.Clear();
            points_free.Clear();
            RedrawCanvas();
            ResetButtonColor();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string extension = System.IO.Path.GetExtension(saveFileDialog1.FileName);

            System.Drawing.Image im;

            if (checkPictureBoxOnly.Checked == true)
            {
                im = pictureBox1.Image;
            }
            else
            {
                im = hbmp;
            }

            switch (extension.ToUpper())
            {
                case ".GIF":
                    im.Save(saveFileDialog1.FileName, ImageFormat.Gif);
                    break;
                case ".JPEG":
                    im.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
                    break;
                case ".PNG":
                    im.Save(saveFileDialog1.FileName, ImageFormat.Png);
                    break;
            }
        }

        //
        //<Mouse Down>
        //

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            IsDown = true;
            MoveEvent = 0;

            switch (ClickMode)
            {
                case ConstDef.Mode_Default:
                    ScrollStart.X = e.X;
                    ScrollStart.Y = e.Y;

                    startbk.X = start.X;
                    startbk.Y = start.Y;

                    //MsgOut("e.X=" + e.X + ",e.Y=" + e.Y);


                    GetClickPoint(e.X, e.Y);
                    break;
                case ConstDef.Mode_Line:
                    break;
                case ConstDef.Mode_Polygon:
                    break;
                case ConstDef.Mode_Symbol:
                    break;
                case ConstDef.Mode_String:
                    break;
                case ConstDef.Mode_Freeline:
                    break;
                case ConstDef.Mode_Select:
                    break;
                case ConstDef.Mode_GetInfo:
                    break;
            }
        }

        //
        //<Mouse Up>
        //
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MoveEvent = 0;

            switch (ClickMode)
            {
                case ConstDef.Mode_Default:
                    if (IsDown == true)
                    {
                        IsDown = false;
                        startbk.X = start.X;
                        startbk.Y = start.Y;

                        double dx = lx_b - (double)start.X / (10 * rate);
                        double dy = ly_b + (double)start.Y / (10 * rate);


                        if (Math.Abs(start.X) > 600 || Math.Abs(start.Y) > 600)
                        {
                            RedrawCanvas();
                        }
                    }
                    else
                    {
                        PaintView();
                    }
                    break;
                case ConstDef.Mode_Line:
                    if (IsDown == true)
                    {
                        IsDown = false;
                    }
                    AddData(e.X, e.Y);
                    PaintView();
                    break;
                case ConstDef.Mode_Polygon:
                    if (IsDown == true)
                    {
                        IsDown = false;
                    }
                    AddData(e.X, e.Y);
                    PaintView();
                    break;
                case ConstDef.Mode_Symbol:
                    if (IsDown == true)
                    {
                        IsDown = false;
                    }
                    AddData(e.X, e.Y);
                    PaintView();
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_String:
                    if (IsDown == true)
                    {
                        IsDown = false;
                    }
                    AddData(e.X, e.Y);
                    PaintView();
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_Freeline:
                    if (IsDown == true)
                    {
                        List<Point> m_points = new List<Point>();

                        for(int i = 0; i < wk_points.Count;i++)
                        {
                            Point p = new Point();
                            p.X = wk_points[i].X;
                            p.Y = wk_points[i].Y;
                            m_points.Add(p);
                        }

                        wk_points_free.Add(m_points);

                        List<Point> m_points2 = new List<Point>();

                        for (int i = 0; i < points.Count; i++)
                        {
                            Point p = new Point();
                            p.X = points[i].X;
                            p.Y = points[i].Y;
                            m_points2.Add(p);
                        }

                        points_free.Add(m_points2);

                        IsDown = false;
                        WorkPointCnt = 0;
                        points.Clear();
                        wk_points.Clear();
                        MoveEvent = 0;

                    }
                    break;
                case ConstDef.Mode_Select:
                    IsDown = false;

                    SelectInfo(e.X, e.Y);

                    break;
                case ConstDef.Mode_GetInfo:
                    IsDown = false;
                    ClickMode = ConstDef.Mode_Default;

                    GetPointInfo(e.X, e.Y);
                    break;
            }
        }

        //
        //<Mouse Move>
        //
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            switch (ClickMode)
            {
                case ConstDef.Mode_Default:
                    if (IsDown == true)
                    {
                        MoveEvent++;
                        if (MoveEvent > 10000)
                        {
                            MoveEvent = 0;
                        }

                        if (MoveEvent % 10 == 0)
                        {
                            ScrollEnd.X = e.X;
                            ScrollEnd.Y = e.Y;

                            sc_lx = ScrollEnd.X - ScrollStart.X;
                            sc_ly = ScrollEnd.Y - ScrollStart.Y;

                            start.X = startbk.X + sc_lx;
                            start.Y = startbk.Y + sc_ly;


                            MsgOut("start.X=" + start.X.ToString() + ",start.Y=" + start.Y.ToString());

                            PaintView();
                        }

                    }
                    break;
                case ConstDef.Mode_Line:

                    if(IsPointSelect== true)
                    {
                        if (wk_points.Count > 0)
                        {
                            PaintView();

                            //線
                            int LineWidth = Int32.Parse(comboLineWidth.Text);
                            Color LineColor = btnLineColor.BackColor;
                            DashStyle LineType = drawUlay.LINETYPE_TBL[comboLineType.SelectedIndex];

                            Pen w_pe = new Pen(LineColor, LineWidth);
                            w_pe.DashStyle = LineType;

                            int sx = wk_points[wk_points.Count - 1].X;
                            int sy = wk_points[wk_points.Count - 1].Y;

                            g2.DrawLine(w_pe, sx, sy, e.X, e.Y);

                            w_pe.Dispose();
                            //                        PaintView();
                        }
                    }

                    break;
                case ConstDef.Mode_Polygon:
                    if (IsPointSelect == true)
                    {
                        if (wk_points.Count > 0)
                        {
                            PaintView();

                            //線
                            int LineWidth = Int32.Parse(comboLineWidth.Text);
                            Color LineColor = btnLineColor.BackColor;
                            DashStyle LineType = drawUlay.LINETYPE_TBL[comboLineType.SelectedIndex];

                            Pen w_pe = new Pen(LineColor, LineWidth);
                            w_pe.DashStyle = LineType;

                            int sx = wk_points[wk_points.Count - 1].X;
                            int sy = wk_points[wk_points.Count - 1].Y;

                            g2.DrawLine(w_pe, sx, sy, e.X, e.Y);

                            w_pe.Dispose();
                            //                        PaintView();
                        }
                    }
                    break;
                case ConstDef.Mode_Symbol:
                    if (IsPointSelect == true)
                    {
                        PaintView();

                        pngpath = DATA_PATH + "\\icon\\png";
                        Bitmap wkIcon = new Bitmap(pngpath + "\\" + IconName);

                        Double SymbolAngle = Double.Parse(comboSymbolAngle.Text);

                        double deg = SymbolAngle;
                        Point iconP = new Point(e.X, e.Y);
                        int iconSize = 32;

                        DrawIcon(g2, wkIcon, deg, iconP, iconSize);


                        //                        g2.DrawImage(wkIcon, e.X - 16, e.Y - 16);
                        wkIcon.Dispose();

                    }
                    break;
                case ConstDef.Mode_String:
                    if (IsPointSelect == true)
                    {
                        PaintView();
                        //文字
                        int StringSize = Int32.Parse(comboStringSize.Text);
                        Color StringBrush = btnFontColor.BackColor;
                        int StringType = drawUlay.STRINGTYPE_TBL[comboStringType.SelectedIndex];
                        int StringAngle = Int32.Parse(comboStringAngle.Text);
                        FontStyle StringFontStyle = drawUlay.FONTSTYLE_TBL[comboFontStyle.SelectedIndex];

                        Font w_font = new Font("ＭＳ Ｐゴシック", StringSize, StringFontStyle);

                        SolidBrush w_sbr = new SolidBrush(StringBrush);

                        String text = textString.Text;
                        textBuf = "";


                        textBuf = text;
                        var format = new StringFormat();
                        format.Alignment = StringAlignment.Near;      // 左右方向は中心寄せ
                        format.LineAlignment = StringAlignment.Near;  // 上下方向は中心寄せ

                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        if (StringType == 0)
                        {
                            format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                            StringCOM.DrawString(g2, text, w_font, w_sbr,e.X, e.Y, StringAngle, format);
                        }
                        else
                        {
                            format.FormatFlags = StringFormatFlags.DirectionVertical;
                            StringCOM.DrawString(g2, text, w_font, w_sbr, e.X, e.Y, StringAngle, format);
                        }

                    }
                    break;
                case ConstDef.Mode_Freeline:
                    if (IsDown == true)
                    {
                        MoveEvent++;
                        if (MoveEvent > 10000)
                        {
                            MoveEvent = 0;
                        }

                        if (MoveEvent % 5 == 0)
                        {
                            AddData(e.X, e.Y);
                            PaintView();
                        }
                    }
                    break;
                case ConstDef.Mode_Select:
                    break;
                case ConstDef.Mode_GetInfo:
                    break;
            }
        }
        //
        //<Mouse DoubleClick>
        //
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            /*
            switch (ClickMode)
            {
                case ConstDef.Mode_Line:
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_Polygon:
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_Symbol:
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_String:
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_Freeline:
                    break;
                case ConstDef.Mode_Select:
                    break;
                case ConstDef.Mode_GetInfo:
                    break;
            }
            */
        }

        //
        // <ボタン処理> 
        // 
        //
        // <SAVE ボタン処理> 
        // 

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            SaveLayer();
        }

        //
        // <CLOSE ボタン処理> 
        // 
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void btnEditLine_Click(object sender, EventArgs e)
        {
            IsEdit = true;
            IsPointSelect = true;

            ClickMode = ConstDef.Mode_Line;
            WorkPointCnt = 0;
            wk_points.Clear();
            points.Clear();

            SetButtonColor((System.Windows.Forms.Button)sender, Color.Orange);
        }

        private void btnEditPolygon_Click(object sender, EventArgs e)
        {
            IsEdit = true;
            IsPointSelect = true;

            ClickMode = ConstDef.Mode_Polygon;
            WorkPointCnt = 0;
            wk_points.Clear();
            points.Clear();

            SetButtonColor((System.Windows.Forms.Button)sender, Color.Orange);
        }
        private void btnEditFreeline_Click(object sender, EventArgs e)
        {
            IsEdit = true;


            MoveEvent = 0;

            ClickMode = ConstDef.Mode_Freeline;
            WorkPointCnt = 0;
            wk_points.Clear();
            points.Clear();
            wk_points_free.Clear();
            points_free.Clear();

            SetButtonColor((System.Windows.Forms.Button)sender, Color.Orange);
        }

        private void btnEditSymbol_Click(object sender, EventArgs e)
        {
            IsEdit = true;
            IsPointSelect = true;


            ClickMode = ConstDef.Mode_Symbol;
            WorkPointCnt = 0;
            wk_points.Clear();
            points.Clear();

            SetButtonColor((System.Windows.Forms.Button)sender, Color.Orange);
        }
        private void btnEditString_Click(object sender, EventArgs e)
        {
            IsEdit = true;
            IsPointSelect = true;

            ClickMode = ConstDef.Mode_String;
            WorkPointCnt = 0;
            wk_points.Clear();
            points.Clear();

            SetButtonColor((System.Windows.Forms.Button)sender, Color.Orange);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            IsEdit = false;

            SetButtonColor((System.Windows.Forms.Button)sender, Color.Orange);

            PaintView();

            ClickMode = ConstDef.Mode_Select;
        }


        private void btnEditCancel_Click(object sender, EventArgs e)
        {
            IsEdit = false;

            ResetButtonColor();

            WorkPointCnt = 0;
            points.Clear();
            wk_points.Clear();
            wk_points_free.Clear();
            points_free.Clear();

            SelectedLineCnt = 0;

            IsSelected = false;

            PaintView();

            /*
            try
                {
                    g2.Clear(Color.White);

                    Point pos = new Point();

                    pos.X = start.X - canvas.Width / 3;
                    pos.Y = start.Y - canvas.Height / 3;
                    pos.X = start.X - canvas.Width / 3;
                    pos.Y = start.Y - canvas.Height / 3;

                    g2.DrawImage(canvas, pos);


                    if (checkGuide.Checked == true)
                    {
                        DrawGuide();
                    }

                    pictureBox1.Image = canvas2;
                }
                catch (Exception ex)
                {

                }
            */
            ClickMode = ConstDef.Mode_Default;
        }

        private void btnAddressSearch_Click(object sender, EventArgs e)
        {
            FormAddress fa = new FormAddress();
            fa.f1 = this;
            fa.Show();
        }


        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;

            if (checkPictureBoxOnly.Checked == true)
            {
            }
            else
            {
                RECT r;
                IntPtr active = GetForegroundWindow();
                GetWindowRect(active, out r);
                Rectangle rect = new Rectangle(r.left, r.top, r.right - r.left, r.bottom - r.top);

                hbmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                hg = Graphics.FromImage(hbmp);

                hg.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }

            string save_dir = datapath + "\\" + "Image";

            if (Directory.Exists(save_dir) == false)
            {
                Directory.CreateDirectory(save_dir);
            }

            saveFileDialog1.InitialDirectory = save_dir;
            saveFileDialog1.Filter = "PNG形式|*.png|GIF形式|*.gif|JPEG形式|*.jpeg";
            saveFileDialog1.FileName = "sample_" + dt.ToString("yyyyMMddHHmmss") + ".png";
            saveFileDialog1.ShowDialog();

        }
        //
        //<UP ボタン>
        //
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (rate > 1)
            {
                rate -= 0.1;
                textVal.Text = rate.ToString("N1");

                double lx = 0;
                double ly = 0;

//                lx = double.Parse(textLx.Text);
//                ly = double.Parse(textLy.Text);
                lx = lx_b;
                ly = ly_b;

                DrawCanvas(lx, ly);
            }
        }

        //
        //<DOWN ボタン>
        //
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (rate <= 100)
            {
                rate += 0.1;
                textVal.Text = rate.ToString("N1");

                double lx = 0;
                double ly = 0;

//                lx = double.Parse(textLx.Text);
//                ly = double.Parse(textLy.Text);
                lx = lx_b;
                ly = ly_b;

                DrawCanvas(lx, ly);

            }
        }

        //
        //メッセージ出力
        //

        public void MsgOut(string msg)
        {

            DateTime dt = DateTime.Now;
            lg.LogOut(dt.ToString("G") + "," + msg);

            if (checkMsg.Checked == true)
            {
                listBox1.Items.Add(dt.ToString("G") + "," + msg);

                int cnt = listBox1.Items.Count;
                if (cnt > 0)
                {
                    listBox1.SelectedIndex = cnt - 1;
                }
            }

        }

        //
        public void SetLayerCheck(Boolean[] layer_disp)
        {
            for(int i = 0;i < 7; i++)
            {
                drawZMD.layer_disp[i] = layer_disp[i];
            }

            RedrawCanvas();

        }

        //
        //<検索画面からの呼び出し
        //
        public void CallDrawCanvas(double lx,double ly)
        {
             lx_b = lx;
             ly_b = ly;

             start.X = 0;
             start.Y = 0;

            rate = Double.Parse(textVal.Text);

            DrawCanvas(lx, ly);
        }
        public void CallDrawCanvas2(double lx, double ly,double s_rate)
        {
            rate = s_rate;

            lx_b = lx;
            ly_b = ly;

            start.X = 0;
            start.Y = 0;

            DrawCanvas(lx, ly);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(IsSelected == true && SelectedLineCnt>0)
            {
                if (MessageBox.Show("削除しますか？", "確認", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    drawUlay.DeleteUserLayer(SelectedMesh,SelectedLineCnt);
                    RedrawCanvas();
                }
            }
            ClickMode = ConstDef.Mode_Default;
            SelectedLineCnt = 0;
            IsSelected = false;
            ResetButtonColor();
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            ClickMode = ConstDef.Mode_GetInfo;
        }

        private void bntDataList_Click(object sender, EventArgs e)
        {
            DataList dt = new DataList();
            dt.f1 = this;
            dt.ShowDialog();
        }

        private void btnMapLayer_Click(object sender, EventArgs e)
        {
            FormLayer flay = new FormLayer();
            flay.f1 = this;

            for(int i = 0; i < 7; i++)
            {
                flay.layer_disp[i] = drawZMD.layer_disp[i];
            }
            flay.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
                timer1.Enabled = true;
                timer1.Interval = 500;
            }
            else
            {
                timer1.Enabled = true;
                timer1.Interval = 500;
            }

            */
            if (rate < 2.0)
            {
                rate += 0.1;
                textVal.Text = rate.ToString("N1");
            }

            try
            {
                g2.Clear(Color.White);

  //              Point pos = new Point();

                //pos.X = start.X - canvas.Width / 3;
                //pos.Y = start.Y - canvas.Height / 3;
    //            pos.X = start.X - canvas.Width / 3;
      //          pos.Y = start.Y - canvas.Height / 3;

//                g2.DrawImage(canvas, pos);

                Rectangle srcRect = new Rectangle();

                srcRect.X = canvas2.Width;
                srcRect.Y = canvas2.Width;

                srcRect.Width = (int)(canvas2.Width/(1/rate));
                srcRect.Height =(int)(canvas2.Height/(1/rate));

                Rectangle destRect = new Rectangle();
                destRect.X = 0;
                destRect.Y = 0;
                destRect.Width = canvas2.Width;
                destRect.Height = canvas2.Height;

                g2.DrawImage(canvas, destRect, srcRect, GraphicsUnit.Pixel);
                /*

                Rectangle destRect = new Rectangle();
                destRect.X = 0;
                destRect.Y = 0;
                destRect.Width = canvas.Width;
                destRect.Height = canvas.Height;

                Rectangle srcRect = new Rectangle();

                srcRect.X = start.X - canvas.Width / 3 +(int)(canvas.Width*(rate-1.0)/2);
                srcRect.Y = start.Y - canvas.Height / 3 + (int)(canvas.Height * (rate-1.0)/2);

                srcRect.Width = (int)(canvas.Width*rate);
                srcRect.Height = (int)(canvas.Height*rate);

                g2.DrawImage(canvas, destRect, srcRect,GraphicsUnit.Pixel);
                */

                if (checkGuide.Checked == true)
                {
                    DrawGuide();
                }

                pictureBox1.Image = canvas2;
            }
            catch (Exception ex)
            {

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
                timer1.Enabled = true;
                timer1.Interval = 500;
            }
            else
            {
                timer1.Enabled = true;
                timer1.Interval = 500;
            }
            */
            if (rate > 1.0)
            {
                rate -= 0.1;

                textVal.Text = rate.ToString("N1");
            }

            try
            {
                g2.Clear(Color.White);

                Rectangle srcRect = new Rectangle();

                srcRect.X = canvas2.Width;
                srcRect.Y = canvas2.Width;

                srcRect.Width = (int)(canvas2.Width / (1 / rate));
                srcRect.Height = (int)(canvas2.Height / (1 / rate));

                Rectangle destRect = new Rectangle();
                destRect.X = 0;
                destRect.Y = 0;
                destRect.Width = canvas2.Width;
                destRect.Height = canvas2.Height;

                g2.DrawImage(canvas, destRect, srcRect, GraphicsUnit.Pixel);

                if (checkGuide.Checked == true)
                {
                    DrawGuide();
                }

                pictureBox1.Image = canvas2;
            }
            catch (Exception ex)
            {

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            MsgOut("timer処理");
            double lx = 0;
            double ly = 0;
            lx = lx_b;
            ly = ly_b;

            DrawCanvas(lx, ly);

        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
                timer1.Enabled = true;
                timer1.Interval = 500;
            }
            else
            {
                timer1.Enabled = true;
                timer1.Interval = 500;
            }


            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            MsgOut(numberOfTextLinesToMove.ToString());

            if (numberOfTextLinesToMove > 0)
            {
                if (rate < 2.0)
                {
                    rate += 0.1;
                    textVal.Text = rate.ToString("N1");
                }


            }
            else
            {
                if (rate > 1.0)
                {
                    rate -= 0.1;

                    textVal.Text = rate.ToString("N1");
                }
            }
            /*
            try
            {
                g2.Clear(Color.White);

                Rectangle destRect = new Rectangle();
                destRect.X = 0;
                destRect.Y = 0;
                destRect.Width = canvas.Width;
                destRect.Height = canvas.Height;

                Rectangle srcRect = new Rectangle();

                srcRect.X = 0;
                srcRect.Y = 0;

                srcRect.Width = (int)(canvas.Width * rate);
                srcRect.Height = (int)(canvas.Height * rate);

                g2.DrawImage(canvas, destRect, srcRect, GraphicsUnit.Pixel);


                if (checkGuide.Checked == true)
                {
                    DrawGuide();
                }

                pictureBox1.Image = canvas2;
            }
            catch (Exception ex)
            {

            }
            */
        }

        private void btnLineColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;

            //はじめに選択されている色を設定
            //cd.Color = TextBox1.BackColor;
            //色の作成部分を表示可能にする
            //デフォルトがTrueのため必要はない
            cd.AllowFullOpen = true;
            //純色だけに制限しない
            //デフォルトがFalseのため必要はない
            cd.SolidColorOnly = true;
            //[作成した色]に指定した色（RGB値）を表示する
/*
            cd.CustomColors = new int[] {
    0x33, 0x66, 0x99, 0xCC, 0x3300, 0x3333,
    0x3366, 0x3399, 0x33CC, 0x6600, 0x6633,
    0x6666, 0x6699, 0x66CC, 0x9900, 0x9933};
*/
            //ダイアログを表示する
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //選択された色の取得
                b.BackColor = cd.Color;
            }
        }

        private void btnBrushColorFore_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;

            //はじめに選択されている色を設定
            //cd.Color = TextBox1.BackColor;
            //色の作成部分を表示可能にする
            //デフォルトがTrueのため必要はない
            cd.AllowFullOpen = true;
            //純色だけに制限しない
            //デフォルトがFalseのため必要はない
            cd.SolidColorOnly = true;
            //[作成した色]に指定した色（RGB値）を表示する
            /*
                        cd.CustomColors = new int[] {
                0x33, 0x66, 0x99, 0xCC, 0x3300, 0x3333,
                0x3366, 0x3399, 0x33CC, 0x6600, 0x6633,
                0x6666, 0x6699, 0x66CC, 0x9900, 0x9933};
            */
            //ダイアログを表示する
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //選択された色の取得
                b.BackColor = cd.Color;
            }
        }

        private void btnBrushColorBack_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;

            //はじめに選択されている色を設定
            //cd.Color = TextBox1.BackColor;
            //色の作成部分を表示可能にする
            //デフォルトがTrueのため必要はない
            cd.AllowFullOpen = true;
            //純色だけに制限しない
            //デフォルトがFalseのため必要はない
            cd.SolidColorOnly = true;
            //[作成した色]に指定した色（RGB値）を表示する
            /*
                        cd.CustomColors = new int[] {
                0x33, 0x66, 0x99, 0xCC, 0x3300, 0x3333,
                0x3366, 0x3399, 0x33CC, 0x6600, 0x6633,
                0x6666, 0x6699, 0x66CC, 0x9900, 0x9933};
            */
            //ダイアログを表示する
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //選択された色の取得
                b.BackColor = cd.Color;

                //Color newColor = Color.FromArgb(128, cd.Color);

                //String name = newColor.Name;

                //Color newColor2 = ColorTranslator.FromHtml("0x"+name);

            }
        }

        private void btnFontColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            System.Windows.Forms.Button b = (System.Windows.Forms.Button)sender;

            //はじめに選択されている色を設定
            //cd.Color = TextBox1.BackColor;
            //色の作成部分を表示可能にする
            //デフォルトがTrueのため必要はない
            cd.AllowFullOpen = true;
            //純色だけに制限しない
            //デフォルトがFalseのため必要はない
            cd.SolidColorOnly = true;
            //[作成した色]に指定した色（RGB値）を表示する
            /*
                        cd.CustomColors = new int[] {
                0x33, 0x66, 0x99, 0xCC, 0x3300, 0x3333,
                0x3366, 0x3399, 0x33CC, 0x6600, 0x6633,
                0x6666, 0x6699, 0x66CC, 0x9900, 0x9933};
            */
            //ダイアログを表示する
            if (cd.ShowDialog() == DialogResult.OK)
            {
                //選択された色の取得
                b.BackColor = cd.Color;

                //Color newColor = Color.FromArgb(128, cd.Color);

                //String name = newColor.Name;

                //Color newColor2 = ColorTranslator.FromHtml("0x" + name);

            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FormIconList fi = new FormIconList();
            fi.f1 = this;
            
            if (fi.ShowDialog() == DialogResult.OK)
            {
                IconName = fi.IconName;

                pngpath = DATA_PATH + "\\icon\\png";
                Bitmap wIcon = new Bitmap(pngpath + "\\" + IconName);

                g3.Clear(Color.White);
                g3.DrawImage(wIcon, 0, 0, pictureBox2.Width, pictureBox2.Height);
                wIcon.Dispose();
                pictureBox2.Image = canvas3;
            }

        }
        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (ClickMode)
            {
                case ConstDef.Mode_Line:
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_Polygon:
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_Symbol:
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_String:
                    IsPointSelect = false;
                    break;
                case ConstDef.Mode_Freeline:
                    break;
                case ConstDef.Mode_Select:
                    break;
                case ConstDef.Mode_GetInfo:
                    break;
            }

        }

        private void btnPointSearch_Click(object sender, EventArgs e)
        {
            FormPointSearch fs = new FormPointSearch();
            fs.f1 = this;
            fs.Show();
        }

        private void comboScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            string scale = comboScale.SelectedItem.ToString();
            textVal.Text = scale;

            rate = Double.Parse(textVal.Text);

            RedrawCanvas();
        }
    }

    public class ConstDef
    {
        public const int Mode_Default = 0;
        public const int Mode_Line = 1;
        public const int Mode_Polygon = 2;
        public const int Mode_Symbol = 3;
        public const int Mode_String = 4;
        public const int Mode_Freeline = 5;
        public const int Mode_Select = 10;
        public const int Mode_GetInfo = 11;
    }

}
