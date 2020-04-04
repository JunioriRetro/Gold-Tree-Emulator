using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
using GoldTree.HabboHotel.SoundMachine;
using GoldTree.Messages;
using GoldTree.Source.HabboHotel.SoundMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldTree.Communication.Messages.SoundMachine
{
    class AddNewJukeboxCD : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (((Session != null) && (Session.GetHabbo() != null)) && (Session.GetHabbo().CurrentRoom != null))
            {
                Room currentRoom = Session.GetHabbo().CurrentRoom;
                if (currentRoom.CheckRights(Session, true))
                {
                    RoomMusicController roomMusicController = currentRoom.GetRoomMusicController();
                    if (roomMusicController.PlaylistSize < roomMusicController.PlaylistCapacity)
                    {
                        int num = Event.PopWiredInt32();
                        UserItem item = Session.GetHabbo().GetInventoryComponent().GetItemById((uint)num);
                        if ((item != null) && (item.method_1().InteractionType == "musicdisc"))
                        {
                            SongItem diskItem = new SongItem(item);
                            if (roomMusicController.AddDisk(diskItem) >= 0)
                            {
                                //diskItem.SaveToDatabase((int)currentRoom.Id); // <-- old
                                diskItem.SaveToDatabase((int)roomMusicController.LinkedItemId); // <-- new
                                Session.GetHabbo().GetInventoryComponent().method_12((uint)num, 0u, true);
                                Session.GetHabbo().GetInventoryComponent().method_9(true);
                                Session.SendMessage(JukeboxDiscksComposer.Compose(roomMusicController.PlaylistCapacity, roomMusicController.Playlist.Values.ToList<SongInstance>()));
                            }
                        }
                    }
                }
            }
        }
    }
}
