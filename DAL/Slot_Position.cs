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

namespace DAL
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
		public bool Add(Model.Slot_Position model)
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
		public bool Update(Model.Slot_Position model)
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
		public Model.Slot_Position GetModel(int iSlotID)
		{
          
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 SLOT_ID,SLOT_INDEX,POSITION from Slot_Position ");
			strSql.Append(" where  SLOT_ID = @slotID");
			SqlParameter[] parameters = {
                new SqlParameter("@slotID", SqlDbType.Int)
			};

            parameters[0].Value = iSlotID;
		
			DataTable dt = Common.DB.SqlDB.Query(strSql.ToString(),parameters);
			if(dt.Rows.Count>0)
			{
				return DataRowToModel(dt.Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.Slot_Position DataRowToModel(DataRow row)
		{
			Model.Slot_Position model=new Model.Slot_Position();
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
		public DataTable GetList(string strWhere)
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

        public DataTable GetAllList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SLOT_ID,SLOT_INDEX,POSITION FROM Slot_Position");
           
            return Common.DB.SqlDB.Query(strSql.ToString());
        }


        #endregion  BasicMethod
    }
}

