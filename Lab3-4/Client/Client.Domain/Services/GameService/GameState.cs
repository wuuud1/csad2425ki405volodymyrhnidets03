using Client.Domain.Services.Settings.GameSettingsService;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media.Media3D;

namespace Client.Domain.Services.GameService;

/// <summary>
/// Represents the state of a game, including the board configuration, game mode, 
/// status, and player information.
/// </summary>
public struct GameState
{
    /// <summary>
    /// Character representation for the 'X' cell.
    /// </summary>
    public const char CharCellX = 'x';

    /// <summary>
    /// Character representation for the 'O' cell.
    /// </summary>
    public const char CharCellO = 'o';

    /// <summary>
    /// Character representation for an empty cell.
    /// </summary>
    public const char CharEmptyCell = ' ';

    /// <summary>
    /// The dimension size of the game board (e.g., 3x3 grid).
    /// </summary>
    public const int CellDimensionSize = 3;

    /// <summary>
    /// The game board represented as a 2D array of nullable boolean values. 
    /// True represents 'X', false represents 'O', and null represents an empty cell.
    /// </summary>
    public bool?[,] Board;

    /// <summary>
    /// Gets the number of 'X' cells on the board.
    /// </summary>
    public int XNumber => Board.Cast<bool?>().Count(c => c == true);

    /// <summary>
    /// Gets the number of 'O' cells on the board.
    /// </summary>
    public int ONumber => Board.Cast<bool?>().Count(c => c == false);

    /// <summary>
    /// The mode of the game (e.g., single-player, multiplayer).
    /// </summary>
    public GameMode Mode;

    /// <summary>
    /// The current status of the game (e.g., ongoing, finished).
    /// </summary>
    public GameStatus Status;

    /// <summary>
    /// Specifies whether the human player is represented by 'X' (true) or 'O' (false), 
    /// or null if not applicable.
    /// </summary>
    public bool? ManPlayer;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameState"/> struct with 
    /// an empty board and default values for the mode and status.
    /// </summary>
    public GameState()
    {
        Board = new bool?[CellDimensionSize, CellDimensionSize];
        Mode = GameMode.None;
        Status = GameStatus.Ongoing;
    }

    /// <summary>
    /// Converts the game board into a server-compatible string representation.
    /// Each cell is encoded as a combination of row, column, and value.
    /// </summary>
    /// <returns>A string representing the current state of the board for the server.</returns>
    public string GetServerBoardString()
    {
        string board = "";
        char cellValue = ' ';
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                cellValue = Board[i, j] == true ? CharCellX : Board[i, j] == false ? CharCellO : CharEmptyCell;
                board += i.ToString() + j.ToString() + cellValue;
            }
        }
        return board;
    }

    /// <summary>
    /// Determines whether two <see cref="GameState"/> instances are equal.
    /// </summary>
    /// <param name="left">The first game state.</param>
    /// <param name="right">The second game state.</param>
    /// <returns>True if the two game states are equal; otherwise, false.</returns>
    public static bool operator ==(GameState? left, GameState? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two <see cref="GameState"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first game state.</param>
    /// <param name="right">The second game state.</param>
    /// <returns>True if the two game states are not equal; otherwise, false.</returns>
    public static bool operator !=(GameState? left, GameState? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Determines whether the current game state is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current game state.</param>
    /// <returns>True if the current game state is equal to the specified object; otherwise, false.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null || obj.GetType() != typeof(GameState)) return false;

        var state = (GameState)obj;

        return Board.Cast<bool?>().SequenceEqual(state.Board.Cast<bool?>())
            && (Mode == state.Mode)
            && (Status == state.Status)
            && (ManPlayer == state.ManPlayer);
    }
}

