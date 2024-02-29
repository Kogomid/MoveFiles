namespace MoveFiles.ConsoleInteraction;

using Configuration;
public class UserMenu
{

    public static string ListUserFileSelectionOption()
    {
        string userOption;
        do
        {
            Console.Clear();
            var config = ConfigurationManager.LoadConfiguration();
            if (config.DownloadPath == Constants.EnterNewPath)
            {
               ConsoleInteraction.PrintMessage($"{Constants.ChangeDirectoryOption}.To change download folder\n");
            }
            else
            {
                try
                {
                    ConsoleInteraction.PrintMessage($"The default download folder is {config.DownloadPath}");
                }
                catch
                {
                    ConsoleInteraction.PrintMessage("Unable to show the default folder");
                }
                ConsoleInteraction.PrintMessage("Choose the type of the file you want to move:\n" +
                                                $"1. {DirectoryNameAndExtensions.DirectoryNames(0)}\n" +
                                                $"2. {DirectoryNameAndExtensions.DirectoryNames(1)}\n" +
                                                $"3. {DirectoryNameAndExtensions.DirectoryNames(2)}\n" +
                                                $"4. {DirectoryNameAndExtensions.DirectoryNames(3)}\n" +
                                                $"5. {DirectoryNameAndExtensions.DirectoryNames(4)}\n" +
                                                $"6. {DirectoryNameAndExtensions.DirectoryNames(5)}\n" +
                                                $"{Constants.ChangeDirectoryOption}.If you want to change download folder\n" +
                                                $"{Constants.ChangeNameOption}.To change the name of the sub folders\n" +
                                                $"{Constants.QuitOption}.To quit the program");
            }
            
            userOption = ConsoleInteraction.GetInput();
            ConsoleInteraction.GetUserFileSelectionOption(userOption);
        }while (string.IsNullOrEmpty(userOption) || (!DirectoryNameAndExtensions.FileTypes.ContainsKey(userOption) && ConsoleInteraction.UserNoCancelNoDirectoryChangeNoQuitNoNameChange(userOption)));
        
        return userOption;
    }
}
    