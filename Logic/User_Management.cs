using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataProvider;
using System.Data;
using StaticRes;

namespace Logic
{
    public class User_Management
    {
        public static DataTable User_Search(string User_ID)
        {
            return DataProvider.Local.User.Select(User_ID);
        }

        public static ObjectModule.Local.User Validate_User(string User_ID)
        {
            if (User_ID.Length == 0)
                throw new System.Exception("Please input your user ID first !!");
            DataTable dt = DataProvider.Local.User.Select(User_ID);
            if (dt.Rows.Count == 0)
                throw new System.Exception("Invaild User !!");
            ObjectModule.Local.User lu = new ObjectModule.Local.User(dt.Rows[0]);
            return lu;
        }

        public static bool User_Update(ObjectModule.Local.User gu)
        {
            if (gu.DEPARTMENT.Length == 0)
                throw new System.Exception("Please select department !!");
            if (gu.PASSWORD.Length == 0)
                throw new System.Exception("Please input password !!");
            if (gu.USER_GROUP.Length == 0)
                throw new System.Exception("Please input user group !!");
            if (gu.USER_ID.Length == 0)
                throw new System.Exception("Please input user ID !!");
            if (gu.USER_NAME.Length == 0)
                throw new System.Exception("Please input user name !!");
            return DataProvider.Local.User.Update(gu);
        }

        public static bool User_Insert(ObjectModule.Local.User gu)
        {
            if (gu.DEPARTMENT.Length == 0)
                throw new System.Exception("Please select department !!");
            if (gu.PASSWORD.Length == 0)
                throw new System.Exception("Please input password !!");
            if (gu.USER_GROUP.Length == 0)
                throw new System.Exception("Please input user group !!");
            if (gu.USER_ID.Length == 0)
                throw new System.Exception("Please input user ID !!");
            if (gu.USER_NAME.Length == 0)
                throw new System.Exception("Please input user name !!");
            return DataProvider.Local.User.Insert(gu);
        }

        public static bool User_Delete(string User_ID,string Department)
        {
            return DataProvider.Local.User.Delete(User_ID,Department);
        }
    }
}
