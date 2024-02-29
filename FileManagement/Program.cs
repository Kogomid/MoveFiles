namespace MoveFiles.FileManagement;

using Configuration;
using ConsoleInteraction;

class Program
{
    public static void Main()
    {
        string userOption;
        do
        {
            userOption = UserMenu.ListUserFileSelectionOption();

            if (ConsoleInteraction.UserNoCancelNoDirectoryChangeNoQuitNoNameChange(userOption))
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
            else if (userOption.ToUpper() == Constants.ChangeDirectoryOption)
            {
                ConfigurationManager.ChangeDownloadPathInConfig();
            }
            else if (userOption.ToUpper() == Constants.ChangeNameOption)
            {
                ConfigurationManager.ChangeDirectoryNamesInConfig();
            }
        } while (userOption.ToUpper() != Constants.QuitOption);
    }
}