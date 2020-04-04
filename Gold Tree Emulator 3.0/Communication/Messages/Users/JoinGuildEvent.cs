using System;
using System.Data;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Users
{
	internal sealed class JoinGuildEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			int num = Event.PopWiredInt32();
			if (num > 0 && (Session != null && Session.GetHabbo() != null))
			{
				GroupsManager @class = Groups.smethod_2(num);
				if (@class != null && !Session.GetHabbo().list_0.Contains(@class.int_0) && !@class.list_0.Contains((int)Session.GetHabbo().Id))
				{
					if (@class.string_3 == "open") 
					{
						@class.method_0((int)Session.GetHabbo().Id);
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO group_memberships (groupid, userid) VALUES (",
								@class.int_0,
								", ",
								Session.GetHabbo().Id,
								");"
							}));
							Session.GetHabbo().dataTable_0 = class2.ReadDataTable("SELECT * FROM group_memberships WHERE userid = " + Session.GetHabbo().Id);
							goto IL_1C4;
						}
					}
					if (@class.string_3 == "locked")
					{
						Session.GetHabbo().list_0.Add(@class.int_0);
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO group_requests (groupid, userid) VALUES (",
								@class.int_0,
								", ",
								Session.GetHabbo().Id,
								");"
							}));
						}
					}
					IL_1C4:
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
								goto IL_2FC;
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
								goto IL_2FC;
							}
						}
						Message.AppendInt32(-1);
						Message.AppendStringWithBreak("");
						IL_2FC:
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
}
