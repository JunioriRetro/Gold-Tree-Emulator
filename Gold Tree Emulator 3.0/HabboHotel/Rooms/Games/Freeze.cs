using GoldTree.HabboHotel.Items;
using GoldTree.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
namespace GoldTree.HabboHotel.Rooms.Games
{
    internal class Freeze
    {
        private RoomItem exitTeleport;
        public Hashtable freezeBlocks;
        public Hashtable freezeTiles;
        private bool gameStarted;
        private Random rnd;
        private Room room;

        public Freeze(Room room)
        {
            this.room = room;
            this.freezeTiles = new Hashtable();
            this.freezeBlocks = new Hashtable();
            this.exitTeleport = null;
            this.rnd = new Random();
            this.gameStarted = false;
        }

        private static void ActivateShield(RoomUser user)
        {
            int Effect = (int)user.team + 48;
            user.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(Effect, true);
            user.shieldActive = true;
            user.shieldCounter += 10;
        }

        internal void AddFreezeBlock(RoomItem item)
        {
            if (this.freezeBlocks.ContainsKey(item.uint_0))
            {
                this.freezeBlocks.Remove(item.uint_0);
            }
            this.freezeBlocks.Add(item.uint_0, item);
        }

        internal void AddFreezeTile(RoomItem item)
        {
            if (this.freezeTiles.Contains(item.uint_0))
            {
                this.freezeTiles.Remove(item.uint_0);
            }
            this.freezeTiles.Add(item.uint_0, item);
        }

        internal void RemoveFreezeBlock(RoomItem item)
        {
            if (this.freezeBlocks.ContainsKey(item.uint_0))
            {
                this.freezeBlocks.Remove(item.uint_0);
            }
        }

        internal void RemoveFreezeTile(RoomItem item)
        {
            if (this.freezeTiles.Contains(item.uint_0))
            {
                this.freezeTiles.Remove(item.uint_0);
            }
        }

        private void CountTeamPoints()
        {
            this.room.GetGameManager().Reset();
            for (int i = 0; i < room.RoomUsers.Length; i++)
            {
                RoomUser user = room.RoomUsers[i];
                if (user != null)
                {
                    if ((!user.IsBot && (user.team != Team.None)) && (user.GetClient() != null))
                    {
                        user.FreezeBalls = 1;
                        user.FreezeRange = 3;
                        user.FreezeLives = 3;
                        user.freezePowerUp = FreezePowerUp.None;
                        user.shieldActive = false;
                        user.shieldCounter = 11;
                        this.room.GetGameManager().AddPointToTeam(user.team, 30, null);
                        /*ServerMessage message = new ServerMessage();
                        message.Init(Outgoing.UpdateFreezeLives);
                        message.AppendInt32(user.InternalRoomID);
                        message.AppendInt32(user.FreezeLives);
                        user.GetClient().SendMessage(message);*/
                    }
                }
            }
        }

