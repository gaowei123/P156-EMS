/**  版本信息模板在安装目录下，可自行修改。
* Sapcode.cs
*
* 功 能： N/A
* 类 名： Sapcode
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/5/20 11:15:48   N/A    初版
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
using Maticsoft.DBUtility;//Please add references
namespace Common.DAL
{
	/// <summary>
	/// 数据访问类:Sapcode
	/// </summary>
	public partial class Sapcode
	{
		public Sapcode()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Common.Model.Sapcode model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sapcode(");
			strSql.Append("SAPCODE,DESCRIPTION,THAWING_TIME,USAGE_LIFE,DEPARTMENT,NEW_MIN_WEIGHT,NEW_MAX_WEIGHT,EMPTY_SYRINGE_WEIGHT,SCRAP_WEIGHT,CAPACITY,ON_HOLD,UPDATED_TIME,UPDATED_BY)");
			strSql.Append(" values (");
			strSql.Append("@SAPCODE,@DESCRIPTION,@THAWING_TIME,@USAGE_LIFE,@DEPARTMENT,@NEW_MIN_WEIGHT,@NEW_MAX_WEIGHT,@EMPTY_SYRINGE_WEIGHT,@SCRAP_WEIGHT,@CAPACITY,@ON_HOLD,@UPDATED_TIME,@UPDATED_BY)");
			SqlParameter[] parameters = {
					new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
					new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,100),
					new SqlParameter("@THAWING_TIME", SqlDbType.Int,4),
					new SqlParameter("@USAGE_LIFE", SqlDbType.Int,4),
					new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
					new SqlParameter("@NEW_MIN_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@NEW_MAX_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@EMPTY_SYRINGE_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@SCRAP_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@CAPACITY", SqlDbType.Int,4),
					new SqlParameter("@ON_HOLD", SqlDbType.VarChar,20),
					new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
					new SqlParameter("@UPDATED_BY", SqlDbType.VarChar,50)};
			parameters[0].Value = model.SAPCODE;
			parameters[1].Value = model.DESCRIPTION;
			parameters[2].Value = model.THAWING_TIME;
			parameters[3].Value = model.USAGE_LIFE;
			parameters[4].Value = model.DEPARTMENT;
			parameters[5].Value = model.NEW_MIN_WEIGHT;
			parameters[6].Value = model.NEW_MAX_WEIGHT;
			parameters[7].Value = model.EMPTY_SYRINGE_WEIGHT;
			parameters[8].Value = model.SCRAP_WEIGHT;
			parameters[9].Value = model.CAPACITY;
			parameters[10].Value = model.ON_HOLD;
			parameters[11].Value = model.UPDATED_TIME;
			parameters[12].Value = model.UPDATED_BY;

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
		public bool Update(Common.Model.Sapcode model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sapcode set ");
			strSql.Append("SAPCODE=@SAPCODE,");
			strSql.Append("DESCRIPTION=@DESCRIPTION,");
			strSql.Append("THAWING_TIME=@THAWING_TIME,");
			strSql.Append("USAGE_LIFE=@USAGE_LIFE,");
			strSql.Append("DEPARTMENT=@DEPARTMENT,");
			strSql.Append("NEW_MIN_WEIGHT=@NEW_MIN_WEIGHT,");
			strSql.Append("NEW_MAX_WEIGHT=@NEW_MAX_WEIGHT,");
			strSql.Append("EMPTY_SYRINGE_WEIGHT=@EMPTY_SYRINGE_WEIGHT,");
			strSql.Append("SCRAP_WEIGHT=@SCRAP_WEIGHT,");
			strSql.Append("CAPACITY=@CAPACITY,");
			strSql.Append("ON_HOLD=@ON_HOLD,");
			strSql.Append("UPDATED_TIME=@UPDATED_TIME,");
			strSql.Append("UPDATED_BY=@UPDATED_BY");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
					new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,100),
					new SqlParameter("@THAWING_TIME", SqlDbType.Int,4),
					new SqlParameter("@USAGE_LIFE", SqlDbType.Int,4),
					new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
					new SqlParameter("@NEW_MIN_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@NEW_MAX_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@EMPTY_SYRINGE_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@SCRAP_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@CAPACITY", SqlDbType.Int,4),
					new SqlParameter("@ON_HOLD", SqlDbType.VarChar,20),
					new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
					new SqlParameter("@UPDATED_BY", SqlDbType.VarChar,50)};
			parameters[0].Value = model.SAPCODE;
			parameters[1].Value = model.DESCRIPTION;
			parameters[2].Value = model.THAWING_TIME;
			parameters[3].Value = model.USAGE_LIFE;
			parameters[4].Value = model.DEPARTMENT;
			parameters[5].Value = model.NEW_MIN_WEIGHT;
			parameters[6].Value = model.NEW_MAX_WEIGHT;
			parameters[7].Value = model.EMPTY_SYRINGE_WEIGHT;
			parameters[8].Value = model.SCRAP_WEIGHT;
			parameters[9].Value = model.CAPACITY;
			parameters[10].Value = model.ON_HOLD;
			parameters[11].Value = model.UPDATED_TIME;
			parameters[12].Value = model.UPDATED_BY;

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
			strSql.Append("delete from Sapcode ");
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
		public Common.Model.Sapcode GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 SAPCODE,DESCRIPTION,THAWING_TIME,USAGE_LIFE,DEPARTMENT,NEW_MIN_WEIGHT,NEW_MAX_WEIGHT,EMPTY_SYRINGE_WEIGHT,SCRAP_WEIGHT,CAPACITY,ON_HOLD,UPDATED_TIME,UPDATED_BY from Sapcode ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			Common.Model.Sapcode model=new Common.Model.Sapcode();
			DataSet ds=Common.DB.SqlDB.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Common.Model.Sapcode DataRowToModel(DataRow row)
		{
			Common.Model.Sapcode model=new Common.Model.Sapcode();
			if (row != null)
			{
				if(row["SAPCODE"]!=null)
				{
					model.SAPCODE=row["SAPCODE"].ToString();
				}
				if(row["DESCRIPTION"]!=null)
				{
					model.DESCRIPTION=row["DESCRIPTION"].ToString();
				}
				if(row["THAWING_TIME"]!=null && row["THAWING_TIME"].ToString()!="")
				{
					model.THAWING_TIME=int.Parse(row["THAWING_TIME"].ToString());
				}
				if(row["USAGE_LIFE"]!=null && row["USAGE_LIFE"].ToString()!="")
				{
					model.USAGE_LIFE=int.Parse(row["USAGE_LIFE"].ToString());
				}
				if(row["DEPARTMENT"]!=null)
				{
					model.DEPARTMENT=row["DEPARTMENT"].ToString();
				}
				if(row["NEW_MIN_WEIGHT"]!=null && row["NEW_MIN_WEIGHT"].ToString()!="")
				{
					model.NEW_MIN_WEIGHT=decimal.Parse(row["NEW_MIN_WEIGHT"].ToString());
				}
				if(row["NEW_MAX_WEIGHT"]!=null && row["NEW_MAX_WEIGHT"].ToString()!="")
				{
					model.NEW_MAX_WEIGHT=decimal.Parse(row["NEW_MAX_WEIGHT"].ToString());
				}
				if(row["EMPTY_SYRINGE_WEIGHT"]!=null && row["EMPTY_SYRINGE_WEIGHT"].ToString()!="")
				{
					model.EMPTY_SYRINGE_WEIGHT=decimal.Parse(row["EMPTY_SYRINGE_WEIGHT"].ToString());
				}
				if(row["SCRAP_WEIGHT"]!=null && row["SCRAP_WEIGHT"].ToString()!="")
				{
					model.SCRAP_WEIGHT=decimal.Parse(row["SCRAP_WEIGHT"].ToString());
				}
				if(row["CAPACITY"]!=null && row["CAPACITY"].ToString()!="")
				{
					model.CAPACITY=int.Parse(row["CAPACITY"].ToString());
				}
				if(row["ON_HOLD"]!=null)
				{
					model.ON_HOLD=row["ON_HOLD"].ToString();
				}
				if(row["UPDATED_TIME"]!=null && row["UPDATED_TIME"].ToString()!="")
				{
					model.UPDATED_TIME=DateTime.Parse(row["UPDATED_TIME"].ToString());
				}
				if(row["UPDATED_BY"]!=null)
				{
					model.UPDATED_BY=row["UPDATED_BY"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select SAPCODE,DESCRIPTION,THAWING_TIME,USAGE_LIFE,DEPARTMENT,NEW_MIN_WEIGHT,NEW_MAX_WEIGHT,EMPTY_SYRINGE_WEIGHT,SCRAP_WEIGHT,CAPACITY,ON_HOLD,UPDATED_TIME,UPDATED_BY ");
			strSql.Append(" FROM Sapcode ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return Common.DB.SqlDB.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" SAPCODE,DESCRIPTION,THAWING_TIME,USAGE_LIFE,DEPARTMENT,NEW_MIN_WEIGHT,NEW_MAX_WEIGHT,EMPTY_SYRINGE_WEIGHT,SCRAP_WEIGHT,CAPACITY,ON_HOLD,UPDATED_TIME,UPDATED_BY ");
			strSql.Append(" FROM Sapcode ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return Common.DB.SqlDB.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM Sapcode ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.diagram_id desc");
			}
			strSql.Append(")AS Row, T.*  from Sapcode T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return Common.DB.SqlDB.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "Sapcode";
			parameters[1].Value = "diagram_id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return Common.DB.SqlDB.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

