using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataProvider.Local
{
    public class Equipment
    {
        public static DataTable Select(string Equip_ID)
        {
            try
            {
                string sql = "Select * from Equipment where 1=1 ";
                if (Equip_ID.Length > 0)
                    sql += " and EQUIP_ID=@EQUIP_ID ";
                sql += " order by EQUIP_ID ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                if (Equip_ID.Length > 0)
                    cmd.Parameters.Add("@EQUIP_ID", System.Data.SqlDbType.VarChar).Value = Equip_ID;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static DataTable Select_All()
        {
            try
            {
                string sql = "Select * from Equipment order by EQUIP_ID desc ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Update(ObjectModule.Local.Equipment ge)
        {
            try
            {
                string sql = @"Update Equipment set EQUIP_MAKER=@EQUIP_MAKER,EQUIP_MODEL=@EQUIP_MODEL,LOCID=@LOCID,
                               UPDATED_TIME =@UPDATED_TIME,UPDATED_BY=@UPDATED_BY where EQUIP_ID=@EQUIP_ID and DEPARTMENT=@DEPARTMENT ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@EQUIP_MAKER", System.Data.SqlDbType.VarChar).Value = ge.EQUIP_MAKER;
                cmd.Parameters.Add("@EQUIP_MODEL", System.Data.SqlDbType.VarChar).Value = ge.EQUIP_MODEL;
                cmd.Parameters.Add("@LOCID", System.Data.SqlDbType.VarChar).Value = ge.LOCID;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = ge.UPDATED_TIME;
                cmd.Parameters.Add("@UPDATED_BY", System.Data.SqlDbType.VarChar).Value = ge.UPDATED_BY;
                cmd.Parameters.Add("@EQUIP_ID", System.Data.SqlDbType.VarChar).Value = ge.EQUIP_ID;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = ge.DEPARTMENT;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Insert(ObjectModule.Local.Equipment ge)
        {
            try
            {
                string sql = @"insert into Equipment
                               (EQUIP_ID,DEPARTMENT,EQUIP_MAKER,EQUIP_MODEL,LOCID,UPDATED_TIME,UPDATED_BY) 
                                VALUES                  
                               (@EQUIP_ID,@DEPARTMENT,@EQUIP_MAKER,@EQUIP_MODEL,@LOCID,@UPDATED_TIME,@UPDATED_BY)";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@EQUIP_MAKER", System.Data.SqlDbType.VarChar).Value = ge.EQUIP_MAKER;
                cmd.Parameters.Add("@EQUIP_MODEL", System.Data.SqlDbType.VarChar).Value = ge.EQUIP_MODEL;
                cmd.Parameters.Add("@LOCID", System.Data.SqlDbType.VarChar).Value = ge.LOCID;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = ge.UPDATED_TIME;
                cmd.Parameters.Add("@UPDATED_BY", System.Data.SqlDbType.VarChar).Value = ge.UPDATED_BY;
                cmd.Parameters.Add("@EQUIP_ID", System.Data.SqlDbType.VarChar).Value = ge.EQUIP_ID;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = ge.DEPARTMENT;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Delete(string Equip_ID, string Department)
        {
            try
            {
                string sql = "Delete from Equipment where EQUIP_ID=@EQUIP_ID and DEPARTMENT=@DEPARTMENT ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@EQUIP_ID", System.Data.SqlDbType.VarChar).Value = Equip_ID;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = Department;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }
    }
}
