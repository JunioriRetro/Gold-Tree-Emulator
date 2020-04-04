using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Util;
using GoldTree.HabboHotel.Items;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Catalogs
{
	internal sealed class Marketplace
	{
		public List<uint> list_0;
		public List<MarketplaceOffers> list_1;
		public Dictionary<int, int> dictionary_0;
		public Dictionary<int, int> dictionary_1;
		public Marketplace()
		{
			this.list_0 = new List<uint>();
			this.list_1 = new List<MarketplaceOffers>();
			this.dictionary_0 = new Dictionary<int, int>();
			this.dictionary_1 = new Dictionary<int, int>();
		}
		public bool method_0(UserItem class39_0)
		{
			return class39_0.method_1().AllowTrade && class39_0.method_1().AllowMarketplaceSell;
		}
		public void method_1(GameClient Session, uint uint_0, int int_0)
		{
			UserItem @class = Session.GetHabbo().GetInventoryComponent().GetItemById(uint_0);
			if (@class == null || int_0 > ServerConfiguration.MarketplacePriceLimit || !this.method_0(@class))
			{
				ServerMessage Message = new ServerMessage(610u);
				Message.AppendBoolean(false);
				Session.SendMessage(Message);
			}
			else
			{
				int num = this.method_2((float)int_0);
				int num2 = int_0 + num;
				int num3 = 1;
				if (@class.method_1().Type == 'i')
				{
					num3++;
				}
				using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
				{
					class2.AddParamWithValue("public_name", @class.method_1().PublicName);
					class2.AddParamWithValue("extra_data", @class.string_0);
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO catalog_marketplace_offers (furni_id, item_id,user_id,asking_price,total_price,public_name,sprite_id,item_type,timestamp,extra_data) VALUES ('",
						uint_0,
						"','",
						@class.uint_1,
						"','",
						Session.GetHabbo().Id,
						"','",
						int_0,
						"','",
						num2,
						"',@public_name,'",
						@class.method_1().Sprite,
						"','",
						num3,
						"','",
						GoldTree.GetUnixTimestamp(),
						"',@extra_data)"
					}));
				}
                Session.GetHabbo().GetInventoryComponent().method_12(uint_0, 0u, true);
				ServerMessage Message2 = new ServerMessage(610u);
				Message2.AppendBoolean(true);
				Session.SendMessage(Message2);
			}
		}
		public int method_2(float float_0)
		{
			double num = (double)(float_0 / 100f);
			return (int)Math.Ceiling((double)((float)(num * (double)ServerConfiguration.MarketplaceTax)));
		}
		internal double method_3()
		{
			return GoldTree.GetUnixTimestamp() - 172800.0;
		}
		internal string method_4()
		{
			return this.method_3().ToString().Split(new char[]
			{
				','
			})[0];
		}
		public ServerMessage method_5(int int_0, int int_1, string string_0, int int_2)
		{
			DataTable dataTable = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("WHERE state = '1' AND timestamp >= " + this.method_4());
			if (int_0 >= 0)
			{
				stringBuilder.Append(" AND total_price > " + int_0);
			}
			if (int_1 >= 0)
			{
				stringBuilder.Append(" AND total_price < " + int_1);
			}
			string text;
			switch (int_2)
			{
			case 1:
				text = "ORDER BY asking_price DESC";
				goto IL_82;
			}
			text = "ORDER BY asking_price ASC";
			IL_82:
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("search_query", "%" + string_0 + "%");
				if (string_0.Length >= 1)
				{
					stringBuilder.Append(" AND public_name LIKE @search_query");
				}
				dataTable = @class.ReadDataTable(string.Concat(new string[]
				{
					"SELECT offer_id, item_type, sprite_id, total_price FROM catalog_marketplace_offers ",
					stringBuilder.ToString(),
					" ",
					text,
					" LIMIT 500"
				}));
			}
			ServerMessage Message = new ServerMessage(615u);
			this.list_1.Clear();
			this.list_0.Clear();
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					if (!this.list_0.Contains((uint)dataRow["offer_id"]))
					{
						MarketplaceOffers item = new MarketplaceOffers((uint)dataRow["offer_id"], (int)dataRow["sprite_id"], (int)dataRow["total_price"], int.Parse(dataRow["item_type"].ToString()));
						this.list_0.Add((uint)dataRow["offer_id"]);
						this.list_1.Add(item);
					}
				}
				return this.method_6(int_0, int_1);
			}
			else
			{
				Message.AppendInt32(0);
				return Message;
			}
		}
		public ServerMessage method_6(int int_0, int int_1)
		{
			Dictionary<int, MarketplaceOffers> dictionary = new Dictionary<int, MarketplaceOffers>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			ServerMessage Message = new ServerMessage(615u);
			foreach (MarketplaceOffers current in this.list_1)
			{
				if (dictionary.ContainsKey(current.int_0))
				{
					if (dictionary[current.int_0].int_1 > current.int_1)
					{
						dictionary.Remove(current.int_0);
						dictionary.Add(current.int_0, current);
					}
					int num = dictionary2[current.int_0];
					dictionary2.Remove(current.int_0);
					dictionary2.Add(current.int_0, num + 1);
				}
				else
				{
					dictionary.Add(current.int_0, current);
					dictionary2.Add(current.int_0, 1);
				}
			}
			if (dictionary.Count > 0)
			{
				Message.AppendInt32(dictionary.Count);
				using (Dictionary<int, MarketplaceOffers>.Enumerator enumerator2 = dictionary.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, MarketplaceOffers> current2 = enumerator2.Current;
						Message.AppendUInt(current2.Value.uint_0);
						Message.AppendInt32(1);
						Message.AppendInt32(current2.Value.int_2);
						Message.AppendInt32(current2.Value.int_0);
						Message.AppendStringWithBreak("");
						Message.AppendInt32(current2.Value.int_1);
						Message.AppendInt32(current2.Value.int_0);
						Message.AppendInt32(this.method_8(current2.Value.int_0));
						Message.AppendInt32(dictionary2[current2.Value.int_0]);
					}
					return Message;
				}
			}
			Message.AppendInt32(0);
			return Message;
		}
		public int method_7(int int_0)
		{
			Dictionary<int, MarketplaceOffers> dictionary = new Dictionary<int, MarketplaceOffers>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			foreach (MarketplaceOffers current in this.list_1)
			{
				if (dictionary.ContainsKey(current.int_0))
				{
					if (dictionary[current.int_0].int_1 > current.int_1)
					{
						dictionary.Remove(current.int_0);
						dictionary.Add(current.int_0, current);
					}
					int num = dictionary2[current.int_0];
					dictionary2.Remove(current.int_0);
					dictionary2.Add(current.int_0, num + 1);
				}
				else
				{
					dictionary.Add(current.int_0, current);
					dictionary2.Add(current.int_0, 1);
				}
			}
			int result;
			if (dictionary2.ContainsKey(int_0))
			{
				result = dictionary2[int_0];
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public int method_8(int int_0)
		{
			int num = 0;
			int num2 = 0;
			if (this.dictionary_0.ContainsKey(int_0) && this.dictionary_1.ContainsKey(int_0))
			{
				if (this.dictionary_1[int_0] > 0)
				{
					return this.dictionary_0[int_0] / this.dictionary_1[int_0];
				}
				else
				{
					return 0;
				}
			}
			else
			{
				try
				{
					using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
					{
						num = @class.ReadInt32("SELECT avgprice FROM catalog_marketplace_data WHERE sprite = '" + int_0 + "' AND daysago = 0 LIMIT 1;");
						num2 = @class.ReadInt32("SELECT sold FROM catalog_marketplace_data WHERE sprite = '" + int_0 + "' AND daysago = 0 LIMIT 1;");
					}
				}
				catch
				{
				}
				this.dictionary_0.Add(int_0, num);
				this.dictionary_1.Add(int_0, num2);
				if (num2 > 0)
				{
					return (int)Math.Ceiling((double)((float)(num / num2)));
				}
				else
				{
					return 0;
				}
			}
		}
		public ServerMessage method_9(uint uint_0)
		{
			int int_ = 0;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				DataTable dataTable = @class.ReadDataTable("SELECT timestamp, state, offer_id, item_type, sprite_id, total_price FROM catalog_marketplace_offers WHERE user_id = '" + uint_0 + "'");
				string text = @class.ReadDataRow("SELECT SUM(asking_price) FROM catalog_marketplace_offers WHERE state = '2' AND user_id = '" + uint_0 + "'")[0].ToString();
				if (text.Length > 0)
				{
					int_ = int.Parse(text);
				}
				ServerMessage Message = new ServerMessage(616u);
				Message.AppendInt32(int_);
				if (dataTable != null)
				{
					Message.AppendInt32(dataTable.Rows.Count);
					IEnumerator enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							int num = (int)Math.Floor(((double)dataRow["timestamp"] + 172800.0 - GoldTree.GetUnixTimestamp()) / 60.0);
							int num2 = int.Parse(dataRow["state"].ToString());
							if (num <= 0 && num2 != 2)
							{
								num2 = 3;
								num = 0;
							}
							Message.AppendUInt((uint)dataRow["offer_id"]);
							Message.AppendInt32(num2);
							Message.AppendInt32(int.Parse(dataRow["item_type"].ToString()));
							Message.AppendInt32((int)dataRow["sprite_id"]);
							Message.AppendStringWithBreak("");
							Message.AppendInt32((int)dataRow["total_price"]);
							Message.AppendInt32(num);
							Message.AppendInt32((int)dataRow["sprite_id"]);
						}
						goto IL_1DE;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				Message.AppendInt32(0);
				IL_1DE:
				return Message;
			}
		}
	}
}
