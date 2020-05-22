/**  版本信息模板在安装目录下，可自行修改。
* Slot_Position.cs
*
* 功 能： N/A
* 类 名： Slot_Position
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
	/// 数据访问类:Slot_Position
	/// </summary>
	public partial class Slot_Position
	{
		public Slot_Position()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Common.Model.Slot_Position model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Slot_Position(");
			strSql.Append("SLOT_ID,SLOT_INDEX,POSITION)");
			strSql.Append(" values (");
			strSql.Append("@SLOT_ID,@SLOT_INDEX,@POSITION)");
			SqlParameter[] parameters = {
					new SqlParameter("@SLOT_ID", SqlDbType.Int,4),
					new SqlParameter("@SLOT_INDEX", SqlDbType.Int,4),
					new SqlParameter("@POSITION", SqlDbType.Int,4)};
			parameters[0].Value = model.SLOT_ID;
			parameters[1].Value = model.SLOT_INDEX;
			parameters[2].Value = model.POSITION;

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
		public bool Update(Common.Model.Slot_Position model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Slot_Position set ");
			strSql.Append("SLOT_ID=@SLOT_ID,");
			strSql.Append("SLOT_INDEX=@SLOT_INDEX,");
			strSql.Append("POSITION=@POSITION");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@SLOT_ID", SqlDbType.Int,4),
					new SqlParameter("@SLOT_INDEX", SqlDbType.Int,4),
					new SqlParameter("@POSITION", SqlDbType.Int,4)};
			parameters[0].Value = model.SLOT_ID;
			parameters[1].Value = model.SLOT_INDEX;
			parameters[2].Value = model.POSITION;

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
			strSql.Append("delete from Slot_Position ");
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
		public Common.Model.Slot_Position GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 SLOT_ID,SLOT_INDEX,POSITION from Slot_Position ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
			};

			Common.Model.Slot_Position model=new Common.Model.Slot_Position();
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
		public Common.Model.Slot_Position DataRowToModel(DataRow row)
		{
			Common.Model.Slot_Position model=new Common.Model.Slot_Position();
			if (row != null)
			{
				if(row["SLOT_ID"]!=null && row["SLOT_ID"].ToString()!="")
				{
					model.SLOT_ID=int.Parse(row["SLOT_ID"].ToString());
				}
				if(row["SLOT_INDEX"]!=null && row["SLOT_INDEX"].ToString()!="")
				{
					model.SLOT_INDEX=int.Parse(row["SLOT_INDEX"].ToString());
				}
				if(row["POSITION"]!=null && row["POSITION"].ToString()!="")
				{
					model.POSITION=int.Parse(row["POSITION"].ToString());
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
			strSql.Append("select SLOT_ID,SLOT_INDEX,POSITION ");
			strSql.Append(" FROM Slot_Position ");
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
			strSql.Append(" SLOT_ID,SLOT_INDEX,POSITION ");
			strSql.Append(" FROM Slot_Position ");
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
			strSql.Append("select count(1) FROM Slot_Position ");
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
			strSql.Append(")AS Row, T.*  from Slot_Position T ");
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
			parameters[0].Value = "Slot_Position";
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

