namespace MoveFiles.Configuration;

public class Constants
{
    public const string CancelOption = "C";

    public const string ChangeDirectoryOption = "D";

    public const string QuitOption = "Q";

    public const string ChangeNameOption = "N";

    public const string DefaultNameSettingsOption = "DEFAULT";

    public static readonly List<string> DefaultDirectoryNameSettings =
        new() {"Documents", "Programs", "Video", "Photos", "Compressed", "Music"};

    public const string EnterNewPath = "no path";
}