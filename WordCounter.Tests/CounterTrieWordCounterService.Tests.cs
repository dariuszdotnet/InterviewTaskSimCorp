using Moq;
using WordCounter.Common;
using WordCounter.DataStructures;
using WordCounter.Services.WordCounterService;

namespace WordCounter.Tests;

public class CounterTrieWordCounterServiceTests
{
    [Fact]
    public void GivingWordCounter_ShouldProcessProperTexts_WhenAddTwoTextsAndRemoveSecond()
    {
        // Arrange
        var processedTexts = new List<string>();
        ProcessText processText = (string text, Action<string> processSingleResult) =>
        {
            processedTexts.Add(text);

            // for coverage of ProcessSingleResult(string word) method:
            var words = text.Split(' ');
            foreach (var word in words)
                processSingleResult(word);
        };
        ConsumeSingleResult consumeSingleResult = (string word, int count) => { };
        var traversal = new Mock<ITrieTraversal>();
        var wordCounterService = new CounterTrieWordCounterService(processText, consumeSingleResult, traversal.Object);

        // Act
        var firstText = File.ReadAllText(@"Data\1st text from task description.txt");
        wordCounterService.AddText("first", firstText);
        wordCounterService.AddText("second", File.ReadAllText(@"Data\2nd text from task description.txt"));
        wordCounterService.RemoveText("second");
        wordCounterService.GetWordFrequencies();

        // Assert
        Assert.Equal(processedTexts.First(), firstText);
    }
}