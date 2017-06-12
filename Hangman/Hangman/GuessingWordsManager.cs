using Hangman.Utilities;

namespace Hangman
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Console = Colorful.Console;

    public class GuessingWordsManager
    {
        private const string wordsPath = "../../Dictionary/words.txt";

        public static void AddWords()
        {
            if (!File.Exists(wordsPath))
            {
                File.Create(wordsPath);
            }

            Console.Clear();
            Console.WriteLine("Enter a word to add or STOP to go back");

            var words = new List<string>();
            while (true)
            {
                var word = Console.ReadLine();
                if (word == Message.Warning.Stop)
                {
                    break;
                }
                words.Add(word);
            }

            File.AppendAllLines(wordsPath, words);

            Menu.Initialize();
        }

        public static void RemoveWord()
        {
            Console.Clear();
            Console.WriteLine("Enter the word to be removed: ", Color.Yellow);
            var word = Console.ReadLine();
            var words = File.ReadAllLines(wordsPath).ToList();
            if (words.Remove(word))
            {
                File.WriteAllLines(wordsPath, words);
                Console.WriteLine("Success!", Color.Lime);
            }
            else
            {
                Console.WriteLine("The word is not contained in the list!", Color.Red);
            }
            Console.WriteLine(Message.Info.PressAnyKeyForMenu, Color.LightSteelBlue);

            Menu.Initialize();
        }

        public static void ListWords()
        {
            Console.Clear();
            var words = File.ReadAllLines(wordsPath);
            var groupedWords = words.Where(w => !string.IsNullOrWhiteSpace(w)).GroupBy(w => w[0]).OrderBy(g => g.Key);

            foreach (var group in groupedWords)
            {
                Console.WriteLine(group.Key, Color.Red);
                Console.WriteLine($"[{string.Join(", ", group.ToList().OrderBy(w => w))}]", Color.DarkOrchid);
            }
            Console.WriteLine(Message.Info.PressAnyKeyForMenu, Color.LightSteelBlue);
            Console.ReadKey();

            Menu.Initialize();
        }
    }
}
