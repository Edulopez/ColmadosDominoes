using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{
    public class Bot : Player
    {
        const int DUMMYVAL = 1000000;
        readonly int RecursiveCalls;
        public Hashtable dpHashTable;

        public Bot(List<Dominoes.DominoeTile> StartHand, int TypeOfPlayer, int _idPlayer , int Level = 11): base(StartHand, TypeOfPlayer, _idPlayer)
        {
            dpHashTable = new Hashtable();
            RecursiveCalls = Level;
            //AvailableDominoes.Sort();
            //Hand.Sort();
        }

        // retorna los puntos del juego.
        private int Eval(LinkedList<Dominoes.DominoeTile> ActualGame)
        {
            int pointsInHand = 1;
            for (int i = 0; i < Hand.Count; ++i)
                pointsInHand += Hand[i].TopNumber + Hand[i].BottomNumber;

            for (int i = 0; i < EnemysHandCount.Length; ++i)
                if (i != idPlayer)
                {
                    if(EnemysHandCount[i]==0) return DUMMYVAL;
                }

            return pointsInHand;
        }

        private string GetStringIndex(LinkedList<Dominoes.DominoeTile> ActualGame)
        {
            string idx;
            idx = PrintHand(false,"");

            string map = "";
            LinkedListNode<Dominoes.DominoeTile> Node = ActualGame.First;
            for (int i = 0; i < ActualGame.Count; i++)
            {
                if (Node.Value.TopNumber == Node.Value.BottomNumber)
                {
                    Node = Node.Next;
                    continue;
                }
                map += Node.Value.GetDominoString(false);
                Node = Node.Next;
            }
            return map + idx;
        }

        public DominoeTile MakeAMove(LinkedList<DominoeTile> ActualGame)
        {
            DominoeTile DominoInUse = null;
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
                    DominoInUse = Hand[0];
                    Hand.RemoveAt(0);
                    if(CanIMove(ActualGame,DominoInUse,DominoBoardSide.RigthSide)){
                        ActualGame.AddLast(SetPositionOfDomino(ActualGame, DominoInUse, DominoBoardSide.RigthSide));
                        TempValue = GetBestMove(ActualGame, 0, RecursiveCalls, idPlayer + 1);
                        ActualGame.RemoveLast();
                        Console.WriteLine("En la derecha La ficha " + DominoInUse.GetDominoString(true)+ " Tiene  " + TempValue);
                    }
                    if ((value) > (TempValue))
                    {
                        value = TempValue;
                        BestDomino = i;
                        LOR = 1;
                    }
                    if (CanIMove(ActualGame, DominoInUse, DominoBoardSide.LeftSide))
                    {
                        ActualGame.AddFirst(SetPositionOfDomino(ActualGame, DominoInUse, DominoBoardSide.LeftSide));
                        TempValue = GetBestMove(ActualGame, 0, RecursiveCalls, idPlayer + 1);
                        ActualGame.RemoveFirst();
                        Console.WriteLine("En la izquierda La ficha " + DominoInUse.GetDominoString(true) + " Tiene  " + TempValue);
                    
                    }
                    if ((value) > (TempValue))
                    {
                        value = TempValue;
                        BestDomino = i;
                        LOR = -1;
                    }
                   Hand.Add(DominoInUse);
                }

                char side;
                if (LOR < 0) side = 'L';
                else side = 'R';
               DominoInUse= MakeAMove(ActualGame, BestDomino + 1, side);
               Console.Read();
            }

            EnemysHandCount[idPlayer]--;
            return DominoInUse;
        }



        private int GetBestMove(LinkedList<DominoeTile> ActualGame, int depth, int finalDepth, int IsMe)
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

            DominoeTile DominoInUse;
            int tempValue = DUMMYVAL;
            IsMe %= 4;

            int stop;
            if (IsMe != idPlayer) stop = AvailableDominoes.Count;
            else stop = Hand.Count;

            for (int i = 0; i < stop; i++)
            {
                if (IsMe == idPlayer)
                {
                    DominoInUse = Hand[0];
                    Hand.RemoveAt(0);
                }
                else
                {
                    DominoInUse = AvailableDominoes[0];
                    AvailableDominoes.RemoveAt(0);
                }

                if (CanIMove(ActualGame,DominoInUse,DominoBoardSide.RigthSide))
                {
                    EnemysHandCount[IsMe]--;
                    ActualGame.AddLast(SetPositionOfDomino(ActualGame, DominoInUse, DominoBoardSide.RigthSide));
                    tempValue = GetBestMove(ActualGame, depth + 1, finalDepth - 1,IsMe+1);
                    ActualGame.RemoveLast();

                    EnemysHandCount[IsMe]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }

                else if (CanIMove(ActualGame, DominoInUse, DominoBoardSide.LeftSide))
                {
                    EnemysHandCount[IsMe]--;
                    ActualGame.AddFirst(SetPositionOfDomino(ActualGame, DominoInUse, DominoBoardSide.LeftSide));
                    tempValue = GetBestMove(ActualGame, depth + 1, finalDepth - 1, IsMe + 1);
                    ActualGame.RemoveFirst();

                    EnemysHandCount[IsMe]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }

                if (IsMe == idPlayer)
                    Hand.Add(DominoInUse);
                else
                    AvailableDominoes.Add(DominoInUse);
            }

            if(!dpHashTable.Contains(idx)) dpHashTable.Add(idx, value);

            return value;
        }

       

        private DominoeTile SetPositionOfDomino(LinkedList<DominoeTile> ActualGame, DominoeTile DominoInUse, DominoBoardSide side)
        {
            if (ActualGame.Count == 0) return DominoInUse;

            if (side == DominoBoardSide.RigthSide)
            {
                return DominoInUse.GetDominoInPosition(ActualGame.Last.Value, side);
            }
            else //DominoBoardSide.LeftSide
            {
                return DominoInUse.GetDominoInPosition(ActualGame.First.Value, side);
            }
        }

        
        private bool CanIMove(LinkedList<DominoeTile> ActualGame, DominoeTile DominoInUse, DominoBoardSide side)
        {
            if (ActualGame.Count == 0) return true;

            if (side == DominoBoardSide.RigthSide)
            {
                if (ActualGame.Last.Value.BottomNumber == DominoInUse.TopNumber || ActualGame.Last.Value.BottomNumber == DominoInUse.BottomNumber)
                    return true;
            }
            else
            {
                if (ActualGame.First.Value.TopNumber == DominoInUse.TopNumber || ActualGame.First.Value.TopNumber == DominoInUse.BottomNumber)
                    return true;
            }
            return false;
        }

        public DominoeTile DummyMove(LinkedList<DominoeTile> ActualGame)
        {
            DominoeTile res = null;
            if (ActualGame.Count == 0)
            {
                Random rnd = new Random();
                int id = rnd.Next(0, Hand.Count);
                res = Hand[id];
                Hand.RemoveAt(id);
                ActualGame.AddFirst(res);
                return res;
            }
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].TopNumber == ActualGame.First.Value.TopNumber)
                {
                    res = Hand[i].SwipedDomino();
                    ActualGame.AddFirst(res);
                }
                else if (Hand[i].BottomNumber == ActualGame.First.Value.TopNumber)
                {
                    res = Hand[i];
                    ActualGame.AddFirst(res);
                }
                else if (Hand[i].TopNumber == ActualGame.Last.Value.BottomNumber)
                {
                    res = Hand[i];
                    ActualGame.AddLast(res);
                }
                else if (Hand[i].BottomNumber == ActualGame.Last.Value.BottomNumber)
                {
                    res = Hand[i].SwipedDomino();
                    ActualGame.AddLast(res);
                }
                if (res != null)
                {
                    Hand.RemoveAt(i);
                    break;
                }
            }
            return res;
        }
    }
}
