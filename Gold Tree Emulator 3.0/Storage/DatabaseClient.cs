using MySql.Data.MySqlClient;
using System;
using System.Data;
using GoldTree.Storage;
namespace GoldTree.Storage
{
	internal sealed class DatabaseClient : IDisposable
	{
		private DatabaseManager Manager;
		private MySqlConnection Connection;
		private MySqlCommand Command;
		public DatabaseClient(DatabaseManager _Manager)
		{
			Manager = _Manager;
			Connection = new MySqlConnection(_Manager.ConnectionString);
			Command = this.Connection.CreateCommand();
			Connection.Open();
		}
		public void Dispose()
		{
			Connection.Close();
			Command.Dispose();
			Connection.Dispose();
		}
		public void AddParamWithValue(string sParam, object val)
		{
			Command.Parameters.AddWithValue(sParam, val);
		}
		public void ExecuteQuery(string sQuery, int timeout = 30)
		{
            Command.CommandTimeout = timeout;
			Command.CommandText = sQuery;
			Command.ExecuteScalar();
			Command.CommandText = null;
		}
        public DataSet ReadDataSet(string Query, int timeout = 30)
		{
			DataSet dataSet = new DataSet();
            Command.CommandTimeout = timeout;
			Command.CommandText = Query;
			using (MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(this.Command))
			{
				mySqlDataAdapter.Fill(dataSet);
			}
			Command.CommandText = null;
			return dataSet;
		}
        public DataTable ReadDataTable(string Query, int timeout = 30)
		{
			DataTable dataTable = new DataTable();
            Command.CommandTimeout = timeout;
            Command.CommandText = Query;
			using (MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(this.Command))
			{
				mySqlDataAdapter.Fill(dataTable);
			}
			Command.CommandText = null;
			return dataTable;
		}
        public DataRow ReadDataRow(string Query, int timeout = 30)
		{
            Command.CommandTimeout = timeout;
			DataTable dataTable = this.ReadDataTable(Query);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0];
			}
			return null;
		}
		public string ReadString(string Query, int timeout = 30)
		{
            Command.CommandTimeout = timeout;
			Command.CommandText = Query;
			string result = this.Command.ExecuteScalar().ToString();
			Command.CommandText = null;
			return result;
		}
		public int ReadInt32(string Query, int timeout = 30)
		{
            Command.CommandTimeout = timeout;
			Command.CommandText = Query;
			int result = int.Parse(this.Command.ExecuteScalar().ToString());
			Command.CommandText = null;
			return result;
		}
		public uint ReadUInt32(string Query, int timeout = 30)
		{
            Command.CommandTimeout = timeout;
			Command.CommandText = Query;
			uint result = (uint)this.Command.ExecuteScalar();
			Command.CommandText = null;
			return result;
		}
	}
}
