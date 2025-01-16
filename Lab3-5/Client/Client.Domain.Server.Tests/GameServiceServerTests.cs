using Client.Domain.Services.GameServices;
using Client.Domain.Services.IStorageManager;
using Client.Domain.Services.Settings;
using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;
using FakeItEasy;
using System.IO.Ports;

namespace Client.Domain.Server.Tests;

[TestClass]
public class GameServiceServerTests
{
    private IGameStorageManager _storageManager;
    private IGameSettingsService _gameSettings;
    private IPortSettingsService _portSettings;
    private ISettingsService _settings;
    private GameService _gameService;
    private bool wait;

    /// <summary>
    /// Setup method for every test in the <see cref="GameServiceServerTests"/> class.
    /// Sets up mock dependencies and initializes the <see cref="GameService"/> instance.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        string portName = "COM2";
        int portSpeed = 9600;

        wait = true;
        _storageManager = A.Fake<IGameStorageManager>();
        _gameSettings = A.Fake<IGameSettingsService>();
        _settings = A.Fake<ISettingsService>();
        _portSettings = new PortSettingsService();

        A.CallTo(() => _settings.GetGameSettings()).Returns(_gameSettings);
        A.CallTo(() => _settings.GetPortSettings()).Returns(_portSettings);

        _portSettings.ChangePort(portName);
        _portSettings.ChangePortSpeed(portSpeed);

        _gameService = new(_storageManager, _settings);
    }

    [TestMethod]
    public void SendRequestForAIMove_ShouldInvokeMethodWrite()
    {
        var gameState = new GameState();
        gameState.Status = GameStatus.Ongoing;

        _gameService.SetGameState(gameState);

        _gameService.AddReceivedEventHandler(new(GotMoveFromAI));
        _gameService.SendRequestForAIMove();
        while (wait)
        {

        }


        gameState.Board[0, 0] = true;
        Assert.AreEqual(_gameService.GetGameState(), gameState);
    }

    private void GotMoveFromAI(object sender, SerialDataReceivedEventArgs e)
    {
        string strForReceive = String.Empty;
        strForReceive = _gameService.GetServerPort().ReadLine();

        int row = (int)char.GetNumericValue(strForReceive[0]);
        int column = (int)char.GetNumericValue(strForReceive[1]);

        _gameService.Move(row, column);

        wait = false;
    }
}