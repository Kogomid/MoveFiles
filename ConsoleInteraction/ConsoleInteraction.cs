namespace MoveFiles.ConsoleInteraction;

using Configuration;

public class ConsoleInteraction
{
    public static string GetInput()
    {
        return Console.ReadLine();
    }

    public static void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }

    public static string GetNewDownloadPath()
    {
        PrintMessage($"Enter the path of the Download folder ({Constants.CancelOption} to cancel)");
        string input = GetInput();
        if (input.ToUpper() == Constants.CancelOption)
        {
            return Constants.CancelOption;
        }
        if (!Directory.Exists(input) || input == @"C:\" || input == "C:")
        {
            PrintMessage("Path can not be found.");
            return GetNewDownloadPath();
        }
        if (Directory.Exists(input))
        {
            return input;
        }
        if (input[0] == '"' && input[^1] == '"')
        {
            string pathWithoutQuotation = input.Substring(1, input.Length - 2);
            if (Directory.Exists(pathWithoutQuotation))
            {
                return pathWithoutQuotation;
            }
                
        }
        return "";
    }

    public static string GetUserFolderNameSelection()
    {
        string pickFolder;
        do
        {
            pickFolder = GetInput();

        } while (string.IsNullOrEmpty(pickFolder) || (!FolderNameAndExtensions.FileTypes.ContainsKey(pickFolder) &&
                                                      pickFolder.ToUpper() != Constants.CancelOption &&
                                                      pickFolder.ToUpper() != Constants.ChangeNameSettingsOption));
        return pickFolder;
    }
    public static bool UserNoCancelNoDirectoryChangeNoQuitNoNameChange(string userOption)
    {
        return (userOption.ToUpper() != Constants.CancelOption && userOption.ToUpper() != Constants.ChangeDirectoryOption && userOption.ToUpper() != Constants.QuitOption && userOption.ToUpper() != Constants.ChangeNameOption);
    }

    public static string GetUserFileSelectionOption(string userOption)
    {
        if (string.IsNullOrEmpty(userOption) || (!FolderNameAndExtensions.FileTypes.ContainsKey(userOption) && UserNoCancelNoDirectoryChangeNoQuitNoNameChange(userOption)))
        {
            PrintMessage("Enter a valid option");
            Console.ReadKey();
        }
        return userOption;
    }
    
    public static void DisplayFolderNames()
    {
        Console.WriteLine("Which name would you like to change? (C to cancel)\n" +
                          UserMenu.FolderNames +
                          $"Enter {Constants.ChangeNameSettingsOption} to revert the changes");
    }
}