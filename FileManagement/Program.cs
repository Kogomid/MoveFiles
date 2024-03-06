namespace MoveFiles.FileManagement;

using NLog;
using Configuration;
using ConsoleInteraction;

class Program
{
    static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public static void Main()
    {
        string userOption;
        do
        {
            userOption = UserMenu.ListUserFileSelectionOption().ToUpper();

            if (ConsoleInteraction.UserNoCancelNoDirectoryChangeNoQuitNoNameChange(userOption))
            {
                HandleFileMoving(userOption);
            }
            else if (userOption == Constants.ChangeDirectoryOption)
            {
                HandleDirectoryChange();
            }
            else if (userOption == Constants.ChangeNameOption)
            {
                HandleNameChange();
            }
        } while (userOption != Constants.QuitOption);
    }

    static void HandleFileMoving(string userOption)
    {
        if (ConfigurationManager.CheckIfConfigFileExists() && DirectoryManager.CheckIfDownloadPathExists())
        {
            (string nameOfTheDirectory, string[] fileExtensions) =
                DirectoryNameAndExtensions.GetDirectoryNameAndExtensions(userOption);
            string downloadDirectoryPath = DirectoryManager.GetDownloadDirectoryPath();
            string subDirectoryPath = DirectoryManager.GetSubDirectoryPath(nameOfTheDirectory);
            (int transferredAmount, int skippedAmount) =
                FileMover.MoveFiles(subDirectoryPath, downloadDirectoryPath, fileExtensions);
            FeedbackProvider.ProvideFeedback(transferredAmount, skippedAmount);
        }
        else if (!ConfigurationManager.CheckIfConfigFileExists())
        {
            ConfigurationManager.CreateConfigFile();
        }
    }
    
    
    static void HandleDirectoryChange()
    {
        try
        {
            ConfigurationManager.ChangeDownloadPathInConfig();
        }
        catch (Exception e)
        {
            Logger.Error(e, "An error occurred while changing the download path in config.");
        }
    }

    static void HandleNameChange()
    {
        try
        {
            ConfigurationManager.ChangeDirectoryNamesInConfig();
        }
        catch (Exception e)
        {
            Logger.Error(e, "An error occurred while changing the directory names in config.");
        }
    }
}