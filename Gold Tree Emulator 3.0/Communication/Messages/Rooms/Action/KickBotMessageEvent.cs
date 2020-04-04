using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Action
{
	internal sealed class KickBotMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				RoomUser class2 = @class.method_52(Event.PopWiredInt32());
				if (class2 != null && class2.IsBot)
				{
					@class.method_6(class2.VirtualId, true);
				}
			}
		}
	}
}
