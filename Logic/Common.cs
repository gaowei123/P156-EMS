using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataProvider;
using ObjectModule;
using System.Reflection;

namespace Logic
{
    public class Common
    {
        public static List<ObjectModule.Local.Binning> Slot_Status()
        {
            List<ObjectModule.Local.Binning> xx = new List<ObjectModule.Local.Binning>();
            ObjectModule.Local.Binning fb = new ObjectModule.Local.Binning();
            DataTable dt = DataProvider.Local.Binning.Select.All();
            foreach (DataRow a in dt.Rows)
            {
                fb = new ObjectModule.Local.Binning(a);
                xx.Add(fb);
            }
            return xx;
        }

        public static void MultiBarcode_Cut(string MultiBarcode, ref string SSDS_PN, ref string Gross_Weight, ref string Part_ID)
        {
            try
            {
                SSDS_PN = MultiBarcode.Substring(0, 11);
                Gross_Weight = MultiBarcode.Substring(11, 2) + "." + MultiBarcode.Substring(13, 2);
                Part_ID = MultiBarcode.Substring(15);
            }
            catch
            {
                SSDS_PN = "";
                Gross_Weight = "0";
                Part_ID = "";
            }
        }

        public static DataTable Event_Search(string DateFrom, string DateTo)
        {
            DateTime Date_From = DateTime.Parse(DateFrom);
            DateTime Date_To = DateTime.Parse(DateTo);
            return DataProvider.Local.Event.Select(Date_From, Date_To);
        }

        public static DataTable Tracking_Search(string Part_ID, string MC_ID)
        {
            return DataProvider.Local.Tracking.Select.All(Part_ID, MC_ID);
        }

        public static DataTable History_Search(DateTime dateFrom, DateTime dateTo,string Part_ID,string MC_ID,bool Return_With_Expiry)
        {
            return DataProvider.Local.History.Select(dateFrom, dateTo, Part_ID, MC_ID,Return_With_Expiry);
        }

