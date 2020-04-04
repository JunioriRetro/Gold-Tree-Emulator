using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using GoldTree.Core;
using GoldTree.HabboHotel;
using GoldTree.Net;
using GoldTree.Storage;
using GoldTree.Util;
using GoldTree.Communication;
using GoldTree.Messages;
using System.Net;
using System.IO;
using System.Data;
using System.Collections;
namespace GoldTree
{
    internal sealed class GoldTree
    {
        public static readonly int build = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build;
        public const string string_0 = "localhost";

        private static PacketManager PacketManager;

        private static ConfigurationData Configuration;

        private static DatabaseManager DatabaseManager;

        private static SocketsManager SocketsManager;
        //private static ConnectionHandeling ConnectionManage;
        private static MusListener MusListener;

        private static Game Internal_Game;

        internal static DateTime ServerStarted;

        public string string_2 = GoldTree.smethod_1(14986.ToString());

        public static bool bool_0 = false;
        public static int int_1 = 0;
        public static int int_2 = 0;
        public static string string_5 = null;

        private static bool bool_1 = false;

        public static List<string> UserAdMessage;
        public static int UserAdType;
        public static string UserAdLink;

        public static int Build
        {
            get
            {
                return Build;
            }
        }

        public static string PrettyVersion
        {
            get
            {
                return "Gold Tree Emulator v3.19.0 ALPHA 7 (Build " + build + ")";
            }
        }

        internal static Game Game
        {
            get
            {
                return GoldTree.Internal_Game;
            }
            set
            {
                GoldTree.Internal_Game = value;
            }
        }

        public static PacketManager GetPacketManager()
        {
            return GoldTree.PacketManager;
        }

        public static ConfigurationData GetConfig()
        {
            return Configuration;
        }

        public static DatabaseManager GetDatabase()
        {
            return DatabaseManager;
        }

        public static Encoding GetDefaultEncoding()
        {
            return Encoding.Default;
        }

        public static SocketsManager GetSocketsManager()
        {
            return GoldTree.SocketsManager;
        }

        //public static ConnectionHandeling smethod_14()
        //{
        //    return GoldTree.ConnectionManage;
        //}

        internal static Game GetGame()
        {
            return Internal_Game;
        }

