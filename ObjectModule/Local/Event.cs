using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Event
    {
        public Event()
        {
        }
        public Event(DataRow x)
        {
            EVENT_TYPE = x["EVENT_TYPE"].ToString();
            EVENT_NAME = x["EVENT_NAME"].ToString();
            EVENT_MESSAGE = x["EVENT_MESSAGE"].ToString();
            DEPARTMENT = x["DEPARTMENT"].ToString();
            SLOT_NO = x["SLOT_NO"].ToString();
            PROCESS_CODE = x["PROCESS_CODE"].ToString();
            PART_ID = x["PART_ID"].ToString();
            USER_ID = x["USER_ID"].ToString();
            UPDATED_TIME = DateTime.Parse(x["UPDATED_TIME"].ToString());
            WEEK = int.Parse(x["WEEK"].ToString());
            MONTH = int.Parse(x["MONTH"].ToString());
            YEAR = int.Parse(x["YEAR"].ToString());
        }

        public string EVENT_TYPE { get; set; }
        public string EVENT_NAME { get; set; }
        public string EVENT_MESSAGE { get; set; }
        public string DEPARTMENT { get; set; }
        public string SLOT_NO { get; set; }
        public string PROCESS_CODE { get; set; }
        public string PART_ID { get; set; }
        public string USER_ID { get; set; }
        public DateTime UPDATED_TIME { get; set; }
        public int WEEK { get; set; }
        public int MONTH { get; set; }
        public int YEAR { get; set; }
    }
}
