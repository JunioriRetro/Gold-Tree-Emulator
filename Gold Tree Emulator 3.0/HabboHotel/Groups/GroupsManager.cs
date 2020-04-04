using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.Storage;
namespace GoldTree
{
	internal sealed class GroupsManager
	{
		public int int_0;
		public string string_0;
		public string string_1;
		public int int_1;
		public List<int> list_0;
		public string string_2;
		public uint uint_0;
		public string string_3;
		public GroupsManager(int int_2, DataRow Row, DatabaseClient class6_0)
		{
			this.int_0 = int_2;
			this.string_0 = (string)Row["name"];
			this.string_1 = (string)Row["desc"];
			this.int_1 = (int)Row["OwnerId"];
			this.string_2 = (string)Row["badge"];
			this.uint_0 = (uint)Row["roomid"];
			this.string_3 = (string)Row["locked"];
			this.list_0 = new List<int>();
			DataTable dataTable = class6_0.ReadDataTable("SELECT userid FROM group_memberships WHERE groupid = " + int_2 + ";");
			foreach (DataRow dataRow in dataTable.Rows)
			{
				this.method_0((int)dataRow["userid"]);
			}
		}
		public void method_0(int int_2)
		{
			if (!this.list_0.Contains(int_2))
			{
				this.list_0.Add(int_2);
			}
		}
		public void method_1(int int_2)
		{
			if (this.list_0.Contains(int_2))
			{
				this.list_0.Remove(int_2);
			}
		}
	}
}
