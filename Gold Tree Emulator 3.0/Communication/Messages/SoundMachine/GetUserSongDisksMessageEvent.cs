using System;
using System.Collections.Generic;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.Messages;
using GoldTree.HabboHotel.SoundMachine;

namespace GoldTree.Communication.Messages.SoundMachine
{
    internal sealed class GetUserSongDisksMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            List<UserItem> list = new List<UserItem>();
            foreach (UserItem current in Session.GetHabbo().GetInventoryComponent().Items)
            {
                if (current != null && !(current.method_1().Name != "song_disk") && !Session.GetHabbo().GetInventoryComponent().list_1.Contains(current.uint_0))
                {
                    list.Add(current);
                }
            }
            /*ServerMessage Message = new ServerMessage(333u);
            Message.AppendInt32(list.Count);
            foreach (UserItem current2 in list) //PHOENIX SEN OMA
            {
                int int_ = 0;
                if (current2.string_0.Length > 0)
                {
                    int_ = int.Parse(current2.string_0);
                }
                Soundtrack @class = GoldTree.GetGame().GetItemManager().method_4(int_);
                if (@class == null)
                {
                    return;
                }
                Message.AppendUInt(current2.uint_0);
                Message.AppendInt32(@class.Id);
            }*/

            ServerMessage Message = new ServerMessage(333u);
            Message.AppendInt32(list.Count);
            foreach (UserItem current2 in list) //MUN OMA
            {
                int int_ = 0;
                if (current2.string_0.Length > 0)
                {
                    int_ = int.Parse(current2.string_0);
                }
                SongData SongData = SongManager.GetSong(int_);
                if (SongData == null)
                {
                    return;
                }
                Message.AppendUInt(current2.uint_0);
                Message.AppendInt32(SongData.Id);
            }
            Session.SendMessage(Message);
        }
    }
}
