using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Messenger
{
	internal sealed class DeclineBuddyMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetMessenger() != null)
			{
				int num = Event.PopWiredInt32();
				int num2 = Event.PopWiredInt32();
				if (num == 0 && num2 == 1)
				{
					uint uint_ = Event.PopWiredUInt();
					Session.GetHabbo().GetMessenger().method_11(uint_);
				}
				else
				{
					if (num == 1)
					{
						Session.GetHabbo().GetMessenger().method_10();
					}
				}
			}
		}
	}
}
