using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Client.Presentation.HostBuilders;

/// <summary>
/// Extension methods for configuring the <see cref="IHostBuilder"/> to load application settings.
/// </summary>
internal static class AddConfigurationHostBuilderExtensions
{
    /// <summary>
    /// Adds configuration sources to the <see cref="IHostBuilder"/>. This includes loading environment variables, 
    /// JSON configuration files, and setting the base directory for the configuration.
    /// </summary>
    /// <param name="hostBuilder">The <see cref="IHostBuilder"/> instance to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    /// <remarks>
    /// This method will configure the application to load:
    /// <list type="bullet">
    /// <item>Environment variables with a prefix of "PREFIX_"</item>
    /// <item>appsettings.json (if it exists)</item>
    /// <item>appsettings.{environmentName}.json (where environmentName is derived from the CORE_ENVIRONMENT environment variable, defaulting to "Development")</item>
    /// <item>All environment variables</item>
    /// </list>
    /// </remarks>
    public static IHostBuilder AddConfiguration(this IHostBuilder hostBuilder)
    {
        var location = AppContext.BaseDirectory;
        string environmentName = Environment.GetEnvironmentVariable("CORE_ENVIRONMENT") ?? "Development";
        Environment.SetEnvironmentVariable("BASEDIR", location);

        hostBuilder.ConfigureAppConfiguration(c =>
        {
            c.AddEnvironmentVariables("PREFIX_");
            c.SetBasePath(location);
            c.AddJsonFile("appsettings.json", optional: true);
            c.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
            c.AddEnvironmentVariables();
        });

        return hostBuilder;
    }
}

