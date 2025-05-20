using System.Text.RegularExpressions;

namespace WordCounter.Services.WordCounterService;

/// <summary>
/// Strategy to process text using a <see cref="Regex"/> (Regular Expressions) tool.
/// </summary>
public static class ProcessTextByRegex
{
    private const string _wordBoundariesPattern = @"[a-zA-Z]+";

    /// <summary>
    /// Method processing text using a <see cref="Regex"/> (Regular Expressions) tool.<br/>
    /// It uses action defined in parameter for processing a single result. For example it may add it to a data structure.<br/>
    /// This method implements <see cref="Common.ProcessText"/> delegate requirements.
    /// </summary>
    /// <param name="text">Text to be processed.</param>
    /// <param name="processSingleResult">Action executed for every single result.</param>
    public static void ProcessText(string text, Action<string> processSingleResult)
    {
        var wordsRegex = new Regex(_wordBoundariesPattern, RegexOptions.Compiled);

        foreach (Match word in wordsRegex.Matches(text))
            processSingleResult(word.Value);
    }
}