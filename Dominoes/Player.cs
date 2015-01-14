using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominoes
{

    public class Player
    {

        public List<Tuple<int, int>> Hand { get; set; }
        public List<Tuple<int, int>> AvailableDominoes { get; set; }
        public List<bool[]> Dominos_EnemyDontHave { get; set; }
        public List<List<int>> Dominos_EnemyHave { get; set; }

        public readonly int TypePlayer;
        public int[] EnemysHandCount;
        public readonly int idPlayer; 
        

        public Player(List<Tuple<int, int>> StartHand, int TypeOfPlayer, int _idPlayer)
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

        static public List<Tuple<int, int>> GetAvaiableDominoes(List<Tuple<int, int>> UsedDominoes)
        {
            List<Tuple<int, int>> Res = new List<Tuple<int, int>>();
            for (int i=0 ; i<7 ; i++)
            {
                for (int j=i; j<7 ; j++)
                {
                    bool can = true;
                    for (int q = 0; q < UsedDominoes.Count; q++)
                    {
                        if (i == UsedDominoes[q].Item1 && j == UsedDominoes[q].Item2)
                            can = false;
                        else if (j == UsedDominoes[q].Item1 && i == UsedDominoes[q].Item2)
                            can = false;
                    }
                    if (can) Res.Add(new Tuple<int, int>(i, j));
                }
            }
            return Res;
        }

        public bool CanPlay(LinkedList<Tuple<int, int>> ActualGame)
        {
            if (ActualGame.Count == 0) return true;

            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Item1 == ActualGame.First.Value.Item1 || Hand[i].Item2 == ActualGame.First.Value.Item1 ||
                    Hand[i].Item1 == ActualGame.Last.Value.Item2 || Hand[i].Item2 == ActualGame.Last.Value.Item2)
                    return true;
            }
            return false;
        }

        public string PrintHand()
        {
            string res = "";
            for (int i = 0; i < Hand.Count; i++)
            {
                res += Hand[i].Item1.ToString() + "|" + Hand[i].Item2.ToString() + " ";
            }
            return res;
        }
        public string PrintHand(bool AllTogether)
        {
            if (AllTogether == false) return PrintHand();

            string res = "";
            for (int i = 0; i < Hand.Count; i++)
            {
                res += Hand[i].Item1.ToString()  + Hand[i].Item2.ToString();
            }
            return res;
        }

        public  Tuple<int, int> MakeAMove(LinkedList<Tuple<int, int>> ActualGame, int idHandDomino, char Side)
        {
            if (idHandDomino == 0 || (Side != 'L' && Side != 'R')) return null;
            idHandDomino--;

            Tuple<int, int> res = null;
            if (ActualGame.Count==0)
            {
                res = Hand[idHandDomino];
                ActualGame.AddLast(new Tuple<int, int>(Hand[idHandDomino].Item1, Hand[idHandDomino].Item2));
                Hand.RemoveAt(idHandDomino);
                return res;
            } 

            if (Side == 'L')
            {
                if (Hand[idHandDomino].Item1 == ActualGame.First.Value.Item1)
                {
                    res = new Tuple<int, int>(Hand[idHandDomino].Item2, Hand[idHandDomino].Item1);
                    Hand.RemoveAt(idHandDomino);
                    ActualGame.AddFirst(res);
                }
                else if (Hand[idHandDomino].Item2 == ActualGame.First.Value.Item1)
                {
                    res = new Tuple<int, int>(Hand[idHandDomino].Item1, Hand[idHandDomino].Item2);
                    Hand.RemoveAt(idHandDomino);
                    ActualGame.AddFirst(res);
                }
            }
            else if (Side=='R')
            {
                if (Hand[idHandDomino].Item1 == ActualGame.Last.Value.Item2)
                {
                    res = new Tuple<int, int>(Hand[idHandDomino].Item1, Hand[idHandDomino].Item2);
                    Hand.RemoveAt(idHandDomino);
                    ActualGame.AddLast(res);
                }
                else if (Hand[idHandDomino].Item2 == ActualGame.Last.Value.Item2)
                {
                    res = new Tuple<int, int>(Hand[idHandDomino].Item2, Hand[idHandDomino].Item1);
                    Hand.RemoveAt(idHandDomino);
                    ActualGame.AddLast(res);
                }
            }
            return res;
        }


        public void Learn(LinkedList<Tuple<int, int>> Game, int _IdPlayer, Tuple<int, int> EnemyDomino = null)
        {
            if (EnemyDomino == null)
            {
 //               Dominos_EnemyDontHave[_IdPlayer][(Game.First.Value.Item1)]=true;
   //             Dominos_EnemyDontHave[_IdPlayer][(Game.Last.Value.Item2)]=true;
                return;
            }

            EnemysHandCount[_IdPlayer]--;
            Tuple<int, int> copy = new Tuple<int, int>(EnemyDomino.Item2, EnemyDomino.Item1);
            for (int i = 0; i < AvailableDominoes.Count(); i++)
            {
                if (AvailableDominoes[i] == EnemyDomino || AvailableDominoes[i] == copy)
                {
                    AvailableDominoes.RemoveAt(i);
                    break;
                }
            }
        }

    }
}
