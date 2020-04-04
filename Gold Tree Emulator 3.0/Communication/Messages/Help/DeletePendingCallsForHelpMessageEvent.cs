using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class DeletePendingCallsForHelpMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (GoldTree.GetGame().GetModerationTool().method_9(Session.GetHabbo().Id))
			{
				GoldTree.GetGame().GetModerationTool().method_10(Session.GetHabbo().Id);
				ServerMessage Message5_ = new ServerMessage(320u);
				Session.SendMessage(Message5_);
			}
		}
	}
}
