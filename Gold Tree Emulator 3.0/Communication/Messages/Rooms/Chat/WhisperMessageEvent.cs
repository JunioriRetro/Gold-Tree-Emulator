using System;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Util;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Rooms.Chat
{
	internal sealed class WhisperMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && Session != null)
			{
				if (Session.GetHabbo().IsMuted)
				{
					Session.SendNotification(GoldTreeEnvironment.GetExternalText("error_muted"));
				}
				else
				{
					if (Session.GetHabbo().HasFuse("ignore_roommute") || !@class.bool_4)
					{
						string text = GoldTree.FilterString(Event.PopFixedString());
						string text2 = text.Split(new char[]
						{
							' '
						})[0];
						string text3 = text.Substring(text2.Length + 1);
						text3 = ChatCommandHandler.smethod_4(text3);
						RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
						RoomUser class3 = @class.method_56(text2);
						if (Session.GetHabbo().method_4() > 0)
						{
							TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
							if (timeSpan.Seconds > 4)
							{
								Session.GetHabbo().int_23 = 0;
							}
							if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 5 && !class2.IsBot)
							{
								ServerMessage Message = new ServerMessage(27u);
								Message.AppendInt32(Session.GetHabbo().method_4());
								Session.SendMessage(Message);
								Session.GetHabbo().IsMuted = true;
								Session.GetHabbo().int_4 = Session.GetHabbo().method_4();
								return;
							}
							Session.GetHabbo().dateTime_0 = DateTime.Now;
							Session.GetHabbo().int_23++;
						}
						ServerMessage Message2 = new ServerMessage(25u);
						Message2.AppendInt32(class2.VirtualId);
						Message2.AppendStringWithBreak(text3);
						Message2.AppendBoolean(false);
						if (class2 != null && !class2.IsBot)
						{
							class2.GetClient().SendMessage(Message2);
						}
						class2.Unidle();
						if (class3 != null && !class3.IsBot && (class3.GetClient().GetHabbo().list_2.Count <= 0 || !class3.GetClient().GetHabbo().list_2.Contains(Session.GetHabbo().Id)))
						{
							class3.GetClient().SendMessage(Message2);
							if (ServerConfiguration.EnableChatlog && !Session.GetHabbo().IsJuniori)
							{
								using (DatabaseClient class4 = GoldTree.GetDatabase().GetClient())
								{
									class4.AddParamWithValue("message", "<Whisper to " + class3.GetClient().GetHabbo().Username + ">: " + text3);
									class4.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
										Session.GetHabbo().Id,
										"','",
										@class.Id,
										"','",
										DateTime.Now.Hour,
										"','",
										DateTime.Now.Minute,
										"',UNIX_TIMESTAMP(),@message,'",
										Session.GetHabbo().Username,
										"','",
										DateTime.Now.ToLongDateString(),
										"')"
									}));
								}
							}
						}
					}
				}
			}
		}
	}
}
