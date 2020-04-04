using System;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Support
{
	internal sealed class SupportTicket
	{
		private uint Id;
		public int Score;
		public int Type;
		public TicketStatus Status;
		public uint SenderId;
		public uint ReportedId;
		public uint ModeratorId;
		public string Message;
		public uint RoomId;
		public string RoomName;
		public double Timestamp;
		private string string_2;
		private string string_3;
		private string string_4;
		public int TabId
		{
			get
			{
				int result;
				if (this.Status == TicketStatus.OPEN)
				{
					result = 1;
				}
				else
				{
					if (this.Status == TicketStatus.PICKED)
					{
						result = 2;
					}
					else
					{
						result = 0;
					}
				}
				return result;
			}
		}
		public uint UInt32_0
		{
			get
			{
				return this.Id;
			}
		}
		public SupportTicket(uint mId, int mScore, int mType, uint mSenderId, uint mReportedId, string mMessage, uint mRoomId, string mRoomName, double mTimestamp, uint mModeratorId)
		{
			this.Id = mId;
			this.Score = mScore;
			this.Type = mType;
			this.Status = TicketStatus.OPEN;
			this.SenderId = mSenderId;
			this.ReportedId = mReportedId;
			this.ModeratorId = mModeratorId;
			this.Message = mMessage;
			this.RoomId = mRoomId;
			this.RoomName = mRoomName;
			this.Timestamp = mTimestamp;
			this.string_2 = GoldTree.GetGame().GetClientManager().GetNameById(mSenderId);
			this.string_3 = GoldTree.GetGame().GetClientManager().GetNameById(mReportedId);
			this.string_4 = GoldTree.GetGame().GetClientManager().GetNameById(mModeratorId);
		}
		public void method_0(uint ModeratorId, bool UpdateInDb)
		{
			this.Status = TicketStatus.PICKED;
			this.ModeratorId = ModeratorId;
			this.string_4 = GoldTree.GetGame().GetClientManager().GetNameById(ModeratorId);
			if (UpdateInDb)
			{
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					@class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE moderation_tickets SET status = 'picked', moderator_id = '",
						ModeratorId,
						"' WHERE Id = '",
						this.Id,
						"' LIMIT 1"
					}));
				}
			}
		}
        public void Close(TicketStatus NewStatus, bool UpdateInDb)
        {
            String dbType = null;
            this.Status = NewStatus;
            if (UpdateInDb)
            {
                switch (NewStatus)
                {
                    case TicketStatus.RESOLVED:
                        {
                            dbType = "resolved";
                            break;
                        }
                    case TicketStatus.ABUSIVE:
                        {
                            dbType = "abusive";
                            break;
                        }
                    case TicketStatus.INVALID:
                        {
                            dbType = "invalid";
                            break;
                        }
                }

                using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                {
                    @class.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE moderation_tickets SET status = '",
						dbType,
						"' WHERE Id = '",
						this.Id,
						"' LIMIT 1"
					}));
                }
            }
        }
		public void Release(bool UpdateInDb)
		{
			this.Status = TicketStatus.OPEN;
			if (UpdateInDb)
			{
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					@class.ExecuteQuery("UPDATE moderation_tickets SET status = 'open' WHERE Id = '" + this.Id + "' LIMIT 1");
				}
			}
		}
		public void Delete(bool UpdateInDb)
		{
			this.Status = TicketStatus.DELETED;
			if (UpdateInDb)
			{
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					@class.ExecuteQuery("UPDATE moderation_tickets SET status = 'deleted' WHERE Id = '" + this.Id + "' LIMIT 1");
				}
			}
		}
		public ServerMessage Serialize()
		{
			ServerMessage Message = new ServerMessage(530u);
			Message.AppendUInt(this.Id);
			Message.AppendInt32(this.TabId);
			Message.AppendInt32(11);
			Message.AppendInt32(this.Type);
			Message.AppendInt32(11);
			Message.AppendInt32(this.Score);
			Message.AppendUInt(this.SenderId);
			Message.AppendStringWithBreak(this.string_2);
			Message.AppendUInt(this.ReportedId);
			Message.AppendStringWithBreak(this.string_3);
			Message.AppendUInt(this.ModeratorId);
			Message.AppendStringWithBreak(this.string_4);
			Message.AppendStringWithBreak(this.Message);
			Message.AppendUInt(this.RoomId);
			Message.AppendStringWithBreak(this.RoomName);
			return Message;
		}
	}
}
