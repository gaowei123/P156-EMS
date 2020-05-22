using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Tracking
    {
        public Tracking()
        {
        }

        public Tracking(DataRow x)
        {
            PART_ID = x["PART_ID"].ToString();
            SAPCODE = x["SAPCODE"].ToString();
            BATCH_NO = x["BATCH_NO"].ToString();
            DESCRIPTION = x["DESCRIPTION"].ToString();
            DEPARTMENT = x["DEPARTMENT"].ToString();
            START_WEIGHT = float.Parse(x["START_WEIGHT"].ToString());
            CURRENT_WEIGHT = float.Parse(x["CURRENT_WEIGHT"].ToString());
            CAPACITY = int.Parse(x["CAPACITY"].ToString());
            STATUS = x["STATUS"].ToString();
            EQUIP_ID = x["EQUIP_ID"].ToString();
            LOCID = x["LOCID"].ToString();
            THAWING_DATETIME = DateTime.Parse(x["THAWING_DATETIME"].ToString());
            READY_DATETIME = DateTime.Parse(x["READY_DATETIME"].ToString());
            EXPIRY_DATETIME = DateTime.Parse(x["EXPIRY_DATETIME"].ToString());
            MF_EXPIRY_DATE = DateTime.Parse(x["MF_EXPIRY_DATE"].ToString());
            LOT_ID = x["LOT_ID"].ToString();
            DEVICE = x["DEVICE"].ToString();
            EMPTY_SYRINGE_WEIGHT = float.Parse(x["EMPTY_SYRINGE_WEIGHT"].ToString());
            USER_ID = x["USER_ID"].ToString();
            USER_NAME = x["USER_NAME"].ToString();
            ACTION = x["ACTION"].ToString();
            REMARKS = x["REMARKS"].ToString();
            UPDATED_TIME = DateTime.Parse(x["UPDATED_TIME"].ToString());
            WEEK = int.Parse(x["WEEK"].ToString());
            MONTH = int.Parse(x["MONTH"].ToString());
            YEAR = int.Parse(x["YEAR"].ToString());
        }

        public string PART_ID { get; set; }
        public string SAPCODE { get; set; }
        public string BATCH_NO { get; set; }
        public string DESCRIPTION { get; set; }
        public string DEPARTMENT { get; set; }
        public float START_WEIGHT { get; set; }
        public float CURRENT_WEIGHT { get; set; }
        public int CAPACITY { get; set; }
        public string STATUS { get; set; }
        public string EQUIP_ID { get; set; }
        public string LOCID { get; set; }
        public DateTime THAWING_DATETIME { get; set; }
        public DateTime READY_DATETIME { get; set; }
        public DateTime EXPIRY_DATETIME { get; set; }
        public DateTime MF_EXPIRY_DATE { get; set; }
        public string LOT_ID { get; set; }
        public string DEVICE { get; set; }
        public float EMPTY_SYRINGE_WEIGHT { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string ACTION { get; set; }
        public string REMARKS { get; set; }
        public DateTime UPDATED_TIME { get; set; }
        public int WEEK { get; set; }
        public int MONTH { get; set; }
        public int YEAR { get; set; }
    }
}
