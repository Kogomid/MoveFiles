namespace MoveFiles.Configuration;
using ConsoleInteraction;
public class DirectoryManager
{
    public static string GetDownloadFolderPath()
    {
        var config = ConfigurationManager.LoadConfiguration();
        return config.DownloadPath;
    }
    
    public static string GetSubFolderPath(string nameOfTheFolder)
    {
        var config = ConfigurationManager.LoadConfiguration();
        string downloadPathFolder = config.DownloadPath;
        string subFolderPath = Path.Combine(downloadPathFolder, nameOfTheFolder);

        return (subFolderPath);
    }

    public static void CreateANewSubFolder(string subFolderPath)
    {
        try
        {
            Directory.CreateDirectory(subFolderPath);
        }
        catch
        {
            ConsoleInteraction.PrintMessage("Error creating the directory. Please check the path and try again.");
        }
    }

    public static bool CheckIfDownloadPathExists()
    {
        try
        {
            var config = ConfigurationManager.LoadConfiguration();
            return Directory.Exists(config.DownloadPath);
        }
        catch
        {
            return false;
        }
    }
}