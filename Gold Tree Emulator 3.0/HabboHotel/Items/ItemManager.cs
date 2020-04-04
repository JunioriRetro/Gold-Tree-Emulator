using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.Core;
using GoldTree.Storage;
using GoldTree.HabboHotel.SoundMachine;
namespace GoldTree.HabboHotel.Items
{
	internal sealed class ItemManager
	{
		private Dictionary<uint, Item> dictionary_0;
		//private Dictionary<int, Soundtrack> dictionary_1;
		public ItemManager()
		{
			this.dictionary_0 = new Dictionary<uint, Item>();
			//this.dictionary_1 = new Dictionary<int, Soundtrack>();
		}
		public void method_0(DatabaseClient class6_0)
		{
            Logging.Write("Loading Items..");
			this.dictionary_0 = new Dictionary<uint, Item>();
			DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM furniture;");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					try
					{
                        this.dictionary_0.Add((uint)dataRow["Id"], new Item((uint)dataRow["Id"], (int)dataRow["sprite_id"], (string)dataRow["public_name"], (string)dataRow["item_name"], (string)dataRow["type"], (int)dataRow["width"], (int)dataRow["length"], (double)dataRow["stack_height"], GoldTree.StringToBoolean(dataRow["can_stack"].ToString()), GoldTree.StringToBoolean(dataRow["is_walkable"].ToString()), GoldTree.StringToBoolean(dataRow["can_sit"].ToString()), GoldTree.StringToBoolean(dataRow["allow_recycle"].ToString()), GoldTree.StringToBoolean(dataRow["allow_trade"].ToString()), GoldTree.StringToBoolean(dataRow["allow_marketplace_sell"].ToString()), GoldTree.StringToBoolean(dataRow["allow_gift"].ToString()), GoldTree.StringToBoolean(dataRow["allow_inventory_stack"].ToString()), (string)dataRow["interaction_type"], (int)dataRow["interaction_modes_count"], (string)dataRow["vending_ids"], dataRow["height_adjustable"].ToString(), Convert.ToByte((int)dataRow["EffectF"]), Convert.ToByte((int)dataRow["EffectM"]), GoldTree.StringToBoolean(dataRow["HeightOverride"].ToString())));
					}
					catch (Exception e)
					{
						Logging.WriteLine("Could not load item #" + (uint)dataRow["Id"] + ", please verify the data is okay.");
                        Logging.LogItemError(e.Message);
					}
				}
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
			/*Logging.smethod_0("Loading Soundtracks.."); //OMA LUOTU :3
			this.dictionary_1 = new Dictionary<int, Soundtrack>();
			DataTable dataTable2 = class6_0.ReadDataTable("SELECT * FROM soundtracks;");
			if (dataTable2 != null)
			{
				foreach (DataRow dataRow in dataTable2.Rows)
				{
					try
					{
						this.dictionary_1.Add((int)dataRow["Id"], new Soundtrack((int)dataRow["Id"], (string)dataRow["name"], (string)dataRow["author"], (string)dataRow["track"], (int)dataRow["length"]));
					}
					catch (Exception)
					{
						Logging.WriteLine("Could not load item #" + (uint)dataRow["Id"] + ", please verify the data is okay.");
					}
				}
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);*/
            Logging.Write("Loading Soundtracks..");
            SongManager.Initialize();
            Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		public bool method_1(uint uint_0)
		{
			return this.dictionary_0.ContainsKey(uint_0);
		}
		public Item method_2(uint uint_0)
		{
			Item result;
			if (this.method_1(uint_0))
			{
				result = this.dictionary_0[uint_0];
			}
			else
			{
				result = null;
			}
			return result;
		}
		/*public bool method_3(int int_0)
		{
			return this.dictionary_1.ContainsKey(int_0);
		}
		public Soundtrack method_4(int int_0)
		{
			Soundtrack result;
			if (this.method_3(int_0))
			{
				result = this.dictionary_1[int_0];
			}
			else
			{
				result = null;
			}
			return result;
		}*/
	}
}
