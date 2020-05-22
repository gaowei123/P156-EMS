using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Automation.BDaq;
using AdvantechDAC.Device;
using AdvantechDAC.Others;
using AdvantechDAC.DIO;
using System.Globalization;

namespace Hardware.IO_DLL
{
    public class PCI_1733
    {
        #region New way for WIN7
        //public static BDaqDio dio = null;
        //public static BDaqDevice device;
        //static InstantDiCtrl instantDiCtrl1 = new InstantDiCtrl();
        //static InstantDoCtrl instantDoCtrl1 = new InstantDoCtrl();

        //public static bool Open_Connect(int Device_Num)
        //{
        //    if (BDaqDevice.Open(Device_Num, AccessMode.ModeWrite, out device) == ErrorCode.Success)
        //        return true;
        //    else
        //        return false;
        //}
        //public static bool Close_Connect()
        //{
        //    device.Close();//Close the device
        //    return true;
        //}

        //public static int Input_Status(int Port_No, int IO_No)
        //{
        //    int Read_Byte = 0;
        //    device.GetModule(0, out dio);
        //    byte bufferForReading;
        //    dio.DiRead(Port_No, out bufferForReading);
        //    Read_Byte = bufferForReading;
        //    int maskA = (int)Math.Pow(2, IO_No);
        //    int result = Read_Byte & maskA;
        //    return result;
        //}

        ////public static int[] Input_Status_Serial()
        ////{
        ////    device.GetModule(0, out dio);
        ////    byte[] bufferForReading = new Byte[16];
        ////    dio.DiRead(0, 16, bufferForReading);
        ////    int[] result = new int[64];
        ////    int j = 0;
        ////    int Read_Byte = 0;
        ////    for (int Port_No = 0; Port_No < 8; Port_No++)
        ////    {
        ////        Read_Byte = bufferForReading[Port_No];
        ////        for (int i = 0; i < 8; i++)
        ////        {
        ////            int maskA = (int)Math.Pow(2, i);
        ////            if ((Read_Byte & maskA) == 0)
        ////                result[j] = 0;
        ////            else
        ////                result[j] = 1;
        ////            j++;
        ////        }
        ////    }
        ////    return result;
        ////}

        //public static List<int> Output_Status()
        //{
        //    instantDoCtrl1.SelectedDevice = new DeviceInformation(2);
        //    byte portData = 0;
        //    byte portDir = 0xFF;
        //    ErrorCode err = ErrorCode.Success;
        //    byte[] mask = instantDoCtrl1.Features.DataMask;
        //    List<int> portList = new List<int>();
        //    for (int i = 0; (i + ConstVal.StartPort) < instantDoCtrl1.Features.PortCount && i < ConstVal.PortCountShow; ++i)
        //    {
        //        err = instantDoCtrl1.Read(i + ConstVal.StartPort, out portData);
        //        if (err != ErrorCode.Success)
        //        {
        //            break;
        //        }

        //        if (instantDoCtrl1.PortDirection != null)
        //        {
        //            portDir = (byte)instantDoCtrl1.PortDirection[i + ConstVal.StartPort].Direction;
        //        }

        //        // Set picture box state
        //        for (int j = 0; j < 8; ++j)
        //        {
        //            if (((portDir >> j) & 0x1) == 0 || ((mask[i] >> j) & 0x1) == 0)  // Bit direction is input.
        //            {
        //                //m_pictrueBox[i, j].Image = imageList1.Images[2];
        //                //m_pictrueBox[i, j].Enabled = false;
        //                break;
        //            }
        //            else
        //            {
        //                //m_pictrueBox[i, j].Click += new EventHandler(PictureBox_Click);
        //                //m_pictrueBox[i, j].Tag = new DoBitInformation((portData >> j) & 0x1, i + ConstVal.StartPort, j);
        //                //m_pictrueBox[i, j].Image = imageList1.Images[(portData >> j) & 0x1];
        //                int data = (portData >> j) & 0x1;
        //                portList.Add(data);
        //            }

        //        }
        //    }
        //    return portList;
        //}
        //public static bool Output_Excut(int Port_No, int IO_No, int Status, int portHex)
        //{
        //    bool flag = true;
        //    ErrorCode err = ErrorCode.Success;
        //    int state = Int32.Parse(portHex.ToString(), NumberStyles.AllowHexSpecifier);
        //    int BitValue = (~(int)Status) & 0x1;
        //    state &= ~(0x1 << IO_No);
        //    state |= BitValue << IO_No;
        //    err = instantDoCtrl1.Write(Port_No, (byte)state);
        //    if (err != ErrorCode.Success)
        //    {
        //        flag = false;
        //    }
        //    return flag;
        //}

        //public static class ConstVal
        //{
        //    public const int StartPort = 0;
        //    public const int PortCountShow = 4;
        //}
        #endregion

        #region Old way for XP
        public static bool Open_Connect(int Device_Num, ref int Device_Handle, ref DEVFEATURES Dev_Features)
        {
            if (CDeviceFunc.DRV_DeviceOpen(Device_Num, ref Device_Handle) == 0)
            {
                CDeviceFunc.DRV_DeviceGetFeature(Device_Handle, ref Dev_Features);
                return true;
            }
            {
                return false;
            }
        }

        public static bool Close_Connect(ref int Device_Handle)
        {
            if (CDeviceFunc.DRV_DeviceClose(ref Device_Handle) == 0)
            {
                return true;
            }
            else
                return false;
        }

        public static int Input_Status(int Port_No, int IO_No, int Device_Handle)
        {
            //0-Light is Off
            //defult-Light is On
            int Read_Byte = 0;
            PT_DioReadPortByte ptDioReadPortByte;
            ptDioReadPortByte.Port = Port_No;
            ptDioReadPortByte.Value = 0;
            CDIOFunc.DRV_DioReadPortByte(Device_Handle, ref ptDioReadPortByte);
            Read_Byte = ptDioReadPortByte.Value;
            int maskA = (int)Math.Pow(2, IO_No);
            int result = Read_Byte & maskA;
            return result;
        }

        public static int[] Input_Status_Serial(int Device_Handle)
        {
            int[] result = new int[32];
            int Read_Byte = 0;
            PT_DioReadPortByte ptDioReadPortByte;
            for (int Port_No = 0; Port_No < 4; Port_No++)
            {
                ptDioReadPortByte.Port = Port_No;
                ptDioReadPortByte.Value = 0;
                CDIOFunc.DRV_DioReadPortByte(Device_Handle, ref ptDioReadPortByte);
                Read_Byte = ptDioReadPortByte.Value;
                for (int i = 8 * Port_No; i < (8 * Port_No + 8); i++)
                {
                    int maskA = (int)Math.Pow(2, i % 8);
                    result[i] = Read_Byte & maskA;
                }
            }
            return result;
        }

        public static bool Output_Excut(int Port_No, int IO_No, int Status, int Device_Handle)
        {
            //0 and 1 Status
            PT_DioWriteBit ptDioWriteBit;
            ptDioWriteBit.Port = Port_No;
            ptDioWriteBit.Bit = IO_No;
            ptDioWriteBit.State = Status;
            if (CDIOFunc.DRV_DioWriteBit(Device_Handle, ref ptDioWriteBit) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int Port_Handle(int Port_No, int Device_Handle)
        {
            PT_DioReadPortByte ptDioReadPortByte;
            ptDioReadPortByte.Port = Port_No;
            ptDioReadPortByte.Value = 0;
            CDIOFunc.DRV_DioReadPortByte(Device_Handle, ref ptDioReadPortByte);
            return ptDioReadPortByte.Value;
        }
        #endregion
    }
}
