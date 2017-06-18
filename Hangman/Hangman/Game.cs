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
        private static Users player = dbContext.Users.Where(x => x.Name == UserManager.CurrentUser).FirstOrDefault();

        public static void StartGame()
        {
            Mode.Set(GameMode.Game);
            isWon = true;
                       
            //if (words.Length == 0)
            //{
            //    Console.WriteLine(Message.GuessingWordFileEmpty, Color.Red);
            //    Console.WriteLine(Message.PressAnyKeyForMenu, Color.PaleVioletRed);
            //    Console.ReadKey();

            //    Menu.Initialize();
            //    return;
            //}         
            ScoreBoard scores = new ScoreBoard();
            while (gameLevel<6&&isWon == true)
            {
                mistakes = 0;
                var words = dbContext.Words.Where(x => x.Level == gameLevel).Select(x => x.Name).ToArray();
                PlayGame(words);                
                gameLevel++;
            }
            if (isWon)
            {
                 DisplayResult();
            }                       
        }

        private static void PlayGame(string[] words)
        {
            Random r = new Random();

            word = words[r.Next(words.Length)].ToUpper();
            WordGuesser guesser = new WordGuesser(word);
            HashSet<char> guessed = new HashSet<char>();

            //TODO: fix hardcoding this
            GibbetDrawing gibbet = new GibbetDrawing(0, 11 + 15);

            while (guesser.ToString() != word && mistakes <= Constants.AllowedMistakes)
            {
                //display word guesser clears whole console => gibbet too
                DrawGame(guesser, guessed, word, gibbet);

                Console.SetCursorPosition(gibbet.Location[0], gibbet.Location[1] + 2);
                Console.WriteWithGradient("Your guess: (or \"Escape\" to end) ", Color.Yellow, Color.Fuchsia, 15);
                Console.WriteLine();
                ConsoleKeyInfo letterChoice = Console.ReadKey();

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
                        DisplayResult();
                        break;
                    }
                    gibbet.Update();
                }

                guesser.Update(letter);
            }           
        }
        private static void DrawGame(WordGuesser guesser, HashSet<char> guessed,string word,GibbetDrawing gibbet)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------", Color.Aquamarine);
            Console.WriteLine($"Player:{player.Name}  --  Best score:{player.Score}",Color.Khaki);
            Console.WriteLine("----------------------------------", Color.Aquamarine);
            Console.WriteLine($"Current level:{gameLevel}    {Message.ChooseLetter}",Color.Khaki);
            Console.WriteLine("----------------------------------" ,Color.Aquamarine);
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
            if (isWon)
            {
                Console.WriteLine("You got my word!");
                Console.WriteLine($"You won with {mistakes} mistakes",Color.GreenYellow);
                ScoreBoard.AddNewScore(player, mistakes);
            }
            else
            {
                Console.WriteLine("Maybe next time");
                Console.WriteLine($"Your word was {word}",Color.Aqua);
            }
        }

    }
}
