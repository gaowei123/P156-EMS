/**  版本信息模板在安装目录下，可自行修改。
* Event.cs
*
* 功 能： N/A
* 类 名： Event
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/5/20 11:15:45   N/A    初版
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
	/// 数据访问类:Event
	/// </summary>
	public partial class Event
	{
		public Event()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Common.Model.Event model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Event(");
			strSql.Append("EVENT_TYPE,EVENT_NAME,EVENT_MESSAGE,DEPARTMENT,SLOT_NO,PROCESS_CODE,PART_ID,USER_ID,UPDATED_TIME,WEEK,MONTH,YEAR)");
			strSql.Append(" values (");
			strSql.Append("@EVENT_TYPE,@EVENT_NAME,@EVENT_MESSAGE,@DEPARTMENT,@SLOT_NO,@PROCESS_CODE,@PART_ID,@USER_ID,@UPDATED_TIME,@WEEK,@MONTH,@YEAR)");
			SqlParameter[] parameters = {
					new SqlParameter("@EVENT_TYPE", SqlDbType.VarChar,50),
					new SqlParameter("@EVENT_NAME", SqlDbType.VarChar,50),
					new SqlParameter("@EVENT_MESSAGE", SqlDbType.VarChar,100),
					new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
					new SqlParameter("@SLOT_NO", SqlDbType.VarChar,20),
					new SqlParameter("@PROCESS_CODE", SqlDbType.VarChar,20),
					new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
					new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
					new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
					new SqlParameter("@WEEK", SqlDbType.Int,4),
					new SqlParameter("@MONTH", SqlDbType.Int,4),
					new SqlParameter("@YEAR", SqlDbType.Int,4)};
			parameters[0].Value = model.EVENT_TYPE;
			parameters[1].Value = model.EVENT_NAME;
			parameters[2].Value = model.EVENT_MESSAGE;
			parameters[3].Value = model.DEPARTMENT;
			parameters[4].Value = model.SLOT_NO;
			parameters[5].Value = model.PROCESS_CODE;
			parameters[6].Value = model.PART_ID;
			parameters[7].Value = model.USER_ID;
			parameters[8].Value = model.UPDATED_TIME;
			parameters[9].Value = model.WEEK;
			parameters[10].Value = model.MONTH;
			parameters[11].Value = model.YEAR;

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
		public bool Update(Common.Model.Event model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Event set ");
			strSql.Append("EVENT_TYPE=@EVENT_TYPE,");
			strSql.Append("EVENT_NAME=@EVENT_NAME,");
			strSql.Append("EVENT_MESSAGE=@EVENT_MESSAGE,");
			strSql.Append("DEPARTMENT=@DEPARTMENT,");
			strSql.Append("SLOT_NO=@SLOT_NO,");
			strSql.Append("PROCESS_CODE=@PROCESS_CODE,");
			strSql.Append("PART_ID=@PART_ID,");
			strSql.Append("USER_ID=@USER_ID,");
			strSql.Append("UPDATED_TIME=@UPDATED_TIME,");
			strSql.Append("WEEK=@WEEK,");
			strSql.Append("MONTH=@MONTH,");
			strSql.Append("YEAR=@YEAR");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@EVENT_TYPE", SqlDbType.VarChar,50),
					new SqlParameter("@EVENT_NAME", SqlDbType.VarChar,50),
					new SqlParameter("@EVENT_MESSAGE", SqlDbType.VarChar,100),
					new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
					new SqlParameter("@SLOT_NO", SqlDbType.VarChar,20),
					new SqlParameter("@PROCESS_CODE", SqlDbType.VarChar,20),
					new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
					new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
					new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
					new SqlParameter("@WEEK", SqlDbType.Int,4),
					new SqlParameter("@MONTH", SqlDbType.Int,4),
					new SqlParameter("@YEAR", SqlDbType.Int,4)};
			parameters[0].Value = model.EVENT_TYPE;
			parameters[1].Value = model.EVENT_NAME;
			parameters[2].Value = model.EVENT_MESSAGE;
			parameters[3].Value = model.DEPARTMENT;
			parameters[4].Value = model.SLOT_NO;
			parameters[5].Value = model.PROCESS_CODE;
			parameters[6].Value = model.PART_ID;
			parameters[7].Value = model.USER_ID;
			parameters[8].Value = model.UPDATED_TIME;
			parameters[9].Value = model.WEEK;
			parameters[10].Value = model.MONTH;
			parameters[11].Value = model.YEAR;

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
			strSql.Append("delete from Event ");
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
		public Common.Model.Event GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 EVENT_TYPE,EVENT_NAME,EVENT_MESSAGE,DEPARTMENT,SLOT_NO,PROCESS_CODE,PART_ID,USER_ID,UPDATED_TIME,WEEK,MONTH,YEAR from Event ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			Common.Model.Event model=new Common.Model.Event();
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
		public Common.Model.Event DataRowToModel(DataRow row)
		{
			Common.Model.Event model=new Common.Model.Event();
			if (row != null)
			{
				if(row["EVENT_TYPE"]!=null)
				{
					model.EVENT_TYPE=row["EVENT_TYPE"].ToString();
				}
				if(row["EVENT_NAME"]!=null)
				{
					model.EVENT_NAME=row["EVENT_NAME"].ToString();
				}
				if(row["EVENT_MESSAGE"]!=null)
				{
					model.EVENT_MESSAGE=row["EVENT_MESSAGE"].ToString();
				}
				if(row["DEPARTMENT"]!=null)
				{
					model.DEPARTMENT=row["DEPARTMENT"].ToString();
				}
				if(row["SLOT_NO"]!=null)
				{
					model.SLOT_NO=row["SLOT_NO"].ToString();
				}
				if(row["PROCESS_CODE"]!=null)
				{
					model.PROCESS_CODE=row["PROCESS_CODE"].ToString();
				}
				if(row["PART_ID"]!=null)
				{
					model.PART_ID=row["PART_ID"].ToString();
				}
				if(row["USER_ID"]!=null)
				{
					model.USER_ID=row["USER_ID"].ToString();
				}
				if(row["UPDATED_TIME"]!=null && row["UPDATED_TIME"].ToString()!="")
				{
					model.UPDATED_TIME=DateTime.Parse(row["UPDATED_TIME"].ToString());
				}
				if(row["WEEK"]!=null && row["WEEK"].ToString()!="")
				{
					model.WEEK=int.Parse(row["WEEK"].ToString());
				}
				if(row["MONTH"]!=null && row["MONTH"].ToString()!="")
				{
					model.MONTH=int.Parse(row["MONTH"].ToString());
				}
				if(row["YEAR"]!=null && row["YEAR"].ToString()!="")
				{
					model.YEAR=int.Parse(row["YEAR"].ToString());
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
			strSql.Append("select EVENT_TYPE,EVENT_NAME,EVENT_MESSAGE,DEPARTMENT,SLOT_NO,PROCESS_CODE,PART_ID,USER_ID,UPDATED_TIME,WEEK,MONTH,YEAR ");
			strSql.Append(" FROM Event ");
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
			strSql.Append(" EVENT_TYPE,EVENT_NAME,EVENT_MESSAGE,DEPARTMENT,SLOT_NO,PROCESS_CODE,PART_ID,USER_ID,UPDATED_TIME,WEEK,MONTH,YEAR ");
			strSql.Append(" FROM Event ");
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
			strSql.Append("select count(1) FROM Event ");
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
			strSql.Append(")AS Row, T.*  from Event T ");
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
			parameters[0].Value = "Event";
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

