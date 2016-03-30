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

        
        /// <summary>
        /// Get points to evaluate the efectiveness of your current game and how much is left to win
        /// </summary>
        /// <param name="CurrentGame">Current Dominoes Tiles played in the game</param>
        /// <returns>Points left to win</returns>
        private int Eval(LinkedList<Dominoes.DominoeTile> CurrentGame)
        {
            for (int i = 0; i < EnemiesHandCount.Length; ++i)
                if (i != Id)
                {
                    // You lost
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
            map.Append(idx);
            return map.ToString();
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
                int HandCount = Hand.Count;
                int value=MaximunPoint;
                int TempValue=MaximunPoint;
                int BestDomino=0;
                int LOR=0;
                for (int i = 0; i < HandCount; i++)
                {
                    DominoInUse = Hand[0];
                    Hand.RemoveAt(0);
                    if(DominoTileCanBePlayed(CurrentGame,DominoInUse,DominoBoardSides.Rigth)){
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
                    if (DominoTileCanBePlayed(CurrentGame, DominoInUse, DominoBoardSides.Left))
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

                DominoBoardSides Side;
                if (LOR < 0) Side = DominoBoardSides.Left;
                else Side = DominoBoardSides.Rigth;
               DominoInUse= MakeAMove(CurrentGame, BestDomino + 1, Side);
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
            if (dpHashTable.Contains(idx) == true)
                return (int)dpHashTable[idx];

            int value = Eval(CurrentGame);

            // If you win or if you cannot analize more
            if (FinalDepth == 0 || value == 0)
            { 
                dpHashTable.Add(idx, value); 
                return value; 
            }

            // If you lost
            if (value == MaximunPoint)
            {
                dpHashTable.Add(idx, value);
                return value; 
            }

            int tempValue = MaximunPoint;
            PlayerId %= 4;

            int MaxIterations;
            if (PlayerId != Id) 
                MaxIterations = AvailableDominoes.Count;
            else 
                MaxIterations = Hand.Count;


            DominoeTile DominoInUse;

            for (int i = 0; i < MaxIterations; i++)
            {
                // Remove a tile from the stack of availables depending of the current player playing. Actios will be taken with this tile
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

                bool DominoIsPlayed = false;

                // Added the tile to the board and check the futures plays
                if (DominoTileCanBePlayed(CurrentGame,DominoInUse,DominoBoardSides.Rigth))
                {
                    DominoIsPlayed = true;
                    EnemiesHandCount[PlayerId]--;
                    CurrentGame.AddLast(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSides.Rigth));
                    tempValue = GetBestMove(CurrentGame, CurrentDepth + 1, FinalDepth - 1,PlayerId+1);
                    CurrentGame.RemoveLast();
                }
                else if (DominoTileCanBePlayed(CurrentGame, DominoInUse, DominoBoardSides.Left))
                {
                    DominoIsPlayed = true;
                    EnemiesHandCount[PlayerId]--;
                    CurrentGame.AddFirst(SetPositionOfDomino(CurrentGame, DominoInUse, DominoBoardSides.Left));
                    tempValue = GetBestMove(CurrentGame, CurrentDepth + 1, FinalDepth - 1, PlayerId + 1);
                    CurrentGame.RemoveFirst();
                }

                if(DominoIsPlayed)
                {
                    EnemiesHandCount[PlayerId]++;
                    if ((value) > (tempValue)) { value = (tempValue); }
                }

                //Return the tile to the initial asumption
                if (PlayerId == Id)
                    Hand.Add(DominoInUse);
                else
                    AvailableDominoes.Add(DominoInUse);
            }

            if(!dpHashTable.Contains(idx))
                dpHashTable.Add(idx, value);

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
