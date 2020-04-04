using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.Core;
using GoldTree.Messages;
using GoldTree.Storage;
namespace GoldTree.HabboHotel.Support
{
	internal sealed class HelpTool
	{
		public Dictionary<uint, HelpCategory> dictionary_0;
		public Dictionary<uint, HelpTopic> dictionary_1;
		public List<HelpTopic> list_0;
		public List<HelpTopic> list_1;
		public HelpTool()
		{
			this.dictionary_0 = new Dictionary<uint, HelpCategory>();
			this.dictionary_1 = new Dictionary<uint, HelpTopic>();
			this.list_0 = new List<HelpTopic>();
			this.list_1 = new List<HelpTopic>();
		}
		public void method_0(DatabaseClient class6_0)
		{
			Logging.Write("Loading Help Categories..");
			this.dictionary_0.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT Id, caption FROM help_subjects");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					this.dictionary_0.Add((uint)dataRow["Id"], new HelpCategory((uint)dataRow["Id"], (string)dataRow["caption"]));
				}
				Logging.WriteLine("completed!", ConsoleColor.Green);
			}
		}
		public HelpCategory method_1(uint uint_0)
		{
			HelpCategory result;
			if (this.dictionary_0.ContainsKey(uint_0))
			{
				result = this.dictionary_0[uint_0];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public void method_2()
		{
			this.dictionary_0.Clear();
		}
		public void method_3(DatabaseClient class6_0)
		{
			Logging.Write("Loading Help Topics..");
			this.dictionary_1.Clear();
			DataTable dataTable = class6_0.ReadDataTable("SELECT Id, title, body, subject, known_issue FROM help_topics");
			if (dataTable != null)
			{
				foreach (DataRow dataRow in dataTable.Rows)
				{
					HelpTopic @class = new HelpTopic((uint)dataRow["Id"], (string)dataRow["title"], (string)dataRow["body"], (uint)dataRow["subject"]);
					this.dictionary_1.Add((uint)dataRow["Id"], @class);
					int num = int.Parse(dataRow["known_issue"].ToString());
					if (num == 1)
					{
						this.list_1.Add(@class);
					}
					else
					{
						if (num == 2)
						{
							this.list_0.Add(@class);
						}
					}
				}
				Logging.WriteLine("completed!", ConsoleColor.Green);
			}
		}
		public HelpTopic method_4(uint uint_0)
		{
			HelpTopic result;
			if (this.dictionary_1.ContainsKey(uint_0))
			{
				result = this.dictionary_1[uint_0];
			}
			else
			{
				result = null;
			}
			return result;
		}
		public void method_5()
		{
			this.dictionary_1.Clear();
			this.list_0.Clear();
			this.list_1.Clear();
		}
		public int method_6(uint uint_0)
		{
			int num = 0;
			using (TimedLock.Lock(this.dictionary_1))
			{
				foreach (HelpTopic current in this.dictionary_1.Values)
				{
					if (current.uint_1 == uint_0)
					{
						num++;
					}
				}
			}
			return num;
		}
		public ServerMessage method_7()
		{
			ServerMessage Message = new ServerMessage(518u);
			Message.AppendInt32(this.list_0.Count);
			using (TimedLock.Lock(this.list_0))
			{
				foreach (HelpTopic current in this.list_0)
				{
					Message.AppendUInt(current.UInt32_0);
					Message.AppendStringWithBreak(current.string_0);
				}
			}
			Message.AppendInt32(this.list_1.Count);
			using (TimedLock.Lock(this.list_1))
			{
				foreach (HelpTopic current in this.list_1)
				{
					Message.AppendUInt(current.UInt32_0);
					Message.AppendStringWithBreak(current.string_0);
				}
			}
			return Message;
		}
		public ServerMessage method_8()
		{
			ServerMessage Message = new ServerMessage(519u);
			Message.AppendInt32(this.dictionary_0.Count);
			using (TimedLock.Lock(this.dictionary_0))
			{
				foreach (HelpCategory current in this.dictionary_0.Values)
				{
					Message.AppendUInt(current.CategoryId);
					Message.AppendStringWithBreak(current.Caption);
					Message.AppendInt32(this.method_6(current.CategoryId));
				}
			}
			return Message;
		}
		public ServerMessage method_9(HelpTopic class130_0)
		{
			ServerMessage Message = new ServerMessage(520u);
			Message.AppendUInt(class130_0.UInt32_0);
			Message.AppendStringWithBreak(class130_0.string_1);
			return Message;
		}
		public ServerMessage method_10(string string_0)
		{
			DataTable dataTable = null;
			using (DatabaseClient @class = GoldTree.GetDatabase().GetClient())
			{
				@class.AddParamWithValue("query", string_0);
				dataTable = @class.ReadDataTable("SELECT Id,title FROM help_topics WHERE title LIKE @query OR body LIKE @query LIMIT 25");
			}
			ServerMessage Message = new ServerMessage(521u);
			ServerMessage result;
			if (dataTable == null)
			{
				Message.AppendBoolean(false);
				result = Message;
			}
			else
			{
				Message.AppendInt32(dataTable.Rows.Count);
				foreach (DataRow dataRow in dataTable.Rows)
				{
					Message.AppendUInt((uint)dataRow["Id"]);
					Message.AppendStringWithBreak((string)dataRow["title"]);
				}
				result = Message;
			}
			return result;
		}
		public ServerMessage method_11(HelpCategory class131_0)
		{
			ServerMessage Message = new ServerMessage(522u);
			Message.AppendUInt(class131_0.CategoryId);
			Message.AppendStringWithBreak("");
			Message.AppendInt32(this.method_6(class131_0.CategoryId));
			using (TimedLock.Lock(this.dictionary_1))
			{
				foreach (HelpTopic current in this.dictionary_1.Values)
				{
					if (current.uint_1 == class131_0.CategoryId)
					{
						Message.AppendUInt(current.UInt32_0);
						Message.AppendStringWithBreak(current.string_0);
					}
				}
			}
			return Message;
		}
	}
}
