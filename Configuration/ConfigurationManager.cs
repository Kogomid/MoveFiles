namespace MoveFiles.Configuration;
using FileManagement;
using ConsoleInteraction;
using Newtonsoft.Json;

public class ConfigurationManager
    {
        static readonly string ConfigFile = "config.json";

        public string? DownloadPath { get; set; }
        
        public static bool CheckIfConfigFileExists()
        {
            return File.Exists(ConfigFile);
        }
        
        public static void CreateOrChangeConfigFile()
        {
            string newDownloadPath = ConsoleInteraction.GetNewDownloadPath();

            if (newDownloadPath == Constants.ChangeDirectoryOption)
            {
                return;
            }
            if (Directory.Exists(newDownloadPath))
            {
                var config = new ConfigurationManager()
                {
                    DownloadPath = newDownloadPath
                };
                try
                {
                    SaveConfiguration(config);
                    ConsoleInteraction.PrintMessage("The path has been changed");
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    ConsoleInteraction.PrintMessage("An error occurred: " + e.Message);
                    Console.ReadKey();
                    Program.Main();
                }
            }
            else
            {
                ConsoleInteraction.PrintMessage("That path does not exist. Please try again.");
                CreateOrChangeConfigFile();
            }
        }

        public static ConfigurationManager LoadConfiguration()
        {
            if (CheckIfConfigFileExists() == false)
            {
                CreateOrChangeConfigFile();
            }
            try
            {
                string json = File.ReadAllText(ConfigFile);
                return JsonConvert.DeserializeObject<ConfigurationManager>(json);
            }
            catch
            {
                CreateOrChangeConfigFile();
            }
            return null;
        }

        public static void SaveConfiguration(ConfigurationManager config)
        {
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigFile, json);
        }
        
    }
