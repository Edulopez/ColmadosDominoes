using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{

    public class Player
    {
        public const int MaxDominoesOnHand = 7;

        public readonly int Id;
        public readonly int PlayerType;

        public List<Dominoes.DominoeTile> Hand { get; set; }
        public List<Dominoes.DominoeTile> AvailableDominoes { get; set; }
        public List<bool[]> Dominos_EnemyDontHave { get; set; }
        public List<List<int>> Dominos_EnemyHave { get; set; }

        public int[] EnemiesHandCount;


        public Player(List<Dominoes.DominoeTile> StartHand, int _PlayerType, int _idPlayer)
        {
            Hand = StartHand;
            Id = _idPlayer;
            PlayerType = _PlayerType;

            AvailableDominoes = GetAvaiableDominoes(StartHand);
            Dominos_EnemyDontHave = GetDominosEnemyDontHaveInitialValue();
            Dominos_EnemyHave = new List<List<int>>();
            EnemiesHandCount = GetEnemiesHandCountInitialValue(_idPlayer);
        }

        private static List<bool[]>  GetDominosEnemyDontHaveInitialValue()
        {
            var res = new List<bool[]>();
            for (int i = 0; i < 4; i++)
            {
                res.Add(new bool[MaxDominoesOnHand]);
            }
            return res;
        }
        private static int[] GetEnemiesHandCountInitialValue(int currentPlayerId)
        {
            var res =new int[4];

            for (int i=0 ; i<4 ; i++)
            {
                if (i != currentPlayerId)
                {
                    res[i] = MaxDominoesOnHand;
                }
            }
            return res;
        }

        static public List<Dominoes.DominoeTile> GetAvaiableDominoes(List<Dominoes.DominoeTile> UsedDominoes) 
        {
            List<Dominoes.DominoeTile> Res = new List<Dominoes.DominoeTile>();
            for (int i = 0; i < MaxDominoesOnHand; i++)
            {
                for (int j = i; j < MaxDominoesOnHand; j++)
                {
                    bool can = false;
                    for (int k = 0; k < UsedDominoes.Count; k++)
                    {
                        can = (UsedDominoes[k].Contains(i) && UsedDominoes[k].Contains(j));
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

        public string GetHandString()
        {
            return GetHandString(true);
        }

        public string GetHandString(bool Separator, string SpaceBetweenTiles = " ")
        {
            bool UseSeparator = !string.IsNullOrEmpty(SpaceBetweenTiles);
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < Hand.Count; i++)
            {
                res.Append(Hand[i].GetDominoString(Separator));
                if(UseSeparator)
                    res.Append(SpaceBetweenTiles);
            }
            return res.ToString();
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

            EnemiesHandCount[PlayerId]--;
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
