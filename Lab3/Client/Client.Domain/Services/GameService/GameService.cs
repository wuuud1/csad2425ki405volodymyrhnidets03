using Client.Domain.Services.IStorageManager;
using Client.Domain.Services.ServerService;
using Client.Domain.Services.Settings;
using Client.Domain.Services.Settings.GameSettingsService;
using System.IO.Ports;

namespace Client.Domain.Services.GameService;

public class GameService : IGameService
{
    private readonly ISettingsService _settings;
    private readonly IGameStorageManager _storageManager;
    private Dictionary<GameCommand, Action> GameCommands;
    private GameState _gameState;

    public GameService(IGameStorageManager storageManager, ISettingsService settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _storageManager = storageManager ?? throw new ArgumentNullException(nameof(storageManager));

        _gameState = new GameState();

        AddImplementationForGameCommands();
    }

    public void InvokeGameCommand(GameCommand command)
    {
        GameCommands[command].Invoke();
    }

    public SerialPort GetServerPort() => _settings.GetPortSettings().ConnectedPort;

    public GameState GetGameState()
    {
        return _gameState;
    }

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

    public bool? IsWinner()
    {
        return CheckRows() ?? CheckColumns() ?? CheckDiagonals();
    }

    public void SendRequestForAIMove()
    {
        GetServerPort().Write(_gameState.GetServerBoardString());
    }

    public void AddReceivedEventHandler(SerialDataReceivedEventHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        _settings.GetPortSettings().AddSerialDataReceivedEventHandler(handler);
    }

    private void AddImplementationForGameCommands()
    {
        GameCommands = new()
        {
            { GameCommand.NewGame,  StartNewGameCommand},
            { GameCommand.LoadGame,  LoadGameCommand},
            { GameCommand.SaveGame,  SaveGameCommand}
        };
    }

    private void StartNewGameCommand()
    {
        _gameState = new GameState();
        _gameState.Mode = _settings.GetGameSettings().GetGameMode();

        if (_gameState.Mode == GameMode.ManvsAI)
            _gameState.ManPlayer = _settings.GetGameSettings().GetManPlayerSide();
    }

    private void LoadGameCommand()
    {
        try
        {
            _gameState = _storageManager.LoadGame();
            _settings.GetGameSettings().SetManPlayerSide(_gameState.ManPlayer);
            _settings.GetGameSettings().ChangeGameMode(_gameState.Mode);
        }
        catch (Exception)
        {

        }
    }

    private void SaveGameCommand()
    {
        _storageManager.SaveGame(_gameState);
    }

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

    private bool? CheckColumns()
    {
        bool? winSide = null, result = null;

        for (int i = 0; i < GameState.CellDimensionSize; i++)
        {
            winSide = _gameState.Board[0, i];

            if (!winSide.HasValue)
                break;

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

    private bool? CheckDiagonals()
    {
        bool? winSide = CheckMainDiagonal() ?? CheckOtherDiagonal();

        return winSide;
    }

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
