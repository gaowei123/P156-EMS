/**  版本信息模板在安装目录下，可自行修改。
* Slot_Position.cs
*
* 功 能： N/A
* 类 名： Slot_Position
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
	/// Slot_Position:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Slot_Position
	{
		public Slot_Position()
		{}
		#region Model
		private int? _slot_id;
		private int? _slot_index;
		private int? _position;
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
		public int? POSITION
		{
			set{ _position=value;}
			get{return _position;}
		}
		#endregion Model

	}
}

