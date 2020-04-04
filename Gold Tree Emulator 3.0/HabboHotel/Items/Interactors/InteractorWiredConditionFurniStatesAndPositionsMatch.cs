using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTree.HabboHotel.Items.Interactors
{
    class InteractorWiredConditionFurniStatesAndPositionsMatch : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
        }
        public override void OnRemove(GameClient Session, RoomItem Item)
        {
        }
        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            if (UserHasRights && Session != null)
            {
                Item.method_9();
                ServerMessage Message = new ServerMessage(652u);
                Message.AppendInt32(0);
                Message.AppendInt32(5);
                if (Item.string_5.Length > 0)
                {
                    Message.AppendString(Item.string_5);
                }
                else
                {
                    Message.AppendInt32(0);
                }
                Message.AppendInt32(Item.GetBaseItem().Sprite);
                Message.AppendUInt(Item.uint_0);
                Message.AppendStringWithBreak("");
                Message.AppendString("J");
                if (Item.string_3.Length > 0)
                {
                    Message.AppendString(Item.string_3);
                }
                else
                {
                    Message.AppendString("HHH");
                }
                Session.SendMessage(Message);
            }
        }
    }
}
