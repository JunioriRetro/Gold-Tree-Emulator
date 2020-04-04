using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class SearchFaqsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			string text = GoldTree.FilterString(Event.PopFixedString());
			if (text.Length >= 1)
			{
				Session.SendMessage(GoldTree.GetGame().GetHelpTool().method_10(text));
			}
		}
	}
}
