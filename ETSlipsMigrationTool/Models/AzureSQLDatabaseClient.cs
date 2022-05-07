using ETSlipsMigrationTool.Interface;
using Microsoft.Data.SqlClient;
using System.Text;

namespace ETSlipsMigrationTool.Models
{
    internal class AzureSQLDatabaseClient : IDatabaseClient
    {
        private readonly string _connectionString;

        public AzureSQLDatabaseClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task DeleteCategories() => await DeleteTable("categories");

        public async Task DeleteEvents() => await DeleteTable("events");

        public async Task DeletePairs() => await DeleteTable("pairs");

        public async Task DeletePrefixes() => await DeleteTable("prefixes");

        public async Task DeleteRuns() => await DeleteTable("runs");

        public async Task InsertCategories(List<Category> categories)
        {
            using SqlConnection conn = new(_connectionString);
            string sql = $"INSERT INTO categories (id, name) VALUES ";

            StringBuilder stringBuilder = new();

            for (int i = 0; i < categories.Count; i++)
            {
                //stringBuilder.Append($"({categories[i].Id}, {categories[i].Name})");
                stringBuilder.Append($"(@id{i}, @name{i})");

                if (i == categories.Count - 1)
                    stringBuilder.Append(';');
                else
                    stringBuilder.Append(',');
            }

            await conn.OpenAsync();
            using SqlCommand cmd = new(sql, conn);

            for (int i = 0; i < categories.Count; i++)
            {
                cmd.Parameters.AddWithValue($"id{i}", categories[i].Id);
                cmd.Parameters.AddWithValue($"name{i}", categories[i].Name);
            }

            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        public Task InsertEvents(List<RaceEvent> raceEvents)
        {
            throw new NotImplementedException();
        }

        public Task InsertPairs(List<Pair> pairs)
        {
            throw new NotImplementedException();
        }

        public Task InsertPrefixes(List<Prefix> prefixes)
        {
            throw new NotImplementedException();
        }

        public Task InsertRuns(List<Run> runs)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> ListCategories() => throw new NotImplementedException();

        public Task<List<RaceEvent>> ListEvents() => throw new NotImplementedException();

        public Task<List<Pair>> ListPairs() => throw new NotImplementedException();

        public Task<List<Prefix>> ListPrefixes() => throw new NotImplementedException();

        public Task<List<Run>> ListRuns() => throw new NotImplementedException();

        private async Task DeleteTable(string tableName)
        {
            using SqlConnection conn = new(_connectionString);
            string sql = $"DELETE FROM {tableName}";

            await conn.OpenAsync();
            using SqlCommand cmd = new(sql, conn);
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }
    }
}