using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ObjectModule.Local;
using StaticRes;

namespace HardwareControl
{
    public class Load
    {
        private ObjectModule.Local.Event et = new Event();

        #region delegate
        public delegate void LoadCompleteEventHandler(bool Complete);
        private LoadCompleteEventHandler loadcomplete;
        public event LoadCompleteEventHandler LoadComplete
        {
            add
            {
                loadcomplete += value;
            }
            remove
            {
                loadcomplete -= value;
            }
        }

        public delegate void loaderrorEventHandler(ObjectModule.Local.Event ee);
        private loaderrorEventHandler loaderror;
        public event loaderrorEventHandler LoadError
        {
            add
            {
                loaderror += value;
            }
            remove
            {
                loaderror -= value;
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
        #endregion

        private Thread Thread_Load = null;

        public void Stop()
        {
            this.Thread_Load.Abort();
        }

        void Load_LoadComplete(bool Complete)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Load_LoadError(ObjectModule.Local.Event x)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Load_Step(string Event)
        {
        }

        class Params
        {
            public int Slot_ID;
            public int Slot_Index;
        }

        public int Start_Load(int Slot_ID,int Slot_Index)
        {
            if (StaticRes.Global.IsOnProgress)
            {
                loaderror(et);
                return 0;
            }
            else
                StaticRes.Global.IsOnProgress = true;
            try
            {
                Params x = new Params();
                x.Slot_ID = Slot_ID;
                x.Slot_Index = Slot_Index;
                LoadError+=new loaderrorEventHandler(Load_LoadError);
                LoadComplete+=new LoadCompleteEventHandler(Load_LoadComplete);
                Step+=new StepEventHandler(Load_Step);
                this.Thread_Load = new Thread(new ParameterizedThreadStart(Loading));
                this.Thread_Load.Start(x);
            }
            catch
            {
                loaderror(et);
            }
            return 10;
        }

        private void Loading(object x)
        {
            try
            {
                Params y = (Params)x;
                LogicLoad(y.Slot_ID,y.Slot_Index);
            }
            catch
            {
                //StaticRes.Global.Process_Code.Loading = "L000";
            }
        }

        private void LogicLoad(int Slot_ID, int Slot_Index)
        {
            try
            {
                if (!StaticRes.Global.Hardware_Connection)
                {
                    loadcomplete(true);
                    return;
                }
                if (!StaticRes.Global.Need_Homing)
                {              

                    #region  ***(L000)*** Rotary move to point position
                    if (StaticRes.Global.Process_Code.Loading == "L000" && StaticRes.Global.Transaction_Continue)
                    {                        
                        IO_Control.Green_Tower_Light_Setting();
                        step("L000 - Rotary move to Slot-" + Slot_ID.ToString() + ",Position:" + StaticRes.Global.Slot_Position[Slot_ID - 1].ToString() + "");//" + StaticRes.Global.Slot_Position[Slot_ID - 1].ToString + "
                        try
                        {
                            Motion_Control.Rotary_MoveTo_Slot(Slot_ID);
                            Common.Reports.LogFile.Log("load move to slot successful ,current position:" + Motion_Control.Got_Rotary_Position().ToString() + ""  );
                            Motion_Control.Motion_Speed_Checking();                          
                            StaticRes.Global.Process_Code.Loading = "L200";
                        }
                        catch
                        {
                            Error_Throw(StaticRes.Global.Error_List.Motion_failed, "L000");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L200)*** Check slot index and move to related process
                    if (StaticRes.Global.Process_Code.Loading == "L200" && StaticRes.Global.Transaction_Continue)
                    {
                        switch (Slot_Index)
                        {
                            case 1:
                                StaticRes.Global.Process_Code.Loading = "L300";
                                break;
                            case 2:
                                StaticRes.Global.Process_Code.Loading = "L700";
                                break;
                            case 3:
                                StaticRes.Global.Process_Code.Loading = "L1100";
                                break;
                            case 4:
                                StaticRes.Global.Process_Code.Loading = "L1500";
                                break;
                        }
                    }
                    #endregion

                    #region Index 1
                    #region ***(L300)*** Open Gate A and check X109
                    if (StaticRes.Global.Process_Code.Loading == "L300" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L300 - Open Gate A and check X109");
                        if (IO_Control.X109_Gate_A_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Loading = "L400";
                            loadcomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_A_not_opened, "L300");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L400)*** Check loading door close sensor X102
                    if (StaticRes.Global.Process_Code.Loading == "L400" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L400 - Close loading door and check X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Loading = "L500";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "L400");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L500)*** Check syringe A present sensor X104
                    if (StaticRes.Global.Process_Code.Loading == "L500" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L500 - Check syringe A present sensor X104");
                        if (Hardware.IO_LIST.Input.X104_Syringe_A_Present())
                            StaticRes.Global.Process_Code.Loading = "L600";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_A_not_present, "L500");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L600)*** Close gate A and check X108
                    if (StaticRes.Global.Process_Code.Loading == "L600" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L600 - Close Gate A and check X108");
                        if (IO_Control.X108_Gate_A_Close_and_Check())
                            StaticRes.Global.Process_Code.Loading = "L1900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_A_not_closed, "L600");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 2
                    #region ***(L700)*** Open Gate B and check X111
                    if (StaticRes.Global.Process_Code.Loading == "L700" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L700 - Open Gate B and check X111");
                        if (IO_Control.X111_Gate_B_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Loading = "L800";
                            loadcomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_B_not_opened, "L700");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L800)*** Check loading door close sensor X102
                    if (StaticRes.Global.Process_Code.Loading == "L800" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L800 - Close loading door and check X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Loading = "L900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "L800");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L900)*** Check syringe B present sensor X105
                    if (StaticRes.Global.Process_Code.Loading == "L900" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L900 - Check syringe B present sensor X105");
                        if (Hardware.IO_LIST.Input.X105_Syringe_B_Present())
                            StaticRes.Global.Process_Code.Loading = "L1000";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_B_not_present, "L900");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L1000)*** Close gate B and check X110
                    if (StaticRes.Global.Process_Code.Loading == "L1000" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1000 - Close gate B and check X110 sensor");
                        if (IO_Control.X110_Gate_B_Close_and_Check())
                            StaticRes.Global.Process_Code.Loading = "L1900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_B_not_closed, "L1000");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 3
                    #region ***(L1100)*** Open Gate C and check X113
                    if (StaticRes.Global.Process_Code.Loading == "L1100" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1100 - Open Gate C and check X113");
                        if (IO_Control.X113_Gate_C_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Loading = "L1200";
                            loadcomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_C_not_opened, "L1100");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L1200)*** Check loading door close sensor X102
                    if (StaticRes.Global.Process_Code.Loading == "L1200" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1200 - Check loading door close sensor X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Loading = "L1300";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "L1200");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L1300)*** Check syringe C present sensor X106
                    if (StaticRes.Global.Process_Code.Loading == "L1300" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1300 - Check syringe C present sensor X106");
                        if (Hardware.IO_LIST.Input.X106_Syringe_C_Present())
                            StaticRes.Global.Process_Code.Loading = "L1400";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_C_not_present, "L1300");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L1400)*** Close gate C and check X112
                    if (StaticRes.Global.Process_Code.Loading == "L1400" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1400 - Close gate C and check X112 sensor");
                        if (IO_Control.X112_Gate_C_Close_and_Check())
                            StaticRes.Global.Process_Code.Loading = "L1900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_C_not_closed, "L1400");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 4
                    #region ***(L1500)*** Open Gate D and check X115
                    if (StaticRes.Global.Process_Code.Loading == "L1500" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1500 - Open Gate D and check X115");
                        if (IO_Control.X115_Gate_D_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Loading = "L1600";
                            loadcomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_D_not_opened, "L1500");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L1600)*** Check loading door close sensor X102
                    if (StaticRes.Global.Process_Code.Loading == "L1600" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1600 - Check loading door close sensor X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Loading = "L1700";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "L1600");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L1700)*** Check syringe D present sensor X107
                    if (StaticRes.Global.Process_Code.Loading == "L1700" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1700 - Check syringe D present sensor X107");
                        if (Hardware.IO_LIST.Input.X107_Syringe_D_Present())
                            StaticRes.Global.Process_Code.Loading = "L1800";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_C_not_present, "L1700");
                            return;
                        }
                    }
                    #endregion

                    #region ***(L1800)*** Close gate D and check X114
                    if (StaticRes.Global.Process_Code.Loading == "L1800" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1800 - Close gate D and check X114 sensor");
                        if (IO_Control.X114_Gate_D_Close_and_Check())
                            StaticRes.Global.Process_Code.Loading = "L1900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_D_not_closed, "L1800");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region ***(L1900)*** Loading completed , rotary continue moving
                    if (StaticRes.Global.Process_Code.Loading == "L1900" && StaticRes.Global.Transaction_Continue)
                    {
                        step("L1900 - Loading completed");
                        try
                        {
                            IO_Control.Yellow_Tower_Light_Setting();
                            StaticRes.Global.Process_Code.Loading = "L000";
                            StaticRes.Global.IsOnProgress = false;
                            loadcomplete(true);
                            return;
                        }
                        catch (Exception ee)
                        {
                            Error_Throw(StaticRes.Global.Error_List.Motion_failed, "L1900");
                            return;
                        }
                    }
                    #endregion

                    return;
                }
                else
                {
                    Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "L000");
                    return;
                }
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Loading Error -- " + ee.Message + ",L000 , user:" + StaticRes.Global.Current_User.USER_ID);
                Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "From Load Exception");
                return;
            }
        }
        void Error_Throw(string Event_Name, string Process)
        {
            et.EVENT_NAME = Event_Name;
            et.PROCESS_CODE = Process;
            loaderror(et);
        }

        public void LoadingHandle()
        {
            try
            {
                HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                this.Thread_Load.Abort();
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Load error when stop -- " + ee.Message + " , user:" + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Need_Homing = true;
            }
        }
    }
}
