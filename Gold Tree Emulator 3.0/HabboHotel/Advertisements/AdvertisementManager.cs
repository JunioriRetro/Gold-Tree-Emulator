using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.Core;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Advertisements
{
	internal sealed class AdvertisementManager
	{
		public List<RoomAdvertisement> RoomAdvertisements;
		public AdvertisementManager()
		{
			this.RoomAdvertisements = new List<RoomAdvertisement>();
		}
		public void method_0(DatabaseClient class6_0)
		{
            Logging.Write("Loading Room Adverts..");
			this.RoomAdvertisements.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM room_ads WHERE enabled = '1'");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.RoomAdvertisements.Add(new RoomAdvertisement((uint)dataRow["Id"], (string)dataRow["ad_image"], (string)dataRow["ad_link"], (int)dataRow["views"], (int)dataRow["views_limit"]));
				}
				Logging.WriteLine("completed!", ConsoleColor.Green);
			}
		}
		public RoomAdvertisement method_1()
		{
			if (this.RoomAdvertisements.Count <= 0)
			{
				return null;
			}
			else
			{
				int index;
				do
				{
					index = GoldTree.smethod_5(0, this.RoomAdvertisements.Count - 1);
				}
				while (this.RoomAdvertisements[index] == null || this.RoomAdvertisements[index].Boolean_0);
				return RoomAdvertisements[index];
			}
		}
	}
}
