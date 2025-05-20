using WordCounter.Common;

namespace WordCounter.DataStructures;

/// <summary>
/// Defines algorithm to traverse a <see cref="CounterTrie"/>.
/// </summary>
public interface ITrieTraversal
{
    /// <summary>
    /// Method going through the whole <see cref="CounterTrie"/> and executing injected action for every traversed node (including head).
    /// </summary>
    /// <param name="head">Head of <see cref="CounterTrie"/> data structure.</param>
    /// <param name="processSingleResult">Action to be executed on every traversed node.</param>
    public void ProcessAllWords(Node head, ConsumeSingleResult processSingleResult);
}