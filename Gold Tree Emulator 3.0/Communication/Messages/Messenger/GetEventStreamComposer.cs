using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using System.Data;
namespace GoldTree.Communication.Messages.Messenger
{
    internal sealed class GetEventStreamComposer : Interface
	{
		public void Handle(GameClient session, ClientMessage message)
		{
            ServerMessage response = new ServerMessage(950u);

            session.GetHabbo().GetUserDataFactory().UpdateFriendStream();

            int streamCount = session.GetHabbo().GetUserDataFactory().GetFriendStream().Rows.Count;

            DataTable dataTable_ = session.GetHabbo().GetUserDataFactory().GetFriendStream();

            foreach (DataRow row in dataTable_.Rows)
            {
                int type = (int)row["type"];

                if (type == 1)
                {
                    DataRow[] tmpRow = session.GetHabbo().GetUserDataFactory().GetFriends().Select("id = " + (uint)row["userid"]);

                    uint userid = (uint)row["userid"];
                    string username = (string)tmpRow[0]["username"];

                    string gender = (string)row["gender"].ToString().ToLower();
                    string look = (string)row["look"];

                    int time = (int)((GoldTree.GetUnixTimestamp() - (double)row["time"]) / 60);

                    string data = (string)row["data"];

                    response.AppendInt32(streamCount);

                    response.AppendUInt(1u);

                    response.AppendInt32(type);

                    response.AppendStringWithBreak(userid.ToString());
                    response.AppendStringWithBreak(username);

                    response.AppendStringWithBreak(gender);
                    response.AppendStringWithBreak("http://127.0.0.1/retro/r63/c_images/friendstream/index.gif?figure=" + look + ".gif");

                    response.AppendInt32WithBreak(time);

                    response.AppendInt32WithBreak(type + 1);

                    uint roomId;

                    RoomData RoomData;

                    if (uint.TryParse(data, out roomId))
                        RoomData = GoldTree.GetGame().GetRoomManager().method_12(roomId);
                    else
                        RoomData = GoldTree.GetGame().GetRoomManager().method_12(0);

                    if (RoomData != null)
                    {
                        response.AppendStringWithBreak(RoomData.Id.ToString()); //data
                        response.AppendStringWithBreak(RoomData.Name); //extra data
                    }
                    else
                    {
                        response.AppendStringWithBreak("");
                        response.AppendStringWithBreak("Room deleted");
                    }
                }
            }

            session.SendMessage(response);
		}
	}
}
