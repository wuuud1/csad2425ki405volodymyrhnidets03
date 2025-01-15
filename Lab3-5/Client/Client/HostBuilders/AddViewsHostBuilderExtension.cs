using Client.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client.Presentation.HostBuilders;

/// <summary>
/// Extension method for configuring views and view models in the <see cref="IHostBuilder"/>.
/// </summary>
internal static class AddViewsHostBuilderExtension
{
    /// <summary>
    /// Adds view and view model services to the <see cref="IHostBuilder"/> for dependency injection.
    /// </summary>
    /// <param name="host">The <see cref="IHostBuilder"/> instance to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    /// <remarks>
    /// This method registers the following views and view models as scoped services:
    /// <list type="bullet">
    /// <item><see cref="MainWindow"/></item>
    /// <item><see cref="MainViewModel"/></item>
    /// <item><see cref="HomeViewModel"/></item>
    /// <item><see cref="SettingsViewModel"/></item>
    /// <item><see cref="GameViewModel"/></item>
    /// </list>
    /// These views and view models will be resolved from the dependency injection container when needed.
    /// </remarks>
    public static IHostBuilder AddViews(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddScoped<MainWindow>();

            services.AddScoped<MainViewModel>();
            services.AddScoped<HomeViewModel>();
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<GameViewModel>();
        });

        return host;
    }
}

