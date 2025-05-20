using BenchmarkDotNet.Attributes;
using WordCounter.Common;
using WordCounter.DataStructures;
using WordCounter.Services.WordCounterService;

namespace WordCounter
{
    /// <summary>
    /// Class defining the benchmark that would be done by BenchmarkDotNet NuGet package.
    /// </summary>
    public class WordCounterServicesBenchmark
    {
        private ConsumeSingleResult _consumeSingleResults;

        public WordCounterServicesBenchmark()
        {
            _consumeSingleResults = (string word, int counter) => { Console.WriteLine($"{counter}: {word}"); };
        }

        [Benchmark]
        public void DictionaryBuffer() =>   ProcessBenchamrk(new DictionaryWordCounterService(ProcessTextByBuffer.ProcessText, _consumeSingleResults));

        [Benchmark]
        public void DictionaryRegex() => ProcessBenchamrk(new DictionaryWordCounterService(ProcessTextByRegex.ProcessText, _consumeSingleResults));

        [Benchmark]
        public void CounterTrieBuffer() => ProcessBenchamrk(new CounterTrieWordCounterService(ProcessTextByBuffer.ProcessText, _consumeSingleResults, new CounterTrieDfsTraversal()));

        [Benchmark]
        public void CounterTrieRegex() => ProcessBenchamrk(new CounterTrieWordCounterService(ProcessTextByRegex.ProcessText, _consumeSingleResults, new CounterTrieDfsTraversal()));

        private static void ProcessBenchamrk(IWordCounterService wordCounterService)
        {
            wordCounterService.AddText("alice", File.ReadAllText(@"Data\SimCorp article from Wikipedia.txt"));
            //wordCounterService.AddText("alice", File.ReadAllText(@"Data\Alice's Adventures in Wonderland.txt"));
            //wordCounterService.AddText("shakespeare", File.ReadAllText(@"Data\The Complete Works of William Shakespeare.txt"));
            wordCounterService.GetWordFrequencies();
        }
    }
}