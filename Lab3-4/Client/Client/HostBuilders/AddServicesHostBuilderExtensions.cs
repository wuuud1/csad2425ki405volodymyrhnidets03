using Client.Domain.Services.GameService;
using Client.Domain.Services.GameStorageManager.INI;
using Client.Domain.Services.IStorageManager;
using Client.Domain.Services.ServerService;
using Client.Domain.Services.Settings;
using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;
using Client.Presentation.Services.Navigator;
using Client.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client.Presentation.HostBuilders;

/// <summary>
/// Extension methods for configuring services in the <see cref="IHostBuilder"/>.
/// </summary>
internal static class AddServicesHostBuilderExtensions
{
    /// <summary>
    /// Adds necessary services to the <see cref="IHostBuilder"/> for dependency injection.
    /// </summary>
    /// <param name="hostBuilder">The <see cref="IHostBuilder"/> instance to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    /// <remarks>
    /// This method registers the following services with the dependency injection container:
    /// <list type="bullet">
    /// <item><see cref="INavigator"/> to <see cref="Navigator"/></item>
    /// <item><see cref="IGameSettingsService"/> to <see cref="GameSettingsService"/></item>
    /// <item><see cref="IPortSettingsService"/> to <see cref="PortSettingsService"/></item>
    /// <item><see cref="ISettingsService"/> to <see cref="SettingsService"/></item>
    /// <item><see cref="IGameStorageManager"/> to <see cref="GameStorageINIManager"/></item>
    /// <item><see cref="IGameService"/> to <see cref="GameService"/></item>
    /// <item>A scoped service factory to resolve <see cref="BaseViewModel"/> instances by type.</item>
    /// </list>
    /// </remarks>
    public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IGameSettingsService, GameSettingsService>();
            services.AddSingleton<IPortSettingsService, PortSettingsService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IGameStorageManager, GameStorageINIManager>();
            services.AddSingleton<IGameService, GameService>();
            services.AddScoped<Func<Type, BaseViewModel>>(serviceProvider => viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));
        });

        return hostBuilder;
    }
}

