namespace MoveFiles.Configuration;

using FileManagement;
using ConsoleInteraction;
using Newtonsoft.Json;

public class ConfigurationManager
{
    static readonly string ConfigFile = "config.json";

    public string? DownloadPath { get; set; }
    public List<string>? FolderNames { get; set; }

    public static bool CheckIfConfigFileExists()
    {
        return File.Exists(ConfigFile);
    }

    public static void CreateConfigFile()
    {
        var config = new ConfigurationManager()
        {
            DownloadPath = "enter a new path",
            FolderNames = Constants.DefaultFolderNameSettings
        };
        try
        {
            SaveConfiguration(config);
        }
        catch (Exception e)
        {
            ConsoleInteraction.PrintMessage("An error occurred: " + e.Message);
            Console.ReadKey();
            Program.Main();
        }
    }

    public static void ChangeFolderNamesInConfig()
    {
        ConsoleInteraction.DisplayFolderNames();
        string pickFolder = ConsoleInteraction.GetUserFolderNameSelection();
        
        if (pickFolder.ToUpper() == Constants.CancelOption)
        {
            return;
        }
        if (pickFolder.ToUpper() == Constants.ChangeNameSettingsOption)
        {
            FolderNameAndExtensions.ResetFolderNamesToDefault();
        }
        else
        {
            FolderNameAndExtensions.ChangeFolderName(pickFolder);
        }

    }

    public static void ChangeDownloadPathInConfig()
    {
        string newDownloadPath = ConsoleInteraction.GetNewDownloadPath();

        if (newDownloadPath.ToUpper() == Constants.CancelOption)
        {
            return;
        }
        if (Directory.Exists(newDownloadPath))
        {
            var config = LoadConfiguration();
            config.DownloadPath = newDownloadPath;

            try
            {
                SaveConfiguration(config);
                ConsoleInteraction.PrintMessage("The path has been changed");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                ConsoleInteraction.PrintMessage("An error occurred: " + e.Message);
                Console.ReadKey();
                Program.Main();
            }
        }
        else if (!Directory.Exists(newDownloadPath))
        {
            ConsoleInteraction.PrintMessage("That path does not exist. Please try again.");
            ChangeDownloadPathInConfig();
        }
    }

    public static ConfigurationManager LoadConfiguration()
    {
        if (CheckIfConfigFileExists() == false)
        {
            CreateConfigFile();
        }
        try
        {
            string json = File.ReadAllText(ConfigFile);
            return JsonConvert.DeserializeObject<ConfigurationManager>(json);
        }
        catch
        {
            CreateConfigFile();
        }
        return null;
    }

    public static void SaveConfiguration(ConfigurationManager config)
    {
        try
        {
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigFile, json);
        }
        catch (Exception e)
        {
            ConsoleInteraction.PrintMessage($"An error occurred while saving configuration: {e.Message}");
        }
    }
}