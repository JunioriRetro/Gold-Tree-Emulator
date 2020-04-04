using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Action
{
	internal sealed class BanUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				uint uint_ = Event.PopWiredUInt();
				RoomUser class2 = @class.GetRoomUserByHabbo(uint_);
				if (class2 != null && !class2.IsBot && !class2.GetClient().GetHabbo().HasFuse("acc_unbannable"))
				{
					@class.method_70(uint_);
					@class.method_47(class2.GetClient(), true, true);
				}
			}
		}
	}
}
