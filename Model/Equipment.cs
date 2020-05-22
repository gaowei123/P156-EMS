/**  版本信息模板在安装目录下，可自行修改。
* Equipment.cs
*
* 功 能： N/A
* 类 名： Equipment
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
namespace Model
{
	/// <summary>
	/// Equipment:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Equipment
	{
		public Equipment()
		{}
		#region Model
		private string _equip_id;
		private string _department;
		private string _equip_maker;
		private string _equip_model;
		private string _locid;
		private DateTime? _updated_time;
		private string _updated_by;
		/// <summary>
		/// 
		/// </summary>
		public string EQUIP_ID
		{
			set{ _equip_id=value;}
			get{return _equip_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DEPARTMENT
		{
			set{ _department=value;}
			get{return _department;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EQUIP_MAKER
		{
			set{ _equip_maker=value;}
			get{return _equip_maker;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EQUIP_MODEL
		{
			set{ _equip_model=value;}
			get{return _equip_model;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LOCID
		{
			set{ _locid=value;}
			get{return _locid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? UPDATED_TIME
		{
			set{ _updated_time=value;}
			get{return _updated_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UPDATED_BY
		{
			set{ _updated_by=value;}
			get{return _updated_by;}
		}
		#endregion Model

	}
}

