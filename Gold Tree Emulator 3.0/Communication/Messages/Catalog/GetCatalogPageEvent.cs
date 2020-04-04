using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Catalogs;
namespace GoldTree.Communication.Messages.Catalog
{
	internal sealed class GetCatalogPageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			CatalogPage @class = GoldTree.GetGame().GetCatalog().method_5(Event.PopWiredInt32());
			if (@class != null && @class.bool_1 && @class.bool_0 && @class.uint_0 <= Session.GetHabbo().Rank)
			{
                if (@class.bool_2 && !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip"))
				{
					Session.SendNotification("This page is for GoldTree Club members only!");
				}
				else
				{
					Session.SendMessage(@class.message5_0);
					if (@class.string_1 == "recycler")
					{
						ServerMessage Message = new ServerMessage(507u);
						Message.AppendBoolean(true);
						Message.AppendBoolean(false);
						Session.SendMessage(Message);
					}
					else
					{
						if (@class.string_1 == "club_buy")
						{
							ServerMessage Message2 = new ServerMessage(625u);
							if (Session.GetHabbo().IsVIP)
							{
								Message2.AppendInt32(2);
								Message2.AppendInt32(4535);
								Message2.AppendStringWithBreak("HABBO_CLUB_VIP_1_MONTH");
								Message2.AppendInt32(25);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(1);
								Message2.AppendInt32(101);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Day);
								Message2.AppendInt32(4536);
								Message2.AppendStringWithBreak("HABBO_CLUB_VIP_3_MONTHS");
								Message2.AppendInt32(60);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(3);
								Message2.AppendInt32(163);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Day);
							}
							else
							{
								Message2.AppendInt32(4);
								Message2.AppendInt32(4533);
								Message2.AppendStringWithBreak("HABBO_CLUB_BASIC_1_MONTH");
								Message2.AppendInt32(15);
								Message2.AppendInt32(0);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(31);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Day);
								Message2.AppendInt32(4534);
								Message2.AppendStringWithBreak("HABBO_CLUB_BASIC_3_MONTHS");
								Message2.AppendInt32(45);
								Message2.AppendInt32(0);
								Message2.AppendInt32(0);
								Message2.AppendInt32(3);
								Message2.AppendInt32(93);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Day);
								Message2.AppendInt32(4535);
								Message2.AppendStringWithBreak("HABBO_CLUB_VIP_1_MONTH");
								Message2.AppendInt32(25);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(1);
								Message2.AppendInt32(101);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(30.0).Day);
								Message2.AppendInt32(4536);
								Message2.AppendStringWithBreak("HABBO_CLUB_VIP_3_MONTHS");
								Message2.AppendInt32(60);
								Message2.AppendInt32(0);
								Message2.AppendInt32(1);
								Message2.AppendInt32(3);
								Message2.AppendInt32(163);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Year);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Month);
								Message2.AppendInt32(DateTime.Today.AddDays(90.0).Day);
							}
							Session.SendMessage(Message2);
						}
					}
				}
			}
		}
	}
}
