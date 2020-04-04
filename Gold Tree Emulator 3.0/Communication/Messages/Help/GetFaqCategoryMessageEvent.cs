using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Support;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class GetFaqCategoryMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			HelpCategory @class = GoldTree.GetGame().GetHelpTool().method_1(uint_);
			if (@class != null)
			{
				Session.SendMessage(GoldTree.GetGame().GetHelpTool().method_11(@class));
			}
		}
	}
}
