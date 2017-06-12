using System.Linq;

namespace Hangman
{
    using System;
    using System.Collections.Generic;
    using Hangman.Utilities;
    using System.Drawing;
    using System.IO;
    using Console = Colorful.Console;

    public class Game
    {
        public static void StartGame()
        {
            //string wordsPath = "./../../testwords.txt";
            string wordsPath = "../../Dictionary/words.txt";
            if (!File.Exists(wordsPath))
            {
                File.Create(wordsPath);
            }
            var words = File.ReadAllLines(wordsPath).Where(w => w != "").ToArray();

            if (words.Length == 0)
            {
                Console.WriteLine(Message.Error.GuessingWordFileEmpty, Color.Red);
                Console.WriteLine(Message.Info.PressAnyKeyForMenu, Color.PaleVioletRed);
                Console.ReadKey();

                Menu.Initialize();
                return;
            }

            Random r = new Random();

            string word = words[r.Next(words.Length)].ToUpper();
            char[] letters = word.ToCharArray();
            char[] board = new string('_', word.Length).ToCharArray();

            //the first and the last letters always show up
            board[0] = word[0];
            board[word.Length - 1] = word[word.Length - 1];

            HashSet<char> guessed = new HashSet<char>();

            string letterChoice = "";

            //add scoreboard class
            ScoreBoard scores = new ScoreBoard();

            var mistakes = 0;
            while (String.Join("", board) != word && letterChoice != "QUIT")
            {
                int missedLetters = 0;
                DisplayBoard(board, guessed);
                Console.WriteWithGradient("Your guess: (or \"quit\" to end) ", Color.Yellow, Color.Fuchsia, 15);
                letterChoice = Console.ReadLine().Trim().ToUpper();
                char letter = letterChoice.ToCharArray()[0];

                if (!word.Contains(letter.ToString()))
                {
                    guessed.Add(letter);
                    mistakes++;
                }

                if (letterChoice.Length > 0 && letterChoice != "QUIT")
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (letters[i] == letter)
                        {
                            board[i] = letter;
                        }
                        else
                        {
                            missedLetters++;
                        }
                    }
                }
            }

            if (letterChoice != "QUIT")
            {
                DisplayBoard(board, guessed);
                Console.WriteLine("You got my word!");

                /* please add player mistake in your code
                 */
                Console.WriteLine($"You won with {mistakes} mistakes");
                if (mistakes >= scores.GetLastPositionMistakes())
                {
                    Console.WriteLine(Message.Info.NotHighScore);
                }
                else
                {
                    /*please add player name in your code*/
                    string playerName = "Tanyta";
                    scores.AddNewScore(playerName, mistakes);
                    scores.PrintScoreBoard();
                }
            }
            else
            {
                Console.WriteLine("Maybe next time!");
            }
        }

        private static void DisplayBoard(char[] board, HashSet<char> guessed)
        {
            Console.Clear();
            Console.WriteLine(Message.Info.ChooseLetter, Color.Aquamarine);
            Console.WriteLine("----------------------------------");
            Console.WriteLine("");
            Console.WriteLine(String.Join(" ", board), Color.CornflowerBlue);
            Console.WriteLine("");
            Console.WriteLine(Message.Warning.AlreadyGuessed, Color.Yellow);
            Console.WriteLine("");
            Console.WriteLine(String.Join(" ", guessed), Color.Red);
            Console.WriteLine("");
        }
    }
}
