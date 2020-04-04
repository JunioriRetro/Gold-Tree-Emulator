using System;
using System.Collections.Generic;
using System.Text;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class UpdateRoomThumbnailMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true))
			{
				Event.PopWiredInt32();
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				int num = Event.PopWiredInt32();
				int num2 = Event.PopWiredInt32();
				int num3 = Event.PopWiredInt32();
				for (int i = 0; i < num3; i++)
				{
					int num4 = Event.PopWiredInt32();
					int num5 = Event.PopWiredInt32();
					if (num4 < 0 || num4 > 10 || (num5 < 1 || num5 > 27) || dictionary.ContainsKey(num4))
					{
						return;
					}
					dictionary.Add(num4, num5);
				}
				if (num >= 1 && num <= 24 && (num2 >= 0 && num2 <= 11))
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num6 = 0;
					foreach (KeyValuePair<int, int> current in dictionary)
					{
						if (num6 > 0)
						{
							stringBuilder.Append("|");
						}
						stringBuilder.Append(current.Key + "," + current.Value);
						num6++;
					}
					using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
					{
						class2.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE rooms SET icon_bg = '",
							num,
							"', icon_fg = '",
							num2,
							"', icon_items = '",
							stringBuilder.ToString(),
							"' WHERE Id = '",
							@class.Id,
							"' LIMIT 1"
						}));
					}
					@class.RoomIcon = new RoomIcon(num, num2, dictionary);
					ServerMessage Message = new ServerMessage(457u);
					Message.AppendUInt(@class.Id);
					Message.AppendBoolean(true);
					Session.SendMessage(Message);
					ServerMessage Message2 = new ServerMessage(456u);
					Message2.AppendUInt(@class.Id);
					Session.SendMessage(Message2);
					RoomData class27_ = @class.RoomData;
					ServerMessage Message3 = new ServerMessage(454u);
					Message3.AppendBoolean(false);
					class27_.method_3(Message3, false, false);
					Session.SendMessage(Message3);
				}
			}
		}
	}
}
