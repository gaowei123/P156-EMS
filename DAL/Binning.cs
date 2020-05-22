/**  版本信息模板在安装目录下，可自行修改。
* Binning.cs
*
* 功 能： N/A
* 类 名： Binning
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/5/20 11:15:43   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace Common.DAL
{
    /// <summary>
    /// 数据访问类:Binning
    /// </summary>
    public partial class Binning
    {
        public Binning()
        { }
        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.Binning model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Binning(");
            strSql.Append("SLOT_ID,SLOT_INDEX,CAPACITY,PART_ID,SAPCODE,DESCRIPTION,BATCH_NO,STATUS,START_WEIGHT,CURRENT_WEIGHT,THAWING_DATETIME,READY_DATETIME,EXPIRY_DATETIME,MF_EXPIRY_DATE,USER_ID,USER_NAME,USER_GROUP,DEPARTMENT,UPDATED_TIME)");
            strSql.Append(" values (");
            strSql.Append("@SLOT_ID,@SLOT_INDEX,@CAPACITY,@PART_ID,@SAPCODE,@DESCRIPTION,@BATCH_NO,@STATUS,@START_WEIGHT,@CURRENT_WEIGHT,@THAWING_DATETIME,@READY_DATETIME,@EXPIRY_DATETIME,@MF_EXPIRY_DATE,@USER_ID,@USER_NAME,@USER_GROUP,@DEPARTMENT,@UPDATED_TIME)");
            SqlParameter[] parameters = {
                    new SqlParameter("@SLOT_ID", SqlDbType.Int,4),
                    new SqlParameter("@SLOT_INDEX", SqlDbType.Int,4),
                    new SqlParameter("@CAPACITY", SqlDbType.Int,4),
                    new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
                    new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,50),
                    new SqlParameter("@BATCH_NO", SqlDbType.VarChar,50),
                    new SqlParameter("@STATUS", SqlDbType.VarChar,50),
                    new SqlParameter("@START_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@CURRENT_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@THAWING_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@READY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@EXPIRY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@MF_EXPIRY_DATE", SqlDbType.DateTime),
                    new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@USER_NAME", SqlDbType.VarChar,50),
                    new SqlParameter("@USER_GROUP", SqlDbType.VarChar,50),
                    new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
                    new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime)};
            parameters[0].Value = model.SLOT_ID;
            parameters[1].Value = model.SLOT_INDEX;
            parameters[2].Value = model.CAPACITY;
            parameters[3].Value = model.PART_ID;
            parameters[4].Value = model.SAPCODE;
            parameters[5].Value = model.DESCRIPTION;
            parameters[6].Value = model.BATCH_NO;
            parameters[7].Value = model.STATUS;
            parameters[8].Value = model.START_WEIGHT;
            parameters[9].Value = model.CURRENT_WEIGHT;
            parameters[10].Value = model.THAWING_DATETIME;
            parameters[11].Value = model.READY_DATETIME;
            parameters[12].Value = model.EXPIRY_DATETIME;
            parameters[13].Value = model.MF_EXPIRY_DATE;
            parameters[14].Value = model.USER_ID;
            parameters[15].Value = model.USER_NAME;
            parameters[16].Value = model.USER_GROUP;
            parameters[17].Value = model.DEPARTMENT;
            parameters[18].Value = model.UPDATED_TIME;

            int rows = Common.DB.SqlDB.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.Binning model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Binning set ");
            strSql.Append("SLOT_ID=@SLOT_ID,");
            strSql.Append("SLOT_INDEX=@SLOT_INDEX,");
            strSql.Append("CAPACITY=@CAPACITY,");
            strSql.Append("PART_ID=@PART_ID,");
            strSql.Append("SAPCODE=@SAPCODE,");
            strSql.Append("DESCRIPTION=@DESCRIPTION,");
            strSql.Append("BATCH_NO=@BATCH_NO,");
            strSql.Append("STATUS=@STATUS,");
            strSql.Append("START_WEIGHT=@START_WEIGHT,");
            strSql.Append("CURRENT_WEIGHT=@CURRENT_WEIGHT,");
            strSql.Append("THAWING_DATETIME=@THAWING_DATETIME,");
            strSql.Append("READY_DATETIME=@READY_DATETIME,");
            strSql.Append("EXPIRY_DATETIME=@EXPIRY_DATETIME,");
            strSql.Append("MF_EXPIRY_DATE=@MF_EXPIRY_DATE,");
            strSql.Append("USER_ID=@USER_ID,");
            strSql.Append("USER_NAME=@USER_NAME,");
            strSql.Append("USER_GROUP=@USER_GROUP,");
            strSql.Append("DEPARTMENT=@DEPARTMENT,");
            strSql.Append("UPDATED_TIME=@UPDATED_TIME");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
                    new SqlParameter("@SLOT_ID", SqlDbType.Int,4),
                    new SqlParameter("@SLOT_INDEX", SqlDbType.Int,4),
                    new SqlParameter("@CAPACITY", SqlDbType.Int,4),
                    new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
                    new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,50),
                    new SqlParameter("@BATCH_NO", SqlDbType.VarChar,50),
                    new SqlParameter("@STATUS", SqlDbType.VarChar,50),
                    new SqlParameter("@START_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@CURRENT_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@THAWING_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@READY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@EXPIRY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@MF_EXPIRY_DATE", SqlDbType.DateTime),
                    new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@USER_NAME", SqlDbType.VarChar,50),
                    new SqlParameter("@USER_GROUP", SqlDbType.VarChar,50),
                    new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
                    new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime)};
            parameters[0].Value = model.SLOT_ID;
            parameters[1].Value = model.SLOT_INDEX;
            parameters[2].Value = model.CAPACITY;
            parameters[3].Value = model.PART_ID;
            parameters[4].Value = model.SAPCODE;
            parameters[5].Value = model.DESCRIPTION;
            parameters[6].Value = model.BATCH_NO;
            parameters[7].Value = model.STATUS;
            parameters[8].Value = model.START_WEIGHT;
            parameters[9].Value = model.CURRENT_WEIGHT;
            parameters[10].Value = model.THAWING_DATETIME;
            parameters[11].Value = model.READY_DATETIME;
            parameters[12].Value = model.EXPIRY_DATETIME;
            parameters[13].Value = model.MF_EXPIRY_DATE;
            parameters[14].Value = model.USER_ID;
            parameters[15].Value = model.USER_NAME;
            parameters[16].Value = model.USER_GROUP;
            parameters[17].Value = model.DEPARTMENT;
            parameters[18].Value = model.UPDATED_TIME;

            int rows = Common.DB.SqlDB.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Binning ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
            };

            int rows = Common.DB.SqlDB.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.Binning GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 SLOT_ID,SLOT_INDEX,CAPACITY,PART_ID,SAPCODE,DESCRIPTION,BATCH_NO,STATUS,START_WEIGHT,CURRENT_WEIGHT,THAWING_DATETIME,READY_DATETIME,EXPIRY_DATETIME,MF_EXPIRY_DATE,USER_ID,USER_NAME,USER_GROUP,DEPARTMENT,UPDATED_TIME from Binning ");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
            };

            Model.Binning model = new Model.Binning();
            DataTable dt = Common.DB.SqlDB.Query(strSql.ToString(), parameters);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return DataRowToModel(dt.Rows[0]);
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.Binning DataRowToModel(DataRow row)
        {
            Model.Binning model = new Model.Binning();
            if (row != null)
            {
                if (row["SLOT_ID"] != null && row["SLOT_ID"].ToString() != "")
                {
                    model.SLOT_ID = int.Parse(row["SLOT_ID"].ToString());
                }
                if (row["SLOT_INDEX"] != null && row["SLOT_INDEX"].ToString() != "")
                {
                    model.SLOT_INDEX = int.Parse(row["SLOT_INDEX"].ToString());
                }
                if (row["CAPACITY"] != null && row["CAPACITY"].ToString() != "")
                {
                    model.CAPACITY = int.Parse(row["CAPACITY"].ToString());
                }
                if (row["PART_ID"] != null)
                {
                    model.PART_ID = row["PART_ID"].ToString();
                }
                if (row["SAPCODE"] != null)
                {
                    model.SAPCODE = row["SAPCODE"].ToString();
                }
                if (row["DESCRIPTION"] != null)
                {
                    model.DESCRIPTION = row["DESCRIPTION"].ToString();
                }
                if (row["BATCH_NO"] != null)
                {
                    model.BATCH_NO = row["BATCH_NO"].ToString();
                }
                if (row["STATUS"] != null)
                {
                    model.STATUS = row["STATUS"].ToString();
                }
                if (row["START_WEIGHT"] != null && row["START_WEIGHT"].ToString() != "")
                {
                    model.START_WEIGHT = decimal.Parse(row["START_WEIGHT"].ToString());
                }
                if (row["CURRENT_WEIGHT"] != null && row["CURRENT_WEIGHT"].ToString() != "")
                {
                    model.CURRENT_WEIGHT = decimal.Parse(row["CURRENT_WEIGHT"].ToString());
                }
                if (row["THAWING_DATETIME"] != null && row["THAWING_DATETIME"].ToString() != "")
                {
                    model.THAWING_DATETIME = DateTime.Parse(row["THAWING_DATETIME"].ToString());
                }
                if (row["READY_DATETIME"] != null && row["READY_DATETIME"].ToString() != "")
                {
                    model.READY_DATETIME = DateTime.Parse(row["READY_DATETIME"].ToString());
                }
                if (row["EXPIRY_DATETIME"] != null && row["EXPIRY_DATETIME"].ToString() != "")
                {
                    model.EXPIRY_DATETIME = DateTime.Parse(row["EXPIRY_DATETIME"].ToString());
                }
                if (row["MF_EXPIRY_DATE"] != null && row["MF_EXPIRY_DATE"].ToString() != "")
                {
                    model.MF_EXPIRY_DATE = DateTime.Parse(row["MF_EXPIRY_DATE"].ToString());
                }
                if (row["USER_ID"] != null)
                {
                    model.USER_ID = row["USER_ID"].ToString();
                }
                if (row["USER_NAME"] != null)
                {
                    model.USER_NAME = row["USER_NAME"].ToString();
                }
                if (row["USER_GROUP"] != null)
                {
                    model.USER_GROUP = row["USER_GROUP"].ToString();
                }
                if (row["DEPARTMENT"] != null)
                {
                    model.DEPARTMENT = row["DEPARTMENT"].ToString();
                }
                if (row["UPDATED_TIME"] != null && row["UPDATED_TIME"].ToString() != "")
                {
                    model.UPDATED_TIME = DateTime.Parse(row["UPDATED_TIME"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SLOT_ID,SLOT_INDEX,CAPACITY,PART_ID,SAPCODE,DESCRIPTION,BATCH_NO,STATUS,START_WEIGHT,CURRENT_WEIGHT,THAWING_DATETIME,READY_DATETIME,EXPIRY_DATETIME,MF_EXPIRY_DATE,USER_ID,USER_NAME,USER_GROUP,DEPARTMENT,UPDATED_TIME ");
            strSql.Append(" FROM Binning ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return Common.DB.SqlDB.Query(strSql.ToString());
        }



        #endregion  BasicMethod






        public SqlCommand UpdateCommand(Model.Binning model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Binning set ");
            strSql.Append("SLOT_ID=@SLOT_ID,");
            strSql.Append("SLOT_INDEX=@SLOT_INDEX,");
            strSql.Append("CAPACITY=@CAPACITY,");
            strSql.Append("PART_ID=@PART_ID,");
            strSql.Append("SAPCODE=@SAPCODE,");
            strSql.Append("DESCRIPTION=@DESCRIPTION,");
            strSql.Append("BATCH_NO=@BATCH_NO,");
            strSql.Append("STATUS=@STATUS,");
            strSql.Append("START_WEIGHT=@START_WEIGHT,");
            strSql.Append("CURRENT_WEIGHT=@CURRENT_WEIGHT,");
            strSql.Append("THAWING_DATETIME=@THAWING_DATETIME,");
            strSql.Append("READY_DATETIME=@READY_DATETIME,");
            strSql.Append("EXPIRY_DATETIME=@EXPIRY_DATETIME,");
            strSql.Append("MF_EXPIRY_DATE=@MF_EXPIRY_DATE,");
            strSql.Append("USER_ID=@USER_ID,");
            strSql.Append("USER_NAME=@USER_NAME,");
            strSql.Append("USER_GROUP=@USER_GROUP,");
            strSql.Append("DEPARTMENT=@DEPARTMENT,");
            strSql.Append("UPDATED_TIME=@UPDATED_TIME");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
                    new SqlParameter("@SLOT_ID", SqlDbType.Int,4),
                    new SqlParameter("@SLOT_INDEX", SqlDbType.Int,4),
                    new SqlParameter("@CAPACITY", SqlDbType.Int,4),
                    new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
                    new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,50),
                    new SqlParameter("@BATCH_NO", SqlDbType.VarChar,50),
                    new SqlParameter("@STATUS", SqlDbType.VarChar,50),
                    new SqlParameter("@START_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@CURRENT_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@THAWING_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@READY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@EXPIRY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@MF_EXPIRY_DATE", SqlDbType.DateTime),
                    new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@USER_NAME", SqlDbType.VarChar,50),
                    new SqlParameter("@USER_GROUP", SqlDbType.VarChar,50),
                    new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
                    new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime)};
            parameters[0].Value = model.SLOT_ID;
            parameters[1].Value = model.SLOT_INDEX;
            parameters[2].Value = model.CAPACITY;
            parameters[3].Value = model.PART_ID;
            parameters[4].Value = model.SAPCODE;
            parameters[5].Value = model.DESCRIPTION;
            parameters[6].Value = model.BATCH_NO;
            parameters[7].Value = model.STATUS;
            parameters[8].Value = model.START_WEIGHT;
            parameters[9].Value = model.CURRENT_WEIGHT;
            parameters[10].Value = model.THAWING_DATETIME;
            parameters[11].Value = model.READY_DATETIME;
            parameters[12].Value = model.EXPIRY_DATETIME;
            parameters[13].Value = model.MF_EXPIRY_DATE;
            parameters[14].Value = model.USER_ID;
            parameters[15].Value = model.USER_NAME;
            parameters[16].Value = model.USER_GROUP;
            parameters[17].Value = model.DEPARTMENT;
            parameters[18].Value = model.UPDATED_TIME;

           return Common.DB.SqlDB.GenerateCommand(strSql.ToString(), parameters);
           
        }






    }
}

