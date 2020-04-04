using System;
using System.Diagnostics;
using System.Threading;
using GoldTree.Core;
using GoldTree.Storage;
using System.Globalization;
using GoldTree.Messages;
namespace GoldTree.HabboHotel.Misc
{
    public sealed class LowPriorityWorker
    {
        public static void Initialise()
        {
            double lastDatabaseUpdate = GoldTree.GetUnixTimestamp();

            while (true)
            {
                try
                {
                    DateTime now = DateTime.Now;
                    TimeSpan timeSpan = now - GoldTree.ServerStarted;
                    new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    int Status = 1;

                    int UsersOnline = GoldTree.GetGame().GetClientManager().ClientCount;
                    int RoomsLoaded = GoldTree.GetGame().GetRoomManager().LoadedRoomsCount;

                    try
                    {
                        if (GoldTree.GetConfig().data["shutdown-server"] != null)
                        {
                            DateTime shutdown_server_time = Convert.ToDateTime(GoldTree.GetConfig().data["shutdown-server"]);
                            var time = shutdown_server_time.TimeOfDay.TotalSeconds;
                            string s = DateTime.Now.ToString("HH:mm:ss");
                            DateTime dt2 = DateTime.ParseExact(s, "HH:mm:ss", CultureInfo.InvariantCulture);
                            var time2 = dt2.TimeOfDay.TotalSeconds;
                            try
                            {
                                if (GoldTree.GetConfig().data["shutdown-warning-alert"] != null)
                                {
                                    if (time - time2 <= 60 && time - time2 >= 50)
                                    {
                                        try
                                        {
                                            if (int.Parse(GoldTree.GetConfig().data["shutdown-server-player-limit"]) < UsersOnline || int.Parse(GoldTree.GetConfig().data["shutdown-server-player-limit"]) <= 0)
                                            {
                                                string str = GoldTree.GetConfig().data["shutdown-warning-alert"];
                                                ServerMessage Message2 = new ServerMessage(808u);
                                                Message2.AppendStringWithBreak(GoldTreeEnvironment.GetExternalText("cmd_ha_title"));
                                                Message2.AppendStringWithBreak(str + "\r\n- " + "Hotel");
                                                ServerMessage Message3 = new ServerMessage(161u);
                                                Message3.AppendStringWithBreak(str + "\r\n- " + "Hotel");
                                                GoldTree.GetGame().GetClientManager().method_15(Message2, Message3);
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                            if (time - time2 <= 11 && time - time2 >= 0)
                            {
                                try
                                {
                                    if (int.Parse(GoldTree.GetConfig().data["shutdown-server-player-limit"]) < UsersOnline || int.Parse(GoldTree.GetConfig().data["shutdown-server-player-limit"]) <= 0)
                                    {
                                        GoldTree.Destroy("SERVER SHUTDOWN! YOU HAVE SETUP TO CONFIG.CONF FILE SHUTDOWN TIME!", true);
                                    }
                                }
                                catch
                                {
                                    GoldTree.Destroy("SERVER SHUTDOWN! YOU HAVE SETUP TO CONFIG.CONF FILE SHUTDOWN TIME!", true);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    double timestamp = GoldTree.GetUnixTimestamp() - lastDatabaseUpdate;

                    if (timestamp >= 30)
                    {
                        using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                        {
                            dbClient.ExecuteQuery(string.Concat(new object[]
						    {
							    "UPDATE server_status SET stamp = UNIX_TIMESTAMP(), status = '", Status, "', users_online = '",	UsersOnline, "', rooms_loaded = '",	RoomsLoaded, "', server_ver = '", GoldTree.PrettyVersion,	"' LIMIT 1" 	}));
                                uint num3 = (uint)dbClient.ReadInt32("SELECT users FROM system_stats ORDER BY ID DESC LIMIT 1");
                                if ((long)UsersOnline > (long)((ulong)num3))
                                {
                                    dbClient.ExecuteQuery(string.Concat(new object[]
							    {
								    "UPDATE system_stats SET users = '",
								    UsersOnline,
								    "', rooms = '",
								    RoomsLoaded,
								    "' ORDER BY ID DESC LIMIT 1"
							    }));
                            }
                        }

                        lastDatabaseUpdate = timestamp;
                    }

                    GoldTree.GetGame().GetClientManager().method_23();

                    Console.Title = string.Concat(new object[]
					{
						"GTE 3.0 | Online Users: ",
						UsersOnline,
						" | Rooms Loaded: ",
						RoomsLoaded,
						" | Uptime: ",
						timeSpan.Days,
						" days, ",
						timeSpan.Hours,
						" hours and ",
						timeSpan.Minutes,
						" minutes"
					});
                }
                catch (Exception ex)
                {
                    Program.DeleteMenu(Program.GetSystemMenu(Program.GetConsoleWindow(), true), Program.SC_CLOSE, Program.MF_BYCOMMAND);
                    Logging.LogThreadException(ex.ToString(), "Server status update task");
                }
                Thread.Sleep(5000);
            }
        }
    }
}
