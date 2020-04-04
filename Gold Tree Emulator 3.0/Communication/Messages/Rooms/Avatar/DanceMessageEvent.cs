using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Avatar
{
	internal sealed class DanceMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            Room room = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (roomUserByHabbo != null)
                {
                    roomUserByHabbo.Unidle();
                    int num = Event.PopWiredInt32();
                    if (num < 0 || num > 4 || (!Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") && num > 1))
                    {
                        num = 0;
                    }
                    if (num > 0 && roomUserByHabbo.CarryItemID > 0)
                    {
                        roomUserByHabbo.CarryItem(0);
                    }
                    roomUserByHabbo.DanceId = num;
                    ServerMessage serverMessage = new ServerMessage(480u);
                    serverMessage.AppendInt32(roomUserByHabbo.VirtualId);
                    serverMessage.AppendInt32(num);
                    room.SendMessage(serverMessage, null);
                    if (Session.GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "DANCE")
                    {
                        GoldTree.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                    }
                }
            }
		}
	}
}
