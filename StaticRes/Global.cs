using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectModule;

namespace StaticRes
{
    public class Global
    {
        public static System.Windows.ResourceDictionary CurrentLanguageRes = new System.Windows.ResourceDictionary();

        public static string CurrentLanguage = string.Empty;

        public static string Remove_Slot_Position = string.Empty;

        public static ObjectModule.Local.User Current_User = new ObjectModule.Local.User();

        public static ObjectModule.Local.User Current_User_Cancel = new ObjectModule.Local.User();

        public static string Current_Humidity = "100";

        public const string SBMS_ID = "EMS_Vishay";

        public static int[,] Rotary_Slot_Position = new int[12, 26];
        public static int[,] Arm_X_Slot_Position = new int[12, 26];
        public static int[,] Arm_Y_Slot_Position = new int[12, 26];
        public static int[,] Arm_X_Load_Slot_Position = new int[8, 6];
        public static int[,] Arm_Y_Load_Slot_Position = new int[8, 6];

        public static class Version
        {
            public const int VersionNumber = 1;
            public const int RevisionNumber = 1;
            public const string Date = "2020.05.25";

            public static string GetInfo()
            {
                string info = " Version " + VersionNumber.ToString() +
                              " Revision " + RevisionNumber.ToString() +
                              " (" + Date + ")";

                return info;
            }
        }

        public static class User_Group
        {
            public const string Admin = "Admin";
            public const string Engineer = "Engineer";
            public const string Supervisor = "Supervisor";
            public const string Maintenance = "Maint";
            public const string MH = "MH";
            public const string OP = "OP";
        }

        public static class SlotScannerPort{
            public const string Index1Port = "COM1";
            public const string Index2Port = "COM2";
            public const string Index3Port = "COM3";
            public const string Index4Port = "COM4";

        }

        public static class Event_Type
        {
            public const string Alarm = "Alarm";
            public const string Event = "Event";
        }

        public static class Humidity_Data
        {
            public static string PV = string.Empty;
            public static string SV = string.Empty;
        }

        public static class Process_Code
        {
            public static string Homing = "H000";
            public static string Loading = "L000";
            public static string Unloading = "U000";
            public static string Returning = "R000";
            public static string Weighting = "W000";
            public static string Inventory_Check = "I000";
        }

        public static class Event
        {
            public const string Load = "LOAD";
            public const string Unload = "UNLOAD";
            public const string Reuse = "REUSE";
            public const string Scrap = "SCRAP";
            public const string Remove = "REMOVE";
        }

        public static class Status
        {
            public const string Load = "NEW";
            public const string PendingMix = "PENDING_MIX";
            public const string Unload = "IN_USED"; 
            public const string Reuse = "REUSE";
            public const string Scrap = "SCRAP";
            public const string Remove = "REMOVE";

            
        }

        public static class AxisNo
        {
            public const string Rotary = "Rotary";
            public const string Vertical = "Vertical";
        }

        public static class Binning_Status
        {
            public const string New = "NEW";
            public const string Reuse = "REUSE";
            public const string Empty = "EMPTY";
        }

        public static class CommonError
        {
            public static string Job = "EMPTY";
            public static string Carrier_ID = "EMPTY";
            public static string Slot_ID = "EMPTY";
            public static DateTime Error_Time = System.DateTime.Now;
            public static string Error_Name = string.Empty;
        }

