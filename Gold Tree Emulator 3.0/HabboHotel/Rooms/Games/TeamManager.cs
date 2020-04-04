using GoldTree.HabboHotel.Items;
using System.Collections.Generic;
namespace GoldTree.HabboHotel.Rooms.Games
{
    internal class TeamManager
    {
        private Room room;
        internal List<RoomUser> BlueTeam;
        internal List<RoomUser> GreenTeam;
        internal List<RoomUser> RedTeam;
        internal List<RoomUser> YellowTeam;

        public TeamManager(Room room)
        {
            this.room = room;
            this.BlueTeam = new List<RoomUser>();
            this.RedTeam = new List<RoomUser>();
            this.GreenTeam = new List<RoomUser>();
            this.YellowTeam = new List<RoomUser>();
        }

        internal void Destroy()
        {
            this.BlueTeam.Clear();
            this.RedTeam.Clear();
            this.GreenTeam.Clear();
            this.YellowTeam.Clear();
            this.BlueTeam = null;
            this.RedTeam = null;
            this.GreenTeam = null;
            this.YellowTeam = null;
            this.room = null;
        }

        internal void AddUser(RoomUser user)
        {
            if (user.team.Equals(Team.Blue))
            {
                this.BlueTeam.Add(user);
            }
            else if (user.team.Equals(Team.Red))
            {
                this.RedTeam.Add(user);
            }
            else if (user.team.Equals(Team.Yellow))
            {
                this.YellowTeam.Add(user);
            }
            else if (user.team.Equals(Team.Green))
            {
                this.GreenTeam.Add(user);
            }

            if (this.room.GetFreeze().GameIsStarted == false)
            {
                foreach (RoomItem item in this.room.Hashtable_0.Values)
                {
                    if (item.GetBaseItem().InteractionType.ToLower() == "freeze_blue_gate")
                    {
                        item.ExtraData = this.BlueTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "freeze_red_gate")
                    {
                        item.ExtraData = this.RedTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "freeze_green_gate")
                    {
                        item.ExtraData = this.GreenTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "freeze_yellow_gate")
                    {
                        item.ExtraData = this.YellowTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "bb_blue_gate")
                    {
                        item.ExtraData = this.BlueTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "bb_red_gate")
                    {
                        item.ExtraData = this.RedTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "bb_green_gate")
                    {
                        item.ExtraData = this.GreenTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "bb_yellow_gate")
                    {
                        item.ExtraData = this.YellowTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                }
            }
        }

        public bool CanEnterOnTeam(Team t)
        {
            if (t.Equals(Team.Blue))
            {
                return (this.BlueTeam.Count < 5);
            }
            if (t.Equals(Team.Red))
            {
                return (this.RedTeam.Count < 5);
            }
            if (t.Equals(Team.Yellow))
            {
                return (this.YellowTeam.Count < 5);
            }
            return (t.Equals(Team.Green) && (this.GreenTeam.Count < 5));
        }

        internal void OnUserLeave(RoomUser user)
        {
            if (user.team.Equals(Team.Blue))
            {
                this.BlueTeam.Remove(user);
            }
            else if (user.team.Equals(Team.Red))
            {
                this.RedTeam.Remove(user);
            }
            else if (user.team.Equals(Team.Yellow))
            {
                this.YellowTeam.Remove(user);
            }
            else if (user.team.Equals(Team.Green))
            {
                this.GreenTeam.Remove(user);
            }

            if (this.room.GetFreeze().GameIsStarted == false)
            {
                foreach (RoomItem item in this.room.Hashtable_0.Values)
                {
                    if (item.GetBaseItem().InteractionType.ToLower() == "freeze_blue_gate")
                    {
                        item.ExtraData = this.BlueTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "freeze_red_gate")
                    {
                        item.ExtraData = this.RedTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "freeze_green_gate")
                    {
                        item.ExtraData = this.GreenTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "freeze_yellow_gate")
                    {
                        item.ExtraData = this.YellowTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "bb_blue_gate")
                    {
                        item.ExtraData = this.BlueTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "bb_red_gate")
                    {
                        item.ExtraData = this.RedTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "bb_green_gate")
                    {
                        item.ExtraData = this.GreenTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                    else if (item.GetBaseItem().InteractionType.ToLower() == "bb_yellow_gate")
                    {
                        item.ExtraData = this.YellowTeam.Count.ToString();
                        item.UpdateState(false, true);
                    }
                }
            }

            user.game = Game.None;
            user.team = Team.None;
        }
    }
}