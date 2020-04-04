using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.Users.UserDataManagement;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Util;
using GoldTree.Messages;
using GoldTree.HabboHotel.Users.Messenger;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Users.Messenger
{
	internal sealed class HabboMessenger
	{
		private uint uint_0;
		private Hashtable hashtable_0;
		private Hashtable hashtable_1;
		internal bool bool_0;
		public HabboMessenger(uint uint_1)
		{
			this.hashtable_0 = new Hashtable();
			this.hashtable_1 = new Hashtable();
			this.uint_0 = uint_1;
		}
		internal void method_0(UserDataFactory class12_0)
		{
			this.hashtable_0 = new Hashtable();

			DataTable dataTable_ = class12_0.GetFriends();

			if (dataTable_ != null)
			{
				foreach (DataRow dataRow in dataTable_.Rows)
				{
                    if (!this.hashtable_0.Contains((uint)dataRow["Id"]))
                    {
                        this.hashtable_0.Add((uint)dataRow["Id"], new MessengerBuddy((uint)dataRow["Id"], dataRow["username"] as string, dataRow["look"] as string, dataRow["motto"] as string, dataRow["last_online"] as string));
                    }
				}
				try
				{
					if (this.method_25().GetHabbo().HasFuse("receive_sa"))
					{
						this.hashtable_0.Add(0, new MessengerBuddy(0u, "Staff Chat", this.method_25().GetHabbo().Figure, "Staff Chat Room", "0"));
					}
				}
				catch
				{
				}
			}
		}
		internal void method_1(UserDataFactory class12_0)
		{
			this.hashtable_1 = new Hashtable();
			DataTable dataTable_ = class12_0.GetFriendRequests();
			if (dataTable_ != null)
			{
				foreach (DataRow dataRow in dataTable_.Rows)
				{
                    if (!this.hashtable_1.ContainsKey((uint)dataRow["from_id"]))
                    {
                        this.hashtable_1.Add((uint)dataRow["from_id"], new MessengerRequest((uint)dataRow["Id"], this.uint_0, (uint)dataRow["from_id"], dataRow["username"] as string, dataRow["gender"] as string, dataRow["look"] as string));
                    }
				}
			}
		}

		internal void method_2()
		{
			this.hashtable_0.Clear();
		}
		public void method_3()
		{
			this.hashtable_1.Clear();
		}
		internal MessengerRequest method_4(uint uint_1)
		{
			return this.hashtable_1[uint_1] as MessengerRequest;
		}
		internal void method_5(bool bool_1)
		{
			Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				try
				{
					GameClient class2 = GoldTree.GetGame().GetClientManager().method_2(@class.UInt32_0);
					if (class2 != null && class2.GetHabbo() != null && class2.GetHabbo().GetMessenger() != null)
					{
						class2.GetHabbo().GetMessenger().method_6(this.uint_0);
						if (bool_1)
						{
							class2.GetHabbo().GetMessenger().method_7();
						}
					}
				}
				catch
				{
				}
			}
			hashtable.Clear();
			hashtable = null;
		}
		internal bool method_6(uint uint_1)
		{
			Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
			bool result;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				if (@class.UInt32_0 == uint_1)
				{
					@class.bool_0 = true;
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		internal void method_7()
		{
			this.method_25().SendMessage(this.SerializeUpdates());
		}
		internal bool method_8(uint uint_1, uint uint_2)
		{
			bool result;
			if (uint_1 == uint_2)
			{
				result = true;
			}
			else
			{
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					if (@class.ReadDataRow(string.Concat(new object[]
					{
						"SELECT Id FROM messenger_requests WHERE to_id = '",
						uint_1,
						"' AND from_id = '",
						uint_2,
						"' LIMIT 1"
					})) != null)
					{
						result = true;
						return result;
					}
					if (@class.ReadDataRow(string.Concat(new object[]
					{
						"SELECT Id FROM messenger_requests WHERE to_id = '",
						uint_2,
						"' AND from_id = '",
						uint_1,
						"' LIMIT 1"
					})) != null)
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}
		internal bool method_9(uint uint_1, uint uint_2)
		{
			bool result;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				if (@class.ReadDataRow(string.Concat(new object[]
				{
					"SELECT user_one_id FROM messenger_friendships WHERE user_one_id = '",
					uint_1,
					"' AND user_two_id = '",
					uint_2,
					"' LIMIT 1"
				})) != null)
				{
					result = true;
					return result;
				}
				if (@class.ReadDataRow(string.Concat(new object[]
				{
					"SELECT user_one_id FROM messenger_friendships WHERE user_one_id = '",
					uint_2,
					"' AND user_two_id = '",
					uint_1,
					"' LIMIT 1"
				})) != null)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		internal void method_10()
		{
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.ExecuteQuery("DELETE FROM messenger_requests WHERE to_id = '" + this.uint_0 + "'");
			}
			this.method_3();
		}
		internal void method_11(uint uint_1)
		{
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("userid", this.uint_0);
				@class.AddParamWithValue("fromid", uint_1);
				@class.ExecuteQuery("DELETE FROM messenger_requests WHERE to_id = @userid AND from_id = @fromid LIMIT 1");
			}
			if (this.method_4(uint_1) != null)
			{
				this.hashtable_1.Remove(uint_1);
			}
		}
		internal void method_12(uint uint_1)
		{
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("toid", uint_1);
				@class.AddParamWithValue("userid", this.uint_0);
				@class.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@userid,@toid)");
				@class.ExecuteQuery("INSERT INTO messenger_friendships (user_one_id,user_two_id) VALUES (@toid,@userid)");
			}
			this.method_14(uint_1);
			GameClient class2 = GoldTree.GetGame().GetClientManager().method_2(uint_1);
			if (class2 != null && class2.GetHabbo().GetMessenger() != null)
			{
				class2.GetHabbo().GetMessenger().method_14(this.uint_0);
			}
		}
		internal void method_13(uint uint_1)
		{
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("toid", uint_1);
				@class.AddParamWithValue("userid", this.uint_0);
				@class.ExecuteQuery("DELETE FROM messenger_friendships WHERE user_one_id = @toid AND user_two_id = @userid LIMIT 1");
				@class.ExecuteQuery("DELETE FROM messenger_friendships WHERE user_one_id = @userid AND user_two_id = @toid LIMIT 1");
			}
			this.method_15(uint_1);
			GameClient class2 = GoldTree.GetGame().GetClientManager().method_2(uint_1);
			if (class2 != null && class2.GetHabbo().GetMessenger() != null)
			{
				class2.GetHabbo().GetMessenger().method_15(this.uint_0);
			}
		}
		internal void method_14(uint uint_1)
		{
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				DataRow dataRow = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + uint_1 + "' LIMIT 1");
				MessengerBuddy class2 = new MessengerBuddy(uint_1, dataRow["username"] as string, dataRow["look"] as string, dataRow["motto"] as string, dataRow["last_online"] as string);
				class2.bool_0 = true;
				if (!this.hashtable_0.Contains(class2.UInt32_0))
				{
					this.hashtable_0.Add(class2.UInt32_0, class2);
				}
				this.method_7();
			}
		}
		internal void method_15(uint uint_1)
		{
			this.hashtable_0.Remove(uint_1);
			ServerMessage Message = new ServerMessage(13u);
			Message.AppendInt32(0);
			Message.AppendInt32(1);
			Message.AppendInt32(-1);
			Message.AppendUInt(uint_1);
			this.method_25().SendMessage(Message);
		}
		internal void method_16(string string_0)
		{
			DataRow dataRow = null;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("query", string_0.ToLower());
				dataRow = @class.ReadDataRow("SELECT Id,block_newfriends FROM users WHERE username = @query LIMIT 1");
			}
			if (dataRow != null)
			{
				if (GoldTree.StringToBoolean(dataRow["block_newfriends"].ToString()) && !this.method_25().GetHabbo().HasFuse("ignore_friendsettings"))
				{
					ServerMessage Message = new ServerMessage(260u);
					Message.AppendInt32(39);
					Message.AppendInt32(3);
					this.method_25().SendMessage(Message);
				}
				else
				{
					uint num = (uint)dataRow["Id"];
					if (!this.method_8(this.uint_0, num))
					{
						using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
						{
							@class.AddParamWithValue("toid", num);
							@class.AddParamWithValue("userid", this.uint_0);
							@class.ExecuteQuery("INSERT INTO messenger_requests (to_id,from_id) VALUES (@toid,@userid)");
						}
						GameClient class2 = GoldTree.GetGame().GetClientManager().method_2(num);
						if (class2 != null && class2.GetHabbo() != null)
						{
							uint num2 = 0u;
							using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
							{
								@class.AddParamWithValue("toid", num);
								@class.AddParamWithValue("userid", this.uint_0);
								num2 = @class.ReadUInt32("SELECT Id FROM messenger_requests WHERE to_id = @toid AND from_id = @userid ORDER BY Id DESC LIMIT 1");
							}

                            string gender = GoldTree.GetGame().GetClientManager().GetDataById(this.uint_0, "gender");
                            string look = GoldTree.GetGame().GetClientManager().GetDataById(this.uint_0, "look");
                            string username = GoldTree.GetGame().GetClientManager().GetNameById(this.uint_0);

                            if (!(string.IsNullOrEmpty(gender) && string.IsNullOrEmpty(look) && string.IsNullOrEmpty(username)))
                            {
                                MessengerRequest class3 = new MessengerRequest(num2, num, this.uint_0, username, gender, look);
                                class2.GetHabbo().GetMessenger().method_17(num2, num, this.uint_0, username, look, gender);
                                ServerMessage Message5_ = new ServerMessage(132u);
                                class3.method_0(Message5_);
                                class2.SendMessage(Message5_);
                            }
						}
					}
				}
			}
		}
		internal void method_17(uint uint_1, uint uint_2, uint uint_3, string username, string gender, string look)
		{
			if (!this.hashtable_1.ContainsKey(uint_3))
			{
				this.hashtable_1.Add(uint_3, new MessengerRequest(uint_1, uint_2, uint_3, username, gender, look));
			}
		}
		internal void method_18(uint uint_1, string string_0)
		{
			if (!this.method_9(uint_1, this.uint_0))
			{
				this.method_20(6, uint_1);
			}
			else
			{
				GameClient @class = GoldTree.GetGame().GetClientManager().method_2(uint_1);
				if (@class == null || @class.GetHabbo().GetMessenger() == null)
				{
					this.method_20(5, uint_1);
				}
				else
				{
					if (this.method_25().GetHabbo().IsMuted)
					{
						this.method_20(4, uint_1);
					}
					else
					{
						if (@class.GetHabbo().IsMuted)
						{
							this.method_20(3, uint_1);
						}
						if (this.method_25().GetHabbo().method_4() > 0)
						{
							TimeSpan timeSpan = DateTime.Now - this.method_25().GetHabbo().dateTime_0;
							if (timeSpan.Seconds > 4)
							{
								this.method_25().GetHabbo().int_23 = 0;
							}
							if (timeSpan.Seconds < 4 && this.method_25().GetHabbo().int_23 > 5)
							{
								this.method_20(4, uint_1);
								return;
							}
							this.method_25().GetHabbo().dateTime_0 = DateTime.Now;
							this.method_25().GetHabbo().int_23++;
						}
						string_0 = ChatCommandHandler.smethod_4(string_0);
						if (ServerConfiguration.EnableChatlog && !this.method_25().GetHabbo().IsJuniori)
						{
							using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
							{
								class2.AddParamWithValue("message", "<PM to " + @class.GetHabbo().Username + ">: " + string_0);
								class2.ExecuteQuery(string.Concat(new object[]
								{
									"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
									this.method_25().GetHabbo().Id,
									"','0','",
									DateTime.Now.Hour,
									"','",
									DateTime.Now.Minute,
									"',UNIX_TIMESTAMP(),@message,'",
									this.method_25().GetHabbo().Username,
									"','",
									DateTime.Now.ToLongDateString(),
									"')"
								}));
							}
						}
						@class.GetHabbo().GetMessenger().method_19(string_0, this.uint_0);
					}
				}
			}
		}
		internal void method_19(string string_0, uint uint_1)
		{
			ServerMessage Message = new ServerMessage(134u);
			Message.AppendUInt(uint_1);
			Message.AppendString(string_0);
			this.method_25().SendMessage(Message);
		}
		internal void method_20(int int_0, uint uint_1)
		{
            if (this != null)
            {
                ServerMessage Message = new ServerMessage(261u);
                Message.AppendInt32(int_0);
                Message.AppendUInt(uint_1);
                this.method_25().SendMessage(Message);
            }
		}
		internal ServerMessage method_21()
		{
			ServerMessage Message = new ServerMessage(12u);
			Message.AppendInt32(6000);
			Message.AppendInt32(200);
			Message.AppendInt32(6000);
			Message.AppendInt32(900);
			Message.AppendBoolean(false);
			Message.AppendInt32(this.hashtable_0.Count);
			Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				@class.method_0(Message, false);
			}
			return Message;
		}
		internal ServerMessage SerializeUpdates()
		{
			List<MessengerBuddy> list = new List<MessengerBuddy>();
			int num = 0;
			Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
			foreach (MessengerBuddy @class in hashtable.Values)
			{
				if (@class.bool_0)
				{
					num++;
					list.Add(@class);
					@class.bool_0 = false;
				}
			}
			hashtable.Clear();
			ServerMessage Message = new ServerMessage(13u);
			Message.AppendInt32(0);
			Message.AppendInt32(num);
			Message.AppendInt32(0);
			foreach (MessengerBuddy @class in list)
			{
				@class.method_0(Message, false);
				Message.AppendBoolean(false);
			}
			return Message;
		}
		internal ServerMessage method_23()
		{
			ServerMessage Message = new ServerMessage(314u);
			Message.AppendInt32(this.hashtable_1.Count);
			Message.AppendInt32(this.hashtable_1.Count);
			Hashtable hashtable = this.hashtable_1.Clone() as Hashtable;
			foreach (MessengerRequest @class in hashtable.Values)
			{
				@class.method_0(Message);
			}
			return Message;
		}
		internal ServerMessage method_24(string string_0)
		{
			DataTable dataTable = null;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("query", string_0 + "%");
				dataTable = @class.ReadDataTable("SELECT Id FROM users WHERE username LIKE @query LIMIT 50");
			}
			List<DataRow> list = new List<DataRow>();
			List<DataRow> list2 = new List<DataRow>();
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					if (this.method_9(this.uint_0, (uint)dataRow["Id"]))
					{
						list.Add(dataRow);
					}
					else
					{
						list2.Add(dataRow);
					}
				}
			}
			ServerMessage Message = new ServerMessage(435u);
			Message.AppendInt32(list.Count);
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				foreach (DataRow dataRow in list)
				{
					uint num = (uint)dataRow["Id"];
					DataRow dataRow2 = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + num + "' LIMIT 1");
					new MessengerBuddy(num, dataRow2["username"] as string, dataRow2["look"] as string, dataRow2["motto"] as string, dataRow2["last_online"] as string).method_0(Message, true);
				}
				Message.AppendInt32(list2.Count);
				foreach (DataRow dataRow in list2)
				{
					uint num = (uint)dataRow["Id"];
					DataRow dataRow2 = @class.ReadDataRow("SELECT username,motto,look,last_online FROM users WHERE Id = '" + num + "' LIMIT 1");
					new MessengerBuddy(num, dataRow2["username"] as string, dataRow2["look"] as string, dataRow2["motto"] as string, dataRow2["last_online"] as string).method_0(Message, true);
				}
			}
			return Message;
		}
		private GameClient method_25()
		{
			return GoldTree.GetGame().GetClientManager().method_2(this.uint_0);
		}
		internal Hashtable method_26()
		{
			return this.hashtable_0.Clone() as Hashtable;
		}

        internal bool UserInFriends(uint id)
        {
            return this.hashtable_0.ContainsKey(id);
        }
	}
}
