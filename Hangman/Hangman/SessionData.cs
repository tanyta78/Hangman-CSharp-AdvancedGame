using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class SessionData
    {
        public static bool LoggedIn { get; set; }

        public static string UserDataPath = @".\..\..\user.txt";

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
                LoggedIn = true;
            }
            else
            {
                LoggedIn = false;
            }
        }

        public static void LogOut()
        {
            File.Delete(UserDataPath);
            LoggedIn = false;
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
