/**  版本信息模板在安装目录下，可自行修改。
* MixHistory.cs
*
* 功 能： N/A
* 类 名： MixHistory
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2020/5/20 11:15:47   N/A    初版
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
	/// MixHistory:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class MixHistory
	{
		public MixHistory()
		{}
		#region Model
		private int? _id;
		private string _part_id;
		private DateTime? _mix_datetime;
		private decimal? _mix_time;
		private string _mix_by;
		private string _remark;
		/// <summary>
		/// 
		/// </summary>
		public int? ID
		{
			set{ _id=value;}
			get{return _id;}
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
		public DateTime? MIX_DATETIME
		{
			set{ _mix_datetime=value;}
			get{return _mix_datetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? MIX_TIME
		{
			set{ _mix_time=value;}
			get{return _mix_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MIX_BY
		{
			set{ _mix_by=value;}
			get{return _mix_by;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string REMARK
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

