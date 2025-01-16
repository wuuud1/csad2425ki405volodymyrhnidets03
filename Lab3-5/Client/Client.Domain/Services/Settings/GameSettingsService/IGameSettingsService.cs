namespace Client.Domain.Services.Settings.GameSettingsService;

/// <summary>
/// Interface for managing game settings, including game modes and player preferences.
/// </summary>
public interface IGameSettingsService
{
    /// <summary>
    /// Gets a list of available game modes.
    /// </summary>
    /// <returns>An enumerable collection of game mode names as strings.</returns>
    public IEnumerable<string> GetAvaiableGameModes();

    /// <summary>
    /// Gets the current game mode.
    /// </summary>
    /// <returns>The current <see cref="GameMode"/>.</returns>
    public GameMode GetGameMode();

    /// <summary>
    /// Gets the side chosen by the human player in "Man vs AI" mode.
    /// </summary>
    /// <returns>A nullable <c>bool</c> where <c>true</c> represents one side, <c>false</c> represents the other, and <c>null</c> represents an unchosen side.</returns>
    public bool? GetManPlayerSide();

    /// <summary>
    /// Sets the side for the human player in "Man vs AI" mode.
    /// </summary>
    /// <param name="value">
    /// A nullable <c>bool</c> where <c>true</c> represents one side, <c>false</c> represents the other, and <c>null</c> indicates the side has not been chosen.
    /// </param>
    public void SetManPlayerSide(bool? value);

    /// <summary>
    /// Changes the current game mode.
    /// </summary>
    /// <param name="mode">The new <see cref="GameMode"/> to set.</param>
    public void ChangeGameMode(GameMode mode);

    /// <summary>
    /// Checks whether all game settings have been configured.
    /// </summary>
    /// <returns><c>true</c> if all settings are configured; otherwise, <c>false</c>.</returns>
    public bool IsAllSettingSet();
}

