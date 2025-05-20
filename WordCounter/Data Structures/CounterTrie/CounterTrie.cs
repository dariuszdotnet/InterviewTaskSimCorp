using WordCounter.Common;

namespace WordCounter.DataStructures;

/// <summary>
/// Data structure based on Trie (called also Prefix Tree).<br/>
/// The main modification is that it stores integer in every node. This might be used for storing frequency for a specific word.<br/>
/// If this number is higher than 0 (defualt value) it means relevant frequency occured for a word (from root to that particular node, included).<br/>
/// Head contain default value for <see cref="char"/> by design.
/// </summary>
public class CounterTrie
{
    private Node _head;
    private ITrieTraversal _traversal;

    /// <summary>
    /// Constructor for <see cref="CounterTrie"/>.
    /// </summary>
    /// <param name="traversal">Traversal algorithm to be injected as <see cref="ITrieTraversal"/> abstraction.</param>
    public CounterTrie(ITrieTraversal traversal)
    {
        _head = new();
        _traversal = traversal;
    }

    /// <summary>
    /// This method allows to enter a word into a Counter Trie.<br/>
    /// It would create required nodes on the way and increment a whole word counter.
    /// </summary>
    /// <param name="word">Word to be added.</param>
    public void EnterWord(string word)
    {
        var currentNode = _head;

        foreach (var character in word)
        {
            currentNode?.AddSuccessorIfNeeded(character);
            currentNode = currentNode?.GetSuccessor(character);
        }

        if (currentNode != null)
            currentNode.Counter++;
    }

    /// <summary>
    /// Execute algorithm that would process through the whole Counter Trie structure and execute injected action.
    /// </summary>
    /// <param name="consumeSingleResult">Action to be excuted for every node in the Counter Trie.</param>
    public void ProcessAllWords(ConsumeSingleResult consumeSingleResult)
    {
        _traversal.ProcessAllWords(_head, consumeSingleResult);
    }
}