using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
using GoldTree.Storage;
using System;
namespace GoldTree.Communication.Messages.Rooms.Action
{
    internal sealed class AssignRightsMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            uint num = Event.PopWiredUInt();
            Room room = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (room != null)
            {
                RoomUser roomUserByHabbo = room.GetRoomUserByHabbo(num);
                if (room != null && room.CheckRights(Session, true) && roomUserByHabbo != null && !roomUserByHabbo.IsBot && !room.UsersWithRights.Contains(num))
                {
                    room.UsersWithRights.Add(num);
                    using (DatabaseClient client = GoldTree.GetDatabase().GetClient())
                    {
                        client.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO room_rights (room_id,user_id) VALUES (",
							room.Id,
							",",
							num,
							")"
						}));
                    }
                    ServerMessage serverMessage = new ServerMessage(510u);
                    serverMessage.AppendUInt(room.Id);
                    serverMessage.AppendUInt(num);
                    serverMessage.AppendStringWithBreak(roomUserByHabbo.GetClient().GetHabbo().Username);
                    Session.SendMessage(serverMessage);
                    roomUserByHabbo.AddStatus("flatctrl", "");
                    roomUserByHabbo.UpdateNeeded = true;
                    roomUserByHabbo.GetClient().SendMessage(new ServerMessage(42u));
                }
            }
        }
    }
}
