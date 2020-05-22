using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataProvider.Local
{
    public class History
    {
        public static DataTable Select(DateTime dateFrom, DateTime dateTo,string Part_ID,string MC_ID,bool Return_With_Expiry )
        {
            try
            {
                if (Part_ID.Length > 0 || MC_ID.Length > 0)
                {
                    string sql = "Select * from History where 1=1 ";
                    if (Part_ID.Length > 0)
                        sql += " and PART_ID=@PART_ID ";
                    if(MC_ID.Length>0)
                        sql += " and EQUIP_ID=@EQUIP_ID ";
                    SqlCommand cmd = new SqlCommand(sql);
                    if(Part_ID.Length > 0)
                        cmd.Parameters.Add("@PART_ID", SqlDbType.VarChar).Value = dateFrom;
                    if(MC_ID.Length>0)
                        cmd.Parameters.Add("@EQUIP_ID", SqlDbType.VarChar).Value = dateTo;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                else
                {
                    string sql = "Select * from History where UPDATED_TIME between @F and @T ";
                    if (Return_With_Expiry)
                        sql += " and EXPIRY_DATETIME<UPDATED_TIME ";
                    sql += "order by UPDATED_TIME";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@F", SqlDbType.VarChar).Value = dateFrom;
                    cmd.Parameters.Add("@T", SqlDbType.VarChar).Value = dateTo;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static DataTable PartID_List(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string sql = "Select distinct(PART_ID) as PART_ID from History where UPDATED_TIME between @F and @T ";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@F", SqlDbType.VarChar).Value = dateFrom;
                cmd.Parameters.Add("@T", SqlDbType.VarChar).Value = dateTo;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static DataTable Select_Summary(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string sql = @"SELECT a.PART_ID,a.SAPCODE,a.BATCH_NO,a.DESCRIPTION,a.DEPARTMENT,a.LOT_ID,a.DEVICE,a.EQUIP_ID,a.LOCID,a.LOAD_WEIGHT,
                                b.CURRENT_WEIGHT as SCRAP_WEIGHT,a.CAPACITY,a.THAWING_DATETIME,a.READY_DATETIME,a.EXPIRY_DATETIME,a.MF_EXPIRY_DATE,
                                a.EMPTY_SYRINGE_WEIGHT,a.LOAD_USER,a.LOAD_USER_NAME,a.UNLOAD_USER,a.UNLOAD_USER_NAME,b.USER_ID as SCRAP_USER,
                                b.USER_NAME as SCRAP_USER_NAME,a.LOAD_TIME,a.UNLOAD_TIME,b.UPDATED_TIME as SCRAP_TIME FROM
                                (SELECT a.PART_ID,a.SAPCODE,a.BATCH_NO,a.DESCRIPTION,a.DEPARTMENT,b.LOT_ID,b.DEVICE,b.EQUIP_ID,b.LOCID,a.LOAD_WEIGHT,
                                a.CAPACITY,a.THAWING_DATETIME,a.READY_DATETIME,a.EXPIRY_DATETIME,a.MF_EXPIRY_DATE,a.EMPTY_SYRINGE_WEIGHT,
                                a.LOAD_USER,a.LOAD_USER_NAME,b.USER_ID as UNLOAD_USER,b.USER_NAME as UNLOAD_USER_NAME,
                                a.LOAD_TIME,b.UPDATED_TIME as UNLOAD_TIME from
                                (select PART_ID, SAPCODE,BATCH_NO,DESCRIPTION,DEPARTMENT,START_WEIGHT AS LOAD_WEIGHT,CAPACITY,
                                THAWING_DATETIME,READY_DATETIME,EXPIRY_DATETIME,MF_EXPIRY_DATE,EMPTY_SYRINGE_WEIGHT,USER_ID AS LOAD_USER,
                                USER_NAME AS LOAD_USER_NAME,UPDATED_TIME AS LOAD_TIME ,WEEK,MONTH,YEAR from EMS.dbo.History where ACTION='LOAD' and UPDATED_TIME between @F and @T) a left join
                                (select distinct(PART_ID),LOT_ID,DEVICE,EQUIP_ID,LOCID,USER_ID,USER_NAME,UPDATED_TIME from EMS.dbo.History where ACTION='UNLOAD'and UPDATED_TIME between @F and @T) b 
                                on a.PART_ID=b.PART_ID ) a left join
                                (select distinct(PART_ID),CURRENT_WEIGHT,USER_ID ,USER_NAME,UPDATED_TIME from EMS.dbo.History where ACTION='SCRAP'and UPDATED_TIME between @F and @T) b 
                                on a.PART_ID=b.PART_ID ";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@F", SqlDbType.VarChar).Value = dateFrom;
                    cmd.Parameters.Add("@T", SqlDbType.VarChar).Value = dateTo;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static DataTable Usage_Trend(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string sql = @"select CONVERT(varchar(100),UPDATED_TIME,1) as UPDATED_TIME,SUM(START_WEIGHT) AS WEIGHT from History where STATUS='IN_USED'
                               and START_WEIGHT=CURRENT_WEIGHT and UPDATED_TIME between @F and @T GROUP BY CONVERT(varchar(100),UPDATED_TIME,1) ORDER BY CONVERT(varchar(100),UPDATED_TIME,1) ";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@F", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@T", SqlDbType.DateTime).Value = dateTo;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static DataTable Shift_Report(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
//                string sql = @"SELECT a.SHIFT,a.UPDATED_TIME as Date ,a.EQUIP_ID,a.LOCID,SUM(a.CURRENT_LENGTH) as LENGTH,a.GW_TYPE as Type,a.DIAMETER,a.PACKAGE,count(*) as QTY FROM
//                               (SELECT EQUIP_ID,LOCID,CURRENT_LENGTH ,SHIFT,GW_TYPE,DIAMETER,PACKAGE,CONVERT(varchar(100), UPDATED_TIME, 23) as UPDATED_TIME 
//                               FROM dbo.SBMS_History WHERE ACTION='UNLOAD' and UPDATED_TIME between @F and @T )a 
//                               GROUP BY a.EQUIP_ID,a.SHIFT,a.GW_TYPE,a.DIAMETER,a.PACKAGE,a.DIAMETER,a.UPDATED_TIME,A.LOCID ";
//                SqlCommand cmd = new SqlCommand(sql);
//                cmd.Parameters.Add("@F", SqlDbType.DateTime).Value = dateFrom;
//                cmd.Parameters.Add("@T", SqlDbType.DateTime).Value = dateTo;
                return new DataTable(); // SBMS.Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static DataTable Graf_Report_DailyIssue(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
//                string sql = @"SELECT a.EQUIP_ID,SUM(a.CURRENT_LENGTH)as CURRENT_LENGTH,a.UPDATED_TIME FROM (SELECT EQUIP_ID,CURRENT_LENGTH,
//                               CONVERT(varchar(100), UPDATED_TIME, 23) as UPDATED_TIME FROM dbo.SBMS_History WHERE ACTION='UNLOAD'  
//                               and UPDATED_TIME between @F and @T)a group by EQUIP_ID,UPDATED_TIME ORDER BY UPDATED_TIME ";
//                SqlCommand cmd = new SqlCommand(sql);
//                cmd.Parameters.Add("@F", SqlDbType.DateTime).Value = dateFrom;
//                cmd.Parameters.Add("@T", SqlDbType.DateTime).Value = dateTo;
                return new DataTable(); // SBMS.Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static DataTable Graf_Report_DailyReturn(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
//                string sql = @"SELECT a.EQUIP_ID,SUM(a.CURRENT_LENGTH)as CURRENT_LENGTH,a.UPDATED_TIME FROM (SELECT EQUIP_ID,CURRENT_LENGTH,
//                               CONVERT(varchar(100), UPDATED_TIME, 23) as UPDATED_TIME FROM dbo.SBMS_History WHERE ACTION='REUSE'  
//                               and UPDATED_TIME between @F and @T)a group by EQUIP_ID,UPDATED_TIME ORDER BY UPDATED_TIME ";
//                SqlCommand cmd = new SqlCommand(sql);
//                cmd.Parameters.Add("@F", SqlDbType.DateTime).Value = dateFrom;
//                cmd.Parameters.Add("@T", SqlDbType.DateTime).Value = dateTo;
                return new DataTable(); //SBMS.Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
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
                string sql = @"insert into History
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
