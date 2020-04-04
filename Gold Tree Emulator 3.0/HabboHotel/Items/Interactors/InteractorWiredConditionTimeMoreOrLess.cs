using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTree.HabboHotel.Items.Interactors
{
    class InteractorWiredConditionTimeMoreOrLess : FurniInteractor
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
                ServerMessage Message = new ServerMessage(650u);
                Message.AppendInt32(0);
                Message.AppendInt32(5);
                Message.AppendInt32(0);
                Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
                Message.AppendUInt(RoomItem_0.uint_0);
                Message.AppendStringWithBreak("");
                Message.AppendString("I");
                if (RoomItem_0.string_3.Length > 0)
                {
                    Message.AppendString(RoomItem_0.string_3);
                }
                else
                {
                    Message.AppendString("I");
                }
                Message.AppendStringWithBreak("IKH");
                Session.SendMessage(Message);
            }
        }
    }
}
