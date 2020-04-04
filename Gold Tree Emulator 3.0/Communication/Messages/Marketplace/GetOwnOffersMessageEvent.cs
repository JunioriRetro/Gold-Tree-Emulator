using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Marketplace
{
	internal sealed class GetOwnOffersMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(GoldTree.GetGame().GetCatalog().method_22().method_9(Session.GetHabbo().Id));
		}
	}
}
