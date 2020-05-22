using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Binning
    {
        public Binning()
        {
        }

        public Binning(DataRow x)
        {
            SLOT_ID = int.Parse(x["SLOT_ID"].ToString());
            SLOT_INDEX = int.Parse(x["SLOT_INDEX"].ToString());
            PART_ID = x["PART_ID"].ToString();
            SAPCODE = x["SAPCODE"].ToString();
            DESCRIPTION = x["DESCRIPTION"].ToString();
            BATCH_NO = x["BATCH_NO"].ToString();
            CAPACITY = int.Parse(x["CAPACITY"].ToString());
            STATUS = x["STATUS"].ToString();
            START_WEIGHT = float.Parse(x["START_WEIGHT"].ToString());
            CURRENT_WEIGHT = float.Parse(x["CURRENT_WEIGHT"].ToString());
            THAWING_DATETIME = DateTime.Parse(x["THAWING_DATETIME"].ToString());
            READY_DATETIME = DateTime.Parse(x["READY_DATETIME"].ToString());
            EXPIRY_DATETIME = DateTime.Parse(x["EXPIRY_DATETIME"].ToString());
            MF_EXPIRY_DATE = DateTime.Parse(x["MF_EXPIRY_DATE"].ToString());
            USER_ID = x["USER_ID"].ToString();
            USER_NAME = x["USER_NAME"].ToString();
            USER_GROUP = x["USER_GROUP"].ToString();
            UPDATED_TIME = DateTime.Parse(x["UPDATED_TIME"].ToString());
            DEPARTMENT = x["DEPARTMENT"].ToString();
        }

        public int SLOT_ID { get; set; }
        public int SLOT_INDEX { get; set; }
        public string PART_ID { get; set; }
        public string SAPCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string BATCH_NO { get; set; }
        public int CAPACITY { get; set; }
        public string STATUS { get; set; }
        public float START_WEIGHT { get; set; }
        public float CURRENT_WEIGHT { get; set; }
        public DateTime THAWING_DATETIME { get; set; }
        public DateTime READY_DATETIME { get; set; }
        public DateTime EXPIRY_DATETIME { get; set; }
        public DateTime MF_EXPIRY_DATE { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_GROUP { get; set; }  
        public string DEPARTMENT { get; set; }
        public DateTime UPDATED_TIME { get; set; }       
    }
}
