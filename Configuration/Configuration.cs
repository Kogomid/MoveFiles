namespace MoveFiles;

using Newtonsoft.Json;

public class Configuration
    {
        static readonly string ConfigFile = "config.json";

        public string DownloadPath { get; set; }
        
        public static string GetDownloadFolderPath()
        {
            var config = LoadConfiguration();
            return config.DownloadPath;
        }
        
        public static bool CheckIfConfigFileExists()
        {
            return File.Exists(ConfigFile);
        }
        
        public static void CreateOrChangeConfigFile()
        {
            string newDownloadPath = ConsoleInteraction.GetNewDownloadPath();

            if (newDownloadPath == "C")
            {
                return;
            }
            if (Directory.Exists(newDownloadPath))
            {
                var config = new Configuration()
                {
                    DownloadPath = newDownloadPath
                };
                try
                {
                    SaveConfiguration(config);
                    Console.WriteLine("The path has been changed");
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
                Console.WriteLine("That path does not exist. Please try again.");
                CreateOrChangeConfigFile();
            }
        }

        public static Configuration LoadConfiguration()
        {
            if (CheckIfConfigFileExists() == false)
            {
                CreateOrChangeConfigFile();
            }
            try
            {
                string json = File.ReadAllText(ConfigFile);
                return JsonConvert.DeserializeObject<Configuration>(json);
            }
            catch
            {
                CreateOrChangeConfigFile();
            }
            return null;
        }

        public static void SaveConfiguration(Configuration config)
        {
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(ConfigFile, json);
        }

        public static string GetSubFolderPath(string nameOfTheFolder)
        {
            var config = LoadConfiguration();
            string downloadPathFolder = config.DownloadPath;
            string folderPath = Path.Combine(downloadPathFolder, nameOfTheFolder);

            return (folderPath);
        }
        

        public static void CreateANewFolder(string folderPath)
        {
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch
            {
                ConsoleInteraction.PrintMessage("Error creating the directory. Please check the path and try again.");
            }

        }
    }