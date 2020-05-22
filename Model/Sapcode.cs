/**  版本信息模板在安装目录下，可自行修改。
* Sapcode.cs
*
* 功 能： N/A
* 类 名： Sapcode
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
namespace Model
{
	/// <summary>
	/// Sapcode:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sapcode
	{
		public Sapcode()
		{}
		#region Model
		private string _sapcode;
		private string _description;
		private int? _thawing_time;
		private int? _usage_life;
		private string _department;
		private decimal? _new_min_weight;
		private decimal? _new_max_weight;
		private decimal? _empty_syringe_weight;
		private decimal? _scrap_weight;
		private int? _capacity;
		private string _on_hold;
		private DateTime? _updated_time;
		private string _updated_by;
		/// <summary>
		/// 
		/// </summary>
		public string SAPCODE
		{
			set{ _sapcode=value;}
			get{return _sapcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DESCRIPTION
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? THAWING_TIME
		{
			set{ _thawing_time=value;}
			get{return _thawing_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? USAGE_LIFE
		{
			set{ _usage_life=value;}
			get{return _usage_life;}
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
		public decimal? NEW_MIN_WEIGHT
		{
			set{ _new_min_weight=value;}
			get{return _new_min_weight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? NEW_MAX_WEIGHT
		{
			set{ _new_max_weight=value;}
			get{return _new_max_weight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? EMPTY_SYRINGE_WEIGHT
		{
			set{ _empty_syringe_weight=value;}
			get{return _empty_syringe_weight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? SCRAP_WEIGHT
		{
			set{ _scrap_weight=value;}
			get{return _scrap_weight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CAPACITY
		{
			set{ _capacity=value;}
			get{return _capacity;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ON_HOLD
		{
			set{ _on_hold=value;}
			get{return _on_hold;}
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

