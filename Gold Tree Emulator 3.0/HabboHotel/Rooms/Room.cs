using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

using GoldTree.Core;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Rooms;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Pets;
using GoldTree.HabboHotel.Pathfinding;
using GoldTree.Util;
using GoldTree.Messages;
using GoldTree.HabboHotel.RoomBots;
using GoldTree.HabboHotel.Navigators;
using GoldTree.Storage;
using GoldTree.HabboHotel.Users;
using GoldTree.HabboHotel.SoundMachine;
using GoldTree.Source.HabboHotel.SoundMachine;
using GoldTree.HabboHotel.Rooms.Games;

namespace GoldTree.HabboHotel.Rooms
{
    internal sealed class Room
    {
        public delegate void Delegate2(int Team);

        public uint Id;

        public uint Achievement;

        public string Name;
        public string Description;
        public string Type;
        public string Owner;

        public string Password;

        public int Category;
        public int State;

        public int UsersNow;
        public int UsersMax;

        public string ModelName;
        public string CCTs;

        public int Score;

        public List<string> Tags;

        public bool AllowPet;
        public bool AllowPetsEating;
        public bool AllowWalkthrough;

        public bool Hidewall;

        public int Wallthick;
        public int Floorthick;

        internal bool bool_4;
        internal bool bool_5;

        private Timer timer_0;

        private bool bool_6;
        private bool bool_7;

        internal RoomUser[] RoomUsers;

        public int int_7 = 0;
        private int int_8;

        public RoomIcon RoomIcon;

        public List<uint> UsersWithRights;

        internal bool bool_8;

        private Dictionary<uint, double> dictionary_0;

        public RoomEvent Event;

        public string Wallpaper;
        public string Floor;
        public string Landscape;

        private Hashtable hashtable_0;
        private Hashtable hashtable_1;
        private Hashtable hashtable_2;
        private Hashtable hashtable_3;
        private Hashtable hashtable_4;

        public MoodlightData MoodlightData;

        public List<Trade> list_2;

        public bool bool_9;

        public List<RoomItem> list_3;

        public List<uint> list_4;

        public List<RoomItem> list_5;
        public List<RoomItem> list_6;
        public List<RoomItem> list_7;
        public List<RoomItem> list_8;
        public List<RoomItem> list_9;
        public List<RoomItem> list_10;
        public List<RoomItem> list_11;
        public List<RoomItem> list_12;
        public List<RoomItem> list_13;

        public int int_9;
        public int int_10;
        public int int_11;
        public int int_12;

        private bool bool_10;

        public List<RoomItem> list_14;
        public List<RoomItem> list_15;
        public List<RoomItem> list_16;

        public List<GroupsManager> list_17;

        public double[,] double_0;
        private byte[,] byte_0;
        public ThreeDCoord[,] gstruct1_0;
        private byte[,] byte_1;
        private byte[,] byte_2;
        private double[,] double_1;
        private double[,] double_2;

        private RoomModel class28_0;

        private bool bool_11;
        private int int_14;
        private int int_15;

        private RoomData class27_0;

        private int int_16;
        private bool bool_12;

        private bool[,] HeightOverride;

        public int CurrentPollId;

        private RoomMusicController MusicController;

        public bool frzTimer = false;

        internal TeamManager TeamManager;
        private GameManager GameManager;
        private Freeze freeze;

        public List<int> InfobusAnswers;

        internal bool isCycling = false;

        public bool HasEvent
        {
            get
            {
                return this.Event != null;
            }
        }

        internal bool Boolean_1
        {
            get
            {
                return this.bool_11;
            }
            set
            {
                this.bool_11 = value;
            }
        }

        internal bool GotMusicController()
        {
            return (this.MusicController != null);
        }

        public int UserCount
        {
            get
            {
                int num = 0;
                int result;
                if (this.RoomUsers == null)
                {
                    result = 0;
                }
                else
                {
                    for (int i = 0; i < this.RoomUsers.Length; i++)
                    {
                        if (this.RoomUsers[i] != null && !this.RoomUsers[i].IsBot && !this.RoomUsers[i].IsPet)
                        {
                            num++;
                        }
                    }
                    result = num;
                }
                return result;
            }
        }

        public int Int32_1
        {
            get
            {
                return this.Tags.Count;
            }
        }

        public RoomModel RoomModel
        {
            get
            {
                return this.class28_0;
            }
        }

