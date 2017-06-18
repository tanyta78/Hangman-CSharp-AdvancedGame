using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Utilities
{
    public enum GameMode
    {
        Menu,
        Game,
        Dictionary
    }

    public class Mode
    {
        public static void Set(GameMode gm)
        {
            switch (gm)
            {
                case GameMode.Menu:
                    UpdateConsole(Constants.ConsoleMenuWidth, Constants.ConsoleMenuHeight, true);
                    break;
                case GameMode.Game:
                    UpdateConsole(Constants.ConsoleGameWidth, Constants.ConsoleGameHeight, true);
                    break;
                case GameMode.Dictionary:
                    UpdateConsole(Constants.ConsoleDictionaryWidth, Constants.ConsoleMenuHeight, false);
                    break;
            }
        }

        private static void UpdateConsole(int width, int height, bool isStatic)
        {
            Console.Clear();
            Console.SetWindowSize(width, height);
            if (isStatic)
            {
                Console.SetBufferSize(width, height);
            }
            else
            {
                Console.SetBufferSize(width, Int16.MaxValue - 1);
            }
        }
    }
}