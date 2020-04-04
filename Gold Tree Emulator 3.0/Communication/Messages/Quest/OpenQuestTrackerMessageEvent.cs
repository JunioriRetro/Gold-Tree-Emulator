using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Quest
{
	internal sealed class OpenQuestTrackerMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			GoldTree.GetGame().GetQuestManager().method_4(Session);
		}
	}
}
