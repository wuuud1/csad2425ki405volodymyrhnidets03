using Client.Domain.Services.GameService;

namespace Client.Domain.Services.IStorageManager;

/// <summary>
/// Interface for managing the saving and loading of game states.
/// </summary>
public interface IGameStorageManager
{
    /// <summary>
    /// Saves the current game state to a storage medium.
    /// </summary>
    /// <param name="game">The game state to be saved.</param>
    public void SaveGame(GameState game);

    /// <summary>
    /// Loads a game state from a storage medium.
    /// </summary>
    /// <returns>The loaded <see cref="GameState"/> object.</returns>
    public GameState LoadGame();
}
