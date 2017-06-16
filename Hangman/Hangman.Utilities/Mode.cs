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
        Game
    }

    public class Mode
    {
        public static void Set(GameMode gm)
        {
            switch (gm)
            {
                case GameMode.Menu:
                    UpdateConsole(Constants.ConsoleMenuWidth, Constants.ConsoleMenuHeight);
                    break;
                case GameMode.Game:
                    UpdateConsole(Constants.ConsoleGameWidth, Constants.ConsoleGameHeight);
                    break;
            }
        }

        private static void UpdateConsole(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }
    }
}