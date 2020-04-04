using System;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Advertisements
{
	internal sealed class RoomAdvertisement
	{
		public uint uint_0;
		public string string_0;
		public string string_1;
		public int int_0;
		public int int_1;
		public bool Boolean_0
		{
			get
			{
				return this.int_1 > 0 && this.int_0 >= this.int_1;
			}
		}
		public RoomAdvertisement(uint uint_1, string string_2, string string_3, int int_2, int int_3)
		{
			this.uint_0 = uint_1;
			this.string_0 = string_2;
			this.string_1 = string_3;
			this.int_0 = int_2;
			this.int_1 = int_3;
		}
		public void method_0()
		{
			this.int_0++;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.ExecuteQuery("UPDATE room_ads SET views = views + 1 WHERE Id = '" + this.uint_0 + "' LIMIT 1");
			}
		}
	}
}
