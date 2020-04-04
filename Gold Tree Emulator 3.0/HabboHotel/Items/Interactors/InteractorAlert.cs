using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class Class89 : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
			RoomItem_0.ExtraData = "0";
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int int_0, bool bool_0)
		{
			if (bool_0 && Item.ExtraData == "0")
			{
				Item.ExtraData = "1";
				Item.UpdateState(false, true);
				Item.ReqUpdate(4);
			}
		}
	}
}
