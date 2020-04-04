using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class GetRoomVisitsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint uint_ = Event.PopWiredUInt();
				Session.SendMessage(GoldTree.GetGame().GetModerationTool().method_19(uint_));
			}
		}
	}
}
