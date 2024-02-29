namespace MoveFiles.Configuration;

using ConsoleInteraction;
public class DirectoryNameAndExtensions
{
    public static readonly Dictionary<string, (string, string[])> FileTypes = new()
    {
        {"1", (DirectoryNames(0), new string[] {"*.txt", "*.doc", "*.docx", "*.odt", "*.pdf", "*.xls", "*.xlsx", "*.ods", "*.csv", "*.ppt", "*.pptx", "*.odp", "*.mdb", "*.accdb", "*.html", "*.htm"})},
        {"2", (DirectoryNames(1), new string[] {"*.exe", "*.app", "*.apk", "*.dmg", "*.dll", "*.so", "*.dylib", "*.py", "*.js", "*.rb", "*.php", "*.java", "*.class", "*.jar", "*.msi"})},
        {"3", (DirectoryNames(2), new string[] {"*.mp4", "*.mkv", "*.flv", "*.avi", "*.mov", "*.wmv", "*.webm"})},
        {"4", (DirectoryNames(3), new string[] {"*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.svg", "*.heic", "*.ico"})},
        {"5", (DirectoryNames(4), new string[] {"*.zip", "*.rar", "*.tar", "*.gz", "*.7z"})},
        {"6", (DirectoryNames(5), new string[] {"*.mp3", "*.m4a", "*.wav", "*.aac", "*.flac", "*.ogg", "*.m4a"})}
    };
    
    
    public static (string, string[]) GetDirectoryNameAndExtensions(string userOption)
    {
        FileTypes.TryGetValue(userOption, out var fileNameAndType);
        (string nameOfTheDirectory, string[] fileExtensions) = fileNameAndType;
        return (nameOfTheDirectory, fileExtensions);
    }

    public static string DirectoryNames(int option)
    {
        var config = ConfigurationManager.LoadConfiguration();
        List<string> directoryName = config.DirectoryNames;
        return directoryName[option];
    }
    
    public static void ResetDirectoryNamesToDefault()
    {
        var config = ConfigurationManager.LoadConfiguration();
        config.DirectoryNames = Constants.DefaultDirectoryNameSettings;
        ConfigurationManager.SaveConfiguration(config);
    }

    public static (string, string) ChangeDirectoryName()
    {
        string pickDirectory = ConsoleInteraction.GetUserDirectoryNameSelection();

        if (pickDirectory != String.Empty)
        {
            Console.WriteLine("Enter a new name for this folder.");
            string newName = ConsoleInteraction.GetInput();
            return (pickDirectory, newName);
        }
        return (string.Empty, string.Empty);
    }
}