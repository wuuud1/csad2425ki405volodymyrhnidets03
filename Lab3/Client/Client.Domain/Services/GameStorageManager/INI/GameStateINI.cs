using Client.Domain.Services.GameService;
using Client.Domain.Services.Settings.GameSettingsService;

namespace Client.Domain.Services.GameStorageManager.INI;

[Serializable]
public class GameStateINI
{
    public string Mode { get; set; }
    public string Status { get; set; }
    public bool? ManPlayer { get; set; }
    public List<string> Board { get; set; }

    public GameStateINI()
    {
        Board = new List<string>();
    }

    public GameStateINI(GameState state)
    {
        Board = ConvertBoardToStrings(state.Board);
        Mode = state.Mode.ToString();
        Status = state.Status.ToString();
        ManPlayer = state.ManPlayer;
    }

    public GameState ToGameState()
    {
        GameState state = new();
        state.Board = ConvertStringsToBoard(Board);
        state.Mode = Enum.Parse<GameMode>(Mode);
        state.Status = Enum.Parse<GameStatus>(Status);
        state.ManPlayer = ManPlayer;
        return state;
    }

    private static List<string> ConvertBoardToStrings(bool?[,] array)
    {
        var rows = new List<string>();
        for (int i = 0; i < array.GetLength(0); i++)
        {
            rows.Add(string.Join(",", Enumerable.Range(0, array.GetLength(1)).Select(j => array[i, j]?.ToString() ?? "null")));
        }
        return rows;
    }

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

