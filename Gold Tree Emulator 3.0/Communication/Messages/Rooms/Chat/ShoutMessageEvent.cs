using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Chat
{
	internal sealed class ShoutMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (class2 != null)
				{
					class2.HandleSpeech(Session, GoldTree.FilterString(Event.PopFixedString()), true);
				}
			}
		}
	}
}
