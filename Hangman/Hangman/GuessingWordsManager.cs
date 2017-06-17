    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Database;
    using Hangman.Utilities;
    using Console = Colorful.Console;

namespace Hangman
{
    

    public class GuessingWordsManager
    {
        private const string wordsPath = "../../Dictionary/words.txt";
        private static HangmanContext dbContext = new HangmanContext();

        public static void AddWords()
        {
            if (!File.Exists(wordsPath))
            {
                File.Create(wordsPath);
            }

            Console.Clear();
            Console.WriteLine("Enter a word to add or STOP to go back");

            int level = 0;
            var words = new List<string>();

            while (true)
            {
                var word = Console.ReadLine().ToLower();
                if (word == Message.Stop.ToLower())
                {
                    break;
                }
                words.Add(word);
            }
            foreach (var word in words)
            {
                if (word.Length<=5)
                {
                    level = 1;
                }
                else if (word.Length>5&& word.Length<=10)
                {
                    level = 2;
                }
                else if (word.Length > 10 && word.Length <= 15)
                {
                    level = 3;
                }
                else if (word.Length > 15 && word.Length <= 20)
                {
                    level = 4;
                }
                else if (word.Length>20)
                {
                    level = 5;
                }
                var wordToAdd = new Words()
                {
                    Name = word,
                    Level = level
                };
                var isInDB =  dbContext.Words.Where(x => x.Name.ToLower() == word).ToList().Count;
                if (isInDB > 0)
                {
                    Console.WriteLine("{0} is already available", word, Color.Red);
                }
                else
                {
                    dbContext.Words.Add(wordToAdd);
                    dbContext.SaveChanges();
                }
                
            }
            Menu.Initialize();
        }

        public static void RemoveWord()
        {
            Console.Clear();
            Console.WriteLine("Enter the word to be removed: ", Color.Yellow);
            var word = Console.ReadLine().ToLower();

            var wordToRemove = dbContext.Words.Where(x => x.Name == word).FirstOrDefault<Words>();
            if (wordToRemove==null)
            {     
                Console.WriteLine("The word is not contained in the list!", Color.Red);                     
            }
            else
            {
                dbContext.Entry(wordToRemove).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();
                Console.WriteLine("Success!", Color.Lime);
            }
            Console.WriteLine(Message.PressAnyKeyForMenu, Color.LightSteelBlue);

            Menu.Initialize();
        }

        public static void ListWords()
        {
            Console.Clear();
            var list = dbContext.Words.Select(x => x.Name).ToList();
            var groupedWords = list.Where(w => !string.IsNullOrWhiteSpace(w)).GroupBy(w => w[0]).OrderBy(g => g.Key);

            foreach (var group in groupedWords)
            {
                Console.WriteLine(group.Key, Color.Red);
                Console.WriteLine($"[{string.Join(", ", group.ToList().OrderBy(w => w))}]", Color.DarkOrchid);
            }
            Console.WriteLine(Message.PressAnyKeyForMenu, Color.LightSteelBlue);
            Console.ReadKey();

            Menu.Initialize();
        }
    }
}
