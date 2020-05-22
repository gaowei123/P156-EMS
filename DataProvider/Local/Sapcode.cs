using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataProvider.Local
{
    public class Sapcode
    {
        public static DataTable Select(string Sapcode)
        {
            try
            {
                string sql = "Select * from Sapcode where 1=1 ";
                if (Sapcode.Length > 0)
                    sql += " and SAPCODE=@SAPCODE ";
                sql += " order by SAPCODE ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                if (Sapcode.Length > 0)
                    cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = Sapcode;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static DataTable Sap_Infor(string Sapcode, string Department)
        {
            try
            {
                string sql = "Select * from Sapcode where SAPCODE=@SAPCODE and DEPARTMENT=@DEPARTMENT ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = Sapcode;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = Department;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Delete(string Sapcode, string Department)
        {
            try
            {
                string sql = "Delete Sapcode where SAPCODE=@SAPCODE and DEPARTMENT=@DEPARTMENT ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = Sapcode;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = Department;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Update(ObjectModule.Local.Sapcode gs)
        {
            try
            {
                string sql = @"Update Sapcode set DESCRIPTION=@DESCRIPTION,THAWING_TIME=@THAWING_TIME,USAGE_LIFE=@USAGE_LIFE,
                               NEW_MAX_WEIGHT=@NEW_MAX_WEIGHT, NEW_MIN_WEIGHT=@NEW_MIN_WEIGHT,EMPTY_SYRINGE_WEIGHT=@EMPTY_SYRINGE_WEIGHT,
                               SCRAP_WEIGHT=@SCRAP_WEIGHT,CAPACITY=@CAPACITY,ON_HOLD=@ON_HOLD,UPDATED_TIME=@UPDATED_TIME,UPDATED_BY=@UPDATED_BY
                               where SAPCODE=@SAPCODE and DEPARTMENT=@DEPARTMENT";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = gs.SAPCODE;
                cmd.Parameters.Add("@DESCRIPTION", System.Data.SqlDbType.VarChar).Value = gs.DESCRIPTION;
                cmd.Parameters.Add("@THAWING_TIME", System.Data.SqlDbType.Int).Value = gs.THAWING_TIME;
                cmd.Parameters.Add("@USAGE_LIFE", System.Data.SqlDbType.Int).Value = gs.USAGE_LIFE;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = gs.DEPARTMENT;
                cmd.Parameters.Add("@NEW_MIN_WEIGHT", System.Data.SqlDbType.Float).Value = gs.NEW_MIN_WEIGHT;
                cmd.Parameters.Add("@NEW_MAX_WEIGHT", System.Data.SqlDbType.Float).Value = gs.NEW_MAX_WEIGHT;
                cmd.Parameters.Add("@EMPTY_SYRINGE_WEIGHT", System.Data.SqlDbType.Float).Value = gs.EMPTY_SYRINGE_WEIGHT;
                cmd.Parameters.Add("@SCRAP_WEIGHT", System.Data.SqlDbType.Float).Value = gs.SCRAP_WEIGHT;
                cmd.Parameters.Add("@CAPACITY", System.Data.SqlDbType.Int).Value = gs.CAPACITY;
                cmd.Parameters.Add("@ON_HOLD", System.Data.SqlDbType.VarChar).Value = gs.ON_HOLD;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = gs.UPDATED_TIME;
                cmd.Parameters.Add("@UPDATED_BY", System.Data.SqlDbType.VarChar).Value = gs.UPDATED_BY;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Insert(ObjectModule.Local.Sapcode gs)
        {
            try
            {
                string sql = @"insert into Sapcode
                               (SAPCODE,DESCRIPTION,THAWING_TIME,USAGE_LIFE,DEPARTMENT,NEW_MIN_WEIGHT,NEW_MAX_WEIGHT,
                                EMPTY_SYRINGE_WEIGHT,SCRAP_WEIGHT,CAPACITY,ON_HOLD,UPDATED_TIME,UPDATED_BY)
                                VALUES                  
                               (@SAPCODE,@DESCRIPTION,@THAWING_TIME,@USAGE_LIFE,@DEPARTMENT,@NEW_MIN_WEIGHT,@NEW_MAX_WEIGHT,
                                @EMPTY_SYRINGE_WEIGHT,@SCRAP_WEIGHT,@CAPACITY,@ON_HOLD,@UPDATED_TIME,@UPDATED_BY)";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = gs.SAPCODE;
                cmd.Parameters.Add("@DESCRIPTION", System.Data.SqlDbType.VarChar).Value = gs.DESCRIPTION;
                cmd.Parameters.Add("@THAWING_TIME", System.Data.SqlDbType.Int).Value = gs.THAWING_TIME;
                cmd.Parameters.Add("@USAGE_LIFE", System.Data.SqlDbType.Int).Value = gs.USAGE_LIFE;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = gs.DEPARTMENT;
                cmd.Parameters.Add("@NEW_MIN_WEIGHT", System.Data.SqlDbType.Float).Value = gs.NEW_MIN_WEIGHT;
                cmd.Parameters.Add("@NEW_MAX_WEIGHT", System.Data.SqlDbType.Float).Value = gs.NEW_MAX_WEIGHT;
                cmd.Parameters.Add("@EMPTY_SYRINGE_WEIGHT", System.Data.SqlDbType.Float).Value = gs.EMPTY_SYRINGE_WEIGHT;
                cmd.Parameters.Add("@SCRAP_WEIGHT", System.Data.SqlDbType.Float).Value = gs.SCRAP_WEIGHT;
                cmd.Parameters.Add("@CAPACITY", System.Data.SqlDbType.Int).Value = gs.CAPACITY;
                cmd.Parameters.Add("@ON_HOLD", System.Data.SqlDbType.VarChar).Value = gs.ON_HOLD;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = gs.UPDATED_TIME;
                cmd.Parameters.Add("@UPDATED_BY", System.Data.SqlDbType.VarChar).Value = gs.UPDATED_BY;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }
    }
}