namespace Client.Domain.Services.ServerService;

/// <summary>
/// Represents the available commands for game operations.
/// </summary>
public enum GameCommand
{
    /// <summary>
    /// Starts a new game.
    /// </summary>
    NewGame,

    /// <summary>
    /// Loads a previously saved game.
    /// </summary>
    LoadGame,

    /// <summary>
    /// Saves the current game state.
    /// </summary>
    SaveGame
}
