using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Configure
    {
        public Configure()
        {
        }

        public Configure(DataRow x)
        {
            CATEGORY = x["CATEGORY"].ToString();
            NAME = x["NAME"].ToString();
            VALUE = x["VALUE"].ToString();
            UNIT = x["UNIT"].ToString();
            UPDATED_TIME = DateTime.Parse(x["UPDATED_TIME"].ToString());
            USER_ID = x["USER_ID"].ToString();
            USER_GROUP = x["USER_GROUP"].ToString();
            DEFAULT_VALUE = x["DEFAULT_VALUE"].ToString();
        }

        public string CATEGORY { get; set; }
        public string NAME { get; set; }
        public string VALUE { get; set; }
        public string UNIT { get; set; }
        public DateTime UPDATED_TIME { get; set; }
        public string USER_ID { get; set; }
        public string USER_GROUP { get; set; }
        public string DEFAULT_VALUE { get; set; }
    }
}
