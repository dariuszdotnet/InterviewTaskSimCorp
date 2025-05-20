using WordCounter.Common;

namespace WordCounter.Services.WordCounterService;

/// <summary>
/// Implementation of <see cref="BaseWordCounterService"/> that uses <see cref="Dictionary{TKey, TValue}"/> behind the scene.
/// </summary>
public class DictionaryWordCounterService : BaseWordCounterService
{
    private Dictionary<string, int> _dictionary;
    private readonly ConsumeSingleResult _consumeSingleResult;

    /// <summary>
    /// Constructor for <see cref="CounterTrieWordCounterService"/>.
    /// </summary>
    /// <param name="processText">Injected strategy defining how text should be processed. For example by buffer or Regex tool.</param>
    /// <param name="consumeSingleResult">Injected delegate defining how the single result might be consumed. For example displayed into output.</param>
    public DictionaryWordCounterService(ProcessText processText, ConsumeSingleResult consumeSingleResult) : base(processText)
    {
        _dictionary = new();
        _consumeSingleResult = consumeSingleResult;
    }

    /// <inheritdoc/>
    protected override void ProcessSingleResult(string word)
    {
        if (_dictionary.TryAdd(word, 1) == false)
            _dictionary[word]++;
    }

    /// <inheritdoc/>
    protected override void ConsumeResults()
    {
        foreach (var word in _dictionary)
            _consumeSingleResult(word.Key, word.Value);
    }
}