namespace Client.Domain.Services.GameService;

/// <summary>
/// Represents the possible statuses of a game.
/// </summary>
public enum GameStatus
{
    /// <summary>
    /// The game is ongoing.
    /// </summary>
    Ongoing,

    /// <summary>
    /// The game ended in a draw.
    /// </summary>
    Draw,

    /// <summary>
    /// Player X won the game.
    /// </summary>
    WonPlayerX,

    /// <summary>
    /// Player O won the game.
    /// </summary>
    WonPlayerO
}
