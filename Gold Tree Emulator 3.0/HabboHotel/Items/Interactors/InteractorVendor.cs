using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
using GoldTree.HabboHotel.Pathfinding;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorVendor : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
			if (RoomItem_0.uint_3 > 0u)
			{
				RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(RoomItem_0.uint_3);
				if (@class != null)
				{
					@class.bool_0 = true;
				}
			}
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
			if (RoomItem_0.uint_3 > 0u)
			{
				RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(RoomItem_0.uint_3);
				if (@class != null)
				{
					@class.bool_0 = true;
				}
			}
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (RoomItem_0.ExtraData != "1" && RoomItem_0.GetBaseItem().VendingIds.Count >= 1 && RoomItem_0.uint_3 == 0u)
			{
				if (Session != null)
				{
					RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class == null)
					{
						return;
					}
					if (!RoomItem_0.method_8().method_99(@class.int_3, @class.int_4, RoomItem_0.Int32_0, RoomItem_0.Int32_1))
					{
						if (!@class.bool_0)
						{
							return;
						}
						try
						{
							@class.MoveTo(RoomItem_0.GStruct1_1);
							return;
						}
						catch
						{
							return;
						}
					}
					RoomItem_0.uint_3 = Session.GetHabbo().Id;
					@class.bool_0 = false;
					@class.method_3(true);
					@class.method_9(Class107.smethod_0(@class.int_3, @class.int_4, RoomItem_0.Int32_0, RoomItem_0.Int32_1));
				}
				RoomItem_0.ReqUpdate(2);
				RoomItem_0.ExtraData = "1";
				RoomItem_0.UpdateState(false, true);
			}
		}
	}
}
