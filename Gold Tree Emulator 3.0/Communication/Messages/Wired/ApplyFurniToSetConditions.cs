using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
using GoldTree.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTree.Communication.Messages.Wired
{
    class ApplyFurniToSetConditions : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            RoomItem class2 = @class.method_28(Event.PopWiredUInt());
            if (@class != null && class2 != null)
            {
                string text = class2.GetBaseItem().InteractionType.ToLower();
                if (text != null)
                {
                    if (text != null && (text == "wf_act_matchfurni" || text == "wf_cnd_match_snapshot"))
                    {
                        class2.ExtraData = "1";
                        class2.UpdateState(false, true);
                        class2.ReqUpdate(1);
                        class2.method_9();
                        if (class2.string_4.Length > 0 && class2.string_2.Length > 0)
                        {
                            string[] collection = class2.string_4.Split(new char[]
									{
										','
									});
                            IEnumerable<string> enumerable = new List<string>(collection);
                            string[] collection2 = class2.string_2.Split(new char[]
									{
										';'
									});
                            List<string> list8 = new List<string>(collection2);
                            int num8 = 0;
                            foreach (string current in enumerable)
                            {
                                RoomItem class3 = @class.method_28(Convert.ToUInt32(current));
                                if (class3 != null && !(class3.GetBaseItem().InteractionType.ToLower() == "dice"))
                                {
                                    string[] collection3 = list8[num8].Split(new char[]
											{
												','
											});
                                    List<string> list9 = new List<string>(collection3);
                                    bool flag6 = false;
                                    bool flag7 = false;
                                    if (class2.string_3 != "" && class3 != null)
                                    {
                                        int int_ = class3.Int32_0;
                                        int int_2 = class3.Int32_1;
                                        if (class2.string_3.StartsWith("I"))
                                        {
                                            class3.ExtraData = list9[4];
                                            flag7 = true;
                                        }
                                        if (class2.string_3.Substring(1, 1) == "I")
                                        {
                                            class3.int_3 = Convert.ToInt32(list9[3]);
                                            flag6 = true;
                                        }
                                        if (class2.string_3.EndsWith("I"))
                                        {
                                            int_ = Convert.ToInt32(list9[0]);
                                            int_2 = Convert.ToInt32(list9[1]);
                                            flag6 = true;
                                        }
                                        if (flag6)
                                        {
                                            @class.method_40(class3, int_, int_2, class2.uint_0, class3.Double_0);
                                        }
                                        if (flag7)
                                        {
                                            class3.UpdateState(false, true);
                                        }
                                        @class.method_22();
                                    }
                                    num8++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}