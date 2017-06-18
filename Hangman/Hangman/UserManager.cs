using System;
using Database;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;
namespace Hangman
{
    public static class UserManager
    {
        private static string currentUser = "";
        private static HangmanContext dbContext = new HangmanContext();

        public static string CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                currentUser = value;
            }
        }

        public static void TryReadUserFromFile()
        {
            if (!SessionData.LoggedIn)
            {
                SessionData.ReadUserData();
                if (SessionData.LoggedIn)
                {
                    CurrentUser = SessionData.CurrentUser;
                }
            }
        }

        public static void RegisterUser()
        {
            Console.WriteLine("Please input a username:", Color.Aquamarine);
            var name = Console.ReadLine();
            var users = dbContext.Users.Where(x => x.Name.ToLower() == name.ToLower()).ToList();
            if (users.Count == 0)
            {
                Console.WriteLine("Please input a password:", Color.Aquamarine);
                var password = ReadLinePassword();
                var newUser = new Users()
                {
                    Name = name,
                    Password = password,
                    Score = 0
                };

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                currentUser = name;
                SessionData.WriteUserData(name, password);
                Menu.Initialize();
            }
            else
            {
                Console.WriteLine("Name is not available", Color.Aquamarine);
                RegisterUser();
            }
        }

        public static void LogIn()
        {
            Console.WriteLine("Please input a username:", Color.Aquamarine);
            var name = Console.ReadLine();
            var users = dbContext.Users.Where(x => x.Name.ToLower() == name.ToLower()).ToList();
            if (users.Count == 0)
            {
                Console.WriteLine("No such user!", Color.Red);
                RegisterUser();
            }
            else
            {
                Console.WriteLine("Please input a password:", Color.Aquamarine);
                var password = ReadLinePassword();
                if (password != users[0].Password)
                {
                    Console.WriteLine("Wrong password:", Color.Red);
                    LogIn();
                }
                else
                {
                    currentUser = users[0].Name;
                    SessionData.WriteUserData(name, password);
                    Menu.choice = 1;
                    Menu.Initialize();
                }
            }
        }

        public static void LogOut()
        {
            currentUser = "";
            SessionData.LogOut();
            Menu.choice = 1;
            SessionData.LogOut();
            Menu.Initialize();
        }
        public static string ReadLinePassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }
    }
}