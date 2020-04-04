using System;
using System.Data;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Users
{
	internal sealed class GetHabboGroupDetailsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			if (num > 0 && (Session != null && Session.GetHabbo() != null))
			{
				GroupsManager @class = Groups.smethod_2(num);
				if (@class != null)
				{
					ServerMessage Message = new ServerMessage(311u);
					Message.AppendInt32(@class.int_0);
					Message.AppendStringWithBreak(@class.string_0);
					Message.AppendStringWithBreak(@class.string_1);
					Message.AppendStringWithBreak(@class.string_2);
					if (@class.uint_0 > 0u)
					{
						Message.AppendUInt(@class.uint_0);
						if (GoldTree.GetGame().GetRoomManager().GetRoom(@class.uint_0) != null)
						{
							Message.AppendStringWithBreak(GoldTree.GetGame().GetRoomManager().GetRoom(@class.uint_0).Name);
							goto IL_15A;
						}
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							try
							{
								DataRow dataRow_ = class2.ReadDataRow("SELECT * FROM rooms WHERE Id = " + @class.uint_0 + " LIMIT 1;");
								string string_ = GoldTree.GetGame().GetRoomManager().method_17(@class.uint_0, dataRow_).Name;
								Message.AppendStringWithBreak(string_);
							}
							catch
							{
								Message.AppendInt32(-1);
								Message.AppendStringWithBreak("");
							}
							goto IL_15A;
						}
					}
					Message.AppendInt32(-1);
					Message.AppendStringWithBreak("");
					IL_15A:
					bool flag = false;
					foreach (DataRow dataRow in Session.GetHabbo().dataTable_0.Rows)
					{
						if ((int)dataRow["groupid"] == @class.int_0)
						{
							flag = true;
						}
					}
					if (Session.GetHabbo().list_0.Contains(@class.int_0))
					{
						Message.AppendInt32(2);
					}
					else
					{
						if (flag)
						{
							Message.AppendInt32(1);
						}
						else
						{
							if (@class.string_3 == "closed")
							{
								Message.AppendInt32(1);
							}
							else
							{
								if (@class.list_0.Contains((int)Session.GetHabbo().Id))
								{
									Message.AppendInt32(1);
								}
								else
								{
									Message.AppendInt32(0);
								}
							}
						}
					}
					Message.AppendInt32(@class.list_0.Count);
					if (Session.GetHabbo().int_0 == @class.int_0)
					{
						Message.AppendBoolean(true);
					}
					else
					{
						Message.AppendBoolean(false);
					}
					Session.SendMessage(Message);
				}
			}
		}
	}
}
