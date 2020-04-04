using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorDice : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			RoomUser @class = null;
			if (Session != null)
			{
				@class = RoomItem_0.method_8().GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (@class == null)
				{
					return;
				}
			}
			if (Session == null || RoomItem_0.method_8().method_99(RoomItem_0.Int32_0, RoomItem_0.Int32_1, @class.int_3, @class.int_4))
			{
				if (RoomItem_0.ExtraData != "-1")
				{
					if (int_0 == -1)
					{
						RoomItem_0.ExtraData = "0";
						RoomItem_0.method_4();
					}
					else
					{
						RoomItem_0.uint_3 = @class.UId;
						RoomItem_0.ExtraData = "-1";
						RoomItem_0.UpdateState(false, true);
						RoomItem_0.ReqUpdate(4);
					}
				}
			}
			else
			{
				if (Session != null && @class != null && @class.bool_0)
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
