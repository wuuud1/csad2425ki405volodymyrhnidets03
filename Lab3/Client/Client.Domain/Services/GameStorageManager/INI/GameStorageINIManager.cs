using Client.Domain.Services.GameService;
using Client.Domain.Services.IStorageManager;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System.IO;

namespace Client.Domain.Services.GameStorageManager.INI;

public class GameStorageINIManager : IGameStorageManager
{
    private readonly string _defaultFolder = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\Games"));

    public GameState LoadGame()
    {
        GameStateINI readState = new();
        string path = GetPath(_defaultFolder, false);

        var parser = new IniDataParser();
        IniData data = new FileIniDataParser().ReadFile(path);

        readState.Mode = data["Game"]["Mode"];
        readState.Status = data["Game"]["Status"];
        readState.ManPlayer = bool.TryParse(data["Game"]["ManPlayer"], out bool manPlayer) ? manPlayer : (bool?)null;
        readState.Board = data["Game"]["Board"].Split(';').ToList();

        return readState.ToGameState();
    }

    public void SaveGame(GameState game)
    {
        var t = Directory.GetCurrentDirectory();
        GameStateINI state = new(game);
        string path = GetPath(_defaultFolder);

        var data = new IniData();
        data["Game"]["Mode"] = state.Mode;
        data["Game"]["Status"] = state.Status;
        data["Game"]["ManPlayer"] = state.ManPlayer?.ToString() ?? "null";
        data["Game"]["Board"] = string.Join(";", state.Board);

        new FileIniDataParser().WriteFile(path, data);
    }

    private string GetPath(string defaultPath = "", bool saveFile = true)
    {
        string path = string.IsNullOrEmpty(defaultPath) ? "C:\\" : defaultPath;
        return saveFile ? GetPathToSaveIni(path) : GetPathToLoadIni(path);
    }

    public static string GetPathToSaveIni(string defaultPath)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save INI File",
            Filter = "INI Files (*.ini)|*.ini|All Files (*.*)|*.*",
            DefaultExt = "ini",
            InitialDirectory = defaultPath
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            return saveFileDialog.FileName;
        }

        return null;
    }

    public static string GetPathToLoadIni(string defaultPath)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Load INI File",
            Filter = "INI Files (*.ini)|*.ini|All Files (*.*)|*.*",
            InitialDirectory = defaultPath
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            return openFileDialog.FileName;
        }

        return null; // User canceled
    }
}