        public static class Error_List
        {
            public const string Emergency_stop = "Error1";
            public const string Lower_air_pressure = "Error2";
            public const string Loading_door_not_closed = "Error3";
            public const string Loading_door_not_opened = "Error4";
            public const string Thawing_door_not_closed = "Error5";
            public const string Syringe_A_present = "Error6";
            public const string Syringe_A_not_present = "Error7";
            public const string Syringe_B_present = "Error8";
            public const string Syringe_B_not_present = "Error4"; //mistake , so change to 4 -YP 2014/12/21
            public const string Syringe_C_present = "Error9";
            public const string Syringe_C_not_present = "Error10";
            public const string Syringe_D_present = "Error11";
            public const string Syringe_D_not_present = "Error12";
            public const string Gate_A_not_closed = "Error13";
            public const string Gate_A_not_opened = "Error14";
            public const string Gate_B_not_closed = "Error15";
            public const string Gate_B_not_opened = "Error16";
            public const string Gate_C_not_closed = "Error17";
            public const string Gate_C_not_opened = "Error18";
            public const string Gate_D_not_closed = "Error19";
            public const string Gate_D_not_opened = "Error20";
            public const string Scrap_drawer_1_not_closed = "Error21";
            public const string Scrap_drawer_2_not_closed = "Error22";
            public const string Scrap_chute_cylinder_to_drawer_1_failed = "Error23";
            public const string Scrap_shute_cyiinder_to_drawer_2_failed = "Error24";
            public const string Weighing_tray_cylinder_forward_failed = "Error25";
            public const string Weighing_tray_cylinder_backward_failed = "Error26";
            public const string Scrap_drawer_1_not_present = "Error27";
            public const string Scrap_drawer_1_full = "Error28";
            public const string Scrap_drawer_2_not_present = "Error29";
            public const string Scrap_drawer_2_full = "Error30";
            public const string Ejector_A_not_up = "Error31";
            public const string Ejector_A_not_down = "Error32";
            public const string Ejector_B_not_up = "Error33";
            public const string Ejector_B_not_down = "Error34";
            public const string Ejector_C_not_up = "Error35";
            public const string Ejector_C_not_down = "Error36";
            public const string Ejector_D_not_up = "Error37";
            public const string Ejector_D_not_down = "Error38";
            public const string Weighing_door_not_opened = "Error39";
            public const string Weighing_door_not_closed = "Error40";
            public const string Syringe_top_cover_present = "Error41";
            public const string Syringe_top_cover_not_present = "Error42";
            public const string Syringe_30cc_cap_present = "Error43";
            public const string Syringe_30cc_cap_not_present = "Error44";
            public const string Syringe_10cc_cap_present = "Error45";
            public const string Syringe_10cc_cap_not_present = "Error46";
            public const string Syringe_5cc_cap_not_present = "Error47";
            public const string Syringe_5cc_cap_present = "Error48";
            public const string Syringe_present = "Error49";
            public const string Syringe_not_present = "Error50";
            public const string Motion_failed = "Error51";
            public const string Rotary_homing_failed = "Error52";
            public const string Please_homing_first = "Error53";
            public const string Weighing_Failed = "Error54";
            public const string Update_database_failed = "Error55";



            public const string SlotBarcode_Read_Fail = "Error56";
            public const string MissingPlace = "Error57";
        }

        public static class System_Setting
        {
            
            public static string SlotScanner_Index1Port = string.Empty;
            public static string SlotScanner_Index2Port = string.Empty;
            public static string SlotScanner_Index3Port = string.Empty;
            public static string SlotScanner_Index4Port = string.Empty;
            public static int SlotScanner_BaudRate = 0;
            public static int SlotScanner_DataBits = 0;
            public static int SlotScanner_ReceivedBytesThreshold = 0;
            


            public static string Print_Label_Before_Load = string.Empty;
            public static string Print_Label_After_Unload = string.Empty;
            public static string Weighing_Scale_COM_Port = string.Empty;
            public static int Weighing_Scale_BaudRate = 0;
            public static int Weighing_Scale_DataBits = 0;
            public static int Weighing_Scale_ReceivedBytesThreshold = 0;
            public static string Printer_COM_Port = string.Empty;
            public static int Printer_BaudRate = 0;
            public static int Printer_DataBits = 0;
            public static int Printer_ReceivedBytesThreshold = 0;
            public static string Handle_Scanner_COM_Port = string.Empty;
            public static int Handle_Scanner_BaudRate = 0;
            public static int Handle_Scanner_DataBits = 0;
            public static int Handle_Scanner_ReceivedBytesThreshold = 0;
            public static int Idle_Time_To_Exit = 0;
            public static string Online_Mode = string.Empty;
            public static string Allow_Manual_KeyIn = string.Empty;
            public static int Storage_Pre_Set_Qty = 0;
            public static int One_Cycle_Pulse = 0;
            public static int Rotary_Home_Low_Velocity = 0;
            public static int Rotary_Home_High_Velocity = 0;
            public static int Rotary_Home_Acc = 0;
            public static int Rotary_Home_Dcc = 0;
            public static int Rotary_Home_Jerk = 0;
            public static int Rotary_Move_Low_Velocity = 0;
            public static int Rotary_Move_High_Velocity = 0;
            public static int Rotary_Move_Acc = 0;
            public static int Rotary_Move_Dcc = 0;
            public static int Rotary_Move_Jerk = 0;  
            public static int Rotary_Homing_Pitch = 0;
            public static int Index_1_Capacity = 0;
            public static int Index_2_Capacity = 0;
            public static int Index_3_Capacity = 0;
            public static int Index_4_Capacity = 0;
            public static string Bypass_Syringe_Top_Cover_Sensor = string.Empty;
            public static string Bypass_Syringe_30cc_Cap_Present_Sensor = string.Empty;
            public static string Bypass_Syringe_10cc_Cap_Present_Sensor = string.Empty;
            public static string Bypass_Syringe_5cc_Cap_Present_Sensor = string.Empty;
            public static int Time_1 = 0;//yakun.zhou.2015.07.01
            public static string Reminder_time = string.Empty;
        }

