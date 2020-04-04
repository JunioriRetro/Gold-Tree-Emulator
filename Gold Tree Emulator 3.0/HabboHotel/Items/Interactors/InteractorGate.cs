using System;
using System.Collections.Generic;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorGate : FurniInteractor
	{
		private int Modes;
		public InteractorGate(int Modes)
		{
			this.Modes = Modes - 1;
			if (this.Modes < 0)
			{
				this.Modes = 0;
			}
		}
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_1, bool bool_0)
		{
			if (bool_0)
			{
				if (this.Modes == 0)
				{
					RoomItem_0.UpdateState(false, true);
				}
				int num = 0;
				int num2 = 0;
				if (RoomItem_0.ExtraData.Length > 0)
				{
					num = int.Parse(RoomItem_0.ExtraData);
				}
				if (num <= 0)
				{
					num2 = 1;
				}
				else
				{
					if (num >= this.Modes)
					{
						num2 = 0;
					}
					else
					{
						num2 = num + 1;
					}
				}
				if (num2 == 0)
				{
					if (RoomItem_0.method_8().method_97(RoomItem_0.Int32_0, RoomItem_0.Int32_1))
					{
						return;
					}
					Dictionary<int, AffectedTile> dictionary = RoomItem_0.method_8().method_94(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, RoomItem_0.Int32_0, RoomItem_0.Int32_1, RoomItem_0.int_3);
					if (dictionary == null)
					{
						dictionary = new Dictionary<int, AffectedTile>();
					}
					foreach (AffectedTile current in dictionary.Values)
					{
						if (RoomItem_0.method_8().method_97(current.Int32_0, current.Int32_1))
						{
							return;
						}
					}
				}
				RoomItem_0.ExtraData = num2.ToString();
				RoomItem_0.method_4();
				RoomItem_0.method_8().method_22();
			}
		}
	}
}
