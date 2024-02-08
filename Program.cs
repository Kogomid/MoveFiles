namespace MoveFiles
{
    using System;
    using System.IO;

    internal class Program
    {
        static void Main(string[] args)
        {
            string userOption;
            do
            {
                MoveFiles();
                Console.WriteLine("Do you want to continue? (Q to quit)");
                userOption = Console.ReadLine();
            } while (userOption.ToUpper() != "Q");
        }

        static void MoveFiles()
        {
            string userOption = GetUserInput();
            (string userFolder, string[] fileExtensions) = GetUserFolderAndExtensions(userOption);
            (string downloadsPath, string userPath) = GetDownloadPath(userFolder);
            (int transferedAmount, int skippedAmount) = MoveFilesToFolder(downloadsPath, userPath, fileExtensions);
            ProvideFeedback(transferedAmount, skippedAmount, userOption);
        }

        static string GetUserInput()
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
            string userOption = Console.ReadLine();
            return userOption;
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
                    File.WriteAllText("downloadsPath.txt", newPath);
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
                string downloadsPath = "";
                string filePath = "downloadsPath.txt";
                if (File.Exists("downloadsPath.txt"))
                {
                    downloadsPath = File.ReadAllText(filePath);
                }
                else
                {
                    Console.WriteLine("Enter the path of the Download folder");
                    downloadsPath = Console.ReadLine();
                    File.WriteAllText(filePath, downloadsPath);
                }

                string userPath = Path.Combine(downloadsPath, userFolder);
                Directory.CreateDirectory(userPath);
                
                return (downloadsPath, userPath);
            }

            static (int, int) MoveFilesToFolder(string downloadsPath, string userPath, string[] fileExtensions)
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
                            File.Move(userFile, destFile);
                        }
                    }

                }
                return (transferedAmount, skippedAmount);
            }

            static void ProvideFeedback(int transferedAmount, int skippedAmount, string userOption)
            {
                
                if (transferedAmount >= 0 && userOption != "Q")
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