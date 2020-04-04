using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal abstract class FurniInteractor
	{
		public abstract void OnPlace(GameClient Session, RoomItem RoomItem_0);
		public abstract void OnRemove(GameClient Session, RoomItem RoomItem_0);
		public abstract void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0);
	}
}
