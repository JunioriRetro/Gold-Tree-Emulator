using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Achievements;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Inventory.Achievements
{
	internal sealed class GetAchievementsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Session.SendMessage(AchievementManager.smethod_1(Session));
		}
	}
}
