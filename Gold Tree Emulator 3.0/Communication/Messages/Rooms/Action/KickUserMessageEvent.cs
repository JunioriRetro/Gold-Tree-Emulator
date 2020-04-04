using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Action
{
	internal sealed class KickUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.method_26(Session))
			{
				uint uint_ = Event.PopWiredUInt();
				RoomUser class2 = @class.GetRoomUserByHabbo(uint_);
                if (class2 != null && !class2.IsBot && (!@class.CheckRights(class2.GetClient(), true) && !class2.GetClient().GetHabbo().HasFuse("acc_unkickable")))
				{
					@class.method_78(Session.GetHabbo().Id);
					@class.method_47(class2.GetClient(), true, true);
				}
			}
		}
	}
}
