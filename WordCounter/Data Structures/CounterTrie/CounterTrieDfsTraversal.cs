using System.Text;
using WordCounter.Common;

namespace WordCounter.DataStructures;

/// <summary>
/// Traversal for <see cref="CounterTrie"/> that process in Depth-first search style.
/// </summary>
public class CounterTrieDfsTraversal : ITrieTraversal
{
    /// <inheritdoc/>
    public void ProcessAllWords(Node head, ConsumeSingleResult processSingleResult)
    {
        ProcessAllWords(head, processSingleResult, new StringBuilder());
    }

    private static void ProcessAllWords(Node node, ConsumeSingleResult processSingleResult, StringBuilder stringBuilder)
    {
        if(node.Character != default(char)) // check if not root case
           stringBuilder.Append(node.Character);

        if (node.Counter > 0)
            processSingleResult(stringBuilder.ToString(), node.Counter);
 
        foreach (var successor in node.GetSuccessors())
            ProcessAllWords(successor, processSingleResult, stringBuilder);

        if(stringBuilder.Length > 0)
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
    }
}