using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Engine
{
    internal sealed class MoveAvatarMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room class14_ = Session.GetHabbo().CurrentRoom;
                if (class14_ != null)
                {
                    RoomUser @class = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (@class != null && @class.bool_0)
                    {
                        int num = Event.PopWiredInt32();
                        int num2 = Event.PopWiredInt32();
                        if (num != @class.int_3 || num2 != @class.int_4)
                        {
                            if (@class.RoomUser_0 != null)
                            {
                                try
                                {
                                    if (@class.RoomUser_0.IsBot)
                                    {
                                        @class.Unidle();
                                    }
                                    @class.RoomUser_0.MoveTo(num, num2);
                                    return;
                                }
                                catch
                                {
                                    @class.RoomUser_0 = null;
                                    @class.class34_1 = null;
                                    @class.MoveTo(num, num2);
                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                    return;
                                }
                            }
                            if (@class.TeleportMode)
                            {
                                @class.int_3 = num;
                                @class.int_4 = num2;
                                @class.UpdateNeeded = true;
                            }
                            else
                            {
                                @class.MoveTo(num, num2);
                            }
                        }
                    }
                }
            }
        }
    }
}
