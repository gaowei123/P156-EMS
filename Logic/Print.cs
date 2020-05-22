using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;

namespace Logic
{
    public class Print
    { 
        public static void EMS(string Part_ID, string Machine_ID,string Expiry_Time,
            string Thawing_Time,string Ready_Time,string Sapcode)
        {
            try
            {
                // Read the file as one string.
                string sFilename = string.Empty;
                string sMessage = string.Empty;
                sFilename = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "123.txt";
                System.IO.StreamReader myFile = new System.IO.StreamReader(sFilename);
                sMessage = myFile.ReadToEnd();
                myFile.Close();

                sMessage = sMessage.Replace("20141220123456001", Part_ID);
                sMessage = sMessage.Replace("20141220123456001", Part_ID);
                sMessage = sMessage.Replace("R008-0001X", Sapcode);
                sMessage = sMessage.Replace("R008-0001X", Sapcode);
                sMessage = sMessage.Replace("2014/12/20 00:00:00", Expiry_Time);
                sMessage = sMessage.Replace("2014/12/20 00:00:00", Expiry_Time);
               // sMessage = sMessage.Replace("2014/12/19 00:00:00", Ready_Time);
               // sMessage = sMessage.Replace("2014/12/18 00:00:00", Thawing_Time);
                sMessage = sMessage.Replace("DA001", Machine_ID);
                //sValue1 = sValue;
                //sValue1 = sValue1.Insert(2, "&D");
                //sMessage = sMessage.Replace("B3&D33333333333", sValue1);
                sMessage = sMessage.Trim();

                int iLen = sMessage.IndexOf("E1");
                sMessage = sMessage.PadLeft(iLen + 2);
                sMessage += "\r\n\0";

                // Output it to the barcode printer.
                System.IO.Ports.SerialPort Printer = new System.IO.Ports.SerialPort();
                Printer.BaudRate = StaticRes.Global.System_Setting.Printer_BaudRate;
                Printer.StopBits = System.IO.Ports.StopBits.One;
                Printer.DataBits = StaticRes.Global.System_Setting.Printer_DataBits;
                Printer.PortName = StaticRes.Global.System_Setting.Printer_COM_Port;
                if (!Printer.IsOpen)
                    Printer.Open();
                Printer.Write(sMessage);
                if (Printer.IsOpen)
                    Printer.Close();
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }
    }
}
