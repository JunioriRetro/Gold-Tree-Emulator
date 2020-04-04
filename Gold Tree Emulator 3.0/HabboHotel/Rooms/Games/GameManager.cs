using GoldTree.Collections;
using GoldTree.HabboHotel.Items;
using System;
using System.Collections;
namespace GoldTree.HabboHotel.Rooms.Games
{
    internal class GameManager
    {
        private Room room;
        internal int[] TeamPoints = new int[5];
        private Hashtable freezeGates = new Hashtable();
        private Hashtable freezeScoreboards = new Hashtable();

        public GameManager(Room room)
        {
            this.room = room;
        }

        internal void Destroy()
        {
            Array.Clear(this.TeamPoints, 0, this.TeamPoints.Length);
            this.freezeGates.Clear();
            this.freezeScoreboards.Clear();
            this.freezeScoreboards = null;
            this.freezeGates = null;
            this.TeamPoints = null;
            this.room = null;
        }

        internal Room GetRoom()
        {
            return this.room;
        }

        internal void AddPointToTeam(Team team, RoomUser user)
        {
            this.AddPointToTeam(team, 1, user);
        }

        internal void AddFreezeGate(RoomItem item)
        {
            if (this.freezeGates.Contains(item.uint_0))
            {
                this.freezeGates.Remove(item.uint_0);
            }
            this.freezeGates.Add(item.uint_0, item);
        }

        internal void AddFreezeScoreboard(RoomItem item)
        {
            if (this.freezeScoreboards.Contains(item.uint_0))
            {
                this.freezeScoreboards.Remove(item.uint_0);
            }
            this.freezeScoreboards.Add(item.uint_0, item);
        }

        internal void RemoveFreezeGate(RoomItem item)
        {
            if (this.freezeGates.Contains(item.uint_0))
            {
                this.freezeGates.Remove(item.uint_0);
            }
        }

        internal void RemoveFreezeScoreboard(RoomItem item)
        {
            if (this.freezeScoreboards.Contains(item.uint_0))
            {
                this.freezeScoreboards.Remove(item.uint_0);
            }
        }

        internal void AddPointToTeam(Team team, int points, RoomUser user)
        {
            int num = this.TeamPoints[(int)team] += points;
            if (num < 0)
            {
                num = 0;
            }
            this.TeamPoints[(int)team] = num;

            foreach (RoomItem item in this.freezeScoreboards.Values)
            {
                if (item.team == team)
                {
                    if (item.GetBaseItem().Name.ToLower() == "es_score_y")
                    {
                        item.ExtraData = this.TeamPoints[(int)team].ToString();
                        item.UpdateState(false, true);
                    }
                }
                if (item.team == team)
                {
                    if (item.GetBaseItem().Name.ToLower() == "es_score_g")
                    {
                        item.ExtraData = this.TeamPoints[(int)team].ToString();
                        item.UpdateState(false, true);
                    }
                }
                if (item.team == team)
                {
                    if (item.GetBaseItem().Name.ToLower() == "es_score_b")
                    {
                        item.ExtraData = this.TeamPoints[(int)team].ToString();
                        item.UpdateState(false, true);
                    }
                }
                if (item.team == team)
                {
                    if (item.GetBaseItem().Name.ToLower() == "es_score_r")
                    {
                        item.ExtraData = this.TeamPoints[(int)team].ToString();
                        item.UpdateState(false, true);
                    }
                }
            }
        }
        internal Team getWinningTeam()
        {
            int num = 1;
            int num2 = 0;
            for (int i = 1; i < 5; i++)
            {
                if (this.TeamPoints[i] > num2)
                {
                    num2 = this.TeamPoints[i];
                    num = i;
                }
            }
            return (Team)num;
        }


        internal void Reset()
        {
            this.AddPointToTeam(Team.Blue, this.GetScoreForTeam(Team.Blue) * -1, null);
            this.AddPointToTeam(Team.Green, this.GetScoreForTeam(Team.Green) * -1, null);
            this.AddPointToTeam(Team.Red, this.GetScoreForTeam(Team.Red) * -1, null);
            this.AddPointToTeam(Team.Yellow, this.GetScoreForTeam(Team.Yellow) * -1, null);
        }

        private int GetScoreForTeam(Team team)
        {
            return this.TeamPoints[(int)team];
        }

        private void LockGate(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType.ToLower())
            {
                case "freeze_blue_gate":
                case "freeze_red_gate":
                case "freeze_green_gate":
                case "freeze_yellow_gate":
                    item.method_8().method_39(item.Int32_0, item.Int32_1);
                    break;
            }
        }

        internal void LockGates()
        {
            foreach (RoomItem item in this.freezeGates.Values)
            {
                this.LockGate(item);
            }
        }

        private void UnlockGate(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case "freeze_blue_gate":
                case "freeze_red_gate":
                case "freeze_green_gate":
                case "freeze_yellow_gate":
                    item.method_8().method_38(item.Int32_0, item.Int32_1);
                    break;
            }
        }

        internal void UnlockGates()
        {
            foreach (RoomItem item in this.freezeGates.Values)
            {
                this.UnlockGate(item);
            }
        }

        internal int[] Points
        {
            get
            {
                return this.TeamPoints;
            }
            set
            {
                this.TeamPoints = value;
            }
        }
    }
}

