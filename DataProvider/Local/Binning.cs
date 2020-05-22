using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataProvider.Local
{
    public class Binning
    {
        public class Select
        {
            public static DataTable Search_Inventory()
            {
                try
                {
                    string sql = @"Select COUNT(SAPCODE) QTY,SAPCODE,STATUS,DEPARTMENT from Binning where STATUS in ('NEW','REUSE') group by SAPCODE,STATUS,DEPARTMENT";
                    SqlCommand cmd = new SqlCommand(sql);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Search_bySlot(int Slot_ID, int Slot_Index)
            {
                try
                {
                    string sql = @"Select * from Binning where SLOT_INDEX=@SLOT_INDEX and SLOT_ID=@SLOT_ID and STATUS in ('NEW','REUSE') order by SLOT_ID";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@SLOT_ID", System.Data.SqlDbType.Int).Value = Slot_ID;
                    cmd.Parameters.Add("@SLOT_INDEX", System.Data.SqlDbType.Int).Value = Slot_Index;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Search_Inventory_SlotNo(int Slot_ID)
            {
                try
                {
                    string sql = @"Select * from Binning where SLOT_ID=@SLOT_ID order by SLOT_INDEX";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@SLOT_ID", System.Data.SqlDbType.Int).Value = Slot_ID;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static int Search_Empty()
            {
                try
                {
                    string sql = "select COUNT(STATUS) QTY from BINNING where STATUS ='EMPTY'";
                    SqlCommand cmd = new SqlCommand(sql);
                    int count = Int32.Parse(Common.DB.SqlDB.GetData(cmd, StaticRes.Local).Rows[0]["QTY"].ToString());
                    return count;
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Search_Data()
            {
                try
                {
                    string sql = @"select * from BINNING where STATUS in ('NEW','REUSE') order by SLOT_ID";
                    SqlCommand cmd = new SqlCommand(sql);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }



            public static int Empty_Positon_Count()
            {
                try
                {
                    string sql = "select * from Binning where STATUS='EMPTY' ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    DataTable dt = Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                    if (dt.Rows.Count > 0)
                        return dt.Rows.Count;
                    else
                        return 0;
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable All()
            {
                try
                {
                    string sql = "select * from Binning order by SLOT_ID ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch(SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable By_New_Sapcode(string Sapcode,string Department)
            {
                try
                {
                    string sql = @"select * from Binning where SAPCODE=@SAPCODE and DEPARTMENT=@DEPARTMENT and STATUS ='NEW' 
                                   and EXPIRY_DATETIME>@EXPIRY_DATETIME and READY_DATETIME<=@READY_DATETIME  order by UPDATED_TIME ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = Sapcode;
                    cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = Department;
                    cmd.Parameters.Add("@READY_DATETIME", System.Data.SqlDbType.DateTime).Value = System.DateTime.Now;
                    cmd.Parameters.Add("@EXPIRY_DATETIME", System.Data.SqlDbType.DateTime).Value = System.DateTime.Now;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable By_SapcodeBatchNo(string Sapcode, string Batch_No)
            {
                try
                {
                    string sql = @"select * from Binning where SAPCODE=@SAPCODE and STATUS ='NEW' and BATCH_NO=@BATCH_NO ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = Sapcode;
                    cmd.Parameters.Add("@BATCH_NO", System.Data.SqlDbType.VarChar).Value = Batch_No;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable By_Reuse_Sapcode(string Sapcode, string Department)
            {
                try
                {
                    string sql = @"select * from Binning where SAPCODE=@SAPCODE and DEPARTMENT=@DEPARTMENT and STATUS ='REUSE' 
                                   and EXPIRY_DATETIME>@EXPIRY_DATETIME and READY_DATETIME<=@READY_DATETIME order by UPDATED_TIME ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@SAPCODE", System.Data.SqlDbType.VarChar).Value = Sapcode;
                    cmd.Parameters.Add("@DEPARTMENT", System.Data.SqlDbType.VarChar).Value = Department;
                    cmd.Parameters.Add("@EXPIRY_DATETIME", System.Data.SqlDbType.DateTime).Value = System.DateTime.Now;
                    cmd.Parameters.Add("@READY_DATETIME", System.Data.SqlDbType.DateTime).Value = System.DateTime.Now;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Bin_WithMateria()
            {
                try
                {
                    string sql = "select * from BINNING where STATUS in ('NEW','REUSE') order by SLOT_ID ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Search_Empty_Position(int Capacity)
            {
                try
                {
                    string sql = "select * from BINNING where STATUS='EMPTY' and CAPACITY=@CAPACITY order by SLOT_ID ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@CAPACITY", SqlDbType.Int).Value = Capacity;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch(SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Empty_Slot_No()
            {
                try
                {
                    string sql = "select distinct(SLOT_ID) from BINNING WHERE STATUS='EMPTY' ORDER BY SLOT_ID ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Inventory()
            {
                try
                {
                    string sql = "select SAPCODE , count(*) as QTY from BINNING where STATUS IN ('NEW','REUSE') Group by SAPCODE  ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Material_Positon_Count()
            {
                try
                {
                    string sql = "select count(*) as QTY from BINNING where STATUS in ('NEW','REUSE') ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }

            public static DataTable Mateiral_in_Index(int Slot_Index)
            {
                try
                {
                    string sql = "select * from BINNING where STATUS in ('NEW','REUSE') and SLOT_INDEX=@SLOT_INDEX ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@SLOT_INDEX", SqlDbType.Int).Value = Slot_Index;
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }
        }

        public static bool Update_Capacity(int Slot_Index, int Capacity)
        {
            try
            {
                string sql = "update Binning set CAPACITY=@CAPAICITY where SLOT_INDEX=@SLOT_INDEX ";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@SLOT_INDEX", SqlDbType.Int).Value = Slot_Index;
                cmd.Parameters.Add("@CAPAICITY", SqlDbType.Int).Value = Capacity;
                return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);

            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static SqlCommand Update(ObjectModule.Local.Binning gb)
        {
            try
            {
                string sql = @"Update Binning set PART_ID=@PART_ID,SAPCODE=@SAPCODE,DESCRIPTION=@DESCRIPTION,BATCH_NO=@BATCH_NO,START_WEIGHT=@START_WEIGHT,CURRENT_WEIGHT=@CURRENT_WEIGHT,
                               THAWING_DATETIME=@THAWING_DATETIME,READY_DATETIME=@READY_DATETIME,EXPIRY_DATETIME=@EXPIRY_DATETIME,MF_EXPIRY_DATE=@MF_EXPIRY_DATE,USER_ID=@USER_ID,USER_NAME=@USER_NAME,
                               USER_GROUP=@USER_GROUP,UPDATED_TIME=@UPDATED_TIME,STATUS=@STATUS,DEPARTMENT=@DEPARTMENT where SLOT_ID=@SLOT_ID and SLOT_INDEX=@SLOT_INDEX ";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@SLOT_ID", SqlDbType.Int).Value = gb.SLOT_ID;
                cmd.Parameters.Add("@SLOT_INDEX", SqlDbType.Int).Value = gb.SLOT_INDEX;
                cmd.Parameters.Add("@PART_ID", SqlDbType.VarChar).Value = gb.PART_ID;
                cmd.Parameters.Add("@SAPCODE", SqlDbType.VarChar).Value = gb.SAPCODE;
                cmd.Parameters.Add("@DESCRIPTION", SqlDbType.VarChar).Value = gb.DESCRIPTION;
                cmd.Parameters.Add("@BATCH_NO", SqlDbType.VarChar).Value = gb.BATCH_NO;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = gb.STATUS;
                cmd.Parameters.Add("@START_WEIGHT", SqlDbType.Float).Value = gb.START_WEIGHT;
                cmd.Parameters.Add("@CURRENT_WEIGHT", SqlDbType.Float).Value = gb.CURRENT_WEIGHT;
                cmd.Parameters.Add("@THAWING_DATETIME", SqlDbType.DateTime).Value = gb.THAWING_DATETIME;
                cmd.Parameters.Add("@READY_DATETIME", SqlDbType.DateTime).Value = gb.READY_DATETIME;
                cmd.Parameters.Add("@EXPIRY_DATETIME", SqlDbType.DateTime).Value = gb.EXPIRY_DATETIME;
                cmd.Parameters.Add("@MF_EXPIRY_DATE", SqlDbType.Date).Value = gb.MF_EXPIRY_DATE;
                cmd.Parameters.Add("@USER_ID", SqlDbType.VarChar).Value = gb.DEPARTMENT;
                cmd.Parameters.Add("@USER_NAME", SqlDbType.VarChar).Value = gb.USER_NAME;
                cmd.Parameters.Add("@USER_GROUP", SqlDbType.VarChar).Value = gb.USER_GROUP;
                cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = gb.DEPARTMENT ;
                cmd.Parameters.Add("@UPDATED_TIME", SqlDbType.DateTime).Value = gb.UPDATED_TIME;
                return cmd;
            }
            catch (SqlException ee)
            {
                throw ee;
            }
        }
    }
}
