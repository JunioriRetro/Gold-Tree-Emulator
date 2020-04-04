using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Users.Badges;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Achievements
{
	internal sealed class AchievementManager
	{
		public static Dictionary<uint, Achievement> dictionary_0;
		public static Dictionary<string, uint> dictionary_1;
		public AchievementManager()
		{
			AchievementManager.dictionary_0 = new Dictionary<uint, Achievement>();
			AchievementManager.dictionary_1 = new Dictionary<string, uint>();
		}
		public static void smethod_0(DatabaseClient class6_0)
		{
            Logging.Write("Loading Achievements..");
			AchievementManager.dictionary_0.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM achievements");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
                    AchievementManager.dictionary_0.Add((uint)dataRow["Id"], new Achievement((uint)dataRow["Id"], (string)dataRow["type"], (int)dataRow["levels"], (string)dataRow["badge"], (int)dataRow["pixels_base"], (double)dataRow["pixels_multiplier"], GoldTree.StringToBoolean(dataRow["dynamic_badgelevel"].ToString()), (int)dataRow["score_base"], (int)dataRow["pixels_base"]));
				}
				AchievementManager.dictionary_1.Clear();
				dataTable = class6_0.ReadDataTable("SELECT * FROM badges");
				if (dataTable != null)
				{
					foreach (DataRow dataRow in dataTable.Rows)
					{
						AchievementManager.dictionary_1.Add((string)dataRow["badge"], (uint)dataRow["Id"]);
					}
					Logging.WriteLine("completed!", ConsoleColor.Green);
				}
			}
		}
		public uint method_0(string string_0)
		{
			if (AchievementManager.dictionary_1.ContainsKey(string_0))
			{
				return AchievementManager.dictionary_1[string_0];
			}
			else
			{
                return 0;
			}
		}
		public bool method_1(GameClient Session, uint uint_0, int int_0)
		{
			return Session.GetHabbo().dictionary_0.ContainsKey(uint_0) && Session.GetHabbo().dictionary_0[uint_0] >= int_0;
		}
		public static ServerMessage smethod_1(GameClient Session)
		{
			int num = AchievementManager.dictionary_0.Count;
			foreach (KeyValuePair<uint, Achievement> current in AchievementManager.dictionary_0)
			{
				if (current.Value.Type == "hidden")
				{
					num--;
				}
			}
			ServerMessage Message = new ServerMessage(436u);
			Message.AppendInt32(num);
			foreach (KeyValuePair<uint, Achievement> current in AchievementManager.dictionary_0)
			{
				if (!(current.Value.Type == "hidden"))
				{
					int num2 = 0;
					int num3 = 1;
					if (Session.GetHabbo().dictionary_0.ContainsKey(current.Value.Id))
					{
						num2 = Session.GetHabbo().dictionary_0[current.Value.Id];
					}
					if (current.Value.Levels > 1 && num2 > 0)
					{
						num3 = num2 + 1;
					}
					if (num3 > current.Value.Levels)
					{
						num3 = current.Value.Levels;
					}
                    int Need = GetNeed(current.Value.Id, num3, Session.GetHabbo());
                    int Have = GetHave(current.Value.Id, Session.GetHabbo());
                    int Pixels = AchievementManager.smethod_2(current.Value.Dynamic_badgelevel, current.Value.PixelMultiplier, num3);
					Message.AppendUInt(current.Value.Id);
					Message.AppendInt32(num3);
					Message.AppendStringWithBreak(AchievementManager.smethod_3(current.Value.BadgeCode, num3, current.Value.DynamicBadgeLevel));
                    Message.AppendInt32(Need);
					Message.AppendInt32(Pixels);
					Message.AppendInt32(0);
					Message.AppendInt32(Have);
					Message.AppendBoolean(num2 == current.Value.Levels);
					Message.AppendStringWithBreak(current.Value.Type);
					Message.AppendInt32(current.Value.Levels);
				}
			}
			return Message;
		}
		public void addAchievement(GameClient Session, uint uint_0)
		{
			if (!AchievementManager.dictionary_0.ContainsKey(uint_0))
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("AchievementID: " + uint_0 + " does not exist in your database!");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else
			{
				Achievement @class = AchievementManager.dictionary_0[uint_0];
				if (@class != null)
				{
					if (Session.GetHabbo().dictionary_0.ContainsKey(uint_0))
					{
						//this.addAchievement(Session, uint_0, Session.GetHabbo().dictionary_0[uint_0 + 1u]); //HEY AARON! THIS NOT WORKING! ACHIEVEMENT ID + 1? WHAT? IT NEED ARE ACHIEVEMENT LEVEL + 1!!!
                        int nextlevel = Session.GetHabbo().dictionary_0[uint_0] + 1;
                        this.addAchievement(Session, uint_0, nextlevel);
					}
					else
					{
						this.addAchievement(Session, uint_0, 1);
					}
				}
			}
		}
        public void addAchievement(GameClient Session, uint uint_0, int int_0)
        {
            if (!AchievementManager.dictionary_0.ContainsKey(uint_0))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("AchievementID: " + uint_0 + " does not exist in our database!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Achievement @class = AchievementManager.dictionary_0[uint_0];
                if (@class != null && !this.method_1(Session, @class.Id, int_0) && int_0 >= 1 && int_0 <= @class.Levels)
                {
                    int num = AchievementManager.smethod_2(@class.Dynamic_badgelevel, @class.PixelMultiplier, int_0);
                    int num2 = AchievementManager.smethod_2(@class.ScoreBase, @class.PixelMultiplier, int_0);
                    using (TimedLock.Lock(Session.GetHabbo().GetBadgeComponent().GetBadges()))
                    {
                        List<string> list = new List<string>();
                        foreach (Badge current in Session.GetHabbo().GetBadgeComponent().GetBadges())
                        {
                            if (current.Code.StartsWith(@class.BadgeCode))
                            {
                                list.Add(current.Code);
                            }
                        }
                        foreach (string current2 in list)
                        {
                            Session.GetHabbo().GetBadgeComponent().RemoveBadge(current2);
                        }
                    }
                    Session.GetHabbo().GetBadgeComponent().SendBadge(Session, AchievementManager.smethod_3(@class.BadgeCode, int_0, @class.DynamicBadgeLevel), true);
                    if (Session.GetHabbo().dictionary_0.ContainsKey(@class.Id))
                    {
                        Session.GetHabbo().dictionary_0[@class.Id] = int_0;
                        using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
                        {
                            class2.ExecuteQuery(string.Concat(new object[]
							{
								"UPDATE user_achievements SET achievement_level = '",
								int_0,
								"' WHERE user_id = '",
								Session.GetHabbo().Id,
								"' AND achievement_id = '",
								@class.Id,
								"' LIMIT 1; UPDATE user_stats SET AchievementScore = AchievementScore + ",
								num2,
								" WHERE Id = '",
								Session.GetHabbo().Id,
								"' LIMIT 1; "
							}));
                            goto IL_346;
                        }
                    }
                    Session.GetHabbo().dictionary_0.Add(@class.Id, int_0);
                    using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
                    {
                        class2.ExecuteQuery(string.Concat(new object[]
						{
							"INSERT INTO user_achievements (user_id,achievement_id,achievement_level) VALUES ('",
							Session.GetHabbo().Id,
							"','",
							@class.Id,
							"','",
							int_0,
							"'); UPDATE user_stats SET AchievementScore = AchievementScore + ",
							num2,
							" WHERE Id = '",
							Session.GetHabbo().Id,
							"' LIMIT 1; "
						}));
                    }
                IL_346:
                    ServerMessage Message = new ServerMessage(437u);
                    Message.AppendUInt(@class.Id);
                    Message.AppendInt32(int_0);
                    Message.AppendInt32(1337);
                    Message.AppendStringWithBreak(AchievementManager.smethod_3(@class.BadgeCode, int_0, @class.DynamicBadgeLevel));
                    Message.AppendInt32(num2);
                    Message.AppendInt32(num);
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                    Message.AppendInt32(0);
                    if (int_0 > 1)
                    {
                        Message.AppendStringWithBreak(AchievementManager.smethod_3(@class.BadgeCode, int_0 - 1, @class.DynamicBadgeLevel));
                    }
                    else
                    {
                        Message.AppendStringWithBreak("");
                    }
                    Message.AppendStringWithBreak(@class.Type);
                    Session.SendMessage(Message);
                    Session.GetHabbo().AchievementScore += num2;
                    Session.GetHabbo().ActivityPoints += num;
                    Session.GetHabbo().method_16(num);

                    if (Session.GetHabbo().FriendStreamEnabled)
                    {
                        using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
                        {
                            string BadgeCode = "";
                            if (@class.DynamicBadgeLevel)
                            {
                                BadgeCode = @class.BadgeCode + int_0.ToString();
                            }
                            else
                            {
                                BadgeCode = @class.BadgeCode;
                            }

                            if (!string.IsNullOrEmpty(BadgeCode))
                            {
                                string look = GoldTree.FilterString(Session.GetHabbo().Figure);
                                class2.AddParamWithValue("look", look);
                                class2.ExecuteQuery("INSERT INTO `friend_stream` (`id`, `type`, `userid`, `gender`, `look`, `time`, `data`) VALUES (NULL, '2', '" + Session.GetHabbo().Id + "', '" + Session.GetHabbo().Gender + "', @look, UNIX_TIMESTAMP(), '" + BadgeCode + "');");
                            }
                        }
                    }
                }
            }
        }
		public static int smethod_2(int int_0, double double_0, int int_1)
		{
			return (int)((double)int_0 * ((double)int_1 * double_0));
		}
		public static string smethod_3(string string_0, int int_0, bool bool_0)
		{
			if (!bool_0)
			{
				return string_0;
			}
			else
			{
				return string_0 + int_0;
			}
		}

        internal static int GetNeed(uint AchievementId, int AchLevel, Users.Habbo Habbo)
        {
            if (AchievementId == 4)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 2;
                    case 2:
                        return 5;
                    case 3:
                        return 10;
                    case 4:
                        return 20;
                    case 5:
                        return 40;
                    case 6:
                        return 70;
                    case 7:
                        return 110;
                    case 8:
                        return 170;
                    case 9:
                        return 250;
                    case 10:
                        return 350;
                    case 11:
                        return 470;
                    case 12:
                        return 610;
                    case 13:
                        return 770;
                    case 14:
                        return 950;
                    case 15:
                        return 1150;
                    case 16:
                        return 1370;
                    case 17:
                        return 1610;
                    case 18:
                        return 1870;
                    case 19:
                        return 2150;
                    case 20:
                        return 2450;
                }
            }

            if (AchievementId == 6)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 6;
                    case 3:
                        return 16;
                    case 4:
                        return 66;
                    case 5:
                        return 166;
                    case 6:
                        return 366;
                    case 7:
                        return 566;
                    case 8:
                        return 766;
                    case 9:
                        return 966;
                    case 10:
                        return 1166;
                }
            }

            if (AchievementId == 7)
            {
                return 5;
            }

            if (AchievementId == 8)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 5;
                    case 2:
                        return 10;
                    case 3:
                        return 50;
                    case 4:
                        return 100;
                    case 5:
                        return 160;
                    case 6:
                        return 240;
                    case 7:
                        return 360;
                    case 8:
                        return 500;
                    case 9:
                        return 660;
                    case 10:
                        return 860;
                    case 11:
                        return 1080;
                    case 12:
                        return 1320;
                    case 13:
                        return 1580;
                    case 14:
                        return 1860;
                    case 15:
                        return 2160;
                    case 16:
                        return 2480;
                    case 17:
                        return 2820;
                    case 18:
                        return 3180;
                    case 19:
                        return 3560;
                    case 20:
                        return 3960;
                }
            }

            if (AchievementId == 10)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 6;
                    case 3:
                        return 14;
                    case 4:
                        return 26;
                    case 5:
                        return 46;
                    case 6:
                        return 86;
                    case 7:
                        return 146;
                    case 8:
                        return 236;
                    case 9:
                        return 366;
                    case 10:
                        return 566;
                    case 11:
                        return 816;
                    case 12:
                        return 1066;
                    case 13:
                        return 1316;
                    case 14:
                        return 1566;
                    case 15:
                        return 1816;
                }
            }

            if (AchievementId == 11)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 6;
                    case 3:
                        return 14;
                    case 4:
                        return 26;
                    case 5:
                        return 46;
                    case 6:
                        return 86;
                    case 7:
                        return 146;
                    case 8:
                        return 236;
                    case 9:
                        return 366;
                    case 10:
                        return 566;
                }
            }

            if (AchievementId == 13)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 20;
                    case 2:
                        return 100;
                    case 3:
                        return 420;
                    case 4:
                        return 600;
                    case 5:
                        return 1920;
                    case 6:
                        return 3120;
                    case 7:
                        return 4620;
                    case 8:
                        return 6420;
                    case 9:
                        return 8520;
                    case 10:
                        return 10920;
                }
            }

            if (AchievementId == 14)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 5;
                    case 3:
                        return 10;
                    case 4:
                        return 15;
                    case 5:
                        return 20;
                    case 6:
                        return 25;
                    case 7:
                        return 30;
                    case 8:
                        return 40;
                    case 9:
                        return 50;
                    case 10:
                        return 75;
                }
            }

            if (AchievementId == 15)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1800 / 60;
                    case 2:
                        return 3600 / 60;
                    case 3:
                        return 7200 / 60;
                    case 4:
                        return 10800 / 60;
                    case 5:
                        return 21600 / 60;
                    case 6:
                        return 43200 / 60;
                    case 7:
                        return 86400 / 60;
                    case 8:
                        return 129600 / 60;
                    case 9:
                        return 172800 / 60;
                    case 10:
                        return 259200 / 60;
                    case 11:
                        return 432000 / 60;
                    case 12:
                        return 604800 / 60;
                    case 13:
                        return 1209600 / 60;
                    case 14:
                        return 1814400 / 60;
                    case 15:
                        return 2419200 / 60;
                    case 16:
                        return 3024000 / 60;
                    case 17:
                        return 3628800 / 60;
                    case 18:
                        return 4838400 / 60;
                    case 19:
                        return 6048000 / 60;
                    case 20:
                        return 8294400 / 60;
                }
            }

            if (AchievementId == 16)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 12;
                    case 3:
                        return 24;
                    case 4:
                        return 36;
                    case 5:
                        return 48;
                }
            }

            if (AchievementId == 17)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 3;
                    case 3:
                        return 10;
                    case 4:
                        return 20;
                    case 5:
                        return 30;
                    case 6:
                        return 56;
                    case 7:
                        return 84;
                    case 8:
                        return 126;
                    case 9:
                        return 168;
                    case 10:
                        return 224;
                    case 11:
                        return 280;
                    case 12:
                        return 365;
                    case 13:
                        return 548;
                    case 14:
                        return 730;
                    case 15:
                        return 913;
                    case 16:
                        return 1095;
                    case 17:
                        return 1278;
                    case 18:
                        return 1460;
                    case 19:
                        return 1643;
                    case 20:
                        return 1825;
                }
            }

            if (AchievementId == 18)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 5;
                    case 2:
                        return 8;
                    case 3:
                        return 15;
                    case 4:
                        return 28;
                    case 5:
                        return 35;
                    case 6:
                        return 50;
                    case 7:
                        return 60;
                    case 8:
                        return 70;
                    case 9:
                        return 80;
                    case 10:
                        return 100;
                    case 11:
                        return 120;
                    case 12:
                        return 140;
                    case 13:
                        return 160;
                    case 14:
                        return 180;
                    case 15:
                        return 200;
                    case 16:
                        return 220;
                    case 17:
                        return 240;
                    case 18:
                        return 260;
                    case 19:
                        return 280;
                    case 20:
                        return 300;
                }
            }

            if (AchievementId == 19)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 10;
                    case 3:
                        return 100;
                    case 4:
                        return 1000;
                    case 5:
                        return 10000;
                }
            }

            if (AchievementId == 20)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 20;
                    case 3:
                        return 400;
                    case 4:
                        return 8000;
                    case 5:
                        return 160000;
                }
            }

            if (AchievementId == 21)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 25;
                    case 2:
                        return 65;
                    case 3:
                        return 125;
                    case 4:
                        return 205;
                    case 5:
                        return 335;
                    case 6:
                        return 525;
                    case 7:
                        return 805;
                    case 8:
                        return 1235;
                    case 9:
                        return 1875;
                    case 10:
                        return 2875;
                    case 11:
                        return 4375;
                    case 12:
                        return 6875;
                    case 13:
                        return 10775;
                    case 14:
                        return 17075;
                    case 15:
                        return 27175;
                    case 16:
                        return 43275;
                    case 17:
                        return 69075;
                    case 18:
                        return 110375;
                    case 19:
                        return 176375;
                    case 20:
                        return 282075;
                }
            }

            if (AchievementId == 22)
            {
                switch (AchLevel)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                    case 3:
                        return 3;
                    case 4:
                        return 4;
                    case 5:
                        return 5;
                    case 6:
                        return 6;
                    case 7:
                        return 7;
                    case 8:
                        return 8;
                    case 9:
                        return 9;
                    case 10:
                        return 10;
                }
            }

            return 1;
        }

        internal static int GetHave(uint AchievementId, Users.Habbo Habbo)
        {
            switch (AchievementId)
            {
                case 4:
                    return Habbo.RespectGiven;
                case 6:
                    return Habbo.Respect;
                case 7:
                    return Habbo.list_3.Count;
                case 8:
                    return Habbo.RoomVisits;
                case 10:
                    return Habbo.GiftsGiven;
                case 11:
                    return Habbo.GiftsReceived;
                case 13:
                    return Habbo.FireworkPixelLoadedCount;
                case 14:
                    return Habbo.PetBuyed;
                case 15:
                    return Habbo.OnlineTime / 60;
                case 16:
                    return Habbo.GetSubscriptionManager().CalculateHCSubscription(Habbo);
                case 17:
                    return Habbo.RegistrationDuration;
                case 18:
                    return Habbo.RegularVisitor;
                case 19:
                    return Habbo.FootballGoalScorer;
                case 20:
                    return Habbo.FootballGoalHost;
                case 21:
                    return Habbo.TilesLocked;
                case 22:
                    return Habbo.StaffPicks;
            }
            return 0;
        }
	}
}
