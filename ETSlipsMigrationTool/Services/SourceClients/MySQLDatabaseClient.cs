using ETSlipsMigrationTool.Interface;
using ETSlipsMigrationTool.Models;
using MySql.Data.MySqlClient;

namespace ETSlipsMigrationTool.Services.SourceClients
{
    /// <summary>
    /// A MySQL client
    /// </summary>
    /// <seealso cref="ISourceDatabase" />
    internal class MySQLDatabaseClient : ISourceDatabase
    {
        #region Private fields

        /// <summary>
        /// The database connection string
        /// </summary>
        private readonly string _connectionString;

        #endregion Private fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MySQLDatabaseClient"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MySQLDatabaseClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion Constructors

        #region List queries

        /// <summary>
        /// Gets a lists of all categories from the categories table.
        /// </summary>
        /// <returns>
        /// A list of categories
        /// </returns>
        public async Task<List<Category>> ListCategories()
        {
            List<Category> categories = new();
            string sql = "SELECT id, name from categories order by id";

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

        /// <summary>
        /// Gets a lists of all events from the events table.
        /// </summary>
        /// <returns>
        /// A list of events
        /// </returns>
        public async Task<List<RaceEvent>> ListEvents()
        {
            List<RaceEvent> raceEvents = new();
            string sql = "SELECT id, name from events order by id";

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

        /// <summary>
        /// Gets a lists of all pairs from the pairs table.
        /// </summary>
        /// <returns>
        /// A list of pairs
        /// </returns>
        public async Task<List<Pair>> ListPairs()
        {
            List<Pair> pairs = new();
            string sql = "SELECT timestamp, event, category, round, finish FROM `pairs` order by timestamp;";

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

        /// <summary>
        /// Gets a lists of all prefixes from the prefixes table.
        /// </summary>
        /// <returns>
        /// A list of prefixes
        /// </returns>
        public async Task<List<Prefix>> ListPrefixes()
        {
            List<Prefix> prefixes = new();
            string sql = "SELECT id, category_id, name from prefixes order by id";

            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Prefix prefix = new(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
                prefixes.Add(prefix);
            }

            return prefixes;
        }

        /// <summary>
        /// Gets a lists of all runs from the runs table.
        /// </summary>
        /// <returns>
        /// A list of runs
        /// </returns>
        public async Task<List<Run>> ListRuns()
        {
            List<Run> runs = new();
            string sql = @"SELECT timestamp, racenumber, prefix, drivername, lane, runs.index, reaction, et60, et330, et594,
et660, sp660, et936, et1000, sp1000, et1254, et1320, sp1320, result, remarks  FROM `runs` order by timestamp;";

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

        #endregion List queries
    }
}