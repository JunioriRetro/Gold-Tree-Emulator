using System;
namespace GoldTree.HabboHotel.Support
{
	internal sealed class HelpCategory
	{
		private uint Id;
		public string Caption;
		public uint CategoryId
		{
			get
			{
				return this.Id;
			}
		}
		public HelpCategory(uint Id, string Caption)
		{
			this.Id = Id;
			this.Caption = Caption;
		}
	}
}