        public static class Time_Frequency
        {
            public const string Daily = "Daily";
            public const string Weekly = "Weekly";
            public const string Monthly = "Monthly";
        }

        public static class Motion_Error
        {
            public const string Rotary_Alarm = "Rotary Motion Failed";
            public const string X_Axis_Alarm = "X-Axis Motion Failed";
            public const string Y_Axis_Alarm = "Y-Axis Motion Failed";
        }

        public static bool Scale_Open = false;

        public static string Com_Content = string.Empty;

        public static bool IsOnProgress = false;

        public static bool Homing_Complete = false;
        public static bool Need_Homing = true;

        public static int Device_Handle_IO_PCI1756 = 0;
        public static int Device_Handle_IO_PCI1733 = 1;

        public static bool Hardware_Connection = false;

        public static int[] Slot_Position = new int[100];

        public static int[] Scanner_Level = new int[7];

        #region Motion Parameter
        public class Rotary_Motor_Parameter
        {
            //Current read from config file
            public static int PlsPerUnit = 1;
            public static int MaxAcc = 50000000;
            public static int MaxVel = 5000000;
            public static int MaxDec = 50000000;
            public static int MaxJerk = 1;
            public static int VelHigh = 40000;
            public static int VelLow = 40000;
            public static int Acc = 20000;
            public static int Dec = 20000;
            public static int PlsInMde = 2;
            public static int PlsInLogic = 0;
            public static int PlsInSrc = 2;
            public static int PlsOutMde = 8;
            public static int AlmEnable = 0;
            public static int AlmLogic = 0;
            public static int AlmReact = 1;
            public static int InpEnable = 0;
            public static int InpLogic = 1;
            public static int ErcLogic = 1;
            public static int ErcOnTime = 8;
            public static int ErcOffTime = 4;
            public static int ErcEnMde = 0;
            public static int SdEnable = 2;
            public static int SdLogic = 2;
            public static int SdReact = 2;
            public static int SdLatch = 2;
            public static int ElEnable = 1;
            public static int ElLogic = 1;
            public static int ElReact = 0;
            public static int SwMelEnable = 0;
            public static int SwPelEnable = 0;
            public static int SwMelReact = 1;
            public static int SwPelReact = 1;
            public static int SwMelValue = -10000000;
            public static int SwPelValue = 10000000;
            public static int OrgLogic = 0;
            public static int EzLogic = 0;
            public static int PlsInMaxFreq = 1;
            public static int HomeExSwitchMode = 2;
            public static int HomeCrossDis = 10000;
            public static int HomeResetEnable = 1;
            public static int OrgReact = 1;
            public static int BacklashEnable = 0;
            public static int BacklashPulses = 10;
            public static int BacklashVel = 1000;
            public static int CmpSrc = 0;
            public static int CmpMethod = 0;
            public static int CmpPulseMode = 0;
            public static int CmpPulseLogic = 1;
            public static int CmpPulseWidth = 0;
            public static int CmpEnable = 0;
            public static int LatchEn = 0;
            public static int LatchLogic = 0;
            public static int LatchBufEnable = 2;
            public static int LatchBufMinDist = 0;
            public static int LatchBufEventNum = 0;
            public static int LatchBufSource = 2;
            public static int LatchBufAxisID = 65535;
            public static int LatchBufEdge = 65535;
            public static int GenDoEnable = 1;
            public static int ExtMasterSrc = 0;
            public static int ExtSelEnable = 2;
            public static int ExtPulseNum = 1;
            public static int ExtPulseInMode = 2;
            public static int ExtPresetNum = 1;
            public static int CamDoEnable = 0;
            public static int CamDOLoLimit = 10000;
            public static int CamDOHiLimit = 20000;
            public static int CamDoCmpSrc = 0;
            public static int CamDoLogic = 1;
            public static int ModuleRange = 0;
            public static int SimStartSource = 0;
            public static int IN1StopEnable = 0;
            public static int IN1StopLogic = 0;
            public static int IN1StopReact = 1;
            public static int IN2StopEnable = 0;
            public static int IN2StopLogic = 0;
            public static int IN2StopReact = 1;
            public static int IN4StopEnable = 0;
            public static int IN4StopLogic = 0;
            public static int IN4StopReact = 1;
            public static int IN5StopEnable = 0;
            public static int IN5StopLogic = 0;
            public static int IN5StopReact = 1;
            public static int PosLagEn = 0;
            public static int MaxPosLag = 0;
        }

