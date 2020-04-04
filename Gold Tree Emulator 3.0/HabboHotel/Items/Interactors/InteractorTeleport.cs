using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Pathfinding;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorTeleport : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
			if (RoomItem_0.uint_3 != 0u)
			{
				RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(RoomItem_0.uint_3);
				if (@class != null)
				{
					@class.method_3(true);
					@class.bool_1 = false;
					@class.bool_0 = true;
				}
				RoomItem_0.uint_3 = 0u;
			}
			if (RoomItem_0.uint_4 != 0u)
			{
				RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(RoomItem_0.uint_4);
				if (@class != null)
				{
					@class.method_3(true);
					@class.bool_1 = false;
					@class.bool_0 = true;
				}
				RoomItem_0.uint_4 = 0u;
			}
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
			if (RoomItem_0.uint_3 != 0u)
			{
				RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(RoomItem_0.uint_3);
				if (@class != null)
				{
					@class.method_6();
				}
				RoomItem_0.uint_3 = 0u;
			}
			if (RoomItem_0.uint_4 != 0u)
			{
				RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(RoomItem_0.uint_4);
				if (@class != null)
				{
					@class.method_6();
				}
				RoomItem_0.uint_4 = 0u;
			}
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (@class != null && @class.class34_1 == null)
			{
				if (ThreeDCoord.smethod_0(@class.Position, RoomItem_0.GStruct1_0) || ThreeDCoord.smethod_0(@class.Position, RoomItem_0.GStruct1_1))
				{
					if (RoomItem_0.uint_3 == 0u)
					{
						@class.int_19 = -1;
						RoomItem_0.uint_3 = @class.GetClient().GetHabbo().Id;
						@class.RoomItem_0 = RoomItem_0;
					}
				}
				else
				{
					if (@class.bool_0)
					{
						try
						{
							@class.MoveTo(RoomItem_0.GStruct1_1);
						}
						catch
						{
						}
					}
				}
			}
		}
	}
}
