using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ObjectModule.Local
{
    public class Summary_List
    {
        public Summary_List()
        {

        }

        public Summary_List(DataRow x)
        {
            PART_ID = x["PART_ID"].ToString();
            SAPCODE = x["SAPCODE"].ToString();
            BATCH_NO = x["BATCH_NO"].ToString();
            DESCRIPTION = x["DESCRIPTION"].ToString();
            DEPARTMENT = x["DEPARTMENT"].ToString();
            LOT_ID_1 = x["LOT_ID_1"].ToString();
            LOT_ID_2 = x["LOT_ID_2"].ToString();
            DEVICE_1 = x["DEVICE_1"].ToString();
            DEVICE_2 = x["DEVICE_2"].ToString();
            EQUIP_ID_1 = x["EQUIP_ID_1"].ToString();
            EQUIP_ID_2 = x["EQUIP_ID_2"].ToString();
            LOCID_1 = x["LOCID_1"].ToString();
            LOCID_2 = x["LOCID_2"].ToString();
            LOAD_WEIGHT = x["LOAD_WEIGHT"].ToString();
            UNLOAD_WEIGHT_1 = x["UNLOAD_WEIGHT_1"].ToString();
            UNLOAD_WEIGHT_2 = x["UNLOAD_WEIGHT_2"].ToString();
            RETURN_WEIGHT_1 = x["RETURN_WEIGHT_1"].ToString();
            RETURN_WEIGHT_2 = x["RETURN_WEIGHT_2"].ToString();
            SCRAP_TIME = x["SCRAP_WEIGHT"].ToString();
            CAPACITY = x["CAPACITY"].ToString();
            THAWING_DATETIME = x["THAWING_DATETIME"].ToString();
            READY_DATETIME = x["READY_DATETIME"].ToString();
            EXPIRY_DATETIME = x["EXPIRY_DATETIME"].ToString();
            MF_EXPIRY_DATETIME = x["MF_EXPIRY_DATETIME"].ToString();
            EMPTY_SYRINGE_WEIGHT = x["EMPTY_SYRINGE_WEIGHT"].ToString();
            LOAD_USER = x["LOAD_USER"].ToString();
            LOAD_USER_NAME = x["LOAD_USER_NAME"].ToString();
            UNLOAD_USER_1 = x["UNLOAD_USER_1"].ToString();
            UNLOAD_USER_2 = x["UNLOAD_USER_2"].ToString();
            UNLOAD_USER_NAME_1 = x["UNLOAD_USER_NAME_1"].ToString();
            UNLOAD_USER_NAME_2 = x["UNLOAD_USER_NAME_2"].ToString();
            RETURN_USER_1 = x["RETURN_USER_1"].ToString();
            RETURN_USER_2 = x["RETURN_USER_2"].ToString();
            RETURN_USER_NAME_1 = x["RETURN_USER_NAME_1"].ToString();
            RETURN_USER_NAME_2 = x["RETURN_USER_NAME_2"].ToString();
            SCRAP_USER = x["SCRAP_USER"].ToString();
            SCRAP_USER_NAME = x["SCRAP_USER_NAME"].ToString();
            LOAD_TIME = x["LOAD_TIME"].ToString();
            UNLOAD_TIME_1 = x["UNLOAD_TIME_1"].ToString();
            UNLOAD_TIME_2 = x["UNLOAD_TIME_2"].ToString();
            RETURN_TIME_1 = x["RETURN_TIME_1"].ToString();
            RETURN_TIME_2 = x["RETURN_TIME_2"].ToString();
            SCRAP_TIME = x["SCRAP_TIME"].ToString();
        }

        public string PART_ID { get; set; }
        public string SAPCODE { get; set; }
        public string BATCH_NO { get; set; }
        public string DESCRIPTION { get; set; }
        public string DEPARTMENT { get; set; }
        public string LOT_ID_1 { get; set; }
        public string LOT_ID_2 { get; set; }
        public string DEVICE_1 { get; set; }
        public string DEVICE_2 { get; set; }
        public string EQUIP_ID_1 { get; set; }
        public string EQUIP_ID_2 { get; set; }
        public string LOCID_1 { get; set; }
        public string LOCID_2 { get; set; }
        public string LOAD_WEIGHT { get; set; }
        public string UNLOAD_WEIGHT_1 { get; set; }
        public string UNLOAD_WEIGHT_2 { get; set; }
        public string RETURN_WEIGHT_1 { get; set; }
        public string RETURN_WEIGHT_2 { get; set; }
        public string SCRAP_WEIGHT { get; set; }
        public string CAPACITY { get; set; }
        public string THAWING_DATETIME { get; set; }
        public string READY_DATETIME { get; set; }
        public string EXPIRY_DATETIME { get; set; }
        public string MF_EXPIRY_DATETIME { get; set; }
        public string EMPTY_SYRINGE_WEIGHT { get; set; }
        public string LOAD_USER { get; set; }
        public string LOAD_USER_NAME { get; set; }
        public string UNLOAD_USER_1 { get; set; }
        public string UNLOAD_USER_NAME_1 { get; set; }
        public string UNLOAD_USER_2 { get; set; }
        public string UNLOAD_USER_NAME_2 { get; set; }
        public string RETURN_USER_1 { get; set; }
        public string RETURN_USER_NAME_1 { get; set; }
        public string RETURN_USER_2 { get; set; }
        public string RETURN_USER_NAME_2 { get; set; }
        public string SCRAP_USER { get; set; }
        public string SCRAP_USER_NAME { get; set; }
        public string LOAD_TIME { get; set; }
        public string UNLOAD_TIME_1 { get; set; }
        public string UNLOAD_TIME_2 { get; set; }
        public string RETURN_TIME_1 { get; set; }
        public string RETURN_TIME_2 { get; set; }
        public string SCRAP_TIME { get; set; }
         
    }
}
