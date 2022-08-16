using ETSlipsMigrationTool.Interface;
using ETSlipsMigrationTool.Models;
using Npgsql;
using System.Text;

namespace ETSlipsMigrationTool.Services.DestinationClients
{
    /// <summary>
    /// A postgreSQL client
    /// </summary>
    /// <seealso cref="IDestinationDatabase" />
    internal class PostgresDatabaseClient : IDestinationDatabase
    {
        /// <summary>
        /// The database connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureSQLDatabaseClient"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PostgresDatabaseClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Delete queries

        /// <summary>
        /// Deletes all categories from the categories table.
        /// </summary>
        /// <returns>
        /// An awaitable task
        /// </returns>
        public Task DeleteCategories() => DeleteTable("categories");

        /// <summary>
        /// Deletes all events from the events table.
        /// </summary>
        /// <returns>
        /// An awaitable task
        /// </returns>
        public Task DeleteEvents() => DeleteTable("events");

        /// <summary>
        /// Deletes all pairs from the pairs table.
        /// </summary>
        /// <returns>
        /// An awaitable task
        /// </returns>
        public Task DeletePairs() => DeleteTable("pairs");

        /// <summary>
        /// Deletes all prefixes from the prefixes table.
        /// </summary>
        /// <returns>
        /// An awaitable task
        /// </returns>
        public Task DeletePrefixes() => DeleteTable("prefixes");

        /// <summary>
        /// Deletes all runs from the runs table.
        /// </summary>
        /// <returns>
        /// An awaitable task
        /// </returns>
        public Task DeleteRuns() => DeleteTable("runs");

        #endregion Delete queries

        #region Insert queries

        /// <summary>
        /// Inserts a list of categories into the categories table.
        /// </summary>
        /// <param name="categories">The list of categories</param>
        /// <returns>
        /// A dictionary of old ID to new ID mappings
        /// </returns>
        public async Task<Dictionary<int, int>> InsertCategories(List<Category> categories)
        {
            using NpgsqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            Dictionary<int, int> mappings = new();

            foreach (var category in categories)
            {
                string sql = $"INSERT INTO categories (name) VALUES (@name) returning id;";
                using NpgsqlCommand cmd = new(sql, conn);

                cmd.Parameters.AddWithValue("name", category.Name);
                int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                mappings[category.Id] = newId;
            }

            await conn.CloseAsync();
            return mappings;
        }

        /// <summary>
        /// Inserts a list of events into the events table.
        /// </summary>
        /// <param name="raceEvents">The list of race events.</param>
        /// <returns>
        /// A dictionary of old ID to new ID mappings
        /// </returns>
        public async Task<Dictionary<int, int>> InsertEvents(List<RaceEvent> raceEvents)
        {
            using NpgsqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            Dictionary<int, int> mappings = new();

            foreach (var raceEvent in raceEvents)
            {
                string sql = $"INSERT INTO events (name) VALUES (@name) returning id;";
                using NpgsqlCommand cmd = new(sql, conn);

                cmd.Parameters.AddWithValue("name", raceEvent.Name);
                int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                mappings[raceEvent.Id] = newId;
            }

            await conn.CloseAsync();
            return mappings;
        }

        /// <summary>
        /// Inserts a list of pairs into the pairs table.
        /// </summary>
        /// <param name="pairs">The list of pairs.</param>
        /// <param name="eventMappings">The event id mappings.</param>
        /// <param name="categoryMappings">The category id mappings.</param>
        /// <returns>
        /// An awaitable task
        /// </returns>
        public async Task InsertPairs(List<Pair> pairs, Dictionary<int, int> eventMappings, Dictionary<int, int> categoryMappings)
        {
            using NpgsqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            foreach (Pair[] batch in pairs.Chunk(400))
            {
                string sql = $"INSERT INTO pairs (timestamp, event, category, round, finish) VALUES";
                StringBuilder values = new();

                for (int i = 0; i < batch.Length; i++)
                {
                    values.Append($"(@timestamp{i}, @event{i}, @category{i}, @round{i}, @finish{i})");

                    if (i < batch.Length - 1)
                    {
                        values.Append(',');
                    }
                }

                sql += values.ToString() + ";";
                using NpgsqlCommand cmd = new(sql, conn);

                for (int i = 0; i < batch.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"timestamp{i}", batch[i].Timestamp);
                    cmd.Parameters.AddWithValue($"event{i}", eventMappings[batch[i].EventId]);
                    cmd.Parameters.AddWithValue($"category{i}", categoryMappings[batch[i].CategoryId]);
                    cmd.Parameters.AddWithValue($"round{i}", batch[i].Round);
                    cmd.Parameters.AddWithValue($"finish{i}", (int)batch[i].Finish);
                }

                await cmd.ExecuteNonQueryAsync();
            }

            await conn.CloseAsync();
        }

