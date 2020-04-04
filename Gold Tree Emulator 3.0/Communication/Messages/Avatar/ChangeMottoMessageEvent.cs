using System;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Avatar
{
	internal sealed class ChangeMottoMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			string text = GoldTree.FilterString(Event.PopFixedString());
			if (text.Length <= 50 && !(text != ChatCommandHandler.smethod_4(text)) && !(text == Session.GetHabbo().Motto))
			{
				Session.GetHabbo().Motto = text;
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("motto", text);
					@class.ExecuteQuery("UPDATE users SET motto = @motto WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
				}
                if (Session.GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "CHANGEMOTTO")
				{
                    GoldTree.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
				}
				ServerMessage Message = new ServerMessage(484u);
				Message.AppendInt32(-1);
				Message.AppendStringWithBreak(Session.GetHabbo().Motto);
				Session.SendMessage(Message);
                if (Session.GetHabbo().InRoom)
				{
					Room class14_ = Session.GetHabbo().CurrentRoom;
					if (class14_ == null)
					{
						return;
					}
					RoomUser class2 = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (class2 == null)
					{
						return;
					}
					ServerMessage Message2 = new ServerMessage(266u);
					Message2.AppendInt32(class2.VirtualId);
					Message2.AppendStringWithBreak(Session.GetHabbo().Figure);
					Message2.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower()); 
					Message2.AppendStringWithBreak(Session.GetHabbo().Motto);
					Message2.AppendInt32(Session.GetHabbo().AchievementScore);
					Message2.AppendStringWithBreak("");
					class14_.SendMessage(Message2, null);
				}

                Session.GetHabbo().MottoAchievementsCompleted();

                if (Session.GetHabbo().FriendStreamEnabled)
                {
                    using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
                    {
                        class2.AddParamWithValue("motto", text);
                        string look = GoldTree.FilterString(Session.GetHabbo().Figure);
                        class2.AddParamWithValue("look", look);
                        class2.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`) VALUES (NULL, '3', '" + Session.GetHabbo().Id + "', '" + Session.GetHabbo().Gender + "', @look, UNIX_TIMESTAMP(), @motto);");
                    }
                }
			}
		}
	}
}
