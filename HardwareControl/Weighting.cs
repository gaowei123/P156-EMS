using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObjectModule.Local;
using System.Threading;

namespace HardwareControl
{
    public class Weighting
    {
        private float Weight_Return = 0;

        #region Weight Scale Setting
        private System.IO.Ports.SerialPort Weight_Scale = new System.IO.Ports.SerialPort();
        private void Open_WeightScale_ComPort()
        {
            try
            {
                Weight_Scale.BaudRate = StaticRes.Global.System_Setting.Weighing_Scale_BaudRate;
                Weight_Scale.StopBits = System.IO.Ports.StopBits.One;
                Weight_Scale.DataBits = StaticRes.Global.System_Setting.Weighing_Scale_DataBits;
                Weight_Scale.PortName = StaticRes.Global.System_Setting.Weighing_Scale_COM_Port;
                Weight_Scale.ReceivedBytesThreshold = StaticRes.Global.System_Setting.Weighing_Scale_ReceivedBytesThreshold;
                if (!Weight_Scale.IsOpen)
                {
                    Weight_Scale.Open();
                    Weight_Scale.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Weight_Scale_DataReceived);
                }
            }
            catch
            {
                return;
            }
        }
        private void Close_WeightScale_ComPort()
        {
            try
            {
                Weight_Scale.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(Weight_Scale_DataReceived);
                if (Weight_Scale.IsOpen)
                    Weight_Scale.Close();
            }
            catch
            {
            }
        }
        void Weight_Scale_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                Weight_Return = float.Parse(Weight_Scale.ReadLine().Replace("g\r", "").Trim());
            }
            catch
            {
            }
        }
        #endregion  

        private Event et = new Event();
        public delegate void WeightCompleteEventHandler(float Weight_Return);
        private WeightCompleteEventHandler weightcomplete;
        public event WeightCompleteEventHandler WeightComplete
        {
            add
            {
                weightcomplete += value;
            }
            remove
            {
                weightcomplete -= value;
            }
        }

        public delegate void WeightContinueEventHandler(float Weight_Return);
        private WeightContinueEventHandler weightcontinue;
        public event WeightContinueEventHandler WeightContinue
        {
            add
            {
                weightcontinue += value;
            }
            remove
            {
                weightcontinue -= value;
            }
        }

        public delegate void weighterrorEventHandler(Event e);
        private weighterrorEventHandler weighterror;
        public event weighterrorEventHandler WeightError
        {
            add
            {
                weighterror += value;
            }
            remove
            {
                weighterror -= value;
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

        private Thread Thread_Weight = null;

        void Weight_WeightComplete(float Weight_Return)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Weight_WeightContinue(float Weight_Return)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Weight_WeightError(Event x)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Weight_Step(string Event)
        {
        }

        class Params
        {
            public int Capacity;
            public bool Load_Process;
            public string Scrap_Weight;
            public string Expiry_Time;
            public bool Force_Scrap;
            public string Syringe_Weight;
        }

        public void  Start_Weight(int Capacity, bool Load_Process, string Scrap_Weight, string Expiry_Time,  bool Force_Scrap,string Syringe_Weight)
        {
            if (StaticRes.Global.IsOnProgress)
                weighterror(et);
            else
                StaticRes.Global.IsOnProgress = true;
            try
            {
                Params x = new Params();
                x.Capacity = Capacity;
                x.Load_Process = Load_Process;
                x.Scrap_Weight = Scrap_Weight;
                x.Expiry_Time = Expiry_Time;
                x.Force_Scrap = Force_Scrap;
                x.Syringe_Weight = Syringe_Weight;
                weighterror += new weighterrorEventHandler(Weight_WeightError);
                weightcomplete += new WeightCompleteEventHandler(Weight_WeightComplete);
                weightcontinue += new WeightContinueEventHandler(Weight_WeightContinue);
                Step += new StepEventHandler(Weight_Step);
                this.Thread_Weight = new Thread(new ParameterizedThreadStart(weighting));
                this.Thread_Weight.Start(x);
            }
            catch
            {
                weighterror(et);
            }
        }

        private void weighting(object x)
        {
            try
            {
                Params y = (Params)x;
                LogicWeight(y.Capacity,y.Load_Process,y.Scrap_Weight,y.Expiry_Time,y.Force_Scrap,y.Syringe_Weight);
            }
            catch
            {
                StaticRes.Global.Process_Code.Weighting = "W000";
            }
        }

        private void LogicWeight(int Capacity, bool Load_Process, string Scrap_Weight, string Expiry_Time, bool Force_Scrap, string Syringe_Weight)
        {
            try
            {
                if (!StaticRes.Global.Need_Homing)
                {
                    #region ***(W000)*** Weighing tray cylinder forward and check X204
                    if (StaticRes.Global.Process_Code.Weighting == "W000")
                    {
                        step("W000 - Weighing tray cylinder forward and check X204");
                        if (IO_Control.X204_Weighing_Tray_Cylinder_Forward_and_Check())
                            StaticRes.Global.Process_Code.Weighting = "W100";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_tray_cylinder_forward_failed, "W000");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W100)*** Open weighting door and check X300 sensor to ask user put in material
                    if (StaticRes.Global.Process_Code.Weighting == "W100")
                    {
                        step("W100 - Open weighing door and check X300");
                        if (IO_Control.X300_Weighing_Door_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Weighting = "W200";
                            weightcontinue(Weight_Return);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W100");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W200)*** Close weight door and check X301 sensor
                    if (StaticRes.Global.Process_Code.Weighting == "W200")
                    {
                        step("W200 - Close weighing door and check X301");
                        if (IO_Control.X301_Weighing_Door_Close_and_Check())
                            StaticRes.Global.Process_Code.Weighting = "W300";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_closed, "W200");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W300)*** Check cap present sensor base on cap type
                    if (StaticRes.Global.Process_Code.Weighting == "W300")
                    {
                        switch (Capacity)
                        {
                            case 5:
                                {
                                    step("W300 - Check 5CC cap present sensor X305");
                                    if (StaticRes.Global.System_Setting.Bypass_Syringe_5cc_Cap_Present_Sensor == "Y")
                                        StaticRes.Global.Process_Code.Weighting = "W400";
                                    else
                                    {
                                        if (Hardware.IO_LIST.Input.X305_5cc_Syringe_Cap_Present())
                                            StaticRes.Global.Process_Code.Weighting = "W400";
                                        else
                                        {
                                            if (IO_Control.X300_Weighing_Door_Open_and_Check())
                                            {
                                                StaticRes.Global.Process_Code.Weighting = "W200";
                                                Error_Throw(StaticRes.Global.Error_List.Syringe_5cc_cap_not_present, "W300");
                                                return;
                                            }
                                            else
                                            {
                                                Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W300");
                                                return;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 10:
                                {
                                    step("W300 - Check 10CC cap present sensor X304");
                                    if (StaticRes.Global.System_Setting.Bypass_Syringe_10cc_Cap_Present_Sensor == "Y")
                                        StaticRes.Global.Process_Code.Weighting = "W400";
                                    else
                                    {
                                        if (Hardware.IO_LIST.Input.X304_10cc_Syringe_Cap_Present())
                                            StaticRes.Global.Process_Code.Weighting = "W400";
                                        else
                                        {
                                            if (IO_Control.X300_Weighing_Door_Open_and_Check())
                                            {
                                                StaticRes.Global.Process_Code.Weighting = "W200";
                                                Error_Throw(StaticRes.Global.Error_List.Syringe_10cc_cap_not_present, "W300");
                                                return;
                                            }
                                            else
                                            {
                                                Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W300");
                                                return;
                                            }
                                        }
                                    }
                                }
                                break;
                            case 30:
                                {
                                    step("W300 - Check 30CC cap present sensor X303");
                                    if (StaticRes.Global.System_Setting.Bypass_Syringe_30cc_Cap_Present_Sensor == "Y")
                                        StaticRes.Global.Process_Code.Weighting = "W400";
                                    else
                                    {
                                        if (Hardware.IO_LIST.Input.X303_30cc_Syringe_Cap_Present())
                                            StaticRes.Global.Process_Code.Weighting = "W400";
                                        else
                                        {
                                            if (IO_Control.X300_Weighing_Door_Open_and_Check())
                                            {
                                                StaticRes.Global.Process_Code.Weighting = "W200";
                                                Error_Throw(StaticRes.Global.Error_List.Syringe_30cc_cap_not_present, "W300");
                                                return;
                                            }
                                            else
                                            {
                                                Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W300");
                                                return;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    #endregion

                    #region ***(W400)*** Check syringe top cover present sensor X302
                    if (StaticRes.Global.Process_Code.Weighting == "W400")
                    {
                        step("W400 - Check syringe top cover present sensor X302");
                        if (StaticRes.Global.System_Setting.Bypass_Syringe_Top_Cover_Sensor == "Y")
                        {
                            StaticRes.Global.Process_Code.Weighting = "W500";
                        }
                        else
                        {
                            if (Hardware.IO_LIST.Input.X302_Syringe_Top_Cover_Present())
                            {
                                StaticRes.Global.Process_Code.Weighting = "W500";
                            }
                            else
                            {
                                if (IO_Control.X300_Weighing_Door_Open_and_Check())
                                {
                                    StaticRes.Global.Process_Code.Weighting = "W200";
                                    Error_Throw(StaticRes.Global.Error_List.Syringe_top_cover_not_present, "W400");
                                    return;
                                }
                                else
                                {
                                    Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W400");
                                    return;
                                }
                            }
                        }
                    }
                    #endregion

                    #region ***(W500)*** Check syringe present sensor X306
                    if (StaticRes.Global.Process_Code.Weighting == "W500")
                    {
                        step("W500 - Check syringe present sensor X306");
                        if (Hardware.IO_LIST.Input.X306_Syringe_Present())
                            StaticRes.Global.Process_Code.Weighting = "W600";
                        else
                        {
                            if (IO_Control.X300_Weighing_Door_Open_and_Check())
                            {
                                StaticRes.Global.Process_Code.Weighting = "W200";
                                Error_Throw(StaticRes.Global.Error_List.Syringe_not_present, "W500");
                                return;
                            }
                            else
                            {
                                Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W500");
                                return;
                            }
                        }
                    }
                    #endregion

                    #region ***(W600)*** Weighting And Read Data
                    if (StaticRes.Global.Process_Code.Weighting == "W600")
                    {
                        try
                        {
                            Open_WeightScale_ComPort();                            
                            step("W600 - Weighting....Result:" + Weight_Return.ToString() + "");
                            DateTime Start_Time = System.DateTime.Now;
                            System.Threading.Thread.Sleep(1000);             
                            while (Weight_Return < float.Parse(Syringe_Weight))
                            {
                                TimeSpan ts = System.DateTime.Now - Start_Time;
                                if (ts.TotalMilliseconds > 10000)
                                    break ;
                            }                            
                            Close_WeightScale_ComPort();
                            if (Weight_Return >= float.Parse(Syringe_Weight))
                            {
                                StaticRes.Global.Process_Code.Weighting = "W700";
                            }
                            else
                            {
                                Common.Reports.LogFile.Log("Weighting Error [W600] -- " + Weight_Return.ToString() + "<" + Syringe_Weight.ToString());
                                if (IO_Control.X300_Weighing_Door_Open_and_Check())
                                {
                                    StaticRes.Global.Process_Code.Weighting = "W200";
                                    Error_Throw(StaticRes.Global.Error_List.Weighing_Failed, "W600");
                                    return;
                                }
                                else
                                {
                                    Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W600");
                                    return;
                                }
                            }
                        }
                        catch
                        {
                            Common.Reports.LogFile.Log("Weighting Error [W600] -- " + Weight_Return.ToString() + "<" + Syringe_Weight.ToString());
                            if (IO_Control.X300_Weighing_Door_Open_and_Check())
                            {
                                StaticRes.Global.Process_Code.Weighting = "W200";
                                Error_Throw(StaticRes.Global.Error_List.Weighing_Failed, "W600");
                                return;
                            }
                            else
                            {
                                Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W600");
                                return;
                            }
                        }
                    }
                    #endregion

                    #region ***(W700)*** Check either is load or return , if load-go W1000,if return- go W800
                    if (StaticRes.Global.Process_Code.Weighting == "W700")
                    {
                        if (Load_Process)
                            StaticRes.Global.Process_Code.Weighting = "W1000";
                        else
                            StaticRes.Global.Process_Code.Weighting = "W800";
                    }
                    #endregion

                    #region ***(W800)*** Check either is scrap or reuse , if reuse-go W1000, if scrap-go W900
                    if (StaticRes.Global.Process_Code.Weighting == "W800")
                    {
                        try
                        {
                            string Status = string.Empty;
                            if (Force_Scrap)
                                Status = StaticRes.Global.Status.Scrap;
                            else
                            {
                                if (DateTime.Parse(Expiry_Time) <= System.DateTime.Now)
                                    Status = StaticRes.Global.Status.Scrap;
                                else if (Weight_Return <= float.Parse(Scrap_Weight))
                                    Status = StaticRes.Global.Status.Scrap;
                                else if (DateTime.Parse(Expiry_Time) <= System.DateTime.Now.AddHours(StaticRes.Global.System_Setting.Time_1))//yakun.zhoou 2015/06/30
                                    Status = StaticRes.Global.Status.Scrap;
                                else
                                    Status = StaticRes.Global.Status.Reuse;
                            }
                            if (Status == StaticRes.Global.Status.Reuse)
                                StaticRes.Global.Process_Code.Weighting = "W1000";
                            else
                                StaticRes.Global.Process_Code.Weighting = "W900";
                            step("W800 - Syringe go " + Status + "");
                        }
                        catch
                        {
                            if (IO_Control.X300_Weighing_Door_Open_and_Check())
                            {
                                StaticRes.Global.Process_Code.Weighting = "W200";
                                Error_Throw(StaticRes.Global.Error_List.Weighing_Failed, "W800");
                                return;
                            }
                            else
                            {
                                Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W800");
                                return;
                            }
                        }
                    }
                    #endregion

                    #region ***(W900)*** if force scrap , scrap chute open
                    if (StaticRes.Global.Process_Code.Weighting == "W900")
                    {
                        if (Force_Scrap)
                        {
                            step("W900 - Force scrap ,open scrap chute to scrap drawer 2 and check X203 sensor");
                            if (IO_Control.X203_Scrap_Chute_To_Drawer_2_and_Check())
                                StaticRes.Global.Process_Code.Weighting = "W910";
                            else
                            {
                                Error_Throw(StaticRes.Global.Error_List.Scrap_shute_cyiinder_to_drawer_2_failed, "W900");
                                return;
                            }
                        }
                        else
                        {
                            step("W900 - Normal scrap , open scrap chute to scrap drawer 1 and check X202 sensor");
                            if (IO_Control.X202_Scrap_Chute_To_Drawer_1_and_Check())
                                StaticRes.Global.Process_Code.Weighting = "W910";
                            else
                            {
                                Error_Throw(StaticRes.Global.Error_List.Scrap_chute_cylinder_to_drawer_1_failed, "W900");
                                return;
                            }
                        }

                    }
                    #endregion

                    #region ***(W910)*** Weighting tray cylinder backward and check X205
                    if (StaticRes.Global.Process_Code.Weighting == "W910")
                    {
                        step("W910 - Weighting tray cylinder backward and check X205");
                        if (IO_Control.X205_Weighing_Tray_Cylinder_Backward_and_Check())
                            StaticRes.Global.Process_Code.Weighting = "W920";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_tray_cylinder_backward_failed, "W910");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W920)*** Null
                    if (StaticRes.Global.Process_Code.Weighting == "W920")
                    {
                         StaticRes.Global.Process_Code.Weighting = "W930";
                    }
                    #endregion

                    #region ***(W930)*** Weighting tray cylinder backward and check X205
                    if (StaticRes.Global.Process_Code.Weighting == "W930")
                    {
                        step("W930 - Weighting tray cylinder forward and check X204");
                        if (IO_Control.X204_Weighing_Tray_Cylinder_Forward_and_Check())
                        {
                            Weight_Return = 100 + Weight_Return; //for indicate whether need update server after weighing
                            StaticRes.Global.Process_Code.Weighting = "W1600";
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_tray_cylinder_forward_failed, "W930");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W1000)*** Open Weight Scale door and check X300 sensor to ask user take out material
                    if (StaticRes.Global.Process_Code.Weighting == "W1000")
                    {
                        step("W1000 - Open weighting door and check X300");
                        if (IO_Control.X300_Weighing_Door_Open_and_Check())
                        {
                            StaticRes.Global.Process_Code.Weighting = "W1100";
                            weightcontinue(Weight_Return);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_opened, "W1000");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W1100)*** Check syringe top cover present sensor X302
                    if (StaticRes.Global.Process_Code.Weighting == "W1100")
                    {
                        step("W1100 - Check syringe top cover present sensor X302");
                        if (!Hardware.IO_LIST.Input.X302_Syringe_Top_Cover_Present())
                            StaticRes.Global.Process_Code.Weighting = "W1200";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_top_cover_present, "W1100");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W1200)*** Check 30cc cap present sensor X303
                    if (StaticRes.Global.Process_Code.Weighting == "W1200")
                    {
                        step("W1200 - Check 30cc cap present sensor X303");
                        if (!Hardware.IO_LIST.Input.X303_30cc_Syringe_Cap_Present())
                            StaticRes.Global.Process_Code.Weighting = "W1300";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_30cc_cap_present, "W1200");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W1300)*** Check 10cc cap present sensor X304
                    if (StaticRes.Global.Process_Code.Weighting == "W1300")
                    {
                        step("W1300 - Check 10cc syringe cap present sensor X304");
                        if (!Hardware.IO_LIST.Input.X304_10cc_Syringe_Cap_Present())
                            StaticRes.Global.Process_Code.Weighting = "W1400";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_10cc_cap_present, "W1300");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W1400)*** Check 5cc cap present sensor X305
                    if (StaticRes.Global.Process_Code.Weighting == "W1400")
                    {
                        step("W1400 - Check 5cc cap present sensor X305");
                        if (!Hardware.IO_LIST.Input.X305_5cc_Syringe_Cap_Present())
                            StaticRes.Global.Process_Code.Weighting = "W1500";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_5cc_cap_present, "W1400");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W1500)*** Check syringe present sensor X306
                    if (StaticRes.Global.Process_Code.Weighting == "W1500")
                    {
                        step("W1500 - Check syringe present sensor X306");
                        if (!Hardware.IO_LIST.Input.X306_Syringe_Present())
                            StaticRes.Global.Process_Code.Weighting = "W1600";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_present, "W1500");
                            return;
                        }
                    }
                    #endregion

                    #region ***(W1600)*** Close weight scale door and check X301 sensor
                    if (StaticRes.Global.Process_Code.Weighting == "W1600")
                    {
                        step("W1600 - Close weighing door and check X301 sensor");
                        if (IO_Control.X301_Weighing_Door_Close_and_Check())
                        {
                            StaticRes.Global.Process_Code.Weighting = "W000";
                            StaticRes.Global.IsOnProgress = false;
                            weightcomplete(Weight_Return);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Weighing_door_not_closed, "W1600");
                            return;
                        }
                    }
                    #endregion
                }
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Weighting Error -- " + ee.Message + ",W000 , user:" + StaticRes.Global.Current_User.USER_ID);
                //Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "From Weighting Exception");
                return;
            }
        }

        void Error_Throw(string Event_Name, string Process)
        {
            et.EVENT_NAME = Event_Name;
            et.PROCESS_CODE = Process;
            weighterror(et);
        }
    }
}
