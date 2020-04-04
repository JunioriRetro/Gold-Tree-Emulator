using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Quest
{
	internal sealed class AcceptQuestMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			GoldTree.GetGame().GetQuestManager().method_7(uint_, Session);
		}
	}
}
