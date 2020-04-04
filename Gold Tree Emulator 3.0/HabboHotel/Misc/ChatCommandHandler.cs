using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Achievements;
using GoldTree.HabboHotel.Users;
using GoldTree.Util;
using GoldTree.Messages;
using GoldTree.HabboHotel.Users.Authenticator;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
using System.Threading;
using GoldTree.HabboHotel.Pathfinding;
namespace GoldTree.HabboHotel.Misc
{
    internal sealed class ChatCommandHandler
    {
        private static List<string> list_0;
        private static List<string> list_1;
        private static List<int> list_2;
        private static List<string> list_3;
        public static void smethod_0(DatabaseClient class6_0)
        {
            Logging.Write("Loading Chat Filter..");
            ChatCommandHandler.list_0 = new List<string>();
            ChatCommandHandler.list_1 = new List<string>();
            ChatCommandHandler.list_2 = new List<int>();
            ChatCommandHandler.list_3 = new List<string>();
            ChatCommandHandler.InitWords(class6_0);
            Logging.WriteLine("completed!", ConsoleColor.Green);
        }
        public static void InitWords(DatabaseClient dbClient)
        {
            ChatCommandHandler.list_0.Clear();
            ChatCommandHandler.list_1.Clear();
            ChatCommandHandler.list_2.Clear();
            ChatCommandHandler.list_3.Clear();
            DataTable dataTable = dbClient.ReadDataTable("SELECT * FROM wordfilter ORDER BY word ASC;");
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ChatCommandHandler.list_0.Add(dataRow["word"].ToString());
                    ChatCommandHandler.list_1.Add(dataRow["replacement"].ToString());
                    ChatCommandHandler.list_2.Add(GoldTree.StringToInt(dataRow["strict"].ToString()));
                }
            }
            DataTable dataTable2 = dbClient.ReadDataTable("SELECT * FROM linkfilter;");
            if (dataTable2 != null)
            {
                foreach (DataRow dataRow in dataTable2.Rows)
                {
                    ChatCommandHandler.list_3.Add(dataRow["externalsite"].ToString());
                }
            }
        }
        public static bool InitLinks(string URLs)
        {
            if (ServerConfiguration.EnableExternalLinks == "disabled")
            {
                return false;
            }
            else
            {
                if ((URLs.StartsWith("http://") || URLs.StartsWith("www.") || URLs.StartsWith("https://")) && ChatCommandHandler.list_3 != null && ChatCommandHandler.list_3.Count > 0)
                {
                    foreach (string current in ChatCommandHandler.list_3)
                    {
                        if (URLs.Contains(current))
                        {
                            if (ServerConfiguration.EnableExternalLinks == "whitelist")
                            {
                                return true;
                            }
                            if (!(ServerConfiguration.EnableExternalLinks == "blacklist"))
                            {
                            }
                        }
                    }
                }
                return (URLs.StartsWith("http://") || URLs.StartsWith("www.") || (URLs.StartsWith("https://") && ServerConfiguration.EnableExternalLinks == "blacklist") || (ServerConfiguration.EnableExternalLinks == "whitelist" && false));
            }
        }
        public static string smethod_3(string string_0)
        {
            try
            {
            }
            catch
            {
            }
            return string_0;
        }
        public static string smethod_4(string string_0)
        {
            if (ChatCommandHandler.list_0 != null && ChatCommandHandler.list_0.Count > 0)
            {
                int num = -1;
                foreach (string current in ChatCommandHandler.list_0)
                {
                    num++;
                    if (string_0.ToLower().Contains(current.ToLower()) && ChatCommandHandler.list_2[num] == 1)
                    {
                        string_0 = Regex.Replace(string_0, current, ChatCommandHandler.list_1[num], RegexOptions.IgnoreCase);
                    }
                    else if (ChatCommandHandler.list_2[num] == 2)
                    {
                        string cheaters = @"\s*";
                        var re = new Regex(
                        @"\b("
                        + string.Join("|", list_0.Select(word =>
                            string.Join(cheaters, word.ToCharArray())))
                        + @")\b", RegexOptions.IgnoreCase);
                        return re.Replace(string_0, match =>
                        {
                            //return new string('*', match.Length);
                            return ChatCommandHandler.list_1[num];
                        });
                    }
                    else
                    {
                        if (string_0.ToLower().Contains(" " + current.ToLower() + " "))
                        {
                            string_0 = Regex.Replace(string_0, current, ChatCommandHandler.list_1[num], RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            return string_0;
        }

        public static bool smethod_5(GameClient Session, string Input)
        {
            string[] Params = Input.Split(new char[]
			{
				' '
			});
            GameClient TargetClient = null;
            Room class2 = Session.GetHabbo().CurrentRoom;
            if (!GoldTree.GetGame().GetRoleManager().dictionary_4.ContainsKey(Params[0]))
            {
                return false;
            }
            else
            {
                try
                {
                    int num;
                    if (class2 != null && class2.CheckRights(Session, true))
                    {
                        num = GoldTree.GetGame().GetRoleManager().dictionary_4[Params[0]];
                        if (num <= 33)
                        {
                            if (num == 8)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                if (class2.bool_5)
                                {
                                    class2.bool_5 = false;
                                }
                                else
                                {
                                    class2.bool_5 = true;
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (num == 33)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                if (class2 != null && class2.CheckRights(Session, true))
                                {
                                    List<RoomItem> list = class2.method_24(Session);
                                    Session.GetHabbo().GetInventoryComponent().method_17(list);
                                    Session.GetHabbo().GetInventoryComponent().method_9(true);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input + " " + Session.GetHabbo().CurrentRoomId);
                                    return true;
                                }
                                return false;
                            }
                        }
                        else
                        {
                            if (num == 46)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                try
                                {
                                    int num2 = int.Parse(Params[1]);
                                    if (Session.GetHabbo().Rank >= 6u)
                                    {
                                        class2.UsersMax = num2;
                                    }
                                    else
                                    {
                                        if (num2 > 100 || num2 < 5)
                                        {
                                            Session.SendNotification("ERROR: Use a number between 5 and 100");
                                        }
                                        else
                                        {
                                            class2.UsersMax = num2;
                                        }
                                    }
                                }
                                catch
                                {
                                    return false;
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (num == 53)
                            {
                                class2 = Session.GetHabbo().CurrentRoom;
                                GoldTree.GetGame().GetRoomManager().method_16(class2);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        }
                    }
                    switch (GoldTree.GetGame().GetRoleManager().dictionary_4[Params[0]])
                    {
                        case 2:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_alert"))
                                {
                                    return false;
                                }
                                string TargetUser = Params[1];
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(TargetUser);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Could not find user: " + TargetUser);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.SendNotification(ChatCommandHandler.MergeParams(Params, 2), 0);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 3:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_award"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Could not find user: " + text);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                GoldTree.GetGame().GetAchievementManager().addAchievement(TargetClient, Convert.ToUInt32(ChatCommandHandler.MergeParams(Params, 2)));
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 4:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_ban"))
                                {
                                    return false;
                                }
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("User not found.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().IsJuniori)
                                {
                                    Session.SendNotification("You are not allowed to ban that user.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }

                                string banlenght = "0";
                                try
                                {
                                    banlenght = Params[2];
                                }
                                catch (FormatException)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("OOPS! Something went wrong when trying format ban length! Report this and your date format!");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }

                                banlenght = banlenght.Replace("m", "");
                                banlenght = banlenght.Replace("h", "");
                                banlenght = banlenght.Replace("d", "");

                                Console.WriteLine(banlenght);
                                int num3 = 0;

                                try
                                {
                                    num3 = int.Parse(banlenght);
                                }
                                catch (FormatException)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("OOPS! Something went wrong when trying format ban length! Report this and your date format!");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }

                                if (Params[2].Contains("m"))
                                {
                                    num3 *= 60;
                                }

                                if (Params[2].Contains("h"))
                                {
                                    num3 *= 3600;
                                }

                                if (Params[2].Contains("d"))
                                {
                                    num3 *= 86400;
                                }

                                Console.WriteLine(num3);

                                int laskettupaivia = 0;
                                int laskettutunteja = 0;
                                int laskettuminuutteja = 0;
                                int laskettavaa = num3;

                                for (int i = 0; laskettavaa >= 86400; i++)
                                {
                                    laskettupaivia += 1;
                                    laskettavaa -= 86400;
                                }
                                for (int i = 0; laskettavaa >= 3600; i++)
                                {
                                    laskettutunteja += 1;
                                    laskettavaa -= 3600;
                                }
                                for (int i = 0; laskettavaa >= 60; i++)
                                {
                                    laskettuminuutteja += 1;
                                    laskettavaa -= 60;
                                }
                                if (num3 < 600)
                                {
                                    Session.SendNotification("Ban time is in seconds and must be at least 600 seconds (ten minutes). For more specific preset ban times, use the mod tool.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                Session.SendNotification("You banned user: " + TargetClient.GetHabbo().Username + " Ban lenght is: " + laskettupaivia + " day " + laskettutunteja + "  hour " + laskettuminuutteja + " minute " + laskettavaa + " second");
                                GoldTree.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, (double)num3, ChatCommandHandler.MergeParams(Params, 3), false);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 6:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_coins"))
                                {
                                    return false;
                                }
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("User could not be found.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                int num4;
                                if (int.TryParse(Params[2], out num4))
                                {
                                    long NoBug = 0;
                                    NoBug += TargetClient.GetHabbo().Credits;
                                    NoBug += num4;
                                    Console.WriteLine(NoBug);
                                    if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                                    {
                                        TargetClient.GetHabbo().Credits = TargetClient.GetHabbo().Credits + num4;
                                        TargetClient.GetHabbo().UpdateCredits(true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " has awarded you " + num4.ToString() + " credits!");
                                        Session.SendNotification("Credit balance updated successfully.");
                                        GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    }
                                    else
                                    {
                                        if (num4 > 0)
                                        {
                                            TargetClient.GetHabbo().Credits = 2147483647;
                                            TargetClient.GetHabbo().UpdateCredits(true);
                                            TargetClient.SendNotification("You just received max credits from staff!");
                                        }
                                        else if (num4 < 0)
                                        {
                                            TargetClient.GetHabbo().Credits = -2147483648;
                                            TargetClient.GetHabbo().UpdateCredits(true);
                                            TargetClient.SendNotification("You just received max negative credits from staff!");
                                        }
                                    }
                                    return true;
                                }
                                Session.SendNotification("Please send a valid amount of credits.");
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 7:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_coords"))
                                {
                                    return false;
                                }
                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    return false;
                                }
                                Session.SendNotification(string.Concat(new object[]
						{
							"X: ",
							class3.int_3,
							" - Y: ",
							class3.int_4,
							" - Z: ",
							class3.double_0,
							" - Rot: ",
							class3.int_8,
							", sqState: ",
							class2.Byte_0[class3.int_3, class3.int_4].ToString()
						}));
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 11:
                            if (Session.GetHabbo().HasFuse("cmd_enable"))
                            {
                                int int_ = int.Parse(Params[1]);
                                Session.GetHabbo().GetEffectsInventoryComponent().method_2(int_, true);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 14:
                            if (Session.GetHabbo().HasFuse("cmd_freeze"))
                            {
                                RoomUser class4 = Session.GetHabbo().CurrentRoom.method_56(Params[1]);
                                if (class4 != null)
                                {
                                    class4.bool_5 = !class4.bool_5;
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 15:
                            if (Session.GetHabbo().HasFuse("cmd_givebadge"))
                            {
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null)
                                {
                                    TargetClient.GetHabbo().GetBadgeComponent().SendBadge(TargetClient, GoldTree.FilterString(Params[2]), true);
                                }
                                else
                                {
                                    Session.SendNotification("User: " + Params[1] + " could not be found in the database.\rPlease try your request again.");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 16:
                            if (Session.GetHabbo().HasFuse("cmd_globalcredits"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    GoldTree.GetGame().GetClientManager().method_18(num5);
                                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                    {
                                        dbClient.ExecuteQuery("UPDATE users SET credits = credits + " + num5);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Input must be a number");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 17:
                            if (Session.GetHabbo().HasFuse("cmd_globalpixels"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    GoldTree.GetGame().GetClientManager().method_19(num5, false);
                                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                    {
                                        dbClient.ExecuteQuery("UPDATE users SET activity_points = activity_points + " + num5);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Input must be a number");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 18:
                            if (Session.GetHabbo().HasFuse("cmd_globalpoints"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    GoldTree.GetGame().GetClientManager().method_20(num5, false);
                                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                    {
                                        dbClient.ExecuteQuery("UPDATE users SET vip_points = vip_points + " + num5);
                                    }
                                }
                                catch
                                {
                                    Session.SendNotification("Input must be a number");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 19:
                            if (Session.GetHabbo().HasFuse("cmd_hal"))
                            {
                                string text2 = Params[1];
                                Input = Input.Substring(4).Replace(text2, "");
                                string text3 = Input.Substring(1);
                                ServerMessage Message = new ServerMessage(161u);
                                Message.AppendStringWithBreak(string.Concat(new string[]
							{
								GoldTreeEnvironment.GetExternalText("cmd_hal_title"),
								"\r\n",
								text3,
								"\r\n-",
								Session.GetHabbo().Username
							}));
                                Message.AppendStringWithBreak(text2);
                                GoldTree.GetGame().GetClientManager().BroadcastMessage(Message);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 20:
                            if (Session.GetHabbo().HasFuse("cmd_ha"))
                            {
                                string str = Input.Substring(3);
                                ServerMessage Message2 = new ServerMessage(808u);
                                Message2.AppendStringWithBreak(GoldTreeEnvironment.GetExternalText("cmd_ha_title"));
                                Message2.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
                                ServerMessage Message3 = new ServerMessage(161u);
                                Message3.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
                                GoldTree.GetGame().GetClientManager().method_15(Message2, Message3);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 21:
                            if (Session.GetHabbo().HasFuse("cmd_invisible"))
                            {
                                Session.GetHabbo().IsVisible = !Session.GetHabbo().IsVisible;
                                Session.SendNotification("You are now " + (Session.GetHabbo().IsVisible ? "visible" : "invisible") + "\nTo apply the changes reload the room ;D");
                                return true;
                            }
                            return false;
                        case 22:
                            if (!Session.GetHabbo().HasFuse("cmd_ipban"))
                            {
                                return false;
                            }
                            TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotification("User not found.");
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().IsJuniori)
                            {
                                Session.SendNotification("You are not allowed to ban that user.");
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            GoldTree.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), true);
                            GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                            return true;
                        case 23:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_kick"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Could not find user: " + text);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank && !Session.GetHabbo().IsJuniori)
                                {
                                    Session.SendNotification("You are not allowed to kick that user.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (TargetClient.GetHabbo().CurrentRoomId < 1u)
                                {
                                    Session.SendNotification("That user is not in a room and can not be kicked.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(TargetClient.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                class2.method_47(TargetClient, true, false);
                                if (Params.Length > 2)
                                {
                                    TargetClient.SendNotification("A moderator has kicked you from the room for the following reason: " + ChatCommandHandler.MergeParams(Params, 2));
                                }
                                else
                                {
                                    TargetClient.SendNotification("A moderator has kicked you from the room.");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 24:
                            if (Session.GetHabbo().HasFuse("cmd_massbadge"))
                            {
                                GoldTree.GetGame().GetClientManager().method_21(Params[1]);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 25:
                            if (Session.GetHabbo().HasFuse("cmd_masscredits"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    GoldTree.GetGame().GetClientManager().method_18(num5);
                                }
                                catch
                                {
                                    Session.SendNotification("Input must be a number");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 26:
                            if (Session.GetHabbo().HasFuse("cmd_masspixels"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    GoldTree.GetGame().GetClientManager().method_19(num5, true);
                                }
                                catch
                                {
                                    Session.SendNotification("Input must be a number");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 27:
                            if (Session.GetHabbo().HasFuse("cmd_masspoints"))
                            {
                                try
                                {
                                    int num5 = int.Parse(Params[1]);
                                    GoldTree.GetGame().GetClientManager().method_20(num5, true);
                                }
                                catch
                                {
                                    Session.SendNotification("Input must be a number");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 30:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_motd"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Could not find user: " + text);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.SendNotification(ChatCommandHandler.MergeParams(Params, 2), 2);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 31:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_mute"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null || TargetClient.GetHabbo() == null)
                                {
                                    Session.SendNotification("Could not find user: " + text);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().IsJuniori)
                                {
                                    Session.SendNotification("You are not allowed to (un)mute that user.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.GetHabbo().Mute();
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 32:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_override"))
                                {
                                    return false;
                                }
                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    return false;
                                }
                                if (class3.bool_1)
                                {
                                    class3.bool_1 = false;
                                    Session.SendNotification("Walking override disabled.");
                                }
                                else
                                {
                                    class3.bool_1 = true;
                                    Session.SendNotification("Walking override enabled.");
                                }
                                class2.method_22();
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 34:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_pixels"))
                                {
                                    return false;
                                }
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("User could not be found.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                int num4;
                                if (int.TryParse(Params[2], out num4))
                                {
                                    long NoBug = 0;
                                    NoBug += TargetClient.GetHabbo().ActivityPoints;
                                    NoBug += num4;
                                    if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                                    {
                                        TargetClient.GetHabbo().ActivityPoints = TargetClient.GetHabbo().ActivityPoints + num4;
                                        TargetClient.GetHabbo().UpdateActivityPoints(true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " has awarded you " + num4.ToString() + " Pixels!");
                                        Session.SendNotification("Pixels balance updated successfully.");
                                        GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    }
                                    else
                                    {
                                        if (num4 > 0)
                                        {
                                            TargetClient.GetHabbo().Credits = 2147483647;
                                            TargetClient.GetHabbo().UpdateCredits(true);
                                            TargetClient.SendNotification("You just received max pixels from staff!");
                                        }
                                        else if (num4 < 0)
                                        {
                                            TargetClient.GetHabbo().Credits = -2147483648;
                                            TargetClient.GetHabbo().UpdateCredits(true);
                                            TargetClient.SendNotification("You just received max negative pixels from staff!");
                                        }
                                    }
                                    return true;
                                }
                                Session.SendNotification("Please send a valid amount of pixels.");
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 35:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_points"))
                                {
                                    return false;
                                }
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("User could not be found.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                int num4;
                                if (int.TryParse(Params[2], out num4))
                                {
                                    long NoBug = 0;
                                    NoBug += TargetClient.GetHabbo().VipPoints;
                                    NoBug += num4;
                                    if (NoBug <= 2147483647 || -2147483648 >= NoBug)
                                    {
                                        TargetClient.GetHabbo().VipPoints = TargetClient.GetHabbo().VipPoints + num4;
                                        TargetClient.GetHabbo().UpdateVipPoints(false, true);
                                        TargetClient.SendNotification(Session.GetHabbo().Username + " has awarded you " + num4.ToString() + " Points!");
                                        Session.SendNotification("Points balance updated successfully.");
                                        GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    }
                                    else
                                    {
                                        if (num4 > 0)
                                        {
                                            TargetClient.GetHabbo().Credits = 2147483647;
                                            TargetClient.GetHabbo().UpdateCredits(true);
                                            TargetClient.SendNotification("You just received max points from staff!");
                                        }
                                        else if (num4 < 0)
                                        {
                                            TargetClient.GetHabbo().Credits = -2147483648;
                                            TargetClient.GetHabbo().UpdateCredits(true);
                                            TargetClient.SendNotification("You just received max negative points from staff!");
                                        }
                                    }
                                    return true;
                                }
                                Session.SendNotification("Please send a valid amount of points.");
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 39:
                            if (Session.GetHabbo().HasFuse("cmd_removebadge"))
                            {
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null)
                                {
                                    TargetClient.GetHabbo().GetBadgeComponent().RemoveBadge(GoldTree.FilterString(Params[2]));
                                }
                                else
                                {
                                    Session.SendNotification("User: " + Params[1] + " could not be found in the database.\rPlease try your request again.");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 41:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_roomalert"))
                                {
                                    return false;
                                }
                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                string string_ = ChatCommandHandler.MergeParams(Params, 1);
                                for (int i = 0; i < class2.RoomUsers.Length; i++)
                                {
                                    RoomUser class6 = class2.RoomUsers[i];
                                    if (class6 != null)
                                    {
                                        class6.GetClient().SendNotification(string_);
                                    }
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 42:
                            if (!Session.GetHabbo().HasFuse("cmd_roombadge"))
                            {
                                return false;
                            }
                            if (Session.GetHabbo().CurrentRoom == null)
                            {
                                return false;
                            }
                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                            {
                                try
                                {
                                    RoomUser class6 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                    if (class6 != null)
                                    {
                                        if (!class6.IsBot)
                                        {
                                            if (class6.GetClient() != null)
                                            {
                                                if (class6.GetClient().GetHabbo() != null)
                                                {
                                                    class6.GetClient().GetHabbo().GetBadgeComponent().SendBadge(class6.GetClient(), Params[1], true);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Session.SendNotification("Error: " + ex.ToString());
                                }
                            }
                            GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                            return true;
                        case 43:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_roomkick"))
                                {
                                    return false;
                                }
                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                bool flag = true;
                                string text4 = ChatCommandHandler.MergeParams(Params, 1);
                                if (text4.Length > 0)
                                {
                                    flag = false;
                                }
                                for (int i = 0; i < class2.RoomUsers.Length; i++)
                                {
                                    RoomUser class7 = class2.RoomUsers[i];
                                    if (class7 != null && class7.GetClient().GetHabbo().Rank < Session.GetHabbo().Rank)
                                    {
                                        if (!flag)
                                        {
                                            class7.GetClient().SendNotification("You have been kicked by an moderator: " + text4);
                                        }
                                        class2.method_47(class7.GetClient(), true, flag);
                                    }
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 44:
                            if (Session.GetHabbo().HasFuse("cmd_roommute"))
                            {
                                if (Session.GetHabbo().CurrentRoom.bool_4)
                                {
                                    Session.GetHabbo().CurrentRoom.bool_4 = false;
                                }
                                else
                                {
                                    Session.GetHabbo().CurrentRoom.bool_4 = true;
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 45:
                            if (Session.GetHabbo().HasFuse("cmd_sa"))
                            {
                                ServerMessage Logging = new ServerMessage(134u);
                                Logging.AppendUInt(0u);
                                Logging.AppendString(Session.GetHabbo().Username + ": " + Input.Substring(3));
                                GoldTree.GetGame().GetClientManager().method_16(Logging, Logging);
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 47:
                            if (Session.GetHabbo().HasFuse("cmd_setspeed"))
                            {
                                int.Parse(Params[1]);
                                Session.GetHabbo().CurrentRoom.method_102(int.Parse(Params[1]));
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 48:
                            if (Session.GetHabbo().HasFuse("cmd_shutdown"))
                            {
                                Logging.LogCriticalException("User " + Session.GetHabbo().Username + " shut down the server " + DateTime.Now.ToString());
                                Task task = new Task(new Action(GoldTree.Close));
                                task.Start();
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 49:
                            if (Session.GetHabbo().HasFuse("cmd_spull"))
                            {
                                try
                                {
                                    string a = "down";
                                    string text = Params[1];
                                    TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                    class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (Session == null || TargetClient == null)
                                    {
                                        return false;
                                    }
                                    RoomUser class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                    if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                    {
                                        Session.GetHabbo().Whisper("You cannot pull yourself");
                                        return true;
                                    }
                                    class6.HandleSpeech(Session, "*pulls " + TargetClient.GetHabbo().Username + " to them*", false);
                                    if (class6.int_8 == 0)
                                    {
                                        a = "up";
                                    }
                                    if (class6.int_8 == 2)
                                    {
                                        a = "right";
                                    }
                                    if (class6.int_8 == 4)
                                    {
                                        a = "down";
                                    }
                                    if (class6.int_8 == 6)
                                    {
                                        a = "left";
                                    }
                                    if (a == "up")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (!(class6.int_3 == class2.RoomModel.int_0 && class6.int_4 - 1 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                class4.MoveTo(class6.int_3, class6.int_4 - 1);
                                            else
                                                class4.MoveTo(class6.int_3, class6.int_4 + 1);
                                        }
                                        else
                                        {
                                            class4.MoveTo(class6.int_3, class6.int_4 - 1);
                                        }
                                    }
                                    if (a == "right")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (!(class6.int_3 + 1 == class2.RoomModel.int_0 && class6.int_4 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                class4.MoveTo(class6.int_3 + 1, class6.int_4);
                                            else
                                                class4.MoveTo(class6.int_3 - 1, class6.int_4);
                                        }
                                        else
                                        {
                                            class4.MoveTo(class6.int_3 + 1, class6.int_4);
                                        }
                                    }
                                    if (a == "down")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (!(class6.int_3 == class2.RoomModel.int_0 && class6.int_4 + 1 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                class4.MoveTo(class6.int_3, class6.int_4 + 1);
                                            else
                                                class4.MoveTo(class6.int_3, class6.int_4 - 1);
                                        }
                                        else
                                        {
                                            class4.MoveTo(class6.int_3, class6.int_4 + 1);
                                        }
                                    }
                                    if (a == "left")
                                    {
                                        if (ServerConfiguration.PreventDoorPush)
                                        {
                                            if (!(class6.int_3 - 1 == class2.RoomModel.int_0 && class6.int_4 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                class4.MoveTo(class6.int_3 - 1, class6.int_4);
                                            else
                                                class4.MoveTo(class6.int_3 + 1, class6.int_4);
                                        }
                                        else
                                        {
                                            class4.MoveTo(class6.int_3 - 1, class6.int_4);
                                        }
                                    }
                                    return true;
                                }
                                catch
                                {
                                    return false;
                                }
                            }
                            return false;
                        case 50:
                            if (Session.GetHabbo().HasFuse("cmd_summon"))
                            {
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                if (TargetClient != null && TargetClient.GetHabbo().CurrentRoom != Session.GetHabbo().CurrentRoom)
                                {
                                    ServerMessage Message5 = new ServerMessage(286u);
                                    Message5.AppendBoolean(Session.GetHabbo().CurrentRoom.IsPublic);
                                    Message5.AppendUInt(Session.GetHabbo().CurrentRoomId);
                                    TargetClient.SendMessage(Message5);
                                    TargetClient.SendNotification(Session.GetHabbo().Username + " has summoned you to them");
                                }
                                else
                                {
                                    Session.GetHabbo().Whisper("User: " + Params[1] + " could not be found - Maybe they're not online anymore :(");
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 51:
                            if (!Session.GetHabbo().HasFuse("cmd_superban"))
                            {
                                return false;
                            }
                            TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                            if (TargetClient == null)
                            {
                                Session.SendNotification("User not found.");
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank && !Session.GetHabbo().IsJuniori)
                            {
                                Session.SendNotification("You are not allowed to ban that user.");
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            GoldTree.GetGame().GetBanManager().BanUser(TargetClient, Session.GetHabbo().Username, 360000000.0, ChatCommandHandler.MergeParams(Params, 2), false);
                            GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                            return true;
                        case 52:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_teleport"))
                                {
                                    return false;
                                }
                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                if (class2 == null)
                                {
                                    return false;
                                }
                                RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                if (class3 == null)
                                {
                                    return false;
                                }
                                if (class3.TeleportMode)
                                {
                                    class3.TeleportMode = false;
                                    Session.SendNotification("Teleporting disabled.");
                                }
                                else
                                {
                                    class3.TeleportMode = true;
                                    Session.SendNotification("Teleporting enabled.");
                                }
                                class2.method_22();
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 54:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_unmute"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null || TargetClient.GetHabbo() == null)
                                {
                                    Session.SendNotification("Could not find user: " + text);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                TargetClient.GetHabbo().UnMute();
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 55:
                            if (Session.GetHabbo().HasFuse("cmd_update_achievements"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    AchievementManager.smethod_0(dbClient);
                                }
                                return true;
                            }
                            return false;
                        case 56:
                            if (Session.GetHabbo().HasFuse("cmd_update_bans"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    GoldTree.GetGame().GetBanManager().Initialise(dbClient);
                                }
                                GoldTree.GetGame().GetClientManager().method_28();
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 57:
                            if (Session.GetHabbo().HasFuse("cmd_update_bots"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    GoldTree.GetGame().GetBotManager().method_0(dbClient);
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 58:
                            if (Session.GetHabbo().HasFuse("cmd_update_catalogue"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    GoldTree.GetGame().GetCatalog().method_0(dbClient);
                                }
                                GoldTree.GetGame().GetCatalog().method_1();
                                GoldTree.GetGame().GetClientManager().BroadcastMessage(new ServerMessage(441u));
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 59:
                            if (Session.GetHabbo().HasFuse("cmd_update_filter"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    ChatCommandHandler.InitWords(dbClient);
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 60:
                            if (Session.GetHabbo().HasFuse("cmd_update_items"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    GoldTree.GetGame().GetItemManager().method_0(dbClient);
                                }
                                Session.SendNotification("Item defenitions reloaded successfully.");
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 61:
                            if (Session.GetHabbo().HasFuse("cmd_update_navigator"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    GoldTree.GetGame().GetNavigator().method_0(dbClient);
                                    GoldTree.GetGame().GetRoomManager().method_8(dbClient);
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 62:
                            if (Session.GetHabbo().HasFuse("cmd_update_permissions"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    GoldTree.GetGame().GetRoleManager().method_0(dbClient);
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 63:
                            if (Session.GetHabbo().HasFuse("cmd_update_settings"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    GoldTree.GetGame().LoadServerSettings(dbClient);
                                }
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                            return false;
                        case 64:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_userinfo"))
                                {
                                    return false;
                                }
                                string text5 = Params[1];
                                bool flag2 = true;
                                if (string.IsNullOrEmpty(text5))
                                {
                                    Session.SendNotification("Please enter a username");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                GameClient class8 = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text5);
                                Habbo class9;
                                if (class8 == null)
                                {
                                    flag2 = false;
                                    class9 = Authenticator.CreateHabbo(text5);
                                }
                                else
                                {
                                    class9 = class8.GetHabbo();
                                }
                                if (class9 == null)
                                {
                                    Session.SendNotification("Unable to find user " + Params[1]);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                StringBuilder stringBuilder = new StringBuilder();
                                if (class9.CurrentRoom != null)
                                {
                                    stringBuilder.Append(" - ROOM INFORMATION FOR ROOMID: " + class9.CurrentRoom.Id + " - \r");
                                    stringBuilder.Append("Owner: " + class9.CurrentRoom.Owner + "\r");
                                    stringBuilder.Append("Room name: " + class9.CurrentRoom.Name + "\r");
                                    stringBuilder.Append(string.Concat(new object[]
							{
								"Users in room: ",
								class9.CurrentRoom.UserCount,
								"/",
								class9.CurrentRoom.UsersMax
							}));
                                }
                                uint num6 = class9.Rank;
                                //if (class9.isAaronble)
                                //{
                                //	num6 = 1u;
                                //}
                                string text6 = "";
                                if (Session.GetHabbo().HasFuse("cmd_userinfo_viewip"))
                                {
                                    text6 = "UserIP: " + class9.LastIp + " \r";
                                }
                                Session.SendNotification(string.Concat(new object[]
						{
							"User information for user: ",
							text5,
							":\rRank: ",
							num6,
							" \rUser online: ",
							flag2.ToString(),
							" \rUserID: ",
							class9.Id,
							" \r",
							text6,
							"Visiting room: ",
							class9.CurrentRoomId,
							" \rUser motto: ",
							class9.Motto,
							" \rUser credits: ",
							class9.Credits,
							" \rUser pixels: ",
							class9.ActivityPoints,
							" \rUser points: ",
							class9.VipPoints,
							" \rUser muted: ",
							class9.IsMuted.ToString(),
							"\r\r\r",
							stringBuilder.ToString()
						}));
                                GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                return true;
                            }
                        case 65:
                            if (Session.GetHabbo().HasFuse("cmd_update_texts"))
                            {
                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                {
                                    GoldTreeEnvironment.LoadExternalTexts(dbClient);
                                }
                                return true;
                            }
                            return false;
                        case 66:
                            {
                                if (!Session.GetHabbo().HasFuse("cmd_disconnect"))
                                {
                                    return false;
                                }
                                string text = Params[1];
                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                if (TargetClient == null)
                                {
                                    Session.SendNotification("Could not find user: " + text);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (Session.GetHabbo().Rank <= TargetClient.GetHabbo().Rank && !Session.GetHabbo().IsJuniori)
                                {
                                    Session.SendNotification("You are not allowed to kick that user.");
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                if (!TargetClient.GetHabbo().IsJuniori)
                                {
                                    TargetClient.method_12();
                                }
                                return true;
                            }
                        case 87:
                            if (Session.GetHabbo().HasFuse("cmd_vipha"))
                            {
                                if (GoldTree.GetUnixTimestamp() - Session.GetHabbo().LastVipAlert >= ServerConfiguration.VIPHotelAlertInterval)
                                {
                                    Session.GetHabbo().LastVipAlert = GoldTree.GetUnixTimestamp();
                                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                    {
                                        dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                                        dbClient.ExecuteQuery("UPDATE users SET vipha_last = UNIX_TIMESTAMP() WHERE id = @sessionid");
                                    }
                                    string str = Input.Substring(6);
                                    ServerMessage Message2 = new ServerMessage(808u);
                                    Message2.AppendStringWithBreak(GoldTreeEnvironment.GetExternalText("cmd_vipha_title"));
                                    Message2.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
                                    ServerMessage Message3 = new ServerMessage(161u);
                                    Message3.AppendStringWithBreak(str + "\r\n- " + Session.GetHabbo().Username);
                                    GoldTree.GetGame().GetClientManager().method_15(Message2, Message3);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }
                                else
                                {
                                    Session.SendNotification("You need wait: " + (int)((Session.GetHabbo().LastVipAlert - GoldTree.GetUnixTimestamp() + ServerConfiguration.VIPHotelAlertInterval) / 60) + " minute!");
                                    return true;
                                }
                            }
                            return false;
                        case 91:
                            if (!Session.GetHabbo().HasFuse("cmd_roomeffect"))
                            {
                                return false;
                            }
                            if (Session.GetHabbo().CurrentRoom == null)
                            {
                                return false;
                            }
                            for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                            {
                                try
                                {
                                    RoomUser class6 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                    if (class6 != null)
                                    {
                                        if (!class6.IsBot)
                                        {
                                            if (class6.GetClient() != null)
                                            {
                                                if (class6.GetClient().GetHabbo() != null)
                                                {
                                                    int int_ = int.Parse(Params[1]);
                                                    class6.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(int_, true);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Session.SendNotification("Error: " + ex.ToString());
                                }
                            }
                            GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                            return true;
                        case 97:
                            if (Session.GetHabbo().HasFuse("cmd_viphal"))
                            {
                                if (GoldTree.GetUnixTimestamp() - Session.GetHabbo().LastVipAlertLink >= ServerConfiguration.VIPHotelAlertLinkInterval)
                                {
                                    Session.GetHabbo().LastVipAlertLink = GoldTree.GetUnixTimestamp();
                                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                    {
                                        dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                                        dbClient.ExecuteQuery("UPDATE users SET viphal_last = UNIX_TIMESTAMP() WHERE id = @sessionid");
                                    }

                                    string text2 = Params[1];
                                    Input = Input.Substring(4).Replace(text2, "");
                                    string text3 = Input.Substring(1);
                                    ServerMessage Message = new ServerMessage(161u);
                                    Message.AppendStringWithBreak(string.Concat(new string[]
							{
								GoldTreeEnvironment.GetExternalText("cmd_viphal_title"),
								"\r\n",
								text3,
								"\r\n-",
								Session.GetHabbo().Username
							}));
                                    Message.AppendStringWithBreak(text2);
                                    GoldTree.GetGame().GetClientManager().BroadcastMessage(Message);
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                }

                                else
                                {
                                    Session.SendNotification("You need wait: " + (int)((Session.GetHabbo().LastVipAlertLink - GoldTree.GetUnixTimestamp() + ServerConfiguration.VIPHotelAlertLinkInterval) / 60) + " minute!");
                                    return true;
                                }
                            }
                            return false;
                    }
                    num = GoldTree.GetGame().GetRoleManager().dictionary_4[Params[0]];
                    if (num <= 13)
                    {
                        if (num != 1)
                        {
                            switch (num)
                            {
                                case 5:
                                    {
                                        int num7 = (int)Convert.ToInt16(Params[1]);
                                        if (num7 > 0 && num7 < 101)
                                        {
                                            Session.GetHabbo().int_24 = (int)Convert.ToInt16(Params[1]);
                                        }
                                        else
                                        {
                                            Session.GetHabbo().Whisper("Please choose a value between 1 - 100");
                                        }
                                        return true;
                                    }
                                case 6:
                                case 7:
                                case 8:
                                case 11:
                                    goto IL_3F91;
                                case 9:
                                    Session.GetHabbo().GetInventoryComponent().ClearInventory();
                                    Session.SendNotification(GoldTreeEnvironment.GetExternalText("cmd_emptyitems_success"));
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                case 10:
                                    if (Session.GetHabbo().HasFuse("cmd_empty") && Params[1] != null)
                                    {
                                        GameClient class10 = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                        if (class10 != null && class10.GetHabbo() != null)
                                        {
                                            class10.GetHabbo().GetInventoryComponent().ClearInventory();
                                            Session.SendNotification("Inventory cleared! (Database and cache)");
                                        }
                                        else
                                        {
                                            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                            {
                                                dbClient.AddParamWithValue("usrname", Params[1]);
                                                int num8 = int.Parse(dbClient.ReadString("SELECT Id FROM users WHERE username = @usrname LIMIT 1;"));
                                                dbClient.ExecuteQuery("DELETE FROM items WHERE user_id = '" + num8 + "' AND room_id = 0;");
                                                Session.SendNotification("Inventory cleared! (Database)");
                                            }
                                        }
                                        GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                        return true;
                                    }
                                    return false;
                                case 12:
                                    {
                                        if (!(ServerConfiguration.UnknownBoolean3 || Session.GetHabbo().HasFuse("cmd_flagme")))
                                        {
                                            Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_disabled"));
                                            return true;
                                        }
                                        if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_flagme")))
                                        {
                                            Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_permission_vip"));
                                            return true;
                                        }
                                        ServerMessage Message5_ = new ServerMessage(573u);
                                        Session.SendMessage(Message5_);
                                        return true;
                                    }
                                case 13:
                                    if (!(ServerConfiguration.UnknownBoolean9 || Session.GetHabbo().HasFuse("cmd_follow")))
                                    {
                                        Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_disabled"));
                                        return true;
                                    }
                                    if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_follow")))
                                    {
                                        Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        return true;
                                    }
                                    TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                    if (TargetClient != null && TargetClient.GetHabbo().InRoom && Session.GetHabbo().CurrentRoom != TargetClient.GetHabbo().CurrentRoom && !TargetClient.GetHabbo().HideInRom)
                                    {
                                        ServerMessage Message5 = new ServerMessage(286u);
                                        Message5.AppendBoolean(TargetClient.GetHabbo().CurrentRoom.IsPublic);
                                        Message5.AppendUInt(TargetClient.GetHabbo().CurrentRoomId);
                                        Session.SendMessage(Message5);
                                    }
                                    else
                                    {
                                        Session.GetHabbo().Whisper("User: " + Params[1] + " could not be found - Maybe they're not online or not in a room anymore (or maybe they're a ninja)");
                                    }
                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                    return true;
                                default:
                                    goto IL_3F91;
                            }
                        }
                    }
                    else
                    {
                        switch (num)
                        {
                            case 28:
                                {
                                    if (!(ServerConfiguration.UnknownBoolean7 || Session.GetHabbo().HasFuse("cmd_mimic")))
                                    {
                                        Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_disabled"));
                                        return true;
                                    }
                                    if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_mimic")))
                                    {
                                        Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        return true;
                                    }
                                    string text = Params[1];
                                    TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                    if (TargetClient == null)
                                    {
                                        Session.GetHabbo().Whisper("Could not find user: " + text);
                                        return true;
                                    }
                                    Session.GetHabbo().Figure = TargetClient.GetHabbo().Figure;
                                    Session.GetHabbo().UpdateLook(false, Session);
                                    return true;
                                }
                            case 29:
                                {
                                    if (!(ServerConfiguration.UnknownBoolean8 || Session.GetHabbo().HasFuse("cmd_moonwalk")))
                                    {
                                        Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_disabled"));
                                        return true;
                                    }
                                    if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_moonwalk")))
                                    {
                                        Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_permission_vip"));
                                        return true;
                                    }
                                    class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    if (class2 == null)
                                    {
                                        return false;
                                    }
                                    RoomUser class3 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    if (class3 == null)
                                    {
                                        return false;
                                    }
                                    if (class3.bool_3)
                                    {
                                        class3.bool_3 = false;
                                        Session.GetHabbo().Whisper("Your moonwalk has been disabled.");
                                        return true;
                                    }
                                    class3.bool_3 = true;
                                    Session.GetHabbo().Whisper("Your moonwalk has been enabled.");
                                    return true;
                                }
                            default:
                                {
                                    RoomUser class6;
                                    switch (num)
                                    {
                                        case 36:
                                            try
                                            {
                                                if (!(ServerConfiguration.UnknownBoolean2 || Session.GetHabbo().HasFuse("cmd_pull")))
                                                {
                                                    Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_disabled"));
                                                    return true;
                                                }
                                                if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_pull")))
                                                {
                                                    Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_permission_vip"));
                                                    return true;
                                                }
                                                string a = "down";
                                                string text = Params[1];
                                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                if (Session == null || TargetClient == null)
                                                {
                                                    return false;
                                                }
                                                class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                {
                                                    Session.GetHabbo().Whisper("You cannot pull yourself");
                                                    return true;
                                                }
                                                if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(class6.int_3 - class4.int_3) < 3 && Math.Abs(class6.int_4 - class4.int_4) < 3)
                                                {
                                                    class6.HandleSpeech(Session, "*pulls " + TargetClient.GetHabbo().Username + " to them*", false);
                                                    if (class6.int_8 == 0)
                                                    {
                                                        a = "up";
                                                    }
                                                    if (class6.int_8 == 2)
                                                    {
                                                        a = "right";
                                                    }
                                                    if (class6.int_8 == 4)
                                                    {
                                                        a = "down";
                                                    }
                                                    if (class6.int_8 == 6)
                                                    {
                                                        a = "left";
                                                    }
                                                    if (a == "up")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (!(class6.int_3 == class2.RoomModel.int_0 && class6.int_4 - 1 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                class4.MoveTo(class6.int_3, class6.int_4 - 1);
                                                            else
                                                                class4.MoveTo(class6.int_3, class6.int_4 + 1);
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class6.int_3, class6.int_4 - 1);
                                                        }
                                                    }
                                                    if (a == "right")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (!(class6.int_3 + 1 == class2.RoomModel.int_0 && class6.int_4 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                class4.MoveTo(class6.int_3 + 1, class6.int_4);
                                                            else
                                                                class4.MoveTo(class6.int_3 - 1, class6.int_4);
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class6.int_3 + 1, class6.int_4);
                                                        }
                                                    }
                                                    if (a == "down")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (!(class6.int_3 == class2.RoomModel.int_0 && class6.int_4 + 1 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                class4.MoveTo(class6.int_3, class6.int_4 + 1);
                                                            else
                                                                class4.MoveTo(class6.int_3, class6.int_4 - 1);
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class6.int_3, class6.int_4 + 1);
                                                        }
                                                    }
                                                    if (a == "left")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (!(class6.int_3 - 1 == class2.RoomModel.int_0 && class6.int_4 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                                class4.MoveTo(class6.int_3 - 1, class6.int_4);
                                                            else
                                                                class4.MoveTo(class6.int_3 + 1, class6.int_4);
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class6.int_3 - 1, class6.int_4);
                                                        }
                                                    }
                                                    return true;
                                                }
                                                Session.GetHabbo().Whisper("That user is not close enough to you to be pulled, try getting closer");
                                                return true;
                                            }
                                            catch
                                            {
                                                return false;
                                            }
                                        case 37:
                                            break;
                                        case 38:
                                            goto IL_3F03;
                                        case 39:
                                            goto IL_3F91;
                                        case 40:
                                            {
                                                string text = Params[1];
                                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                RoomUser class4 = class2.method_57(text);
                                                if (class6.class34_1 != null)
                                                {
                                                    Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_ride_err_riding"));
                                                    return true;
                                                }
                                                if (!class4.IsBot || class4.PetData.Type != 13u)
                                                {
                                                    Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_ride_err_nothorse"));
                                                    return true;
                                                }
                                                bool arg_40EB_0;
                                                if ((class6.int_3 + 1 != class4.int_3 || class6.int_4 != class4.int_4) && (class6.int_3 - 1 != class4.int_3 || class6.int_4 != class4.int_4) && (class6.int_4 + 1 != class4.int_4 || class6.int_3 != class4.int_3))
                                                {
                                                    if (class6.int_4 - 1 == class4.int_4)
                                                    {
                                                        if (class6.int_3 == class4.int_3)
                                                        {
                                                            goto IL_40C2;
                                                        }
                                                    }
                                                    arg_40EB_0 = (class6.int_3 != class4.int_3 || class6.int_4 != class4.int_4);
                                                    goto IL_40EB;
                                                }
                                            IL_40C2:
                                                arg_40EB_0 = false;
                                            IL_40EB:
                                                if (arg_40EB_0)
                                                {
                                                    Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_ride_err_toofar"));
                                                    return true;
                                                }
                                                if (class4.RoomBot.RoomUser_0 == null)
                                                {
                                                    class4.RoomBot.RoomUser_0 = class6;
                                                    class6.class34_1 = class4.RoomBot;
                                                    class6.int_3 = class4.int_3;
                                                    class6.int_4 = class4.int_4;
                                                    class6.double_0 = class4.double_0 + 1.0;
                                                    class6.int_8 = class4.int_8;
                                                    class6.int_7 = class4.int_7;
                                                    class6.UpdateNeeded = true;
                                                    class2.method_87(class6, false, false);
                                                    class6.RoomUser_0 = class4;
                                                    class6.Statusses.Clear();
                                                    class4.Statusses.Clear();
                                                    Session.GetHabbo().GetEffectsInventoryComponent().method_2(77, true);
                                                    Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_ride_instr_getoff"));
                                                    class2.method_22();
                                                    return true;
                                                }
                                                Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_ride_err_tooslow"));
                                                return true;
                                            }
                                        case 88:
                                            try
                                            {
                                                if (!Session.GetHabbo().HasFuse("cmd_spush"))
                                                {
                                                    return false;
                                                }
                                                string a = "down";
                                                string text = Params[1];
                                                TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                                class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                if (Session == null || TargetClient == null)
                                                {
                                                    return false;
                                                }
                                                class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                {
                                                    Session.GetHabbo().Whisper("It can't be that bad mate, no need to push yourself!");
                                                    return true;
                                                }
                                                bool arg_3DD2_0;
                                                if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                                {
                                                    if ((class6.int_3 + 1 != class4.int_3 || class6.int_4 != class4.int_4) && (class6.int_3 - 1 != class4.int_3 || class6.int_4 != class4.int_4) && (class6.int_4 + 1 != class4.int_4 || class6.int_3 != class4.int_3))
                                                    {
                                                        if (class6.int_4 - 1 == class4.int_4)
                                                        {
                                                            if (class6.int_3 == class4.int_3)
                                                            {
                                                                goto IL_3DA6;
                                                            }
                                                        }
                                                        arg_3DD2_0 = (class6.int_3 != class4.int_3 || class6.int_4 != class4.int_4);
                                                        goto IL_3DD2;
                                                    }
                                                IL_3DA6:
                                                    arg_3DD2_0 = false;
                                                }
                                                else
                                                {
                                                    arg_3DD2_0 = true;
                                                }
                                            IL_3DD2:
                                                if (!arg_3DD2_0)
                                                {
                                                    class6.HandleSpeech(Session, "*pushes " + TargetClient.GetHabbo().Username + "*", false);
                                                    if (class6.int_8 == 0)
                                                    {
                                                        a = "up";
                                                    }
                                                    if (class6.int_8 == 2)
                                                    {
                                                        a = "right";
                                                    }
                                                    if (class6.int_8 == 4)
                                                    {
                                                        a = "down";
                                                    }
                                                    if (class6.int_8 == 6)
                                                    {
                                                        a = "left";
                                                    }
                                                    if (a == "up")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class4.int_3, class4.int_4 - 1);
                                                                class4.MoveTo(class4.int_3, class4.int_4 - 2);
                                                                class4.MoveTo(class4.int_3, class4.int_4 - 3);
                                                                class4.MoveTo(class4.int_3, class4.int_4 - 4);
                                                                class4.MoveTo(class4.int_3, class4.int_4 - 5);
                                                            }
                                                            else
                                                            {
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 - 1 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 - 1);
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 - 2 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 - 2);
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 - 3 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 - 3);
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 - 4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 - 4);
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 - 5 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 - 5);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class4.int_3, class4.int_4 - 1);
                                                            class4.MoveTo(class4.int_3, class4.int_4 - 2);
                                                            class4.MoveTo(class4.int_3, class4.int_4 - 3);
                                                            class4.MoveTo(class4.int_3, class4.int_4 - 4);
                                                            class4.MoveTo(class4.int_3, class4.int_4 - 5);
                                                        }
                                                    }
                                                    if (a == "right")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class4.int_3 + 1, class4.int_4);
                                                                class4.MoveTo(class4.int_3 + 2, class4.int_4);
                                                                class4.MoveTo(class4.int_3 + 3, class4.int_4);
                                                                class4.MoveTo(class4.int_3 + 4, class4.int_4);
                                                                class4.MoveTo(class4.int_3 + 5, class4.int_4);
                                                            }
                                                            else
                                                            {
                                                                if (!(class4.int_3 + 1 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 + 1, class4.int_4);
                                                                if (!(class4.int_3 + 2 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 + 2, class4.int_4);
                                                                if (!(class4.int_3 + 3 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 + 3, class4.int_4);
                                                                if (!(class4.int_3 + 4 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 + 4, class4.int_4);
                                                                if (!(class4.int_3 + 5 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 + 5, class4.int_4);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class4.int_3 + 1, class4.int_4);
                                                            class4.MoveTo(class4.int_3 + 2, class4.int_4);
                                                            class4.MoveTo(class4.int_3 + 3, class4.int_4);
                                                            class4.MoveTo(class4.int_3 + 4, class4.int_4);
                                                            class4.MoveTo(class4.int_3 + 5, class4.int_4);
                                                        }
                                                    }
                                                    if (a == "down")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class4.int_3, class4.int_4 + 1);
                                                                class4.MoveTo(class4.int_3, class4.int_4 + 2);
                                                                class4.MoveTo(class4.int_3, class4.int_4 + 3);
                                                                class4.MoveTo(class4.int_3, class4.int_4 + 4);
                                                                class4.MoveTo(class4.int_3, class4.int_4 + 5);
                                                            }
                                                            else
                                                            {
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 + 1 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 + 1);
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 + 2 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 + 2);
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 + 3 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 + 3);
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 + 4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 + 4);
                                                                if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 + 5 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3, class4.int_4 + 5);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class4.int_3, class4.int_4 + 1);
                                                            class4.MoveTo(class4.int_3, class4.int_4 + 2);
                                                            class4.MoveTo(class4.int_3, class4.int_4 + 3);
                                                            class4.MoveTo(class4.int_3, class4.int_4 + 4);
                                                            class4.MoveTo(class4.int_3, class4.int_4 + 5);
                                                        }
                                                    }
                                                    if (a == "left")
                                                    {
                                                        if (ServerConfiguration.PreventDoorPush)
                                                        {
                                                            if (Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                            {
                                                                class4.MoveTo(class4.int_3 - 1, class4.int_4);
                                                                class4.MoveTo(class4.int_3 - 2, class4.int_4);
                                                                class4.MoveTo(class4.int_3 - 3, class4.int_4);
                                                                class4.MoveTo(class4.int_3 - 4, class4.int_4);
                                                                class4.MoveTo(class4.int_3 - 5, class4.int_4);
                                                            }
                                                            else
                                                            {
                                                                if (!(class4.int_3 - 1 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 - 1, class4.int_4);
                                                                if (!(class4.int_3 - 2 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 - 2, class4.int_4);
                                                                if (!(class4.int_3 - 3 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 - 3, class4.int_4);
                                                                if (!(class4.int_3 - 4 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 - 4, class4.int_4);
                                                                if (!(class4.int_3 - 5 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1))
                                                                    class4.MoveTo(class4.int_3 - 5, class4.int_4);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            class4.MoveTo(class4.int_3 - 1, class4.int_4);
                                                            class4.MoveTo(class4.int_3 - 2, class4.int_4);
                                                            class4.MoveTo(class4.int_3 - 3, class4.int_4);
                                                            class4.MoveTo(class4.int_3 - 4, class4.int_4);
                                                            class4.MoveTo(class4.int_3 - 5, class4.int_4);
                                                        }
                                                    }
                                                }
                                                return true;
                                            }
                                            catch
                                            {
                                                return false;
                                            }
                                        default:
                                            switch (num)
                                            {
                                                case 67:
                                                    {
                                                        string text7 = "Your Commands:\r\r";
                                                        if (Session.GetHabbo().HasFuse("cmd_update_settings"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_settings_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_bans"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_bans_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_permissions"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_permissions_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_filter"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_filter_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_bots"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_bots_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_catalogue"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_catalogue_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_items"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_items_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_navigator"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_navigator_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_achievements"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_achievements_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_award"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_award_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_coords"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_coords_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_override"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_override_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_teleport"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_teleport_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_coins"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_coins_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_pixels"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_pixels_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_points"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_points_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_alert"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_alert_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_motd"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_motd_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roomalert"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_roomalert_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_ha"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_ha_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_hal"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_hal_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_freeze"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_freeze_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_enable"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_enable_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roommute"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_roommute_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_setspeed"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_setspeed_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_globalcredits"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_globalcredits_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_globalpixels"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_globalpixels_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_globalpoints"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_globalpoints_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_masscredits"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_masscredits_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_masspixels"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_masspixels_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_masspoints"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_masspoints_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_givebadge"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_givebadge_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_removebadge"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_removebadge_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_summon"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_summon_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roombadge"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_roombadge_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_massbadge"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_massbadge_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_userinfo"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_userinfo_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_shutdown"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_shutdown_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_invisible"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_invisible_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_ban"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_ban_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_superban"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_superban_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_ipban"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_ipban_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_kick"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_kick_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roomkick"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_roomkick_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_mute"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_mute_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_unmute"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_unmute_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_sa"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_sa_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_spull"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_spull_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_empty"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_empty_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_update_texts"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_update_texts_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_dance"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_dance_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_rave"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_rave_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roll"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_roll_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_control"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_control_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_makesay"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_makesay_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_sitdown"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_sitdown_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_lay"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_lay_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_startquestion"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_startquestion_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_handitem"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_handitem_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_vipha"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_vipha_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_spush"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_spush_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().HasFuse("cmd_roomeffect"))
                                                        {
                                                            text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_roomeffect_desc") + "\r\r";
                                                        }
                                                        if (Session.GetHabbo().IsVIP)
                                                        {
                                                            if (ServerConfiguration.UnknownBoolean8 || Session.GetHabbo().HasFuse("cmd_moonwalk"))
                                                            {
                                                                text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_moonwalk_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean7 || Session.GetHabbo().HasFuse("cmd_mimi"))
                                                            {
                                                                text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_mimic_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean9 || Session.GetHabbo().HasFuse("cmd_follow"))
                                                            {
                                                                text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_follow_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean1 || Session.GetHabbo().HasFuse("cmd_push"))
                                                            {
                                                                text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_push_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean2 || Session.GetHabbo().HasFuse("cmd_pull"))
                                                            {
                                                                text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_pull_desc") + "\r\r";
                                                            }
                                                            if (ServerConfiguration.UnknownBoolean3 || Session.GetHabbo().HasFuse("cmd_flagme"))
                                                            {
                                                                text7 = text7 + GoldTreeEnvironment.GetExternalText("cmd_flagme_desc") + "\r\r";
                                                            }
                                                        }
                                                        string text8 = "";
                                                        if (ServerConfiguration.EnableRedeemCredits)
                                                        {
                                                            text8 = text8 + GoldTreeEnvironment.GetExternalText("cmd_redeemcreds_desc") + "\r\r";
                                                        }
                                                        string text9 = "";
                                                        if (ServerConfiguration.EnableRedeemPixels)
                                                        {
                                                            text9 = text9 + GoldTreeEnvironment.GetExternalText("cmd_redeempixel_desc") + "\r\r";
                                                        }
                                                        string redeemshell = "";
                                                        if (ServerConfiguration.EnableRedeemShells)
                                                        {
                                                            redeemshell = redeemshell + GoldTreeEnvironment.GetExternalText("cmd_redeemshell_desc") + "\r\r";
                                                        }
                                                        string text11 = text7;
                                                        text7 = string.Concat(new string[]
									{
										text11,
										"- - - - - - - - - - - \r\r",
										GoldTreeEnvironment.GetExternalText("cmd_about_desc"),
										"\r\r",
										GoldTreeEnvironment.GetExternalText("cmd_pickall_desc"),
										"\r\r",
										GoldTreeEnvironment.GetExternalText("cmd_unload_desc"),
										"\r\r",
										GoldTreeEnvironment.GetExternalText("cmd_disablediagonal_desc"),
										"\r\r",
										GoldTreeEnvironment.GetExternalText("cmd_setmax_desc"),
										"\r\r",
										text8,
                                        text9,
                                        redeemshell,
										GoldTreeEnvironment.GetExternalText("cmd_ride_desc"),
										"\r\r",
										GoldTreeEnvironment.GetExternalText("cmd_buy_desc"),
										"\r\r",
										GoldTreeEnvironment.GetExternalText("cmd_emptypets_desc"),
										"\r\r",
										GoldTreeEnvironment.GetExternalText("cmd_emptyitems_desc")
									});
                                                        Session.SendNotification(text7, 2);
                                                        return true;
                                                    }
                                                case 68:
                                                    goto IL_2F05;
                                                case 69:
                                                    {
                                                        StringBuilder stringBuilder2 = new StringBuilder();
                                                        for (int i = 0; i < Session.GetHabbo().CurrentRoom.RoomUsers.Length; i++)
                                                        {
                                                            class6 = Session.GetHabbo().CurrentRoom.RoomUsers[i];
                                                            if (class6 != null)
                                                            {
                                                                stringBuilder2.Append(string.Concat(new object[]
											{
												"UserID: ",
												class6.UId,
												" RoomUID: ",
												class6.int_20,
												" VirtualID: ",
												class6.VirtualId,
												" IsBot:",
												class6.IsBot.ToString(),
												" X: ",
												class6.int_3,
												" Y: ",
												class6.int_4,
												" Z: ",
												class6.double_0,
												" \r\r"
											}));
                                                            }
                                                        }
                                                        Session.SendNotification(stringBuilder2.ToString());
                                                        Session.SendNotification("RoomID: " + Session.GetHabbo().CurrentRoomId);
                                                        return true;
                                                    }
                                                case 70:
                                                    {
                                                        return false;
                                                    }
                                                case 71:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_dance"))
                                                    {
                                                        class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        GameClient class10 = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                        RoomUser class3 = class2.GetRoomUserByHabbo(class10.GetHabbo().Id);
                                                        class3.DanceId = 1;
                                                        ServerMessage Message6 = new ServerMessage(480u);
                                                        Message6.AppendInt32(class3.VirtualId);
                                                        Message6.AppendInt32(1);
                                                        class2.SendMessage(Message6, null);
                                                        return true;
                                                    }
                                                    return false;
                                                case 72:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_rave"))
                                                    {
                                                        class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        class2.Rave();
                                                        return true;
                                                    }
                                                    return false;
                                                case 73:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_roll"))
                                                    {
                                                        GameClient class10 = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Params[1]);
                                                        class10.GetHabbo().int_1 = (int)Convert.ToInt16(Params[2]);
                                                        return true;
                                                    }
                                                    return false;
                                                case 74:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_control"))
                                                    {
                                                        string text = Params[1];
                                                        try
                                                        {
                                                            TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                                            class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (Session == null || TargetClient == null)
                                                            {
                                                                return false;
                                                            }
                                                            RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                            class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class6.RoomUser_0 = class4;
                                                        }
                                                        catch
                                                        {
                                                            class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (Session == null || TargetClient == null)
                                                            {
                                                                return false;
                                                            }
                                                            class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                            class6.RoomUser_0 = null;
                                                        }
                                                        return true;
                                                    }
                                                    return false;
                                                case 75:
                                                    {
                                                        if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_makesay"))
                                                        {
                                                            string text2 = Params[1];
                                                            TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text2);
                                                            class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            if (Session == null || TargetClient == null)
                                                            {
                                                                return false;
                                                            }
                                                            RoomUser roomUser = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                            roomUser.HandleSpeech(TargetClient, Input.Substring(9 + text2.Length), false);
                                                            return true;
                                                        }
                                                        return false;
                                                    }
                                                case 76:
                                                    if (Session.GetHabbo().IsJuniori || Session.GetHabbo().HasFuse("cmd_sitdown"))
                                                    {
                                                        class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        class2.method_55();
                                                        return true;
                                                    }
                                                    return false;
                                                case 77:
                                                    {
                                                        return false;
                                                    }
                                                case 78:
                                                    goto IL_3F91;
                                                case 79:
                                                    {
                                                        if (!Session.GetHabbo().InRoom)
                                                        {
                                                            return false;
                                                        }
                                                        class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                        int int_2 = class2.method_56(Session.GetHabbo().Username).CarryItemID;
                                                        if (int_2 <= 0)
                                                        {
                                                            Session.GetHabbo().Whisper("You're not holding anything, pick something up first!");
                                                            return true;
                                                        }
                                                        string text = Params[1];
                                                        TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                                        class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                                        if (Session == null || TargetClient == null)
                                                        {
                                                            return false;
                                                        }
                                                        if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                                        {
                                                            return true;
                                                        }
                                                        if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && Math.Abs(class6.int_3 - class4.int_3) < 3 && Math.Abs(class6.int_4 - class4.int_4) < 3)
                                                        {
                                                            try
                                                            {
                                                                class2.method_56(Params[1]).CarryItem(int_2);
                                                                class2.method_56(Session.GetHabbo().Username).CarryItem(0);
                                                            }
                                                            catch
                                                            {
                                                            }
                                                            return true;
                                                        }
                                                        Session.GetHabbo().Whisper("You are too far away from " + Params[1] + ", try getting closer");
                                                        return true;
                                                    }
                                                case 80:
                                                    if (!Session.GetHabbo().InRoom)
                                                    {
                                                        return false;
                                                    }
                                                    class6 = Session.GetHabbo().CurrentRoom.method_56(Session.GetHabbo().Username);
                                                    if (class6.Statusses.ContainsKey("sit") || class6.Statusses.ContainsKey("lay") || class6.int_8 == 1 || class6.int_8 == 3 || class6.int_8 == 5 || class6.int_8 == 7)
                                                    {
                                                        return true;
                                                    }
                                                    if (class6.byte_1 > 0 || class6.class34_1 != null)
                                                    {
                                                        return true;
                                                    }
                                                    class6.AddStatus("sit", ((class6.double_0 + 1.0) / 2.0 - class6.double_0 * 0.5).ToString().Replace(",", "."));
                                                    class6.UpdateNeeded = true;
                                                    return true;
                                                case 81:
                                                case 82:
                                                    class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                    if (class6.class34_1 != null)
                                                    {
                                                        Session.GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                                        class6.class34_1.RoomUser_0 = null;
                                                        class6.class34_1 = null;
                                                        class6.double_0 -= 1.0;
                                                        class6.Statusses.Clear();
                                                        class6.UpdateNeeded = true;
                                                        int int_3 = GoldTree.smethod_5(0, class2.RoomModel.int_4);
                                                        int int_4 = GoldTree.smethod_5(0, class2.RoomModel.int_5);
                                                        class6.RoomUser_0.MoveTo(int_3, int_4);
                                                        class6.RoomUser_0 = null;
                                                        class2.method_87(class6, false, false);
                                                    }
                                                    return true;
                                                case 83:
                                                    Session.GetHabbo().GetInventoryComponent().RemovePetsFromInventory();
                                                    Session.SendNotification(GoldTreeEnvironment.GetExternalText("cmd_emptypets_success"));
                                                    GoldTree.GetGame().GetClientManager().method_31(Session, Params[0].ToLower(), Input);
                                                    return true;
                                                case 85:
                                                    if (!Session.GetHabbo().HasFuse("cmd_handitem"))
                                                    {
                                                        return false;
                                                    }
                                                    class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    if (class2 == null)
                                                    {
                                                        return false;
                                                    }
                                                    class2.method_56(Session.GetHabbo().Username).CarryItem(int.Parse(Params[1]));
                                                    return true;
                                                case 86:
                                                    {
                                                        if (!Session.GetHabbo().HasFuse("cmd_lay"))
                                                        {
                                                            return false;
                                                        }
                                                        Room currentRoom = Session.GetHabbo().CurrentRoom;
                                                        if (currentRoom == null)
                                                        {
                                                            return false;
                                                        }
                                                        RoomUser roomUserByHabbo2 = currentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                        if (roomUserByHabbo2 == null)
                                                        {
                                                            return false;
                                                        }
                                                        if (!roomUserByHabbo2.Statusses.ContainsKey("lay"))
                                                        {
                                                            if (roomUserByHabbo2.int_8 % 2 == 0)
                                                            {
                                                                roomUserByHabbo2.Statusses.Add("lay", Convert.ToString((double)Session.GetHabbo().CurrentRoom.Byte_0[roomUserByHabbo2.int_3, roomUserByHabbo2.int_4] + 0.55).ToString().Replace(",", "."));
                                                                roomUserByHabbo2.UpdateNeeded = true;
                                                            }
                                                            else
                                                            {
                                                                Session.GetHabbo().Whisper("You cant lay if you are diagonal!");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            roomUserByHabbo2.Statusses.Remove("lay");
                                                            roomUserByHabbo2.UpdateNeeded = true;
                                                        }
                                                        return true;
                                                    }
                                                /*case 93:
                                                       // V3.11.0
                                                       // R63
                                                       // OS: Any?
                                                       // PURPOSE: Run retros
                                                       // XD
                                                   return true;*/
                                                case 94:
                                                    if (Session.GetHabbo().HasFuse("cmd_startquestion"))
                                                    {
                                                        if (Params[1] != null)
                                                        {
                                                            Room Room = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                            DataTable Data = null;
                                                            int QuestionId = int.Parse(Params[1]);
                                                            Room.CurrentPollId = QuestionId;
                                                            string Question;

                                                            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                                            {
                                                                Question = dbClient.ReadString("SELECT question FROM infobus_questions WHERE id = '" + QuestionId + "' LIMIT 1");
                                                                Data = dbClient.ReadDataTable("SELECT * FROM infobus_answers WHERE question_id = '" + QuestionId + "'");
                                                            }

                                                            ServerMessage InfobusQuestion = new ServerMessage(79);
                                                            InfobusQuestion.AppendStringWithBreak(Question);
                                                            InfobusQuestion.AppendInt32(Data.Rows.Count);
                                                            if (Data != null)
                                                            {
                                                                foreach (DataRow Row in Data.Rows)
                                                                {
                                                                    InfobusQuestion.AppendInt32((int)Row["id"]);
                                                                    InfobusQuestion.AppendStringWithBreak((string)Row["answer_text"]);
                                                                }
                                                            }
                                                            Room.SendMessage(InfobusQuestion, null);

                                                            Thread Infobus = new Thread(delegate() { Room.ShowResults(Room, QuestionId, Session); });
                                                            Infobus.Start();
                                                            return true;
                                                        }
                                                    }
                                                    return false;
                                                case 95:
                                                    class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                    if (class6.Boolean_3)
                                                    {
                                                        Session.GetHabbo().Whisper("Command unavailable while trading!");
                                                        return true;
                                                    }
                                                    if (ServerConfiguration.EnableRedeemPixels)
                                                    {
                                                        Session.GetHabbo().GetInventoryComponent().RedeemPixel(Session);
                                                    }
                                                    else
                                                    {
                                                        Session.GetHabbo().Whisper(GoldTree.smethod_1("cmd_error_disabled"));
                                                    }
                                                    return true;
                                                case 96:
                                                    class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                                    if (class6.Boolean_3)
                                                    {
                                                        Session.GetHabbo().Whisper("Command unavailable while trading!");
                                                        return true;
                                                    }
                                                    if (ServerConfiguration.EnableRedeemShells)
                                                    {
                                                        Session.GetHabbo().GetInventoryComponent().RedeemShell(Session);
                                                    }
                                                    else
                                                    {
                                                        Session.GetHabbo().Whisper(GoldTree.smethod_1("cmd_error_disabled"));
                                                    }
                                                    return true;
                                                default:
                                                    goto IL_3F91;
                                            }
                                    }
                                    try
                                    {
                                        if (!(ServerConfiguration.UnknownBoolean1 || Session.GetHabbo().HasFuse("cmd_push")))
                                        {
                                            Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_disabled"));
                                            return true;
                                        }
                                        if (!(Session.GetHabbo().IsVIP || Session.GetHabbo().HasFuse("cmd_push")))
                                        {
                                            Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_permission_vip"));
                                            return true;
                                        }
                                        string a = "down";
                                        string text = Params[1];
                                        TargetClient = GoldTree.GetGame().GetClientManager().GetClientByHabbo(text);
                                        class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                        if (Session == null || TargetClient == null)
                                        {
                                            return false;
                                        }
                                        class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                        RoomUser class4 = class2.GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
                                        if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
                                        {
                                            Session.GetHabbo().Whisper("It can't be that bad mate, no need to push yourself!");
                                            return true;
                                        }
                                        bool arg_3DD2_0;
                                        if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId)
                                        {
                                            if ((class6.int_3 + 1 != class4.int_3 || class6.int_4 != class4.int_4) && (class6.int_3 - 1 != class4.int_3 || class6.int_4 != class4.int_4) && (class6.int_4 + 1 != class4.int_4 || class6.int_3 != class4.int_3))
                                            {
                                                if (class6.int_4 - 1 == class4.int_4)
                                                {
                                                    if (class6.int_3 == class4.int_3)
                                                    {
                                                        goto IL_3DA6;
                                                    }
                                                }
                                                arg_3DD2_0 = (class6.int_3 != class4.int_3 || class6.int_4 != class4.int_4);
                                                goto IL_3DD2;
                                            }
                                        IL_3DA6:
                                            arg_3DD2_0 = false;
                                        }
                                        else
                                        {
                                            arg_3DD2_0 = true;
                                        }
                                    IL_3DD2:
                                        if (!arg_3DD2_0)
                                        {
                                            class6.HandleSpeech(Session, "*pushes " + TargetClient.GetHabbo().Username + "*", false);
                                            if (class6.int_8 == 0)
                                            {
                                                a = "up";
                                            }
                                            if (class6.int_8 == 2)
                                            {
                                                a = "right";
                                            }
                                            if (class6.int_8 == 4)
                                            {
                                                a = "down";
                                            }
                                            if (class6.int_8 == 6)
                                            {
                                                a = "left";
                                            }
                                            if (a == "up")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if ((!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 - 1 == class2.RoomModel.int_1)) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                    {
                                                        class4.MoveTo(class4.int_3, class4.int_4 - 1);
                                                    }
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.int_3, class4.int_4 - 1);
                                                }
                                            }
                                            if (a == "right")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (!(class4.int_3 + 1 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                        class4.MoveTo(class4.int_3 + 1, class4.int_4);
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.int_3 + 1, class4.int_4);
                                                }
                                            }
                                            if (a == "down")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (!(class4.int_3 == class2.RoomModel.int_0 && class4.int_4 + 1 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                        class4.MoveTo(class4.int_3, class4.int_4 + 1);
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.int_3, class4.int_4 + 1);
                                                }
                                            }
                                            if (a == "left")
                                            {
                                                if (ServerConfiguration.PreventDoorPush)
                                                {
                                                    if (!(class4.int_3 - 1 == class2.RoomModel.int_0 && class4.int_4 == class2.RoomModel.int_1) || Session.GetHabbo().HasFuse("acc_moveotheruserstodoor"))
                                                        class4.MoveTo(class4.int_3 - 1, class4.int_4);
                                                }
                                                else
                                                {
                                                    class4.MoveTo(class4.int_3 - 1, class4.int_4);
                                                }
                                            }
                                        }
                                        return true;
                                    }
                                    catch
                                    {
                                        return false;
                                    }
                                IL_3F03:
                                    class2 = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                                    class6 = class2.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    if (class6.Boolean_3)
                                    {
                                        Session.GetHabbo().Whisper("Command unavailable while trading");
                                        return true;
                                    }
                                    if (ServerConfiguration.EnableRedeemCredits)
                                    {
                                        Session.GetHabbo().GetInventoryComponent().ConvertCoinsToCredits();
                                    }
                                    else
                                    {
                                        Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("cmd_error_disabled"));
                                    }
                                    return true;
                                }
                        }
                    }
                IL_2F05:
                    DateTime now = DateTime.Now;
                    TimeSpan timeSpan = now - GoldTree.ServerStarted;

                    int clients = GoldTree.GetGame().GetClientManager().ClientCount;
                    int rooms = GoldTree.GetGame().GetRoomManager().LoadedRoomsCount;

                    string text10 = "";

                    if (ServerConfiguration.ShowUsersAndRoomsInAbout)
                    {
                        text10 = string.Concat(new object[]
						{
							"\nUsers Online: ",
							clients,
							"\nRooms Loaded: ",
							rooms
						});
                    }
                    Session.method_10(string.Concat(new object[]
					{
						"Gold Tree Emulator 3.0\n\nThanks/Credits;\nJuniori [Project Leader] \nBustanity [Code cleanup]\nSojobo [Phoenix Emu]\nMatty [Phoenix Emu]\nRoy [Uber Emu]\n\n",
						GoldTree.PrettyVersion,
						"\nUptime: ",
						timeSpan.Days,
						" days, ",
						timeSpan.Hours,
						" hours and ",
						timeSpan.Minutes,
						" minutes",
						text10,
					}), "");
                    return true;
                IL_3F91: ;
                }
                catch
                {
                }
                return false;
            }
        }
        public static string MergeParams(string[] Params, int Start)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < Params.Length; i++)
            {
                if (i >= Start)
                {
                    if (i > Start)
                    {
                        stringBuilder.Append(" ");
                    }
                    stringBuilder.Append(Params[i]);
                }
            }
            return stringBuilder.ToString();
        }
    }
}