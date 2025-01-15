using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;

namespace Client.Domain.Services.Settings;

/// <summary>
/// Interface for settings service providing methods to manage game and port settings.
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Gets the game settings service.
    /// </summary>
    /// <returns>An instance of <see cref="IGameSettingsService"/>.</returns>
    public IGameSettingsService GetGameSettings();

    /// <summary>
    /// Gets the port settings service.
    /// </summary>
    /// <returns>An instance of <see cref="IPortSettingsService"/>.</returns>
    public IPortSettingsService GetPortSettings();

    /// <summary>
    /// Checks if all settings have been configured.
    /// </summary>
    /// <returns><c>true</c> if all settings are configured; otherwise, <c>false</c>.</returns>
    public bool IsAllSettingSet();
}
