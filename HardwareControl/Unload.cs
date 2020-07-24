using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ObjectModule.Local;
using System.Data;
using StaticRes;
using System.Reflection;

namespace HardwareControl
{
    public class Unload
    {
        private ObjectModule.Local.Event et = new Event();

        #region delegate
        public delegate void UnloadCompleteEventHandler(bool complete);
        private UnloadCompleteEventHandler unloadcomplete;
        public event UnloadCompleteEventHandler UnloadComplete
        {
            add
            {
                unloadcomplete += value;
            }
            remove
            {
                unloadcomplete -= value;
            }
        }

        public delegate void unloaderrorEventHandler(ObjectModule.Local.Event ee);
        private unloaderrorEventHandler unloaderror;
        public event unloaderrorEventHandler UnloadError
        {
            add
            {
                unloaderror += value;
            }
            remove
            {
                unloaderror -= value;
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

        private Thread Thread_Unload = null;


        #region slot scanner
        private System.IO.Ports.SerialPort slotScanner = new System.IO.Ports.SerialPort();
        private string receivedSlotID = string.Empty;


        private void InitAndOpen(string comPort)
        {
            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In Func");

            if (slotScanner.IsOpen)
            {
                Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "com port is already opened.");
                return;
            }

            slotScanner.PortName = comPort;
            slotScanner.BaudRate = StaticRes.Global.System_Setting.SlotScanner_BaudRate;
            slotScanner.StopBits = System.IO.Ports.StopBits.One;
            slotScanner.DataBits = StaticRes.Global.System_Setting.SlotScanner_DataBits;
            slotScanner.DataReceived += SoltScanner_DataReceived;

            if (!slotScanner.IsOpen)
                slotScanner.Open();

            string msg = string.Format("init slot scanner,  portName:{0} baudRate:{1} stopBits:{2} dataBits:{3}", slotScanner.PortName, slotScanner.BaudRate, slotScanner.StopBits, slotScanner.DataBits);
            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), msg);
        }

