namespace MoveFiles.FileManagement;

using ConsoleInteraction;
using Configuration;

public class FileMover
{
    public static (int, int) MoveFiles(string subDirectoryPath, string downloadDirectoryPath, string[] fileExtensions)
    {
        int skippedAmount = 0;
        int transferredAmount = 0;

        DirectoryManager.CreateANewSubDirectory(subDirectoryPath);

        foreach (string fileExtension in fileExtensions)
        {
            string[] filesInDownloadDirectory = Directory.GetFiles(downloadDirectoryPath, fileExtension);

            foreach (string selectedFile in filesInDownloadDirectory)
            {
                string fileNameAndExtension = Path.GetFileName(selectedFile);
                string destinationDirectory = Path.Combine(subDirectoryPath, fileNameAndExtension);

                if (File.Exists(destinationDirectory))
                {
                    ConsoleInteraction.PrintMessage($"{fileNameAndExtension} already exists in the folder.");
                    skippedAmount += 1;
                }
                else
                {
                    try
                    {
                        File.Move(selectedFile, destinationDirectory);
                        transferredAmount += 1;
                    }
                    catch
                    {
                        ConsoleInteraction.PrintMessage($"Error moving {fileNameAndExtension}");
                        skippedAmount += 1;
                    }
                }
            }
        }
        return (transferredAmount, skippedAmount);
    }
}