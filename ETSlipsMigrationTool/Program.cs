using ETSlipsMigrationTool.Helpers;
using ETSlipsMigrationTool.Interface;
using ETSlipsMigrationTool.Models;
using ETSlipsMigrationTool.Models.DatabaseClients;
using Microsoft.Extensions.Configuration;

// Build the configuration
ConfigurationBuilder builder = new();
ConfigurationBuilderHelpers.BuildConfig(builder);
IConfiguration build = builder.Build();

// Create the database clients
ISourceDatabase mySQL = new MySQLDatabaseClient(build.GetValue<string>("MySqlConnectionString"));
IDestinationDatabase azureSQL = new AzureSQLDatabaseClient(build.GetValue<string>("AzureSqlConnectionString"));

// Fetch the data from the source database
Console.WriteLine("Fetching data");
List<RaceEvent> raceEvents = await mySQL.ListEvents();
List<Category> categories = await mySQL.ListCategories();
List<Prefix> prefixes = await mySQL.ListPrefixes();
List<Pair> pairs = await mySQL.ListPairs();
List<Run> runs = await mySQL.ListRuns();

// Delete any data from the destination database
Console.WriteLine("Deleting data");
await azureSQL.DeleteRuns();
await azureSQL.DeletePairs();
await azureSQL.DeletePrefixes();
await azureSQL.DeleteCategories();
await azureSQL.DeleteEvents();

// Insert the new data into destination database
Console.WriteLine("Inserting data");
var raceEventMappings = await azureSQL.InsertEvents(raceEvents);
var categoryMappings = await azureSQL.InsertCategories(categories);
var prefixMappings = await azureSQL.InsertPrefixes(prefixes, categoryMappings);
await azureSQL.InsertPairs(pairs, raceEventMappings, categoryMappings);
await azureSQL.InsertRuns(runs, prefixMappings);

Console.WriteLine("Operation completed");
// extensions 