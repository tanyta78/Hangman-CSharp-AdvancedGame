using Database;
using Hangman.Utilities;
using System.Drawing;

namespace Hangman
{
    using System;

    public class ScoreBoard
    {
        public const int ScoresCount = 10;
        private static string[] bestPlayerNames = new string[ScoresCount];

        private static int[] mistakes = new int[ScoresCount];

        public ScoreBoard()
        {
            for (int i = 0; i < bestPlayerNames.Length; i++)
            {
                bestPlayerNames[i] = null;
                mistakes[i] = int.MaxValue;
            }
        }

        public static void PrintScoreBoard()
        {
            Console.WriteLine(Message.BestPlayersLabel);
            int i = 0;
            while (bestPlayerNames[i] != null)
            {
                Colorful.Console.WriteLine($"{i + 1}. {bestPlayerNames[i],15} ===> {mistakes[i],3} mistakes", Color.Crimson);
                i++;
                if (i >= bestPlayerNames.Length)
                {
                    break;
                }
            }
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