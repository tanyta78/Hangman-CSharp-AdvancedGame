namespace Hangman
{
    using System;
    using System.Collections.Generic;
    using Hangman.Utilities;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Console = Colorful.Console;

    public class Menu
    {
        public static void Initialize()
        {
            MakeChoice();
            ExecuteCommand();
        }

        private static List<string> choices = new List<string>()
            {
                "1. Start Game",
                "2. Add words",
                "3. View Highscores",
                "4. Exit"
            };

        private static int choice = 1;

        private static void PrintChoices()
        {
            Console.Clear();
            for (int i = 0; i < choices.Count; i++)
            {
                if (choice != i + 1)
                {
                    Console.WriteLine($"{choices[i]}", System.Drawing.Color.GhostWhite);
                }
                else
                {
                    Console.WriteLine($">> {choices[i]} <<", System.Drawing.Color.Lime);
                }
            }
        }

        private static void MakeChoice()
        {
            while (true)
            {
                PrintChoices();
                var pressedKey = Console.ReadKey().Key;
                if (pressedKey == ConsoleKey.Enter)
                {
                    break;
                }
                switch (pressedKey)
                {
                    case ConsoleKey.UpArrow:
                        if (choice == 1)
                        {
                            choice = choices.Count;
                        }
                        else
                        {
                            choice--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (choice == choices.Count)
                        {
                            choice = 1;
                        }
                        else
                        {
                            choice++;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ExecuteCommand()
        {
            switch (choice)
            {
                case 1:
                    Game.StartGame();
                    break;
                case 2:
                    GuessingWordsManager.AddWords();
                    break;
                case 3:
                    throw new NotImplementedException();
                case 4:
                    Console.WriteLine(Message.Info.ThanksForPlaying, System.Drawing.Color.Gold);
                    break;
                default:
                    break;
            }
        }


    }
}
