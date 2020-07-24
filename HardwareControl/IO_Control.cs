using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hardware;
using System.Reflection;

namespace HardwareControl
{
    public class IO_Control
    {
        public static bool X001_Emergency_Stop_and_Check(ref string Event_Name)
        {
            if (!Hardware.IO_LIST.Input.X100_Emergency_Stop())
            {
                HardwareControl.Motion_Control.Rotary_EMG_Stop();
                StaticRes.Global.Need_Homing = true;
                StaticRes.Global.Process_Code.Homing = "H000";
                StaticRes.Global.Process_Code.Loading = "L000";
                StaticRes.Global.Process_Code.Returning = "R000";
                StaticRes.Global.Process_Code.Unloading = "U000";
                Event_Name = "Error1";
                return true;
            }
            else if (!Hardware.IO_LIST.Input.X101_Pressure_Switch_CDA())
            {
                HardwareControl.Motion_Control.Rotary_EMG_Stop();
                StaticRes.Global.Need_Homing = true;
                StaticRes.Global.Process_Code.Homing = "H000";
                StaticRes.Global.Process_Code.Loading = "L000";
                StaticRes.Global.Process_Code.Returning = "R000";
                StaticRes.Global.Process_Code.Unloading = "U000";
                Event_Name = "Error2";
                return true;
            }
            return false;
        }

        public static bool X108_Gate_A_Close_and_Check()
        {
            if (Hardware.IO_LIST.Input.X108_Gate_A_Closed())
            {
                if (!Hardware.IO_LIST.Input.X109_Gate_A_Opened())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y104_Gate_A_Close())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (Hardware.IO_LIST.Input.X109_Gate_A_Opened() || !Hardware.IO_LIST.Input.X108_Gate_A_Closed());
            }
            return true;
        }

