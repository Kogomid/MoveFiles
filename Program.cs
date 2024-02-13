namespace MoveFiles
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    class Program
    {
        static readonly string ConfigFile = "config.json";

        static void Main()
        {
            string userOption;
            do
            {
                userOption = GetUserFileSelectionOption();

                if (userOption.ToUpper() != "Q" && userOption.ToUpper() != "C")
                {
                    if (CheckIfConfigFileExists())
                    {
                        (string userFolder, string[] fileExtensions) = GetUserFolderAndExtensions(userOption);
                        (string downloadsPath, string userPath) = GetDownloadPath(userFolder);
                        (int transferedAmount, int skippedAmount) = MoveFiles(downloadsPath, userPath, fileExtensions);
                        ProvideFeedback(transferedAmount, skippedAmount, userOption);
                    }
                    else
                    {
                        ChangeDirectory();
                    }

                }
                else if (userOption.ToUpper() == "C")
                {
                    ChangeDirectory();
                    Console.WriteLine("The path has been changed");
                    Console.ReadLine();
                }
            } while (userOption.ToUpper() != "Q");
        }


        static bool CheckIfConfigFileExists()
        {
            if (File.Exists(ConfigFile))
            {
                using (StreamReader sr = File.OpenText(ConfigFile))
                {
                    string firstLine = sr.ReadLine();
                    var config = JsonConvert.DeserializeObject<Configuration>(firstLine);
                    if (Directory.Exists(config.DownloadPath))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        static string GetUserFileSelectionOption()
        {
            Console.Clear();
            Console.WriteLine("Choose the type of the file you want to move:\n" +
                              "1.Documents\n" +
                              "2.Programs\n" +
                              "3.Video\n" +
                              "4.Photos\n" +
                              "5.Compressed\n" +
                              "6.Music\n" +
                              "C.If you want to change download folder\n" +
                              "Q.To quit the program");
            return Console.ReadLine();
        }
        
        static Dictionary<string, (string, string[])> fileTypes = new Dictionary<string, (string, string[])>()
        {
            {"1", ("Documents", new string[] {"*.txt", "*.doc", "*.docx", "*.odt", "*.pdf", "*.xls", "*.xlsx", "*.ods", "*.csv", "*.ppt", "*.pptx", "*.odp", "*.mdb", "*.accdb", "*.html", "*.htm"})},
            {"2", ("Programs", new string[] {"*.exe", "*.app", "*.dmg", "*.dll", "*.so", "*.dylib", "*.py", "*.js", "*.rb", "*.php", "*.java", "*.class", "*.jar", "*.msi"})},
            {"3", ("Video", new string[] {"*.mp4", "*.mkv", "*.flv", "*.avi", "*.mov", "*.wmv"})},
            {"4", ("Photos", new string[] {"*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.svg", "*.heic"})},
            {"5", ("Compressed", new string[] {"*.zip", "*.rar", "*.tar", "*.gz", "*.7z"})},
            {"6", ("Music", new string[] {"*.mp3", "*.m4a", "*.wav", "*.aac", "*.flac", "*.ogg", "*.m4a"})}
        };

        static (string, string[]) GetUserFolderAndExtensions(string userOption)
        {
            if (fileTypes.TryGetValue(userOption, out var fileType))
            {
                (string userFolder, string[] fileExtensions) = fileType;
                return (userFolder, fileExtensions);
            }
            else if (userOption.ToUpper() != "C" && userOption.ToUpper() != "Q")
            {
                Console.WriteLine("Invalid option. Please try again.");
                Console.ReadLine();
                return GetUserFolderAndExtensions(GetUserFileSelectionOption());
            }
            else
            {
                return ("", new string[] { });
            }
        }


        static (string, string) GetDownloadPath(string userFolder)
        {
            string downloadPath = "";
            if (File.Exists(ConfigFile))
            {
                if (new FileInfo(ConfigFile).Length == 0)
                {
                    ChangeDirectory();
                }
                string json = File.ReadAllText(ConfigFile);
                var config = JsonConvert.DeserializeObject<Configuration>(json);
                downloadPath = config.DownloadPath;
            }
            else
            {
                ChangeDirectory();
            }

            string userPath = Path.Combine(downloadPath, userFolder);
            try
            {
                Directory.CreateDirectory(userPath);
            }
            catch
            {
                Console.WriteLine("Error creating the directory. Please check the path and try again.");
            }
            return (downloadPath, userPath);
        }


        static string ChangeDirectory()
        {
            Console.WriteLine("Enter the path of the Download folder (C to cancel)");
            string downloadPath = Console.ReadLine();

            if (downloadPath.ToUpper() == "C")
            {
                Main();
            }
            else if (downloadPath == "")
            {
                Console.WriteLine("Input can not be empty.");
                ChangeDirectory();
            }
            else if (downloadPath[0] == '"' && downloadPath[downloadPath.Length - 1] == '"')
            {
                downloadPath = downloadPath.Substring(1, downloadPath.Length - 2);
            }

            if (Directory.Exists(downloadPath))
            {
                var config = new Configuration
                {
                    DownloadPath = downloadPath,
                };
                string json = JsonConvert.SerializeObject(config);
                try
                {
                    File.WriteAllText(ConfigFile, json);
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    Console.WriteLine("Going back to the main menu");
                    Console.ReadKey();
                    Main();
                }

            }
            else
            {
                Console.WriteLine("That path does not exist. Please try again.");
                ChangeDirectory();
            }
            return downloadPath;
        }


        static (int, int) MoveFiles(string downloadPath, string userPath, string[] fileExtensions)
        {
            int skippedAmount = 0;
            int transferedAmount = 0;

            foreach (string fileExtension in fileExtensions)
            {
                string[] userFiles = Directory.GetFiles(downloadPath, fileExtension);

                foreach (string userFile in userFiles)
                {
                    string fileName = Path.GetFileName(userFile);
                    string destFile = Path.Combine(userPath, fileName);
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

    public class Configuration
    {
        public string DownloadPath { get; set; }
    }
}