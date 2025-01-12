namespace Client.Domain.Services.Settings.GameSettingsService;

public interface IGameSettingsService
{
    public IEnumerable<string> GetAvaiableGameModes();

    public GameMode GetGameMode();

    public bool? GetManPlayerSide();

    public void SetManPlayerSide(bool? value);

    public void ChangeGameMode(GameMode mode);

    public bool IsAllSettingSet();
}
