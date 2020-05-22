using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DB.Connection
{
    public class SqlServer
    {
        public static System.Data.SqlClient.SqlConnection EMS
        {
            get
            {
                //本地配置
                //return new System.Data.SqlClient.SqlConnection(@"Data Source=DESKTOP-BS114EV\SQLEXPRESS;Initial Catalog=P156-VISHAY-EMS;Persist Security Info=True;User ID=sa;Password=sa0");
                //return new System.Data.SqlClient.SqlConnection(@"Data Source=UBCT-YK\SQLEXPRESS;Initial Catalog=EMS;Persist Security Info=True;User ID=sa;Password=sa0");


                //dwyane - pc
                return new System.Data.SqlClient.SqlConnection(@"Data Source=127.0.0.1;Initial Catalog=P165-VISHAY-EMS;Persist Security Info=True;User ID=sa;Password=gaowei1124");


                //return new System.Data.SqlClient.SqlConnection(@"Data Source=P156-PC\SQLEXPRESS;Initial Catalog=EMS;Persist Security Info=True;User ID=sa;Password=sa0");
            }
        }
    }
}