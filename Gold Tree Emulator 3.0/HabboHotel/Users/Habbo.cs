using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using GoldTree.HabboHotel.Users.UserDataManagement;
using GoldTree.HabboHotel.Users.Subscriptions;
using GoldTree.HabboHotel.Users.Inventory;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Util;
using GoldTree.Messages;
using GoldTree.HabboHotel.Users.Messenger;
using GoldTree.HabboHotel.Users.Badges;
using GoldTree.Storage;
using System.Globalization;
namespace GoldTree.HabboHotel.Users
{
	internal sealed class Habbo
	{
		public uint Id;

		public string Username;
		public string RealName;

        public bool IsJuniori;

		public bool IsVisible;

		public bool TradingDisabled;

		public string SSO;

		public string LastIp;

		public uint Rank;

		public string Motto;

		public string Figure;
		public string Gender;

		public int int_0;
		public DataTable dataTable_0;
		public List<int> list_0;
		public int int_1;

		public int Credits;

		public int ActivityPoints;
		public double LastActivityPointsUpdate;

		public bool IsMuted;

		public int int_4;
		internal bool bool_4 = false;
		public uint uint_2;
		public bool bool_5;
		public bool bool_6;

		public uint CurrentRoomId;
		public uint HomeRoomId;

		public bool bool_7;
		public uint uint_5;
		public List<uint> list_1;
		public List<uint> list_2;
		public List<string> list_3;
		public Dictionary<uint, int> dictionary_0;
		public List<uint> list_4;

		private SubscriptionManager SubscriptionManager;

		private HabboMessenger Messenger;

		private BadgeComponent BadgeComponent;
        private InventoryComponent InventoryComponent;
		private AvatarEffectsInventoryComponent EffectsInventoryComponent;

		private GameClient Session;

		public List<uint> CompletedQuests;
		public uint CurrentQuestId;
		public int CurrentQuestProgress;

		public int BuilderLevel;
		public int SocialLevel;
		public int IdentityLevel;
		public int ExplorerLevel;

		public uint uint_7;

		public int NewbieStatus;

		public bool bool_8;
		public bool bool_9;
		public bool bool_10;

		public bool BlockNewFriends;

		public bool HideInRom;
		public bool HideOnline;

		public bool IsVIP;
        public int VipPoints;

		public int Volume;

		public int AchievementScore;

		public int RoomVisits;

		public int OnlineTime;
		public int LoginTimestamp;

		public int Respect;
		public int RespectGiven;

		public int GiftsGiven;
		public int GiftsReceived;

		private UserDataFactory UserDataFactory;

		internal List<RoomData> OwnedRooms;

		public int int_23;

		public DateTime dateTime_0;

		public bool bool_15;
		public int int_24;
		private bool bool_16 = false;

        public int FireworkPixelLoadedCount;

        public int NewPetsBuyed;

        public string DataCadastro;

        public string LastOnline;

        public int RegularVisitor;

        public int PetBuyed;

        public int RegistrationDuration;

        public int FootballGoalScorer;
        public int FootballGoalHost;

        public bool Online = false;

        public int TilesLocked;

        public int RespectPoints;
        public int PetRespectPoints;

        public int StaffPicks;

        public double LastVipAlert;
        public double LastVipAlertLink;

        public bool FriendStreamEnabled;

        public int QuestsCustom1Progress;

        public bool InRoom
		{
			get
			{
				return this.CurrentRoomId >= 1u;
			}
		}

        public Room CurrentRoom
		{
			get
			{
				if (this.CurrentRoomId <= 0u)
				{
					return null;
				}
				else
				{
					return GoldTree.GetGame().GetRoomManager().GetRoom(this.CurrentRoomId);
				}
			}
		}

		internal string UpdateQuery
		{
			get
			{
				this.bool_16 = true;
                this.Online = false;
				int num = (int)GoldTree.GetUnixTimestamp() - this.LoginTimestamp;
				string text = string.Concat(new object[]
				{
					"UPDATE users SET last_online = UNIX_TIMESTAMP(), online = '0', activity_points_lastupdate = '",
					this.LastActivityPointsUpdate,
					"' WHERE Id = '",
					this.Id,
					"' LIMIT 1; "
				});
				object obj = text;
				return string.Concat(new object[]
				{
					obj,
					"UPDATE user_stats SET RoomVisits = '",
					this.RoomVisits,
					"', OnlineTime = OnlineTime + ",
					num,
					", Respect = '",
					this.Respect,
					"', RespectGiven = '",
					this.RespectGiven,
					"', GiftsGiven = '",
					this.GiftsGiven,
					"', GiftsReceived = '",
					this.GiftsReceived,
                    "', FootballGoalScorer = '",
                    this.FootballGoalScorer,
                    "', FootballGoalHost = '",
                    this.FootballGoalHost,
                    "', TilesLocked = '",
                    this.TilesLocked,
                    "', staff_picks = '",
                    this.StaffPicks,
					"' WHERE Id = '",
					this.Id,
					"' LIMIT 1; "
				});
			}
		}

        private GameClient GetClient()
        {
            return GoldTree.GetGame().GetClientManager().method_2(this.Id);
        }

        public SubscriptionManager GetSubscriptionManager()
        {
            return this.SubscriptionManager;
        }

        public HabboMessenger GetMessenger()
        {
            return this.Messenger;
        }

        public UserDataFactory GetUserDataFactory()
        {
            return this.UserDataFactory;
        }

