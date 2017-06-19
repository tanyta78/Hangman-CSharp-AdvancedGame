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
                    UpdateConsole(Constants.ConsoleMenuWidth, Constants.ConsoleMenuHeigth);
                    break;
                case GameMode.Game:
                    UpdateConsole(Constants.ConsoleGameWidth, Constants.ConsoleGameHeigth);
                    break;
                case GameMode.Dictionary:
                    UpdateConsole(Constants.ConsoleDictionaryWidth, Constants.ConsoleDictionaryHeigth);
                    break;
            }
        }

        private static void UpdateConsole(int width, int height)
        {
            Console.Clear();
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }
    }
}