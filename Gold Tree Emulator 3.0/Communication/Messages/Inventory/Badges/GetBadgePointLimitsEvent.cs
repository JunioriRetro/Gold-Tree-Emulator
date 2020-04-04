using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Inventory.Badges
{
	internal sealed class GetBadgePointLimitsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(443u);
			Message.AppendInt32(Session.GetHabbo().AchievementScore);
			Message.AppendStringWithBreak("");
			Session.SendMessage(Message);
		}
	}
}
