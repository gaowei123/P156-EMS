﻿/**  版本信息模板在安装目录下，可自行修改。
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
namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class Tracking
	{
		public Tracking()
		{}
		#region Model
		private string _part_id;
		private string _sapcode;
		private string _batch_no;
		private string _description;
		private string _department;
		private decimal? _start_weight;
		private decimal? _current_weight;
		private int? _capacity;
		private string _status;
		private string _equip_id;
		private string _locid;
		private DateTime? _thawing_datetime;
		private DateTime? _ready_datetime;
		private DateTime? _expiry_datetime;
		private DateTime? _mf_expiry_date;
		private string _lot_id;
		private string _device;
		private decimal? _empty_syringe_weight;
		private string _user_id;
		private string _user_name;
		private string _action;
		private string _remarks;
		private DateTime? _updated_time;
		private int? _week;
		private int? _month;
		private int? _year;
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
		public string BATCH_NO
		{
			set{ _batch_no=value;}
			get{return _batch_no;}
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
		public string DEPARTMENT
		{
			set{ _department=value;}
			get{return _department;}
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
		public int? CAPACITY
		{
			set{ _capacity=value;}
			get{return _capacity;}
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
		public string EQUIP_ID
		{
			set{ _equip_id=value;}
			get{return _equip_id;}
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
		public string LOT_ID
		{
			set{ _lot_id=value;}
			get{return _lot_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DEVICE
		{
			set{ _device=value;}
			get{return _device;}
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
		public string ACTION
		{
			set{ _action=value;}
			get{return _action;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string REMARKS
		{
			set{ _remarks=value;}
			get{return _remarks;}
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
		public int? WEEK
		{
			set{ _week=value;}
			get{return _week;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MONTH
		{
			set{ _month=value;}
			get{return _month;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? YEAR
		{
			set{ _year=value;}
			get{return _year;}
		}
		#endregion Model

	}
}

