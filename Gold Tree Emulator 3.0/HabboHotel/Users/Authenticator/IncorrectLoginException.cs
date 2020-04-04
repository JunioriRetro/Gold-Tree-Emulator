using System;
namespace GoldTree.HabboHotel.Users.Authenticator
{
    [Serializable]
	public class IncorrectLoginException : Exception
	{
		public IncorrectLoginException(string Reason) : base(Reason)
		{
		}
	}
}
