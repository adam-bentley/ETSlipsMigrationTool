using ETSlipsMigrationTool.Models;

namespace ETSlipsMigrationTool.Interface
{
    /// <summary>
    /// An interface for the source database
    /// </summary>
    internal interface ISourceDatabase
    {
        #region List queries

        /// <summary>
        /// Gets a lists of all events from the events table.
        /// </summary>
        /// <returns>A list of events</returns>
        Task<List<RaceEvent>> ListEvents();

        /// <summary>
        /// Gets a lists of all categories from the categories table.
        /// </summary>
        /// <returns>A list of categories</returns>
        Task<List<Category>> ListCategories();

        /// <summary>
        /// Gets a lists of all prefixes from the prefixes table.
        /// </summary>
        /// <returns>A list of prefixes</returns>
        Task<List<Prefix>> ListPrefixes();

        /// <summary>
        /// Gets a lists of all pairs from the pairs table.
        /// </summary>
        /// <returns>A list of pairs</returns>
        Task<List<Pair>> ListPairs();

        /// <summary>
        /// Gets a lists of all runs from the runs table.
        /// </summary>
        /// <returns>A list of runs</returns>
        Task<List<Run>> ListRuns();

        #endregion List queries
    }
}