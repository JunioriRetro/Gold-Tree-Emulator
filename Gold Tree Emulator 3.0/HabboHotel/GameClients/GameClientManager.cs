using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Support;
using GoldTree.HabboHotel.Achievements;
using GoldTree.Net;
using GoldTree.Util;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.GameClients
{
	internal sealed class GameClientManager
	{
		private Task task_0;

		private GameClient[] Clients;

		private Hashtable hashtable_0;
		private Hashtable hashtable_1;

		private Timer DisposeTimer;

        private List<SocketConnection> DisposeQueue;

		public int ClientCount
		{
			get
			{
                if (this.Clients == null)
                    return 0;

                int num = 0;

                for (int i = 0; i < this.Clients.Length; i++)
                {
                    if (this.Clients[i] != null && this.Clients[i].GetHabbo() != null && !string.IsNullOrEmpty(this.Clients[i].GetHabbo().Username))
                    {
                        num++;
                    }
                }

                return num;
			}
		}

		public GameClientManager(int clientCapacity)
		{
			this.hashtable_0 = new Hashtable();
			this.hashtable_1 = new Hashtable();

			this.Clients = new GameClient[clientCapacity];

            this.DisposeQueue = new List<SocketConnection>();
			this.DisposeTimer = new Timer(new TimerCallback(this.DisposeTimerCallback), null, 500, 500);
		}

		public void method_0(uint uint_0, string string_0, GameClient class16_1)
		{
			this.hashtable_0[uint_0] = class16_1;
			this.hashtable_1[string_0.ToLower()] = class16_1;
		}

		public void method_1(uint uint_0, string string_0)
		{
			this.hashtable_0[uint_0] = null;
			this.hashtable_1[string_0.ToLower()] = null;
		}

		public GameClient method_2(uint id)
		{
			GameClient result;

			if (this.Clients == null || this.hashtable_0 == null)
			{
				result = null;
			}

			else
			{
				if (this.hashtable_0.ContainsKey(id))
				{
					result = (GameClient)this.hashtable_0[id];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		public GameClient GetClientByHabbo(string string_0)
		{
			GameClient result;
			if (this.Clients == null || this.hashtable_1 == null || string.IsNullOrEmpty(string_0))
			{
				result = null;
			}
			else
			{
				if (this.hashtable_1.ContainsKey(string_0.ToLower()))
				{
					result = (GameClient)this.hashtable_1[string_0.ToLower()];
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		private void DisposeTimerCallback(object sender)
		{
			try
			{
                List<SocketConnection> list = this.DisposeQueue;
                this.DisposeQueue = new List<SocketConnection>();

				if (list != null)
				{
                    foreach (SocketConnection current in list)
                    //foreach (ConnectionInformation current in list)
					{
						if (current != null)
						{
                            current.method_1();
                            //current.disconnect();
						}
					}
				}
			}
			catch (Exception ex)
			{
                Logging.LogThreadException(ex.ToString(), "Disconnector task");
			}
		}

        internal void DisposeConnection(SocketConnection connection)
        {
            if (!this.DisposeQueue.Contains(connection))
            {
                this.DisposeQueue.Add(connection);
            }
        }

        //internal void method_5(SocketConnection Message1_0)
        //{
        //    if (!this.list_0.Contains(Message1_0))
        //    {
        //        this.list_0.Add(Message1_0);
        //    }
        //}

        //internal void method_5(ConnectionInformation Message1_0)
        //{
        //    if (!this.list_0.Contains(Message1_0))
        //    {
        //        this.list_0.Add(Message1_0);
        //    }
        //}

		public void method_6()
		{
		}

		public GameClient GetClientById(uint id)
		{
			GameClient result;

			try
			{
				result = this.Clients[(int)((UIntPtr)id)];
			}
			catch
			{
				result = null;
			}

			return result;
		}

        internal void method_8(uint uint_0, ref SocketConnection Message1_0)
        {
            this.Clients[(int)((UIntPtr)uint_0)] = new GameClient(uint_0, ref Message1_0);
            this.Clients[(int)((UIntPtr)uint_0)].GetSocketConnection();
        }

		public void method_9(uint uint_0)
		{
			GameClient @class = this.GetClientById(uint_0);
			if (@class != null)
			{
                //GoldTree.smethod_14().method_6(uint_0);
				@class.method_11();
				this.Clients[(int)((UIntPtr)uint_0)] = null;
			}
		}

		public void method_10()
		{
			if (this.task_0 == null)
			{
				this.task_0 = new Task(new Action(this.method_12));
				this.task_0.Start();
			}
		}

		public void method_11()
		{
			if (this.task_0 != null)
			{
				this.task_0 = null;
			}
		}

		private void method_12()
		{
			int num = int.Parse(GoldTree.GetConfig().data["client.ping.interval"]);

			if (num <= 100)
			{
				throw new ArgumentException("Invalid configuration value for ping interval! Must be above 100 miliseconds.");
			}

			while (true)
			{
				try
				{
					ServerMessage Message = new ServerMessage(50u);
					List<GameClient> list = new List<GameClient>();
					List<GameClient> list2 = new List<GameClient>();
					for (int i = 0; i < this.Clients.Length; i++)
					{
						GameClient @class = this.Clients[i];
						if (@class != null)
						{
							if (@class.bool_0)
							{
								@class.bool_0 = false;
								list2.Add(@class);
							}
							else
							{
								list.Add(@class);
							}
						}
					}
                    foreach (GameClient @class in list)
                    {
                        try
                        {
                            @class.method_12();
                        }
                        catch
                        {
                        }
                    }
					byte[] byte_ = Message.GetBytes();
					foreach (GameClient @class in list2)
					{
						try
						{
                            @class.GetConnection().SendData(byte_);
						}
						catch
						{
                            @class.method_12();
						}
					}
				}
				catch (Exception ex)
				{
                    Logging.LogThreadException(ex.ToString(), "Connection checker task");
				}
				Thread.Sleep(num);
			}
		}
		internal void method_13()
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
						@class.SendMessage(AchievementManager.smethod_1(@class));
					}
					catch
					{
					}
				}
			}
		}
		internal void BroadcastMessage(ServerMessage message)
		{
			byte[] bytes = message.GetBytes();

			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient client = this.Clients[i];
				if (client != null)
				{
                    try
                    {
                        client.GetConnection().SendData(bytes);
                    }
                    catch { }
				}
			}
		}
		internal void method_15(ServerMessage Message5_0, ServerMessage Message5_1)
		{
			byte[] byte_ = Message5_0.GetBytes();
			byte[] byte_2 = Message5_1.GetBytes();
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
                        if (@class.GetHabbo().InRoom)
						{
							@class.GetConnection().SendData(byte_);
						}
						else
						{
							@class.GetConnection().SendData(byte_2);
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void method_16(ServerMessage Message5_0, ServerMessage Message5_1)
		{
			byte[] byte_ = Message5_0.GetBytes();
			byte[] byte_2 = Message5_1.GetBytes();
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
						if (@class.GetHabbo().HasFuse("receive_sa"))
						{
                            if (@class.GetHabbo().InRoom)
							{
								@class.GetConnection().SendData(byte_);
							}
							else
							{
								@class.GetConnection().SendData(byte_2);
							}
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void method_17(GameClient class16_1, ServerMessage Message5_0)
		{
			byte[] byte_ = Message5_0.GetBytes();
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class != class16_1)
				{
					try
					{
						if (@class.GetHabbo().HasFuse("receive_sa"))
						{
							@class.GetConnection().SendData(byte_);
						}
					}
					catch
					{
					}
				}
			}
		}
		internal void method_18(int int_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
                    try
                    {
                        long NoBug = 0;
                        NoBug += @class.GetHabbo().Credits;
                        NoBug += int_0;
                        if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                        {
                            @class.GetHabbo().Credits += int_0;
                            @class.GetHabbo().UpdateCredits(true);
                            @class.SendNotification("You just received " + int_0 + " credits from staff!");
                        }
                        else
                        {
                            if (int_0 > 0)
                            {
                                @class.GetHabbo().Credits = 2147483647;
                                @class.GetHabbo().UpdateCredits(true);
                                @class.SendNotification("You just received max credits from staff!");
                            }
                            else if (int_0 < 0)
                            {
                                @class.GetHabbo().Credits = -2147483648;
                                @class.GetHabbo().UpdateCredits(true);
                                @class.SendNotification("You just received max negative credits from staff!");
                            }
                        }
                    }
                    catch
                    {
                    }
				}
			}
		}
		internal void method_19(int int_0, bool bool_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
                    try
                    {
                        long NoBug = 0;
                        NoBug += @class.GetHabbo().ActivityPoints;
                        NoBug += int_0;
                        if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                        {
                            @class.GetHabbo().ActivityPoints += int_0;
                            @class.GetHabbo().UpdateActivityPoints(bool_0);
                            @class.SendNotification("You just received " + int_0 + " pixels from staff!");
                        }
                        else
                        {
                            if (int_0 > 0)
                            {
                                @class.GetHabbo().Credits = 2147483647;
                                @class.GetHabbo().UpdateCredits(true);
                                @class.SendNotification("You just received max pixels from staff!");
                            }
                            else if (int_0 < 0)
                            {
                                @class.GetHabbo().Credits = -2147483648;
                                @class.GetHabbo().UpdateCredits(true);
                                @class.SendNotification("You just received max negative pixels from staff!");
                            }
                        }
                    }
                    catch
                    {
                    }
				}
			}
		}
		internal void method_20(int int_0, bool bool_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
                    try
                    {
                        long NoBug = 0;
                        NoBug += @class.GetHabbo().VipPoints;
                        NoBug += int_0;
                        if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                        {
                            @class.GetHabbo().VipPoints += int_0;
                            @class.GetHabbo().UpdateVipPoints(false, bool_0);
                            @class.SendNotification("You just received " + int_0 + " points from staff!");
                        }
                        else
                        {
                            if (int_0 > 0)
                            {
                                @class.GetHabbo().Credits = 2147483647;
                                @class.GetHabbo().UpdateCredits(true);
                                @class.SendNotification("You just received max points from staff!");
                            }
                            else if (int_0 < 0)
                            {
                                @class.GetHabbo().Credits = -2147483648;
                                @class.GetHabbo().UpdateCredits(true);
                                @class.SendNotification("You just received max negative points from staff!");
                            }
                        }
                    }
                    catch
                    {
                    }
				}
			}
		}
		internal void method_21(string string_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
					try
					{
						@class.GetHabbo().GetBadgeComponent().SendBadge(@class, string_0, true);
						@class.SendNotification("You just received a badge from hotel staff!");
					}
					catch
					{
					}
				}
			}
		}
		public void method_22(ServerMessage Message5_0, string string_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
						if (string_0.Length <= 0 || (@class.GetHabbo() != null && @class.GetHabbo().HasFuse(string_0)))
						{
							@class.SendMessage(Message5_0);
						}
					}
					catch
					{
					}
				}
			}
		}
		public void method_23()
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && (@class.GetHabbo() != null && @class.GetHabbo().GetEffectsInventoryComponent() != null))
				{
					@class.GetHabbo().GetEffectsInventoryComponent().method_7();
				}
			}
		}
		internal void CloseAll()
		{
			StringBuilder stringBuilder = new StringBuilder();

			bool flag = false;

			using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
			{
				for (int i = 0; i < Clients.Length; i++)
				{
					GameClient client = Clients[i];

					if (client != null && client.GetHabbo() != null)
					{
                        try
                        {
                            client.GetHabbo().GetInventoryComponent().SavePets(dbClient, true);
                            stringBuilder.Append(client.GetHabbo().UpdateQuery);
                            flag = true;
                        }
                        catch { }
					}
				}

				if (flag)
				{
					try
					{
						dbClient.ExecuteQuery(stringBuilder.ToString());
					}
					catch (Exception ex)
					{
						Logging.HandleException(ex.ToString());
					}
				}
			}

			Console.WriteLine("Done saving users inventory!");
			Console.WriteLine("Closing server connections...");

			try
			{
				for (int i = 0; i < this.Clients.Length; i++)
				{
					GameClient class2 = this.Clients[i];
					if (class2 != null && class2.GetConnection() != null)
					{
						try
						{
							class2.GetConnection().Close();
						}
						catch
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logging.HandleException(ex.ToString());
			}

			Array.Clear(this.Clients, 0, this.Clients.Length);

			Console.WriteLine("Connections closed!");
		}

		public void method_25(uint uint_0)
		{
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null && @class.GetHabbo().Id == uint_0)
				{
					@class.method_12();
				}
			}
		}
		public string GetNameById(uint uint_0)
		{
			GameClient @class = this.method_2(uint_0);
			string result;
			if (@class != null)
			{
				result = @class.GetHabbo().Username;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
				{
					dataRow = class2.ReadDataRow("SELECT username FROM users WHERE Id = '" + uint_0 + "' LIMIT 1");
				}
				if (dataRow == null)
				{
					result = "Unknown User";
				}
				else
				{
					result = (string)dataRow[0];
				}
			}
			return result;
		}
        public string GetDataById(uint uint_0, string data)
        {
            string result = "";

            if (data != "gender" || data != "look")
            {
                return result;
            }

            GameClient @class = this.method_2(uint_0);
            if (@class != null)
            {
                if (data == "gender")
                {
                    result = @class.GetHabbo().Gender;
                }
                else if (data == "look")
                {
                    result = @class.GetHabbo().Figure;
                }
            }
            else
            {
                DataRow dataRow = null;
                using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
                {
                    dataRow = class2.ReadDataRow("SELECT " + data + " FROM users WHERE Id = '" + uint_0 + "' LIMIT 1");
                }
                if (dataRow == null)
                {
                    result = "Unknown data";
                }
                else
                {
                    result = (string)dataRow[0];
                }
            }
            return result;
        }

		public uint method_27(string string_0)
		{
			GameClient @class = this.GetClientByHabbo(string_0);
			uint result;
			if (@class != null && @class.GetHabbo() != null)
			{
				result = @class.GetHabbo().Id;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
				{
					dataRow = class2.ReadDataRow("SELECT Id FROM users WHERE username = '" + string_0 + "' LIMIT 1");
				}
				if (dataRow == null)
				{
					result = 0u;
				}
				else
				{
					result = (uint)dataRow[0];
				}
			}
			return result;
		}
		public void method_28()
		{
			Dictionary<GameClient, ModerationBanException> dictionary = new Dictionary<GameClient, ModerationBanException>();
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null)
				{
					try
					{
						GoldTree.GetGame().GetBanManager().method_1(@class);
					}
					catch (ModerationBanException value)
					{
						dictionary.Add(@class, value);
					}
				}
			}
			foreach (KeyValuePair<GameClient, ModerationBanException> current in dictionary)
			{
				current.Key.NotifyBan(current.Value.Message);
				current.Key.method_12();
			}
		}

		public void method_29()
		{
			try
			{
				if (this.Clients != null)
				{
					for (int i = 0; i < this.Clients.Length; i++)
					{
						GameClient @class = this.Clients[i];
						if (@class != null && (@class.GetHabbo() != null && GoldTree.GetGame().GetPixelManager().method_2(@class)))
						{
							GoldTree.GetGame().GetPixelManager().method_3(@class);
						}
					}
				}
			}
			catch (Exception ex)
			{
                Logging.LogThreadException(ex.ToString(), "GCMExt.CheckPixelUpdates task");
			}
		}

		internal List<ServerMessage> method_30()
		{
			List<ServerMessage> list = new List<ServerMessage>();
			int num = 0;
			ServerMessage Message = new ServerMessage();
			Message.Init(161u);
			Message.AppendStringWithBreak("Users online:\r");
			for (int i = 0; i < this.Clients.Length; i++)
			{
				GameClient @class = this.Clients[i];
				if (@class != null && @class.GetHabbo() != null)
				{
					if (num > 20)
					{
						list.Add(Message);
						num = 0;
						Message = new ServerMessage();
						Message.Init(161u);
					}
					num++;
					Message.AppendStringWithBreak(string.Concat(new object[]
					{
						@class.GetHabbo().Username,
						" {",
						@class.GetHabbo().Rank,
						"}\r"
					}));
				}
			}
			list.Add(Message);
			return list;
		}

		internal void method_31(GameClient class16_1, string string_0, string string_1)
		{
            if (ServerConfiguration.EnableCommandLog)
			{
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("extra_data", string_1);
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO cmdlogs (user_id,user_name,command,extra_data,timestamp) VALUES ('",
						class16_1.GetHabbo().Id,
						"','",
						class16_1.GetHabbo().Username,
						"','",
						string_0,
						"', @extra_data, UNIX_TIMESTAMP())"
					}));
				}
			}
        }

        //#region NEWSOCKETS
        //internal void CreateAndStartClient(uint clientID, ConnectionInformation connection)
        //{
        //    this.Session[(int)((UIntPtr)clientID)] = new GameClient(clientID, connection);
        //    this.Session[(int)((UIntPtr)clientID)].method_3();
        //}
        //#endregion
    }
}
