namespace MoveFiles
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    class Program
    {
        public static void Main()
        {
            string userOption;
            do
            {
                userOption = UserInput.GetUserFileSelectionOption();

                if (userOption.ToUpper() != "Q" && userOption.ToUpper() != "C")
                {
                    if (Configuration.CheckIfConfigFileExists())
                    {
                        (string nameOfTheFolder, string[] fileExtensions) = UserInput.GetFolderNameAndExtensions(userOption);
                        var config = Configuration.LoadConfiguration();
                        if (Directory.Exists(config.DownloadPath))
                        {
                            string downloadFolderPath = Configuration.GetADownloadFolderPath();
                            string folderPath = Configuration.GetASubFolderPath(nameOfTheFolder);
                            (int transferedAmount, int skippedAmount) = MoveFiles(folderPath, downloadFolderPath, fileExtensions);
                            ProvideFeedback(transferedAmount, skippedAmount, userOption);
                        }
                        else
                        {
                            Configuration.CreateOrChangeConfigFile();
                        }
                        
                    }
                    else
                    {
                        Configuration.CreateOrChangeConfigFile();
                    }

                }
                else if (userOption.ToUpper() == "C")
                {
                    Configuration.CreateOrChangeConfigFile();
                    Console.WriteLine("The path has been changed");
                    Console.ReadLine();
                }
            } while (userOption.ToUpper() != "Q");
        }
        
        
        static (int, int) MoveFiles(string folderPath, string downloadFolderPath, string[] fileExtensions)
        {
            int skippedAmount = 0;
            int transferedAmount = 0;
            Configuration.CreateANewFolder(downloadFolderPath);
            foreach (string fileExtension in fileExtensions)
            {
                string[] userFiles = Directory.GetFiles(downloadFolderPath, fileExtension);

                foreach (string userFile in userFiles)
                {
                    string fileName = Path.GetFileName(userFile);
                    string destFile = Path.Combine(folderPath, fileName);
                    if (File.Exists(destFile))
                    {
                        skippedAmount += 1;
                    }
                    else
                    {
                        transferedAmount += 1;
                        try
                        {
                            File.Move(userFile, destFile);
                        }
                        catch
                        {
                            Console.WriteLine($"Error moving {fileName}");
                        }
                    }
                }
            }
            return (transferedAmount, skippedAmount);
        }
        

        static void ProvideFeedback(int transferedAmount, int skippedAmount, string userOption)
        {
            if (transferedAmount == 0 && skippedAmount == 0 && userOption.ToUpper() != "C" &&
                userOption.ToUpper() != "Q")
            {
                Console.WriteLine("No files to move.");
            }
            if (transferedAmount > 0 && userOption != "C" && userOption != "Q")
            {
                Console.WriteLine($"Moved {transferedAmount} files.");
            }
            if (skippedAmount > 0)
            {
                Console.WriteLine($"Skipped {skippedAmount} files.");
            }
            Console.ReadLine();
        }
    }
    
    
    public class ConsoleInteraction
    {
        static string GetInput()
        {
            return Console.ReadLine();
        }

        public static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static string GetNewDownloadPath()
        {
            PrintMessage("Enter the path of the Download folder (C to cancel)");
            string input = GetInput();
            if (input.ToUpper() == "C")
            {
                Program.Main();
            }
            else if (input == "")
            {
                PrintMessage("Input can not be empty.");
                Configuration.CreateOrChangeConfigFile();
            }
            else if (Directory.Exists(input))
            {
                return input;
            }
            else if (input[0] == '"' && input[input.Length - 1] == '"')
            {
                string pathWithoutQuotation = input.Substring(1, input.Length - 2);
                if (Directory.Exists(pathWithoutQuotation))
                {
                    return pathWithoutQuotation;
                }
                
            }
            return null;
        }
    }

    public class Configuration
    {
        static readonly string ConfigFile = "config.json";

        public string DownloadPath { get; set; }

        public static bool CheckIfConfigFileExists()
        {
            return File.Exists(ConfigFile);
        }

        public static void CreateOrChangeConfigFile()
        {
            string newDownloadPath = ConsoleInteraction.GetNewDownloadPath();

            if (Directory.Exists(newDownloadPath))
            {
                var config = new Configuration()
                {
                    DownloadPath = newDownloadPath
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

        public static string GetASubFolderPath(string nameOfTheFolder)
        {
            var config = LoadConfiguration();
            string downloadPathFolder = config.DownloadPath;
            string folderPath = Path.Combine(downloadPathFolder, nameOfTheFolder);

            return (folderPath);
        }

        public static string GetADownloadFolderPath()
        {
            var config = LoadConfiguration();
            return config.DownloadPath;
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

    public class UserInput
    {
         
        static Dictionary<string, (string, string[])> _fileTypes = new Dictionary<string, (string, string[])>()
        {
            {"1", ("Documents", new string[] {"*.txt", "*.doc", "*.docx", "*.odt", "*.pdf", "*.xls", "*.xlsx", "*.ods", "*.csv", "*.ppt", "*.pptx", "*.odp", "*.mdb", "*.accdb", "*.html", "*.htm"})},
            {"2", ("Programs", new string[] {"*.exe", "*.app", "*.dmg", "*.dll", "*.so", "*.dylib", "*.py", "*.js", "*.rb", "*.php", "*.java", "*.class", "*.jar", "*.msi"})},
            {"3", ("Video", new string[] {"*.mp4", "*.mkv", "*.flv", "*.avi", "*.mov", "*.wmv"})},
            {"4", ("Photos", new string[] {"*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.svg", "*.heic"})},
            {"5", ("Compressed", new string[] {"*.zip", "*.rar", "*.tar", "*.gz", "*.7z"})},
            {"6", ("Music", new string[] {"*.mp3", "*.m4a", "*.wav", "*.aac", "*.flac", "*.ogg", "*.m4a"})}
        };
        
        
        public static string GetUserFileSelectionOption()
        {
            string userOption;
            do
            {
                Console.Clear();
                var config = Configuration.LoadConfiguration();
                try
                {
                    Console.WriteLine($"The default download folder is {config.DownloadPath}");
                }
                catch
                {
                    Console.WriteLine("Unavailable to show the default folder");
                }
                Console.WriteLine("Choose the type of the file you want to move:\n" +
                                  "1.Documents\n" +
                                  "2.Programs\n" +
                                  "3.Video\n" +
                                  "4.Photos\n" +
                                  "5.Compressed\n" +
                                  "6.Music\n" +
                                  "C.If you want to change download folder\n" +
                                  "Q.To quit the program");
                userOption = Console.ReadLine();
                if (string.IsNullOrEmpty(userOption) || (!_fileTypes.ContainsKey(userOption) && userOption.ToUpper() != "C" && userOption.ToUpper() != "Q"))
                {
                    Console.WriteLine("Enter a valid option");
                    Console.ReadKey();
                }
            }while (string.IsNullOrEmpty(userOption) || (!_fileTypes.ContainsKey(userOption) && userOption.ToUpper() != "C" && userOption.ToUpper() != "Q"));
            
            return userOption;
        }


        public static (string, string[]) GetFolderNameAndExtensions(string userOption)
        {
            if (_fileTypes.TryGetValue(userOption, out var fileType))
            {
                (string nameOfTheFolder, string[] fileExtensions) = fileType;
                return (nameOfTheFolder, fileExtensions);
            }
            
            return (null, null);
        }
    }
    
    
    
    
    
}