        public static string smethod_0(string string_8)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = Encoding.UTF8.GetBytes(string_8);
            array = mD5CryptoServiceProvider.ComputeHash(array);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2").ToLower());
            }
            string text = stringBuilder.ToString();
            return text.ToUpper();
        }

        public static string smethod_1(string string_8)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(string_8);
            byte[] array = new SHA1Managed().ComputeHash(bytes);
            string text = string.Empty;
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                text += b.ToString("X2");
            }
            return text;
        }

        public void Initialize()
        {
            GoldTree.ServerStarted = DateTime.Now;

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();
            Console.WriteLine("                      _______   _________   ______ ");
            Console.WriteLine("                     |  _____| |___   ___| | _____|");
            Console.WriteLine("                     | |  ___      | |     | |____");
            Console.WriteLine("                     | | |_  |     | |     |  ____|");
            Console.WriteLine("                     | |___| |     | |     | |____");
            Console.WriteLine("                     |_______|     |_|     |______|");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("                  " + PrettyVersion);
            Console.WriteLine();

            try
            {
                UserAdMessage = new List<string>();

                WebClient client2 = new WebClient();

                Stream stream2 = client2.OpenRead("https://raw.github.com/JunioriRetro/Gold-Tree-Emulator/master/useradtype.txt");
                StreamReader reader2 = new StreamReader(stream2);

                String content2 = reader2.ReadLine();

                try
                {
                    UserAdType = int.Parse(content2);
                }
                catch { }

                WebClient client3 = new WebClient();

                Stream stream3 = client3.OpenRead("https://raw.github.com/JunioriRetro/Gold-Tree-Emulator/master/useradmessage.txt");
                StreamReader reader3 = new StreamReader(stream3);

                string line2;
                while ((line2 = reader3.ReadLine()) != null)
                {
                    UserAdMessage.Add(line2);
                }

                WebClient client4 = new WebClient();

                Stream stream4 = client4.OpenRead("https://raw.github.com/JunioriRetro/Gold-Tree-Emulator/master/useradlink.txt");
                StreamReader reader4 = new StreamReader(stream4);

                String content4 = reader4.ReadLine();

                UserAdLink = content4;

                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("https://raw.github.com/JunioriRetro/Gold-Tree-Emulator/master/consoleads.txt");
                    StreamReader reader = new StreamReader(stream);

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(":"))
                        {
                            string[] Params = line.Split(new char[]
			                {
				                ' '
			                });

                            if (Params[0] == ":textcolor")
                            {
                                if (!string.IsNullOrEmpty(Params[1]))
                                {
                                    ConsoleColor Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Params[1]);
                                    Console.ForegroundColor = Color;
                                }
                            }

                            else if (Params[0] == ":colorchangingtext")
                            {
                                string text = line.Substring(Params[0].Length + Params[1].Length + Params[2].Length + Params[3].Length + Params[4].Length + 5);
                                RainbowText(text, IntToArray(Params[1]), 0, int.Parse(Params[2]), 0, int.Parse(Params[3]), bool.Parse(Params[4]), -1);
                            }
                        }
                        else
                        {
                            Console.WriteLine(line);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
                catch
                {

                }
            }
            catch
            {
                Console.WriteLine("Sad cant find ads :(");
            }

            Console.ResetColor();

            try
            {
                GoldTree.Configuration = new ConfigurationData("config.conf");

                DateTime now = DateTime.Now;

                //Lookds = new Random().Next(Int32.MaxValue).ToString();

                DatabaseServer dbServer = new DatabaseServer(GoldTree.GetConfig().data["db.hostname"], uint.Parse(GoldTree.GetConfig().data["db.port"]), GoldTree.GetConfig().data["db.username"], GoldTree.GetConfig().data["db.password"]);
                Database database = new Database(GoldTree.GetConfig().data["db.name"], uint.Parse(GoldTree.GetConfig().data["db.pool.minsize"]), uint.Parse(GoldTree.GetConfig().data["db.pool.maxsize"]));
                GoldTree.DatabaseManager = new DatabaseManager(dbServer, database);

                try
                {
                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                    {
                        dbClient.ExecuteQuery("UPDATE users SET online = '0'");
                        dbClient.ExecuteQuery("UPDATE rooms SET users_now = '0'");

                        DataRow DataRow;
                        DataRow = dbClient.ReadDataRow("SHOW COLUMNS FROM `items` WHERE field = 'fw_count'");

                        DataRow DataRow2;
                        DataRow2 = dbClient.ReadDataRow("SHOW COLUMNS FROM `items` WHERE field = 'extra_data'");

                        if (DataRow != null || DataRow2 != null)
                        {
                            if (DoYouWantContinue("Remember get backups before continue! Do you want continue? [Y/N]"))
                            {
                                if (DataRow != null)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("UPDATING ITEMS POSSIBLY TAKE A LONG TIME! DONT SHUTDOWN EMULATOR! PLEASE WAIT!");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write("Updating items (Fireworks) ...");

                                    dbClient.ExecuteQuery("DROP TABLE IF EXISTS items_firework");
                                    dbClient.ExecuteQuery("CREATE TABLE IF NOT EXISTS `items_firework` (`item_id` int(10) unsigned NOT NULL, `fw_count` int(10) NOT NULL, PRIMARY KEY (`item_id`)) ENGINE=MyISAM DEFAULT CHARSET=latin1;");
                                    dbClient.ExecuteQuery("INSERT INTO items_firework SELECT Id, fw_count FROM items WHERE fw_count > 0;");
                                    dbClient.ExecuteQuery("ALTER TABLE items DROP fw_count");

                                    Console.WriteLine("completed!");
                                }

                                if (DataRow2 != null)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("UPDATING ITEMS POSSIBLY TAKE A LONG TIME! DONT SHUTDOWN EMULATOR! PLEASE WAIT!");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write("Updating items (Extra data) ...");

                                    dbClient.ExecuteQuery("DROP TABLE IF EXISTS items_extra_data");
                                    dbClient.ExecuteQuery("CREATE TABLE IF NOT EXISTS `items_extra_data` (`item_id` int(10) unsigned NOT NULL, `extra_data` text NOT NULL, PRIMARY KEY (`item_id`)) ENGINE=MyISAM DEFAULT CHARSET=latin1;");
                                    dbClient.ExecuteQuery("INSERT INTO items_extra_data SELECT Id, extra_data FROM items WHERE extra_data != '';");
                                    dbClient.ExecuteQuery("ALTER TABLE items DROP extra_data");

                                    Console.WriteLine("completed!");
                                }

                            }
                            else
                            {
                                Logging.WriteLine("Press any key to shut down ...");
                                Console.ReadKey(true);
                                GoldTree.Destroy();
                                Logging.WriteLine("Press any key to close window ...");
                                Console.ReadKey(true);
                                Environment.Exit(0);
                                return;
                            }
                        }
                    }
                    //GoldTree.ConnectionManage.method_7();
                    GoldTree.Internal_Game.ContinueLoading();
                }
                catch { }

                GoldTree.Internal_Game = new Game(int.Parse(GoldTree.GetConfig().data["game.tcp.conlimit"]));

                GoldTree.PacketManager = new PacketManager();

                GoldTree.PacketManager.Handshake();

                GoldTree.PacketManager.Messenger();

                GoldTree.PacketManager.Navigator();

                GoldTree.PacketManager.RoomsAction();
                GoldTree.PacketManager.RoomsAvatar();
                GoldTree.PacketManager.RoomsChat();
                GoldTree.PacketManager.RoomsEngine();
                GoldTree.PacketManager.RoomsFurniture();
                GoldTree.PacketManager.RoomsPets();
                GoldTree.PacketManager.RoomsPools();
                GoldTree.PacketManager.RoomsSession();
                GoldTree.PacketManager.RoomsSettings();

                GoldTree.PacketManager.Catalog();
                GoldTree.PacketManager.Marketplace();
                GoldTree.PacketManager.Recycler();

                GoldTree.PacketManager.Quest();

                GoldTree.PacketManager.InventoryAchievements();
                GoldTree.PacketManager.InventoryAvatarFX();
                GoldTree.PacketManager.InventoryBadges();
                GoldTree.PacketManager.InventoryFurni();
                GoldTree.PacketManager.InventoryPurse();
                GoldTree.PacketManager.InventoryTrading();

                GoldTree.PacketManager.Avatar();
                GoldTree.PacketManager.Users();
                GoldTree.PacketManager.Register();

                GoldTree.PacketManager.Help();

                GoldTree.PacketManager.Sound();

                GoldTree.PacketManager.Wired();

                GoldTree.PacketManager.Jukebox();

                GoldTree.PacketManager.FriendStream();

                GoldTree.MusListener = new MusListener(GoldTree.GetConfig().data["mus.tcp.bindip"], int.Parse(GoldTree.GetConfig().data["mus.tcp.port"]), GoldTree.GetConfig().data["mus.tcp.allowedaddr"].Split(new char[] { ';' }), 20);
                GoldTree.SocketsManager = new SocketsManager(GoldTree.GetConfig().data["game.tcp.bindip"], int.Parse(GoldTree.GetConfig().data["game.tcp.port"]), int.Parse(GoldTree.GetConfig().data["game.tcp.conlimit"]));
                //ConnectionManage = new ConnectionHandeling(GoldTree.GetConfig().data["game.tcp.port"], int.Parse(GoldTree.GetConfig().data["game.tcp.conlimit"]), int.Parse(GoldTree.GetConfig().data["game.tcp.conlimit"]), true);
                GoldTree.SocketsManager.method_3().method_0();
                //ConnectionManage.init();
                //ConnectionManage.Start();

                /*try
                {
                    if (int.Parse(GoldTree.GetConfig().data["automatic-error-report"]) < 1 || int.Parse(GoldTree.GetConfig().data["automatic-error-report"]) > 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Logging.WriteLine("Erroreita ei raportoida automaattisesti!!!");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    if (int.Parse(GoldTree.GetConfig().data["automatic-error-report"]) == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Logging.WriteLine("Kaikki errorit reportoidaan automaattisesti");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    if (int.Parse(GoldTree.GetConfig().data["automatic-error-report"]) > 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Logging.WriteLine("Vain kritikaaliset virheiden reportoidaan automaattisesti");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Logging.WriteLine("Erroreita ei raportoida automaattisesti!!!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }*/

                TimeSpan timeSpan = DateTime.Now - now;
                Logging.WriteLine(string.Concat(new object[]
				    {
					    "Server -> READY! (",
					    timeSpan.Seconds,
					    " s, ",
					    timeSpan.Milliseconds,
					    " ms)"
				    }));
                Console.Beep();
            }
            catch (KeyNotFoundException KeyNotFoundException)
            {
                Logging.WriteLine("Failed to boot, key not found: " + KeyNotFoundException);
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                GoldTree.Destroy();
            }
            catch (InvalidOperationException ex)
            {
                Logging.WriteLine("Failed to initialize GoldTreeEmulator: " + ex.Message);
                Logging.WriteLine("Press any key to shut down ...");
                Console.ReadKey(true);
                GoldTree.Destroy();
            }
        }

        public static int StringToInt(string str)
        {
            return Convert.ToInt32(str);
        }

        public static bool StringToBoolean(string str)
        {
            return (str == "1" || str == "true");
        }

        public static string BooleanToString(bool b)
        {
            if (b)
                return "1";
            else
                return "0";
        }

        public static int smethod_5(int int_3, int int_4)
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] array = new byte[4];
            rNGCryptoServiceProvider.GetBytes(array);
            int seed = BitConverter.ToInt32(array, 0);
            return new Random(seed).Next(int_3, int_4 + 1);
        }

        public static double GetUnixTimestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public static string FilterString(string str)
        {
            return DoFilter(str, false, false);
        }

        public static string DoFilter(string Input, bool bool_2, bool bool_3)
        {
            Input = Input.Replace(Convert.ToChar(1), ' ');
            Input = Input.Replace(Convert.ToChar(2), ' ');
            Input = Input.Replace(Convert.ToChar(9), ' ');
            if (!bool_2)
            {
                Input = Input.Replace(Convert.ToChar(13), ' ');
            }
            if (bool_3)
            {
                Input = Input.Replace('\'', ' ');
            }
            return Input;
        }

        public static bool smethod_9(string string_8)
        {
            if (string.IsNullOrEmpty(string_8))
            {
                return false;
            }
            else
            {
                for (int i = 0; i < string_8.Length; i++)
                {
                    if (!char.IsLetter(string_8[i]) && !char.IsNumber(string_8[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static void Destroy()
        {
            Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);
            Logging.WriteLine("Destroying GoldTreeEmu environment...");

            if (GoldTree.GetGame() != null)
            {
                GoldTree.GetGame().ContinueLoading();
                GoldTree.Internal_Game = null;
            }

            if (GoldTree.GetSocketsManager() != null)
            {
                Logging.WriteLine("Destroying connection manager.");
                GoldTree.GetSocketsManager().method_3().method_2();
                //GoldTree.smethod_14().Destroy();
                GoldTree.GetSocketsManager().method_0();
                GoldTree.SocketsManager = null;
            }

            if (GoldTree.GetDatabase() != null)
            {
                try
                {
                    Logging.WriteLine("Destroying database manager.");
                    MySqlConnection.ClearAllPools();
                    GoldTree.DatabaseManager = null;
                }
                catch
                {
                }
            }

            Logging.WriteLine("Uninitialized successfully. Closing.");
        }

        internal static void smethod_17(string string_8)
        {
            try
            {
                ServerMessage Message = new ServerMessage(139u);
                Message.AppendStringWithBreak(string_8);
                GoldTree.GetGame().GetClientManager().BroadcastMessage(Message);
            }
            catch
            {
            }
        }

        internal static void Close()
        {
            GoldTree.Destroy("", true);
        }

        internal static void Destroy(string string_8, bool ExitWhenDone, bool waitExit = false)
        {
            Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);

            try
            {
                Internal_Game.StopGameLoop();
            }
            catch { }

            try
            {
                if (GoldTree.GetPacketManager() != null)
                {
                    GoldTree.GetPacketManager().Clear();
                }
            }
            catch { }

            if (string_8 != "")
            {
                if (GoldTree.bool_1)
                {
                    return;
                }
                Console.WriteLine(string_8);
                Logging.Disable();
                GoldTree.smethod_17("ATTENTION:\r\nThe server is shutting down. All furniture placed in rooms/traded/bought after this message is on your own responsibillity.");
                GoldTree.bool_1 = true;
                Console.WriteLine("Server shutting down...");
                try
                {
                    GoldTree.Internal_Game.GetRoomManager().method_4();
                }
                catch
                {
                }
                try
                {
                    GoldTree.GetSocketsManager().method_3().method_1();
                    //GoldTree.smethod_14().Destroy();
                    GoldTree.GetGame().GetClientManager().CloseAll();
                }
                catch
                {
                }
                try
                {
                    Console.WriteLine("Destroying database manager.");
                    MySqlConnection.ClearAllPools();
                    GoldTree.DatabaseManager = null;
                }
                catch
                {
                }
                Console.WriteLine("System disposed, goodbye!");
            }
            else
            {
                Logging.Disable();
                GoldTree.bool_1 = true;
                try
                {
                    if (GoldTree.Internal_Game != null && GoldTree.Internal_Game.GetRoomManager() != null)
                    {
                        GoldTree.Internal_Game.GetRoomManager().UnloadAllRooms();
                        GoldTree.Internal_Game.GetRoomManager().method_4();
                    }
                }
                catch
                {
                }
                try
                {
                    if (GoldTree.GetSocketsManager() != null)
                    {
                        GoldTree.GetSocketsManager().method_3().method_1();
                        //GoldTree.smethod_14().Destroy();
                        GoldTree.GetGame().GetClientManager().CloseAll();
                    }
                }
                catch
                {
                }
                if (SocketsManager != null)
                {
                    //GoldTree.ConnectionManage.method_7();
                }
                if (GoldTree.Internal_Game != null)
                {
                    GoldTree.Internal_Game.ContinueLoading();
                }
                Console.WriteLine(string_8);
            }
            if (ExitWhenDone)
            {
                if (waitExit)
                {
                    Console.WriteLine("Press any key to exit..");
                    Console.ReadKey();
                }

                Environment.Exit(0);
            }
        }

        public static bool CanBeDividedBy(int i, int j)
        {
            return i % j == 0;
        }

        public static DateTime TimestampToDate(double timestamp)
        {
            DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return result.AddSeconds(timestamp).ToLocalTime();
        }


        public static int[] IntToArray(string numbers)
        {
            string[] ColorList = numbers.Split(new char[]
			{
				'|'
			});

            var digits = new List<int>();

            if (ColorList.Count() > 1)
            {
                for (int i = 0; i < ColorList.Count(); i++)
                {
                    digits.Add(int.Parse(ColorList[i]));
                }
            }
            else
            {
                digits.Add(int.Parse(ColorList[0]));
            }

            var arr = digits.ToArray();
            Array.Reverse(arr);
            return arr;
        }

        public static void RainbowText(string text, int[] colors, int color, int interval, int count, int maxcount, bool randomcolors, int lastcolor)
        {
            if (count > maxcount)
            {
                Console.ResetColor();
                Console.Write("\r{0}   ", text);
                Console.WriteLine();
                return;
            }

            count++;

            if (randomcolors)
            {
                Random random = new Random();
                int randomcolor = random.Next(1, 15);

                while (lastcolor == randomcolor || randomcolor == 0)
                {
                    randomcolor = random.Next(1, 15);
                }

                color = randomcolor;
            }
            else
            {
                if (colors.Count() > 1)
                {
                    if (!(color >= 0 && color <= 15))
                    {
                        color = 0;
                    }

                    while (!colors.Contains(color) || lastcolor == color || (!(color >= 0 && color <= 15)))
                    {
                        color++;

                        if (!(color >= 0 && color <= 15))
                        {
                            color = 0;
                        }
                    }
                }
                else
                {
                    color = colors[1];
                }
            }

            if (color > 0 && color <= 15)
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.Write("\r{0}   ", text);
                Console.ResetColor();
                lastcolor = color;
            }

            System.Threading.Thread.Sleep(interval);

            RainbowText(text, colors, color, interval, count, maxcount, randomcolors, lastcolor);
        }

        public bool DoYouWantContinue(string message)
        {
            Console.WriteLine(message);
            ConsoleKeyInfo ConsoleKeyInfo = Console.ReadKey();
            if (ConsoleKeyInfo.Key == ConsoleKey.Y)
            {
                return true;
            }
            else if (ConsoleKeyInfo.Key == ConsoleKey.N)
            {
                return false;
            }
            else
            {
                DoYouWantContinue(message);
            }

            return false;
        }
    }
}
