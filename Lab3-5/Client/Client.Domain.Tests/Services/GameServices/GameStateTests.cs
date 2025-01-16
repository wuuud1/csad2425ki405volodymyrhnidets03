using Client.Domain.Services.GameServices;

namespace Client.Domain.Tests.Services.GameServices;

[TestClass]
public class GameStateTests
{
    [TestMethod]
    public void GetServerBoardString_ReturnStringThatrRepresentBoard()
    {
        GameState state = new();
        state.Board = new bool?[,] { { true, true, true }, { false, false, null }, { null, null, null } };
        string expectedBoardString = "00x01x02x10o11o12 20 21 22 ";

        string receivedBoardString = state.GetServerBoardString();

        Assert.AreEqual(expectedBoardString, receivedBoardString);
    }

    [TestMethod]
    public void OperatorEqual_ReturnTrue_WhenItsSameReference()
    {
        var state = new GameState();

        Assert.IsTrue(state == state);
    }

    [TestMethod]
    public void OperatorEqual_ReturnFalse_WheOneOfThemIsNull()
    {
        var state = new GameState();

        Assert.IsFalse(state == null);
    }
}
