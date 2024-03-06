namespace MoveFiles.ConsoleInteraction;

public class FeedbackProvider
{
    public static void ProvideFeedback(int transferredAmount, int skippedAmount)
    {
        PrintNoFilesMessage(transferredAmount, skippedAmount);
        PrintMovedFilesMessage(transferredAmount);
        PrintSkippedFilesMessage(skippedAmount);
        PauseConsole();
    }

    static void PrintNoFilesMessage(int transferredAmount, int skippedAmount)
    {
        if (transferredAmount == 0 && skippedAmount == 0)
        {
            ConsoleInteraction.PrintMessage("No files to move.");
        }
    }

    static void PrintMovedFilesMessage(int transferredAmount)
    {
        if (transferredAmount > 0)
        {
            ConsoleInteraction.PrintMessage($"Moved {transferredAmount} files.");
        }
    }

    static void PrintSkippedFilesMessage(int skippedAmount)
    {
        if (skippedAmount > 0)
        {
            ConsoleInteraction.PrintMessage($"Skipped {skippedAmount} files.");
        }
    }

    static void PauseConsole()
    {
        Console.ReadKey();
    }
}