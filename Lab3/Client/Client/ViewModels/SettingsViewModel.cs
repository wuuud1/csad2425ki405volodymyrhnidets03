using Client.Domain.Services;
using Client.Domain.Services.Settings;
using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;
using Client.Presentation.Services.Navigator;
using System.Windows;
using System.Windows.Input;

namespace Client.Presentation.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly ISettingsService _settings;
    private readonly IGameSettingsService _gameSettings;
    private readonly IPortSettingsService _portSettings;

    public IEnumerable<string> Ports => _portSettings.GetAvailablePorts();
    public string SelectedPort { get => _selectedPortName; set => _selectedPortName = value; }
    private string _selectedPortName;

    public IEnumerable<int> PortSpeeds => _portSettings.GetAvailablePortSpeeds();
    public int SelectedPortSpeed { get => _selectedPortSpeed; set => _selectedPortSpeed = value; }
    private int _selectedPortSpeed;

    public IEnumerable<string> GameModes => _gameSettings.GetAvaiableGameModes();
    public string SelectedGameMode { get => _selectedGameMode; set => _selectedGameMode = value; }
    private string _selectedGameMode;

    private bool? _selectedPlayerSide { get => _gameSettings.GetManPlayerSide(); }
    public double OpacityXSide => (_selectedPlayerSide == true) ? 0.8 : 0;
    public double OpacityOSide => (_selectedPlayerSide == false) ? 0.8 : 0;

    #region Save settings
    private ICommand _openHomePageCommand = default!;
    public ICommand OpenHomePageCommand => _openHomePageCommand ??= new RelayCommand(OnOpenHomeCommandExecuted);
    private void OnOpenHomeCommandExecuted(object o)
    {
        ChangePort(_selectedPortName);
        ChangePortSpeed(_selectedPortSpeed);
        ChangeGameMode(_selectedGameMode);

        Navigator.NavigateTo<HomeViewModel>();
    }
    #endregion

    #region Change player side
    private ICommand _changeSelectedPlayerSideCommand = default!;
    public ICommand ChangeSelectedPlayerSideCommand => _changeSelectedPlayerSideCommand ??= new RelayCommand(_changeSelectedPlayerSideCommandExecuted);
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


    public SettingsViewModel(INavigator navigator, ISettingsService settings) : base(navigator)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        _gameSettings = _settings.GetGameSettings() ?? throw new ArgumentNullException(nameof(_settings.GetGameSettings));
        _portSettings = _settings.GetPortSettings() ?? throw new ArgumentNullException(nameof(_settings.GetPortSettings));

        _selectedPortName = _portSettings.GetPortName();
        _selectedPortSpeed = _portSettings.GetPortSpeed();
        _selectedGameMode = _gameSettings.GetGameMode().ToString();
    }

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
