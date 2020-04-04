using System;
using GoldTree.Messages;
namespace GoldTree.HabboHotel.Users.Messenger
{
	internal sealed class MessengerRequest
	{
		private uint xRequestId;
		private uint ToUser;
		private uint FromUser;
		private string SenderUsername;
        private string Gender;
        private string Figure;
		internal uint RequestId
		{
			get
			{
				return this.FromUser;
			}
		}
		internal uint To
		{
			get
			{
				return this.ToUser;
			}
		}
		internal uint From
		{
			get
			{
				return this.FromUser;
			}
		}
		internal string senderUsername
		{
			get
			{
				return this.SenderUsername;
			}
		}
        internal string SenderGender
        {
            get
            {
                return this.Gender;
            }
        }
        internal string SenderFigure
        {
            get
            {
                return this.Figure;
            }
        }
		public MessengerRequest(uint RequestId, uint ToUser, uint FromUser, string SenderUsername, string Gender, string Figure)
		{
			this.xRequestId = RequestId;
			this.ToUser = ToUser;
			this.FromUser = FromUser;
			this.SenderUsername = SenderUsername;
            this.Gender = Gender;
            this.Figure = Figure;
		}
		public void method_0(ServerMessage Message5_0)
		{
			Message5_0.AppendUInt(this.FromUser);
			Message5_0.AppendStringWithBreak(this.SenderUsername);
			Message5_0.AppendStringWithBreak(this.FromUser.ToString());
		}
	}
}
