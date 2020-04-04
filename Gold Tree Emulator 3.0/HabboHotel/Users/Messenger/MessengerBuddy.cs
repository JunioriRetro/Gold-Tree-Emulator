using System;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
using System.Globalization;
namespace GoldTree.HabboHotel.Users.Messenger
{
	internal sealed class MessengerBuddy
	{
		private uint uint_0;
		internal bool bool_0;
		private string string_0;
		private string string_1;
		private string string_2;
		private string string_3;
		public uint UInt32_0
		{
			get
			{
				return this.uint_0;
			}
		}
		internal string String_0
		{
			get
			{
				return this.string_0;
			}
		}
		internal string String_1
		{
			get
			{
				GameClient @class = GoldTree.GetGame().GetClientManager().method_2(this.uint_0);
				string result;
				if (@class != null)
				{
					result = @class.GetHabbo().RealName;
				}
				else
				{
					using (DatabaseClient class2 = GoldTree.GetDatabase().GetClient())
					{
						result = class2.ReadString("SELECT real_name FROM users WHERE Id = '" + this.uint_0 + "' LIMIT 1");
					}
				}
				return result;
			}
		}
		internal string String_2
		{
			get
			{
				return this.string_1;
			}
		}
		internal string String_3
		{
			get
			{
				return this.string_2;
			}
		}
		internal string String_4
		{
			get
			{
				return this.string_3;
			}
		}
		internal bool Boolean_0
		{
			get
			{
				GameClient @class = GoldTree.GetGame().GetClientManager().method_2(this.uint_0);
				return @class != null && @class.GetHabbo() != null && @class.GetHabbo().GetMessenger() != null && !@class.GetHabbo().GetMessenger().bool_0 && !@class.GetHabbo().HideOnline;
			}
		}
		internal bool Boolean_1
		{
			get
			{
				GameClient @class = GoldTree.GetGame().GetClientManager().method_2(this.uint_0);
                return @class != null && (@class.GetHabbo().InRoom && !@class.GetHabbo().HideInRom);
			}
		}
		public MessengerBuddy(uint uint_1, string string_4, string string_5, string string_6, string string_7)
		{
			this.uint_0 = uint_1;
			this.string_0 = string_4;
			this.string_1 = string_5;
			this.string_2 = string_6;
            double timestamp;
            if (double.TryParse(string_7, NumberStyles.Any, CustomCultureInfo.GetCustomCultureInfo(), out timestamp))
            {
                this.string_3 = GoldTree.TimestampToDate(timestamp).ToString();
            }
            else
            {
                this.string_3 = GoldTree.TimestampToDate(GoldTree.GetUnixTimestamp()).ToString();
            }
			this.bool_0 = false;
		}
		public void method_0(ServerMessage Message5_0, bool bool_1)
		{
			if (bool_1)
			{
				Message5_0.AppendUInt(this.uint_0);
				Message5_0.AppendStringWithBreak(this.string_0);
				Message5_0.AppendStringWithBreak(this.string_2);
				bool boolean_ = this.Boolean_0;
				Message5_0.AppendBoolean(boolean_);
				if (boolean_)
				{
					Message5_0.AppendBoolean(this.Boolean_1);
				}
				else
				{
					Message5_0.AppendBoolean(false);
				}
				Message5_0.AppendStringWithBreak("");
				Message5_0.AppendBoolean(false);
				Message5_0.AppendStringWithBreak(this.string_1);
				Message5_0.AppendStringWithBreak(this.string_3);
				Message5_0.AppendStringWithBreak("");
			}
			else
			{
				Message5_0.AppendUInt(this.uint_0);
				Message5_0.AppendStringWithBreak(this.string_0);
				Message5_0.AppendBoolean(true);
				if (this.uint_0 == 0u)
				{
					Message5_0.AppendBoolean(true);
					Message5_0.AppendBoolean(false);
				}
				else
				{
					bool boolean_ = this.Boolean_0;
					Message5_0.AppendBoolean(boolean_);
					if (boolean_)
					{
						Message5_0.AppendBoolean(this.Boolean_1);
					}
					else
					{
						Message5_0.AppendBoolean(false);
					}
				}
				Message5_0.AppendStringWithBreak(this.string_1);
				Message5_0.AppendBoolean(false);
				Message5_0.AppendStringWithBreak(this.string_2);
				Message5_0.AppendStringWithBreak(this.string_3);
				Message5_0.AppendStringWithBreak("");
				Message5_0.AppendStringWithBreak("");
			}
		}
	}
}
