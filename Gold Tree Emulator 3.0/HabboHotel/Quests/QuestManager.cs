using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Quests;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Quests
{
	internal sealed class QuestManager
	{
		public List<Quest> Quests;
        public int QuestsCount;
		public QuestManager()
		{
			this.Quests = new List<Quest>();
		}
		public void method_0()
		{
            Logging.Write("Loading Quests..");
			this.Quests.Clear();
			DataTable dataTable = null;
            DataTable dataTable2 = null;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				dataTable = @class.ReadDataTable("SELECT Id,type,action,needofcount,level_num,pixel_reward FROM quests WHERE enabled = '1' ORDER by level_num");
                dataTable2 = @class.ReadDataTable("SELECT COUNT(*) as count FROM quests GROUP BY type ORDER BY count DESC;");
			}
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					Quest class2 = new Quest((uint)dataRow["Id"], (string)dataRow["type"], (string)dataRow["action"], (int)dataRow["needofcount"], (int)dataRow["level_num"], (int)dataRow["pixel_reward"]);
					if (class2 != null)
					{
						this.Quests.Add(class2);
					}
				}
				//Logging.WriteLine("completed!", ConsoleColor.Green);
			}

            if (dataTable2 != null)
            {
                QuestsCount = dataTable2.Rows.Count;

                Logging.WriteLine("completed!", ConsoleColor.Green);
            }
		}
		public void ProgressUserQuest(uint uint_0, GameClient Session)
		{
			Session.GetHabbo().CurrentQuestProgress++;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.ExecuteQuery("UPDATE user_stats SET quest_progress = quest_progress + 1 WHERE Id = '" + Session.GetHabbo().Id + "' LIMIT 1");
			}
			GoldTree.GetGame().GetQuestManager().method_7(uint_0, Session);
		}
		public int GetHighestLevelForType(string string_0)
		{
			int num = 0;
			foreach (Quest current in this.Quests)
			{
				if (current.Type == string_0)
				{
					num++;
				}
			}
			return num;
		}
		public uint method_3(int int_0, string string_0)
		{
			uint result;
			foreach (Quest current in this.Quests)
			{
				if (current.Type == string_0 && current.Level == int_0 + 1)
				{
					result = current.QuestId();
					return result;
				}
			}
			result = 0u;
			return result;
		}
		public void method_4(GameClient Session)
		{
			Quest @class = this.method_6(Session.GetHabbo().uint_7);
			string text = @class.Type.ToLower();
			int int_ = 0;
			string text2 = text;
			if (text2 != null)
			{
				if (!(text2 == "social"))
				{
					if (!(text2 == "room_builder"))
					{
						if (!(text2 == "identity"))
						{
                            if (!(text2 == "explore"))
                            {
                                if (text2 == "custom1")
                                {
                                    int_ = Session.GetHabbo().QuestsCustom1Progress;
                                }
                            }
                            else
                            {
                                int_ = Session.GetHabbo().ExplorerLevel;
                            }
						}
						else
						{
							int_ = Session.GetHabbo().IdentityLevel;
						}
					}
					else
					{
						int_ = Session.GetHabbo().BuilderLevel;
					}
				}
				else
				{
					int_ = Session.GetHabbo().SocialLevel;
				}
			}
			if (this.method_3(int_, text) != 0u)
			{
				this.method_7(this.method_3(int_, text), Session);
			}
		}
		public ServerMessage method_5(GameClient Session)
		{
			ServerMessage Message = new ServerMessage(800u);
            Message.AppendInt32(QuestsCount);
			this.method_9(Session, Message);
			Message.AppendInt32(1);
			return Message;
		}
		public Quest method_6(uint uint_0)
		{
			Quest result;
			foreach (Quest current in this.Quests)
			{
				if (current.QuestId() == uint_0)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}
		public void method_7(uint uint_0, GameClient Session)
		{
			if (Session != null && Session.GetHabbo() != null)
			{
				if (Session.GetHabbo().CurrentQuestId != uint_0)
				{
					Session.GetHabbo().CurrentQuestId = uint_0;
					Session.GetHabbo().CurrentQuestProgress = 0;
					using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
					{
						@class.AddParamWithValue("uid", Session.GetHabbo().Id);
						@class.AddParamWithValue("qid", uint_0);
						@class.ExecuteQuery("UPDATE user_stats SET quest_id = @qid, quest_progress = '0' WHERE Id = @uid LIMIT 1");
					}
				}
				if (uint_0 == 0u)
				{
					Session.SendMessage(this.method_5(Session));
					ServerMessage Message5_ = new ServerMessage(803u);
					Session.SendMessage(Message5_);
				}
				else
				{
					Quest class2 = this.method_6(uint_0);
					if (class2.NeedForLevel <= Session.GetHabbo().CurrentQuestProgress)
					{
						this.method_8(uint_0, Session);
					}
					else
					{
						ServerMessage Message5_ = new ServerMessage(802u);
						class2.Serialize(Message5_, Session, true);
						Session.SendMessage(Message5_);
					}
				}
			}
		}
		public void method_8(uint uint_0, GameClient Session)
		{
			Session.GetHabbo().CurrentQuestId = 0u;
			Session.GetHabbo().uint_7 = uint_0;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("userid", Session.GetHabbo().Id);
				@class.AddParamWithValue("questid", uint_0);
				@class.ExecuteQuery(string.Concat(new string[]
				{
					"UPDATE user_stats SET quest_id = '0',quest_progress = '0', lev_",
					this.method_6(uint_0).Type.Replace("room_", ""),
					" = lev_",
					this.method_6(uint_0).Type.Replace("room_", ""),
					" + 1 WHERE Id = @userid LIMIT 1"
				}));
				@class.ExecuteQuery("INSERT INTO user_quests (user_id,quest_id) VALUES (@userid,@questid)");
			}
			string text = this.method_6(uint_0).Type.ToLower();
			if (text != null)
			{
				if (!(text == "identity"))
				{
					if (!(text == "room_builder"))
					{
						if (!(text == "social"))
						{
                            if (!(text == "explore"))
                            {
                                if (text == "custom1")
                                {
                                    Session.GetHabbo().QuestsCustom1Progress++;
                                }
                            }
                            else
                            {
                                Session.GetHabbo().ExplorerLevel++;
                            }
						}
						else
						{
							Session.GetHabbo().SocialLevel++;
						}
					}
					else
					{
						Session.GetHabbo().BuilderLevel++;
					}
				}
				else
				{
					Session.GetHabbo().IdentityLevel++;
				}
			}
			Session.GetHabbo().LoadQuests();
			ServerMessage Message = new ServerMessage(801u);
			Quest class2 = this.method_6(uint_0);
			class2.Serialize(Message, Session, true);
			this.method_9(Session, Message);
			Message.AppendInt32(1);
			Session.SendMessage(Message);
			Session.GetHabbo().ActivityPoints += class2.PixelReward;
			Session.GetHabbo().UpdateActivityPoints(true);
			Session.GetHabbo().CurrentQuestProgress = 0;
		}
		public void method_9(GameClient Session, ServerMessage Message5_0)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
            bool flag5 = false;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
            int num5 = 0;
			foreach (Quest current in this.Quests)
			{
                if (current == null || Session == null ||Session.GetHabbo() == null)
                {
                    continue;
                }

				if (current.QuestId() == Session.GetHabbo().CurrentQuestId)
				{
					current.Serialize(Message5_0, Session, false);
					string text = current.Type.ToLower();
					if (text != null)
					{
						if (!(text == "social"))
						{
							if (!(text == "room_builder"))
							{
								if (!(text == "identity"))
								{
                                    if (!(text == "explore"))
                                    {
                                        if (text == "custom1")
                                        {
                                            flag5 = true;
                                        }
                                    }
                                    else
                                    {
                                        flag4 = true;
                                    }
								}
								else
								{
									flag3 = true;
								}
							}
							else
							{
								flag2 = true;
							}
						}
						else
						{
							flag = true;
						}
					}
				}
				if (current.Type.ToLower() == "room_builder" && num2 < Session.GetHabbo().BuilderLevel)
				{
					num2 = current.Level;
				}
				if (current.Type.ToLower() == "social" && num < Session.GetHabbo().SocialLevel)
				{
					num = current.Level;
				}
				if (current.Type.ToLower() == "identity" && num3 < Session.GetHabbo().IdentityLevel)
				{
					num3 = current.Level;
				}
				if (current.Type.ToLower() == "explore" && num4 < Session.GetHabbo().ExplorerLevel)
				{
					num4 = current.Level;
				}
                if (current.Type.ToLower() == "custom1" && num5 < Session.GetHabbo().QuestsCustom1Progress)
                {
                    num4 = current.Level;
                }
				if (current.Type.ToLower() == "room_builder" && !flag2 && current.Level == this.GetHighestLevelForType("room_builder") && Session.GetHabbo().BuilderLevel == this.GetHighestLevelForType("room_builder"))
				{
					current.Serialize(Message5_0, Session, false);
					flag2 = true;
				}
				if (current.Type.ToLower() == "social" && !flag && current.Level == this.GetHighestLevelForType("social") && Session.GetHabbo().SocialLevel == this.GetHighestLevelForType("room_social"))
				{
					current.Serialize(Message5_0, Session, false);
					flag = true;
				}
				if (current.Type.ToLower() == "identity" && !flag3 && current.Level == this.GetHighestLevelForType("identity") && Session.GetHabbo().IdentityLevel == this.GetHighestLevelForType("identity"))
				{
					current.Serialize(Message5_0, Session, false);
					flag3 = true;
				}
				if (current.Type.ToLower() == "explore" && !flag4 && current.Level == this.GetHighestLevelForType("explore") && Session.GetHabbo().ExplorerLevel == this.GetHighestLevelForType("explore"))
				{
					current.Serialize(Message5_0, Session, false);
					flag4 = true;
				}
                if (current.Type.ToLower() == "custom1" && !flag5 && current.Level == this.GetHighestLevelForType("custom1") && Session.GetHabbo().QuestsCustom1Progress == this.GetHighestLevelForType("custom1"))
                {
                    current.Serialize(Message5_0, Session, false);
                    flag5 = true;
                }
				if (current.Type.ToLower() == "room_builder" && !flag2 && current.Level == Session.GetHabbo().BuilderLevel + 1)
				{
					current.Serialize(Message5_0, Session, false);
					flag2 = true;
				}
				if (current.Type.ToLower() == "social" && !flag && current.Level == Session.GetHabbo().SocialLevel + 1)
				{
					current.Serialize(Message5_0, Session, false);
					flag = true;
				}
				if (current.Type.ToLower() == "identity" && !flag3 && current.Level == Session.GetHabbo().IdentityLevel + 1)
				{
					current.Serialize(Message5_0, Session, false);
					flag3 = true;
				}
				if (current.Type.ToLower() == "explore" && !flag4 && current.Level == Session.GetHabbo().ExplorerLevel + 1)
				{
					current.Serialize(Message5_0, Session, false);
					flag4 = true;
				}
                if (current.Type.ToLower() == "custom1" && !flag4 && current.Level == Session.GetHabbo().QuestsCustom1Progress + 1)
                {
                    current.Serialize(Message5_0, Session, false);
                    flag5 = true;
                }
			}
			if (!flag2 || !flag || !flag3 || !flag4 || !flag5)
			{
				foreach (Quest current in this.Quests)
				{
					if (current.Type.ToLower() == "room_builder" && !flag2 && current.Level == num2)
					{
						current.Serialize(Message5_0, Session, false);
						flag2 = true;
					}
					if (current.Type.ToLower() == "social" && !flag && current.Level == num)
					{
						current.Serialize(Message5_0, Session, false);
						flag = true;
					}
					if (current.Type.ToLower() == "identity" && !flag3 && current.Level == num3)
					{
						current.Serialize(Message5_0, Session, false);
						flag3 = true;
					}
					if (current.Type.ToLower() == "explore" && !flag4 && current.Level == num4)
					{
						current.Serialize(Message5_0, Session, false);
						flag4 = true;
					}
                    if (current.Type.ToLower() == "custom1" && !flag4 && current.Level == num5)
                    {
                        current.Serialize(Message5_0, Session, false);
                        flag4 = true;
                    }
				}
			}
		}

        public string GetQuestAction(uint id)
        {
            string result;
            foreach (Quest current in this.Quests)
            {
                if (current.QuestId() == id)
                {
                    result = current.Action;
                    return result;
                }
            }
            result = null;
            return result;
        }
	}
}