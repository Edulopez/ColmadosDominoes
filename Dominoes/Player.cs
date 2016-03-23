using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{

    public class Player
    {
        public const int MaxDiominoesOnHand = 7;
        public List<Dominoes.DominoeTile> Hand { get; set; }
        public List<Dominoes.DominoeTile> AvailableDominoes { get; set; }
        public List<bool[]> Dominos_EnemyDontHave { get; set; }
        public List<List<int>> Dominos_EnemyHave { get; set; }

        public readonly int PlayerType;
        public int[] EnemysHandCount;
        public readonly int PlayerId;


        public Player(List<Dominoes.DominoeTile> StartHand, int _PlayerType, int _idPlayer)
        {
            Hand = StartHand;
            PlayerId = _idPlayer;
            PlayerType = _PlayerType;

            AvailableDominoes = GetAvaiableDominoes(StartHand);
            Dominos_EnemyDontHave = new List<bool[]>();
            for (int i = 0; i < 4; i++)
            {
                Dominos_EnemyDontHave.Add( new bool[7]);
            }
            Dominos_EnemyHave = new List<List<int>>();

            EnemysHandCount = new int[4];

            for (int i=0 ; i<4 ; i++)
            {
                if (i!= PlayerId)
                {
                    EnemysHandCount[i]=7;
                }
            }
        }

        static public List<Dominoes.DominoeTile> GetAvaiableDominoes(List<Dominoes.DominoeTile> UsedDominoes) 
        {
            List<Dominoes.DominoeTile> Res = new List<Dominoes.DominoeTile>();
            for (int i = 0; i < MaxDiominoesOnHand; i++)
            {
                for (int j = i; j < MaxDiominoesOnHand; j++)
                {
                    bool can = false;
                    for (int q = 0; q < UsedDominoes.Count; q++)
                    {
                        can = (UsedDominoes[q].Contains(i) && UsedDominoes[q].Contains(j));
                        if (can) break;
                    }
                    if (can) Res.Add(new Dominoes.DominoeTile(i, j));
                }
            }
            return Res;
        }

        public bool CanPlay(LinkedList<Dominoes.DominoeTile> CurrentGame)
        {
            if (CurrentGame.Count == 0) return true;

            for (int i = 0; i < Hand.Count; i++)
            {
                if ( CurrentGame.First.Value.Match(Hand[i]))
                    return true;
            }
            return false;
        }

        public string PrintHand()
        {
            return PrintHand(true);
        }

        public string PrintHand(bool Separator, string SpaceBetweenTiles =" ")
        {
            string res = "";
            for (int i = 0; i < Hand.Count; i++)
            {
                res += Hand[i].GetDominoString(Separator) + SpaceBetweenTiles;
            }
            return res;
        }

        public DominoeTile MakeAMove(LinkedList<Dominoes.DominoeTile> CurrentGame, int HandDominoId, char Side)
        {
            
            if (HandDominoId == 0 || (Side != 'L' && Side != 'R')) return null;
            HandDominoId--;

            DominoeTile res = null;
            if (CurrentGame.Count==0)
            {
                res = Hand[HandDominoId];
                CurrentGame.AddLast(Hand[HandDominoId]);
                Hand.RemoveAt(HandDominoId);
                return res;
            } 

            if (Side == 'L')
            {
                if (Hand[HandDominoId].BottomNumber == CurrentGame.First.Value.TopNumber)
                {
                    res = Hand[HandDominoId];
                    CurrentGame.AddFirst(res);
                }
                else if (Hand[HandDominoId].TopNumber == CurrentGame.First.Value.TopNumber)
                {
                    res = Hand[HandDominoId].SwipedDomino();
                    CurrentGame.AddFirst(res);
                }
            }
            else if (Side=='R')
            {
                if (Hand[HandDominoId].TopNumber == CurrentGame.Last.Value.BottomNumber)
                {
                    res = Hand[HandDominoId];
                    CurrentGame.AddLast(res);
                }
                else if (Hand[HandDominoId].BottomNumber == CurrentGame.Last.Value.BottomNumber)
                {
                    res = Hand[HandDominoId].SwipedDomino();
                    CurrentGame.AddLast(res);
                }
            }

            if (res != null)
                Hand.RemoveAt(HandDominoId);

            return res;
        }


        public void Learn(LinkedList<Dominoes.DominoeTile> Game, int PlayerId, Dominoes.DominoeTile EnemyDomino = null)
        {
            if (EnemyDomino == null)
            {
                //Dominos_EnemyDontHave[_IdPlayer][(Game.First.Value.Item1)]=true;
                //Dominos_EnemyDontHave[_IdPlayer][(Game.Last.Value.Item2)]=true;
                return;
            }

            EnemysHandCount[PlayerId]--;
            for (int i = 0; i < AvailableDominoes.Count(); i++)
            {
                if (AvailableDominoes[i] == EnemyDomino)
                {
                    AvailableDominoes.RemoveAt(i);
                    break;
                }
            }
        }

    }
}
