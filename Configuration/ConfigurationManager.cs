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
                DownloadPath = ConsoleInteraction.GetNewDownloadPath(),
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
            string pickFolder;
            do
            {
                Console.WriteLine("Which name would you like to change? (C to cancel)\n" +
                                  $"1. {FolderNameAndExtensions.FolderNames(0)}\n" +
                                  $"2. {FolderNameAndExtensions.FolderNames(1)}\n" +
                                  $"3. {FolderNameAndExtensions.FolderNames(2)}\n" +
                                  $"4. {FolderNameAndExtensions.FolderNames(3)}\n" +
                                  $"5. {FolderNameAndExtensions.FolderNames(4)}\n" +
                                  $"6. {FolderNameAndExtensions.FolderNames(5)}\n\n" +
                                  $"Enter {Constants.ChangeNameSettingsOption} to revert the changes");

                pickFolder = ConsoleInteraction.GetInput();
                
            }while(string.IsNullOrEmpty(pickFolder) || (!FolderNameAndExtensions.FileTypes.ContainsKey(pickFolder) && pickFolder.ToUpper() != Constants.CancelOption && pickFolder.ToUpper() != Constants.ChangeNameSettingsOption));
            if (pickFolder.ToUpper() == Constants.CancelOption)
            {
                return;
            }
            if (pickFolder.ToUpper() == Constants.ChangeNameSettingsOption)
            {
                var config = LoadConfiguration();
                config.FolderNames = Constants.DefaultFolderNameSettings;
                SaveConfiguration(config);
            }
            else
            {
                Console.WriteLine("Enter a new name for this folder.");
                string newName = ConsoleInteraction.GetInput();
            
                var config = LoadConfiguration();
                config.FolderNames[(Convert.ToInt32(pickFolder) - 1)] = newName;
                SaveConfiguration(config);
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
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigFile, json);
        }
        
    }
