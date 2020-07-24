using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Common.Reports
{
    public class LogFile
    {
        public static void Log(string msg)
        {
            //if (!modData.LogFile_Enabled) return; // 27 Aug 2014, jjwong. Positioned this code line. After: At the first position right the function declaration. Before: Right after 'try'.
            try
            {
                string datePatt = @"yyyyMMdd";
                string path = "";
                string file = "";


                #region File.
                path = ".\\Log\\";

                if (!Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                file = path + DateTime.Now.ToString(datePatt) + ".txt";

                #endregion

                StreamWriter tw = File.AppendText(file);

                // Write a line of text to the file.
                tw.WriteLine(DateTime.Now.ToString("yyyyMMdd HH:mm:ss tt -- ") + msg);

                // Close the stream.
                tw.Flush();
                tw.Close();
            }
            catch
            {
                ; // .
            }
        }

        public static void DebugLog(MethodBase methodBase, string sMsg)
        {
           string path = ".\\DebugLog\\";

            if (!Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            string file = path + DateTime.Now.ToString("yyyyMMdd") + ".txt";

          

            StreamWriter tw = File.AppendText(file);


            string nameSpace = methodBase.DeclaringType.Namespace;
            string functionName = methodBase.Name;

            string logText = string.Format("[{0}] -- [{1}]-[{2}],  {3}", DateTime.Now.ToString("yyyyMMdd HH:mm:ss tt"), nameSpace, functionName, sMsg);
            tw.WriteLine(logText);

           
            tw.Flush();
            tw.Close();
        }
    }
}
