using System;
using System.Data;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Users
{
	internal sealed class LoadUserGroupsEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			DataTable dataTable_ = Session.GetHabbo().dataTable_0;
			if (dataTable_ != null)
			{
				ServerMessage Message = new ServerMessage(915u);
				Message.AppendInt32(dataTable_.Rows.Count);
				foreach (DataRow dataRow in dataTable_.Rows)
				{
					GroupsManager @class = Groups.smethod_2((int)dataRow["groupid"]);
					Message.AppendInt32(@class.int_0);
					Message.AppendStringWithBreak(@class.string_0);
					Message.AppendStringWithBreak(@class.string_2);
					if (Session.GetHabbo().int_0 == @class.int_0) // is favorite group?
					{
						Message.AppendBoolean(true);
					}
					else
					{
						Message.AppendBoolean(false);
					}
				}
				Session.SendMessage(Message);
			}
		}
	}
}
