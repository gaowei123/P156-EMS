using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ObjectModule.Local;
using StaticRes;
using System.Data;

namespace HardwareControl
{
    public class Inventory_Check
    {
        private List<Temp_Inventory_Check> list = new List<Temp_Inventory_Check>();
        private ObjectModule.Local.Event et = new Event();
        #region delegate
        public delegate void InventoryCheckCompleteEventHandler();
        private InventoryCheckCompleteEventHandler inventorycheckcomplete;
        public event InventoryCheckCompleteEventHandler InventoryCheckComplete
        {
            add
            {
                inventorycheckcomplete += value;
            }
            remove
            {
                inventorycheckcomplete -= value;
            }
        }

        public delegate void inventorycheckerrorEventHandler(ObjectModule.Local.Event ee);
        private inventorycheckerrorEventHandler inventorycheckerror;
        public event inventorycheckerrorEventHandler InventoryCheckError
        {
            add
            {
                inventorycheckerror += value;
            }
            remove
            {
                inventorycheckerror -= value;
            }
        }

        public delegate void StepEventHandler(string Event,List<Temp_Inventory_Check> list);
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

        private Thread Thread_Inventory_Check = null;

        public void Stop()
        {
            this.Thread_Inventory_Check.Abort();
        }

        void Inventory_Check_InventoryCheckComplete()
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Inventory_Check_InventoryCheckError(ObjectModule.Local.Event x)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Inventory_Check_Step(string Event,List<Temp_Inventory_Check> list)
        {
        }

        public int Start_InventoryCheck()
        {
            if (StaticRes.Global.IsOnProgress)
            {
                inventorycheckcomplete();
            }
            else
                StaticRes.Global.IsOnProgress = true;
            try
            {
                InventoryCheckComplete+=new InventoryCheckCompleteEventHandler(Inventory_Check_InventoryCheckComplete);
                Step+=new StepEventHandler(Inventory_Check_Step);
                InventoryCheckError+=new inventorycheckerrorEventHandler(Inventory_Check_InventoryCheckError);
                this.Thread_Inventory_Check = new Thread(new ParameterizedThreadStart(Inventory_Checking));
                this.Thread_Inventory_Check.Start();
            }
            catch
            {
                inventorycheckcomplete();
            }
            return 30;
        }

        private void Inventory_Checking(object x)
        {
            try
            {
                LogicInventoryChecking();
            }
            catch
            {
                inventorycheckcomplete();
            }
        }

        private void LogicInventoryChecking()
        {
            //try
            //{
            //    if (!StaticRes.Global.Hardware_Connection)
            //    {
            //        inventorycheckcomplete();
            //        return;
            //    }
            //    if (!StaticRes.Global.Need_Homing)
            //    {
            //        #region ***(I000)*** Turn on green tower light
            //        if (StaticRes.Global.Process_Code.Inventory_Check == "I000" && StaticRes.Global.Transaction_Continue)
            //        {
            //            step("Turn on green light", list);
            //            HardwareControl.IO_Control.Green_Tower_Light_Setting();
            //            StaticRes.Global.Process_Code.Inventory_Check = "I100";
            //        }
            //        #endregion

            //        #region ***(I050)*** Check left side door close sensor X106
            //        if (StaticRes.Global.Process_Code.Inventory_Check == "I050" && StaticRes.Global.Transaction_Continue)
            //        {
            //            step("Check storage left door closed sensor X106", list);
            //            if (HardwareControl.IO_Control.X106_Storage_Left_Door_Closed_Check())
            //                StaticRes.Global.Process_Code.Inventory_Check = "I100";
            //            else
            //            {
            //                Error_Throw(StaticRes.Global.Error_List.Left_side_storage_door_not_close, "I050");
            //                return;
            //            }
            //        }
            //        #endregion

            //        #region ***(I100)*** Check right side door close sensor 
            //        if (StaticRes.Global.Process_Code.Inventory_Check == "I100" && StaticRes.Global.Transaction_Continue)
            //        {
            //            step("Check storage right door closed sensor X107", list);
            //            if (HardwareControl.IO_Control.X107_Storage_Right_Door_Closed_Check())
            //                StaticRes.Global.Process_Code.Inventory_Check = "I200";
            //            else
            //            {
            //                Error_Throw(StaticRes.Global.Error_List.Right_side_storage_door_not_close, "I100");
            //                return;
            //            }
            //        }
            //        #endregion       
                    
            //        #region ***(I200)*** Move to Slot 1 and check
            //        if (StaticRes.Global.Process_Code.Inventory_Check == "I200" && StaticRes.Global.Transaction_Continue)
            //        {
            //            try
            //            {
            //                for (int i = 1; i <= 26; i++)
            //                {
            //                    step(" start check Slot-" + i.ToString() + "", list);
            //                    if (Motion_Control.Inventory_Rotary_Move(i, 6))
            //                    {
            //                        Motion_Control.Motion_Speed_Checking();
            //                        Inventory_Check_Logic(i);
            //                        StaticRes.Global.Process_Code.Inventory_Check = "I300";
            //                    }
            //                    else
            //                    {
            //                        Error_Throw(StaticRes.Global.Error_List.Motion_failed, "I200");
            //                        return;
            //                    }
            //                }
            //            }
            //            catch (Exception ee)
            //            {
            //                Common.Reports.LogFile.Log("Inventory checking error -- " + ee.Message + ",I200 , user:" + StaticRes.Global.Current_User.USER_ID);
            //                Error_Throw(StaticRes.Global.Error_List.Motion_failed, "I200");
            //                return;
            //            }
            //        }
            //        #endregion

            //        #region ***(I1800)*** Inventory Check Complete
            //        if (StaticRes.Global.Process_Code.Inventory_Check == "I300" && StaticRes.Global.Transaction_Continue)
            //        {
            //            step("Turn off green light and trun on yellow light", list);
            //            StaticRes.Global.Process_Code.Inventory_Check = "I000";
            //            HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
            //            StaticRes.Global.IsOnProgress = false;
            //            inventorycheckcomplete();
            //            return;
            //        }
            //        #endregion
            //    }
            //    else
            //    {
            //        Error_Throw(StaticRes.Global.Error_List.Rotary_homing_fail, "I000");
            //        return;
            //    }
            //}
            //catch(Exception ee)
            //{
            //    Common.Reports.LogFile.Log("Inventory Check Error -- " + ee.Message + ",I000 , user:" + StaticRes.Global.Current_User.USER_ID);
            //    Error_Throw(StaticRes.Global.Error_List.Need_homing_first, "From Inventory Check Exception");
            //    return;
            //}
        }
        void Error_Throw(string Event_Name, string Process)
        {
            et.EVENT_NAME = Event_Name;
            et.PROCESS_CODE = Process;
            inventorycheckerror(et);
        }

