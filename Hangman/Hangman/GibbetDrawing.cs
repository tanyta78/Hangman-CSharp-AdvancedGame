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
            mistakes = 0;

            Console.SetCursorPosition(startY, startX + 1);
        }

        //Increments mistakes to visualize next part
        public void Update()
        {
            mistakes++;

            //methodInfo is null
            MethodInfo methodInfo = this.GetType().GetMethod("Part" + mistakes, BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(this,null);
        }

        private static void Part1()
        {
            Console.SetCursorPosition(startY, startX - 1);

            Console.WriteLine(new string('x', 14));
            Console.WriteLine(new string('x', 14));
        }

        private static void Part2()
        {
            Part1();
            Console.SetCursorPosition(startY, startX - 14);

            for (int i = 0; i < 13; i++)
            {
                Console.WriteLine(new string(' ', 6) + 'o');
            }
        }

        private static void Part3()
        {
            Part2();
            Console.SetCursorPosition(startY + 7, startX - 14);

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
                Console.SetCursorPosition(22, startX - 14 + i + 1);
                Console.WriteLine("|");
            }
        }

        private static void Part5()
        {
            Part4();
            Console.SetCursorPosition(21, startX - 14 + 3);

            Console.WriteLine("o o");
            Console.SetCursorPosition(20, startX - 14 + 4);
            Console.WriteLine("o. .o");
            Console.SetCursorPosition(22, startX - 14 + 5);
            Console.WriteLine("o");
        }

        private static void Part6()
        {
            Part5();

            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(22, startX - 14 + 6 + i);
                if (i == 1)
                {
                    Console.SetCursorPosition(21, startX - 14 + 6 + i);
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

            Console.SetCursorPosition(20, startX - 14 + 6);
            Console.Write(@"\ + /");
        }

        private static void Part8()
        {
            Part7();

            int spaces = 1;
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(21 - i, startX - 14 + 9 + i);
                Console.WriteLine($@"/{new string(' ', spaces)}\");
                spaces += 2;
            }
        }
    }
}