namespace MoveFiles.FileManagement;
using MoveFiles.Configuration;
using MoveFiles.ConsoleInteraction;
class Program
{
    public static void Main()
    {
        string userOption;
        do
        {
            userOption = UserInput.GetUserFileSelectionOption();

            if (UserInput.UserNoCancelNoQuit(userOption))
            {
                if (ConfigurationManager.CheckIfConfigFileExists())
                {
                    (string nameOfTheFolder, string[] fileExtensions) = FolderNameAndExtensions.GetFolderNameAndExtensions(userOption);
                    var config = ConfigurationManager.LoadConfiguration();
                    if (Directory.Exists(config.DownloadPath))
                    {
                        string downloadFolderPath = DirectoryManager.GetDownloadFolderPath();
                        string subFolderPath = DirectoryManager.GetSubFolderPath(nameOfTheFolder);
                        (int transferredAmount, int skippedAmount) = FileMover.MoveFiles(subFolderPath, downloadFolderPath, fileExtensions);
                        FeedbackProvider.ProvideFeedback(transferredAmount, skippedAmount, userOption);
                    }

                    else
                    {
                        Console.WriteLine("The path cannot be found. Please provide a new path");
                        ConfigurationManager.CreateOrChangeConfigFile();
                    }

                }
                else
                {
                    ConfigurationManager.CreateOrChangeConfigFile();
                }

            }
            if (userOption.ToUpper() == Constants.ChangeDirectoryOption)
            {
                ConfigurationManager.CreateOrChangeConfigFile();
            }
        } while (userOption.ToUpper() != Constants.QuitOption);
    }
}