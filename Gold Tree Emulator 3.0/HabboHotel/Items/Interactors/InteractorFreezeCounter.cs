using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Items.Interactors;
using GoldTree.HabboHotel.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldTree.HabboHotel.Items.Interactors
{
    class InteractorFreezeCounter : FurniInteractor
    {
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            Item.UpdateState(true, true);
        }

        public override void OnRemove(GameClient Session, RoomItem Item)
        {
            Item.ExtraData = "0";
            Item.UpdateState(true, true);
        }

        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
		{
            if (UserHasRights)
			{
				int num = 0;
                if (Item.ExtraData.Length > 0)
				{
                    num = int.Parse(Item.ExtraData);
				}
                if (Request == 0)
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
                    if (Request >= 1)
					{
                        if (Request == 1)
						{
                            if (!Item.method_8().frzTimer)
							{
                                Item.method_8().frzTimer = true;
                                Item.ReqUpdate(1);
								if (Session != null)
								{
									RoomUser RoomUser_ = Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    Item.method_8().method_14(RoomUser_);
                                    Item.method_8().GetFreeze().StartGame();
								}
							}
							else
							{
								Item.method_8().frzTimer = false;
                                Item.method_8().GetFreeze().StopGame();
							}
						}
						else
						{
                            if (Request == 2)
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
                Item.ExtraData = num.ToString();
                Item.UpdateState(true, true);
			}
		}
	}
}