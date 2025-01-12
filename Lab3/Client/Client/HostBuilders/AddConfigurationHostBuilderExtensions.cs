using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Client.Presentation.HostBuilders;

internal static class AddConfigurationHostBuilderExtensions
{
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
