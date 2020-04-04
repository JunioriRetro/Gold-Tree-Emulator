using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Messenger
{
	internal sealed class SendMsgMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			string text = GoldTree.FilterString(Event.PopFixedString());
			if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().GetMessenger() != null)
			{
				if (num == 0u && Session.GetHabbo().HasFuse("cmd_sa"))
				{
					ServerMessage Message = new ServerMessage(134u);
					Message.AppendUInt(0u);
					Message.AppendString(Session.GetHabbo().Username + ": " + text);
					GoldTree.GetGame().GetClientManager().method_17(Session, Message);
				}
				else
				{
					if (num == 0u)
					{
						ServerMessage Message2 = new ServerMessage(261u);
						Message2.AppendInt32(4);
						Message2.AppendUInt(0u);
						Session.SendMessage(Message2);
					}
					else
					{
                        if (Session != null && Session.GetHabbo() != null)
                        {
                            Session.GetHabbo().GetMessenger().method_18(num, text);
                        }
					}
				}
			}
		}
	}
}
