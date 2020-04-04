using System;
using GoldTree.HabboHotel.Misc;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Wired
{
    internal sealed class UpdateActionMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            try
            {
                Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                uint uint_ = Event.PopWiredUInt();
                RoomItem class2 = @class.method_28(uint_);
                string text = class2.GetBaseItem().InteractionType.ToLower();
                switch (text)
                {
                    case "wf_act_give_phx":
                        {
                            Event.PopWiredBoolean();
                            string text2 = Event.PopFixedString();
                            text2 = GoldTree.DoFilter(text2, false, true);
                            text2 = ChatCommandHandler.smethod_4(text2);
                            if (!(text2 == class2.string_2))
                            {
                                string string_ = text2.Split(new char[]
						{
							':'
						})[0].ToLower();
                                if (GoldTree.GetGame().GetRoleManager().method_12(string_, Session))
                                {
                                    class2.string_2 = text2;
                                }
                                else
                                {
                                    Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("wired_error_permissions"));
                                }
                            }
                            break;
                        }
                    case "wf_cnd_phx":
                        {
                            Event.PopWiredBoolean();
                            string text2 = Event.PopFixedString();
                            text2 = GoldTree.DoFilter(text2, false, true);
                            text2 = ChatCommandHandler.smethod_4(text2);
                            if (!(text2 == class2.string_2))
                            {
                                string string_ = text2.Split(new char[]
						{
							':'
						})[0].ToLower();
                                if (GoldTree.GetGame().GetRoleManager().method_11(string_, Session))
                                {
                                    class2.string_2 = text2;
                                }
                                else
                                {
                                    Session.GetHabbo().Whisper(GoldTreeEnvironment.GetExternalText("wired_error_permissions"));
                                }
                            }
                            break;
                        }
                    case "wf_act_saymsg":
                        {
                            Event.PopWiredBoolean();
                            string text2 = Event.PopFixedString();
                            text2 = GoldTree.DoFilter(text2, false, true);
                            if (text2.Length > 100)
                            {
                                text2 = text2.Substring(0, 100);
                            }
                            class2.string_2 = text2;
                            break;
                        }
                    case "wf_act_kick_user":
                        {
                            Event.PopWiredBoolean();
                            string text2 = Event.PopFixedString();
                            text2 = GoldTree.DoFilter(text2, false, true);
                            if (text2.Length > 200)
                            {
                                text2 = text2.Substring(0, 200);
                            }
                            class2.string_2 = text2;
                            break;
                        }
                    case "wf_trg_furnistate":
                    case "wf_trg_onfurni":
                    case "wf_trg_offfurni":
                    case "wf_act_moveuser":
                    case "wf_act_togglefurni":
                        {
                            Event.PopWiredBoolean();
                            Event.PopFixedString();
                            class2.string_2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
                            class2.string_2 = class2.string_2.Substring(0, class2.string_2.Length - 2);
                            Event.ResetPointer();
                            class2 = @class.method_28(Event.PopWiredUInt());
                            Event.PopWiredBoolean();
                            Event.PopFixedString();
                            int num2 = Event.PopWiredInt32();
                            class2.string_3 = "";
                            for (int i = 0; i < num2; i++)
                            {
                                class2.string_3 = class2.string_3 + "," + Convert.ToString(Event.PopWiredUInt());
                            }
                            if (class2.string_3.Length > 0)
                            {
                                class2.string_3 = class2.string_3.Substring(1);
                            }
                            break;
                        }
                    case "wf_act_givepoints":
                        Event.PopWiredInt32();
                        class2.string_2 = Convert.ToString(Event.PopWiredInt32());
                        class2.string_3 = Convert.ToString(Event.PopWiredInt32());
                        break;
                    case "wf_act_moverotate":
                        {
                            Event.PopWiredInt32();
                            class2.string_2 = Convert.ToString(Event.PopWiredInt32());
                            class2.string_3 = Convert.ToString(Event.PopWiredInt32());
                            Event.PopFixedString();
                            int num2 = Event.PopWiredInt32();
                            class2.string_4 = "";
                            class2.string_5 = "";
                            if (num2 > 0)
                            {
                                class2.string_5 = OldEncoding.encodeVL64(num2);
                                for (int i = 0; i < num2; i++)
                                {
                                    int num3 = Event.PopWiredInt32();
                                    class2.string_5 += OldEncoding.encodeVL64(num3);
                                    class2.string_4 = class2.string_4 + "," + Convert.ToString(num3);
                                }
                                class2.string_4 = class2.string_4.Substring(1);
                            }
                            class2.string_6 = Convert.ToString(Event.PopWiredInt32());
                            break;
                        }
                    case "wf_act_matchfurni":
                        {
                            Event.PopWiredInt32();
                            class2.string_3 = "";
                            if (Event.PopWiredBoolean())
                            {
                                RoomItem expr_4A8 = class2;
                                expr_4A8.string_3 += "I";
                            }
                            else
                            {
                                RoomItem expr_4C0 = class2;
                                expr_4C0.string_3 += "H";
                            }
                            if (Event.PopWiredBoolean())
                            {
                                RoomItem expr_4E1 = class2;
                                expr_4E1.string_3 += "I";
                            }
                            else
                            {
                                RoomItem expr_4F9 = class2;
                                expr_4F9.string_3 += "H";
                            }
                            if (Event.PopWiredBoolean())
                            {
                                RoomItem expr_51A = class2;
                                expr_51A.string_3 += "I";
                            }
                            else
                            {
                                RoomItem expr_532 = class2;
                                expr_532.string_3 += "H";
                            }
                            Event.PopFixedString();
                            int num2 = Event.PopWiredInt32();
                            class2.string_2 = "";
                            class2.string_4 = "";
                            class2.string_5 = "";
                            if (num2 > 0)
                            {
                                class2.string_5 = OldEncoding.encodeVL64(num2);
                                for (int i = 0; i < num2; i++)
                                {
                                    int num3 = Event.PopWiredInt32();
                                    class2.string_5 += OldEncoding.encodeVL64(num3);
                                    class2.string_4 = class2.string_4 + "," + Convert.ToString(num3);
                                    RoomItem class3 = @class.method_28(Convert.ToUInt32(num3));
                                    RoomItem expr_5E6 = class2;
                                    object string_2 = expr_5E6.string_2;
                                    expr_5E6.string_2 = string.Concat(new object[]
							{
								string_2,
								";",
								class3.Int32_0,
								",",
								class3.Int32_1,
								",",
								class3.Double_0,
								",",
								class3.int_3,
								",",
								class3.ExtraData
							});
                                }
                                class2.string_4 = class2.string_4.Substring(1);
                                class2.string_2 = class2.string_2.Substring(1);
                            }
                            class2.string_6 = Convert.ToString(Event.PopWiredInt32());
                            break;
                        }
                }
                class2.UpdateState(true, false);
            }
            catch
            {
            }
        }
    }
}