namespace WordCounter.Services.WordCounterService;

/// <summary>
/// Service for counting the word frequencies in given text items.
/// </summary>
public interface IWordCounterService
{
    /// <summary>
    /// Add text to a collection that would process all of them at once.
    /// </summary>
    /// <param name="name">Name of the text.</param>
    /// <param name="text">Text to be processed.</param>
    /// <returns>True if successfully added, false otherwise.</returns>
    public bool AddText(string name, string text);

    /// <summary>
    /// Remove text from a collection.
    /// </summary>
    /// <param name="name">Name of the text.</param>
    public void RemoveText(string name);

    /// <summary>
    /// Return words and their corresponding frequencies in a given texts collection.
    /// </summary>
    public void GetWordFrequencies();
}