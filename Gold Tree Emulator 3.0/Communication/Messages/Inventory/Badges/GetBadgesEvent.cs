using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Inventory.Badges
{
	internal sealed class GetBadgesEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Session.SendMessage(Session.GetHabbo().GetBadgeComponent().ComposeBadgeListMessage());
        }
	}
}
