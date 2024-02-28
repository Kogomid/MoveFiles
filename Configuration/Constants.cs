namespace MoveFiles.Configuration;

public class Constants
{
    public const string CancelOption = "C";

    public const string ChangeDirectoryOption = "D";

    public const string QuitOption = "Q";

    public const string ChangeNameOption = "N";

    public const string ChangeNameSettingsOption = "DEFAULT";

    public static readonly List<string> DefaultFolderNameSettings =
        new() {"Documents", "Programs", "Video", "Photos", "Compressed", "Music"};
    
}