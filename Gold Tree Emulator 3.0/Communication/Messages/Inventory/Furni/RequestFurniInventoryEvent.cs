using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Inventory.Furni
{
	internal sealed class RequestFurniInventoryEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
				Session.SendMessage(Session.GetHabbo().GetInventoryComponent().method_13());
			}
		}
	}
}
