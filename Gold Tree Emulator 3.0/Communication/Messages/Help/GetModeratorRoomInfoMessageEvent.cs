using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class GetModeratorRoomInfoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
                RoomData class27_ = GoldTree.GetGame().GetRoomManager().method_11(uint_);
				Session.SendMessage(GoldTree.GetGame().GetModerationTool().method_14(class27_));
			}
		}
	}
}
