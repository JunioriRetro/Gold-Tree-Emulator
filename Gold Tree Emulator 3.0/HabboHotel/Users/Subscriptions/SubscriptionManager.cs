using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.HabboHotel.Users.UserDataManagement;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Users.Subscriptions
{
	internal class SubscriptionManager
	{
		public uint UserId;

		private Dictionary<string, Subscription> Subscriptions;

        public Dictionary<string, Subscription> GetSubscriptions() { return Subscriptions; }

		public List<string> SubscriptionTypes
		{
			get
			{
				List<string> list = new List<string>();

				using (TimedLock.Lock(this.Subscriptions.Values))
				{
					foreach (Subscription current in this.Subscriptions.Values)
					{
						list.Add(current.Type);
					}
				}

				return list;
			}
		}

		public SubscriptionManager(uint userId, UserDataFactory userdata)
		{
			this.UserId = userId;
			
            this.Subscriptions = new Dictionary<string, Subscription>();

			DataTable dataTable_ = userdata.GetSubscriptions();

			if (dataTable_ != null)
			{
				foreach (DataRow dataRow in dataTable_.Rows)
				{
					this.Subscriptions.Add((string)dataRow["subscription_id"], new Subscription((string)dataRow["subscription_id"], (int)dataRow["timestamp_activated"], (int)dataRow["timestamp_expire"]));
				}
			}
		}

		public Subscription GetSubscriptionByType(string type)
		{
			if (this.Subscriptions.ContainsKey(type))
			{
				return Subscriptions[type];
			}
			else
			{
				return null;
			}
		}

        public bool HasSubscription(string type)
		{
			if (!this.Subscriptions.ContainsKey(type))
			{
				return false;
			}
			else
			{
				Subscription subscription = this.Subscriptions[type];
				return subscription.HasExpired();
			}
		}

		public void method_3(string type, int time)
		{
			type = type.ToLower();

			if (this.Subscriptions.ContainsKey(type))
			{
				Subscription subscription = this.Subscriptions[type];

				subscription.AppendTime(time);

				if (subscription.ExpirationTime <= 0 || subscription.ExpirationTime >= 2147483647)
					return;

				using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
				{
					dbClient.AddParamWithValue("subcrbr", type);

					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"UPDATE user_subscriptions SET timestamp_expire = '",
						subscription.ExpirationTime,
						"' WHERE user_id = '",
						this.UserId,
						"' AND subscription_id = @subcrbr LIMIT 1"
					}));
					return;
				}
			}

			if (!this.Subscriptions.ContainsKey("habbo_vip"))
			{
				int now = (int)GoldTree.GetUnixTimestamp();
				int expiration = (int)GoldTree.GetUnixTimestamp() + time;

				Subscription subscription = new Subscription(type, now, expiration);

				using (DatabaseClient dbClient = GoldTree.GetDatabase().GetClient())
				{
					dbClient.AddParamWithValue("subcrbr", type);

					dbClient.ExecuteQuery(string.Concat(new object[]
					{
						"INSERT INTO user_subscriptions (user_id,subscription_id,timestamp_activated,timestamp_expire) VALUES ('",
						this.UserId,
						"',@subcrbr,'",
						now,
						"','",
						expiration,
						"')"
					}));
				}

				this.Subscriptions.Add(subscription.Type.ToLower(), subscription);
			}
		}

        public int CalculateHCSubscription(Habbo habbo)
        {
            if (habbo.GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                return ((int)GoldTree.GetUnixTimestamp() - habbo.GetSubscriptionManager().GetSubscriptionByType("habbo_club").StartingTime) / 2678400;
            }
            else
            {
                if (habbo.GetSubscriptionManager().GetSubscriptionByType(habbo.Id.ToString()) != null)
                    return (habbo.GetSubscriptionManager().GetSubscriptionByType("habbo_club").ExpirationTime - habbo.GetSubscriptionManager().GetSubscriptionByType("habbo_club").StartingTime) / 2678400;
                
                return 0;
            }
        }
	}
}
