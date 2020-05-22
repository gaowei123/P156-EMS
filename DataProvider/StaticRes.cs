using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataProvider
{
    public class StaticRes
    {
        public static System.Data.SqlClient.SqlConnection Local = Common.DB.Connection.SqlServer.EMS;
    }
}
