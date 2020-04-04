using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTree.Communication.Messages.FriendStream
{
    class EventStreamingLikeButton : Interface
    {
        public void Handle(GameClient session, ClientMessage message)
        {
            int id = message.PopWiredInt32();
            int userid = message.PopWiredInt32();

            using (DatabaseClient dbclient = GoldTree.GetDatabase().GetClient())
            {
                DataRow datarow = dbclient.ReadDataRow("SELECT id FROM friend_stream_likes WHERE friend_stream_id = '" + id + "' AND userid = '" + userid + "' LIMIT 1");

                if (datarow == null)
                {
                    @dbclient.ExecuteQuery("INSERT INTO friend_stream_likes (friend_stream_id, userid) VALUES ('" + id + "', '" + userid + "')");
                }
                else
                {
                    session.SendNotification("You cant like twice!");
                }
            }
        }
    }
}
