using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class CancelEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true) && @class.Event != null)
			{
				@class.Event = null;
				ServerMessage Message = new ServerMessage(370u);
				Message.AppendStringWithBreak("-1");
				@class.SendMessage(Message, null);
			}
		}
	}
}
