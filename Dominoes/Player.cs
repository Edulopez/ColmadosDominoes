using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{

    public class Player
    {

        public List<Dominoes.DominoeTile> Hand { get; set; }
        public List<Dominoes.DominoeTile> AvailableDominoes { get; set; }
        public List<bool[]> Dominos_EnemyDontHave { get; set; }
        public List<List<int>> Dominos_EnemyHave { get; set; }

        public readonly int TypePlayer;
        public int[] EnemysHandCount;
        public readonly int idPlayer;


        public Player(List<Dominoes.DominoeTile> StartHand, int TypeOfPlayer, int _idPlayer)
        {
            Hand = StartHand;
            idPlayer = _idPlayer;
            TypePlayer = TypeOfPlayer;

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
                if (i!= idPlayer)
                {
                    EnemysHandCount[i]=7;
                }
            }
        }

        static public List<Dominoes.DominoeTile> GetAvaiableDominoes(List<Dominoes.DominoeTile> UsedDominoes)
        {
            List<Dominoes.DominoeTile> Res = new List<Dominoes.DominoeTile>();
            for (int i=0 ; i<7 ; i++)
            {
                for (int j=i; j<7 ; j++)
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

        public string PrintHand(bool Separator, string SpaceBtw =" ")
        {
            string res = "";
            for (int i = 0; i < Hand.Count; i++)
            {
                res += Hand[i].GetDominoString(Separator) + SpaceBtw;
            }
            return res;
        }

        public DominoeTile MakeAMove(LinkedList<Dominoes.DominoeTile> CurrentGame, int idHandDomino, char Side)
        {
            
            if (idHandDomino == 0 || (Side != 'L' && Side != 'R')) return null;
            idHandDomino--;

            DominoeTile res = null;
            if (CurrentGame.Count==0)
            {
                res = Hand[idHandDomino];
                CurrentGame.AddLast(Hand[idHandDomino]);
                Hand.RemoveAt(idHandDomino);
                return res;
            } 

            if (Side == 'L')
            {
                if (Hand[idHandDomino].BottomNumber == CurrentGame.First.Value.TopNumber)
                {
                    res = Hand[idHandDomino];
                    CurrentGame.AddFirst(res);
                }
                else if (Hand[idHandDomino].TopNumber == CurrentGame.First.Value.TopNumber)
                {
                    res = Hand[idHandDomino].SwipedDomino();
                    CurrentGame.AddFirst(res);
                }
            }
            else if (Side=='R')
            {
                if (Hand[idHandDomino].TopNumber == CurrentGame.Last.Value.BottomNumber)
                {
                    res = Hand[idHandDomino];
                    CurrentGame.AddLast(res);
                }
                else if (Hand[idHandDomino].BottomNumber == CurrentGame.Last.Value.BottomNumber)
                {
                    res = Hand[idHandDomino].SwipedDomino();
                    CurrentGame.AddLast(res);
                }
            }

            if (res != null)
                Hand.RemoveAt(idHandDomino);

            return res;
        }


        public void Learn(LinkedList<Dominoes.DominoeTile> Game, int _IdPlayer, Dominoes.DominoeTile EnemyDomino = null)
        {
            if (EnemyDomino == null)
            {
                //Dominos_EnemyDontHave[_IdPlayer][(Game.First.Value.Item1)]=true;
                //Dominos_EnemyDontHave[_IdPlayer][(Game.Last.Value.Item2)]=true;
                return;
            }

            EnemysHandCount[_IdPlayer]--;
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
