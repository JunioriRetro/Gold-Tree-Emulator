using System;
namespace GoldTree.HabboHotel.Rooms
{
	public sealed class AffectedTile
	{
		private int int_0;
		private int int_1;
		private int int_2;
		public int Int32_0
		{
			get
			{
				return this.int_0;
			}
		}
		public int Int32_1
		{
			get
            {
				return this.int_1;
			}
		}
		public int Int32_2
		{
			get
			{
            	return this.int_2;
			}
		}
		public AffectedTile(int x, int y, int i)
		{
			this.int_0 = x;
			this.int_1 = y;
			this.int_2 = i;
        }
	}
}