        internal void CycleUser(RoomUser user)
        {
            if (user.Freezed)
            {
                user.FreezeCounter++;
                user.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(12, true);
                if (user.FreezeCounter > 10)
                {
                    user.bool_5 = !user.bool_5;
                    user.Freezed = false;
                    user.FreezeCounter = 0;
                    if (user.FreezeLives > 0)
                    {
                        ActivateShield(user);
                    }
                    else if (user.FreezeLives <= 0)
                    {
                        user.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                        this.room.GetGameManager().AddPointToTeam(user.team, -10, user);
                        TeamManager teamManagerForFreeze = this.room.GetRoomTeamManager();
                        teamManagerForFreeze.OnUserLeave(user);
                        user.team = Team.None;
                        if (this.exitTeleport != null)
                        {
                            //this.room.GetGameMap().TeleportToItem(user, this.exitTeleport);
                            user.int_3 = this.exitTeleport.Int32_0;
                            user.int_4 = this.exitTeleport.Int32_1;
                            user.UpdateNeeded = true;
                        }
                        /*foreach (RoomItem Item in user.GetClient().GetHabbo().CurrentRoom.Hashtable_0.Values)
                        {
                            if (Item.GetBaseItem().Name == "es_exit")
                            {
                                user.int_3 = Item.Int32_0;
                                user.int_4 = Item.Int32_1;
                                user.UpdateNeeded = true;
                            }
                        }*/
                        user.Freezed = false;
                        user.UpdateNeeded = true;
                        if ((((teamManagerForFreeze.BlueTeam.Count <= 0) && (teamManagerForFreeze.RedTeam.Count <= 0)) && (teamManagerForFreeze.GreenTeam.Count <= 0)) && (teamManagerForFreeze.YellowTeam.Count > 0))
                        {
                            this.StopGame();
                        }
                        else if ((((teamManagerForFreeze.BlueTeam.Count > 0) && (teamManagerForFreeze.RedTeam.Count <= 0)) && (teamManagerForFreeze.GreenTeam.Count <= 0)) && (teamManagerForFreeze.YellowTeam.Count <= 0))
                        {
                            this.StopGame();
                        }
                        else if ((((teamManagerForFreeze.BlueTeam.Count <= 0) && (teamManagerForFreeze.RedTeam.Count > 0)) && (teamManagerForFreeze.GreenTeam.Count <= 0)) && (teamManagerForFreeze.YellowTeam.Count <= 0))
                        {
                            this.StopGame();
                        }
                        else if ((((teamManagerForFreeze.BlueTeam.Count <= 0) && (teamManagerForFreeze.RedTeam.Count <= 0)) && (teamManagerForFreeze.GreenTeam.Count > 0)) && (teamManagerForFreeze.YellowTeam.Count <= 0))
                        {
                            this.StopGame();
                        }
                    }
                }
            }
            if (user.shieldActive)
            {
                user.shieldCounter--;
                if (user.shieldCounter <= 0)
                {
                    user.shieldActive = false;
                    user.shieldCounter = 0;
                    int Effect = (int)user.team + 39;
                    user.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(Effect, true);
                }
            }
        }

        internal void Destroy()
        {
            this.freezeBlocks.Clear();
            this.freezeTiles.Clear();
            this.room = null;
            this.freezeTiles = null;
            this.freezeBlocks = null;
            this.exitTeleport = null;
            this.rnd = null;
        }

        private static void ExitGame(RoomUser user)
        {
            user.team = Team.None;
        }

        internal void FreezeUser(RoomUser user)
        {
            if ((!user.IsBot && !user.shieldActive && !user.Freezed) && (user.team != Team.None))
            {
                user.bool_5 = !user.bool_5;
                user.Freezed = true;
                user.FreezeCounter = 0;
                user.FreezeLives--;
                /*ServerMessage message = new ServerMessage();
                message.Init(Outgoing.UpdateFreezeLives);
                message.AppendInt32(user.InternalRoomID);
                message.AppendInt32(user.FreezeLives);
                user.GetClient().SendMessage(message);*/
                if (user.FreezeLives <= 0)
                {

                }
                else
                {
                    this.room.GetGameManager().AddPointToTeam(user.team, -10, user);
                }
            }
        }

        internal static void OnCycle()
        {
        }

        public void PickUpPowerUp(RoomItem item, RoomUser user)
        {
            switch (item.freezePowerUp)
            {
                case FreezePowerUp.BlueArrow:
                    user.FreezeRange++;
                    break;
                case FreezePowerUp.Snowballs:
                    user.FreezeBalls++;
                    break;
                case FreezePowerUp.GreenArrow:
                case FreezePowerUp.OrangeSnowball:
                    user.freezePowerUp = item.freezePowerUp;
                    break;

                case FreezePowerUp.Shield:
                    ActivateShield(user);
                    break;

                case FreezePowerUp.Heart:
                    {
                        if (user.FreezeLives <= 2)
                        {
                            user.FreezeLives++;
                            this.room.GetGameManager().AddPointToTeam(user.team, 10, user);
                        }
                        /*ServerMessage message = new ServerMessage();
                        message.Init(Outgoing.UpdateFreezeLives);
                        message.AppendInt32(user.InternalRoomID);
                        message.AppendInt32(user.FreezeLives);
                        user.GetClient().SendMessage(message);*/
                        break;
                    }
            }
            item.freezePowerUp = FreezePowerUp.None;
            item.ExtraData = "1" + item.ExtraData;
            item.UpdateState(false, true);
        }

