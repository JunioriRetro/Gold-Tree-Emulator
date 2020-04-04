using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using GoldTree.Core;
using System.Net;
using System.IO;
namespace GoldTree
{
    internal class Program
    {
        private delegate bool EventHandler(CtrlType sig);
        private enum CtrlType
        {
            CTRL_BREAK_EVENT = 1,
            CTRL_C_EVENT = 0,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool bool_0 = false;

        private static EventHandler delegate0_0;

        static ConsoleKeyInfo ConsoleKeyInfo;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(Program.EventHandler handler, bool add);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        public const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void Main(string[] args)
        {
            CustomCultureInfo.SetupCustomCultureInfo();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.smethod_0);

            Program.delegate0_0 = (Program.EventHandler)Delegate.Combine(Program.delegate0_0, new Program.EventHandler(Program.smethod_1));

            Program.SetConsoleCtrlHandler(Program.delegate0_0, true);

            if (DownloadNewVersion())
            {
                return;
            }

            try
            {
                GoldTree @class = new GoldTree();
                @class.Initialize();
                Program.bool_0 = true;
                DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\n~~~ IF YOU WANT CLOSE EMULATOR PLEASE PRESS ESCAPE (Esc) BUTTON ~~~\r\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            Wait();
        }
        private static void smethod_0(object sender, UnhandledExceptionEventArgs e)
        {
            Logging.Disable();
            Exception ex = (Exception)e.ExceptionObject;
            Logging.LogCriticalException(ex.ToString());
        }
        private static bool smethod_1(CtrlType enum0_0)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), true), SC_CLOSE, MF_BYCOMMAND);
            if (Program.bool_0)
            {
                Logging.Disable();
                Console.Clear();
                Console.WriteLine("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!");
                GoldTree.Destroy("", true);
            }
            return true;
        }

        private static bool DownloadNewVersion()
        {
            try
            {
                if (!GoldTree.GetConfig().data.ContainsKey("gte.update.noticy.disable") || int.Parse(GoldTree.GetConfig().data["gte.update.noticy.disable"]) == 0)
                {

                    WebClient client2 = new WebClient();
                    Stream stream2 = client2.OpenRead("https://raw.github.com/JunioriRetro/Gold-Tree-Emulator/master/currentbuild.txt");
                    StreamReader reader2 = new StreamReader(stream2);
                    String content2 = reader2.ReadLine();
                    if (int.Parse(content2) > System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build)
                    {
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead("https://raw.github.com/JunioriRetro/Gold-Tree-Emulator/master/possibleautoupdate.txt");
                        StreamReader reader = new StreamReader(stream);
                        bool PossibleUpdate = false;
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Contains(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString()))
                            {
                                PossibleUpdate = true;
                                if (int.Parse(GoldTree.GetConfig().data["gte.update.autoupdate"]) == 0)
                                {
                                    Console.WriteLine("New version available! Download new version? [Y/N]");
                                    ConsoleKeyInfo = Console.ReadKey();
                                    if (ConsoleKeyInfo.Key == ConsoleKey.Y)
                                    {
                                        client2.DownloadFile("https://raw.github.com/JunioriRetro/Gold-Tree-Emulator/master/Gold%20Tree%20Emulator%203.0/bin/Debug/Gold%20Tree%20Emulator%203.0.exe", Environment.CurrentDirectory + @"\" + content2 + ".exe");
                                        System.Diagnostics.Process.Start(Environment.CurrentDirectory + @"\" + content2 + ".exe");
                                        return true;
                                    }
                                    else if (ConsoleKeyInfo.Key == ConsoleKey.N)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        DownloadNewVersion();
                                    }
                                }
                                else
                                {
                                    client2.DownloadFile("https://raw.github.com/JunioriRetro/Gold-Tree-Emulator/master/Gold%20Tree%20Emulator%203.0/bin/Debug/Gold%20Tree%20Emulator%203.0.exe", Environment.CurrentDirectory + @"\" + content2 + ".exe");
                                    System.Diagnostics.Process.Start(Environment.CurrentDirectory + @"\" + content2 + ".exe");
                                    return true;
                                }
                            }
                        }
                        if (!PossibleUpdate)
                        {
                            Console.WriteLine("New version available! From some reason auto update is not possible.");
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void Wait()
        {
            ConsoleKeyInfo = Console.ReadKey();

            if (ConsoleKeyInfo.Key == ConsoleKey.Escape)
                smethod_1(CtrlType.CTRL_CLOSE_EVENT);

            Wait();
        }
    }
}