        public static List<ObjectModule.Local.Summary_List> History_Summary_Search(DateTime dateFrom, DateTime dateTo)
        {
            //return DataProvider.Local.History.Select_Summary(dateFrom, dateTo);
            List<ObjectModule.Local.Summary_List> List = new List<ObjectModule.Local.Summary_List>();
            DataTable dt = DataProvider.Local.History.Select(dateFrom, dateTo, string.Empty, string.Empty, false);
            List<ObjectModule.Local.Tracking> list_history = new List<ObjectModule.Local.Tracking>();
            foreach (DataRow x in dt.Rows)
            {
                ObjectModule.Local.Tracking t = new ObjectModule.Local.Tracking(x);
                list_history.Add(t);
            }
            DataTable dt_partID_List = DataProvider.Local.History.PartID_List(dateFrom, dateTo);
            foreach (DataRow x in dt_partID_List.Rows)
            {
                List<ObjectModule.Local.Tracking> list_load = (from m in list_history where m.PART_ID == x["PART_ID"].ToString() && m.ACTION == StaticRes.Global.Event.Load select m).ToList<ObjectModule.Local.Tracking>();
                if (list_load.Count >= 1)
                {
                    ObjectModule.Local.Tracking load = list_load.First<ObjectModule.Local.Tracking>();
                    ObjectModule.Local.Summary_List xx = new ObjectModule.Local.Summary_List();
                    xx.BATCH_NO = load.BATCH_NO;
                    xx.CAPACITY = load.CAPACITY.ToString();
                    xx.LOAD_WEIGHT = load.CURRENT_WEIGHT.ToString();
                    xx.DEPARTMENT = load.DEPARTMENT;
                    xx.DESCRIPTION = load.DESCRIPTION;
                    xx.EMPTY_SYRINGE_WEIGHT = load.EMPTY_SYRINGE_WEIGHT.ToString();
                    xx.EXPIRY_DATETIME = load.EXPIRY_DATETIME.ToString();
                    xx.LOAD_TIME = load.UPDATED_TIME.ToString();
                    xx.LOAD_USER = load.USER_ID;
                    xx.LOAD_USER_NAME = load.USER_NAME;
                    xx.LOAD_WEIGHT = load.START_WEIGHT.ToString();
                    xx.MF_EXPIRY_DATETIME = load.MF_EXPIRY_DATE.ToString();
                    xx.PART_ID = load.PART_ID;
                    xx.READY_DATETIME = load.READY_DATETIME.ToString();
                    xx.SAPCODE = load.SAPCODE;
                    xx.THAWING_DATETIME = load.THAWING_DATETIME.ToString();
                    List<ObjectModule.Local.Tracking> list_unload = (from m in list_history where m.PART_ID == x["PART_ID"].ToString() && m.ACTION == StaticRes.Global.Event.Unload select m).ToList<ObjectModule.Local.Tracking>();
                    if (list_unload.Count >= 1)
                    {
                        ObjectModule.Local.Tracking unload = list_unload.First<ObjectModule.Local.Tracking>();
                        xx.UNLOAD_WEIGHT_1 = unload.CURRENT_WEIGHT.ToString();
                        xx.DEVICE_1 = unload.DEVICE.ToString();
                        xx.EQUIP_ID_1 = unload.EQUIP_ID.ToString();
                        xx.LOCID_1 = unload.LOCID.ToString();
                        xx.LOT_ID_1 = unload.LOT_ID.ToString();
                        xx.UNLOAD_TIME_1 = unload.UPDATED_TIME.ToString();
                        xx.UNLOAD_USER_1 = unload.USER_ID;
                        xx.UNLOAD_USER_NAME_1 = unload.USER_NAME;
                        xx.UNLOAD_WEIGHT_1 = unload.CURRENT_WEIGHT.ToString();
                    }
                    if (list_unload.Count == 2)
                    {
                        ObjectModule.Local.Tracking unload = list_unload.Last<ObjectModule.Local.Tracking>();
                        xx.UNLOAD_WEIGHT_2 = unload.CURRENT_WEIGHT.ToString();
                        xx.DEVICE_2 = unload.DEVICE.ToString();
                        xx.EQUIP_ID_2 = unload.EQUIP_ID.ToString();
                        xx.LOCID_2 = unload.LOCID.ToString();
                        xx.LOT_ID_2 = unload.LOT_ID.ToString();
                        xx.UNLOAD_TIME_2 = unload.UPDATED_TIME.ToString();
                        xx.UNLOAD_USER_2 = unload.USER_ID;
                        xx.UNLOAD_USER_NAME_2 = unload.USER_NAME;
                        xx.UNLOAD_WEIGHT_2 = unload.CURRENT_WEIGHT.ToString();
                    }
                    List<ObjectModule.Local.Tracking> list_return = (from m in list_history where m.PART_ID == x["PART_ID"].ToString() && m.ACTION == StaticRes.Global.Event.Reuse select m).ToList<ObjectModule.Local.Tracking>();
                    if (list_return.Count == 1)
                    {
                        ObjectModule.Local.Tracking Return = list_return.First<ObjectModule.Local.Tracking>();
                        xx.RETURN_TIME_1 = Return.UPDATED_TIME.ToString();
                        xx.RETURN_USER_1 = Return.USER_ID;
                        xx.RETURN_USER_NAME_1 = Return.USER_NAME;
                        xx.RETURN_WEIGHT_1 = Return.CURRENT_WEIGHT.ToString();
                    }
                    if (list_return.Count == 2)
                    {
                        ObjectModule.Local.Tracking Return = list_return.Last<ObjectModule.Local.Tracking>();
                        xx.RETURN_TIME_2 = Return.UPDATED_TIME.ToString();
                        xx.RETURN_USER_2 = Return.USER_ID;
                        xx.RETURN_USER_NAME_2 = Return.USER_NAME;
                        xx.RETURN_WEIGHT_2 = Return.CURRENT_WEIGHT.ToString();
                    }
                    List<ObjectModule.Local.Tracking> list_scrap = (from m in list_history where m.PART_ID == x["PART_ID"].ToString() && m.ACTION == StaticRes.Global.Event.Scrap select m).ToList<ObjectModule.Local.Tracking>();
                    if (list_scrap.Count == 1)
                    {
                        ObjectModule.Local.Tracking scrap = list_scrap.First<ObjectModule.Local.Tracking>();
                        xx.SCRAP_TIME = scrap.UPDATED_TIME.ToString();
                        xx.SCRAP_USER = scrap.USER_ID;
                        xx.SCRAP_USER_NAME = scrap.USER_NAME;
                        xx.SCRAP_WEIGHT = scrap.CURRENT_WEIGHT.ToString();
                    }
                    List.Add(xx);
                }
            }
            return List;
        }

