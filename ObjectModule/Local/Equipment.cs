using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Equipment
    {
        public Equipment()
        {
        }

        public Equipment(DataRow x)
        {
            EQUIP_ID = x["EQUIP_ID"].ToString();
            DEPARTMENT = x["DEPARTMENT"].ToString();
            EQUIP_MAKER = x["EQUIP_MAKER"].ToString();
            EQUIP_MODEL = x["EQUIP_MODEL"].ToString();
            LOCID = x["LOCID"].ToString();
            UPDATED_BY = x["UPDATED_BY"].ToString();
            UPDATED_TIME = DateTime.Parse(x["UPDATED_TIME"].ToString());
        }

        public string EQUIP_ID { get; set; }
        public string DEPARTMENT { get; set; }
        public string EQUIP_MAKER { get; set; }
        public string EQUIP_MODEL { get; set; }
        public string LOCID { get; set; }
        public DateTime UPDATED_TIME { get; set; }
        public string UPDATED_BY { get; set; }
    }
}
