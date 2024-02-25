namespace MoveFiles.ConsoleInteraction;
using MoveFiles.Configuration;
public class UserInput
    {
        public static string GetUserFileSelectionOption()
        {
            string userOption;
            do
            {
                Console.Clear();
                var config = ConfigurationManager.LoadConfiguration();
                try
                {
                    Console.WriteLine($"The default download folder is {config.DownloadPath}");
                }
                catch
                {
                    Console.WriteLine("Unable to show the default folder");
                }
                Console.WriteLine("Choose the type of the file you want to move:\n" +
                                  "1.Documents\n" +
                                  "2.Programs\n" +
                                  "3.Video\n" +
                                  "4.Photos\n" +
                                  "5.Compressed\n" +
                                  "6.Music\n" +
                                  $"{Constants.ChangeDirectoryOption}.If you want to change download folder\n" +
                                  $"{Constants.QuitOption}.To quit the program");
                userOption = Console.ReadLine();
                if (string.IsNullOrEmpty(userOption) || (!FolderNameAndExtensions.FileTypes.ContainsKey(userOption) && UserNoCancelNoQuit(userOption)))
                {
                    Console.WriteLine("Enter a valid option");
                    Console.ReadKey();
                }
            }while (string.IsNullOrEmpty(userOption) || (!FolderNameAndExtensions.FileTypes.ContainsKey(userOption) && UserNoCancelNoQuit(userOption)));
            
            return userOption;
        }
        
        
        public static bool UserNoCancelNoQuit(string userOption)
        {
            if (userOption.ToUpper() != Constants.ChangeDirectoryOption && userOption.ToUpper() != Constants.QuitOption)
            {
                return true;
            }
            return false;
        }
    }
    