using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    public class GibbetDrawing
    {
        //bottom left point coordinates
        private static int startX { get; set; }

        private static int startY { get; set; }

        private int mistakes { get; set; }

        public GibbetDrawing(int x, int bottomY)
        {
            startX = x;
            startY = bottomY;
            Location = new[] {startX, startY};

            mistakes = 0;
            Console.SetCursorPosition(startX, startY);
        }

        /// <summary>
        /// 0 element is startX, 1st element is startY
        /// </summary>
        public int[] Location { get; }

        //Increments mistakes to visualize next part
        public void Update()
        {
            mistakes++;
        }

        public void Print()
        {
            if (mistakes == 0)
            {
                return;
            }
            //methodInfo is null
            MethodInfo methodInfo = GetType()
                .GetMethod("Part" + mistakes, BindingFlags.NonPublic | BindingFlags.Static);
            methodInfo.Invoke(this, null);
        }

        private static void Part1()
        {
            Console.SetCursorPosition(startX, startY - 1);

            Console.WriteLine(new string('x', 14));
            Console.WriteLine(new string('x', 14));
        }

        private static void Part2()
        {
            Part1();
            Console.SetCursorPosition(startX, startY - 14);

            for (int i = 0; i < 13; i++)
            {
                Console.WriteLine(new string(' ', 6) + 'o');
            }
        }

        private static void Part3()
        {
            Part2();
            Console.SetCursorPosition(startX + 7, startY - 14);

            for (int i = 0; i < 9; i++)
            {
                Console.Write(" " + "o");
            }
        }

        private static void Part4()
        {
            Part3();
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(22, startY - 14 + i + 1);
                Console.WriteLine("|");
            }
        }

        private static void Part5()
        {
            Part4();
            Console.SetCursorPosition(21, startY - 14 + 3);

            Console.WriteLine("o o");
            Console.SetCursorPosition(20, startY - 14 + 4);
            Console.WriteLine("o. .o");
            Console.SetCursorPosition(22, startY - 14 + 5);
            Console.WriteLine("o");
        }

        private static void Part6()
        {
            Part5();

            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(22, startY - 14 + 6 + i);
                if (i == 1)
                {
                    Console.SetCursorPosition(21, startY - 14 + 6 + i);
                    Console.WriteLine("+++");
                }
                else
                {
                    Console.WriteLine("+");
                }
            }
        }

        private static void Part7()
        {
            Part6();

            Console.SetCursorPosition(20, startY - 14 + 6);
            Console.Write(@"\ + /");
        }

        private static void Part8()
        {
            Part7();

            int spaces = 1;
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(21 - i, startY - 14 + 9 + i);
                Console.WriteLine($@"/{new string(' ', spaces)}\");
                spaces += 2;
            }
        }
    }
}