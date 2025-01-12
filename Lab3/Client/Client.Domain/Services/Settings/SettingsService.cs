using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;

namespace Client.Domain.Services.Settings;

public class SettingsService : ISettingsService
{
    private readonly IGameSettingsService _gameSettingsService;
    private readonly IPortSettingsService _portSettingsService;

    public SettingsService(IGameSettingsService gameSettingsService, IPortSettingsService portSettingsService)
    {
        _gameSettingsService = gameSettingsService ?? throw new ArgumentNullException(nameof(gameSettingsService));
        _portSettingsService = portSettingsService ?? throw new ArgumentNullException(nameof(portSettingsService));
    }

    public IGameSettingsService GetGameSettings() => _gameSettingsService;

    public IPortSettingsService GetPortSettings() => _portSettingsService;

    public bool IsAllSettingSet()
    {
        return _gameSettingsService.IsAllSettingSet() && _portSettingsService.IsAllSettingSet();
    }
}
