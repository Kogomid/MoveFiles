namespace MoveFiles.Test;

using ConsoleInteraction;
using Configuration;

public class UnitTest1
{
    [Fact]
    public void GetUserFileSelectionOption_ReturnsTrue_WhenUserEntersCorrectOption()
    {
        // Arrange
        var inputs = Constants.DefaultDirectoryNameSettings.Count();

        for (int i = 1; i <= inputs; i++)
        {
            // Act
            string s = Convert.ToString(i);
            string result = ConsoleInteraction.GetUserFileSelectionOption(s);
            
            // Assert
            Assert.Equal(s, result);
        }
    }
}