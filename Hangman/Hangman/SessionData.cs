using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Hangman
{
    public class SessionData
    {
        public static bool LoggedIn { get; set; }

        public static bool IsAdmin { get; set; }

        public static string UserDataPath = @".\..\..\user.txt";

        public static string ScoreBoardDataPath = @".\..\..\scoreboard.txt";

        public static string CurrentUser { get; set; }

        public static void WriteUserData(string username, string password)
        {
            var userData = new string[] { username, password };
            File.WriteAllLines(UserDataPath, userData);
        }

        public static void ReadUserData()
        {
            if (File.Exists(UserDataPath))
            {
                var userData = File.ReadAllLines(UserDataPath);
                if (userData.Length != 2)
                {
                    throw new SomethingIsWrongWithUserFileException("Something went wrong with the user data file. Contact an administrator or something.");
                }
                var userName = userData[0];
                CurrentUser = userName;
                if (CurrentUser.ToLower() == "admin")
                {
                    IsAdmin = true;
                }
                else
                {
                    IsAdmin = false;
                }
                LoggedIn = true;
            }
            else
            {
                LoggedIn = false;
            }
        }

        public static void WriteScoreboardData(string username, string mistakes)
        {
            var scoreData = new string[] { username, mistakes };
            File.WriteAllLines(ScoreBoardDataPath, scoreData);
        }

        public static List<string> ReadScoreboardData()
        {
            if (File.Exists(ScoreBoardDataPath))
            {
                var scoreboardData = File.ReadAllLines(ScoreBoardDataPath).ToList();

                return scoreboardData;
            }
            else
            {
                throw new SomethingIsWrongWithUserFileException("Something went wrong with the scoreboard data file. Contact an administrator or something.");
            }
            
        }

        public static void LogOut()
        {
            File.Delete(UserDataPath);
            LoggedIn = false;
            IsAdmin = false;
        }
    }
    
    internal class SomethingIsWrongWithUserFileException : Exception
    {
        public SomethingIsWrongWithUserFileException()
        {
        }

        public SomethingIsWrongWithUserFileException(string message) : base(message)
        {
        }

        public SomethingIsWrongWithUserFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SomethingIsWrongWithUserFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
