using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ObjectModule;
using System.Data;

namespace DataProvider.Local
{
    public class Event
    {
        public static DataTable Select(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string sql = "select * from Event where UPDATED_TIME between @F and @T order by UPDATED_TIME desc ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@F", System.Data.SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@T", System.Data.SqlDbType.DateTime).Value = dateTo;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static DataTable Pareto(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string sql = @"Select EVENT_NAME AS Alarm_Code ,EVENT_MESSAGE as Alarm_Message,COUNT(EVENT_NAME) as Alarm_Frequency from Event 
                                WHERE EVENT_TYPE='Alarm' and UPDATED_TIME between @F and @T GROUP BY EVENT_NAME,EVENT_MESSAGE ORDER BY COUNT(EVENT_NAME) DESC ";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@F", System.Data.SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@T", System.Data.SqlDbType.DateTime).Value = dateTo;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);

            }
            catch(SqlException ee)
            {
                throw ee;
            }
        }

        public static DataTable Error_Trend(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string sql = @"select CONVERT(varchar(100),UPDATED_TIME,1) as Alarm_Date,COUNT(EVENT_NAME) as Alarm_Frequency from Event 
                                where EVENT_TYPE='Alarm' AND UPDATED_TIME between @F and @T GROUP BY CONVERT(varchar(100),UPDATED_TIME,1) ORDER BY CONVERT(varchar(100),UPDATED_TIME,1) ";
                  SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@F", System.Data.SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@T", System.Data.SqlDbType.DateTime).Value = dateTo;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);

            }
            catch(SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Insert(ObjectModule.Local.Event lh)
        {
            try
            {
                string sql = @"insert into Event
                               (EVENT_TYPE,EVENT_NAME,EVENT_MESSAGE,DEPARTMENT,SLOT_NO,PROCESS_CODE,PART_ID,USER_ID,UPDATED_TIME,WEEK,MONTH,YEAR)
                                VALUES                  
                               (@EVENT_TYPE,@EVENT_NAME,@EVENT_MESSAGE,@DEPARTMENT,@SLOT_NO,@PROCESS_CODE,@PART_ID,@USER_ID,@UPDATED_TIME,@WEEK,@MONTH,@YEAR)";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@EVENT_TYPE", System.Data.SqlDbType.VarChar).Value = lh.EVENT_TYPE;
                cmd.Parameters.Add("@EVENT_NAME", System.Data.SqlDbType.VarChar).Value = lh.EVENT_NAME;
                cmd.Parameters.Add("@EVENT_MESSAGE", System.Data.SqlDbType.VarChar).Value = lh.EVENT_MESSAGE;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = lh.DEPARTMENT;
                cmd.Parameters.Add("@SLOT_NO", System.Data.SqlDbType.VarChar).Value = lh.SLOT_NO;
                cmd.Parameters.Add("@PROCESS_CODE", System.Data.SqlDbType.VarChar).Value = lh.PROCESS_CODE;
                cmd.Parameters.Add("@PART_ID", System.Data.SqlDbType.VarChar).Value = lh.PART_ID;
                cmd.Parameters.Add("@USER_ID", System.Data.SqlDbType.VarChar).Value = lh.USER_ID;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = lh.UPDATED_TIME;
                cmd.Parameters.Add("@WEEK", System.Data.SqlDbType.Int).Value = lh.WEEK;
                cmd.Parameters.Add("@MONTH", System.Data.SqlDbType.Int).Value = lh.MONTH;
                cmd.Parameters.Add("@YEAR", System.Data.SqlDbType.Int).Value = lh.YEAR;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch(SqlException ee)
            {
                throw ee;
            }
        }
    }
}
