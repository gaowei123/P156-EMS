using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Sapcode
    {
        public Sapcode()
        {
        }

        public Sapcode(DataRow x)
        {
            SAPCODE = x["SAPCODE"].ToString();
            DESCRIPTION = x["DESCRIPTION"].ToString();
            THAWING_TIME = int.Parse(x["THAWING_TIME"].ToString());
            USAGE_LIFE = int.Parse(x["USAGE_LIFE"].ToString());
            DEPARTMENT = x["DEPARTMENT"].ToString();
            NEW_MAX_WEIGHT = float.Parse(x["NEW_MAX_WEIGHT"].ToString());
            NEW_MIN_WEIGHT = float.Parse(x["NEW_MIN_WEIGHT"].ToString());
            EMPTY_SYRINGE_WEIGHT = float.Parse(x["EMPTY_SYRINGE_WEIGHT"].ToString());
            SCRAP_WEIGHT = float.Parse(x["SCRAP_WEIGHT"].ToString());
            CAPACITY = int.Parse(x["CAPACITY"].ToString());
            ON_HOLD = x["ON_HOLD"].ToString();
            UPDATED_TIME = DateTime.Parse(x["UPDATED_TIME"].ToString());
            UPDATED_BY = x["UPDATED_BY"].ToString();
        }

        public string SAPCODE { get; set; }
        public string DESCRIPTION { get; set; }
        public int THAWING_TIME { get; set; }
        public int USAGE_LIFE { get; set; }
        public string DEPARTMENT { get; set; }
        public float NEW_MIN_WEIGHT { get; set; }
        public float NEW_MAX_WEIGHT { get; set; }
        public float EMPTY_SYRINGE_WEIGHT { get; set; }
        public float SCRAP_WEIGHT { get; set; }
        public int CAPACITY { get; set; }
        public string ON_HOLD { get; set; }
        public DateTime UPDATED_TIME { get; set; }
        public string UPDATED_BY { get; set; }
    }
}
