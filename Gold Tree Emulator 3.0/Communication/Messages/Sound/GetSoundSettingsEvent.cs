using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Sound
{
	internal sealed class GetSoundSettingsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(308u);
			Message.AppendInt32(Session.GetHabbo().Volume);
			Message.AppendBoolean(false);
			Session.SendMessage(Message);
		}
	}
}
