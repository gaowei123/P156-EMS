using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Globalization;

namespace Common.DB
{
   public class OleDB
    {
        public static DataTable GetData(OleDbCommand cmd, OleDbConnection cn)
        {
            cmd.Connection = cn;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            
            dt.Locale = CultureInfo.InvariantCulture;
            try
            {
                da.Fill(dt);
            }
            catch
            {
                throw;
            }

            return dt;

        }

        public static void SetData(OleDbCommand  cmd, OleDbConnection cn)
        {
            cmd.Connection = cn;
            cn.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                cn.Close();
            }

        }
    }
}
