using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
using GoldTree.Storage;
using System;
namespace GoldTree.Communication.Messages.Register
{
    internal sealed class UpdateFigureDataMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            string text = Event.PopFixedString().ToUpper();
            string text2 = GoldTree.FilterString(Event.PopFixedString());
            if (AntiMutant.ValidateLook(text2, text))
            {
                if (Session.GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "CHANGE_FIGURE")
                {
                    GoldTree.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                }
                Session.GetHabbo().Figure = text2;
                Session.GetHabbo().Gender = text.ToLower();
                using (DatabaseClient client = GoldTree.GetDatabase().GetClient())
                {
                    client.AddParamWithValue("look", text2);
                    client.AddParamWithValue("gender", text);
                    client.ExecuteQuery("UPDATE users SET look = @look, gender = @gender WHERE id = " + Session.GetHabbo().Id + " LIMIT 1;");
                }
                GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 1u, 1);
                ServerMessage serverMessage = new ServerMessage(266u);
                serverMessage.AppendInt32(-1);
                serverMessage.AppendStringWithBreak(Session.GetHabbo().Figure);
                serverMessage.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
                serverMessage.AppendStringWithBreak(Session.GetHabbo().Motto);
                serverMessage.AppendInt32(Session.GetHabbo().AchievementScore);
                serverMessage.AppendStringWithBreak("");
                Session.SendMessage(serverMessage);
                if (Session.GetHabbo().InRoom)
                {
                    Room currentRoom = Session.GetHabbo().CurrentRoom;
                    if (currentRoom != null)
                    {
                        RoomUser roomUserByHabbo = currentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                        if (roomUserByHabbo != null)
                        {
                            roomUserByHabbo.string_0 = "";
                            if (Session.GetHabbo().method_4() > 0)
                            {
                                TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
                                if (timeSpan.Seconds > 4)
                                {
                                    Session.GetHabbo().int_23 = 0;
                                }
                                if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 5)
                                {
                                    ServerMessage serverMessage2 = new ServerMessage(27u);
                                    serverMessage2.AppendInt32(Session.GetHabbo().method_4());
                                    Session.SendMessage(serverMessage2);
                                    return;
                                }
                                Session.GetHabbo().dateTime_0 = DateTime.Now;
                                Session.GetHabbo().int_23++;
                            }
                            ServerMessage serverMessage3 = new ServerMessage(266u);
                            serverMessage3.AppendInt32(roomUserByHabbo.VirtualId);
                            serverMessage3.AppendStringWithBreak(Session.GetHabbo().Figure);
                            serverMessage3.AppendStringWithBreak(Session.GetHabbo().Gender.ToLower());
                            serverMessage3.AppendStringWithBreak(Session.GetHabbo().Motto);
                            serverMessage3.AppendInt32(Session.GetHabbo().AchievementScore);
                            serverMessage3.AppendStringWithBreak("");
                            currentRoom.SendMessage(serverMessage3, null);
                        }
                    }
                }
            }
        }
    }
}
