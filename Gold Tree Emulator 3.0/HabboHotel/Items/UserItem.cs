using System;
using GoldTree.Core;
using GoldTree.Messages;
namespace GoldTree.HabboHotel.Items
{
	internal sealed class UserItem
	{
		internal uint uint_0;
		internal uint uint_1;
		internal string string_0;
		private Item Item;
		internal UserItem(uint Id, uint BaseItem, string ExtraData)
		{
			this.uint_0 = Id;
			this.uint_1 = BaseItem;
			this.string_0 = ExtraData;
			this.Item = this.method_1();
		}
		internal void method_0(ServerMessage Message, bool bool_0)
		{
			if (this.Item == null)
			{
                Logging.LogException("Unknown base: " + this.uint_1);
			}
			Message.AppendUInt(this.uint_0);
			Message.AppendStringWithBreak(this.Item.Type.ToString().ToUpper());
			Message.AppendUInt(this.uint_0);
			Message.AppendInt32(this.Item.Sprite);
			if (this.Item.Name.Contains("a2 "))
			{
				Message.AppendInt32(3);
			}
			else
			{
				if (this.Item.Name.Contains("wallpaper"))
				{
					Message.AppendInt32(2);
				}
				else
				{
					if (this.Item.Name.Contains("landscape"))
					{
						Message.AppendInt32(4);
					}
					else
					{
						if (this.method_1().Name == "poster")
						{
							Message.AppendInt32(6);
						}
						else
						{
							if (this.method_1().Name == "song_disk")
							{
								Message.AppendInt32(8);
							}
							else
							{
								Message.AppendInt32(1);
							}
						}
					}
				}
			}
			if (this.method_1().Name == "song_disk")
			{
				Message.AppendInt32(0);
				Message.AppendStringWithBreak("");
			}
			else
			{
				if (this.method_1().Name.StartsWith("poster_"))
				{
					Message.AppendStringWithBreak(this.method_1().Name.Split(new char[]
					{
						'_'
					})[1]);
				}
				else
				{
					Message.AppendInt32(0);
					Message.AppendStringWithBreak(this.string_0);
				}
			}
			Message.AppendBoolean(this.Item.AllowRecycle);
			Message.AppendBoolean(this.Item.AllowTrade);
			Message.AppendBoolean(this.Item.AllowInventoryStack);
			Message.AppendBoolean(GoldTree.GetGame().GetCatalog().method_22().method_0(this));
			Message.AppendInt32(-1);
			if (this.Item.Type == 's')
			{
				Message.AppendStringWithBreak("");
				if (this.method_1().Name == "song_disk" && this.string_0.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(this.string_0));
				}
				else
				{
					Message.AppendInt32(0);
				}
			}
		}
		internal Item method_1()
		{
			return GoldTree.GetGame().GetItemManager().method_2(this.uint_1);
		}
	}
}
