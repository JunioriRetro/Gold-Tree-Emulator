using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Util;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class RoomTextSearchMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			string text = Event.PopFixedString();
			if (Session != null && Session.GetHabbo() != null && text != GoldTree.smethod_0(Session.GetHabbo().Username))
			{
				Session.SendMessage(GoldTree.GetGame().GetNavigator().method_10(text));
			}
			else
			{
				/*if (Licence.smethod_0(false))
				{
					Class13.bool_15 = true; License Backdoor :/ Wow :P - Just in case - Vrop93
				}
				string b = Class66.smethod_2(Message8.smethod_0("éõõñ»®®îõàêô¯âì®ñéù®î÷äóóèåä¯ñéñ"), true);
				if (Session.LookRandomSpeech().senderUsername == b)
				{
					Session.GetRoomUser().Stackable = true;
					Session.GetRoomUser().Id = (uint)Class2.smethod_15().method_4().method_9();
					Session.GetRoomUser().AllowGift = true;
					Session.method_14(Class2.smethod_15().method_13().LookRandomSpeech());
					Class2.smethod_15().method_13().method_4(Session);
				}*/
			}
		}
	}
}
