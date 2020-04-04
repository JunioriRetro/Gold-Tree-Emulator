using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Engine
{
	internal sealed class MoveObjectMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.method_26(Session))
			{
				RoomItem class2 = @class.method_28(Event.PopWiredUInt());
				if (class2 != null)
				{
					int num = Event.PopWiredInt32();
					int num2 = Event.PopWiredInt32();
					int num3 = Event.PopWiredInt32();
					Event.PopWiredInt32();
                    if (Session.GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "MOVE_ITEM" && (num != class2.Int32_0 || num2 != class2.Int32_1))
					{
                        GoldTree.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
					}
					else
					{
                        if (Session.GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "ROTATE_ITEM" && num3 != class2.int_3)
						{
                            GoldTree.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
						}
						else
						{
                            if (Session.GetHabbo().CurrentQuestId > 0 && GoldTree.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "STACKITEM" && class2.Double_0 > 0.0)
							{
                                GoldTree.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
							}
						}
					}
					bool flag = false;
					string text = class2.GetBaseItem().InteractionType.ToLower();
					if (text != null && text == "teleport")
					{
						flag = true;
					}
                    if (text != null && text == "hopper")
                    {
                        flag = true;
                    }
					@class.method_79(Session, class2, num, num2, num3, false, false, false);
					if (flag)
					{
						@class.method_64();
					}
				}
			}
		}
	}
}
