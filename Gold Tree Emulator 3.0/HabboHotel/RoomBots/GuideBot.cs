using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.RoomBots
{
	internal sealed class GuideBot : BotAI
	{
		private int int_2;
		private int int_3;
		public GuideBot()
		{
			this.int_2 = 0;
			this.int_3 = 0;
		}
		public override void OnSelfEnterRoom()
		{
			base.GetRoomUser().HandleSpeech(null, GoldTreeEnvironment.GetExternalText("guidebot_welcome1"), true);
			base.GetRoomUser().HandleSpeech(null, GoldTreeEnvironment.GetExternalText("guidebot_welcome2"), false);
		}
		public override void OnSelfLeaveRoom(bool bool_0)
		{
		}
		public override void OnUserEnterRoom(RoomUser RoomUser_0)
		{
		}
		public override void OnUserLeaveRoom(GameClient Session)
		{
			if (base.method_1().Owner.ToLower() == Session.GetHabbo().Username.ToLower())
			{
				base.method_1().method_6(base.GetRoomUser().VirtualId, false);
			}
		}
		public override void OnUserSay(RoomUser RoomUser_0, string string_0)
		{
			if (base.method_1().method_100(base.GetRoomUser().int_3, base.GetRoomUser().int_4, RoomUser_0.int_3, RoomUser_0.int_4) <= 8)
			{
				BotResponse @class = base.method_3().method_2(string_0);
				if (@class != null)
				{
					string text = base.method_1().method_20(RoomUser_0, @class.Response);
					string text2 = @class.ResponseType.ToLower();
					if (text2 != null)
					{
						if (!(text2 == "say"))
						{
							if (!(text2 == "shout"))
							{
								if (text2 == "whisper")
								{
									ServerMessage Message = new ServerMessage(25u);
									Message.AppendInt32(base.GetRoomUser().VirtualId);
									Message.AppendStringWithBreak(text);
									Message.AppendBoolean(false);
									RoomUser_0.GetClient().SendMessage(Message);
								}
							}
							else
							{
								base.GetRoomUser().HandleSpeech(null, text, true);
							}
						}
						else
						{
							base.GetRoomUser().HandleSpeech(null, text, false);
						}
					}
					if (@class.ServeId >= 1)
					{
						RoomUser_0.CarryItem(@class.ServeId);
					}
				}
			}
		}
		public override void OnUserShout(RoomUser RoomUser_0, string string_0)
		{
		}
		public override void OnTimerTick()
		{
			if (this.int_2 <= 0)
			{
				if (base.method_3() != null && base.method_3().list_0.Count > 0)
				{
					RandomSpeech @class = base.method_3().method_3();
					base.GetRoomUser().HandleSpeech(null, @class.Message, @class.Shout);
				}
				this.int_2 = GoldTree.smethod_5(0, 150);
			}
			else
			{
				this.int_2--;
			}
			if (this.int_3 <= 0)
			{
				int int_ = GoldTree.smethod_5(0, base.method_1().RoomModel.int_4);
				int int_2 = GoldTree.smethod_5(0, base.method_1().RoomModel.int_5);
				base.GetRoomUser().MoveTo(int_, int_2);
				this.int_3 = GoldTree.smethod_5(0, 30);
			}
			else
			{
				this.int_3--;
			}
		}
	}
}