        void Inventory_Check_Logic(int Slot_No)
        {
            //DataTable dt = DataProvider.Local.Binning.Select.Search_Inventory_SlotNo(Slot_No);
            //foreach (DataRow x in dt.Rows)
            //{
            //    bool Sensor_Status = false;
            //    switch (x["SLOT_LEVEL"].ToString())
            //    {
            //        case "1":
            //            Sensor_Status = Hardware.IO_LIST.Input.X415_Level_1();
            //            break;
            //        case "2":
            //            Sensor_Status = Hardware.IO_LIST.Input.X414_Level_2();
            //            break;
            //        case "3":
            //            Sensor_Status = Hardware.IO_LIST.Input.X413_Level_3();
            //            break;
            //        case "4":
            //            Sensor_Status = Hardware.IO_LIST.Input.X412_Level_4();
            //            break;
            //        case "5":
            //            Sensor_Status = Hardware.IO_LIST.Input.X411_Level_5();
            //            break;
            //        case "6":
            //            Sensor_Status = Hardware.IO_LIST.Input.X410_Level_6();
            //            break;
            //        case "7":
            //            Sensor_Status = Hardware.IO_LIST.Input.X409_Level_7();
            //            break;
            //        case "8":
            //            Sensor_Status = Hardware.IO_LIST.Input.X408_Level_8();
            //            break;
            //        case "9":
            //            Sensor_Status = Hardware.IO_LIST.Input.X407_Level_9();
            //            break;
            //        case "10":
            //            Sensor_Status = Hardware.IO_LIST.Input.X406_Level_10();
            //            break;
            //        case "11":
            //            Sensor_Status = Hardware.IO_LIST.Input.X405_Level_11();
            //            break;
            //        case "12":
            //            Sensor_Status = Hardware.IO_LIST.Input.X404_Level_12();
            //            break;
            //    }
            //    bool DB_Status = false;
            //    if (x["STATUS"].ToString() == StaticRes.Global.Binning_Status.Empty || x["STATUS"].ToString() == "Empty")
            //        DB_Status = false; //False=empty
            //    else
            //        DB_Status = true;
            //    if (Sensor_Status != DB_Status)
            //    {
            //        ObjectModule.Local.Temp_Inventory_Check tic = new Temp_Inventory_Check();
            //        tic.DB_Part_ID = x["PART_ID"].ToString();
            //        tic.DB_Status = x["STATUS"].ToString();
            //        tic.DB_Wire_Type = x["SAPCODE"].ToString();
            //        if (Sensor_Status)
            //            tic.Physical_Status = "PRESENT";
            //        else
            //            tic.Physical_Status = StaticRes.Global.Binning_Status.Empty;
            //        tic.Slot_Level = x["SLOT_Level"].ToString();
            //        tic.Slot_No = Slot_No.ToString();
            //        list.Add(tic);
            //        Common.Reports.LogFile.Log("Inventory check finding:S" + Slot_No.ToString() + "L" + x["SLOT_LEVEL"].ToString() +
            //                                   ",DB Status:" + tic.DB_Status + ",Physical Status:" + tic.Physical_Status +
            //                                   ",DB wire type:" + tic.DB_Wire_Type + ",DB part ID:" + tic.DB_Part_ID);
            //    }
            //}
        }

        public void LoadingHandle()
        {
            try
            {
                HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                this.Thread_Inventory_Check.Abort();
            }
            catch
            {
                StaticRes.Global.Need_Homing = true;
            }
        }
    }
}