        internal void RemoveFreezeBlock(uint itemID)
        {
            this.freezeBlocks.Remove(itemID);
        }

        internal void RemoveFreezeTile(uint itemID)
        {
            this.freezeTiles.Remove(itemID);
        }

        private static void RemoveUserFromTeam(RoomUser user)
        {
            user.team = Team.None;
            user.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
        }

        internal void ResetGame()
        {
            foreach (RoomItem item in this.freezeBlocks.Values)
            {
                item.ExtraData = "";
                item.UpdateState(false, true);
                item.method_8().method_39(item.Int32_0, item.Int32_1);
            }

            foreach (RoomItem item in this.freezeTiles.Values)
            {
                item.ExtraData = "";
                item.UpdateState(false, true);
            }
        }

        internal void ResetGameAfterStop()
        {
            foreach (RoomItem item in this.freezeBlocks.Values)
            {
                if (item.freezePowerUp == FreezePowerUp.None && (!string.IsNullOrEmpty(item.ExtraData)))
                {
                    item.method_8().method_38(item.Int32_0, item.Int32_1);
                }
                else if (item.freezePowerUp != FreezePowerUp.None)
                {
                    item.freezePowerUp = FreezePowerUp.None;
                    item.ExtraData = "1" + item.ExtraData;
                    item.UpdateState(true, true);
                    item.method_8().method_38(item.Int32_0, item.Int32_1);
                }
                else
                {
                    item.freezePowerUp = FreezePowerUp.None;
                    item.ExtraData = "";
                    item.UpdateState(true, true);
                    item.method_8().method_39(item.Int32_0, item.Int32_1);
                }
            }

            foreach (RoomItem item in this.freezeTiles.Values)
            {
                item.ExtraData = "";
                item.UpdateState(true, true);
            }
        }

        public void SetRandomPowerUp(RoomItem item)
        {
            if (string.IsNullOrEmpty(item.ExtraData))
            {
                switch (this.rnd.Next(1, 11))
                {
                    case 2:
                        item.ExtraData = "2000";
                        item.freezePowerUp = FreezePowerUp.BlueArrow;
                        break;

                    case 3:
                        item.ExtraData = "3000";
                        item.freezePowerUp = FreezePowerUp.Snowballs;
                        break;

                    case 4:
                        item.ExtraData = "4000";
                        item.freezePowerUp = FreezePowerUp.GreenArrow;
                        break;

                    case 5:
                        item.ExtraData = "5000";
                        item.freezePowerUp = FreezePowerUp.OrangeSnowball;
                        break;

                    case 6:
                        item.ExtraData = "6000";
                        item.freezePowerUp = FreezePowerUp.Heart;
                        break;

                    case 7:
                        item.ExtraData = "7000";
                        item.freezePowerUp = FreezePowerUp.Shield;
                        break;

                    default:
                        item.ExtraData = "1000";
                        item.freezePowerUp = FreezePowerUp.None;
                        break;
                }
                item.method_8().method_38(item.Int32_0, item.Int32_1);
                item.UpdateState(false, true);
            }
        }

        internal void StartGame()
        {
            this.gameStarted = true;
            this.CountTeamPoints();
            this.ResetGame();
            this.room.GetGameManager().LockGates();
        }

        internal void StopGame()
        {
            this.gameStarted = false;
            this.ResetGameAfterStop();
            this.room.GetGameManager().UnlockGates();
            Team team = this.room.GetGameManager().getWinningTeam();
            for (int i = 0; i < room.RoomUsers.Length; i++)
            {
                RoomUser user = room.RoomUsers[i];
                if (user != null)
                {
                    user.FreezeLives = 0;
                    if (user.team == team)
                    {
                        user.Unidle();
                        user.DanceId = 0;
                        ServerMessage message = new ServerMessage(481u);
                        message.AppendInt32(user.VirtualId);
                        message.AppendInt32(1);
                        this.room.SendMessage(message, null);
                    }
                }
            }
        }

        internal RoomItem ExitTeleport
        {
            get
            {
                return this.exitTeleport;
            }
            set
            {
                this.exitTeleport = value;
            }
        }

        internal bool GameIsStarted
        {
            get
            {
                return this.gameStarted;
            }
        }
    }
}