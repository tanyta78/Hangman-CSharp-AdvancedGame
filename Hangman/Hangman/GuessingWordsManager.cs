﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private static HangmanContext dbContext = new HangmanContext();

        private static char startingChar { get; set; }
        private static int selectedWordId { get; set; }
        private static List<string> WordsList { get; set; }
        private static List<string> filtered { get; set; }
        private static ConsoleKeyInfo LastKeyPressed { get; set; }

        public static void AddWords()
        {
            Console.Clear();

            OpenFileDialog ofd = new OpenFileDialog();

            string wordsPath = "";
            var exited = false;
            var t = new Thread((ThreadStart) (() =>
            {
                OpenFileDialog fbd = new OpenFileDialog();
                ofd.Filter = "TXT|*.txt";

                if (ofd.ShowDialog() == DialogResult.Cancel)
                {
                    exited = true;
                }
                else
                {
                    wordsPath = ofd.FileName;
                }
            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            if (!exited)
            {
                if (!File.Exists(wordsPath))
                {
                    File.Create(wordsPath);
                }

                int level = 0;
                var words = File.ReadAllLines(wordsPath).Where(w => w != "").ToList();

                var wordsToAdd = new List<Words>();

                //determine level of hardness for each word
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
                        Console.Write(word, Color.LightPink);
                        Console.Write(" is already available\n", Color.Red);
                    }
                    else
                    {
                        wordsToAdd.Add(wordToAdd);
                    }
                }

                dbContext.Words.AddOrUpdate(wordsToAdd.ToArray());
                try
                {
                    dbContext.SaveChanges();

                    foreach (var word in wordsToAdd)
                    {
                        Console.Write(word.Name, Color.LightPink);
                        Console.Write(" added successfully\n", Color.LimeGreen);
                    }

                    Thread.Sleep(1500);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                    throw;
                }
            }

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
                var flag = true;
                switch (pressedKey.Key)
                {
                    case ConsoleKey.RightArrow:
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

                        startingChar = 'A';
                        PrintAlphabet();
                        ChooseChar();
                        pressedKey = Console.ReadKey();
                        break;
                    case ConsoleKey.Tab:
                        flag = Search();
                        PrintAlphabet();
                        pressedKey = Console.ReadKey();
                        break;
                    default:
                        Functions.ClearCurrentConsoleLine();
                        pressedKey = Console.ReadKey();
                        break;
                }
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

                //update lists
                WordsList = WordsList.Where(x => x != word).ToList();
                filtered = filtered.Where(x => x != word).ToList();
            }

            Thread.Sleep(2000);
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

            System.Console.WriteLine();
            Console.WriteLine("Press Tab to switch to Search mode: ", Color.LightPink);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search">dummy string parameter to print "switch to character selection mode"</param>
        private static void PrintAlphabet(string search)
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

            System.Console.WriteLine();
            Console.WriteLine("Press Tab to switch to character selection mode: ", Color.LightPink);
        }

        private static void LoadWords()
        {
            using (new PleaseWait())
            {
                //create query to db if list is empty
                if (WordsList == null || WordsList.Count == 0)
                {
                    WordsList = dbContext.Words.Select(x => x.Name).ToList();
                }
            }
        }

        public static void ListWords()
        {
            Console.Clear();
            Mode.Set(GameMode.Dictionary);

            LoadWords();
            var groupedWords = WordsList.Where(w => !string.IsNullOrWhiteSpace(w)).GroupBy(w => w.ToLower()[0])
                .OrderBy(g => g.Key);

            var maxLineLenght = Constants.ConsoleDictionaryWidth - 1;
            var linesPerPage = Constants.ConsoleMenuHeigth - 2;
            var currentPageNumber = 0;
            var allText = new StringBuilder();

            foreach (var group in groupedWords)
            {
                allText.AppendLine($"[{group.Key} - {group.ToList().Count} words]");
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
            Mode.Set(GameMode.Dictionary);

            LoadWords();

            selectedWordId = 0;
            filtered = WordsList.Where(w => w.ToLower()[0] == startingChar.ToString().ToLower()[0]).OrderBy(w => w)
                .ToList();

            int currentPage = 1;

            var allowedLines = Constants.ConsoleDictionaryHeigth - 3;
            int pages = filtered.Count / allowedLines;

            if (filtered.Count % Constants.ConsoleDictionaryHeigth != 0)
            {
                pages++;
            }

            //Three lines go to user info output (directions, etc)


            while (true)
            {
                Console.Clear();
                var linesOnCurrentPage = Math.Min(allowedLines, filtered.Count - (currentPage - 1) * allowedLines);

                //printing current page
                var startingIndex = (currentPage - 1) * allowedLines;
                for (int i = startingIndex; i < startingIndex + linesOnCurrentPage; i++)
                {
                    if (i != selectedWordId)
                    {
                        Console.WriteLine($"{i + 1}. {filtered[i]}", Color.LightPink);
                    }
                    else
                    {
                        Console.WriteLine($">> {i + 1}. {filtered[i]}<<", Color.LimeGreen);
                    }
                }

                //print navigation info
                Console.WriteLine("Press Escape to go back, Right/Left arrow to navigate", Color.Yellow);
                Console.WriteLine($"Page {currentPage}|{pages}", Color.Yellow);

                var key = Console.ReadKey();

                //changing pages
                var exit = false;
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentPage == 1)
                        {
                            currentPage = pages;
                        }
                        else
                        {
                            --currentPage;
                        }
                        selectedWordId = (currentPage - 1) * allowedLines;
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentPage == pages)
                        {
                            currentPage = 1;
                        }
                        else
                        {
                            ++currentPage;
                        }
                        selectedWordId = (currentPage - 1) * allowedLines;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedWordId++;
                        if (selectedWordId == filtered.Count)
                        {
                            currentPage = 1;
                            selectedWordId = 0;
                            break;
                        }

                        if (selectedWordId % allowedLines == 0)
                        {
                            ++currentPage;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectedWordId == 0)
                        {
                            selectedWordId = filtered.Count - 1;
                            currentPage = pages;
                        }
                        else
                        {
                            selectedWordId--;
                            if ((selectedWordId + 1) % allowedLines == 0)
                            {
                                --currentPage;
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        DeleteWord(filtered[selectedWordId]);
                        break;
                    default:
                        key = Console.ReadKey();
                        break;
                }

                if (exit)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Searches through the loaded words on character input for words starting with the current substring
        /// </summary>
        /// <param name="substring"></param>
        private static bool Search(string substring = "")
        {
            PrintAlphabet("");
            //create query to db if list is empty
            if (WordsList == null || WordsList.Count == 0)
            {
                WordsList = dbContext.Words.Select(x => x.Name).ToList();
            }

            ListWords(substring);

            string character = "";

            //on input - CAN INPUT LETTERS and TAB ONLY
            if (LastKeyPressed.Key == ConsoleKey.Tab)
            {
                //recursion bottom 
                PrintAlphabet();
                return false;
            }
            else if (LastKeyPressed.Key == ConsoleKey.Backspace)
            {
                try
                {
                    substring = substring.Substring(0, substring.Length - 1);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    return false;
                }
            }

            var lastKeyChar = LastKeyPressed.KeyChar.ToString().ToLower()[0];
            if (lastKeyChar >= 'a' && lastKeyChar <= 'z') // is a letter
            {
                character = lastKeyChar.ToString();
            }
            else if (lastKeyChar == '\b')
            {
                //if backspace do nothing
                if (substring == "")
                {
                    LastKeyPressed = new ConsoleKeyInfo();
                    return false;
                }
            }
            else
            {
                //bad input, read again
                var input = Console.ReadKey();
                character = input.KeyChar.ToString();
            }

            if (!Search(substring + character))
            {
                return false;
            }

            return Search(substring + character);
        }

        private static void PrintSearched(string substring)
        {
            Console.SetCursorPosition(0, 4);
            Functions.ClearCurrentConsoleLine();
            Console.WriteLine("Searched word: " + substring);
        }

        

        private static void ListWords(string substring)
        {
            if (substring == String.Empty)
            {
                return;
            }

            selectedWordId = 0;
            filtered = WordsList.Where(x => x.ToLower().StartsWith(substring.ToLower())).ToList();

            int currentPage = 1;

            //Three lines go to user info output (directions, etc)
            var allowedLines = Constants.ConsoleDictionaryHeigth - 8;

            int pages = filtered.Count / allowedLines;

            if (filtered.Count % Constants.ConsoleDictionaryHeigth != 0)
            {
                pages++;
            }

            //draw pages loop
            while (true)
            {
                Console.Clear();
                PrintAlphabet("");
                PrintSearched(substring);

                var linesOnCurrentPage = Math.Min(allowedLines, filtered.Count - (currentPage - 1) * allowedLines);

                //printing current page
                var startingIndex = (currentPage - 1) * allowedLines;
                for (int i = startingIndex; i < startingIndex + linesOnCurrentPage; i++)
                {
                    if (i != selectedWordId)
                    {
                        Console.WriteLine($"{i + 1}. {filtered[i]}", Color.LightPink);
                    }
                    else
                    {
                        Console.WriteLine($">> {i + 1}. {filtered[i]}<<", Color.LimeGreen);
                    }
                }

                //print navigation info
                Console.WriteLine("Right/Left arrow to navigate", Color.Yellow);

                if (linesOnCurrentPage == 0 && currentPage == 1)
                {
                    Console.WriteLine($"Page 0|{pages}", Color.Yellow);
                }
                else
                {
                    Console.WriteLine($"Page {currentPage}|{pages}", Color.Yellow);
                }

                var key = Console.ReadKey();

                //changing pages
                var exit = false;
                switch (key.Key)
                {
                    case ConsoleKey.Tab:
                    case ConsoleKey.Backspace:
                        exit = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentPage == 1)
                        {
                            currentPage = pages;
                        }
                        else
                        {
                            --currentPage;
                        }
                        selectedWordId = (currentPage - 1) * allowedLines;
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentPage == pages)
                        {
                            currentPage = 1;
                        }
                        else
                        {
                            ++currentPage;
                        }
                        selectedWordId = (currentPage - 1) * allowedLines;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedWordId++;
                        if (selectedWordId == filtered.Count)
                        {
                            currentPage = 1;
                            selectedWordId = 0;
                            break;
                        }

                        if (selectedWordId % allowedLines == 0)
                        {
                            ++currentPage;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectedWordId == 0)
                        {
                            selectedWordId = filtered.Count - 1;
                            currentPage = pages;
                        }
                        else
                        {
                            selectedWordId--;
                            if ((selectedWordId + 1) % allowedLines == 0)
                            {
                                --currentPage;
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        DeleteWord(filtered[selectedWordId]);
                        break;
                    default:
                        exit = true;
                        break;
                }

                LastKeyPressed = key;

                if (exit)
                {
                    break;
                }
            }
        }
    }
}