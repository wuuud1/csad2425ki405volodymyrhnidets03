using Client.Domain.Services.GameService;
using Client.Domain.Services.IStorageManager;
using Client.Domain.Services.ServerService;
using Client.Domain.Services.Settings;
using Client.Domain.Services.Settings.GameSettingsService;
using Client.Domain.Services.Settings.PortSettingsService;
using FakeItEasy;
using System.IO.Ports;

namespace Client.Domain.Tests;


/// <summary>
/// Unit tests for the <see cref="GameService"/> class.
/// These tests validate the behavior of various methods and commands within the <see cref="GameService"/>.
/// </summary>
[TestClass]
public class GameServiceTests
{
    private readonly IGameStorageManager _storageManager;
    private readonly IGameSettingsService _gameSettings;
    private readonly IPortSettingsService _portSettings;
    private readonly ISettingsService _settings;
    private readonly GameService _gameService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameServiceTests"/> class.
    /// Sets up mock dependencies and initializes the <see cref="GameService"/> instance.
    /// </summary>
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

    /// <summary>
    /// Tests the constructor of the <see cref="GameService"/> class.
    /// Ensures that <see cref="ArgumentNullException"/> is thrown when any of the constructor parameters is null.
    /// </summary>
    [TestMethod]
    public void Constructor_ThrowArgumentNullException_WhenArgumentsAreNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new GameService(_storageManager, null));
        Assert.ThrowsException<ArgumentNullException>(() => new GameService(null, _settings));
        Assert.ThrowsException<ArgumentNullException>(() => new GameService(null, null));
    }

    /// <summary>
    /// Tests the <see cref="GameService.InvokeGameCommand"/> method for the <see cref="GameCommand.NewGame"/> command.
    /// Verifies that a new game is created when the game state is in a "new" state.
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="GameService.InvokeGameCommand"/> method for the <see cref="GameCommand.LoadGame"/> command.
    /// Verifies that the game state is correctly loaded when the load command is executed.
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="GameService.InvokeGameCommand"/> method for the <see cref="GameCommand.SaveGame"/> command.
    /// Verifies that the game state is saved correctly when the save command is executed.
    /// </summary>
    [TestMethod]
    public void SaveGameCommand_ExecuteSaveGameCommand_WhenCallsSaveMethodWithGameState()
    {
        _gameService.InvokeGameCommand(GameCommand.SaveGame);

        A.CallTo(() => _storageManager.SaveGame(_gameService.GetGameState())).MustHaveHappened();
    }

    /// <summary>
    /// Tests the <see cref="GameService.GetServerPort"/> method.
    /// Verifies that the correct serial port is returned when querying the connected port.
    /// </summary>
    [TestMethod]
    public void GetServerPort_ReturnConnectedSerialPort_WhenReturnedNeededPort()
    {
        SerialPort port = A.Fake<SerialPort>();

        A.CallTo(() => _portSettings.ConnectedPort).Returns(port);

        var receivedPort = _gameService.GetServerPort();

        Assert.AreEqual(port, receivedPort);
    }

    /// <summary>
    /// Tests the <see cref="GameService.GetGameState"/> method.
    /// Verifies that the current game state is returned correctly.
    /// </summary>
    [TestMethod]
    public void GetGameState_ReturnCurentGameState_WhenReturnedNewGameState()
    {
        Assert.AreEqual(new GameState(), _gameService.GetGameState());
    }

    /// <summary>
    /// Tests the <see cref="GameService.Move"/> method when the game state remains unchanged.
    /// Verifies that the game state does not change after a move if the game status is "Draw".
    /// </summary>
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

    /// <summary>
    /// Tests the <see cref="GameService.Move"/> method when the cell coordinates are out of range.
    /// Verifies that an <see cref="ArgumentOutOfRangeException"/> is thrown for invalid coordinates.
    /// </summary>
    /// <param name="row">The row index of the cell.</param>
    /// <param name="col">The column index of the cell.</param>
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

    /// <summary>
    /// Tests the <see cref="GameService.Move"/> method when the board is changed predictably.
    /// Verifies that the game state is updated correctly after a valid move.
    /// </summary>
    /// <param name="row">The row index of the move.</param>
    /// <param name="col">The column index of the move.</param>
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
