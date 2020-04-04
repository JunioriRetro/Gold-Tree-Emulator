using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.RoomBots;
using GoldTree.HabboHotel.Rooms;
using GoldTree.HabboHotel.Pathfinding;
namespace GoldTree.Communication.Messages.Rooms.Action
{
	internal sealed class CallGuideBotMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				for (int i = 0; i < @class.RoomUsers.Length; i++)
				{
					RoomUser class2 = @class.RoomUsers[i];
					if (class2 != null && (class2.IsBot && class2.RoomBot.AiType == AIType.const_1))
					{
						ServerMessage Message = new ServerMessage(33u);
						Message.AppendInt32(4009);
						Session.SendMessage(Message);
						return;
					}
				}
				if (Session.GetHabbo().bool_10)
				{
					ServerMessage Message = new ServerMessage(33u);
					Message.AppendInt32(4010);
					Session.SendMessage(Message);
				}
				else
				{
					RoomUser class3 = @class.BotToRoomUser(GoldTree.GetGame().GetBotManager().method_3(2u));
					class3.method_7(@class.RoomModel.int_0, @class.RoomModel.int_1, @class.RoomModel.double_0);
					class3.UpdateNeeded = true;
					RoomUser class4 = @class.method_56(@class.Owner);
					if (class4 != null)
					{
						class3.MoveTo(class4.Position);
						class3.method_9(Class107.smethod_0(class3.int_3, class3.int_4, class4.int_3, class4.int_4));
					}
                    Session.GetHabbo().CallGuideBotAchievementsCompleted();
					Session.GetHabbo().bool_10 = true;
				}
			}
		}
	}
}
