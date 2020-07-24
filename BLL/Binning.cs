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
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BLL
{
	/// <summary>
	/// Binning
	/// </summary>
	public partial class Binning
	{
		private readonly Common.DAL.Binning dal=new Common.DAL.Binning();
		public Binning()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.Binning model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.Binning model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.Delete();
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.Binning GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel();
		}

		

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.Binning> GetModelList(string strWhere)
		{
			DataTable dt = dal.GetList(strWhere);
			return DataTableToList(dt);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.Binning> DataTableToList(DataTable dt)
		{
			List<Model.Binning> modelList = new List<Model.Binning>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Model.Binning model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		#endregion  BasicMethod


        public SqlCommand UpdateCommand(Model.Binning model)
        {
            return dal.UpdateCommand(model);
        }

      



        public List<Model.Binning> GetModelList()
        {
            DataTable dt = dal.GetAll();
            if (dt == null || dt.Rows.Count == 0)
                return null;



            List<Model.Binning> modelList = new List<Model.Binning>();
            foreach (DataRow dr in dt.Rows)
            {
                modelList.Add(DataRowToModel(dr));
            }

            return modelList;
        }




        public Model.Binning DataRowToModel(DataRow dr)
        {
            if (dr == null)
                return null;


            Model.Binning model = new Model.Binning();
            if (dr["SLOT_ID"].ToString() !="" || dr["SLOT_ID"] != null)
                model.SLOT_ID = int.Parse(dr["SLOT_ID"].ToString());

            if (dr["SLOT_INDEX"].ToString() != "" || dr["SLOT_INDEX"] != null)
                model.SLOT_INDEX = int.Parse(dr["SLOT_INDEX"].ToString());

            if (dr["CAPACITY"].ToString() != "" || dr["CAPACITY"] != null)
                model.CAPACITY = int.Parse(dr["CAPACITY"].ToString());



            model.PART_ID = dr["PART_ID"].ToString();
            model.SAPCODE = dr["SAPCODE"].ToString();
            model.DESCRIPTION = dr["DESCRIPTION"].ToString();
            model.BATCH_NO = dr["BATCH_NO"].ToString();
            model.STATUS = dr["STATUS"].ToString();



            if (dr["START_WEIGHT"].ToString() != "" || dr["START_WEIGHT"] != null)
                model.START_WEIGHT = decimal.Parse(dr["START_WEIGHT"].ToString());

            if (dr["CURRENT_WEIGHT"].ToString() != "" || dr["CURRENT_WEIGHT"] != null)
                model.CURRENT_WEIGHT = decimal.Parse(dr["CURRENT_WEIGHT"].ToString());




            if (dr["THAWING_DATETIME"].ToString() != "" || dr["THAWING_DATETIME"] != null)
                model.THAWING_DATETIME = DateTime.Parse(dr["THAWING_DATETIME"].ToString());

            if (dr["READY_DATETIME"].ToString() != "" || dr["READY_DATETIME"] != null)
                model.READY_DATETIME = DateTime.Parse(dr["READY_DATETIME"].ToString());

            if (dr["EXPIRY_DATETIME"].ToString() != "" || dr["EXPIRY_DATETIME"] != null)
                model.EXPIRY_DATETIME = DateTime.Parse(dr["EXPIRY_DATETIME"].ToString());

            if (dr["MF_EXPIRY_DATE"].ToString() != "" || dr["MF_EXPIRY_DATE"] != null)
                model.MF_EXPIRY_DATE = DateTime.Parse(dr["MF_EXPIRY_DATE"].ToString());




            model.USER_ID = dr["USER_ID"].ToString();
            model.USER_NAME = dr["USER_NAME"].ToString();
            model.USER_GROUP = dr["USER_GROUP"].ToString();
            model.DEPARTMENT = dr["DEPARTMENT"].ToString();



            if (dr["UPDATED_TIME"].ToString() != "" || dr["UPDATED_TIME"] != null)
                model.UPDATED_TIME = DateTime.Parse(dr["UPDATED_TIME"].ToString());



            return model;
        }

	}
}

