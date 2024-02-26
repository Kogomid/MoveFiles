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
        PrintMessage($"Enter the path of the Download folder ({Constants.CancelOption} to cancel)");
        string input = GetInput();
        if (input.ToUpper() == Constants.CancelOption)
        {
            return Constants.CancelOption;
        }
        if (!Directory.Exists(input))
        {
            PrintMessage("Path can not be found.");
            return GetNewDownloadPath();
        }
        if (Directory.Exists(input))
        {
            return input;
        }
        if (input[0] == '"' && input[^1] == '"')
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