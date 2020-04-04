using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Chat
{
	internal sealed class ChatMessageEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room room = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

                if (room != null)
                {
                    RoomUser user = room.GetRoomUserByHabbo(Session.GetHabbo().Id);

                    if (user != null)
                        user.HandleSpeech(Session, GoldTree.FilterString(Event.PopFixedString()), false);
                }
            }
        }
	}
}
