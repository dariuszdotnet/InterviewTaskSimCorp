namespace WordCounter.Common;

/// <summary>
/// Delegate defining strategy pattern that would be used to process given text.<br/>
/// For example Regex process text in a different way than string buffer method.
/// </summary>
/// <param name="text">Text to be processed.</param>
/// <param name="processSingleResult">Action to be injected to the processing strategy. Defines how single result is going be attached to underlying data structure.</param>
public delegate void ProcessText(string text, Action<string> processSingleResult);

/// <summary>
/// Delegate defining how single result would be consumed.<br/>
/// For example it could be displayed on output, but it can be only count or be processed depending on further logic.
/// </summary>
/// <param name="word">Word result.</param>
/// <param name="count">Number meaning frequency for a related word result.</param>
public delegate void ConsumeSingleResult(string word, int count);