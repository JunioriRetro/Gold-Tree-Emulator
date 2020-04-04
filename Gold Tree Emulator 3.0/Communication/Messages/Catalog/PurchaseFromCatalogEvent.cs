using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Catalog
{
	internal sealed class PurchaseFromCatalogEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int int_ = Event.PopWiredInt32();
			uint uint_ = Event.PopWiredUInt();
			string string_ = Event.PopFixedString();

            if (Session == null || Session.GetHabbo() == null)
            {
                return;
            }

			if (Session.GetHabbo().int_24 > 1)
			{
				int num = 0;
				while (num < Session.GetHabbo().int_24 && GoldTree.GetGame().GetCatalog().method_6(Session, int_, uint_, string_, false, "", "", num == 0))
				{
					num++;
				}
				Session.GetHabbo().int_24 = 1;
			}
			else
			{
				GoldTree.GetGame().GetCatalog().method_6(Session, int_, uint_, string_, false, "", "", true);
			}
		}
	}
}