        /// <summary>
        /// Inserts a list of prefixes into the prefixes table.
        /// </summary>
        /// <param name="prefixes"></param>
        /// <param name="categoryIdMappings"></param>
        /// <returns>
        /// A dictionary of old ID to new ID mappings
        /// </returns>
        public async Task<Dictionary<int, int>> InsertPrefixes(List<Prefix> prefixes, Dictionary<int, int> categoryIdMappings)
        {
            using NpgsqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            Dictionary<int, int> mappings = new();

            foreach (var prefix in prefixes)
            {
                string sql = $"INSERT INTO prefixes (category_id, name) VALUES (@category_id, @name) returning id;";
                using NpgsqlCommand cmd = new(sql, conn);

                cmd.Parameters.AddWithValue("category_id", categoryIdMappings[prefix.CategoryId]);
                cmd.Parameters.AddWithValue("name", prefix.Name);
                int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                mappings[prefix.Id] = newId;
            }

            await conn.CloseAsync();
            return mappings;
        }

        /// <summary>
        /// Inserts a list of runs into the runs table.
        /// </summary>
        /// <param name="runs">The list of runs.</param>
        /// <param name="prefixMappings">The prefix id mappings.</param>
        /// <returns>
        /// An awaitable task
        /// </returns>
        public async Task InsertRuns(List<Run> runs, Dictionary<int, int> prefixMappings)
        {
            using NpgsqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            int batchNumber = 0;
            int totalBatches = runs.Count / 100;

            foreach (Run[] batch in runs.Chunk(100))
            {
                batchNumber++;
                string sql = @"INSERT INTO runs (timestamp, racenumber, prefix, drivername, lane, index, reaction, et60, et330,
et594, et660, sp660, et934, et1000, sp1000, et1254, et1320, sp1320, result, remarks) VALUES";
                StringBuilder values = new();

                for (int i = 0; i < batch.Length; i++)
                {
                    values.Append(@$"(@timestamp{i}, @racenumber{i}, @prefix{i}, @drivername{i}, @lane{i}, @index{i}, @reaction{i},
@et60{i}, @et330{i}, @et594{i}, @et660{i}, @sp660{i}, @et934{i}, @et1000{i}, @sp1000{i}, @et1254{i}, @et1320{i}, @sp1320{i}, @result{i}, @remarks{i})");

                    if (i < batch.Length - 1)
                    {
                        values.Append(',');
                    }
                }

                sql += values.ToString() + ";";
                using NpgsqlCommand cmd = new(sql, conn);
                try
                {
                    for (int i = 0; i < batch.Length; i++)
                    {
                        cmd.Parameters.AddWithValue($"timestamp{i}", batch[i].Timestamp);
                        cmd.Parameters.AddWithValue($"racenumber{i}", batch[i].Racenumber);
                        cmd.Parameters.AddWithValue($"prefix{i}", prefixMappings[batch[i].PrefixId]);
                        cmd.Parameters.AddWithValue($"drivername{i}", batch[i].Drivername);
                        cmd.Parameters.AddWithValue($"lane{i}", batch[i].Lane);
                        cmd.Parameters.AddWithValue($"index{i}", batch[i].Index);
                        cmd.Parameters.AddWithValue($"reaction{i}", batch[i].Reaction);
                        cmd.Parameters.AddWithValue($"et60{i}", batch[i].ET60);
                        cmd.Parameters.AddWithValue($"et330{i}", batch[i].ET330);
                        cmd.Parameters.AddWithValue($"et594{i}", batch[i].ET594);
                        cmd.Parameters.AddWithValue($"et660{i}", batch[i].ET660);
                        cmd.Parameters.AddWithValue($"sp660{i}", batch[i].SP660);
                        cmd.Parameters.AddWithValue($"et934{i}", batch[i].ET934 != null ? batch[i].ET934 : DBNull.Value);
                        cmd.Parameters.AddWithValue($"et1000{i}", batch[i].ET1000);
                        cmd.Parameters.AddWithValue($"sp1000{i}", batch[i].SP1000);
                        cmd.Parameters.AddWithValue($"et1254{i}", batch[i].ET1254 != null ? batch[i].ET1254 : DBNull.Value);
                        cmd.Parameters.AddWithValue($"et1320{i}", batch[i].ET1320);
                        cmd.Parameters.AddWithValue($"sp1320{i}", batch[i].SP1320);
                        cmd.Parameters.AddWithValue($"result{i}", batch[i].Result);
                        cmd.Parameters.AddWithValue($"remarks{i}", batch[i].Remarks);
                    }

                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            await conn.CloseAsync();
        }

        #endregion Insert queries

        #region Private helpers

        /// <summary>
        /// Executes a query that deletes all data from a table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        private async Task DeleteTable(string tableName)
        {
            using NpgsqlConnection conn = new(_connectionString);
            string sql = $"DELETE FROM {tableName}";

            await conn.OpenAsync();
            using NpgsqlCommand cmd = new(sql, conn);
            cmd.CommandTimeout = 600;
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        #endregion Private helpers
    }
}