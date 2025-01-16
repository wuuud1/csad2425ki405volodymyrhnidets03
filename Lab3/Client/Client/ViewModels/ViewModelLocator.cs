using Microsoft.Extensions.DependencyInjection;

namespace Client.Presentation.ViewModels;

public class ViewModelLocator
{
    public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();
    public HomeViewModel HomeViewModel => App.ServiceProvider.GetRequiredService<HomeViewModel>();
    public SettingsViewModel SettingsViewModel => App.ServiceProvider.GetRequiredService<SettingsViewModel>();
    public GameViewModel GameViewModel => App.ServiceProvider.GetRequiredService<GameViewModel>();
}
