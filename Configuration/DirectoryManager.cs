namespace MoveFiles.Configuration;

using NLog;

public class DirectoryManager
{
    static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public static string GetDownloadDirectoryPath()
    {
        var config = ConfigurationManager.LoadConfiguration();
        return config.DownloadPath;
    }

    public static string GetSubDirectoryPath(string nameOfTheDirectory)
    {
        var config = ConfigurationManager.LoadConfiguration();
        string downloadPathDirectory = config.DownloadPath;
        string subDirectoryPath = Path.Combine(downloadPathDirectory, nameOfTheDirectory);

        return (subDirectoryPath);
    }

    public static void CreateANewSubDirectory(string subDirectoryPath)
    {
        try
        {
            Directory.CreateDirectory(subDirectoryPath);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error creating the directory. Please check the path and try again.");
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