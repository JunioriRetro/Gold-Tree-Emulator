using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Marketplace
{
	internal sealed class GetMarketplaceItemStatsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int int_ = Event.PopWiredInt32();
			int num = Event.PopWiredInt32();
			ServerMessage Message = new ServerMessage(617u);
			Message.AppendInt32(1);
			Message.AppendInt32(GoldTree.GetGame().GetCatalog().method_22().method_7(num));
			Dictionary<int, DataRow> dictionary = new Dictionary<int, DataRow>();
			DataTable dataTable = null;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				dataTable = @class.ReadDataTable("SELECT * FROM catalog_marketplace_data WHERE daysago > -30 AND sprite = " + num + " LIMIT 30;");
			}
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					dictionary.Add(Convert.ToInt32(dataRow["daysago"]), dataRow);
				}
			}
			Message.AppendInt32(30);
			Message.AppendInt32(29);
			for (int i = -29; i < 0; i++)
			{
				Message.AppendInt32(i);
				if (dictionary.ContainsKey(i + 1))
				{
					Message.AppendInt32(Convert.ToInt32(dictionary[i + 1]["avgprice"]) / Convert.ToInt32(dictionary[i + 1]["sold"]));
					Message.AppendInt32(Convert.ToInt32(dictionary[i + 1]["sold"]));
				}
				else
				{
					Message.AppendInt32(0);
					Message.AppendInt32(0);
				}
			}
			Message.AppendInt32(int_);
			Message.AppendInt32(num);
			Session.SendMessage(Message);
		}
	}
}
