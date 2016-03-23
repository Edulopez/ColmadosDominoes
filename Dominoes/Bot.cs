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
        /// <summary>
        ///  Dummy value that repesent the maximount amount of point to win in a game
        /// </summary>
        const int MaximunPoint = 1000000;

        /// <summary>
        /// Maximun depth in the recursion algorithm that simulates the knowledge of the bot
        /// </summary>
        readonly int MaxRecursiveCalls;

        /// <summary>
        /// Dynamic programing memo
        /// </summary>
        public Hashtable dpHashTable;

        
        public Bot(List<Dominoes.DominoeTile> StartHand, int PlayerId , int LevelofKnowledge = 11): base(StartHand, PlayerId)
        {
            dpHashTable = new Hashtable();
            MaxRecursiveCalls = LevelofKnowledge;
            //AvailableDominoes.Sort();
            //Hand.Sort();
        }

        // retorna los puntos del juego.
        /// <summary>
        /// Get the points that you can win in a turn
        /// </summary>
        /// <param name="CurrentGame">Current Dominoes Tiles played in the game</param>
        /// <returns>Points in the game</returns>
        private int Eval(LinkedList<Dominoes.DominoeTile> CurrentGame)
        {
            for (int i = 0; i < EnemiesHandCount.Length; ++i)
                if (i != Id)
                {
                    if(EnemiesHandCount[i]==0) return MaximunPoint;
                }

            return PointsInHand;
        }

        /// <summary>
        /// Get an unique index of the game for a player
        /// </summary>
        /// <param name="CurrentGame">Current Dominoes Tiles played in the game</param>
        /// <returns>Unique index of the game</returns>
        private string GetStringIndex(LinkedList<Dominoes.DominoeTile> CurrentGame)
        {
            string idx;
            idx = GetHandString(false, "");

            StringBuilder map = new StringBuilder();
            LinkedListNode<Dominoes.DominoeTile> Node = CurrentGame.First;
            for (int i = 0; i < CurrentGame.Count; i++)
            {
                if (Node.Value.TopNumber == Node.Value.BottomNumber)
                {
                    Node = Node.Next;
                    continue;
                }
                map.Append(Node.Value.GetDominoString(false));
                Node = Node.Next;
            }
            return map + idx;
        }

        /// <summary>
        /// Get the best DominoTile to move  according to the result of GetBestMove in the current game
        /// </summary>
        /// <param name="CurrentGame">Current Dominoes Tiles played in the game</param>
        /// <returns>Best domino tile to play</returns>
        public DominoeTile MakeAMove(LinkedList<DominoeTile> CurrentGame)
        {
            DominoeTile DominoInUse = null;
            if (!CanPlay(CurrentGame)) return null;
            else
            {
                int stop = Hand.Count;
                int value=MaximunPoint;
                int TempValue=MaximunPoint;
                int BestDomino=0;
                int LOR=0;
                for (int i = 0; i < stop; i++)
                {
                    DominoInUse = Hand[0];
                    Hand.RemoveAt(0);
                    if(CanIMove(CurrentGame,DominoInUse,DominoBoardSides.Rigth)){
                        CurrentGame.AddLast(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSides.Rigth));
                        TempValue = GetBestMove(CurrentGame, 0, MaxRecursiveCalls, Id + 1);
                        CurrentGame.RemoveLast();
                        //Console.WriteLine("En la derecha La ficha " + DominoInUse.GetDominoString(true)+ " Tiene  " + TempValue);
                    }
                    if ((value) > (TempValue))
                    {
                        value = TempValue;
                        BestDomino = i;
                        LOR = 1;
                    }
                    if (CanIMove(CurrentGame, DominoInUse, DominoBoardSides.Left))
                    {
                        CurrentGame.AddFirst(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSides.Left));
                        TempValue = GetBestMove(CurrentGame, 0, MaxRecursiveCalls, Id + 1);
                        CurrentGame.RemoveFirst();
                        //Console.WriteLine("En la izquierda La ficha " + DominoInUse.GetDominoString(true) + " Tiene  " + TempValue);
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
               DominoInUse= MakeAMove(CurrentGame, BestDomino + 1, side);
               //Console.Read();
            }

            EnemiesHandCount[Id]--;
            return DominoInUse;
        }


        /// <summary>
        ///  MiniMax like function to get the maximun points that the player can win
        /// </summary>
        /// <param name="CurrentGame">Current Dominoes Tiles played in the game</param>
        /// <param name="CurrentDepth">Current depth in the recursion tree</param>
        /// <param name="FinalDepth">Max depth in te recursion tree</param>
        /// <param name="PlayerId">Player who's playing</param>
        /// <returns>Points to win</returns>
        private int GetBestMove(LinkedList<DominoeTile> CurrentGame, int CurrentDepth, int FinalDepth, int PlayerId)
        {
            string idx = GetStringIndex(CurrentGame);
            if (dpHashTable.Contains(idx) == true) return (int)dpHashTable[idx];

            int value = Eval(CurrentGame);
            if (FinalDepth == 0 || value == 1)
            { 
                dpHashTable.Add(idx, value); 
                return value; 
            }

            if (MaximunPoint == value) {
                if (dpHashTable.Contains(idx) == false) dpHashTable.Add(idx, value);
                return value; }

            DominoeTile DominoInUse;
            int tempValue = MaximunPoint;
            PlayerId %= 4;

            int stop;
            if (PlayerId != Id) stop = AvailableDominoes.Count;
            else stop = Hand.Count;

            for (int i = 0; i < stop; i++)
            {
                if (PlayerId == Id)
                {
                    DominoInUse = Hand[0];
                    Hand.RemoveAt(0);
                }
                else
                {
                    DominoInUse = AvailableDominoes[0];
                    AvailableDominoes.RemoveAt(0);
                }

                if (CanIMove(CurrentGame,DominoInUse,DominoBoardSides.Rigth))
                {
                    EnemiesHandCount[PlayerId]--;
                    CurrentGame.AddLast(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSides.Rigth));
                    tempValue = GetBestMove(CurrentGame, CurrentDepth + 1, FinalDepth - 1,PlayerId+1);
                    CurrentGame.RemoveLast();

                    EnemiesHandCount[PlayerId]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }

                else if (CanIMove(CurrentGame, DominoInUse, DominoBoardSides.Left))
                {
                    EnemiesHandCount[PlayerId]--;
                    CurrentGame.AddFirst(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSides.Left));
                    tempValue = GetBestMove(CurrentGame, CurrentDepth + 1, FinalDepth - 1, PlayerId + 1);
                    CurrentGame.RemoveFirst();

                    EnemiesHandCount[PlayerId]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }

                if (PlayerId == Id)
                    Hand.Add(DominoInUse);
                else
                    AvailableDominoes.Add(DominoInUse);
            }

            if(!dpHashTable.Contains(idx)) dpHashTable.Add(idx, value);

            return value;
        }

             

        /// <summary>
        /// Get a DominoeTile that can be played from the hand
        /// </summary>
        /// <param name="CurrentGame">Current Dominoes Tiles played in the game</param>
        /// <returns>DominoeTile form the Hand object if exist, otherwise return null</returns>
        public DominoeTile DummyMove(LinkedList<DominoeTile> CurrentGame)
        {
            DominoeTile res = null;
            if (CurrentGame.Count == 0)
            {
                Random rnd = new Random();
                int id = rnd.Next(0, Hand.Count);
                res = Hand[id];
                Hand.RemoveAt(id);
                CurrentGame.AddFirst(res);
                return res;
            }
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].TopNumber == CurrentGame.First.Value.TopNumber)
                {
                    res = Hand[i].SwipedDomino();
                    CurrentGame.AddFirst(res);
                }
                else if (Hand[i].BottomNumber == CurrentGame.First.Value.TopNumber)
                {
                    res = Hand[i];
                    CurrentGame.AddFirst(res);
                }
                else if (Hand[i].TopNumber == CurrentGame.Last.Value.BottomNumber)
                {
                    res = Hand[i];
                    CurrentGame.AddLast(res);
                }
                else if (Hand[i].BottomNumber == CurrentGame.Last.Value.BottomNumber)
                {
                    res = Hand[i].SwipedDomino();
                    CurrentGame.AddLast(res);
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
