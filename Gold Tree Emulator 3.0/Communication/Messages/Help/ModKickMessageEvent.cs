using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
namespace GoldTree.Communication.Messages.Help
{
	internal sealed class ModKickMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().HasFuse("acc_supporttool"))
			{
				uint num = Event.PopWiredUInt();
				string text = Event.PopFixedString();
				string string_ = string.Concat(new object[]
				{
					"User: ",
					num,
					", Message: ",
					text
				});
				GoldTree.GetGame().GetClientManager().method_31(Session, "ModTool - Kick User", string_);
				GoldTree.GetGame().GetModerationTool().method_15(Session, num, text, false);
			}
		}
	}
}
