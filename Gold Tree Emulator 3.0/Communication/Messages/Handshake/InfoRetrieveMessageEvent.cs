using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Handshake
{
	internal sealed class InfoRetrieveMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			ServerMessage Message = new ServerMessage(5u);
			Message.AppendStringWithBreak(Session.GetHabbo().Id.ToString());
			Message.AppendStringWithBreak(Session.GetHabbo().Username);
			Message.AppendStringWithBreak(Session.GetHabbo().Figure);
			Message.AppendStringWithBreak(Session.GetHabbo().Gender.ToUpper());
			Message.AppendStringWithBreak(Session.GetHabbo().Motto);
			Message.AppendStringWithBreak(Session.GetHabbo().RealName);
			Message.AppendBoolean(false);
			Message.AppendInt32(Session.GetHabbo().Respect);
			Message.AppendInt32(Session.GetHabbo().RespectPoints);
			Message.AppendInt32(Session.GetHabbo().PetRespectPoints);
			Message.AppendBoolean(Session.GetHabbo().FriendStreamEnabled);
			Session.SendMessage(Message);
		}
	}
}
