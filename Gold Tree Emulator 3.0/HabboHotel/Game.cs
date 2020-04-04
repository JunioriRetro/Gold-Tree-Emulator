using System;
using System.Data;
using System.Threading.Tasks;
using GoldTree.HabboHotel.Misc;
using GoldTree.Core;
using GoldTree.HabboHotel.Navigators;
using GoldTree.HabboHotel.Catalogs;
using GoldTree.HabboHotel.Support;
using GoldTree.HabboHotel.Roles;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
using GoldTree.HabboHotel.Advertisements;
using GoldTree.HabboHotel.Achievements;
using GoldTree.HabboHotel.RoomBots;
using GoldTree.HabboHotel.Quests;
using GoldTree.Util;
using GoldTree.Storage;
using System.Threading;
namespace GoldTree.HabboHotel
{
	internal sealed class Game
	{
		private GameClientManager ClientManager;
		private ModerationBanManager BanManager;
		private RoleManager RoleManager;
		private HelpTool HelpTool;
		private Catalog Catalog;
		private Navigator Navigator;
		private ItemManager ItemManager;
		private RoomManager RoomManager;
		private AdvertisementManager AdvertisementManager;
		private PixelManager PixelManager;
		private AchievementManager AchievementManager;
		private ModerationTool ModerationTool;
		private BotManager BotManager;
		private Task task_0;
		private NavigatorCache NavigatorCache;
		private Marketplace Marketplace;
		private QuestManager QuestManager;
		private GoldTreeEnvironment GoldTreeEnvironment;
		private Groups Groups;

        private Task GameLoop;
        private bool GameLoopActive;
        private bool GameLoopEnded = true;
        private const int GameLoopSleepTime = 25;

		public Game(int conns)
		{
			this.ClientManager = new GameClientManager(conns);

			if (GoldTree.GetConfig().data["client.ping.enabled"] == "1")
			{
				this.ClientManager.method_10();
			}

			DateTime now = DateTime.Now;

			Logging.Write("Connecting to the database.. ");

            try
            {
                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    Logging.WriteLine("completed!", ConsoleColor.Green);

                    GoldTree.Game = this;
                    this.LoadServerSettings(dbClient);
                    this.BanManager = new ModerationBanManager();
                    this.RoleManager = new RoleManager();
                    this.HelpTool = new HelpTool();
                    this.Catalog = new Catalog();
                    this.Navigator = new Navigator();
                    this.ItemManager = new ItemManager();
                    this.RoomManager = new RoomManager();
                    this.AdvertisementManager = new AdvertisementManager();
                    this.PixelManager = new PixelManager();
                    this.AchievementManager = new AchievementManager();
                    this.ModerationTool = new ModerationTool();
                    this.BotManager = new BotManager();
                    this.Marketplace = new Marketplace();
                    this.QuestManager = new QuestManager();
                    this.GoldTreeEnvironment = new GoldTreeEnvironment();

                    this.Groups = new Groups();

                    GoldTreeEnvironment.LoadExternalTexts(dbClient);

                    this.BanManager.Initialise(dbClient);

                    this.RoleManager.method_0(dbClient);

                    this.HelpTool.method_0(dbClient);
                    this.HelpTool.method_3(dbClient);

                    this.ModerationTool.method_1(dbClient);
                    this.ModerationTool.method_2(dbClient);
                    this.ItemManager.method_0(dbClient);
                    this.Catalog.method_0(dbClient);
                    this.Catalog.method_1();
                    this.Navigator.method_0(dbClient);
                    this.RoomManager.method_8(dbClient);
                    this.RoomManager.method_0();
                    this.NavigatorCache = new NavigatorCache();
                    this.AdvertisementManager.method_0(dbClient);
                    this.BotManager.method_0(dbClient);
                    AchievementManager.smethod_0(dbClient);
                    this.PixelManager.method_0();
                    ChatCommandHandler.smethod_0(dbClient);
                    this.QuestManager.method_0();
                    Groups.smethod_0(dbClient);
                    this.RestoreStatistics(dbClient, 1);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException e)
            {
                Logging.WriteLine("failed!", ConsoleColor.Red);
                Logging.WriteLine(e.Message + " Check the given configuration details in config.conf\r\n", ConsoleColor.Yellow);
                GoldTree.Destroy("", true, true);

                return;
            }

			this.task_0 = new Task(new Action(LowPriorityWorker.Initialise));
			this.task_0.Start();

            StartGameLoop();
		}

