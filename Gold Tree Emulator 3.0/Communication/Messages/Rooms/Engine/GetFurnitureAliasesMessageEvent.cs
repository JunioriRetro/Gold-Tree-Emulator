using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Rooms.Engine
{
	internal sealed class GetFurnitureAliasesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().uint_2 > 0u)
			{
				ServerMessage Message = new ServerMessage(297u);
				Message.AppendInt32(0);
				Session.SendMessage(Message);
			}
		}
	}
}
