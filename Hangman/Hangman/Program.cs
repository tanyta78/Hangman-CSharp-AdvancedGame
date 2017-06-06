using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangman
{
    class Program
    {

        static void Main(string[] args)
        {
            Random r = new Random();
            List<string> words = new List<string>();
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
            while (String.Join("", board) != word && letterChoice != "QUIT")
            {
                DisplayBoard(board, guessed);
                Console.Write("Your guess: (or \"quit\" to end) ");
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
                    }
                }
            }
            if (letterChoice != "QUIT")
            {
                DisplayBoard(board, guessed);
                Console.WriteLine("You got my word!");
            }
            else
            {
                Console.WriteLine("Maybe next time!");
            }
        }

        static void DisplayBoard(char[] board, HashSet<char> guessed)
        {
            Console.Clear();
            Console.WriteLine("Choose a letter");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("");
            Console.WriteLine(String.Join(" ", board));
            Console.WriteLine("");
            Console.WriteLine("Guessed letters: ");
            Console.WriteLine("");
            Console.WriteLine(String.Join(" ", guessed));
            Console.WriteLine("");
        }

    }
}