using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class User
    {
        public User()
        {
        }

        public User(DataRow x)
        {
            USER_ID = x["USER_ID"].ToString();
            USER_NAME = x["USER_NAME"].ToString();
            PASSWORD = x["PASSWORD"].ToString();
            DEPARTMENT = x["DEPARTMENT"].ToString();
            USER_GROUP = x["USER_GROUP"].ToString();
            UPDATED_BY = x["UPDATED_BY"].ToString();
            UPDATED_TIME = DateTime.Parse(x["UPDATED_TIME"].ToString());
            SHIFT = x["SHIFT"].ToString();
            FINGER_TEMPLATE = x["FINGER_TEMPLATE"].ToString();
            FINGER_TEMPLATE_1 = x["FINGER_TEMPLATE_1"].ToString();
        }

        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string PASSWORD  { get; set; }
        public string DEPARTMENT { get; set; }
        public string USER_GROUP { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime UPDATED_TIME { get; set; }
        public string SHIFT { get; set; }
        public string FINGER_TEMPLATE { get; set; }
        public string FINGER_TEMPLATE_1 { get; set; }
    }
}
