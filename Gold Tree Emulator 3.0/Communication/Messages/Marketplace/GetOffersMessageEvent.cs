using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Marketplace
{
	internal sealed class GetOffersMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int int_ = Event.PopWiredInt32();
			int int_2 = Event.PopWiredInt32();
			string string_ = Event.PopFixedString();
			int int_3 = Event.PopWiredInt32();
			Session.SendMessage(GoldTree.GetGame().GetCatalog().method_22().method_5(int_, int_2, string_, int_3));
		}
	}
}
