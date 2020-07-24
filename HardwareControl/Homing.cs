using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectModule.Local;
using System.Threading;
using System.Data;

namespace HardwareControl
{
    public class Homing
    {
        private ObjectModule.Local.Event et = new Event();
        public delegate void HomeCompleteEventHandler();
        private HomeCompleteEventHandler homecomplete;
        public event HomeCompleteEventHandler HomeComplete
        {
            add
            {
                homecomplete += value;
            }
            remove
            {
                homecomplete -= value;
            }
        }

        public delegate void HomeErrorEventHandler(ObjectModule.Local.Event lh);
        private HomeErrorEventHandler homeerror;
        public event HomeErrorEventHandler HomeError
        {
            add
            {
                homeerror += value;
            }
            remove
            {
                homeerror -= value;
            }
        }

        public delegate void StepEventHandler(string Event);
        private StepEventHandler step;
        public event StepEventHandler Step
        {
            add
            {
                step += value;
            }
            remove
            {
                step -= value;
            }
        }

        private Thread Thread_Home = null;

        public void Stop()
        {
            this.Thread_Home.Abort();
        }

        void Home_HomeComplete()
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Home_HomeError(ObjectModule.Local.Event x)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Home_Step(string Event)
        {
        }

        class Params
        {
            public bool Continue;
        }

        public int Start_Home(bool Continue)
        {
            if (StaticRes.Global.IsOnProgress)
            {
                homeerror(et);
                return 0;
            }
            else
                StaticRes.Global.IsOnProgress = true;
            try
            {
                Params x = new Params();
                x.Continue = Continue;
                homeerror += new HomeErrorEventHandler(Home_HomeError);
                homecomplete += new HomeCompleteEventHandler(Home_HomeComplete);
                Step += new StepEventHandler(Home_Step);
                this.Thread_Home = new Thread(new ParameterizedThreadStart(Homeing));
                this.Thread_Home.Start(x);
            }
            catch
            {
                homeerror(et);
            }
            return 23;
        }

        private void Homeing(object x)
        {
            try
            {
                Params y = (Params)x;
                LogicHome(y.Continue);
            }
            catch
            {
                StaticRes.Global.Process_Code.Loading = "H000";
            }
        }

        private void LogicHome(bool Continue)
        {
            try
            {
                
                System.IO.Ports.SerialPort soltScanner_1 = new System.IO.Ports.SerialPort(StaticRes.Global.SlotScannerPort.Index1Port);
                System.IO.Ports.SerialPort soltScanner_2 = new System.IO.Ports.SerialPort(StaticRes.Global.SlotScannerPort.Index2Port);
                System.IO.Ports.SerialPort soltScanner_3 = new System.IO.Ports.SerialPort(StaticRes.Global.SlotScannerPort.Index3Port);
                System.IO.Ports.SerialPort soltScanner_4 = new System.IO.Ports.SerialPort(StaticRes.Global.SlotScannerPort.Index4Port);

                if (soltScanner_1.IsOpen)
                    soltScanner_1.Close();
                if (soltScanner_2.IsOpen)
                    soltScanner_2.Close();
                if (soltScanner_3.IsOpen)
                    soltScanner_3.Close();
                if (soltScanner_4.IsOpen)
                    soltScanner_4.Close();



                if (!StaticRes.Global.Hardware_Connection)
                {
                    homecomplete();
                    return;
                }
                Motion_Control.Rotary_Motion_Stop();
                #region ***(H000)*** Turn on green tower light
                if (StaticRes.Global.Process_Code.Homing == "H000" && StaticRes.Global.Transaction_Continue)
                {
                    step("H000 - Turn on green light");
                    HardwareControl.IO_Control.Green_Tower_Light_Setting();
                    StaticRes.Global.Process_Code.Homing = "H100";
                }
                #endregion

                #region ***(H100)*** Loading door close and check X102 sensor
                if (StaticRes.Global.Process_Code.Homing == "H100" && StaticRes.Global.Transaction_Continue)
                {
                    step("H100 - Loading door close and check X102 sensor");
                    if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                        StaticRes.Global.Process_Code.Homing = "H200";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "H100");
                        return;
                    }
                }
                #endregion

