namespace MoveFiles.ConsoleInteraction;

using Configuration;
public class UserMenu
{
    public static string FolderNames = $"1. {FolderNameAndExtensions.FolderNames(0)}\n" +
                                       $"2. {FolderNameAndExtensions.FolderNames(1)}\n" +
                                       $"3. {FolderNameAndExtensions.FolderNames(2)}\n" +
                                       $"4. {FolderNameAndExtensions.FolderNames(3)}\n" +
                                       $"5. {FolderNameAndExtensions.FolderNames(4)}\n" +
                                       $"6. {FolderNameAndExtensions.FolderNames(5)}\n";
    public static string ListUserFileSelectionOption()
    {
        string userOption;
        do
        {
            Console.Clear();
            var config = ConfigurationManager.LoadConfiguration();
            try
            {
                ConsoleInteraction.PrintMessage($"The default download folder is {config.DownloadPath}");
            }
            catch
            {
                ConsoleInteraction.PrintMessage("Unable to show the default folder");
            }
            ConsoleInteraction.PrintMessage("Choose the type of the file you want to move:\n" +
                                            FolderNames + 
                                            $"{Constants.ChangeDirectoryOption}.If you want to change download folder\n" +
                                            $"{Constants.ChangeNameOption}.To change the name of the sub folders\n" +
                                            $"{Constants.QuitOption}.To quit the program");
            userOption = ConsoleInteraction.GetInput();
            ConsoleInteraction.GetUserFileSelectionOption(userOption);
        }while (string.IsNullOrEmpty(userOption) || (!FolderNameAndExtensions.FileTypes.ContainsKey(userOption) && ConsoleInteraction.UserNoCancelNoDirectoryChangeNoQuitNoNameChange(userOption)));
        
        return userOption;
    }
}
    