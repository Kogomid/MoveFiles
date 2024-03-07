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
        do
        {
            PrintMessage($"Enter the path of the Download folder ({Constants.CancelOption} to cancel)");
            string userInput = GetInput();
            if (userInput.ToUpper() == Constants.CancelOption)
            {
                return Constants.CancelOption;
            }
            if (userInput.Length >= 2 && userInput[0] == '"' && userInput[^1] == '"')
            {
                return RemoveQuotationsFromPath(userInput);
            }
            if (Directory.Exists(userInput))
            {
                return userInput;
            }
            PrintMessage("Path can not be found.");
        } while (true);
    }
    
    static string RemoveQuotationsFromPath(string userInput)
    {
        if (userInput.Length >= 2 && userInput[0] == '"' && userInput[^1] == '"')
        {
            return userInput.Substring(1, userInput.Length - 2);
        }
        return userInput;
    }
    public static string GetUserDirectoryNameSelection()
    {
        string pickDirectory;
        do
        {
            DisplayDirectoryNames();
            pickDirectory = GetInput();

        } while (!IsValidDirectorySelection(pickDirectory));
        return ProcessDirectorySelection(pickDirectory);
    }

    static string ProcessDirectorySelection(string pickDirectory)
    {
        if (pickDirectory.ToUpper() == Constants.CancelOption)
        {
            return String.Empty;
        }
        if (pickDirectory.ToUpper() == Constants.DefaultNameSettingsOption)
        { 
            DirectoryNameAndExtensions.ResetDirectoryNamesToDefault();
            return (string.Empty);
        }
        return pickDirectory;
    }
    public static bool UserNoCancelNoDirectoryChangeNoQuitNoNameChange(string userOption)
    {
        return (userOption.ToUpper() != Constants.CancelOption &&
                userOption.ToUpper() != Constants.ChangeDirectoryOption && 
                userOption.ToUpper() != Constants.QuitOption && 
                userOption.ToUpper() != Constants.ChangeNameOption);
    }

    public static string GetUserFileSelectionOption(string userOption)
    {
        if (!IsValidFileSelection(userOption))
        {
            PrintMessage("Enter a valid option");
            Console.ReadKey();
        }
        return userOption;
    }

    static bool IsValidDirectorySelection(string pickDirectory)
    {
        return !string.IsNullOrEmpty(pickDirectory) &&
               (DirectoryNameAndExtensions.FileTypes.ContainsKey(pickDirectory) ||
                pickDirectory.ToUpper() == Constants.CancelOption ||
                pickDirectory.ToUpper() == Constants.DefaultNameSettingsOption);
    }
    static bool IsValidFileSelection(string userOption)
    {
        return !string.IsNullOrEmpty(userOption) && (DirectoryNameAndExtensions.FileTypes.ContainsKey(userOption) ||
                                                     !UserNoCancelNoDirectoryChangeNoQuitNoNameChange(userOption));
    }
    
    public static void DisplayDirectoryNames()
    {
        Console.WriteLine("Which name would you like to change? (C to cancel)\n" +
                          $"1. {DirectoryNameAndExtensions.DirectoryNames(0)}\n" +
                          $"2. {DirectoryNameAndExtensions.DirectoryNames(1)}\n" +
                          $"3. {DirectoryNameAndExtensions.DirectoryNames(2)}\n" +
                          $"4. {DirectoryNameAndExtensions.DirectoryNames(3)}\n" +
                          $"5. {DirectoryNameAndExtensions.DirectoryNames(4)}\n" +
                          $"6. {DirectoryNameAndExtensions.DirectoryNames(5)}\n" +
                          $"Enter {Constants.DefaultNameSettingsOption} to revert the changes");
    }
}