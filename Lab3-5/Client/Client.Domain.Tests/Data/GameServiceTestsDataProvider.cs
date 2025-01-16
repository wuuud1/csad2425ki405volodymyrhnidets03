namespace Client.Domain.Tests.Data;

internal static class GameServiceTestsDataProvider
{
    public static IEnumerable<object[]> XPlayerWinnerBoards { get; } = new List<object[]>
    {
        new object[]
        {
            new bool?[,] { { true, true, true  }, { false, false, null}, { null, null, null } }
        },
        new object[]
        {
            new bool?[,] { { true, false, null }, { true, false, null}, { true, null, null } }
        },
        new object[]
        {
            new bool?[,] { { true, null, false }, { false, true, null}, { null, null, true } }
        },
    };

    public static IEnumerable<object[]> OPlayerWinnerBoards { get; } = new List<object[]>
    {
        new object[]
        {
            new bool?[,] { { false, false, false }, { true, true, null}, { null, null, null } }
        },
        new object[]
        {
            new bool?[,] { { false, true, null }, { false, true, null}, { false, null, null } }
        },
        new object[]
        {
            new bool?[,] { { false, null, true }, { true, false, null}, { null, null, false } }
        },
    };

    public static IEnumerable<object[]> NoWinnerBoards { get; } = new List<object[]>
    {
        new object[]
        {
            new bool?[,] { { null, null, null }, { null, null, null}, { null, null, null } }
        },
        new object[]
        {
            new bool?[,] { { false, true, false }, { null, null, true }, { false, true, null } }
        },
        new object[]
        {
            new bool?[,] { { false, true, null }, { false, true, null}, { null, null, null } }
        },
        new object[]
        {
            new bool?[,] { { true, true, null }, { false, false, null}, { null, null, null } }
        },
    };
}
