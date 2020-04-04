using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Rooms.Session
{
	internal sealed class GoToFlatMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetHabbo().uint_2 = Event.PopWiredUInt();
			Session.GetClientMessageHandler().method_6();
		}
	}
}
