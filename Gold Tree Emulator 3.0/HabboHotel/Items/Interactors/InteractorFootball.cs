using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorFootball : FurniInteractor
	{
		public override void OnPlace(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnRemove(GameClient Session, RoomItem RoomItem_0)
		{
		}
		public override void OnTrigger(GameClient Session, RoomItem RoomItem_0, int int_0, bool bool_0)
		{
			if (Session != null)
			{
				RoomUser @class = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
				Room class2 = RoomItem_0.method_8();
				if (RoomItem_0.method_8().method_99(RoomItem_0.Int32_0, RoomItem_0.Int32_1, @class.int_3, @class.int_4))
				{
					RoomItem_0.method_8().method_10(@class, RoomItem_0);
					int num = RoomItem_0.Int32_0;
					int num2 = RoomItem_0.Int32_1;
					RoomItem_0.ExtraData = "11";
					if (@class.int_8 == 4)
					{
						num2--;
					}
					else
					{
						if (@class.int_8 == 0)
						{
							num2++;
						}
						else
						{
							if (@class.int_8 == 6)
							{
								num++;
							}
							else
							{
								if (@class.int_8 == 2)
								{
									num--;
								}
								else
								{
									if (@class.int_8 == 3)
									{
										num--;
										num2--;
									}
									else
									{
										if (@class.int_8 == 1)
										{
											num--;
											num2++;
										}
										else
										{
											if (@class.int_8 == 7)
											{
												num++;
												num2++;
											}
											else
											{
												if (@class.int_8 == 5)
												{
													num++;
													num2--;
												}
											}
										}
									}
								}
							}
						}
					}
					@class.MoveTo(RoomItem_0.Int32_0, RoomItem_0.Int32_1);
					class2.method_79(null, RoomItem_0, num, num2, 0, false, true, true);
				}
			}
		}
	}
}
