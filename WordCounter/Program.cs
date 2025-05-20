using BenchmarkDotNet.Running;
using WordCounter;
using WordCounter.Common;
using WordCounter.DataStructures;
using WordCounter.Services.WordCounterService;

class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Dictionary<string, string> texts = await PrepareTexts();
            if (texts.Count == 0)
                return;

            ProcessText processTextStrategy = ChooseProcessingTextStrategy();
            IWordCounterService wordCounterService = ChooseUnderlyingDataStructure(processTextStrategy);

            LoadTexts(texts, wordCounterService);

            Console.Clear();
            wordCounterService.GetWordFrequencies();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Global exception has been caught.");
            Console.WriteLine($"Message: {ex.Message}");
        }
        finally
        {
            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit application.");
            Console.ReadLine();

        }
    }

    /// <summary>
    /// Prepare texts to be processed by Word Counter Service later on.
    /// </summary>
    /// <returns>Collection of texts with their names.</returns>
    static async Task<Dictionary<string, string>> PrepareTexts()
    {
        Dictionary<string, string> texts = new();
        string? choice;
        bool reask = true;

        do
        {
            DisplayIntrodutionAndCurrentTexts(texts);

            Console.WriteLine("Choices (case insensitive, confirm with ENTER):");
            Console.WriteLine(" - 'add disk' (or 'ad') - for adding a file from disk");
            Console.WriteLine(" - 'add internet' (or 'ai') - for adding a file from the internet");
            Console.WriteLine(" - 'remove' (or 'r') - for removing a file");
            Console.WriteLine(" - 'default' (or 'd') - for adding both files from task description");
            Console.WriteLine(" - 'next' (or 'n') - for going to next step");
            Console.WriteLine("Instead, type 'benchmark' if you want to execute benchmark framework. You must be in release mode.");
            choice = Console.ReadLine();

            switch (choice?.ToLower())
            {
                case "benchmark":
                    BenchmarkRunner.Run<WordCounterServicesBenchmark>();
                    return new Dictionary<string, string>();
                case "add disk":
                case "ad":
                    AddTextFromDisk(texts);
                    break;
                case "add internet":
                case "ai":
                    await AddTextFromInternet(texts);
                    break;
                case "remove":
                case "r":
                    RemoveText(ref texts);
                    break;
                case "default":
                case "d":
                    texts.Add("first text from description", File.ReadAllText(@"Data\1st text from task description.txt"));
                    texts.Add("second text from description", File.ReadAllText(@"Data\2nd text from task description.txt"));
                    break;
                case "next":
                case "n":
                    reask = false;
                    break;
                default:
                    Console.WriteLine("Not recognized choice. Please try again.");
                    break;
            }
        }
        while (texts.Count == 0 || reask);

        DisplayIntrodutionAndCurrentTexts(texts);

        return texts;

        static void DisplayIntrodutionAndCurrentTexts(Dictionary<string, string> texts, bool cleanConsole = true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The application flow:");
            Console.WriteLine("1. pick texts or run benchmark");
            Console.WriteLine("2. choose processing strategy (buffer or regex)");
            Console.WriteLine("3. choose underlying data structure (dictionary or trie)");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Current texts:");
            foreach (var text in texts)
                Console.WriteLine($"- text named '{text.Key}', {text.Value.Length} characters long");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
        }

        static string? AddTextFromDisk(Dictionary<string, string> texts)
        {
            Console.WriteLine("Write text name:");
            var textName = Console.ReadLine();
            Console.WriteLine("Write file path:");
            var filePath = Console.ReadLine();
            if (string.IsNullOrEmpty(textName) == false && File.Exists(filePath))
                texts.TryAdd(textName, File.ReadAllText(filePath));
            return textName;
        }

        static async Task AddTextFromInternet(Dictionary<string, string> texts)
        {
            Console.WriteLine("Write text name:");
            var textName = Console.ReadLine();
            Console.WriteLine("Write file url:");
            var url = Console.ReadLine();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.ParseAdd("text/plain");
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(textName) == false)
                texts.TryAdd(textName, text);
        }

        static void RemoveText(ref Dictionary<string, string> texts)
        {
            Console.WriteLine("Write text name:");
            var textName = Console.ReadLine();
            if (string.IsNullOrEmpty(textName) == false)
                texts.Remove(textName);
        }
    }

    /// <summary>
    /// Help choose and return strategy that would process texts. For example string buffer or Regex tool.
    /// </summary>
    static ProcessText ChooseProcessingTextStrategy()
    {
        while (true)
        {
            Console.WriteLine("Choose processing text strategy:");
            Console.WriteLine(" - 'buffer' (or 'b')");
            Console.WriteLine(" - 'regex' (or 'r')");
            string? choice = Console.ReadLine();

            switch (choice?.ToLower())
            {
                case "buffer":
                case "b":
                    return ProcessTextByBuffer.ProcessText;
                case "regex":
                case "r":
                    return ProcessTextByRegex.ProcessText;
                default:
                    Console.WriteLine("Not recognized choice. Please try again.");
                    break;
            }
        }
    }

    /// <summary>
    /// Help choose the implementation of Word Counter Service that would use particular data structure behind the scene. It might be <see cref="Dictionary{TKey, TValue}"/> or <see cref="CounterTrie"/>.
    /// </summary>
    static IWordCounterService ChooseUnderlyingDataStructure(ProcessText processText)
    {
        ConsumeSingleResult displayToConsole = (string word, int counter) => Console.WriteLine($"{counter}: {word}");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Choose underlying data structure:");
            Console.WriteLine(" - 'dictionary' (or 'd')");
            Console.WriteLine(" - 'trie' (or 't')");
            string? choice = Console.ReadLine();

            switch (choice?.ToLower())
            {
                case "dictionary":
                case "d":
                    return new DictionaryWordCounterService(processText, displayToConsole);
                case "trie":
                case "t":
                    return new CounterTrieWordCounterService(processText, displayToConsole, new CounterTrieDfsTraversal());
                default:
                    Console.WriteLine("Not recognized choice. Please try again.");
                    break;
            }
        }
    }

    /// <summary>
    /// Load texts from a collection to the specific implementation of Word Counter Service.
    /// </summary>
    static void LoadTexts(Dictionary<string, string> texts, IWordCounterService wordCounterService)
    {
        foreach (var text in texts)
            wordCounterService.AddText(text.Key, text.Value);
    }
}