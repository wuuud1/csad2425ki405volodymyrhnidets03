using Client.Domain.Services.GameService;
using Client.Domain.Services.IStorageManager;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using System.IO;

namespace Client.Domain.Services.GameStorageManager.INI;

/// <summary>
/// Implementation of <see cref="IGameStorageManager"/> that uses INI serialization for saving and loading game states.
/// </summary>
public class GameStorageINIManager : IGameStorageManager
{
    /// <summary>
    /// Default folder path for saving and loading game files.
    /// </summary>
    private readonly string _defaultFolder = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\Games"));

    /// <summary>
    /// Loads a game state from an INI file.
    /// </summary>
    /// <returns>The loaded <see cref="GameState"/> object.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the INI file cannot be deserialized into a <see cref="GameStateINI"/>.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the file specified by the user does not exist.</exception>
    public GameState LoadGame()
    {
        GameStateINI readState = new();
        string path = GetPath(_defaultFolder, false);
        if (string.IsNullOrEmpty(path))
            return new GameState();

        var parser = new IniDataParser();
        IniData data = new FileIniDataParser().ReadFile(path);

        readState.Mode = data["Game"]["Mode"];
        readState.Status = data["Game"]["Status"];
        readState.ManPlayer = bool.TryParse(data["Game"]["ManPlayer"], out bool manPlayer) ? manPlayer : (bool?)null;
        readState.Board = data["Game"]["Board"].Split(';').ToList();

        return readState.ToGameState();
    }

    /// <summary>
    /// Saves a game state to an INI file.
    /// </summary>
    /// <param name="game">The game state to save.</param>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="GameState"/> cannot be serialized.</exception>
    public void SaveGame(GameState game)
    {
        var t = Directory.GetCurrentDirectory();
        GameStateINI state = new(game);
        string path = GetPath(_defaultFolder);

        if (string.IsNullOrEmpty(path))
            return;

        var data = new IniData();
        data["Game"]["Mode"] = state.Mode;
        data["Game"]["Status"] = state.Status;
        data["Game"]["ManPlayer"] = state.ManPlayer?.ToString() ?? "null";
        data["Game"]["Board"] = string.Join(";", state.Board);

        new FileIniDataParser().WriteFile(path, data);
    }

    /// <summary>
    /// Retrieves the file path for saving or loading INI files.
    /// </summary>
    /// <param name="defaultPath">The default folder path to use.</param>
    /// <param name="saveFile"><c>true</c> to get a save path, <c>false</c> to get a load path.</param>
    /// <returns>The file path as a string.</returns>
    private string GetPath(string defaultPath = "", bool saveFile = true)
    {
        string path = string.IsNullOrEmpty(defaultPath) ? "C:\\" : defaultPath;
        return saveFile ? GetPathToSaveIni(path) : GetPathToLoadIni(path);
    }

    /// <summary>
    /// Opens a dialog for selecting a file to save as INI.
    /// </summary>
    /// <param name="defaultPath">The default folder path for the dialog.</param>
    /// <returns>The selected file path, or <c>null</c> if the dialog is canceled.</returns>
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

    /// <summary>
    /// Opens a dialog for selecting an INI file to load.
    /// </summary>
    /// <param name="defaultPath">The default folder path for the dialog.</param>
    /// <returns>The selected file path, or <c>null</c> if the dialog is canceled.</returns>
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
