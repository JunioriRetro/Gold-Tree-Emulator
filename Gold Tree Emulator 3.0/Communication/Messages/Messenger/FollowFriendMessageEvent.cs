using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Messenger
{
	internal sealed class FollowFriendMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			GameClient @class = GoldTree.GetGame().GetClientManager().method_2(uint_);
            if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().InRoom)
			{
				Room class2 = GoldTree.GetGame().GetRoomManager().GetRoom(@class.GetHabbo().CurrentRoomId);
				if (class2 != null && Session != null && Session.GetHabbo() != null && class2 != Session.GetHabbo().CurrentRoom)
				{
					ServerMessage Message = new ServerMessage(286u);
					Message.AppendBoolean(class2.IsPublic);
					Message.AppendUInt(class2.Id);
					Session.SendMessage(Message);
				}
			}
		}
	}
}
