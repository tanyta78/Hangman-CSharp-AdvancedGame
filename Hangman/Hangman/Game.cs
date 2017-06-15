namespace Hangman
{
    using System;
    using System.Collections.Generic;
    using Hangman.Utilities;
    using System.Drawing;
    using System.IO;
    using Console = Colorful.Console;
    using System.Linq;

    public class Game
    {
        private static bool isWon { get; set; }

        public static void StartGame()
        {
            isWon = true;

            //string wordsPath = "./../../testwords.txt";
            string wordsPath = "../../Dictionary/words.txt";

            if (!File.Exists(wordsPath))
            {
                File.Create(wordsPath);
            }
            var words = File.ReadAllLines(wordsPath).Where(w => w != "").ToArray();

            if (words.Length == 0)
            {
                Console.WriteLine(Message.GuessingWordFileEmpty, Color.Red);
                Console.WriteLine(Message.PressAnyKeyForMenu, Color.PaleVioletRed);
                Console.ReadKey();

                Menu.Initialize();
                return;
            }

            Random r = new Random();

            string word = words[r.Next(words.Length)].ToUpper();
            WordGuesser guesser = new WordGuesser(word);

            HashSet<char> guessed = new HashSet<char>();

            ConsoleKeyInfo letterChoice = new ConsoleKeyInfo();

            //TODO: see this shit
            ScoreBoard scores = new ScoreBoard();

            var mistakes = 0;

            while (guesser.ToString() != word && mistakes <= Constants.AllowedMistakes)
            {
                DisplayWordGuesser(guesser, guessed);

                Console.WriteWithGradient("Your guess: (or \"Escape\" to end) ", Color.Yellow, Color.Fuchsia, 15);
                letterChoice = Console.ReadKey();
                if (letterChoice.Key == ConsoleKey.Escape)
                {
                    //TODO: "Are you sure" prompt
                    break;
                }
                char letter = letterChoice.KeyChar;

                if (!word.Contains(letter) && !guessed.Contains(letter))
                {
                    guessed.Add(letter);
                    mistakes++;
                    if (mistakes > Constants.AllowedMistakes)
                    {
                        isWon = false;
                        break;
                    }
                }
            }
                
        }

        private static void DisplayWordGuesser(WordGuesser guesser, HashSet<char> guessed)
        {
            Console.Clear();
            Console.WriteLine(Message.ChooseLetter, Color.Aquamarine);
            Console.WriteLine("----------------------------------");
            Console.WriteLine();
            Console.WriteLine(guesser.ToString(), Color.CornflowerBlue);
            Console.WriteLine();
            Console.WriteLine(Message.AlreadyGuessed, Color.Yellow);
            Console.WriteLine();
            Console.WriteLine(String.Join(" ", guessed), Color.Red);
            Console.WriteLine();
        }
    }
}
