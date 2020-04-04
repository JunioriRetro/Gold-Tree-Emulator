using System;
using System.Threading;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Util;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Misc
{
	internal sealed class PixelManager
	{
		public bool KeepAlive;
		private Thread WorkerThread;
		public PixelManager()
		{
			this.KeepAlive = true;
			this.WorkerThread = new Thread(new ThreadStart(this.method_1));
			this.WorkerThread.Name = "Pixel Manager";
			this.WorkerThread.Priority = ThreadPriority.Lowest;
		}
		public void method_0()
		{
			Logging.Write("Starting Reward Timer..");
			this.WorkerThread.Start();
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		private void method_1()
		{
			try
			{
				while (this.KeepAlive)
				{
					if (GoldTree.GetGame() != null && GoldTree.GetGame().GetClientManager() != null)
					{
						GoldTree.GetGame().GetClientManager().method_29();
					}
					Thread.Sleep(15000);
				}
			}
			catch (ThreadAbortException)
			{
			}
		}
		public bool method_2(GameClient Session)
		{
			double num = (GoldTree.GetUnixTimestamp() - Session.GetHabbo().LastActivityPointsUpdate) / 60.0;
			return num >= (double)ServerConfiguration.CreditingInterval;
		}
		public void method_3(GameClient Session)
		{
			try
			{
                if (Session.GetHabbo().InRoom)
				{
					RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (@class.int_1 <= ServerConfiguration.SleepTimer)
					{
						double double_ = GoldTree.GetUnixTimestamp();
						Session.GetHabbo().LastActivityPointsUpdate = double_;
						if (ServerConfiguration.PointingAmount > 0 && (Session.GetHabbo().ActivityPoints < ServerConfiguration.PixelLimit || ServerConfiguration.PixelLimit == 0))
						{
							Session.GetHabbo().ActivityPoints += ServerConfiguration.PointingAmount;
							Session.GetHabbo().method_16(ServerConfiguration.PointingAmount);
						}
						if (ServerConfiguration.CreditingAmount > 0 && (Session.GetHabbo().Credits < ServerConfiguration.CreditLimit || ServerConfiguration.CreditLimit == 0))
						{
							Session.GetHabbo().Credits += ServerConfiguration.CreditingAmount;
							if (Session.GetHabbo().IsVIP)
							{
								Session.GetHabbo().Credits += ServerConfiguration.CreditingAmount;
							}
							Session.GetHabbo().UpdateCredits(true);
						}
						if (ServerConfiguration.PixelingAmount > 0 && (Session.GetHabbo().VipPoints < ServerConfiguration.PointLimit || ServerConfiguration.PointLimit == 0))
						{
							Session.GetHabbo().VipPoints += ServerConfiguration.PixelingAmount;
							Session.GetHabbo().UpdateVipPoints(false, true);
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
