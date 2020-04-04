using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree
{
	internal interface Interface
	{
		void Handle(GameClient Session, ClientMessage Event);
	}
}
