﻿using System;
using System.Data.SqlClient;
using System.Diagnostics;

using Dapper;
using Tests.DataModel;

namespace Tests
{
	class DapperTests : ITests
	{
		readonly string _connectionString = LinqToDB.Data.DataConnection.GetConnectionString("Test");

		public bool GetSingleColumnFast(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			using (var con = new SqlConnection(_connectionString))
			{
				con.Open();
				for (var i = 0; i < repeatCount; i++)
					con.Query<int>("SELECT ID FROM Narrow WHERE ID = 1");
			}

			watch.Stop();

			return true;
		}

		public bool GetSingleColumnSlow(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var con = new SqlConnection(_connectionString))
				{
					con.Open();
					con.Query<int>("SELECT ID FROM Narrow WHERE ID = 1");
				}

			watch.Stop();

			return true;
		}

		public bool GetSingleColumnParam(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			using (var con = new SqlConnection(_connectionString))
			{
				con.Open();
				for (var i = 0; i < repeatCount; i++)
					con.Query<int>("SELECT ID FROM Narrow WHERE ID = @id AND Field1 = @p", new { id = 1, p = 2 });
			}

			watch.Stop();

			return true;
		}

		public bool GetNarrowList(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			using (var con = new SqlConnection(_connectionString))
			{
				con.Open();

				for (var i = 0; i < repeatCount; i++)
					foreach (var item in con.Query<NarrowLong>($"SELECT TOP {takeCount} ID, Field1 FROM NarrowLong")) {}
			}

			watch.Stop();

			return true;
		}
	}
}