        public static bool X109_Gate_A_Open_and_Check()
        {
            if (Hardware.IO_LIST.Input.X109_Gate_A_Opened())
            {
                if (!Hardware.IO_LIST.Input.X108_Gate_A_Closed())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y104_Gate_A_Open())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X109_Gate_A_Opened() || Hardware.IO_LIST.Input.X108_Gate_A_Closed());
            }
            return true;
        }

        public static bool X110_Gate_B_Close_and_Check()
        {
            if (Hardware.IO_LIST.Input.X110_Gate_B_Closed())
            {
                if (!Hardware.IO_LIST.Input.X111_Gate_B_Opened())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y105_Gate_B_Close())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (Hardware.IO_LIST.Input.X111_Gate_B_Opened() || !Hardware.IO_LIST.Input.X110_Gate_B_Closed());
            }
            return true;
        }

        public static bool X111_Gate_B_Open_and_Check()
        {
            if (Hardware.IO_LIST.Input.X111_Gate_B_Opened())
            {
                if (!Hardware.IO_LIST.Input.X110_Gate_B_Closed())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y105_Gate_B_Open())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X111_Gate_B_Opened() || Hardware.IO_LIST.Input.X110_Gate_B_Closed());
            }
            return true;
        }

        public static bool X112_Gate_C_Close_and_Check()
        {
            if (Hardware.IO_LIST.Input.X112_Gate_C_Closed())
            {
                if (!Hardware.IO_LIST.Input.X113_Gate_C_Opened())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y106_Gate_C_Close())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (Hardware.IO_LIST.Input.X113_Gate_C_Opened() || !Hardware.IO_LIST.Input.X112_Gate_C_Closed());
            }
            return true;
        }

        public static bool X113_Gate_C_Open_and_Check()
        {
            if (Hardware.IO_LIST.Input.X113_Gate_C_Opened())
            {
                if (!Hardware.IO_LIST.Input.X112_Gate_C_Closed())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y106_Gate_C_Open())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X113_Gate_C_Opened() || Hardware.IO_LIST.Input.X112_Gate_C_Closed());
            }
            return true;
        }

        public static bool X114_Gate_D_Close_and_Check()
        {
            if (Hardware.IO_LIST.Input.X114_Gate_D_Closed())
            {
                if (!Hardware.IO_LIST.Input.X115_Gate_D_Opened())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y107_Gate_D_Close())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (Hardware.IO_LIST.Input.X115_Gate_D_Opened() || !Hardware.IO_LIST.Input.X114_Gate_D_Closed());
            }
            return true;
        }

        public static bool X115_Gate_D_Open_and_Check()
        {
            if (Hardware.IO_LIST.Input.X115_Gate_D_Opened())
            {
                if (!Hardware.IO_LIST.Input.X114_Gate_D_Closed())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y107_Gate_D_Open())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X115_Gate_D_Opened() || Hardware.IO_LIST.Input.X114_Gate_D_Closed());
            }
            return true;
        }

        public static bool X301_Weighing_Door_Close_and_Check()
        {
            if (Hardware.IO_LIST.Input.X301_Weighing_Door_Closed())
            {
                if (!Hardware.IO_LIST.Input.X300_Weighing_Door_Opened())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y109_Weighing_Door_Close())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X301_Weighing_Door_Closed() || Hardware.IO_LIST.Input.X300_Weighing_Door_Opened());
            }
            return true;
        }

        public static bool X300_Weighing_Door_Open_and_Check()
        {
            if (Hardware.IO_LIST.Input.X300_Weighing_Door_Opened())
            {
                if (!Hardware.IO_LIST.Input.X301_Weighing_Door_Closed())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y109_Weighing_Door_Open())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X300_Weighing_Door_Opened() || Hardware.IO_LIST.Input.X301_Weighing_Door_Closed());
            }
            return true;
        }

        public static bool X204_Weighing_Tray_Cylinder_Forward_and_Check()
        {
            if (Hardware.IO_LIST.Input.X204_Weighing_Tray_CYL_FWD())
            {
                if (!Hardware.IO_LIST.Input.X205_Weighing_Tray_CYL_BWD())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y110_Weighing_Tray_Forward())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X204_Weighing_Tray_CYL_FWD() || Hardware.IO_LIST.Input.X205_Weighing_Tray_CYL_BWD());
            }
            return true;
        }

        public static bool X205_Weighing_Tray_Cylinder_Backward_and_Check()
        {
            if (Hardware.IO_LIST.Input.X205_Weighing_Tray_CYL_BWD())
            {
                if (!Hardware.IO_LIST.Input.X204_Weighing_Tray_CYL_FWD())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y110_Weighing_Tray_Backward())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X205_Weighing_Tray_CYL_BWD() || Hardware.IO_LIST.Input.X204_Weighing_Tray_CYL_FWD());
            }
            return true;
        }

        public static bool X202_Scrap_Chute_To_Drawer_1_and_Check()
        {
            if (Hardware.IO_LIST.Input.X202_Scrap_Chute_CYL_To_Drawer_1())
            {
                if (!Hardware.IO_LIST.Input.X203_Scrap_Chute_CYL_To_Drawer_2())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y111_Scrap_Bin_Drawer_Chute_Close())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X202_Scrap_Chute_CYL_To_Drawer_1() || Hardware.IO_LIST.Input.X203_Scrap_Chute_CYL_To_Drawer_2());
            }
            return true;
        }

        public static bool X203_Scrap_Chute_To_Drawer_2_and_Check()
        {
            if (Hardware.IO_LIST.Input.X203_Scrap_Chute_CYL_To_Drawer_2())
            {
                if (!Hardware.IO_LIST.Input.X202_Scrap_Chute_CYL_To_Drawer_1())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y111_Scrap_Bin_Drawer_Chute_Open())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X203_Scrap_Chute_CYL_To_Drawer_2() || Hardware.IO_LIST.Input.X202_Scrap_Chute_CYL_To_Drawer_1());
            }
            return true;
        }

        public static bool X208_Ejector_A_Up_and_Check()
        {
            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In Function");

            if (Motion_Control.Got_Rotary_Speed() > 0)
            {
                Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "Motor still running, speed:"+ Motion_Control.Got_Rotary_Speed());
                return false;
            }



            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "X208_Ejector_A_Up");
            if (Hardware.IO_LIST.Input.X208_Ejector_A_Up())
            {
                Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "X208_Ejector_A_Up, result true");
                if (!Hardware.IO_LIST.Input.X209_Ejector_A_Down())
                {
                    Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "X209_Ejector_A_Down, result false");
                    return true;
                }
                   
            }


            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "X208_Ejector_A_Up, result false.  continue Y200_Ejector_A_Up");
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y200_Ejector_A_Up())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                    {
                        Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "Y200_Ejector_A_Up time out");
                        Hardware.IO_LIST.Output.Y200_Ejector_A_Down();
                        return false;
                    }
                }
                while (Hardware.IO_LIST.Input.X209_Ejector_A_Down() || !Hardware.IO_LIST.Input.X208_Ejector_A_Up());
            }
            return true;
        }

        public static bool X209_Ejector_A_Down_and_Check()
        {
            if (Hardware.IO_LIST.Input.X209_Ejector_A_Down())
            {
                if (!Hardware.IO_LIST.Input.X208_Ejector_A_Up())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y200_Ejector_A_Down())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X209_Ejector_A_Down() || Hardware.IO_LIST.Input.X208_Ejector_A_Up());
            }
            return true;
        }

        public static bool X210_Ejector_B_Up_and_Check()
        {
            if (Motion_Control.Got_Rotary_Speed() > 0)
                return false;
            if (Hardware.IO_LIST.Input.X210_Ejector_B_Up())
            {
                if (!Hardware.IO_LIST.Input.X211_Ejector_B_Down())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y201_Ejector_B_Up())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                    {
                        Hardware.IO_LIST.Output.Y201_Ejector_B_Down();
                        return false;
                    }
                }
                while (Hardware.IO_LIST.Input.X211_Ejector_B_Down() || !Hardware.IO_LIST.Input.X210_Ejector_B_Up());
            }
            return true;
        }

        public static bool X211_Ejector_B_Down_and_Check()
        {
            if (Hardware.IO_LIST.Input.X211_Ejector_B_Down())
            {
                if (!Hardware.IO_LIST.Input.X210_Ejector_B_Up())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y201_Ejector_B_Down())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X211_Ejector_B_Down() || Hardware.IO_LIST.Input.X210_Ejector_B_Up());
            }
            return true;
        }

        public static bool X212_Ejector_C_Up_and_Check()
        {
            if (Motion_Control.Got_Rotary_Speed() > 0)
                return false;
            if (Hardware.IO_LIST.Input.X212_Ejector_C_Up())
            {
                if (!Hardware.IO_LIST.Input.X213_Ejector_C_Down())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y202_Ejector_C_Up())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                    {
                        Hardware.IO_LIST.Output.Y202_Ejector_C_Down();
                        return false;
                    }
                }
                while (Hardware.IO_LIST.Input.X213_Ejector_C_Down() || !Hardware.IO_LIST.Input.X212_Ejector_C_Up());
            }
            return true;
        }

        public static bool X213_Ejector_C_Down_and_Check()
        {
            if (Hardware.IO_LIST.Input.X213_Ejector_C_Down())
            {
                if (!Hardware.IO_LIST.Input.X212_Ejector_C_Up())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y202_Ejector_C_Down())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X213_Ejector_C_Down() || Hardware.IO_LIST.Input.X212_Ejector_C_Up());
            }
            return true;
        }

        public static bool X214_Ejector_D_Up_and_Check()
        {
            if (Motion_Control.Got_Rotary_Speed() > 0)
                return false;
            if (Hardware.IO_LIST.Input.X214_Ejector_D_Up())
            {
                if (!Hardware.IO_LIST.Input.X215_Ejector_D_Down())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y203_Ejector_D_Up())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                    {
                        Hardware.IO_LIST.Output.Y203_Ejector_D_Down();
                        return false;
                    }
                }
                while (Hardware.IO_LIST.Input.X215_Ejector_D_Down() || !Hardware.IO_LIST.Input.X214_Ejector_D_Up());
            }
            return true;
        }

        public static bool X215_Ejector_D_Down_and_Check()
        {
            if (Hardware.IO_LIST.Input.X215_Ejector_D_Down())
            {
                if (!Hardware.IO_LIST.Input.X214_Ejector_D_Up())
                    return true;
            }
            DateTime Start_Time = System.DateTime.Now;
            if (Hardware.IO_LIST.Output.Y203_Ejector_D_Down())
            {
                do
                {
                    TimeSpan ts = System.DateTime.Now - Start_Time;
                    if (ts.TotalMilliseconds > 2000)
                        return false;
                }
                while (!Hardware.IO_LIST.Input.X215_Ejector_D_Down() || Hardware.IO_LIST.Input.X214_Ejector_D_Up());
            }
            return true;
        }

        public static void Green_Tower_Light_Setting()
        {
            if (!Hardware.IO_LIST.Output.Y100_Tower_Light_Green_On())
                throw new System.Exception("Turn On Green Light Failed !");
            if (!Hardware.IO_LIST.Output.Y101_Tower_Light_Yellow_Off())
                throw new System.Exception("Turn Off Armer Light Failed !");
            if (!Hardware.IO_LIST.Output.Y102_Tower_Light_Red_Off())
                throw new System.Exception("Turn Off Red Light Failed !");
            if (!Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off())
                throw new System.Exception("Turn Off Buzzer Failed !");
        }

        public static void Alarm_Tower_Light_Setting()
        {
            if (!Hardware.IO_LIST.Output.Y100_Tower_Light_Green_Off())
                throw new System.Exception("Turn Off Green Light Failed !");
            if (!Hardware.IO_LIST.Output.Y101_Tower_Light_Yellow_Off())
                throw new System.Exception("Turn Off Armer Light Failed !");
            if (!Hardware.IO_LIST.Output.Y102_Tower_Light_Red_On())
                throw new System.Exception("Turn On Red Light Failed !");
            if (!Hardware.IO_LIST.Output.Y103_Tower_Buzzer_On())
                throw new System.Exception("Turn On Buzzer Failed !");
        }

        public static void Yellow_Tower_Light_Setting()
        {
            if (!Hardware.IO_LIST.Output.Y100_Tower_Light_Green_Off())
                throw new System.Exception("Turn Off Green Light Failed !");
            if (!Hardware.IO_LIST.Output.Y101_Tower_Light_Yellow_On())
                throw new System.Exception("Turn On Armer Light Failed !");
            if (!Hardware.IO_LIST.Output.Y102_Tower_Light_Red_Off())
                throw new System.Exception("Turn Off Red Light Failed !");
            if (!Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off())
                throw new System.Exception("Turn Off Buzzer Failed !");
        }
    }
}
