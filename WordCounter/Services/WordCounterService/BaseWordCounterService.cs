using WordCounter.Common;

namespace WordCounter.Services.WordCounterService;

/// <summary>
/// Base class for Word Counter Service.<br/>
/// Support collection of texts to be processed.<br/>
/// Main feature is to receive words and their frequencies within a collection of texts.
/// </summary>
public abstract class BaseWordCounterService : IWordCounterService
{
    /// <summary>
    /// Collections of texts to be processed.
    /// </summary>
    protected Dictionary<string, string> _texts;

    /// <summary>
    /// Delegate defining the strategy that would lead processing texts.
    /// </summary>
    protected ProcessText _processText;

    /// <summary>
    /// Constructor for <see cref="BaseWordCounterService"/>.
    /// </summary>
    /// <param name="processText">Injected strategy for processing texts.</param>
    public BaseWordCounterService(ProcessText processText)
    {
        _texts = new();
        _processText = processText;
    }

    /// <inheritdoc/>
    public bool AddText(string name, string text)
    {
        return _texts.TryAdd(name, text);
    }

    /// <inheritdoc/>
    public void RemoveText(string name)
    {
        _texts.Remove(name);
    }

    /// <inheritdoc/>
    public void GetWordFrequencies()
    {
        foreach (var text in _texts.Values)
            _processText(text, ProcessSingleResult);

        ConsumeResults();
    }

    /// <summary>
    /// Logic for processing a single result.<br/>
    /// For example it can be added to data structure.
    /// </summary>
    /// <param name="word">Single word item to be processed.</param>
    protected abstract void ProcessSingleResult(string word);

    /// <summary>
    /// General way how the already processed results should be consumed.<br/>
    /// </summary>
    protected abstract void ConsumeResults();
}
