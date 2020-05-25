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
using System.Collections.Generic;

namespace BLL
{
	/// <summary>
	/// Configure
	/// </summary>
	public partial class Configure
	{
		private readonly Common.DAL.Configure dal=new Common.DAL.Configure();
		public Configure()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.Configure model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.Configure model)
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
		public Model.Configure GetModel(string sName)
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel(sName);
		}

	

		
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.Configure> GetAllModelList()
		{
			DataTable dt = dal.GetAllList();
			return DataTableToList(dt);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.Configure> DataTableToList(DataTable dt)
		{
			List<Model.Configure> modelList = new List<Model.Configure>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Model.Configure model;
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

	}
}

