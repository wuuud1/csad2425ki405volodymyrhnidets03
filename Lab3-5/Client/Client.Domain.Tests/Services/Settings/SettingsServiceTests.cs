using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;
using Client.Domain.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;

namespace Client.Domain.Tests.Services.Settings;

[TestClass]
public class SettingsServiceTests
{

    private IGameSettingsService _fakeGameSettingsService;
    private IPortSettingsService _fakePortSettingsService;
    private SettingsService _settingsService;

    public SettingsServiceTests()
    {
        _fakeGameSettingsService = A.Fake<IGameSettingsService>();
        _fakePortSettingsService = A.Fake<IPortSettingsService>();

        _settingsService = new(_fakeGameSettingsService, _fakePortSettingsService);
    }

    [TestMethod]
    public void Constructor_ShouldThrowArgumentNullException_WhenArgumentsAreNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new SettingsService(_fakeGameSettingsService, null));
        Assert.ThrowsException<ArgumentNullException>(() => new SettingsService(null, _fakePortSettingsService));
        Assert.ThrowsException<ArgumentNullException>(() => new SettingsService(null, null));
    }

    [TestMethod]
    public void GetGameSettings_ShouldReturnGameSettingsService()
    {
        var result = _settingsService.GetGameSettings();

        Assert.AreEqual(_fakeGameSettingsService, result);
    }

    [TestMethod]
    public void GetPortSettings_ShouldReturnPortSettingsService()
    {
        var result = _settingsService.GetPortSettings();

        Assert.AreEqual(_fakePortSettingsService, result);
    }

    [TestMethod]
    public void IsAllSettingSet_ShouldReturnTrue_WhenBothServicesAreSet()
    {
        A.CallTo(() => _fakeGameSettingsService.IsAllSettingSet()).Returns(true);
        A.CallTo(() => _fakePortSettingsService.IsAllSettingsSet()).Returns(true);

        var result = _settingsService.IsAllSettingSet();

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsAllSettingSet_ShouldReturnFalse_WhenGameSettingsAreNotSet()
    {
        A.CallTo(() => _fakeGameSettingsService.IsAllSettingSet()).Returns(false);
        A.CallTo(() => _fakePortSettingsService.IsAllSettingsSet()).Returns(true);

        var result = _settingsService.IsAllSettingSet();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsAllSettingSet_ShouldReturnFalse_WhenPortSettingsAreNotSet()
    {
        A.CallTo(() => _fakeGameSettingsService.IsAllSettingSet()).Returns(true);
        A.CallTo(() => _fakePortSettingsService.IsAllSettingsSet()).Returns(false);

        var result = _settingsService.IsAllSettingSet();

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsAllSettingSet_ShouldReturnFalse_WhenBothServicesAreNotSet()
    {
        A.CallTo(() => _fakeGameSettingsService.IsAllSettingSet()).Returns(false);
        A.CallTo(() => _fakePortSettingsService.IsAllSettingsSet()).Returns(false);

        var result = _settingsService.IsAllSettingSet();

        Assert.IsFalse(result);
    }
}
