using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Rooms.Settings
{
	internal sealed class DeleteRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Room class14_ = Session.GetHabbo().CurrentRoom;
			if (class14_ != null && (!(class14_.Owner != Session.GetHabbo().Username) || Session.GetHabbo().Rank == 7u))
			{
				GoldTree.GetGame().GetRoomManager().method_2(num);
                RoomData @class = GoldTree.GetGame().GetRoomManager().method_12(num);
				if (@class != null && (!(@class.Owner.ToLower() != Session.GetHabbo().Username.ToLower()) || Session.GetHabbo().Rank == 7u))
				{
					Room class2 = GoldTree.GetGame().GetRoomManager().GetRoom(@class.Id);
					if (class2 != null)
					{
						for (int i = 0; i < class2.RoomUsers.Length; i++)
						{
							RoomUser class3 = class2.RoomUsers[i];
							if (class3 != null && !class3.IsBot)
							{
								class3.GetClient().SendMessage(new ServerMessage(18u));
								class3.GetClient().GetHabbo().RemoveFromRoom();
							}
						}
						GoldTree.GetGame().GetRoomManager().method_16(class2);
					}
					using (DatabaseClient class4 = GoldTree.GetDatabase().GetClient())
					{
						class4.ExecuteQuery("DELETE FROM rooms WHERE Id = '" + num + "' LIMIT 1");
						class4.ExecuteQuery("DELETE FROM user_favorites WHERE room_id = '" + num + "'");
						class4.ExecuteQuery("UPDATE items SET room_id = '0' WHERE room_id = '" + num + "'");
						class4.ExecuteQuery("DELETE FROM room_rights WHERE room_id = '" + num + "'");
						class4.ExecuteQuery("UPDATE users SET home_room = '0' WHERE home_room = '" + num + "'");
						class4.ExecuteQuery("UPDATE user_pets SET room_id = '0' WHERE room_id = '" + num + "'");
						Session.GetHabbo().method_1(class4);
					}
					Session.GetHabbo().GetInventoryComponent().method_9(true);
					Session.GetHabbo().GetInventoryComponent().method_3(true);
					Session.SendMessage(GoldTree.GetGame().GetNavigator().method_12(Session, -3));
				}
			}
		}
	}
}
