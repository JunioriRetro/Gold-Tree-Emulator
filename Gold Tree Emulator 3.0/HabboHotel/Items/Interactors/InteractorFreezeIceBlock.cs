using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldTree.HabboHotel.Items.Interactors
{
    class InteractorFreezeIceBlock : FurniInteractor
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

        public async override void OnTrigger(GameClient Session, RoomItem Item, int Request, bool UserHasRights)
        {
            try
            {
                if (Session == null || Session.GetHabbo() == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(Item.ExtraData))
                {
                    return;
                }

                if (Item == null || Item.method_8() == null)
                {
                    return;
                }

                Room @class = Item.method_8();

                if (@class == null)
                {
                    return;
                }

                RoomUser User = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);

                if (User.Freezed == false)
                {
                    if (User.team != Rooms.Games.Team.None)
                    {
                        if (Item.method_8().frzTimer == true)
                        {
                            if (Item.Int32_0 == User.int_3 || Item.Int32_0 - 1 == User.int_3 || Item.Int32_0 + 1 == User.int_3)
                            {
                                if (Item.Int32_1 == User.int_4 || Item.Int32_1 - 1 == User.int_4 || Item.Int32_1 + 1 == User.int_4)
                                {
                                    if (User.FreezeBalls > 0)
                                    {
                                        foreach (RoomItem Item2 in Item.method_8().GetFreeze().freezeTiles.Values)
                                        {
                                            if (Item2.Int32_0 == Item.Int32_0 && Item2.Int32_1 == Item.Int32_1 && (Item2.ExtraData == "11200" || string.IsNullOrEmpty(Item2.ExtraData)))
                                            {
                                                Rooms.Games.FreezePowerUp BallType = User.freezePowerUp;
                                                User.freezePowerUp = Rooms.Games.FreezePowerUp.None;

                                                bool pX, pY, pD1, pD2, nX, nY, nD1, nD2;
                                                pX = false; pY = false; pD1 = false; pD2 = false; nX = false; nY = false; nD1 = false; nD2 = false;

                                                if (BallType == Rooms.Games.FreezePowerUp.OrangeSnowball)
                                                {
                                                    User.FreezeBalls -= 1;
                                                    Item2.ExtraData = "6000";
                                                    Item2.UpdateState(false, true);
                                                    await Task.Delay(2000);
                                                    BreakIceBlock(Item2, Item2);
                                                    FreezeUser(Item2, Item2);
                                                }
                                                else
                                                {
                                                    User.FreezeBalls -= 1;
                                                    Item2.ExtraData = "1000";
                                                    Item2.UpdateState(false, true);
                                                    await Task.Delay(2000);
                                                    BreakIceBlock(Item2, Item2);
                                                    FreezeUser(Item2, Item2);
                                                }

                                                if (BallType == Rooms.Games.FreezePowerUp.None)
                                                {
                                                    for (int i = 1; i < 20; i++)
                                                    {
                                                        if (User.FreezeRange >= i)
                                                        {
                                                            await Task.Delay(200);
                                                            foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                                            {
                                                                if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 + i && !pX) { pX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 - i && !pY) { pY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 && !nX) { nX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 && !nY) { nY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                            }
                                                        }
                                                    }
                                                }

                                                else if (BallType == Rooms.Games.FreezePowerUp.GreenArrow)
                                                {
                                                    for (int i = 1; i < 20; i++)
                                                    {
                                                        if (User.FreezeRange >= i)
                                                        {
                                                            await Task.Delay(200);
                                                            foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                                            {
                                                                if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 + i && !pD1) { pD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 - i && !nD1) { nD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 + i && !pD2) { pD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 - i && !nD2) { nD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                            }
                                                        }
                                                    }
                                                }

                                                else if (BallType == Rooms.Games.FreezePowerUp.OrangeSnowball)
                                                {
                                                    for (int i = 1; i < 20; i++)
                                                    {
                                                        if (User.FreezeRange >= i)
                                                        {
                                                            await Task.Delay(200);
                                                            foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                                            {
                                                                if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 + i && !pX) { pX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 - i && !pY) { pY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 && !nX) { nX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 && !nY) { nY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 + i && !pD1) { pD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 - i && !nD1) { nD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 + i && !pD2) { pD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 - i && !nD2) { nD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                            }
                                                        }
                                                    }
                                                }

                                                BallType = Rooms.Games.FreezePowerUp.None;
                                                User.FreezeBalls += 1;
                                            }
                                        }
                                    }

                                    /*int progress = 0;
                                    System.Timers.Timer t1 = new System.Timers.Timer();
                                    Rooms.Games.FreezePowerUp FreezePowerUp = User.freezePowerUp;
                                    User.freezePowerUp = Rooms.Games.FreezePowerUp.None;
                                    t1.Elapsed += delegate { ThrowBomb(t1, User, Item, FreezePowerUp, progress++); };
                                    t1.Interval = 200;
                                    t1.Start();*/

                                    /*ThreadPool.QueueUserWorkItem(o =>
                                    {
                                        //System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();
                                        //Stopwatch.Start();
                                        if (User.FreezeBalls > 0)
                                        {
                                            foreach (RoomItem Item2 in Item.method_8().GetFreeze().freezeTiles.Values)
                                            {
                                                if (Item2.Int32_0 == Item.Int32_0 && Item2.Int32_1 == Item.Int32_1 && (!string.IsNullOrEmpty(Item2.ExtraData)))
                                                {
                                                    Rooms.Games.FreezePowerUp BallType = User.freezePowerUp;
                                                    User.freezePowerUp = Rooms.Games.FreezePowerUp.None;

                                                    bool pX, pY, pD1, pD2, nX, nY, nD1, nD2;
                                                    pX = false; pY = false; pD1 = false; pD2 = false; nX = false; nY = false; nD1 = false; nD2 = false;

                                                    if (BallType == Rooms.Games.FreezePowerUp.OrangeSnowball)
                                                    {
                                                        User.FreezeBalls -= 1;
                                                        Item2.ExtraData = "6000";
                                                        Item2.UpdateState(false, true);
                                                        Thread.Sleep(2000);
                                                        BreakIceBlock(Item2, Item2);
                                                        FreezeUser(Item2, Item2);
                                                    }
                                                    else
                                                    {
                                                        User.FreezeBalls -= 1;
                                                        Item2.ExtraData = "1000";
                                                        Item2.UpdateState(false, true);
                                                        Thread.Sleep(2000);
                                                        BreakIceBlock(Item2, Item2);
                                                        FreezeUser(Item2, Item2);
                                                    }

                                                    if (BallType == Rooms.Games.FreezePowerUp.None)
                                                    {
                                                        for (int i = 1; i < 20; i++)
                                                        {
                                                            if (User.FreezeRange >= i)
                                                            {
                                                                Thread.Sleep(200);
                                                                foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                                                {
                                                                    if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 + i && !pX) { pX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 - i && !pY) { pY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 && !nX) { nX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 && !nY) { nY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    else if (BallType == Rooms.Games.FreezePowerUp.GreenArrow)
                                                    {
                                                        for (int i = 1; i < 20; i++)
                                                        {
                                                            if (User.FreezeRange >= i)
                                                            {
                                                                Thread.Sleep(200);
                                                                foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                                                {
                                                                    if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 + i && !pD1) { pD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 - i && !nD1) { nD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 + i && !pD2) { pD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 - i && !nD2) { nD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    else if (BallType == Rooms.Games.FreezePowerUp.OrangeSnowball)
                                                    {
                                                        for (int i = 1; i < 20; i++)
                                                        {
                                                            if (User.FreezeRange >= i)
                                                            {
                                                                Thread.Sleep(200);
                                                                foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                                                {
                                                                    if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 + i && !pX) { pX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 - i && !pY) { pY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 && !nX) { nX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 && !nY) { nY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 + i && !pD1) { pD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 - i && !nD1) { nD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 + i && !pD2) { pD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                    if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 - i && !nD2) { nD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    BallType = Rooms.Games.FreezePowerUp.None;
                                                    User.FreezeBalls += 1;
                                                }
                                            }
                                        }
                                    });*/
                                }
                            }
                        }
                    }
                }
            }

            catch
            {
            }
        }

        public bool BreakIceBlock(RoomItem Item, RoomItem Item2)
        {
            if (Item.method_8().frzTimer == true)
            {
                Item2.ExtraData = "11200";
                Item2.UpdateState(false, true);

                foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeBlocks.Values)
                {
                    if (Item2.Int32_0 == Item3.Int32_0 && Item2.Int32_1 == Item3.Int32_1)
                    {
                        if (string.IsNullOrEmpty(Item3.ExtraData))
                        {
                            Item3.method_8().GetFreeze().SetRandomPowerUp(Item3);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void FreezeUser(RoomItem Item, RoomItem Item2)
        {
            if (Item.method_8().frzTimer == true)
            {
                for (int i = 0; i < Item.method_8().RoomUsers.Length; i++)
                {
                    RoomUser User2 = Item.method_8().RoomUsers[i];
                    if (User2 != null)
                    {
                        if (User2.int_3 == Item2.Int32_0 && User2.int_4 == Item2.Int32_1)
                        {
                            Item.method_8().GetFreeze().FreezeUser(User2);
                        }
                    }
                }
            }
        }

        // TIMER

        /*public async void ThrowBomb(System.Timers.Timer timer, RoomUser User, RoomItem Item, Rooms.Games.FreezePowerUp freezePowerUp, int progress)
        {
            //timer.Stop();
            System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();
            Stopwatch.Start();
            if (User.FreezeBalls > 0)
            {
                foreach (RoomItem Item2 in Item.method_8().GetFreeze().freezeTiles.Values)
                {
                    if (Item2.Int32_0 == Item.Int32_0 && Item2.Int32_1 == Item.Int32_1 && (!string.IsNullOrEmpty(Item2.ExtraData)))
                    {
                        bool pX, pY, pD1, pD2, nX, nY, nD1, nD2, freezecontinue;
                        pX = false; pY = false; pD1 = false; pD2 = false; nX = false; nY = false; nD1 = false; nD2 = false;

                        if (progress < 1)
                        {
                            if (freezePowerUp == Rooms.Games.FreezePowerUp.OrangeSnowball)
                            {
                                User.FreezeBalls -= 1;
                                Item2.ExtraData = "6000";
                                Item2.UpdateState(false, true);
                            }
                            else
                            {
                                User.FreezeBalls -= 1;
                                Item2.ExtraData = "1000";
                                Item2.UpdateState(false, true);
                            }
                            break;
                        }
                        else if (progress == 10)
                        {
                            freezecontinue = true;
                            if (freezePowerUp == Rooms.Games.FreezePowerUp.OrangeSnowball)
                            {
                                BreakIceBlock(Item2, Item2);
                                FreezeUser(Item2, Item2);
                            }
                            else
                            {
                                BreakIceBlock(Item2, Item2);
                                FreezeUser(Item2, Item2);
                            }

                            if (freezePowerUp == Rooms.Games.FreezePowerUp.None)
                            {
                                for (int i = 1; i < 20; i++)
                                {
                                    if (User.FreezeRange >= i)
                                    {
                                        await Task.Delay(200);
                                        foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                        {
                                            if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 + i && !pX) { pX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 - i && !pY) { pY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 && !nX) { nX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 && !nY) { nY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                        }
                                    }
                                }
                            }

                            else if (freezePowerUp == Rooms.Games.FreezePowerUp.GreenArrow)
                            {
                                for (int i = 1; i < 20; i++)
                                {
                                    if (User.FreezeRange >= i)
                                    {
                                        if (!freezecontinue)
                                        {
                                            break;
                                        }
                                        freezecontinue = false;
                                        foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                        {
                                            if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 + i && !pD1) { pD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 - i && !nD1) { nD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 + i && !pD2) { pD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 - i && !nD2) { nD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                        }
                                    }
                                }
                            }

                            else if (freezePowerUp == Rooms.Games.FreezePowerUp.OrangeSnowball)
                            {
                                for (int i = 1; i < 20; i++)
                                {
                                    if (User.FreezeRange >= i)
                                    {
                                        if (!freezecontinue)
                                        {
                                            break;
                                        }
                                        freezecontinue = false;
                                        foreach (RoomItem Item3 in Item.method_8().GetFreeze().freezeTiles.Values)
                                        {
                                            if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 + i && !pX) { pX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 && Item3.Int32_1 == Item.Int32_1 - i && !pY) { pY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 && !nX) { nX = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 && !nY) { nY = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 + i && !pD1) { pD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 + i && Item3.Int32_1 == Item.Int32_1 - i && !nD1) { nD1 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 + i && !pD2) { pD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                            if (Item3.Int32_0 == Item.Int32_0 - i && Item3.Int32_1 == Item.Int32_1 - i && !nD2) { nD2 = BreakIceBlock(Item, Item3); FreezeUser(Item, Item3); }
                                        }
                                    }
                                }
                            }

                            freezePowerUp = Rooms.Games.FreezePowerUp.None;
                            User.FreezeBalls += 1;
                            timer.Stop();
                            timer.Dispose();
                        }
                        else if (progress > 10)
                        {
                            timer.Stop();
                            timer.Dispose();
                        }
                    }
                }
            }
            Stopwatch.Stop();
            //Console.WriteLine("Thread: " + Stopwatch.Elapsed);
        }*/
    }
}
