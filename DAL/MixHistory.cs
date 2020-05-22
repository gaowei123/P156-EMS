/**  版本信息模板在安装目录下，可自行修改。
* MixHistory.cs
*
* 功 能： N/A
* 类 名： MixHistory
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/5/20 11:15:47   N/A    初版
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

namespace DAL
{
	/// <summary>
	/// 数据访问类:MixHistory
	/// </summary>
	public partial class MixHistory
	{
		public MixHistory()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.MixHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into MixHistory(");
			strSql.Append("ID,PART_ID,MIX_DATETIME,MIX_TIME,MIX_BY,REMARK)");
			strSql.Append(" values (");
			strSql.Append("@ID,@PART_ID,@MIX_DATETIME,@MIX_TIME,@MIX_BY,@REMARK)");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
					new SqlParameter("@MIX_DATETIME", SqlDbType.DateTime),
					new SqlParameter("@MIX_TIME", SqlDbType.Decimal,9),
					new SqlParameter("@MIX_BY", SqlDbType.VarChar,50),
					new SqlParameter("@REMARK", SqlDbType.VarChar,500)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.PART_ID;
			parameters[2].Value = model.MIX_DATETIME;
			parameters[3].Value = model.MIX_TIME;
			parameters[4].Value = model.MIX_BY;
			parameters[5].Value = model.REMARK;

			int rows=Common.DB.SqlDB.ExecuteSql(strSql.ToString(),parameters);
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
        public bool Update(Model.MixHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update MixHistory set ");
			strSql.Append("ID=@ID,");
			strSql.Append("PART_ID=@PART_ID,");
			strSql.Append("MIX_DATETIME=@MIX_DATETIME,");
			strSql.Append("MIX_TIME=@MIX_TIME,");
			strSql.Append("MIX_BY=@MIX_BY,");
			strSql.Append("REMARK=@REMARK");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
					new SqlParameter("@MIX_DATETIME", SqlDbType.DateTime),
					new SqlParameter("@MIX_TIME", SqlDbType.Decimal,9),
					new SqlParameter("@MIX_BY", SqlDbType.VarChar,50),
					new SqlParameter("@REMARK", SqlDbType.VarChar,500)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.PART_ID;
			parameters[2].Value = model.MIX_DATETIME;
			parameters[3].Value = model.MIX_TIME;
			parameters[4].Value = model.MIX_BY;
			parameters[5].Value = model.REMARK;

			int rows=Common.DB.SqlDB.ExecuteSql(strSql.ToString(),parameters);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from MixHistory ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			int rows=Common.DB.SqlDB.ExecuteSql(strSql.ToString(),parameters);
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
		public Model.MixHistory GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,PART_ID,MIX_DATETIME,MIX_TIME,MIX_BY,REMARK from MixHistory ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

            Model.MixHistory model = new Model.MixHistory();
            DataTable dt=Common.DB.SqlDB.Query(strSql.ToString(),parameters);

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }else
            {
                return DataRowToModel(dt.Rows[0]);
            }		
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.MixHistory DataRowToModel(DataRow row)
		{
			Model.MixHistory model=new Model.MixHistory();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["PART_ID"]!=null)
				{
					model.PART_ID=row["PART_ID"].ToString();
				}
				if(row["MIX_DATETIME"]!=null && row["MIX_DATETIME"].ToString()!="")
				{
					model.MIX_DATETIME=DateTime.Parse(row["MIX_DATETIME"].ToString());
				}
				if(row["MIX_TIME"]!=null && row["MIX_TIME"].ToString()!="")
				{
					model.MIX_TIME=decimal.Parse(row["MIX_TIME"].ToString());
				}
				if(row["MIX_BY"]!=null)
				{
					model.MIX_BY=row["MIX_BY"].ToString();
				}
				if(row["REMARK"]!=null)
				{
					model.REMARK=row["REMARK"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ID,PART_ID,MIX_DATETIME,MIX_TIME,MIX_BY,REMARK ");
			strSql.Append(" FROM MixHistory ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return Common.DB.SqlDB.Query(strSql.ToString());
		}





        #endregion  BasicMethod





        public SqlCommand AddCommand(Model.MixHistory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into MixHistory(");
            strSql.Append("PART_ID,MIX_DATETIME,MIX_TIME,MIX_BY,REMARK)");
            strSql.Append(" values (");
            strSql.Append("@PART_ID,@MIX_DATETIME,@MIX_TIME,@MIX_BY,@REMARK)");
            SqlParameter[] parameters = {
                    new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@MIX_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@MIX_TIME", SqlDbType.Decimal,9),
                    new SqlParameter("@MIX_BY", SqlDbType.VarChar,50),
                    new SqlParameter("@REMARK", SqlDbType.VarChar,500)};         
            parameters[0].Value = model.PART_ID;
            parameters[1].Value = model.MIX_DATETIME;
            parameters[2].Value = model.MIX_TIME;
            parameters[3].Value = model.MIX_BY;
            parameters[4].Value = model.REMARK;

            return Common.DB.SqlDB.GenerateCommand(strSql.ToString(), parameters);
        }
    }
}

