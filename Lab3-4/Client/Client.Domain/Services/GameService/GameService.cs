using Client.Domain.Services.IStorageManager;
using Client.Domain.Services.ServerService;
using Client.Domain.Services.Settings;
using Client.Domain.Services.Settings.GameSettingsService;
using System.IO.Ports;

namespace Client.Domain.Services.GameService;

/// <summary>
/// Provides the core game logic for managing the game state, handling moves,
/// invoking commands, and interacting with settings and storage.
/// </summary>
public class GameService : IGameService
{
    private readonly ISettingsService _settings;
    private readonly IGameStorageManager _storageManager;
    private Dictionary<GameCommand, Action> GameCommands;
    private GameState _gameState;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameService"/> class.
    /// </summary>
    /// <param name="storageManager">The storage manager for saving and loading games.</param>
    /// <param name="settings">The settings service for game configurations.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="storageManager"/> or <paramref name="settings"/> is null.
    /// </exception>
    public GameService(IGameStorageManager storageManager, ISettingsService settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _storageManager = storageManager ?? throw new ArgumentNullException(nameof(storageManager));

        _gameState = new GameState();

        AddImplementationForGameCommands();
    }

    /// <summary>
    /// Invokes the specified game command.
    /// </summary>
    /// <param name="command">The command to invoke.</param>
    public void InvokeGameCommand(GameCommand command)
    {
        GameCommands[command].Invoke();
    }

    /// <summary>
    /// Gets the serial port connected to the game server.
    /// </summary>
    /// <returns>The connected <see cref="SerialPort"/> instance.</returns>
    public SerialPort GetServerPort() => _settings.GetPortSettings().ConnectedPort;

    /// <summary>
    /// Retrieves the current game state.
    /// </summary>
    /// <returns>The current <see cref="GameState"/>.</returns>
    public GameState GetGameState()
    {
        return _gameState;
    }

    /// <summary>
    /// Updates the game board with a move at the specified position.
    /// </summary>
    /// <param name="row">The row index of the move.</param>
    /// <param name="column">The column index of the move.</param>
    /// <returns>The updated <see cref="GameState"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified position is out of bounds.
    /// </exception>
    public GameState Move(int row, int column)
    {
        if (_gameState.Status != GameStatus.Ongoing)
            return _gameState;

        if (row < 0 || column < 0
         || row > GameState.CellDimensionSize - 1 || column > GameState.CellDimensionSize - 1)
            throw new ArgumentOutOfRangeException(nameof(row), nameof(column));

        ChangeBoard(row, column);

        return _gameState;
    }

    /// <summary>
    /// Determines the winner of the game.
    /// </summary>
    /// <returns>
    /// True if 'X' is the winner, false if 'O' is the winner, or null if there is no winner.
    /// </returns>
    public bool? IsWinner()
    {
        return CheckRows() ?? CheckColumns() ?? CheckDiagonals();
    }

    /// <summary>
    /// Sends a request for the AI to make a move based on the current game state.
    /// </summary>
    public void SendRequestForAIMove()
    {
        GetServerPort().Write(_gameState.GetServerBoardString());
    }

    /// <summary>
    /// Adds a handler for the serial data received event.
    /// </summary>
    /// <param name="handler">The event handler to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when the handler is null.</exception>
    public void AddReceivedEventHandler(SerialDataReceivedEventHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        _settings.GetPortSettings().AddSerialDataReceivedEventHandler(handler);
    }

    /// <summary>
    /// Initializes the game commands with their respective implementations.
    /// </summary>
    private void AddImplementationForGameCommands()
    {
        GameCommands = new()
        {
            { GameCommand.NewGame,  StartNewGameCommand},
            { GameCommand.LoadGame,  LoadGameCommand},
            { GameCommand.SaveGame,  SaveGameCommand}
        };
    }

    /// <summary>
    /// Starts a new game and initializes the game state.
    /// </summary>
    private void StartNewGameCommand()
    {
        _gameState = new GameState();
        _gameState.Mode = _settings.GetGameSettings().GetGameMode();

        if (_gameState.Mode == GameMode.ManvsAI)
            _gameState.ManPlayer = _settings.GetGameSettings().GetManPlayerSide();
    }