        public static DataTable ToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }

            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
            }

            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);

                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public static DataTable Bin_Search()
        {
            return DataProvider.Local.Binning.Select.Bin_WithMateria();
        }

        public static DataTable User_Search(string User_ID)
        {
            return DataProvider.Local.User.Select(User_ID);
        }

        public static bool User_Insert(ObjectModule.Local.User gu)
        {
            return DataProvider.Local.User.Insert(gu);
        }

        public static bool User_Update(ObjectModule.Local.User gu)
        {
            return DataProvider.Local.User.Update(gu);
        }

        public static bool User_Delete(string User_ID, string Department)
        {
            return DataProvider.Local.User.Delete(User_ID, Department);
        }

        public static DataTable Equipment_Search(string Equip_ID)
        {
            return DataProvider.Local.Equipment.Select(Equip_ID);
        }

        public static bool Equipment_Insert(ObjectModule.Local.Equipment ge)
        {
            return DataProvider.Local.Equipment.Insert(ge);
        }

        public static bool Equipment_Update(ObjectModule.Local.Equipment ge)
        {
            return DataProvider.Local.Equipment.Update(ge);
        }

        public static bool Equipment_Delete(string Equip_ID,string Department)
        {
            return DataProvider.Local.Equipment.Delete(Equip_ID,Department);
        }

        public static DataTable SAP_Search(string Sapcode)
        {
            return DataProvider.Local.Sapcode.Select(Sapcode);
        }

        public static bool SAP_Insert(ObjectModule.Local.Sapcode gs)
        {
            if (gs.SAPCODE.Length == 0)
                throw new System.Exception("Please input SAP Code !!\n请输入SAP号码！！");
            if (gs.DESCRIPTION.Length == 0)
                throw new System.Exception("Please input Description !!\n请输入描述！！");
            if (gs.EMPTY_SYRINGE_WEIGHT >= gs.NEW_MIN_WEIGHT)
                throw new System.Exception("Empty Syringe Weight can not bigger than New Min Weight\n空注射器重量不能超过新最小重量！！");
            if (gs.NEW_MIN_WEIGHT >= gs.NEW_MAX_WEIGHT)
                throw new System.Exception("New Min Weight can not bigger than New Max Weight\n新的最小重量不能超过最大重量！！");
            if (gs.USAGE_LIFE <= 0)
                throw new System.Exception("Usage Life must bigger than 0\n用量重量必须大于0！！");
            return DataProvider.Local.Sapcode.Insert(gs);
        }

        public static bool SAP_Update(ObjectModule.Local.Sapcode gs)
        {
            if (gs.SAPCODE.Length == 0)
                throw new System.Exception("Please input SAP Code !!\n请输入SAP号码！！");
            if (gs.DESCRIPTION.Length == 0)
                throw new System.Exception("Please input Description !!\n请输入描述！！");
            if (gs.EMPTY_SYRINGE_WEIGHT >= gs.NEW_MIN_WEIGHT)
                throw new System.Exception("Empty Syringe Weight can not bigger than New Min Weight\n空注射器重量不能超过新最小重量！！");
            if (gs.NEW_MIN_WEIGHT >= gs.NEW_MAX_WEIGHT)
                throw new System.Exception("New Min Weight can not bigger than New Max Weight\n新的最小重量不能超过最大重量！！");
            if (gs.USAGE_LIFE <= 0)
                throw new System.Exception("Usage Life must bigger than 0\n用量重量必须大于0！！");
            return DataProvider.Local.Sapcode.Update(gs);
        }

        public static bool SAP_Delete(string Sapcode, string Department)
        {
            return DataProvider.Local.Sapcode.Delete(Sapcode, Department);
        }

        public static DataTable Department_Select()
        {
            return DataProvider.Local.Department.Select();
        }

        public static bool Department_Insert(string Department)
        {
            return DataProvider.Local.Department.Insert(Department);
        }

        public static bool Department_Delete(string Department)
        {
            return DataProvider.Local.Department.Delete(Department);
        }

        public static DataTable Configure_Search()
        {
            return DataProvider.Local.Configure.Select();
        }

        public static bool Configure_Update(ObjectModule.Local.Configure sc)
        {
            return DataProvider.Local.Configure.Update(sc);
        }

        public static int weekofyear(DateTime dtime)
        {
            int weeknum = 0;
            DateTime tmpdate = DateTime.Parse(dtime.Year.ToString() + "-1" + "-1");
            DayOfWeek firstweek = tmpdate.DayOfWeek;
            //if(firstweek) 
            for (int i = (int)firstweek + 1; i <= dtime.DayOfYear; i = i + 7)
            {
                weeknum = weeknum + 1;
            }
            return weeknum;
        }
    }
}
