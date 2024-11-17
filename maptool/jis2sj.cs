using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com
{
    class jis2sj
    {

        public static byte[] Jis2Sjis(byte[] targetByte)
        {

            //JISコード表
            //http://charset.7jp.net/jis.html
            //


            if ((targetByte.Length % 2) != 0)
            {
                throw new Exception("2で割り切れません。");
            }

            List<byte[]> jisList = new List<byte[]>();
            List<byte> sjisList = new List<byte>();
            for (int i = 0; i < targetByte.Length; i = i + 2)
            {
                jisList.Add(new byte[] { targetByte[i], targetByte[i + 1] });
            }

            foreach (byte[] changeByte in jisList)
            {
                // 全角文字エリア
                if ((changeByte[0] >= 0x21 && changeByte[0] <= 0x7E) &&
                  (changeByte[1] >= 0x21 && changeByte[1] <= 0x7E))
                {
                    int firstByte = (int)changeByte[0];
                    int i0x21 = (int)0x21;
                    int i0x5e = (int)0x5e;
                    int i0x81 = (int)0x81;
                    int i0xc1 = (int)0xc1;
                    int firstResult = ((firstByte - i0x21) / 2) + (firstByte <= i0x5e ? i0x81 : i0xc1);

                    int secoundByte = (int)changeByte[1];
                    int i0x5f = (int)0x5f;
                    int i0x1f = (int)0x1f;
                    int i0x20 = (int)0x20;
                    int i0x7e = (int)0x7e;
                    int secondResult = 0;

                    if ((firstByte & 1) >= 1)
                    {
                        secondResult = secoundByte + ((secoundByte <= i0x5f) ? i0x1f : i0x20);
                    }
                    else
                    {
                        secondResult = secoundByte + i0x7e;
                    }

                    byte sjis01 = (byte)firstResult;
                    byte sjis02 = (byte)secondResult;

                    sjisList.Add(sjis01);
                    sjisList.Add(sjis02);
                }
                // その他
                else
                {
                    //外字変換
                    jis2sj.ChangeByte(ref changeByte[0], ref changeByte[1]);

                    sjisList.Add(changeByte[0]);
                    sjisList.Add(changeByte[1]);

                }
            }

            return sjisList.ToArray();
        }

        public static void ChangeByte(ref byte b1, ref byte b2)
        {

            UInt16[] h =
            {
0x81c0,
0x81c1,
0x81c2,
0x81c3,
0x81c4,
0x81c5,
0x81c6,
0x81c7,
0x81c8,
0x81c9,
0x82c0,
0x82c1,
0x82c2,
0x82c3,
0x82c4,
0x82c5,
0x82c6,
0x82c7,
0x82c8,
0x82c9,
0x82d0,
0x82d1,
0x82d2,
0x82d3,
0x82d4,
0x82d5,
0x82d6,
0x82d7,
0x82d8,
0x82d9,
0x83a5,
0x88cf,
0x88b5,
0x88b6,
0x88b7,
0x88b8,
0x88b9,
0x88ba,
0x88bb,
0x88bc,
0x88bd,
0x88be
            };

            UInt16[] l =
            {
0x824f,
0x8250,
0x8251,
0x8252,
0x8253,
0x8254,
0x8255,
0x8256,
0x8257,
0x8258,
0x824f,
0x8250,
0x8251,
0x8252,
0x8253,
0x8254,
0x8255,
0x8256,
0x8257,
0x8258,
0x824f,
0x8250,
0x8251,
0x8252,
0x8253,
0x8254,
0x8255,
0x8256,
0x8257,
0x8258,
0x817c,
0x815b,
0x8340,
0x8342,
0x8344,
0x8346,
0x8348,
0x8362,
0x8383,
0x8385,
0x8387,
0x838e
            };


            UInt16 s1 = (UInt16)(b1 * 256 + b2);
            UInt16 s2 = 0x2020;

            for (int i = 0; i < h.Length; i++)
            {
                if (h[i] == s1)
                {
                    s2 = l[i];
                    break;
                }
            }


            b1 = (byte)(s2 / 256);
            b2 = (byte)(s2 % 256);
        }

    }
}
