using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Items.Interactors
{
	internal sealed class InteractorBanzaiScoreCounter : FurniInteractor
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
				int num = 0;
				if (RoomItem_0.ExtraData.Length > 0)
				{
					num = int.Parse(RoomItem_0.ExtraData);
				}
				if (int_0 == 0)
				{
					if (num <= -1)
					{
						num = 0;
					}
					else
					{
						if (num >= 0)
						{
							num = -1;
						}
					}
				}
				else
				{
					if (int_0 >= 1)
					{
						if (int_0 == 1)
						{
							if (!RoomItem_0.bool_0)
							{
								RoomItem_0.bool_0 = true;
								RoomItem_0.ReqUpdate(1);
								if (Session != null)
								{
									RoomUser RoomUser_ = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
									RoomItem_0.method_8().method_14(RoomUser_);
                                    foreach (RoomItem Item in RoomItem_0.method_8().Hashtable_0.Values)
                                    {
                                        if (Item.GetBaseItem().Name == "bb_apparatus")
                                        {
                                            Item.ExtraData = "1";
                                            Item.UpdateState(false, true);
                                            Item.ReqUpdate(1);
                                        }
                                    }
								}
							}
							else
							{
								RoomItem_0.bool_0 = false;

                                foreach (RoomItem Item in RoomItem_0.method_8().Hashtable_0.Values)
                                {
                                    if (Item.GetBaseItem().Name == "bb_apparatus")
                                    {
                                        Item.ExtraData = "0";
                                        Item.UpdateState(false, true);
                                        Item.ReqUpdate(1);
                                    }
                                }
							}
						}
						else
						{
							if (int_0 == 2)
							{
								num += 60;
								if (num >= 600)
								{
									num = 0;
								}
							}
						}
					}
				}
				RoomItem_0.ExtraData = num.ToString();
				RoomItem_0.UpdateState(true, true);
			}
		}
	}
}
