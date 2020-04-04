using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorWiredMoveRotate : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (bool_0)
			{
				RoomItem_0.method_9();
				ServerMessage Message = new ServerMessage(651u);
				Message.AppendInt32(0);
				Message.AppendInt32(5);
				if (RoomItem_0.string_5.Length > 0)
				{
					Message.AppendString(RoomItem_0.string_5);
				}
				else
				{
					Message.AppendInt32(0);
				}
				Message.AppendInt32(RoomItem_0.GetBaseItem().Sprite);
				Message.AppendUInt(RoomItem_0.uint_0);
				Message.AppendStringWithBreak("");
				Message.AppendString("J");
				if (RoomItem_0.string_2.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(RoomItem_0.string_2));
				}
				else
				{
					Message.AppendInt32(0);
				}
				if (RoomItem_0.string_3.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(RoomItem_0.string_3));
				}
				else
				{
					Message.AppendInt32(0);
				}
				Message.AppendString("HPA");
				if (RoomItem_0.string_6.Length > 0)
				{
					Message.AppendInt32(Convert.ToInt32(RoomItem_0.string_6));
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
