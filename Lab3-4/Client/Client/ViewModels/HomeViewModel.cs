using Client.Domain.Services;
using Client.Domain.Services.Settings;
using Client.Presentation.Services.Navigator;
using System.Windows;
using System.Windows.Input;

namespace Client.Presentation.ViewModels;

/// <summary>
/// ViewModel for the Home page, responsible for navigating to other views
/// and checking if settings are properly configured before starting the game.
/// </summary>
public class HomeViewModel : BaseViewModel
{
    private ISettingsService _settings;

    #region Open Settings

    /// <summary>
    /// Command to navigate to the Settings page.
    /// </summary>
    private ICommand _openSettingsCommand = default!;

    /// <summary>
    /// Gets the command to open the Settings page.
    /// </summary>
    public ICommand OpenSettingsCommand => _openSettingsCommand ??= new RelayCommand(OnOpenSettingsCommandExecuted);

    /// <summary>
    /// Executes the command to navigate to the Settings page.
    /// </summary>
    /// <param name="o">Optional parameter for command execution.</param>
    private void OnOpenSettingsCommandExecuted(object o)
    {
        Navigator.NavigateTo<SettingsViewModel>();
    }

    #endregion

    #region Play game

    /// <summary>
    /// Command to start a new game if settings are properly configured.
    /// </summary>
    private ICommand _openGameCommand = default!;

    /// <summary>
    /// Gets the command to open the game if settings are valid.
    /// </summary>
    public ICommand OpenGameCommand => _openGameCommand ??= new RelayCommand(OnOpenGameCommandExecuted);

    /// <summary>
    /// Executes the command to start the game after validating the settings.
    /// </summary>
    /// <param name="o">Optional parameter for command execution.</param>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
    /// </summary>
    /// <param name="navigator">The navigator for page navigation.</param>
    /// <param name="settings">The settings service for checking if the settings are properly configured.</param>
    /// <exception cref="ArgumentNullException">Thrown when settings is null.</exception>
    public HomeViewModel(INavigator navigator, ISettingsService settings) : base(navigator)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }
}

