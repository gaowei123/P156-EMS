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
namespace Model
{
	/// <summary>
	/// Configure:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Configure
	{
		public Configure()
		{}
		#region Model
		private string _category;
		private string _name;
		private string _value;
		private string _unit;
		private DateTime? _updated_time;
		private string _user_id;
		private string _user_group;
		private string _default_value;
		/// <summary>
		/// 
		/// </summary>
		public string CATEGORY
		{
			set{ _category=value;}
			get{return _category;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NAME
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string VALUE
		{
			set{ _value=value;}
			get{return _value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UNIT
		{
			set{ _unit=value;}
			get{return _unit;}
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
		public string USER_ID
		{
			set{ _user_id=value;}
			get{return _user_id;}
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
		public string DEFAULT_VALUE
		{
			set{ _default_value=value;}
			get{return _default_value;}
		}
		#endregion Model

	}
}

