using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
namespace GoldTree.Communication.Messages.Users
{
	internal sealed class UnignoreUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room class14_ = Session.GetHabbo().CurrentRoom;
			if (class14_ != null)
			{
				Event.PopWiredUInt();
				string string_ = Event.PopFixedString();
				RoomUser @class = class14_.method_56(string_);
				if (@class != null)
				{
					uint uint_ = @class.GetClient().GetHabbo().Id;
					if (Session.GetHabbo().list_2.Contains(uint_))
					{
						Session.GetHabbo().list_2.Remove(uint_);
						using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(string.Concat(new object[]
							{
								"DELETE FROM user_ignores WHERE user_id = ",
								Session.GetHabbo().Id,
								" AND ignore_id = ",
								uint_,
								" LIMIT 1;"
							}));
						}
						ServerMessage Message = new ServerMessage(419u);
						Message.AppendInt32(3);
						Session.SendMessage(Message);
					}
				}
			}
		}
	}
}
