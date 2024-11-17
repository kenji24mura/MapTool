using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDXCom
{
    class MDX
    {
        public const int MDX_10000 = 71;
        public const int MDX_25000 = 72;
        public const int MDX_50000 = 73;
        public const int MDX_200000 = 74;

        public const int MDX_10000_WIDTH = 225000;//ミリ秒
        public const int MDX_10000_HEIGHT = 150000;//ミリ秒
        public const int MDX_200000_WIDTH = 3600000;//ミリ秒
        public const int MDX_200000_HEIGHT = 2400000;//ミリ秒
    }
    public class SymbolDefTable
    {
        public string kind { get; set; }
        public int sym_ptncode { get; set; }
        public int sym_size { get; set; }

    }
    public class SymbolFnameTable
    {
        public int sym_ptncode { get; set; }
        public string sym_fname { get; set; }
    }
    public class PolygonDefTable
    {
        public string kind { get; set; }
        public Color line_color { get; set; }
        public int line_style { get; set; }
        public float line_width { get; set; }
        public Color br_color { get; set; }
        public int br_style { get; set; }
    }
    public class LineDefTable
    {
        public string kind { get; set; }
        public Color line_color { get; set; }
        public int line_style { get; set; }
        public float line_width { get; set; }
    }
    public class StringDefTable
    {
        public string kind { get; set; }
        public int sym_ptncode { get; set; }
        public double sym_size { get; set; }
        public Color str_color { get; set; }
        public double str_size { get; set; }
        public string str_font { get; set; }
        public int str_style { get; set; }

    }

}