        private void CloseSlotScanner()
        {
            if (slotScanner.IsOpen)
                slotScanner.Close();

            slotScanner.DataReceived -= SoltScanner_DataReceived;

            string msg = string.Format("close com port {0}", slotScanner.PortName);
            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), msg);
        }

        private void SoltScanner_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In Function");


            if (!slotScanner.IsOpen)
            {
                Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "Port " + slotScanner.PortName + " is closed ,  return;");
                return;
            }


            receivedSlotID = slotScanner.ReadExisting().Replace("\r", "");
            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "scanned barcode successfully, barcode id: " + receivedSlotID);
        }
        #endregion



        public Unload()
        {

        }
        

        public void Stop()
        {
            this.Thread_Unload.Abort();
        }

        void Unload_UnloadComplete(bool complete)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Unload_UnloadError(ObjectModule.Local.Event x)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Unload_Step(string Event)
        {
        }

        class Params
        {
            public int Slot_ID;
            public int Slot_Index;
        }

        public int Start_UnLoad(int Slot_ID,int Slot_Index)
        {
            if (StaticRes.Global.IsOnProgress)
            {
                unloaderror(et);
                return 0;
            }
            else
                StaticRes.Global.IsOnProgress = true;
            try
            {
                Params x = new Params();
                x.Slot_ID = Slot_ID;
                x.Slot_Index = Slot_Index;
                UnloadError+=new unloaderrorEventHandler(Unload_UnloadError);
                UnloadComplete+=new UnloadCompleteEventHandler(Unload_UnloadComplete);
                Step+=new StepEventHandler(Unload_Step);
                this.Thread_Unload = new Thread(new ParameterizedThreadStart(Unloading));
                this.Thread_Unload.Start(x);
            }
            catch
            {
                unloaderror(et);
            }
            return 10;
        }

        private void Unloading(object x)
        {
            try
            {
                Params y = (Params)x;
                LogicUnload(y.Slot_ID, y.Slot_Index);
            }
            catch
            {
                //StaticRes.Global.Process_Code.Loading = "L000";
            }
        }

        private void LogicUnload(int Slot_ID,int Slot_Index)
        {

            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In Function");

            try
            {

                if (!StaticRes.Global.Hardware_Connection)
                {
                    unloadcomplete(true);
                    return;
                }
                if (!StaticRes.Global.Need_Homing)
                {
                    #region ***(U000)*** Move to pointed position
                    if (StaticRes.Global.Process_Code.Unloading == "U000" && StaticRes.Global.Transaction_Continue)
                    {
                        Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In U000");
                        IO_Control.Green_Tower_Light_Setting();
                        step("U000 -  Rotary move to Slot-" + Slot_ID.ToString() + ",Position:"+StaticRes.Global.Slot_Position[Slot_ID-1].ToString()+"");
                        try
                        {
                            //Motion_Control.Motion_Speed_Checking();
                            //if (StaticRes.Global.Need_Homing)
                            //{
                            //    Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "U000");
                            //    return;
                            //}
                            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U000, Start move to slot");
                            Motion_Control.Rotary_MoveTo_Slot(Slot_ID);
                            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U000, move complete, current position: " + Motion_Control.Got_Rotary_Position().ToString());
                            Common.Reports.LogFile.Log("Unload move to slot successful ,current position:" + Motion_Control.Got_Rotary_Position().ToString() +"");
                            Motion_Control.Motion_Speed_Checking();
                           
                       
                             StaticRes.Global.Process_Code.Unloading = "U050";
                        }
                        catch(Exception ex)
                        {
                            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U000, Catch Exception:" + ex.ToString());               
                            Error_Throw(StaticRes.Global.Error_List.Motion_failed, "U000");
                            return;
                        }
                    }
                    #endregion


                    //2020 07 19 by Dwyane for Slot Barcode Validation 
                    #region ***(U050)*** Slot Barcode Validation
                    if (StaticRes.Global.Process_Code.Unloading == "U050" && StaticRes.Global.Transaction_Continue)
                    {
                        Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In U050");
                        step("U050 -  Scan Slot-" + Slot_ID.ToString() + " Barcode ,Position:" + StaticRes.Global.Slot_Position[Slot_ID - 1].ToString() + "");
                       
                        try
                        {
                            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U050, slot index:" + Slot_Index);
                            string comPort = string.Empty;
                            switch (Slot_Index)
                            {
                                case 1:
                                    comPort = StaticRes.Global.System_Setting.SlotScanner_Index1Port;
                                    break;
                                case 2:
                                    comPort = StaticRes.Global.System_Setting.SlotScanner_Index2Port;

                                    break;
                                case 3:
                                    comPort = StaticRes.Global.System_Setting.SlotScanner_Index3Port;
                                    break;
                                case 4:
                                    comPort = StaticRes.Global.System_Setting.SlotScanner_Index4Port;
                                    break;
                            }


                            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U050, start init & open com port:" );

                            
                            //初始化, 并打开com port
                            InitAndOpen(comPort);



                            //转盘来回摆动5次
                            for (int i = 0; i < 5; i++)
                            {
                                //一旦扫到barcode, 就立马跳出循环并关闭com口
                                if (receivedSlotID != "")
                                {
                                    Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U050, received scanner msg, break");
                                    CloseSlotScanner();
                                    break;
                                }

                                Motion_Control.Rotary_Move(10000);
                                Motion_Control.Motion_Speed_Checking();
                                System.Threading.Thread.Sleep(300);
                                Motion_Control.Rotary_Move(-10000);
                                Motion_Control.Motion_Speed_Checking();
                                System.Threading.Thread.Sleep(300);
                                Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U050, move come and back, time" + i.ToString());
                            }




                            if (receivedSlotID == "")
                            {
                                //来回5次都没扫到报警.
                                Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U050, 5 times moving come and back still can not read slot barcode, start alarming");
                                Error_Throw(StaticRes.Global.Error_List.SlotBarcode_Read_Fail, "U000");
                                return;
                            }
                            else
                            {
                                if (receivedSlotID == Slot_ID.ToString())
                                {
                                    //进入下一步
                                    StaticRes.Global.Process_Code.Unloading = "U100";
                                    receivedSlotID = "";
                                }
                                else
                                {
                                    //扫到的位置不是目标位置则报错.
                                    Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U050, received wrong place received slot id:" + "receivedSlotID" + ", tartget slot id:" + Slot_ID);
                                    Error_Throw(StaticRes.Global.Error_List.MissingPlace, "U000");
                                    return;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U050, Catch Exception:" + ex.ToString());
                            Error_Throw(StaticRes.Global.Error_List.Motion_failed, "U000");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U100)*** Check slot index and move to related process
                    if (StaticRes.Global.Process_Code.Unloading == "U100" && StaticRes.Global.Transaction_Continue)
                    {
                        switch (Slot_Index)
                        {
                            case 1:
                                StaticRes.Global.Process_Code.Unloading = "U200";
                                break;
                            case 2:
                                StaticRes.Global.Process_Code.Unloading = "U900";
                                break;
                            case 3:
                                StaticRes.Global.Process_Code.Unloading = "U1600";
                                break;
                            case 4:
                                StaticRes.Global.Process_Code.Unloading = "U2300";
                                break;
                        }
                    }
                    #endregion

                    #region Index 1
                    #region ***(U200)*** Open Gate A and check X109
                    if (StaticRes.Global.Process_Code.Unloading == "U200" && StaticRes.Global.Transaction_Continue)
                    {
                        Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In U200");

                        step("U200 - Open Gate A and check X109");
                        if (IO_Control.X109_Gate_A_Open_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U300";
                        else
                        {
                            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U200, Gate A not open");
                            Error_Throw(StaticRes.Global.Error_List.Gate_A_not_opened, "U200");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U300)*** Ejector A Up and check X208
                    if (StaticRes.Global.Process_Code.Unloading == "U300" && StaticRes.Global.Transaction_Continue)
                    {
                        Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In U300");
                        step("U030 - Ejector A Up and check X208");
                        if (IO_Control.X208_Ejector_A_Up_and_Check())
                        {
                            StaticRes.Global.Process_Code.Unloading = "U600";
                            unloadcomplete(false);
                            return;
                        }
                        else
                        {
                            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "U300, Ejector A not up");
                            Error_Throw(StaticRes.Global.Error_List.Ejector_A_not_up, "U300");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U600)*** Ejector A down and check X209
                    if (StaticRes.Global.Process_Code.Unloading == "U600" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U600 - Ejector A down and check X209");
                        if (IO_Control.X209_Ejector_A_Down_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U700";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Ejector_A_not_down, "U600");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U700)*** Check syringe a present sensor X104
                    if (StaticRes.Global.Process_Code.Unloading == "U700" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U700 - Check syringe A present sensor X104");
                        if (!Hardware.IO_LIST.Input.X104_Syringe_A_Present())
                            StaticRes.Global.Process_Code.Unloading = "U800";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_A_present, "U700");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U800)*** Close gate A and check X108
                    if (StaticRes.Global.Process_Code.Unloading == "U800" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U800 - Close Gate A and check X108");
                        if (IO_Control.X108_Gate_A_Close_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U3000";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_A_not_closed, "U800");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 2
                    #region ***(U900)*** Open Gate B and check X111
                    if (StaticRes.Global.Process_Code.Unloading == "U900" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U900 - Open Gate B and check X111");
                        if (IO_Control.X111_Gate_B_Open_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U1000";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_B_not_opened, "U900");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U1000)*** Ejector B Up and check X210
                    if (StaticRes.Global.Process_Code.Unloading == "U1000" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U1000 - Ejector B Up and check X210");
                        if (IO_Control.X210_Ejector_B_Up_and_Check())
                        {
                            StaticRes.Global.Process_Code.Unloading = "U1300";
                            unloadcomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Ejector_B_not_up, "U1000");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U1300)*** Ejector B down and check X211
                    if (StaticRes.Global.Process_Code.Unloading == "U1300" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U1300 - Ejector B down and check X211");
                        if (IO_Control.X211_Ejector_B_Down_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U1400";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Ejector_B_not_down, "U1300");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U1400)*** Check syringe B present sensor 
                    if (StaticRes.Global.Process_Code.Unloading == "U1400" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U1400 - Check syringe B present sensor X105");
                        if (!Hardware.IO_LIST.Input.X105_Syringe_B_Present())
                            StaticRes.Global.Process_Code.Unloading = "U1500";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_B_present, "U1400");

                            return;
                        }
                    }
                    #endregion

                    #region ***(U1500)*** Close gate B and check X110
                    if (StaticRes.Global.Process_Code.Unloading == "U1500" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U1500 - Close Gate B and check X110");
                        if (IO_Control.X110_Gate_B_Close_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U3000";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_B_not_closed, "U1500");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 3
                    #region ***(U1600)*** Open Gate C and check X113
                    if (StaticRes.Global.Process_Code.Unloading == "U1600" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U1600 - Open Gate C and check X113");
                        if (IO_Control.X113_Gate_C_Open_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U1700";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_C_not_opened, "U1600");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U1700)*** Ejector C Up and check X212
                    if (StaticRes.Global.Process_Code.Unloading == "U1700" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U1700 - Ejector C Up and check X212");
                        if (IO_Control.X212_Ejector_C_Up_and_Check())
                        {
                            StaticRes.Global.Process_Code.Unloading = "U2000";
                            unloadcomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Ejector_C_not_up, "U1700");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U2000)*** Ejector C down and check X213
                    if (StaticRes.Global.Process_Code.Unloading == "U2000" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U2000 - Ejector C down and check X213");
                        if (IO_Control.X213_Ejector_C_Down_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U2100";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Ejector_C_not_down, "U2000");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U2100)*** Check syringe C present sensor
                    if (StaticRes.Global.Process_Code.Unloading == "U2100" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U2100 - Check syringe C present sensor X106");
                        if (!Hardware.IO_LIST.Input.X106_Syringe_C_Present())
                            StaticRes.Global.Process_Code.Unloading = "U2200";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_C_present, "U2100");

                            return;
                        }
                    }
                    #endregion

                    #region ***(U2200)*** Close gate C and check X112
                    if (StaticRes.Global.Process_Code.Unloading == "U2200" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U2200 - Close Gate C and check X112");
                        if (IO_Control.X112_Gate_C_Close_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U3000";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_C_not_closed, "U2200");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region Index 4
                    #region ***(U2300)*** Open Gate D and check X115
                    if (StaticRes.Global.Process_Code.Unloading == "U2300" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U2300 - Open Gate D and check X115");
                        if (IO_Control.X115_Gate_D_Open_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U2400";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_D_not_opened, "U2300");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U2400)*** Ejector D Up and check X214
                    if (StaticRes.Global.Process_Code.Unloading == "U2400" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U2400 - Ejector D Up and check X214");
                        if (IO_Control.X214_Ejector_D_Up_and_Check())
                        {
                            StaticRes.Global.Process_Code.Unloading = "U2700";
                            unloadcomplete(false);
                            return;
                        }
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Ejector_D_not_up, "U2400");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U2700)*** Ejector D down and check X215
                    if (StaticRes.Global.Process_Code.Unloading == "U2700" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U2700 - Ejector D down and check X215");
                        if (IO_Control.X215_Ejector_D_Down_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U2800";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Ejector_D_not_down, "U2700");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U2800)*** Check syringe D present sensor
                    if (StaticRes.Global.Process_Code.Unloading == "U2800" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U2800 - Check syringe D present sensor X107");
                        if (!Hardware.IO_LIST.Input.X107_Syringe_D_Present())
                            StaticRes.Global.Process_Code.Unloading = "U2900";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Syringe_D_present, "U2800");

                            return;
                        }
                    }
                    #endregion

                    #region ***(U2900)*** Close gate D and check X114
                    if (StaticRes.Global.Process_Code.Unloading == "U2900" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U2900 - Close gate D and check X114");
                        if (IO_Control.X114_Gate_D_Close_and_Check())
                            StaticRes.Global.Process_Code.Unloading = "U3000";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Gate_D_not_closed, "U2900");
                            return;
                        }
                    }
                    #endregion
                    #endregion

                    #region ***(U3000)*** Check loading door close sensor X102
                    if (StaticRes.Global.Process_Code.Unloading == "U3000" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U3000 - Check loading door close sensor X102");
                        if (Hardware.IO_LIST.Input.X102_Loading_Door_Closed())
                            StaticRes.Global.Process_Code.Unloading = "U3100";
                        else
                        {
                            Error_Throw(StaticRes.Global.Error_List.Loading_door_not_closed, "U3000");
                            return;
                        }
                    }
                    #endregion

                    #region ***(U3100)*** Unloading complete
                    if (StaticRes.Global.Process_Code.Unloading == "U3100" && StaticRes.Global.Transaction_Continue)
                    {
                        step("U3100 - unload complete");
                        IO_Control.Yellow_Tower_Light_Setting();
                        StaticRes.Global.Process_Code.Unloading = "U000";
                        StaticRes.Global.IsOnProgress = false;
                        unloadcomplete(true);
                        return;
                    }
                    #endregion
                }
                else
                {
                    Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "U000");
                    return;
                }
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Unload Error -- " + ee.Message + ",U000 , user:" + StaticRes.Global.Current_User.USER_ID);
                Error_Throw(StaticRes.Global.Error_List.Please_homing_first, "From Unload Exception");
                return;
            }
        }
        void Error_Throw(string Event_Name, string Process)
        {
            et.EVENT_NAME = Event_Name;
            et.PROCESS_CODE = Process;
            unloaderror(et);
        }

       
    }
}
