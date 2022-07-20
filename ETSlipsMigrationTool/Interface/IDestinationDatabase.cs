using ETSlipsMigrationTool.Models;

namespace ETSlipsMigrationTool.Interface
{
    /// <summary>
    /// An interface for the destination database
    /// </summary>
    internal interface IDestinationDatabase
    {
        #region Delete queries

        /// <summary>
        /// Deletes all events from the events table.
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task DeleteEvents();

        /// <summary>
        /// Deletes all categories from the categories table.
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task DeleteCategories();

        /// <summary>
        /// Deletes all prefixes from the prefixes table.
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task DeletePrefixes();

        /// <summary>
        /// Deletes all pairs from the pairs table.
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task DeletePairs();

        /// <summary>
        /// Deletes all runs from the runs table.
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task DeleteRuns();

        #endregion Delete queries

        #region Insert queries

        /// <summary>
        /// Inserts a list of events into the events table.
        /// </summary>
        /// <param name="raceEvents">The list of race events.</param>
        /// <returns>A dictionary of old ID to new ID mappings</returns>
        Task<Dictionary<int, int>> InsertEvents(List<RaceEvent> raceEvents);

        /// <summary>
        /// Inserts a list of categories into the categories table.
        /// </summary>
        /// <param name="categories">The list of categories</param>
        /// <returns>A dictionary of old ID to new ID mappings</returns>
        Task<Dictionary<int, int>> InsertCategories(List<Category> categories);

        /// <summary>
        /// Inserts a list of prefixes into the prefixes table.
        /// </summary>
        /// <param name="raceEvents">The list of prefixes.</param>
        /// <returns>A dictionary of old ID to new ID mappings</returns>
        Task<Dictionary<int, int>> InsertPrefixes(List<Prefix> prefixes, Dictionary<int, int> categoryIdMappings);

        /// <summary>
        /// Inserts a list of pairs into the pairs table.
        /// </summary>
        /// <param name="pairs">The list of pairs.</param>
        /// <param name="eventMappings">The event id mappings.</param>
        /// <param name="categoryMappings">The category id mappings.</param>
        /// <returns>An awaitable task</returns>
        Task InsertPairs(List<Pair> pairs, Dictionary<int, int> eventMappings, Dictionary<int, int> categoryMappings);

        /// <summary>
        /// Inserts a list of runs into the runs table.
        /// </summary>
        /// <param name="runs">The list of runs.</param>
        /// <param name="prefixMappings">The prefix id mappings.</param>
        /// <returns>
        /// An awaitable task
        /// </returns>
        Task InsertRuns(List<Run> runs, Dictionary<int, int> prefixMappings);

        #endregion Insert queries
    }
}