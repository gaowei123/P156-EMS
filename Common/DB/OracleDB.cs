using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace Common.DB
{
    public class OracleDB
    {
        public static DataTable GetData(OracleCommand cmd, OracleConnection cn)
        {
            cmd.Connection = cn;
            cn.Open();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
            return dt;

        }

        public static bool SetData(OracleCommand cmd, OracleConnection cn)
        {
            cmd.Connection = cn;
            cn.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();
                if (i >0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }

        }

        public static bool SetData_Rollback(List<OracleCommand> cmdlist, OracleConnection cn)
        {
            bool flag = false;
            cn.Open();
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                for (int i = 0; i <= cmdlist.Count - 1; i++)
                {
                    OracleCommand cmd = cmdlist[i];
                    cmd.Connection = cn;
                    cmd.Transaction = tran;
                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        tran = null;
                        flag = false;
                        break;
                    }
                    else
                    {
                        flag = true;
                        cmd.Parameters.Clear();
                    }
                }
                if (flag)
                {
                    tran.Commit();
                }
                return flag;
            }
            catch(OracleException ee)
            {
                throw ee;
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
