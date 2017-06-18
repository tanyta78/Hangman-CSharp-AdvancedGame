using System.Diagnostics;
using Database;
using Hangman.Utilities;
using System.Drawing;
using System.Linq;


namespace Hangman
{
    using System;

    public class ScoreBoard
    {
        public const int ScoresCount = 10;
        private static string[] bestPlayerNames = new string[ScoresCount];
        private static int[] mistakes = new int[ScoresCount];
        private static int gameLevel = 1;
        private static int mistakesCount = 0;
        public static Stopwatch sw = new Stopwatch();
        public static TimeSpan remainingTime = TimeSpan.FromMinutes(2);
        private static HangmanContext dbContext = new HangmanContext();

        public ScoreBoard(int gameLevel,int mistakesCount)
        {
            for (int i = 0; i < bestPlayerNames.Length; i++)
            {
                bestPlayerNames[i] = null;
                mistakes[i] = int.MaxValue;
            }
            ScoreBoard.gameLevel = gameLevel;
            ScoreBoard.mistakesCount = mistakesCount;
            sw.Reset();
            sw.Start();
        }

        public static double GetScore()
        {
            sw.Stop();
            var gameTime = (remainingTime - sw.Elapsed).Seconds;
            double levelPercent = 1.1;
            if (ScoreBoard.gameLevel==1)
            {
                levelPercent = Constants.levelOne;
            }
            else if (ScoreBoard.gameLevel == 2)
            {
                levelPercent = Constants.levelTwo;
            }
            else if (ScoreBoard.gameLevel == 3)
            {
                levelPercent = Constants.levelThree;
            }
            else if (ScoreBoard.gameLevel == 4)
            {
                levelPercent = Constants.levelFour;
            }
            else if (ScoreBoard.gameLevel == 5)
            {
                levelPercent = Constants.levelFive;
            }
            var score = gameTime*levelPercent*(mistakesCount*Constants.mistakePercent)*100;
            return score;
        }
        public static void PrintScoreBoard()
        {
            var topPlayers = dbContext.Users.OrderByDescending(x => x.Score).ToArray();
            Console.Clear();
            Colorful.Console.WriteLine(Message.BestPlayersLabel.PadLeft(28),Color.Crimson);
            Colorful.Console.WriteLine("---------------------------------",Color.GreenYellow);
            for (int j = 0; j < 5; j++)
            {
                Colorful.Console.WriteLine($"  {j + 1}.{topPlayers[j].Name.PadRight(15)}{topPlayers[j].Score:F0} points", Color.Crimson);
                Colorful.Console.WriteLine("---------------------------------", Color.GreenYellow);
            }
            Colorful.Console.WriteLine("Press any key to go back",Color.GreenYellow);
            Colorful.Console.ReadKey();
            Menu.Initialize();
        }

        public static void AddNewScore(Users player, int playerMistakes)
        {
            int indexInScoreBoard = FindCorrectIndex(playerMistakes);

            if (indexInScoreBoard < bestPlayerNames.Length)
            {
                for (int index = bestPlayerNames.Length - 1; index > indexInScoreBoard; index--)
                {
                    bestPlayerNames[index] = bestPlayerNames[index - 1];
                    mistakes[index] = mistakes[index - 1];
                }

                bestPlayerNames[indexInScoreBoard] = player.Name;
                mistakes[indexInScoreBoard] = playerMistakes;
                Colorful.Console.WriteLine("You enter our highscore list! Congratulations!", Color.Aqua);
            }
            else
            {
                Colorful.Console.WriteLine("You score is not enougth to enter our highscore list! Maybe next time!", Color.DarkOrange);
            }
        }

        private static int FindCorrectIndex(int playerMistakes)
        {
            for (int i = 0; i < mistakes.Length; i++)
            {
                if (playerMistakes < mistakes[i])
                {
                    return i;
                }
            }
            return bestPlayerNames.Length;
        }

        public int GetLastPositionMistakes()
        {
            int worstTopScore = int.MaxValue;
            if (bestPlayerNames[bestPlayerNames.Length - 1] != null)
            {
                worstTopScore = mistakes[bestPlayerNames.Length - 1];
            }

            return worstTopScore;
        }
    }
}