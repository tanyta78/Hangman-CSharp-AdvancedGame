namespace Hangman
{
    using System;
    using System.Collections.Generic;
    using Hangman.Utilities;
    using Console = Colorful.Console;
    using System.Threading;

    public class Menu
    {
        public static int choice = 1;

        public static void Initialize()
        {
            UserManager.TryReadUserFromFile();

            Mode.Set(GameMode.Menu);

            MakeChoice();
            ExecuteCommand();
        }

        public static List<string> choices = new List<string>()
        {
            "Start Game",
            "Add words",
            "List words",
            "Delete word",
            "View Highscores",
            "Logout",
            "Exit"
        };
        public static List<string> loggerChoices = new List<string>()
        {
            "New player",
            "Log in",
            "View Highscores",
            "Exit"
        };

        private static void PrintChoices(List<string> choices)
        {
            Console.Clear();

            if (SessionData.LoggedIn)
            {
                Console.WriteLine($"Hello {SessionData.CurrentUser} :)", System.Drawing.Color.Green);
            }
            else
            {
                Console.WriteLine($"not logged in.", System.Drawing.Color.Red);
            }
            System.Console.WriteLine();
            for (int i = 0; i < choices.Count; i++)
            {
                if (choice != i + 1)
                {
                    Console.WriteLine($"{i + 1}. {choices[i]}", System.Drawing.Color.GhostWhite);
                }
                else
                {
                    Console.WriteLine($">> {i + 1}. {choices[i]} <<", System.Drawing.Color.Lime);
                }
            }
        }

        public static void MakeChoice()
        {
            while (true)
            {
                var currentChoices = choices;
                if (!SessionData.LoggedIn)
                {
                    currentChoices = loggerChoices;
                }

                PrintChoices(currentChoices);

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
                            choice = currentChoices.Count;
                        }
                        else
                        {
                            choice--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (choice == currentChoices.Count)
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
        private static Dictionary<int, Action> LogerCommands = new Dictionary<int, Action>()
        {
            {1, UserManager.RegisterUser },
            {2, UserManager.LogIn },
            {3, GuessingWordsManager.ShowHighScores },
            {4, () => Console.WriteLine(Message.ThanksForPlaying, System.Drawing.Color.Gold) }
        };

        private static Dictionary<int, Action> Commands = new Dictionary<int, Action>()
        {
            {1, Game.StartGame },
            {2, GuessingWordsManager.AddWords },
            {3, GuessingWordsManager.ListWords },
            {4, GuessingWordsManager.RemoveWord },
            {5, ScoreBoard.PrintScoreBoard },
            {6, UserManager.LogOut },
            {7, () => Console.WriteLine(Message.ThanksForPlaying, System.Drawing.Color.Gold) }
        };

        private static void ExecuteCommand()
        {
            if (!Commands.ContainsKey(choice))
            {
                throw new NotImplementedException();
            }
            if (SessionData.LoggedIn)
            {
                Commands[choice]();
            }
            else
            {
                LogerCommands[choice]();
            }

        }
    }
}