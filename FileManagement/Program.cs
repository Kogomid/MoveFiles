namespace MoveFiles
{
    using System.IO;

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
                    if (Configuration.CheckIfConfigFileExists())
                    {
                        (string nameOfTheFolder, string[] fileExtensions) =
                            UserInput.GetFolderNameAndExtensions(userOption);
                        var config = Configuration.LoadConfiguration();
                        if (Directory.Exists(config.DownloadPath))
                        {
                            string downloadFolderPath = Configuration.GetDownloadFolderPath();
                            string subFolderPath = Configuration.GetSubFolderPath(nameOfTheFolder);
                            (int transferredAmount, int skippedAmount) = FileMover.MoveFiles(subFolderPath, downloadFolderPath, fileExtensions);
                            FeedbackProvider.ProvideFeedback(transferredAmount, skippedAmount, userOption);
                        }

                        else
                        {
                            Console.WriteLine("The path cannot be found. Please provide a new path");
                            Configuration.CreateOrChangeConfigFile();
                        }

                    }
                    else
                    {
                        Configuration.CreateOrChangeConfigFile();
                    }

                }
                if (userOption.ToUpper() == "C")
                {
                    Configuration.CreateOrChangeConfigFile();
                }
            } while (userOption.ToUpper() != "Q");
        }
    }
}