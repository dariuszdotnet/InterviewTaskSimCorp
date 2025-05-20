using WordCounter.Services.WordCounterService;

namespace WordCounter.Tests;

public class ProcessTextByBufferTests
{
    [Fact]
    public void GivingDefaultText_ShouldFindAllWords_WhenProcessingText()
    {
        // Arrange
        var text = File.ReadAllText(@"Data\1st text from task description.txt");
        var words = new List<string>();
        Action<string> addWordAction = (string word) => { words.Add(word); };

        // Act
        ProcessTextByBuffer.ProcessText(text, addWordAction);

        // Assert
        Assert.Equal(9, words.Count);
    }

    [Fact]
    public void GivingEmptyFile_ShouldFindZeroWords_WhenProcessingText()
    {
        // Arrange
        var text = File.ReadAllText(@"Data\empty file.txt");
        var words = new List<string>();
        Action<string> addWordAction = (string word) => { words.Add(word); };

        // Act
        ProcessTextByBuffer.ProcessText(text, addWordAction);

        // Assert
        Assert.Empty(words);
    }
}
