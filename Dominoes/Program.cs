using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    class Program
    {
        static void Main(string[] args)
        {
            Tuple<int, int> domino = new Tuple<int, int>(1, 2); ;
            List<Tuple<int, int>> l = new List<Tuple<int, int>>();
            l.Add(domino);

            DominoBoard Board = new DominoBoard();

            Board.PrintGame();
            Board.PlayDomino();
            int x;
        }
        //private static T[] Shuffle<T>(T[] OriginalArray)
        //{
        //    var matrix = new SortedList();
        //    var r = new Random();

        //    for (var x = 0; x <= OriginalArray.GetUpperBound(0); x++)
        //    {
        //        var i = r.Next();

        //        while (matrix.ContainsKey(i)) { i = r.Next(); }

        //        matrix.Add(i, OriginalArray[x]);
        //    }

        //    var OutputArray = new T[OriginalArray.Length];

        //    matrix.Values.CopyTo(OutputArray, 0);

        //    return OutputArray;
        //}

    }
}
