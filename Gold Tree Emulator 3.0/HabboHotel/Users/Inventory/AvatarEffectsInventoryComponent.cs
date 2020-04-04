using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GoldTree.HabboHotel.Users.UserDataManagement;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Users.Inventory
{
	internal sealed class AvatarEffectsInventoryComponent
	{
		private List<AvatarEffect> list_0;
		private uint uint_0;
		public int int_0;
		private GameClient Session;
		public int Int32_0
		{
			get
			{
				return this.list_0.Count;
			}
		}
		public AvatarEffectsInventoryComponent(uint uint_1, GameClient class16_1, UserDataFactory class12_0)
		{
			this.Session = class16_1;
			this.list_0 = new List<AvatarEffect>();
			this.uint_0 = uint_1;
			this.int_0 = -1;
			this.list_0.Clear();
			DataTable dataTable_ = class12_0.GetEffects();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataRow dataRow in dataTable_.Rows)
			{
				AvatarEffect @class = new AvatarEffect((int)dataRow["effect_id"], (int)dataRow["total_duration"], GoldTree.StringToBoolean(dataRow["is_activated"].ToString()), (double)dataRow["activated_stamp"]);
				if (@class.Boolean_0)
				{
					stringBuilder.Append(string.Concat(new object[]
					{
						"DELETE FROM user_effects WHERE user_id = '",
						uint_1,
						"' AND effect_id = '",
						@class.int_0,
						"' LIMIT 1; "
					}));
				}
				else
				{
					this.list_0.Add(@class);
				}
			}
			if (stringBuilder.Length > 0)
			{
				using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(stringBuilder.ToString());
				}
			}
		}
		public void method_0(int int_1, int int_2)
		{
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.ExecuteQuery(string.Concat(new object[]
				{
					"INSERT INTO user_effects (user_id,effect_id,total_duration,is_activated,activated_stamp) VALUES ('",
					this.uint_0,
					"','",
					int_1,
					"','",
					int_2,
					"','0','0')"
				}));
			}
			this.list_0.Add(new AvatarEffect(int_1, int_2, false, 0.0));
			ServerMessage Message = new ServerMessage(461u);
			Message.AppendInt32(int_1);
			Message.AppendInt32(int_2);
			this.method_8().SendMessage(Message);
		}
		public void method_1(int int_1)
		{
			AvatarEffect @class = this.method_5(int_1, true);
			if (@class != null && @class.Boolean_0)
			{
				using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
				{
					class2.ExecuteQuery(string.Concat(new object[]
					{
						"DELETE FROM user_effects WHERE user_id = '",
						this.uint_0,
						"' AND effect_id = '",
						int_1,
						"' AND is_activated = '1' LIMIT 1"
					}));
				}
				this.list_0.Remove(@class);
				ServerMessage Message = new ServerMessage(463u);
				Message.AppendInt32(int_1);
				this.method_8().SendMessage(Message);
				if (this.int_0 >= 0)
				{
					this.method_2(-1, false);
				}
			}
		}
		public void method_2(int int_1, bool bool_0)
		{
			if (this.method_4(int_1, true) || bool_0)
			{
				Room @class = this.method_9();
				if (@class != null && (this.method_8() != null && this.method_8().GetHabbo() != null))
				{
					RoomUser class2 = @class.GetRoomUserByHabbo(this.method_8().GetHabbo().Id);
					if (class2 != null && (class2.byte_1 <= 0 || int_1 == -1 || bool_0) && (class2.class34_1 == null || int_1 == 77 || int_1 == -1))
					{
						this.int_0 = int_1;
                        if (class2.GetClient() != null && class2.GetClient().GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(class2.GetClient().GetHabbo().CurrentQuestId) == "SWIM" && (this.int_0 == 28 || this.int_0 == 29 || this.int_0 == 30 || this.int_0 == 37))
						{
                            GoldTree.GetGame().GetQuestManager().ProgressUserQuest(class2.GetClient().GetHabbo().CurrentQuestId, class2.GetClient());
						}
						ServerMessage Message = new ServerMessage(485u);
						Message.AppendInt32(class2.VirtualId);
						Message.AppendInt32(int_1);
						@class.SendMessage(Message, null);
					}
				}
			}
		}
		public void method_3(int int_1)
		{
			AvatarEffect @class = this.method_5(int_1, false);
			if (@class != null && !@class.Boolean_0 && !@class.bool_0 && (this.method_8() != null && this.method_8().GetHabbo() != null))
			{
				Room class2 = this.method_9();
				if (class2 != null)
				{
					RoomUser class3 = class2.GetRoomUserByHabbo(this.method_8().GetHabbo().Id);
					if (class3.byte_1 <= 0 && class3.class34_1 == null)
					{
						using (DatabaseClient class4 = GoldTree.GetDatabase().GetClient())
						{
							class4.ExecuteQuery(string.Concat(new object[]
							{
								"UPDATE user_effects SET is_activated = '1', activated_stamp = '",
								GoldTree.GetUnixTimestamp(),
								"' WHERE user_id = '",
								this.uint_0,
								"' AND effect_id = '",
								int_1,
								"' LIMIT 1"
							}));
						}
						@class.method_0();
						ServerMessage Message = new ServerMessage(462u);
						Message.AppendInt32(@class.int_0);
						Message.AppendInt32(@class.int_1);
						this.method_8().SendMessage(Message);
					}
				}
			}
		}
		public bool method_4(int int_1, bool bool_0)
		{
			if (int_1 == -1 || int_1 == 28 || int_1 == 29)
			{
				return true;
			}
			else
			{
				using (TimedLock.Lock(this.list_0))
				{
					foreach (AvatarEffect current in this.list_0)
					{
						if ((!bool_0 || current.bool_0) && !current.Boolean_0 && current.int_0 == int_1)
						{
                            return true;
						}
					}
				}
				return false;
			}
		}
		public AvatarEffect method_5(int int_1, bool bool_0)
		{
			using (TimedLock.Lock(this.list_0))
			{
				foreach (AvatarEffect current in this.list_0)
				{
					if ((!bool_0 || current.bool_0) && current.int_0 == int_1)
					{
						return current;
					}
				}
			}
			return null;
		}
		public ServerMessage method_6()
		{
			ServerMessage Message = new ServerMessage(460u);
			Message.AppendInt32(this.Int32_0);
			using (TimedLock.Lock(this.list_0))
			{
				foreach (AvatarEffect current in this.list_0)
				{
					Message.AppendInt32(current.int_0);
					Message.AppendInt32(current.int_1);
					Message.AppendBoolean(!current.bool_0);
					Message.AppendInt32(current.Int32_0);
				}
			}
			return Message;
		}
		public void method_7()
		{
			using (TimedLock.Lock(this.list_0))
			{
				List<int> list = new List<int>();
				foreach (AvatarEffect current in this.list_0)
				{
					if (current.Boolean_0)
					{
						list.Add(current.int_0);
					}
				}
				foreach (int current2 in list)
				{
					this.method_1(current2);
				}
			}
		}
		private GameClient method_8()
		{
			return this.Session;
		}
		private Room method_9()
		{
			return this.Session.GetHabbo().CurrentRoom;
		}
	}
}
