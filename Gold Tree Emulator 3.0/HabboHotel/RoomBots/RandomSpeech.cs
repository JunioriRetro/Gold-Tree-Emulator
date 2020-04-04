using System;
namespace GoldTree.HabboHotel.RoomBots
{
	internal sealed class RandomSpeech
	{
		internal string Message;
		internal bool Shout;
		internal uint Id;
		public RandomSpeech(string Message, bool Shout, uint Id)
		{
			this.Id = Id;
			this.Message = Message;
			this.Shout = Shout;
		}
	}
}
