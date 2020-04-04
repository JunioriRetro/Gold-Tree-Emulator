using System;
using System.Data;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.Messages;
using GoldTree.Storage;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Rooms.Furniture
{
	internal sealed class CreditFurniRedeemMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null && @class.CheckRights(Session, true))
				{
					RoomItem class2 = @class.method_28(Event.PopWiredUInt());
					UserItem class3 = Session.GetHabbo().GetInventoryComponent().GetItemById(class2.uint_0);
					if (class2 != null)
					{
						if (class2.GetBaseItem().Name.StartsWith("CF_") || class2.GetBaseItem().Name.StartsWith("CFC_") || class2.GetBaseItem().Name.StartsWith("PixEx_") || class2.GetBaseItem().Name.StartsWith("PntEx_"))
						{
							if (class3 != null)
							{
								@class.method_29(null, class3.uint_0, true, true);
							}
							else
							{
								DataRow dataRow = null;
								using (DatabaseClient class4 = GoldTree.GetDatabase().GetClient())
								{
									dataRow = class4.ReadDataRow("SELECT ID FROM items WHERE ID = '" + class2.uint_0 + "' LIMIT 1");
								}
								if (dataRow != null)
								{
									string[] array = class2.GetBaseItem().Name.Split(new char[]
									{
										'_'
									});
									int num = int.Parse(array[1]);
									if (num > 0)
									{
										if (class2.GetBaseItem().Name.StartsWith("CF_") || class2.GetBaseItem().Name.StartsWith("CFC_"))
										{
											Session.GetHabbo().Credits += num;
											Session.GetHabbo().UpdateCredits(true);
										}
										else
										{
											if (class2.GetBaseItem().Name.StartsWith("PixEx_"))
											{
												Session.GetHabbo().ActivityPoints += num;
												Session.GetHabbo().UpdateActivityPoints(true);
											}
											else
											{
												if (class2.GetBaseItem().Name.StartsWith("PntEx_"))
												{
													Session.GetHabbo().VipPoints += num;
													Session.GetHabbo().UpdateVipPoints(false, true);
												}
											}
										}
									}
								}
								@class.method_29(null, class2.uint_0, true, true);
								ServerMessage Message5_ = new ServerMessage(219u);
								Session.SendMessage(Message5_);
							}
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
