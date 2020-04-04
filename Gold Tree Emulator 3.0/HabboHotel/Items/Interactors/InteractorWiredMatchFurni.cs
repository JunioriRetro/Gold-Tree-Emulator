using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredMatchFurni : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem Item)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem Item)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
		{
			if (UserHasRights && Session != null)
			{
				Item.method_9();
				ServerMessage Message = new ServerMessage(651u);
				Message.AppendInt32(0);
				Message.AppendInt32(5);
				if (Item.string_5.Length > 0)
				{
					Message.AppendString(Item.string_5);
				}
				else
				{
					Message.AppendInt32(0);
				}
				Message.AppendInt32(Item.GetBaseItem().Sprite);
				Message.AppendUInt(Item.uint_0);
				Message.AppendStringWithBreak("");
				Message.AppendString("K");
				if (Item.string_3.Length > 0)
				{
					Message.AppendString(Item.string_3);
				}
				else
				{
					Message.AppendString("HHH");
				}
				Message.AppendString("IK");
				if (Item.string_6.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(Item.string_6));
				}
				else
				{
					Message.AppendInt32(0);
				}
				Message.AppendStringWithBreak("H");
				Session.SendMessage(Message);
			}
		}
	}
}
