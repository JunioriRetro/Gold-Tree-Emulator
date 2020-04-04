using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class MyRoomsSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session != null)
            {
                Session.SendMessage(GoldTree.GetGame().GetNavigator().method_12(Session, -3));
            }
		}
	}
}
