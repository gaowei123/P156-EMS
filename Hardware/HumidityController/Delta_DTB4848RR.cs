using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Hardware.HumidityController
{
    public class Delta_DTB4848RR
    {
        public static string Temperature_SV_PV_Request()
        {
            try
            {
                string s = "3A 30 32 30 33 31 30 30 30 30 30 30 32 45 39 0D 0A";
                s = s.Replace(" ", "");
                byte[] buff = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                {
                    buff[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
                }
                return Encoding.Default.GetString(buff); //ASCII change to 16 bit
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static string Humidity_SV_PV_Request()
        {
            try
            {
                string s = "3A 30 31 30 33 31 30 30 30 30 30 30 32 45 41 0D 0A";
                s = s.Replace(" ", "");
                byte[] buff = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                {
                    buff[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
                }
                return Encoding.Default.GetString(buff); //ASCII change to 16 bit
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static string Humidity_SV_Setting(string sv_10)
        {
            try
            {
                //sv1+sv2=sv_16
                string sv1 = string.Empty;
                string sv2 = string.Empty;
                string sv_16 = string.Empty;
                sv_10 = sv_10.Replace(".", "");
                sv_16 = Convert.ToString(int.Parse(sv_10), 16).ToUpper(); //转成16进制

                if (sv_16.Length % 2 == 1)
                    sv_16 = "0" + sv_16;
                byte[] ab = System.Text.ASCIIEncoding.Default.GetBytes(sv_16);
                StringBuilder bs = new StringBuilder();
                foreach (byte b in ab)
                {
                    bs.Append(b.ToString("x") + " ");
                }
                string humidity_sv = bs.ToString();

                if (sv_16.Length <= 2)
                {
                    sv1 = "0";
                    sv2 = sv_16;
                }
                else if(sv_16.Length==3)
                {
                    sv1 = sv_16.Substring(0, 1);
                    sv2 = sv_16.Substring(1, 2);
                }
                else if (sv_16.Length == 4)
                {
                    sv1 = sv_16.Substring(0, 2);
                    sv2 = sv_16.Substring(2, 2);
                }
                int result = 24 + Convert.ToInt32(sv1, 16) + Convert.ToInt32(sv2, 16);
                string result_16 = Convert.ToString(result, 16);
                if (result_16.Length > 2)
                {
                    result_16 = result_16.Substring(result_16.Length - 2, 2);
                    result = Convert.ToInt32(result_16, 16);
                }
                int result_reverse = 256 - result;
                sv_16 = Convert.ToString(result_reverse, 16).ToUpper();

                //16 bit change to ASCII
                if (sv_16.Length % 2 == 1)
                    sv_16 = "0" + sv_16;
                byte[] ba = System.Text.ASCIIEncoding.Default.GetBytes(sv_16);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in ba)
                {
                    sb.Append(b.ToString("x") + " ");
                }


                string lrc = sb.ToString().TrimEnd();
                string s = "3A 30 31 30 36 31 30 30 31 " + humidity_sv + lrc + " 0D 0A";



                s = s.Replace(" ", "");
                byte[] buff = new byte[s.Length / 2];
                for (int i = 0; i < s.Length; i += 2)
                {
                    buff[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
                }
                return Encoding.Default.GetString(buff); //ASCII change to 16 bit
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static void Humidity_SV_PV_Reading(string x,ref string pv,ref string sv)
        {
            try
            {
                string result = string.Empty;
                byte[] buff = new byte[1];
                for (int i = 0; i < x.Length; i++)
                {
                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                    int intAsciiCode = (int)asciiEncoding.GetBytes(x.Substring(i, 1))[0]; //string转ASCII码
                    string y = Convert.ToString(intAsciiCode, 16).ToUpper();
                    if (y.Length == 1)
                        y = "0" + y;
                    result = result + y;
                }
                //reading-3A30313033,setting-3A30313036
                if (result.Substring(0, 10) == "3A30313036")
                    return; //Feedback after setting, not reading
                pv = ASCII_To_Ten(result.Substring(14, 8));
                sv = ASCII_To_Ten(result.Substring(22, 8));
                pv = pv.Substring(0, pv.Length - 1) + "." + pv.Substring(pv.Length - 1, 1);
                sv = sv.Substring(0, sv.Length - 1) + "." + sv.Substring(sv.Length - 1, 1);

                StaticRes.Global.Current_Humidity = pv;

                //if (float.Parse(pv) > float.Parse(sv)) //sv=High limit
                //    Hardware.IO_LIST.Output.Y103_N2_Flow_In_Open();
                //else if (float.Parse(pv) <= float.Parse(StaticRes.Global.System_Setting.Humidity_SV_Low_Limit))
                //    Hardware.IO_LIST.Output.Y103_N2_Flow_In_Close();

                //Save to local log file
                string datePatt = @"yyyyMMdd";
                string path = "";
                string file = "";


                #region File.
                path = ".\\Humidity_Log\\";

                if (!Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                file = path + DateTime.Now.ToString(datePatt) + ".txt";

                #endregion

                StreamWriter tw = File.AppendText(file);

                // Write a line of text to the file.
                tw.WriteLine(DateTime.Now.ToString("yyyyMMdd HH:mm:ss -- ") + "SV:" + sv + "%;PV:" + pv + "%");

                // Close the stream.
                tw.Flush();
                tw.Close();

            }
            catch(Exception ee)
            {
                throw ee;
            }
        }


        public static string ASCII_To_Ten(string Ascii_code)
        {
            Ascii_code = Ascii_code.Replace(" ", "");
            byte[] buff = new byte[Ascii_code.Length / 2];
            int index = 0;
            for (int i = 0; i < Ascii_code.Length; i += 2)
            {
                buff[index] = Convert.ToByte(Ascii_code.Substring(i, 2), 16);
                ++index;
            }
            string result = Encoding.Default.GetString(buff); //ASCII change to 16 bit
            return Convert.ToInt32(result, 16).ToString(); //16 bit change to 10 bit
        }

        private string Ten_To_ASCII(int Ten_code)
        {
            string s2 = Convert.ToString(Ten_code, 16).ToUpper(); //10进制 转 16进制
            if (s2.Length % 2 == 1)
                s2 = "0" + s2;
            byte[] ba = System.Text.ASCIIEncoding.Default.GetBytes(s2);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ba)
            {
                sb.Append(b.ToString("x"));
            }
            return sb.ToString();
        }

        #region Document for Controller
        //        湿度SV和PV读取发送：
        //3A  30 31 30 33 31 30 30 30 30 30 30 32 45 41 0D 0A
        // 1    2    3       4         5      6    7

        //1.	起始符
        //2.	仪表站号，湿度表为30 31，温度表为30 32
        //3.	Modbus功能码，03是寄存器读取
        //4.	被读取寄存器的modbus地址，H1000
        //5.	读取寄存器字数，读取两个字
        //6.	LRC校验值
        //7.	结束符

        //湿度SV和PV读取接收：
        //3A 30 31 30 33 30 34 30 31 46 38 30 32 30 38 46 35 0D 0A
        //1	  2    3     4        5          6       7    8
        //1. 起始符
        //2. 仪表站号，湿度表为30 31，温度表为30 32
        //3. Modbus功能码，03是寄存器读取
        //4. 接收代码的数据长度，此为4个字
        //5.湿度的当前值，01F8=504，即当前湿度为50.4%
        //6.湿度的设定值，0208=520，即当前湿度为52.0%
        //7.LRC校验
        //8.结束符

        //温度SV和PV读取发送：
        //3A  30 32 30 33 31 30 30 30 30 30 30 32 45 39 0D 0A
        //1	   2    3       4         5      6    7

        //1.起始符
        //2.仪表站号，湿度表为30 31，温度表为30 32
        //3.Modbus功能码，03是寄存器读取
        //4.被读取寄存器的modbus地址，H1000
        //5.读取寄存器字数，读取两个字
        //6.LRC校验值
        //7.结束符

        //温度SV和PV读取接收：
        //3A 30 32 30 33 30 34 30 30 46 46 30 30 37 30 38 38 0D 0A
        // 1  2     3     4       5          6       7     8

        //1. 起始符
        //2. 仪表站号，湿度表为30 31，温度表为30 32
        //3. Modbus功能码，03是寄存器读取
        //4. 接收代码的数据长度，此为4个字
        //5.温度的当前值，HFF=255，即当前湿度为25.5℃
        //6.温度的设定值，H70=112，即当前湿度为11.2℃
        //7.LRC校验
        //8.结束符


        //湿度SV写入发送：
        //3A 30 31 30 36 31 30 30 31 30 32 32 32 43 34 0D 0A
        //1	  2   3       4         5      6    7
        //1.起始符
        //2.仪表站号，湿度表为30 31，温度表为30 32
        //3.Modbus功能码，06是寄存器写入
        //4.被写入寄存器的modbus地址，H1001
        //5.写入的数值，H222=546，即设定湿度为54.6%
        //6.LRC校验值，LRC校验计算如下，
        //  H01+H06+H10+H01+H02+H22=H3C
        //  H100-H3C=HC4即为43 34
        //7.结束符

        //温度SV写入发送：
        //3A 30 32 30 36 31 30 30 31 30 30 46 46 45 38 0D 0A
        //1	  2   3       4         5      6    7
        //1.起始符
        //2.仪表站号，湿度表为30 31，温度表为30 32
        //3.Modbus功能码，06是寄存器写入
        //4.被写入寄存器的modbus地址，H1001
        //5.写入的数值，HFF=255，即设定温度为25.5℃
        //6.LRC校验值，LRC校验计算如下，
        //  H02+H06+H10+H01+HFF =H118
        //  H100-H18=HE8即为45 38
        //7.结束符


        //注：如返回的代码长度与正确代码不一致，则为错误代码。
        //读取时，需比较返回代码长度，站号，功能码，方可确定读取内容的正确性。
        //写入时，返回的代码与发送代码一致，则写入成功。
        #endregion
    }
}