        public class Spare_Motor_Parameter
        {
            //Current read from config file
            public static int PlsPerUnit = 1;
            public static int MaxAcc = 50000000;
            public static int MaxVel = 5000000;
            public static int MaxDec = 50000000;
            public static int MaxJerk = 1;
            public static int VelHigh = 8000;
            public static int VelLow = 2000;
            public static int Acc = 8000;
            public static int Dec = 8000;
            public static int PlsInMde = 2;
            public static int PlsInLogic = 0;
            public static int PlsInSrc = 2;
            public static int PlsOutMde = 16;
            public static int AlmEnable = 0;
            public static int AlmLogic = 0;
            public static int AlmReact = 1;
            public static int InpEnable = 0;
            public static int InpLogic = 1;
            public static int ErcLogic = 1;
            public static int ErcOnTime = 8;
            public static int ErcOffTime = 4;
            public static int ErcEnMde = 0;
            public static int SdEnable = 2;
            public static int SdLogic = 2;
            public static int SdReact = 2;
            public static int SdLatch = 2;
            public static int ElEnable = 1;
            public static int ElLogic = 0;
            public static int ElReact = 0;
            public static int SwMelEnable = 0;
            public static int SwPelEnable = 0;
            public static int SwMelReact = 1;
            public static int SwPelReact = 1;
            public static int SwMelValue = -10000000;
            public static int SwPelValue = 10000000;
            public static int OrgLogic = 0;
            public static int EzLogic = 0;
            public static int PlsInMaxFreq = 1;
            public static int HomeExSwitchMode = 2;
            public static int HomeCrossDis = 10000;
            public static int HomeResetEnable = 1;
            public static int OrgReact = 1;
            public static int BacklashEnable = 0;
            public static int BacklashPulses = 10;
            public static int BacklashVel = 1000;
            public static int CmpSrc = 0;
            public static int CmpMethod = 0;
            public static int CmpPulseMode = 0;
            public static int CmpPulseLogic = 1;
            public static int CmpPulseWidth = 0;
            public static int CmpEnable = 0;
            public static int LatchEn = 0;
            public static int LatchLogic = 0;
            public static int LatchBufEnable = 2;
            public static int LatchBufMinDist = 0;
            public static int LatchBufEventNum = 0;
            public static int LatchBufSource = 2;
            public static int LatchBufAxisID = 65535;
            public static int LatchBufEdge = 65535;
            public static int GenDoEnable = 1;
            public static int ExtMasterSrc = 0;
            public static int ExtSelEnable = 2;
            public static int ExtPulseNum = 1;
            public static int ExtPulseInMode = 2;
            public static int ExtPresetNum = 1;
            public static int CamDoEnable = 0;
            public static int CamDOLoLimit = 10000;
            public static int CamDOHiLimit = 20000;
            public static int CamDoCmpSrc = 0;
            public static int CamDoLogic = 1;
            public static int ModuleRange = 0;
            public static int SimStartSource = 0;
            public static int IN1StopEnable = 0;
            public static int IN1StopLogic = 0;
            public static int IN1StopReact = 1;
            public static int IN2StopEnable = 0;
            public static int IN2StopLogic = 0;
            public static int IN2StopReact = 1;
            public static int IN4StopEnable = 0;
            public static int IN4StopLogic = 0;
            public static int IN4StopReact = 1;
            public static int IN5StopEnable = 0;
            public static int IN5StopLogic = 0;
            public static int IN5StopReact = 1;
            public static int PosLagEn = 0;
            public static int MaxPosLag = 0;
        }
        #endregion

        public class Motor_Parameter
        {
            public const Int16 ExisCards_No = 0;
            public const Int16 X_Axis_No = 0;
            public const Int16 PLS_Outmode = 4; //Set CW/CCW mode
            public static Int16 Servo_On = 0; //0:Servo_On ; 1:Servo_Off
            public static Double StrVel = 0;
            public static Double MaxVel = 0;
            public static Double Tacc = 0;
            public static Double Tdec = 0;

            public static IntPtr[] m_Axishand = new IntPtr[32];
            public static IntPtr m_DeviceHandle = IntPtr.Zero;
            public static uint m_ulAxisCount = 0;
            public static bool m_bInit = false;
        }

        public static string Last_Error = string.Empty;
        public static DateTime Last_Error_Time = System.DateTime.Now;

        public static bool Transaction_Continue = true;
        public static bool FingerPrint_Continue = true;
        public static bool Re_scan = false;
        //public static bool updata = false;
        public static string Pt = string.Empty;
        
    }
}
