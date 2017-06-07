using System.IO;

namespace Hangman
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Console = Colorful.Console;

    public class HangMam
    {

        public static void Main()
        {

            Random r = new Random();
            List<string> words = new List<string>();
            /* 
            StreamReader reader = new StreamReader("../../Dictionary/words.txt");
            string line = reader.ReadLine();
            while (line != null)
            {
                words.Add(line);
                line = reader.ReadLine();
            }
            */




            words.Add("Software");
            words.Add("SoftUni");
            words.Add("Technology");
            words.Add("Computer");
            words.Add("Fundamentals");

            string word = words[r.Next(words.Count)].ToUpper();
            char[] letters = word.ToCharArray();
            char[] board = new string('_', word.Length).ToCharArray();
            HashSet<char> guessed = new HashSet<char>();

            string letterChoice = "";

            //add scoreboard class
            ScoreBoard scores = new ScoreBoard();

            while (String.Join("", board) != word && letterChoice != "QUIT")
            {
                int missedLetters = 0;
                DisplayBoard(board, guessed);
                Console.WriteWithGradient("Your guess: (or \"quit\" to end) ", Color.Yellow, Color.Fuchsia, 15);
                letterChoice = Console.ReadLine().Trim().ToUpper();
                char letter = letterChoice.ToCharArray()[0];
                guessed.Add(letter);

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
                int playerMistakes = 0;
                Console.WriteLine($"You won with {0} mistakes", playerMistakes);
                if (playerMistakes >= scores.GetLastPositionMistakes())
                {
                    Console.WriteLine("Your score did not enter in the BestPlayer Scoreboard");
                }
                else
                {
                    /*please add player name in your code*/
                    string playerName = "Tanyta";
                    scores.AddNewScore(playerName, playerMistakes);
                    scores.PrintScoreBoard();
                }

            }
            else
            {
                Console.WriteLine("Maybe next time!");
            }
        }

        static void DisplayBoard(char[] board, HashSet<char> guessed)
        {
            Console.Clear();
            Console.WriteLine("Choose a letter", Color.Aquamarine);
            Console.WriteLine("----------------------------------");
            Console.WriteLine("");
            Console.WriteLine(String.Join(" ", board), Color.CornflowerBlue);
            Console.WriteLine("");
            Console.WriteLine("Guessed letters: ", Color.Yellow);
            Console.WriteLine("");
            Console.WriteLine(String.Join(" ", guessed), Color.DarkGoldenrod);
            Console.WriteLine("");
        }
    }
}