using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;

namespace Client.Domain.Services.Settings;

public interface ISettingsService
{
    public IGameSettingsService GetGameSettings();

    public IPortSettingsService GetPortSettings();

    public bool IsAllSettingSet();
}
