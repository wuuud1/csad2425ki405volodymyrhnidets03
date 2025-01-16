namespace Client.Domain.Services.Settings.GameSettingsService;

/// <summary>
/// Represents the different game modes available in the system.
/// </summary>
public enum GameMode
{
    /// <summary>
    /// No game mode selected.
    /// </summary>
    None,

    /// <summary>
    /// A game mode where one human player competes against another human player.
    /// </summary>
    ManvsMan,

    /// <summary>
    /// A game mode where a human player competes against an AI opponent.
    /// </summary>
    ManvsAI,

    /// <summary>
    /// A game mode where two AI opponents compete against each other.
    /// </summary>
    AIvsAI
}