		public void RestoreStatistics(DatabaseClient dbClient, int status)
		{
			Logging.Write(GoldTreeEnvironment.GetExternalText("emu_cleandb"));
			bool flag = true;
			try
			{
				if (int.Parse(GoldTree.GetConfig().data["debug"]) == 1)
				{
					flag = false;
				}
			}
			catch
			{
			}
			if (flag)
			{
				dbClient.ExecuteQuery("UPDATE users SET online = '0' WHERE online != '0'");
				dbClient.ExecuteQuery("UPDATE rooms SET users_now = '0' WHERE users_now != '0'");
				dbClient.ExecuteQuery("UPDATE user_roomvisits SET exit_timestamp = UNIX_TIMESTAMP() WHERE exit_timestamp <= 0");
				dbClient.ExecuteQuery(string.Concat(new object[]
				{
					"UPDATE server_status SET status = '",
					status,
					"', users_online = '0', rooms_loaded = '0', server_ver = '",
					GoldTree.PrettyVersion,
					"', stamp = UNIX_TIMESTAMP() LIMIT 1;"
				}));
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}

		public void ContinueLoading()
		{
			if (this.task_0 != null)
			{
				this.task_0 = null;
			}

            try
            {
                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    this.RestoreStatistics(dbClient, 0);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException e) { /* database connection not available */ }

			if (this.GetClientManager() != null)
			{
				this.GetClientManager().method_6();
				this.GetClientManager().method_11();
			}

			if (this.GetPixelManager() != null)
			{
				this.PixelManager.KeepAlive = false;
			}

			this.ClientManager = null;
			this.BanManager = null;
			this.RoleManager = null;
			this.HelpTool = null;
			this.Catalog = null;
			this.Navigator = null;
			this.ItemManager = null;
			this.RoomManager = null;
			this.AdvertisementManager = null;
			this.PixelManager = null;
		}

		public GameClientManager GetClientManager()
		{
			return this.ClientManager;
		}

		public ModerationBanManager GetBanManager()
		{
			return this.BanManager;
		}

		public RoleManager GetRoleManager()
		{
			return this.RoleManager;
		}

		public HelpTool GetHelpTool()
		{
			return this.HelpTool;
		}

		public Catalog GetCatalog()
		{
			return this.Catalog;
		}

		public Navigator GetNavigator()
		{
			return this.Navigator;
		}

		public ItemManager GetItemManager()
		{
			return this.ItemManager;
		}

		public RoomManager GetRoomManager()
		{
			return this.RoomManager;
		}

		public AdvertisementManager GetAdvertisementManager()
		{
			return this.AdvertisementManager;
		}

		public PixelManager GetPixelManager()
		{
			return this.PixelManager;
		}

		public AchievementManager GetAchievementManager()
		{
			return this.AchievementManager;
		}

		public ModerationTool GetModerationTool()
		{
			return this.ModerationTool;
		}

		public BotManager GetBotManager()
		{
			return this.BotManager;
		}

		internal NavigatorCache GetNavigatorCache()
		{
			return this.NavigatorCache;
		}

		public QuestManager GetQuestManager()
		{
			return this.QuestManager;
		}

		public void LoadServerSettings(DatabaseClient class6_0)
		{
			Logging.Write("Loading your settings..");

			DataRow dataRow = class6_0.ReadDataRow("SELECT * FROM server_settings LIMIT 1");

			ServerConfiguration.RoomUserLimit = (int)dataRow["MaxRoomsPerUser"];

			ServerConfiguration.MOTD = (string)dataRow["motd"];

			ServerConfiguration.CreditingInterval = (int)dataRow["timer"];

			ServerConfiguration.CreditingAmount = (int)dataRow["credits"];
			ServerConfiguration.PointingAmount = (int)dataRow["pixels"];
			ServerConfiguration.PixelingAmount = (int)dataRow["points"];

			ServerConfiguration.PixelLimit = (int)dataRow["pixels_max"];
			ServerConfiguration.CreditLimit = (int)dataRow["credits_max"];
			ServerConfiguration.PointLimit = (int)dataRow["points_max"];

			ServerConfiguration.PetsPerRoomLimit = (int)dataRow["MaxPetsPerRoom"];

			ServerConfiguration.MarketplacePriceLimit = (int)dataRow["MaxMarketPlacePrice"];
			ServerConfiguration.MarketplaceTax = (int)dataRow["MarketPlaceTax"];

			ServerConfiguration.DDoSProtectionEnabled = GoldTree.StringToBoolean(dataRow["enable_antiddos"].ToString());

			ServerConfiguration.HabboClubForClothes = GoldTree.StringToBoolean(dataRow["vipclothesforhcusers"].ToString());

			ServerConfiguration.EnableChatlog = GoldTree.StringToBoolean(dataRow["enable_chatlogs"].ToString());
			ServerConfiguration.EnableCommandLog = GoldTree.StringToBoolean(dataRow["enable_cmdlogs"].ToString());
			ServerConfiguration.EnableRoomLog = GoldTree.StringToBoolean(dataRow["enable_roomlogs"].ToString());

			ServerConfiguration.EnableExternalLinks = (string)dataRow["enable_externalchatlinks"];

			ServerConfiguration.EnableSSO = GoldTree.StringToBoolean(dataRow["enable_securesessions"].ToString());

			ServerConfiguration.AllowFurniDrops = GoldTree.StringToBoolean(dataRow["allow_friendfurnidrops"].ToString());

			ServerConfiguration.EnableRedeemCredits = GoldTree.StringToBoolean(dataRow["enable_cmd_redeemcredits"].ToString());
            ServerConfiguration.EnableRedeemPixels = GoldTree.StringToBoolean(dataRow["enable_cmd_redeempixels"].ToString());
            ServerConfiguration.EnableRedeemShells = GoldTree.StringToBoolean(dataRow["enable_cmd_redeemshells"].ToString());

			ServerConfiguration.UnloadCrashedRooms = GoldTree.StringToBoolean(dataRow["unload_crashedrooms"].ToString());

			ServerConfiguration.ShowUsersAndRoomsInAbout = GoldTree.StringToBoolean(dataRow["ShowUsersAndRoomsInAbout"].ToString());

			ServerConfiguration.SleepTimer = (int)dataRow["idlesleep"];
			ServerConfiguration.KickTimer = (int)dataRow["idlekick"];

			ServerConfiguration.IPLastBan = GoldTree.StringToBoolean(dataRow["ip_lastforbans"].ToString());

            ServerConfiguration.StaffPicksID = (int)dataRow["StaffPicksCategoryID"];

            ServerConfiguration.VIPHotelAlertInterval = (double)dataRow["vipha_interval"];
            ServerConfiguration.VIPHotelAlertLinkInterval = (double)dataRow["viphal_interval"];

            ServerConfiguration.PreventDoorPush = GoldTree.StringToBoolean(dataRow["DisableOtherUsersToMovingOtherUsersToDoor"].ToString());
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}

        internal void StartGameLoop()
        {
            GameLoopEnded = false;
            GameLoopActive = true;
            GameLoop = new Task(MainGameLoop);
            GameLoop.Start();
        }

        internal void StopGameLoop()
        {
            GameLoopActive = false;

            while (!GameLoopEnded)
            {
                Thread.Sleep(GameLoopSleepTime);
            }
        }

        private void MainGameLoop()
        {
            while (GameLoopActive)
            {
                try
                {
                    RoomManager.OnCycle();
                }
                catch
                {
                }
                Thread.Sleep(GameLoopSleepTime);
            }

            GameLoopEnded = true;
        }
	}
}
