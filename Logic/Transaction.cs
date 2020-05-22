using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Logic
{
    public class Transaction
    {
        public static ObjectModule.Local.Sapcode Sap_Infor(string Sapcode, string Department)
        {
            if (Sapcode.Length == 0)
                throw new System.Exception("Please input Sapcode first !!\n请输入材料型号！！");
            DataTable dt = DataProvider.Local.Sapcode.Sap_Infor(Sapcode, Department);
            if (dt.Rows.Count == 0)
                throw new System.Exception("Invalid Sapcode !!\n无效的材料型号");
            ObjectModule.Local.Sapcode gs = new ObjectModule.Local.Sapcode(dt.Rows[0]);
            return gs;
        }

        public static DataTable Expiry_List()
        {
            return DataProvider.Local.Tracking.Select.Expiry_List_on_Machine(float.Parse(StaticRes.Global.System_Setting.Reminder_time));
        }

        public static ObjectModule.Local.Binning Search_Empty_Slot(int Capacity)
        {

            DataTable dt = DataProvider.Local.Binning.Select.Search_Empty_Position(Capacity);
            if (dt.Rows.Count == 0)
                throw new System.Exception("Machine Full Already !!\n机器已满！！");
            if (dt.Rows.Count <= StaticRes.Global.System_Setting.Storage_Pre_Set_Qty)
                throw new System.Exception("Machine empty slot less/equal than pre set qty by admin\n机器空槽/等于小于预先设置数量管理！！");
            ObjectModule.Local.Binning gb = new ObjectModule.Local.Binning(dt.Rows[0]);
            return gb;
        }

        public class Load
        {
            public static void Part_ID_Unique_Validate(string Part_ID,string Batch_No)
            {
                if (Part_ID.Length == 0)
                    throw new System.Exception("Please scan part ID first !!\n请扫描part ID!!");
                if (Part_ID == Batch_No)
                    throw new System.Exception("Part ID can not same with Batch No !!\n part ID不能和Batch No一样");
                DataTable dt = DataProvider.Local.Tracking.Select.All(Part_ID, string.Empty);
                if (dt.Rows.Count > 0)
                    throw new System.Exception("This part ID already exist , please inform engineer !!\n这一部分ID已经存在,请通知工程师!!");
            }

            public static ObjectModule.Local.Binning BatchNo_Validate(string Sapcode, string Batch_No,ref int Record_Count)
            {
                if (Batch_No.Length <= 0 &&Batch_No.Length >=10)
                    throw new System.Exception("Please input Batch No !!\n请输入Batch号码");
                ObjectModule.Local.Binning bin = new ObjectModule.Local.Binning();
                DataTable dt = DataProvider.Local.Binning.Select.By_SapcodeBatchNo(Sapcode, Batch_No);
                Record_Count = dt.Rows.Count;
                if (dt.Rows.Count > 0)
                {
                    bin = new ObjectModule.Local.Binning(dt.Rows[0]);
                }
                return bin;
            }

            public static void Weight_Validation(string Weight, string Max_Weight, string Min_Weight)
            {
                if (float.Parse(Weight) < float.Parse(Min_Weight))
                    throw new System.Exception("Weight is smaller than Min weight, need weight again !!\n重量小于最小重量,请再称一次!!");
                else if (float.Parse(Weight) > float.Parse(Max_Weight))
                    throw new System.Exception("Weight is bigger than Max weight, need weight again !!\n重量比最大重量,请再称一次!!");
            }

            public static bool Local_Update(string Slot_ID,string Slot_Index,string Batch_No,string Sapcode,string Description,
                string Part_ID,string Thawing_Time,string Ready_Time,string Expiry_Time,string MF_Expiry_Date,string Weight,string Capacity,string Empty_Syringe_Weight)
            {
                #region Update BINNING DB
                ObjectModule.Local.Binning gb = new ObjectModule.Local.Binning();
                gb.BATCH_NO = Batch_No;
                gb.CAPACITY = int.Parse(Capacity);
                gb.CURRENT_WEIGHT = float.Parse(Weight);
                gb.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                gb.DESCRIPTION = Description;
                gb.EXPIRY_DATETIME = DateTime.Parse(Expiry_Time);
                gb.MF_EXPIRY_DATE = DateTime.Parse(MF_Expiry_Date);
                gb.PART_ID = Part_ID;
                gb.READY_DATETIME = DateTime.Parse(Ready_Time);
                gb.SAPCODE = Sapcode;
                gb.SLOT_ID = int.Parse(Slot_ID);
                gb.SLOT_INDEX = int.Parse(Slot_Index);
                gb.START_WEIGHT = float.Parse(Weight);
                gb.STATUS = StaticRes.Global.Binning_Status.New;
                gb.THAWING_DATETIME = DateTime.Parse(Thawing_Time);
                gb.UPDATED_TIME = System.DateTime.Now;
                gb.USER_GROUP = StaticRes.Global.Current_User.USER_GROUP;
                gb.USER_ID = StaticRes.Global.Current_User.USER_ID;
                gb.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                #endregion

                #region Insert Trakcing/History
                ObjectModule.Local.Tracking gt = new ObjectModule.Local.Tracking();
                gt.ACTION = StaticRes.Global.Event.Load;
                gt.BATCH_NO = Batch_No;
                gt.CAPACITY = int.Parse(Capacity);
                gt.CURRENT_WEIGHT = float.Parse(Weight);
                gt.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                gt.DESCRIPTION = Description;
                gt.DEVICE = "";
                gt.EMPTY_SYRINGE_WEIGHT = float.Parse(Empty_Syringe_Weight);
                gt.EQUIP_ID = "";
                gt.EXPIRY_DATETIME = DateTime.Parse(Expiry_Time);
                gt.LOCID = "";
                gt.LOT_ID = "";
                gt.MF_EXPIRY_DATE = DateTime.Parse(MF_Expiry_Date);
                gt.MONTH = System.DateTime.Now.Month;
                gt.PART_ID = Part_ID;
                gt.READY_DATETIME = DateTime.Parse(Ready_Time);
                gt.REMARKS = "";
                gt.SAPCODE = Sapcode;
                gt.START_WEIGHT = float.Parse(Weight);
                gt.STATUS = StaticRes.Global.Status.Load;
                gt.THAWING_DATETIME = DateTime.Parse(Thawing_Time);
                gt.UPDATED_TIME = System.DateTime.Now;
                gt.USER_ID = StaticRes.Global.Current_User.USER_ID;
                gt.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                gt.WEEK = Logic.Common.weekofyear(System.DateTime.Now);
                gt.YEAR = System.DateTime.Now.Year;
                return DataProvider.Rollback.Load(gb, gt);
                #endregion
            }
        }

        public class Unload
        {
            public static ObjectModule.Local.Equipment Equip_Infor(string Equip_ID)
            {
                if (Equip_ID.Length == 0)
                    throw new System.Exception("Please input Equip ID first !!\n请先输入装备ID!!");
                DataTable dt = DataProvider.Local.Equipment.Select(Equip_ID);
                if (dt.Rows.Count == 0)
                    throw new System.Exception("Invalid Equip ID !!\n无效的装备ID!!");
                DataTable dt_tracking = DataProvider.Local.Tracking.Equip_Status_Check(Equip_ID, StaticRes.Global.Status.Unload);
                if (dt_tracking.Rows.Count > 0)
                    throw new System.Exception("This machine already withdraw one epoxy , please return \n这台机器已经撤出一个环氧树脂,请返回！！" + dt_tracking.Rows[0]["PART_ID"].ToString() + " first !!");
                ObjectModule.Local.Equipment ge = new ObjectModule.Local.Equipment(dt.Rows[0]);
                return ge;
            }

            public static void Weight_Validation(string Weighting_weight, string sapcode, string department)
            {
                DataTable dt = DataProvider.Local.Sapcode.Sap_Infor(sapcode, department);
                if (dt.Rows.Count == 0)
                    throw new System.Exception("Invalid Sapcode !!\n无效的物料型号！！");
                ObjectModule.Local.Sapcode gs = new ObjectModule.Local.Sapcode(dt.Rows[0]);
                if (float.Parse(Weighting_weight) < gs.NEW_MIN_WEIGHT || float.Parse(Weighting_weight) > gs.NEW_MAX_WEIGHT)
                    throw new System.Exception("This solder ball weight out of SPEC, please weighting again !!");
            }

            public static void Lot_Validation(string Lot_ID)
            {
                if (Lot_ID.Length<6 || Lot_ID.Length>12)
                    throw new System.Exception("Please input lot ID first !!\n请输入lot ID！！");
            }
            public static void mesDevice_Validation(string mesDevice)
            {
                if (mesDevice.Length == 0)
                    throw new System.Exception("Please input PartNo first !!\n请输入PartNo！！");
            }

            public static ObjectModule.Local.Binning search_Inventory(string sapcode, string department)
            {
                DataTable dt = DataProvider.Local.Binning.Select.By_Reuse_Sapcode(sapcode, department);
                if (dt.Rows.Count == 0)
                {
                    dt = DataProvider.Local.Binning.Select.By_New_Sapcode(sapcode, department);
                    if (dt.Rows.Count == 0)
                        throw new System.Exception("Non inventory inside machine !!\n机器里面没有！！");
                    else
                    {
                        ObjectModule.Local.Binning gb = new ObjectModule.Local.Binning(dt.Rows[0]);
                        return gb;
                    }
                }
                else
                {
                    ObjectModule.Local.Binning gb = new ObjectModule.Local.Binning(dt.Rows[0]);
                    return gb;
                }
            }

            public static DateTime Expiry_Time_Validate(DateTime Expiry_Time_Shelf, int Usage_Life)
            {
                DateTime Expiry_Time = System.DateTime.Now.AddHours(Usage_Life);
                if (Expiry_Time <= Expiry_Time_Shelf)
                    return Expiry_Time;
                else
                    return Expiry_Time_Shelf;
            }

            public static bool Local_Update(string Part_ID, string Slot_ID, string Slot_Index, string Equip_ID, string LocID,
                string status, string Start_Weight, string Current_Weight, string Sapcode, string Thawing_Time, string Ready_Time, string Expiry_Time,
                    string Description, string Batch_No, string Capacity, string Lot_ID, string Device)
            {
                #region Update BINNING DB
                ObjectModule.Local.Binning gb = new ObjectModule.Local.Binning();
                gb.BATCH_NO = "";
                gb.CAPACITY = int.Parse(Capacity);
                gb.CURRENT_WEIGHT = 0;
                gb.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                gb.DESCRIPTION = "";
                gb.EXPIRY_DATETIME = System.DateTime.Now;
                gb.MF_EXPIRY_DATE = System.DateTime.Now;
                gb.PART_ID = "";
                gb.READY_DATETIME = System.DateTime.Now;
                gb.SAPCODE = "";
                gb.SLOT_ID = int.Parse(Slot_ID);
                gb.SLOT_INDEX = int.Parse(Slot_Index);
                gb.START_WEIGHT = 0;
                gb.STATUS = StaticRes.Global.Binning_Status.Empty;
                gb.THAWING_DATETIME = System.DateTime.Now;
                gb.UPDATED_TIME = System.DateTime.Now;
                gb.USER_GROUP = StaticRes.Global.Current_User.USER_GROUP;
                gb.USER_ID = StaticRes.Global.Current_User.USER_ID;
                gb.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                #endregion

                #region Insert TRACKING/History
                DataTable dt = new DataTable();
                if (status == StaticRes.Global.Status.Load) //New Material
                    dt = DataProvider.Local.Tracking.Select.by_PartID(Part_ID, StaticRes.Global.Status.Load);
                else
                    dt = DataProvider.Local.Tracking.Select.by_PartID(Part_ID, StaticRes.Global.Status.Reuse);
                if (dt.Rows.Count == 0)
                {
                    #region If data missing
                    ObjectModule.Local.Tracking gt = new ObjectModule.Local.Tracking();
                    gt.ACTION = StaticRes.Global.Event.Unload;
                    gt.BATCH_NO = Batch_No;
                    gt.CAPACITY = int.Parse(Capacity);
                    gt.CURRENT_WEIGHT = float.Parse(Current_Weight);
                    gt.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                    gt.DESCRIPTION = Description;
                    gt.DEVICE = Device;
                    gt.EMPTY_SYRINGE_WEIGHT = 0;
                    gt.EQUIP_ID = Equip_ID;
                    gt.EXPIRY_DATETIME = DateTime.Parse(Expiry_Time);
                    gt.LOCID = LocID;
                    gt.LOT_ID = Lot_ID;
                    gt.MF_EXPIRY_DATE = System.DateTime.Now.AddYears(1);
                    gt.MONTH = System.DateTime.Now.Month;
                    gt.PART_ID = Part_ID;
                    gt.READY_DATETIME = DateTime.Parse(Ready_Time);
                    gt.REMARKS = "Missing";
                    gt.SAPCODE = Sapcode;
                    gt.START_WEIGHT = float.Parse(Start_Weight);
                    gt.STATUS = StaticRes.Global.Status.PendingMix;
                    gt.THAWING_DATETIME = DateTime.Parse(Thawing_Time);
                    gt.UPDATED_TIME = System.DateTime.Now;
                    gt.USER_ID = StaticRes.Global.Current_User.USER_ID;
                    gt.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                    gt.WEEK = Logic.Common.weekofyear(System.DateTime.Now);
                    gt.YEAR = System.DateTime.Now.Year;
                    return DataProvider.Rollback.Unload_DataMissing(gb, gt);
                    #endregion
                }
                else
                {
                    ObjectModule.Local.Tracking gt = new ObjectModule.Local.Tracking(dt.Rows[0]);
                    gt.ACTION = StaticRes.Global.Event.Unload;
                    gt.DEVICE = Device;
                    gt.EQUIP_ID = Equip_ID;
                    gt.LOCID = LocID;
                    gt.LOT_ID = Lot_ID;
                    gt.MONTH = System.DateTime.Now.Month;
                    gt.STATUS = StaticRes.Global.Status.Unload;
                    gt.UPDATED_TIME = System.DateTime.Now;
                    gt.USER_ID = StaticRes.Global.Current_User.USER_ID;
                    gt.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                    gt.WEEK = Logic.Common.weekofyear(System.DateTime.Now);
                    gt.YEAR = System.DateTime.Now.Year; gt.ACTION = StaticRes.Global.Event.Unload;
                    gt.BATCH_NO = Batch_No;
                    gt.CAPACITY = int.Parse(Capacity);
                    gt.CURRENT_WEIGHT = float.Parse(Current_Weight);
                    gt.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                    gt.DESCRIPTION = Description;
                    gt.DEVICE = Device;
                    gt.EXPIRY_DATETIME = DateTime.Parse(Expiry_Time);
                    gt.MONTH = System.DateTime.Now.Month;
                    gt.PART_ID = Part_ID;
                    gt.READY_DATETIME = DateTime.Parse(Ready_Time);
                    gt.REMARKS = "";
                    gt.SAPCODE = Sapcode;
                    gt.START_WEIGHT = float.Parse(Start_Weight);
                    gt.STATUS = StaticRes.Global.Status.PendingMix;
                    gt.THAWING_DATETIME = DateTime.Parse(Thawing_Time);
                    gt.UPDATED_TIME = System.DateTime.Now;
                    gt.USER_ID = StaticRes.Global.Current_User.USER_ID;
                    gt.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                    gt.WEEK = Logic.Common.weekofyear(System.DateTime.Now);
                    gt.YEAR = System.DateTime.Now.Year;
                    return DataProvider.Rollback.Unload(gb, gt);
                }
                #endregion
            }
        }

        public class Return
        {
            public static ObjectModule.Local.Tracking PartID_Validate(string Part_ID)
            {
                DataTable dt = DataProvider.Local.Tracking.Select.by_PartID(Part_ID, StaticRes.Global.Status.Unload);
                if (dt.Rows.Count == 0)
                    throw new System.Exception("Invalid Part ID !!\n错误的Part ID!!");
                ObjectModule.Local.Tracking gt = new ObjectModule.Local.Tracking(dt.Rows[0]);
                return gt;
            }

            public static string Weight_Validation(string Weight, string Scrap_Weight, string Expiry_Time, string Syringe_Weight)
            {
                if (float.Parse(Weight) < float.Parse(Syringe_Weight))
                    throw new System.Exception("can not lighter than syringe weight , please weighing again !!\n不能比空管轻，请重试！！");
                else if  (DateTime.Parse(Expiry_Time) <= System.DateTime.Now.AddHours(StaticRes.Global.System_Setting.Time_1))//yakun.zhoou 2015/06/30
                    return StaticRes.Global.Status.Scrap;
                else if (float.Parse(Weight) <= float.Parse(Scrap_Weight))
                    return StaticRes.Global.Status.Scrap;
                else
                    return StaticRes.Global.Status.Reuse;
            }

            public static bool IsScrapBinFull()
            {
                int scrap_qty = 0;
                System.IO.StreamReader sr = new System.IO.StreamReader(".\\CurScrpQty.txt");
                scrap_qty = int.Parse(sr.ReadLine());
                sr.Close();

                //if (scrap_qty >= StaticRes.Global.System_Setting.Scrap_Limit_Qty)
                //    return true;
                //else
                    return false;
            }

            public static bool Local_Update(string status,string Batch_No,string Capacity,
                string Current_Weight,string Start_Weight,string Description,string Part_ID,string Sapcode,
                string Slot_ID,string Slot_Index,string Remarks,string Thawing_Time,string Ready_Time,string Expiry_Time,string Equip_ID)
            {
                #region Insert TRACKING/History
                DataTable dt = new DataTable();
                dt = DataProvider.Local.Tracking.Select.by_PartID(Part_ID, StaticRes.Global.Status.Unload);
                ObjectModule.Local.Tracking gt = new ObjectModule.Local.Tracking(dt.Rows[0]);
                if (status == StaticRes.Global.Status.Reuse)
                    gt.ACTION = StaticRes.Global.Event.Reuse;
                else
                    gt.ACTION = StaticRes.Global.Event.Scrap;
                gt.BATCH_NO = Batch_No;
                gt.CAPACITY = int.Parse(Capacity);
                gt.CURRENT_WEIGHT = float.Parse(Current_Weight);
                gt.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                gt.DESCRIPTION = Description;
                gt.DEVICE = "";
                //gt.EMPTY_SYRINGE_WEIGHT = float.Parse(Empty_Syringe_Weight);
                gt.EQUIP_ID = Equip_ID;
                gt.EXPIRY_DATETIME = DateTime.Parse(Expiry_Time);
                gt.LOT_ID = "";
                //gt.MF_EXPIRY_DATE = DateTime.Parse(MF_Expiry_Time);
                gt.MONTH = System.DateTime.Now.Month;
                gt.PART_ID = Part_ID;
                gt.READY_DATETIME = DateTime.Parse(Ready_Time);
                gt.REMARKS = Remarks;
                gt.SAPCODE = Sapcode;
                if (status == StaticRes.Global.Status.Reuse)
                    gt.STATUS = StaticRes.Global.Status.Reuse;
                else
                    gt.STATUS = StaticRes.Global.Status.Scrap;
                gt.START_WEIGHT = float.Parse(Start_Weight);
                gt.THAWING_DATETIME = DateTime.Parse(Thawing_Time);
                gt.UPDATED_TIME = System.DateTime.Now;
                gt.USER_ID = StaticRes.Global.Current_User.USER_ID;
                gt.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                gt.WEEK = Logic.Common.weekofyear(System.DateTime.Now);
                gt.YEAR = System.DateTime.Now.Year;
                #endregion

                #region Update BINNING DB
                ObjectModule.Local.Binning gb = new ObjectModule.Local.Binning();
                if (status == StaticRes.Global.Status.Reuse)
                {
                    gb.BATCH_NO = Batch_No;
                    gb.CAPACITY = int.Parse(Capacity);
                    gb.CURRENT_WEIGHT = float.Parse(Current_Weight);
                    gb.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                    gb.DESCRIPTION = Description;
                    gb.EXPIRY_DATETIME = DateTime.Parse(Expiry_Time);
                    gb.MF_EXPIRY_DATE = gt.MF_EXPIRY_DATE;
                    gb.PART_ID = Part_ID;
                    gb.READY_DATETIME = DateTime.Parse(Ready_Time);
                    gb.SAPCODE = Sapcode;
                    gb.SLOT_ID = int.Parse(Slot_ID);
                    gb.SLOT_INDEX = int.Parse(Slot_Index);
                    gb.START_WEIGHT = float.Parse(Start_Weight);
                    gb.STATUS = StaticRes.Global.Status.Reuse;
                    gb.THAWING_DATETIME = DateTime.Parse(Thawing_Time);
                    gb.UPDATED_TIME = System.DateTime.Now;
                    gb.USER_GROUP = StaticRes.Global.Current_User.USER_GROUP;
                    gb.USER_ID = StaticRes.Global.Current_User.USER_ID;
                    gb.USER_NAME = StaticRes.Global.Current_User.USER_NAME;

                }
                return DataProvider.Rollback.Return(gb, gt);
                #endregion
               
            }
        }

        public class Remove
        {
            public static DataTable Bin_withMaterial()
            {
                return DataProvider.Local.Binning.Select.Bin_WithMateria();
            }

            public static bool Local_Update(string Part_ID, string Slot_ID, string Slot_Index, string Equip_ID, 
                string status, string Start_Weight, string Current_Weight, string Sapcode, string Thawing_Time, string Ready_Time, string Expiry_Time,
                    string Description, string Batch_No, string Capacity, string Remark)
            {
                #region Update BINNING DB
                ObjectModule.Local.Binning gb = new ObjectModule.Local.Binning();
                gb.BATCH_NO = "";
                gb.CAPACITY = int.Parse(Capacity);
                gb.CURRENT_WEIGHT = 0;
                gb.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                gb.DESCRIPTION = "";
                gb.EXPIRY_DATETIME = System.DateTime.Now;
                gb.MF_EXPIRY_DATE = System.DateTime.Now;
                gb.PART_ID = "";
                gb.READY_DATETIME = System.DateTime.Now;
                gb.SAPCODE = "";
                gb.SLOT_ID = int.Parse(Slot_ID);
                gb.SLOT_INDEX = int.Parse(Slot_Index);
                gb.START_WEIGHT = 0;
                gb.STATUS = StaticRes.Global.Binning_Status.Empty;
                gb.THAWING_DATETIME = System.DateTime.Now;
                gb.UPDATED_TIME = System.DateTime.Now;
                gb.USER_GROUP = StaticRes.Global.Current_User.USER_GROUP;
                gb.USER_ID = StaticRes.Global.Current_User.USER_ID;
                gb.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                #endregion

                #region Insert TRACKING/History
                DataTable dt = new DataTable();
                if (status == StaticRes.Global.Status.Load) //New Material
                    dt = DataProvider.Local.Tracking.Select.by_PartID(Part_ID, StaticRes.Global.Status.Load);
                else
                    dt = DataProvider.Local.Tracking.Select.by_PartID(Part_ID, StaticRes.Global.Status.Reuse);
                if (dt.Rows.Count == 0)
                {
                    #region If data missing
                    ObjectModule.Local.Tracking gt = new ObjectModule.Local.Tracking();
                    gt.ACTION = StaticRes.Global.Event.Remove;
                    gt.BATCH_NO = Batch_No;
                    gt.CAPACITY = int.Parse(Capacity);
                    gt.CURRENT_WEIGHT = float.Parse(Current_Weight);
                    gt.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                    gt.DESCRIPTION = Description;
                    gt.DEVICE = "";
                    gt.EMPTY_SYRINGE_WEIGHT = 0;
                    if (Equip_ID.Length > 0)
                        gt.EQUIP_ID = Equip_ID;
                    else
                        gt.EQUIP_ID = "";
                    gt.EXPIRY_DATETIME = DateTime.Parse(Expiry_Time);
                    gt.LOCID = "";
                    gt.LOT_ID = "";
                    gt.MF_EXPIRY_DATE = System.DateTime.Now.AddYears(1);
                    gt.MONTH = System.DateTime.Now.Month;
                    gt.PART_ID = Part_ID;
                    gt.READY_DATETIME = DateTime.Parse(Ready_Time);
                    gt.REMARKS = "Missing";
                    gt.SAPCODE = Sapcode;
                    gt.START_WEIGHT = float.Parse(Start_Weight);
                    gt.STATUS = StaticRes.Global.Status.Unload;
                    gt.THAWING_DATETIME = DateTime.Parse(Thawing_Time);
                    gt.UPDATED_TIME = System.DateTime.Now;
                    gt.USER_ID = StaticRes.Global.Current_User.USER_ID;
                    gt.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                    gt.WEEK = Logic.Common.weekofyear(System.DateTime.Now);
                    gt.YEAR = System.DateTime.Now.Year;
                    return DataProvider.Rollback.Unload_DataMissing(gb, gt);
                    #endregion
                }
                else
                {
                    ObjectModule.Local.Tracking gt = new ObjectModule.Local.Tracking(dt.Rows[0]);
                    gt.ACTION = StaticRes.Global.Event.Remove;
                    gt.DEVICE = "";                   
                    gt.LOCID = "";
                    gt.LOT_ID = "";
                    gt.MONTH = System.DateTime.Now.Month;                   
                    gt.UPDATED_TIME = System.DateTime.Now;
                    gt.USER_ID = StaticRes.Global.Current_User.USER_ID;
                    gt.USER_NAME = StaticRes.Global.Current_User.USER_NAME;
                    gt.WEEK = Logic.Common.weekofyear(System.DateTime.Now);
                    gt.YEAR = System.DateTime.Now.Year;                    
                    gt.BATCH_NO = Batch_No;
                    gt.CAPACITY = int.Parse(Capacity);
                    gt.CURRENT_WEIGHT = float.Parse(Current_Weight);
                    gt.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                    gt.DESCRIPTION = Description;
                    if (Equip_ID.Length > 0)
                        gt.EQUIP_ID = Equip_ID;
                    else
                        gt.EQUIP_ID = "";
                    gt.EXPIRY_DATETIME = DateTime.Parse(Expiry_Time);                 
                    gt.PART_ID = Part_ID;
                    gt.READY_DATETIME = DateTime.Parse(Ready_Time);
                    gt.REMARKS = Remark;
                    gt.SAPCODE = Sapcode;
                    gt.START_WEIGHT = float.Parse(Start_Weight);
                    gt.STATUS = StaticRes.Global.Status.Remove;
                    gt.THAWING_DATETIME = DateTime.Parse(Thawing_Time);               
                    return DataProvider.Rollback.Unload(gb, gt);
                }
                #endregion
            }
        }

        


    }
}
