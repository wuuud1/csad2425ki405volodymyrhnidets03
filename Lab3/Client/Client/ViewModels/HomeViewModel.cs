using Client.Domain.Services;
using Client.Domain.Services.Settings;
using Client.Presentation.Services.Navigator;
using System.Windows;
using System.Windows.Input;

namespace Client.Presentation.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private ISettingsService _settings;

    #region Open Settings
    private ICommand _openSettingsCommand = default!;
    public ICommand OpenSettingsCommand => _openSettingsCommand ??= new RelayCommand(OnOpenSettingsCommandExecuted);
    private void OnOpenSettingsCommandExecuted(object o)
    {
        Navigator.NavigateTo<SettingsViewModel>();
    }
    #endregion

    #region Play game
    private ICommand _openGameCommand = default!;
    public ICommand OpenGameCommand => _openGameCommand ??= new RelayCommand(OnOpenGameCommandExecuted);
    private void OnOpenGameCommandExecuted(object o)
    {
        if (!_settings.IsAllSettingSet())
        {
            MessageBox.Show("You have not set all setting, i cannot start game!!");
            return;
        }

        Navigator.NavigateTo<GameViewModel>();
    }
    #endregion

    public HomeViewModel(INavigator navigator, ISettingsService settings) : base(navigator)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }
}
