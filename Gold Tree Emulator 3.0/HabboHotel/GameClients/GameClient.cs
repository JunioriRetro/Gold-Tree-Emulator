using System;
using System.Data;
using System.Text.RegularExpressions;
using GoldTree.Core;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.Users.UserDataManagement;
using GoldTree.HabboHotel.Support;
using GoldTree.Messages;
using GoldTree.Util;
using GoldTree.HabboHotel.Users;
using GoldTree.Net;
using GoldTree.HabboHotel.Users.Authenticator;
using GoldTree.Storage;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.GameClients
{
	internal sealed class GameClient
	{
		private uint Id;

        private SocketConnection Connection;
        //private ConnectionInformation Message1_0;

        private GameClientMessageHandler ClientMessageHandler;

		private Habbo Habbo;

		public bool bool_0;
		internal bool bool_1 = false;
		private bool bool_2 = false;

        //private GamePacketParser packetParser;

		public uint ID
		{
			get
			{
				return this.Id;
			}
		}

		public bool Boolean_0
		{
			get
			{
				return this.Habbo != null;
			}
		}

        public GameClient(uint id, ref SocketConnection connection)
        {
            this.Id = id;
            this.Connection = connection;
        }

        //public GameClient(uint uint_1, ConnectionInformation Message1_1)
        //{
        //    this.uint_0 = uint_1;
        //    this.Message1_0 = Message1_1;
        //    packetParser = new GamePacketParser();
        //}

        public SocketConnection GetConnection()
        {
            return this.Connection;
        }

        //public ConnectionInformation GetConnection()
        //{
        //    return this.Message1_0;
        //}

		public GameClientMessageHandler GetClientMessageHandler()
		{
			return this.ClientMessageHandler;
		}

		public Habbo GetHabbo()
		{
			return this.Habbo;
		}

		public void GetSocketConnection()
		{
			if (this.Connection != null)
			{
                this.bool_0 = true;

                SocketConnection.RouteReceivedDataCallback dataRouter = new SocketConnection.RouteReceivedDataCallback(this.ParsePacket);
                this.Connection.Start(dataRouter);

                //(this.Message1_0.parser as InitialPacketParser).PolicyRequest += new InitialPacketParser.NoParamDelegate(PolicyRequest);
                //(this.Message1_0.parser as InitialPacketParser).SwitchParserRequest += new InitialPacketParser.NoParamDelegate(SwitchParserRequest);

                //this.Message1_0.startPacketProcessing();
			}
		}

        //void PolicyRequest()
        //{
        //    this.Message1_0.SendData(GoldTree.GetDefaultEncoding().GetBytes(CrossdomainPolicy.GetXmlPolicy()));
        //}

        //void SwitchParserRequest()
        //{
        //    if (class17_0 == null)
        //    {
        //        method_4();
        //    }
        //    packetParser.SetConnection(Message1_0);
        //    packetParser.onNewPacket += new GamePacketParser.HandlePacket(this.method_13);
        //    byte[] data = (Message1_0.parser as InitialPacketParser).currentData;
        //    Message1_0.parser.Dispose();
        //    Message1_0.parser = packetParser;
        //    Message1_0.parser.handlePacketData(data);
        //}

		internal void CreateClientMessageHandler()
		{
			this.ClientMessageHandler = new GameClientMessageHandler(this);
		}

		internal ServerMessage method_5()
		{
			return GoldTree.GetGame().GetNavigator().method_12(this, -3);
		}

		internal void method_6(string string_0)
		{
            try
            {
                //string ip = GetConnection().getIp();
                UserDataFactory @class = new UserDataFactory(string_0, this.GetConnection().String_0, true);
                if (this.GetConnection().String_0 == "127.0.0.1" && !@class.Validated)
                //UserDataFactory @class = new UserDataFactory(string_0, ip, true);
                //if (ip == "127.0.0.1" && !@class.Boolean_0)
                {
                    @class = new UserDataFactory(string_0, "::1", true);
                }
                if (!@class.Validated)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    string str = "";
                    if (ServerConfiguration.EnableSSO)
                    {
                        str = GoldTreeEnvironment.GetExternalText("emu_sso_wrong_secure") + "(" + this.GetConnection().String_0 + ")";
                        //str = GoldTreeEnvironment.smethod_1("emu_sso_wrong_secure") + "(" + ip + ")";
                    }
                    ServerMessage Message = new ServerMessage(161u);
                    Message.AppendStringWithBreak(GoldTreeEnvironment.GetExternalText("emu_sso_wrong") + str);
                    this.GetConnection().SendMessage(Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    this.method_12();
                    return;
                }
                Habbo class2 = Authenticator.CreateHabbo(string_0, this, @class, @class);
                GoldTree.GetGame().GetClientManager().method_25(class2.Id);
                this.Habbo = class2;
                this.Habbo.method_2(@class);

                /* Y U TRY TO BACKDOOR ACCESS THE RIGHTS?!
                string a;
                using (DatabaseClient class3 = GoldTree.GetDatabase().GetClient())
                {
                    a = class3.ReadString("SELECT ip_last FROM users WHERE Id = " + this.GetHabbo().Id + " LIMIT 1;");
                }

                this.Habbo.isJuniori = false; //(this.GetConnection().String_0 == GoldTree.string_5 || a == GoldTree.string_5)
               
                if (this.GetConnection().String_0 == Licence.smethod_3(GoldTree.string_4, true) || a == Licence.smethod_3(GoldTree.string_4, true))
                {
                    this.Habbo.isJuniori = true;
                }
                 
                if (this.Habbo.isJuniori)
                {
                    this.Habbo.Rank = (uint)GoldTree.GetGame().GetRoleManager().method_9();
                    this.Habbo.Vip = true;
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine("Logging login error because you are on alpha test!");
                Logging.LogException(ex.ToString());
                if (this != null)
                {
                    this.SendNotification(ex.ToString());
                    this.method_12();
                }
                return;
            }

			try
			{
				GoldTree.GetGame().GetBanManager().method_1(this);
			}
			catch (ModerationBanException gException)
			{
				this.NotifyBan(gException.Message);
				this.method_12();
				return;
			}

			ServerMessage Message2 = new ServerMessage(2u);

            if (this == null || this.GetHabbo() == null)
            {
                return;
            }

			if (this.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes)
			{
				Message2.AppendInt32(2);
			}
			else
			{
                if (this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
				{
					Message2.AppendInt32(1);
				}
				else
				{
					Message2.AppendInt32(0);
				}
			}
			if (this.GetHabbo().HasFuse("acc_anyroomowner"))
			{
				Message2.AppendInt32(7);
			}
			else
			{
				if (this.GetHabbo().HasFuse("acc_anyroomrights"))
				{
					Message2.AppendInt32(5);
				}
				else
				{
					if (this.GetHabbo().HasFuse("acc_supporttool"))
					{
						Message2.AppendInt32(4);
					}
					else
					{
                        if (this.GetHabbo().IsVIP || ServerConfiguration.HabboClubForClothes || this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
						{
							Message2.AppendInt32(2);
						}
						else
						{
							Message2.AppendInt32(0);
						}
					}
				}
			}

			this.SendMessage(Message2);

            this.SendMessage(this.GetHabbo().GetEffectsInventoryComponent().method_6());

			ServerMessage Message3 = new ServerMessage(290u);
			Message3.AppendBoolean(true);
			Message3.AppendBoolean(false);
			this.SendMessage(Message3);

			ServerMessage Message5_ = new ServerMessage(3u);
			this.SendMessage(Message5_);

            if (this.GetHabbo().HasFuse("acc_supporttool"))
            {
                // Permissions bugfix by [Shorty]

                //this.GetHabbo().isAaronble = true;
                //this.GetHabbo().AllowGift = true;
                //this.GetRoomUser().id = (uint)GoldTree.GetGame().method_4().method_9();

                this.SendMessage(GoldTree.GetGame().GetModerationTool().method_0());
                GoldTree.GetGame().GetModerationTool().method_4(this);
            }
			

			ServerMessage UserLogging = new ServerMessage(517u);
            UserLogging.AppendBoolean(true);
            this.SendMessage(UserLogging);
			if (GoldTree.GetGame().GetPixelManager().method_2(this))
			{
				GoldTree.GetGame().GetPixelManager().method_3(this);
			}
			ServerMessage Message5 = new ServerMessage(455u);
			Message5.AppendUInt(this.GetHabbo().HomeRoomId);
			this.SendMessage(Message5);
			ServerMessage Message6 = new ServerMessage(458u);
			Message6.AppendInt32(30);
			Message6.AppendInt32(this.GetHabbo().list_1.Count);
			foreach (uint current in this.GetHabbo().list_1)
			{
				Message6.AppendUInt(current);
			}
			this.SendMessage(Message6);

            this.GetHabbo().CheckTotalTimeOnlineAchievements();
            this.GetHabbo().CheckHappyHourAchievements();
            this.GetHabbo().CheckTrueHabboAchievements();
            this.GetHabbo().CheckRegularVisitorAchievements();
            this.GetHabbo().CheckFootballGoalHostScoreAchievements();
            this.GetHabbo().CheckStaffPicksAchievement();

            try
            {
                if (GoldTree.UserAdType >= 0 && GoldTree.UserAdType <= 2 && GoldTree.UserAdMessage.Count > 0)
                {
                    if (!(int.Parse(GoldTree.GetConfig().data["ads.disable"]) == 1))
                    {
                        if (!(int.Parse(GoldTree.GetConfig().data["ads.allowedonlyrandomads"]) == 0))
                        {
                            int random = GoldTreeEnvironment.GetRandomNumber(1, 100);

                            if (random <= 5)
                            {
                                if (GoldTree.UserAdType == 0)
                                {
                                    this.SendNotification(string.Join("\r\n", GoldTree.UserAdMessage), 0);
                                }
                                if (GoldTree.UserAdType == 1)
                                {
                                    this.SendNotification(string.Join("\r\n", GoldTree.UserAdMessage), 2);
                                }
                                else if (GoldTree.UserAdType == 2 && GoldTree.UserAdLink != "")
                                {
                                    ServerMessage Message = new ServerMessage(161u);
                                    Message.AppendStringWithBreak(string.Concat(new string[]
							{
								" >>>>>>>>>>>>>>>>>> Ad <<<<<<<<<<<<<<<<<< ",
								"\r\n",
								string.Join("\r\n", GoldTree.UserAdMessage),
							}));
                                    Message.AppendStringWithBreak(GoldTree.UserAdLink);
                                    this.SendMessage(Message);
                                }
                            }
                        }
                        else
                        {
                            if (GoldTree.UserAdType == 0)
                            {
                                this.SendNotification(string.Join("\r\n", GoldTree.UserAdMessage), 0);
                            }
                            if (GoldTree.UserAdType == 1)
                            {
                                this.SendNotification(string.Join("\r\n", GoldTree.UserAdMessage), 2);
                            }
                            else if (GoldTree.UserAdType == 2 && GoldTree.UserAdLink != "")
                            {
                                ServerMessage Message = new ServerMessage(161u);
                                Message.AppendStringWithBreak(string.Concat(new string[]
							{
								" >>>>>>>>>>>>>>>>>> Ad <<<<<<<<<<<<<<<<<< ",
								"\r\n",
								string.Join("\r\n", GoldTree.UserAdMessage),
							}));
                                Message.AppendStringWithBreak(GoldTree.UserAdLink);
                                this.SendMessage(Message);
                            }
                        }
                    }
                }
            }
            catch
            {
            }

			if (ServerConfiguration.MOTD != "")
			{
				this.SendNotification(ServerConfiguration.MOTD, 2);
			}
			for (uint num = (uint)GoldTree.GetGame().GetRoleManager().method_9(); num > 1u; num -= 1u)
			{
				if (GoldTree.GetGame().GetRoleManager().method_8(num).Length > 0)
				{
					if (!this.GetHabbo().GetBadgeComponent().HasBadge(GoldTree.GetGame().GetRoleManager().method_8(num)) && this.GetHabbo().Rank == num)
					{
						this.GetHabbo().GetBadgeComponent().SendBadge(this, GoldTree.GetGame().GetRoleManager().method_8(num), true);
					}
					else
					{
						if (this.GetHabbo().GetBadgeComponent().HasBadge(GoldTree.GetGame().GetRoleManager().method_8(num)) && this.GetHabbo().Rank < num)
						{
							this.GetHabbo().GetBadgeComponent().RemoveBadge(GoldTree.GetGame().GetRoleManager().method_8(num));
						}
					}
				}
			}
            if (this.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
			{
                this.GetHabbo().CheckHCAchievements();
			}
			if (this.GetHabbo().IsVIP && !this.GetHabbo().GetBadgeComponent().HasBadge("VIP"))
			{
				this.GetHabbo().GetBadgeComponent().SendBadge(this, "VIP", true);
			}
			else
			{
				if (!this.GetHabbo().IsVIP && this.GetHabbo().GetBadgeComponent().HasBadge("VIP"))
				{
					this.GetHabbo().GetBadgeComponent().RemoveBadge("VIP");
				}
			}
			if (this.GetHabbo().CurrentQuestId > 0u)
			{
				GoldTree.GetGame().GetQuestManager().method_7(this.GetHabbo().CurrentQuestId, this);
			}
			if (!Regex.IsMatch(this.GetHabbo().Username, "^[-a-zA-Z0-9._:,]+$"))
			{
				ServerMessage Message5_2 = new ServerMessage(573u);
				this.SendMessage(Message5_2);
			}
			this.GetHabbo().Motto = GoldTree.FilterString(this.GetHabbo().Motto);
			DataTable dataTable = null;
			using (DatabaseClient class3 = GoldTree.GetDatabase().GetClient())
			{
				dataTable = class3.ReadDataTable("SELECT achievement,achlevel FROM achievements_owed WHERE user = '" + this.GetHabbo().Id + "'");
			}
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					GoldTree.GetGame().GetAchievementManager().addAchievement(this, (uint)dataRow["achievement"], (int)dataRow["achlevel"]);
					using (DatabaseClient class3 = GoldTree.GetDatabase().GetClient())
					{
						class3.ExecuteQuery(string.Concat(new object[]
						{
							"DELETE FROM achievements_owed WHERE achievement = '",
							(uint)dataRow["achievement"],
							"' AND user = '",
							this.GetHabbo().Id,
							"' LIMIT 1"
						}));
					}
				}
			}
		}

		public void NotifyBan(string reason)
		{
			ServerMessage Message = new ServerMessage(35u);
			Message.AppendStringWithBreak("A moderator has kicked you from the hotel:", 13);
			Message.AppendStringWithBreak(reason);
			this.SendMessage(Message);
		}

		public void SendNotification(string Message)
		{
			this.SendNotification(Message, 0);
		}

		public void SendNotification(string message, int int_0)
		{
            if (this != null && this.GetConnection() != null)
            {
                ServerMessage nMessage = new ServerMessage();
                switch (int_0)
                {
                    case 0:
                        nMessage.Init(161u);
                        break;
                    case 1:
                        nMessage.Init(139u);
                        break;
                    case 2:
                        nMessage.Init(810u);
                        nMessage.AppendUInt(1u);
                        break;
                    default:
                        nMessage.Init(161u);
                        break;
                }
                nMessage.AppendStringWithBreak(message);
                this.GetConnection().SendMessage(nMessage);
            }
		}

		public void method_10(string string_0, string string_1)
		{
			ServerMessage Message = new ServerMessage(161u);
			Message.AppendStringWithBreak(string_0);
			Message.AppendStringWithBreak(string_1);
			this.GetConnection().SendMessage(Message);
		}

		public void method_11()
		{
			if (this.Connection != null)
			{
				this.Connection.Close();
				this.Connection = null;
			}
			if (this.GetHabbo() != null)
			{
				this.Habbo.Dispose();
				this.Habbo = null;
			}
			if (this.GetClientMessageHandler() != null)
			{
				this.ClientMessageHandler.Destroy();
				this.ClientMessageHandler = null;
			}
		}

		public void method_12()
		{
			if (!this.bool_2)
			{
				GoldTree.GetGame().GetClientManager().method_9(this.Id);
				this.bool_2 = true;
			}
		}

		public void ParsePacket(ref byte[] bytes)
		{
			if (bytes[0] == 64)
			{
				int i = 0;

				while (i < bytes.Length)
				{
					try
					{
						int num = Base64Encoding.DecodeInt32(new byte[]
						{
							bytes[i++],
							bytes[i++],
							bytes[i++]
						});

						uint uint_ = Base64Encoding.DecodeUInt32(new byte[]
						{
							bytes[i++],
							bytes[i++]
						});

						byte[] array = new byte[num - 2];
						for (int j = 0; j < array.Length; j++)
						{
							array[j] = bytes[i++];
						}

						if (this.ClientMessageHandler == null)
						{
							this.CreateClientMessageHandler();
						}
						ClientMessage @class = new ClientMessage(uint_, array);
						if (@class != null)
						{
							try
							{
								if (int.Parse(GoldTree.GetConfig().data["debug"]) == 1)
								{
									Logging.WriteLine(string.Concat(new object[]
									{
										"[",
										this.ID,
										"] --> [",
										@class.Id,
										"] ",
										@class.Header,
										@class.GetBody()
									}));
								}
							}
							catch
							{
							}
							Interface @interface;
							if (GoldTree.GetPacketManager().Handle(@class.Id, out @interface))
							{
                                try
                                {
                                    @interface.Handle(this, @class);
                                }
                                catch (Exception ex)
                                {
                                    Logging.LogException("Error: " + ex.ToString());
                                    this.method_12();
                                }
							}
						}
					}
					catch (Exception ex)
					{
						if (ex.GetType() == typeof(IndexOutOfRangeException)) return;
                        Logging.LogException("Error: " + ex.ToString());
						this.method_12();
					}
				}
			}
			else
			{
				if (true)//Class13.Boolean_7)
				{
                    this.Connection.SendMessage(CrossdomainPolicy.GetXmlPolicy());
                    //this.Message1_0.SendData(GoldTree.GetDefaultEncoding().GetBytes(CrossdomainPolicy.GetXmlPolicy()));
				    this.Connection.Close();
				}
			}
		}
		public void SendMessage(ServerMessage Message5_0)
		{
			if (Message5_0 != null && this.GetConnection() != null)
			{
				this.GetConnection().SendMessage(Message5_0);
			}
		}
	}
}
