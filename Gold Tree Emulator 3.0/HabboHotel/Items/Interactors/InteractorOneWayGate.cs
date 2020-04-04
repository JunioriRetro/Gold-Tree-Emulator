using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Pathfinding;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorOneWayGate : FurniInteractor
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
					@class.method_6();
				}
				RoomItem_0.uint_3 = 0u;
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
					@class.method_3(true);
					@class.method_6();
				}
				RoomItem_0.uint_3 = 0u;
			}
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			RoomUser @class = RoomItem_0.method_8().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (@class != null && (RoomItem_0.GStruct1_2.x < RoomItem_0.method_8().RoomModel.int_4 && RoomItem_0.GStruct1_2.y < RoomItem_0.method_8().RoomModel.int_5))
			{
				if (ThreeDCoord.smethod_1(@class.Position, RoomItem_0.GStruct1_1) && @class.bool_0)
				{
					@class.MoveTo(RoomItem_0.GStruct1_1);
				}
				else
				{
					if (RoomItem_0.method_8().method_30(RoomItem_0.GStruct1_2.x, RoomItem_0.GStruct1_2.y, RoomItem_0.Double_0, true, false) && RoomItem_0.uint_3 == 0u)
					{
						RoomItem_0.uint_3 = @class.UId;
						@class.bool_0 = false;
						if (@class.bool_6 && (@class.int_10 != RoomItem_0.GStruct1_1.x || @class.int_11 != RoomItem_0.GStruct1_1.y))
						{
							@class.method_3(true);
						}
						@class.bool_1 = true;
						@class.MoveTo(RoomItem_0.GStruct1_0);
						RoomItem_0.ReqUpdate(3);
					}
				}
			}
		}
	}
}
