using Client.Domain.Services.GameService;
using System.IO.Ports;

namespace Client.Domain.Services.ServerService;

public interface IGameService
{
    public void InvokeGameCommand(GameCommand command);

    public GameState GetGameState();

    public SerialPort GetServerPort();

    public GameState Move(int row, int column);

    public bool? IsWinner();

    public void SendRequestForAIMove();

    public void AddReceivedEventHandler(SerialDataReceivedEventHandler handler);
}
