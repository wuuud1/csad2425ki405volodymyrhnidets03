using Client.Domain.Services;
using Client.Domain.Services.Settings;
using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;
using Client.Presentation.Services.Navigator;
using System.Windows;
using System.Windows.Input;

namespace Client.Presentation.ViewModels;

/// <summary>
/// ViewModel for managing the settings page, responsible for interacting with settings services 
/// and updating the configuration for ports, game mode, and player side. 
/// Provides commands for saving settings and changing the selected player side.
/// </summary>
public class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settings;
    private readonly IGameSettingsService _gameSettings;
    private readonly IPortSettingsService _portSettings;

    /// <summary>
    /// Gets the available ports.
    /// </summary>
    public IEnumerable<string> Ports => _portSettings.GetAvailablePorts();

    /// <summary>
    /// Gets or sets the selected port.
    /// </summary>
    public string SelectedPort { get => _selectedPortName; set => _selectedPortName = value; }
    private string _selectedPortName;

    /// <summary>
    /// Gets the available port speeds.
    /// </summary>
    public IEnumerable<int> PortSpeeds => _portSettings.GetAvailablePortSpeeds();

    /// <summary>
    /// Gets or sets the selected port speed.
    /// </summary>
    public int SelectedPortSpeed { get => _selectedPortSpeed; set => _selectedPortSpeed = value; }
    private int _selectedPortSpeed;

    /// <summary>
    /// Gets the available game modes.
    /// </summary>
    public IEnumerable<string> GameModes => _gameSettings.GetAvaiableGameModes();

    /// <summary>
    /// Gets or sets the selected game mode.
    /// </summary>
    public string SelectedGameMode { get => _selectedGameMode; set => _selectedGameMode = value; }
    private string _selectedGameMode;

    /// <summary>
    /// Gets the selected player side (X or O).
    /// </summary>
    private bool? _selectedPlayerSide { get => _gameSettings.GetManPlayerSide(); }

    /// <summary>
    /// Gets the opacity value for the X side.
    /// </summary>
    public double OpacityXSide => (_selectedPlayerSide == true) ? 0.8 : 0;

    /// <summary>
    /// Gets the opacity value for the O side.
    /// </summary>
    public double OpacityOSide => (_selectedPlayerSide == false) ? 0.8 : 0;

    #region Save settings

    /// <summary>
    /// Command to navigate back to the home page and save the current settings.
    /// </summary>
    private ICommand _openHomePageCommand = default!;
    public ICommand OpenHomePageCommand => _openHomePageCommand ??= new RelayCommand(OnOpenHomeCommandExecuted);

    /// <summary>
    /// Executes the action to save settings and navigate to the home page.
    /// </summary>
    private void OnOpenHomeCommandExecuted(object o)
    {
        ChangePort(_selectedPortName);
        ChangePortSpeed(_selectedPortSpeed);
        ChangeGameMode(_selectedGameMode);

        Navigator.NavigateTo<HomeViewModel>();
    }

    #endregion

    #region Change player side

    /// <summary>
    /// Command to change the selected player side (X or O).
    /// </summary>
    private ICommand _changeSelectedPlayerSideCommand = default!;
    public ICommand ChangeSelectedPlayerSideCommand => _changeSelectedPlayerSideCommand ??= new RelayCommand(_changeSelectedPlayerSideCommandExecuted);

    /// <summary>
    /// Executes the action to change the selected player side.
    /// </summary>
    private void _changeSelectedPlayerSideCommandExecuted(object o)
    {
        if (o == null || o.GetType() != typeof(string))
            throw new ArgumentNullException(nameof(o));

        bool? selectedSide = (o.ToString().ToLower()[0] == 'x') ? true : false;

        _gameSettings.SetManPlayerSide(selectedSide);
        OnPropertyChanged(nameof(OpacityXSide));
        OnPropertyChanged(nameof(OpacityOSide));
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
    /// </summary>
    /// <param name="navigator">The navigator used for page navigation.</param>
    /// <param name="settings">The settings service for interacting with application settings.</param>
    public SettingsViewModel(INavigator navigator, ISettingsService settings) : base(navigator)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        _gameSettings = _settings.GetGameSettings() ?? throw new ArgumentNullException(nameof(_settings.GetGameSettings));
        _portSettings = _settings.GetPortSettings() ?? throw new ArgumentNullException(nameof(_settings.GetPortSettings));

        _selectedPortName = _portSettings.GetPortName();
        _selectedPortSpeed = _portSettings.GetPortSpeed();
        _selectedGameMode = _gameSettings.GetGameMode().ToString();
    }

    /// <summary>
    /// Changes the port setting.
    /// </summary>
    /// <param name="portName">The name of the port to select.</param>
    private void ChangePort(string portName)
    {
        try
        {
            _portSettings.ChangePort(portName);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    /// <summary>
    /// Changes the port speed setting.
    /// </summary>
    /// <param name="portSpeed">The port speed to set.</param>
    private void ChangePortSpeed(int portSpeed)
    {
        try
        {
            _portSettings.ChangePortSpeed(portSpeed);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    /// <summary>
    /// Changes the game mode setting.
    /// </summary>
    /// <param name="gameMode">The game mode to set.</param>
    private void ChangeGameMode(string gameMode)
    {
        try
        {
            _gameSettings.ChangeGameMode((GameMode)Enum.Parse(typeof(GameMode), gameMode));
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}

