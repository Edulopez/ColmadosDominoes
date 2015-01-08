using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    class Bot : Player
    {
        const int DUMMYVAL = 1000000;
        int RecursiveCalls = 11;
        public Hashtable dpHashTable;

        public Bot(List<Tuple<int, int>> StartHand, int TypeOfPlayer, int _idPlayer): base(StartHand, TypeOfPlayer, _idPlayer)
        {
            dpHashTable = new Hashtable();
            AvailableDominoes.Sort();
            Hand.Sort();
        }

        // retorna los puntos del juego.
        private int Eval(LinkedList<Tuple<int, int>> ActualGame)
        {
            int pointsInHand = 1;
            for (int i = 0; i < Hand.Count; ++i)
                pointsInHand += Hand[i].Item1 + Hand[i].Item2;

            for (int i = 0; i < EnemysHandCount.Length; ++i)
                if (i != idPlayer)
                {
                    if(EnemysHandCount[i]==0) return DUMMYVAL;
                } 

            return pointsInHand;
        }

        public Tuple<int, int> MakeAMove(LinkedList<Tuple<int, int>> ActualGame)
        {
            Tuple<int, int> UsingDomino=null;
            if (!CanPlay(ActualGame)) return null;
            else
            {
                int stop = Hand.Count;
                int value=DUMMYVAL;
                int TempValue=DUMMYVAL;
                int BestDomino=0;
                int LOR=0;
                for (int i = 0; i < stop; i++)
                {
                    UsingDomino = Hand[0];
                    Hand.RemoveAt(0);
                    if(CanIMove(ActualGame,UsingDomino,1)){
                        ActualGame.AddLast(SetPositionOfDomino(ActualGame,UsingDomino,1));
                        TempValue = GetBestMove(ActualGame, 0, RecursiveCalls, idPlayer + 1);
                        ActualGame.RemoveLast();
                        Console.WriteLine("En la derecha La ficha " + UsingDomino.Item1 + "|" + UsingDomino.Item2 + " Tiene  " + TempValue);
                    }
                    if ((value) > (TempValue))
                    {
                        value = TempValue;
                        BestDomino = i;
                        LOR = 1;
                    }
                     if(CanIMove(ActualGame,UsingDomino,-1)){
                        ActualGame.AddFirst(SetPositionOfDomino(ActualGame,UsingDomino,-1));
                        TempValue = GetBestMove(ActualGame, 0, RecursiveCalls, idPlayer + 1);
                        ActualGame.RemoveFirst();
                        Console.WriteLine("En la izquierda La ficha " + UsingDomino.Item1 + "|" + UsingDomino.Item2 + " Tiene  " + TempValue);
                    
                    }
                    if ((value) > (TempValue))
                    {
                        value = TempValue;
                        BestDomino = i;
                        LOR = -1;
                    }
                   Hand.Add(UsingDomino);
                }
                char side;
                if (LOR < 0) side = 'L';
                else side = 'R';
               UsingDomino= MakeAMove(ActualGame, BestDomino + 1, side);
               Console.Read();
            }

            EnemysHandCount[idPlayer]--;
            return UsingDomino;
        }

        
        private string GetStringIndex(LinkedList<Tuple<int, int>> ActualGame)
        {
            string idx;
            idx = PrintHand(true);

            string map = "";
            LinkedListNode<Tuple<int, int>> Node = ActualGame.First;
            for (int i = 0; i < ActualGame.Count; i++)
            {
                if (Node.Value.Item1 == Node.Value.Item2)
                {
                    Node = Node.Next;
                    continue;
                }

                map += Node.Value.Item1.ToString() + Node.Value.Item1.ToString();
                Node = Node.Next;
            }
            return map + idx;
        }
        private int GetBestMove(LinkedList<Tuple<int, int>> ActualGame, int depth, int finalDepth, int IsMe)
        {
            string idx = GetStringIndex(ActualGame);
            if (dpHashTable.Contains(idx) == true) return (int)dpHashTable[idx];

            int value = Eval(ActualGame);
            if (finalDepth == 0 || value == 1)
            { 
                dpHashTable.Add(idx, value); 
                return value; 
            }

            if (DUMMYVAL == value) {
                if (dpHashTable.Contains(idx) == false) dpHashTable.Add(idx, value);
                return value; }

            Tuple<int,int>UsingDomino;
            int tempValue = DUMMYVAL;
            IsMe %= 4;
            int stop;
            if (IsMe != idPlayer) stop = AvailableDominoes.Count;
            else stop = Hand.Count;
            for (int i = 0; i < stop; i++)
            {
                if (IsMe == idPlayer)
                {
                    UsingDomino = Hand[0];
                    Hand.RemoveAt(0);
                }
                else
                {
                    UsingDomino = AvailableDominoes[0];
                    AvailableDominoes.RemoveAt(0);
                }
                if (CanIMove(ActualGame,UsingDomino,1))
                {
                    EnemysHandCount[IsMe]--;
                    ActualGame.AddLast(SetPositionOfDomino(ActualGame,UsingDomino,1));
                    tempValue = GetBestMove(ActualGame, depth + 1, finalDepth - 1,IsMe+1);
                    ActualGame.RemoveLast();

                    EnemysHandCount[IsMe]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }
                else if (CanIMove(ActualGame, UsingDomino, -1))
                {
                    EnemysHandCount[IsMe]--;
                    ActualGame.AddFirst(SetPositionOfDomino(ActualGame, UsingDomino, -1));
                    tempValue = GetBestMove(ActualGame, depth + 1, finalDepth - 1, IsMe + 1);
                    ActualGame.RemoveFirst();

                    EnemysHandCount[IsMe]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }

                if (IsMe == idPlayer)
                    Hand.Add(UsingDomino);
                else
                    AvailableDominoes.Add(UsingDomino);
            }
            if(!dpHashTable.Contains(idx)) dpHashTable.Add(idx, value);
            return value;
        }
        
        public Tuple<int, int> DummyMove(LinkedList<Tuple<int, int>> ActualGame)
        {
            Tuple<int, int> res =null;
            if (ActualGame.Count == 0)
            {
                Random rnd = new Random();
                int id= rnd.Next(0, Hand.Count);
                res = Hand[id];
                Hand.RemoveAt(id);
                ActualGame.AddFirst(res);
                return res;
            }
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Item1 == ActualGame.First.Value.Item1)
                {
                    res= new Tuple<int, int>(Hand[i].Item2, Hand[i].Item1);
                    ActualGame.AddFirst(res);
                    Hand.RemoveAt(i);
                }
                else if (Hand[i].Item2 == ActualGame.First.Value.Item1)
                {
                    res = new Tuple<int, int>(Hand[i].Item1, Hand[i].Item2);
                    ActualGame.AddFirst(res);
                    Hand.RemoveAt(i);
                }
                else if (Hand[i].Item1 == ActualGame.Last.Value.Item2)
                {
                    res = new Tuple<int, int>(Hand[i].Item1, Hand[i].Item2);
                    ActualGame.AddLast(res);
                    Hand.RemoveAt(i);
                }
                else if (Hand[i].Item2 == ActualGame.Last.Value.Item2)
                {
                    res = new Tuple<int, int>(Hand[i].Item2, Hand[i].Item1);
                    ActualGame.AddLast(res);
                    Hand.RemoveAt(i);
                }
                if (res != null) break;
            }
            return res;
        }

        private Tuple<int, int> SetPositionOfDomino(LinkedList<Tuple<int, int>> ActualGame, Tuple<int, int> UsingDomino, int side)
        {
            if (side == 1)
            {
                if (ActualGame.Last.Value.Item2 == UsingDomino.Item1)
                {
                    return UsingDomino;
                }
                if (ActualGame.Last.Value.Item2 == UsingDomino.Item2)
                {
                    return new Tuple<int, int>(UsingDomino.Item2, UsingDomino.Item1);
                }
            }
            else
            {
                if (ActualGame.First.Value.Item1 == UsingDomino.Item1)
                {
                    return new Tuple<int, int>(UsingDomino.Item2, UsingDomino.Item1);
                }
                if (ActualGame.First.Value.Item1 == UsingDomino.Item2)
                    return UsingDomino;
            }
            return null;
        }

        private bool CanIMove(LinkedList<Tuple<int, int>> ActualGame, Tuple<int, int> UsingDomino, int side)
        {
            //if (ActualGame.Count == 0)
            {
               // DummyMove(ActualGame);
             //   return true;
            }
            if (side == 1)
            {
                if (ActualGame.Last.Value.Item2 == UsingDomino.Item1 || ActualGame.Last.Value.Item2 == UsingDomino.Item2)
                    return true;
            }
            else
            {
                if (ActualGame.First.Value.Item1 == UsingDomino.Item1 || ActualGame.First.Value.Item1 == UsingDomino.Item2)
                    return true;
            }
            return false;
        }
    }
}
