using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Catalog
{
	internal sealed class RedeemVoucherMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			GoldTree.GetGame().GetCatalog().method_21().method_2(Session, Event.PopFixedString());
		}
	}
}
