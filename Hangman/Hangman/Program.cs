using System;
using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;
using System.Linq;
using System.Xml.Serialization;

namespace Hangman
{
    class Program
    {

        static void Main()
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
            Console.WriteLine(String.Join(" ", board),Color.CornflowerBlue);
            Console.WriteLine("");
            Console.WriteLine("Guessed letters: ", Color.Yellow);
            Console.WriteLine("");
            Console.WriteLine(String.Join(" ", guessed),Color.DarkGoldenrod);
            Console.WriteLine("");
        }      
    }
}