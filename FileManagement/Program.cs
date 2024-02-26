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
            userOption = UserInput.GetUserFileSelectionOption();

            if (UserInput.UserNoCancelNoDirectoryChangeNoQuitNoNameChange(userOption))
            {
                if (ConfigurationManager.CheckIfConfigFileExists() && DirectoryManager.CheckIfDownloadPathExists())
                {
                    (string nameOfTheFolder, string[] fileExtensions) = FolderNameAndExtensions.GetFolderNameAndExtensions(userOption);
                    string downloadFolderPath = DirectoryManager.GetDownloadFolderPath();
                    string subFolderPath = DirectoryManager.GetSubFolderPath(nameOfTheFolder);
                    (int transferredAmount, int skippedAmount) = FileMover.MoveFiles(subFolderPath, downloadFolderPath, fileExtensions);
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
                ConfigurationManager.ChangeFolderNamesInConfig();
            }
        } while (userOption.ToUpper() != Constants.QuitOption);
    }
}