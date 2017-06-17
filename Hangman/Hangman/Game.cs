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
            Mode.Set(GameMode.Game);
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

            ScoreBoard scores = new ScoreBoard();

            //TODO: fix hardcoding this
            GibbetDrawing gibbet = new GibbetDrawing(0,11+15);

            var mistakes = 0;

            while (guesser.ToString() != word && mistakes <= Constants.AllowedMistakes)
            {
                //display word guesser clears whole console => gibbet too
                DisplayWordGuesser(guesser, guessed,word, gibbet);

                Console.SetCursorPosition(gibbet.Location[0],gibbet.Location[1] + 2);
                Console.WriteWithGradient("Your guess: (or \"Escape\" to end) ", Color.Yellow, Color.Fuchsia, 15);
                letterChoice = Console.ReadKey();
                if (letterChoice.Key == ConsoleKey.Escape)
                {
                    //TODO: "Are you sure" prompt
                    Menu.Initialize();
                    break;
                }
                char letter = letterChoice.KeyChar.ToString().ToUpper().First();

                if (!word.Contains(letter) && !guessed.Contains(letter))
                {
                    guessed.Add(letter);
                    mistakes++;
                    if (mistakes > Constants.AllowedMistakes)
                    {
                        isWon = false;
                        break;
                    }
                    gibbet.Update();
                }

                guesser.Update(letter);
            }
            DisplayResult(mistakes,word);
        }

        private static void DisplayWordGuesser(WordGuesser guesser, HashSet<char> guessed,string word,GibbetDrawing gibbet)
        {
            Console.Clear();
            Console.WriteLine(Message.ChooseLetter, Color.Aquamarine);
            Console.WriteLine("----------------------------------" + word);
            Console.WriteLine();
            Console.WriteLine(guesser.ToString(), Color.CornflowerBlue);
            Console.WriteLine();
            Console.WriteLine(Message.AlreadyGuessed, Color.Yellow);
            Console.WriteLine();
            Console.WriteLine(String.Join(" ", guessed), Color.Red);
            Console.WriteLine();
            gibbet.Print();

        }

        private static void DisplayResult(int mistakes,string word)
        {
            if (isWon)
            {
                Console.WriteLine("You got my word!");
                Console.WriteLine($"You won with {mistakes} mistakes");
            }
            else
            {
                Console.WriteLine("Maybe next time");
                Console.WriteLine($"Your word was {word}");
            }
        }

    }
}