        public Hashtable Hashtable_0
        {
            get
            {
                Hashtable result;
                if (this.hashtable_0 != null)
                {
                    result = (this.hashtable_0.Clone() as Hashtable);
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }

        public Hashtable Hashtable_1
        {
            get
            {
                return this.hashtable_4.Clone() as Hashtable;
            }
        }

        public bool CanTrade
        {
            get
            {
                if (this.IsPublic)
                    return false;
                else
                {
                    FlatCat category = GoldTree.GetGame().GetNavigator().method_2(this.Category);
                    return (category != null && category.CanTrade);
                }
            }
        }

        public bool IsPublic
        {
            get
            {
                return this.Type == "public";
            }
        }

        public int PetCount
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.RoomUsers.Length; i++)
                {
                    RoomUser @class = this.RoomUsers[i];
                    if (@class != null && @class.IsPet)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        internal RoomData RoomData
        {
            get
            {
                this.class27_0.Fill(this);
                return this.class27_0;
            }
        }

        public byte[,] Byte_0
        {
            get
            {
                return this.byte_0;
            }
        }

        internal bool Boolean_4
        {
            get
            {
                return this.GetPets().Count > 0;
            }
        }

        internal RoomMusicController GetRoomMusicController()
        {
            if (this.MusicController == null)
            {
                this.MusicController = new RoomMusicController();
            }
            return this.MusicController;
        }

        internal void LoadMusic()
        {
            DataTable table;
            DataTable table2;
            using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
            {
                table = @class.ReadDataTable("SELECT * FROM items_rooms_songs WHERE roomid = '" + Id + "'"); // <-- old
                table2 = @class.ReadDataTable("SELECT * FROM items_jukebox_songs WHERE jukeboxid = '" + this.GetRoomMusicController().LinkedItemId + "'"); // <-- new
            }

            foreach (DataRow row in table.Rows)
            {
                int songID = (int)row["songid"];
                uint num2 = (uint)row["itemid"];
                int baseItem = (int)row["baseitem"];
                SongItem diskItem = new SongItem((int)num2, songID, baseItem);
                diskItem.RemoveFromDatabase();
                diskItem.SaveToDatabase(this.GetRoomMusicController().LinkedItemId);
                this.GetRoomMusicController().AddDisk(diskItem);
            }

            foreach (DataRow row in table2.Rows)
            {
                int songID = (int)row["songid"];
                uint num2 = (uint)row["itemid"];
                int baseItem = (int)row["baseitem"];
                SongItem diskItem = new SongItem((int)num2, songID, baseItem);
                this.GetRoomMusicController().AddDisk(diskItem);
            }
        }

        public Room(uint uint_2, string name, string description, string type, string string_13, int int_17, int int_18, int int_19, string string_14, string string_15, int int_20, List<string> list_18, bool bool_13, bool bool_14, bool bool_15, bool bool_16, RoomIcon class29_1, string string_16, string string_17, string string_18, string string_19, RoomData class27_1, bool bool_17, int int_21, int int_22, uint uint_3)
        {
            this.bool_12 = false;
            this.Id = uint_2;
            this.Name = name;
            this.Description = description;
            this.Owner = string_13;
            this.Category = int_17;
            this.Type = type;
            this.State = int_18;
            this.UsersNow = 0;
            this.UsersMax = int_19;
            this.ModelName = string_14;
            this.CCTs = string_15;
            this.Score = int_20;
            this.Tags = list_18;
            this.AllowPet = bool_13;
            this.AllowPetsEating = bool_14;
            this.AllowWalkthrough = bool_15;
            this.Hidewall = bool_16;
            this.Wallthick = int_21;
            this.Floorthick = int_22;
            this.int_7 = 0;
            this.RoomUsers = new RoomUser[500];
            this.RoomIcon = class29_1;
            this.Password = string_16;
            this.dictionary_0 = new Dictionary<uint, double>();
            this.Event = null;
            this.Wallpaper = string_17;
            this.Floor = string_18;
            this.Landscape = string_19;
            this.hashtable_4 = new Hashtable();
            this.hashtable_0 = new Hashtable();
            this.list_2 = new List<Trade>();
            this.class28_0 = GoldTree.GetGame().GetRoomManager().GetModel(this.ModelName, this.Id);
            this.bool_6 = false;
            this.bool_7 = false;
            this.bool_5 = true;
            this.class27_0 = class27_1;
            this.bool_8 = bool_17;
            this.list_17 = new List<GroupsManager>();
            this.list_4 = new List<uint>();
            this.list_5 = new List<RoomItem>();
            this.list_9 = new List<RoomItem>();
            this.list_7 = new List<RoomItem>();
            this.list_6 = new List<RoomItem>();
            this.list_8 = new List<RoomItem>();
            this.list_10 = new List<RoomItem>();
            this.list_11 = new List<RoomItem>();
            this.list_12 = new List<RoomItem>();
            this.list_13 = new List<RoomItem>();
            this.int_10 = 0;
            this.int_11 = 0;
            this.int_9 = 0;
            this.int_12 = 0;
            this.list_3 = new List<RoomItem>();
            this.list_14 = new List<RoomItem>();
            this.list_15 = new List<RoomItem>();
            this.list_16 = new List<RoomItem>();
            this.byte_0 = new byte[this.RoomModel.int_4, this.RoomModel.int_5];
            this.double_1 = new double[this.RoomModel.int_4, this.RoomModel.int_5];
            this.double_2 = new double[this.RoomModel.int_4, this.RoomModel.int_5];
            //this.timer_0 = new Timer(new TimerCallback(this.method_32), null, 480, 480);
            this.int_8 = 0;
            this.bool_4 = false;
            this.bool_9 = true;
            this.bool_11 = false;
            this.int_16 = 0;
            this.int_15 = 4;
            this.Achievement = uint_3;
            this.bool_10 = false;
            this.hashtable_1 = new Hashtable();
            this.hashtable_2 = new Hashtable();
            this.hashtable_3 = new Hashtable();
            this.method_23();
            this.method_25();
            this.method_22();
            this.LoadMusic();
            this.InfobusAnswers = new List<int>();
        }

        public void method_0()
        {
            List<RoomBot> list = GoldTree.GetGame().GetBotManager().method_2(this.Id);
            foreach (RoomBot current in list)
            {
                this.BotToRoomUser(current);
            }
        }

        public void method_1()
        {
            new List<Pet>();
            using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
            {
                @class.AddParamWithValue("roomid", this.Id);
                DataTable dataTable = @class.ReadDataTable("SELECT Id, user_id, room_id, name, type, race, color, expirience, energy, nutrition, respect, createstamp, x, y, z FROM user_pets WHERE room_id = @roomid;");
                if (dataTable != null)
                {
                    foreach (DataRow dataRow_ in dataTable.Rows)
                    {
                        Pet class2 = GoldTree.GetGame().GetCatalog().method_12(dataRow_);
                        List<RandomSpeech> list = new List<RandomSpeech>();
                        List<BotResponse> list2 = new List<BotResponse>();
                        this.method_4(new RoomBot(class2.PetId, this.Id, AIType.const_0, "freeroam", class2.Name, "", class2.Look, class2.X, class2.Y, (int)class2.Z, 0, 0, 0, 0, 0, ref list, ref list2, 0), class2);
                    }
                }
            }
        }

        internal List<Pet> GetPets()
        {
            List<Pet> list = new List<Pet>();

            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                if (this.RoomUsers[i] != null && this.RoomUsers[i].IsPet)
                {
                    list.Add(this.RoomUsers[i].PetData);
                }
            }

            return list;
        }

        public RoomUser BotToRoomUser(RoomBot class34_0)
        {
            return this.method_4(class34_0, null);
        }

        public RoomUser method_4(RoomBot Bot, Pet PetData)
        {
            int num = this.method_5();

            RoomUser user = new RoomUser(Convert.ToUInt32(num + 100000), this.Id, this.int_7++, true);

            user.int_20 = num;

            this.RoomUsers[num] = user;
            if (Bot.x > 0 && Bot.y > 0 && Bot.x < this.RoomModel.int_4 && Bot.y < this.RoomModel.int_5)
            {
                user.method_7(Bot.x, Bot.y, Bot.z);
                user.method_9(Bot.Rotation);
            }
            else
            {
                Bot.x = this.RoomModel.int_0;
                Bot.y = this.RoomModel.int_1;
                user.method_7(this.RoomModel.int_0, this.RoomModel.int_1, this.RoomModel.double_0);
                user.method_9(this.RoomModel.int_2);
            }

            user.RoomBot = Bot;
            user.BotAI = Bot.method_4(user.VirtualId);

            if (user.IsPet)
            {
                user.BotAI.Init((int)Bot.Id, user.VirtualId, this.Id);
                user.PetData = PetData;
                user.PetData.VirtualId = user.VirtualId;
            }
            else
            {
                user.BotAI.Init(-1, user.VirtualId, this.Id);
            }
            this.method_87(user, true, true);
            user.UpdateNeeded = true;
            ServerMessage Message = new ServerMessage(28u);
            Message.AppendInt32(1);
            user.method_14(Message);
            this.SendMessage(Message, null);
            user.BotAI.OnSelfEnterRoom();
            return user;
        }

        private int method_5()
        {
            return Array.IndexOf<RoomUser>(this.RoomUsers, null);
        }

        public void method_6(int int_17, bool bool_13)
        {
            RoomUser @class = this.method_52(int_17);
            if (@class != null && @class.IsBot)
            {
                @class.BotAI.OnSelfLeaveRoom(bool_13);
                ServerMessage Message = new ServerMessage(29u);
                Message.AppendRawInt32(@class.VirtualId);
                this.SendMessage(Message, null);
                uint num = @class.UId;
                for (int i = 0; i < this.RoomUsers.Length; i++)
                {
                    RoomUser class2 = this.RoomUsers[i];
                    if (class2 != null && class2.UId == num)
                    {
                        this.RoomUsers[i] = null;
                    }
                }
            }
        }
        public void method_7(RoomUser RoomUser_1, string string_10, bool bool_13)
        {
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && @class.IsBot)
                {
                    if (bool_13)
                    {
                        @class.BotAI.OnUserShout(RoomUser_1, string_10);
                    }
                    else
                    {
                        @class.BotAI.OnUserSay(RoomUser_1, string_10);
                    }
                }
            }
        }
        public void method_8(RoomUser RoomUser_1)
        {
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_enterroom")
                    {
                        this.method_21(RoomUser_1, current, "");
                    }
                }
            }
            catch
            {
            }
        }
        public bool method_9(RoomUser RoomUser_1, string string_10)
        {
            bool result = false;
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_onsay" && this.method_21(RoomUser_1, current, string_10.ToLower()))
                    {
                        result = true;
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        public void method_10(RoomUser RoomUser_1, RoomItem RoomItem_0)
        {
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_furnistate")
                    {
                        this.method_21(RoomUser_1, current, Convert.ToString(RoomItem_0.uint_0));
                    }
                }
            }
            catch
            {
            }
        }
        public void method_11(RoomUser RoomUser_1, RoomItem RoomItem_0)
        {
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_onfurni")
                    {
                        this.method_21(RoomUser_1, current, Convert.ToString(RoomItem_0.uint_0));
                    }
                }
            }
            catch
            {
            }
        }
        public void method_12(RoomUser RoomUser_1, RoomItem RoomItem_0)
        {
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_offfurni")
                    {
                        this.method_21(RoomUser_1, current, Convert.ToString(RoomItem_0.uint_0));
                    }
                }
            }
            catch
            {
            }
        }
        public void method_13()
        {
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_gameend")
                    {
                        this.method_21(null, current, "GameEnded");
                    }
                }
            }
            catch
            {
            }
        }
        public void method_14(RoomUser RoomUser_1)
        {
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_gamestart")
                    {
                        this.method_21(RoomUser_1, current, "GameBegun");
                    }
                }
            }
            catch
            {
            }
        }
        public void method_15(RoomItem RoomItem_0)
        {
            this.method_21(null, RoomItem_0, "Timer");
        }
        public void method_16(RoomItem RoomItem_0)
        {
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_attime" && current.WiredAtTimeTimer >= double.Parse(current.string_2, CultureInfo.InvariantCulture))
                    {
                        this.method_21(null, current, "AtTime");
                        current.WiredAtTimeTimer = 0;
                        current.WiredAtTimeNeedReset = true;
                    }
                }
            }
            catch
            {
            }
        }
        public void method_17(int int_17)
        {
            try
            {
                foreach (RoomItem current in this.list_14)
                {
                    if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_atscore" && current.string_2 != "" && Convert.ToDouble(current.string_2) == (double)int_17)
                    {
                        this.method_21(null, current, "TheScore");
                    }
                }
            }
            catch
            {
            }
        }
        public bool method_18(RoomUser RoomUser_1, string string_10, string string_11)
        {
            string_11 = this.method_20(RoomUser_1, string_11);
            bool result;
            if (string_10 != null)
            {
                if (MusCommands.dictionary_4 == null)
                {
                    MusCommands.dictionary_4 = new Dictionary<string, int>(39)
					{

						{
							"roomuserseq",
							0
						},

						{
							"roomuserslt",
							1
						},

						{
							"roomusersmt",
							2
						},

						{
							"roomusersmte",
							3
						},

						{
							"roomuserslte",
							4
						},

						{
							"userhasachievement",
							5
						},

						{
							"userhasntachievement",
							6
						},

						{
							"userhasbadge",
							7
						},

						{
							"userhasntbadge",
							8
						},

						{
							"userhasvip",
							9
						},

						{
							"userhasntvip",
							10
						},

						{
							"userhaseffect",
							11
						},

						{
							"userhasnteffect",
							12
						},

						{
							"userrankeq",
							13
						},

						{
							"userrankmt",
							14
						},

						{
							"userrankmte",
							15
						},

						{
							"userranklt",
							16
						},

						{
							"userranklte",
							17
						},

						{
							"usercreditseq",
							18
						},

						{
							"usercreditsmt",
							19
						},

						{
							"usercreditsmte",
							20
						},

						{
							"usercreditslt",
							21
						},

						{
							"usercreditslte",
							22
						},

						{
							"userpixelseq",
							23
						},

						{
							"userpixelsmt",
							24
						},

						{
							"userpixelsmte",
							25
						},

						{
							"userpixelslt",
							26
						},

						{
							"userpixelslte",
							27
						},

						{
							"userpointseq",
							28
						},

						{
							"userpointsmt",
							29
						},

						{
							"userpointsmte",
							30
						},

						{
							"userpointslt",
							31
						},

						{
							"userpointslte",
							32
						},

						{
							"usergroupeq",
							33
						},

						{
							"userisingroup",
							34
						},

						{
							"wearing",
							35
						},

						{
							"notwearing",
							36
						},

						{
							"carrying",
							37
						},

						{
							"notcarrying",
							38
						},

                        {
							"wiredactived",
							39
						},

                        {
							"notwiredactived",
							40
						}
					};
                }
                int num;
                if (MusCommands.dictionary_4.TryGetValue(string_10, out num))
                {
                    switch (num)
                    {
                        case 0:
                            if (this.UserCount == Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 1:
                            if (this.UserCount < Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 2:
                            if (this.UserCount > Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 3:
                            if (this.UserCount >= Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 4:
                            if (this.UserCount <= Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 5:
                            result = GoldTree.GetGame().GetAchievementManager().method_1(RoomUser_1.GetClient(), (uint)Convert.ToUInt16(string_11), 1);
                            return result;
                        case 6:
                            if (!GoldTree.GetGame().GetAchievementManager().method_1(RoomUser_1.GetClient(), (uint)Convert.ToUInt16(string_11), 1))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 7:
                            result = RoomUser_1.GetClient().GetHabbo().GetBadgeComponent().HasBadge(string_11);
                            return result;
                        case 8:
                            if (!RoomUser_1.GetClient().GetHabbo().GetBadgeComponent().HasBadge(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 9:
                            result = RoomUser_1.GetClient().GetHabbo().IsVIP;
                            return result;
                        case 10:
                            if (!RoomUser_1.GetClient().GetHabbo().IsVIP)
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 11:
                            if (RoomUser_1.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 == Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 12:
                            if (RoomUser_1.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 != Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 13:
                            if ((ulong)RoomUser_1.GetClient().GetHabbo().Rank == (ulong)((long)Convert.ToInt32(string_11)))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 14:
                            if ((ulong)RoomUser_1.GetClient().GetHabbo().Rank > (ulong)((long)Convert.ToInt32(string_11)))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 15:
                            if ((ulong)RoomUser_1.GetClient().GetHabbo().Rank >= (ulong)((long)Convert.ToInt32(string_11)))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 16:
                            if ((ulong)RoomUser_1.GetClient().GetHabbo().Rank < (ulong)((long)Convert.ToInt32(string_11)))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 17:
                            if ((ulong)RoomUser_1.GetClient().GetHabbo().Rank <= (ulong)((long)Convert.ToInt32(string_11)))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 18:
                            if (RoomUser_1.GetClient().GetHabbo().Credits == Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 19:
                            if (RoomUser_1.GetClient().GetHabbo().Credits > Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 20:
                            if (RoomUser_1.GetClient().GetHabbo().Credits >= Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 21:
                            if (RoomUser_1.GetClient().GetHabbo().Credits < Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 22:
                            if (RoomUser_1.GetClient().GetHabbo().Credits <= Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 23:
                            if (RoomUser_1.GetClient().GetHabbo().ActivityPoints == Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 24:
                            if (RoomUser_1.GetClient().GetHabbo().ActivityPoints > Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 25:
                            if (RoomUser_1.GetClient().GetHabbo().ActivityPoints >= Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 26:
                            if (RoomUser_1.GetClient().GetHabbo().ActivityPoints < Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 27:
                            if (RoomUser_1.GetClient().GetHabbo().ActivityPoints <= Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 28:
                            if (RoomUser_1.GetClient().GetHabbo().VipPoints == Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 29:
                            if (RoomUser_1.GetClient().GetHabbo().VipPoints > Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 30:
                            if (RoomUser_1.GetClient().GetHabbo().VipPoints >= Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 31:
                            if (RoomUser_1.GetClient().GetHabbo().VipPoints < Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 32:
                            if (RoomUser_1.GetClient().GetHabbo().VipPoints <= Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 33:
                            if (RoomUser_1.GetClient().GetHabbo().int_0 == Convert.ToInt32(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 34:
                            {
                                IEnumerator enumerator = RoomUser_1.GetClient().GetHabbo().dataTable_0.Rows.GetEnumerator();
                                try
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        DataRow dataRow = (DataRow)enumerator.Current;
                                        if ((int)dataRow["groupid"] == Convert.ToInt32(string_11))
                                        {
                                            result = true;
                                            return result;
                                        }
                                    }
                                    goto IL_89E;
                                }
                                finally
                                {
                                    IDisposable disposable = enumerator as IDisposable;
                                    if (disposable != null)
                                    {
                                        disposable.Dispose();
                                    }
                                }
                            }
                        case 35:
                            break;
                        case 36:
                            if (!RoomUser_1.GetClient().GetHabbo().Figure.Contains(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 37:
                            if (this.GetRoomUserByHabbo(RoomUser_1.GetClient().GetHabbo().Id).CarryItemID == (int)Convert.ToInt16(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 38:
                            if (this.GetRoomUserByHabbo(RoomUser_1.GetClient().GetHabbo().Id).CarryItemID != (int)Convert.ToInt16(string_11))
                            {
                                result = true;
                                return result;
                            }
                            goto IL_89E;
                        case 39:
                            {
                                using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                                {
                                    @class.AddParamWithValue("itemid", string_11);
                                    @class.AddParamWithValue("userid", RoomUser_1.GetClient().GetHabbo().Id);

                                    DataRow dataRow2 = @class.ReadDataRow("SELECT wiredid FROM user_wiredactived WHERE wiredid = @itemid AND userid = @userid");
                                    if (dataRow2 != null)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            goto IL_89E;
                        case 40:
                            {
                                using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                                {
                                    @class.AddParamWithValue("itemid", string_11);
                                    @class.AddParamWithValue("userid", RoomUser_1.GetClient().GetHabbo().Id);

                                    DataRow dataRow2 = @class.ReadDataRow("SELECT wiredid FROM user_wiredactived WHERE wiredid = @itemid AND userid = @userid");
                                    if (dataRow2 == null)
                                    {
                                        result = true;
                                        return result;
                                    }
                                }
                            }
                            goto IL_89E;
                        default:
                            goto IL_89E;
                    }
                    if (RoomUser_1.GetClient().GetHabbo().Figure.Contains(string_11))
                    {
                        result = true;
                        return result;
                    }
                }
            }
        IL_89E:
            result = false;
            return result;
        }
        public void method_19(RoomUser RoomUser_1, string string_10, string string_11, uint Item)
        {
            string_11 = this.method_20(RoomUser_1, string_11);
            if (string_10 != null)
            {
                if (MusCommands.dictionary_5 == null)
                {
                    MusCommands.dictionary_5 = new Dictionary<string, int>(13)
					{

						{
							"sql",
							0
						},

						{
							"badge",
							1
						},

						{
							"effect",
							2
						},

						{
							"award",
							3
						},

						{
							"dance",
							4
						},

						{
							"send",
							5
						},

						{
							"credits",
							6
						},

						{
							"pixels",
							7
						},

						{
							"points",
							8
						},

						{
							"rank",
							9
						},

						{
							"respect",
							10
						},

						{
							"handitem",
							11
						},

						{
							"alert",
							12
						},

                        {
							"wiredactived",
							13
						}
					};
                }
                int num;
                if (MusCommands.dictionary_5.TryGetValue(string_10, out num))
                {
                    switch (num)
                    {
                        case 0:
                            using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                            {
                                @class.ExecuteQuery(string_11);
                                return;
                            }
                        case 1:
                            break;
                        case 2:
                            if (RoomUser_1.GetClient() != null)
                            {
                                RoomUser_1.GetClient().GetHabbo().GetEffectsInventoryComponent().method_0(Convert.ToInt32(string_11), 3600);
                                RoomUser_1.GetClient().GetHabbo().GetEffectsInventoryComponent().method_3(Convert.ToInt32(string_11));
                                return;
                            }
                            return;
                        case 3:
                            if (RoomUser_1.GetClient() != null)
                            {
                                GoldTree.GetGame().GetAchievementManager().addAchievement(RoomUser_1.GetClient(), Convert.ToUInt32(string_11));
                                return;
                            }
                            return;
                        case 4:
                            if (RoomUser_1.GetClient() != null)
                            {
                                RoomUser class2 = this.GetRoomUserByHabbo(RoomUser_1.GetClient().GetHabbo().Id);
                                class2.DanceId = Convert.ToInt32(string_11);
                                ServerMessage Message = new ServerMessage(480u);
                                Message.AppendInt32(class2.VirtualId);
                                Message.AppendInt32(Convert.ToInt32(string_11));
                                this.SendMessage(Message, null);
                                return;
                            }
                            return;
                        case 5:
                            {
                                if (RoomUser_1.GetClient() == null)
                                {
                                    return;
                                }
                                uint num2 = Convert.ToUInt32(string_11);
                                Room class3;
                                if (GoldTree.GetGame().GetRoomManager().method_13(num2) || GoldTree.GetGame().GetRoomManager().method_14(num2))
                                {
                                    class3 = GoldTree.GetGame().GetRoomManager().GetRoom(num2);
                                }
                                else
                                {
                                    class3 = GoldTree.GetGame().GetRoomManager().method_15(num2);
                                }
                                if (RoomUser_1 == null)
                                {
                                    return;
                                }
                                if (class3 == null)
                                {
                                    this.method_47(RoomUser_1.GetClient(), true, false);
                                    return;
                                }
                                ServerMessage Message2 = new ServerMessage(286u);
                                Message2.AppendBoolean(class3.IsPublic);
                                Message2.AppendUInt(Convert.ToUInt32(string_11));
                                RoomUser_1.GetClient().SendMessage(Message2);
                                return;
                            }
                        case 6:
                            if (RoomUser_1.GetClient() != null)
                            {
                                RoomUser_1.GetClient().GetHabbo().Credits = RoomUser_1.GetClient().GetHabbo().Credits + Convert.ToInt32(string_11);
                                RoomUser_1.GetClient().GetHabbo().UpdateCredits(true);
                                return;
                            }
                            return;
                        case 7:
                            if (RoomUser_1.GetClient() != null)
                            {
                                RoomUser_1.GetClient().GetHabbo().ActivityPoints = RoomUser_1.GetClient().GetHabbo().ActivityPoints + Convert.ToInt32(string_11);
                                RoomUser_1.GetClient().GetHabbo().UpdateActivityPoints(true);
                                return;
                            }
                            return;
                        case 8:
                            if (RoomUser_1.GetClient() != null)
                            {
                                RoomUser_1.GetClient().GetHabbo().VipPoints = RoomUser_1.GetClient().GetHabbo().VipPoints + Convert.ToInt32(string_11);
                                RoomUser_1.GetClient().GetHabbo().UpdateVipPoints(false, true);
                                return;
                            }
                            return;
                        case 9:
                            if (RoomUser_1.GetClient() != null && (int)Convert.ToUInt16(string_11) < GoldTree.GetGame().GetRoleManager().method_9())
                            {
                                using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                                {
                                    @class.ExecuteQuery(string.Concat(new object[]
								{
									"UPDATE users SET rank = '",
									Convert.ToUInt16(string_11),
									"' WHERE Id = ",
									RoomUser_1.GetClient().GetHabbo().Id,
									" LIMIT 1;"
								}));
                                }
                                RoomUser_1.GetClient().method_12();
                                return;
                            }
                            return;
                        case 10:
                            {
                                if (RoomUser_1.GetClient() == null)
                                {
                                    return;
                                }
                                RoomUser_1.GetClient().GetHabbo().Respect++;
                                using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                                {
                                    @class.ExecuteQuery("UPDATE user_stats SET Respect = respect + 1 WHERE Id = '" + RoomUser_1.GetClient().GetHabbo().Id + "' LIMIT 1");
                                }
                                ServerMessage Message3 = new ServerMessage(440u);
                                Message3.AppendUInt(RoomUser_1.GetClient().GetHabbo().Id);
                                Message3.AppendInt32(RoomUser_1.GetClient().GetHabbo().Respect);
                                this.SendMessage(Message3, null);
                                RoomUser_1.GetClient().GetHabbo().CheckRespectReceivedAchievements();
                                return;
                            }
                        case 11:
                            if (RoomUser_1.GetClient() != null)
                            {
                                this.GetRoomUserByHabbo(RoomUser_1.GetClient().GetHabbo().Id).CarryItem((int)Convert.ToInt16(string_11));
                                return;
                            }
                            return;
                        case 12:
                            if (RoomUser_1.GetClient() != null)
                            {
                                RoomUser_1.GetClient().SendNotification(string_11);
                                return;
                            }
                            return;
                        case 13:
                            if (RoomUser_1.GetClient() != null)
                            {
                                using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                                {
                                    @class.AddParamWithValue("itemid", Convert.ToInt32(Item));
                                    @class.AddParamWithValue("userid", RoomUser_1.GetClient().GetHabbo().Id);

                                    DataRow dataRow2 = @class.ReadDataRow("SELECT wiredid FROM user_wiredactived WHERE wiredid = @itemid AND userid = @userid");
                                    if (dataRow2 == null)
                                    {
                                        @class.ExecuteQuery("INSERT INTO user_wiredactived (wiredid, userid) VALUES (@itemid, @userid)");
                                    }
                                }
                                return;
                            }
                            return;
                        default:
                            return;
                    }
                    if (RoomUser_1.GetClient() != null)
                    {
                        RoomUser_1.GetClient().GetHabbo().GetBadgeComponent().SendBadge(RoomUser_1.GetClient(), GoldTree.FilterString(string_11), true);
                        RoomUser_1.GetClient().SendMessage(RoomUser_1.GetClient().GetHabbo().GetBadgeComponent().ComposeBadgeListMessage());
                    }
                }
            }
        }
        public string method_20(RoomUser RoomUser_1, string string_10)
        {
            if (RoomUser_1 != null)
            {
                if (string_10.ToUpper().Contains("#USERNAME#"))
                {
                    string_10 = Regex.Replace(string_10, "#USERNAME#", RoomUser_1.GetClient().GetHabbo().Username, RegexOptions.IgnoreCase);
                }
                if (string_10.ToUpper().Contains("#USERID#"))
                {
                    string_10 = Regex.Replace(string_10, "#USERID#", RoomUser_1.GetClient().GetHabbo().Id.ToString(), RegexOptions.IgnoreCase);
                }
                if (string_10.ToUpper().Contains("#USERRANK#"))
                {
                    string_10 = Regex.Replace(string_10, "#USERRANK#", RoomUser_1.GetClient().GetHabbo().Rank.ToString(), RegexOptions.IgnoreCase);
                }
            }
            if (string_10.ToUpper().Contains("#ROOMNAME#"))
            {
                string_10 = Regex.Replace(string_10, "#ROOMNAME#", this.Name, RegexOptions.IgnoreCase);
            }
            if (string_10.ToUpper().Contains("#ROOMID#"))
            {
                string_10 = Regex.Replace(string_10, "#ROOMID#", this.Id.ToString(), RegexOptions.IgnoreCase);
            }
            int num = GoldTree.GetGame().GetClientManager().ClientCount + -1;
            int int32_ = GoldTree.GetGame().GetRoomManager().LoadedRoomsCount;
            if (string_10.ToUpper().Contains("#ONLINECOUNT#"))
            {
                string_10 = Regex.Replace(string_10, "#ONLINECOUNT#", num.ToString(), RegexOptions.IgnoreCase);
            }
            if (string_10.ToUpper().Contains("#ROOMSLOADED#"))
            {
                string_10 = Regex.Replace(string_10, "#ROOMSLOADED#", int32_.ToString(), RegexOptions.IgnoreCase);
            }
            return string_10;
        }
        public bool method_21(RoomUser RoomUser_1, RoomItem RoomItem_0, string string_10)
        {
            bool result;
            try
            {
                if (this.bool_6 || this.bool_7)
                {
                    result = false;
                }
                else
                {
                    bool flag = false;
                    int num = 0;
                    int num2 = 0;
                    bool flag2 = false;
                    string text = RoomItem_0.GetBaseItem().InteractionType.ToLower();
                    switch (text)
                    {
                        case "wf_trg_onsay":
                            if (string_10.Contains(RoomItem_0.string_2.ToLower()))
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_enterroom":
                            if (RoomItem_0.string_2 == "" || RoomItem_0.string_2 == RoomUser_1.GetClient().GetHabbo().Username)
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_furnistate":
                            if (RoomItem_0.string_3.Length > 0)
                            {
                                string[] collection = RoomItem_0.string_3.Split(new char[]
							{
								','
							});
                                List<string> list = new List<string>(collection);
                                foreach (string current in list)
                                {
                                    if (current == string_10)
                                    {
                                        flag = true;
                                    }
                                }
                            }
                            break;
                        case "wf_trg_onfurni":
                            if (RoomItem_0.string_3.Length > 0)
                            {
                                string[] collection = RoomItem_0.string_3.Split(new char[]
							{
								','
							});
                                List<string> list = new List<string>(collection);
                                List<string> list2 = list;
                                foreach (string current in list)
                                {
                                    if (!(current != string_10))
                                    {
                                        RoomItem @class = this.method_28(Convert.ToUInt32(string_10));
                                        if (@class != null)
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            list2.Remove(current);
                                        }
                                    }
                                }
                                RoomItem_0.string_3 = string.Join(",", list2.ToArray());
                            }
                            break;
                        case "wf_trg_offfurni":
                            if (RoomItem_0.string_3.Length > 0)
                            {
                                string[] collection = RoomItem_0.string_3.Split(new char[]
							{
								','
							});
                                List<string> list = new List<string>(collection);
                                List<string> list2 = list;
                                foreach (string current in list)
                                {
                                    if (!(current != string_10))
                                    {
                                        RoomItem @class = this.method_28(Convert.ToUInt32(string_10));
                                        if (@class != null)
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            list2.Remove(current);
                                        }
                                    }
                                }
                                RoomItem_0.string_3 = string.Join(",", list2.ToArray());
                            }
                            break;
                        case "wf_trg_gameend":
                            if (string_10 == "GameEnded")
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_gamestart":
                            if (string_10 == "GameBegun")
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_timer":
                            if (string_10 == "Timer")
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_attime":
                            if (string_10 == "AtTime")
                            {
                                flag = true;
                            }
                            break;
                        case "wf_trg_atscore":
                            if (string_10 == "TheScore")
                            {
                                flag = true;
                            }
                            break;
                    }
                    try
                    {
                        List<RoomItem> list3 = this.method_93(RoomItem_0.Int32_0, RoomItem_0.Int32_1);
                        if (list3 == null)
                        {
                            result = false;
                            return result;
                        }
                        foreach (RoomItem current2 in list3)
                        {
                            text = current2.GetBaseItem().InteractionType.ToLower();
                            if (text != null)
                            {
                                int num4;
                                if (!(text == "wf_cnd_phx"))
                                {
                                    if (!(text == "wf_cnd_trggrer_on_frn"))
                                    {
                                        string[] collection;
                                        List<string> list;
                                        List<RoomItem> list4;
                                        if (!(text == "wf_cnd_furnis_hv_avtrs"))
                                        {
                                            if (!(text == "wf_cnd_has_furni_on"))
                                            {
                                                if (text == "wf_cnd_time_more_than")
                                                {
                                                    num++;
                                                    current2.ExtraData = "1";
                                                    current2.UpdateState(false, true);
                                                    current2.ReqUpdate(1);
                                                    if (current2.WiredCounter >= double.Parse(current2.string_2, CultureInfo.InvariantCulture))
                                                    {
                                                        num2++;
                                                        break;
                                                    }
                                                }
                                                else if (text == "wf_cnd_time_less_than")
                                                {
                                                    num++;
                                                    current2.ExtraData = "1";
                                                    current2.UpdateState(false, true);
                                                    current2.ReqUpdate(1);
                                                    if (current2.WiredCounter <= double.Parse(current2.string_2, CultureInfo.InvariantCulture))
                                                    {
                                                        num2++;
                                                        break;
                                                    }
                                                }
                                                else
                                                    continue;
                                            }
                                            num4 = num2;
                                            num++;
                                            current2.ExtraData = "1";
                                            current2.UpdateState(false, true);
                                            current2.ReqUpdate(1);
                                            current2.method_10();
                                            if (current2.string_3.Length <= 0)
                                            {
                                                continue;
                                            }
                                            collection = current2.string_3.Split(new char[]
											{
												','
											});
                                            list = new List<string>(collection);
                                            list4 = new List<RoomItem>();
                                            foreach (string current3 in list)
                                            {
                                                list4.Add(this.method_28(Convert.ToUInt32(current3)));
                                            }
                                            using (List<RoomItem>.Enumerator enumerator3 = list4.GetEnumerator())
                                            {
                                                while (enumerator3.MoveNext())
                                                {
                                                    RoomItem current4 = enumerator3.Current;
                                                    if (current4 != null)
                                                    {
                                                        Dictionary<int, AffectedTile> dictionary = current4.Dictionary_0;
                                                        if (dictionary == null)
                                                        {
                                                            dictionary = new Dictionary<int, AffectedTile>();
                                                        }
                                                        RoomItem TopItem = this.GetTopItem(current4.Int32_0, current4.Int32_1);
                                                        if (TopItem != null && TopItem != current4 && TopItem.Double_0 >= current4.Double_0 && num4 + 1 != num2)
                                                        {
                                                            num2++;
                                                            break;
                                                        }
                                                        foreach (AffectedTile current5 in dictionary.Values)
                                                        {
                                                            TopItem = this.GetTopItem(current5.Int32_0, current5.Int32_1);
                                                            if (TopItem != null && TopItem != current4 && TopItem.Double_0 >= current4.Double_0 && num4 + 1 != num2)
                                                            {
                                                                num2++;
                                                                break;
                                                            }
                                                        }

                                                    }
                                                }
                                                continue;
                                            }
                                        }
                                        num++;
                                        current2.ExtraData = "1";
                                        current2.UpdateState(false, true);
                                        current2.ReqUpdate(1);
                                        current2.method_10();
                                        if (current2.string_3.Length <= 0)
                                        {
                                            continue;
                                        }
                                        collection = current2.string_3.Split(new char[]
										{
											','
										});
                                        list = new List<string>(collection);
                                        list4 = new List<RoomItem>();
                                        foreach (string current3 in list)
                                        {
                                            list4.Add(this.method_28(Convert.ToUInt32(current3)));
                                        }
                                        bool flag3 = true;
                                        foreach (RoomItem current4 in list4)
                                        {
                                            if (current4 != null)
                                            {
                                                bool flag4 = false;
                                                Dictionary<int, AffectedTile> dictionary = current4.Dictionary_0;
                                                if (dictionary == null)
                                                {
                                                    dictionary = new Dictionary<int, AffectedTile>();
                                                }
                                                if (this.method_96(current4.Int32_0, current4.Int32_1))
                                                {
                                                    flag4 = true;
                                                }
                                                foreach (AffectedTile current5 in dictionary.Values)
                                                {
                                                    if (this.method_96(current5.Int32_0, current5.Int32_1))
                                                    {
                                                        flag4 = true;
                                                        break;
                                                    }
                                                }
                                                if (!flag4)
                                                {
                                                    flag3 = false;
                                                }
                                            }
                                        }
                                        if (flag3)
                                        {
                                            num2++;
                                            continue;
                                        }
                                        continue;
                                    }
                                    else
                                    {
                                        num4 = num2;
                                        num++;
                                        current2.ExtraData = "1";
                                        current2.UpdateState(false, true);
                                        current2.ReqUpdate(1);
                                        current2.method_10();
                                        if (current2.string_3.Length <= 0)
                                        {
                                            continue;
                                        }
                                        string[] collection = current2.string_3.Split(new char[]
										{
											','
										});
                                        List<string> list = new List<string>(collection);
                                        List<RoomItem> list4 = new List<RoomItem>();
                                        foreach (string current3 in list)
                                        {
                                            list4.Add(this.method_28(Convert.ToUInt32(current3)));
                                        }
                                        if (RoomUser_1 == null)
                                        {
                                            continue;
                                        }
                                        using (List<RoomItem>.Enumerator enumerator3 = list4.GetEnumerator())
                                        {
                                            while (enumerator3.MoveNext())
                                            {
                                                RoomItem current4 = enumerator3.Current;
                                                if (current4 != null)
                                                {
                                                    Dictionary<int, AffectedTile> dictionary = current4.Dictionary_0;
                                                    if (dictionary == null)
                                                    {
                                                        dictionary = new Dictionary<int, AffectedTile>();
                                                    }
                                                    if (RoomUser_1.int_3 == current4.Int32_0 && RoomUser_1.int_4 == current4.Int32_1 && num4 + 1 != num2)
                                                    {
                                                        num2++;
                                                        break;
                                                    }
                                                    foreach (AffectedTile current5 in dictionary.Values)
                                                    {
                                                        if (RoomUser_1.int_3 == current5.Int32_0 && RoomUser_1.int_4 == current5.Int32_1 && num4 + 1 != num2)
                                                        {
                                                            num2++;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            continue;
                                        }
                                    }
                                }
                                num4 = num2;
                                num++;
                                current2.ExtraData = "1";
                                current2.UpdateState(false, true);
                                current2.ReqUpdate(1);
                                if (current2.string_2.Length > 0)
                                {
                                    string string_11 = current2.string_2.Split(new char[]
									{
										':'
									})[0].ToLower();
                                    string string_12 = current2.string_2.Split(new char[]
									{
										':'
									})[1];
                                    if (RoomUser_1 != null)
                                    {
                                        if (!RoomUser_1.IsBot && this.method_18(RoomUser_1, string_11, string_12))
                                        {
                                            num2++;
                                        }
                                    }
                                    else
                                    {
                                        RoomUser[] array = this.RoomUsers;
                                        for (int i = 0; i < array.Length; i++)
                                        {
                                            RoomUser class2 = array[i];
                                            if (class2 != null && !class2.IsBot && this.method_18(class2, string_11, string_12) && num4 + 1 != num2)
                                            {
                                                num2++;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (num != num2)
                        {
                            result = false;
                            return result;
                        }
                    }
                    catch
                    {
                    }
                    if (flag && num == num2)
                    {
                        RoomItem_0.ExtraData = "1";
                        RoomItem_0.UpdateState(false, true);
                        RoomItem_0.ReqUpdate(1);
                        List<RoomItem> list6 = this.method_93(RoomItem_0.Int32_0, RoomItem_0.Int32_1);
                        if (list6 == null)
                        {
                            result = false;
                            return result;
                        }
                        bool flag5 = false;
                        foreach (RoomItem current2 in list6)
                        {
                            if (current2.GetBaseItem().InteractionType.ToLower() == "wf_xtra_random")
                            {
                                flag5 = true;
                                break;
                            }
                        }
                        if (flag5)
                        {
                            List<RoomItem> list7 = new List<RoomItem>();
                            Random random = new Random();
                            while (list6.Count != 0)
                            {
                                int index = random.Next(0, list6.Count);
                                list7.Add(list6[index]);
                                list6.RemoveAt(index);
                            }
                            list6 = list7;
                        }
                        foreach (RoomItem current2 in list6)
                        {
                            if (flag5 && flag2)
                            {
                                break;
                            }
                            text = current2.GetBaseItem().InteractionType.ToLower();
                            switch (text)
                            {
                                case "wf_act_give_phx":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    if (current2.string_2.Length > 0)
                                    {
                                        string string_11 = current2.string_2.Split(new char[]
									{
										':'
									})[0].ToLower();
                                        string string_12 = current2.string_2.Split(new char[]
									{
										':'
									})[1];
                                        if (RoomUser_1 != null)
                                        {
                                            if (!RoomUser_1.IsBot)
                                            {
                                                this.method_19(RoomUser_1, string_11, string_12, current2.uint_0);
                                            }
                                        }
                                        else
                                        {
                                            RoomUser[] array = this.RoomUsers;
                                            for (int i = 0; i < array.Length; i++)
                                            {
                                                RoomUser class2 = array[i];
                                                if (class2 != null && !class2.IsBot)
                                                {
                                                    this.method_19(class2, string_11, string_12, current2.uint_0);
                                                }
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_saymsg":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    if (current2.string_2.Length > 0)
                                    {
                                        string text2 = current2.string_2;
                                        text2 = ChatCommandHandler.smethod_4(text2);
                                        if (text2.Length > 100)
                                        {
                                            text2 = text2.Substring(0, 100);
                                        }
                                        if (RoomUser_1 != null)
                                        {
                                            if (!RoomUser_1.IsBot)
                                            {
                                                RoomUser_1.GetClient().GetHabbo().Whisper(text2);
                                            }
                                        }
                                        else
                                        {
                                            RoomUser[] array = this.RoomUsers;
                                            for (int i = 0; i < array.Length; i++)
                                            {
                                                RoomUser class2 = array[i];
                                                if (class2 != null && !class2.IsBot)
                                                {
                                                    class2.GetClient().GetHabbo().Whisper(text2);
                                                }
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_moveuser":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    current2.method_10();
                                    if (current2.string_3.Length > 0)
                                    {
                                        string[] collection = current2.string_3.Split(new char[]
									{
										','
									});
                                        List<string> list = new List<string>(collection);
                                        Random random2 = new Random();
                                        int num5 = random2.Next(0, list.Count - 1);
                                        RoomItem class3 = this.method_28(Convert.ToUInt32(list[num5]));
                                        if (class3 != null)
                                        {
                                            if (RoomUser_1 != null)
                                            {
                                                this.byte_0[RoomUser_1.int_3, RoomUser_1.int_4] = 1;
                                                this.byte_0[RoomUser_1.int_12, RoomUser_1.int_13] = 1;
                                                this.byte_0[class3.Int32_0, class3.Int32_1] = 1;
                                                RoomUser_1.bool_6 = false;
                                                RoomUser_1.int_12 = class3.Int32_0;
                                                RoomUser_1.int_13 = class3.Int32_1;
                                                RoomUser_1.double_1 = class3.Double_0;
                                                RoomUser_1.method_7(class3.Int32_0, class3.Int32_1, class3.Double_0);
                                                RoomUser_1.UpdateNeeded = true;
                                                if (!current2.dictionary_1.ContainsKey(RoomUser_1))
                                                {
                                                    current2.dictionary_1.Add(RoomUser_1, 10);
                                                }
                                                if (RoomUser_1.class34_1 != null)
                                                {
                                                    RoomUser_1.class34_1.RoomUser_0 = null;
                                                    RoomUser_1.RoomUser_0 = null;
                                                    RoomUser_1.class34_1 = null;
                                                }
                                                this.method_87(RoomUser_1, true, false);
                                            }
                                            else
                                            {
                                                RoomUser[] array = this.RoomUsers;
                                                for (int i = 0; i < array.Length; i++)
                                                {
                                                    RoomUser class2 = array[i];
                                                    if (class2 != null)
                                                    {
                                                        this.byte_0[class2.int_3, class2.int_4] = 1;
                                                        this.byte_0[class3.Int32_0, class3.Int32_1] = 1;
                                                        class2.method_7(class3.Int32_0, class3.Int32_1, class3.Double_0);
                                                        class2.UpdateNeeded = true;
                                                        if (!current2.dictionary_1.ContainsKey(class2))
                                                        {
                                                            current2.dictionary_1.Add(class2, 10);
                                                        }
                                                    }
                                                }
                                            }
                                            flag2 = true;
                                        }
                                    }
                                    break;
                                case "wf_act_togglefurni":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    if (current2.string_3.Length > 0)
                                    {
                                        string[] collection = current2.string_3.Split(new char[]
									{
										','
									});
                                        IEnumerable<string> enumerable = new List<string>(collection);
                                        List<string> list2 = enumerable.ToList<string>();
                                        foreach (string current in enumerable)
                                        {
                                            RoomItem class3 = this.method_28(Convert.ToUInt32(current));
                                            if (class3 != null)
                                            {
                                                class3.Class69_0.OnTrigger(null, class3, 0, true);
                                            }
                                            else
                                            {
                                                list2.Remove(current);
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_kick_user":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    GameClient GetPlayerSession = GoldTree.GetGame().GetClientManager().GetClientByHabbo(RoomUser_1.GetClient().GetHabbo().Username);
                                    if (GetPlayerSession != null)
                                    {
                                        Room class2 = RoomUser_1.GetClient().GetHabbo().CurrentRoom;
                                        if (!(class2.Owner == GetPlayerSession.GetHabbo().Username || GetPlayerSession.GetHabbo().HasFuse("acc_unkickable")))
                                        {
                                            class2.method_47(GetPlayerSession, true, false);
                                            if (current2.string_2.Length > 0)
                                            {
                                                GetPlayerSession.SendNotification(current2.string_2);
                                            }
                                            else
                                            {
                                                GetPlayerSession.SendNotification("Wired: Potki käyttäjä wired-tavara on potkinut sinut huoneesta!");
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_reset_timers":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    foreach (RoomItem current in this.list_14)
                                    {
                                        if (current.GetBaseItem().InteractionType.ToLower() == "wf_trg_attime")
                                        {
                                            current.WiredAtTimeNeedReset = false;
                                            current.WiredAtTimeTimer = 0;
                                            current.ReqUpdate(1);
                                        }
                                        flag2 = true;
                                    }

                                    foreach (RoomItem current in this.list_16)
                                    {
                                        if (current.GetBaseItem().InteractionType.ToLower() == "wf_cnd_time_more_than")
                                        {
                                            current.WiredCounter = 0;
                                            current.WiredNeedReset = false;
                                            current.ReqUpdate(1);
                                        }
                                        else if (current.GetBaseItem().InteractionType.ToLower() == "wf_cnd_time_less_than")
                                        {
                                            current.WiredCounter = 0;
                                            current.WiredNeedReset = false;
                                            current.ReqUpdate(1);
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_givepoints":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    if (RoomUser_1 != null && current2.string_2.Length > 0)
                                    {
                                        int team = 0;
                                        if (RoomUser_1.team == Team.Yellow)
                                        {
                                            team = 12;
                                        }
                                        else if (RoomUser_1.team == Team.Red)
                                        {
                                            team = 3;
                                        }
                                        else if (RoomUser_1.team == Team.Green)
                                        {
                                            team = 6;
                                        }
                                        else if (RoomUser_1.team == Team.Blue)
                                        {
                                            team = 9;
                                        }
                                        this.method_88(team, Convert.ToInt32(current2.string_2), current2);
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_moverotate":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    current2.method_9();
                                    if (current2.string_4.Length > 0)
                                    {
                                        string[] collection = current2.string_4.Split(new char[]
									{
										','
									});
                                        IEnumerable<string> enumerable2 = new List<string>(collection);
                                        foreach (string current in enumerable2)
                                        {
                                            RoomItem class3 = this.method_28(Convert.ToUInt32(current));
                                            if (class3 != null)
                                            {
                                                if (current2.string_2 != "0" && current2.string_2 != "")
                                                {
                                                    ThreeDCoord gstruct1_ = class3.GStruct1_1;
                                                    int num5 = 0;
                                                    int num6 = 0;
                                                    int num7 = 0;
                                                    if (current2.string_2 == "1")
                                                    {
                                                        Random random3 = new Random();
                                                        num5 = random3.Next(1, 5);
                                                    }
                                                    else
                                                    {
                                                        if (current2.string_2 == "2")
                                                        {
                                                            Random random3 = new Random();
                                                            num6 = random3.Next(1, 3);
                                                        }
                                                        else
                                                        {
                                                            if (current2.string_2 == "3")
                                                            {
                                                                Random random3 = new Random();
                                                                num7 = random3.Next(1, 3);
                                                            }
                                                        }
                                                    }
                                                    if (current2.string_2 == "4" || num5 == 1 || num7 == 1)
                                                    {
                                                        gstruct1_ = class3.method_1(4);
                                                    }
                                                    else
                                                    {
                                                        if (current2.string_2 == "5" || num5 == 2 || num6 == 1)
                                                        {
                                                            gstruct1_ = class3.method_1(6);
                                                        }
                                                        else
                                                        {
                                                            if (current2.string_2 == "6" || num5 == 3 || num7 == 2)
                                                            {
                                                                gstruct1_ = class3.method_1(0);
                                                            }
                                                            else
                                                            {
                                                                if (current2.string_2 == "7" || num5 == 4 || num6 == 2)
                                                                {
                                                                    gstruct1_ = class3.method_1(2);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (this.method_37(gstruct1_.x, gstruct1_.y, true, true, true, false, false, true, true) && class3.GetBaseItem().InteractionType != "wf_trg_timer")
                                                    {
                                                        this.method_41(class3, gstruct1_, current2.uint_0, this.GetTopItemHeight(gstruct1_.x, gstruct1_.y));
                                                    }
                                                }
                                                if (current2.string_3.Length > 0 && current2.string_3 != "0" && current2.string_3 != "")
                                                {
                                                    int num5 = 0;
                                                    if (current2.string_3 == "1")
                                                    {
                                                        num5 = class3.int_3 + 2;
                                                        if (num5 > 6)
                                                        {
                                                            num5 = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (current2.string_3 == "2")
                                                        {
                                                            num5 = class3.int_3 - 2;
                                                            if (num5 < 0)
                                                            {
                                                                num5 = 6;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (current2.string_3 == "3")
                                                            {
                                                                Random random3 = new Random();
                                                                num5 = random3.Next(1, 5);
                                                                if (num5 == 1)
                                                                {
                                                                    num5 = 0;
                                                                }
                                                                else
                                                                {
                                                                    if (num5 == 2)
                                                                    {
                                                                        num5 = 2;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (num5 == 3)
                                                                        {
                                                                            num5 = 4;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (num5 == 4)
                                                                            {
                                                                                num5 = 6;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (current2.method_8().method_79(null, class3, class3.Int32_0, class3.Int32_1, num5, false, false, false))
                                                    {
                                                        flag2 = true;
                                                    }
                                                }
                                            }
                                        }
                                        flag2 = true;
                                    }
                                    break;
                                case "wf_act_matchfurni":
                                    current2.ExtraData = "1";
                                    current2.UpdateState(false, true);
                                    current2.ReqUpdate(1);
                                    current2.method_9();
                                    if (current2.string_4.Length > 0 && current2.string_2.Length > 0)
                                    {
                                        string[] collection = current2.string_4.Split(new char[]
									{
										','
									});
                                        IEnumerable<string> enumerable = new List<string>(collection);
                                        string[] collection2 = current2.string_2.Split(new char[]
									{
										';'
									});
                                        List<string> list8 = new List<string>(collection2);
                                        int num8 = 0;
                                        foreach (string current in enumerable)
                                        {
                                            RoomItem class3 = this.method_28(Convert.ToUInt32(current));
                                            if (class3 != null && !(class3.GetBaseItem().InteractionType.ToLower() == "dice") && !(class3.GetBaseItem().InteractionType.ToLower() == "bb_patch"))
                                            {
                                                string[] collection3 = list8[num8].Split(new char[]
											{
												','
											});
                                                List<string> list9 = new List<string>(collection3);
                                                bool flag6 = false;
                                                bool flag7 = false;
                                                if (current2.string_3 != "" && class3 != null)
                                                {
                                                    int int_ = class3.Int32_0;
                                                    int int_2 = class3.Int32_1;
                                                    if (current2.string_3.StartsWith("I"))
                                                    {
                                                        class3.ExtraData = list9[4];
                                                        flag7 = true;
                                                    }
                                                    if (current2.string_3.Substring(1, 1) == "I")
                                                    {
                                                        class3.int_3 = Convert.ToInt32(list9[3]);
                                                        flag6 = true;
                                                    }
                                                    if (current2.string_3.EndsWith("I"))
                                                    {
                                                        int_ = Convert.ToInt32(list9[0]);
                                                        int_2 = Convert.ToInt32(list9[1]);
                                                        flag6 = true;
                                                    }
                                                    if (flag6)
                                                    {
                                                        this.method_40(class3, int_, int_2, current2.uint_0, class3.Double_0);
                                                    }
                                                    if (flag7)
                                                    {
                                                        class3.UpdateState(false, true);
                                                    }
                                                    this.method_22();
                                                }
                                                num8++;
                                            }
                                        }
                                    }
                                    flag2 = true;
                                    break;
                            }
                        }
                    }
                    result = flag2;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        internal void method_22()
        {
            this.gstruct1_0 = new ThreeDCoord[this.RoomModel.int_4, this.RoomModel.int_5];
            this.double_0 = new double[this.RoomModel.int_4, this.RoomModel.int_5];
            this.byte_2 = new byte[this.RoomModel.int_4, this.RoomModel.int_5];
            this.byte_1 = new byte[this.RoomModel.int_4, this.RoomModel.int_5];
            this.HeightOverride = new bool[this.RoomModel.int_4, this.RoomModel.int_5];
            this.byte_0 = new byte[this.RoomModel.int_4, this.RoomModel.int_5];
            this.double_1 = new double[this.RoomModel.int_4, this.RoomModel.int_5];
            this.double_2 = new double[this.RoomModel.int_4, this.RoomModel.int_5];
            for (int i = 0; i < this.RoomModel.int_5; i++)
            {
                for (int j = 0; j < this.RoomModel.int_4; j++)
                {
                    this.double_0[j, i] = 0.0;
                    this.byte_0[j, i] = 0;
                    this.byte_2[j, i] = 0;
                    this.byte_1[j, i] = 0;
                    this.HeightOverride[j, i] = false;
                    this.gstruct1_0[j, i] = new ThreeDCoord(j, i);
                    if (j == this.RoomModel.int_0 && i == this.RoomModel.int_1)
                    {
                        this.byte_0[j, i] = 3;
                    }
                    else
                    {
                        if (this.RoomModel.squareState[j, i] == SquareState.OPEN)
                        {
                            this.byte_0[j, i] = 1;
                        }
                        else
                        {
                            if (this.RoomModel.squareState[j, i] == SquareState.SEAT)
                            {
                                this.byte_0[j, i] = 3;
                            }
                        }
                    }
                }
            }
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                try
                {
                    if (@class.GetBaseItem().Type == 's')
                    {
                        if (@class.Int32_0 >= this.RoomModel.int_4 || @class.Int32_1 >= this.RoomModel.int_5 || @class.Int32_1 < 0 || @class.Int32_0 < 0)
                        {
                            this.method_29(null, @class.uint_0, true, false);
                            GameClient class2 = GoldTree.GetGame().GetClientManager().GetClientByHabbo(this.Owner);
                            if (class2 != null)
                            {
                                class2.GetHabbo().GetInventoryComponent().method_11(@class.uint_0, @class.uint_2, @class.ExtraData, true);
                            }
                        }
                        else
                        {
                            if (@class.Double_1 > this.double_1[@class.Int32_0, @class.Int32_1])
                            {
                                this.double_1[@class.Int32_0, @class.Int32_1] = @class.Double_1;
                            }
                            if (@class.GetBaseItem().IsSeat)
                            {
                                this.double_2[@class.Int32_0, @class.Int32_1] = @class.Double_1;
                            }
                            if (@class.GetBaseItem().HeightOverride)
                            {
                                this.HeightOverride[@class.Int32_0, @class.Int32_1] = true;
                            }
                            if (@class.GetBaseItem().Height > 0.0 || @class.GetBaseItem().EffectF != 0 || @class.GetBaseItem().EffectM != 0 || @class.GetBaseItem().IsSeat || !(@class.GetBaseItem().InteractionType.ToLower() != "bed"))
                            {
                                if (this.double_0[@class.Int32_0, @class.Int32_1] <= @class.Double_0)
                                {
                                    this.double_0[@class.Int32_0, @class.Int32_1] = @class.Double_0;
                                    if (@class.GetBaseItem().EffectF > 0)
                                    {
                                        this.byte_2[@class.Int32_0, @class.Int32_1] = @class.GetBaseItem().EffectF;
                                    }
                                    else
                                    {
                                        if (this.byte_1[@class.Int32_0, @class.Int32_1] != 0)
                                        {
                                            this.byte_2[@class.Int32_0, @class.Int32_1] = 0;
                                        }
                                    }
                                    if (@class.GetBaseItem().EffectM > 0)
                                    {
                                        this.byte_1[@class.Int32_0, @class.Int32_1] = @class.GetBaseItem().EffectM;
                                    }
                                    else
                                    {
                                        if (this.byte_1[@class.Int32_0, @class.Int32_1] != 0)
                                        {
                                            this.byte_1[@class.Int32_0, @class.Int32_1] = 0;
                                        }
                                    }
                                    if (@class.GetBaseItem().Walkable)
                                    {
                                        if (this.byte_0[@class.Int32_0, @class.Int32_1] != 3)
                                        {
                                            this.byte_0[@class.Int32_0, @class.Int32_1] = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (@class.Double_0 <= this.RoomModel.double_1[@class.Int32_0, @class.Int32_1] + 0.1 && @class.GetBaseItem().InteractionType.ToLower() == "gate" && @class.ExtraData == "1")
                                        {
                                            if (this.byte_0[@class.Int32_0, @class.Int32_1] != 3)
                                            {
                                                this.byte_0[@class.Int32_0, @class.Int32_1] = 1;
                                            }
                                        }
                                        else
                                        {
                                            if (@class.GetBaseItem().IsSeat || @class.GetBaseItem().InteractionType.ToLower() == "bed")
                                            {
                                                this.byte_0[@class.Int32_0, @class.Int32_1] = 3;
                                            }
                                            else
                                            {
                                                if (this.byte_0[@class.Int32_0, @class.Int32_1] != 3)
                                                {
                                                    this.byte_0[@class.Int32_0, @class.Int32_1] = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (@class.GetBaseItem().IsSeat || @class.GetBaseItem().InteractionType.ToLower() == "bed")
                                {
                                    this.byte_0[@class.Int32_0, @class.Int32_1] = 3;
                                }
                                Dictionary<int, AffectedTile> dictionary = @class.Dictionary_0;
                                if (dictionary == null)
                                {
                                    dictionary = new Dictionary<int, AffectedTile>();
                                }
                                foreach (AffectedTile current in dictionary.Values)
                                {
                                    if (@class.Double_1 > this.double_1[current.Int32_0, current.Int32_1])
                                    {
                                        this.double_1[current.Int32_0, current.Int32_1] = @class.Double_1;
                                    }
                                    if (@class.GetBaseItem().IsSeat)
                                    {
                                        this.double_2[current.Int32_0, current.Int32_1] = @class.Double_1;
                                    }
                                    if (@class.GetBaseItem().HeightOverride)
                                    {
                                        this.HeightOverride[current.Int32_0, current.Int32_1] = true;
                                    }
                                    if (this.double_0[current.Int32_0, current.Int32_1] <= @class.Double_0)
                                    {
                                        this.double_0[current.Int32_0, current.Int32_1] = @class.Double_0;
                                        if (@class.GetBaseItem().EffectF > 0)
                                        {
                                            this.byte_2[current.Int32_0, current.Int32_1] = @class.GetBaseItem().EffectF;
                                        }
                                        else
                                        {
                                            if (this.byte_1[current.Int32_0, current.Int32_1] != 0)
                                            {
                                                this.byte_2[current.Int32_0, current.Int32_1] = 0;
                                            }
                                        }
                                        if (@class.GetBaseItem().EffectM > 0)
                                        {
                                            this.byte_1[current.Int32_0, current.Int32_1] = @class.GetBaseItem().EffectM;
                                        }
                                        else
                                        {
                                            if (this.byte_1[current.Int32_0, current.Int32_1] != 0)
                                            {
                                                this.byte_1[current.Int32_0, current.Int32_1] = 0;
                                            }
                                            else
                                            {
                                                if (@class.GetBaseItem().Walkable)
                                                {
                                                    if (this.byte_0[current.Int32_0, current.Int32_1] != 3)
                                                    {
                                                        this.byte_0[current.Int32_0, current.Int32_1] = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    if (@class.Double_0 <= this.RoomModel.double_1[@class.Int32_0, @class.Int32_1] + 0.1 && @class.GetBaseItem().InteractionType.ToLower() == "gate" && @class.ExtraData == "1")
                                                    {
                                                        if (this.byte_0[current.Int32_0, current.Int32_1] != 3)
                                                        {
                                                            this.byte_0[current.Int32_0, current.Int32_1] = 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (@class.GetBaseItem().IsSeat || @class.GetBaseItem().InteractionType.ToLower() == "bed")
                                                        {
                                                            this.byte_0[current.Int32_0, current.Int32_1] = 3;
                                                        }
                                                        else
                                                        {
                                                            if (this.byte_0[current.Int32_0, current.Int32_1] != 3)
                                                            {
                                                                this.byte_0[current.Int32_0, current.Int32_1] = 0;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (@class.GetBaseItem().IsSeat || @class.GetBaseItem().InteractionType.ToLower() == "bed")
                                    {
                                        this.byte_0[current.Int32_0, current.Int32_1] = 3;
                                    }
                                    if (@class.GetBaseItem().InteractionType.ToLower() == "bed")
                                    {
                                        this.byte_0[current.Int32_0, current.Int32_1] = 3;
                                        if (@class.int_3 == 0 || @class.int_3 == 4)
                                        {
                                            this.gstruct1_0[current.Int32_0, current.Int32_1].y = @class.Int32_1;
                                        }
                                        if (@class.int_3 == 2 || @class.int_3 == 6)
                                        {
                                            this.gstruct1_0[current.Int32_0, current.Int32_1].x = @class.Int32_0;
                                        }
                                    }
                                }
                            }
                        }

                        if (@class.GetBaseItem().InteractionType.ToLower() == "freeze_ice_block" && (@class.ExtraData == "0" || string.IsNullOrEmpty(@class.ExtraData)))
                        {
                            @class.method_8().method_39(@class.Int32_0, @class.Int32_1);
                        }
                        else if (@class.GetBaseItem().InteractionType.ToLower() == "freeze_ice_block")
                        {
                            @class.method_8().method_38(@class.Int32_0, @class.Int32_1);
                        }
                        else if (@class.GetBaseItem().InteractionType.ToLower() == "freeze_tile")
                        {
                            @class.ExtraData = "";
                        }
                    }
                }
                catch
                {
                    this.method_29(null, @class.uint_0, true, false);
                    GameClient class2 = GoldTree.GetGame().GetClientManager().GetClientByHabbo(this.Owner);
                    if (class2 != null)
                    {
                        class2.GetHabbo().GetInventoryComponent().method_11(@class.uint_0, @class.uint_2, @class.ExtraData, true);
                    }
                }
            }
            if (!this.AllowWalkthrough)
            {
                for (int k = 0; k < this.RoomUsers.Length; k++)
                {
                    RoomUser class3 = this.RoomUsers[k];
                    if (class3 != null)
                    {
                        this.byte_0[class3.int_3, class3.int_4] = 0;
                    }
                }
            }
            this.byte_0[this.RoomModel.int_0, this.RoomModel.int_1] = 3;
        }
        public void method_23()
        {
            this.UsersWithRights = new List<uint>();
            DataTable dataTable = null;
            using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
            {
                dataTable = @class.ReadDataTable("SELECT room_rights.user_id FROM room_rights WHERE room_id = '" + this.Id + "'");
            }
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    this.UsersWithRights.Add((uint)dataRow["user_id"]);
                }
            }
        }
        internal List<RoomItem> method_24(GameClient Session)
        {
            List<RoomItem> list = new List<RoomItem>();
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                @class.Class69_0.OnRemove(Session, @class);
                ServerMessage Message = new ServerMessage(94u);
                Message.AppendRawUInt(@class.uint_0);
                Message.AppendStringWithBreak("");
                Message.AppendBoolean(false);
                this.SendMessage(Message, null);
                list.Add(@class);
            }
            foreach (RoomItem @class in this.Hashtable_1.Values)
            {
                @class.Class69_0.OnRemove(Session, @class);
                ServerMessage Message = new ServerMessage(84u);
                Message.AppendRawUInt(@class.uint_0);
                Message.AppendStringWithBreak("");
                Message.AppendBoolean(false);
                this.SendMessage(Message, null);
                list.Add(@class);
            }
            this.hashtable_4.Clear();
            this.hashtable_0.Clear();
            this.hashtable_1.Clear();
            this.hashtable_2.Clear();
            this.hashtable_3.Clear();
            using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
            {
                class2.ExecuteQuery(string.Concat(new object[]
				{
					"UPDATE items SET room_id = 0, user_id = '",
					Session.GetHabbo().Id,
					"' WHERE room_id = '",
					this.Id,
					"'"
				}));
            }
            this.method_22();
            this.method_83();
            return list;
        }
        public void method_25()
        {
            this.hashtable_0.Clear();
            this.hashtable_4.Clear();
            DataTable dataTable;
            using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
            {
                dataTable = @class.ReadDataTable("SELECT Id, base_item, x, y, z, rot, wall_pos FROM items WHERE room_id = '" + this.Id + "' ORDER BY room_id DESC");
            }
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataRow DataRowExtraData;
                    string ExtraData;
                    using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                    {
                        DataRowExtraData = @class.ReadDataRow("SELECT extra_data FROM items_extra_data WHERE item_id = '" + (uint)dataRow["Id"] + "'");
                        if (DataRowExtraData != null)
                        {
                            ExtraData = (string)DataRowExtraData["extra_data"];
                        }
                        else
                        {
                            ExtraData = "";
                        }
                    }
                    RoomItem class2 = new RoomItem((uint)dataRow["Id"], this.Id, (uint)dataRow["base_item"], ExtraData, (int)dataRow["x"], (int)dataRow["y"], (double)dataRow["z"], (int)dataRow["rot"], (string)dataRow["wall_pos"], this);
                    if (class2.Boolean_0)
                    {
                        this.bool_11 = true;
                    }
                    if (class2.GetBaseItem().InteractionType.ToLower().Contains("wf_") || class2.GetBaseItem().InteractionType.ToLower().Contains("fbgate"))
                    {
                        DataRow dataRow2;
                        using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                        {
                            dataRow2 = @class.ReadDataRow("SELECT extra1,extra2,extra3,extra4,extra5 FROM wired_items WHERE item_id = '" + class2.uint_0 + "'");
                        }
                        if (dataRow2 != null)
                        {
                            class2.string_2 = (string)dataRow2["extra1"];
                            class2.string_3 = (string)dataRow2["extra2"];
                            class2.string_4 = (string)dataRow2["extra3"];
                            class2.string_5 = (string)dataRow2["extra4"];
                            class2.string_6 = (string)dataRow2["extra5"];
                        }
                    }
                    if (class2.GetBaseItem().InteractionType.ToLower() == "firework")
                    {
                        DataRow dataRow2;
                        using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                        {
                            dataRow2 = @class.ReadDataRow("SELECT fw_count FROM items_firework WHERE item_id = '" + class2.uint_0 + "'");

                            if (dataRow2 != null)
                            {
                                class2.FireWorkCount = (int)dataRow2["fw_count"];
                            }
                            else
                            {
                                @class.ExecuteQuery("INSERT INTO items_firework(item_id, fw_count) VALUES ( '" + class2.uint_0 + "', '0')");
                            }
                        }
                    }

                    string text = class2.GetBaseItem().InteractionType.ToLower();
                    switch (text)
                    {
                        case "dice":
                            if (class2.ExtraData == "-1")
                            {
                                class2.ExtraData = "0";
                            }
                            break;
                        case "fbgate":
                            if (class2.ExtraData != "" && class2.ExtraData.Contains(','))
                            {
                                class2.string_2 = class2.ExtraData.Split(new char[]
							{
								','
							})[0];
                                class2.string_3 = class2.ExtraData.Split(new char[]
							{
								','
							})[1];
                            }
                            break;
                        case "dimmer":
                            if (this.MoodlightData == null)
                            {
                                this.MoodlightData = new MoodlightData(class2.uint_0);
                            }
                            break;
                        case "bb_patch":
                            this.list_5.Add(class2);
                            if (class2.ExtraData == "5")
                            {
                                this.list_6.Add(class2);
                            }
                            else
                            {
                                if (class2.ExtraData == "8")
                                {
                                    this.list_7.Add(class2);
                                }
                                else
                                {
                                    if (class2.ExtraData == "11")
                                    {
                                        this.list_9.Add(class2);
                                    }
                                    else
                                    {
                                        if (class2.ExtraData == "14")
                                        {
                                            this.list_8.Add(class2);
                                        }
                                    }
                                }
                            }
                            break;
                        case "blue_score":
                            this.list_12.Add(class2);
                            break;
                        case "green_score":
                            this.list_13.Add(class2);
                            break;
                        case "red_score":
                            this.list_10.Add(class2);
                            break;
                        case "yellow_score":
                            this.list_11.Add(class2);
                            break;
                        case "stickiepole":
                            this.list_3.Add(class2);
                            break;
                        case "wf_trg_onsay":
                        case "wf_trg_enterroom":
                        case "wf_trg_furnistate":
                        case "wf_trg_onfurni":
                        case "wf_trg_offfurni":
                        case "wf_trg_gameend":
                        case "wf_trg_gamestart":
                            if (!this.list_14.Contains(class2))
                            {
                                this.list_14.Add(class2);
                            }
                            break;
                        case "wf_trg_attime":
                            if (class2.string_2.Length <= 0)
                            {
                                class2.string_2 = "10";
                            }
                            if (!this.list_14.Contains(class2))
                            {
                                this.list_14.Add(class2);
                            }
                            class2.bool_0 = true;
                            class2.ReqUpdate(1);
                            break;
                        case "wf_trg_atscore":
                            if (!this.list_14.Contains(class2))
                            {
                                this.list_14.Add(class2);
                            }
                            class2.WiredAtTimeNeedReset = false;
                            class2.ReqUpdate(1);
                            break;
                        case "wf_trg_timer":
                            if (class2.string_2.Length <= 0)
                            {
                                class2.string_2 = "10";
                            }
                            if (!this.list_14.Contains(class2))
                            {
                                this.list_14.Add(class2);
                            }
                            class2.bool_0 = true;
                            class2.ReqUpdate(1);
                            break;
                        case "wf_act_saymsg":
                        case "wf_act_kick_user":
                        case "wf_act_moveuser":
                        case "wf_act_togglefurni":
                        case "wf_act_givepoints":
                        case "wf_act_moverotate":
                        case "wf_act_matchfurni":
                        case "wf_act_give_phx":
                            if (!this.list_15.Contains(class2))
                            {
                                this.list_15.Add(class2);
                            }
                            break;
                        case "wf_cnd_trggrer_on_frn":
                        case "wf_cnd_furnis_hv_avtrs":
                        case "wf_cnd_has_furni_on":
                        case "wf_cnd_match_snapshot":
                        case "wf_cnd_time_more_than":
                        case "wf_cnd_time_less_than":
                        case "wf_cnd_phx":
                            if (!this.list_16.Contains(class2))
                            {
                                this.list_16.Add(class2);
                            }
                            break;
                        case "jukebox":
                            RoomMusicController roomMusicController = this.GetRoomMusicController();
                            roomMusicController.LinkRoomOutputItemIfNotAlreadyExits(class2);
                            break;
                        case "freeze_tile":
                            this.GetFreeze().AddFreezeTile(class2);
                            break;
                        case "freeze_ice_block":
                            this.GetFreeze().AddFreezeBlock(class2);
                            break;
                        case "freeze_exit":
                            RoomItem exitTeleport = this.GetFreeze().ExitTeleport;
                            if (exitTeleport == null)
                            {
                                this.GetFreeze().ExitTeleport = class2;
                            }
                            break;
                        case "freeze_blue_gate":
                            this.GetGameManager().AddFreezeGate(class2);
                            break;
                        case "freeze_red_gate":
                            this.GetGameManager().AddFreezeGate(class2);
                            break;
                        case "freeze_green_gate":
                            this.GetGameManager().AddFreezeGate(class2);
                            break;
                        case "freeze_yellow_gate":
                            this.GetGameManager().AddFreezeGate(class2);
                            break;
                        case "freeze_blue_score":
                            this.GetGameManager().AddFreezeScoreboard(class2);
                            break;
                        case "freeze_red_score":
                            this.GetGameManager().AddFreezeScoreboard(class2);
                            break;
                        case "freeze_green_score":
                            this.GetGameManager().AddFreezeScoreboard(class2);
                            break;
                        case "freeze_yellow_score":
                            this.GetGameManager().AddFreezeScoreboard(class2);
                            break;
                    }
                    if (this.hashtable_0.Contains(class2.uint_0))
                    {
                        this.hashtable_0.Remove(class2.uint_0);
                    }
                    if (this.hashtable_4.Contains(class2.uint_0))
                    {
                        this.hashtable_4.Remove(class2.uint_0);
                    }
                    if (class2.Boolean_2)
                    {
                        this.hashtable_0.Add(class2.uint_0, class2);
                    }
                    else
                    {
                        this.hashtable_4.Add(class2.uint_0, class2);
                    }
                }
            }
        }
        public bool method_26(GameClient Session)
        {
            return this.CheckRights(Session, false);
        }
        public bool CheckRights(GameClient Session, bool bool_13)
        {
            bool result;
            try
            {
                if (Session.GetHabbo().Username.ToLower() == this.Owner.ToLower())
                {
                    result = true;
                    return result;
                }
                if (Session.GetHabbo().HasFuse("acc_anyroomowner") && bool_13)
                {
                    result = true;
                    return result;
                }
                if (!bool_13)
                {
                    if (Session.GetHabbo().HasFuse("acc_anyroomrights"))
                    {
                        result = true;
                        return result;
                    }
                    if (this.UsersWithRights.Contains(Session.GetHabbo().Id))
                    {
                        result = true;
                        return result;
                    }
                    if (this.bool_8)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            catch
            {
            }
            result = false;
            return result;
        }
        public RoomItem method_28(uint uint_2)
        {
            RoomItem result;
            if ((this.hashtable_0 != null && this.hashtable_0.ContainsKey(uint_2)) || (this.hashtable_4 != null && this.hashtable_4.ContainsKey(uint_2)))
            {
                RoomItem @class = this.hashtable_0[uint_2] as RoomItem;
                if (@class != null)
                {
                    result = @class;
                }
                else
                {
                    result = (this.hashtable_4[uint_2] as RoomItem);
                }
            }
            else
            {
                result = null;
            }
            return result;
        }
        public void method_29(GameClient Session, uint uint_2, bool bool_13, bool bool_14)
        {
            RoomItem @class = this.method_28(uint_2);
            if (@class != null)
            {
                Dictionary<int, AffectedTile> dictionary = this.method_94(@class.GetBaseItem().Length, @class.GetBaseItem().Width, @class.Int32_0, @class.Int32_1, @class.int_3);
                @class.Class69_0.OnRemove(Session, @class);
                if (@class.Boolean_1)
                {
                    ServerMessage Message = new ServerMessage(84u);
                    Message.AppendRawUInt(@class.uint_0);
                    Message.AppendStringWithBreak("");
                    Message.AppendBoolean(false);
                    this.SendMessage(Message, null);
                }
                else
                {
                    if (@class.Boolean_2)
                    {
                        ServerMessage Message = new ServerMessage(94u);
                        Message.AppendRawUInt(@class.uint_0);
                        Message.AppendStringWithBreak("");
                        Message.AppendBoolean(false);
                        this.SendMessage(Message, null);
                        string text = @class.GetBaseItem().InteractionType.ToLower();
                        switch (text)
                        {
                            case "bb_patch":
                                this.list_5.Remove(@class);
                                if (@class.ExtraData == "5")
                                {
                                    this.list_6.Remove(@class);
                                }
                                else
                                {
                                    if (@class.ExtraData == "8")
                                    {
                                        this.list_7.Remove(@class);
                                    }
                                    else
                                    {
                                        if (@class.ExtraData == "11")
                                        {
                                            this.list_9.Remove(@class);
                                        }
                                        else
                                        {
                                            if (@class.ExtraData == "14")
                                            {
                                                this.list_8.Remove(@class);
                                            }
                                        }
                                    }
                                }
                                break;
                            case "blue_score":
                                this.list_12.Remove(@class);
                                break;
                            case "green_score":
                                this.list_13.Remove(@class);
                                break;
                            case "red_score":
                                this.list_10.Remove(@class);
                                break;
                            case "yellow_score":
                                this.list_11.Remove(@class);
                                break;
                            case "stickiepole":
                                this.list_3.Remove(@class);
                                break;
                            case "wf_trg_onsay":
                            case "wf_trg_enterroom":
                            case "wf_trg_furnistate":
                            case "wf_trg_onfurni":
                            case "wf_trg_offfurni":
                            case "wf_trg_gameend":
                            case "wf_trg_gamestart":
                            case "wf_trg_atscore":
                                this.list_14.Remove(@class);
                                break;
                            case "wf_trg_attime":
                                @class.bool_0 = false;
                                this.list_14.Remove(@class);
                                break;
                            case "wf_trg_timer":
                                @class.bool_0 = false;
                                this.list_14.Remove(@class);
                                break;
                            case "wf_act_saymsg":
                            case "wf_act_kick_user":
                            case "wf_act_moveuser":
                            case "wf_act_togglefurni":
                            case "wf_act_givepoints":
                            case "wf_act_moverotate":
                            case "wf_act_matchfurni":
                            case "wf_act_give_phx":
                                this.list_15.Remove(@class);
                                break;
                            case "wf_cnd_trggrer_on_frn":
                            case "wf_cnd_furnis_hv_avtrs":
                            case "wf_cnd_has_furni_on":
                            case "wf_cnd_match_snapshot":
                            case "wf_cnd_time_more_than":
                            case "wf_cnd_time_less_than":
                            case "wf_cnd_phx":
                                this.list_16.Remove(@class);
                                break;
                            case "freeze_tile":
                                this.GetFreeze().RemoveFreezeTile(@class);
                                break;
                            case "freeze_ice_block":
                                this.GetFreeze().RemoveFreezeBlock(@class);
                                break;
                            case "freeze_exit":
                                RoomItem exitTeleport = this.GetFreeze().ExitTeleport;
                                if ((exitTeleport != null) && (@class.uint_0 == exitTeleport.uint_0))
                                {
                                    this.GetFreeze().ExitTeleport = null;
                                }
                                break;
                            case "freeze_blue_gate":
                                this.GetGameManager().RemoveFreezeGate(@class);
                                break;
                            case "freeze_red_gate":
                                this.GetGameManager().RemoveFreezeGate(@class);
                                break;
                            case "freeze_green_gate":
                                this.GetGameManager().RemoveFreezeGate(@class);
                                break;
                            case "freeze_yellow_gate":
                                this.GetGameManager().RemoveFreezeGate(@class);
                                break;
                            case "freeze_blue_score":
                                this.GetGameManager().RemoveFreezeScoreboard(@class);
                                break;
                            case "freeze_red_score":
                                this.GetGameManager().RemoveFreezeScoreboard(@class);
                                break;
                            case "freeze_green_score":
                                this.GetGameManager().RemoveFreezeScoreboard(@class);
                                break;
                            case "freeze_yellow_score":
                                this.GetGameManager().RemoveFreezeScoreboard(@class);
                                break;
                        }
                    }
                }
                if (@class.Boolean_1)
                {
                    this.hashtable_4.Remove(@class.uint_0);
                }
                else
                {
                    this.hashtable_0.Remove(@class.uint_0);
                }
                if (this.hashtable_3.Contains(@class.uint_0))
                {
                    this.hashtable_3.Remove(@class.uint_0);
                }
                if (this.hashtable_2.Contains(@class.uint_0))
                {
                    this.hashtable_2.Remove(@class.uint_0);
                }
                if (!this.hashtable_1.Contains(@class.uint_0))
                {
                    this.hashtable_1.Add(@class.uint_0, @class);
                }
                if (bool_13)
                {
                    using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
                    {
                        class2.ExecuteQuery("DELETE FROM items WHERE Id = '" + uint_2 + "' LIMIT 1");
                    }
                }
                if (bool_14)
                {
                    this.method_22();
                }
                this.method_87(this.method_43(@class.Int32_0, @class.Int32_1), true, true);
                foreach (AffectedTile current in dictionary.Values)
                {
                    this.method_87(this.method_43(current.Int32_0, current.Int32_1), true, true);
                }
            }
        }
        public bool method_30(int int_17, int int_18, double double_3, bool bool_13, bool bool_14)
        {
            return this.AllowWalkthrough || bool_14 || this.method_43(int_17, int_18) == null;
        }
        private void method_31(string string_10)
        {
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && !@class.IsBot)
                {
                    @class.GetClient().SendNotification(string_10);
                }
            }
        }

        internal void method_32(object object_0)
        {
            this.method_33();
        }
        private void method_33()
        {
            isCycling = true;
            int num = 0;
            if (!this.bool_6 && !this.bool_7)
            {
                try
                {
                    this.int_14++;
                    if (this.bool_10 && this.int_14 >= 30)
                    {
                        using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                        {
                            @class.ExecuteQuery(string.Concat(new object[]
							{
								"UPDATE rooms SET users_now = '",
								this.UserCount,
								"' WHERE Id = '",
								this.Id,
								"' LIMIT 1"
							}));
                        }
                        this.int_14 = 0;
                    }
                    this.method_35();
                    int num2 = 0;
                    try
                    {
                        if (this.hashtable_0 != null)
                        {
                            foreach (RoomItem class2 in this.Hashtable_0.Values)
                            {
                                if (class2.bool_1)
                                {
                                    class2.method_2();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.Id + "] cycle task -- Process Floor Items");
                        this.method_34();
                    }
                    try
                    {
                        if (this.hashtable_4 != null)
                        {
                            foreach (RoomItem class2 in this.Hashtable_1.Values)
                            {
                                if (class2.bool_1)
                                {
                                    class2.method_2();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.Id + "] cycle task -- Process Wall Items");
                        this.method_34();
                    }
                    List<uint> list = new List<uint>();
                    int num3 = 0;
                    if (this.RoomUsers != null)
                    {
                        try
                        {
                            for (int i = 0; i < this.RoomUsers.Length; i++)
                            {
                                RoomUser class3 = this.RoomUsers[i];
                                if (class3 != null)
                                {
                                    num = 1;
                                    if (!class3.IsBot && class3.GetClient() != null)
                                    {
                                        num3++;
                                        if (class3.GetClient().GetHabbo() != null && class3.GetClient().GetHabbo().int_4 > 0)
                                        {
                                            class3.GetClient().GetHabbo().int_4--;
                                            if (class3.GetClient().GetHabbo().int_4 == 0)
                                            {
                                                class3.GetClient().GetHabbo().IsMuted = false;
                                            }
                                        }
                                    }
                                    if (this.MusicController != null)
                                    {
                                        this.MusicController.Update(this);
                                    }
                                    class3.int_1++;
                                    this.GetFreeze().CycleUser(class3);
                                    num = 2;
                                    if (!class3.bool_8 && class3.int_1 >= ServerConfiguration.SleepTimer)
                                    {
                                        class3.bool_8 = true;
                                        ServerMessage Message = new ServerMessage(486u);
                                        Message.AppendInt32(class3.VirtualId);
                                        Message.AppendBoolean(true);
                                        this.SendMessage(Message, null);
                                    }
                                    num = 3;
                                    if (class3.GetClient() == null && !class3.IsBot)
                                    {
                                        this.RoomUsers[i] = null;
                                        if (!class3.bool_1)
                                        {
                                            this.byte_0[class3.int_3, class3.int_4] = class3.byte_0;
                                        }
                                        ServerMessage Message2 = new ServerMessage(29u);
                                        Message2.AppendRawInt32(class3.VirtualId);
                                        this.SendMessage(Message2, null);
                                        this.method_50();
                                    }
                                    num = 4;
                                    if (class3.Boolean_2 && !list.Contains(class3.UId))
                                    {
                                        list.Add(class3.UId);
                                    }
                                    num = 5;
                                    if (class3.CarryItemID > 0)
                                    {
                                        class3.int_6--;
                                        if (class3.int_6 <= 0)
                                        {
                                            class3.CarryItem(0);
                                        }
                                    }
                                    num = 6;
                                    if (class3.bool_4 && class3.class34_1 == null)
                                    {
                                        num = 7;
                                        if (class3.IsBot && class3.RoomBot.RoomUser_0 != null && this.method_30(class3.int_12, class3.int_13, 0.0, true, true))
                                        {
                                            num = 8;
                                            this.method_85(class3);
                                            class3.int_3 = class3.int_12;
                                            class3.int_4 = class3.int_13;
                                            class3.double_0 = class3.double_1;
                                            class3.RoomBot.RoomUser_0.int_3 = class3.int_12;
                                            class3.RoomBot.RoomUser_0.int_4 = class3.int_13;
                                            class3.RoomBot.RoomUser_0.double_0 = class3.double_1 + 1.0;
                                            class3.RoomBot.RoomUser_0.bool_4 = false;
                                            class3.RoomBot.RoomUser_0.RemoveStatus("mv");
                                            if (class3.int_3 == this.RoomModel.int_0 && class3.int_4 == this.RoomModel.int_1 && !list.Contains(class3.RoomBot.RoomUser_0.UId))
                                            {
                                                list.Add(class3.RoomBot.RoomUser_0.UId);
                                            }
                                            this.method_87(class3, true, true);
                                        }
                                        else
                                        {
                                            if (this.method_30(class3.int_12, class3.int_13, 0.0, true, class3.bool_1))
                                            {
                                                num = 8;
                                                this.method_85(class3);
                                                class3.int_3 = class3.int_12;
                                                class3.int_4 = class3.int_13;
                                                class3.double_0 = class3.double_1;
                                                if (class3.int_3 == this.RoomModel.int_0 && class3.int_4 == this.RoomModel.int_1 && !list.Contains(class3.UId) && !class3.IsBot)
                                                {
                                                    list.Add(class3.UId);
                                                }
                                                this.method_87(class3, true, true);
                                            }
                                        }
                                        class3.bool_4 = false;
                                    }
                                    num = 9;
                                    if (class3.bool_6 && !class3.bool_5 && class3.class34_1 == null)
                                    {
                                        num = 10;
                                        SquarePoint @struct = DreamPathfinder.GetNextStep(class3.int_3, class3.int_4, class3.int_10, class3.int_11, this.byte_0, this.double_1, this.class28_0.double_1, this.double_2, this.class28_0.int_4, this.class28_0.int_5, class3.bool_1, this.bool_5, this.HeightOverride);
                                        num = 11;
                                        if (@struct.X != class3.int_3 || @struct.Y != class3.int_4)
                                        {
                                            num = 12;
                                            int int32_ = @struct.X;
                                            int int32_2 = @struct.Y;
                                            class3.RemoveStatus("mv");
                                            double num4 = this.method_84(int32_, int32_2, this.method_93(int32_, int32_2));
                                            class3.Statusses.Remove("lay");
                                            class3.Statusses.Remove("sit");
                                            class3.AddStatus("mv", string.Concat(new object[]
											{
												int32_,
												",",
												int32_2,
												",",
												num4.ToString().Replace(',', '.')
											}));
                                            num = 13;
                                            if (class3.IsBot && class3.RoomBot.RoomUser_0 != null)
                                            {
                                                class3.RoomBot.RoomUser_0.AddStatus("mv", string.Concat(new object[]
												{
													int32_,
													",",
													int32_2,
													",",
													(num4 + 1.0).ToString().Replace(',', '.')
												}));
                                            }
                                            int num5;
                                            if (class3.bool_3)
                                            {
                                                num5 = Class107.smethod_1(class3.int_3, class3.int_4, int32_, int32_2);
                                            }
                                            else
                                            {
                                                num5 = Class107.smethod_0(class3.int_3, class3.int_4, int32_, int32_2);
                                            }
                                            class3.int_8 = num5;
                                            class3.int_7 = num5;
                                            class3.bool_4 = true;
                                            class3.int_12 = int32_;
                                            class3.int_13 = int32_2;
                                            class3.double_1 = num4;
                                            num = 14;
                                            if (class3.IsBot && class3.RoomBot.RoomUser_0 != null)
                                            {
                                                class3.RoomBot.RoomUser_0.int_8 = num5;
                                                class3.RoomBot.RoomUser_0.int_7 = num5;
                                                class3.RoomBot.RoomUser_0.bool_4 = true;
                                                class3.RoomBot.RoomUser_0.int_12 = int32_;
                                                class3.RoomBot.RoomUser_0.int_13 = int32_2;
                                                class3.RoomBot.RoomUser_0.double_1 = num4 + 1.0;
                                            }
                                            try
                                            {
                                                num = 15;
                                                if (!class3.IsBot)
                                                {
                                                    if (class3.GetClient().GetHabbo().Gender.ToLower() == "m" && this.byte_1[int32_, int32_2] > 0 && class3.byte_1 != this.byte_1[int32_, int32_2])
                                                    {
                                                        class3.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2((int)this.byte_1[int32_, int32_2], true);
                                                        class3.byte_1 = this.byte_1[int32_, int32_2];
                                                    }
                                                    else
                                                    {
                                                        if (class3.GetClient().GetHabbo().Gender.ToLower() == "f" && this.byte_2[int32_, int32_2] > 0 && class3.byte_1 != this.byte_2[int32_, int32_2])
                                                        {
                                                            class3.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2((int)this.byte_2[int32_, int32_2], true);
                                                            class3.byte_1 = this.byte_2[int32_, int32_2];
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!class3.IsPet)
                                                    {
                                                        if (this.byte_1[int32_, int32_2] > 0)
                                                        {
                                                            class3.RoomBot.EffectId = (int)this.byte_1[int32_, int32_2];
                                                            class3.byte_1 = this.byte_1[int32_, int32_2];
                                                        }
                                                        ServerMessage Message3 = new ServerMessage(485u);
                                                        Message3.AppendInt32(class3.VirtualId);
                                                        Message3.AppendInt32(class3.RoomBot.EffectId);
                                                        this.SendMessage(Message3, null);
                                                    }
                                                }
                                                goto IL_CE1;
                                            }
                                            catch
                                            {
                                                goto IL_CE1;
                                            }
                                        IL_B8B:
                                            this.method_87(class3, false, true);
                                            class3.UpdateNeeded = true;
                                            if (class3.IsBot && class3.RoomBot.RoomUser_0 != null)
                                            {
                                                this.method_87(class3.RoomBot.RoomUser_0, true, true);
                                                class3.RoomBot.RoomUser_0.UpdateNeeded = true;
                                                goto IL_BE0;
                                            }
                                            goto IL_BE0;
                                        IL_CE1:
                                            num = 16;
                                            this.byte_0[class3.int_3, class3.int_4] = class3.byte_0;
                                            class3.byte_0 = this.byte_0[class3.int_12, class3.int_13];
                                            if (this.AllowWalkthrough)
                                            {
                                                goto IL_B8B;
                                            }
                                            this.byte_0[int32_, int32_2] = 0;
                                            goto IL_B8B;
                                        }
                                        num = 12;
                                        class3.bool_6 = false;
                                        class3.RemoveStatus("mv");
                                        class3.bool_10 = false;
                                        if (class3.IsBot && class3.RoomBot.RoomUser_0 != null)
                                        {
                                            class3.RoomBot.RoomUser_0.RemoveStatus("mv");
                                            class3.RoomBot.RoomUser_0.bool_6 = false;
                                            class3.RoomBot.RoomUser_0.bool_10 = false;
                                            class3.RoomBot.RoomUser_0.UpdateNeeded = true;
                                        }
                                    IL_BE0:
                                        class3.UpdateNeeded = true;
                                    }
                                    else
                                    {
                                        num = 17;
                                        if (class3.Statusses.ContainsKey("mv") && class3.class34_1 == null)
                                        {
                                            num = 18;
                                            class3.RemoveStatus("mv");
                                            class3.UpdateNeeded = true;
                                            if (class3.IsBot && class3.RoomBot.RoomUser_0 != null)
                                            {
                                                class3.RoomBot.RoomUser_0.RemoveStatus("mv");
                                                class3.RoomBot.RoomUser_0.UpdateNeeded = true;
                                            }
                                        }
                                    }
                                    if (class3.IsBot || class3.IsPet)
                                    {
                                        try
                                        {
                                            class3.BotAI.OnTimerTick();
                                            goto IL_C9F;
                                        }
                                        catch
                                        {
                                            goto IL_C9F;
                                        }
                                    }
                                    goto IL_C9B;
                                IL_C9F:
                                    if (class3.int_9 > 0)
                                    {
                                        if (class3.int_9 == 1)
                                        {
                                            this.method_87(class3, true, true);
                                        }
                                        class3.int_9--;
                                        goto IL_CD6;
                                    }
                                    goto IL_CD6;
                                IL_C9B:
                                    num2++;
                                    goto IL_C9F;
                                }
                            IL_CD6: ;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.LogThreadException(ex.ToString(), string.Concat(new object[]
							{
								"Room [ID: ",
								this.Id,
								"] [Part: ",
								num,
								" cycle task -- Process Users Updates"
							}));
                            this.method_34();
                        }
                    }
                    try
                    {
                        foreach (uint current in list)
                        {
                            this.method_47(GoldTree.GetGame().GetClientManager().method_2(current), true, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.Id + "] cycle task -- Remove Users");
                        this.method_34();
                    }
                    if (num2 >= 1)
                    {
                        this.int_8 = 0;
                    }
                    else
                    {
                        this.int_8++;
                    }
                    if (!this.bool_6 && !this.bool_7)
                    {
                        try
                        {
                            if (this.int_8 >= 60)
                            {
                                GoldTree.GetGame().GetRoomManager().method_16(this);
                                return;
                            }

                            ServerMessage Logging = this.method_67(false);
                            if (Logging != null)
                            {
                                this.SendMessage(Logging, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.Id + "] cycle task -- Cycle End");
                            this.method_34();
                        }
                    }
                    this.class27_0.UsersNow = num3;
                }
                catch (Exception ex)
                {
                    Logging.LogThreadException(ex.ToString(), "Room [ID: " + this.Id + "] cycle task");
                }
            }
            isCycling = false;
        }
        internal void method_34()
        {
            if (!this.bool_7 && ServerConfiguration.UnloadCrashedRooms)
            {
                this.bool_7 = true;
                try
                {
                    this.method_31(GoldTreeEnvironment.GetExternalText("error_roomunload"));
                }
                catch
                {
                }
                GoldTree.GetGame().GetRoomManager().method_16(this);
            }
        }
        private void method_35()
        {
            if (this.bool_11)
            {
                if (this.int_16 >= this.int_15 || this.int_15 == 0)
                {
                    Hashtable hashtable = this.hashtable_0.Clone() as Hashtable;
                    List<uint> list = new List<uint>();
                    List<uint> list2 = new List<uint>();
                    foreach (RoomItem @class in hashtable.Values)
                    {
                        if (@class.Boolean_0)
                        {
                            ThreeDCoord gStruct1_ = @class.GStruct1_1;
                            if (gStruct1_.x >= this.RoomModel.int_4 || gStruct1_.y >= this.RoomModel.int_5 || gStruct1_.x < 0 || gStruct1_.y < 0)
                            {
                                return;
                            }
                            List<RoomItem> list3 = this.method_45(@class.Int32_0, @class.Int32_1);
                            RoomUser class2 = this.method_43(@class.Int32_0, @class.Int32_1);
                            if (list3.Count > 0 || class2 != null)
                            {
                                List<RoomItem> list4 = this.method_45(gStruct1_.x, gStruct1_.y);
                                double num = this.RoomModel.double_1[gStruct1_.x, gStruct1_.y];
                                int num2 = 0;
                                int num3 = 0;
                                bool flag = false;
                                foreach (RoomItem current in list4)
                                {
                                    if (current.Double_1 > num)
                                    {
                                        num = current.Double_1;
                                    }
                                    if (!current.Boolean_0)
                                    {
                                        num2++;
                                    }
                                    else
                                    {
                                        num3++;
                                    }
                                    if (!flag && current.GetBaseItem().InteractionType.ToLower() == "wf_trg_timer")
                                    {
                                        flag = true;
                                    }
                                }
                                bool flag2 = num2 > 0;
                                if (this.method_43(gStruct1_.x, gStruct1_.y) != null)
                                {
                                    flag2 = true;
                                }
                                bool flag3 = num3 > 0;
                                foreach (RoomItem current in list3)
                                {
                                    bool flag4 = current.GetBaseItem().InteractionType.ToLower() == "wf_trg_timer";
                                    if (!current.Boolean_0 && !list.Contains(current.uint_0) && this.method_36(gStruct1_.x, gStruct1_.y) && (!flag2 || !flag3) && @class.Double_0 < current.Double_0 && this.method_43(gStruct1_.x, gStruct1_.y) == null && (!flag4 || !flag))
                                    {
                                        double double_;
                                        if (flag3)
                                        {
                                            double_ = current.Double_0;
                                        }
                                        else
                                        {
                                            double_ = current.Double_0 - @class.Double_1 + this.RoomModel.double_1[gStruct1_.x, gStruct1_.y];
                                        }
                                        this.method_41(current, gStruct1_, @class.uint_0, double_);
                                        list.Add(current.uint_0);
                                    }
                                }
                                if (class2 != null && (!flag2 || !flag3) && this.method_37(gStruct1_.x, gStruct1_.y, false, true, false, true, true, false, false) && !list2.Contains(class2.UId) && !class2.bool_6)
                                {
                                    if (this.double_2[gStruct1_.x, gStruct1_.y] > 0.0)
                                    {
                                        num = this.method_84(gStruct1_.x, gStruct1_.y, this.method_93(gStruct1_.x, gStruct1_.y));
                                    }
                                    if (class2.IsBot && class2.RoomBot.RoomUser_0 != null)
                                    {
                                        this.method_42(class2, gStruct1_, @class.uint_0, num);
                                        list2.Add(class2.UId);
                                        this.method_42(class2.RoomBot.RoomUser_0, gStruct1_, @class.uint_0, num + 1.0);
                                        list2.Add(class2.RoomBot.RoomUser_0.UId);
                                    }
                                    else
                                    {
                                        if (class2.class34_1 == null)
                                        {
                                            this.method_42(class2, gStruct1_, @class.uint_0, num);
                                            list2.Add(class2.UId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    hashtable.Clear();
                    hashtable = null;
                    list.Clear();
                    list2.Clear();
                    this.int_16 = 0;
                }
                else
                {
                    this.int_16++;
                }
            }
        }
        public bool method_36(int int_17, int int_18)
        {
            bool result;
            if (!this.method_92(int_17, int_18))
            {
                result = false;
            }
            else
            {
                if (this.RoomModel.squareState[int_17, int_18] == SquareState.BLOCKED)
                {
                    result = false;
                }
                else
                {
                    List<RoomItem> list = this.method_93(int_17, int_18);
                    if (list != null && list.Count > 1)
                    {
                        foreach (RoomItem current in list)
                        {
                            if (current.Boolean_0)
                            {
                                result = true;
                                return result;
                            }
                        }
                    }
                    result = true;
                }
            }
            return result;
        }
        public bool method_37(int int_17, int int_18, bool bool_13, bool bool_14, bool bool_15, bool bool_16, bool bool_17, bool IsNotSeat, bool IsNotBed)
        {
            bool result;
            if (!this.method_92(int_17, int_18))
            {
                result = false;
            }
            else
            {
                if (this.RoomModel.squareState[int_17, int_18] == SquareState.BLOCKED)
                {
                    result = false;
                }
                else
                {
                    if (bool_17 && this.double_2[int_17, int_18] > 0.0)
                    {
                        result = true;
                    }
                    else
                    {
                        if (bool_13 && this.method_97(int_17, int_18))
                        {
                            result = false;
                        }
                        else
                        {
                            if (bool_14)
                            {
                                List<RoomItem> list = this.method_93(int_17, int_18);
                                if (list != null && list.Count > 0)
                                {
                                    if (!bool_15 && !bool_16 && !bool_17 && !IsNotSeat)
                                    {
                                        result = false;
                                        return result;
                                    }
                                    if (bool_15)
                                    {
                                        foreach (RoomItem current in list)
                                        {
                                            if (!current.GetBaseItem().Stackable)
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }
                                    if (bool_16 && bool_17)
                                    {
                                        using (List<RoomItem>.Enumerator enumerator = list.GetEnumerator())
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                RoomItem current = enumerator.Current;
                                                if (!current.GetBaseItem().Walkable && !current.GetBaseItem().IsSeat)
                                                {
                                                    result = false;
                                                    return result;
                                                }
                                            }
                                            goto IL_1DD;
                                        }
                                    }
                                    if (bool_16)
                                    {
                                        using (List<RoomItem>.Enumerator enumerator = list.GetEnumerator())
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                RoomItem current = enumerator.Current;
                                                if (!current.GetBaseItem().Walkable)
                                                {
                                                    result = false;
                                                    return result;
                                                }
                                            }
                                            goto IL_1DD;
                                        }
                                    }
                                    if (bool_17)
                                    {
                                        foreach (RoomItem current in list)
                                        {
                                            if (!current.GetBaseItem().IsSeat)
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }

                                    if (IsNotSeat)
                                    {
                                        foreach (RoomItem current in list)
                                        {
                                            if (current.GetBaseItem().IsSeat)
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }

                                    if (IsNotBed)
                                    {
                                        foreach (RoomItem current in list)
                                        {
                                            if (current.GetBaseItem().InteractionType.ToLower() == "bed")
                                            {
                                                result = false;
                                                return result;
                                            }
                                        }
                                    }
                                }
                            }
                        IL_1DD:
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        internal void method_38(int int_17, int int_18)
        {
            this.byte_0[int_17, int_18] = 1;
        }
        internal void method_39(int int_17, int int_18)
        {
            this.byte_0[int_17, int_18] = 0;
        }
        internal void method_40(RoomItem RoomItem_0, int int_17, int int_18, uint uint_2, double double_3)
        {
            ServerMessage Message = new ServerMessage();
            Message.Init(230u);
            Message.AppendInt32(RoomItem_0.Int32_0);
            Message.AppendInt32(RoomItem_0.Int32_1);
            Message.AppendInt32(int_17);
            Message.AppendInt32(int_18);
            Message.AppendInt32(1);
            Message.AppendUInt(RoomItem_0.uint_0);
            Message.AppendStringWithBreak(RoomItem_0.Double_0.ToString().Replace(',', '.'));
            Message.AppendStringWithBreak(double_3.ToString().Replace(',', '.'));
            Message.AppendUInt(uint_2);
            this.SendMessage(Message, null);
            this.method_81(RoomItem_0, int_17, int_18, double_3);
        }
        private void method_41(RoomItem RoomItem_0, ThreeDCoord gstruct1_1, uint uint_2, double double_3)
        {
            this.method_40(RoomItem_0, gstruct1_1.x, gstruct1_1.y, uint_2, double_3);
        }
        private void method_42(RoomUser RoomUser_1, ThreeDCoord gstruct1_1, uint uint_2, double double_3)
        {
            ServerMessage Message = new ServerMessage();
            Message.Init(230u);
            Message.AppendInt32(RoomUser_1.int_3);
            Message.AppendInt32(RoomUser_1.int_4);
            Message.AppendInt32(gstruct1_1.x);
            Message.AppendInt32(gstruct1_1.y);
            Message.AppendInt32(0);
            Message.AppendUInt(uint_2);
            Message.AppendString("J");
            Message.AppendInt32(RoomUser_1.VirtualId);
            Message.AppendStringWithBreak(RoomUser_1.double_0.ToString().Replace(',', '.'));
            Message.AppendStringWithBreak(double_3.ToString().Replace(',', '.'));
            this.SendMessage(Message, null);
            this.byte_0[RoomUser_1.int_3, RoomUser_1.int_4] = 1;
            RoomUser_1.int_3 = gstruct1_1.x;
            RoomUser_1.int_4 = gstruct1_1.y;
            RoomUser_1.double_0 = double_3;
            RoomUser_1.int_12 = gstruct1_1.x;
            RoomUser_1.int_13 = gstruct1_1.y;
            RoomUser_1.double_1 = double_3;
            RoomUser_1.int_9 = 2;
            this.byte_0[RoomUser_1.int_3, RoomUser_1.int_4] = 0;
            this.method_87(RoomUser_1, false, true);
        }
        internal RoomUser method_43(int int_17, int int_18)
        {
            RoomUser result;
            if (this.RoomUsers != null)
            {
                for (int i = 0; i < this.RoomUsers.Length; i++)
                {
                    RoomUser @class = this.RoomUsers[i];
                    if (@class != null && (@class.int_3 == int_17 && @class.int_4 == int_18))
                    {
                        result = @class;
                        return result;
                    }
                }
            }
            result = null;
            return result;
        }
        internal RoomUser method_44(int int_17, int int_18)
        {
            RoomUser result;
            if (this.RoomUsers != null)
            {
                for (int i = 0; i < this.RoomUsers.Length; i++)
                {
                    RoomUser @class = this.RoomUsers[i];
                    if (@class != null)
                    {
                        if (@class.int_3 == int_17 && @class.int_4 == int_18)
                        {
                            result = @class;
                            return result;
                        }
                        if (@class.int_12 == int_17 && @class.int_13 == int_18)
                        {
                            result = @class;
                            return result;
                        }
                    }
                }
            }
            result = null;
            return result;
        }
        private List<RoomItem> method_45(int int_17, int int_18)
        {
            List<RoomItem> list = new List<RoomItem>();
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                if (@class.Int32_0 == int_17 && @class.Int32_1 == int_18)
                {
                    list.Add(@class);
                }
            }
            return list;
        }
        public void method_46(GameClient Session, bool bool_13)
        {
            RoomUser @class = new RoomUser(Session.GetHabbo().Id, this.Id, this.int_7++, Session.GetHabbo().IsVisible);
            if (@class != null && @class.GetClient() != null && @class.GetClient().GetHabbo() != null)
            {
                if (bool_13 || !@class.bool_12)
                {
                    @class.bool_11 = true;
                }
                else
                {
                    @class.method_7(this.RoomModel.int_0, this.RoomModel.int_1, this.RoomModel.double_0);
                    @class.method_9(this.RoomModel.int_2);
                    if (this.CheckRights(Session, true))
                    {
                        @class.AddStatus("flatctrl", "useradmin");
                    }
                    else
                    {
                        if (this.method_26(Session))
                        {
                            @class.AddStatus("flatctrl", "");
                        }
                    }
                    if (!@class.IsBot && @class.GetClient().GetHabbo().bool_7)
                    {
                        RoomItem class2 = this.method_28(@class.GetClient().GetHabbo().uint_5);
                        if (class2 != null)
                        {
                            @class.method_7(class2.Int32_0, class2.Int32_1, class2.Double_0);
                            @class.method_9(class2.int_3);
                            class2.uint_4 = Session.GetHabbo().Id;
                            class2.ExtraData = "2";
                            class2.UpdateState(false, true);
                        }
                    }
                    @class.GetClient().GetHabbo().bool_7 = false;
                    @class.GetClient().GetHabbo().uint_5 = 0u;
                    ServerMessage Message = new ServerMessage(28u);
                    Message.AppendInt32(1);
                    @class.method_14(Message);
                    this.SendMessage(Message, null);
                }
                int num = this.method_5();
                @class.int_20 = num;
                this.RoomUsers[num] = @class;
                if (!bool_13)
                {
                    this.bool_10 = true;
                }
                Session.GetHabbo().CurrentRoomId = this.Id;
                Session.GetHabbo().GetMessenger().method_5(false);
                Session.GetHabbo().RoomVisits++;
                Session.GetHabbo().CheckRoomEntryAchievements();
                Session.GetHabbo().SendToRoom(this.Id);
                if (Session.GetHabbo().int_0 > 0)
                {
                    GroupsManager class3 = Groups.smethod_2(Session.GetHabbo().int_0);
                    if (class3 != null && !this.list_17.Contains(class3))
                    {
                        this.list_17.Add(class3);
                        ServerMessage Message2 = new ServerMessage(309u);
                        Message2.AppendInt32(this.list_17.Count);
                        foreach (GroupsManager current in this.list_17)
                        {
                            Message2.AppendInt32(current.int_0);
                            Message2.AppendStringWithBreak(current.string_2);
                        }
                        this.SendMessage(Message2, null);
                    }
                }
                if (!bool_13)
                {
                    this.method_51();
                    for (int i = 0; i < this.RoomUsers.Length; i++)
                    {
                        RoomUser class4 = this.RoomUsers[i];
                        if (class4 != null && class4.IsBot)
                        {
                            class4.BotAI.OnUserEnterRoom(@class);
                        }
                    }
                }

                bool RoomHasPoll = false;
                bool UserFilledPoll = false;
                using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                {
                    if (dbClient.ReadDataRow("SELECT null FROM room_polls WHERE room_id = '" + Session.GetHabbo().CurrentRoomId + "' LIMIT 1") != null)
                    {
                        RoomHasPoll = true;
                    }
                }

                if (RoomHasPoll == true)
                {

                    int PollId;
                    string PollDetails;

                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                    {
                        PollId = dbClient.ReadInt32("SELECT id FROM room_polls WHERE room_id = '" + Session.GetHabbo().CurrentRoomId + "' LIMIT 1");
                        PollDetails = dbClient.ReadString("SELECT details FROM room_polls WHERE id = '" + PollId + "' LIMIT 1");


                        if (dbClient.ReadDataRow("SELECT null FROM room_poll_results WHERE user_id = '" + Session.GetHabbo().Id + "' AND poll_id = '" + PollId + "' LIMIT 1") != null)
                        {
                            UserFilledPoll = true;
                        }

                    }

                    if (UserFilledPoll == false)
                    {
                        Thread ShowPoll = new Thread(delegate() { Room.ShowPoll(Session, PollId, PollDetails); });
                        ShowPoll.Start();

                    }

                }
            }
        }

        public static void ShowPoll(GameClient Session, int PollId, string PollDetails)
        {
            Thread.Sleep(10000);

            if (Session.GetConnection() != null)
            {
                if (Session.GetHabbo().InRoom)
                {
                    Room Room = Session.GetHabbo().CurrentRoom;
                    if (Room == null)
                    {
                        return;
                    }
                    ServerMessage NewPoll = new ServerMessage(316);
                    NewPoll.AppendInt32(PollId);
                    NewPoll.AppendStringWithBreak(PollDetails);
                    Session.SendMessage(NewPoll);
                }
            }
        }

        public static void ShowResults(Room Room, int QuestionId, GameClient Session)
        {
            Thread.Sleep(30000);
            string Question;
            DataTable Data = null;
            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
            {
                Question = dbClient.ReadString("SELECT question FROM infobus_questions WHERE id = '" + QuestionId + "' LIMIT 1");
                Data = dbClient.ReadDataTable("SELECT * FROM infobus_answers WHERE question_id = '" + QuestionId + "'");
            }

            /*ServerMessage InfobusQuestion = new ServerMessage(80);
            InfobusQuestion.AppendStringWithBreak(Question);
            InfobusQuestion.AppendInt32(Data.Rows.Count);
            if (Data != null)
            {
                foreach (DataRow Row in Data.Rows)
                {
                    int ResultCount;
                    InfobusQuestion.AppendInt32((int)Row["id"]);
                    InfobusQuestion.AppendStringWithBreak((string)Row["answer_text"]);
                    using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
                    {
                        ResultCount = dbClient.ReadInt32("SELECT COUNT(*) FROM infobus_results WHERE answer_id = '" + (int)Row["id"] + "' AND question_id = '" + QuestionId + "'");
                    }
                    InfobusQuestion.AppendInt32(ResultCount);
                }
            }
            int AnswerUserCount;
            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
            {
                AnswerUserCount = dbClient.ReadInt32("SELECT COUNT(*) FROM infobus_results WHERE question_id = '" + QuestionId + "'");
            }
            InfobusQuestion.AppendInt32(AnswerUserCount);
            Room.SendMessage(InfobusQuestion, null);

            using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
            {
                dbClient.ExecuteQuery("DELETE FROM infobus_results WHERE question_id = '" + QuestionId + "'");
            }*/

            ServerMessage InfobusQuestion = new ServerMessage(80);
            InfobusQuestion.AppendStringWithBreak(Question);
            InfobusQuestion.AppendInt32(Data.Rows.Count);
            if (Data != null)
            {
                foreach (DataRow Row in Data.Rows)
                {
                    int ResultCount = Room.InfobusAnswers.Where(number => (int)Data.Rows[number - 1]["id"] == (int)Row["id"]).Count();
                    InfobusQuestion.AppendInt32((int)Row["id"]);
                    InfobusQuestion.AppendStringWithBreak((string)Row["answer_text"]);
                    InfobusQuestion.AppendInt32(ResultCount);
                }
            }
            int AnswerUserCount = Room.InfobusAnswers.Count;
            InfobusQuestion.AppendInt32(AnswerUserCount);
            Room.SendMessage(InfobusQuestion, null);

            Room.InfobusAnswers.Clear();
        }

        public void method_47(GameClient Session, bool bool_13, bool bool_14)
        {
            int num = 1;
            if (Session != null && Session.GetHabbo() != null)
            {
                RoomUser @class = this.GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (@class != null)
                {
                    this.GetRoomTeamManager().OnUserLeave(@class);
                }
            }
            if (!bool_14 || !Session.GetHabbo().IsJuniori)
            {
                if (this.bool_12)
                {
                    if (bool_13 && Session != null)
                    {
                        if (bool_14)
                        {
                            ServerMessage Message = new ServerMessage(33u);
                            Message.AppendInt32(4008);
                            Session.SendMessage(Message);
                        }
                        ServerMessage Message5_ = new ServerMessage(18u);
                        Session.SendMessage(Message5_);
                    }
                }
                else
                {
                    try
                    {
                        if (Session != null && Session.GetHabbo() != null)
                        {
                            num = 2;
                            RoomUser @class = this.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            if (@class != null)
                            {
                                this.RoomUsers[@class.int_20] = null;
                                @class.int_20 = -1;
                                this.byte_0[@class.int_3, @class.int_4] = @class.byte_0;
                            }
                            num = 3;
                            if (bool_13)
                            {
                                if (bool_14)
                                {
                                    ServerMessage Message = new ServerMessage(33u);
                                    Message.AppendInt32(4008);
                                    Session.SendMessage(Message);
                                }
                                ServerMessage Message5_ = new ServerMessage(18u);
                                Session.SendMessage(Message5_);
                            }
                            num = 4;
                            if (@class != null && !@class.bool_11)
                            {
                                if (@class.byte_1 > 0 && @class.GetClient() != null)
                                {
                                    @class.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 = -1;
                                }
                                this.byte_0[@class.int_3, @class.int_4] = @class.byte_0;
                                if (!this.IsPublic)
                                {
                                    ServerMessage Message2 = new ServerMessage(700u);
                                    Message2.AppendBoolean(false);
                                    Session.SendMessage(Message2);
                                }
                                ServerMessage Message3 = new ServerMessage(29u);
                                Message3.AppendRawInt32(@class.VirtualId);
                                this.SendMessage(Message3, null);
                                if (this.method_74(Session.GetHabbo().Id))
                                {
                                    this.method_78(Session.GetHabbo().Id);
                                }
                                num = 5;
                                if (Session.GetHabbo().Username.ToLower() == this.Owner.ToLower() && this.HasEvent)
                                {
                                    this.Event = null;
                                    ServerMessage Logging = new ServerMessage(370u);
                                    Logging.AppendStringWithBreak("-1");
                                    this.SendMessage(Logging, null);
                                }
                                num = 6;
                                if (@class.class34_1 != null)
                                {
                                    @class.class34_1.RoomUser_0 = null;
                                    @class.class34_1 = null;
                                    Session.GetHabbo().GetEffectsInventoryComponent().int_0 = -1;
                                }
                                Session.GetHabbo().RemoveFromRoom();
                                this.bool_10 = true;
                                this.method_51();
                                List<RoomUser> list = new List<RoomUser>();
                                for (int i = 0; i < this.RoomUsers.Length; i++)
                                {
                                    RoomUser class2 = this.RoomUsers[i];
                                    if (class2 != null && class2.IsBot)
                                    {
                                        list.Add(class2);
                                    }
                                }
                                num = 7;
                                foreach (RoomUser current in list)
                                {
                                    current.BotAI.OnUserLeaveRoom(Session);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogCriticalException(string.Concat(new object[]
						{
							"Error during removing user from room [Part: ",
							num,
							"]: ",
							ex.ToString()
						}));
                    }
                }
            }
        }
        public RoomUser method_48(uint uint_2)
        {
            RoomUser result;
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && @class.IsBot && @class.IsPet && @class.PetData != null && @class.PetData.PetId == uint_2)
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }
        public bool method_49(uint uint_2)
        {
            return this.method_48(uint_2) != null;
        }
        public void method_50()
        {
            this.UsersNow = this.UserCount;
            using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
            {
                @class.ExecuteQuery(string.Concat(new object[]
				{
					"UPDATE rooms SET users_now = '",
					this.UserCount,
					"' WHERE Id = '",
					this.Id,
					"' LIMIT 1"
				}));
            }
        }
        public void method_51()
        {
            this.UsersNow = this.UserCount;
        }
        public RoomUser method_52(int int_17)
        {
            RoomUser result;
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && @class.VirtualId == int_17)
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }

        public RoomUser GetRoomUserByHabbo(uint uint_2)
        {
            RoomUser result;
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && !@class.IsBot && @class.UId == uint_2)
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }

        public void Rave()
        {
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && (!@class.IsBot && @class.class34_1 == null))
                {
                    @class.DanceId = 1;
                    ServerMessage Message = new ServerMessage(480u);
                    Message.AppendInt32(@class.VirtualId);
                    Message.AppendInt32(1);
                    this.SendMessage(Message, null);
                }
            }
        }
        public void method_55()
        {
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && (!@class.IsBot && @class.class34_1 == null) && (!@class.Statusses.ContainsKey("sit") && !@class.Statusses.ContainsKey("lay") && @class.int_8 != 1 && @class.int_8 != 3 && @class.int_8 != 5 && @class.int_8 != 7))
                {
                    @class.AddStatus("sit", ((@class.double_0 + 1.0) / 2.0 - @class.double_0 * 0.5).ToString().Replace(",", "."));
                    @class.UpdateNeeded = true;
                }
            }
        }
        public RoomUser method_56(string string_10)
        {
            RoomUser result;
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && !@class.IsBot && @class.GetClient() != null && @class.GetClient().GetHabbo() != null && @class.GetClient().GetHabbo().Username.ToLower() == string_10.ToLower())
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }
        public RoomUser method_57(string string_10)
        {
            RoomUser result;
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null && @class.IsBot && @class.RoomBot.Name.ToLower() == string_10.ToLower())
                {
                    result = @class;
                    return result;
                }
            }
            result = null;
            return result;
        }
        internal void method_58(ServerMessage Message5_0, List<uint> list_18, uint uint_2)
        {
            List<uint> list = new List<uint>();
            if (list_18 != null)
            {
                if (this.RoomUsers == null)
                {
                    return;
                }
                for (int i = 0; i < this.RoomUsers.Length; i++)
                {
                    RoomUser @class = this.RoomUsers[i];
                    if (@class != null && !@class.IsBot)
                    {
                        GameClient class2 = @class.GetClient();
                        if (class2 != null && class2.GetHabbo().Id != uint_2 && class2.GetHabbo().list_2.Contains(uint_2))
                        {
                            list.Add(class2.GetHabbo().Id);
                        }
                    }
                }
            }
            this.SendMessage(Message5_0, list);
        }
        internal void SendMessage(ServerMessage Message5_0, List<uint> list_18)
        {
            try
            {
                if (this.RoomUsers != null)
                {
                    byte[] array = Message5_0.GetBytes();
                    for (int i = 0; i < this.RoomUsers.Length; i++)
                    {
                        RoomUser @class = this.RoomUsers[i];
                        if (@class != null && !@class.IsBot)
                        {
                            GameClient class2 = @class.GetClient();
                            if (class2 != null && (list_18 == null || !list_18.Contains(class2.GetHabbo().Id)))
                            {
                                try
                                {
                                    class2.GetConnection().SendData(array);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        internal void method_60(ServerMessage Message5_0, int int_17)
        {
            try
            {
                byte[] array = Message5_0.GetBytes();
                for (int i = 0; i < this.RoomUsers.Length; i++)
                {
                    RoomUser @class = this.RoomUsers[i];
                    if (@class != null && !@class.IsBot)
                    {
                        GameClient class2 = @class.GetClient();
                        if (class2 != null && class2.GetHabbo() != null && (ulong)class2.GetHabbo().Rank >= (ulong)((long)int_17))
                        {
                            try
                            {
                                class2.GetConnection().SendData(array);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        public void method_61(ServerMessage Message5_0)
        {
            try
            {
                byte[] array = Message5_0.GetBytes();
                for (int i = 0; i < this.RoomUsers.Length; i++)
                {
                    RoomUser @class = this.RoomUsers[i];
                    if (@class != null && !@class.IsBot)
                    {
                        GameClient class2 = @class.GetClient();
                        if (class2 != null && this.method_26(class2))
                        {
                            try
                            {
                                class2.GetConnection().SendData(array);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        public void method_62()
        {
            this.SendMessage(new ServerMessage(18u), null);
            this.method_63();
        }
        public void method_63()
        {
            this.method_66(true);
            GC.SuppressFinalize(this);
        }
        internal void method_64()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<uint, bool> dictionary = new Dictionary<uint, bool>();
            try
            {
                try
                {
                    using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                    {
                        if (this.list_14.Count > 0)
                        {
                            lock (this.list_14)
                            {
                                foreach (RoomItem class2 in this.list_14)
                                {
                                    try
                                    {
                                        if (!dictionary.ContainsKey(class2.uint_0))
                                        {
                                            @class.AddParamWithValue(class2.uint_0 + "Extra1", class2.string_2);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra2", class2.string_3);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra3", class2.string_4);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra4", class2.string_5);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra5", class2.string_6);
                                            stringBuilder.Append(string.Concat(new object[]
												{
													"DELETE FROM wired_items WHERE item_id = '",
													class2.uint_0,
													"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
													class2.uint_0,
													"',@",
													class2.uint_0,
													"Extra1,@",
													class2.uint_0,
													"Extra2,@",
													class2.uint_0,
													"Extra3,@",
													class2.uint_0,
													"Extra4,@",
													class2.uint_0,
													"Extra5); "
												}));
                                        }
                                        dictionary.Add(class2.uint_0, true);

                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        if (this.list_15.Count > 0)
                        {
                            lock (this.list_15)
                            {
                                foreach (RoomItem class2 in this.list_15)
                                {
                                    try
                                    {
                                        if (!dictionary.ContainsKey(class2.uint_0))
                                        {
                                            @class.AddParamWithValue(class2.uint_0 + "Extra1", class2.string_2);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra2", class2.string_3);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra3", class2.string_4);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra4", class2.string_5);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra5", class2.string_6);
                                            stringBuilder.Append(string.Concat(new object[]
												{
													"DELETE FROM wired_items WHERE item_id = '",
													class2.uint_0,
													"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
													class2.uint_0,
													"',@",
													class2.uint_0,
													"Extra1,@",
													class2.uint_0,
													"Extra2,@",
													class2.uint_0,
													"Extra3,@",
													class2.uint_0,
													"Extra4,@",
													class2.uint_0,
													"Extra5); "
												}));
                                        }
                                        dictionary.Add(class2.uint_0, true);
                                    }

                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        if (this.list_16.Count > 0)
                        {
                            lock (this.list_16)
                            {
                                foreach (RoomItem class2 in this.list_16)
                                {
                                    try
                                    {
                                        if (!dictionary.ContainsKey(class2.uint_0))
                                        {
                                            @class.AddParamWithValue(class2.uint_0 + "Extra1", class2.string_2);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra2", class2.string_3);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra3", class2.string_4);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra4", class2.string_5);
                                            @class.AddParamWithValue(class2.uint_0 + "Extra5", class2.string_6);
                                            stringBuilder.Append(string.Concat(new object[]
												{
													"DELETE FROM wired_items WHERE item_id = '",
													class2.uint_0,
													"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
													class2.uint_0,
													"',@",
													class2.uint_0,
													"Extra1,@",
													class2.uint_0,
													"Extra2,@",
													class2.uint_0,
													"Extra3,@",
													class2.uint_0,
													"Extra4,@",
													class2.uint_0,
													"Extra5); "
												}));
                                        }
                                        dictionary.Add(class2.uint_0, true);
                                    }

                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        if (stringBuilder.Length > 0)
                        {
                            @class.ExecuteQuery(stringBuilder.ToString());
                        }
                        dictionary.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogCriticalException(string.Concat(new object[]
					{
						"Error during saving wired items for room ",
						this.Id,
						". Stack: ",
						ex.ToString(),
						"\rQuery: ",
						stringBuilder.ToString()
					}));
                }
                if (this.hashtable_3.Count > 0 || this.hashtable_1.Count > 0 || this.hashtable_2.Count > 0 || this.Boolean_4)
                {
                    using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                    {
                        stringBuilder.Clear();
                        lock (this.hashtable_1)
                        {
                            foreach (RoomItem class2 in this.hashtable_1.Values)
                            {
                                stringBuilder.Append(string.Concat(new object[]
								{
									"UPDATE items SET room_id = '0' WHERE Id = '",
									class2.uint_0,
									"' AND room_id = '",
									this.Id,
									"' LIMIT 1; "
								}));
                            }
                        }
                        this.hashtable_1.Clear();
                        lock (this.hashtable_3)
                        {
                            if (this.hashtable_3.Count > 0)
                            {
                                int num = 0;
                                int num2 = 0;
                                foreach (RoomItem class2 in this.hashtable_3.Values)
                                {
                                    if (class2.Boolean_2)
                                    {
                                        num2++;
                                    }
                                    else
                                    {
                                        num++;
                                    }
                                }
                                if (num2 > 0)
                                {
                                    foreach (RoomItem class2 in this.hashtable_3.Values)
                                    {
                                        if (class2.Boolean_2)
                                        {
                                            stringBuilder.Append(string.Concat(new object[]
											{
												"UPDATE items SET room_id = '",
												this.Id,
												"', base_item = '",
												class2.uint_2,
												"', x = '",
												class2.Int32_0,
												"', y = '",
												class2.Int32_1,
												"', z = '",
												class2.Double_0.ToString().Replace(",", "."),
												"', rot = '",
												class2.int_3,
												"', wall_pos = '' WHERE Id = '",
												class2.uint_0,
												"' LIMIT 1; "
											}));

                                            if (!string.IsNullOrEmpty(class2.ExtraData))
                                            {
                                                @class.AddParamWithValue("extra_data" + class2.uint_0, class2.ExtraData);
                                                stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + class2.uint_0 + "'; ",
                                                "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('" + class2.uint_0 + "' , @extra_data" + class2.uint_0 + "); ",
                                            }));
                                            }
                                            else
                                            {
                                                stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + class2.uint_0 + "'; "
                                            }));
                                            }
                                        }
                                    }
                                }
                                if (num > 0)
                                {
                                    foreach (RoomItem class2 in this.hashtable_3.Values)
                                    {
                                        if (class2.Boolean_1)
                                        {
                                            @class.AddParamWithValue("pos" + class2.uint_0, class2.string_7);
                                            stringBuilder.Append(string.Concat(new object[]
											{
												"UPDATE items SET room_id = '",
												this.Id,
												"', base_item = '",
												class2.uint_2,
												"', x = '0', y = '0', z = '0', rot = '0', wall_pos = @pos",
												class2.uint_0,
												" WHERE Id = '",
												class2.uint_0,
												"' LIMIT 1; "
											}));

                                            if (!string.IsNullOrEmpty(class2.ExtraData))
                                            {
                                                @class.AddParamWithValue("extra_data" + class2.uint_0, class2.ExtraData);
                                                stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + class2.uint_0 + "'; ",
                                                "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('" + class2.uint_0 + "' , @extra_data" + class2.uint_0 + "); ",
                                            }));
                                            }
                                            else
                                            {
                                                stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + class2.uint_0 + "'; "
                                            }));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        this.hashtable_3.Clear();
                        lock (this.hashtable_2)
                        {
                            foreach (RoomItem class2 in this.hashtable_2.Values)
                            {
                                stringBuilder.Append(string.Concat(new object[]
								{
									"UPDATE items SET x = '",
									class2.Int32_0,
									"', y = '",
									class2.Int32_1,
									"', z = '",
									class2.Double_0.ToString().Replace(",", "."),
									"', rot = '",
									class2.int_3,
									"', wall_pos = '",
									class2.string_7,
									"' WHERE Id = '",
									class2.uint_0,
									"' LIMIT 1; "
								}));

                                if (!string.IsNullOrEmpty(class2.ExtraData))
                                {
                                    @class.AddParamWithValue("mextra_data" + class2.uint_0, class2.ExtraData);
                                    stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + class2.uint_0 + "'; ",
                                                "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('" + class2.uint_0 + "' , @mextra_data" + class2.uint_0 + "); ",
                                            }));
                                }
                                else
                                {
                                    stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + class2.uint_0 + "'; "
                                            }));
                                }
                            }
                        }
                        this.hashtable_2.Clear();
                        lock (this.GetPets())
                        {
                            foreach (Pet current in this.GetPets())
                            {
                                if (current.DBState == DatabaseUpdateState.NeedsInsert)
                                {
                                    @class.AddParamWithValue("petname" + current.PetId, current.Name);
                                    @class.AddParamWithValue("petcolor" + current.PetId, current.Color);
                                    @class.AddParamWithValue("petrace" + current.PetId, current.Race);
                                    stringBuilder.Append(string.Concat(new object[]
									{
										"INSERT INTO `user_pets` VALUES ('",
										current.PetId,
										"', '",
										current.OwnerId,
										"', '",
										current.RoomId,
										"', @petname",
										current.PetId,
										", @petrace",
										current.PetId,
										", @petcolor",
										current.PetId,
										", '",
										current.Type,
										"', '",
										current.Expirience,
										"', '",
										current.Energy,
										"', '",
										current.Nutrition,
										"', '",
										current.Respect,
										"', '",
										current.CreationStamp,
										"', '",
										current.X,
										"', '",
										current.Y,
										"', '",
										current.Z,
										"'); "
									}));
                                }
                                else
                                {
                                    if (current.DBState == DatabaseUpdateState.NeedsUpdate)
                                    {
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"UPDATE user_pets SET room_id = '",
											current.RoomId,
											"', expirience = '",
											current.Expirience,
											"', energy = '",
											current.Energy,
											"', nutrition = '",
											current.Nutrition,
											"', respect = '",
											current.Respect,
											"', x = '",
											current.X,
											"', y = '",
											current.Y,
											"', z = '",
											current.Z,
											"' WHERE Id = '",
											current.PetId,
											"' LIMIT 1; "
										}));
                                    }
                                }
                                current.DBState = DatabaseUpdateState.Updated;
                            }
                        }
                        if (stringBuilder.Length > 0)
                        {
                            @class.ExecuteQuery(stringBuilder.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogCriticalException(string.Concat(new object[]
				{
					"Error during saving furniture for room ",
					this.Id,
					". Stack: ",
					ex.ToString(),
					"\r Query: ",
					stringBuilder.ToString()
				}));
            }
        }
        internal void method_65(DatabaseClient class6_0)
        {
            try
            {
                Dictionary<uint, bool> dictionary = new Dictionary<uint, bool>();
                StringBuilder stringBuilder = new StringBuilder();
                if (this.list_14.Count > 0)
                {
                    foreach (RoomItem @class in this.list_14)
                    {

                        try
                        {
                            if (!dictionary.ContainsKey(@class.uint_0))
                            {

                                class6_0.AddParamWithValue(@class.uint_0 + "Extra1", @class.string_2);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra2", @class.string_3);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra3", @class.string_4);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra4", @class.string_5);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra5", @class.string_6);
                                stringBuilder.Append(string.Concat(new object[]
										{
											"DELETE FROM wired_items WHERE item_id = '",
											@class.uint_0,
											"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
											@class.uint_0,
											"',@",
											@class.uint_0,
											"Extra1,@",
											@class.uint_0,
											"Extra2,@",
											@class.uint_0,
											"Extra3,@",
											@class.uint_0,
											"Extra4,@",
											@class.uint_0,
											"Extra5); "
										}));
                            }
                            dictionary.Add(@class.uint_0, true);

                        }
                        catch
                        {
                        }
                    }

                }
                if (this.list_15.Count > 0)
                {
                    foreach (RoomItem @class in this.list_15)
                    {

                        try
                        {
                            if (!dictionary.ContainsKey(@class.uint_0))
                            {

                                class6_0.AddParamWithValue(@class.uint_0 + "Extra1", @class.string_2);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra2", @class.string_3);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra3", @class.string_4);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra4", @class.string_5);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra5", @class.string_6);
                                stringBuilder.Append(string.Concat(new object[]
										{
											"DELETE FROM wired_items WHERE item_id = '",
											@class.uint_0,
											"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
											@class.uint_0,
											"',@",
											@class.uint_0,
											"Extra1,@",
											@class.uint_0,
											"Extra2,@",
											@class.uint_0,
											"Extra3,@",
											@class.uint_0,
											"Extra4,@",
											@class.uint_0,
											"Extra5); "
										}));
                            }
                            dictionary.Add(@class.uint_0, true);

                        }
                        catch
                        {
                        }

                    }
                }
                if (this.list_16.Count > 0)
                {
                    foreach (RoomItem @class in this.list_16)
                    {

                        try
                        {
                            if (!dictionary.ContainsKey(@class.uint_0))
                            {

                                class6_0.AddParamWithValue(@class.uint_0 + "Extra1", @class.string_2);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra2", @class.string_3);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra3", @class.string_4);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra4", @class.string_5);
                                class6_0.AddParamWithValue(@class.uint_0 + "Extra5", @class.string_6);
                                stringBuilder.Append(string.Concat(new object[]
										{
											"DELETE FROM wired_items WHERE item_id = '",
											@class.uint_0,
											"' LIMIT 1; INSERT INTO wired_items (item_id,extra1,extra2,extra3,extra4,extra5) VALUES ('",
											@class.uint_0,
											"',@",
											@class.uint_0,
											"Extra1,@",
											@class.uint_0,
											"Extra2,@",
											@class.uint_0,
											"Extra3,@",
											@class.uint_0,
											"Extra4,@",
											@class.uint_0,
											"Extra5); "
										}));
                            }
                            dictionary.Add(@class.uint_0, true);
                        }

                        catch
                        {
                        }
                    }

                }
                dictionary.Clear();
                if (this.hashtable_3.Count > 0 || this.hashtable_1.Count > 0 || this.hashtable_2.Count > 0 || this.Boolean_4)
                {
                    foreach (RoomItem @class in this.hashtable_1.Values)
                    {
                        stringBuilder.Append(string.Concat(new object[]
						{
							"UPDATE items SET room_id = 0 WHERE Id = '",
							@class.uint_0,
							"' AND room_id = '",
							this.Id,
							"' LIMIT 1; "
						}));
                    }
                    this.hashtable_1.Clear();
                    IEnumerator enumerator2;
                    if (this.hashtable_3.Count > 0)
                    {
                        enumerator2 = this.hashtable_3.Values.GetEnumerator();
                        try
                        {
                            while (enumerator2.MoveNext())
                            {
                                RoomItem @class = (RoomItem)enumerator2.Current;
                                stringBuilder.Append("UPDATE items SET room_id = 0 WHERE Id = '" + @class.uint_0 + "' LIMIT 1; ");
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator2 as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                        int num = 0;
                        int num2 = 0;
                        enumerator2 = this.hashtable_3.Values.GetEnumerator();
                        try
                        {
                            while (enumerator2.MoveNext())
                            {
                                RoomItem @class = (RoomItem)enumerator2.Current;
                                if (@class.Boolean_2)
                                {
                                    num2++;
                                }
                                else
                                {
                                    num++;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator2 as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                        if (num2 > 0)
                        {
                            enumerator2 = this.hashtable_3.Values.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    RoomItem @class = (RoomItem)enumerator2.Current;
                                    if (@class.Boolean_2)
                                    {
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"UPDATE items SET room_id = '",
											this.Id,
											"', base_item = '",
											@class.uint_2,
											", x = '",
											@class.Int32_0,
											"', y = '",
											@class.Int32_1,
											"', z = '",
											@class.Double_0.ToString().Replace(",", "."),
											"', rot = '",
											@class.int_3,
											"', wall_pos = '' WHERE Id = '",
											@class.uint_0,
											"' LIMIT 1; "
										}));

                                            if (!string.IsNullOrEmpty(@class.ExtraData))
                                            {
                                                class6_0.AddParamWithValue("extra_data" + @class.uint_0, @class.ExtraData);
                                                stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + @class.uint_0 + "'; ",
                                                "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('" + @class.uint_0 + "' , @extra_data" + @class.uint_0 + "); ",
                                            }));
                                            }
                                            else
                                            {
                                                stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + @class.uint_0 + "'; "
                                            }));
                                            }
                                    }
                                }
                            }
                            finally
                            {
                                IDisposable disposable = enumerator2 as IDisposable;
                                if (disposable != null)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                        if (num > 0)
                        {
                            enumerator2 = this.hashtable_3.Values.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    RoomItem @class = (RoomItem)enumerator2.Current;
                                    if (@class.Boolean_1)
                                    {
                                        class6_0.AddParamWithValue("pos" + @class.uint_0, @class.string_7);
                                        stringBuilder.Append(string.Concat(new object[]
										{
											"UPDATE items SET room_id = '",
											this.Id,
											"', base_item = '",
											@class.uint_2,
											", x = '0', y = '0', z = '0', rot = '0', wall_pos = @pos",
											@class.uint_0,
											" WHERE Id = '",
											@class.uint_0,
											"' LIMIT 1; "
										}));

                                            if (!string.IsNullOrEmpty(@class.ExtraData))
                                            {
                                                class6_0.AddParamWithValue("extra_data" + @class.uint_0, @class.ExtraData);
                                                stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + @class.uint_0 + "'; ",
                                                "INSERT INTO items_extra_data (item_id,extra_data) VALUES ('" + @class.uint_0 + "' , @extra_data" + @class.uint_0 + "); ",
                                            }));
                                            }
                                            else
                                            {
                                                stringBuilder.Append(string.Concat(new object[]
                                            {
                                                "DELETE FROM items_extra_data WHERE item_id = '" + @class.uint_0 + "'; "
                                            }));
                                            }
                                    }
                                }
                            }
                            finally
                            {
                                IDisposable disposable = enumerator2 as IDisposable;
                                if (disposable != null)
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                        this.hashtable_3.Clear();
                    }
                    enumerator2 = this.hashtable_2.Values.GetEnumerator();
                    try
                    {
                        while (enumerator2.MoveNext())
                        {
                            RoomItem @class = (RoomItem)enumerator2.Current;
                            stringBuilder.Append(string.Concat(new object[]
							{
								"UPDATE items SET x = '",
								@class.Int32_0,
								"', y = '",
								@class.Int32_1,
								"', z = '",
								@class.Double_0.ToString().Replace(",", "."),
								"', rot = '",
								@class.int_3,
								"', wall_pos = '' WHERE Id = '",
								@class.uint_0,
								"' LIMIT 1; "
							}));
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator2 as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                    this.hashtable_2.Clear();
                    foreach (Pet current in this.GetPets())
                    {
                        if (current.DBState == DatabaseUpdateState.NeedsInsert)
                        {
                            class6_0.AddParamWithValue("petname" + current.PetId, current.Name);
                            class6_0.AddParamWithValue("petcolor" + current.PetId, current.Color);
                            class6_0.AddParamWithValue("petrace" + current.PetId, current.Race);
                            stringBuilder.Append(string.Concat(new object[]
							{
								"INSERT INTO `user_pets` VALUES ('",
								current.PetId,
								"', '",
								current.OwnerId,
								"', '",
								current.RoomId,
								"', @petname",
								current.PetId,
								", @petrace",
								current.PetId,
								", @petcolor",
								current.PetId,
								", '",
								current.Type,
								"', '",
								current.Expirience,
								"', '",
								current.Energy,
								"', '",
								current.Nutrition,
								"', '",
								current.Respect,
								"', '",
								current.CreationStamp,
								"', '",
								current.X,
								"', '",
								current.Y,
								"', '",
								current.Z,
								"');"
							}));
                        }
                        else
                        {
                            if (current.DBState == DatabaseUpdateState.NeedsUpdate)
                            {
                                stringBuilder.Append(string.Concat(new object[]
								{
									"UPDATE user_pets SET room_id = '",
									current.RoomId,
									"', expirience = '",
									current.Expirience,
									"', energy = '",
									current.Energy,
									"', nutrition = '",
									current.Nutrition,
									"', respect = '",
									current.Respect,
									"', x = '",
									current.X,
									"', y = '",
									current.Y,
									"', z = '",
									current.Z,
									"' WHERE Id = '",
									current.PetId,
									"' LIMIT 1; "
								}));
                            }
                        }
                        current.DBState = DatabaseUpdateState.Updated;
                    }
                }
                if (stringBuilder.Length > 0)
                {
                    class6_0.ExecuteQuery(stringBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Logging.LogCriticalException(string.Concat(new object[]
				{
					"Error during saving furniture for room ",
					this.Id,
					". Stack: ",
					ex.ToString()
				}));
            }
        }
        private void method_66(bool bool_13)
        {
            if (!this.bool_12)
            {
                this.bool_12 = true;
                if (bool_13)
                {
                    this.bool_11 = false;
                    if (this.timer_0 != null)
                    {
                        this.bool_6 = true;
                        this.timer_0.Change(-1, -1);
                    }
                    this.bool_6 = true;
                    this.method_64();
                    using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                    {
                        @class.ExecuteQuery(string.Concat(new object[]
						{
							"UPDATE user_pets SET room_id = 0 WHERE room_id = ",
							this.Id,
							" AND NOT user_id = ",
							GoldTree.GetGame().GetClientManager().method_27(this.Owner)
						}));
                    }
                    //this.timer_0.Dispose();
                    this.timer_0 = null;
                    this.bool_9 = false;
                    if (this.Tags != null)
                    {
                        this.Tags.Clear();
                    }
                    this.Tags = null;
                    if (this.RoomUsers != null)
                    {
                        Array.Clear(this.RoomUsers, 0, this.RoomUsers.Length);
                    }
                    this.RoomUsers = null;
                    this.RoomIcon = null;
                    if (this.UsersWithRights != null)
                    {
                        this.UsersWithRights.Clear();
                    }
                    this.RoomIcon = null;
                    if (this.dictionary_0 != null)
                    {
                        this.dictionary_0.Clear();
                    }
                    this.dictionary_0 = null;
                    this.Wallpaper = null;
                    this.Floor = null;
                    this.Landscape = null;
                    if (this.hashtable_0 != null)
                    {
                        this.hashtable_0.Clear();
                    }
                    this.hashtable_0 = null;
                    if (this.hashtable_4 != null)
                    {
                        this.hashtable_4.Clear();
                    }
                    this.hashtable_4 = null;
                    this.MoodlightData = null;
                    if (this.list_2 != null)
                    {
                        this.list_2.Clear();
                    }
                    this.list_2 = null;
                    if (this.MusicController != null)
                    {
                        this.MusicController.UnLinkRoomOutputItem();
                    }
                    this.MusicController = null;
                    if (this.InfobusAnswers != null)
                    {
                        this.InfobusAnswers.Clear();
                    }
                    this.InfobusAnswers = null;
                }
            }
        }
        public ServerMessage method_67(bool bool_13)
        {
            List<RoomUser> list = new List<RoomUser>();
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null)
                {
                    if (!bool_13)
                    {
                        if (!@class.UpdateNeeded)
                        {
                            goto IL_35;
                        }
                        @class.UpdateNeeded = false;
                    }
                    list.Add(@class);
                }
            IL_35: ;
            }
            ServerMessage result;
            if (list.Count == 0)
            {
                result = null;
            }
            else
            {
                ServerMessage Message = new ServerMessage(34u);
                Message.AppendInt32(list.Count);
                foreach (RoomUser @class in list)
                {
                    @class.method_15(Message);
                }
                result = Message;
            }
            return result;
        }
        public bool method_68(uint uint_2)
        {
            return this.dictionary_0.ContainsKey(uint_2);
        }
        public void method_69(uint uint_2)
        {
            this.dictionary_0.Remove(uint_2);
        }
        public void method_70(uint uint_2)
        {
            this.dictionary_0.Add(uint_2, GoldTree.GetUnixTimestamp());
        }
        public bool method_71(uint uint_2)
        {
            bool result;
            if (!this.method_68(uint_2))
            {
                result = true;
            }
            else
            {
                double num = GoldTree.GetUnixTimestamp() - this.dictionary_0[uint_2];
                result = (num > 900.0);
            }
            return result;
        }
        public int method_72(string string_10)
        {
            int num = 0;
            foreach (RoomItem @class in this.Hashtable_1.Values)
            {
                if (@class.GetBaseItem().InteractionType.ToLower() == string_10.ToLower())
                {
                    num++;
                }
            }
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                if (@class.GetBaseItem().InteractionType.ToLower() == string_10.ToLower())
                {
                    num++;
                }
            }
            return num;
        }
        public bool method_73(RoomUser RoomUser_1)
        {
            return !RoomUser_1.IsBot && this.method_74(RoomUser_1.GetClient().GetHabbo().Id);
        }
        public bool method_74(uint uint_2)
        {
            bool result;
            using (TimedLock.Lock(this.list_2))
            {
                foreach (Trade current in this.list_2)
                {
                    if (current.method_0(uint_2))
                    {
                        result = true;
                        return result;
                    }
                }
            }
            result = false;
            return result;
        }
        public Trade method_75(RoomUser RoomUser_1)
        {
            Trade result;
            if (RoomUser_1.IsBot)
            {
                result = null;
            }
            else
            {
                result = this.method_76(RoomUser_1.GetClient().GetHabbo().Id);
            }
            return result;
        }
        public Trade method_76(uint uint_2)
        {
            Trade result;
            using (TimedLock.Lock(this.list_2))
            {
                foreach (Trade current in this.list_2)
                {
                    if (current.method_0(uint_2))
                    {
                        result = current;
                        return result;
                    }
                }
            }
            result = null;
            return result;
        }
        public void method_77(RoomUser RoomUser_1, RoomUser RoomUser_2)
        {
            if (RoomUser_1 != null && RoomUser_2 != null && (!RoomUser_1.IsBot || RoomUser_1.RoomBot.Boolean_1) && (!RoomUser_2.IsBot || RoomUser_2.RoomBot.Boolean_1) && !RoomUser_1.Boolean_3 && !RoomUser_2.Boolean_3 && !this.method_73(RoomUser_1) && !this.method_73(RoomUser_2))
            {
                this.list_2.Add(new Trade(RoomUser_1.GetClient().GetHabbo().Id, RoomUser_2.GetClient().GetHabbo().Id, this.Id));
            }
        }
        public void method_78(uint uint_2)
        {
            Trade @class = this.method_76(uint_2);
            if (@class != null)
            {
                @class.method_12(uint_2);
                this.list_2.Remove(@class);
            }
        }
        public bool method_79(GameClient Session, RoomItem RoomItem_0, int int_17, int int_18, int int_19, bool bool_13, bool bool_14, bool bool_15)
        {
            Dictionary<int, AffectedTile> dictionary = this.method_94(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, int_17, int_18, int_19);
            bool result;
            if (!this.method_92(int_17, int_18))
            {
                result = false;
            }
            else
            {
                foreach (AffectedTile current in dictionary.Values)
                {
                    if (!this.method_92(current.Int32_0, current.Int32_1))
                    {
                        result = false;
                        return result;
                    }
                }
                double num = this.RoomModel.double_1[int_17, int_18];
                if (!bool_14)
                {
                    if (RoomItem_0.int_3 == int_19 && RoomItem_0.Int32_0 == int_17 && RoomItem_0.Int32_1 == int_18 && RoomItem_0.Double_0 != num)
                    {
                        result = false;
                        return result;
                    }
                    if (this.RoomModel.squareState[int_17, int_18] != SquareState.OPEN)
                    {
                        result = false;
                        return result;
                    }
                    foreach (AffectedTile current in dictionary.Values)
                    {
                        if (this.RoomModel.squareState[current.Int32_0, current.Int32_1] != SquareState.OPEN)
                        {
                            result = false;
                            return result;
                        }
                    }
                    if (RoomItem_0.GetBaseItem().IsSeat || RoomItem_0.Boolean_0)
                    {
                        goto IL_1FE;
                    }
                    if (this.method_97(int_17, int_18) && !RoomItem_0.GetBaseItem().Walkable)
                    {
                        result = false;
                        return result;
                    }
                    using (Dictionary<int, AffectedTile>.ValueCollection.Enumerator enumerator = dictionary.Values.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            AffectedTile current = enumerator.Current;
                            if (this.method_97(current.Int32_0, current.Int32_1) && !RoomItem_0.GetBaseItem().Walkable)
                            {
                                result = false;
                                return result;
                            }
                        }
                        goto IL_1FE;
                    }
                }
                if (this.RoomModel.squareState[int_17, int_18] != SquareState.OPEN)
                {
                    result = false;
                    return result;
                }
                if ((!bool_15 || !RoomItem_0.GetBaseItem().Walkable) && this.method_97(int_17, int_18))
                {
                    result = false;
                    return result;
                }
                else if (!RoomItem_0.GetBaseItem().Walkable)
                {
                    if (!bool_15 && this.method_97(int_17, int_18))
                    {
                        result = false;
                        return result;
                    }
                }
            IL_1FE:
                List<RoomItem> list = this.method_93(int_17, int_18);
                List<RoomItem> list2 = new List<RoomItem>();
                List<RoomItem> list3 = new List<RoomItem>();
                foreach (AffectedTile current in dictionary.Values)
                {
                    List<RoomItem> list4 = this.method_93(current.Int32_0, current.Int32_1);
                    if (list4 != null)
                    {
                        list2.AddRange(list4);
                    }
                }
                if (list == null)
                {
                    list = new List<RoomItem>();
                }
                list3.AddRange(list);
                list3.AddRange(list2);
                int num2 = 0;
                foreach (RoomItem current2 in list3)
                {
                    if (current2 != null && current2.uint_0 != RoomItem_0.uint_0 && current2.GetBaseItem() != null)
                    {
                        if (!current2.GetBaseItem().Stackable)
                        {
                            result = false;
                            return result;
                        }
                        if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "wf_trg_timer" && current2.GetBaseItem().InteractionType.ToLower() == "wf_trg_timer")
                        {
                            result = false;
                            return result;
                        }
                        if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "ball")
                        {
                            if (current2.GetBaseItem().InteractionType.ToLower() == "blue_goal")
                            {
                                num2 = 11;
                            }
                            if (current2.GetBaseItem().InteractionType.ToLower() == "red_goal")
                            {
                                num2 = 5;
                            }
                            if (current2.GetBaseItem().InteractionType.ToLower() == "yellow_goal")
                            {
                                num2 = 14;
                            }
                            if (current2.GetBaseItem().InteractionType.ToLower() == "green_goal")
                            {
                                num2 = 8;

                                Habbo User;
                                GameClient GetPlayerSession = GoldTree.GetGame().GetClientManager().GetClientByHabbo(RoomItem_0.LastPlayerHitFootball);

                                if (GetPlayerSession != null)
                                {
                                    User = GetPlayerSession.GetHabbo();
                                    User.FootballGoalScorer++;
                                    User.CheckFootballGoalScorerScoreAchievements();
                                }

                                Habbo User2;
                                GameClient GetRoomOwner = GoldTree.GetGame().GetClientManager().GetClientByHabbo(Owner);

                                if (GetRoomOwner != null)
                                {
                                    User2 = GetRoomOwner.GetHabbo();
                                    User2.FootballGoalHost++;
                                    User2.CheckFootballGoalHostScoreAchievements();
                                }
                                else if (GetRoomOwner == null)
                                {
                                    using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
                                    {
                                        int RoomOwnerID = @class.ReadInt32("SELECT id FROM users WHERE username = '" + Owner + "'");
                                        @class.AddParamWithValue("roomownerid", RoomOwnerID);
                                        @class.ExecuteQuery("UPDATE user_stats SET FootballGoalHost = FootballGoalHost + 1 WHERE id = @roomownerid LIMIT 1");
                                    }
                                }
                            }
                        }
                    }
                }
                if (num2 > 0)
                {
                    this.method_89(num2, RoomItem_0, false);
                }
                if (!RoomItem_0.Boolean_0)
                {
                    if (RoomItem_0.int_3 != int_19 && RoomItem_0.Int32_0 == int_17 && RoomItem_0.Int32_1 == int_18)
                    {
                        num = RoomItem_0.Double_0;
                    }
                    foreach (RoomItem current2 in list3)
                    {
                        if (current2.uint_0 != RoomItem_0.uint_0 && current2.Double_1 > num)
                        {
                            num = current2.Double_1;
                        }
                    }
                }
                if (int_19 != 0 && int_19 != 2 && int_19 != 4 && int_19 != 6 && int_19 != 8)
                {
                    int_19 = 0;
                }
                Dictionary<int, AffectedTile> dictionary2 = new Dictionary<int, AffectedTile>();
                dictionary2 = this.method_94(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, RoomItem_0.Int32_0, RoomItem_0.Int32_1, RoomItem_0.int_3);
                int num3 = 0;
                int num4 = 0;
                if (!bool_13)
                {
                    num3 = RoomItem_0.Int32_0;
                    num4 = RoomItem_0.Int32_1;
                }
                RoomItem_0.int_3 = int_19;
                RoomItem_0.method_0(int_17, int_18, num);
                if (!bool_14 && Session != null)
                {
                    RoomItem_0.Class69_0.OnPlace(Session, RoomItem_0);
                }
                if (bool_13)
                {
                    if (this.hashtable_1.Contains(RoomItem_0.uint_0))
                    {
                        this.hashtable_1.Remove(RoomItem_0.uint_0);
                    }
                    if (this.hashtable_3.Contains(RoomItem_0.uint_0))
                    {
                        result = false;
                        return result;
                    }
                    this.hashtable_3.Add(RoomItem_0.uint_0, RoomItem_0);
                    if (RoomItem_0.Boolean_2)
                    {
                        if (this.hashtable_0.Contains(RoomItem_0.uint_0))
                        {
                            this.hashtable_0.Remove(RoomItem_0.uint_0);
                        }
                        this.hashtable_0.Add(RoomItem_0.uint_0, RoomItem_0);
                    }
                    else
                    {
                        if (this.hashtable_4.Contains(RoomItem_0.uint_0))
                        {
                            this.hashtable_4.Remove(RoomItem_0.uint_0);
                        }
                        this.hashtable_4.Add(RoomItem_0.uint_0, RoomItem_0);
                    }
                    ServerMessage Message5_ = new ServerMessage(93u);
                    RoomItem_0.method_6(Message5_);
                    this.SendMessage(Message5_, null);
                    string text = RoomItem_0.GetBaseItem().InteractionType.ToLower();
                    switch (text)
                    {
                        case "bb_patch":
                            this.list_5.Add(RoomItem_0);
                            if (RoomItem_0.ExtraData == "5")
                            {
                                this.list_6.Add(RoomItem_0);
                            }
                            else
                            {
                                if (RoomItem_0.ExtraData == "8")
                                {
                                    this.list_7.Add(RoomItem_0);
                                }
                                else
                                {
                                    if (RoomItem_0.ExtraData == "11")
                                    {
                                        this.list_9.Add(RoomItem_0);
                                    }
                                    else
                                    {
                                        if (RoomItem_0.ExtraData == "14")
                                        {
                                            this.list_8.Add(RoomItem_0);
                                        }
                                    }
                                }
                            }
                            break;
                        case "blue_score":
                            this.list_12.Add(RoomItem_0);
                            break;
                        case "green_score":
                            this.list_13.Add(RoomItem_0);
                            break;
                        case "red_score":
                            this.list_10.Add(RoomItem_0);
                            break;
                        case "yellow_score":
                            this.list_11.Add(RoomItem_0);
                            break;
                        case "stickiepole":
                            this.list_3.Add(RoomItem_0);
                            break;
                        case "wf_trg_onsay":
                        case "wf_trg_enterroom":
                        case "wf_trg_furnistate":
                        case "wf_trg_onfurni":
                        case "wf_trg_offfurni":
                        case "wf_trg_gameend":
                        case "wf_trg_gamestart":
                        case "wf_trg_atscore":
                            if (!this.list_14.Contains(RoomItem_0))
                            {
                                this.list_14.Add(RoomItem_0);
                            }
                            break;
                        case "wf_trg_attime":
                            if (RoomItem_0.string_2.Length <= 0)
                            {
                                RoomItem_0.string_2 = "10";
                            }
                            if (!this.list_14.Contains(RoomItem_0))
                            {
                                this.list_14.Add(RoomItem_0);
                            }
                            RoomItem_0.bool_0 = true;
                            RoomItem_0.ReqUpdate(1);
                            break;
                        case "wf_trg_timer":
                            if (RoomItem_0.string_2.Length <= 0)
                            {
                                RoomItem_0.string_2 = "10";
                            }
                            if (!this.list_14.Contains(RoomItem_0))
                            {
                                this.list_14.Add(RoomItem_0);
                            }
                            RoomItem_0.bool_0 = true;
                            RoomItem_0.ReqUpdate(1);
                            break;
                        case "wf_act_saymsg":
                        case "wf_act_kick_user":
                        case "wf_act_moveuser":
                        case "wf_act_togglefurni":
                        case "wf_act_givepoints":
                        case "wf_act_moverotate":
                        case "wf_act_matchfurni":
                        case "wf_act_give_phx":
                            if (!this.list_15.Contains(RoomItem_0))
                            {
                                this.list_15.Add(RoomItem_0);
                            }
                            break;
                        case "wf_cnd_trggrer_on_frn":
                        case "wf_cnd_furnis_hv_avtrs":
                        case "wf_cnd_has_furni_on":
                        case "wf_cnd_match_snapshot":
                        case "wf_cnd_time_more_than":
                        case "wf_cnd_time_less_than":
                        case "wf_cnd_phx":
                            if (!this.list_16.Contains(RoomItem_0))
                            {
                                this.list_16.Add(RoomItem_0);
                            }
                            break;
                        case "freeze_tile":
                            this.GetFreeze().AddFreezeTile(RoomItem_0);
                            break;
                        case "freeze_ice_block":
                            this.GetFreeze().AddFreezeBlock(RoomItem_0);
                            break;
                        case "freeze_exit":
                            RoomItem exitTeleport = this.GetFreeze().ExitTeleport;
                            if (exitTeleport == null)
                            {
                                this.GetFreeze().ExitTeleport = RoomItem_0;
                            }
                            break;
                        case "freeze_blue_gate":
                            this.GetGameManager().AddFreezeGate(RoomItem_0);
                            break;
                        case "freeze_red_gate":
                            this.GetGameManager().AddFreezeGate(RoomItem_0);
                            break;
                        case "freeze_green_gate":
                            this.GetGameManager().AddFreezeGate(RoomItem_0);
                            break;
                        case "freeze_yellow_gate":
                            this.GetGameManager().AddFreezeGate(RoomItem_0);
                            break;
                        case "freeze_blue_score":
                            this.GetGameManager().AddFreezeScoreboard(RoomItem_0);
                            break;
                        case "freeze_red_score":
                            this.GetGameManager().AddFreezeScoreboard(RoomItem_0);
                            break;
                        case "freeze_green_score":
                            this.GetGameManager().AddFreezeScoreboard(RoomItem_0);
                            break;
                        case "freeze_yellow_score":
                            this.GetGameManager().AddFreezeScoreboard(RoomItem_0);
                            break;
                    }
                }
                else
                {
                    if (!this.hashtable_2.Contains(RoomItem_0.uint_0) && !this.hashtable_3.ContainsKey(RoomItem_0.uint_0))
                    {
                        this.hashtable_2.Add(RoomItem_0.uint_0, RoomItem_0);
                    }
                    if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "wf_act_give_phx" && Session != null)
                    {
                        string text2 = RoomItem_0.string_2.Split(new char[]
						{
							':'
						})[0].ToLower();
                        if (!GoldTree.GetGame().GetRoleManager().method_12(text2, Session))
                        {
                            RoomItem_0.string_2 = "";
                        }
                    }
                    if (RoomItem_0.GetBaseItem().InteractionType.ToLower() == "wf_cnd_phx" && Session != null)
                    {
                        string text2 = RoomItem_0.string_2.Split(new char[]
						{
							':'
						})[0].ToLower();
                        if (!GoldTree.GetGame().GetRoleManager().method_11(text2, Session))
                        {
                            RoomItem_0.string_2 = "";
                        }
                    }
                    ServerMessage Message5_ = new ServerMessage(95u);
                    RoomItem_0.method_6(Message5_);
                    this.SendMessage(Message5_, null);
                }
                this.method_22();
                if (!bool_14)
                {
                    this.method_87(this.method_43(int_17, int_18), true, true);
                    foreach (AffectedTile current in dictionary.Values)
                    {
                        this.method_87(this.method_43(current.Int32_0, current.Int32_1), true, true);
                    }
                    if (num3 > 0 || num4 > 0)
                    {
                        this.method_87(this.method_43(num3, num4), true, true);
                    }
                    foreach (AffectedTile current in dictionary2.Values)
                    {
                        this.method_87(this.method_43(current.Int32_0, current.Int32_1), true, true);
                    }
                }
                result = true;
            }
            return result;
        }
        internal void method_80(RoomItem RoomItem_0)
        {
            if (!this.hashtable_2.Contains(RoomItem_0.uint_0) && !this.hashtable_3.ContainsKey(RoomItem_0.uint_0))
            {
                this.hashtable_2.Add(RoomItem_0.uint_0, RoomItem_0);
            }
        }
        public bool method_81(RoomItem RoomItem_0, int int_17, int int_18, double double_3)
        {
            Dictionary<int, AffectedTile> dictionary = this.method_94(RoomItem_0.GetBaseItem().Length, RoomItem_0.GetBaseItem().Width, int_17, int_18, RoomItem_0.int_3);
            RoomItem_0.method_0(int_17, int_18, double_3);
            if (!this.hashtable_2.Contains(RoomItem_0.uint_0))
            {
                this.hashtable_2.Add(RoomItem_0.uint_0, RoomItem_0);
            }
            this.method_22();
            this.method_87(this.method_43(int_17, int_18), true, true);
            foreach (AffectedTile current in dictionary.Values)
            {
                this.method_87(this.method_43(current.Int32_0, current.Int32_1), true, true);
            }
            return true;
        }
        public bool method_82(GameClient Session, RoomItem RoomItem_0, bool bool_13, string string_10)
        {
            if (bool_13)
            {
                RoomItem_0.Class69_0.OnPlace(Session, RoomItem_0);
                string text = RoomItem_0.GetBaseItem().InteractionType.ToLower();
                if (text != null && text == "dimmer" && this.MoodlightData == null)
                {
                    this.MoodlightData = new MoodlightData(RoomItem_0.uint_0);
                    RoomItem_0.ExtraData = this.MoodlightData.method_7();
                }
                if (!this.hashtable_3.ContainsKey(RoomItem_0.uint_0))
                {
                    this.hashtable_3.Add(RoomItem_0.uint_0, RoomItem_0);
                    if (RoomItem_0.Boolean_2)
                    {
                        this.hashtable_0.Add(RoomItem_0.uint_0, RoomItem_0);
                    }
                    else
                    {
                        if (!this.hashtable_4.Contains(RoomItem_0.uint_0))
                        {
                            this.hashtable_4.Add(RoomItem_0.uint_0, RoomItem_0);
                        }
                    }
                }
                ServerMessage Message5_ = new ServerMessage(83u);
                RoomItem_0.method_6(Message5_);
                this.SendMessage(Message5_, null);
            }
            else
            {
                if (!this.hashtable_2.Contains(RoomItem_0.uint_0))
                {
                    this.hashtable_2.Add(RoomItem_0.uint_0, RoomItem_0);
                }
            }
            if (!bool_13)
            {
                RoomItem_0.string_7 = string_10;
                ServerMessage Message5_ = new ServerMessage(85u);
                RoomItem_0.method_6(Message5_);
                this.SendMessage(Message5_, null);
            }
            return true;
        }
        public void method_83()
        {
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null)
                {
                    this.method_87(@class, true, true);
                }
            }
        }
        public double method_84(int int_17, int int_18, List<RoomItem> list_18)
        {
            double result;
            try
            {
                bool flag = false;
                if (this.double_2[int_17, int_18] != 0.0)
                {
                    flag = true;
                }
                double num = 0.0;
                bool flag2 = false;
                double num2 = 0.0;
                if (list_18 == null)
                {
                    list_18 = new List<RoomItem>();
                }
                if (list_18 != null)
                {
                    foreach (RoomItem current in list_18)
                    {
                        if ((current.GetBaseItem().IsSeat || current.GetBaseItem().InteractionType.ToLower() == "bed") && flag)
                        {
                            result = current.Double_0;
                            return result;
                        }
                        if (current.Double_1 > num)
                        {
                            if (current.GetBaseItem().IsSeat || current.GetBaseItem().InteractionType.ToLower() == "bed")
                            {
                                if (flag)
                                {
                                    result = current.Double_0;
                                    return result;
                                }
                                flag2 = true;
                                num2 = current.GetBaseItem().Height;
                            }
                            else
                            {
                                flag2 = false;
                            }
                            num = current.Double_1;
                        }
                    }
                }
                double num3 = this.RoomModel.double_1[int_17, int_18];
                double num4 = num - this.RoomModel.double_1[int_17, int_18];
                if (flag2)
                {
                    num4 -= num2;
                }
                if (num4 < 0.0)
                {
                    num4 = 0.0;
                }
                result = num3 + num4;
            }
            catch
            {
                result = 0.0;
            }
            return result;
        }
        public void method_85(RoomUser RoomUser_1)
        {
            List<RoomItem> list = this.method_93(RoomUser_1.int_3, RoomUser_1.int_4);
            foreach (RoomItem current in list)
            {
                this.method_12(RoomUser_1, current);
                if (current.GetBaseItem().InteractionType.ToLower() == "pressure_pad")
                {
                    current.ExtraData = "0";
                    current.UpdateState(false, true);
                }
            }
            this.byte_0[RoomUser_1.int_3, RoomUser_1.int_4] = 1;
        }
        public void method_86(RoomUser RoomUser_1)
        {
            List<RoomItem> list = this.method_93(RoomUser_1.int_3, RoomUser_1.int_4);
            foreach (RoomItem current in list)
            {
                string text = current.GetBaseItem().InteractionType.ToLower();
                if (text != null)
                {
                    if (!(text == "pressure_pad"))
                    {
                        if (text == "fbgate" && (!string.IsNullOrEmpty(current.string_2) || !string.IsNullOrEmpty(current.string_3)))
                        {
                            RoomUser_1 = this.method_43(current.GStruct1_0.x, current.GStruct1_0.y);
                            if (RoomUser_1 != null && !RoomUser_1.IsBot && current.string_2 != null && current.string_3 != null)
                            {
                                string a = RoomUser_1.GetClient().GetHabbo().Gender;
                                if (a == "m")
                                {
                                    AntiMutant.smethod_1(RoomUser_1, current.string_2);
                                }
                                else
                                {
                                    AntiMutant.smethod_1(RoomUser_1, current.string_3);
                                }
                                ServerMessage Message = new ServerMessage(266u);
                                Message.AppendInt32(RoomUser_1.VirtualId);
                                Message.AppendStringWithBreak(RoomUser_1.GetClient().GetHabbo().Figure);
                                Message.AppendStringWithBreak(RoomUser_1.GetClient().GetHabbo().Gender.ToLower());
                                Message.AppendStringWithBreak(RoomUser_1.GetClient().GetHabbo().Motto);
                                Message.AppendInt32(RoomUser_1.GetClient().GetHabbo().AchievementScore);
                                Message.AppendStringWithBreak("");
                                this.SendMessage(Message, null);
                            }
                        }
                    }
                    else
                    {
                        current.ExtraData = "1";
                        current.UpdateState(false, true);
                    }
                }
            }
        }
        public void method_87(RoomUser User, bool bool_13, bool bool_14)
        {
            int num = 0;
            try
            {
                if (User != null)
                {
                    num = 1;
                    if (User.IsPet)
                    {
                        User.PetData.X = User.int_3;
                        User.PetData.Y = User.int_4;
                        User.PetData.Z = User.double_0;
                    }
                    else
                    {
                        if (User.IsBot)
                        {
                            User.RoomBot.x = User.int_3;
                            User.RoomBot.y = User.int_4;
                            User.RoomBot.z = User.double_0;
                        }
                        else
                        {
                            if (User.class34_1 != null && User.RoomUser_0 != null)
                            {
                                return;
                            }
                        }
                    }
                    num = 2;
                    if (!User.bool_12)
                    {
                        User.UpdateNeeded = false;
                    }
                    else
                    {
                        num = 3;
                        if (bool_13)
                        {
                            num = 4;
                            if (User.byte_1 > 0)
                            {
                                num = 5;
                                if (User.IsBot)
                                {
                                    if (this.byte_1[User.int_3, User.int_4] == 0)
                                    {
                                        User.RoomBot.EffectId = -1;
                                        User.byte_1 = 0;
                                    }
                                }
                                else
                                {
                                    num = 6;
                                    if ((User.GetClient().GetHabbo().Gender.ToLower() == "m" && this.byte_1[User.int_3, User.int_4] == 0) || (User.GetClient().GetHabbo().Gender.ToLower() == "f" && this.byte_2[User.int_3, User.int_4] == 0))
                                    {
                                        User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                        User.byte_1 = 0;
                                    }
                                }
                            }
                            num = 7;
                            if (User.Statusses.ContainsKey("lay") || User.Statusses.ContainsKey("sit"))
                            {
                                User.Statusses.Remove("lay");
                                User.Statusses.Remove("sit");
                                User.UpdateNeeded = true;
                            }
                            List<RoomItem> list = this.method_93(User.int_3, User.int_4);
                            double num2 = this.method_84(User.int_3, User.int_4, list);
                            if (num2 != User.double_0)
                            {
                                User.double_0 = num2;
                                User.UpdateNeeded = true;
                            }
                            num = 8;
                            if (this.RoomModel.squareState[User.int_3, User.int_4] == SquareState.SEAT)
                            {
                                if (!User.Statusses.ContainsKey("sit"))
                                {
                                    User.Statusses.Add("sit", "1.0");
                                    if (User.byte_1 > 0)
                                    {
                                        if (!User.IsBot)
                                        {
                                            User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                        }
                                        else
                                        {
                                            User.RoomBot.EffectId = -1;
                                        }
                                        User.byte_1 = 0;
                                    }
                                }
                                num = 9;
                                User.double_0 = this.RoomModel.double_1[User.int_3, User.int_4];
                                User.int_7 = this.RoomModel.int_3[User.int_3, User.int_4];
                                User.int_8 = this.RoomModel.int_3[User.int_3, User.int_4];
                                if (User.IsBot && User.RoomBot.RoomUser_0 != null)
                                {
                                    User.RoomBot.RoomUser_0.double_0 = User.double_0 + 1.0;
                                    User.RoomBot.RoomUser_0.int_7 = User.int_7;
                                    User.RoomBot.RoomUser_0.int_8 = User.int_8;
                                }
                                User.UpdateNeeded = true;
                            }
                            if (list.Count < 1 && this.list_4.Contains(User.UId))
                            {
                                User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, false);
                                this.list_4.Remove(User.UId);
                                User.int_14 = 0;
                                User.UpdateNeeded = true;
                            }
                            num = 10;
                            lock (list)
                            {
                                foreach (RoomItem Item in list)
                                {
                                    num = 11;
                                    if (Item.GetBaseItem().IsSeat && (!User.IsPet || User.RoomBot.RoomUser_0 == null))
                                    {
                                        if (!User.Statusses.ContainsKey("sit"))
                                        {
                                            double num3;
                                            try
                                            {
                                                if (Item.GetBaseItem().Height_Adjustable.Count > 1)
                                                {
                                                    num3 = Item.GetBaseItem().Height_Adjustable[(int)Convert.ToInt16(Item.ExtraData)];
                                                }
                                                else
                                                {
                                                    num3 = Item.GetBaseItem().Height;
                                                }
                                                goto IL_BCA;
                                            }
                                            catch
                                            {
                                                num3 = Item.GetBaseItem().Height;
                                                goto IL_BCA;
                                            }
                                        IL_51B:
                                            if (User.byte_1 > 0)
                                            {
                                                if (!User.IsBot)
                                                {
                                                    User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                                }
                                                else
                                                {
                                                    User.RoomBot.EffectId = -1;
                                                }
                                                User.byte_1 = 0;
                                                goto IL_55D;
                                            }
                                            goto IL_55D;
                                        IL_BCA:
                                            if (User.Statusses.ContainsKey("sit"))
                                            {
                                                goto IL_51B;
                                            }
                                            User.Statusses.Add("sit", num3.ToString().Replace(',', '.'));
                                            goto IL_51B;
                                        }
                                    IL_55D:
                                        User.double_0 = Item.Double_0;
                                        User.int_7 = Item.int_3;
                                        User.int_8 = Item.int_3;
                                        if (User.IsBot && User.RoomBot.RoomUser_0 != null)
                                        {
                                            User.RoomBot.RoomUser_0.double_0 = User.double_0 + 1.0;
                                            User.RoomBot.RoomUser_0.int_7 = User.int_7;
                                            User.RoomBot.RoomUser_0.int_8 = User.int_8;
                                        }
                                        User.UpdateNeeded = true;
                                    }
                                    num = 12;
                                    if (Item.GetBaseItem().InteractionType.ToLower() == "bed")
                                    {
                                        if (!User.Statusses.ContainsKey("lay"))
                                        {
                                            double num3;
                                            try
                                            {
                                                if (Item.GetBaseItem().Height_Adjustable.Count > 1)
                                                {
                                                    num3 = Item.GetBaseItem().Height_Adjustable[(int)Convert.ToInt16(Item.ExtraData)];
                                                }
                                                else
                                                {
                                                    num3 = Item.GetBaseItem().Height;
                                                }
                                            }
                                            catch
                                            {
                                                num3 = Item.GetBaseItem().Height;
                                            }
                                            if (!User.Statusses.ContainsKey("lay"))
                                            {
                                                User.Statusses.Add("lay", Item.GetBaseItem().Height.ToString().Replace(',', '.') + " null");
                                            }
                                            if (User.byte_1 > 0)
                                            {
                                                if (!User.IsBot)
                                                {
                                                    User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(-1, true);
                                                }
                                                else
                                                {
                                                    User.RoomBot.EffectId = -1;
                                                }
                                                User.byte_1 = 0;
                                            }
                                        }
                                        User.double_0 = Item.Double_0;
                                        User.int_7 = Item.int_3;
                                        User.int_8 = Item.int_3;
                                        if (User.IsBot && User.RoomBot.RoomUser_0 != null)
                                        {
                                            User.RoomBot.RoomUser_0.double_0 = User.double_0 + 1.0;
                                            User.RoomBot.RoomUser_0.int_7 = User.int_7;
                                            User.RoomBot.RoomUser_0.int_8 = User.int_8;
                                        }
                                        User.UpdateNeeded = true;
                                    }

                                    if (!User.IsBot && !User.IsPet)
                                    {
                                        if (User.team != Team.None)
                                        {
                                            if (Item.method_8().frzTimer == true)
                                            {
                                                if (Item.GetBaseItem().Name == "es_box")
                                                {
                                                    if (Item.freezePowerUp != FreezePowerUp.None)
                                                    {
                                                        this.GetFreeze().PickUpPowerUp(Item, User);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    TeamManager roomTeamManager;
                                    roomTeamManager = Item.method_8().GetRoomTeamManager();

                                    if (!User.IsBot && !User.IsPet && (Item.GetBaseItem().InteractionType.ToLower() == "freeze_blue_gate" || Item.GetBaseItem().InteractionType.ToLower() == "freeze_red_gate" || Item.GetBaseItem().InteractionType.ToLower() == "freeze_green_gate" || Item.GetBaseItem().InteractionType.ToLower() == "freeze_yellow_gate"))
                                    {
                                        if (roomTeamManager.CanEnterOnTeam(Item.team))
                                        {
                                            if (User.team == Team.None)
                                            {
                                                User.game = Rooms.Games.Game.Freeze;
                                                User.team = Item.team;
                                                roomTeamManager.AddUser(User);
                                                int FreezeEffect = ((int)Item.team) + 39;
                                                if (User.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 != FreezeEffect)
                                                {
                                                    User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(FreezeEffect, true);
                                                }
                                            }
                                            else
                                            {
                                                roomTeamManager.OnUserLeave(User);
                                                User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                            }
                                        }
                                    }

                                    /*if (!User.IsBot && (Item.GetBaseItem().InteractionType.ToLower() == "bb_blue_gate" || Item.GetBaseItem().InteractionType.ToLower() == "bb_red_gate" || Item.GetBaseItem().InteractionType.ToLower() == "bb_green_gate" || Item.GetBaseItem().InteractionType.ToLower() == "bb_yellow_gate"))
                                    {
                                        if (teamManagerForBanzai.CanEnterOnTeam(Item.team))
                                        {
                                            if (User.team != Team.None)
                                            {
                                                teamManagerForBanzai.OnUserLeave(User);
                                                teamManagerForFreeze.OnUserLeave(User);
                                            }
                                            User.team = Item.team;
                                            teamManagerForBanzai.AddUser(User);
                                            int BanzaiEffect = ((int)Item.team) + 32;
                                            if (User.GetClient().GetHabbo().method_24().int_0 != BanzaiEffect)
                                            {
                                                User.GetClient().GetHabbo().method_24().method_2(BanzaiEffect, true);
                                            }
                                        }
                                    }*/

                                    /*if (Item.GetBaseItem().Name == "es_gate_r" && Item.method_8().frzTimer == false)
                                    {
                                        #region es_gate_r
                                        User.GetClient().GetHabbo().method_24().method_2(40, true);
                                        User.GetClient().GetHabbo().frzTeam = 1;
                                        User.GetClient().GetHabbo().frzRange = 3;
                                        User.GetClient().GetHabbo().frzLives = 3;
                                        User.GetClient().GetHabbo().frzBalls = 1;
                                        User.GetClient().GetHabbo().frzBall = 0;
                                        #endregion
                                    }

                                    if (Item.GetBaseItem().Name == "es_gate_g" && Item.method_8().frzTimer == false)
                                    {
                                        #region es_gate_g
                                        User.GetClient().GetHabbo().method_24().method_2(41, true);
                                        User.GetClient().GetHabbo().frzTeam = 2;
                                        User.GetClient().GetHabbo().frzRange = 3;
                                        User.GetClient().GetHabbo().frzLives = 3;
                                        User.GetClient().GetHabbo().frzBalls = 1;
                                        User.GetClient().GetHabbo().frzBall = 0;
                                        #endregion
                                    }

                                    if (Item.GetBaseItem().Name == "es_gate_b" && Item.method_8().frzTimer == false)
                                    {
                                        #region es_gate_b
                                        User.GetClient().GetHabbo().method_24().method_2(42, true);
                                        User.GetClient().GetHabbo().frzTeam = 3;
                                        User.GetClient().GetHabbo().frzRange = 3;
                                        User.GetClient().GetHabbo().frzLives = 3;
                                        User.GetClient().GetHabbo().frzBalls = 1;
                                        User.GetClient().GetHabbo().frzBall = 0;
                                        #endregion
                                    }

                                    if (Item.GetBaseItem().Name == "es_gate_y" && Item.method_8().frzTimer == false)
                                    {
                                        #region es_gate_y
                                        User.GetClient().GetHabbo().method_24().method_2(43, true);
                                        User.GetClient().GetHabbo().frzTeam = 4;
                                        User.GetClient().GetHabbo().frzRange = 3;
                                        User.GetClient().GetHabbo().frzLives = 3;
                                        User.GetClient().GetHabbo().frzBalls = 1;
                                        User.GetClient().GetHabbo().frzBall = 0;
                                        #endregion
                                    }*/

                                    num = 13;
                                    if (Item.GetBaseItem().InteractionType.ToLower().IndexOf("bb_") > -1 && !User.IsBot && !User.IsPet)
                                    {
                                        if (Item.GetBaseItem().InteractionType.ToLower().IndexOf("_gate") > -1)
                                        {
                                            if (Item.GetBaseItem().InteractionType.ToLower() == "bb_yellow_gate" || Item.GetBaseItem().InteractionType.ToLower() == "bb_red_gate" || Item.GetBaseItem().InteractionType.ToLower() == "bb_green_gate" || Item.GetBaseItem().InteractionType.ToLower() == "bb_blue_gate")
                                            {
                                                if (roomTeamManager.CanEnterOnTeam(Item.team))
                                                {
                                                    if (User.team == Team.None)
                                                    {
                                                        User.game = Rooms.Games.Game.BattleBanzai;
                                                        User.team = Item.team;
                                                        roomTeamManager.AddUser(User);
                                                        int FreezeEffect = ((int)Item.team) + 32;
                                                        if (User.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 != FreezeEffect)
                                                        {
                                                            User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(FreezeEffect, true);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        roomTeamManager.OnUserLeave(User);
                                                        User.GetClient().GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
                                                    }
                                                }
                                            }
                                        }

                                        if (Item.GetBaseItem().InteractionType.ToLower() == "bb_teleport")
                                        {
                                            this.method_91(Item, User);
                                        }
                                        if (Item.GetBaseItem().InteractionType.ToLower() == "bb_patch" && this.GetTopItem(Item.Int32_0, Item.Int32_1) == Item && User.team != Team.None && User.game == Rooms.Games.Game.BattleBanzai && User.bool_6 && Item.ExtraData != "14" && Item.ExtraData != "5" && Item.ExtraData != "8" && Item.ExtraData != "11" && Item.ExtraData != "1")
                                        {
                                            if (Item.ExtraData == "0" || Item.ExtraData == "")
                                            {
                                                if (User.team == Team.Yellow)
                                                {
                                                    Item.ExtraData = Convert.ToString(12);
                                                }
                                                else if (User.team == Team.Red)
                                                {
                                                    Item.ExtraData = Convert.ToString(3);
                                                }
                                                else if (User.team == Team.Green)
                                                {
                                                    Item.ExtraData = Convert.ToString(6);
                                                }
                                                else if (User.team == Team.Blue)
                                                {
                                                    Item.ExtraData = Convert.ToString(9);
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(Item.ExtraData) > 0)
                                                {
                                                    if (User.team == Team.Yellow && (Item.ExtraData == "12" || Item.ExtraData == "13"))
                                                    {
                                                        Item.ExtraData = Convert.ToString(Convert.ToInt32(Item.ExtraData) + 1);
                                                    }
                                                    else
                                                    {
                                                        if (User.team == Team.Red && (Item.ExtraData == "3" || Item.ExtraData == "4"))
                                                        {
                                                            Item.ExtraData = Convert.ToString(Convert.ToInt32(Item.ExtraData) + 1);
                                                        }
                                                        else
                                                        {
                                                            if (User.team == Team.Green && (Item.ExtraData == "6" || Item.ExtraData == "7"))
                                                            {
                                                                Item.ExtraData = Convert.ToString(Convert.ToInt32(Item.ExtraData) + 1);
                                                            }
                                                            else
                                                            {
                                                                if (User.team == Team.Blue && (Item.ExtraData == "9" || Item.ExtraData == "10"))
                                                                {
                                                                    Item.ExtraData = Convert.ToString(Convert.ToInt32(Item.ExtraData) + 1);
                                                                }
                                                                else
                                                                {
                                                                    if (User.team == Team.Yellow)
                                                                    {
                                                                        Item.ExtraData = Convert.ToString(12);
                                                                    }
                                                                    else if (User.team == Team.Red)
                                                                    {
                                                                        Item.ExtraData = Convert.ToString(3);
                                                                    }
                                                                    else if (User.team == Team.Green)
                                                                    {
                                                                        Item.ExtraData = Convert.ToString(6);
                                                                    }
                                                                    else if (User.team == Team.Blue)
                                                                    {
                                                                        Item.ExtraData = Convert.ToString(9);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            if (User.team == Team.Yellow && Item.ExtraData == "14")
                                            {
                                                User.GetClient().GetHabbo().TilesLocked++;
                                                User.GetClient().GetHabbo().CheckBattleBanzaiTilesLockedAchievements();
                                            }

                                            if (User.team == Team.Red && Item.ExtraData == "5")
                                            {
                                                User.GetClient().GetHabbo().TilesLocked++;
                                                User.GetClient().GetHabbo().CheckBattleBanzaiTilesLockedAchievements();
                                            }

                                            if (User.team == Team.Green && Item.ExtraData == "8")
                                            {
                                                User.GetClient().GetHabbo().TilesLocked++;
                                                User.GetClient().GetHabbo().CheckBattleBanzaiTilesLockedAchievements();
                                            }

                                            if (User.team == Team.Blue && Item.ExtraData == "11")
                                            {
                                                User.GetClient().GetHabbo().TilesLocked++;
                                                User.GetClient().GetHabbo().CheckBattleBanzaiTilesLockedAchievements();
                                            }

                                            int team = 0;
                                            if (User.team == Team.Yellow)
                                            {
                                                team = 14;
                                            }
                                            else if (User.team == Team.Red)
                                            {
                                                team = 5;
                                            }
                                            else if (User.team == Team.Green)
                                            {
                                                team = 8;
                                            }
                                            else if (User.team == Team.Blue)
                                            {
                                                team = 11;
                                            }

                                            this.method_89(team, Item, false);
                                            Item.UpdateState(true, true);
                                        }
                                    }
                                }
                                goto IL_1155;
                            }
                        }
                        num = 14;
                        List<RoomItem> list2 = this.method_93(User.int_12, User.int_13);
                        lock (list2)
                        {
                            foreach (RoomItem current in list2)
                            {
                                if (this.double_0[current.Int32_0, current.Int32_1] <= current.Double_0)
                                {
                                    if (bool_14)
                                    {
                                        this.method_11(User, current);
                                    }
                                    if (current.GetBaseItem().InteractionType.ToLower() == "pressure_pad")
                                    {
                                        current.ExtraData = "1";
                                        current.UpdateState(false, true);
                                    }
                                    num = 15;
                                    if (current.GetBaseItem().InteractionType.ToLower() == "fbgate" && (!string.IsNullOrEmpty(current.string_2) || !string.IsNullOrEmpty(current.string_3)) && User != null && !User.IsBot)
                                    {
                                        if (User.string_0 != "")
                                        {
                                            User.GetClient().GetHabbo().Figure = User.string_0;
                                            User.string_0 = "";
                                            ServerMessage Message = new ServerMessage(266u);
                                            Message.AppendInt32(User.VirtualId);
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Figure);
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Gender.ToLower());
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Motto);
                                            Message.AppendInt32(User.GetClient().GetHabbo().AchievementScore);
                                            Message.AppendStringWithBreak("");
                                            this.SendMessage(Message, null);
                                        }
                                        else
                                        {
                                            string a = User.GetClient().GetHabbo().Gender;
                                            User.string_0 = User.GetClient().GetHabbo().Figure;
                                            if (a == "m")
                                            {
                                                AntiMutant.smethod_1(User, current.string_2);
                                            }
                                            else
                                            {
                                                AntiMutant.smethod_1(User, current.string_3);
                                            }
                                            ServerMessage Message = new ServerMessage(266u);
                                            Message.AppendInt32(User.VirtualId);
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Figure);
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Gender.ToLower());
                                            Message.AppendStringWithBreak(User.GetClient().GetHabbo().Motto);
                                            Message.AppendInt32(User.GetClient().GetHabbo().AchievementScore);
                                            Message.AppendStringWithBreak("");
                                            this.SendMessage(Message, null);
                                        }
                                    }
                                    num = 16;
                                    if (current.GetBaseItem().InteractionType.ToLower() == "ball")
                                    {
                                        int num6 = current.Int32_0;
                                        int num7 = current.Int32_1;
                                        current.ExtraData = "11";
                                        if (User != null && User.GetClient() != null && User.GetClient().GetHabbo() != null)
                                        {
                                            current.LastPlayerHitFootball = User.GetClient().GetHabbo().Username;
                                        }

                                        if (User.int_8 == 4)
                                        {
                                            num7++;
                                            if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                            {
                                                this.method_79(null, current, num6, num7 - 2, 0, false, true, true);
                                            }
                                        }
                                        else
                                        {
                                            if (User.int_8 == 0)
                                            {
                                                num7--;
                                                if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                {
                                                    this.method_79(null, current, num6, num7 + 2, 0, false, true, true);
                                                }
                                            }
                                            else
                                            {
                                                if (User.int_8 == 6)
                                                {
                                                    num6--;
                                                    if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                    {
                                                        this.method_79(null, current, num6 + 2, num7, 0, false, true, true);
                                                    }
                                                }
                                                else
                                                {
                                                    if (User.int_8 == 2)
                                                    {
                                                        num6++;
                                                        if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                        {
                                                            this.method_79(null, current, num6 - 2, num7, 0, false, true, true);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (User.int_8 == 3)
                                                        {
                                                            num6++;
                                                            num7++;
                                                            if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                            {
                                                                this.method_79(null, current, num6 - 2, num7 - 2, 0, false, true, true);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (User.int_8 == 1)
                                                            {
                                                                num6++;
                                                                num7--;
                                                                if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                                {
                                                                    this.method_79(null, current, num6 - 2, num7 + 2, 0, false, true, true);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (User.int_8 == 7)
                                                                {
                                                                    num6--;
                                                                    num7--;
                                                                    if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                                    {
                                                                        this.method_79(null, current, num6 + 2, num7 + 2, 0, false, true, true);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (User.int_8 == 5)
                                                                    {
                                                                        num6--;
                                                                        num7++;
                                                                        if (!this.method_79(null, current, num6, num7, 0, false, true, false))
                                                                        {
                                                                            this.method_79(null, current, num6 + 2, num7 - 2, 0, false, true, true);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    IL_1155: ;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogThreadException(ex.ToString(), string.Concat(new object[]
				{
					"Room [ID: ",
					this.Id,
					"] [Part: ",
					num,
					"] Update User Status"
				}));
                this.method_34();
            }
        }
        public void method_88(int int_17, int int_18, RoomItem RoomItem_0)
        {
            if (int_17 == 5)
            {
                this.int_9 += int_18 - 1;
            }
            else
            {
                if (int_17 == 8)
                {
                    this.int_12 += int_18 - 1;
                }
                else
                {
                    if (int_17 == 11)
                    {
                        this.int_11 += int_18 - 1;
                    }
                    else
                    {
                        if (int_17 == 14)
                        {
                            this.int_10 += int_18 - 1;
                        }
                    }
                }
            }
            this.method_89(int_17, RoomItem_0, false);
        }
        public void method_89(int int_17, RoomItem RoomItem_0, bool bool_13)
        {
            if (int_17 == 5)
            {
                this.int_9++;
                if (RoomItem_0.ExtraData == "5")
                {
                    this.list_6.Add(RoomItem_0);
                }
                if (this.list_10.Count > 0)
                {
                    foreach (RoomItem current in this.list_10)
                    {
                        current.ExtraData = Convert.ToString(this.int_9);
                        current.UpdateState(true, true);
                    }
                }
                this.method_17(this.int_9);
            }
            else
            {
                if (int_17 == 8)
                {
                    this.int_12++;
                    if (RoomItem_0.ExtraData == "8")
                    {
                        this.list_7.Add(RoomItem_0);
                    }
                    if (this.list_13.Count > 0)
                    {
                        foreach (RoomItem current in this.list_13)
                        {
                            current.ExtraData = Convert.ToString(this.int_12);
                            current.UpdateState(true, true);
                        }
                    }
                    this.method_17(this.int_12);
                }
                else
                {
                    if (int_17 == 11)
                    {
                        this.int_11++;
                        if (RoomItem_0.ExtraData == "11")
                        {
                            this.list_9.Add(RoomItem_0);
                        }
                        if (this.list_12.Count > 0)
                        {
                            foreach (RoomItem current in this.list_12)
                            {
                                current.ExtraData = Convert.ToString(this.int_11);
                                current.UpdateState(true, true);
                            }
                        }
                        this.method_17(this.int_11);
                    }
                    else
                    {
                        if (int_17 == 14)
                        {
                            this.int_10++;
                            if (RoomItem_0.ExtraData == "14")
                            {
                                this.list_8.Add(RoomItem_0);
                            }
                            if (this.list_11.Count > 0)
                            {
                                foreach (RoomItem current in this.list_11)
                                {
                                    current.ExtraData = Convert.ToString(this.int_10);
                                    current.UpdateState(true, true);
                                }
                            }
                            this.method_17(this.int_10);
                        }
                    }
                }
            }
            if (bool_13 || (this.list_5.Count > 0 && this.list_6.Count + this.list_7.Count + this.list_9.Count + this.list_8.Count >= this.list_5.Count))
            {
                bool_13 = true;
                if (this.int_10 > this.int_9 && this.int_10 > this.int_11 && this.int_10 > this.int_12)
                {
                    new Room.Delegate2(this.method_90).BeginInvoke(14, null, null);
                }
                else
                {
                    if (this.int_9 > this.int_10 && this.int_9 > this.int_11 && this.int_9 > this.int_12)
                    {
                        new Room.Delegate2(this.method_90).BeginInvoke(5, null, null);
                    }
                    else
                    {
                        if (this.int_11 > this.int_9 && this.int_11 > this.int_10 && this.int_11 > this.int_12)
                        {
                            new Room.Delegate2(this.method_90).BeginInvoke(11, null, null);
                        }
                        else
                        {
                            if (this.int_12 > this.int_9 && this.int_12 > this.int_11 && this.int_12 > this.int_10)
                            {
                                new Room.Delegate2(this.method_90).BeginInvoke(8, null, null);
                            }
                        }
                    }
                }
            }
            if (bool_13)
            {
                this.method_13();
            }
        }
        public void method_90(int int_17)
        {
            List<RoomItem> list = new List<RoomItem>();
            if (int_17 == 5)
            {
                list = this.list_6;
            }
            else
            {
                if (int_17 == 8)
                {
                    list = this.list_7;
                }
                else
                {
                    if (int_17 == 11)
                    {
                        list = this.list_9;
                    }
                    else
                    {
                        if (int_17 == 14)
                        {
                            list = this.list_8;
                        }
                    }
                }
            }
            try
            {
                for (int i = 4; i > 0; i--)
                {
                    Thread.Sleep(500);
                    foreach (RoomItem current in list)
                    {
                        current.ExtraData = "1";
                        current.UpdateState(false, true);
                    }
                    Thread.Sleep(500);
                    foreach (RoomItem current in list)
                    {
                        current.ExtraData = Convert.ToString(int_17);
                        current.UpdateState(false, true);
                    }
                }
                foreach (RoomItem current in this.list_5)
                {
                    current.ExtraData = "0";
                    current.UpdateState(true, true);
                }
            }
            catch
            {
            }
            this.list_9.Clear();
            this.list_7.Clear();
            this.list_6.Clear();
            this.list_8.Clear();
            this.int_10 = 0;
            this.int_11 = 0;
            this.int_9 = 0;
            this.int_12 = 0;
            foreach (RoomItem current in this.list_10)
            {
                current.ExtraData = "0";
                current.UpdateState(true, true);
            }
            foreach (RoomItem current in this.list_13)
            {
                current.ExtraData = "0";
                current.UpdateState(true, true);
            }
            foreach (RoomItem current in this.list_12)
            {
                current.ExtraData = "0";
                current.UpdateState(true, true);
            }
            foreach (RoomItem current in this.list_11)
            {
                current.ExtraData = "0";
                current.UpdateState(true, true);
            }
        }
        public void method_91(RoomItem RoomItem_0, RoomUser RoomUser_1)
        {
            RoomItem_0.ExtraData = "1";
            RoomItem_0.UpdateState(false, true);
            RoomItem_0.ReqUpdate(1);
            List<RoomItem> list = new List<RoomItem>();
            RoomUser_1.method_3(true);
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                if (@class != RoomItem_0 && !(@class.GetBaseItem().InteractionType.ToLower() != "bb_teleport"))
                {
                    list.Add(@class);
                }
            }
            if (list.Count > 0)
            {
                Random random = new Random((int)GoldTree.GetUnixTimestamp() * (int)RoomUser_1.UId);
                int index = random.Next(0, list.Count);
                list[index].ExtraData = "1";
                list[index].UpdateState(false, true);
                list[index].ReqUpdate(1);
                this.byte_0[RoomUser_1.int_3, RoomUser_1.int_4] = 1;
                this.byte_0[list[index].Int32_0, list[index].Int32_1] = 1;
                RoomUser_1.method_7(list[index].Int32_0, list[index].Int32_1, list[index].Double_0);
                RoomUser_1.UpdateNeeded = true;
            }
        }
        public bool method_92(int int_17, int int_18)
        {
            return int_17 >= 0 && int_18 >= 0 && int_17 < this.RoomModel.int_4 && int_18 < this.RoomModel.int_5;
        }
        public List<RoomItem> method_93(int int_17, int int_18)
        {
            List<RoomItem> list = new List<RoomItem>();
            List<RoomItem> result;
            if (this.Hashtable_0 != null)
            {
                foreach (RoomItem @class in this.Hashtable_0.Values)
                {
                    if (@class.Int32_0 == int_17 && @class.Int32_1 == int_18)
                    {
                        list.Add(@class);
                    }
                    Dictionary<int, AffectedTile> dictionary = this.method_94(@class.GetBaseItem().Length, @class.GetBaseItem().Width, @class.Int32_0, @class.Int32_1, @class.int_3);
                    foreach (AffectedTile current in dictionary.Values)
                    {
                        if (current.Int32_0 == int_17 && current.Int32_1 == int_18)
                        {
                            list.Add(@class);
                        }
                    }
                }
                result = list;
            }
            else
            {
                result = null;
            }
            return result;
        }
        public Dictionary<int, AffectedTile> method_94(int int_17, int int_18, int int_19, int int_20, int int_21)
        {
            int num = 0;
            Dictionary<int, AffectedTile> dictionary = new Dictionary<int, AffectedTile>();
            if (int_17 > 1)
            {
                if (int_21 == 0 || int_21 == 4)
                {
                    for (int i = 1; i < int_17; i++)
                    {
                        dictionary.Add(num++, new AffectedTile(int_19, int_20 + i, i));
                        for (int j = 1; j < int_18; j++)
                        {
                            dictionary.Add(num++, new AffectedTile(int_19 + j, int_20 + i, (i < j) ? j : i));
                        }
                    }
                }
                else
                {
                    if (int_21 == 2 || int_21 == 6)
                    {
                        for (int i = 1; i < int_17; i++)
                        {
                            dictionary.Add(num++, new AffectedTile(int_19 + i, int_20, i));
                            for (int j = 1; j < int_18; j++)
                            {
                                dictionary.Add(num++, new AffectedTile(int_19 + i, int_20 + j, (i < j) ? j : i));
                            }
                        }
                    }
                }
            }
            if (int_18 > 1)
            {
                if (int_21 == 0 || int_21 == 4)
                {
                    for (int i = 1; i < int_18; i++)
                    {
                        dictionary.Add(num++, new AffectedTile(int_19 + i, int_20, i));
                        for (int j = 1; j < int_17; j++)
                        {
                            dictionary.Add(num++, new AffectedTile(int_19 + i, int_20 + j, (i < j) ? j : i));
                        }
                    }
                }
                else
                {
                    if (int_21 == 2 || int_21 == 6)
                    {
                        for (int i = 1; i < int_18; i++)
                        {
                            dictionary.Add(num++, new AffectedTile(int_19, int_20 + i, i));
                            for (int j = 1; j < int_17; j++)
                            {
                                dictionary.Add(num++, new AffectedTile(int_19 + j, int_20 + i, (i < j) ? j : i));
                            }
                        }
                    }
                }
            }
            return dictionary;
        }
        public bool method_95(int int_17, int int_18, bool bool_13)
        {
            return !this.AllowWalkthrough && this.method_96(int_17, int_18);
        }
        public bool method_96(int int_17, int int_18)
        {
            return this.method_43(int_17, int_18) != null;
        }
        public bool method_97(int int_17, int int_18)
        {
            return this.method_44(int_17, int_18) != null;
        }
        public string method_98(string string_10)
        {
            string result;
            try
            {
                if (string_10.Contains(Convert.ToChar(13)))
                {
                    result = null;
                }
                else
                {
                    if (string_10.Contains(Convert.ToChar(9)))
                    {
                        result = null;
                    }
                    else
                    {
                        string[] array = string_10.Split(new char[]
						{
							' '
						});
                        if (array[2] != "l" && array[2] != "r")
                        {
                            result = null;
                        }
                        else
                        {
                            string[] array2 = array[0].Substring(3).Split(new char[]
							{
								','
							});
                            int num = int.Parse(array2[0]);
                            int num2 = int.Parse(array2[1]);
                            if (num < 0 || num2 < 0 || num > 200 || num2 > 200)
                            {
                                result = null;
                            }
                            else
                            {
                                string[] array3 = array[1].Substring(2).Split(new char[]
								{
									','
								});
                                int num3 = int.Parse(array3[0]);
                                int num4 = int.Parse(array3[1]);
                                if (num3 < 0 || num4 < 0 || num3 > 200 || num4 > 200)
                                {
                                    result = null;
                                }
                                else
                                {
                                    result = string.Concat(new object[]
									{
										":w=",
										num,
										",",
										num2,
										" l=",
										num3,
										",",
										num4,
										" ",
										array[2]
									});
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }
        public bool method_99(int int_17, int int_18, int int_19, int int_20)
        {
            return (Math.Abs(int_17 - int_19) <= 1 && Math.Abs(int_18 - int_20) <= 1) || (int_17 == int_19 && int_18 == int_20);
        }
        public int method_100(int int_17, int int_18, int int_19, int int_20)
        {
            return Math.Abs(int_17 - int_19) + Math.Abs(int_18 - int_20);
        }
        internal void method_101()
        {
            for (int i = 0; i < this.RoomUsers.Length; i++)
            {
                RoomUser @class = this.RoomUsers[i];
                if (@class != null)
                {
                    @class.int_10 = @class.int_3;
                    @class.int_11 = @class.int_4;
                    @class.ClearStatuses();
                    @class.method_3(false);
                }
            }
        }
        internal void method_102(int int_17)
        {
            this.int_15 = int_17;
        }

        internal TeamManager GetRoomTeamManager()
        {
            if (this.TeamManager == null)
            {
                this.TeamManager = new TeamManager(this);
            }
            return this.TeamManager;
        }

        internal GameManager GetGameManager()
        {
            if (this.GameManager == null)
            {
                this.GameManager = new GameManager(this);
            }
            return this.GameManager;
        }

        internal Freeze GetFreeze()
        {
            if (this.freeze == null)
            {
                this.freeze = new Freeze(this);
            }
            return this.freeze;
        }

        private RoomItem GetTopItem(int X, int Y)
        {
            RoomItem Item = null;
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                if (@class != null)
                {
                    if (@class.Int32_0 == X && @class.Int32_1 == Y)
                    {
                        if (this.double_1[X, Y] == @class.Double_1)
                        {
                            Item = @class;
                        }
                    }
                }
            }
            return Item;
        }

        private double GetTopItemHeight(int X, int Y)
        {
            double Height = 0.0;
            foreach (RoomItem @class in this.Hashtable_0.Values)
            {
                if (@class.Int32_0 == X && @class.Int32_1 == Y)
                {
                    Height = @class.Double_1;
                }
            }
            return Height;
        }
    }
}