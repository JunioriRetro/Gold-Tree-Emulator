using System;
using System.Collections.Generic;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.SoundMachine;
namespace GoldTree.Catalogs
{
	internal sealed class CatalogItem
	{
		public uint uint_0;
		public List<uint> list_0;
		public string string_0;
		public int int_0;
		public int int_1;
		public int int_2;
		public int int_3;
		internal int int_4;
		internal uint uint_1;
		internal int int_5;
		internal uint uint_2;
        internal int song_id;
        public string BadgeID;
		public bool Boolean_0
		{
			get
			{
				return this.list_0.Count > 1;
			}
		}
        public CatalogItem(uint uint_3, string string_1, string string_2, int int_6, int int_7, int int_8, int int_9, int int_10, int int_11, uint uint_4, int song_id, string BadgeID)
		{
			this.uint_0 = uint_3;
			this.string_0 = string_1;
			this.list_0 = new List<uint>();
			this.int_4 = int_10;
			string[] array = string_2.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string s = array[i];
				this.list_0.Add(uint.Parse(s));
			}
			this.int_0 = int_6;
			this.int_1 = int_7;
			this.int_2 = int_8;
			this.int_3 = int_9;
			this.int_5 = int_11;
			this.uint_2 = uint_4;
			this.uint_1 = 7u;
            this.song_id = song_id;
            this.BadgeID = BadgeID;
		}
		public Item method_0()
		{
			if (this.Boolean_0)
			{
				return null;
			}
			else
			{
				return GoldTree.GetGame().GetItemManager().method_2(this.list_0[0]);
			}
		}
		public void method_1(ServerMessage Message5_0)
		{
			if (this.Boolean_0)
			{
				throw new NotImplementedException("Multipile item ids set for catalog item #" + this.uint_0 + ", but this is usupported at this point");
			}
			Message5_0.AppendUInt(this.uint_0);
			if (this.string_0.StartsWith("disc_"))
			{
				Message5_0.AppendStringWithBreak(SongManager.GetSong(Convert.ToInt32(this.string_0.Split(new char[]
				{
					'_'
				})[1])).Name);
			}
			else
			{
				Message5_0.AppendStringWithBreak(this.string_0);
			}
			Message5_0.AppendInt32(this.int_0);
			Message5_0.AppendInt32(this.int_1);
			Message5_0.AppendInt32(this.int_2);
			Message5_0.AppendInt32(1);
			Message5_0.AppendStringWithBreak(this.method_0().Type.ToString());
			Message5_0.AppendInt32(this.method_0().Sprite);
			string text = "";
			if (this.string_0.Contains("wallpaper_single") || this.string_0.Contains("floor_single") || this.string_0.Contains("landscape_single"))
			{
				string[] array = this.string_0.Split(new char[]
				{
					'_'
				});
				text = array[2];
			}
			else
			{
				if (this.string_0.StartsWith("disc_"))
				{
					text = this.string_0.Split(new char[]
					{
						'_'
					})[1];
				}
				else
				{
					if (this.method_0().Name.StartsWith("poster_"))
					{
						text = this.method_0().Name.Split(new char[]
						{
							'_'
						})[1];
					}
				}
			}
			Message5_0.AppendStringWithBreak(text);
			Message5_0.AppendInt32(this.int_3);
			Message5_0.AppendInt32(-1);
			Message5_0.AppendInt32(this.int_5);
		}
	}
}
