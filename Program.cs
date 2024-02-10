namespace MoveFiles
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using System.Collections.Generic; 

    internal class Program
    {
        static void Main(string[] args)
        {
            string userOption;
            do
            {
                userOption = GetUserFileSelectionOption();
                if (userOption.ToUpper() != "Q" && userOption.ToUpper() != "C")
                {
                    (string userFolder, string[] fileExtensions) = GetUserFolderAndExtensions(userOption);
                    (string downloadsPath, string userPath) = GetDownloadPath(userFolder);
                    (int transferedAmount, int skippedAmount) = MoveFiles(downloadsPath, userPath, fileExtensions);
                    ProvideFeedback(transferedAmount, skippedAmount, userOption);
                }
                else if (userOption.ToUpper() == "C")
                {
                    ChangeDirectory();
                    Console.WriteLine("The path has been changed");
                    Console.ReadLine();
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
                              "C.If you want to change download folder\n" +
                              "Q.To quit the program");
            return Console.ReadLine();
        }

        static Dictionary <string, (string, string[])> fileTypes = new Dictionary<string, (string, string[])>()
        {
            {"1", ("Documents", new string[] { "*.txt", "*.doc", "*.docx", "*.odt", "*.pdf", "*.xls", "*.xlsx", "*.ods", "*.csv", "*.ppt", "*.pptx", "*.odp", "*.mdb", "*.accdb", "*.html", "*.htm" })},
            {"2", ("Programs", new string[] { "*.exe", "*.app", "*.dmg", "*.dll", "*.so", "*.dylib", "*.py", "*.js", "*.rb", "*.php", "*.java", "*.class", "*.jar", "*.msi" })},
            {"3", ("Video", new string[] { "*.mp4", "*.mkv", "*.flv", "*.avi", "*.mov", "*.wmv" })},
            {"4", ("Photos", new string[] { "*.jpg", "*.jpeg", "*.png", "*.gif", "*.bmp", "*.tiff", "*.svg" })},
            {"5", ("Compressed", new string[] { "*.zip", "*.rar", "*.tar", "*.gz", "*.7z" })},
            {"6", ("Music", new string[] { "*.mp3", "*.m4a", "*.wav", "*.aac", "*.flac", "*.ogg", "*.m4a" })}
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
                    File.WriteAllText("downloadsConfig.json", json);
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


        static void ChangeDirectory()
        {
            Console.WriteLine("Enter a new path (without the \" \")");
            string downloadPath = Console.ReadLine();
            string json = JsonConvert.SerializeObject(downloadPath);
            File.WriteAllText("downloadsConfig.json", json);
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
                if (transferedAmount == 0 && skippedAmount == 0 && userOption.ToUpper() != "C" && userOption.ToUpper() != "Q")
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
}