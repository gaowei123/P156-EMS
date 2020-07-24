using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace DataProvider.Local
{
    public class Tracking
    {
        public class Select
        {
            public static DataTable All(string Part_ID, string MC_ID)
            {
                try
                {
                    string sql = "Select * from Tracking where 1=1 ";
                    if (Part_ID.Length > 0)
                        sql += " and PART_ID=@PART_ID ";
                    if (MC_ID.Length > 0)
                        sql += " and EQUIP_ID=@EQUIP_ID ";
                    sql += " order by EQUIP_ID ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    if (Part_ID.Length > 0)
                        cmd.Parameters.Add("@PART_ID", System.Data.SqlDbType.VarChar).Value = Part_ID;
                    if(MC_ID.Length>0)
                        cmd.Parameters.Add("@EQUIP_ID", System.Data.SqlDbType.VarChar).Value = Part_ID;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable by_PartID(string part_ID,string Status)
            {
                try
                {
                    string sql = "Select * from Tracking where PART_ID=@PART_ID and STATUS=@STATUS";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@PART_ID", System.Data.SqlDbType.VarChar).Value = part_ID;
                    cmd.Parameters.Add("@STATUS", System.Data.SqlDbType.VarChar).Value = Status;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }


            public static DataTable GetList(string sPartID, string sSAPCode, string sStatus)
            {
               
                StringBuilder strSql = new StringBuilder();
                strSql.Append("Select * from Tracking where 1=1 ");

                if (!string.IsNullOrEmpty(sPartID)) strSql.Append(" and PART_ID=@PART_ID ");
                if (!string.IsNullOrEmpty(sSAPCode)) strSql.Append(" and SAPCODE=@SAPCODE ");
                if (!string.IsNullOrEmpty(sStatus)) strSql.Append(" and STATUS=@STATUS ");



                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strSql.ToString());

                if (!string.IsNullOrEmpty(sPartID))
                    cmd.Parameters.Add("@PART_ID", System.Data.SqlDbType.VarChar).Value = sPartID;

                if (!string.IsNullOrEmpty(sSAPCode))
                    cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = sSAPCode;

                if (!string.IsNullOrEmpty(sStatus))
                    cmd.Parameters.Add("@STATUS", System.Data.SqlDbType.VarChar).Value = sStatus;




                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }





            public static DataTable Expiry_List()
            {
                try
                {
                    string sql = @"SELECT PART_ID,SAPCODE,BATCH_NO,DESCRIPTION,EQUIP_ID,EXPIRY_DATETIME 
                                    FROM EMS.dbo.Tracking where EXPIRY_DATETIME<=@EXPIRY_DATETIME  "; // VISHAY_EMS
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@EXPIRY_DATETIME", System.Data.SqlDbType.DateTime).Value = System.DateTime.Now;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Expiry_List_on_Machine(float time)
            {
                try
                {
                    string sql = "SELECT * FROM EMS.dbo.Tracking where EXPIRY_DATETIME<=@EXPIRY_DATETIME and STATUS='IN_USED' ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@EXPIRY_DATETIME", System.Data.SqlDbType.DateTime).Value = System.DateTime.Now.AddHours(time);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }
        }

        public static DataTable Equip_Status_Check(string Equip_ID)
        {
            try
            {
                string sql = "Select * from Tracking where EQUIP_ID=@EQUIP_ID and ( STATUS=@STATUS1 or STATUS=@STATUS2 ) ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@EQUIP_ID", System.Data.SqlDbType.VarChar).Value = Equip_ID;
                cmd.Parameters.Add("@STATUS1", System.Data.SqlDbType.VarChar).Value = "PENDING_MIX";
                cmd.Parameters.Add("@STATUS2", System.Data.SqlDbType.VarChar).Value = "IN_USED";
                
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static SqlCommand Delete(string Part_ID)
        {
            try
            {
                string sql = "Delete from Tracking where PART_ID=@PART_ID ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@PART_ID", System.Data.SqlDbType.VarChar).Value = Part_ID;
                return cmd;
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static SqlCommand Update(ObjectModule.Local.Tracking gt)
        {
            try
            {
                string sql = @"Update Tracking set CURRENT_WEIGHT=@CURRENT_WEIGHT,STATUS=@STATUS,DEVICE=@DEVICE,USER_NAME=@USER_NAME,
                               EQUIP_ID =@EQUIP_ID,LOCID=@LOCID,USER_ID=@USER_ID,LOT_ID=@LOT_ID,ACTION=@ACTION,REMARKS=@REMARKS,
                               UPDATED_TIME=@UPDATED_TIME,WEEK=@WEEK,MONTH=@MONTH,YEAR=@YEAR where PART_ID=@PART_ID ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@PART_ID", System.Data.SqlDbType.VarChar).Value = gt.PART_ID;
                cmd.Parameters.Add("@CURRENT_WEIGHT", System.Data.SqlDbType.Float).Value = gt.CURRENT_WEIGHT;
                cmd.Parameters.Add("@STATUS", System.Data.SqlDbType.VarChar).Value = gt.STATUS;
                cmd.Parameters.Add("@DEVICE", System.Data.SqlDbType.VarChar).Value = gt.DEVICE;
                cmd.Parameters.Add("@EQUIP_ID", System.Data.SqlDbType.VarChar).Value = gt.EQUIP_ID;
                cmd.Parameters.Add("@LOCID", System.Data.SqlDbType.VarChar).Value = gt.LOCID;
                cmd.Parameters.Add("@USER_ID", System.Data.SqlDbType.VarChar).Value = gt.USER_ID;
                cmd.Parameters.Add("@USER_NAME", System.Data.SqlDbType.VarChar).Value = gt.USER_NAME;
                cmd.Parameters.Add("@LOT_ID", System.Data.SqlDbType.VarChar).Value = gt.LOT_ID;
                cmd.Parameters.Add("@ACTION", System.Data.SqlDbType.VarChar).Value = gt.ACTION;
                cmd.Parameters.Add("@REMARKS", System.Data.SqlDbType.VarChar).Value = gt.REMARKS;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = gt.UPDATED_TIME;
                cmd.Parameters.Add("@WEEK", System.Data.SqlDbType.Int).Value = gt.WEEK;
                cmd.Parameters.Add("@MONTH", System.Data.SqlDbType.Int).Value = gt.MONTH;
                cmd.Parameters.Add("@YEAR", System.Data.SqlDbType.Int).Value = gt.YEAR;
                return cmd;
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static SqlCommand Insert(ObjectModule.Local.Tracking gt)
        {
            try
            {
                string sql = @"insert into Tracking
                               (PART_ID,SAPCODE,BATCH_NO,DESCRIPTION,DEPARTMENT,START_WEIGHT,CURRENT_WEIGHT,STATUS,EQUIP_ID,LOCID,THAWING_DATETIME,READY_DATETIME,
                                CAPACITY,EXPIRY_DATETIME,MF_EXPIRY_DATE,LOT_ID,DEVICE,EMPTY_SYRINGE_WEIGHT,USER_ID,USER_NAME,ACTION,REMARKS,UPDATED_TIME,WEEK,MONTH,YEAR) 
                                VALUES                  
                               (@PART_ID,@SAPCODE,@BATCH_NO,@DESCRIPTION,@DEPARTMENT,@START_WEIGHT,@CURRENT_WEIGHT,@STATUS,@EQUIP_ID,@LOCID,@THAWING_DATETIME,@READY_DATETIME,
                                @CAPACITY,@EXPIRY_DATETIME,@MF_EXPIRY_DATE,@LOT_ID,@DEVICE,@EMPTY_SYRINGE_WEIGHT,@USER_ID,@USER_NAME,@ACTION,@REMARKS,@UPDATED_TIME,@WEEK,@MONTH,@YEAR)";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@PART_ID", System.Data.SqlDbType.VarChar).Value = gt.PART_ID;
                cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = gt.SAPCODE;
                cmd.Parameters.Add("@BATCH_NO", System.Data.SqlDbType.VarChar).Value = gt.BATCH_NO;
                cmd.Parameters.Add("@DESCRIPTION", System.Data.SqlDbType.VarChar).Value = gt.DESCRIPTION;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = gt.DEPARTMENT;
                cmd.Parameters.Add("@START_WEIGHT", System.Data.SqlDbType.Float).Value = gt.START_WEIGHT;
                cmd.Parameters.Add("@CURRENT_WEIGHT", System.Data.SqlDbType.Float).Value = gt.CURRENT_WEIGHT;
                cmd.Parameters.Add("@CAPACITY", System.Data.SqlDbType.Int).Value = gt.CAPACITY;
                cmd.Parameters.Add("@STATUS", System.Data.SqlDbType.VarChar).Value = gt.STATUS;
                cmd.Parameters.Add("@EQUIP_ID", System.Data.SqlDbType.VarChar).Value = gt.EQUIP_ID;
                cmd.Parameters.Add("@LOCID", System.Data.SqlDbType.VarChar).Value = gt.LOCID;
                cmd.Parameters.Add("@THAWING_DATETIME", System.Data.SqlDbType.DateTime).Value = gt.THAWING_DATETIME;
                cmd.Parameters.Add("@READY_DATETIME", System.Data.SqlDbType.DateTime).Value = gt.READY_DATETIME;
                cmd.Parameters.Add("@EXPIRY_DATETIME", System.Data.SqlDbType.DateTime).Value = gt.EXPIRY_DATETIME;
                cmd.Parameters.Add("@MF_EXPIRY_DATE", System.Data.SqlDbType.Date).Value = gt.MF_EXPIRY_DATE;
                cmd.Parameters.Add("@LOT_ID", System.Data.SqlDbType.VarChar).Value = gt.LOT_ID;
                cmd.Parameters.Add("@DEVICE", System.Data.SqlDbType.VarChar).Value = gt.DEVICE;
                cmd.Parameters.Add("@EMPTY_SYRINGE_WEIGHT", System.Data.SqlDbType.Float).Value = gt.EMPTY_SYRINGE_WEIGHT;
                cmd.Parameters.Add("@USER_ID", System.Data.SqlDbType.VarChar).Value = gt.USER_ID;
                cmd.Parameters.Add("@USER_NAME", System.Data.SqlDbType.VarChar).Value = gt.USER_NAME;
                cmd.Parameters.Add("@ACTION", System.Data.SqlDbType.VarChar).Value = gt.ACTION;
                cmd.Parameters.Add("@REMARKS", System.Data.SqlDbType.VarChar).Value = gt.REMARKS;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = gt.UPDATED_TIME;
                cmd.Parameters.Add("@WEEK", System.Data.SqlDbType.Int).Value = gt.WEEK;
                cmd.Parameters.Add("@MONTH", System.Data.SqlDbType.Int).Value = gt.MONTH;
                cmd.Parameters.Add("@YEAR", System.Data.SqlDbType.Int).Value = gt.YEAR;
                return cmd;
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }
    }
}
