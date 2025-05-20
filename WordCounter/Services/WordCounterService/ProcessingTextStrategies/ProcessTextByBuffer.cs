using System.Text;

namespace WordCounter.Services.WordCounterService;

/// <summary>
/// Strategy to process text using a <see cref="StringReader"/> buffer.
/// </summary>
public static class ProcessTextByBuffer
{
    /// <summary>
    /// Method processing text using a <see cref="StringReader"/> buffer.<br/>
    /// It uses action defined in parameter for processing a single result. For example it may add it to a data structure.<br/>
    /// This method implements <see cref="Common.ProcessText"/> delegate requirements.
    /// </summary>
    /// <param name="text">Text to be processed.</param>
    /// <param name="processSingleResult">Action executed for every single result.</param>
    public static void ProcessText(string text, Action<string> processSingleResult)
    {
        using (var buffer = new StringReader(text))
        {
            int currentCharacter;
            StringBuilder wordBuilder = new();
            while ((currentCharacter = buffer.Read()) > -1)
            {
                if (IsLetter(currentCharacter))
                    wordBuilder.Append((char)currentCharacter);
                else
                {
                    ProcessSingleWord(processSingleResult, wordBuilder);
                    wordBuilder = new();
                }
            }
            ProcessSingleWord(processSingleResult, wordBuilder);
        }
    }

    private static void ProcessSingleWord(Action<string> processSingleResult, StringBuilder wordBuilder)
    {
        if (wordBuilder.Length > 0)
            processSingleResult(wordBuilder.ToString());
    }

    private static bool IsLetter(int asciiNumber)
    {
        return asciiNumber >= 65 && asciiNumber <= 90 || asciiNumber >= 97 && asciiNumber <= 122;
    }
}
