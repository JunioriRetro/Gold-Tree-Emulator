using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class DeleteFavouriteRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			Session.GetHabbo().list_1.Remove(num);
			ServerMessage Message = new ServerMessage(459u);
			Message.AppendUInt(num);
			Message.AppendBoolean(false);
			Session.SendMessage(Message);
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.ExecuteQuery(string.Concat(new object[]
				{
					"DELETE FROM user_favorites WHERE user_id = '",
					Session.GetHabbo().Id,
					"' AND room_id = '",
					num,
					"' LIMIT 1"
				}));
			}
		}
	}
}
