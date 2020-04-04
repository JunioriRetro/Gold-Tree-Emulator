using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Rooms.Engine
{
	internal sealed class GetInterstitialMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetClientMessageHandler().method_4();
		}
	}
}
