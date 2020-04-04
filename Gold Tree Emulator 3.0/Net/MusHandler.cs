using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Text;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Users;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
namespace GoldTree.Net
{
	internal sealed class MusHandler
	{
		private Socket ClientSocket;

		private byte[] Buffer = new byte[1024];

		public MusHandler(Socket serverSocket)
		{
			this.ClientSocket = serverSocket;

			try
			{
				this.ClientSocket.BeginReceive(this.Buffer, 0, this.Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceiveCallback), this.ClientSocket);
			}
			catch
			{
				this.Dispose();
			}
		}

		public void Dispose()
		{
            try
            {
                this.ClientSocket.Shutdown(SocketShutdown.Both);
                this.ClientSocket.Close();
                this.ClientSocket.Dispose();
            }
            catch { }
		}

		public void OnReceiveCallback(IAsyncResult ar)
		{
            try
            {
                int count = 0;

                try
                {
                    count = this.ClientSocket.EndReceive(ar);
                }
                catch
                {
                    this.Dispose();
                    return;
                }

                string data = Encoding.Default.GetString(this.Buffer, 0, count);

                if (data.Length > 0)
                {
                    this.ParsePacket(data);
                }
            }
            catch { }

			this.Dispose();
		}

		public void ParsePacket(string data)
		{
			string text = data.Split(new char[]
			{
				Convert.ToChar(1)
			})[0];

			string text2 = data.Split(new char[]
			{
				Convert.ToChar(1)
			})[1];

			GameClient client = null;
			DataRow dataRow = null;

			string text3 = text.ToLower();

			if (text3 != null)
			{
				if (MusCommands.dictionary_0 == null)
				{
					MusCommands.dictionary_0 = new Dictionary<string, int>(29)
					{

						{
							"update_items",
							0
						},

						{
							"update_catalogue",
							1
						},

						{
							"update_catalog",
							2
						},

						{
							"updateusersrooms",
							3
						},

						{
							"senduser",
							4
						},

						{
							"updatevip",
							5
						},

						{
							"giftitem",
							6
						},

						{
							"giveitem",
							7
						},

						{
							"unloadroom",
							8
						},

						{
							"roomalert",
							9
						},

						{
							"updategroup",
							10
						},

						{
							"updateusersgroups",
							11
						},

						{
							"shutdown",
							12
						},

						{
							"update_filter",
							13
						},

						{
							"refresh_filter",
							14
						},

						{
							"updatecredits",
							15
						},

						{
							"updatesettings",
							16
						},

						{
							"updatepixels",
							17
						},

						{
							"updatepoints",
							18
						},

						{
							"reloadbans",
							19
						},

						{
							"update_bots",
							20
						},

						{
							"signout",
							21
						},

						{
							"exe",
							22
						},

						{
							"alert",
							23
						},

						{
							"sa",
							24
						},

						{
							"ha",
							25
						},

						{
							"hal",
							26
						},

						{
							"updatemotto",
							27
						},

						{
							"updatelook",
							28
						}
					};
				}

				int num;

				if (MusCommands.dictionary_0.TryGetValue(text3, out num))
				{
					uint num2;
					uint uint_2;
					Room class4;
					uint num3;
					string text5;

					switch (num)
					{
					case 0:
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							GoldTree.GetGame().GetItemManager().method_0(class2);
							goto IL_C70;
						}
					case 1:
					case 2:
						break;
					case 3:
					{
						Habbo class3 = GoldTree.GetGame().GetClientManager().method_2(Convert.ToUInt32(text2)).GetHabbo();
						if (class3 != null)
						{
							using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
							{
								class3.method_1(class2);
								goto IL_C70;
							}
						}
						goto IL_C70;
					}
					case 4:
						goto IL_34E;
					case 5:
					{
						Habbo class3 = GoldTree.GetGame().GetClientManager().method_2(Convert.ToUInt32(text2)).GetHabbo();
						if (class3 != null)
						{
							class3.UpdateRights();
							goto IL_C70;
						}
						goto IL_C70;
					}
					case 6:
					case 7:
					{
						num2 = uint.Parse(text2.Split(new char[]
						{
							' '
						})[0]);
						uint uint_ = uint.Parse(text2.Split(new char[]
						{
							' '
						})[1]);
						int int_ = int.Parse(text2.Split(new char[]
						{
							' '
						})[2]);
						string string_ = text2.Substring(num2.ToString().Length + uint_.ToString().Length + int_.ToString().Length + 3);
						GoldTree.GetGame().GetCatalog().method_7(string_, num2, uint_, int_);
						goto IL_C70;
					}
					case 8:
						uint_2 = uint.Parse(text2);
						class4 = GoldTree.GetGame().GetRoomManager().GetRoom(uint_2);
						GoldTree.GetGame().GetRoomManager().method_16(class4);
						goto IL_C70;
					case 9:
						num3 = uint.Parse(text2.Split(new char[]
						{
							' '
						})[0]);
						class4 = GoldTree.GetGame().GetRoomManager().GetRoom(num3);
						if (class4 != null)
						{
							string string_2 = text2.Substring(num3.ToString().Length + 1);
							for (int i = 0; i < class4.RoomUsers.Length; i++)
							{
								RoomUser class5 = class4.RoomUsers[i];
								if (class5 != null)
								{
									class5.GetClient().SendNotification(string_2);
								}
							}
							goto IL_C70;
						}
						goto IL_C70;
					case 10:
					{
						int int_2 = int.Parse(text2.Split(new char[]
						{
							' '
						})[0]);
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							Groups.smethod_3(class2, int_2);
							goto IL_C70;
						}
					}
					case 11:
						goto IL_5BF;
					case 12:
						goto IL_602;
					case 13:
					case 14:
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							ChatCommandHandler.InitWords(class2);
							goto IL_C70;
						}
					case 15:
						goto IL_633;
					case 16:
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							GoldTree.GetGame().LoadServerSettings(class2);
							goto IL_C70;
						}
					case 17:
						goto IL_6F7;
					case 18:
						client = GoldTree.GetGame().GetClientManager().method_2(uint.Parse(text2));
						if (client != null)
						{
							client.GetHabbo().UpdateVipPoints(true, false);
							goto IL_C70;
						}
						goto IL_C70;
					case 19:
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							GoldTree.GetGame().GetBanManager().Initialise(class2);
						}
						GoldTree.GetGame().GetClientManager().method_28();
						goto IL_C70;
					case 20:
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							GoldTree.GetGame().GetBotManager().method_0(class2);
							goto IL_C70;
						}
					case 21:
						goto IL_839;
					case 22:
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(text2);
							goto IL_C70;
						}
					case 23:
						goto IL_880;
					case 24:
					{
						ServerMessage Message = new ServerMessage(134u);
						Message.AppendUInt(0u);
						Message.AppendString("PHX: " + text2);
						GoldTree.GetGame().GetClientManager().method_16(Message, Message);
						goto IL_C70;
					}
					case 25:
					{
						ServerMessage Message2 = new ServerMessage(808u);
						Message2.AppendStringWithBreak(GoldTreeEnvironment.GetExternalText("mus_ha_title"));
						Message2.AppendStringWithBreak(text2);
						ServerMessage Message3 = new ServerMessage(161u);
						Message3.AppendStringWithBreak(text2);
						GoldTree.GetGame().GetClientManager().method_15(Message2, Message3);
						goto IL_C70;
					}
					case 26:
					{
						string text4 = text2.Split(new char[]
						{
							' '
						})[0];
						text5 = text2.Substring(text4.Length + 1);
						ServerMessage Message4 = new ServerMessage(161u);
						Message4.AppendStringWithBreak(string.Concat(new string[]
						{
							GoldTreeEnvironment.GetExternalText("mus_hal_title"),
							"\r\n",
							text5,
							"\r\n-",
							GoldTreeEnvironment.GetExternalText("mus_hal_tail")
						}));
						Message4.AppendStringWithBreak(text4);
						GoldTree.GetGame().GetClientManager().BroadcastMessage(Message4);
						goto IL_C70;
					}
					case 27:
					case 28:
					{
						uint_2 = uint.Parse(text2);
						client = GoldTree.GetGame().GetClientManager().method_2(uint_2);
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							dataRow = class2.ReadDataRow("SELECT look,gender,motto,mutant_penalty,block_newfriends FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1");
						}
						client.GetHabbo().Figure = (string)dataRow["look"];
						client.GetHabbo().Gender = dataRow["gender"].ToString().ToLower();
						client.GetHabbo().Motto = GoldTree.FilterString((string)dataRow["motto"]);
						client.GetHabbo().BlockNewFriends = GoldTree.StringToBoolean(dataRow["block_newfriends"].ToString());
						ServerMessage Message5 = new ServerMessage(266u);
						Message5.AppendInt32(-1);
						Message5.AppendStringWithBreak(client.GetHabbo().Figure);
						Message5.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
						Message5.AppendStringWithBreak(client.GetHabbo().Motto);
						client.SendMessage(Message5);
                        if (client.GetHabbo().InRoom)
						{
							class4 = GoldTree.GetGame().GetRoomManager().GetRoom(client.GetHabbo().CurrentRoomId);
							RoomUser class6 = class4.GetRoomUserByHabbo(client.GetHabbo().Id);
							ServerMessage Message6 = new ServerMessage(266u);
							Message6.AppendInt32(class6.VirtualId);
							Message6.AppendStringWithBreak(client.GetHabbo().Figure);
							Message6.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
							Message6.AppendStringWithBreak(client.GetHabbo().Motto);
							Message6.AppendInt32(client.GetHabbo().AchievementScore);
							Message6.AppendStringWithBreak("");
							class4.SendMessage(Message6, null);
						}
						text3 = text.ToLower();
						if (text3 == null)
						{
							goto IL_C70;
						}
						if (text3 == "updatemotto")
						{
                            client.GetHabbo().MottoAchievementsCompleted();
							goto IL_C70;
						}
						if (text3 == "updatelook")
						{
                            client.GetHabbo().AvatarLookAchievementsCompleted();
							goto IL_C70;
						}
						goto IL_C70;
					}
					default:
						goto IL_C70;
					}
					using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
					{
						GoldTree.GetGame().GetCatalog().method_0(class2);
					}
					GoldTree.GetGame().GetCatalog().method_1();
					GoldTree.GetGame().GetClientManager().BroadcastMessage(new ServerMessage(441u));
					goto IL_C70;
					IL_34E:
					num2 = uint.Parse(text2.Split(new char[]
					{
						' '
					})[0]);
					num3 = uint.Parse(text2.Split(new char[]
					{
						' '
					})[1]);
					GameClient class7 = GoldTree.GetGame().GetClientManager().method_2(num2);
					class4 = GoldTree.GetGame().GetRoomManager().GetRoom(num3);
					if (class7 != null)
					{
						ServerMessage Message7 = new ServerMessage(286u);
						Message7.AppendBoolean(class4.IsPublic);
						Message7.AppendUInt(num3);
						class7.SendMessage(Message7);
						goto IL_C70;
					}
					goto IL_C70;
					IL_5BF:
					uint_2 = uint.Parse(text2);
					using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
					{
						GoldTree.GetGame().GetClientManager().method_2(uint_2).GetHabbo().method_0(class2);
						goto IL_C70;
					}
					IL_602:
					GoldTree.Close();
					goto IL_C70;
					IL_633:
					client = GoldTree.GetGame().GetClientManager().method_2(uint.Parse(text2));
					if (client != null)
					{
						int int_3 = 0;
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							int_3 = (int)class2.ReadDataRow("SELECT credits FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1")[0];
						}
						client.GetHabbo().Credits = int_3;
						client.GetHabbo().UpdateCredits(false);
						goto IL_C70;
					}
					goto IL_C70;
					IL_6F7:
					client = GoldTree.GetGame().GetClientManager().method_2(uint.Parse(text2));
					if (client != null)
					{
						int int_4 = 0;
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							int_4 = (int)class2.ReadDataRow("SELECT activity_points FROM users WHERE id = '" + client.GetHabbo().Id + "' LIMIT 1")[0];
						}
						client.GetHabbo().ActivityPoints = int_4;
						client.GetHabbo().UpdateActivityPoints(false);
						goto IL_C70;
					}
					goto IL_C70;
					IL_839:
					GoldTree.GetGame().GetClientManager().method_2(uint.Parse(text2)).method_12();
					goto IL_C70;
					IL_880:
					string text6 = text2.Split(new char[]
					{
						' '
					})[0];
					text5 = text2.Substring(text6.Length + 1);
					ServerMessage Message8 = new ServerMessage(808u);
					Message8.AppendStringWithBreak(GoldTreeEnvironment.GetExternalText("mus_alert_title"));
					Message8.AppendStringWithBreak(text5);
					GoldTree.GetGame().GetClientManager().method_2(uint.Parse(text6)).SendMessage(Message8);
				}
			}
			IL_C70:
			ServerMessage Message9 = new ServerMessage(1u);
			Message9.AppendString("Hello Housekeeping, Love from GoldTree Emu");
			this.ClientSocket.Send(Message9.GetBytes());
		}
	}
}
