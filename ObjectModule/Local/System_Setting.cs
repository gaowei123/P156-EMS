using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class System_Setting
    {
        public System_Setting()
        {
        }

        public System_Setting(DataRow x)
        {
            ROTARY_HOMING_PITCH = int.Parse(x["ROTARY_HOMING_PITCH"].ToString());
            VERTICAL_HOMING_PITCH = int.Parse(x["VERTICAL_HOMING_PITCH"].ToString());
            INLINE_SCANNER_POSITION = int.Parse(x["INLINE_SCANNER_POSITION"].ToString());
            INLINE_SCANNER_ROTARY_PITCH = int.Parse(x["INLINE_SCANNER_ROTARY_PITCH"].ToString());
            EMAIL_SERVER_IP_ADDRESS = x["EMAIL_SERVER_IP_ADDRESS"].ToString();
            EMAIL_SERVER_PORT = x["EMAIL_SERVER_PORT"].ToString();
            EMAIL_SEND_TO_ADDRESS = x["EMAIL_SEND_TO_ADDRESS"].ToString();
            EMAIL_SEND_FROM_ADDRESS = x["EMAIL_SEND_FROM_ADDRESS"].ToString();
            EMAIL_USER_ACCOUNT = x["EMAIL_USER_ACCOUNT"].ToString();
            EMAIL_USER_PASSWORD = x["EMAIL_USER_PASSWORD"].ToString();
            EMAIL_SENDOUT_FREQUENCY = x["EMAIL_SENDOUT_FREQUENCY"].ToString();
            PRINT_DURING_LOAD = x["PRINT_DURING_LOAD"].ToString();
            PRINT_DURING_UNLOAD = x["PRINT_DURING_UNLOAD"].ToString();
            SCRAP_BIN_CAPACITY = int.Parse(x["SCRAP_BIN_CAPACITY"].ToString());
            SBMS_ID = x["SBMS_ID"].ToString();
        }

        public int ROTARY_HOMING_PITCH { get; set; }
        public int VERTICAL_HOMING_PITCH { get; set; }
        public int INLINE_SCANNER_POSITION { get; set; }
        public int INLINE_SCANNER_ROTARY_PITCH { get; set; }
        public string EMAIL_SERVER_IP_ADDRESS { get; set; }
        public string EMAIL_SERVER_PORT { get; set; }
        public string EMAIL_SEND_TO_ADDRESS { get; set; }
        public string EMAIL_SEND_FROM_ADDRESS { get; set; }
        public string EMAIL_USER_ACCOUNT { get; set; }
        public string EMAIL_USER_PASSWORD { get; set; }
        public string EMAIL_SENDOUT_FREQUENCY { get; set; }
        public string PRINT_DURING_LOAD { get; set; }
        public string PRINT_DURING_UNLOAD { get; set; }
        public int SCRAP_BIN_CAPACITY { get; set; }
        public string SBMS_ID { get; set; }
    }
}
