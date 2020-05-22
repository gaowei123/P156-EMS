using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace Common.DB
{
    public class SqlDB
    {
        public static int ExecuteSql(string strSql, SqlParameter[] parameters)
        {
            SqlConnection cn = new SqlConnection();

            try
            {
                cn = Common.DB.Connection.SqlServer.EMS;

                SqlCommand cmd = new SqlCommand(strSql, cn);

                foreach (SqlParameter par in parameters)
                {
                    if (par != null)
                    {
                        cmd.Parameters.Add(par);
                    }
                }

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                return i;
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



        public static System.Data.DataTable Query(string strSql)
        {
            SqlConnection cn = new SqlConnection();

            try
            {
                cn = Common.DB.Connection.SqlServer.EMS;
                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strSql;
                DataTable dt = GetData(cmd, cn);
                return dt;
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

        public static System.Data.DataTable Query(string strSql, SqlParameter[] parameters)
        {
            SqlConnection cn = new SqlConnection();

            try
            {
                cn = Common.DB.Connection.SqlServer.EMS;

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strSql;


                foreach (SqlParameter par in parameters)
                {
                    if (par != null)  //2017 02 13 remove null parameter
                    {
                        cmd.Parameters.Add(par);
                    }
                }


                DataTable dt = GetData(cmd, cn);
                return dt;
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


        public static System.Data.DataTable GetData(SqlCommand cmd, SqlConnection cn)
        {
            
            cmd.Connection = cn;
            DataTable dt = new DataTable();
            try
            {
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public static bool SetData(SqlCommand cmd, SqlConnection cn)
        {
            cmd.Connection = cn;
            try
            {
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
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

        public static bool SetData_Rollback(List<SqlCommand> cmdlist, SqlConnection cn)
        {
            bool flag = false;
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                for (int i = 0; i <= cmdlist.Count - 1; i++)
                {
                    SqlCommand cmd = cmdlist[i];
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
            catch (SqlException ee)
            {
                throw ee;
            }
            finally
            {
                cn.Close();
            }
        }




        public static SqlCommand GenerateCommand(string strSql, SqlParameter[] spParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strSql;
            foreach (SqlParameter par in spParameters)
            {
                if (par != null)  //2017 02 13 remove null parameter
                {
                    cmd.Parameters.Add(par);
                }
            }
            return cmd;
        }


    }
}
