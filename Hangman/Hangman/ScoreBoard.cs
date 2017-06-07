namespace Hangman
{
    using System;

    public class ScoreBoard
   {
       public const int ScoresCount = 10;

        private string [] bestPlayerNames = new string[ScoresCount];

        private int[] mistakes = new int[ScoresCount];


        public ScoreBoard()
        {
            for (int i = 0; i < ScoresCount; i++)
            {
                bestPlayerNames[i]=String.Empty;
                mistakes[i] = int.MaxValue;
            }
        }

       public void PrintScoreBoard()
       {
           Console.WriteLine("Best hangmam players:");

           for (int i = 0; i < ScoresCount; i++)
           {
               if (bestPlayerNames[i]!=String.Empty)
               {
                   Console.WriteLine(String.Format("{0}. {1,15}===> {2,3} mistakes",i+1,bestPlayerNames[i],mistakes[i]));
               }
           }
       }

       public void AddNewScore(string playerName, int playerMistakes)
       {
            //find correct place index for player's result
           int indexInScoreBoard = -1;
           for (int i = 0; i < mistakes.Length; i++)
           {
               if (playerMistakes < mistakes[i])
               {
                   indexInScoreBoard = i;
               }
           }

           //change scoreList
           if (indexInScoreBoard!=-1)
           {
               for (int index = ScoresCount-1; index >indexInScoreBoard; index--)
               {
                   bestPlayerNames[index] = bestPlayerNames[index - 1];
                   mistakes[index] = mistakes[index - 1];
               }
           }

           bestPlayerNames[indexInScoreBoard] = playerName;
           mistakes[indexInScoreBoard] = playerMistakes;

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
