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
namespace Model
{
	/// <summary>
	/// Binning:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Binning
	{
		public Binning()
		{}
		#region Model
		private int? _slot_id;
		private int? _slot_index;
		private int? _capacity;
		private string _part_id;
		private string _sapcode;
		private string _description;
		private string _batch_no;
		private string _status;
		private decimal? _start_weight;
		private decimal? _current_weight;
		private DateTime? _thawing_datetime;
		private DateTime? _ready_datetime;
		private DateTime? _expiry_datetime;
		private DateTime? _mf_expiry_date;
		private string _user_id;
		private string _user_name;
		private string _user_group;
		private string _department;
		private DateTime? _updated_time;
		/// <summary>
		/// 
		/// </summary>
		public int? SLOT_ID
		{
			set{ _slot_id=value;}
			get{return _slot_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SLOT_INDEX
		{
			set{ _slot_index=value;}
			get{return _slot_index;}
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
		public string PART_ID
		{
			set{ _part_id=value;}
			get{return _part_id;}
		}
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
		public string BATCH_NO
		{
			set{ _batch_no=value;}
			get{return _batch_no;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string STATUS
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? START_WEIGHT
		{
			set{ _start_weight=value;}
			get{return _start_weight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? CURRENT_WEIGHT
		{
			set{ _current_weight=value;}
			get{return _current_weight;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? THAWING_DATETIME
		{
			set{ _thawing_datetime=value;}
			get{return _thawing_datetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? READY_DATETIME
		{
			set{ _ready_datetime=value;}
			get{return _ready_datetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EXPIRY_DATETIME
		{
			set{ _expiry_datetime=value;}
			get{return _expiry_datetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? MF_EXPIRY_DATE
		{
			set{ _mf_expiry_date=value;}
			get{return _mf_expiry_date;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string USER_ID
		{
			set{ _user_id=value;}
			get{return _user_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string USER_NAME
		{
			set{ _user_name=value;}
			get{return _user_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string USER_GROUP
		{
			set{ _user_group=value;}
			get{return _user_group;}
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
		public DateTime? UPDATED_TIME
		{
			set{ _updated_time=value;}
			get{return _updated_time;}
		}
		#endregion Model

	}
}

