namespace Hangman
{
    using System.Collections.Generic;
    using System.IO;
    using Console = Colorful.Console;

    public class GuessingWordsManager
    {
        private const string path = "./../../words.txt";

        public static void AddWords()
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            Console.Clear();
            Console.WriteLine("Enter a word to add or STOP to go back");

            var words = new List<string>();
            while (true)
            {
                var word = Console.ReadLine();
                if (word == "STOP")
                {
                    break;
                }
                words.Add(word);
            }
            File.AppendAllLines(path, words);

            Menu.InitialiseMenu();
        }
    }
}
