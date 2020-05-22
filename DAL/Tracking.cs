/**  版本信息模板在安装目录下，可自行修改。
* Tracking.cs
*
* 功 能： N/A
* 类 名： Tracking
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/5/20 11:15:50   N/A    初版
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
	/// 数据访问类:Tracking
	/// </summary>
	public partial class Tracking
	{
		public Tracking()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.Tracking model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tracking(");
			strSql.Append("PART_ID,SAPCODE,BATCH_NO,DESCRIPTION,DEPARTMENT,START_WEIGHT,CURRENT_WEIGHT,CAPACITY,STATUS,EQUIP_ID,LOCID,THAWING_DATETIME,READY_DATETIME,EXPIRY_DATETIME,MF_EXPIRY_DATE,LOT_ID,DEVICE,EMPTY_SYRINGE_WEIGHT,USER_ID,USER_NAME,ACTION,REMARKS,UPDATED_TIME,WEEK,MONTH,YEAR)");
			strSql.Append(" values (");
			strSql.Append("@PART_ID,@SAPCODE,@BATCH_NO,@DESCRIPTION,@DEPARTMENT,@START_WEIGHT,@CURRENT_WEIGHT,@CAPACITY,@STATUS,@EQUIP_ID,@LOCID,@THAWING_DATETIME,@READY_DATETIME,@EXPIRY_DATETIME,@MF_EXPIRY_DATE,@LOT_ID,@DEVICE,@EMPTY_SYRINGE_WEIGHT,@USER_ID,@USER_NAME,@ACTION,@REMARKS,@UPDATED_TIME,@WEEK,@MONTH,@YEAR)");
			SqlParameter[] parameters = {
					new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
					new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
					new SqlParameter("@BATCH_NO", SqlDbType.VarChar,50),
					new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,100),
					new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
					new SqlParameter("@START_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@CURRENT_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@CAPACITY", SqlDbType.Int,4),
					new SqlParameter("@STATUS", SqlDbType.VarChar,20),
					new SqlParameter("@EQUIP_ID", SqlDbType.VarChar,20),
					new SqlParameter("@LOCID", SqlDbType.VarChar,50),
					new SqlParameter("@THAWING_DATETIME", SqlDbType.DateTime),
					new SqlParameter("@READY_DATETIME", SqlDbType.DateTime),
					new SqlParameter("@EXPIRY_DATETIME", SqlDbType.DateTime),
					new SqlParameter("@MF_EXPIRY_DATE", SqlDbType.DateTime),
					new SqlParameter("@LOT_ID", SqlDbType.VarChar,50),
					new SqlParameter("@DEVICE", SqlDbType.VarChar,100),
					new SqlParameter("@EMPTY_SYRINGE_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
					new SqlParameter("@USER_NAME", SqlDbType.VarChar,50),
					new SqlParameter("@ACTION", SqlDbType.VarChar,50),
					new SqlParameter("@REMARKS", SqlDbType.VarChar,100),
					new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
					new SqlParameter("@WEEK", SqlDbType.Int,4),
					new SqlParameter("@MONTH", SqlDbType.Int,4),
					new SqlParameter("@YEAR", SqlDbType.Int,4)};
			parameters[0].Value = model.PART_ID;
			parameters[1].Value = model.SAPCODE;
			parameters[2].Value = model.BATCH_NO;
			parameters[3].Value = model.DESCRIPTION;
			parameters[4].Value = model.DEPARTMENT;
			parameters[5].Value = model.START_WEIGHT;
			parameters[6].Value = model.CURRENT_WEIGHT;
			parameters[7].Value = model.CAPACITY;
			parameters[8].Value = model.STATUS;
			parameters[9].Value = model.EQUIP_ID;
			parameters[10].Value = model.LOCID;
			parameters[11].Value = model.THAWING_DATETIME;
			parameters[12].Value = model.READY_DATETIME;
			parameters[13].Value = model.EXPIRY_DATETIME;
			parameters[14].Value = model.MF_EXPIRY_DATE;
			parameters[15].Value = model.LOT_ID;
			parameters[16].Value = model.DEVICE;
			parameters[17].Value = model.EMPTY_SYRINGE_WEIGHT;
			parameters[18].Value = model.USER_ID;
			parameters[19].Value = model.USER_NAME;
			parameters[20].Value = model.ACTION;
			parameters[21].Value = model.REMARKS;
			parameters[22].Value = model.UPDATED_TIME;
			parameters[23].Value = model.WEEK;
			parameters[24].Value = model.MONTH;
			parameters[25].Value = model.YEAR;

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
		public bool Update(Model.Tracking model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Tracking set ");
			strSql.Append("PART_ID=@PART_ID,");
			strSql.Append("SAPCODE=@SAPCODE,");
			strSql.Append("BATCH_NO=@BATCH_NO,");
			strSql.Append("DESCRIPTION=@DESCRIPTION,");
			strSql.Append("DEPARTMENT=@DEPARTMENT,");
			strSql.Append("START_WEIGHT=@START_WEIGHT,");
			strSql.Append("CURRENT_WEIGHT=@CURRENT_WEIGHT,");
			strSql.Append("CAPACITY=@CAPACITY,");
			strSql.Append("STATUS=@STATUS,");
			strSql.Append("EQUIP_ID=@EQUIP_ID,");
			strSql.Append("LOCID=@LOCID,");
			strSql.Append("THAWING_DATETIME=@THAWING_DATETIME,");
			strSql.Append("READY_DATETIME=@READY_DATETIME,");
			strSql.Append("EXPIRY_DATETIME=@EXPIRY_DATETIME,");
			strSql.Append("MF_EXPIRY_DATE=@MF_EXPIRY_DATE,");
			strSql.Append("LOT_ID=@LOT_ID,");
			strSql.Append("DEVICE=@DEVICE,");
			strSql.Append("EMPTY_SYRINGE_WEIGHT=@EMPTY_SYRINGE_WEIGHT,");
			strSql.Append("USER_ID=@USER_ID,");
			strSql.Append("USER_NAME=@USER_NAME,");
			strSql.Append("ACTION=@ACTION,");
			strSql.Append("REMARKS=@REMARKS,");
			strSql.Append("UPDATED_TIME=@UPDATED_TIME,");
			strSql.Append("WEEK=@WEEK,");
			strSql.Append("MONTH=@MONTH,");
			strSql.Append("YEAR=@YEAR");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
					new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
					new SqlParameter("@BATCH_NO", SqlDbType.VarChar,50),
					new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,100),
					new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
					new SqlParameter("@START_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@CURRENT_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@CAPACITY", SqlDbType.Int,4),
					new SqlParameter("@STATUS", SqlDbType.VarChar,20),
					new SqlParameter("@EQUIP_ID", SqlDbType.VarChar,20),
					new SqlParameter("@LOCID", SqlDbType.VarChar,50),
					new SqlParameter("@THAWING_DATETIME", SqlDbType.DateTime),
					new SqlParameter("@READY_DATETIME", SqlDbType.DateTime),
					new SqlParameter("@EXPIRY_DATETIME", SqlDbType.DateTime),
					new SqlParameter("@MF_EXPIRY_DATE", SqlDbType.DateTime),
					new SqlParameter("@LOT_ID", SqlDbType.VarChar,50),
					new SqlParameter("@DEVICE", SqlDbType.VarChar,100),
					new SqlParameter("@EMPTY_SYRINGE_WEIGHT", SqlDbType.Decimal,5),
					new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
					new SqlParameter("@USER_NAME", SqlDbType.VarChar,50),
					new SqlParameter("@ACTION", SqlDbType.VarChar,50),
					new SqlParameter("@REMARKS", SqlDbType.VarChar,100),
					new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
					new SqlParameter("@WEEK", SqlDbType.Int,4),
					new SqlParameter("@MONTH", SqlDbType.Int,4),
					new SqlParameter("@YEAR", SqlDbType.Int,4)};
			parameters[0].Value = model.PART_ID;
			parameters[1].Value = model.SAPCODE;
			parameters[2].Value = model.BATCH_NO;
			parameters[3].Value = model.DESCRIPTION;
			parameters[4].Value = model.DEPARTMENT;
			parameters[5].Value = model.START_WEIGHT;
			parameters[6].Value = model.CURRENT_WEIGHT;
			parameters[7].Value = model.CAPACITY;
			parameters[8].Value = model.STATUS;
			parameters[9].Value = model.EQUIP_ID;
			parameters[10].Value = model.LOCID;
			parameters[11].Value = model.THAWING_DATETIME;
			parameters[12].Value = model.READY_DATETIME;
			parameters[13].Value = model.EXPIRY_DATETIME;
			parameters[14].Value = model.MF_EXPIRY_DATE;
			parameters[15].Value = model.LOT_ID;
			parameters[16].Value = model.DEVICE;
			parameters[17].Value = model.EMPTY_SYRINGE_WEIGHT;
			parameters[18].Value = model.USER_ID;
			parameters[19].Value = model.USER_NAME;
			parameters[20].Value = model.ACTION;
			parameters[21].Value = model.REMARKS;
			parameters[22].Value = model.UPDATED_TIME;
			parameters[23].Value = model.WEEK;
			parameters[24].Value = model.MONTH;
			parameters[25].Value = model.YEAR;

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
			strSql.Append("delete from Tracking ");
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
		public Model.Tracking GetModel(string sPartID)
		{
            if (string.IsNullOrEmpty(sPartID))
                return null;


      
            StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 PART_ID,SAPCODE,BATCH_NO,DESCRIPTION,DEPARTMENT,START_WEIGHT,CURRENT_WEIGHT,CAPACITY,STATUS,EQUIP_ID,LOCID,THAWING_DATETIME,READY_DATETIME,EXPIRY_DATETIME,MF_EXPIRY_DATE,LOT_ID,DEVICE,EMPTY_SYRINGE_WEIGHT,USER_ID,USER_NAME,ACTION,REMARKS,UPDATED_TIME,WEEK,MONTH,YEAR from Tracking ");
			strSql.Append(" where 1=1 and PART_ID = @PART_ID");



			SqlParameter[] parameters = {
                new SqlParameter("@PART_ID", SqlDbType.VarChar,50)
			};
            parameters[0].Value = sPartID;

		
			DataTable dt = Common.DB.SqlDB.Query(strSql.ToString(),parameters);
            if (dt == null || dt.Rows.Count ==0)
                return null;
            else
                return DataRowToModel(dt.Rows[0]);
        }


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.Tracking DataRowToModel(DataRow row)
		{
			Model.Tracking model=new Model.Tracking();
			if (row != null)
			{
				if(row["PART_ID"]!=null)
				{
					model.PART_ID=row["PART_ID"].ToString();
				}
				if(row["SAPCODE"]!=null)
				{
					model.SAPCODE=row["SAPCODE"].ToString();
				}
				if(row["BATCH_NO"]!=null)
				{
					model.BATCH_NO=row["BATCH_NO"].ToString();
				}
				if(row["DESCRIPTION"]!=null)
				{
					model.DESCRIPTION=row["DESCRIPTION"].ToString();
				}
				if(row["DEPARTMENT"]!=null)
				{
					model.DEPARTMENT=row["DEPARTMENT"].ToString();
				}
				if(row["START_WEIGHT"]!=null && row["START_WEIGHT"].ToString()!="")
				{
					model.START_WEIGHT=decimal.Parse(row["START_WEIGHT"].ToString());
				}
				if(row["CURRENT_WEIGHT"]!=null && row["CURRENT_WEIGHT"].ToString()!="")
				{
					model.CURRENT_WEIGHT=decimal.Parse(row["CURRENT_WEIGHT"].ToString());
				}
				if(row["CAPACITY"]!=null && row["CAPACITY"].ToString()!="")
				{
					model.CAPACITY=int.Parse(row["CAPACITY"].ToString());
				}
				if(row["STATUS"]!=null)
				{
					model.STATUS=row["STATUS"].ToString();
				}
				if(row["EQUIP_ID"]!=null)
				{
					model.EQUIP_ID=row["EQUIP_ID"].ToString();
				}
				if(row["LOCID"]!=null)
				{
					model.LOCID=row["LOCID"].ToString();
				}
				if(row["THAWING_DATETIME"]!=null && row["THAWING_DATETIME"].ToString()!="")
				{
					model.THAWING_DATETIME=DateTime.Parse(row["THAWING_DATETIME"].ToString());
				}
				if(row["READY_DATETIME"]!=null && row["READY_DATETIME"].ToString()!="")
				{
					model.READY_DATETIME=DateTime.Parse(row["READY_DATETIME"].ToString());
				}
				if(row["EXPIRY_DATETIME"]!=null && row["EXPIRY_DATETIME"].ToString()!="")
				{
					model.EXPIRY_DATETIME=DateTime.Parse(row["EXPIRY_DATETIME"].ToString());
				}
				if(row["MF_EXPIRY_DATE"]!=null && row["MF_EXPIRY_DATE"].ToString()!="")
				{
					model.MF_EXPIRY_DATE=DateTime.Parse(row["MF_EXPIRY_DATE"].ToString());
				}
				if(row["LOT_ID"]!=null)
				{
					model.LOT_ID=row["LOT_ID"].ToString();
				}
				if(row["DEVICE"]!=null)
				{
					model.DEVICE=row["DEVICE"].ToString();
				}
				if(row["EMPTY_SYRINGE_WEIGHT"]!=null && row["EMPTY_SYRINGE_WEIGHT"].ToString()!="")
				{
					model.EMPTY_SYRINGE_WEIGHT=decimal.Parse(row["EMPTY_SYRINGE_WEIGHT"].ToString());
				}
				if(row["USER_ID"]!=null)
				{
					model.USER_ID=row["USER_ID"].ToString();
				}
				if(row["USER_NAME"]!=null)
				{
					model.USER_NAME=row["USER_NAME"].ToString();
				}
				if(row["ACTION"]!=null)
				{
					model.ACTION=row["ACTION"].ToString();
				}
				if(row["REMARKS"]!=null)
				{
					model.REMARKS=row["REMARKS"].ToString();
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
		public DataTable GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select PART_ID,SAPCODE,BATCH_NO,DESCRIPTION,DEPARTMENT,START_WEIGHT,CURRENT_WEIGHT,CAPACITY,STATUS,EQUIP_ID,LOCID,THAWING_DATETIME,READY_DATETIME,EXPIRY_DATETIME,MF_EXPIRY_DATE,LOT_ID,DEVICE,EMPTY_SYRINGE_WEIGHT,USER_ID,USER_NAME,ACTION,REMARKS,UPDATED_TIME,WEEK,MONTH,YEAR ");
			strSql.Append(" FROM Tracking ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return Common.DB.SqlDB.Query(strSql.ToString());
		}
        #endregion  BasicMethod




        public SqlCommand AddCommand(Model.Tracking model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Tracking(");
            strSql.Append("PART_ID,SAPCODE,BATCH_NO,DESCRIPTION,DEPARTMENT,START_WEIGHT,CURRENT_WEIGHT,CAPACITY,STATUS,EQUIP_ID,LOCID,THAWING_DATETIME,READY_DATETIME,EXPIRY_DATETIME,MF_EXPIRY_DATE,LOT_ID,DEVICE,EMPTY_SYRINGE_WEIGHT,USER_ID,USER_NAME,ACTION,REMARKS,UPDATED_TIME,WEEK,MONTH,YEAR)");
            strSql.Append(" values (");
            strSql.Append("@PART_ID,@SAPCODE,@BATCH_NO,@DESCRIPTION,@DEPARTMENT,@START_WEIGHT,@CURRENT_WEIGHT,@CAPACITY,@STATUS,@EQUIP_ID,@LOCID,@THAWING_DATETIME,@READY_DATETIME,@EXPIRY_DATETIME,@MF_EXPIRY_DATE,@LOT_ID,@DEVICE,@EMPTY_SYRINGE_WEIGHT,@USER_ID,@USER_NAME,@ACTION,@REMARKS,@UPDATED_TIME,@WEEK,@MONTH,@YEAR)");
            SqlParameter[] parameters = {
                    new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
                    new SqlParameter("@BATCH_NO", SqlDbType.VarChar,50),
                    new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,100),
                    new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
                    new SqlParameter("@START_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@CURRENT_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@CAPACITY", SqlDbType.Int,4),
                    new SqlParameter("@STATUS", SqlDbType.VarChar,20),
                    new SqlParameter("@EQUIP_ID", SqlDbType.VarChar,20),
                    new SqlParameter("@LOCID", SqlDbType.VarChar,50),
                    new SqlParameter("@THAWING_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@READY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@EXPIRY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@MF_EXPIRY_DATE", SqlDbType.DateTime),
                    new SqlParameter("@LOT_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@DEVICE", SqlDbType.VarChar,100),
                    new SqlParameter("@EMPTY_SYRINGE_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@USER_NAME", SqlDbType.VarChar,50),
                    new SqlParameter("@ACTION", SqlDbType.VarChar,50),
                    new SqlParameter("@REMARKS", SqlDbType.VarChar,100),
                    new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
                    new SqlParameter("@WEEK", SqlDbType.Int,4),
                    new SqlParameter("@MONTH", SqlDbType.Int,4),
                    new SqlParameter("@YEAR", SqlDbType.Int,4)};
            parameters[0].Value = model.PART_ID;
            parameters[1].Value = model.SAPCODE;
            parameters[2].Value = model.BATCH_NO;
            parameters[3].Value = model.DESCRIPTION;
            parameters[4].Value = model.DEPARTMENT;
            parameters[5].Value = model.START_WEIGHT;
            parameters[6].Value = model.CURRENT_WEIGHT;
            parameters[7].Value = model.CAPACITY;
            parameters[8].Value = model.STATUS;
            parameters[9].Value = model.EQUIP_ID;
            parameters[10].Value = model.LOCID;
            parameters[11].Value = model.THAWING_DATETIME;
            parameters[12].Value = model.READY_DATETIME;
            parameters[13].Value = model.EXPIRY_DATETIME;
            parameters[14].Value = model.MF_EXPIRY_DATE;
            parameters[15].Value = model.LOT_ID;
            parameters[16].Value = model.DEVICE;
            parameters[17].Value = model.EMPTY_SYRINGE_WEIGHT;
            parameters[18].Value = model.USER_ID;
            parameters[19].Value = model.USER_NAME;
            parameters[20].Value = model.ACTION;
            parameters[21].Value = model.REMARKS;
            parameters[22].Value = model.UPDATED_TIME;
            parameters[23].Value = model.WEEK;
            parameters[24].Value = model.MONTH;
            parameters[25].Value = model.YEAR;

           return Common.DB.SqlDB.GenerateCommand(strSql.ToString(), parameters);
         
        }


        public SqlCommand UpdateCommand(Model.Tracking model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tracking set ");
            strSql.Append("PART_ID=@PART_ID,");
            strSql.Append("SAPCODE=@SAPCODE,");
            strSql.Append("BATCH_NO=@BATCH_NO,");
            strSql.Append("DESCRIPTION=@DESCRIPTION,");
            strSql.Append("DEPARTMENT=@DEPARTMENT,");
            strSql.Append("START_WEIGHT=@START_WEIGHT,");
            strSql.Append("CURRENT_WEIGHT=@CURRENT_WEIGHT,");
            strSql.Append("CAPACITY=@CAPACITY,");
            strSql.Append("STATUS=@STATUS,");
            strSql.Append("EQUIP_ID=@EQUIP_ID,");
            strSql.Append("LOCID=@LOCID,");
            strSql.Append("THAWING_DATETIME=@THAWING_DATETIME,");
            strSql.Append("READY_DATETIME=@READY_DATETIME,");
            strSql.Append("EXPIRY_DATETIME=@EXPIRY_DATETIME,");
            strSql.Append("MF_EXPIRY_DATE=@MF_EXPIRY_DATE,");
            strSql.Append("LOT_ID=@LOT_ID,");
            strSql.Append("DEVICE=@DEVICE,");
            strSql.Append("EMPTY_SYRINGE_WEIGHT=@EMPTY_SYRINGE_WEIGHT,");
            strSql.Append("USER_ID=@USER_ID,");
            strSql.Append("USER_NAME=@USER_NAME,");
            strSql.Append("ACTION=@ACTION,");
            strSql.Append("REMARKS=@REMARKS,");
            strSql.Append("UPDATED_TIME=@UPDATED_TIME,");
            strSql.Append("WEEK=@WEEK,");
            strSql.Append("MONTH=@MONTH,");
            strSql.Append("YEAR=@YEAR");
            strSql.Append(" where ");
            SqlParameter[] parameters = {
                    new SqlParameter("@PART_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@SAPCODE", SqlDbType.VarChar,50),
                    new SqlParameter("@BATCH_NO", SqlDbType.VarChar,50),
                    new SqlParameter("@DESCRIPTION", SqlDbType.VarChar,100),
                    new SqlParameter("@DEPARTMENT", SqlDbType.VarChar,50),
                    new SqlParameter("@START_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@CURRENT_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@CAPACITY", SqlDbType.Int,4),
                    new SqlParameter("@STATUS", SqlDbType.VarChar,20),
                    new SqlParameter("@EQUIP_ID", SqlDbType.VarChar,20),
                    new SqlParameter("@LOCID", SqlDbType.VarChar,50),
                    new SqlParameter("@THAWING_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@READY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@EXPIRY_DATETIME", SqlDbType.DateTime),
                    new SqlParameter("@MF_EXPIRY_DATE", SqlDbType.DateTime),
                    new SqlParameter("@LOT_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@DEVICE", SqlDbType.VarChar,100),
                    new SqlParameter("@EMPTY_SYRINGE_WEIGHT", SqlDbType.Decimal,5),
                    new SqlParameter("@USER_ID", SqlDbType.VarChar,50),
                    new SqlParameter("@USER_NAME", SqlDbType.VarChar,50),
                    new SqlParameter("@ACTION", SqlDbType.VarChar,50),
                    new SqlParameter("@REMARKS", SqlDbType.VarChar,100),
                    new SqlParameter("@UPDATED_TIME", SqlDbType.DateTime),
                    new SqlParameter("@WEEK", SqlDbType.Int,4),
                    new SqlParameter("@MONTH", SqlDbType.Int,4),
                    new SqlParameter("@YEAR", SqlDbType.Int,4)};
            parameters[0].Value = model.PART_ID;
            parameters[1].Value = model.SAPCODE;
            parameters[2].Value = model.BATCH_NO;
            parameters[3].Value = model.DESCRIPTION;
            parameters[4].Value = model.DEPARTMENT;
            parameters[5].Value = model.START_WEIGHT;
            parameters[6].Value = model.CURRENT_WEIGHT;
            parameters[7].Value = model.CAPACITY;
            parameters[8].Value = model.STATUS;
            parameters[9].Value = model.EQUIP_ID;
            parameters[10].Value = model.LOCID;
            parameters[11].Value = model.THAWING_DATETIME;
            parameters[12].Value = model.READY_DATETIME;
            parameters[13].Value = model.EXPIRY_DATETIME;
            parameters[14].Value = model.MF_EXPIRY_DATE;
            parameters[15].Value = model.LOT_ID;
            parameters[16].Value = model.DEVICE;
            parameters[17].Value = model.EMPTY_SYRINGE_WEIGHT;
            parameters[18].Value = model.USER_ID;
            parameters[19].Value = model.USER_NAME;
            parameters[20].Value = model.ACTION;
            parameters[21].Value = model.REMARKS;
            parameters[22].Value = model.UPDATED_TIME;
            parameters[23].Value = model.WEEK;
            parameters[24].Value = model.MONTH;
            parameters[25].Value = model.YEAR;

            return Common.DB.SqlDB.GenerateCommand(strSql.ToString(), parameters);
        }


    }
}

