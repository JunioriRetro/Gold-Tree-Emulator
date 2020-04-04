using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class CanCreateEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				bool bool_ = true;
				int int_ = 0;
				if (@class.State != 0)
				{
					bool_ = false;
					int_ = 3;
				}
				ServerMessage Message = new ServerMessage(367u);
				Message.AppendBoolean(bool_);
				Message.AppendInt32(int_);
				Session.SendMessage(Message);
			}
		}
	}
}
