using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Support;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class GetFaqTextMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			HelpTopic @class = GoldTree.GetGame().GetHelpTool().method_4(uint_);
			if (@class != null)
			{
				Session.SendMessage(GoldTree.GetGame().GetHelpTool().method_9(@class));
			}
		}
	}
}
