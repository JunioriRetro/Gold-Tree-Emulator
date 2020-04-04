using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Util;
using GoldTree.Messages;
using GoldTree.HabboHotel.Users.Messenger;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Navigators
{
	internal sealed class Navigator
	{
		[CompilerGenerated]
		private sealed class Class219
		{
			public int int_0;
			public bool method_0(RoomData class27_0)
			{
				return class27_0.Category == this.int_0;
			}
			public bool method_1(Room class14_0)
			{
				return class14_0.Category == this.int_0;
			}
		}
		private List<FlatCat> list_0;
		private Dictionary<int, PublicItem> dictionary_0;
		private Dictionary<int, PublicItem> dictionary_1;
		[CompilerGenerated]
		private static Comparison<KeyValuePair<string, int>> comparison_0;
		[CompilerGenerated]
		private static Func<RoomData, int> func_0;
		[CompilerGenerated]
		private static Func<Room, int> func_1;
		[CompilerGenerated]
		private static Func<Room, int> func_2;
		public Navigator()
		{
			this.list_0 = new List<FlatCat>();
			this.dictionary_0 = new Dictionary<int, PublicItem>();
			this.dictionary_1 = new Dictionary<int, PublicItem>();
		}
		public void method_0(DatabaseClient class6_0)
		{
			Logging.Write("Loading Navigator..");
			this.list_0.Clear();
			this.dictionary_0.Clear();
			this.dictionary_1.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT Id,caption,min_rank,cantrade FROM navigator_flatcats WHERE enabled = '1'");
			DataTable dataTable2 = class6_0.ReadDataTable("SELECT Id,bannertype,caption,image,image_type,room_id,category,category_parent_id FROM navigator_publics ORDER BY ordernum ASC;");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.list_0.Add(new FlatCat((int)dataRow["Id"], (string)dataRow["caption"], (int)dataRow["min_rank"], GoldTree.StringToBoolean(dataRow["cantrade"].ToString())));
				}
			}
			if (dataTable2 != null)
			{
				foreach (DataRow dataRow in dataTable2.Rows)
				{
					this.dictionary_0.Add((int)dataRow["Id"], new PublicItem((int)dataRow["Id"], int.Parse(dataRow["bannertype"].ToString()), (string)dataRow["caption"], (string)dataRow["image"], (dataRow["image_type"].ToString().ToLower() == "internal") ? PublicImageType.INTERNAL : PublicImageType.EXTERNAL, (uint)dataRow["room_id"], GoldTree.StringToBoolean(dataRow["category"].ToString()), (int)dataRow["category_parent_id"]));
					if (!this.dictionary_0[(int)dataRow["Id"]].bool_0)
					{
						this.dictionary_1.Add((int)dataRow["Id"], this.dictionary_0[(int)dataRow["Id"]]);
					}
				}
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		public int method_1(int int_0)
		{
			int num = 0;
			foreach (PublicItem current in this.dictionary_0.Values)
			{
				if (current.int_2 == int_0 || int_0 == -1)
				{
					num++;
				}
			}
			return num;
		}
		public FlatCat method_2(int int_0)
		{
			FlatCat result;
			using (TimedLock.Lock(this.list_0))
			{
				foreach (FlatCat current in this.list_0)
				{
					if (current.Id == int_0)
					{
						result = current;
						return result;
					}
				}
			}
			result = null;
			return result;
		}
		public ServerMessage method_3()
		{
			ServerMessage Message = new ServerMessage(221u);
			Message.AppendInt32(this.list_0.Count);
			foreach (FlatCat current in this.list_0)
			{
				if (current.Id > 0)
				{
					Message.AppendBoolean(true);
				}
				if (current.Id != 15)
				{
					Message.AppendInt32(current.Id);
				}
				Message.AppendStringWithBreak(current.Caption);
			}
			Message.AppendStringWithBreak("");
			return Message;
		}
		public void method_4(int int_0, ServerMessage Message5_0)
		{
			foreach (PublicItem current in this.dictionary_0.Values)
			{
				if (current.int_2 == int_0 && !current.bool_0)
				{
					current.method_0(Message5_0);
				}
			}
		}
		public ServerMessage method_5()
		{
			ServerMessage Message = new ServerMessage(450u);
			Message.AppendInt32(this.dictionary_0.Count);
			foreach (PublicItem current in this.dictionary_0.Values)
			{
				if (current.bool_0)
				{
					current.method_0(Message);
					this.method_4(current.Int32_0, Message);
				}
				if (!current.bool_0 && (current.int_2 == 0 || current.int_2 == -1))
				{
					current.method_0(Message);
				}
			}
			return Message;
		}
		public ServerMessage method_6(GameClient Session)
		{
			ServerMessage Message = new ServerMessage(451u);
			Message.AppendInt32(0);
			Message.AppendInt32(6);
			Message.AppendStringWithBreak("");
			Message.AppendInt32(Session.GetHabbo().list_1.Count);
			using (TimedLock.Lock(Session.GetHabbo().list_1))
			{
				foreach (uint current in Session.GetHabbo().list_1)
				{
					GoldTree.GetGame().GetRoomManager().method_11(current).method_3(Message, false, false);
				}
			}
			return Message;
		}
		public ServerMessage method_7(GameClient Session)
		{
			ServerMessage result;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				DataTable dataTable = @class.ReadDataTable("SELECT * FROM rooms JOIN user_roomvisits ON rooms.Id = user_roomvisits.room_id WHERE user_roomvisits.user_id = '" + Session.GetHabbo().Id + "' ORDER BY entry_timestamp DESC LIMIT 50;");
				List<RoomData> list = new List<RoomData>();
				List<uint> list2 = new List<uint>();
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						RoomData class2 = GoldTree.GetGame().GetRoomManager().method_17((uint)dataRow["Id"], dataRow);
						class2.method_1(dataRow);
						list.Add(class2);
						list2.Add(class2.Id);
					}
				}
				ServerMessage Message = new ServerMessage(451u);
				Message.AppendInt32(0);
				Message.AppendInt32(7);
				Message.AppendStringWithBreak("");
				Message.AppendInt32(list.Count);
				foreach (RoomData current in list)
				{
					current.method_3(Message, false, false);
				}
				result = Message;
			}
			return result;
		}
		public ServerMessage method_8(GameClient Session, int int_0)
		{
			ServerMessage Message = new ServerMessage(451u);
			Message.AppendInt32(int_0);
			Message.AppendInt32(12);
			Message.AppendStringWithBreak("");
			List<Room> list = GoldTree.GetGame().GetRoomManager().method_6(int_0);
			Message.AppendInt32(list.Count);
			foreach (Room current in list)
			{
				RoomData class27_ = current.RoomData;
				class27_.method_3(Message, true, false);
			}
			return Message;
		}
		public ServerMessage method_9()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			ServerMessage result;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				DataTable dataTable = @class.ReadDataTable("SELECT tags,users_now FROM rooms WHERE roomtype = 'private' AND users_now > 0 ORDER BY users_now DESC LIMIT 50;");
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						List<string> list = new List<string>();
						string[] array = dataRow["tags"].ToString().Split(new char[]
						{
							','
						});
						for (int i = 0; i < array.Length; i++)
						{
							string text = array[i];
							list.Add(text);
						}
						foreach (string text in list)
						{
							if (dictionary.ContainsKey(text))
							{
								Dictionary<string, int> dictionary2;
								string key;
								(dictionary2 = dictionary)[key = text] = dictionary2[key] + (int)dataRow["users_now"];
							}
							else
							{
								dictionary.Add(text, (int)dataRow["users_now"]);
							}
						}
					}
				}
				List<KeyValuePair<string, int>> list2 = new List<KeyValuePair<string, int>>(dictionary);
				List<KeyValuePair<string, int>> arg_163_0 = list2;
				if (Navigator.comparison_0 == null)
				{
					Navigator.comparison_0 = new Comparison<KeyValuePair<string, int>>(Navigator.smethod_0);
				}
				arg_163_0.Sort(Navigator.comparison_0);
				ServerMessage Message = new ServerMessage(452u);
				Message.AppendInt32(list2.Count);
				foreach (KeyValuePair<string, int> current in list2)
				{
					Message.AppendStringWithBreak(current.Key);
					Message.AppendInt32(current.Value);
				}
				result = Message;
			}
			return result;
		}
		public ServerMessage method_10(string string_0)
		{
			DataTable dataTable = null;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				string_0 = GoldTree.FilterString(string_0.ToLower()).Trim();
				if (string_0.Length > 0)
				{
					if (string_0.StartsWith("owner:"))
					{
						string_0 = string_0.Replace(" ", "");
						@class.AddParamWithValue("query", string_0.Substring(6));
						dataTable = @class.ReadDataTable("SELECT * FROM rooms WHERE owner = @query AND roomtype = 'private' ORDER BY users_now DESC LIMIT " + ServerConfiguration.RoomUserLimit);
					}
					else
					{
						string_0 = string_0.Replace("%", "\\%");
						string_0 = string_0.Replace("_", "\\_");
						@class.AddParamWithValue("query", string_0 + "%");
						@class.AddParamWithValue("tags_query", "%" + string_0 + "%");
						dataTable = @class.ReadDataTable("SELECT * FROM rooms WHERE caption LIKE @query AND roomtype = 'private' OR owner LIKE @query AND roomtype = 'private' ORDER BY users_now DESC LIMIT 40");
					}
				}
			}
			List<RoomData> list = new List<RoomData>();
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					RoomData item = GoldTree.GetGame().GetRoomManager().method_17((uint)dataRow["Id"], dataRow);
					list.Add(item);
				}
			}
			ServerMessage Message = new ServerMessage(451u);
			Message.AppendInt32(1);
			Message.AppendInt32(9);
			Message.AppendStringWithBreak(string_0);
			Message.AppendInt32(list.Count);
			foreach (RoomData current in list)
			{
				current.method_3(Message, false, false);
			}
			return Message;
		}
		internal byte[] SerializeNavigator(GameClient Session, int int_0)
		{
			byte[] result;
			if (int_0 != -2)
			{
				result = this.method_12(Session, int_0).GetBytes();
			}
			else
			{
				byte[] array = GoldTree.GetGame().GetNavigatorCache().method_1(int_0);
				if (array != null)
				{
					result = array;
				}
				else
				{
					result = this.method_12(null, int_0).GetBytes();
				}
			}
			return result;
		}
		public ServerMessage method_12(GameClient Session, int int_0)
		{
			Func<RoomData, bool> func = null;
			Func<Room, bool> func2 = null;
			Navigator.Class219 @class = new Navigator.Class219();
			@class.int_0 = int_0;
			ServerMessage Message = new ServerMessage(451u);
			if (@class.int_0 >= -1)
			{
				Message.AppendInt32(@class.int_0);
				Message.AppendInt32(1);
			}
			else
			{
				if (@class.int_0 == -2)
				{
					Message.AppendInt32(0);
					Message.AppendInt32(2);
				}
				else
				{
					if (@class.int_0 == -3)
					{
						Message.AppendInt32(0);
						Message.AppendInt32(5);
					}
					else
					{
						if (@class.int_0 == -4)
						{
							Message.AppendInt32(0);
							Message.AppendInt32(3);
						}
						else
						{
							if (@class.int_0 == -5)
							{
								Message.AppendInt32(0);
								Message.AppendInt32(4);
							}
						}
					}
				}
			}
			Message.AppendStringWithBreak("");
			List<RoomData> list = new List<RoomData>();
			switch (@class.int_0)
			{
			case -5:
			case -4:
				break;
			case -3:
				goto IL_3A2;
			case -2:
				goto IL_3E5;
			case -1:
				goto IL_47E;
			default:
			{
				Dictionary<Room, int> dictionary = GoldTree.GetGame().GetRoomManager().method_21();
				IEnumerable<RoomData> arg_11F_0 = GoldTree.GetGame().GetRoomManager().list_3;
				if (func == null)
				{
					func = new Func<RoomData, bool>(@class.method_0);
				}
				IEnumerable<RoomData> enumerable = arg_11F_0.Where(func);
				IEnumerable<Room> arg_13E_0 = dictionary.Keys;
				if (func2 == null)
				{
					func2 = new Func<Room, bool>(@class.method_1);
				}
				IEnumerable<Room> arg_160_0 = arg_13E_0.Where(func2);
				if (Navigator.func_2 == null)
				{
					Navigator.func_2 = new Func<Room, int>(Navigator.smethod_3);
				}
				IOrderedEnumerable<Room> orderedEnumerable = arg_160_0.OrderByDescending(Navigator.func_2);
				new List<RoomData>();
				int num = 0;
				foreach (Room current in orderedEnumerable)
				{
					if (num > 40)
					{
						break;
					}
					list.Add(current.RoomData);
					num++;
				}
				using (IEnumerator<RoomData> enumerator2 = enumerable.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						RoomData current2 = enumerator2.Current;
						if (num > 40)
						{
							break;
						}
						if (list.Contains(current2))
						{
							list.Remove(current2);
						}
						list.Add(current2);
						num++;
					}
					goto IL_508;
				}
			}
			}
			List<string> list2 = new List<string>();
			Dictionary<int, Room> dictionary2 = new Dictionary<int, Room>();
			Hashtable hashtable = Session.GetHabbo().GetMessenger().method_26().Clone() as Hashtable;
			Dictionary<RoomData, int> dictionary3 = new Dictionary<RoomData, int>();
			foreach (MessengerBuddy class2 in hashtable.Values)
			{
				if (class2.Boolean_0 && class2.Boolean_1)
				{
					GameClient class3 = GoldTree.GetGame().GetClientManager().method_2(class2.UInt32_0);
					if (class3 != null && class3.GetHabbo() != null && class3.GetHabbo().CurrentRoom != null)
					{
						RoomData class27_ = class3.GetHabbo().CurrentRoom.RoomData;
						if (!dictionary3.ContainsKey(class27_))
						{
							dictionary3.Add(class27_, class27_.UsersNow);
						}
					}
				}
			}
			IEnumerable<RoomData> arg_344_0 = dictionary3.Keys;
			if (Navigator.func_0 == null)
			{
				Navigator.func_0 = new Func<RoomData, int>(Navigator.smethod_1);
			}
			IOrderedEnumerable<RoomData> orderedEnumerable2 = arg_344_0.OrderByDescending(Navigator.func_0);
			list2.Clear();
			dictionary2.Clear();
			hashtable.Clear();
			dictionary3.Clear();
			using (IEnumerator<RoomData> enumerator2 = orderedEnumerable2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RoomData current3 = enumerator2.Current;
					list.Add(current3);
				}
				goto IL_508;
			}
			IL_3A2:
			using (List<RoomData>.Enumerator enumerator4 = Session.GetHabbo().OwnedRooms.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					RoomData current3 = enumerator4.Current;
					list.Add(current3);
				}
				goto IL_508;
			}
			IL_3E5:
			DataTable dataTable;
			using (DatabaseClient class4 = GoldTree.GetDatabase().GetClient())
			{
				dataTable = class4.ReadDataTable("SELECT * FROM rooms WHERE score > 0 AND roomtype = 'private' ORDER BY score DESC LIMIT 40");
			}
			IEnumerator enumerator3 = dataTable.Rows.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					DataRow dataRow = (DataRow)enumerator3.Current;
					list.Add(GoldTree.GetGame().GetRoomManager().method_17((uint)dataRow["Id"], dataRow));
				}
				goto IL_508;
			}
			finally
			{
				IDisposable disposable = enumerator3 as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			IL_47E:
			Dictionary<Room, int> dictionary4 = GoldTree.GetGame().GetRoomManager().method_21();
			IEnumerable<Room> arg_4B3_0 = dictionary4.Keys;
			if (Navigator.func_1 == null)
			{
				Navigator.func_1 = new Func<Room, int>(Navigator.smethod_2);
			}
			IOrderedEnumerable<Room> orderedEnumerable3 = arg_4B3_0.OrderByDescending(Navigator.func_1);
			int num2 = 0;
			foreach (Room current4 in orderedEnumerable3)
			{
				if (num2 >= 40)
				{
					break;
				}
				num2++;
				list.Add(current4.RoomData);
			}
			IL_508:
			Message.AppendInt32(list.Count);
			foreach (RoomData current5 in list)
			{
				current5.method_3(Message, false, false);
			}
			Random random = new Random();
			Message.AppendStringWithBreak("");
			this.dictionary_1.ElementAt(random.Next(0, this.dictionary_1.Count)).Value.method_0(Message);
			return Message;
		}
		[CompilerGenerated]
		private static int smethod_0(KeyValuePair<string, int> keyValuePair_0, KeyValuePair<string, int> keyValuePair_1)
		{
			return keyValuePair_0.Value.CompareTo(keyValuePair_1.Value);
		}
		[CompilerGenerated]
		private static int smethod_1(RoomData class27_0)
		{
			return class27_0.UsersNow;
		}
		[CompilerGenerated]
		private static int smethod_2(Room class14_0)
		{
			return class14_0.UserCount;
		}
		[CompilerGenerated]
		private static int smethod_3(Room class14_0)
		{
			return class14_0.UserCount;
		}
	}
}
