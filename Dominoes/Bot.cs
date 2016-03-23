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
        /// 
        /// </summary>
        const int DUMMYVAL = 1000000;

        /// <summary>
        /// Maximun depth in the recursion algorithm that simulates the knowledge of the bot
        /// </summary>
        readonly int MaxRecursiveCalls;

        /// <summary>
        /// Dynamic programing memo
        /// </summary>
        public Hashtable dpHashTable;

        /// <summary>
        /// Constructor of the bot
        /// </summary>
        /// <param name="StartHand">Initial Dominoes Tiles in the hand</param>
        /// <param name="Type"></param>
        /// <param name="PlayerId">Unique Id of the player</param>
        /// <param name="LevelofKnowledge">Level of knowledge</param>
        public Bot(List<Dominoes.DominoeTile> StartHand, int Type, int PlayerId , int LevelofKnowledge = 11): base(StartHand, Type, PlayerId)
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
            int pointsInHand = 1;
            for (int i = 0; i < Hand.Count; ++i)
                pointsInHand += Hand[i].TopNumber + Hand[i].BottomNumber;

            for (int i = 0; i < EnemiesHandCount.Length; ++i)
                if (i != Id)
                {
                    if(EnemiesHandCount[i]==0) return DUMMYVAL;
                }

            return pointsInHand;
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

            string map = "";
            LinkedListNode<Dominoes.DominoeTile> Node = CurrentGame.First;
            for (int i = 0; i < CurrentGame.Count; i++)
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
                int value=DUMMYVAL;
                int TempValue=DUMMYVAL;
                int BestDomino=0;
                int LOR=0;
                for (int i = 0; i < stop; i++)
                {
                    DominoInUse = Hand[0];
                    Hand.RemoveAt(0);
                    if(CanIMove(CurrentGame,DominoInUse,DominoBoardSide.RigthSide)){
                        CurrentGame.AddLast(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSide.RigthSide));
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
                    if (CanIMove(CurrentGame, DominoInUse, DominoBoardSide.LeftSide))
                    {
                        CurrentGame.AddFirst(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSide.LeftSide));
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
        /// <param name="depth">Current depth in the recursion tree</param>
        /// <param name="finalDepth">Max depth in te recursion tree</param>
        /// <param name="_IdPlayer">Player who's playing</param>
        /// <returns>Points to win</returns>
        private int GetBestMove(LinkedList<DominoeTile> CurrentGame, int depth, int finalDepth, int _IdPlayer)
        {
            string idx = GetStringIndex(CurrentGame);
            if (dpHashTable.Contains(idx) == true) return (int)dpHashTable[idx];

            int value = Eval(CurrentGame);
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
            _IdPlayer %= 4;

            int stop;
            if (_IdPlayer != Id) stop = AvailableDominoes.Count;
            else stop = Hand.Count;

            for (int i = 0; i < stop; i++)
            {
                if (_IdPlayer == Id)
                {
                    DominoInUse = Hand[0];
                    Hand.RemoveAt(0);
                }
                else
                {
                    DominoInUse = AvailableDominoes[0];
                    AvailableDominoes.RemoveAt(0);
                }

                if (CanIMove(CurrentGame,DominoInUse,DominoBoardSide.RigthSide))
                {
                    EnemiesHandCount[_IdPlayer]--;
                    CurrentGame.AddLast(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSide.RigthSide));
                    tempValue = GetBestMove(CurrentGame, depth + 1, finalDepth - 1,_IdPlayer+1);
                    CurrentGame.RemoveLast();

                    EnemiesHandCount[_IdPlayer]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }

                else if (CanIMove(CurrentGame, DominoInUse, DominoBoardSide.LeftSide))
                {
                    EnemiesHandCount[_IdPlayer]--;
                    CurrentGame.AddFirst(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSide.LeftSide));
                    tempValue = GetBestMove(CurrentGame, depth + 1, finalDepth - 1, _IdPlayer + 1);
                    CurrentGame.RemoveFirst();

                    EnemiesHandCount[_IdPlayer]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }

                if (_IdPlayer == Id)
                    Hand.Add(DominoInUse);
                else
                    AvailableDominoes.Add(DominoInUse);
            }

            if(!dpHashTable.Contains(idx)) dpHashTable.Add(idx, value);

            return value;
        }

       
        /// <summary>
        ///   Get the domino in the desired position wich will be played
        /// </summary>
        /// <param name="CurrentGame">Current Dominoes Tiles played in the game</param>
        /// <param name="DominoInUse">Domino that will be used</param>
        /// <param name="side">Side of the board that you will play</param>
        /// <returns>Dominoe in the desired position, null if you cant play it</returns>
        private DominoeTile SetPositionOfDomino(LinkedList<DominoeTile> CurrentGame, DominoeTile DominoInUse, DominoBoardSide side)
        {
            if (CurrentGame.Count == 0) return DominoInUse;

            if (side == DominoBoardSide.RigthSide)
            {
                return DominoInUse.GetDominoInPosition(CurrentGame.Last.Value, side);
            }
            else //DominoBoardSide.LeftSide
            {
                return DominoInUse.GetDominoInPosition(CurrentGame.First.Value, side);
            }
        }

        /// <summary>
        /// Check if you can move a dominoe in a desired position
        /// </summary>
        /// <param name="CurrentGame">Current Dominoes Tiles played in the game</param>
        /// <param name="DominoInUse">Dominoe tile that you want to move</param>
        /// <param name="side">Side wich you want to move</param>
        /// <returns>True if you can move</returns>
        private bool CanIMove(LinkedList<DominoeTile> CurrentGame, DominoeTile DominoInUse, DominoBoardSide side)
        {
            if (CurrentGame.Count == 0) return true;

            if (side == DominoBoardSide.RigthSide)
            {
                if (CurrentGame.Last.Value.BottomNumber == DominoInUse.TopNumber || CurrentGame.Last.Value.BottomNumber == DominoInUse.BottomNumber)
                    return true;
            }
            else
            {
                if (CurrentGame.First.Value.TopNumber == DominoInUse.TopNumber || CurrentGame.First.Value.TopNumber == DominoInUse.BottomNumber)
                    return true;
            }
            return false;
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
