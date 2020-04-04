using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Users
{
	internal sealed class RespectUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && Session.GetHabbo().RespectPoints > 0)
			{
				RoomUser class2 = @class.GetRoomUserByHabbo(Event.PopWiredUInt());
				if (class2 != null && class2.GetClient().GetHabbo().Id != Session.GetHabbo().Id && !class2.IsBot)
				{
					Session.GetHabbo().RespectPoints--;
					Session.GetHabbo().RespectGiven++;
					class2.GetClient().GetHabbo().Respect++;
					using (DatabaseClient class3 = GoldTree.GetDatabase().GetClient())
					{
						class3.ExecuteQuery("UPDATE user_stats SET Respect = respect + 1 WHERE Id = '" + class2.GetClient().GetHabbo().Id + "' LIMIT 1");
						class3.ExecuteQuery("UPDATE user_stats SET RespectGiven = RespectGiven + 1 WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
						class3.ExecuteQuery("UPDATE user_stats SET dailyrespectpoints = dailyrespectpoints - 1 WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
					}
					ServerMessage Message = new ServerMessage(440u);
					Message.AppendUInt(class2.GetClient().GetHabbo().Id);
					Message.AppendInt32(class2.GetClient().GetHabbo().Respect);
					@class.SendMessage(Message, null);
                    Session.GetHabbo().CheckRespectGivedAchievements();
                    class2.GetClient().GetHabbo().CheckRespectReceivedAchievements();
                    if (Session.GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "GIVE_RESPECT")
					{
                        GoldTree.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
					}
				}
			}
		}
	}
}
