using Client.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client.Presentation.HostBuilders;

internal static class AddViewModelsHostBuilderExtension
{
    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddScoped<MainViewModel>();
            services.AddScoped<HomeViewModel>();
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<GameViewModel>();
        });
        return hostBuilder;
    }
}
