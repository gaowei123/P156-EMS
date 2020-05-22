using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataProvider.Local
{
    public class User
    {
        public static DataTable Select(string User_ID)
        {
            try
            {
                string sql = "Select * from User_DB where 1=1 ";
                if (User_ID.Length > 0)
                    sql += " and USER_ID=@USER_ID ";
                sql += " order by USER_ID ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                if (User_ID.Length > 0)
                    cmd.Parameters.Add("@USER_ID", System.Data.SqlDbType.VarChar).Value = User_ID;
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Update(ObjectModule.Local.User gu)
        {
            try
            {
                string sql = @"Update User_DB set USER_NAME=@USER_NAME,PASSWORD=@PASSWORD,USER_GROUP=@USER_GROUP,SHIFT=@SHIFT,
                               UPDATED_TIME =@UPDATED_TIME,UPDATED_BY=@UPDATED_BY,FINGER_TEMPLATE=@FINGER_TEMPLATE,FINGER_TEMPLATE_1=@FINGER_TEMPLATE_1 where USER_ID=@USER_ID and DEPARTMENT=@DEPARTMENT ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@USER_NAME", System.Data.SqlDbType.VarChar).Value = gu.USER_NAME ;
                cmd.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar).Value = gu.PASSWORD;
                cmd.Parameters.Add("@USER_GROUP", System.Data.SqlDbType.VarChar).Value = gu.USER_GROUP;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = System.DateTime.Now;
                cmd.Parameters.Add("@UPDATED_BY", System.Data.SqlDbType.VarChar).Value = gu.UPDATED_BY;
                cmd.Parameters.Add("@USER_ID", System.Data.SqlDbType.VarChar).Value = gu.USER_ID;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = gu.DEPARTMENT;
                cmd.Parameters.Add("@SHIFT", System.Data.SqlDbType.VarChar).Value = gu.SHIFT;
                cmd.Parameters.Add("@FINGER_TEMPLATE", System.Data.SqlDbType.VarChar).Value = gu.FINGER_TEMPLATE;
                cmd.Parameters.Add("@FINGER_TEMPLATE_1", System.Data.SqlDbType.VarChar).Value = gu.FINGER_TEMPLATE_1;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Insert(ObjectModule.Local.User gu)
        {
            try
            {
                string sql = @"insert into User_DB
                               (USER_ID,USER_NAME,PASSWORD,USER_GROUP,UPDATED_TIME,UPDATED_BY,DEPARTMENT,SHIFT,FINGER_TEMPLATE,FINGER_TEMPLATE_1) 
                                VALUES                  
                               (@USER_ID,@USER_NAME,@PASSWORD,@USER_GROUP,@UPDATED_TIME,@UPDATED_BY,@DEPARTMENT,@SHIFT,@FINGER_TEMPLATE,@FINGER_TEMPLATE_1)";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@USER_NAME", System.Data.SqlDbType.VarChar).Value = gu.USER_NAME;
                cmd.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar).Value = gu.PASSWORD;
                cmd.Parameters.Add("@USER_GROUP", System.Data.SqlDbType.VarChar).Value = gu.USER_GROUP;
                cmd.Parameters.Add("@UPDATED_TIME", System.Data.SqlDbType.DateTime).Value = System.DateTime.Now;
                cmd.Parameters.Add("@UPDATED_BY", System.Data.SqlDbType.VarChar).Value = gu.UPDATED_BY;
                cmd.Parameters.Add("@USER_ID", System.Data.SqlDbType.VarChar).Value = gu.USER_ID;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = gu.DEPARTMENT;
                cmd.Parameters.Add("@SHIFT", System.Data.SqlDbType.VarChar).Value = gu.SHIFT;
                cmd.Parameters.Add("@FINGER_TEMPLATE", System.Data.SqlDbType.VarChar).Value = gu.FINGER_TEMPLATE;
                cmd.Parameters.Add("@FINGER_TEMPLATE_1", System.Data.SqlDbType.VarChar).Value = gu.FINGER_TEMPLATE_1;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch(SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Delete(string User_ID,string Department)
        {
            try
            {
                string sql = "Delete from User_DB where USER_ID=@USER_ID and DEPARTMENT=@DEPARTMENT ";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                cmd.Parameters.Add("@USER_ID", System.Data.SqlDbType.VarChar).Value = User_ID;
                cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = Department;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch(SqlException ee)
            {
                throw ee;
            }
        }
    }
}
