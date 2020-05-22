using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataProvider.Local;

namespace DataProvider
{
    public class Rollback
    {
        public static bool Load(ObjectModule.Local.Binning gb , ObjectModule.Local.Tracking gt)
        {
            try
            {
                List<SqlCommand> list = new List<SqlCommand>();

                SqlCommand cmd_Binning = Binning.Update(gb);
                list.Add(cmd_Binning);
                SqlCommand cmd_Tracking = Tracking.Insert(gt);
                list.Add(cmd_Tracking);
                SqlCommand cmd_History = History.Insert(gt);
                list.Add(cmd_History);
                return Common.DB.SqlDB.SetData_Rollback(list, StaticRes.Local);
            }
            catch
            {
                return false;
            }
        }

        public static bool Unload(ObjectModule.Local.Binning gb, ObjectModule.Local.Tracking gt)
        {
            List<SqlCommand> list = new List<SqlCommand>();
            try
            {
                SqlCommand cmd_Binning = Binning.Update(gb);
                list.Add(cmd_Binning);
                SqlCommand cmd_Tracking = Tracking.Update(gt);
                list.Add(cmd_Tracking);
                SqlCommand cmd_History = History.Insert(gt);
                list.Add(cmd_History);
                return Common.DB.SqlDB.SetData_Rollback(list, StaticRes.Local);
            }
            catch
            {
                return false;
            }
        }

        public static bool Unload_DataMissing(ObjectModule.Local.Binning gb, ObjectModule.Local.Tracking gt)
        {
            List<SqlCommand> list = new List<SqlCommand>();
            try
            {
                SqlCommand cmd_Binning = Binning.Update(gb);
                list.Add(cmd_Binning);
                SqlCommand cmd_Tracking = Tracking.Insert(gt);
                list.Add(cmd_Tracking);
                SqlCommand cmd_History = History.Insert(gt);
                list.Add(cmd_History);
                return Common.DB.SqlDB.SetData_Rollback(list, StaticRes.Local);
            }
            catch
            {
                return false;
            }
        }


        public static bool Mix_DataMissing(ObjectModule.Local.Tracking trackingModel)
        {
            try
            {
                List<SqlCommand> cmdList = new List<SqlCommand>();

                cmdList.Add(Tracking.Insert(trackingModel));
                cmdList.Add(History.Insert(trackingModel));


                return Common.DB.SqlDB.SetData_Rollback(cmdList, StaticRes.Local);
            }
            catch
            {
                return false;
            }
        }









        public static bool Spool_Missing(ObjectModule.Local.Tracking gt,ObjectModule.Local.Binning gb)
        {
            List<SqlCommand> list = new List<SqlCommand>();
            try
            {
                SqlCommand cmd_GEMS_Binning = Binning.Update(gb);
                list.Add(cmd_GEMS_Binning);
                SqlCommand cmd_GEMS_Tracking = Tracking.Delete(gt.PART_ID);
                list.Add(cmd_GEMS_Tracking);
                SqlCommand cmd_GEMS_History = History.Insert(gt);
                list.Add(cmd_GEMS_History);
                return Common.DB.SqlDB.SetData_Rollback(list, StaticRes.Local);
            }
            catch
            {
                return false;
            }
        }

        public static bool Return(ObjectModule.Local.Binning gb,ObjectModule.Local.Tracking gt)
        {
            List<SqlCommand> list = new List<SqlCommand>();
            try
            {              
                if (gt.STATUS == "REUSE")
                {
                    SqlCommand cmd_Binning = Binning.Update(gb);
                    list.Add(cmd_Binning);
                    SqlCommand cmd_Tracking = Tracking.Update(gt);
                    list.Add(cmd_Tracking);
                }
                else
                {
                    SqlCommand cmd_Tracing = Tracking.Delete(gt.PART_ID);
                    list.Add(cmd_Tracing);
                }
                
                SqlCommand cmd_History = History.Insert(gt);
                list.Add(cmd_History);
                return Common.DB.SqlDB.SetData_Rollback(list, StaticRes.Local);
            }
            catch
            {
                return false;
            }
        }
    }
}
