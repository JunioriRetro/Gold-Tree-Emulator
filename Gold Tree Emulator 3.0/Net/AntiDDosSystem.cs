using System;
using System.Net.Sockets;
using GoldTree.Core;
using GoldTree.Util;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace GoldTree.Net
{
	internal sealed class AntiDDosSystem
	{
        public static IntPtr HWND_BROADCAST = new IntPtr(0xffff);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern uint RegisterWindowMessage(string lpProcName);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private static string[] mConnectionStorage;
        private static string mLastIpBlocked;
        internal static void SetupTcpAuthorization(int ConnectionCount)
		{
            AntiDDosSystem.mConnectionStorage = new string[ConnectionCount];
		}
        internal static bool CheckConnection(Socket Sock)
		{
            string text = Sock.RemoteEndPoint.ToString().Split(new char[]
			{
				':'
			})[0];
            if (text == AntiDDosSystem.mLastIpBlocked)
			{
				return false;
			}
			else
			{
                if (AntiDDosSystem.GetConnectionAmount(text) > 10 && text != "127.0.0.1" && text != ServerConfiguration.ProxyIP && !ServerConfiguration.DDoSProtectionEnabled)
				{
                    Process[] peerblockrunning = Process.GetProcessesByName("peerblock");
                    if (peerblockrunning.Length == 0)
                    {
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("PeerBlock is running! Adding IP to PeerBlock blacklist!");
                        Console.ForegroundColor = ConsoleColor.White;
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"ddos.p2p", true))
                        {
                            file.Write("DDoS IP:" + text + "-" + text + "\n");
                        }
                        uint msg = RegisterWindowMessage("PeerBlockLoadLists");
                        SendMessage(HWND_BROADCAST, msg, IntPtr.Zero, IntPtr.Zero);
                    }
					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine(text + " was banned by Anti-DDoS system.");
					Console.ForegroundColor = ConsoleColor.White;
                    Logging.LogDDoS(text + " - " + DateTime.Now.ToString());
                    AntiDDosSystem.mLastIpBlocked = text;
					return false;
				}
				else
				{
                    int num = AntiDDosSystem.GetFreeConnectionID();
					if (num < 0)
					{
						return false;
					}
					else
					{
                        AntiDDosSystem.mConnectionStorage[num] = text;
						return true;
					}
				}
			}
		}
        private static int GetConnectionAmount(string IP)
		{
			int num = 0;
            for (int i = 0; i < AntiDDosSystem.mConnectionStorage.Length; i++)
			{
                if (AntiDDosSystem.mConnectionStorage[i] == IP)
				{
					num++;
				}
			}
			return num;
		}
        internal static void FreeConnection(string IP)
		{
            for (int i = 0; i < AntiDDosSystem.mConnectionStorage.Length; i++)
			{
                if (AntiDDosSystem.mConnectionStorage[i] == IP)
				{
                    AntiDDosSystem.mConnectionStorage[i] = null;
					return;
				}
			}
		}
        private static int GetFreeConnectionID()
		{
            for (int i = 0; i < AntiDDosSystem.mConnectionStorage.Length; i++)
			{
                if (AntiDDosSystem.mConnectionStorage[i] == null)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
