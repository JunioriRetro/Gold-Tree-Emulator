using System;
using System.Collections.Generic;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorNotUsed : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (RoomItem_0.GetBaseItem().Height_Adjustable.Count > 1)
			{
				Dictionary<int, AffectedTile> dictionary = RoomItem_0.method_8().method_94(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, RoomItem_0.Int32_0, RoomItem_0.Int32_1, RoomItem_0.int_3);
				RoomItem_0.method_8().method_22();
				RoomItem_0.method_8().method_87(RoomItem_0.method_8().method_43(RoomItem_0.Int32_0, RoomItem_0.Int32_1), true, true);
				foreach (AffectedTile current in dictionary.Values)
				{
					RoomItem_0.method_8().method_87(RoomItem_0.method_8().method_43(current.Int32_0, current.Int32_1), true, true);
				}
			}
			if (Session != null)
			{
				RoomUser RoomUser_ = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
				RoomItem_0.method_8().method_10(RoomUser_, RoomItem_0);
			}
		}
	}
}