        public BadgeComponent GetBadgeComponent()
        {
            return this.BadgeComponent;
        }

        public InventoryComponent GetInventoryComponent()
        {
            return this.InventoryComponent;
        }

        public AvatarEffectsInventoryComponent GetEffectsInventoryComponent()
        {
            return this.EffectsInventoryComponent;
        }

        public Habbo(uint UserId, string Username, string Name, string SSO, uint Rank, string Motto, string Look, string Gender, int Credits, int Pixels, double Activity_Points_LastUpdate, string DataCadastro, bool Muted, uint HomeRoom, int NewbieStatus, bool BlockNewFriends, bool HideInRoom, bool HideOnline, bool Vip, int Volume, int Points, bool AcceptTrading, string LastIp, GameClient Session, UserDataFactory userDataFactory, string last_online, int daily_respect_points, int daily_pet_respect_points, double vipha_last, double viphal_last, bool FriendStream)
		{
			if (Session != null)
				GoldTree.GetGame().GetClientManager().method_0(UserId, Username, Session);

			this.Id = UserId;
			this.Username = Username;
			this.RealName = Name;
            this.IsJuniori = false;
            this.IsVisible = true;
			this.SSO = SSO;
			this.Rank = Rank;
			this.Motto = Motto;
			this.Figure = GoldTree.FilterString(Look.ToLower());
			this.Gender = Gender.ToLower();
			this.Credits = Credits;
			this.VipPoints = Points;
			this.ActivityPoints = Pixels;
			this.LastActivityPointsUpdate = Activity_Points_LastUpdate;

			this.TradingDisabled = AcceptTrading;
			this.IsMuted = Muted;
			this.uint_2 = 0u;
			this.bool_5 = false;
			this.bool_6 = false;

			this.CurrentRoomId = 0u;
			this.HomeRoomId = HomeRoom;

			this.list_1 = new List<uint>();
			this.list_2 = new List<uint>();
			this.list_3 = new List<string>();
			this.dictionary_0 = new Dictionary<uint, int>();
			this.list_4 = new List<uint>();

			this.NewbieStatus = NewbieStatus;

			this.bool_10 = false;

			this.BlockNewFriends = BlockNewFriends;

			this.HideInRom = HideInRoom;
			this.HideOnline = HideOnline;

			this.IsVIP = Vip;
			this.Volume = Volume;
			this.int_1 = 0;
			this.int_24 = 1;

			this.LastIp = LastIp;

			this.bool_7 = false;
			this.uint_5 = 0u;

			this.Session = Session;

			this.UserDataFactory = userDataFactory;

			this.OwnedRooms = new List<RoomData>();

			this.list_0 = new List<int>();

            this.DataCadastro = DataCadastro;

            this.LastOnline = last_online;

            this.Online = true;

            this.RespectPoints = daily_respect_points;
            this.PetRespectPoints = daily_pet_respect_points;

            this.LastVipAlert = vipha_last;
            this.LastVipAlertLink = viphal_last;

            this.FriendStreamEnabled = FriendStream;

			DataRow dataRow = null;
			using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
			{
				dbClient.AddParamWithValue("user_id", UserId);

				dataRow = dbClient.ReadDataRow("SELECT * FROM user_stats WHERE Id = @user_id LIMIT 1");

				if (dataRow == null)
				{
					dbClient.ExecuteQuery("INSERT INTO user_stats (Id) VALUES ('" + UserId + "')");
					dataRow = dbClient.ReadDataRow("SELECT * FROM user_stats WHERE Id = @user_id LIMIT 1");
				}

				this.dataTable_0 = dbClient.ReadDataTable("SELECT * FROM group_memberships WHERE userid = @user_id");

				IEnumerator enumerator;
				if (this.dataTable_0 != null)
				{
					enumerator = this.dataTable_0.Rows.GetEnumerator();

					try
					{
						while (enumerator.MoveNext())
						{
							DataRow dataRow2 = (DataRow)enumerator.Current;
							GroupsManager class2 = Groups.smethod_2((int)dataRow2["groupid"]);

							if (class2 == null)
							{
								DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM groups WHERE Id = " + (int)dataRow2["groupid"] + " LIMIT 1;");
								IEnumerator enumerator2 = dataTable.Rows.GetEnumerator();

								try
								{
									while (enumerator2.MoveNext())
									{
										DataRow dataRow3 = (DataRow)enumerator2.Current;
										if (!Groups.GroupsManager.ContainsKey((int)dataRow3["Id"]))
										{
											Groups.GroupsManager.Add((int)dataRow3["Id"], new GroupsManager((int)dataRow3["Id"], dataRow3, dbClient));
										}
									}
									continue;
								}
								finally
								{
									IDisposable disposable = enumerator2 as IDisposable;
									if (disposable != null)
									{
										disposable.Dispose();
									}
								}
							}

							if (!class2.list_0.Contains((int)UserId))
							{
								class2.method_0((int)UserId);
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}

					int num = (int)dataRow["groupid"];
					GroupsManager class3 = Groups.smethod_2(num);

					if (class3 != null)
						this.int_0 = num;
					else
						this.int_0 = 0;
				}
				else
				{
					this.int_0 = 0;
				}

				DataTable dataTable2 = dbClient.ReadDataTable("SELECT groupid FROM group_requests WHERE userid = '" + UserId + "';");
				enumerator = dataTable2.Rows.GetEnumerator();

				try
				{
					while (enumerator.MoveNext())
					{
						DataRow dataRow2 = (DataRow)enumerator.Current;
						this.list_0.Add((int)dataRow2["groupid"]);
					}
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

			this.RoomVisits = (int)dataRow["RoomVisits"];
			this.LoginTimestamp = (int)GoldTree.GetUnixTimestamp();
			this.OnlineTime = (int)dataRow["OnlineTime"];
			this.Respect = (int)dataRow["Respect"];

			this.RespectGiven = (int)dataRow["RespectGiven"];
			this.GiftsGiven = (int)dataRow["GiftsGiven"];

            this.FireworkPixelLoadedCount = (int)dataRow["fireworks"];

			this.GiftsReceived = (int)dataRow["GiftsReceived"];

			this.RespectPoints = (int)dataRow["DailyRespectPoints"];
			this.PetRespectPoints = (int)dataRow["DailyPetRespectPoints"];

			this.AchievementScore = (int)dataRow["AchievementScore"];

			this.CompletedQuests = new List<uint>();

			this.uint_7 = 0u;

			this.CurrentQuestId = (uint)dataRow["quest_id"];
			this.CurrentQuestProgress = (int)dataRow["quest_progress"];

			this.BuilderLevel = (int)dataRow["lev_builder"];
			this.IdentityLevel = (int)dataRow["lev_identity"];
			this.SocialLevel = (int)dataRow["lev_social"];
			this.ExplorerLevel = (int)dataRow["lev_explore"];

            this.RegularVisitor = (int)dataRow["RegularVisitor"];

            this.FootballGoalScorer = (int)dataRow["FootballGoalScorer"];
            this.FootballGoalHost = (int)dataRow["FootballGoalHost"];

            this.TilesLocked = (int)dataRow["TilesLocked"];

            this.StaffPicks = (int)dataRow["staff_picks"];

			if (Session != null)
			{
				this.SubscriptionManager = new SubscriptionManager(UserId, userDataFactory);
				this.BadgeComponent = new BadgeComponent(UserId, userDataFactory);
                this.InventoryComponent = new InventoryComponent(UserId, Session, userDataFactory);
				this.EffectsInventoryComponent = new AvatarEffectsInventoryComponent(UserId, Session, userDataFactory);

				this.bool_8 = false;
				this.bool_9 = false;

				foreach (DataRow dataRow3 in userDataFactory.GetRooms().Rows)
				{
					this.OwnedRooms.Add(GoldTree.GetGame().GetRoomManager().method_17((uint)dataRow3["Id"], dataRow3));
				}
			}
		}
		public void method_0(DatabaseClient class6_0)
		{
			this.dataTable_0 = class6_0.ReadDataTable("SELECT * FROM group_memberships WHERE userid = " + this.Id);
			if (this.dataTable_0 != null)
			{
				foreach (DataRow dataRow in this.dataTable_0.Rows)
				{
					GroupsManager @class = Groups.smethod_2((int)dataRow["groupid"]);
					if (@class == null)
					{
						DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM groups WHERE Id = " + (int)dataRow["groupid"] + " LIMIT 1;");
						IEnumerator enumerator2 = dataTable.Rows.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								DataRow dataRow2 = (DataRow)enumerator2.Current;
								if (!Groups.GroupsManager.ContainsKey((int)dataRow2["Id"]))
								{
									Groups.GroupsManager.Add((int)dataRow2["Id"], new GroupsManager((int)dataRow2["Id"], dataRow2, class6_0));
								}
							}
							continue;
						}
						finally
						{
							IDisposable disposable = enumerator2 as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					if (!@class.list_0.Contains((int)this.Id))
					{
						@class.method_0((int)this.Id);
					}
				}
				int num = class6_0.ReadInt32("SELECT groupid FROM user_stats WHERE Id = " + this.Id + " LIMIT 1");
				GroupsManager class2 = Groups.smethod_2(num);
				if (class2 != null)
				{
					this.int_0 = num;
				}
				else
				{
					this.int_0 = 0;
				}
			}
			else
			{
				this.int_0 = 0;
			}
		}

		internal void method_1(DatabaseClient class6_0)
		{
			this.OwnedRooms.Clear();
			class6_0.AddParamWithValue("name", this.Username);
			DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM rooms WHERE owner = @name ORDER BY Id ASC");
			foreach (DataRow dataRow in dataTable.Rows)
			{
				this.OwnedRooms.Add(GoldTree.GetGame().GetRoomManager().method_17((uint)dataRow["Id"], dataRow));
			}
		}

		public void method_2(UserDataFactory class12_1)
		{
			this.LoadAchievements(class12_1);
			this.LoadFavorites(class12_1);
			this.LoadIgnores(class12_1);
			this.LoadTags(class12_1);
			this.LoadQuests();
		}

		public bool HasFuse(string string_7)
		{
			if (GoldTree.GetGame().GetRoleManager().method_3(this.Id))
			{
				return GoldTree.GetGame().GetRoleManager().method_4(this.Id, string_7);
			}
			else
			{
				return GoldTree.GetGame().GetRoleManager().method_1(this.Rank, string_7);
			}
		}

		public int method_4()
		{
			if (this.IsJuniori)
			{
                return GoldTree.GetGame().GetRoleManager().method_9();
			}
			else
			{
				return GoldTree.GetGame().GetRoleManager().method_2(this.Rank);
			}
		}

		public void LoadFavorites(UserDataFactory class12_1)
		{
			this.list_1.Clear();

			DataTable dataTable_ = class12_1.GetFavorites();
			foreach (DataRow dataRow in dataTable_.Rows)
			{
				this.list_1.Add((uint)dataRow["room_id"]);
			}
		}

		public void LoadIgnores(UserDataFactory userdata)
		{
			DataTable dataTable_ = userdata.GetIgnores();
			foreach (DataRow dataRow in dataTable_.Rows)
			{
				this.list_2.Add((uint)dataRow["ignore_id"]);
			}
		}

		public void LoadTags(UserDataFactory userdata)
		{
			this.list_3.Clear();

			DataTable dataTable_ = userdata.GetTags();
			foreach (DataRow dataRow in dataTable_.Rows)
			{
				this.list_3.Add((string)dataRow["tag"]);
			}
			if (this.list_3.Count >= 5 && this.GetClient() != null)
			{
                this.TagAchievementsCompleted();
			}
		}

        public void LoadAchievements(UserDataFactory userdata)
        {
            DataTable dataTable = userdata.GetAchievements();

            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (!this.dictionary_0.ContainsKey((uint)dataRow["achievement_id"]))
                    {
                        this.dictionary_0.Add((uint)dataRow["achievement_id"], (int)dataRow["achievement_level"]);
                    }
                }
            }
        }

		public void Dispose()
		{
			if (!this.bool_9)
			{
				this.bool_9 = true;

				GoldTree.GetGame().GetClientManager().method_1(this.Id, this.Username);

				if (!this.bool_16)
				{
					this.bool_16 = true;
                    this.Online = false;

                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE users SET last_online = UNIX_TIMESTAMP(), users.online = '0', activity_points = '",
							this.ActivityPoints,
							"', activity_points_lastupdate = '",
							this.LastActivityPointsUpdate,
							"', credits = '",
							this.Credits,
							"' WHERE Id = '",
							this.Id,
							"' LIMIT 1;"
						}));

                        int num = (int)GoldTree.GetUnixTimestamp() - this.LoginTimestamp;

                        dbClient.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_stats SET RoomVisits = '",
							this.RoomVisits,
							"', OnlineTime = OnlineTime + ",
							num,
							", Respect = '",
							this.Respect,
							"', RespectGiven = '",
							this.RespectGiven,
							"', GiftsGiven = '",
							this.GiftsGiven,
							"', GiftsReceived = '",
							this.GiftsReceived,
                            "', FootballGoalScorer = '",
                            this.FootballGoalScorer,
                            "', FootballGoalHost = '",
                            this.FootballGoalHost,
                            "', TilesLocked = '",
                            this.TilesLocked,
                            "', staff_picks = '",
                            this.StaffPicks,
							"' WHERE Id = '",
							this.Id,
							"' LIMIT 1; "
						}));
                    }
				}

                if (this.InRoom && this.CurrentRoom != null)
					this.CurrentRoom.method_47(this.Session, false, false);

				if (this.Messenger != null)
				{
					this.Messenger.bool_0 = true;
					this.Messenger.method_5(true);
					this.Messenger = null;
				}

				if (this.SubscriptionManager != null)
				{
                    this.SubscriptionManager.GetSubscriptions().Clear();
					this.SubscriptionManager = null;
				}

                this.InventoryComponent.SavePets();
			}
		}

		internal void SendToRoom(uint roomId)
		{
			if (ServerConfiguration.EnableRoomLog)
			{
				using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
				{
					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO user_roomvisits (user_id,room_id,entry_timestamp,exit_timestamp,hour,minute) VALUES ('",
						this.Id,
						"','",
						roomId,
						"',UNIX_TIMESTAMP(),'0','",
						DateTime.Now.Hour,
						"','",
						DateTime.Now.Minute,
						"')"
					}));
				}
			}

			this.CurrentRoomId = roomId;

            if (this.CurrentQuestId > 0 && this.CurrentRoom.Owner != this.Username && GoldTree.GetGame().GetQuestManager().GetQuestAction(this.CurrentQuestId) == "ENTEROTHERSROOM")
                GoldTree.GetGame().GetQuestManager().ProgressUserQuest(this.CurrentQuestId, this.GetClient());

			this.Messenger.method_5(false);
		}

		public void RemoveFromRoom()
		{
            try
            {
                if (ServerConfiguration.EnableRoomLog)
                {
                    using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                    {
                        @class.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_roomvisits SET exit_timestamp = UNIX_TIMESTAMP() WHERE room_id = '",
							this.CurrentRoomId,
							"' AND user_id = '",
							this.Id,
							"' ORDER BY entry_timestamp DESC LIMIT 1"
						}));
                    }
                }
            }
            catch { }

			this.CurrentRoomId = 0u;

			if (this.Messenger != null)
				this.Messenger.method_5(false);
		}

		public void method_12()
		{
			if (this.GetMessenger() == null)
			{
				this.Messenger = new HabboMessenger(this.Id);

				this.Messenger.method_0(this.UserDataFactory);
				this.Messenger.method_1(this.UserDataFactory);

				GameClient client = this.GetClient();

				if (client != null)
				{
					client.SendMessage(this.Messenger.method_21());
					client.SendMessage(this.Messenger.method_23());

					this.Messenger.method_5(true);
				}
			}
		}

		public void UpdateCredits(bool updateDatabase)
		{
			ServerMessage Message = new ServerMessage(6u);

			Message.AppendStringWithBreak(this.Credits + ".0");

			this.Session.SendMessage(Message);

			if (updateDatabase)
			{
				using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
				{
					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET credits = '",
						this.Credits,
						"' WHERE Id = '",
						this.Id,
						"' LIMIT 1;"
					}));
				}
			}
		}

		public void UpdateVipPoints(bool getPoints, bool updateDatabase)
		{
			if (getPoints)
			{
				using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
				{
					this.VipPoints = dbClient.ReadInt32("SELECT vip_points FROM users WHERE Id = '" + this.Id + "' LIMIT 1;");
				}
			}

			if (updateDatabase)
			{
				using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
				{
					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET vip_points = '",
						this.VipPoints,
						"' WHERE Id = '",
						this.Id,
						"' LIMIT 1;"
					}));
				}
			}

			this.method_16(0);
		}

		public void UpdateActivityPoints(bool updateDatabase)
		{
			this.method_16(0);

			if (updateDatabase)
			{
				using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
				{
					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE users SET activity_points = '",
						this.ActivityPoints,
						"' WHERE Id = '",
						this.Id,
						"' LIMIT 1;"
					}));
				}
			}
		}

		public void method_16(int int_25)
		{
			ServerMessage Message = new ServerMessage(438u);
			Message.AppendInt32(this.ActivityPoints);
			Message.AppendInt32(int_25);
			Message.AppendInt32(0);

			ServerMessage Message2 = new ServerMessage(438u);
			Message2.AppendInt32(this.VipPoints);
			Message2.AppendInt32(0);
			Message2.AppendInt32(1);

			ServerMessage Message3 = new ServerMessage(438u);
			Message3.AppendInt32(this.VipPoints);
			Message3.AppendInt32(0);
			Message3.AppendInt32(2);

			ServerMessage Message4 = new ServerMessage(438u);
			Message4.AppendInt32(this.VipPoints);
			Message4.AppendInt32(0);
			Message4.AppendInt32(3);

			ServerMessage Message5 = new ServerMessage(438u);
			Message5.AppendInt32(this.VipPoints);
			Message5.AppendInt32(0);
			Message5.AppendInt32(4);

			this.Session.SendMessage(Message);
			this.Session.SendMessage(Message2);
			this.Session.SendMessage(Message3);
			this.Session.SendMessage(Message4);
			this.Session.SendMessage(Message5);
		}

		public void Mute()
		{
			if (!this.IsMuted)
			{
				this.GetClient().SendNotification("You have been muted by a moderator.");
				this.IsMuted = true;
			}
		}

		public void UnMute()
		{
			this.IsMuted = false;
		}

		public void LoadQuests()
		{
			this.CompletedQuests.Clear();
			DataTable dataTable = null;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				dataTable = @class.ReadDataTable("SELECT quest_id FROM user_quests WHERE user_id = '" + this.Id + "'");
			}
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.CompletedQuests.Add((uint)dataRow["quest_Id"]);
				}
			}
		}

		public void UpdateLook(bool getFromDatabase, GameClient client)
		{
			ServerMessage Message = new ServerMessage(266u);

			Message.AppendInt32(-1);

			Message.AppendStringWithBreak(client.GetHabbo().Figure);
			Message.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
			Message.AppendStringWithBreak(client.GetHabbo().Motto);

			Message.AppendInt32(client.GetHabbo().AchievementScore);

			Message.AppendStringWithBreak("");

			client.SendMessage(Message);

            if (client.GetHabbo().InRoom)
			{
				Room room = client.GetHabbo().CurrentRoom;

				if (room != null)
				{
					RoomUser roomUser = room.GetRoomUserByHabbo(client.GetHabbo().Id);

					if (roomUser != null)
					{
						if (getFromDatabase)
						{
							DataRow dataRow = null;
							using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
							{
								class2.AddParamWithValue("userid", client.GetHabbo().Id);
								dataRow = class2.ReadDataRow("SELECT * FROM users WHERE Id = @userid LIMIT 1");
							}
							client.GetHabbo().Motto = GoldTree.FilterString((string)dataRow["motto"]);
							client.GetHabbo().Figure = GoldTree.FilterString((string)dataRow["look"]);
						}

						ServerMessage Message2 = new ServerMessage(266u);

						Message2.AppendInt32(roomUser.VirtualId);

						Message2.AppendStringWithBreak(client.GetHabbo().Figure);
						Message2.AppendStringWithBreak(client.GetHabbo().Gender.ToLower());
						Message2.AppendStringWithBreak(client.GetHabbo().Motto);

						Message2.AppendInt32(client.GetHabbo().AchievementScore);

						Message2.AppendStringWithBreak("");

						room.SendMessage(Message2, null);
					}
				}
			}
		}

		public void UpdateRights()
		{
			DataRow dataRow;

			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				dataRow = @class.ReadDataRow("SELECT vip FROM users WHERE Id = '" + this.Id + "' LIMIT 1;");
			}

			this.IsVIP = GoldTree.StringToBoolean(dataRow["vip"].ToString());

			ServerMessage Message = new ServerMessage(2u);

			if (this.IsVIP || ServerConfiguration.HabboClubForClothes)
			{
				Message.AppendInt32(2);
			}
			else
			{
				if (this.GetSubscriptionManager().HasSubscription("habbo_club"))
				{
					Message.AppendInt32(1);
				}
				else
				{
					Message.AppendInt32(0);
				}
			}
			if (this.HasFuse("acc_anyroomowner"))
			{
				Message.AppendInt32(7);
			}
			else
			{
				if (this.HasFuse("acc_anyroomrights"))
				{
					Message.AppendInt32(5);
				}
				else
				{
					if (this.HasFuse("acc_supporttool"))
					{
						Message.AppendInt32(4);
					}
					else
					{
						if (this.IsVIP || ServerConfiguration.HabboClubForClothes || this.GetSubscriptionManager().HasSubscription("habbo_club"))
						{
							Message.AppendInt32(2);
						}
						else
						{
							Message.AppendInt32(0);
						}
					}
				}
			}

			this.GetClient().SendMessage(Message);
		}

		public void Whisper(string str)
		{
			Room room = GoldTree.GetGame().GetRoomManager().GetRoom(this.CurrentRoomId);

			if (room != null)
			{
				RoomUser roomUser = room.GetRoomUserByHabbo(this.Id);

				ServerMessage Message = new ServerMessage(25u);

				Message.AppendInt32(roomUser.VirtualId);
				Message.AppendStringWithBreak(str);
				Message.AppendBoolean(false);

				this.GetClient().SendMessage(Message);
			}
		}

        public void CheckFireworkAchievements()
        {
            int Count = FireworkPixelLoadedCount;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 20) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 1); }
            if (Count >= 100) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 2); }
            if (Count >= 420) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 3); }
            if (Count >= 600) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 4); }
            if (Count >= 1920) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 5); }
            if (Count >= 3120) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 6); }
            if (Count >= 4620) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 7); }
            if (Count >= 6420) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 8); }
            if (Count >= 8520) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 9); }
            if (Count >= 10920) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 13u, 10); }
        }

        public void MottoAchievementsCompleted()
        {
            GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 2u, 1);
        }

        public void TagAchievementsCompleted()
        {
            GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 7u, 1);
        }

        public void AvatarLookAchievementsCompleted()
        {
            GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 1u, 1);
        }

        public void CallGuideBotAchievementsCompleted()
        {
            GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 3u, 1);
        }

        public void ChangeNamaAchievementsCompleted()
        {
            GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 5u, 1);
        }

        public void CheckRoomEntryAchievements()
        {
            int Count = RoomVisits;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 5) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 1); }
            if (Count >= 20) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 2); }
            if (Count >= 50) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 3); }
            if (Count >= 100) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 4); }
            if (Count >= 160) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 5); }
            if (Count >= 240) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 6); }
            if (Count >= 360) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 7); }
            if (Count >= 500) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 8); }
            if (Count >= 660) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 9); }
            if (Count >= 860) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 10); }
            if (Count >= 1080) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 11); }
            if (Count >= 1320) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 12); }
            if (Count >= 1580) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 13); }
            if (Count >= 1860) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 14); }
            if (Count >= 2160) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 15); }
            if (Count >= 2480) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 16); }
            if (Count >= 2820) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 17); }
            if (Count >= 3180) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 18); }
            if (Count >= 3560) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 19); }
            if (Count >= 3960) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 8u, 20); }
        }

        public void CheckRespectGivedAchievements()
        {
            int Count = RespectGiven;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 2) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 1); }
            if (Count >= 5) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 2); }
            if (Count >= 10) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 3); }
            if (Count >= 20) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 4); }
            if (Count >= 40) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 5); }
            if (Count >= 70) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 6); }
            if (Count >= 110) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 7); }
            if (Count >= 170) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 8); }
            if (Count >= 250) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 9); }
            if (Count >= 350) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 10); }
            if (Count >= 470) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 11); }
            if (Count >= 610) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 12); }
            if (Count >= 770) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 13); }
            if (Count >= 950) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 14); }
            if (Count >= 1150) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 15); }
            if (Count >= 1370) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 16); }
            if (Count >= 1610) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 17); }
            if (Count >= 1870) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 18); }
            if (Count >= 2150) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 19); }
            if (Count >= 2450) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 4u, 20); }
        }

        public void CheckRespectReceivedAchievements()
        {
            int Count = Respect;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 1) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 1); }
            if (Count >= 6) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 2); }
            if (Count >= 16) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 3); }
            if (Count >= 66) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 4); }
            if (Count >= 166) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 5); }
            if (Count >= 366) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 6); }
            if (Count >= 566) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 7); }
            if (Count >= 766) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 8); }
            if (Count >= 966) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 9); }
            if (Count >= 1166) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 6u, 10); }
        }

        public void CheckGiftReceivedAchievements()
        {
            int Count = GiftsReceived;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 1) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 1); }
            if (Count >= 6) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 2); }
            if (Count >= 14) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 3); }
            if (Count >= 26) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 4); }
            if (Count >= 46) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 5); }
            if (Count >= 86) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 6); }
            if (Count >= 146) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 7); }
            if (Count >= 236) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 8); }
            if (Count >= 366) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 9); }
            if (Count >= 566) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 11u, 10); }
        }

        public void CheckGiftGivenAchievements()
        {
            int Count = GiftsGiven;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 1) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 1); }
            if (Count >= 6) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 2); }
            if (Count >= 14) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 3); }
            if (Count >= 26) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 4); }
            if (Count >= 46) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 5); }
            if (Count >= 86) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 6); }
            if (Count >= 146) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 7); }
            if (Count >= 236) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 8); }
            if (Count >= 366) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 9); }
            if (Count >= 566) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 10); }
            if (Count >= 816) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 11); }
            if (Count >= 1066) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 12); }
            if (Count >= 1316) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 13); }
            if (Count >= 1566) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 14); }
            if (Count >= 1816) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 10u, 15); }
        }

        public void CheckTotalTimeOnlineAchievements()
        {
            int Count = OnlineTime;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 1800) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 1); }
            if (Count >= 3600) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 2); }
            if (Count >= 7200) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 3); }
            if (Count >= 10800) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 4); }
            if (Count >= 21600) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 5); }
            if (Count >= 43200) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 6); }
            if (Count >= 86400) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 7); }
            if (Count >= 129600) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 8); }
            if (Count >= 172800) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 9); }
            if (Count >= 259200) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 10); }
            if (Count >= 432000) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 11); }
            if (Count >= 604800) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 12); }
            if (Count >= 1209600) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 13); }
            if (Count >= 1814400) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 14); }
            if (Count >= 2419200) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 15); }
            if (Count >= 3024000) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 16); }
            if (Count >= 3628800) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 17); }
            if (Count >= 4838400) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 18); }
            if (Count >= 6048000) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 19); }
            if (Count >= 8294400) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 15u, 20); }
        }

        public void CheckPetCountAchievements()
        {
            int Count = 0;

            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                DataTable dataTable = dbClient.ReadDataTable("SELECT user_id FROM  `user_pets` WHERE user_id = @sessionid;");

                if (dataTable == null)
                {
                    Count = 0;
                }
                else
                {
                    Count = dataTable.Rows.Count;
                }
            }

            PetBuyed = Count = Count + NewPetsBuyed;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 1) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 1); }
            if (Count >= 5) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 2); }
            if (Count >= 10) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 3); }
            if (Count >= 15) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 4); }
            if (Count >= 20) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 5); }
            if (Count >= 25) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 6); }
            if (Count >= 30) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 7); }
            if (Count >= 40) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 8); }
            if (Count >= 50) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 9); }
            if (Count >= 75) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 14u, 10); }
        }

        public void CheckHappyHourAchievements()
        {
            string s = DateTime.Now.ToString("HH:mm:ss");
            DateTime dt = DateTime.ParseExact(s, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
            var time = dt.TimeOfDay;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday || DateTime.Now.DayOfWeek == DayOfWeek.Wednesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday || DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                if (time >= new TimeSpan(15, 00, 00) && time <= new TimeSpan(17, 00, 00))
                {
                    GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 9u, 1);
                }
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                if (time >= new TimeSpan(13, 00, 00) && time <= new TimeSpan(14, 00, 00))
                {
                    GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 9u, 1);
                }
            }
        }

        public void CheckTrueHabboAchievements()
        {
            //DateTime AccountCreated = UnixTimeStampToDateTime(double.Parse(Session.GetHabbo().DataCadastro));
            //DateTime AccountCreated2 = UnixTimeStampToDateTime2(double.Parse(Session.GetHabbo().DataCadastro));

            double dAccountCreated;
            DateTime AccountCreated;

            if (double.TryParse(Session.GetHabbo().DataCadastro, out dAccountCreated))
                AccountCreated = GoldTree.TimestampToDate(dAccountCreated);
            else
            {
                if (!DateTime.TryParseExact(Session.GetHabbo().DataCadastro, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out AccountCreated))
                    return;
            }

            //string[] dataC = Session.GetHabbo().DataCadastro.Split('-');
            //string[] dataC = AccountCreated.Split('.');
            var hoje = DateTime.Now.ToString("dd-MM-yyyy");
            string[] Hoje = hoje.Split('-');

           /*string Mes = "01";
            switch (dataC[1])
            {
                case "Jan": { Mes = "01"; break; }
                case "Feb": { Mes = "02"; break; }
                case "Mar": { Mes = "03"; break; }
                case "Apr": { Mes = "04"; break; }
                case "May": { Mes = "05"; break; }
                case "Jun": { Mes = "06"; break; }
                case "Jul": { Mes = "07"; break; }
                case "Aug": { Mes = "08"; break; }
                case "Sep": { Mes = "09"; break; }
                case "Oct": { Mes = "10"; break; }
                case "Nov": { Mes = "11"; break; }
                case "Dec": { Mes = "12"; break; }
            }*/

            DateTime dataCadastro = Convert.ToDateTime(AccountCreated);
            DateTime data_hoje = new DateTime(int.Parse(Hoje[2]), int.Parse(Hoje[1]), int.Parse(Hoje[0]));

            TimeSpan dif = data_hoje.Subtract(dataCadastro);

            int Dias = dif.Days;
            Session.GetHabbo().RegistrationDuration = Dias;

            if (Dias >= 1) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 1); }
            if (Dias >= 3) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 2); }
            if (Dias >= 10) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 3); }
            if (Dias >= 20) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 4); }
            if (Dias >= 30) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 5); }
            if (Dias >= 56) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 6); }
            if (Dias >= 84) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 7); }
            if (Dias >= 126) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 8); }
            if (Dias >= 168) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 9); }
            if (Dias >= 224) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 10); }
            if (Dias >= 280) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 11); }
            if (Dias >= 365) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 12); }
            if (Dias >= 548) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 13); }
            if (Dias >= 730) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 14); }
            if (Dias >= 913) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 15); }
            if (Dias >= 1095) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 16); }
            if (Dias >= 1278) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 17); }
            if (Dias >= 1460) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 18); }
            if (Dias >= 1643) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 19); }
            if (Dias >= 1825) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 17u, 20); }
        }

        public void CheckRegularVisitorAchievements()
        {
            DateTime LastLoggedIn = GoldTree.TimestampToDate(double.Parse(Session.GetHabbo().LastOnline));
            DateTime yesterday = DateTime.Now.AddDays(-1);

            if (LastLoggedIn.ToString("dd-MM-yyyy") == yesterday.ToString("dd-MM-yyyy"))
            {
                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    RegularVisitor++;
                    dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                    dbClient.ExecuteQuery("UPDATE user_stats SET RegularVisitor = RegularVisitor + 1 WHERE id = @sessionid LIMIT 1");
                    dbClient.AddParamWithValue("daily_respect_points", Session.GetHabbo().RespectPoints);
                    dbClient.AddParamWithValue("daily_pet_respect_points", Session.GetHabbo().PetRespectPoints);
                    dbClient.ExecuteQuery("UPDATE user_stats SET DailyRespectPoints = @daily_respect_points, DailyPetRespectPoints = @daily_pet_respect_points WHERE id = @sessionid");
                }
            }
            else if (LastLoggedIn.ToString("dd-MM-yyyy") == DateTime.Now.ToString("dd-MM-yyyy"))
            {

            }
            else
            {
                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    RegularVisitor = 1;
                    dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                    dbClient.ExecuteQuery("UPDATE user_stats SET RegularVisitor = 1 WHERE id = @sessionid LIMIT 1");
                    dbClient.AddParamWithValue("daily_respect_points", Session.GetHabbo().RespectPoints);
                    dbClient.AddParamWithValue("daily_pet_respect_points", Session.GetHabbo().PetRespectPoints);
                    dbClient.ExecuteQuery("UPDATE user_stats SET DailyRespectPoints = @daily_respect_points, DailyPetRespectPoints = @daily_pet_respect_points WHERE id = @sessionid");
                }
            }

            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
            {
                dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                dbClient.ExecuteQuery("UPDATE users SET last_loggedin = UNIX_TIMESTAMP() WHERE id = @sessionid");
            }

            int Count = RegularVisitor;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 5) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 1); }
            if (Count >= 8) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 2); }
            if (Count >= 15) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 3); }
            if (Count >= 28) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 4); }
            if (Count >= 35) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 5); }
            if (Count >= 50) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 6); }
            if (Count >= 60) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 7); }
            if (Count >= 70) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 8); }
            if (Count >= 80) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 9); }
            if (Count >= 100) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 10); }
            if (Count >= 120) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 11); }
            if (Count >= 140) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 12); }
            if (Count >= 160) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 13); }
            if (Count >= 180) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 14); }
            if (Count >= 200) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 15); }
            if (Count >= 220) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 16); }
            if (Count >= 240) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 17); }
            if (Count >= 260) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 18); }
            if (Count >= 280) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 19); }
            if (Count >= 300) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 18u, 20); }
        }

        public void CheckHCAchievements()
        {
            int Count = Session.GetHabbo().GetSubscriptionManager().CalculateHCSubscription(Session.GetHabbo());

            if (Count < 0)
            {
                return;
            }

            if (Count >= 0) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 16u, 1); }
            if (Count >= 12) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 16u, 2); }
            if (Count >= 24) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 16u, 3); }
            if (Count >= 36) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 16u, 4); }
            if (Count >= 48) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 16u, 5); }
        }

        public void CheckFootballGoalScorerScoreAchievements()
        {
            int Count = Session.GetHabbo().FootballGoalScorer;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 1) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 19u, 1); }
            if (Count >= 10) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 19u, 2); }
            if (Count >= 100) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 19u, 3); }
            if (Count >= 1000) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 19u, 4); }
            if (Count >= 10000) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 19u, 5); }
        }

        public void CheckFootballGoalHostScoreAchievements()
        {
            int Count = Session.GetHabbo().FootballGoalHost;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 1) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 20u, 1); }
            if (Count >= 20) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 20u, 2); }
            if (Count >= 400) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 20u, 3); }
            if (Count >= 8000) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 20u, 4); }
            if (Count >= 160000) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 20u, 5); }
        }

        public void CheckBattleBanzaiTilesLockedAchievements()
        {
            int Count = TilesLocked;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 25) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 1); }
            if (Count >= 65) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 2); }
            if (Count >= 125) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 3); }
            if (Count >= 205) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 4); }
            if (Count >= 335) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 5); }
            if (Count >= 525) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 6); }
            if (Count >= 805) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 7); }
            if (Count >= 1235) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 8); }
            if (Count >= 1875) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 9); }
            if (Count >= 2875) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 10); }
            if (Count >= 4375) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 11); }
            if (Count >= 6875) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 12); }
            if (Count >= 10775) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 13); }
            if (Count >= 17075) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 14); }
            if (Count >= 27175) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 15); }
            if (Count >= 43275) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 16); }
            if (Count >= 69075) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 17); }
            if (Count >= 110375) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 18); }
            if (Count >= 176375) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 19); }
            if (Count >= 282075) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 21u, 20); }
        }

        public void CheckStaffPicksAchievement()
        {
            int Count = Session.GetHabbo().StaffPicks;

            if (Count <= 0)
            {
                return;
            }

            if (Count >= 1) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 1); }
            if (Count >= 2) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 2); }
            if (Count >= 3) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 3); }
            if (Count >= 4) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 4); }
            if (Count >= 5) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 5); }
            if (Count >= 6) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 6); }
            if (Count >= 7) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 7); }
            if (Count >= 8) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 8); }
            if (Count >= 9) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 9); }
            if (Count >= 10) { GoldTree.GetGame().GetAchievementManager().addAchievement(Session, 22u, 10); }
        }
    }
}