    /// <summary>
    /// Loads a saved game from storage and updates the game settings.
    /// </summary>
    private void LoadGameCommand()
    {
        try
        {
            var gameState = _storageManager.LoadGame();
            if(gameState != new GameState())
            {
                _gameState = gameState;
                _settings.GetGameSettings().SetManPlayerSide(_gameState.ManPlayer);
                _settings.GetGameSettings().ChangeGameMode(_gameState.Mode);
            }
        }
        catch (Exception)
        {

        }
    }

    /// <summary>
    /// Saves the current game state to storage.
    /// </summary>
    private void SaveGameCommand()
    {
        _storageManager.SaveGame(_gameState);
    }

    /// <summary>
    /// Updates the game board and checks for a winner or draw condition.
    /// </summary>
    /// <param name="row">The row index of the move.</param>
    /// <param name="column">The column index of the move.</param>
    private void ChangeBoard(int row, int column)
    {
        int maxXNumber = (GameState.CellDimensionSize * GameState.CellDimensionSize) / 2 + 1;

        if (_gameState.Board[row, column] == null)
            _gameState.Board[row, column] = _gameState.XNumber == _gameState.ONumber;

        if (IsWinner() != null)
        {
            _gameState.Status = (IsWinner() == true) ? GameStatus.WonPlayerX : GameStatus.WonPlayerO;
        }

        if (_gameState.XNumber == maxXNumber && IsWinner() == null)
            _gameState.Status = GameStatus.Draw;
    }

    /// <summary>
    /// Checks for a winning condition in the rows.
    /// </summary>
    /// <returns>The winning player, if any.</returns>
    private bool? CheckRows()
    {
        bool? winSide = null, result = null;

        for (int i = 0; i < GameState.CellDimensionSize; i++)
        {
            winSide = _gameState.Board[i, 0];

            if (!winSide.HasValue)
                break;

            for (int j = 1; j < GameState.CellDimensionSize; j++)
            {
                if (!_gameState.Board[i, j].HasValue || _gameState.Board[i, j] != winSide)
                    break;

                if (j == GameState.CellDimensionSize - 1)
                    result = winSide;
            }
        }

        return result;
    }

    /// <summary>
    /// Checks for a winning condition in the columns.
    /// </summary>
    /// <returns>The winning player, if any.</returns>
    private bool? CheckColumns()
    {
        bool? winSide = null, result = null;

        for (int i = 0; i < GameState.CellDimensionSize; i++)
        {
            winSide = _gameState.Board[0, i];

            if (!winSide.HasValue)
                continue;

            for (int j = 1; j < GameState.CellDimensionSize; j++)
            {
                if (!_gameState.Board[j, i].HasValue || _gameState.Board[j, i] != winSide)
                    break;

                if (j == GameState.CellDimensionSize - 1)
                    result = winSide;
            }
        }

        return result;
    }

    /// <summary>
    /// Checks for a winning condition in the diagonals.
    /// </summary>
    /// <returns>The winning player, if any.</returns>
    private bool? CheckDiagonals()
    {
        bool? winSide = CheckMainDiagonal() ?? CheckOtherDiagonal();

        return winSide;
    }

    /// <summary>
    /// Checks the main diagonal for a winning condition.
    /// </summary>
    /// <returns>The winning player, if any.</returns>
    private bool? CheckMainDiagonal()
    {
        bool? winSide = null;

        if (!_gameState.Board[0, 0].HasValue)
            return null;

        winSide = _gameState.Board[0, 0];

        for (int i = 1; i < GameState.CellDimensionSize; i++)
        {
            if (!_gameState.Board[i, i].HasValue || _gameState.Board[i, i] != winSide)
                return null;
        }

        return winSide;
    }

    /// <summary>
    /// Checks the other diagonal for a winning condition.
    /// </summary>
    /// <returns>The winning player, if any.</returns>
    private bool? CheckOtherDiagonal()
    {
        bool? winSide = _gameState.Board[0, GameState.CellDimensionSize - 1];

        if (!winSide.HasValue)
            return null;

        for (int i = 0; i < GameState.CellDimensionSize; i++)
        {
            bool? cell = _gameState.Board[i, GameState.CellDimensionSize - 1 - i];

            if (!cell.HasValue || cell != winSide)
                return null;
        }

        return winSide;
    }
}
