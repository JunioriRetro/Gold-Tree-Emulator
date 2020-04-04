using System;
using System.Collections.Generic;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Navigator
{
	internal sealed class EditEventMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && @class.CheckRights(Session, true) && @class.Event != null)
			{
				int int_ = Event.PopWiredInt32();
				string string_ = GoldTree.FilterString(Event.PopFixedString());
				string string_2 = GoldTree.FilterString(Event.PopFixedString());
				int num = Event.PopWiredInt32();
				@class.Event.Category = int_;
				@class.Event.Name = string_;
				@class.Event.Description = string_2;
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
