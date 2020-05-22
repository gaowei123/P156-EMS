using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataProvider.Local
{
    public class Configure
    {
        public static DataTable Select()
        {
            try
            {
                string sql = "Select * from Configure order by NAME";
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }

        public static bool Update(ObjectModule.Local.Configure sc)
        {
            try
            {
                string sql = @"Update Configure set VALUE=@VALUE,UPDATED_TIME=@UPDATED_TIME,
                                USER_ID=@USER_ID,USER_GROUP=@USER_GROUP where NAME=@NAME ";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@VALUE", SqlDbType.VarChar).Value = sc.VALUE ;
                cmd.Parameters.Add("@UPDATED_TIME", SqlDbType.DateTime).Value = sc.UPDATED_TIME;
                cmd.Parameters.Add("@USER_ID", SqlDbType.VarChar).Value = sc.USER_ID;
                cmd.Parameters.Add("@USER_GROUP", SqlDbType.VarChar).Value = sc.USER_GROUP;
                cmd.Parameters.Add("@NAME", SqlDbType.VarChar).Value = sc.NAME;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }
    }
}
