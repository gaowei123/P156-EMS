/**  版本信息模板在安装目录下，可自行修改。
* Event.cs
*
* 功 能： N/A
* 类 名： Event
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
	/// Event:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Event
	{
		public Event()
		{}
		#region Model
		private string _event_type;
		private string _event_name;
		private string _event_message;
		private string _department;
		private string _slot_no;
		private string _process_code;
		private string _part_id;
		private string _user_id;
		private DateTime? _updated_time;
		private int? _week;
		private int? _month;
		private int? _year;
		/// <summary>
		/// 
		/// </summary>
		public string EVENT_TYPE
		{
			set{ _event_type=value;}
			get{return _event_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EVENT_NAME
		{
			set{ _event_name=value;}
			get{return _event_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EVENT_MESSAGE
		{
			set{ _event_message=value;}
			get{return _event_message;}
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
		public string SLOT_NO
		{
			set{ _slot_no=value;}
			get{return _slot_no;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PROCESS_CODE
		{
			set{ _process_code=value;}
			get{return _process_code;}
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
		public string USER_ID
		{
			set{ _user_id=value;}
			get{return _user_id;}
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

