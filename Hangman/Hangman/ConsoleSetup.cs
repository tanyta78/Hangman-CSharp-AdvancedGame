using System;

namespace Hangman
{
    public class ConsoleSetup
    {
        private static bool IsSetUp = false;

        public static void SetUp()
        {
            if (!IsSetUp)
            {
                Console.CursorVisible = false;
                Console.Title = "HANGMAN";
                IsSetUp = true;
            }
        }
    }
}
