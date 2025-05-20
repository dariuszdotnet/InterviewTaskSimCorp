# InterviewTaskSimCorp
The solution is the answer to my interview task from SimCorp company.
Please check [task description](/Documentation/task_description.md).

# Dear Hiring Manager, Developers

Below I would explain some issues around my implementation. You don't need to read it, I would be happy to present my application and explain all in person. However, maybe you can get some wonders while reading my solution. Then I believe you can find answers for most questions, below.

# Assumptions

## Legit words only
There is no specific definition of word provided. So I make assumption - we want to count only legit words like `well`. Being more specific we want to count only continuous presence of characters from latin alphabet. 

Additionally, we try to withdraw proper words from text surrounded with some other characters, like `well[5]`, `well,` etc. So here, part `well` would count as well. 

However words like `ain't` is not count as a single word. This might be quite easily changed, if requirements needs would specify that. Changes would be required in implementations of `ProcessText` delegates only.

## Data

The task deliver two short texts as samples. I have prepared a small data set in the repository, in a [Data](Data) directory. There are:
- mentioned 2 sample texts
- 2 well known books from [Gutenberg free books repository](https://www.gutenberg.org/ebooks/search/?sort_order=downloads)
- 2 articles from [Wikipedia](https://wikipedia.org)
- empty file
- file with Windows style's new line

This set was used for testing and benchmarking purposes.

## Usage of logic

While description didn't define how the logic should be executable - just algorithm in library, a console app, a web API, a desktop app - I chose the most basic way - a console application. I believe using it is pretty explanatory due to console outputs.

# Ideas

## Data structure to hold results

Solution that I upload is probably more complex than expected. I haven't plan it, but I leave it in this form to show my reasoning and some of my favorite programming principles, best practises.

After reading the description first that came to my mind was a data structure called Trie or Prefix Tree, dedicated to word processing. While this might be an overhead, this kind of structures works sometimes really well. So I amended my old implementation of this data structure for this task purpose.

After a moment, other obvious solution came to my mind - Dictionary, which is well known for O(1) worst case for inserting and retrieving data, if hash function is well implemented.

Having both ideas and some time I decided to give a try to both data structures.

## Processing texts

The other critical thing to consider was in my opinion - how we would process texts, especially long ones. Since I am a big fan of Regular Expressions and they are perfect for processing texts, I decided to give them a chance, despite this tool is known to be not most performant one. 

If dealing with long inputs buffers was a second choice.

Again, I decided to implement both versions and measure the results. Since I didn't want to make a complex hierarchy, I decided to use on of my favorite architectural principle - composition over inheritence.

## Final solution

Finally, we have something typical inheritence implementation with abstraction of interface and abstract class, as well as particular implementations for both underlying data structures. Then we can inject processing text style, as well as the way how we want to consume results (displaying them into console, or other action). Overall, I can see something similar to template design pattern, strategy design pattern, dependency injection.

My aim was to make this application extendable. So one can easily extend it with another data structure or processing strategy. This should manifest my biggest belief - in long-term Open/Close principle is a the key.

### Regex

Regex is powerful and sensitive tool. We can define pattern for words at least in few styles, among others.

- [a-zA-Z]+ (used)
- \b\w+\b - this takes also numbers, becasue \w is equivalent for all letters, digits and underscore! [check here](https://regex101.com/r/5MDZjE/1)

In production application I would try to extend it to catch unicode characters and words that consists from more than only continuous presence of characters from latin alphabet, so for example include also `ain't`.

# Benchmarking

As we have few implementation ways, I used well known BenchmarkDotNet NuGet package to measure the performance of different solutions.

One can fire benchmarking on its machine, from a console app menu.

It is hardcoded to load 2 entire books - "Alice's Adventures in Wonderland" and "The Complete Works of William Shakespeare".

The results are not suprising. Buffers beats Regex. Dictionary beats Trie. 

| Method            | Mean       | Error    | StdDev   |
|------------------ |-----------:|---------:|---------:|
| DictionaryBuffer  |   165.0 ms |  3.16 ms |  3.64 ms |
| DictionaryRegex   |   776.3 ms | 14.36 ms | 18.16 ms |
| CounterTrieBuffer |   435.0 ms |  5.82 ms |  4.55 ms |
| CounterTrieRegex  | 1,384.7 ms | 27.65 ms | 52.61 ms |

## Unreliable round

I was suprise to observe when Trie won with Dictionary.

This time as an input I loaded a text from the Wikipedia article about SimCorp company. 

Trying to mimick the real scenario - I benchmarked with displaying the words and their relevant frequencies. I am not yet sure about the reason. It might be due to some internals involving JIT, Garbage Collector pressure, or better data preparation.

I believe this result is not trustable. And by doing such scenario I violated a bit benchmarking best practices. When repeating the test, I was getting different results (sometimes dictionary win, sometimes trie).

| Method            | Mean     | Error    | StdDev   |
|------------------ |---------:|---------:|---------:|
| DictionaryBuffer  | 33.02 ms | 0.658 ms | 1.135 ms |
| DictionaryRegex   | 31.88 ms | 0.634 ms | 0.846 ms |
| CounterTrieBuffer | 31.20 ms | 0.614 ms | 0.707 ms |
| CounterTrieRegex  | 33.53 ms | 0.671 ms | 1.064 ms |

# Optimization

## Concurrency

While my solution is already complex for such a task, there is one more critical technique to cover. Multithreading, concurrency, parallelization of computation. All about using more CPU cores at the same time.

I had a quick try to extend my current solution to support this feature. However with no big difference in performance.

I tried to use seperate Task for each processed text. This however would operate on same instance of underlying data. There is big amount of writes. Such a scenario involved using locks which could be the reason while the solution didn't lead to significant performance gains.

Then I tried to use `ConcurrentDictionary` collection, but this didn't change much neither.

At the end I tried to make a local Dictionaries for each Task. But that way requires merging step after all. At least I assume we would like to have a one collection at the end.

Making efficient use of multithreading could be my next priority, if I would work on this project any more.

## Buffering big files

While I have used buffer (particularly `StringReader`) for processing texts, there is one more idea. I could use buffering for scenario of very big files. For now I tested text files of few Mb with no noticeable problem. But also, in case of multithreading, I could use buffers for chunking input text, in scenario when there are less texts to be processed than available CPU cores.

# Ideas for the future

- [ ] efficient multithreading
- [ ] chunking very big files
- [ ] preprocessing texts and normalizing them
- [ ] small WPF app to display results