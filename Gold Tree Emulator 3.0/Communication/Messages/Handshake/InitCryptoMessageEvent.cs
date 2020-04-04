using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Handshake
{
	internal sealed class InitCryptoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Interface @interface;
			if (GoldTree.GetPacketManager().Handle(1817u, out @interface))
			{
				@interface.Handle(Session, null);
			}
		}
	}
}
