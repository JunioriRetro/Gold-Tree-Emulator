using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Marketplace
{
	internal sealed class MakeOfferMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().GetInventoryComponent() != null)
			{
                if (Session.GetHabbo().InRoom)
				{
					Room class14_ = Session.GetHabbo().CurrentRoom;
					RoomUser @class = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class.Boolean_3)
					{
						return;
					}
				}
				int int_ = Event.PopWiredInt32();
				Event.PopWiredInt32();
				uint uint_ = Event.PopWiredUInt();
				UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItemById(uint_);
				if (class2 != null && class2.method_1().AllowTrade)
				{
					GoldTree.GetGame().GetCatalog().method_22().method_1(Session, class2.uint_0, int_);
				}
			}
		}
	}
}
