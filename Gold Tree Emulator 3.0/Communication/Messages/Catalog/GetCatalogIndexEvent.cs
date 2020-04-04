using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Catalog
{
	internal sealed class GetCatalogIndexEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            if (Session != null && Session.GetHabbo() != null)
            {
                Session.SendMessage(GoldTree.GetGame().GetCatalog().method_18(Session.GetHabbo().Rank));
            }
		}
	}
}
