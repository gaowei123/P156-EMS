/**  版本信息模板在安装目录下，可自行修改。
* User_DB.cs
*
* 功 能： N/A
* 类 名： User_DB
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/5/20 11:15:51   N/A    初版
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
	public partial class User_DB
	{
		public User_DB()
		{}
		#region Model
		private string _user_id;
		private string _user_name;
		private string _password;
		private string _user_group;
		private DateTime? _updated_time;
		private string _updated_by;
		private string _department;
		private string _finger_template;
		private string _shift;
		private string _finger_template_1;
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
		public string PASSWORD
		{
			set{ _password=value;}
			get{return _password;}
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
		public string FINGER_TEMPLATE
		{
			set{ _finger_template=value;}
			get{return _finger_template;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SHIFT
		{
			set{ _shift=value;}
			get{return _shift;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FINGER_TEMPLATE_1
		{
			set{ _finger_template_1=value;}
			get{return _finger_template_1;}
		}
		#endregion Model

	}
}

