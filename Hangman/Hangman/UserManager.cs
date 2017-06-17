using Database;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;
namespace Hangman
{
    public static class UserManager
    {

        public static void TryReadUserFromFile()
        {
            if (!SessionData.LoggedIn)
            {
                SessionData.ReadUserData();
                if (SessionData.LoggedIn)
                {
                    UserManager.CurrentUser = SessionData.CurrentUser;
                }
            }
        }

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

        public static void RegisterUser()
        {
            Console.WriteLine("Please input a username:", Color.Aquamarine);
            var name = Console.ReadLine();
            var users = dbContext.Users.Where(x => x.Name.ToLower() == name.ToLower()).ToList();
            if (users.Count == 0)
            {
                Console.WriteLine("Please input a password:", Color.Aquamarine);
                var password = Console.ReadLine();
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
                UserManager.RegisterUser();
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
                UserManager.RegisterUser();
            }
            else
            {
                Console.WriteLine("Please input a password:", Color.Aquamarine);
                var password = Console.ReadLine();
                if (password != users[0].Password)
                {
                    Console.WriteLine("Wrong password:", Color.Red);
                    UserManager.LogIn();
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

    }
}