using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GoldTree.Communication.Messages.Rooms.Polls
{
    class ShowRoomPoll : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            int PollId = Event.PopWiredInt32();
            int PollQuestion = Event.PopWiredInt32();
            string PollTitle;
            string PollThanks;
            int QuestionsCount;

            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
            {
                PollTitle = dbClient.ReadString("SELECT title FROM room_polls WHERE id = '" + PollId + "' LIMIT 1");
                PollThanks = dbClient.ReadString("SELECT thanks FROM room_polls WHERE id = '" + PollId + "' LIMIT 1");
                QuestionsCount = dbClient.ReadInt32("SELECT COUNT(*) FROM room_poll_questions WHERE poll_id = '" + PollId + "'");
            }
            if (QuestionsCount <= 0)
            {
                ServerMessage ShortPoll = new ServerMessage(317);
                ShortPoll.AppendInt32(PollId);
                ShortPoll.AppendStringWithBreak(PollTitle);
                ShortPoll.AppendStringWithBreak("The poll was so short, because the query author has not put any question to the poll!");
                ShortPoll.AppendInt32(QuestionsCount);
                ShortPoll.AppendInt32((int)Session.GetHabbo().CurrentRoomId);
                ShortPoll.AppendInt32(0);
                ShortPoll.AppendInt32(0);
                Session.SendMessage(ShortPoll);
                return;
            }
            DataTable Data = null;
            string QuestionTitle;
            int QuestionTypeInt;
            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
            {
                Data = dbClient.ReadDataTable("SELECT * FROM room_poll_questions WHERE poll_id = '" + PollId + "'");

                QuestionTitle = dbClient.ReadString("SELECT title FROM room_poll_questions WHERE poll_id = '" + PollId + "'");

            }
            QuestionTypeInt = 1;

            int PollStep = 0;
            ServerMessage ShowPoll = new ServerMessage(317);
            ShowPoll.AppendInt32(PollId);
            ShowPoll.AppendStringWithBreak(PollTitle);
            ShowPoll.AppendStringWithBreak(PollThanks);
            ShowPoll.AppendInt32(Data.Rows.Count);
            foreach (DataRow Row in Data.Rows)
            {
                PollStep++;

                switch ((string)Row["type"])
                {
                    case "radio":
                        QuestionTypeInt = 1;
                        break;

                    case "checkbox":
                        QuestionTypeInt = 2;
                        break;

                    case "textbox":
                        QuestionTypeInt = 3;
                        break;
                }

                ShowPoll.AppendInt32((int)Row["id"]);
                ShowPoll.AppendInt32(PollStep);
                ShowPoll.AppendInt32(QuestionTypeInt);
                ShowPoll.AppendStringWithBreak((string)Row["title"]);
                if (QuestionTypeInt != 3)
                {
                    DataTable QuestionAnswers = null;
                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                    {
                        QuestionAnswers = dbClient.ReadDataTable("SELECT * FROM room_poll_answers WHERE question_id = '" + (int)Row["id"] + "'");
                    }

                    ShowPoll.AppendInt32(QuestionAnswers.Rows.Count);
                    ShowPoll.AppendInt32((int)Row["min_selects"]);
                    ShowPoll.AppendInt32(QuestionAnswers.Rows.Count);
                    foreach (DataRow Answer in QuestionAnswers.Rows)
                    {
                        ShowPoll.AppendStringWithBreak((string)Answer["answer"]);
                    }

                }
            }

            Session.SendMessage(ShowPoll);
        }
    }
}
