using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdvantechDAC.Device;
using AdvantechDAC.Others;
using AdvantechDAC.DIO;
using System.Data;

namespace HardwareControl
{
    public class Initial_Hardware
    {
     

        public static void Connect_readParameter()
        {

            Read_Parameter();


            try
            {
                DEVFEATURES devFeatures_IO_1756 = new DEVFEATURES();
                DEVFEATURES devFeatures_IO_1733 = new DEVFEATURES();

                if (!Hardware.IO_DLL.PCI_1756.Open_Connect(1, ref StaticRes.Global.Device_Handle_IO_PCI1756, ref devFeatures_IO_1756))
                    throw new System.Exception("Open connection of I/O card PCI-1756 faild !!");
                if (!Hardware.IO_DLL.PCI_1733.Open_Connect(0, ref StaticRes.Global.Device_Handle_IO_PCI1733, ref devFeatures_IO_1733))
                    throw new System.Exception("Open connection of I/O card PCI-1733 faild !!");
                if (!HardwareControl.Motion_Control.Open_Connection())
                    throw new System.Exception("Open connection of motion card PCI-1220U failed !!");

                //if (!HardwareControl.Motion_Control.Rotary_Servo_On())
                //    throw new System.Exception("Turn on rotary servo function failed !!");
                //if (!HardwareControl.Motion_Control.Arm_X_Servo_On())
                //    throw new System.Exception("Turn on arm x servo function failed !!");
                //if (!HardwareControl.Motion_Control.Arm_Y_Servo_On())
                //    throw new System.Exception("Turn on arm y servo function failed !!");
                StaticRes.Global.Hardware_Connection = true;
            }
            catch (Exception ee)
            {
                StaticRes.Global.Hardware_Connection = false;
                throw ee;
            }
        }

