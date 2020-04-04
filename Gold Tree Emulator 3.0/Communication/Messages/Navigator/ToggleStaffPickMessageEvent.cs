using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Storage;
using GoldTree.Util;
namespace GoldTree.Communication.Messages.Navigator
{
    internal sealed class ToggleStaffPickMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Room Room = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);

            int AlreadyStaffPicks;
            AlreadyStaffPicks = 0;

            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
            {
                if (dbClient.ReadDataRow("SELECT * FROM navigator_publics WHERE room_id = '" + Room.Id + "'") != null)
                {
                    AlreadyStaffPicks = 1;
                }
            }


            if (AlreadyStaffPicks == 0)
            {
                string Owner;
                int OwnerID;
                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    Owner = dbClient.ReadString("SELECT owner FROM rooms WHERE id = '" + Room.Id + "'");
                    dbClient.ExecuteQuery("INSERT INTO `navigator_publics` (`bannertype`, `caption`, `room_id`, `category_parent_id`, `image`, `image_type`) VALUES ('1', '" + Room.Name + "', '" + Room.Id + "', '" + ServerConfiguration.StaffPicksID + "', 'officialrooms_hq/staffpickfolder.gif', 'external')");
                }

                GameClient RoomOwner = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Owner);
                if (RoomOwner != null)
                {
                    RoomOwner.GetHabbo().StaffPicks++;
                    RoomOwner.GetHabbo().CheckStaffPicksAchievement();
                }
                else
                {
                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                    {
                        try
                        {
                            OwnerID = dbClient.ReadInt32("SELECT id FROM users WHERE username = '" + Owner + "'");
                            dbClient.ExecuteQuery("UPDATE user_stats SET staff_picks = staff_picks + 1 WHERE id = '" + OwnerID + "' LIMIT 1");
                        }
                        catch (Exception)
                        {
                            Session.SendNotification("Room owner is not in database!");
                        }
                    }
                }

                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    GoldTree.GetGame().GetNavigator().method_0(dbClient);
                }

                Session.SendNotification("Room added to Staff Picks successfully.");

            }
            else
            {
                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {

                    dbClient.ExecuteQuery("DELETE FROM `navigator_publics` WHERE (`room_id`='" + Room.Id + "')");
                }

                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    GoldTree.GetGame().GetNavigator().method_0(dbClient);
                }

                Session.SendNotification("Room removed from Staff Picks successfully.");
            }
        }
    }
}
