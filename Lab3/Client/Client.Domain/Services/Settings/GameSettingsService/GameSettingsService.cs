namespace Client.Domain.Services.Settings.GameSettingsService;

public class GameSettingsService : IGameSettingsService
{
    private GameMode _currentGameMode = GameMode.None;

    private bool? _manPlayerSide = null;

    public IEnumerable<string> GetAvaiableGameModes() => Enum.GetNames(typeof(GameMode));

    public GameMode GetGameMode()
    {
        return _currentGameMode;
    }

    public void ChangeGameMode(GameMode mode)
    {
        _currentGameMode = mode;
    }

    public bool IsAllSettingSet()
    {
        return !(_currentGameMode == GameMode.None || (_currentGameMode == GameMode.ManvsAI && _manPlayerSide == null));
    }

    public bool? GetManPlayerSide()
    {
        return _manPlayerSide;
    }

    public void SetManPlayerSide(bool? value)
    {
        _manPlayerSide = value;
    }
}
