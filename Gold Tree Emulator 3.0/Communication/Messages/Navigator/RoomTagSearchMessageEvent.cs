using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class RoomTagSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Event.PopWiredInt32();
			Session.SendMessage(GoldTree.GetGame().GetNavigator().method_10(Event.PopFixedString()));
		}
	}
}
