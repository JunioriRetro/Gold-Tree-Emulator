using System;
using GoldTree.HabboHotel.Items;
namespace GoldTree.HabboHotel.Catalogs
{
	internal sealed class EcotronReward
	{
		public uint uint_0;
		public uint uint_1;
		public uint uint_2;
		public uint uint_3;
		public EcotronReward(uint Id, uint DisplayId, uint BaseId, uint RewardLevel)
		{
			this.uint_0 = Id;
			this.uint_1 = DisplayId;
			this.uint_2 = BaseId;
			this.uint_3 = RewardLevel;
		}
		public Item method_0()
		{
			return GoldTree.GetGame().GetItemManager().method_2(this.uint_2);
		}
	}
}
