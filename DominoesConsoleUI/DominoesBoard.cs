using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;
using Dominoes;

namespace DominoesConsoleUI
{
    class DominoesBoard
    {

        Bot P1;
        Bot P2, P3, P4;
        List<string> TimesP = new List<string>();
        LinkedList<Tuple<int, int>> DominoGame;

        public DominoesBoard()
        {
            DominoGame = new LinkedList<Tuple<int, int>>();

            List<Tuple<int, int>> L = new List<Tuple<int, int>>();
            List<Tuple<int, int>> Q = new List<Tuple<int, int>>();

            for (int i = 0; i < 7; i++)
            {
                for (int j = i; j < 7; j++)
                {
                    Q.Add(new Tuple<int,int>(i,j));
                }
            }
            Shuffle(Q);
            for (int i = 0; i < 7; i++)
            {
                L.Add(Q[i]);
            }
            P1 = new Bot(L.ToArray().ToList(),1,0);
            L.Clear();
            //L.Add(new Tuple<int,int>(
            for (int i = 7; i < 14; i++)
            {
                L.Add(Q[i]);
            }
            P2 = new Bot(L.ToArray().ToList(), -1,1);
            L.Clear();
            for (int i = 14; i < 21; i++)
            {
                L.Add(Q[i]);
            }
            P3 = new Bot(L.ToArray().ToList(), -1,2);
            L.Clear();
            for (int i = 21; i < 28; i++)
            {
                L.Add(Q[i]);
            }
            P4 = new Bot(L.ToArray().ToList(), -1,3);
            L.Clear();
            Q.Clear();
            GC.Collect();
        }

        private Random _random = new Random();
        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array to shuffle.</param>
        public void Shuffle(List<Tuple<int,int>> array)
        {

            var random = _random;
            for (int i = array.Count; i > 1; i--)
            {
                // Pick random element to swap.
                int j = random.Next(i); // 0 <= j <= i-1
                // Swap.
                Tuple<int, int> tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
        }

        public void PrintGame()
        {
            LinkedListNode<Tuple<int, int>> Nodes = DominoGame.First;

            Console.WriteLine("\n\n========================================================================");
            Console.Write(" ");
            for (int i = 0; i < DominoGame.Count; i++)
            {
                Console.Write(Nodes.Value.Item1.ToString() + '|' + Nodes.Value.Item2.ToString() + " ");
                Nodes = Nodes.Next;
            }
            Console.WriteLine();
            Console.WriteLine("========================================================================\n\n");
           //Console.WriteLine(P1.PrintHand());
           //Console.WriteLine(P2.PrintHand());
           //Console.WriteLine(P3.PrintHand());
           //Console.WriteLine(P4.PrintHand());
        }
      
        Tuple<int, int> PlayerPlaying(Player P)
        {
            bool canPlay = P.CanPlay(DominoGame);
            if (!canPlay)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine(P.PrintHand());
                Console.WriteLine("-----------------------------\n No vas...");
                Thread.Sleep(1500);
                return null;
            }

            Tuple<int, int> res = null;
                Console.WriteLine("-----------------------------");
                Console.WriteLine(P.PrintHand());
            while (res == null)
            {
                Console.WriteLine("-----------------------------\n Digita el lado y el id de la ficha.");
                char side = Convert.ToChar(Console.ReadLine());
                int id = Convert.ToInt32(Console.ReadLine());
                res =P.MakeAMove(DominoGame, id, (char)side);
            }
            return res;//P.MakeAMove(DominoGame, id, char.Parse(side.ToString()));
        }

        Tuple<int, int> PlayerPlaying(Bot B)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine(B.PrintHand());
            Console.WriteLine("-----------------------------\n");
            Tuple<int, int> res;
            // @TODO Eliminar el dummyMove
            if (B.idPlayer == 0)
                res = B.DummyMove(DominoGame);
           else
                res=B.MakeAMove(DominoGame);

            if (res == null) Console.WriteLine("El jugador " + (B.idPlayer+1).ToString() + " No va waching...\n");
            return res;
        }

        public int DoblesCant( Player Pl)
        {
            int dbs = 0;
            for (int i = 0; i < P1.Hand.Count; i++)
                if (Pl.Hand[i].Item1 == Pl.Hand[i].Item2) dbs++;
            return dbs;
        }

        public void PrintHands()
        {
            Console.WriteLine("P1:  " + P1.PrintHand());
            Console.WriteLine("P2:  " + P2.PrintHand());
            Console.WriteLine("P3:  " + P3.PrintHand());
            Console.WriteLine("P4:  " + P4.PrintHand());
        }

        public void PlayDomino() 
        {
            //if (DoblesCant(P1) >= 3 || DoblesCant(P2) >= 3 || DoblesCant(P3) >= 3 || DoblesCant(P4) >= 3)
            //{
            //    Console.WriteLine("Hay un jugador con 3 dobles o mas");
            //  //  return;
            //}
            int EndGame = 0;
            Stopwatch clock = Stopwatch.StartNew();
            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    i = 0;
                }
                if (EndGame == 4 || P1.Hand.Count == 0 || P2.Hand.Count == 0 || P3.Hand.Count == 0 || P4.Hand.Count == 0)
                {
                    Console.WriteLine("||||||||||| Juego terminado|||||||||||");
                    PrintGame();
                    PrintHands();
                    for (int q = 0; q < TimesP.Count;q++) Console.WriteLine(TimesP[q]);
                    Console.WriteLine("Con un aproximado de:" + " P2" + P2.dpHashTable.Count + " P3 " + P3.dpHashTable.Count + " P4 " + P4.dpHashTable.Count+" Nodos");
                    Console.Read();
                    return;
                }
                Console.Clear();
                string t = clock.Elapsed.ToString();
                Console.WriteLine(t);
                TimesP.Add("P"+(i+1)+": "+t);
                Console.WriteLine("||||||||||| Esta jugando el jugador " + (i + 1).ToString() + " |||||||||||");
                PrintGame();

                PrintHands();
                Tuple<int, int> res=null;
                Console.Read();
                if (i == 0)
                {
                    res= PlayerPlaying(P1);
                    if ( res== null) EndGame++;
                    else EndGame = 0;
                }
                else if (i == 1)
                {
                    res = PlayerPlaying(P2);
                    if (res == null) EndGame++;
                    else EndGame = 0;
                }
                else if (i == 2)
                {
                    res = PlayerPlaying(P3);
                    if (res == null) EndGame++;
                    else EndGame = 0;
                }
                else//if (i==3)
                {
                    res = PlayerPlaying(P4);
                    if (res == null) EndGame++;
                    else EndGame = 0;
                }
                PlayersLearn(i, res);
                //Console.Read();
   //             Thread.Sleep(1000);
            }
        }

        public void PlayersLearn(int idPlayer,Tuple<int, int> Moved)
        {
            if (idPlayer != 0)
                P1.Learn(DominoGame, 0, Moved);
            if (idPlayer != 1)
                P2.Learn(DominoGame, 1, Moved);
            if (idPlayer != 2)
                P3.Learn(DominoGame, 2, Moved);
            if (idPlayer != 3)
                P4.Learn(DominoGame, 3, Moved);
        }

    }
}
