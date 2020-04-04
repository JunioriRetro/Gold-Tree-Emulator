using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.Messages;
using GoldTree.HabboHotel.Catalogs;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Recycler
{
	internal sealed class RecycleItemsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session.GetHabbo().InRoom)
			{
				int num = Event.PopWiredInt32();
				if (num == 5)
				{
					for (int i = 0; i < num; i++)
					{
						UserItem @class = Session.GetHabbo().GetInventoryComponent().GetItemById(Event.PopWiredUInt());
						if (@class == null || !@class.method_1().AllowRecycle)
						{
							return;
						}
                        Session.GetHabbo().GetInventoryComponent().method_12(@class.uint_0, 0u, false);
					}
					uint num2 = GoldTree.GetGame().GetCatalog().method_14();
					EcotronReward class2 = GoldTree.GetGame().GetCatalog().method_15();
					using (DatabaseClient class3 = GoldTree.GetDatabase().GetClient())
					{
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items (Id,user_id,base_item,wall_pos) VALUES ('",
							num2,
							"','",
							Session.GetHabbo().Id,
							"','1478','')"
						}));
                        class3.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
							num2,
							"','",
							DateTime.Now.ToLongDateString(),
							"')"
						}));
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
							num2,
							"','",
							class2.uint_2,
							"','1','')"
						}));
					}
					Session.GetHabbo().GetInventoryComponent().method_9(true);
					ServerMessage Message = new ServerMessage(508u);
					Message.AppendBoolean(true);
					Message.AppendUInt(num2);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
