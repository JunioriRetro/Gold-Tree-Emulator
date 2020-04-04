using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Util;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class CreateFlatMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().OwnedRooms.Count <= ServerConfiguration.RoomUserLimit)
			{
				string string_ = GoldTree.FilterString(Event.PopFixedString());
				string string_2 = Event.PopFixedString();
				Event.PopFixedString();
                RoomData @class = GoldTree.GetGame().GetRoomManager().method_20(Session, string_, string_2);
				if (@class != null)
				{
					ServerMessage Message = new ServerMessage(59u);
					Message.AppendUInt(@class.Id);
					Message.AppendStringWithBreak(@class.Name);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
