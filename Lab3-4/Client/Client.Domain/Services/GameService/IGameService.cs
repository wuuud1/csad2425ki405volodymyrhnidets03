using Client.Domain.Services.GameService;
using System.IO.Ports;

namespace Client.Domain.Services.ServerService;

/// <summary>
/// Represents the interface for the game service, defining the contract for game-related operations.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Invokes a specific game command.
    /// </summary>
    /// <param name="command">The game command to invoke.</param>
    void InvokeGameCommand(GameCommand command);

    /// <summary>
    /// Retrieves the current state of the game.
    /// </summary>
    /// <returns>The current <see cref="GameState"/>.</returns>
    GameState GetGameState();

    /// <summary>
    /// Retrieves the server's serial port settings.
    /// </summary>
    /// <returns>The <see cref="SerialPort"/> object for server communication.</returns>
    SerialPort GetServerPort();

    /// <summary>
    /// Makes a move in the game at the specified board position.
    /// </summary>
    /// <param name="row">The row index of the move.</param>
    /// <param name="column">The column index of the move.</param>
    /// <returns>The updated <see cref="GameState"/>.</returns>
    GameState Move(int row, int column);

    /// <summary>
    /// Determines if there is a winner in the game.
    /// </summary>
    /// <returns>
    /// A nullable boolean indicating the winning side:
    /// <c>true</c> for Player X, <c>false</c> for Player O, or <c>null</c> for no winner.
    /// </returns>
    bool? IsWinner();

    /// <summary>
    /// Sends a request for the AI to make a move.
    /// </summary>
    void SendRequestForAIMove();

    /// <summary>
    /// Adds a handler for the event when serial data is received.
    /// </summary>
    /// <param name="handler">The event handler for serial data received.</param>
    void AddReceivedEventHandler(SerialDataReceivedEventHandler handler);
}
