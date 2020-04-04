using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class LatestEventsSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int int_ = int.Parse(Event.PopFixedString());
			Session.SendMessage(GoldTree.GetGame().GetNavigator().method_8(Session, int_));
		}
	}
}
