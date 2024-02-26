namespace MoveFiles.ConsoleInteraction;

public class FeedbackProvider
{
    public static void ProvideFeedback(int transferredAmount, int skippedAmount)
    {
        if (transferredAmount == 0 && skippedAmount == 0)
        {
            ConsoleInteraction.PrintMessage("No files to move.");
        }
        if (transferredAmount > 0)
        {
            ConsoleInteraction.PrintMessage($"Moved {transferredAmount} files.");
        }
        if (skippedAmount > 0)
        {
            ConsoleInteraction.PrintMessage($"Skipped {skippedAmount} files.");
        }
        Console.ReadLine();
    }
}