using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDCom
{
    public class zmd_def
    {
        public const int ZMDORGX = -18750000;//m
        public const int ZMDORGY = -32500000;//m

        public const long MAP_W = 7500;//750m
        public const long MAP_H = 5000;//500m

    }

    // 年月日タイプ
    public class Date
    {

        public System.UInt16 y;       // 西暦
        public byte m;        // 月
        public byte d;        // 日
    }
    // 座標系
    public class XY
    {

        public short x;        // Ｘ座標
        public short y;     // Ｙ座標
    }

    //形状記述ファイルヘッダ
    public class FRM_HEAD
    {
        public PicKind_Code pickind;   // 図種別コード
        public Date date;              // ﾃﾞｰﾀ作成年月日
        public ZNO zno;                // このファイルが記述する図の図番号
        public System.UInt16 maxlay;   // 最大レイヤ番号
        public UInt16 rsv;              // リザーブ
        public System.UInt32 brk_ptr;          // ブロック管理部アドレス
        public System.UInt32 brk_cnt;          // ブロック情報部レコード数
        public System.UInt32 inf_ptr;          // 属性対応管理部アドレス
        public System.UInt32 inf_cnt;          // 属性対応情報部レコード数
        public System.UInt32 brk_str;          // ブロック部開始アドレス
        public System.UInt32 ref_ptr;          // 関連図管理部アドレス
        public System.UInt32 ref_cnt;          // 関連図情報部レコード数
        public System.UInt32 rsv_ptr;          // 予約領域部アドレス
        public System.UInt32 file_sz;          // ファイル容量

    }
    // 図種別コード
    public class PicKind_Code
    {
        public byte pic1;              // ｂ８：０ｼｽﾃﾑ提供 １：ﾕｰｻﾞ作成
        public byte pic2;              // ﾃﾞｰﾀ種別文字の文字コード
        public byte pic3;              // 図種別文字の文字コード
        public byte pic4;              // 座標系系番号
    }
    // 図面番号
    public class ZNO
    {
        public System.UInt16 x;       // 図番号（Ｘ成分）
        public System.UInt16 y;       // 図番号（Ｙ成分）
    }
    public class FRM_BRK
    {

        public System.UInt32 brk_ptr;          // ブロックアドレス
        public System.UInt32 brk_sz;          // ブロック容量
    }

    public class FRM_INF
    {

        public System.UInt32 inf_num;          // 図内属性番号
        public System.UInt16 inf_z_brk;        // 図形形状所属ブロック
        public System.UInt16 inf_m_brk;        // 文字形状所属ブロック
        public System.UInt32 inf_z_off;        // 図形形状ブロック内オフセット
        public System.UInt32 inf_m_off;        // 文字形状ブロック内オフセット
        public System.UInt32 inf_recnum;       // 属性情報部レコード番号
        public System.UInt32 rsv;			   // リザーブ
    }
    public class FRM_LAY
    {

        public System.UInt32 lay_off;  // 対応するレイヤのブロック内オフセット
        public System.UInt32 lay_size; // 対応するレイヤの容量
        public System.UInt32 lay_frm_off;	// ブロックに跨らない形状情報部の先頭のブロック内オフセット
    }

    //
    public class FRM_FRMCOM
    {

        public System.UInt32 frmcom_recnum;    //属性対応情報部レコード番号
        public System.UInt16 frmcom_size; //形状情報部容量
        public byte frmcom_type;   //形状種別コード
        public byte frmcom_status;	//形状状態コード
    }


    //文字概要部
    public class FRM_STRINF
    {

        public XY ldxy;        //座標
        public XY ruxy;        //座標
        public System.UInt16 height;      //文字高さ
        public System.UInt16 angle;       //文字傾き
        public byte c1;                //変形方向
        public byte c2;                //変形率
        public byte form;          //書式
        public byte line;           //レコード数
    }

    //行情報
    public class FRM_STRLINE
    {
        public XY kxy;     //基準点
        public System.UInt16 width;       //文字幅
        public System.UInt16 height;      //文字高さ
        public System.UInt16 cnt;       //文字数
    }

    //シンボル
    public class FRM_SYMBOL
    {

        public XY ldxy;                //左下座標
        public XY ruxy;                //右上座標
        public System.UInt16 width;       //大きさ
        public System.UInt16 angle;       //傾き
        public XY kxy; //基準点
        public System.UInt16 symno;     //シンボル番号
    }
    //線分
    public class FRM_LINES
    {

        public XY ldxy;        //左下座標
        public XY ruxy;        //右上座標
        public System.UInt16 cnt;       //ライン数
    }
    //単純ポリゴン
    public class FRM_POLYGON
    {

        public XY ldxy;        //左下座標
        public XY ruxy;        //右上座標
        public System.UInt16 cnt;       //ライン数
    }

    //外枠ポリゴン情報部
    public class FRM_OUTPOLY
    {

        public XY ldxy;        //左下座標
        public XY ruxy;        //右上座標
        public System.UInt16 inpoly;      //中抜きポリゴン数
        public System.UInt16 cnt;       //外枠ポリゴン座標数
    }
    //中抜きポリゴン情報部
    public class FRM_INPOLY
    {
        public System.UInt16 cnt;       //中抜きポリゴン座標数
    }

    //行政界ポリゴン
    public class FRM_AREAPOLY
    {

        public XY ldxy;        //左下座標
        public XY ruxy;        //右上座標
        public System.UInt16 addr1;       //市町村
        public System.UInt16 addr2;       //大字
        public System.UInt16 addr3;       //字丁目
        public System.UInt16 addr4;       //街区
        public System.UInt16 inpoly;      //中抜きポリゴン数
        public System.UInt16 cnt;       //外枠ポリゴン座標数
    }
    //文字付き部品
    public class FRM_STRSYM
    {

        public XY ldxy;                //左下座標
        public XY ruxy;                //右上座標
        public System.UInt16 width;       //大きさ
        public System.UInt16 angle;       //傾き
        public XY kxy; //基準点
        public System.UInt16 symno;       //部品番号
        public System.UInt16 cnt;       //文字数
    }
}
