using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class WordGuesser
    {
        private char[] Content { get; set; }
        private string Word { get; set; }

        public WordGuesser(string word)
        {
            Word = word;
            Content = new char[word.Length];

            //sets default value
            for (int i = 0; i < word.Length; i++)
            {
                Content[i] = '_';
            }

            ShowMarginalChars();
        }

        //makes 1st and last letters show
        private void ShowMarginalChars()
        {
            char left = Word[0];
            char right = Word[Word.Length - 1];

            for (int i = 0; i < Word.Length; i++)
            {
                if (Word[i] == left || Word[i] == right)
                {
                    Content[i] = Word[i];
                }   
            }
        }
        public char this[int index]
        {
            get
            {
                return Content[index];
            }
            set
            {
                Content[index] = value;
            }
        }

        public override string ToString()
        {
            return string.Join("",Content);
        }

        public void Update(char c)
        {
            if (!Word.Contains(c))
            {
                return;
            }
            for (int i = 0; i < Content.Length; i++)
            {
                if (Word[i] == c)
                {
                    Content[i] = c;
                }  
            }
        }
    }
}
