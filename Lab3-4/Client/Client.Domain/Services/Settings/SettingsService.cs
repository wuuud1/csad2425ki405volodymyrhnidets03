using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;

namespace Client.Domain.Services.Settings;

/// <summary>
/// Implementation of <see cref="ISettingsService"/> providing methods to manage game and port settings.
/// </summary>
public class SettingsService : ISettingsService
{
    /// <summary>
    /// Service for managing game settings.
    /// </summary>
    private readonly IGameSettingsService _gameSettingsService;

    /// <summary>
    /// Service for managing port settings.
    /// </summary>
    private readonly IPortSettingsService _portSettingsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsService"/> class.
    /// </summary>
    /// <param name="gameSettingsService">The game settings service.</param>
    /// <param name="portSettingsService">The port settings service.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="gameSettingsService"/> or <paramref name="portSettingsService"/> is <c>null</c>.
    /// </exception>
    public SettingsService(IGameSettingsService gameSettingsService, IPortSettingsService portSettingsService)
    {
        _gameSettingsService = gameSettingsService ?? throw new ArgumentNullException(nameof(gameSettingsService));
        _portSettingsService = portSettingsService ?? throw new ArgumentNullException(nameof(portSettingsService));
    }

    /// <inheritdoc/>
    public IGameSettingsService GetGameSettings() => _gameSettingsService;

    /// <inheritdoc/>
    public IPortSettingsService GetPortSettings() => _portSettingsService;

    /// <inheritdoc/>
    public bool IsAllSettingSet()
    {
        return _gameSettingsService.IsAllSettingSet() && _portSettingsService.IsAllSettingSet();
    }
}
