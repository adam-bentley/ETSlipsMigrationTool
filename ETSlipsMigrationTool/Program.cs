using ETSlipsMigrationTool.Helpers;
using ETSlipsMigrationTool.Interface;
using ETSlipsMigrationTool.Models;
using ETSlipsMigrationTool.Services.DestinationClients;
using Microsoft.Extensions.Configuration;

// Build the configuration
ConfigurationBuilder builder = new();
ConfigurationBuilderHelpers.BuildConfig(builder);
IConfiguration build = builder.Build();

// Create the database clients
ISourceDatabase source = new ETSlipsMigrationTool.Services.SourceClients.AzureSQLDatabaseClient(build.GetValue<string>("SourceConnectionString"));
IDestinationDatabase destination = new PostgresDatabaseClient(build.GetValue<string>("DestinationConnectionString"));

//Fetch the data from the source database
Console.WriteLine("Fetching data");
List<RaceEvent> raceEvents = await source.ListEvents();
List<Category> categories = await source.ListCategories();
List<Prefix> prefixes = await source.ListPrefixes();
List<Pair> pairs = await source.ListPairs();
List<Run> runs = await source.ListRuns();

// Delete any data from the destination database
Console.WriteLine("Deleting data");
await destination.DeleteRuns();
await destination.DeletePairs();
await destination.DeletePrefixes();
await destination.DeleteCategories();
await destination.DeleteEvents();

// Insert the new data into destination database
Console.WriteLine("Inserting data");
var raceEventMappings = await destination.InsertEvents(raceEvents);
var categoryMappings = await destination.InsertCategories(categories);
var prefixMappings = await destination.InsertPrefixes(prefixes, categoryMappings);
await destination.InsertPairs(pairs, raceEventMappings, categoryMappings);
await destination.InsertRuns(runs, prefixMappings);

Console.WriteLine("Operation completed");