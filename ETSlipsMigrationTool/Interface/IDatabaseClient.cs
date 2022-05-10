using ETSlipsMigrationTool.Models;

namespace ETSlipsMigrationTool.Interface
{
    internal interface IDatabaseClient
    {
        Task<List<RaceEvent>> ListEvents();

        Task<List<Category>> ListCategories();

        Task<List<Prefix>> ListPrefixes();

        Task<List<Pair>> ListPairs();

        Task<List<Run>> ListRuns();

        Task DeleteEvents();

        Task DeleteCategories();

        Task DeletePrefixs();

        Task DeletePairs();

        Task DeleteRuns();

        Task InsertEvents(List<RaceEvent> raceEvents);

        Task InsertCategories(List<Category> categories);

        Task InsertPrefixes(List<Prefix> prefixes);

        Task InsertPairs(List<Pair> pairs);

        Task InsertRuns(List<Run> runs);
    }
}