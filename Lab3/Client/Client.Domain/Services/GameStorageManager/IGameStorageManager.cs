using Client.Domain.Services.GameService;

namespace Client.Domain.Services.IStorageManager;

public interface IGameStorageManager
{
    public void SaveGame(GameState game);

    public GameState LoadGame();
}
