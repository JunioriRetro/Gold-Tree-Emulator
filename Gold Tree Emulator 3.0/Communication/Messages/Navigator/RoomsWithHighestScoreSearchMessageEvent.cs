using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class RoomsWithHighestScoreSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.GetConnection().SendData(GoldTree.GetGame().GetNavigator().SerializeNavigator(Session, -2));
		}
	}
}
