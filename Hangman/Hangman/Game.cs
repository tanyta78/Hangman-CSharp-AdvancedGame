using Database;

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
        private static int mistakes { get; set; }
        private static string word { get; set; }
        private static HangmanContext dbContext = new HangmanContext();
        private static int gameLevel = 1;
        private static Users player = dbContext.Users.FirstOrDefault(x => x.Name == UserManager.CurrentUser);
        private static double finalScore = 0d;
        private static double currentScore = 0d;
        private static GibbetDrawing gibbet;

        public static void StartGame()
        {
            Mode.Set(GameMode.Game);
            isWon = true;

            while (gameLevel < 6 && isWon)
            {
                mistakes = 0;
                string[] words;

                using (new PleaseWait())
                {
                    words = dbContext.Words.Where(x => x.Level == gameLevel).Select(x => x.Name).ToArray();
                }

                PlayGame(words);
                ScoreBoard score = new ScoreBoard(gameLevel, mistakes);

                gameLevel++;
            }

            if (isWon)
            {
                DisplayResult();
            }
            if (player.Score < finalScore)
            {
                player.Score = finalScore;
                dbContext.SaveChanges();
            }

            gameLevel = 1;
            Console.WriteLine("Press any key to go back to the menu.");
            Console.ReadKey();

            Menu.Initialize();
        }

        private static void PlayGame(string[] words)
        {
            Random r = new Random();

            word = words[r.Next(words.Length)].ToUpper();
            WordGuesser guesser = new WordGuesser(word);
            HashSet<char> guessed = new HashSet<char>();

            //11 rows for user info output
            gibbet = new GibbetDrawing(0, Constants.GibbetHeight + 11);

            while (guesser.ToString() != word && mistakes <= Constants.AllowedMistakes)
            {
                DrawGame(guesser, guessed, word, gibbet);

                Console.SetCursorPosition(gibbet.Location[0], gibbet.Location[1] + 2);
                Console.WriteWithGradient("Your guess: (or \"Escape\" to end) ", Color.Yellow, Color.Fuchsia, 25);
                Console.WriteLine();
                ConsoleKeyInfo letterChoice = Console.ReadKey();

                if (letterChoice.Key == ConsoleKey.Escape)
                {
                    Menu.Initialize();
                    break;
                }
                char letter = letterChoice.KeyChar.ToString().ToUpper().First();

                if (!word.Contains(letter) && !guessed.Contains(letter))
                {
                    guessed.Add(letter);
                    mistakes++;
                    gibbet.Update();
                    DrawGame(guesser, guessed, word, gibbet);
                    if (mistakes >= Constants.AllowedMistakes)
                    {
                        isWon = false;
                        currentScore = ScoreBoard.GetScore();
                        finalScore += currentScore;
                        DisplayResult();
                        break;
                    }
                }

                guesser.Update(letter);
                DrawGame(guesser, guessed, word, gibbet);
            }
            currentScore = ScoreBoard.GetScore();
            finalScore += currentScore;
        }

        /// <summary>
        /// Displays 11 rows of user info + gibbet drawing
        /// </summary>
        /// <param name="guesser"></param>
        /// <param name="guessed"></param>
        /// <param name="word"></param>
        /// <param name="gibbet"></param>
        private static void DrawGame(WordGuesser guesser, HashSet<char> guessed, string word, GibbetDrawing gibbet)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------", Color.Aquamarine);
            Console.WriteLine($"Player:{player.Name}  --  Best score:{player.Score:F0}", Color.Khaki);
            Console.WriteLine("----------------------------------", Color.Aquamarine);
            Console.WriteLine($"Current level:{gameLevel}    {Message.ChooseLetter}", Color.Khaki);
            if (!Constants.DEBUG_MODE)
            {
                Console.WriteLine($"---------DEBUG: {word}--------", Color.Aquamarine);
            }
            else
            {
                Console.WriteLine("----------------------------------", Color.Aquamarine);
            }
            Console.WriteLine();
            Console.WriteLine(guesser.ToString(), Color.CornflowerBlue);
            Console.WriteLine();
            Console.WriteLine(Message.AlreadyGuessed, Color.Yellow);
            Console.WriteLine();
            Console.WriteLine(String.Join(" ", guessed), Color.Red);
            Console.WriteLine();
            gibbet.Print();
        }

        private static void DisplayResult()
        {
            Console.SetCursorPosition(0, gibbet.Location[1] + 2);
            if (isWon)
            {
                Console.WriteLine("You got my word!");
                Console.WriteLine($"You won with -> Score:{finalScore:F0}", Color.GreenYellow);
                ScoreBoard.AddNewScore(player, mistakes);
            }
            else
            {
                Console.WriteLine("Maybe next time");
                Console.WriteLine($"Your word was {word}, Score:{finalScore:F0}", Color.Aqua);
            }
        }
    }
}