using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Temp_Inventory_Check
    {
        public Temp_Inventory_Check()
        {
        }

        public Temp_Inventory_Check(DataRow x)
        {
            Slot_No = x["Slot_No"].ToString();
            Slot_Level = x["Slot_Level"].ToString();
            DB_Part_ID = x["DB_Part_ID"].ToString();
            DB_Wire_Type = x["DB_Wire_Type"].ToString();
            DB_Status = x["DB_Status"].ToString();
            Physical_Status = x["Physical_Status"].ToString();
        }

        public string Slot_No { get; set; }
        public string Slot_Level { get; set; }
        public string DB_Part_ID { get; set; }
        public string DB_Wire_Type { get; set; }
        public string DB_Status { get; set; }
        public string Physical_Status { get; set; }
    }
}
