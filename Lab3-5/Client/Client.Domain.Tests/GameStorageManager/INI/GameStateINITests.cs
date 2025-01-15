using Client.Domain.Services.GameServices;
using Client.Domain.Services.GameStorageManager.INI;
using Client.Domain.Services.Settings.GameSettingsService;

namespace Client.Domain.Tests.GameStorageManager.XML;

[TestClass]
public class GameStateINITests
{
    [TestMethod]
    public void Constructor_ShouldInitializeEmptyBoard()
    {
        var gameStateXML = new GameStateINI();

        Assert.IsNotNull(gameStateXML.Board);
        Assert.AreEqual(0, gameStateXML.Board.Count);
    }

    [TestMethod]
    public void Constructor_ShouldConvertGameStateToGameStateINI()
    {
        var gameState = new GameState
        {
            Board = new bool?[,]
            {
                    { true, false, null },
                    { null, true, false }
            },
            Mode = GameMode.ManvsMan,
            Status = GameStatus.Ongoing,
            ManPlayer = true
        };

        var gameStateXML = new GameStateINI(gameState);

        Assert.AreEqual(GameMode.ManvsMan, Enum.Parse<GameMode>(gameStateXML.Mode));
        Assert.AreEqual(GameStatus.Ongoing, Enum.Parse<GameStatus>(gameStateXML.Status));
        Assert.AreEqual(true, gameStateXML.ManPlayer);
        Assert.AreEqual(2, gameStateXML.Board.Count);
        Assert.AreEqual(3, gameStateXML.Board[0].Split(",").Count());
        Assert.AreEqual(true, bool.Parse(gameStateXML.Board[0].Split(",").FirstOrDefault()));
        Assert.AreEqual("null", gameStateXML.Board[1].Split(",").FirstOrDefault() );
    }

    [TestMethod]
    public void ToGameState_ShouldConvertGameStateINIToGameState()
    {
        var board = new List<string>
            {
                "false,false,null",
                "true,false,null",
                "null,true,false"
            };

        var gameStateXML = new GameStateINI
        {
            Board = board,
            Mode = GameMode.ManvsAI.ToString(),
            Status = GameStatus.WonPlayerO.ToString(),
            ManPlayer = false
        };

        var gameState = gameStateXML.ToGameState();

        Assert.AreEqual(GameMode.ManvsAI, gameState.Mode);
        Assert.AreEqual(GameStatus.WonPlayerO, gameState.Status);
        Assert.AreEqual(false, gameState.ManPlayer);
        Assert.AreEqual(board.Count, gameState.Board.GetLength(0));
        Assert.AreEqual(3, gameState.Board.GetLength(1));
        Assert.AreEqual(false, gameState.Board[0, 0]);
        Assert.AreEqual(true, gameState.Board[1, 0]);
    }
}
