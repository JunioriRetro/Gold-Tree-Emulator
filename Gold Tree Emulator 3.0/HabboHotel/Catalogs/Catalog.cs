using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using GoldTree.Core;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Pets;
using GoldTree.Util;
using GoldTree.Catalogs;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Catalogs
{
	internal sealed class Catalog
	{
		public Dictionary<int, CatalogPage> dictionary_0;
		public List<EcotronReward> list_0;
		private VoucherHandler VoucherHandler_0;
		private Marketplace class43_0;
		private ServerMessage[] Message5_0;
		private uint uint_0 = 0u;
		private readonly object object_0 = new object();
		public Catalog()
		{
			this.VoucherHandler_0 = new VoucherHandler();
			this.class43_0 = new Marketplace();
		}
		public void method_0(DatabaseClient class6_0)
		{
			Logging.Write("Loading Catalogue..");
			this.dictionary_0 = new Dictionary<int, CatalogPage>();
			this.list_0 = new List<EcotronReward>();
            DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM catalog_pages WHERE order_num >= '0' ORDER BY order_num ASC");
			DataTable dataTable2 = class6_0.ReadDataTable("SELECT * FROM ecotron_rewards ORDER BY item_id");
            DataTable dataTable4 = class6_0.ReadDataTable("SELECT * FROM catalog_pages WHERE order_num = '-1' ORDER BY caption ASC");
			try
			{
				this.uint_0 = (uint)class6_0.ReadDataRow("SELECT ID FROM items ORDER BY ID DESC LIMIT 1")[0];
			}
			catch
			{
				this.uint_0 = 0u;
			}
			this.uint_0 += 1u;
			Hashtable hashtable = new Hashtable();
			DataTable dataTable3 = class6_0.ReadDataTable("SELECT * FROM catalog_items");
			if (dataTable3 != null)
			{
				foreach (DataRow dataRow in dataTable3.Rows)
				{
					if (!(dataRow["item_ids"].ToString() == "") && (int)dataRow["amount"] > 0)
					{
                        string BadgeID = dataRow["BadgeID"].ToString();
                        if (string.IsNullOrEmpty(BadgeID) || string.IsNullOrWhiteSpace(BadgeID)) BadgeID = string.Empty;
                        hashtable.Add((uint)dataRow["Id"], new CatalogItem((uint)dataRow["Id"], (string)dataRow["catalog_name"], (string)dataRow["item_ids"], (int)dataRow["cost_credits"], (int)dataRow["cost_pixels"], (int)dataRow["cost_snow"], (int)dataRow["amount"], (int)dataRow["page_id"], GoldTree.StringToInt(dataRow["vip"].ToString()), (uint)dataRow["achievement"], (int)dataRow["song_id"], BadgeID));
					}
				}
			}
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					bool bool_ = false;
					bool bool_2 = false;
					if (dataRow["visible"].ToString() == "1")
					{
						bool_ = true;
					}
					if (dataRow["enabled"].ToString() == "1")
					{
						bool_2 = true;
					}
					this.dictionary_0.Add((int)dataRow["Id"], new CatalogPage((int)dataRow["Id"], (int)dataRow["parent_id"], (string)dataRow["caption"], bool_, bool_2, (uint)dataRow["min_rank"], GoldTree.StringToBoolean(dataRow["club_only"].ToString()), (int)dataRow["icon_color"], (int)dataRow["icon_image"], (string)dataRow["page_layout"], (string)dataRow["page_headline"], (string)dataRow["page_teaser"], (string)dataRow["page_special"], (string)dataRow["page_text1"], (string)dataRow["page_text2"], (string)dataRow["page_text_details"], (string)dataRow["page_text_teaser"], (string)dataRow["page_link_description"], (string)dataRow["page_link_pagename"], ref hashtable));
				}
			}
            if (dataTable4 != null)
            {
                foreach (DataRow dataRow in dataTable4.Rows)
                {
                    bool bool_ = false;
                    bool bool_2 = false;
                    if (dataRow["visible"].ToString() == "1")
                    {
                        bool_ = true;
                    }
                    if (dataRow["enabled"].ToString() == "1")
                    {
                        bool_2 = true;
                    }
                    this.dictionary_0.Add((int)dataRow["Id"], new CatalogPage((int)dataRow["Id"], (int)dataRow["parent_id"], (string)dataRow["caption"], bool_, bool_2, (uint)dataRow["min_rank"], GoldTree.StringToBoolean(dataRow["club_only"].ToString()), (int)dataRow["icon_color"], (int)dataRow["icon_image"], (string)dataRow["page_layout"], (string)dataRow["page_headline"], (string)dataRow["page_teaser"], (string)dataRow["page_special"], (string)dataRow["page_text1"], (string)dataRow["page_text2"], (string)dataRow["page_text_details"], (string)dataRow["page_text_teaser"], (string)dataRow["page_link_description"], (string)dataRow["page_link_pagename"], ref hashtable));
                }
            }
			if (dataTable2 != null)
			{
				foreach (DataRow dataRow in dataTable2.Rows)
				{
					this.list_0.Add(new EcotronReward((uint)dataRow["Id"], (uint)dataRow["display_id"], (uint)dataRow["item_id"], (uint)dataRow["reward_level"]));
				}
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		internal void method_1()
		{
			Logging.Write("Loading Catalogue Cache..");
			int num = GoldTree.GetGame().GetRoleManager().dictionary_2.Count + 1;
			this.Message5_0 = new ServerMessage[num];
			for (int i = 1; i < num; i++)
			{
				this.Message5_0[i] = this.method_17(i);
			}
			foreach (CatalogPage current in this.dictionary_0.Values)
			{
				current.method_0();
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		public CatalogItem method_2(uint uint_1)
		{
			foreach (CatalogPage current in this.dictionary_0.Values)
			{
				foreach (CatalogItem current2 in current.list_0)
				{
					if (current2.uint_0 == uint_1)
					{
						return current2;
					}
				}
			}
			return null;
		}
		public bool method_3(uint uint_1)
		{
			DataRow dataRow = null;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				dataRow = @class.ReadDataRow("SELECT Id FROM catalog_items WHERE item_ids = '" + uint_1 + "' LIMIT 1");
			}
			return dataRow != null;
		}
		public int method_4(int int_0, int int_1)
		{
			int num = 0;
			foreach (CatalogPage current in this.dictionary_0.Values)
			{
				if ((ulong)current.uint_0 <= (ulong)((long)int_0) && current.int_1 == int_1)
				{
					num++;
				}
			}
			return num;
		}
		public CatalogPage method_5(int int_0)
		{
			if (!this.dictionary_0.ContainsKey(int_0))
			{
				return null;
			}
			else
			{
				return this.dictionary_0[int_0];
			}
		}
		public bool method_6(GameClient Session, int int_0, uint uint_1, string string_0, bool bool_0, string string_1, string string_2, bool bool_1)
		{
			CatalogPage @class = this.method_5(int_0);
			if (@class == null || !@class.bool_1 || !@class.bool_0 || @class.uint_0 > Session.GetHabbo().Rank)
			{
				return false;
			}
			else
			{
                if (@class.bool_2 && (!Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club") || !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip")))
				{
					return false;
				}
				else
				{
					CatalogItem class2 = @class.method_1(uint_1);
					if (class2 == null)
					{
						return false;
					}
					else
					{
						uint num = 0u;
						if (bool_0)
						{
							if (!class2.method_0().AllowGift)
							{
								return false;
							}
							if (Session.GetHabbo().method_4() > 0)
							{
								TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
								if (timeSpan.Seconds > 4)
								{
									Session.GetHabbo().int_23 = 0;
								}
								if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 3)
								{
									Session.GetHabbo().bool_15 = true;
									return false;
								}
								if (Session.GetHabbo().bool_15 && timeSpan.Seconds < Session.GetHabbo().method_4())
								{
									return false;
								}
								Session.GetHabbo().bool_15 = false;
								Session.GetHabbo().dateTime_0 = DateTime.Now;
								Session.GetHabbo().int_23++;
							}
							using (DatabaseClient class3 = GoldTree.GetDatabase().GetClient())
							{
								class3.AddParamWithValue("gift_user", string_1);
								try
								{
									num = (uint)class3.ReadDataRow("SELECT Id FROM users WHERE username = @gift_user LIMIT 1")[0];
								}
								catch (Exception)
								{
								}
							}
							if (num == 0u)
							{
								ServerMessage Message = new ServerMessage(76u);
								Message.AppendBoolean(true);
								Message.AppendStringWithBreak(string_1);
								Session.SendMessage(Message);
								return false;
							}
						}
						bool flag = false;
						bool flag2 = false;
						int int_ = class2.int_2;
						if (Session.GetHabbo().Credits < class2.int_0)
						{
							flag = true;
						}
						if ((int_ == 0 && Session.GetHabbo().ActivityPoints < class2.int_1) || (int_ > 0 && Session.GetHabbo().VipPoints < class2.int_1))
						{
							flag2 = true;
						}
						if (flag || flag2)
						{
							ServerMessage Message2 = new ServerMessage(68u);
							Message2.AppendBoolean(flag);
							Message2.AppendBoolean(flag2);
							Session.SendMessage(Message2);
							return false;
						}
						else
						{
							if (bool_0 && class2.method_0().Type == 'e')
							{
								Session.SendNotification("You can not send this item as a gift.");
								return false;
							}
							else
							{
								string text = class2.method_0().InteractionType.ToLower();
								if (text != null)
								{
									if (!(text == "pet"))
									{
										if (text == "roomeffect")
										{
											double num2 = 0.0;
											try
											{
												num2 = double.Parse(string_0);
											}
											catch (Exception)
											{
											}
											string_0 = num2.ToString().Replace(',', '.');
											goto IL_4FC;
										}
										if (text == "postit")
										{
											string_0 = "FFFF33";
											goto IL_4FC;
										}
										if (text == "dimmer")
										{
											string_0 = "1,1,1,#000000,255";
											goto IL_4FC;
										}
										if (text == "trophy")
										{
											string_0 = string.Concat(new object[]
											{
												Session.GetHabbo().Username,
												Convert.ToChar(9),
												DateTime.Now.Day,
												"-",
												DateTime.Now.Month,
												"-",
												DateTime.Now.Year,
												Convert.ToChar(9),
												ChatCommandHandler.smethod_4(GoldTree.DoFilter(string_0, true, true))
											});
											goto IL_4FC;
										}
                                        if (text == "musicdisc")
                                        {
                                            string_0 = class2.song_id.ToString();
                                            goto IL_4FC;
                                        }
									}
									else
									{
										try
										{
											string[] array = string_0.Split(new char[]
											{
												'\n'
											});
											string string_3 = array[0];
											string text2 = array[1];
											string text3 = array[2];
											int.Parse(text2);
											if (!this.method_8(string_3))
											{
												return false;
											}
											if (text2.Length > 2)
											{
												return false;
											}
											if (text3.Length != 6)
											{
												return false;
											}
											goto IL_4FC;
										}
										catch (Exception)
										{
											return false;
										}
									}
								}
								if (class2.string_0.StartsWith("disc_"))
								{
									string_0 = class2.string_0.Split(new char[]
									{
										'_'
									})[1];
								}
								else
								{
									string_0 = "";
								}
								IL_4FC:
								if (class2.int_0 > 0)
								{
									Session.GetHabbo().Credits -= class2.int_0;
									Session.GetHabbo().UpdateCredits(true);
								}
								if (class2.int_1 > 0 && int_ == 0)
								{
									Session.GetHabbo().ActivityPoints -= class2.int_1;
									Session.GetHabbo().UpdateActivityPoints(true);
								}
								else
								{
									if (class2.int_1 > 0 && int_ > 0)
									{
										Session.GetHabbo().VipPoints -= class2.int_1;
										Session.GetHabbo().method_16(0);
										Session.GetHabbo().UpdateVipPoints(false, true);
									}
								}
								ServerMessage Message3 = new ServerMessage(67u);
								Message3.AppendUInt(class2.method_0().UInt32_0);
								Message3.AppendStringWithBreak(class2.method_0().Name);
								Message3.AppendInt32(class2.int_0);
								Message3.AppendInt32(class2.int_1);
								Message3.AppendInt32(class2.int_2);
								if (bool_1)
								{
									Message3.AppendInt32(1);
								}
								else
								{
									Message3.AppendInt32(0);
								}
								Message3.AppendStringWithBreak(class2.method_0().Type.ToString());
								Message3.AppendInt32(class2.method_0().Sprite);
								Message3.AppendStringWithBreak("");
								Message3.AppendInt32(1);
								Message3.AppendInt32(-1);
								Message3.AppendStringWithBreak("");
								Session.SendMessage(Message3);
								if (bool_0)
								{
									uint num3 = this.method_14();
									Item class4 = this.method_10();
									using (DatabaseClient class3 = GoldTree.GetDatabase().GetClient())
									{
										class3.AddParamWithValue("gift_message", "!" + ChatCommandHandler.smethod_4(GoldTree.DoFilter(string_2, true, true)) + " - " + Session.GetHabbo().Username);
										class3.AddParamWithValue("extra_data", string_0);
										class3.ExecuteQuery(string.Concat(new object[]
										{
											"INSERT INTO items (Id,user_id,base_item,wall_pos) VALUES ('",
											num3,
											"','",
											num,
											"','",
											class4.UInt32_0,
											"','')"
										}));
                                        class3.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
							num3,
							"',@gift_message)"
						}));
										class3.ExecuteQuery(string.Concat(new object[]
										{
											"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
											num3,
											"','",
											class2.method_0().UInt32_0,
											"','",
											class2.int_3,
											"',@extra_data)"
										}));
									}
									GameClient class5 = GoldTree.GetGame().GetClientManager().method_2(num);
									if (class5 != null)
									{
										class5.SendNotification("You have received a gift! Check your inventory.");
										class5.GetHabbo().GetInventoryComponent().method_9(true);
										class5.GetHabbo().GiftsReceived++;
                                        class5.GetHabbo().CheckGiftReceivedAchievements();
									}
									Session.GetHabbo().GiftsGiven++;
                                    Session.GetHabbo().CheckGiftGivenAchievements();
									Session.SendNotification("Gift sent successfully!");
									return true;
								}
								else
								{
									this.method_9(Session, class2.method_0(), class2.int_3, string_0, true, 0u);
									if (class2.uint_2 > 0u)
									{
										GoldTree.GetGame().GetAchievementManager().addAchievement(Session, class2.uint_2, 1);
									}
                                    if (!string.IsNullOrEmpty(class2.BadgeID))
                                    {
                                        Session.GetHabbo().GetBadgeComponent().SendBadge(Session, class2.BadgeID, true);
                                    }
									return true;
								}
							}
						}
					}
				}
			}
		}
		public void method_7(string string_0, uint uint_1, uint uint_2, int int_0)
		{
			CatalogPage @class = this.method_5(int_0);
			CatalogItem class2 = @class.method_1(uint_2);
			uint num = this.method_14();
			Item class3 = this.method_10();
			using (DatabaseClient class4 = GoldTree.GetDatabase().GetClient())
			{
				class4.AddParamWithValue("gift_message", "!" + ChatCommandHandler.smethod_4(GoldTree.DoFilter(string_0, true, true)));
				class4.ExecuteQuery(string.Concat(new object[]
				{
					"INSERT INTO items (Id,user_id,base_item,wall_pos) VALUES ('",
					num,
					"','",
					uint_1,
					"','",
					class3.UInt32_0,
					"','')"
				}));
                class4.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO items_extra_data (item_id,extra_data) VALUES ('",
							num,
							"',@gift_message)"
						}));
				class4.ExecuteQuery(string.Concat(new object[]
				{
					"INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES ('",
					num,
					"','",
					class2.method_0().UInt32_0,
					"','",
					class2.int_3,
					"','')"
				}));
			}
			GameClient class5 = GoldTree.GetGame().GetClientManager().method_2(uint_1);
			if (class5 != null)
			{
				class5.SendNotification("You have received a gift! Check your inventory.");
				class5.GetHabbo().GetInventoryComponent().method_9(true);
			}
		}
		public bool method_8(string string_0)
		{
			return string_0.Length >= 1 && string_0.Length <= 16 && GoldTree.smethod_9(string_0) && !(string_0 != ChatCommandHandler.smethod_4(string_0));
		}
        public void method_9(GameClient Session, Item Item, int int_0, string string_0, bool bool_0, uint uint_1)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                string text = Item.Type.ToString();
                if (text != null)
                {
                    if (text == "i" || text == "s")
                    {
                        int i = 0;
                        while (i < int_0)
                        {
                            uint num;
                            if (!bool_0 && uint_1 > 0u)
                            {
                                num = uint_1;
                            }
                            else
                            {
                                num = this.method_14();
                            }
                            text = Item.InteractionType.ToLower();
                            if (text == null)
                            {
                                goto IL_4CF;
                            }
                            if (!(text == "pet"))
                            {
                                if (!(text == "teleport"))
                                {
                                    if (!(text == "dimmer"))
                                    {
                                        goto IL_4CF;
                                    }
                                    using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                                    {
                                        @class.ExecuteQuery("INSERT INTO room_items_moodlight (item_id,enabled,current_preset,preset_one,preset_two,preset_three) VALUES ('" + num + "','0','1','#000000,255,0','#000000,255,0','#000000,255,0')");
                                    }
                                    Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, string_0, bool_0);
                                }
                                else
                                {
                                    uint num2 = this.method_14();
                                    using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                                    {
                                        @class.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO tele_links (tele_one_id,tele_two_id) VALUES ('",
										num,
										"','",
										num2,
										"')"
									}));
                                        @class.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO tele_links (tele_one_id,tele_two_id) VALUES ('",
										num2,
										"','",
										num,
										"')"
									}));
                                    }
                                    Session.GetHabbo().GetInventoryComponent().method_11(num2, Item.UInt32_0, "0", bool_0);
                                    Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, "0", bool_0);
                                }
                            }
                            else
                            {
                                string[] array = string_0.Split(new char[]
							{
								'\n'
							});
                                Pet class15_ = this.method_11(Session.GetHabbo().Id, array[0], Convert.ToInt32(Item.Name.Split(new char[]
							{
								't'
							})[1]), array[1], array[2]);
                                Session.GetHabbo().GetInventoryComponent().AddPet(class15_);
                                Session.GetHabbo().GetInventoryComponent().method_11(num, 320u, "0", bool_0);
                            }
                        IL_4EA:
                            ServerMessage Message = new ServerMessage(832u);
                            Message.AppendInt32(1);
                            if (Item.InteractionType.ToLower() == "pet")
                            {
                                Message.AppendInt32(3);
                                Session.GetHabbo().NewPetsBuyed++;
                                Session.GetHabbo().CheckPetCountAchievements();
                            }
                            else
                            {
                                if (Item.Type.ToString() == "i")
                                {
                                    Message.AppendInt32(2);
                                }
                                else
                                {
                                    Message.AppendInt32(1);
                                }
                            }
                            Message.AppendInt32(1);
                            Message.AppendUInt(num);
                            Session.SendMessage(Message);
                            i++;
                            continue;
                        IL_4CF:
                            Session.GetHabbo().GetInventoryComponent().method_11(num, Item.UInt32_0, string_0, bool_0);
                            goto IL_4EA;
                        }
                        Session.GetHabbo().GetInventoryComponent().method_9(false);
                        return;
                    }
                    if (text == "e")
                    {
                        for (int i = 0; i < int_0; i++)
                        {
                            Session.GetHabbo().GetEffectsInventoryComponent().method_0(Item.Sprite, 3600);
                        }
                        return;
                    }
                    if (text == "h")
                    {
                        for (int i = 0; i < int_0; i++)
                        {
                            Session.GetHabbo().GetSubscriptionManager().method_3("habbo_club", 2678400);
                            Session.GetHabbo().CheckHCAchievements();
                        }
                        ServerMessage Message2 = new ServerMessage(7u);
                        Message2.AppendStringWithBreak("habbo_club");
                        if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                        {
                            double num3 = (double)Session.GetHabbo().GetSubscriptionManager().GetSubscriptionByType("habbo_club").ExpirationTime;
                            double num4 = num3 - GoldTree.GetUnixTimestamp();
                            int num5 = (int)Math.Ceiling(num4 / 86400.0);
                            int num6 = num5 / 31;
                            if (num6 >= 1)
                            {
                                num6--;
                            }
                            Message2.AppendInt32(num5 - num6 * 31);
                            Message2.AppendBoolean(true);
                            Message2.AppendInt32(num6);
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Message2.AppendInt32(0);
                            }
                        }
                        Session.SendMessage(Message2);
                        ServerMessage Message3 = new ServerMessage(2u);
                        if (Session.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes)
                        {
                            Message3.AppendInt32(2);
                        }
                        else
                        {
                            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                            {
                                Message3.AppendInt32(1);
                            }
                            else
                            {
                                Message3.AppendInt32(0);
                            }
                        }
                        if (Session.GetHabbo().HasFuse("acc_anyroomowner"))
                        {
                            Message3.AppendInt32(7);
                        }
                        else
                        {
                            if (Session.GetHabbo().HasFuse("acc_anyroomrights"))
                            {
                                Message3.AppendInt32(5);
                            }
                            else
                            {
                                if (Session.GetHabbo().HasFuse("acc_supporttool"))
                                {
                                    Message3.AppendInt32(4);
                                }
                                else
                                {
                                    if (Session.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes || Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                                    {
                                        Message3.AppendInt32(2);
                                    }
                                    else
                                    {
                                        Message3.AppendInt32(0);
                                    }
                                }
                            }
                        }
                        Session.SendMessage(Message3);
                        return;
                    }
                }
                Session.SendNotification("Something went wrong! The item type could not be processed. Please do not try to buy this item anymore, instead inform support as soon as possible.");
            }
        }
		public Item method_10()
		{
			switch (GoldTree.smethod_5(0, 6))
			{
			case 0:
			{
                return GoldTree.GetGame().GetItemManager().method_2(164u);
			}
			case 1:
			{
				return GoldTree.GetGame().GetItemManager().method_2(165u);
			}
			case 2:
			{
				return GoldTree.GetGame().GetItemManager().method_2(166u);
			}
			case 3:
			{
				return GoldTree.GetGame().GetItemManager().method_2(167u);
			}
			case 4:
			{
                return GoldTree.GetGame().GetItemManager().method_2(168u);
			}
			case 5:
			{
                return GoldTree.GetGame().GetItemManager().method_2(169u);
			}
			case 6:
			{
                return GoldTree.GetGame().GetItemManager().method_2(170u);
			}
            default:
            {
                return null;
            }
			}
		}
		public Pet method_11(uint uint_1, string string_0, int int_0, string string_1, string string_2)
		{
			return new Pet(this.method_14(), uint_1, 0u, string_0, (uint)int_0, string_1, string_2, 0, 100, 100, 0, GoldTree.GetUnixTimestamp(), 0, 0, 0.0)
			{
				DBState = DatabaseUpdateState.NeedsInsert
			};
		}
		public Pet method_12(DataRow dataRow_0)
		{
			if (dataRow_0 == null)
			{
				return null;
			}
			else
			{
				return new Pet((uint)dataRow_0["Id"], (uint)dataRow_0["user_id"], (uint)dataRow_0["room_id"], (string)dataRow_0["name"], (uint)dataRow_0["type"], (string)dataRow_0["race"], (string)dataRow_0["color"], (int)dataRow_0["expirience"], (int)dataRow_0["energy"], (int)dataRow_0["nutrition"], (int)dataRow_0["respect"], (double)dataRow_0["createstamp"], (int)dataRow_0["x"], (int)dataRow_0["y"], (double)dataRow_0["z"]);
			}
		}
		internal Pet method_13(DataRow dataRow_0, uint uint_1)
		{
			if (dataRow_0 == null)
			{
				return null;
			}
			else
			{
				return new Pet(uint_1, (uint)dataRow_0["user_id"], (uint)dataRow_0["room_id"], (string)dataRow_0["name"], (uint)dataRow_0["type"], (string)dataRow_0["race"], (string)dataRow_0["color"], (int)dataRow_0["expirience"], (int)dataRow_0["energy"], (int)dataRow_0["nutrition"], (int)dataRow_0["respect"], (double)dataRow_0["createstamp"], (int)dataRow_0["x"], (int)dataRow_0["y"], (double)dataRow_0["z"]);
			}
		}
		internal uint method_14()
		{
			lock (this.object_0)
			{
				return this.uint_0++;
			}
		}
		public EcotronReward method_15()
		{
			uint uint_ = 1u;
			if (GoldTree.smethod_5(1, 2000) == 2000)
			{
				uint_ = 5u;
			}
			else
			{
				if (GoldTree.smethod_5(1, 200) == 200)
				{
					uint_ = 4u;
				}
				else
				{
					if (GoldTree.smethod_5(1, 40) == 40)
					{
						uint_ = 3u;
					}
					else
					{
						if (GoldTree.smethod_5(1, 4) == 4)
						{
							uint_ = 2u;
						}
					}
				}
			}
			List<EcotronReward> list = this.method_16(uint_);
			if (list != null && list.Count >= 1)
			{
				return list[GoldTree.smethod_5(0, list.Count - 1)];
			}
			else
			{
				return new EcotronReward(0u, 0u, 1479u, 0u);
			}
		}
		public List<EcotronReward> method_16(uint uint_1)
		{
			List<EcotronReward> list = new List<EcotronReward>();
			foreach (EcotronReward current in this.list_0)
			{
				if (current.uint_3 == uint_1)
				{
					list.Add(current);
				}
			}
			return list;
		}
		public ServerMessage method_17(int int_0)
		{
			ServerMessage Message = new ServerMessage(126u);
			Message.AppendBoolean(true);
			Message.AppendInt32(0);
			Message.AppendInt32(0);
			Message.AppendInt32(-1);
			Message.AppendStringWithBreak("");
			Message.AppendInt32(this.method_4(int_0, -1));
			Message.AppendBoolean(true);
			foreach (CatalogPage current in this.dictionary_0.Values)
			{
				if (current.int_1 == -1)
				{
					current.method_2(int_0, Message);
					foreach (CatalogPage current2 in this.dictionary_0.Values)
					{
						if (current2.int_1 == current.Int32_0)
						{
							current2.method_2(int_0, Message);
						}
					}
				}
			}
			return Message;
		}
		internal ServerMessage method_18(uint uint_1)
		{
			if (uint_1 < 1u)
			{
				uint_1 = 1u;
			}
			if ((ulong)uint_1 > (ulong)((long)GoldTree.GetGame().GetRoleManager().dictionary_2.Count))
			{
				uint_1 = (uint)GoldTree.GetGame().GetRoleManager().dictionary_2.Count;
			}
			return this.Message5_0[(int)((UIntPtr)uint_1)];
		}
		public ServerMessage method_19(CatalogPage class48_0)
		{
			ServerMessage Message = new ServerMessage(127u);
			Message.AppendInt32(class48_0.Int32_0);
			string string_ = class48_0.string_1;
			switch (string_)
			{
			case "frontpage":
				Message.AppendStringWithBreak("frontpage3");
				Message.AppendInt32(3);
				Message.AppendStringWithBreak(class48_0.string_2);
				Message.AppendStringWithBreak(class48_0.string_3);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(11);
				Message.AppendStringWithBreak(class48_0.string_5);
				Message.AppendStringWithBreak(class48_0.string_9);
				Message.AppendStringWithBreak(class48_0.string_6);
				Message.AppendStringWithBreak(class48_0.string_7);
				Message.AppendStringWithBreak(class48_0.string_10);
				Message.AppendStringWithBreak("#FAF8CC");
				Message.AppendStringWithBreak("#FAF8CC");
				Message.AppendStringWithBreak("Read More >");
				Message.AppendStringWithBreak("magic.credits");
				goto IL_47F;
			case "recycler_info":
				Message.AppendStringWithBreak(class48_0.string_1);
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(class48_0.string_2);
				Message.AppendStringWithBreak(class48_0.string_3);
				Message.AppendInt32(3);
				Message.AppendStringWithBreak(class48_0.string_5);
				Message.AppendStringWithBreak(class48_0.string_6);
				Message.AppendStringWithBreak(class48_0.string_7);
				goto IL_47F;
			case "recycler_prizes":
				Message.AppendStringWithBreak("recycler_prizes");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak("catalog_recycler_headline3");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak(class48_0.string_5);
				goto IL_47F;
			case "spaces":
				Message.AppendStringWithBreak("spaces_new");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak(class48_0.string_2);
				Message.AppendInt32(1);
				Message.AppendStringWithBreak(class48_0.string_5);
				goto IL_47F;
			case "recycler":
				Message.AppendStringWithBreak(class48_0.string_1);
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(class48_0.string_2);
				Message.AppendStringWithBreak(class48_0.string_3);
				Message.AppendInt32(1);
				Message.AppendStringWithBreak(class48_0.string_5, 10);
				Message.AppendStringWithBreak(class48_0.string_6);
				Message.AppendStringWithBreak(class48_0.string_7);
				goto IL_47F;
			case "trophies":
				Message.AppendStringWithBreak("trophies");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak(class48_0.string_2);
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(class48_0.string_5);
				Message.AppendStringWithBreak(class48_0.string_7);
				goto IL_47F;
			case "pets":
				Message.AppendStringWithBreak("pets");
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(class48_0.string_2);
				Message.AppendStringWithBreak(class48_0.string_3);
				Message.AppendInt32(4);
				Message.AppendStringWithBreak(class48_0.string_5);
				Message.AppendStringWithBreak("");
				Message.AppendStringWithBreak("Pick a color:");
				Message.AppendStringWithBreak("Pick a race:");
				goto IL_47F;
			case "club_buy":
				Message.AppendStringWithBreak("club_buy");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak("habboclub_2");
				Message.AppendInt32(1);
				goto IL_47F;
			case "club_gifts":
				Message.AppendStringWithBreak("club_gifts");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak("habboclub_2");
				Message.AppendInt32(1);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(1);
				goto IL_47F;
			case "soundmachine":
				Message.AppendStringWithBreak(class48_0.string_1);
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(class48_0.string_2);
				Message.AppendStringWithBreak(class48_0.string_3);
				Message.AppendInt32(2);
				Message.AppendStringWithBreak(class48_0.string_5);
				Message.AppendStringWithBreak(class48_0.string_7);
				goto IL_47F;
			}
			Message.AppendStringWithBreak(class48_0.string_1);
			Message.AppendInt32(3);
			Message.AppendStringWithBreak(class48_0.string_2);
			Message.AppendStringWithBreak(class48_0.string_3);
			Message.AppendStringWithBreak(class48_0.string_4);
			Message.AppendInt32(3);
			Message.AppendStringWithBreak(class48_0.string_5);
			Message.AppendStringWithBreak(class48_0.string_7);
			Message.AppendStringWithBreak(class48_0.string_8);
			IL_47F:
			Message.AppendInt32(class48_0.list_0.Count);
			foreach (CatalogItem current in class48_0.list_0)
			{
				current.method_1(Message);
			}
			return Message;
		}
		public ServerMessage method_20()
		{
			return new ServerMessage(625u);
		}
		public VoucherHandler method_21()
		{
			return this.VoucherHandler_0;
		}
		public Marketplace method_22()
		{
			return this.class43_0;
		}
	}
}
