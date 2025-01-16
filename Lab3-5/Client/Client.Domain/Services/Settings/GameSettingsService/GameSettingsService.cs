namespace Client.Domain.Services.Settings.GameSettingsService;

/// <summary>
/// Implementation of <see cref="IGameSettingsService"/> for managing game settings.
/// </summary>
public class GameSettingsService : IGameSettingsService
{
    /// <summary>
    /// The current game mode. Default is <see cref="GameMode.None"/>.
    /// </summary>
    private GameMode _currentGameMode = GameMode.None;

    /// <summary>
    /// The side chosen by the human player in "Man vs AI" mode. Default is <c>null</c>.
    /// </summary>
    private bool? _manPlayerSide = null;

    /// <inheritdoc/>
    public IEnumerable<string> GetAvaiableGameModes() => Enum.GetNames(typeof(GameMode));

    /// <inheritdoc/>
    public GameMode GetGameMode()
    {
        return _currentGameMode;
    }

    /// <inheritdoc/>
    public void ChangeGameMode(GameMode mode)
    {
        _currentGameMode = mode;
    }

    /// <inheritdoc/>
    public bool IsAllSettingSet()
    {
        return !(_currentGameMode == GameMode.None || (_currentGameMode == GameMode.ManvsAI && _manPlayerSide == null));
    }

    /// <inheritdoc/>
    public bool? GetManPlayerSide()
    {
        return _manPlayerSide;
    }

    /// <inheritdoc/>
    public void SetManPlayerSide(bool? value)
    {
        _manPlayerSide = value;
    }
}