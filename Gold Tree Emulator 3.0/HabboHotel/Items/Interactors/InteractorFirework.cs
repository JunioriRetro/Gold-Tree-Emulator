using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using GoldTree.Storage;
using GoldTree.Messages;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Pathfinding;
using GoldTree.HabboHotel.Rooms;
using System.Threading;
using System.Threading.Tasks;
using GoldTree.HabboHotel.Items.Interactors;
using GoldTree.HabboHotel.Items;

namespace GoldTree.HabboHotel.Items.Interactors
{
    class InteractorFirework : FurniInteractor
    {
        int Fireworks = 10;
        int Pixels = 20;
        public override void OnPlace(GameClient Session, RoomItem Item)
        {
            DataRow dataRow2;
            using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
            {
                dataRow2 = @class.ReadDataRow("SELECT fw_count FROM items_firework WHERE id = '" + Item.uint_0 + "'");

                if (dataRow2 != null)
                {
                    Item.FireWorkCount = (int)dataRow2["fw_count"];
                }
                else
                {
                    @class.ExecuteQuery("INSERT INTO items_firework(item_id, fw_count) VALUES ( '" + Item.uint_0 + "', '0')");
                }
            }

            if (Item.FireWorkCount > 0)
            {
                Item.ExtraData = "1";
                Item.UpdateState(true, true);
            }

            else if (Item.FireWorkCount <= 0)
            {
                Item.ExtraData = "0";
                Item.UpdateState(true, true);
            }
        }

        public override void OnRemove(GameClient Session, RoomItem Item)
        {
            DataRow dataRow2;
            using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
            {
                dataRow2 = @class.ReadDataRow("SELECT fw_count FROM items_firework WHERE id = '" + Item.uint_0 + "'");

                if (dataRow2 != null)
                {
                    Item.FireWorkCount = (int)dataRow2["fw_count"];
                }
                else
                {
                    @class.ExecuteQuery("INSERT INTO items_firework(item_id, fw_count) VALUES ( '" + Item.uint_0 + "', '0')");
                }
            }

            if (Item.FireWorkCount > 0)
            {
                Item.ExtraData = "1";
                Item.UpdateState(true, true);
            }

            else if (Item.FireWorkCount <= 0)
            {
                Item.ExtraData = "0";
                Item.UpdateState(true, true);
            }
        }

        public override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            RoomUser User = Item.method_8().GetRoomUserByHabbo((uint)Session.GetHabbo().Id);

            if (User == null)
            {
                return;
            }

            if (Request == 0 && Item.FireWorkCount > 0 && Item.ExtraData == "1")
            {
                Item.ExtraData = "2";
                Item.UpdateState(false, true);

                if (Item.FireWorkCount > 0)
                {
                    Task T = null;
                    T = new Task(delegate() { Action(Item, T, "1"); });
                    T.Start();
                }
                else
                {
                    Task T = null;
                    T = new Task(delegate() { Action(Item, T, "0"); });
                    T.Start();
                }
            }
            else if (Request == 1)
            {
                ServerMessage PixelMessage = new ServerMessage(629);
                PixelMessage.AppendUInt(Item.uint_0);
                PixelMessage.AppendInt32(Item.FireWorkCount);
                PixelMessage.AppendBoolean(false);
                PixelMessage.AppendInt32(Pixels); // pixels :D
                PixelMessage.AppendBoolean(false);
                PixelMessage.AppendInt32(Fireworks); // 10 fireworks :D
                Session.SendMessage(PixelMessage);
            }
            else if (Request == 2 && Session.GetHabbo().ActivityPoints >= 20)
            {
                Item.FireWorkCount += Fireworks;

                Session.GetHabbo().FireworkPixelLoadedCount += Pixels;
                Session.GetHabbo().ActivityPoints -= 20;
                Session.GetHabbo().UpdateActivityPoints(true);

                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    dbClient.AddParamWithValue("itemid", Item.uint_0);
                    dbClient.AddParamWithValue("sessionid", Session.GetHabbo().Id);
                    dbClient.ExecuteQuery("UPDATE items_firework SET fw_count = fw_count + '" + Fireworks + "' WHERE item_id = @itemid LIMIT 1");
                    dbClient.ExecuteQuery("UPDATE user_stats SET fireworks = fireworks + '" + Pixels + "' WHERE id = @sessionid LIMIT 1");
                }

                ServerMessage PixelMessage = new ServerMessage(629);
                PixelMessage.AppendUInt(Item.uint_0);
                PixelMessage.AppendInt32(Item.FireWorkCount);
                PixelMessage.AppendBoolean(false);
                PixelMessage.AppendInt32(Pixels); // pixels :D
                PixelMessage.AppendBoolean(false);
                PixelMessage.AppendInt32(Fireworks); // 10 fireworks :D
                Session.SendMessage(PixelMessage);

                if (Item.ExtraData == "0")
                {
                    Item.ExtraData = "1";
                    Item.UpdateState(true, true);
                }

                Session.GetHabbo().CheckFireworkAchievements();
            }
        }

        public void Action(RoomItem Item, Task Task, string ExtraData)
        {
            if (ExtraData == "1")
            {
                Task.Wait(7000);
                Item.FireWorkCount--;

                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    dbClient.AddParamWithValue("itemid", Item.uint_0);
                    dbClient.ExecuteQuery("UPDATE items_firework SET fw_count = fw_count - 1 WHERE item_id = @itemid LIMIT 1");
                }
            }

            if (Item.FireWorkCount == 0)
            {
                ExtraData = "0";
            }

            Item.ExtraData = ExtraData;
            Item.UpdateState(true, true);

            Task.Wait();
            Task.Dispose();
        }
    }
}
