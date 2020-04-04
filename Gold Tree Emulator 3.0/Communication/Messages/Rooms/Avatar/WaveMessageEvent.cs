using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Avatar
{
	internal sealed class WaveMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (class2 != null)
				{
					class2.Unidle();
					class2.DanceId = 0;
					ServerMessage Message = new ServerMessage(481u);
					Message.AppendInt32(class2.VirtualId);
					@class.SendMessage(Message, null);
                    if (Session.GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "WAVE")
					{
                        GoldTree.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
					}
				}
			}
		}
	}
}
