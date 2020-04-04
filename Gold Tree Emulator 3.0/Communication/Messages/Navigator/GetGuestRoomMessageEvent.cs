using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class GetGuestRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			bool bool_ = Event.PopWiredBoolean();
			bool flag = Event.PopWiredBoolean();
            RoomData @class = GoldTree.GetGame().GetRoomManager().method_12(uint_);
			if (@class != null)
			{
				ServerMessage Message = new ServerMessage(454u);
				Message.AppendBoolean(bool_);
				@class.method_3(Message, false, flag);
				Message.AppendBoolean(flag);
				Message.AppendBoolean(bool_);
				Session.SendMessage(Message);
			}
		}
	}
}
