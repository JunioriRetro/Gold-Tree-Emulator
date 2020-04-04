using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class GetRoomChatlogMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_chatlogs"))
			{
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				if (GoldTree.GetGame().GetRoomManager().GetRoom(uint_) != null)
				{
					Session.SendMessage(GoldTree.GetGame().GetModerationTool().method_22(uint_));
				}
			}
		}
	}
}
