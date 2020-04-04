using System;
using System.Collections.Generic;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class CreateEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true) && @class.Event == null && @class.State == 0)
			{
				int int_ = Event.PopWiredInt32();
				string text = GoldTree.FilterString(Event.PopFixedString());
				string string_ = GoldTree.FilterString(Event.PopFixedString());
				int num = Event.PopWiredInt32();
				if (text.Length >= 1)
				{
					@class.Event = new RoomEvent(@class.Id, text, string_, int_, null);
					@class.Event.Tags = new List<string>();
					for (int i = 0; i < num; i++)
					{
						@class.Event.Tags.Add(GoldTree.FilterString(Event.PopFixedString()));
					}
					@class.SendMessage(@class.Event.Serialize(Session), null);
				}
			}
		}
	}
}
