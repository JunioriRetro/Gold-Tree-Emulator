using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Util;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Marketplace
{
	internal sealed class GetMarketplaceConfigurationMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			DateTime now = DateTime.Now;
			TimeSpan timeSpan = now - GoldTree.ServerStarted;
			ServerMessage Message = new ServerMessage(612u);
			Message.AppendBoolean(true);
			Message.AppendInt32(ServerConfiguration.MarketplaceTax);
			Message.AppendInt32(1);
			Message.AppendInt32(5);
			Message.AppendInt32(1);
			Message.AppendInt32(ServerConfiguration.MarketplacePriceLimit);
			Message.AppendInt32(48);
			Message.AppendInt32(timeSpan.Days);
			Session.SendMessage(Message);
		}
	}
}
