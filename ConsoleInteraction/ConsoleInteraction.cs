namespace MoveFiles;

public class ConsoleInteraction
{
    static string GetInput()
    {
        return Console.ReadLine();
    }

    public static void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }

    public static string GetNewDownloadPath()
    {
        PrintMessage("Enter the path of the Download folder (C to cancel)");
        string input = GetInput();
        if (input.ToUpper() == "C")
        {
            return "C";
        }
        else if (input == "")
        {
            PrintMessage("Input can not be empty.");
            Configuration.CreateOrChangeConfigFile();
        }
        else if (Directory.Exists(input))
        {
            return input;
        }
        else if (input[0] == '"' && input[input.Length - 1] == '"')
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