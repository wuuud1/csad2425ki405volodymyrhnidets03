using Client.Domain.Services;
using Client.Domain.Services.GameService;
using Client.Domain.Services.ServerService;
using Client.Domain.Services.Settings.GameSettingsService;
using Client.Presentation.Services.Navigator;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Client.Presentation.ViewModels;

/// <summary>
/// ViewModel for managing the game's state, user interaction, and communication with the game service.
/// It handles commands for starting, loading, saving games, making moves, and navigating between views.
/// </summary>
public class GameViewModel : BaseViewModel
{
    /// <summary>
    /// Represents the images on the game board.
    /// </summary>
    public ObservableCollection<ObservableCollection<BitmapImage>> BoardImages { get; set; } = new()
    {
        new() { new(), new(), new() },
        new() { new(), new(), new() },
        new() { new(), new(), new() }
    };

    private readonly IGameService _gameService;
    private GameState _gameState => _gameService.GetGameState();

    #region Start new game

    /// <summary>
    /// Command to start a new game.
    /// </summary>
    public ICommand StartNewGameCommand => _startNewGameCommand ??= new RelayCommand(OnStartNewGameCommandExecuted);

    private ICommand _startNewGameCommand = default!;

    private void OnStartNewGameCommandExecuted(object o)
    {
        _gameService.InvokeGameCommand(GameCommand.NewGame);
        ChangeBoardView(_gameState);

        if (IsNextMoveOfAI())
            _gameService.SendRequestForAIMove();
    }

    #endregion

    #region Load game

    /// <summary>
    /// Command to load a saved game.
    /// </summary>
    public ICommand LoadGameCommand => _loadGameCommand ??= new RelayCommand(OnLoadGameCommandExecuted);

    private ICommand _loadGameCommand = default!;

    private void OnLoadGameCommandExecuted(object o)
    {
        _gameService.InvokeGameCommand(GameCommand.LoadGame);
        ChangeBoardView(_gameState);

        if (IsNextMoveOfAI())
            _gameService.SendRequestForAIMove();
    }

    #endregion

    #region Save game

    /// <summary>
    /// Command to save the current game state.
    /// </summary>
    public ICommand SaveGameCommand => _saveGameCommand ??= new RelayCommand(OnSaveGameCommandExecuted);

    private ICommand _saveGameCommand = default!;

    private void OnSaveGameCommandExecuted(object o)
    {
        _gameService.InvokeGameCommand(GameCommand.SaveGame);
    }

    #endregion

    #region Return to home page

    /// <summary>
    /// Command to navigate back to the home page.
    /// </summary>
    public ICommand ReturnToHomePageCommand => _returnToHomePageCommand ??= new RelayCommand(OnReturnToHomePageCommandExecuted);

    private ICommand _returnToHomePageCommand = default!;

    private void OnReturnToHomePageCommandExecuted(object o)
    {
        Navigator.NavigateTo<HomeViewModel>();
    }

    #endregion

    #region Try move

    /// <summary>
    /// Command that is executed when a cell on the board is clicked.
    /// </summary>
    public ICommand CellClickedCommand => _cellClickedCommand ??= new RelayCommand(OnCellClickedCommandExecuted);

    private ICommand _cellClickedCommand = default!;

    private void OnCellClickedCommandExecuted(object o)
    {
        if (_gameState.Status != GameStatus.Ongoing)
            return;

        if (_gameState.Mode == GameMode.AIvsAI)
            return;

        bool queryAI = (_gameState.XNumber > _gameState.ONumber && _gameState.ManPlayer == true)
                    || (_gameState.XNumber == _gameState.ONumber && _gameState.ManPlayer == false);
        if (_gameState.Mode == GameMode.ManvsAI && queryAI)
            return;

        if (o == null || o.GetType() != typeof(string))
            throw new ArgumentException(nameof(o));

        string move = o as string;

        MakeMove(move);

        if (IsNextMoveOfAI())
            _gameService.SendRequestForAIMove();
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GameViewModel"/> class.
    /// </summary>
    /// <param name="navigator">The navigator instance used for view navigation.</param>
    /// <param name="service">The game service that manages the game logic and state.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="service"/> is null.</exception>
    public GameViewModel(INavigator navigator, IGameService service) : base(navigator)
    {
        _gameService = service ?? throw new ArgumentNullException(nameof(service));

        _gameService.AddReceivedEventHandler(new(GotMoveFromAI));
    }

    /// <summary>
    /// Makes a move on the game board based on the provided move string.
    /// </summary>
    /// <param name="move">A string representing the move, typically in the format "rowcolumn".</param>
    private void MakeMove(string move)
    {
        int row = (int)char.GetNumericValue(move[0]);
        int column = (int)char.GetNumericValue(move[1]);

        ChangeBoardView(_gameService.Move(row, column));

        CheckWinner();

        if (IsNextMoveOfAI())
            _gameService.SendRequestForAIMove();
    }

    /// <summary>
    /// Updates the board view with the current game state.
    /// </summary>
    /// <param name="state">The current game state containing the updated board information.</param>
    private void ChangeBoardView(GameState state)
    {
        var DefaultCellPath = (BitmapImage)Application.Current.Resources["DefaultCell"];
        var CrossCellPath = (BitmapImage)Application.Current.Resources["CrosstCell"];
        var ZeroCellPath = (BitmapImage)Application.Current.Resources["ZeroCell"];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var image = (state.Board[i, j] == null) ? DefaultCellPath : (state.Board[i, j] == true) ? CrossCellPath : ZeroCellPath;
                BoardImages[i][j] = image;
            }
        }
    }

    /// <summary>
    /// Checks the game state to determine if there is a winner or a draw, and displays a message if there is.
    /// </summary>
    private void CheckWinner()
    {
        if (_gameService.IsWinner() != null)
            MessageBox.Show("Player" + ((_gameService.IsWinner() == true) ? "X" : "O") + " WON!!!!");

        if (_gameState.Status == GameStatus.Draw)
            MessageBox.Show("DRAW!!!!");
    }

    /// <summary>
    /// Determines if it is the AI's turn to make a move.
    /// </summary>
    /// <returns>True if it is the AI's turn, false otherwise.</returns>
    private bool IsNextMoveOfAI()
    {
        return (_gameState.Mode == GameMode.AIvsAI || (_gameState.Mode == GameMode.ManvsAI && IsManMoved())) && _gameState.Mode != GameMode.ManvsMan && _gameState.Status == GameStatus.Ongoing;
    }

    /// <summary>
    /// Determines if the human player has made a move in the "Man vs AI" game mode.
    /// </summary>
    /// <returns>True if the human player has made a move, false otherwise.</returns>
    private bool IsManMoved()
    {
        return (_gameState.Mode == GameMode.ManvsAI)
            && ((_gameState.ManPlayer == true && _gameState.XNumber > _gameState.ONumber)
            || (_gameState.ManPlayer == false && _gameState.XNumber == _gameState.ONumber));
    }

    /// <summary>
    /// Handles the move received from the AI and updates the game state accordingly.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments containing the AI move data.</param>
    private void GotMoveFromAI(object sender, SerialDataReceivedEventArgs e)
    {
        string strForReceive = String.Empty;
        Application.Current.Dispatcher.Invoke(() =>
        {
            strForReceive = _gameService.GetServerPort().ReadLine();
            MakeMove(strForReceive);
        });
    }
}
