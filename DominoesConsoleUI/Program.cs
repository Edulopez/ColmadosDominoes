using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominoes;
namespace DominoesConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Tuple<int, int> domino = new Tuple<int, int>(1, 2); ;
            List<Tuple<int, int>> l = new List<Tuple<int, int>>();
            l.Add(domino);

            DominoesConsoleUI.DominoesBoard Board = new DominoesBoard();

            Board.PrintGame();
            Board.PlayDomino();
            int x;
        }
    }
}
