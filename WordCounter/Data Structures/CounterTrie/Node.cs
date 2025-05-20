namespace WordCounter.DataStructures;

/// <summary>
/// Single node that is build item for a <see cref="CounterTrie"/>.
/// </summary>
public class Node
{
    private Dictionary<char, Node> _successors;

    /// <summary>
    /// Constructor for <see cref="Node"/>.<br/>
    /// Initializes basic internals.
    /// </summary>
    public Node()
    {
        _successors = new();
        Counter = 0;
    }

    /// <summary>
    /// Constructor for <see cref="Node"/>.<br/>
    /// In addition to default constructor, this one allows to set a containing character.
    /// </summary>
    /// <param name="character">Character that should represent the node.</param>
    public Node(char character) : this()
    {
        Character = character;
    }

    /// <summary>
    /// Character representing the node.
    /// </summary>
    public char Character { get; set; }

    /// <summary>
    /// Number representing the frequency of the word in processed texts.<br/>
    /// Value `0` means it is intermediary node and does not define a word.
    /// </summary>
    public int Counter { get; set; }

    /// <summary>
    /// Allows to add a node that would be successor of the current node.<br/>
    /// Safe against a duplication addition try (false value would be return, no exception).
    /// </summary>
    /// <param name="character">Character representing the new node.</param>
    /// <returns>If a node was added it would return true, otherwise false.</returns>
    public bool AddSuccessorIfNeeded(char character)
    {
        return _successors.TryAdd(character, new Node(character));
    }

    /// <summary>
    /// Get successors of the current node.
    /// </summary>
    /// <returns>Return list of successor nodes.</returns>
    public List<Node> GetSuccessors()
    {
        return _successors.Values.ToList();
    }

    /// <summary>
    /// Get specific successor that is representing by particular character given in a parameter.
    /// </summary>
    /// <param name="character">Character that defines a successor to be returned.</param>
    /// <returns>
    /// A successor of the current node that is represented by particular character.<br/>
    /// If there is no, then return null.
    /// </returns>
    public Node? GetSuccessor(char character)
    {
        _successors.TryGetValue(character, out Node? node);
        return node;
    }
}