using Microsoft.Extensions.Configuration;

namespace ETSlipsMigrationTool.Helpers
{
    /// <summary>
    /// Static methods for building the configuration file.
    /// </summary>
    internal static class ConfigurationBuilderHelpers
    {
        /// <summary>
        /// Builds the configuration file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}