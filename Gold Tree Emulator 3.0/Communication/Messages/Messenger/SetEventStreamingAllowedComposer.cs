using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoldTree.Communication.Messages.Messenger
{
    class SetEventStreamingAllowedComposer : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                bool Enabled = Event.PopBase64Boolean();
                Session.GetHabbo().FriendStreamEnabled = Enabled;
                using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                {
                    @class.AddParamWithValue("user_id", Session.GetHabbo().Id);
                    @class.ExecuteQuery("UPDATE users SET friend_stream_enabled = '" + (Enabled ? 1 : 0) + "' WHERE Id = @user_id LIMIT 1;");
                }
            }
        }
    }
}