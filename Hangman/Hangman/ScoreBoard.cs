namespace Hangman
{
    using System;

    public class ScoreBoard
    {
        public const int ScoresCount = 10;

        private string[] bestPlayerNames = new string[ScoresCount];

        private int[] mistakes = new int[ScoresCount];


        public ScoreBoard()
        {
            for (int i = 0; i < bestPlayerNames.Length; i++)
            {
                bestPlayerNames[i] = null;
                mistakes[i] = int.MaxValue;
            }
        }

        public void PrintScoreBoard()
        {
            Console.WriteLine("Best hangmam players:");
            int i = 0;
            while (bestPlayerNames[i] != null)
            {
                Console.WriteLine(String.Format("{0}. {1,15}===> {2,3} mistakes", i + 1, bestPlayerNames[i], mistakes[i]));
                i++;
                if (i >= bestPlayerNames.Length)
                {
                    break;
                }

            }
        }

        public void AddNewScore(string playerName, int playerMistakes)
        {
            //find correct place index for player's result
          

          int indexInScoreBoard = FindCorrectIndex(playerMistakes);

            //change scoreList

            if (indexInScoreBoard<bestPlayerNames.Length)
            {
                for (int index = bestPlayerNames.Length - 1; index > indexInScoreBoard; index--)
                {
                    bestPlayerNames[index] = bestPlayerNames[index - 1];
                    mistakes[index] = mistakes[index - 1];
                }


                bestPlayerNames[indexInScoreBoard] = playerName;
                mistakes[indexInScoreBoard] = playerMistakes;
            }
            

        }

        private int FindCorrectIndex(int playerMistakes)
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
