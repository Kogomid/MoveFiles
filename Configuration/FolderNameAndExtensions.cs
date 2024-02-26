namespace MoveFiles.Configuration;
using Configuration;
public class FolderNameAndExtensions
{
    public static readonly Dictionary<string, (string, string[])> FileTypes = new()
    {
        {"1", (FolderNames(0), new string[] {"*.txt", "*.doc", "*.docx", "*.odt", "*.pdf", "*.xls", "*.xlsx", "*.ods", "*.csv", "*.ppt", "*.pptx", "*.odp", "*.mdb", "*.accdb", "*.html", "*.htm"})},
        {"2", (FolderNames(1), new string[] {"*.exe", "*.app", "*.apk", "*.dmg", "*.dll", "*.so", "*.dylib", "*.py", "*.js", "*.rb", "*.php", "*.java", "*.class", "*.jar", "*.msi"})},
        {"3", (FolderNames(2), new string[] {"*.mp4", "*.mkv", "*.flv", "*.avi", "*.mov", "*.wmv", "*.webm"})},
        {"4", (FolderNames(3), new string[] {"*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.svg", "*.heic", "*.ico"})},
        {"5", (FolderNames(4), new string[] {"*.zip", "*.rar", "*.tar", "*.gz", "*.7z"})},
        {"6", (FolderNames(5), new string[] {"*.mp3", "*.m4a", "*.wav", "*.aac", "*.flac", "*.ogg", "*.m4a"})}
    };
    
    
    public static (string, string[]) GetFolderNameAndExtensions(string userOption)
    {
        FileTypes.TryGetValue(userOption, out var fileNameAndType);
        (string nameOfTheFolder, string[] fileExtensions) = fileNameAndType;
        return (nameOfTheFolder, fileExtensions);
    }

    public static string FolderNames(int option)
    {
        var config = ConfigurationManager.LoadConfiguration();
        List<string> FolderName = config.FolderNames;
        return FolderName[option];
    }
}