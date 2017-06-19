using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Database;
using Hangman.Utilities;
using Console = Colorful.Console;
using Message = Hangman.Utilities.Message;
using System.Text;

namespace Hangman
{
    public class GuessingWordsManager
    {
        //        private const string wordsPath = "../../Dictionary/words.txt";
        private static HangmanContext dbContext = new HangmanContext();

        private static char startingChar { get; set; }
        private static int selectedWordId { get; set; }
        private static List<string> WordsList { get; set; }
        private static List<string> filtered { get; set; }

        public static void AddWords()
        {
            Console.Clear();

            OpenFileDialog ofd = new OpenFileDialog();

            string wordsPath = "";
            var t = new Thread((ThreadStart)(() =>
           {
               OpenFileDialog fbd = new OpenFileDialog();
               ofd.Filter = "TXT|*.txt";

               if (ofd.ShowDialog() == DialogResult.Cancel)
                   return;
               else
               {
                   wordsPath = ofd.FileName;
               }
           }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            Console.WriteLine(wordsPath);


            if (!File.Exists(wordsPath))
            {
                File.Create(wordsPath);
            }

            int level = 0;
            var words = File.ReadAllLines(wordsPath).Where(w => w != "").ToList();

            var wordsToAdd = new List<Words>();

            foreach (var word in words)
            {
                if (word.Length <= 5)
                {
                    level = 1;
                }
                else if (word.Length > 5 && word.Length <= 10)
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
                else if (word.Length > 20)
                {
                    level = 5;
                }

                var wordToAdd = new Words()
                {
                    Name = word,
                    Level = level
                };

                var isInDB = dbContext.Words.Any(x => x.Name.ToLower() == word);

                if (isInDB)
                {
                    Console.WriteLine("{0} is already available", word, Color.Red);
                }
                else
                {
                    wordsToAdd.Add(wordToAdd);
                }
            }

            dbContext.Words.AddOrUpdate(wordsToAdd.ToArray());
            dbContext.SaveChanges();

            Menu.Initialize();
        }

        public static void RemoveWord()
        {
            startingChar = 'A';

            PrintAlphabet();
            ChooseChar();

            Console.WriteLine(Message.PressAnyKeyForMenu, Color.LightSteelBlue);
            Menu.Initialize();
        }

        private static void ChooseChar()
        {
            var pressedKey = Console.ReadKey();

            while (pressedKey.Key != ConsoleKey.Escape)
            {
                switch (pressedKey.Key)
                {
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.UpArrow:
                        if (startingChar != 'Z')
                        {
                            ++startingChar;
                            PrintAlphabet();
                        }
                        else
                        {
                            startingChar = 'A';
                            PrintAlphabet();
                        }
                        pressedKey = Console.ReadKey();
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.DownArrow:
                        if (startingChar != 'A')
                        {
                            --startingChar;
                            PrintAlphabet();
                        }
                        else
                        {
                            startingChar = 'Z';
                            PrintAlphabet();
                        }
                        pressedKey = Console.ReadKey();
                        break;
                    case ConsoleKey.Enter:
                        selectedWordId = 0;
                        ListWords(selectedWordId);
                        break;
                }

            }
        }

        private static void ChooseWord()
        {
            //TODO Pagination
            Console.SetCursorPosition(1, selectedWordId - 7 >= 0 ? selectedWordId - 7 : 0);
            var pressedKey = Console.ReadKey();

            while (pressedKey.Key != ConsoleKey.Escape)
            {
                switch (pressedKey.Key)
                {
                    case ConsoleKey.DownArrow:
                        ListWords(++selectedWordId);
                        break;
                    case ConsoleKey.UpArrow:
                        ListWords(--selectedWordId);
                        break;
                    case ConsoleKey.Enter:
                        DeleteWord(filtered[selectedWordId]);
                        break;
                }

                System.Console.WriteLine("press ESCAPE key to go back to menu");
                pressedKey = Console.ReadKey();
            }
        }

        private static bool areYouSure()
        {
            Console.Clear();
            Console.WriteLine(">>> Are you sure you want to delete this word??? Y/N <<<", Color.Red);

            while (true)
            {
                var input = Console.ReadKey();
                switch (input.KeyChar)
                {
                    case 'y':
                    case 'Y':
                        return true;
                    case 'n':
                    case 'N':
                        return false;
                    default:
                        input = Console.ReadKey();
                        break;
                }
            }
        }

        private static void DeleteWord(string word)
        {
            if (!areYouSure())
            {
                return;
            }

            var wordToRemove = dbContext.Words.FirstOrDefault(x => x.Name == word);
            if (wordToRemove == null)
            {
                Console.WriteLine("The word is not contained in the list!", Color.Red);
            }
            else
            {
                dbContext.Entry(wordToRemove).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();
                Console.SetCursorPosition(0, 1);
                Console.WriteLine("Success!", Color.Lime);
                WordsList = WordsList.Where(x => x != word).ToList();
            }

        }

        private static void PrintAlphabet()
        {
            Console.Clear();
            for (char i = 'A'; i <= 'Z'; i++)
            {
                if (i != startingChar)
                {
                    Console.Write($"[{i}] ", Color.AliceBlue);
                }
                else
                {
                    Console.Write($"[{i}] ", Color.LimeGreen);
                }
            }
            System.Console.WriteLine();
        }

        public static void ListWords()
        {
            Console.Clear();
            Mode.Set(GameMode.Dictionary);

            WordsList = dbContext.Words.Select(x => x.Name).ToList();
            var groupedWords = WordsList.Where(w => !string.IsNullOrWhiteSpace(w)).GroupBy(w => w.ToLower()[0])
                .OrderBy(g => g.Key);

            var maxLineLenght = Constants.ConsoleDictionaryWidth - 1;
            var linesPerPage = Constants.ConsoleMenuHeight - 2;
            var currentPageNumber = 0;
            var allText = new StringBuilder();
            foreach (var group in groupedWords)
            {
                allText.AppendLine($"[{group.Key}]");
                var words = new Stack<string>(group.ToList());
                while (words.Count > 0)
                {
                    var currentLine = "";
                    while (words.Count > 0 && currentLine.Length + words.Peek().Length < maxLineLenght - 2)
                    {
                        currentLine = string.Concat(currentLine, words.Pop());
                        if (words.Count != 0)
                        {
                            currentLine = string.Concat(currentLine, ", ");
                        }
                    }
                    allText.AppendLine(currentLine);
                }
                allText.AppendLine();
            }

            var allLines = allText.ToString().Split('\n');

            while (true)
            {
                Console.Clear();
                var linesOnCurrentPage = Math.Min(currentPageNumber + linesPerPage, allLines.Length);
                for (int i = currentPageNumber; i < linesOnCurrentPage; i++)
                {
                    Console.WriteLine(allLines[i], Color.Pink);
                }
                Console.WriteLine("Press the up and down arrow to navigate or ESC to exit");
                var key = Console.ReadKey();

                var exit = false;
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    case ConsoleKey.UpArrow:
                        if (currentPageNumber > 0)
                        {
                            currentPageNumber -= linesPerPage / 2;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentPageNumber < allLines.Length - linesPerPage)
                        {
                            currentPageNumber += linesPerPage / 2;
                        }
                        break;
                    default:
                        break;
                }

                if (exit)
                {
                    break;
                }
            }

            Menu.Initialize();
        }

        public static void ListWords(int dummy)
        {
            Console.Clear();
            Console.WriteLine("Press Escape to go back", Color.Yellow);
            Mode.Set(GameMode.Dictionary);

            if (WordsList == null || WordsList.Count == 0)
            {
                WordsList = dbContext.Words.Select(x => x.Name).ToList();
            }
            filtered = WordsList.Where(w => w[0].ToString().ToUpper() == startingChar.ToString()).ToList();

            if (selectedWordId == filtered.Count)
            {
                selectedWordId = 0;
            }

            for (int i = 0; i < filtered.Count; i++)
            {
                if (i != selectedWordId)
                {
                    Console.WriteLine(filtered[i], Color.DarkOrchid);
                }
                else
                {
                    Console.WriteLine($">>{filtered[i]}<<", Color.LimeGreen);
                }
            }

            ChooseWord();
        }

        public static void ShowHighScores()
        {
            throw new NotImplementedException();
        }
    }
}