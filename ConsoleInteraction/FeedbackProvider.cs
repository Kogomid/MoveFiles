namespace MoveFiles.ConsoleInteraction;

public class FeedbackProvider
{
    public static void ProvideFeedback(int transferredAmount, int skippedAmount, string userOption)
    {
        if (transferredAmount == 0 && skippedAmount == 0 && UserInput.UserNoCancelNoQuit(userOption))
        {
            ConsoleInteraction.PrintMessage("No files to move.");
        }
        if (transferredAmount > 0 && UserInput.UserNoCancelNoQuit(userOption))
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