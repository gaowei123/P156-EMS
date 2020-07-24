using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ObjectModule.Local;
using StaticRes;

namespace HardwareControl
{
    public class Return
    {
        private ObjectModule.Local.Event et = new Event();
        #region delegate
        public delegate void ReturnCompleteEventHandler(bool complete);
        private ReturnCompleteEventHandler returncomplete;
        public event ReturnCompleteEventHandler ReturnComplete
        {
            add
            {
                returncomplete += value;
            }
            remove
            {
                returncomplete -= value;
            }
        }

        public delegate void ReturnerrorEventHandler(ObjectModule.Local.Event ee);
        private ReturnerrorEventHandler returnerror;
        public event ReturnerrorEventHandler ReturnError
        {
            add
            {
                returnerror += value;
            }
            remove
            {
                returnerror -= value;
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

        private Thread Thread_Return = null;

        public void Stop()
        {
            this.Thread_Return.Abort();
        }

        void Return_ReturnComplete(bool complete)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Return_ReturnError(ObjectModule.Local.Event x)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Return_Step(string Event)
        {
        }

        class Params
        {
            public int Slot_ID;
            public int Slot_Index;
            public bool Reuse;
        }

        public int Start_Return(int Slot_ID,int SLot_Index,bool Reuse)
        {
            if (StaticRes.Global.IsOnProgress)
            {
                returnerror(et);
                return 0;
            }
            else
                StaticRes.Global.IsOnProgress =true;
            try
            {
                Params x = new Params();
                x.Slot_ID = Slot_ID;
                x.Slot_Index = SLot_Index;
                x.Reuse = Reuse;
                ReturnError+=new ReturnerrorEventHandler(Return_ReturnError);
                ReturnComplete+=new ReturnCompleteEventHandler(Return_ReturnComplete);
                Step+=new StepEventHandler(Return_Step);
                this.Thread_Return = new Thread(new ParameterizedThreadStart(Returning));
                this.Thread_Return.Start(x);
            }
            catch
            {
                returnerror(et);
            }
            return 30;
        }

        private void Returning(object x)
        {
            try
            {
                Params y = (Params)x;
                LogicReturn(y.Slot_ID,y.Slot_Index,y.Reuse);
            }
            catch
            {
                //StaticRes.Global.Process_Code.Loading = "L000";
            }
        }

        private void LogicReturn(int Slot_ID,int Slot_Index,bool Reuse)
        {
            try
            {
                if (!StaticRes.Global.Hardware_Connection)
                {
                    returncomplete(true);
                    return;
                }
                if (!StaticRes.Global.Need_Homing)
                {
                    #region  ***(R000)*** Rotary move to point position
                    if (StaticRes.Global.Process_Code.Returning == "R000" && StaticRes.Global.Transaction_Continue)
                    {                       
                        IO_Control.Green_Tower_Light_Setting();
                        step("R000 -  Rotary move to Slot-" + Slot_ID.ToString() + ",Position:" + StaticRes.Global.Slot_Position[Slot_ID - 1].ToString() + "");
                        try
                        {
                            //Motion_Control.Motion_Speed_Checking();
                            //if (StaticRes.Global.Need_Homing)
                            //{
                            //    Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "L000");
                            //    return;
                            //}
                            Motion_Control.Rotary_MoveTo_Slot(Slot_ID);
                            Motion_Control.Motion_Speed_Checking();
                            StaticRes.Global.Process_Code.Returning = "R200";
                        }
                        catch(Exception ex)
                        {
                            Common.Reports.LogFile.Log("Return R000, Exception:" + ex.ToString());
                            Error_Throw(StaticRes.Global.Error_List.Motion_failed, "R000");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R200)*** Check slot index and move to related process
                    if (StaticRes.Global.Process_Code.Returning == "R200" && StaticRes.Global.Transaction_Continue)
                    {
                        switch (Slot_Index)
                        {
                            case 1:
                                StaticRes.Global.Process_Code.Returning = "R300";
                                break;
                            case 2:
                                StaticRes.Global.Process_Code.Returning = "R700";
                                break;
                            case 3:
                                StaticRes.Global.Process_Code.Returning = "R1100";
                                break;
                            case 4:
                                StaticRes.Global.Process_Code.Returning = "R1500";
                                break;
                        }
                    }
                    #endregion

                    #region Index 1
                    #region ***(R300)*** Open Gate A and check X109
                    if (StaticRes.Global.Process_Code.Returning == "R300" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R300 - Open Gate A and check X109");
                        if (IO_Control.X109_Gate_A_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Returning = "R400";
                            returncomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_A_not_opened, "R300");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R400)*** Check loading door closed sensor X102
                    if (StaticRes.Global.Process_Code.Returning == "R400" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R400 - Check loading door closed sensor X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Returning = "R500";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "R400");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R500)*** Check syringe A present sensor X104
                    if (StaticRes.Global.Process_Code.Returning == "R500" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R500 - Check syringe A present sensor X104");
                        if (Hardware.IO_LIST.Input.X104_Syringe_A_Present())
                            StaticRes.Global.Process_Code.Returning = "R600";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_A_not_present, "R500");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R600)*** Close gate A and check X108
                    if (StaticRes.Global.Process_Code.Returning == "R600" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R600 - Close gate A and check X108 sensor");
                        if (IO_Control.X108_Gate_A_Close_and_Check())
                            StaticRes.Global.Process_Code.Returning = "R1900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_A_not_closed, "R600");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 2
                    #region ***(R700)*** Open Gate B and check X111
                    if (StaticRes.Global.Process_Code.Returning == "R700" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R700 - Open Gate B and check X111");
                        if (IO_Control.X111_Gate_B_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Returning = "R800";
                            returncomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_B_not_opened, "R700");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R800)*** Check loading door closed sensor X102
                    if (StaticRes.Global.Process_Code.Returning == "R800" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R800 - Check loading door closed sensor X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Returning = "R900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "R800");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R900)*** Check syringe B present sensor X105
                    if (StaticRes.Global.Process_Code.Returning == "R900" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R900 - Check syringe B present sensor X105");
                        if (Hardware.IO_LIST.Input.X105_Syringe_B_Present())
                            StaticRes.Global.Process_Code.Returning = "R1000";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_B_not_present, "R900");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R1000)*** Close gate B and check X110
                    if (StaticRes.Global.Process_Code.Returning == "R1000" && StaticRes.Global.Transaction_Continue)
                    { 
                        step("R1000 - Close gate B and check X110");
                        if (IO_Control.X110_Gate_B_Close_and_Check())
                            StaticRes.Global.Process_Code.Returning = "R1900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_B_not_closed, "R1000");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 3
                    #region ***(R1100)*** Open Gate C and check X113
                    if (StaticRes.Global.Process_Code.Returning == "R1100" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1100 - Open Gate C and check X113");
                        if (IO_Control.X113_Gate_C_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Returning = "R1200";
                            returncomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_C_not_opened, "R1100");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R1200)*** Check loading door closed sensor X102
                    if (StaticRes.Global.Process_Code.Returning == "R1200" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1200 - Close loading window and check X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Returning = "R1300";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "R1200");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R1300)*** Check syringe C present sensor X106
                    if (StaticRes.Global.Process_Code.Returning == "R1300" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1300 - Check syringe C present sensor X106");
                        if (Hardware.IO_LIST.Input.X106_Syringe_C_Present())
                            StaticRes.Global.Process_Code.Returning = "R1400";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_C_not_present, "R1300");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R1400)*** Close gate C and check X112
                    if (StaticRes.Global.Process_Code.Returning == "R1400" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1400 - Close gate C and check X112");
                        if (IO_Control.X112_Gate_C_Close_and_Check())
                            StaticRes.Global.Process_Code.Returning = "R1900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_C_not_closed, "R1400");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 4
                    #region ***(R1500)*** Open Gate D and check X115
                    if (StaticRes.Global.Process_Code.Returning == "R1500" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1500 - Open Gate D and check X115");
                        if (IO_Control.X115_Gate_D_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Returning = "R1600";
                            returncomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_D_not_opened, "R1500");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R1600)*** Check loadind door closed sensor X102
                    if (StaticRes.Global.Process_Code.Returning == "R1600" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1600 - Close loading window and check X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Returning = "R1700";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "R1600");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R1700)*** Check syringe D present sensor X107
                    if (StaticRes.Global.Process_Code.Returning == "R1700" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1700 - Check syringe D present sensor X107");
                        if (Hardware.IO_LIST.Input.X107_Syringe_D_Present())
                            StaticRes.Global.Process_Code.Returning = "R1800";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_C_not_present, "R1700");
                            return;
                        }
                    }
                    #endregion

                    #region ***(R1800)*** Close gate D and check X114
                    if (StaticRes.Global.Process_Code.Returning == "R1800" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1800 - Close gate D and check X114");
                        if (IO_Control.X114_Gate_D_Close_and_Check())
                            StaticRes.Global.Process_Code.Returning = "R1900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_D_not_closed, "R1800");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region ***(R1900)*** Return completed , rotary continue moving
                    if (StaticRes.Global.Process_Code.Returning == "R1900" && StaticRes.Global.Transaction_Continue)
                    {
                        step("R1900 - return completed");
                        try
                        {
                            IO_Control.Yellow_Tower_Light_Setting();
                            StaticRes.Global.Process_Code.Returning = "R000";
                            StaticRes.Global.IsOnProgress = false;
                            returncomplete(true);
                            return;
                        }
                        catch(Exception ex)
                        {
                            Common.Reports.LogFile.Log("Return R1900, Exception:" + ex.ToString());
                            Error_Throw(StaticRes.Global.Error_List.Motion_failed, "R1900");
                            return;
                        }
                    }
                    #endregion
                   
                }
                else
                {
                    Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "R000");
                    return;
                }
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Return Error -- " + ee.Message + ",R000 , user:" + StaticRes.Global.Current_User.USER_ID);
                Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "From Return Exception");
                return;
            }
        }
        void Error_Throw(string Event_Name, string Process)
        {
            et.EVENT_NAME = Event_Name;
            et.PROCESS_CODE = Process;
            returnerror(et);
        }

        public void LoadingHandle()
        {
            try
            {
                HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                this.Thread_Return.Abort();
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Return error when stop -- " + ee.Message + " , user:" + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Need_Homing = true;
            }
        }
    }
}
