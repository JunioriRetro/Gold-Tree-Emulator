using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;

namespace GoldTree.HabboHotel.Items.Interactors
{
    class InteractorWiredKickUser : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
        {
            if (bool_0)
            {
                ServerMessage Message = new ServerMessage(651u);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
                Message.AppendUInt(RoomItem_0.uint_0);
                Message.AppendStringWithBreak(RoomItem_0.string_2);
                Message.AppendStringWithBreak("HHSAHH");
                Session.SendMessage(Message);
            }
        }
    }
}
