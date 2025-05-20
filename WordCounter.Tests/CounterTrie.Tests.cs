using Moq;
using WordCounter.DataStructures;

namespace WordCounter.Tests;

public class CounterTrieTests
{
    [Fact]
    public void GivingMockedTraversalAlgorithm_ShouldBuildCounterTrie_WhenInitialized()
    {
        // Arrange
        var traversalMock = new Mock<ITrieTraversal>();
        var counterTrie = new CounterTrie(traversalMock.Object);

        // Act

        // Assert
        Assert.NotNull(counterTrie);
    }

    [Theory]
    [InlineData("Go", "do", "that", "thing", "that", "you", "do", "so", "well")]
    [InlineData("I", "play", "football", "well")]
    [InlineData("that", "these", "those", "the")]
    [InlineData()]
    [InlineData(" ")]
    public void GivingSomeWords_ShouldCountDistinctWords_WhenTraversed(params string[] words)
    {
        // Arrange
        var counterTrie = new CounterTrie(new CounterTrieDfsTraversal());

        // Act
        int wordsCount = 0;
        foreach (var word in words)
            counterTrie.EnterWord(word);
        counterTrie.ProcessAllWords((string word, int count) => { wordsCount++; });

        // Assert
        Assert.Equal(wordsCount, words.Distinct().ToList().Count);
    }
}