using System;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Handshake
{
	internal sealed class SSOTicketMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() == null)
			{
				Session.method_6(Event.PopFixedString());
				if (Session.GetHabbo() != null && Session.GetHabbo().list_2 != null && Session.GetHabbo().list_2.Count > 0)
				{
					using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
					{
						try
						{
							ServerMessage Message = new ServerMessage(420u);
							Message.AppendInt32(Session.GetHabbo().list_2.Count);
							foreach (uint current in Session.GetHabbo().list_2)
							{
								string string_ = @class.ReadString("SELECT username FROM users WHERE Id = " + current + " LIMIT 1;");
								Message.AppendStringWithBreak(string_);
							}
							Session.SendMessage(Message);
						}
						catch
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Logging.WriteLine("Login error: User is ignoring a user that no longer exists");
							Console.ForegroundColor = ConsoleColor.Gray;
						}

					}
				}
			}
		}
	}
}
