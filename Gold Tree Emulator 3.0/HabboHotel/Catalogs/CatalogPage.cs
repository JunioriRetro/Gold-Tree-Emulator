using System;
using System.Collections;
using System.Collections.Generic;
using GoldTree.Catalogs;
using GoldTree.Messages;
namespace GoldTree.HabboHotel.Catalogs
{
	internal sealed class CatalogPage
	{
		private int int_0;
		public int int_1;
		public string string_0;
		public bool bool_0;
		public bool bool_1;
		public uint uint_0;
		public bool bool_2;
		public int int_2;
		public int int_3;
		public string string_1;
		public string string_2;
		public string string_3;
		public string string_4;
		public string string_5;
		public string string_6;
		public string string_7;
		public string string_8;
		public string string_9;
		public string string_10;
		public List<CatalogItem> list_0;
		private ServerMessage Message5_0;
		public int Int32_0
		{
			get
			{
				return this.int_0;
			}
		}
		internal ServerMessage message5_0
		{
			get
			{
				return this.Message5_0;
			}
		}
		public CatalogPage(int int_4, int int_5, string string_11, bool bool_3, bool bool_4, uint uint_1, bool bool_5, int int_6, int int_7, string string_12, string string_13, string string_14, string string_15, string string_16, string string_17, string string_18, string string_19, string string_20, string string_21, ref Hashtable hashtable_0)
		{
			this.list_0 = new List<CatalogItem>();
			this.int_0 = int_4;
			this.int_1 = int_5;
			this.string_0 = string_11;
			this.bool_0 = bool_3;
			this.bool_1 = bool_4;
			this.uint_0 = uint_1;
			this.bool_2 = bool_5;
			this.int_2 = int_6;
			this.int_3 = int_7;
			this.string_1 = string_12;
			this.string_2 = string_13;
			this.string_3 = string_14;
			this.string_4 = string_15;
			this.string_5 = string_16;
			this.string_6 = string_17;
			this.string_7 = string_18;
			this.string_8 = string_19;
			this.string_9 = string_20;
			this.string_10 = string_21;
			foreach (CatalogItem @class in hashtable_0.Values)
			{
				if (@class.int_4 == int_4)
				{
					this.list_0.Add(@class);
				}
			}
		}
		internal void method_0()
		{
			this.Message5_0 = GoldTree.GetGame().GetCatalog().method_19(this);
		}
		public CatalogItem method_1(uint uint_1)
		{
			using (TimedLock.Lock(this.list_0))
			{
				foreach (CatalogItem current in this.list_0)
				{
					if (current.uint_0 == uint_1)
					{
						return current;
					}
				}
			}
			return null;
		}
		public void method_2(int int_4, ServerMessage Message5_1)
		{
			Message5_1.AppendInt32(this.int_2);
			Message5_1.AppendInt32(this.int_3);
			Message5_1.AppendInt32(this.int_0);
			Message5_1.AppendStringWithBreak(this.string_0);
			Message5_1.AppendInt32(GoldTree.GetGame().GetCatalog().method_4(int_4, this.int_0));
			Message5_1.AppendBoolean(this.bool_0);
		}
	}
}
