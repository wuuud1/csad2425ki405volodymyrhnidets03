using Client.Presentation.HostBuilders;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Client;

public partial class App : Application
{
    private IHost _host;
    public static IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        _host = CreateHostBuilder().Build();
        ServiceProvider = _host.Services;
    }

    private static IHostBuilder CreateHostBuilder(string[] args = null)
    {
        return Host.CreateDefaultBuilder(args)
            .AddConfiguration()
            .AddServices()
            .AddViewModels()
            .AddViews();
    }
}
