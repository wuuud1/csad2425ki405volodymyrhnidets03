using Microsoft.Extensions.DependencyInjection;

namespace Client.Presentation.ViewModels;

/// <summary>
/// A locator class that provides access to different view models in the application. 
/// This class serves as a central point for resolving view model instances using the 
/// dependency injection container, ensuring that view models are retrieved through 
/// the service provider.
/// </summary>
public class ViewModelLocator
{
    /// <summary>
    /// Gets the <see cref="MainViewModel"/> instance from the service provider.
    /// </summary>
    public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();

    /// <summary>
    /// Gets the <see cref="HomeViewModel"/> instance from the service provider.
    /// </summary>
    public HomeViewModel HomeViewModel => App.ServiceProvider.GetRequiredService<HomeViewModel>();

    /// <summary>
    /// Gets the <see cref="SettingsViewModel"/> instance from the service provider.
    /// </summary>
    public SettingsViewModel SettingsViewModel => App.ServiceProvider.GetRequiredService<SettingsViewModel>();

    /// <summary>
    /// Gets the <see cref="GameViewModel"/> instance from the service provider.
    /// </summary>
    public GameViewModel GameViewModel => App.ServiceProvider.GetRequiredService<GameViewModel>();
}

