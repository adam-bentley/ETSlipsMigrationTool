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

        public async Task DeletePrefixs() => await DeleteTable("prefixs");

        public async Task DeleteRuns() => await DeleteTable("runs");

        public async Task InsertCategories(List<Category> categories)
        {
            using SqlConnection conn = new(_connectionString);
            string sql = $"INSERT INTO categories (name) VALUES ";

            StringBuilder values = new();

            for (int i = 0; i < categories.Count; i++)
            {
                values.Append($"(@name{i})");

                if (i == categories.Count - 1)
                    values.Append(';');
                else
                    values.Append(',');
            }

            await conn.OpenAsync();
            using SqlCommand cmd = new(sql + values.ToString(), conn);

            for (int i = 0; i < categories.Count; i++)
            {
                cmd.Parameters.AddWithValue($"name{i}", categories[i].Name);
            }

            
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        public async Task InsertEvents(List<RaceEvent> raceEvents)
        {
            using SqlConnection conn = new(_connectionString);
            string sql = $"INSERT INTO events (name) VALUES ";

            StringBuilder values = new();

            for (int i = 0; i < raceEvents.Count; i++)
            {
                values.Append($"(@name{i})");

                if (i == raceEvents.Count - 1)
                    values.Append(';');
                else
                    values.Append(',');
            }

            await conn.OpenAsync();
            using SqlCommand cmd = new(sql + values.ToString(), conn);

            for (int i = 0; i < raceEvents.Count; i++)
            {
                cmd.Parameters.AddWithValue($"id{i}", raceEvents[i].Id);
                cmd.Parameters.AddWithValue($"name{i}", raceEvents[i].Name);
            }

            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        public Task InsertPairs(List<Pair> pairs)
        {
            throw new NotImplementedException();
        }

        public async Task InsertPrefixes(List<Prefix> prefixes)
        {
            using SqlConnection conn = new(_connectionString);
            string sql = $"INSERT INTO prefixs (id, category_id, name) VALUES ";

            StringBuilder values = new();

            for (int i = 0; i < prefixes.Count; i++)
            {
                values.Append($"(@id{i}, @category_id{i}, @name{i})");

                if (i == prefixes.Count - 1)
                    values.Append(';');
                else
                    values.Append(',');
            }

            await conn.OpenAsync();
            using SqlCommand cmd = new(sql + values.ToString(), conn);

            for (int i = 0; i < prefixes.Count; i++)
            {
                cmd.Parameters.AddWithValue($"id{i}", prefixes[i].Id);
                cmd.Parameters.AddWithValue($"category_id{i}", prefixes[i].CategoryId);
                cmd.Parameters.AddWithValue($"name{i}", prefixes[i].Name);
            }

            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        public Task InsertRuns(List<Run> runs)
        {
            throw new NotImplementedException();
        }

        #region Lists

        public Task<List<Category>> ListCategories() => throw new NotImplementedException();

        public Task<List<RaceEvent>> ListEvents() => throw new NotImplementedException();

        public Task<List<Pair>> ListPairs() => throw new NotImplementedException();

        public Task<List<Prefix>> ListPrefixes() => throw new NotImplementedException();

        public Task<List<Run>> ListRuns() => throw new NotImplementedException();

        #endregion

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