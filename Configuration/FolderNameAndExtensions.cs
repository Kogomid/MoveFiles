namespace MoveFiles.Configuration;

public class FolderNameAndExtensions
{
    public static readonly Dictionary<string, (string, string[])> FileTypes = new()
    {
        {"1", ("Documents", new string[] {"*.txt", "*.doc", "*.docx", "*.odt", "*.pdf", "*.xls", "*.xlsx", "*.ods", "*.csv", "*.ppt", "*.pptx", "*.odp", "*.mdb", "*.accdb", "*.html", "*.htm"})},
        {"2", ("Programs", new string[] {"*.exe", "*.app", "*.dmg", "*.dll", "*.so", "*.dylib", "*.py", "*.js", "*.rb", "*.php", "*.java", "*.class", "*.jar", "*.msi"})},
        {"3", ("Video", new string[] {"*.mp4", "*.mkv", "*.flv", "*.avi", "*.mov", "*.wmv"})},
        {"4", ("Photos", new string[] {"*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.svg", "*.heic"})},
        {"5", ("Compressed", new string[] {"*.zip", "*.rar", "*.tar", "*.gz", "*.7z"})},
        {"6", ("Music", new string[] {"*.mp3", "*.m4a", "*.wav", "*.aac", "*.flac", "*.ogg", "*.m4a"})}
    };
    
    
    public static (string, string[]) GetFolderNameAndExtensions(string userOption)
    {
        FileTypes.TryGetValue(userOption, out var fileNameAndType);
        (string nameOfTheFolder, string[] fileExtensions) = fileNameAndType;
        return (nameOfTheFolder, fileExtensions);
    }
    
}