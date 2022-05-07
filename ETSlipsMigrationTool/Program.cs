using ETSlipsMigrationTool.Interface;
using ETSlipsMigrationTool.Models;
using Microsoft.Extensions.Configuration;

// Build the configuration
ConfigurationBuilder builder = new();
BuildConfig(builder);
IConfiguration build = builder.Build();

IDatabaseClient mySQL = new MySQLDatabaseClient(build.GetValue<string>("MySqlConnectionString"));
IDatabaseClient azureSQL = new AzureSQLDatabaseClient(build.GetValue<string>("AzureSqlConnectionString"));

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables();
}

List<RaceEvent> raceEvents = await mySQL.ListEvents();
List<Category> categories = await mySQL.ListCategories();
List<Prefix> prefixes = await mySQL.ListPrefixes();
List<Pair> pairs = await mySQL.ListPairs();
List<Run> runs = await mySQL.ListRuns();

await azureSQL.DeleteRuns();
await azureSQL.DeletePairs();
await azureSQL.DeletePrefixes();
await azureSQL.DeleteCategories();
await azureSQL.DeleteEvents();

await azureSQL.InsertEvents(raceEvents);
await azureSQL.InsertCategories(categories);
await azureSQL.InsertPrefixes(prefixes);
await azureSQL.InsertPairs(pairs);
await azureSQL.InsertRuns(runs);

Console.WriteLine("Data loaded");