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
        PrintMessage($"Enter the path of the Download folder ({Constants.ChangeDirectoryOption} to cancel)");
        string input = GetInput();
        if (input.ToUpper() == Constants.ChangeDirectoryOption)
        {
            return Constants.ChangeDirectoryOption;
        }
        else if (input == "")
        {
            PrintMessage("Input can not be empty.");
            ConfigurationManager.CreateOrChangeConfigFile();
        }
        else if (Directory.Exists(input))
        {
            return input;
        }
        else if (input[0] == '"' && input[^1] == '"')
        {
            string pathWithoutQuotation = input.Substring(1, input.Length - 2);
            if (Directory.Exists(pathWithoutQuotation))
            {
                return pathWithoutQuotation;
            }
                
        }
        return "";
    }
}