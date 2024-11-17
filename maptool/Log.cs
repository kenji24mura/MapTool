using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com
{
    public class Log
    {
        public string LOGFNAME;
        public int loglimit = 1000;
        public int logmax = 1000;

        public void LogOut(string msg)
        {
            string fname1;
            string fname2;

            try
            {
                if (System.IO.File.Exists(LOGFNAME))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(LOGFNAME);
                    long filesize = fi.Length;

                    if (filesize > loglimit)
                    {
                        for (int i = logmax; i > 1; i--)
                        {
                            fname1 = LOGFNAME + "." + i.ToString().PadLeft(2, '0');
                            fname2 = LOGFNAME + "." + (i - 1).ToString().PadLeft(2, '0');
                            if (System.IO.File.Exists(fname2))
                            {
                                if (System.IO.File.Exists(fname1))
                                {
                                    System.IO.File.Delete(fname1);
                                }
                                System.IO.File.Move(fname2, fname1);
                            }
                        }
                        fname1 = LOGFNAME + "." + 1.ToString().PadLeft(2, '0');
                        if (System.IO.File.Exists(fname1))
                        {
                            System.IO.File.Delete(fname1);
                        }
                        System.IO.File.Move(LOGFNAME, fname1);
                    }
                }
                //
                System.IO.StreamWriter sw = new System.IO.StreamWriter(
                    LOGFNAME,
                    true,
                    System.Text.Encoding.GetEncoding("shift_jis"));

                DateTime dt = DateTime.Now;
                sw.WriteLine(dt + "," + msg);
                sw.Close();

            }
            catch (Exception ex)
            {

            }
        }
    }
}