namespace MoveFiles.Configuration;

using NLog;
using FileManagement;
using ConsoleInteraction;
using Newtonsoft.Json;

public class ConfigurationManager
{
    static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    static readonly string ConfigFile = "config.json";
    public string? DownloadPath { get; set; }
    public List<string>? DirectoryNames { get; set; }

    public static bool CheckIfConfigFileExists()
    {
        return File.Exists(ConfigFile);
    }

    public static void CreateConfigFile()
    {
        var config = new ConfigurationManager()
        {
            DownloadPath = Constants.EnterNewPath,
            DirectoryNames = Constants.DefaultDirectoryNameSettings
        };
        try
        {
            SaveConfiguration(config);
        }
        catch (Exception e)
        {
            Logger.Error(e,"An error occured while creating configuratuin.");
            Console.ReadKey();
            Program.Main();
        }
    }

    public static void ChangeDirectoryNamesInConfig()
    {
        (string pickDirectory, string newName) = DirectoryNameAndExtensions.ChangeDirectoryName();
        if (pickDirectory != String.Empty)
        {
            try
            {
                var config = LoadConfiguration();
                config.DirectoryNames[(Convert.ToInt32(pickDirectory) - 1)] = newName;
                SaveConfiguration(config);
            }
            catch (Exception e)
            {
                Logger.Error(e,"An error occured while changing name in config.");
            }
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
                Console.WriteLine($"An error occured: {e} ");
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
        if (!CheckIfConfigFileExists())
        {
            CreateConfigFile();
        }
        try
        {
            return LoadConfigFromFile();
        }
        catch (FileNotFoundException e)
        {
            Logger.Error(e, "Configuration file not found. A new one will be created.");
            CreateConfigFile();
            return LoadConfigFromFile();
        }
        catch (JsonException e)
        {
            Logger.Error(e, "Error parsing the configuration file. Please check the file format.");
            throw;
        }
        catch (Exception e)
        {
            Logger.Error(e, "An unexpected error occurred while loading the configuration.");
            throw;
        }
    }

    static ConfigurationManager LoadConfigFromFile()
    {
        string json = File.ReadAllText(ConfigFile);
        return JsonConvert.DeserializeObject<ConfigurationManager>(json);
    }

    public static void SaveConfiguration(ConfigurationManager config)
    {
        try
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigFile, json);
        }
        catch (Exception e)
        {
            Logger.Error(e,"An error occured while saving config file.");
        }
    }
}