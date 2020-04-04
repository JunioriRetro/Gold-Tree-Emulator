using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.HabboHotel.Pathfinding;
namespace GoldTree.Communication.Messages.Rooms.Avatar
{
	internal sealed class LookToMessageEvent : Interface
	{
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null)
                {
                    RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (class2 != null)
                    {
                        class2.Unidle();
                        int num = Event.PopWiredInt32();
                        int num2 = Event.PopWiredInt32();
                        if (num != class2.int_3 || num2 != class2.int_4)
                        {
                            int int_ = Class107.smethod_0(class2.int_3, class2.int_4, num, num2);
                            class2.method_9(int_);
                            if (class2.class34_1 != null && class2.RoomUser_0 != null)
                            {
                                class2.RoomUser_0.method_9(int_);
                            }
                        }
                    }
                }
            }
        }
	}
}
