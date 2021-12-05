using System;
using Maze;

namespace Labirynth
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Grid m = new Grid(5, 5);
            m.Generate();
            m.Print();
            m.PrintNums();
        }
    }
}
