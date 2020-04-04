using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Engine
{
	internal sealed class SetObjectDataMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null && @class.method_26(Session))
			{
				int num = Event.PopWiredInt32();
				int num2 = Event.PopWiredInt32();
				string text = Event.PopFixedString();
				string text2 = Event.PopFixedString();
				string text3 = Event.PopFixedString();
				string text4 = Event.PopFixedString();
				string text5 = Event.PopFixedString();
				string text6 = Event.PopFixedString();
				string text7 = Event.PopFixedString();
				string text8 = Event.PopFixedString();
				string text9 = Event.PopFixedString();
				string text10 = Event.PopFixedString();
				string text11 = "";
				if (num2 == 10 || num2 == 8)
				{
					text11 = string.Concat(new object[]
					{
						text,
						"=",
						text2,
						Convert.ToChar(9),
						text3,
						"=",
						text4,
						Convert.ToChar(9),
						text5,
						"=",
						text6,
						Convert.ToChar(9),
						text7,
						"=",
						text8
					});
					if (text9 != "")
					{
						text11 = string.Concat(new object[]
						{
							text11,
							Convert.ToChar(9),
							text9,
							"=",
							text10
						});
					}
					using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
					{
						class2.AddParamWithValue("extradata", text11);
						class2.ExecuteQuery("UPDATE items_extra_data SET extra_data = @extradata WHERE item_id = '" + num + "' LIMIT 1");
					}
					ServerMessage Message = new ServerMessage(88u);
					Message.AppendStringWithBreak(num.ToString());
					Message.AppendStringWithBreak(text11);
					@class.SendMessage(Message, null);
					@class.method_28((uint)num).ExtraData = text11;
					@class.method_28((uint)num).UpdateState(true, false);
				}
			}
		}
	}
}
