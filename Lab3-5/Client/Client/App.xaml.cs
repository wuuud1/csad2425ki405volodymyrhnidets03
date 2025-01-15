using Client.Presentation.HostBuilders;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Client;

/// <summary>
/// Represents the entry point for the application. 
/// This class is responsible for setting up the application's services and view models, 
/// and initializing the main service provider for dependency injection.
/// </summary>
public partial class App : Application
{
    private IHost _host;

    /// <summary>
    /// Gets the service provider that can be used for resolving services throughout the application.
    /// </summary>
    public static IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// This constructor sets up the application's host and configures the service provider.
    /// </summary>
    public App()
    {
        _host = CreateHostBuilder().Build();
        ServiceProvider = _host.Services;
    }

    /// <summary>
    /// Creates and configures the host builder with various services, configurations, 
    /// and view models for the application.
    /// </summary>
    /// <param name="args">Optional arguments to pass to the host builder.</param>
    /// <returns>An <see cref="IHostBuilder"/> instance for configuring the application host.</returns>
    private static IHostBuilder CreateHostBuilder(string[] args = null)
    {
        return Host.CreateDefaultBuilder(args)
            .AddConfiguration()
            .AddServices()
            .AddViewModels()
            .AddViews();
    }
}

