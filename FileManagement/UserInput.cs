namespace MoveFiles;

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
                if (string.IsNullOrEmpty(userOption) || (!_fileTypes.ContainsKey(userOption) && UserNoCancelNoQuit(userOption)))
                {
                    Console.WriteLine("Enter a valid option");
                    Console.ReadKey();
                }
            }while (string.IsNullOrEmpty(userOption) || (!_fileTypes.ContainsKey(userOption) && UserNoCancelNoQuit(userOption)));
            
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

        public static bool UserNoCancelNoQuit(string userOption)
        {
            if (userOption.ToUpper() != "C" && userOption.ToUpper() != "Q")
            {
                return true;
            }
            return false;
        }
    }
    