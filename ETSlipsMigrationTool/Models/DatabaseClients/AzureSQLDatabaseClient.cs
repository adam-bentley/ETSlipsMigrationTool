using ETSlipsMigrationTool.Interface;
using Microsoft.Data.SqlClient;
using System.Text;

namespace ETSlipsMigrationTool.Models.DatabaseClients
{
    /// <summary>
    /// An Azure SQL (SQL Server) client
    /// </summary>
    /// <seealso cref="ETSlipsMigrationTool.Interface.IDestinationDatabase" />
    internal class AzureSQLDatabaseClient : IDestinationDatabase
    {
        #region private fields

        /// <summary>
        /// The database connection string
        /// </summary>
        private readonly string _connectionString;

        #endregion private fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureSQLDatabaseClient"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AzureSQLDatabaseClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion Constructor

        #region Delete queries

        /// <summary>
        /// Deletes all categories from the categories table.
        /// </summary>
        public async Task DeleteCategories() => await DeleteTable("categories");

        /// <summary>
        /// Deletes all events from the events table.
        /// </summary>
        public async Task DeleteEvents() => await DeleteTable("events");

        /// <summary>
        /// Deletes all pairs from the pairs table.
        /// </summary>
        public async Task DeletePairs() => await DeleteTable("pairs");

        /// <summary>
        /// Deletes all prefixs from the prefixs table.
        /// </summary>
        public async Task DeletePrefixs() => await DeleteTable("prefixs");

        /// <summary>
        /// Deletes all runs from the runs table.
        /// </summary>
        public async Task DeleteRuns() => await DeleteTable("runs");

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
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            Dictionary<int, int> mappings = new();

            foreach (var category in categories)
            {
                string sql = $"INSERT INTO categories (name) VALUES (@name); SELECT SCOPE_IDENTITY();";
                using SqlCommand cmd = new(sql, conn);

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
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            Dictionary<int, int> mappings = new();

            foreach (var raceEvent in raceEvents)
            {
                string sql = $"INSERT INTO events (name) VALUES (@name); SELECT SCOPE_IDENTITY();";
                using SqlCommand cmd = new(sql, conn);

                cmd.Parameters.AddWithValue("name", raceEvent.Name);
                int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                mappings[raceEvent.Id] = newId;
            }

            await conn.CloseAsync();
            return mappings;
        }

        /// <summary>
        /// Inserts the pairs.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        /// <param name="raceEventMappings">The race event mappings.</param>
        /// <param name="categoryMappings">The category mappings.</param>
        public async Task InsertPairs(List<Pair> pairs, Dictionary<int, int> raceEventMappings, Dictionary<int, int> categoryMappings)
        {
            using SqlConnection conn = new(_connectionString);
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
                using SqlCommand cmd = new(sql, conn);

                for (int i = 0; i < batch.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"timestamp{i}", batch[i].Timestamp);
                    cmd.Parameters.AddWithValue($"event{i}", raceEventMappings[batch[i].EventId]);
                    cmd.Parameters.AddWithValue($"category{i}", categoryMappings[batch[i].CategoryId]);
                    cmd.Parameters.AddWithValue($"round{i}", batch[i].Round);
                    cmd.Parameters.AddWithValue($"finish{i}", batch[i].Finish);
                }

                await cmd.ExecuteNonQueryAsync();
            }

            await conn.CloseAsync();
        }

        /// <summary>
        /// Inserts the prefixes.
        /// </summary>
        /// <param name="prefixs">The prefixs.</param>
        /// <param name="categoryMappings">The category mappings.</param>
        /// <returns></returns>
        public async Task<Dictionary<int, int>> InsertPrefixes(List<Prefix> prefixs, Dictionary<int, int> categoryMappings)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            Dictionary<int, int> mappings = new();

            foreach (var prefix in prefixs)
            {
                string sql = $"INSERT INTO prefixs (category_id, name) VALUES (@category_id, @name); SELECT SCOPE_IDENTITY();";
                using SqlCommand cmd = new(sql, conn);

                cmd.Parameters.AddWithValue("category_id", categoryMappings[prefix.CategoryId]);
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
        public async Task InsertRuns(List<Run> runs, Dictionary<int, int> prefixMappings)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            int batchNumber = 0;
            int totalBatches = runs.Count / 100;

            foreach (Run[] batch in runs.Chunk(100))
            {
                batchNumber++;
                string sql = @"INSERT INTO runs (timestamp, racenumber, prefix, drivername, lane, runs.[index], reaction, et60, et330,
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
                using SqlCommand cmd = new(sql, conn);
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
            using SqlConnection conn = new(_connectionString);
            string sql = $"DELETE FROM {tableName}";

            await conn.OpenAsync();
            using SqlCommand cmd = new(sql, conn);
            cmd.CommandTimeout = 600;
            await cmd.ExecuteNonQueryAsync();
            await conn.CloseAsync();
        }

        #endregion Private helpers
    }
}