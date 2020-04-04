using System;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Rooms.Session
{
	internal sealed class OpenFlatConnectionMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint num = Event.PopWiredUInt();
			string string_ = Event.PopFixedString();
			Event.PopWiredInt32();
			if (GoldTree.GetConfig().data["emu.messages.roommgr"] == "1")
			{
				Logging.WriteLine("[RoomMgr] Requesting Private Room [ID: " + num + "]");
			}
			Session.GetClientMessageHandler().method_5(num, string_);
		}
	}
}
