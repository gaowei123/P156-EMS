/**  版本信息模板在安装目录下，可自行修改。
* History.cs
*
* 功 能： N/A
* 类 名： History
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/5/20 11:15:46   N/A    初版
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
	/// History
	/// </summary>
	public partial class History
	{
		private readonly Common.DAL.History dal=new Common.DAL.History();
		public History()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.History model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.History model)
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
		public Model.History GetModel()
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
		public List<Model.History> GetModelList(string strWhere)
		{
			DataTable dt = dal.GetList(strWhere);
			return DataTableToList(dt);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.History> DataTableToList(DataTable dt)
		{
			List<Model.History> modelList = new List<Model.History>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Model.History model;
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



        public SqlCommand AddCommand(Model.History model)
        {
            return dal.AddCommand(model);
        }
    
    
        public SqlCommand UpdateCommand(Model.History model)
        {
            return dal.UpdateCommand(model);
        }

        public Model.History CopyTrackModel( Model.Tracking trackModel)
        {
            if (trackModel == null)
                return null;

            Model.History model = new Model.History();
            model.PART_ID = trackModel.PART_ID;
            model.SAPCODE = trackModel.SAPCODE;
            model.BATCH_NO = trackModel.BATCH_NO;
            model.DESCRIPTION = trackModel.DESCRIPTION;
            model.DEPARTMENT = trackModel.DEPARTMENT;
            model.START_WEIGHT = trackModel.START_WEIGHT;
            model.CURRENT_WEIGHT = trackModel.CURRENT_WEIGHT;
            model.CAPACITY = trackModel.CAPACITY;
            model.STATUS = trackModel.STATUS;
            model.EQUIP_ID = trackModel.EQUIP_ID;
            model.THAWING_DATETIME = trackModel.THAWING_DATETIME;
            model.READY_DATETIME = trackModel.READY_DATETIME;
            model.EXPIRY_DATETIME = trackModel.EXPIRY_DATETIME;
            model.MF_EXPIRY_DATE = trackModel.MF_EXPIRY_DATE;
            model.LOT_ID = trackModel.LOT_ID;
            model.DEVICE = trackModel.DEVICE;
            model.EMPTY_SYRINGE_WEIGHT = trackModel.EMPTY_SYRINGE_WEIGHT;
            model.USER_ID = trackModel.USER_ID;
            model.USER_NAME = trackModel.USER_NAME;
            model.ACTION = trackModel.ACTION;
            model.REMARKS = trackModel.REMARKS;
            model.UPDATED_TIME = trackModel.UPDATED_TIME;
            model.WEEK = trackModel.WEEK;
            model.MONTH = trackModel.MONTH;
            model.YEAR = trackModel.YEAR ;
            model.LOCID = trackModel.LOCID;
            
            return model;
        }



    }
}