        public static void Close_Connect()
        {
            try
            {
                if (!Hardware.IO_DLL.PCI_1756.Close_Connect(ref StaticRes.Global.Device_Handle_IO_PCI1756))
                    throw new System.Exception("Close connection of I/O card PCI-1756 failed !!");
                if (!Hardware.IO_DLL.PCI_1733.Close_Connect(ref StaticRes.Global.Device_Handle_IO_PCI1733))
                    throw new System.Exception("Close connection of I/O card PCI-1733 failed !!");
                if (!HardwareControl.Motion_Control.Close_Connection())
                    throw new System.Exception("Close connection of motion card PCI-1220 failed !!");
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        private static void Read_Parameter()
        {
            DataTable dt_sys = DataProvider.Local.Configure.Select();
            if (dt_sys.Rows.Count == 0)
                throw new System.Exception("Read system paramater failed , please check with admin !!");

            List<ObjectModule.Local.Configure> configure_list = new List<ObjectModule.Local.Configure>();
            foreach (DataRow a in dt_sys.Rows)
            {
                ObjectModule.Local.Configure sc = new ObjectModule.Local.Configure(a);
                configure_list.Add(sc);
            }






            StaticRes.Global.System_Setting.SlotScanner_Index1Port =  (from m in configure_list where m.NAME == "Slot_Index1_Scanner_COM_Port" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.SlotScanner_Index2Port =  (from m in configure_list where m.NAME == "Slot_Index2_Scanner_COM_Port" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.SlotScanner_Index3Port =  (from m in configure_list where m.NAME == "Slot_Index3_Scanner_COM_Port" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.SlotScanner_Index4Port =  (from m in configure_list where m.NAME == "Slot_Index4_Scanner_COM_Port" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.SlotScanner_BaudRate = int.Parse((from m in configure_list where m.NAME == "Slot_Scanner_BaudRate" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.SlotScanner_DataBits = int.Parse((from m in configure_list where m.NAME == "Slot_Scanner_DataBits" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.SlotScanner_ReceivedBytesThreshold = int.Parse((from m in configure_list where m.NAME == "Slot_Scanner_ReceivedBytesThreshold" select m.VALUE).First<string>());
            

            StaticRes.Global.System_Setting.Print_Label_Before_Load = (from m in configure_list where m.NAME == "Print_Label_Before_Load" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Print_Label_After_Unload = (from m in configure_list where m.NAME == "Print_Label_After_Unload" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Weighing_Scale_COM_Port = (from m in configure_list where m.NAME == "Weighing_Scale_COM_Port" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Weighing_Scale_BaudRate = int.Parse((from m in configure_list where m.NAME == "Weighing_Scale_BaudRate" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Weighing_Scale_DataBits = int.Parse((from m in configure_list where m.NAME == "Weighing_Scale_DataBits" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Weighing_Scale_ReceivedBytesThreshold = int.Parse((from m in configure_list where m.NAME == "Weighing_Scale_ReceivedBytesThreshold" select m.VALUE).First<string>());

            StaticRes.Global.System_Setting.Printer_COM_Port = (from m in configure_list where m.NAME == "Printer_COM_Port" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Printer_BaudRate = int.Parse((from m in configure_list where m.NAME == "Printer_BaudRate" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Printer_DataBits = int.Parse((from m in configure_list where m.NAME == "Printer_DataBits" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Printer_ReceivedBytesThreshold = int.Parse((from m in configure_list where m.NAME == "Printer_ReceivedBytesThreshold" select m.VALUE).First<string>());

            StaticRes.Global.System_Setting.Handle_Scanner_COM_Port = (from m in configure_list where m.NAME == "Handle_Scanner_COM_Port" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Handle_Scanner_BaudRate = int.Parse((from m in configure_list where m.NAME == "Handle_Scanner_BaudRate" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Handle_Scanner_DataBits = int.Parse((from m in configure_list where m.NAME == "Handle_Scanner_DataBits" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Handle_Scanner_ReceivedBytesThreshold = int.Parse((from m in configure_list where m.NAME == "Handle_Scanner_ReceivedBytesThreshold" select m.VALUE).First<string>());

            StaticRes.Global.System_Setting.Idle_Time_To_Exit = int.Parse((from m in configure_list where m.NAME == "Idle_Time_To_Exit" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Online_Mode = (from m in configure_list where m.NAME == "Online_Mode" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Allow_Manual_KeyIn = (from m in configure_list where m.NAME == "Allow_Manual_KeyIn" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Storage_Pre_Set_Qty = int.Parse((from m in configure_list where m.NAME == "Storage_Pre_Set_Qty" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.One_Cycle_Pulse = int.Parse((from m in configure_list where m.NAME == "One_Cycle_Pulse" select m.VALUE).First<string>());

            StaticRes.Global.System_Setting.Rotary_Home_Low_Velocity = int.Parse((from m in configure_list where m.NAME == "Rotary_Home_Low_Velocity" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Rotary_Home_High_Velocity = int.Parse((from m in configure_list where m.NAME == "Rotary_Home_High_Velocity" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Rotary_Home_Acc = int.Parse((from m in configure_list where m.NAME == "Rotary_Home_Acc" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Rotary_Home_Dcc = int.Parse((from m in configure_list where m.NAME == "Rotary_Home_Dcc" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Rotary_Home_Jerk = int.Parse((from m in configure_list where m.NAME == "Rotary_Home_Jerk" select m.VALUE).First<string>());

            StaticRes.Global.System_Setting.Rotary_Move_Low_Velocity = int.Parse((from m in configure_list where m.NAME == "Rotary_Move_Low_Velocity" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Rotary_Move_High_Velocity = int.Parse((from m in configure_list where m.NAME == "Rotary_Move_High_Velocity" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Rotary_Move_Acc = int.Parse((from m in configure_list where m.NAME == "Rotary_Move_Acc" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Rotary_Move_Dcc = int.Parse((from m in configure_list where m.NAME == "Rotary_Move_Dcc" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Rotary_Move_Jerk = int.Parse((from m in configure_list where m.NAME == "Rotary_Move_Jerk" select m.VALUE).First<string>());
          
            StaticRes.Global.System_Setting.Rotary_Homing_Pitch = int.Parse((from m in configure_list where m.NAME == "Rotary_Homing_Pitch" select m.VALUE).First<string>());

            StaticRes.Global.System_Setting.Index_1_Capacity = int.Parse((from m in configure_list where m.NAME == "Index_1_Capacity" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Index_2_Capacity = int.Parse((from m in configure_list where m.NAME == "Index_2_Capacity" select m.VALUE).First<string>());
            StaticRes.Global.System_Setting.Index_3_Capacity = int.Parse((from m in configure_list where m.NAME == "Index_3_Capacity" select m.VALUE).First<string>());
           

            StaticRes.Global.System_Setting.Bypass_Syringe_Top_Cover_Sensor = (from m in configure_list where m.NAME == "Bypass_Syringe_Top_Cover_Sensor" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Bypass_Syringe_30cc_Cap_Present_Sensor = (from m in configure_list where m.NAME == "Bypass_Syringe_30cc_Cap_Present_Sensor" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Bypass_Syringe_10cc_Cap_Present_Sensor = (from m in configure_list where m.NAME == "Bypass_Syringe_10cc_Cap_Present_Sensor" select m.VALUE).First<string>();
            StaticRes.Global.System_Setting.Bypass_Syringe_5cc_Cap_Present_Sensor = (from m in configure_list where m.NAME == "Bypass_Syringe_5cc_Cap_Present_Sensor" select m.VALUE).First<string>();

            StaticRes.Global.System_Setting.Time_1 = int.Parse((from m in configure_list where m.NAME == "Time_1" select m.VALUE).First<string>());//yaskun.zhou 2015.07.01
            StaticRes.Global.System_Setting.Reminder_time = (from m in configure_list where m.NAME == "Reminder_time" select m.VALUE).First<string>();//yaskun.zhou 2015.07.01

            DataTable dt = DataProvider.Local.Slot_Position.Select.All();
            for (int i = 0; i < 100; i++)
            {
                StaticRes.Global.Slot_Position[i] = int.Parse(dt.Rows[i]["POSITION"].ToString());
            }
            Motion_Control.Read_Motion_Config();
        }

        public static bool Save_Slot_Parameter(int Slot_ID,int Slot_Index,int Position)
        {
            StaticRes.Global.Slot_Position[Slot_ID - 1] = Position;
            return DataProvider.Local.Slot_Position.Update.By_SlotNo(Slot_ID, Slot_Index, Position);
        }
    }
}
