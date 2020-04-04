using System;
using System.Data;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Users
{
	internal sealed class RemoveGuildFavorite : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			if (num > 0 && (Session != null && Session.GetHabbo() != null))
			{
				Session.GetHabbo().int_0 = 0;
                if (Session.GetHabbo().InRoom)
				{
					Room class14_ = Session.GetHabbo().CurrentRoom;
					RoomUser @class = class14_.GetRoomUserByHabbo(Session.GetHabbo().Id);
					ServerMessage Message = new ServerMessage(28u);
					Message.AppendInt32(1);
					@class.method_14(Message);
					class14_.SendMessage(Message, null);
				}
				using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
				{
					class2.ExecuteQuery("UPDATE user_stats SET groupid = 0 WHERE Id = " + Session.GetHabbo().Id + " LIMIT 1;");
				}
				DataTable dataTable_ = Session.GetHabbo().dataTable_0;
				if (dataTable_ != null)
				{
					ServerMessage Message2 = new ServerMessage(915u);
					Message2.AppendInt32(dataTable_.Rows.Count);
					foreach (DataRow dataRow in dataTable_.Rows)
					{
						GroupsManager class3 = Groups.smethod_2((int)dataRow["groupid"]);
						Message2.AppendInt32(class3.int_0);
						Message2.AppendStringWithBreak(class3.string_0);
						Message2.AppendStringWithBreak(class3.string_2);
						if (Session.GetHabbo().int_0 == class3.int_0)
						{
							Message2.AppendBoolean(true);
						}
						else
						{
							Message2.AppendBoolean(false);
						}
					}
					Session.SendMessage(Message2);
				}
			}
		}
	}
}
