using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using GoldTree.Core;
using GoldTree.Storage;
namespace GoldTree
{
	internal sealed class GoldTreeEnvironment
	{
		private static Dictionary<string, string> ExternalTexts;

		public GoldTreeEnvironment()
		{
			GoldTreeEnvironment.ExternalTexts = new Dictionary<string, string>();
		}

		public static void LoadExternalTexts(DatabaseClient dbClient)
		{
            Logging.Write("Fetching external texts for the Gold Tree Environment.. ");

            if (ExternalTexts.Count > 0)
                ExternalTexts.Clear();

			DataTable dataTable = dbClient.ReadDataTable("SELECT identifier, display_text FROM texts ORDER BY identifier ASC;");

			if (dataTable != null)
			{
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GoldTreeEnvironment.ExternalTexts.Add(dataRow["identifier"].ToString(), dataRow["display_text"].ToString());
                }
			}

            Logging.WriteLine("completed!", ConsoleColor.Green);
		}

		public static string GetExternalText(string key)
		{
			string result;

            if (GoldTreeEnvironment.ExternalTexts != null && GoldTreeEnvironment.ExternalTexts.ContainsKey(key))
				result = GoldTreeEnvironment.ExternalTexts[key];
			else
				result = key;

			return result;
		}

        public static int GetRandomNumber(int Min, int Max)
        {
            Random Quick = new Random();

            try
            {
                return Quick.Next(Min, Max);
            }
            catch
            {
                return Min;
            }
        }
	}
}