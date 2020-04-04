using System;
using System.Collections.Generic;
using System.Data;
using GoldTree.Core;
using GoldTree.Storage;
namespace GoldTree
{
	internal sealed class Groups
	{
		public static Dictionary<int, GroupsManager> GroupsManager;
		public Groups()
		{
			Groups.GroupsManager = new Dictionary<int, GroupsManager>();
		}
		public static void smethod_0(DatabaseClient class6_0)
		{
            Logging.Write("Loading groups...");
			Groups.smethod_1();
			DataTable dataTable = class6_0.ReadDataTable("SELECT * FROM groups;");
			foreach (DataRow dataRow in dataTable.Rows)
			{
				Groups.GroupsManager.Add((int)dataRow["Id"], new GroupsManager((int)dataRow["Id"], dataRow, class6_0));
			}
			Logging.WriteLine("completed!", ConsoleColor.Green);
		}
		public static void smethod_1()
		{
			Groups.GroupsManager.Clear();
		}
		public static GroupsManager smethod_2(int int_0)
		{
			if (Groups.GroupsManager.ContainsKey(int_0))
			{
				return Groups.GroupsManager[int_0];
			}
			else
			{
				return null;
			}
		}
		public static void smethod_3(DatabaseClient class6_0, int int_0)
		{
			GroupsManager @class = Groups.smethod_2(int_0);
			if (@class != null)
			{
				DataRow Row = class6_0.ReadDataRow("SELECT * FROM groups WHERE Id = " + int_0 + " LIMIT 1");
				@class.string_0 = (string)Row["name"];
				@class.string_2 = (string)Row["badge"];
				@class.uint_0 = (uint)Row["roomid"];
				@class.string_1 = (string)Row["desc"];
				@class.string_3 = (string)Row["locked"];
				@class.list_0.Clear();
				DataTable dataTable = class6_0.ReadDataTable("SELECT userid FROM group_memberships WHERE groupid = " + int_0 + ";");
				foreach (DataRow dataRow2 in dataTable.Rows)
				{
					@class.method_0((int)dataRow2["userid"]);
				}
			}
		}
	}
}
