namespace MoveFiles;

public class FileMover
{
    public static (int, int) MoveFiles(string subFolderPath, string downloadFolderPath, string[] fileExtensions)
    {
        int skippedAmount = 0;
        int transferredAmount = 0;
        Configuration.CreateANewFolder(downloadFolderPath);
        foreach (string fileExtension in fileExtensions)
        {
            string[] filesInDownloadFolder = Directory.GetFiles(downloadFolderPath, fileExtension);

            foreach (string oneFile in filesInDownloadFolder)
            {
                string fileName = Path.GetFileName(oneFile);
                string destFile = Path.Combine(subFolderPath, fileName);
                if (File.Exists(destFile))
                {
                    skippedAmount += 1;
                }
                else
                {
                    try
                    {
                        File.Move(oneFile, destFile);
                        transferredAmount += 1;
                    }
                    catch
                    {
                        Console.WriteLine($"Error moving {fileName}");
                        skippedAmount += 1;
                    }
                }
            }
        }
        return (transferredAmount, skippedAmount);
    }
}