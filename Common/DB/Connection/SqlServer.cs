using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Common.DB.Connection
{
    public class SqlServer
    {
        public static System.Data.SqlClient.SqlConnection EMS
        {
            get
            {
                string hostName = Dns.GetHostName();

                if (hostName == "DWYANE")
                {
                    return new System.Data.SqlClient.SqlConnection(@"Data Source=127.0.0.1;Initial Catalog=P156_EMS;Persist Security Info=True;User ID=sa;Password=sa0");
                }else
                {
                    return new System.Data.SqlClient.SqlConnection(@"Data Source=P156-PC\SQLEXPRESS;Initial Catalog=EMS;Persist Security Info=True;User ID=sa;Password=sa0");
                }                
            }
        }
    }
}