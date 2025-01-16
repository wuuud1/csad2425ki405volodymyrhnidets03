using Client.Domain.Services.GameServices;
using Client.Domain.Services.Settings.GameSettingsService;

namespace Client.Domain.Services.GameStorageManager.INI;

[Serializable]
/// <summary>
/// Serializable representation of the game state, used for INI storage.
/// </summary>
public class GameStateINI
{
    /// <summary>
    /// The current game mode.
    /// </summary>
    public string Mode { get; set; }
    /// <summary>
    /// The current status of the game.
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// Indicates which player is the human player, if applicable.
    /// </summary>
    public bool? ManPlayer { get; set; }
    /// <summary>
    /// The game board represented as a string list.
    /// </summary>
    public List<string> Board { get; set; }

    /// <summary>
    /// Default constructor. Initializes the board as an empty list.
    /// </summary>
    public GameStateINI()
    {
        Board = new List<string>();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="GameStateINI"/> based on the given <see cref="GameState"/>.
    /// </summary>
    /// <param name="state">The <see cref="GameState"/> to convert into a serializable format.</param>
    public GameStateINI(GameState state)
    {
        Board = ConvertBoardToStrings(state.Board);
        Mode = state.Mode.ToString();
        Status = state.Status.ToString();
        ManPlayer = state.ManPlayer;
    }

    /// <summary>
    /// Converts this <see cref="GameStateINI"/> instance back into a <see cref="GameState"/> object.
    /// </summary>
    /// <returns>The deserialized <see cref="GameState"/> object.</returns>
    public GameState ToGameState()
    {
        GameState state = new();
        state.Board = ConvertStringsToBoard(Board);
        state.Mode = Enum.Parse<GameMode>(Mode);
        state.Status = Enum.Parse<GameStatus>(Status);
        state.ManPlayer = ManPlayer;
        return state;
    }

    /// <summary>
    /// Converts a two-dimensional nullable boolean array into a string list.
    /// </summary>
    /// <param name="array">The two-dimensional array to convert.</param>
    /// <returns>A string list representing the array.</returns>
    private static List<string> ConvertBoardToStrings(bool?[,] array)
    {
        var rows = new List<string>();
        for (int i = 0; i < array.GetLength(0); i++)
        {
            rows.Add(string.Join(",", Enumerable.Range(0, array.GetLength(1)).Select(j => array[i, j]?.ToString() ?? "null")));
        }
        return rows;
    }

    /// <summary>
    /// Converts a string list of nullable booleans into a two-dimensional array.
    /// </summary>
    /// <param name="rows">The string list to convert.</param>
    /// <returns>A two-dimensional array representing the list.</returns>
    /// <remarks>
    /// If the input list is empty or contains empty rows, the returned array will have zero dimensions.
    /// </remarks>
    private static bool?[,] ConvertStringsToBoard(List<string> rows)
    {
        if (rows.Count == 0)
            return new bool?[0, 0];

        int cols = rows[0].Split(',').Length;
        var board = new bool?[rows.Count, cols];

        for (int i = 0; i < rows.Count; i++)
        {
            var cells = rows[i].Split(',');
            for (int j = 0; j < cells.Length; j++)
            {
                board[i, j] = cells[j] == "null" ? (bool?)null : bool.Parse(cells[j]);
            }
        }

        return board;
    }
}
