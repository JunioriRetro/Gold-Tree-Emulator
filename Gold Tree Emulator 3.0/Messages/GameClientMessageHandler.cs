using System;
using System.Data;
using System.Text.RegularExpressions;
using GoldTree.Core;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Advertisements;
using GoldTree.HabboHotel.Items;
using GoldTree.Storage;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.Messages
{
	internal sealed class GameClientMessageHandler
	{
		private delegate void Delegate();
		private const int HIGHEST_MESSAGE_ID = 4004;
		private GameClient Session;
		private ClientMessage Request;
		private ServerMessage Response;
		private GameClientMessageHandler.Delegate[] RequestHandlers;
		public GameClientMessageHandler(GameClient Session)
		{
			this.Session = Session;
            this.RequestHandlers = new GameClientMessageHandler.Delegate[HIGHEST_MESSAGE_ID];
			this.Response = new ServerMessage(0);
		}
		public ServerMessage GetResponse()
		{
			return this.Response;
		}
		public void Destroy()
		{
			this.Session = null;
			this.RequestHandlers = null;
			this.Request = null;
			this.Response = null;
		}
		public void HandleRequest(ClientMessage Request)
		{
			uint arg_06_0 = Request.Id;
            if (Request.Id > HIGHEST_MESSAGE_ID)
			{
				Logging.WriteLine("Warning - out of protocol request: " + Request.Header);
			}
			else
			{
				if (this.RequestHandlers[(int)((UIntPtr)Request.Id)] != null && Request != null)
				{
					this.Request = Request;
					this.RequestHandlers[(int)((UIntPtr)Request.Id)]();
					this.Request = null;
				}
			}
		}
		public void method_3()
		{
			if (this.Response != null && this.Response.Id > 0u && this.Session.GetConnection() != null)
			{
				this.Session.GetConnection().SendMessage(this.Response);
			}
		}
		public void method_4()
		{
            if (this != null)
            {
                RoomAdvertisement @class = GoldTree.GetGame().GetAdvertisementManager().method_1();
                this.Response.Init(258);
                if (@class == null)
                {
                    this.Response.AppendStringWithBreak("");
                    this.Response.AppendStringWithBreak("");
                }
                else
                {
                    this.Response.AppendStringWithBreak(@class.string_0);
                    this.Response.AppendStringWithBreak(@class.string_1);
                    @class.method_0();
                }
                this.method_3();
            }
		}
		public void method_5(uint uint_0, string string_0)
		{
			this.method_7();
			if (GoldTree.GetGame().GetRoomManager().method_12(uint_0) != null)
			{
                if (this.Session != null && this.Session.GetHabbo() != null && this.Session.GetHabbo().InRoom)
				{
					Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().CurrentRoomId);
					if (@class != null)
					{
						@class.method_47(this.Session, false, false);
					}
				}
				Room class2 = GoldTree.GetGame().GetRoomManager().method_15(uint_0);
				if (class2 != null && Session != null && Session.GetHabbo() != null)
				{
					this.Session.GetHabbo().uint_2 = uint_0;
					if (class2.method_68(this.Session.GetHabbo().Id))
					{
						if (!class2.method_71(this.Session.GetHabbo().Id))
						{
							ServerMessage Message = new ServerMessage(224u);
							Message.AppendInt32(4);
							this.Session.SendMessage(Message);
							ServerMessage Message2 = new ServerMessage(18u);
							this.Session.SendMessage(Message2);
							return;
						}
						class2.method_69(this.Session.GetHabbo().Id);
					}
					if (class2.UsersNow >= class2.UsersMax && !GoldTree.GetGame().GetRoleManager().method_1(this.Session.GetHabbo().Rank, "acc_enter_fullrooms") && !this.Session.GetHabbo().IsVIP)
					{
						ServerMessage Message = new ServerMessage(224u);
						Message.AppendInt32(1);
						this.Session.SendMessage(Message);
						ServerMessage Message2 = new ServerMessage(18u);
						this.Session.SendMessage(Message2);
					}
					else
					{
						if (class2.Type == "public")
						{
							if (class2.State > 0 && !this.Session.GetHabbo().HasFuse("acc_restrictedrooms"))
							{
								this.Session.SendNotification("This public room is accessible to GoldTree staff only.");
								ServerMessage Message2 = new ServerMessage(18u);
								this.Session.SendMessage(Message2);
								return;
							}
							ServerMessage Message3 = new ServerMessage(166u);
							Message3.AppendStringWithBreak("/client/public/" + class2.ModelName + "/0");
							this.Session.SendMessage(Message3);
						}
						else
						{
							if (class2.Type == "private")
							{
								ServerMessage Logging = new ServerMessage(19u);
								this.Session.SendMessage(Logging);
								if (this.Session.GetHabbo().bool_7)
								{
									RoomItem class3 = class2.method_28(this.Session.GetHabbo().uint_5);
									if (class3 == null)
									{
										this.Session.GetHabbo().bool_7 = false;
										this.Session.GetHabbo().uint_5 = 0u;
										ServerMessage Message5 = new ServerMessage(131u);
										this.Session.SendMessage(Message5);
										return;
									}
								}
                                if (!this.Session.GetHabbo().HasFuse("acc_enter_anyroom") && !class2.CheckRights(this.Session, true) && !this.Session.GetHabbo().bool_7)
								{
									if (class2.State == 1)
									{
										if (class2.UserCount == 0)
										{
											ServerMessage Message5 = new ServerMessage(131u);
											this.Session.SendMessage(Message5);
											return;
										}
										ServerMessage Message6 = new ServerMessage(91u);
										Message6.AppendStringWithBreak("");
										this.Session.SendMessage(Message6);
										this.Session.GetHabbo().bool_6 = true;
										ServerMessage Message7 = new ServerMessage(91u);
										Message7.AppendStringWithBreak(this.Session.GetHabbo().Username);
										class2.method_61(Message7);
										return;
									}
									else
									{
										if (class2.State == 2 && string_0.ToLower() != class2.Password.ToLower())
										{
											ServerMessage Message8 = new ServerMessage(33u);
											Message8.AppendInt32(-100002);
											this.Session.SendMessage(Message8);
											ServerMessage Message2 = new ServerMessage(18u);
											this.Session.SendMessage(Message2);
											return;
										}
									}
								}
								ServerMessage Message3 = new ServerMessage(166u);
								Message3.AppendStringWithBreak("/client/private/" + class2.Id + "/Id");
								this.Session.SendMessage(Message3);
							}
						}
						this.Session.GetHabbo().bool_5 = true;
						this.method_6();
					}
				}
			}
		}
		public void method_6()
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(this.Session.GetHabbo().uint_2);
			if (@class != null && this.Session.GetHabbo().bool_5)
			{
				ServerMessage Message = new ServerMessage(69u);
				Message.AppendStringWithBreak(@class.ModelName);
				Message.AppendUInt(@class.Id);
				this.Session.SendMessage(Message);
				if (this.Session.GetHabbo().bool_8)
				{
					ServerMessage Message2 = new ServerMessage(254u);
					this.Session.SendMessage(Message2);
				}
				if (@class.Type == "private")
				{
					if (@class.Wallpaper != "0.0")
					{
						ServerMessage Message3 = new ServerMessage(46u);
						Message3.AppendStringWithBreak("wallpaper");
						Message3.AppendStringWithBreak(@class.Wallpaper);
						this.Session.SendMessage(Message3);
					}
					if (@class.Floor != "0.0")
					{
						ServerMessage Logging = new ServerMessage(46u);
						Logging.AppendStringWithBreak("floor");
						Logging.AppendStringWithBreak(@class.Floor);
						this.Session.SendMessage(Logging);
					}
					ServerMessage Message5 = new ServerMessage(46u);
					Message5.AppendStringWithBreak("landscape");
					Message5.AppendStringWithBreak(@class.Landscape);
					this.Session.SendMessage(Message5);
                    if (@class.CheckRights(this.Session, true))
					{
						ServerMessage Message6 = new ServerMessage(42u);
						this.Session.SendMessage(Message6);
						ServerMessage Message7 = new ServerMessage(47u);
						this.Session.SendMessage(Message7);
					}
					else
					{
						if (@class.method_26(this.Session))
						{
							ServerMessage Message6 = new ServerMessage(42u);
							this.Session.SendMessage(Message6);
						}
					}
					ServerMessage Message8 = new ServerMessage(345u);
                    if (this.Session.GetHabbo().list_4.Contains(@class.Id) || @class.CheckRights(this.Session, true))
					{
						Message8.AppendInt32(@class.Score);
					}
					else
					{
						Message8.AppendInt32(-1);
					}
					this.Session.SendMessage(Message8);
					if (@class.HasEvent)
					{
						this.Session.SendMessage(@class.Event.Serialize(this.Session));
					}
					else
					{
						ServerMessage Message9 = new ServerMessage(370u);
						Message9.AppendStringWithBreak("-1");
						this.Session.SendMessage(Message9);
					}
				}
				this.method_4();
			}
		}
		public void method_7()
		{
			this.Session.GetHabbo().uint_2 = 0u;
			this.Session.GetHabbo().bool_5 = false;
			this.Session.GetHabbo().bool_6 = false;
		}
		public bool method_8(string string_0)
		{
			if (!Regex.IsMatch(string_0, "^[-a-zA-Z0-9._:,]+$"))
			{
				return false;
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					dataRow = @class.ReadDataRow("SELECT * FROM users WHERE username = '" + string_0 + "'");
				}
				return (dataRow == null);
			}
		}
	}
}
