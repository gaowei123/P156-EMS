using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Model;

namespace BLL
{
	/// <summary>
	/// MixHistory
	/// </summary>
	public partial class MixHistory
	{
		private readonly DAL.MixHistory dal=new DAL.MixHistory();
		public MixHistory()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.MixHistory model)
		{
			return dal.Add(model);
		}

        public SqlCommand AddCommand(Model.MixHistory model)
        {
            return dal.AddCommand(model);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.MixHistory model)
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
		public Model.MixHistory GetModel()
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
		public List<Model.MixHistory> GetModelList(string strWhere)
		{
			DataTable dt = dal.GetList(strWhere);
			return DataTableToList(dt);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.MixHistory> DataTableToList(DataTable dt)
		{
			List<Model.MixHistory> modelList = new List<Model.MixHistory>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Model.MixHistory model;
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

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetAllList()
		{
			return GetList("");
		}


		#endregion  BasicMethod

	}
}

