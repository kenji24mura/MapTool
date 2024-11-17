using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM
{
    class StringCOM
    {
        /// <summary>
        /// 文字列の描画、回転、基準位置指定
        /// </summary>
        /// <param name="g">描画先のGraphicsオブジェクト</param>
        /// <param name="s">描画する文字列</param>
        /// <param name="f">文字のフォント</param>
        /// <param name="brush">描画用ブラシ</param>
        /// <param name="x">基準位置のX座標</param>
        /// <param name="y">基準位置のY座標</param>
        /// <param name="deg">回転角度（度数、時計周りが正）</param>
        /// <param name="format">基準位置をStringFormatクラスオブジェクトで指定します</param>
        public static void DrawString(Graphics g, string s, Font f, SolidBrush brush, float x, float y, float deg, StringFormat format)
        {

            try
            {
                using (var pathText = new System.Drawing.Drawing2D.GraphicsPath())  // パスの作成
                using (var mat = new System.Drawing.Drawing2D.Matrix())             // アフィン変換行列
                {
                    // 描画用Format
                    var formatTemp = (StringFormat)format.Clone();
                    formatTemp.Alignment = StringAlignment.Near;        // 左寄せに修正
                    formatTemp.LineAlignment = StringAlignment.Near;    // 上寄せに修正

                    // 文字列の描画
                    pathText.AddString(
                            s,
                            f.FontFamily,
                            (int)f.Style,
                            f.SizeInPoints,
                            new PointF(0, 0),
                            format);
                    formatTemp.Dispose();

                    // 文字の領域取得
                    var rect = pathText.GetBounds();

                    // 回転中心のX座標
                    float px;
                    switch (format.Alignment)
                    {
                        case StringAlignment.Near:
                            px = rect.Left;
                            break;
                        case StringAlignment.Center:
                            px = rect.Left + rect.Width / 2f;
                            break;
                        case StringAlignment.Far:
                            px = rect.Right;
                            break;
                        default:
                            px = 0;
                            break;
                    }
                    // 回転中心のY座標
                    float py;
                    switch (format.LineAlignment)
                    {
                        case StringAlignment.Near:
                            py = rect.Top;
                            break;
                        case StringAlignment.Center:
                            py = rect.Top + rect.Height / 2f;
                            break;
                        case StringAlignment.Far:
                            py = rect.Bottom;
                            break;
                        default:
                            py = 0;
                            break;
                    }

                    // 文字の回転中心座標を原点へ移動
                    mat.Translate(-px, -py, System.Drawing.Drawing2D.MatrixOrder.Append);
                    // 文字の回転
                    mat.Rotate(deg, System.Drawing.Drawing2D.MatrixOrder.Append);
                    // 表示位置まで移動

                    mat.Translate(x, y, System.Drawing.Drawing2D.MatrixOrder.Append);

                    // パスをアフィン変換
                    pathText.Transform(mat);

                    // 描画
                    g.FillPath(brush, pathText);
                }

            }
            catch (Exception ex)
            {

            }


        }
    }
}