                #region ***(H200)*** Check thawing box door closed sensor X103
                if (StaticRes.Global.Process_Code.Homing == "H200" && StaticRes.Global.Transaction_Continue)
                {
                    step("H200 - Check thawing door close sensor X103 ");
                    if (Hardware.IO_LIST.Input.X103_Thawing_Door_Closed())
                    {
                        StaticRes.Global.Process_Code.Homing = "H300";
                    }
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Thawing_door_not_closed, "H200");
                        return;
                    }
                }
                #endregion

                #region ***(H300)*** Check syringe top cover present sensor X302
                if (StaticRes.Global.Process_Code.Homing == "H300" && StaticRes.Global.Transaction_Continue)
                {
                    step("H300 - Check syringe top cover present sensor X302");
                    if (!Hardware.IO_LIST.Input.X302_Syringe_Top_Cover_Present())
                        StaticRes.Global.Process_Code.Homing = "H400";
                    else
                    {
                        if (IO_Control.X300_Weighing_Door_Open_and_Check())
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_top_cover_present, "H300");
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "H300");
                            return;
                        }
                    }
                }
                #endregion

                #region ***(H400)*** Check 30cc syringe cap present sensor X303
                if (StaticRes.Global.Process_Code.Homing == "H400" && StaticRes.Global.Transaction_Continue)
                {
                    step("H400 - Check 30 CC syringe cap present sensor X303");
                    if (!Hardware.IO_LIST.Input.X303_30cc_Syringe_Cap_Present())
                        StaticRes.Global.Process_Code.Homing = "H500";
                    else
                    {
                        if (IO_Control.X300_Weighing_Door_Open_and_Check())
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_30cc_cap_present, "H400");
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "H400");
                            return;
                        }
                    }
                }
                #endregion

                #region ***(H500)*** Check 10 cc syringe cap present sensor X304
                if (StaticRes.Global.Process_Code.Homing == "H500" && StaticRes.Global.Transaction_Continue)
                {
                    step("H500 - Check 10 CC syringe cap present sensor X304");
                    if (!Hardware.IO_LIST.Input.X304_10cc_Syringe_Cap_Present())
                        StaticRes.Global.Process_Code.Homing = "H600";
                    else
                    {
                        if (IO_Control.X300_Weighing_Door_Open_and_Check())
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_10cc_cap_present, "H500");
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "H500");
                            return;
                        }
                    }
                }
                #endregion

                #region ***(H600)*** Check 5cc syringe cap present sensor X305
                if (StaticRes.Global.Process_Code.Homing == "H600" && StaticRes.Global.Transaction_Continue)
                {
                    step("H600 - Check 5CC syringe cap present sensor X305");
                    if (!Hardware.IO_LIST.Input.X304_10cc_Syringe_Cap_Present())
                        StaticRes.Global.Process_Code.Homing = "H700";
                    else
                    {
                        if (IO_Control.X300_Weighing_Door_Open_and_Check())
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_5cc_cap_present, "H600");
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "H600");
                            return;
                        }
                    }
                }
                #endregion

                #region ***(H700)*** Check syringe present sensor X306
                if (StaticRes.Global.Process_Code.Homing == "H700" && StaticRes.Global.Transaction_Continue)
                {
                    step("H700 - Check syringe present sensor X306");
                    if (!Hardware.IO_LIST.Input.X306_Syringe_Present())
                        StaticRes.Global.Process_Code.Homing = "H800";
                    else
                    {
                        if (IO_Control.X300_Weighing_Door_Open_and_Check())
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_present, "H700");
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "H700");
                            return;
                        }
                    }
                }
                #endregion

                #region ***(H800)*** Weighing door not close and check X301 sensor
                if (StaticRes.Global.Process_Code.Homing == "H800" && StaticRes.Global.Transaction_Continue)
                {
                    step("H800 - Weighing door close and check X301 sensor");
                    if (IO_Control.X301_Weighing_Door_Close_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H900";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_closed, "H800");
                        return;
                    }
                }
                #endregion

                #region ***(H900)*** Null
                if (StaticRes.Global.Process_Code.Homing == "H900" && StaticRes.Global.Transaction_Continue)
                {
                     StaticRes.Global.Process_Code.Homing = "H1000";
                }
                #endregion

                #region***(H1000)*** Null
                if (StaticRes.Global.Process_Code.Homing == "H1000" && StaticRes.Global.Transaction_Continue)
                {
                    StaticRes.Global.Process_Code.Homing = "H1100";
                }
                #endregion

                #region ***(H1100)*** Close scrap drawer 1 and check X200
                if (StaticRes.Global.Process_Code.Homing == "H1100" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1100 - Close scrap drawer 1 and check X200");
                    if (Hardware.IO_LIST.Input.X200_Scrap_Drawer_1_Closed())
                        StaticRes.Global.Process_Code.Homing = "H1200";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Scrap_drawer_1_not_closed, "H1100");
                        return;
                    }
                }
                #endregion

                #region ***(H1200)*** Close scrap drawer 2 and check X201
                if (StaticRes.Global.Process_Code.Homing == "H1200" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1200 - Close scrap drawer 2 and check X201");
                    if (Hardware.IO_LIST.Input.X201_Scrap_Drawer_2_Closed())
                        StaticRes.Global.Process_Code.Homing = "H1300";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Scrap_drawer_2_not_closed, "H1200");
                        return;
                    }
                }
                #endregion

                #region ***(H1300)*** Weighing tray forward and check X204
                if (StaticRes.Global.Process_Code.Homing == "H1300" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1300 - Weighing tray forward and check X204");
                    if (IO_Control.X204_Weighing_Tray_Cylinder_Forward_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H1400";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Weighing_tray_cylinder_forward_failed, "H1300");
                        return;
                    }
                }
                #endregion

                #region ***(H1400)*** Ejector A down and check X209
                if (StaticRes.Global.Process_Code.Homing == "H1400" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1400 - Ejector A down and check X209");
                    if (IO_Control.X209_Ejector_A_Down_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H1500";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Ejector_A_not_down, "H1400");
                        return;
                    }
                }
                #endregion

                #region ***(H1500)*** Ejector B down and check X211
                if (StaticRes.Global.Process_Code.Homing == "H1500" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1500 - Ejector B down and check X211");
                    if (IO_Control.X211_Ejector_B_Down_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H1600";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Ejector_B_not_down, "H1500");
                        return;
                    }
                }
                #endregion

                #region ***(H1600)*** Ejector C down and check X213
                if (StaticRes.Global.Process_Code.Homing == "H1600" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1600 - Ejector C down and check X213");
                    if (IO_Control.X213_Ejector_C_Down_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H1700";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Ejector_C_not_down, "H1600");
                        return;
                    }
                }
                #endregion

                #region ***(H1700)*** Ejector D down and check X215
                if (StaticRes.Global.Process_Code.Homing == "H1700" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1700 - Ejector D down and check X215");
                    if (IO_Control.X215_Ejector_D_Down_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H1800";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Ejector_D_not_down, "H1700");
                        return;
                    }
                }
                #endregion

                #region ***(H1800)*** Close Gate A and check X108
                if (StaticRes.Global.Process_Code.Homing == "H1800" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1800 - Close Gate A and check X108 ");
                    if (IO_Control.X108_Gate_A_Close_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H1900";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Gate_A_not_closed, "H1800");
                        return;
                    }
                }
                #endregion

                #region ***(H1900)*** Close Gate B and check X110
                if (StaticRes.Global.Process_Code.Homing == "H1900" && StaticRes.Global.Transaction_Continue)
                {
                    step("H1900 - Close Gate B and check X110 ");
                    if (IO_Control.X110_Gate_B_Close_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H2000";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Gate_B_not_closed, "H1900");
                        return;
                    }
                }
                #endregion

                #region ***(H2000)*** Close Gate C and check X112
                if (StaticRes.Global.Process_Code.Homing == "H2000" && StaticRes.Global.Transaction_Continue)
                {
                    step("H2000 - Close Gate C and check X112 ");
                    if (IO_Control.X112_Gate_C_Close_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H2100";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Gate_A_not_closed, "H2000");
                        return;
                    }
                }
                #endregion

                #region ***(H2100)*** Close Gate D and check X114
                if (StaticRes.Global.Process_Code.Homing == "H2100" && StaticRes.Global.Transaction_Continue)
                {
                    step("H2100 - Close Gate D and check X114 ");
                    if (IO_Control.X114_Gate_D_Close_and_Check())
                        StaticRes.Global.Process_Code.Homing = "H2200";
                    else
                    {
                        Error_Throw(StaticRes.Global.Error_List.Gate_A_not_closed, "H2100");
                        return;
                    }
                }
                #endregion

                #region ***(H2200)*** Rotary Homing
                if (StaticRes.Global.Process_Code.Homing == "H2200" && StaticRes.Global.Transaction_Continue)
                {
                    try
                    {
                        step("H2200 - Rotary motor homing ");     
                        int i = 1;
                        do
                        {
                            i++;
                            HardwareControl.Motion_Control.Motion_Speed_Checking();
                            HardwareControl.Motion_Control.Rotary_Move(100000);
                            HardwareControl.Motion_Control.Motion_Speed_Checking();
                            HardwareControl.Motion_Control.Rotary_Motor_Homing();
                            HardwareControl.Motion_Control.Motion_Speed_Checking();
                            HardwareControl.Motion_Control.Rotary_Move(-StaticRes.Global.System_Setting.Rotary_Homing_Pitch);
                            HardwareControl.Motion_Control.Motion_Speed_Checking();
                        }
                        while (!HardwareControl.Motion_Control.Rotary_Homing_Sensor_On() && i < 3);           
                        if (HardwareControl.Motion_Control.Rotary_Homing_Sensor_On())
                        {
                            HardwareControl.Motion_Control.Set_Rotary_Zero_Position();
                            StaticRes.Global.Process_Code.Homing = "H2300";
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Rotary_homing_failed, "H2200");
                            return;
                        }
                    }
                    catch
                    {
                    }
                }
                #endregion

                #region ***(H2300)*** Move to standard position
                if (StaticRes.Global.Process_Code.Homing == "H2300" && StaticRes.Global.Transaction_Continue)
                {
                    step("H2300 - Homing complete");
                    try
                    {
                        StaticRes.Global.Process_Code.Homing = "H000";
                        StaticRes.Global.Process_Code.Loading = "L000";
                        StaticRes.Global.Process_Code.Unloading = "U000";
                        StaticRes.Global.Process_Code.Returning = "R000";
                        StaticRes.Global.Process_Code.Weighting = "W000";
                        StaticRes.Global.Need_Homing = false;
                        DataTable dt = DataProvider.Local.Binning.Select.Bin_WithMateria();
                        if (dt.Rows.Count > 0)
                        {
                            HardwareControl.Motion_Control.Rotary_Continue_Move();
                        }
                    }
                    catch(Exception ex)
                    {
                        Common.Reports.LogFile.Log("Homing H2300 Exception, Msg:" + ex.ToString());
                        Error_Throw(StaticRes.Global.Error_List.Motion_failed, "H2300");
                        return;
                    }
                }
                #endregion
                StaticRes.Global.IsOnProgress = false;
                homecomplete();
                return;
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Homing Error -- " + ee.Message + ",H000 , user:" + StaticRes.Global.Current_User.USER_ID);
                Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "From Homing Exception");
                return;
            }
        }

        void Error_Throw(string Event_Name, string Process)
        {
            et.EVENT_NAME = Event_Name;
            et.PROCESS_CODE = Process;
            homeerror(et);
        }
    }
}
