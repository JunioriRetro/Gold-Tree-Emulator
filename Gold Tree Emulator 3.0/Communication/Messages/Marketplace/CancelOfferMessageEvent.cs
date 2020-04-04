using System;
using System.Data;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Marketplace
{
	internal sealed class CancelOfferMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null)
			{
				uint num = Event.PopWiredUInt();
				DataRow dataRow = null;
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					dataRow = @class.ReadDataRow("SELECT furni_id, item_id, user_id, extra_data, offer_id, state, timestamp FROM catalog_marketplace_offers WHERE offer_id = '" + num + "' LIMIT 1");
				}
				if (dataRow != null)
				{
					int num2 = (int)Math.Floor(((double)dataRow["timestamp"] + 172800.0 - GoldTree.GetUnixTimestamp()) / 60.0);
					int num3 = int.Parse(dataRow["state"].ToString());
					if (num2 <= 0)
					{
						num3 = 3;
					}
					if ((uint)dataRow["user_id"] == Session.GetHabbo().Id && num3 != 2)
					{
						Item class2 = GoldTree.GetGame().GetItemManager().method_2((uint)dataRow["item_id"]);
						if (class2 != null)
						{
							GoldTree.GetGame().GetCatalog().method_9(Session, class2, 1, (string)dataRow["extra_data"], false, (uint)dataRow["furni_id"]);
							using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
							{
								@class.ExecuteQuery("DELETE FROM catalog_marketplace_offers WHERE offer_id = '" + num + "' LIMIT 1");
							}
							ServerMessage Message = new ServerMessage(614u);
							Message.AppendUInt((uint)dataRow["offer_id"]);
							Message.AppendBoolean(true);
							Session.SendMessage(Message);
						}
					}
				}
			}
		}
	}
}
