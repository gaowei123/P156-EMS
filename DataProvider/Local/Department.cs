using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataProvider.Local
{
    public class Department
    {
        public static DataTable Select()
        {
            try
            {
                string sql = "Select * from Department ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Insert(string Department)
        {
            try
            {
                string sql = @"insert into Department
                               (DEPARTMENT) 
                                VALUES                  
                               (@DEPARTMENT)";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = Department;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Delete(string Department)
        {
            try
            {
                string sql = "Delete from Department where DEPARTMENT=@DEPARTMENT ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
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
