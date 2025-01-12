using Client.Domain.Services.GameService;
using Client.Domain.Services.IStorageManager;
using Client.Domain.Services.ServerService;
using Client.Domain.Services.Settings;
using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;
using FakeItEasy;
using System.IO.Ports;

namespace Client.Domain.Tests;

[TestClass]
public class GameServiceTests
{
    private readonly IGameStorageManager _storageManager;
    private readonly IGameSettingsService _gameSettings;
    private readonly IPortSettingsService _portSettings;
    private readonly ISettingsService _settings;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _storageManager = A.Fake<IGameStorageManager>();
        _gameSettings = A.Fake<IGameSettingsService>();
        _portSettings = A.Fake<IPortSettingsService>();
        _settings = A.Fake<ISettingsService>();

        A.CallTo(() => _settings.GetGameSettings()).Returns(_gameSettings);
        A.CallTo(() => _settings.GetPortSettings()).Returns(_portSettings);

        _gameService = new(_storageManager, _settings);
    }

    [TestMethod]
    public void Constructor_ThrowArgumentNullException_WhenArgumentsAreNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new GameService(_storageManager, null));
        Assert.ThrowsException<ArgumentNullException>(() => new GameService(null, _settings));
        Assert.ThrowsException<ArgumentNullException>(() => new GameService(null, null));
    }

    [TestMethod]
    public void InvokeGameCommand_ExecuteNewGameCommand_WhenStateIsNew()
    {
        var gameState = new GameState();
        gameState.Mode = GameMode.ManvsMan;

        A.CallTo(() => _gameSettings.GetGameMode()).Returns(gameState.Mode);

        _gameService.InvokeGameCommand(GameCommand.NewGame);

        A.CallTo(() => _settings.GetGameSettings().GetGameMode()).MustHaveHappened();
        Assert.AreEqual(gameState, _gameService.GetGameState());
    }

    [TestMethod]
    public void InvokeGameCommand_ExecuteLoadGameCommand_WhenCallsLoadMethodAndStateIsPredicted()
    {
        var gameState = new GameState();
        gameState.Mode = GameMode.ManvsMan;

        A.CallTo(() => _storageManager.LoadGame()).Returns(gameState);

        _gameService.InvokeGameCommand(GameCommand.LoadGame);

        A.CallTo(() => _storageManager.LoadGame()).MustHaveHappened();
        Assert.AreEqual(gameState, _gameService.GetGameState());
    }

    [TestMethod]
    public void SaveGameCommand_ExecuteSaveGameCommand_WhenCallsSaveMethodWithGameState()
    {
        _gameService.InvokeGameCommand(GameCommand.SaveGame);

        A.CallTo(() => _storageManager.SaveGame(_gameService.GetGameState())).MustHaveHappened();
    }

    [TestMethod]
    public void GetServerPort_ReturnConnectedSerialPort_WhenReturnedNeededPort()
    {
        SerialPort port = A.Fake<SerialPort>();

        A.CallTo(() => _portSettings.ConnectedPort).Returns(port);

        var receivedPort = _gameService.GetServerPort();

        Assert.AreEqual(port, receivedPort);
    }

    [TestMethod]
    public void GetGameState_ReturnCurentGameState_WhenReturnedNewGameState()
    {
        Assert.AreEqual(new GameState(), _gameService.GetGameState());
    }

    [TestMethod]
    public void Move_ReturnGameState_WhenGameStateUnchanged()
    {
        int randCellCoordinate = 0;
        var gameState = new GameState();
        gameState.Status = GameStatus.Draw;
        gameState.Mode = GameMode.ManvsMan;

        A.CallTo(() => _storageManager.LoadGame()).Returns(gameState);
        _gameService.InvokeGameCommand(GameCommand.LoadGame);

        _gameService.Move(randCellCoordinate, randCellCoordinate);

        Assert.AreEqual(gameState, _gameService.GetGameState());
    }

    [TestMethod]
    [DataRow(-1, 0)]
    [DataRow(0, -1)]
    [DataRow(-1, -1)]
    [DataRow(GameState.CellDimensionSize, 0)]
    [DataRow(0, GameState.CellDimensionSize)]
    [DataRow(GameState.CellDimensionSize, GameState.CellDimensionSize)]
    public void Move_ThrowArgumentOutOfRangeException_WhenCoordinateCellDoNotExist(int row, int col)
    {
        var gameState = new GameState();
        gameState.Status = GameStatus.Ongoing;
        gameState.Mode = GameMode.ManvsMan;

        A.CallTo(() => _storageManager.LoadGame()).Returns(gameState);
        _gameService.InvokeGameCommand(GameCommand.LoadGame);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => _gameService.Move(row, col));
    }

    [TestMethod]
    [DataRow(0, 0)]
    public void Move_MakeMove_WhenBoardChangedPredictably(int row, int col)
    {
        var gameState = new GameState();
        gameState.Status = GameStatus.Ongoing;
        gameState.Mode = GameMode.ManvsMan;

        A.CallTo(() => _storageManager.LoadGame()).Returns(gameState);
        _gameService.InvokeGameCommand(GameCommand.LoadGame);

        _gameService.Move(row, col);
        var currentState = _gameService.GetGameState();

        gameState.Board[row, col] = true;
        Assert.AreEqual(gameState, currentState);
    }
}