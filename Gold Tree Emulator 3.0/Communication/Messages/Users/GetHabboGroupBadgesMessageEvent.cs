using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Users
{
	internal sealed class GetHabboGroupBadgesMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().uint_2 > 0u)
			{
				Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().uint_2);
				if (@class != null && Session.GetHabbo().int_0 > 0)
				{
					GroupsManager class2 = Groups.smethod_2(Session.GetHabbo().int_0);
					if (class2 != null && !@class.list_17.Contains(class2))
					{
						@class.list_17.Add(class2);
						ServerMessage Message = new ServerMessage(309u);
						Message.AppendInt32(@class.list_17.Count);
						foreach (GroupsManager current in @class.list_17)
						{
							Message.AppendInt32(current.int_0);
							Message.AppendStringWithBreak(current.string_2);
						}
						@class.SendMessage(Message, null);
					}
					else
					{
						foreach (GroupsManager current2 in @class.list_17)
						{
							if (current2 == class2 && current2.string_2 != class2.string_2)
							{
								ServerMessage Message = new ServerMessage(309u);
								Message.AppendInt32(@class.list_17.Count);
								foreach (GroupsManager current in @class.list_17)
								{
									Message.AppendInt32(current.int_0);
									Message.AppendStringWithBreak(current.string_2);
								}
								@class.SendMessage(Message, null);
							}
						}
					}
				}
				if (@class != null && @class.list_17.Count > 0)
				{
					ServerMessage Message = new ServerMessage(309u);
					Message.AppendInt32(@class.list_17.Count);
					foreach (GroupsManager current in @class.list_17)
					{
						Message.AppendInt32(current.int_0);
						Message.AppendStringWithBreak(current.string_2);
					}
					Session.SendMessage(Message);
				}
			}
		}
	}
}
