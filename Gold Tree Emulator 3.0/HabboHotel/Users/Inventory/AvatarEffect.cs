using System;
namespace GoldTree.HabboHotel.Users.Inventory
{
	internal sealed class AvatarEffect
	{
		public int int_0;
		public int int_1;
		public bool bool_0;
		public double double_0;
		public int Int32_0
		{
			get
			{
				int result;
				if (!this.bool_0)
				{
					result = -1;
				}
				else
				{
					double num = GoldTree.GetUnixTimestamp() - this.double_0;
					if (num >= (double)this.int_1)
					{
						result = 0;
					}
					else
					{
						result = (int)((double)this.int_1 - num);
					}
				}
				return result;
			}
		}
		public bool Boolean_0
		{
			get
			{
				return this.Int32_0 != -1 && this.Int32_0 <= 0;
			}
		}
		public AvatarEffect(int int_2, int int_3, bool bool_1, double double_1)
		{
			this.int_0 = int_2;
			this.int_1 = int_3;
			this.bool_0 = bool_1;
			this.double_0 = double_1;
		}
		public void method_0()
		{
			this.bool_0 = true;
			this.double_0 = GoldTree.GetUnixTimestamp();
		}
	}
}
