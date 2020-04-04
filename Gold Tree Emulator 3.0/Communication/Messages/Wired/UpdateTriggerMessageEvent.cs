using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Communication.Messages.Wired
{
    internal sealed class UpdateTriggerMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null)
            {
            RoomItem class2 = @class.method_28(Event.PopWiredUInt());
            if (class2 != null)
            {
                string text = class2.GetBaseItem().InteractionType.ToLower();
                if (text != null)
                {
                    if (!(text == "wf_trg_onsay"))
                    {
                        if (!(text == "wf_trg_enterroom"))
                        {
                            if (!(text == "wf_trg_timer"))
                            {
                                if (!(text == "wf_trg_attime"))
                                {
                                    if (text == "wf_trg_atscore")
                                    {
                                        Event.PopWiredBoolean();
                                        string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
                                        string[] array = text2.Split(new char[]
										{
											'@'
										});
                                        class2.string_3 = array[0];
                                        class2.string_2 = Convert.ToString(Event.PopWiredInt32());
                                    }

                                    if (text != null && (text == "wf_cnd_time_more_than" || text == "wf_cnd_time_less_than"))
                                    {
                                        Event.PopWiredBoolean();
                                        string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
                                        string[] array = text2.Split(new char[]
								{
									'@'
								});
                                        class2.string_3 = array[0];
                                        class2.string_2 = Convert.ToString(Convert.ToString((double)Event.PopWiredInt32() * 0.5));
                                    }
                                }
                                else
                                {
                                    Event.PopWiredBoolean();
                                    string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
                                    string[] array = text2.Split(new char[]
									{
										'@'
									});
                                    class2.string_3 = array[0];
                                    class2.string_2 = Convert.ToString(Convert.ToString((double)Event.PopWiredInt32() * 0.5));
                                }
                            }
                            else
                            {
                                Event.PopWiredBoolean();
                                string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
                                string[] array = text2.Split(new char[]
								{
									'@'
								});
                                class2.string_3 = array[0];
                                class2.string_2 = Convert.ToString(Convert.ToString((double)Event.PopWiredInt32() * 0.5));
                            }
                        }
                        else
                        {
                            Event.PopWiredBoolean();
                            string text3 = Event.PopFixedString();
                            class2.string_2 = text3;
                        }
                    }
                    else
                    {
                        Event.PopWiredBoolean();
                        bool value = Event.PopWiredBoolean();
                        string text3 = Event.PopFixedString();
                        text3 = GoldTree.DoFilter(text3, false, true);
                        if (text3.Length > 100)
                        {
                            text3 = text3.Substring(0, 100);
                        }
                        class2.string_2 = text3;
                        class2.string_3 = Convert.ToString(value);
                    }
                }
                class2.UpdateState(true, false);
            }
        }
        }
    }
}
