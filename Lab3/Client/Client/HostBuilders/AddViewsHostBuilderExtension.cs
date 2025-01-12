using Client.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client.Presentation.HostBuilders;

internal static class AddViewsHostBuilderExtension
{
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
