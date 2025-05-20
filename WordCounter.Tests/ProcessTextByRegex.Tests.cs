using WordCounter.Services.WordCounterService;

namespace WordCounter.Tests;

public class ProcessTextByRegexTests
{
    [Fact]
    public void GivingDefaultText_ShouldFindAllWords_WhenProcessingText()
    {
        // Arrange
        var text = File.ReadAllText(@"Data\2nd text from task description.txt");
        var words = new List<string>();
        Action<string> addWordAction = (string word) => { words.Add(word); };

        // Act
        ProcessTextByRegex.ProcessText(text, addWordAction);

        // Assert
        Assert.Equal(4, words.Count);
    }

    [Fact]
    public void GivingTextWithNewLine_ShouldFindZeroWords_WhenProcessingText()
    {
        // Arrange
        var text = File.ReadAllText(@"Data\file with Windows style new line.txt");
        var words = new List<string>();
        Action<string> addWordAction = (string word) => { words.Add(word); };

        // Act
        ProcessTextByRegex.ProcessText(text, addWordAction);

        // Assert
        Assert.Empty(words);
    }
}
