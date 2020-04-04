using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Pets;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
using GoldTree.HabboHotel.Pathfinding;
namespace GoldTree.HabboHotel.RoomBots
{
	internal sealed class PetBot : BotAI
	{
		private int int_2;
		private int int_3;
		public PetBot(int int_4)
		{
			this.int_2 = new Random((int_4 ^ 2) + DateTime.Now.Millisecond).Next(25, 60);
			this.int_3 = new Random((int_4 ^ 2) + DateTime.Now.Millisecond).Next(10, 60);
		}
		private int method_4()
		{
			RoomUser @class = base.GetRoomUser();
			int result = 5;
			if (@class.PetData.Level >= 1)
			{
				result = GoldTree.smethod_5(1, 8);
			}
			else
			{
				if (@class.PetData.Level >= 5)
				{
					result = GoldTree.smethod_5(1, 7);
				}
				else
				{
					if (@class.PetData.Level >= 10)
					{
						result = GoldTree.smethod_5(1, 6);
					}
				}
			}
			return result;
		}
		private void method_5(int int_4, int int_5, bool bool_0)
		{
			RoomUser @class = base.GetRoomUser();
			if (bool_0)
			{
				int int_6 = GoldTree.smethod_5(0, base.method_1().RoomModel.int_4);
				int int_7 = GoldTree.smethod_5(0, base.method_1().RoomModel.int_5);
				@class.MoveTo(int_6, int_7);
			}
			else
			{
				if (int_4 < base.method_1().RoomModel.int_4 && int_5 < base.method_1().RoomModel.int_5 && int_4 >= 0 && int_5 >= 0)
				{
					@class.MoveTo(int_4, int_5);
				}
			}
		}
		public override void OnSelfEnterRoom()
		{
			if (base.GetRoomUser().PetData.X > 0 && base.GetRoomUser().PetData.Y > 0)
			{
				base.GetRoomUser().int_3 = base.GetRoomUser().PetData.X;
				base.GetRoomUser().int_4 = base.GetRoomUser().PetData.Y;
			}
			this.method_5(0, 0, true);
		}
		public override void OnSelfLeaveRoom(bool bool_0)
		{
			if (base.method_3().RoomUser_0 != null)
			{
				RoomUser RoomUser_ = base.method_3().RoomUser_0;
				if (RoomUser_.class34_1 != null && RoomUser_ == base.method_3().RoomUser_0)
				{
					base.method_3().RoomUser_0 = null;
					RoomUser_.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
					RoomUser_.class34_1 = null;
					RoomUser_.RoomUser_0 = null;
				}
			}
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				if (base.GetRoomUser().PetData.DBState == DatabaseUpdateState.NeedsInsert)
				{
					@class.AddParamWithValue("petname" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Name);
					@class.AddParamWithValue("petcolor" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Color);
					@class.AddParamWithValue("petrace" + base.GetRoomUser().PetData.PetId, base.GetRoomUser().PetData.Race);
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO `user_pets` VALUES ('",
						base.GetRoomUser().PetData.PetId,
						"', '",
						base.GetRoomUser().PetData.OwnerId,
						"', '0', @petname",
						base.GetRoomUser().PetData.PetId,
						", @petrace",
						base.GetRoomUser().PetData.PetId,
						", @petcolor",
						base.GetRoomUser().PetData.PetId,
						", '",
						base.GetRoomUser().PetData.Type,
						"', '",
						base.GetRoomUser().PetData.Expirience,
						"', '",
						base.GetRoomUser().PetData.Energy,
						"', '",
						base.GetRoomUser().PetData.Nutrition,
						"', '",
						base.GetRoomUser().PetData.Respect,
						"', '",
						base.GetRoomUser().PetData.CreationStamp,
						"', '",
						base.GetRoomUser().PetData.X,
						"', '",
						base.GetRoomUser().PetData.Y,
						"', '",
						base.GetRoomUser().PetData.Z,
						"');"
					}));
				}
				else
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE user_pets SET room_id = '0', expirience = '",
						base.GetRoomUser().PetData.Expirience,
						"', energy = '",
						base.GetRoomUser().PetData.Energy,
						"', nutrition = '",
						base.GetRoomUser().PetData.Nutrition,
						"', respect = '",
						base.GetRoomUser().PetData.Respect,
						"' WHERE Id = '",
						base.GetRoomUser().PetData.PetId,
						"' LIMIT 1; "
					}));
				}
				base.GetRoomUser().PetData.DBState = DatabaseUpdateState.Updated;
			}
		}
		public override void OnUserEnterRoom(RoomUser RoomUser_0)
		{
		}
		public override void OnUserLeaveRoom(GameClient Session)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
				string string_ = Session.GetHabbo().Username;
				RoomUser @class = base.method_1().GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (base.method_3().RoomUser_0 != null && @class != null && @class.class34_1 != null && @class == base.method_3().RoomUser_0)
				{
					base.method_3().RoomUser_0 = null;
				}
				try
				{
					if (string_.ToLower() == base.GetRoomUser().PetData.OwnerName.ToLower() && string_.ToLower() != base.method_1().Owner.ToLower())
					{
						base.method_1().method_6(base.GetRoomUser().PetData.VirtualId, false);
						Session.GetHabbo().GetInventoryComponent().AddPet(base.GetRoomUser().PetData);
					}
				}
				catch
				{
				}
			}
		}
		public override void OnUserSay(RoomUser RoomUser_0, string string_0)
		{
			RoomUser @class = base.GetRoomUser();
			if (@class.RoomBot.RoomUser_0 == null)
			{
				if (string_0.ToLower().Equals(@class.PetData.Name.ToLower()))
				{
					@class.method_9(Class107.smethod_0(@class.int_3, @class.int_4, RoomUser_0.int_3, RoomUser_0.int_4));
				}
				else
				{
					if (string_0.ToLower().StartsWith(@class.PetData.Name.ToLower() + " ") && RoomUser_0.GetClient().GetHabbo().Username.ToLower() == base.GetRoomUser().PetData.OwnerName.ToLower())
					{
						string key = string_0.Substring(@class.PetData.Name.ToLower().Length + 1);
						if ((@class.PetData.Energy >= 10 && this.method_4() < 6) || @class.PetData.Level >= 15)
						{
							@class.Statusses.Clear();
							if (!GoldTree.GetGame().GetRoleManager().dictionary_5.ContainsKey(key))
							{
								string[] array = new string[]
								{
									GoldTreeEnvironment.GetExternalText("pet_response_confused1"),
									GoldTreeEnvironment.GetExternalText("pet_response_confused2"),
									GoldTreeEnvironment.GetExternalText("pet_response_confused3"),
									GoldTreeEnvironment.GetExternalText("pet_response_confused4"),
									GoldTreeEnvironment.GetExternalText("pet_response_confused5"),
									GoldTreeEnvironment.GetExternalText("pet_response_confused6"),
									GoldTreeEnvironment.GetExternalText("pet_response_confused7")
								};
								Random random = new Random();
								@class.HandleSpeech(null, array[random.Next(0, array.Length - 1)], false);
							}
							else
							{
								switch (GoldTree.GetGame().GetRoleManager().dictionary_5[key])
								{
								case 1:
									@class.PetData.AddExpirience(10, -25);
									@class.HandleSpeech(null, GoldTreeEnvironment.GetExternalText("pet_response_sleep"), false);
									@class.Statusses.Add("lay", @class.double_0.ToString());
									break;
								case 2:
									this.method_5(0, 0, true);
									@class.PetData.AddExpirience(5, 5);
									break;
								case 3:
									@class.PetData.AddExpirience(5, 5);
									@class.Statusses.Add("sit", @class.double_0.ToString());
									break;
								case 4:
									@class.PetData.AddExpirience(5, 5);
									@class.Statusses.Add("lay", @class.double_0.ToString());
									break;
								case 5:
									@class.PetData.AddExpirience(10, 10);
									this.int_3 = 60;
									break;
								case 6:
								{
									int int_ = RoomUser_0.int_3;
									int int_2 = RoomUser_0.int_4;
									if (RoomUser_0.int_8 == 4)
									{
										int_2 = RoomUser_0.int_4 + 1;
									}
									else
									{
										if (RoomUser_0.int_8 == 0)
										{
											int_2 = RoomUser_0.int_4 - 1;
										}
										else
										{
											if (RoomUser_0.int_8 == 6)
											{
												int_ = RoomUser_0.int_3 - 1;
											}
											else
											{
												if (RoomUser_0.int_8 == 2)
												{
													int_ = RoomUser_0.int_3 + 1;
												}
												else
												{
													if (RoomUser_0.int_8 == 3)
													{
														int_ = RoomUser_0.int_3 + 1;
														int_2 = RoomUser_0.int_4 + 1;
													}
													else
													{
														if (RoomUser_0.int_8 == 1)
														{
															int_ = RoomUser_0.int_3 + 1;
															int_2 = RoomUser_0.int_4 - 1;
														}
														else
														{
															if (RoomUser_0.int_8 == 7)
															{
																int_ = RoomUser_0.int_3 - 1;
																int_2 = RoomUser_0.int_4 - 1;
															}
															else
															{
																if (RoomUser_0.int_8 == 5)
																{
																	int_ = RoomUser_0.int_3 - 1;
																	int_2 = RoomUser_0.int_4 + 1;
																}
															}
														}
													}
												}
											}
										}
									}
									@class.PetData.AddExpirience(15, 15);
									this.method_5(int_, int_2, false);
									break;
								}
								case 7:
									@class.PetData.AddExpirience(20, 20);
									@class.Statusses.Add("ded", @class.double_0.ToString());
									break;
								case 8:
									@class.PetData.AddExpirience(10, 10);
									@class.Statusses.Add("beg", @class.double_0.ToString());
									break;
								case 9:
									@class.PetData.AddExpirience(15, 15);
									@class.Statusses.Add("jmp", @class.double_0.ToString());
									break;
								case 10:
									@class.PetData.AddExpirience(25, 25);
									@class.HandleSpeech(null, GoldTreeEnvironment.GetExternalText("pet_response_silent"), false);
									this.int_2 = 120;
									break;
								case 11:
									@class.PetData.AddExpirience(15, 15);
									this.int_2 = 2;
									break;
								}
							}
						}
						else
						{
							string[] array2 = new string[]
							{
								GoldTreeEnvironment.GetExternalText("pet_response_sleeping1"),
								GoldTreeEnvironment.GetExternalText("pet_response_sleeping2"),
								GoldTreeEnvironment.GetExternalText("pet_response_sleeping3"),
								GoldTreeEnvironment.GetExternalText("pet_response_sleeping4"),
								GoldTreeEnvironment.GetExternalText("pet_response_sleeping5")
							};
							string[] array3 = new string[]
							{
								GoldTreeEnvironment.GetExternalText("pet_response_refusal1"),
								GoldTreeEnvironment.GetExternalText("pet_response_refusal2"),
								GoldTreeEnvironment.GetExternalText("pet_response_refusal3"),
								GoldTreeEnvironment.GetExternalText("pet_response_refusal4"),
								GoldTreeEnvironment.GetExternalText("pet_response_refusal5")
							};
							@class.int_10 = @class.int_12;
							@class.int_11 = @class.int_13;
							@class.Statusses.Clear();
							if (@class.PetData.Energy < 10)
							{
								Random random2 = new Random();
								@class.HandleSpeech(null, array2[random2.Next(0, array2.Length - 1)], false);
								if (@class.PetData.Type != 13u)
								{
									@class.Statusses.Add("lay", @class.double_0.ToString());
								}
								else
								{
									@class.Statusses.Add("lay", (@class.double_0 - 1.0).ToString());
								}
								this.int_2 = 25;
								this.int_3 = 20;
								base.GetRoomUser().PetData.PetEnergy(-25);
							}
							else
							{
								Random random2 = new Random();
								@class.HandleSpeech(null, array3[random2.Next(0, array3.Length - 1)], false);
							}
						}
						@class.UpdateNeeded = true;
					}
				}
			}
		}
		public override void OnUserShout(RoomUser RoomUser_0, string string_0)
		{
		}
		public override void OnTimerTick()
		{
			if (this.int_2 <= 0)
			{
				RoomUser @class = base.GetRoomUser();
				string[] array = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog5")
				};
				string[] array2 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_cat1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_cat2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_cat3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_cat4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_cat5")
				};
				string[] array3 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_croc1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_croc2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_croc3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_croc4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_croc5")
				};
				string[] array4 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dog5")
				};
				string[] array5 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_bear1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_bear2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_bear3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_bear4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_bear5")
				};
				string[] array6 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_pig1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_pig2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_pig3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_pig4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_pig5")
				};
				string[] array7 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_lion1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_lion2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_lion3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_lion4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_lion5")
				};
				string[] array8 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_rhino1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_rhino2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_rhino3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_rhino4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_rhino5")
				};
				string[] array9 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_spider1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_spider2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_spider3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_spider4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_spider5")
				};
				string[] array10 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_turtle1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_turtle2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_turtle3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_turtle4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_turtle5")
				};
				string[] array11 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_chic1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_chic2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_chic3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_chic4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_chic5")
				};
				string[] array12 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_frog1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_frog2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_frog3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_frog4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_frog5")
				};
				string[] array13 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_dragon1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dragon2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dragon3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dragon4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_dragon5")
				};
				string[] array14 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_horse1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_horse2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_horse3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_horse4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_horse5")
				};
				string[] array15 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_monkey1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_monkey2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_monkey3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_monkey4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_monkey5")
				};
				string[] array16 = new string[]
				{
					GoldTreeEnvironment.GetExternalText("pet_chatter_generic1"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_generic2"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_generic3"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_generic4"),
					GoldTreeEnvironment.GetExternalText("pet_chatter_generic5")
				};
				string[] array17 = new string[]
				{
					"sit",
					"lay",
					"snf",
					"ded",
					"jmp",
					"snf",
					"sit",
					"snf"
				};
				string[] array18 = new string[]
				{
					"sit",
					"lay"
				};
				string[] array19 = new string[]
				{
					"wng",
					"grn",
					"flm",
					"std",
					"swg",
					"sit",
					"lay",
					"snf",
					"plf",
					"jmp",
					"flm",
					"crk",
					"rlx",
					"flm"
				};
				if (@class != null)
				{
					Random random = new Random();
					int num = GoldTree.smethod_5(1, 4);
					if (num == 2)
					{
						@class.Statusses.Clear();
						if (base.GetRoomUser().RoomBot.RoomUser_0 == null)
						{
							if (@class.PetData.Type == 13u)
							{
								@class.Statusses.Add(array18[random.Next(0, array18.Length - 1)], @class.double_0.ToString());
							}
							else
							{
								if (@class.PetData.Type != 12u)
								{
									@class.Statusses.Add(array17[random.Next(0, array17.Length - 1)], @class.double_0.ToString());
								}
								else
								{
									@class.Statusses.Add(array19[random.Next(0, array19.Length - 1)], @class.double_0.ToString());
								}
							}
						}
					}
					switch (@class.PetData.Type)
					{
					case 0u:
						@class.HandleSpeech(null, array[random.Next(0, array.Length - 1)], false);
						break;
					case 1u:
						@class.HandleSpeech(null, array2[random.Next(0, array2.Length - 1)], false);
						break;
					case 2u:
						@class.HandleSpeech(null, array3[random.Next(0, array3.Length - 1)], false);
						break;
					case 3u:
						@class.HandleSpeech(null, array4[random.Next(0, array4.Length - 1)], false);
						break;
					case 4u:
						@class.HandleSpeech(null, array5[random.Next(0, array5.Length - 1)], false);
						break;
					case 5u:
						@class.HandleSpeech(null, array6[random.Next(0, array6.Length - 1)], false);
						break;
					case 6u:
						@class.HandleSpeech(null, array7[random.Next(0, array7.Length - 1)], false);
						break;
					case 7u:
						@class.HandleSpeech(null, array8[random.Next(0, array8.Length - 1)], false);
						break;
					case 8u:
						@class.HandleSpeech(null, array9[random.Next(0, array9.Length - 1)], false);
						break;
					case 9u:
						@class.HandleSpeech(null, array10[random.Next(0, array10.Length - 1)], false);
						break;
					case 10u:
						@class.HandleSpeech(null, array11[random.Next(0, array11.Length - 1)], false);
						break;
					case 11u:
						@class.HandleSpeech(null, array12[random.Next(0, array12.Length - 1)], false);
						break;
					case 12u:
						@class.HandleSpeech(null, array13[random.Next(0, array13.Length - 1)], false);
						break;
					case 13u:
						@class.HandleSpeech(null, array14[random.Next(0, array14.Length - 1)], false);
						break;
					case 14u:
						@class.HandleSpeech(null, array15[random.Next(0, array15.Length - 1)], false);
						break;
					default:
						@class.HandleSpeech(null, array16[random.Next(0, array16.Length - 1)], false);
						break;
					}
				}
				this.int_2 = GoldTree.smethod_5(30, 120);
			}
			else
			{
				this.int_2--;
			}
			if (this.int_3 <= 0)
			{
				base.GetRoomUser().PetData.PetEnergy(-10);
				if (base.GetRoomUser().RoomBot.RoomUser_0 == null)
				{
					this.method_5(0, 0, true);
				}
				this.int_3 = 30;
			}
			else
			{
				this.int_3--;
			}
		}
	}
}
