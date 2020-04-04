using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class UpdateNavigatorSettingsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
            RoomData @class = GoldTree.GetGame().GetRoomManager().method_12(num);
			if (num == 0u || (@class != null && !(@class.Owner.ToLower() != Session.GetHabbo().Username.ToLower())))
			{
				Session.GetHabbo().HomeRoomId = num;
				using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET home_room = '",
						num,
						"' WHERE Id = '",
						Session.GetHabbo().Id,
						"' LIMIT 1;"
					}));
				}
				ServerMessage Message = new ServerMessage(455u);
				Message.AppendUInt(num);
				Session.SendMessage(Message);
			}
		}
	}
}
