using System;
using System.Collections.Generic;
using System.Linq;
using GoldTree.Core;
using GoldTree.HabboHotel.Pathfinding;
using GoldTree.HabboHotel.Items;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
using GoldTree.HabboHotel.Items.Interactors;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Storage;
using GoldTree.HabboHotel.SoundMachine;
using GoldTree.HabboHotel.Rooms.Games;
using GoldTree.Util;
using System.Globalization;
namespace GoldTree.HabboHotel.Items
{
	internal sealed class RoomItem
	{
		internal enum Enum5
		{
			const_0,
			const_1,
			const_2
		}
		internal uint uint_0;
		internal uint uint_1;
		internal uint uint_2;
		internal string ExtraData;
        internal FreezePowerUp freezePowerUp;
		internal bool bool_0;
		internal string string_1;
		internal string string_2;
		internal string string_3;
		internal string string_4;
		internal string string_5;
		internal string string_6;
		private Dictionary<int, AffectedTile> dictionary_0;
		private int int_1;
		private int int_2;
		private double double_0;
		internal RoomItem.Enum5 enum5_0;
		internal int int_3;
		internal string string_7;
		internal bool bool_1;
		internal int int_4;
		internal uint uint_3;
		internal uint uint_4;
		internal Dictionary<RoomUser, int> dictionary_1;
		private Item Item;
		private Room class14_0;
		private bool bool_2;
		private bool bool_3;
		private bool bool_4;
        public int FireWorkCount;
        public string LastPlayerHitFootball;
        internal Team team;
        internal bool WiredAtTimeNeedReset;
        internal double WiredAtTimeTimer;
        internal bool WiredNeedReset;
        internal double WiredCounter;
		internal Dictionary<int, AffectedTile> Dictionary_0
		{
			get
			{
				return this.dictionary_0;
			}
		}
		internal int Int32_0
		{
			get
			{
				return this.int_1;
			}
		}
		internal int Int32_1
		{
			get
			{
				return this.int_2;
			}
		}
		internal double Double_0
		{
			get
			{
				return this.double_0;
			}
		}
		internal bool Boolean_0
		{
			get
			{
				return this.bool_4;
			}
		}
		internal ThreeDCoord GStruct1_0
		{
			get
			{
				return new ThreeDCoord(this.int_1, this.int_2);
			}
		}
		internal double Double_1
		{
			get
			{
				double result;
				if (this.GetBaseItem().Height_Adjustable.Count > 1)
				{
					int index;
					if (int.TryParse(this.ExtraData, out index))
					{
						result = this.double_0 + this.GetBaseItem().Height_Adjustable[index];
					}
					else
					{
						result = this.double_0 + this.GetBaseItem().Height;
					}
				}
				else
				{
					result = this.double_0 + this.GetBaseItem().Height;
				}
				return result;
			}
		}
		internal bool Boolean_1
		{
			get
			{
				return this.bool_2;
			}
		}
		internal bool Boolean_2
		{
			get
			{
				return this.bool_3;
			}
		}
		internal ThreeDCoord GStruct1_1
		{
			get
			{
				ThreeDCoord result = new ThreeDCoord(this.int_1, this.int_2);
				if (this.int_3 == 0)
				{
					result.y--;
				}
				else
				{
					if (this.int_3 == 2)
					{
						result.x++;
					}
					else
					{
						if (this.int_3 == 4)
						{
							result.y++;
						}
						else
						{
							if (this.int_3 == 6)
							{
								result.x--;
							}
						}
					}
				}
				return result;
			}
		}
		internal ThreeDCoord GStruct1_2
		{
			get
			{
				ThreeDCoord result = new ThreeDCoord(this.int_1, this.int_2);
				if (this.int_3 == 0)
				{
					result.y++;
				}
				else
				{
					if (this.int_3 == 2)
					{
						result.x--;
					}
					else
					{
						if (this.int_3 == 4)
						{
							result.y--;
						}
						else
						{
							if (this.int_3 == 6)
							{
								result.x++;
							}
						}
					}
				}
				return result;
			}
		}
		internal FurniInteractor Class69_0
		{
			get
			{
				string text = this.GetBaseItem().InteractionType.ToLower();
				FurniInteractor result;
				switch (text)
				{
				case "ball":
					result = new InteractorFootball();
					return result;
				case "teleport":
					result = new InteractorTeleport();
					return result;
				case "bottle":
					result = new InteractorSpinningBottle();
					return result;
				case "dice":
					result = new InteractorDice();
					return result;
				case "habbowheel":
					result = new InteractorHabboWheel();
					return result;
				case "loveshuffler":
					result = new InteractorLoveShuffler();
					return result;
				case "onewaygate":
					result = new InteractorOneWayGate();
					return result;
				case "alert":
					result = new Class89();
					return result;
				case "vendingmachine":
					result = new InteractorVendor();
					return result;
				case "gate":
					result = new InteractorGate(this.GetBaseItem().Modes);
					return result;
				case "scoreboard":
					result = new InteractorScoreboard();
					return result;
				case "counter":
					result = new InteractorBanzaiScoreCounter();
					return result;
				case "wired":
					result = new WiredInteractor();
					return result;
				case "wf_trg_onsay":
					result = new InteractorWiredOnSay();
					return result;
				case "wf_trg_enterroom":
					result = new InteractorWiredEnterRoom();
					return result;
				case "wf_act_saymsg":
				case "wf_act_give_phx":
				case "wf_cnd_phx":
					result = new InteractorSuperWired();
					return result;
				case "wf_trg_furnistate":
				case "wf_trg_onfurni":
				case "wf_trg_offfurni":
				case "wf_act_moveuser":
				case "wf_act_togglefurni":
					result = new InteractorWiredTriggerState();
					return result;
				case "wf_trg_gameend":
				case "wf_trg_gamestart":
					result = new InteractorWiredTriggerGame();
					return result;
				case "wf_trg_timer":
					result = new InteractorWiredTriggerTimer();
					return result;
				case "wf_act_givepoints":
					result = new InteractorWiredGivePoints();
					return result;
				case "wf_trg_attime":
					result = new InteractorWiredAtTime();
					return result;
				case "wf_trg_atscore":
					result = new InteractorWiredAtScore();
					return result;
				case "wf_act_moverotate":
					result = new InteractorWiredMoveRotate();
					return result;
				case "wf_act_matchfurni":
					result = new InteractorWiredMatchFurni();
					return result;
				case "wf_cnd_trggrer_on_frn":
				case "wf_cnd_furnis_hv_avtrs":
				case "wf_cnd_has_furni_on":
					result = new InteractorWiredCondition();
					return result;
                case "wf_cnd_match_snapshot":
                    result = new InteractorWiredConditionFurniStatesAndPositionsMatch();
                    return result;
                    case "wf_cnd_time_more_than":
                    case "wf_cnd_time_less_than":
                                            result = new InteractorWiredConditionTimeMoreOrLess();
                    return result;
				case "puzzlebox":
					result = new InteractorPuzzleBox();
					return result;
                case "firework":
                    result = new InteractorFirework();
					return result;
                case "wf_act_kick_user":
                    result = new InteractorWiredKickUser();
                    return result;
                case "hopper":
                    result = new InteractorHopper();
                    return result;
                case "jukebox":
                    result = new InteractorJukebox();
                    return result;
                case "freeze_tile":
                    result = new InteractorFreezeTile();
                    return result;
                case "freeze_counter":
                    result = new InteractorFreezeCounter();
                    return result;
                case "freeze_ice_block":
                    result = new InteractorFreezeIceBlock();
                    return result;
				}
				result = new InteractorDefault(this.GetBaseItem().Modes);
				return result;
			}
		}
        public RoomItem(uint uint_5, uint uint_6, uint uint_7, string string_8, int int_5, int int_6, double double_1, int int_7, string string_9, Room class14_1)
        {
            this.uint_0 = uint_5;
            this.uint_1 = uint_6;
            this.uint_2 = uint_7;
            this.ExtraData = string_8;
            this.int_1 = int_5;
            this.int_2 = int_6;
            this.double_0 = double_1;
            this.int_3 = int_7;
            this.string_7 = string_9;
            this.bool_1 = false;
            this.int_4 = 0;
            this.uint_3 = 0u;
            this.uint_4 = 0u;
            this.bool_0 = false;
            this.string_1 = "none";
            this.enum5_0 = RoomItem.Enum5.const_0;
            this.string_2 = "";
            this.string_3 = "";
            this.string_4 = "";
            this.string_5 = "";
            this.string_6 = "";
            this.FireWorkCount = 0;
            this.dictionary_1 = new Dictionary<RoomUser, int>();
            this.Item = GoldTree.GetGame().GetItemManager().method_2(uint_7);
            this.class14_0 = class14_1;
            if (this.GetBaseItem() == null)
            {
                Logging.LogException("Unknown baseID: " + uint_7);
            }
            string text = this.GetBaseItem().InteractionType.ToLower();
            if (text != null)
            {
                if (!(text == "teleport"))
                {
                    if (!(text == "hopper"))
                    {
                        if (!(text == "roller"))
                        {
                            if (!(text == "blue_score"))
                            {
                                if (!(text == "green_score"))
                                {
                                    if (!(text == "red_score"))
                                    {
                                        if (text == "yellow_score")
                                        {
                                            this.string_1 = "yellow";
                                        }
                                    }
                                    else
                                    {
                                        this.string_1 = "red";
                                    }
                                }
                                else
                                {
                                    this.string_1 = "green";
                                }
                            }
                            else
                            {
                                this.string_1 = "blue";
                            }
                        }
                        else
                        {
                            this.bool_4 = true;
                            class14_1.Boolean_1 = true;
                        }
                    }
                    else
                    {
                        this.ReqUpdate(0);
                    }
                }
                else
                {
                    this.ReqUpdate(0);
                }
            }

            if (text != null)
            {
                switch (text)
                {
                    case "freeze_blue_gate":
                    case "freeze_blue_score":
                    case "bb_blue_gate":
                        this.team = Team.Blue;
                        break;
                    case "freeze_red_gate":
                    case "freeze_red_score":
                    case "bb_red_gate":
                        this.team = Team.Red;
                        break;
                    case "freeze_green_gate":
                    case "freeze_green_score":
                    case "bb_green_gate":
                        this.team = Team.Green;
                        break;
                    case "freeze_yellow_gate":
                    case "freeze_yellow_score":
                    case "bb_yellow_gate":
                        this.team = Team.Yellow;
                        break;
                    case "jukebox":
                        RoomMusicController roomMusicController = this.method_8().GetRoomMusicController();
                        roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(this);
                        break;
                }
            }

            this.bool_2 = (this.GetBaseItem().Type == 'i');
            this.bool_3 = (this.GetBaseItem().Type == 's');
            this.dictionary_0 = this.method_8().method_94(this.GetBaseItem().Length, this.GetBaseItem().Width, this.int_1, this.int_2, int_7);
        }
		internal void method_0(int int_5, int int_6, double double_1)
		{
			this.int_1 = int_5;
			this.int_2 = int_6;
			this.double_0 = double_1;
			this.dictionary_0 = this.method_8().method_94(this.GetBaseItem().Length, this.GetBaseItem().Width, this.int_1, this.int_2, this.int_3);
		}
		internal ThreeDCoord method_1(int int_5)
		{
			ThreeDCoord result = new ThreeDCoord(this.int_1, this.int_2);
			if (int_5 == 0)
			{
				result.y++;
			}
			else
			{
				if (int_5 == 2)
				{
					result.x--;
				}
				else
				{
					if (int_5 == 4)
					{
						result.y--;
					}
					else
					{
						if (int_5 == 6)
						{
							result.x++;
						}
					}
				}
			}
			return result;
		}
        internal void method_2()
        {
            this.int_4--;
            if (this.int_4 <= 0)
            {
                this.bool_1 = false;
                this.int_4 = 0;
                string text = this.GetBaseItem().InteractionType.ToLower();
                switch (text)
                {
                    case "onewaygate":
                        {
                            RoomUser @class = null;
                            if (this.uint_3 > 0u)
                            {
                                @class = this.method_8().GetRoomUserByHabbo(this.uint_3);
                            }
                            if (@class != null && @class.int_3 == this.int_1 && @class.int_4 == this.int_2 && this.string_2 != "tried")
                            {
                                this.ExtraData = "1";
                                this.string_2 = "tried";
                                @class.method_6();
                                @class.MoveTo(this.GStruct1_2);
                                this.ReqUpdate(0);
                                this.UpdateState(false, true);
                            }
                            else
                            {
                                if ((@class != null && ThreeDCoord.smethod_0(@class.Position, this.GStruct1_2)) || this.string_2 == "tried")
                                {
                                    this.string_2 = "";
                                    this.ExtraData = "0";
                                    this.uint_3 = 0u;
                                    this.UpdateState(false, true);
                                    this.method_8().method_22();
                                }
                                else
                                {
                                    if (this.ExtraData == "1")
                                    {
                                        this.ExtraData = "0";
                                        this.UpdateState(false, true);
                                    }
                                }
                            }
                            if (@class == null)
                            {
                                this.uint_3 = 0u;
                            }
                            break;
                        }
                    case "teleport":
                        {
                            bool flag = false;
                            bool flag2 = false;
                            if (this.uint_3 > 0u)
                            {
                                RoomUser @class = this.method_8().GetRoomUserByHabbo(this.uint_3);
                                if (@class != null)
                                {
                                    if (ThreeDCoord.smethod_0(@class.Position, this.GStruct1_0))
                                    {
                                        @class.bool_1 = false;
                                        if (@class.int_19 == -1)
                                        {
                                            @class.int_19 = 1;
                                        }
                                        if (TeleHandler.smethod_2(this.uint_0))
                                        {
                                            flag2 = true;

                                            if (this.GetBaseItem().Name == "xmas10_fireplace")
                                            {
                                                string look = @class.GetClient().GetHabbo().Figure;

                                                string[] lissut = look.Split('.');

                                                if (look.Contains("ha-"))
                                                {
                                                    look = look.Replace("" + lissut[Array.FindIndex(lissut, row => row.Contains("ha-"))], "ha-1006-62");

                                                }
                                                else
                                                {
                                                    look = look + ".ha-1006-62";
                                                }
                                                @class.GetClient().GetHabbo().Figure = GoldTree.FilterString(look.ToLower());

                                                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                                                {
                                                    dbClient.AddParamWithValue("look", look);
                                                    dbClient.ExecuteQuery("UPDATE users SET look =  @look WHERE id = " + @class.GetClient().GetHabbo().Id);
                                                }

                                                ServerMessage serverMessage = new ServerMessage(266u);
                                                serverMessage.AppendInt32(-1);
                                                serverMessage.AppendStringWithBreak(@class.GetClient().GetHabbo().Figure);
                                                serverMessage.AppendStringWithBreak(@class.GetClient().GetHabbo().Gender.ToLower());
                                                serverMessage.AppendStringWithBreak(@class.GetClient().GetHabbo().Motto);
                                                serverMessage.AppendInt32(@class.GetClient().GetHabbo().AchievementScore);
                                                serverMessage.AppendStringWithBreak("");
                                                @class.GetClient().SendMessage(serverMessage);
                                            }

                                            if (@class.int_19 == 0)
                                            {
                                                uint num2 = TeleHandler.smethod_0(this.uint_0);
                                                uint num3 = TeleHandler.smethod_1(num2);
                                                if (num3 == this.uint_1)
                                                {
                                                    RoomItem class2 = this.method_8().method_28(num2);
                                                    if (class2 == null)
                                                    {
                                                        @class.method_6();
                                                    }
                                                    else
                                                    {
                                                        @class.method_7(class2.Int32_0, class2.Int32_1, class2.Double_0);
                                                        @class.method_9(class2.int_3);
                                                        class2.ExtraData = "2";
                                                        class2.UpdateState(false, true);
                                                        class2.uint_4 = this.uint_3;
                                                    }
                                                }
                                                else
                                                {
                                                    if (!@class.IsBot)
                                                    {
                                                        GoldTree.GetGame().GetRoomManager().method_5(new TeleUserData(@class.GetClient().GetClientMessageHandler(), @class.GetClient().GetHabbo(), num3, num2));
                                                    }
                                                }
                                                this.uint_3 = 0u;
                                            }
                                            else
                                            {
                                                @class.int_19--;
                                            }
                                        }
                                        else
                                        {
                                            @class.method_6();
                                            this.uint_3 = 0u;
                                            @class.MoveTo(this.GStruct1_1);
                                        }
                                    }
                                    else
                                    {
                                        if (ThreeDCoord.smethod_0(@class.Position, this.GStruct1_1) && @class.RoomItem_0 == this)
                                        {
                                            @class.bool_1 = true;
                                            flag = true;
                                            if (@class.bool_6 && (@class.int_10 != this.int_1 || @class.int_11 != this.int_2))
                                            {
                                                @class.method_3(true);
                                            }
                                            @class.bool_0 = false;
                                            @class.bool_1 = true;
                                            @class.MoveTo(this.GStruct1_0);
                                        }
                                        else
                                        {
                                            this.uint_3 = 0u;
                                        }
                                    }
                                }
                                else
                                {
                                    this.uint_3 = 0u;
                                }
                            }
                            if (this.uint_4 > 0u)
                            {
                                RoomUser class3 = this.method_8().GetRoomUserByHabbo(this.uint_4);
                                if (class3 != null)
                                {
                                    flag = true;
                                    class3.method_6();
                                    if (ThreeDCoord.smethod_0(class3.Position, this.GStruct1_0))
                                    {
                                        class3.MoveTo(this.GStruct1_1);
                                    }
                                }
                                this.uint_4 = 0u;
                            }
                            if (flag)
                            {
                                if (this.ExtraData != "1")
                                {
                                    this.ExtraData = "1";
                                    this.UpdateState(false, true);
                                }
                            }
                            else
                            {
                                if (flag2)
                                {
                                    if (this.ExtraData != "2")
                                    {
                                        this.ExtraData = "2";
                                        this.UpdateState(false, true);
                                    }
                                }
                                else
                                {
                                    if (this.ExtraData != "0")
                                    {
                                        this.ExtraData = "0";
                                        this.UpdateState(false, true);
                                    }
                                }
                            }
                            this.ReqUpdate(1);
                            break;
                        }
                    case "hopper":
                        {
                            bool flag = false;
                            bool flag2 = false;
                            if (this.uint_3 > 0u)
                            {
                                RoomUser @class = this.method_8().GetRoomUserByHabbo(this.uint_3);
                                if (@class != null)
                                {
                                    if (ThreeDCoord.smethod_0(@class.Position, this.GStruct1_0))
                                    {
                                        @class.bool_1 = false;
                                        if (@class.int_19 == -1)
                                        {
                                            @class.int_19 = 1;
                                        }
                                        if (HopperHandler.smethod_2(this.uint_0))
                                        {
                                            flag2 = true;
                                            if (@class.int_19 == 0)
                                            {
                                                uint num2 = HopperHandler.smethod_0(this.uint_0);
                                                uint num3 = HopperHandler.smethod_1(num2);
                                                if (num3 == this.uint_1)
                                                {
                                                    RoomItem class2 = this.method_8().method_28(num2);
                                                    if (class2 == null)
                                                    {
                                                        @class.method_6();
                                                    }
                                                    else
                                                    {
                                                        @class.method_7(class2.Int32_0, class2.Int32_1, class2.Double_0);
                                                        @class.method_9(class2.int_3);
                                                        class2.ExtraData = "2";
                                                        class2.UpdateState(false, true);
                                                        class2.uint_4 = this.uint_3;
                                                    }
                                                }
                                                else
                                                {
                                                    if (!@class.IsBot)
                                                    {
                                                        GoldTree.GetGame().GetRoomManager().method_5(new TeleUserData(@class.GetClient().GetClientMessageHandler(), @class.GetClient().GetHabbo(), num3, num2));
                                                    }
                                                }
                                                this.uint_3 = 0u;
                                            }
                                            else
                                            {
                                                @class.int_19--;
                                            }
                                        }
                                        else
                                        {
                                            @class.method_6();
                                            this.uint_3 = 0u;
                                            @class.MoveTo(this.GStruct1_1);
                                        }
                                    }
                                    else
                                    {
                                        if (ThreeDCoord.smethod_0(@class.Position, this.GStruct1_1) && @class.RoomItem_0 == this)
                                        {
                                            @class.bool_1 = true;
                                            flag = true;
                                            if (@class.bool_6 && (@class.int_10 != this.int_1 || @class.int_11 != this.int_2))
                                            {
                                                @class.method_3(true);
                                            }
                                            @class.bool_0 = false;
                                            @class.bool_1 = true;
                                            @class.MoveTo(this.GStruct1_0);
                                        }
                                        else
                                        {
                                            this.uint_3 = 0u;
                                        }
                                    }
                                }
                                else
                                {
                                    this.uint_3 = 0u;
                                }
                            }
                            if (this.uint_4 > 0u)
                            {
                                RoomUser class3 = this.method_8().GetRoomUserByHabbo(this.uint_4);
                                if (class3 != null)
                                {
                                    flag = true;
                                    class3.method_6();
                                    if (ThreeDCoord.smethod_0(class3.Position, this.GStruct1_0))
                                    {
                                        class3.MoveTo(this.GStruct1_1);
                                    }
                                }
                                this.uint_4 = 0u;
                            }
                            if (flag)
                            {
                                if (this.ExtraData != "1")
                                {
                                    this.ExtraData = "1";
                                    this.UpdateState(false, true);
                                }
                            }
                            else
                            {
                                if (flag2)
                                {
                                    if (this.ExtraData != "2")
                                    {
                                        this.ExtraData = "2";
                                        this.UpdateState(false, true);
                                    }
                                }
                                else
                                {
                                    if (this.ExtraData != "0")
                                    {
                                        this.ExtraData = "0";
                                        this.UpdateState(false, true);
                                    }
                                }
                            }
                            this.ReqUpdate(1);
                            break;
                        }
                    case "bottle":
                        {
                            int num = GoldTree.smethod_5(0, 7);
                            this.ExtraData = num.ToString();
                            this.method_4();
                            break;
                        }
                    case "dice":
                        try
                        {
                            RoomUser @class = this.method_8().GetRoomUserByHabbo(this.uint_3);
                            if (@class.GetClient().GetHabbo().int_1 > 0)
                            {
                                this.ExtraData = @class.GetClient().GetHabbo().int_1.ToString();
                                @class.GetClient().GetHabbo().int_1 = 0;
                            }
                            else
                            {
                                int num = GoldTree.smethod_5(1, 6);
                                this.ExtraData = num.ToString();
                            }
                        }
                        catch
                        {
                            int num = GoldTree.smethod_5(1, 6);
                            this.ExtraData = num.ToString();
                        }
                        this.method_4();
                        break;
                    case "habbowheel":
                        {
                            int num = GoldTree.smethod_5(1, 10);
                            this.ExtraData = num.ToString();
                            this.method_4();
                            break;
                        }
                    case "loveshuffler":
                        if (this.ExtraData == "0")
                        {
                            int num = GoldTree.smethod_5(1, 4);
                            this.ExtraData = num.ToString();
                            this.ReqUpdate(20);
                        }
                        else
                        {
                            if (this.ExtraData != "-1")
                            {
                                this.ExtraData = "-1";
                            }
                        }
                        this.UpdateState(false, true);
                        break;
                    case "alert":
                        if (this.ExtraData == "1")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        break;
                    case "vendingmachine":
                        if (this.ExtraData == "1")
                        {
                            RoomUser @class = this.method_8().GetRoomUserByHabbo(this.uint_3);
                            if (@class != null)
                            {
                                @class.method_6();
                                int int_ = this.GetBaseItem().VendingIds[GoldTree.smethod_5(0, this.GetBaseItem().VendingIds.Count - 1)];
                                @class.CarryItem(int_);
                            }

                            this.uint_3 = 0u;
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        break;

                    case "wf_trg_onsay":
                    case "wf_trg_enterroom":
                    case "wf_trg_furnistate":
                    case "wf_trg_onfurni":
                    case "wf_trg_offfurni":
                    case "wf_trg_gameend":
                    case "wf_trg_gamestart":
                    case "wf_trg_atscore":
                    case "wf_act_saymsg":
                    case "wf_act_togglefurni":
                    case "wf_act_givepoints":
                    case "wf_act_moverotate":
                    case "wf_act_matchfurni":
                    case "wf_act_give_phx":
                    case "wf_cnd_trggrer_on_frn":
                    case "wf_cnd_furnis_hv_avtrs":
                    case "wf_cnd_has_furni_on":
                    case "wf_cnd_match_snapshot":
                    case "wf_cnd_phx":
                    case "bb_teleport":
                        if (this.ExtraData == "1")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        break;
                    case "wf_trg_timer":
                        if (this.ExtraData == "1")
                        {
                            this.ExtraData = "0";
                            this.UpdateState(false, true);
                        }
                        if (this.string_2.Length > 0)
                        {
                            this.method_8().method_15(this);
                            this.ReqUpdate(Convert.ToInt32(Convert.ToDouble(this.string_2) * 2.0));
                        }
                        break;
                    case "wf_act_moveuser":
                        if (this.dictionary_1.Count > 0 && this.method_8().RoomUsers != null)
                        {
                            int num4 = 0;
                            RoomUser[] RoomUser_ = this.method_8().RoomUsers;
                            for (int i = 0; i < RoomUser_.Length; i++)
                            {
                                RoomUser class4 = RoomUser_[i];
                                if (class4 != null)
                                {
                                    if (class4.IsBot)
                                    {
                                        this.dictionary_1.Remove(class4);
                                    }
                                    if (this.dictionary_1.ContainsKey(class4))
                                    {
                                        if (this.dictionary_1[class4] > 0)
                                        {
                                            if (this.dictionary_1[class4] == 10 && !class4.IsBot && class4 != null && class4.GetClient() != null && class4.GetClient().GetHabbo() != null)
                                            {
                                                class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(4, true);
                                            }
                                            Dictionary<RoomUser, int> dictionary;
                                            RoomUser key;
                                            (dictionary = this.dictionary_1)[key = class4] = dictionary[key] - 1;
                                            num4++;
                                        }
                                        else
                                        {
                                            this.dictionary_1.Remove(class4);
                                            class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(0, true);

                                            if (class4.team != Team.None && class4.game == Rooms.Games.Game.Freeze)
                                            {
                                                int FreezeEffect = ((int)class4.team) + 39;
                                                if (class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 != FreezeEffect)
                                                {
                                                    class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(FreezeEffect, true);
                                                }
                                            }

                                            else if (class4.team != Team.None && class4.game == Rooms.Games.Game.BattleBanzai)
                                            {
                                                int FreezeEffect = ((int)class4.team) + 32;
                                                if (class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 != FreezeEffect)
                                                {
                                                    class4.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(FreezeEffect, true);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (num4 > 0)
                            {
                                this.ReqUpdate(0);
                            }
                            else
                            {
                                this.dictionary_1.Clear();
                            }
                        }
                        break;
                    case "counter":
                        if (this.bool_0 && this.string_2 != "1")
                        {
                            this.ExtraData = Convert.ToString(Convert.ToInt32(this.ExtraData) - 1);
                            if (Convert.ToInt32(this.ExtraData) <= 0)
                            {
                                this.ExtraData = "0";
                                this.bool_0 = false;
                                this.method_8().method_89(0, this, true);

                                foreach (RoomItem Item in this.method_8().Hashtable_0.Values)
                                {
                                    if (Item.GetBaseItem().Name == "bb_apparatus")
                                    {
                                        Item.ExtraData = "0";
                                        Item.UpdateState(false, true);
                                        Item.ReqUpdate(1);
                                    }
                                }
                            }
                            this.UpdateState(true, true);
                            this.string_2 = "1";
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            if (this.bool_0)
                            {
                                this.string_2 = "0";
                                this.ReqUpdate(1);
                            }
                        }
                        break;
                    case "freeze_counter":
                        if (this.method_8().frzTimer && this.string_2 != "1")
                        {
                            this.ExtraData = Convert.ToString(Convert.ToInt32(this.ExtraData) - 1);
                            if (Convert.ToInt32(this.ExtraData) <= 0)
                            {
                                this.ExtraData = "0";
                                this.method_8().frzTimer = false;
                                this.method_8().GetFreeze().StopGame();
                            }
                            this.UpdateState(true, true);
                            this.string_2 = "1";
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            if (this.method_8().frzTimer)
                            {
                                this.string_2 = "0";
                                this.ReqUpdate(1);
                            }
                        }
                        break;
                    case "wf_trg_attime":
                        if (!this.WiredAtTimeNeedReset)
                        {
                            if (this.WiredAtTimeTimer < 0)
                            {
                                this.WiredAtTimeTimer = 0;
                            }
                            this.WiredAtTimeTimer += 0.5;
                            this.method_8().method_16(this);
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            this.WiredAtTimeNeedReset = true;
                        }
                        break;
                    case "wf_cnd_time_more_than":
                        if (!this.WiredNeedReset)
                        {
                            if (this.WiredCounter >= double.Parse(this.string_2, CultureInfo.InvariantCulture))
                            {
                                this.WiredNeedReset = true;
                            }
                            this.WiredCounter += 0.5;
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            this.WiredNeedReset = true;
                        }
                        break;
                    case "wf_cnd_time_less_than":
                        if (!this.WiredNeedReset)
                        {
                            if (this.WiredCounter >= double.Parse(this.string_2, CultureInfo.InvariantCulture))
                            {
                                this.WiredNeedReset = true;
                            }
                            this.WiredCounter += 0.5;
                            this.ReqUpdate(1);
                        }
                        else
                        {
                            this.WiredNeedReset = true;
                        }
                        break;
                }
            }
        }
		internal void ReqUpdate(int int_5)
		{
			this.int_4 = int_5;
			this.bool_1 = true;
		}
		internal void method_4()
		{
			this.UpdateState(true, true);
		}
		internal void UpdateState(bool bool_5, bool bool_6)
		{
			if (bool_5)
			{
				this.method_8().method_80(this);
			}
			if (bool_6)
			{
				ServerMessage Message = new ServerMessage();
				if (this.Boolean_2)
				{
					Message.Init(88u);
					Message.AppendStringWithBreak(this.uint_0.ToString());
					Message.AppendStringWithBreak(this.ExtraData);
				}
				else
				{
					Message.Init(85u);
					this.method_6(Message);
				}
				this.method_8().SendMessage(Message, null);
			}
		}
		internal void method_6(ServerMessage Message5_0)
		{
			if (this.Boolean_2)
			{
				Message5_0.AppendUInt(this.uint_0);
				Message5_0.AppendInt32(this.GetBaseItem().Sprite);
				Message5_0.AppendInt32(this.int_1);
				Message5_0.AppendInt32(this.int_2);
				Message5_0.AppendInt32(this.int_3);
				Message5_0.AppendStringWithBreak(this.double_0.ToString().Replace(',', '.'));
				if (this.GetBaseItem().Name == "song_disk" && this.ExtraData.Length > 0)
				{
					Message5_0.AppendInt32(Convert.ToInt32(this.ExtraData));
					Message5_0.AppendStringWithBreak("");
				}
				else
				{
					Message5_0.AppendInt32(0);
					Message5_0.AppendStringWithBreak(this.ExtraData);
				}
				Message5_0.AppendInt32(-1);
				Message5_0.AppendBoolean(!(this.GetBaseItem().InteractionType.ToLower() == "default"));
			}
			else
			{
				if (this.Boolean_1)
				{
					Message5_0.AppendStringWithBreak(string.Concat(this.uint_0));
					Message5_0.AppendInt32(this.GetBaseItem().Sprite);
					Message5_0.AppendStringWithBreak(this.string_7);
					if (this.GetBaseItem().Name.StartsWith("poster_"))
					{
						Message5_0.AppendString(this.GetBaseItem().Name.Split(new char[]
						{
							'_'
						})[1]);
					}
					string text = this.GetBaseItem().InteractionType.ToLower();
					if (text != null && text == "postit")
					{
						Message5_0.AppendStringWithBreak(this.ExtraData.Split(new char[]
						{
							' '
						})[0]);
					}
					else
					{
						Message5_0.AppendStringWithBreak(this.ExtraData);
					}
					Message5_0.AppendBoolean(!(this.GetBaseItem().InteractionType == "default"));
				}
			}
		}
		internal Item GetBaseItem()
		{
			if (this.Item == null)
			{
				this.Item = GoldTree.GetGame().GetItemManager().method_2(this.uint_2);
			}
			return this.Item;
		}
		internal Room method_8()
		{
			if (this.class14_0 == null)
			{
				this.class14_0 = GoldTree.GetGame().GetRoomManager().GetRoom(this.uint_1);
			}
			return this.class14_0;
		}
		internal void method_9()
		{
			if (!(this.string_4 == ""))
			{
				string[] collection = this.string_4.Split(new char[]
				{
					','
				});
				IEnumerable<string> enumerable = new List<string>(collection);
				List<string> list = enumerable.ToList<string>();
				bool flag = false;
				if (list.Count > 5)
				{
					this.string_4 = "";
					this.string_5 = "";
				}
				else
				{
					foreach (string current in enumerable)
					{
						RoomItem @class = null;
						if (current.Length > 0)
						{
							@class = this.method_8().method_28(Convert.ToUInt32(current));
						}
						if (@class == null)
						{
							list.Remove(current);
							flag = true;
						}
					}
					if (flag)
					{
						this.string_5 = OldEncoding.encodeVL64(list.Count);
						for (int i = 0; i < list.Count; i++)
						{
							int value = Convert.ToInt32(list[i]);
							this.string_5 += OldEncoding.encodeVL64(value);
							this.string_4 = this.string_4 + "," + Convert.ToString(value);
						}
						this.string_4 = this.string_4.Substring(1);
					}
				}
			}
		}
		internal void method_10()
		{
			if (!(this.string_3 == ""))
			{
				string[] collection = this.string_3.Split(new char[]
				{
					','
				});
				IEnumerable<string> enumerable = new List<string>(collection);
				List<string> list = enumerable.ToList<string>();
				bool flag = false;
				foreach (string current in enumerable)
				{
					RoomItem @class = this.method_8().method_28(Convert.ToUInt32(current));
					if (@class == null)
					{
						list.Remove(current);
						flag = true;
					}
				}
				if (flag)
				{
					this.string_2 = OldEncoding.encodeVL64(list.Count);
					for (int i = 0; i < list.Count; i++)
					{
						int num = Convert.ToInt32(list[i]);
						this.string_2 += OldEncoding.encodeVL64(num);
					}
					this.string_3 = string.Join(",", list.ToArray());
				}
			}
		}
	}
}
