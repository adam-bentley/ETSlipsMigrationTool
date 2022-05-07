using ETSlipsMigrationTool.Interface;
using MySql.Data.MySqlClient;

namespace ETSlipsMigrationTool.Models
{
    internal class MySQLDatabaseClient : IDatabaseClient
    {
        private readonly string _connectionString;

        public MySQLDatabaseClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task DeleteCategories() => throw new NotImplementedException();

        public Task DeleteEvents() => throw new NotImplementedException();

        public Task DeletePairs() => throw new NotImplementedException();

        public Task DeletePrefixes() => throw new NotImplementedException();

        public Task DeleteRuns() => throw new NotImplementedException();

        public Task InsertCategories(List<Category> categories) => throw new NotImplementedException();

        public Task InsertEvents(List<RaceEvent> raceEvents) => throw new NotImplementedException();

        public Task InsertPairs(List<Pair> pairs) => throw new NotImplementedException();

        public Task InsertPrefixes(List<Prefix> prefixes) => throw new NotImplementedException();

        public Task InsertRuns(List<Run> runs) => throw new NotImplementedException();

        public async Task<List<Category>> ListCategories()
        {
            List<Category> categories = new();
            string sql = "SELECT id, name from categories";

            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Category category = new(reader.GetInt32(0), reader.GetString(1));
                categories.Add(category);
            }

            return categories;
        }

        public async Task<List<RaceEvent>> ListEvents()
        {
            List<RaceEvent> raceEvents = new();
            string sql = "SELECT id, name from events";

            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                RaceEvent raceEvent = new(reader.GetInt32(0), reader.GetString(1));
                raceEvents.Add(raceEvent);
            }

            return raceEvents;
        }

        public async Task<List<Pair>> ListPairs()
        {
            List<Pair> pairs = new();
            string sql = "SELECT timestamp, event, category, round, finish FROM `pairs`;";

            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Pair pair = new(reader.GetDateTime(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetInt32(4));
                pairs.Add(pair);
            }

            return pairs;
        }

        public async Task<List<Prefix>> ListPrefixes()
        {
            List<Prefix> prefixs = new();
            string sql = "SELECT id, category_id, name from prefixs";

            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Prefix prefix = new(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
                prefixs.Add(prefix);
            }

            return prefixs;
        }

        public async Task<List<Run>> ListRuns()
        {
            List<Run> runs = new();
            string sql = @"SELECT timestamp, racenumber, prefix, drivername, lane, runs.index, reaction, et60, et330, et594,
et660, sp660, et936, et1000, sp1000, et1254, et1320, sp1320, result, remarks  FROM `runs`;";

            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var dt = reader.GetDateTime(0);
                try
                {
                    var racenum = reader.GetString(1);
                    var prefix = reader.GetInt32(2);
                    var drivername = reader.GetString(3);
                    var lane = reader.GetChar(4);
                    var index = reader.GetDecimal(5);
                    var reaction = reader.GetDecimal(6);
                    var et60 = reader.GetDecimal(7);
                    var et330 = reader.GetDecimal(8);
                    var et594 = reader.GetDecimal(9);
                    var et660 = reader.GetDecimal(10);
                    var sp660 = reader.GetDecimal(11);
                    decimal? et934 = await reader.IsDBNullAsync(12) ? null : reader.GetDecimal(12);
                    var et1000 = reader.GetDecimal(13);
                    var sp1000 = reader.GetDecimal(14);
                    decimal? et1254 = await reader.IsDBNullAsync(15) ? null : reader.GetDecimal(15);
                    var et1320 = reader.GetDecimal(16);
                    var sp1320 = reader.GetDecimal(17);
                    var result = reader.GetString(18);
                    var remark = reader.GetString(19);

                    Run run = new(dt, racenum, prefix, drivername, lane, index, reaction,
                        et60, et330, et594, et660, sp660, et934, et1000, sp1000, et1254,
                        et1320, sp1320, result, remark);

                    runs.Add(run);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return runs;
        }
    }
}