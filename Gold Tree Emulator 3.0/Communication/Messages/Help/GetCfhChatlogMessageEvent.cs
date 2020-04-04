using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Support;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class GetCfhChatlogMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				SupportTicket @class = GoldTree.GetGame().GetModerationTool().method_5(Event.PopWiredUInt());
				if (@class != null)
				{
                    RoomData class2 = GoldTree.GetGame().GetRoomManager().method_11(@class.RoomId);
					if (class2 != null)
					{
                        Session.SendMessage(GoldTree.GetGame().GetModerationTool().method_21(@class, class2, @class.Timestamp));
					}
				}
			}
		}
	}
}
