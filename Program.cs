namespace MoveFiles
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    internal class Program
    {
        static void Main(string[] args)
        {
            string userOption;
            do
            {
                userOption = GetUserFileSelectionOption();
                if (userOption.ToUpper() != "Q")
                {
                    (string userFolder, string[] fileExtensions) = GetUserFolderAndExtensions(userOption);
                    (string downloadsPath, string userPath) = GetDownloadPath(userFolder);
                    (int transferedAmount, int skippedAmount) = MoveFiles(downloadsPath, userPath, fileExtensions);
                    ProvideFeedback(transferedAmount, skippedAmount, userOption);
                }
            } while (userOption.ToUpper() != "Q");
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
                              "7.If you want to change download folder\n" +
                              "Q.To quit the program");
            return Console.ReadLine();
        }
        
        
        static (string, string[]) GetUserFolderAndExtensions(string userOption)
        {
            string userFolder = "";
            string[] fileExtensions = { };
            switch (userOption)
            {
                case "1":
                    userFolder = "Documents";
                    fileExtensions = new string[]
                    {
                        "*.txt", "*.doc", "*.docx", "*.odt", "*.pdf", "*.xls", "*.xlsx", "*.ods", "*.csv", "*.ppt",
                        "*.pptx", "*.odp", "*.mdb", "*.accdb", "*.html", "*.htm"
                    };
                    break;
                case "2":
                    userFolder = "Programs";
                    fileExtensions = new string[]
                    {
                        "*.exe", "*.app", "*.dmg", "*.dll", "*.so", "*.dylib", "*.py", "*.js", "*.rb", "*.php",
                        "*.java", "*.class", "*.jar", "*.msi"
                    };
                    break;
                case "3":
                    userFolder = "Video";
                    fileExtensions = new string[] {"*.mp4", "*.mkv", "*.flv", "*.avi", "*.mov", "*.wmv"};
                    break;
                case "4":
                    userFolder = "Photos";
                    fileExtensions = new string[] {"*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.svg"};
                    break;
                case "5":
                    userFolder = "Compressed";
                    fileExtensions = new string[] {"*.zip", "*.rar", "*.tar", "*.gz", "*.7z"};
                    break;
                case "6":
                    userFolder = "Music";
                    fileExtensions = new string[] {"*.mp3", "*.m4a", "*.wav", "*.aac", "*.flac", "*.ogg", "*.m4a"};
                    break;
                case "7":
                    Console.WriteLine("Enter a new path");
                    string newPath = Console.ReadLine();
                    string json = JsonConvert.SerializeObject(newPath);
                    File.WriteAllText("downloadsConfig.json", json);
                    break;
                case "Q":
                    Console.WriteLine("Closing the program");
                    break;
                default:
                    Console.WriteLine("That option doesn't exist");
                    break;
            }
            return (userFolder,fileExtensions);
        }
    

        static (string, string) GetDownloadPath(string userFolder)
            {
                string downloadPath = "";
                string downloadConfig = "downloadsConfig.json";
                if (File.Exists("downloadsConfig.json"))
                {
                   string json =  File.ReadAllText(downloadConfig);
                   downloadPath = JsonConvert.DeserializeObject<string>(json);
                }
                else
                {
                    Console.WriteLine("Enter the path of the Download folder (without the \" \")");
                    downloadPath = Console.ReadLine();
                    string json = JsonConvert.SerializeObject(downloadPath);
                    File.WriteAllText(downloadConfig, json);
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

            static (int, int) MoveFiles(string downloadsPath, string userPath, string[] fileExtensions)
            {
                int skippedAmount = 0;
                int transferedAmount = 0;

                foreach (string fileExtension in fileExtensions)
                {
                    string[] userFiles = Directory.GetFiles(downloadsPath, fileExtension);

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
                if (transferedAmount == 0 && skippedAmount == 0 && userOption != "7" && userOption != "Q")
                {
                    Console.WriteLine("No files to move.");
                }
                if (transferedAmount >= 0 && userOption != "7" && userOption != "Q")
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
}