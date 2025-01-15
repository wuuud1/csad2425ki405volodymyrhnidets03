using Client.Domain.Services.Settings.GameSettingsService;

namespace Client.Domain.Tests.Services.Settings;

[TestClass]
public class GameSettingsServiceTests
{
    private IGameSettingsService _service;

    [TestInitialize]
    public void Setup()
    {
        _service = new GameSettingsService();
    }

    [TestMethod]
    public void GetAvaiableGameModes_ReturnsAllGameModes()
    {
        var expectedModes = Enum.GetNames(typeof(GameMode));

        var result = _service.GetAvaiableGameModes();

        CollectionAssert.AreEquivalent(expectedModes.ToList(), result.ToList());
    }

    [TestMethod]
    public void GetGameMode_DefaultValue_IsNone()
    {
        var result = _service.GetGameMode();

        Assert.AreEqual(GameMode.None, result);
    }

    [TestMethod]
    public void ChangeGameMode_UpdatesGameMode()
    {
        var newMode = GameMode.ManvsAI;

        _service.ChangeGameMode(newMode);
        var result = _service.GetGameMode();

        Assert.AreEqual(newMode, result);
    }

    [TestMethod]
    public void IsAllSettingSet_DefaultSettings_ReturnsFalse()
    {
        var result = _service.IsAllSettingSet();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsAllSettingSet_ManVsAIModeWithoutSide_ReturnsFalse()
    {
        _service.ChangeGameMode(GameMode.ManvsAI);

        var result = _service.IsAllSettingSet();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsAllSettingSet_ManVsAIModeWithSide_ReturnsTrue()
    {
        _service.ChangeGameMode(GameMode.ManvsAI);
        _service.SetManPlayerSide(true);

        var result = _service.IsAllSettingSet();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsAllSettingSet_OtherModes_ReturnsTrue()
    {
        _service.ChangeGameMode(GameMode.ManvsMan);

        var result = _service.IsAllSettingSet();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetManPlayerSide_DefaultValue_IsNull()
    {
        var result = _service.GetManPlayerSide();

        Assert.IsNull(result);
    }

    [TestMethod]
    public void SetManPlayerSide_UpdatesManPlayerSide()
    {
        var side = true;

        _service.SetManPlayerSide(side);
        var result = _service.GetManPlayerSide();

        Assert.AreEqual(side, result);
    }
}
