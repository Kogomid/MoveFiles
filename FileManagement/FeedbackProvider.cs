namespace MoveFiles;

public class FeedbackProvider
{
    public static void ProvideFeedback(int transferredAmount, int skippedAmount, string userOption)
    {
        if (transferredAmount == 0 && skippedAmount == 0 && UserInput.UserNoCancelNoQuit(userOption))
        {
            Console.WriteLine("No files to move.");
        }
        if (transferredAmount > 0 && UserInput.UserNoCancelNoQuit(userOption))
        {
            Console.WriteLine($"Moved {transferredAmount} files.");
        }
        if (skippedAmount > 0)
        {
            Console.WriteLine($"Skipped {skippedAmount} files.");
        }
        Console.ReadLine();
    }
}