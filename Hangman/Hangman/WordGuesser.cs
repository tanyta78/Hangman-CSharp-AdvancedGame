using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class WordGuesser
    {
        public char[] Content { get; set; }

        public WordGuesser(int length)
        {
            Content = new char[length];
        }

        public char this[int subscript]
        {
            get => Content[subscript];
            set => Content[subscript] = value;
        }

    }
}
