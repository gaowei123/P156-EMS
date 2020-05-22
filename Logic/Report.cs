using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Logic
{
    public class Report
    {
        public static DataTable Search_Inventory()
        {
            return DataProvider.Local.Binning.Select.Search_Inventory();
        }

        public static int Search_Empty()
        {
            return DataProvider.Local.Binning.Select.Search_Empty();
        }

        public static DataTable Search_Data()
        {
            return DataProvider.Local.Binning.Select.Search_Data();
        }

        public static DataTable Equipment_List()
        {
            return DataProvider.Local.Equipment.Select_All();
        }

        public static DataTable Shift_Report(DateTime dateFrom, DateTime dateTo)
        {
            return DataProvider.Local.History.Shift_Report(dateFrom, dateTo);
        }


    }
}
