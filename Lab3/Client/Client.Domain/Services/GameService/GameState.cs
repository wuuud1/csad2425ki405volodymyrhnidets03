using Client.Domain.Services.Settings.GameSettingsService;
using System.Diagnostics.CodeAnalysis;

namespace Client.Domain.Services.GameService;

public struct GameState
{
    public const char CharCellX = 'x';
    public const char CharCellO = 'o';
    public const char CharEmptyCell = ' ';
    public const int CellDimensionSize = 3;

    public bool?[,] Board;
    public int XNumber => Board.Cast<bool?>().Count(c => c == true);
    public int ONumber => Board.Cast<bool?>().Count(c => c == false);
    public GameMode Mode;
    public GameStatus Status;
    public bool? ManPlayer;

    public GameState()
    {
        Board = new bool?[CellDimensionSize, CellDimensionSize];
        Mode = GameMode.None;
        Status = GameStatus.Ongoing;
    }

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

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null) return false;

        if (obj.GetType() != typeof(GameState)) return false;

        var state = (GameState)obj;

        return Board.Cast<bool?>().SequenceEqual(state.Board.Cast<bool?>())
            && (Mode == state.Mode)
            && (Status == state.Status)
            && (ManPlayer == state.ManPlayer);
    }
}
