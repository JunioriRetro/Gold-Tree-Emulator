using System;
using System.Collections.Generic;
using GoldTree.HabboHotel.GameClients;
using GoldTree.HabboHotel.Items;
using GoldTree.HabboHotel.Rooms;
namespace GoldTree.HabboHotel.Rooms
{
	internal sealed class TradeUser
	{
		public uint UserId;
		private uint RoomId;
		private bool Accepted;
		public List<UserItem> OfferedItems;
		public bool Boolean_0
		{
			get
			{
				return this.Accepted;
			}
			set
			{
				this.Accepted = value;
			}
		}
		public TradeUser(uint UserId, uint RoomId)
		{
			this.UserId = UserId;
			this.RoomId = RoomId;
			this.Accepted = false;
			this.OfferedItems = new List<UserItem>();
		}
		public RoomUser method_0()
		{
			Room @class = GoldTree.GetGame().GetRoomManager().GetRoom(this.RoomId);
			if (@class == null)
			{
				return null;
			}
			else
			{
				return @class.GetRoomUserByHabbo(this.UserId);
			}
		}
		public GameClient method_1()
		{
			return GoldTree.GetGame().GetClientManager().method_2(this.UserId);
		}
	}
}
