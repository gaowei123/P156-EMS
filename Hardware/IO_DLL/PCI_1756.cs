using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvantechDAC.Device;
using AdvantechDAC.Others;
using AdvantechDAC.DIO;

namespace Hardware.IO_DLL
{
    public class PCI_1756
    {
        public static bool Open_Connect(int Device_Num, ref int Device_Handle, ref DEVFEATURES Dev_Features)
        {
            if (CDeviceFunc.DRV_DeviceOpen(Device_Num, ref Device_Handle) == 0)
            {
                CDeviceFunc.DRV_DeviceGetFeature(Device_Handle, ref Dev_Features);
                return true;
            }
            else
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
    }
}
