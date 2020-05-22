using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataProvider.Local
{
    public class Slot_Position
    {
        public class Select
        {
            public static DataTable All()
            {
                try
                {
                    string sql = "select * from Slot_Position order by SLOT_ID,SLOT_INDEX ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    return Common.DB.SqlDB.GetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }
        }

        public class Update
        {
            public static bool By_SlotNo(int Slot_ID,int Slot_Index, int Position)
            {
                try
                {
                    string sql = "Update Slot_Position set POSITION=@POSITION where SLOT_ID=@SLOT_ID and SLOT_INDEX=@SLOT_INDEX ";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql);
                    cmd.Parameters.Add("@SLOT_ID", System.Data.SqlDbType.Int).Value = Slot_ID;
                    cmd.Parameters.Add("@SLOT_INDEX", System.Data.SqlDbType.Int).Value =Slot_Index;
                    cmd.Parameters.Add("@POSITION", System.Data.SqlDbType.Int).Value = Position;
                    return Common.DB.SqlDB.SetData(cmd, StaticRes.Local);
                }
                catch (SqlException ee)
                {
                    throw ee;
                }
            }
        }
    }
}
