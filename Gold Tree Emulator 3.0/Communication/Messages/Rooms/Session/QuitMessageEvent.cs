using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Rooms.Session
{
	internal sealed class QuitMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
                if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().InRoom)
				{
					GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).method_47(Session, true, false);
				}
			}
			catch
			{
			}
		}
	}
}
