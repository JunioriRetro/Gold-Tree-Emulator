using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Util;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class CanCreateRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(512u);
			if (Session.GetHabbo().OwnedRooms.Count > ServerConfiguration.RoomUserLimit)
			{
				Message.AppendBoolean(true);
				Message.AppendInt32(ServerConfiguration.RoomUserLimit);
			}
			else
			{
				Message.AppendBoolean(false);
			}
			Session.SendMessage(Message);
		}
	}
}
