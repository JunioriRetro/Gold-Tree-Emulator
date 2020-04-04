using System;
using System.Data;
using GoldTree.HabboHotel.GameClients;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.Catalogs
{
	internal sealed class VoucherHandler
	{
		public bool method_0(string string_0)
		{
			bool result;
			using (DatabaseClient adapter = GoldTree.GetDatabase().GetClient())
			{
				adapter.AddParamWithValue("code", string_0);
				if (adapter.ReadDataRow("SELECT null FROM vouchers WHERE code = @code LIMIT 1") != null)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public void method_1(string string_0)
		{
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("code", string_0);
				@class.ExecuteQuery("DELETE FROM vouchers WHERE code = @code LIMIT 1");
			}
		}
		public void method_2(GameClient Session, string string_0)
		{
			if (!this.method_0(string_0))
			{
				ServerMessage Message = new ServerMessage(213u);
				Message.AppendRawInt32(0);
				Session.SendMessage(Message);
			}
			else
			{
				DataRow dataRow = null;
				using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
				{
					@class.AddParamWithValue("code", string_0);
					dataRow = @class.ReadDataRow("SELECT * FROM vouchers WHERE code = @code LIMIT 1");
				}
				int num = (int)dataRow["credits"];
				int num2 = (int)dataRow["pixels"];
				int num3 = (int)dataRow["vip_points"];
				this.method_1(string_0);
				if (num > 0)
				{
					Session.GetHabbo().Credits += num;
					Session.GetHabbo().UpdateCredits(true);
				}
				if (num2 > 0)
				{
					Session.GetHabbo().ActivityPoints += num2;
					Session.GetHabbo().UpdateActivityPoints(true);
				}
				if (num3 > 0)
				{
					Session.GetHabbo().VipPoints += num3;
					Session.GetHabbo().UpdateVipPoints(false, true);
				}
				Session.SendMessage(new ServerMessage(212u));
			}
		}
	}
}
