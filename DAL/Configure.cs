/**  版本信息模板在安装目录下，可自行修改。
* Configure.cs
*
* 功 能： N/A
* 类 名： Configure
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
using Maticsoft.DBUtility;//Please add references
namespace Common.DAL
{
	/// <summary>
	/// 数据访问类:Configure
	/// </summary>
	public partial class Configure
	{
		public Configure()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Common.Model.Configure model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Configure(");
			strSql.Append("CATEGORY,NAME,VALUE,UNIT,UPDATED_TIME,USER_ID,USER_GROUP,DEFAULT_VALUE)");
			strSql.Append(" values (");
			strSql.Append("@CATEGORY,@NAME,@VALUE,@UNIT,@UPDATED_TIME,@USER_ID,@USER_GROUP,@DEFAULT_VALUE)");
			SqlParameter[] parameters = {
					new SqlParameter("@CATEGORY", SqlDbType.VarChar,50),
					new SqlParameter("@NAME", SqlDbType.VarChar,200),
					new SqlParameter("@VALUE", SqlDbType.VarChar,100),
					new SqlParameter("@UNIT", SqlDbType.VarChar,50),
					new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
					new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
					new SqlParameter("@USER_GROUP", SqlDbType.VarChar,50),
					new SqlParameter("@DEFAULT_VALUE", SqlDbType.VarChar,100)};
			parameters[0].Value = model.CATEGORY;
			parameters[1].Value = model.NAME;
			parameters[2].Value = model.VALUE;
			parameters[3].Value = model.UNIT;
			parameters[4].Value = model.UPDATED_TIME;
			parameters[5].Value = model.USER_ID;
			parameters[6].Value = model.USER_GROUP;
			parameters[7].Value = model.DEFAULT_VALUE;

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
		public bool Update(Common.Model.Configure model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Configure set ");
			strSql.Append("CATEGORY=@CATEGORY,");
			strSql.Append("NAME=@NAME,");
			strSql.Append("VALUE=@VALUE,");
			strSql.Append("UNIT=@UNIT,");
			strSql.Append("UPDATED_TIME=@UPDATED_TIME,");
			strSql.Append("USER_ID=@USER_ID,");
			strSql.Append("USER_GROUP=@USER_GROUP,");
			strSql.Append("DEFAULT_VALUE=@DEFAULT_VALUE");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@CATEGORY", SqlDbType.VarChar,50),
					new SqlParameter("@NAME", SqlDbType.VarChar,200),
					new SqlParameter("@VALUE", SqlDbType.VarChar,100),
					new SqlParameter("@UNIT", SqlDbType.VarChar,50),
					new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
					new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
					new SqlParameter("@USER_GROUP", SqlDbType.VarChar,50),
					new SqlParameter("@DEFAULT_VALUE", SqlDbType.VarChar,100)};
			parameters[0].Value = model.CATEGORY;
			parameters[1].Value = model.NAME;
			parameters[2].Value = model.VALUE;
			parameters[3].Value = model.UNIT;
			parameters[4].Value = model.UPDATED_TIME;
			parameters[5].Value = model.USER_ID;
			parameters[6].Value = model.USER_GROUP;
			parameters[7].Value = model.DEFAULT_VALUE;

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
			strSql.Append("delete from Configure ");
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
		public Common.Model.Configure GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 CATEGORY,NAME,VALUE,UNIT,UPDATED_TIME,USER_ID,USER_GROUP,DEFAULT_VALUE from Configure ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			Common.Model.Configure model=new Common.Model.Configure();
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
		public Common.Model.Configure DataRowToModel(DataRow row)
		{
			Common.Model.Configure model=new Common.Model.Configure();
			if (row != null)
			{
				if(row["CATEGORY"]!=null)
				{
					model.CATEGORY=row["CATEGORY"].ToString();
				}
				if(row["NAME"]!=null)
				{
					model.NAME=row["NAME"].ToString();
				}
				if(row["VALUE"]!=null)
				{
					model.VALUE=row["VALUE"].ToString();
				}
				if(row["UNIT"]!=null)
				{
					model.UNIT=row["UNIT"].ToString();
				}
				if(row["UPDATED_TIME"]!=null && row["UPDATED_TIME"].ToString()!="")
				{
					model.UPDATED_TIME=DateTime.Parse(row["UPDATED_TIME"].ToString());
				}
				if(row["USER_ID"]!=null)
				{
					model.USER_ID=row["USER_ID"].ToString();
				}
				if(row["USER_GROUP"]!=null)
				{
					model.USER_GROUP=row["USER_GROUP"].ToString();
				}
				if(row["DEFAULT_VALUE"]!=null)
				{
					model.DEFAULT_VALUE=row["DEFAULT_VALUE"].ToString();
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
			strSql.Append("select CATEGORY,NAME,VALUE,UNIT,UPDATED_TIME,USER_ID,USER_GROUP,DEFAULT_VALUE ");
			strSql.Append(" FROM Configure ");
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
			strSql.Append(" CATEGORY,NAME,VALUE,UNIT,UPDATED_TIME,USER_ID,USER_GROUP,DEFAULT_VALUE ");
			strSql.Append(" FROM Configure ");
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
			strSql.Append("select count(1) FROM Configure ");
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
			strSql.Append(")AS Row, T.*  from Configure T ");
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
			parameters[0].Value = "Configure";
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

