namespace MoveFiles.FileManagement;
using ConsoleInteraction;
using Configuration;
public class FileMover
{
    public static (int, int) MoveFiles(string subFolderPath, string downloadFolderPath, string[] fileExtensions)
    {
        int skippedAmount = 0;
        int transferredAmount = 0;
        
        DirectoryManager.CreateANewSubFolder(subFolderPath);
        
        foreach (string fileExtension in fileExtensions)
        {
            string[] filesInDownloadFolder = Directory.GetFiles(downloadFolderPath, fileExtension);

            foreach (string selectedFile in filesInDownloadFolder)
            {
                string fileNameAndExtension = Path.GetFileName(selectedFile);
                string destinationFolder = Path.Combine(subFolderPath, fileNameAndExtension);

                if (File.Exists(destinationFolder))
                {
                    ConsoleInteraction.PrintMessage($"{fileNameAndExtension} already exists in the folder.");
                    skippedAmount += 1;
                }
                else
                {
                    try
                    {
                        File.Move(selectedFile, destinationFolder);
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