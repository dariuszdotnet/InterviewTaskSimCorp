using WordCounter.Common;
using WordCounter.DataStructures;

namespace WordCounter.Services.WordCounterService;

/// <summary>
/// Implementation of <see cref="BaseWordCounterService"/> that uses <see cref="CounterTrie"/> behind the scene.
/// </summary>
public class CounterTrieWordCounterService : BaseWordCounterService
{
    private CounterTrie _trie;
    private readonly ConsumeSingleResult _consumeSingleResult;

    /// <summary>
    /// Constructor for <see cref="CounterTrieWordCounterService"/>.
    /// </summary>
    /// <param name="processText">Injected strategy defining how text should be processed. For example by buffer or Regex tool.</param>
    /// <param name="consumeSingleResult">Injected delegate defining how the single result might be consumed. For example displayed into output.</param>
    /// <param name="traversalAlgorithm">Algorithm for traversing the <see cref="CounterTrie"/>.</param>
    public CounterTrieWordCounterService(ProcessText processText, ConsumeSingleResult consumeSingleResult, ITrieTraversal traversalAlgorithm) : base(processText)
    {
        _trie = new(traversalAlgorithm);
        _consumeSingleResult = consumeSingleResult;
    }

    /// <inheritdoc/>
    protected override void ProcessSingleResult(string word)
    {
        _trie.EnterWord(word);
    }

    /// <inheritdoc/>
    protected override void ConsumeResults()
    {
        _trie.ProcessAllWords(_consumeSingleResult);
    }
}
