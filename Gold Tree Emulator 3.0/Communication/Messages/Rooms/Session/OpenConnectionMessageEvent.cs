using System;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Rooms.Session
{
	internal sealed class OpenConnectionMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Event.PopWiredInt32();
			uint num = Event.PopWiredUInt();
			Event.PopWiredInt32();
			if (GoldTree.GetConfig().data["emu.messages.roommgr"] == "1")
			{
				Logging.WriteLine("[RoomMgr] Requesting Public Room [ID: " + num + "]");
			}
            RoomData @class = GoldTree.GetGame().GetRoomManager().method_12(num);
			if (@class != null && !(@class.Type != "public"))
			{
				Session.GetClientMessageHandler().method_5(num, "");
			}
		}
	}
}
