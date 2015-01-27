using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominoes
{
    public class DominoeTile
    {
        public readonly int TopNumber;
        public readonly int BottomNumber;

        public DominoeTile(int TopN, int BotN)
        {
            TopNumber = TopN;
            BottomNumber = BotN;
        }

        public string GetDominoeString( bool Separator = false)
        {
            string sep = "";
            if (Separator) sep = "|";
            return TopNumber.ToString() + sep + BottomNumber.ToString();
        }

        string GetDominoeString(char SeparatorChar)
        {
            string sep = SeparatorChar.ToString();
            return TopNumber.ToString() + sep + BottomNumber.ToString();
        }

        public DominoeTile SwipedDomino()
        {
            return new DominoeTile(BottomNumber, TopNumber);
        }

        public bool Match(DominoeTile b)
        {
            return (
                    TopNumber == b.TopNumber
                || TopNumber == b.BottomNumber
                || BottomNumber == b.TopNumber
                || BottomNumber == b.BottomNumber
                );
        }

        //public DominoeTile GetMatchedPositionedDominoe(DominoeTile b)
        //{
        //    if (!Match(b))
        //        return null;

            
        //}
        public bool Contains(int number)
        {
            return (number == TopNumber || number == BottomNumber);
        }
        #region "Operators"

        public static bool operator ==(DominoeTile a, int b)
        {
            return
            (
                (a.TopNumber == b) || (a.BottomNumber == b)
            );
        }
        public static bool operator !=(DominoeTile a, int b)
        {
            return
            (
                (a.TopNumber != b) && (a.BottomNumber != b)
            );
        }
        public static bool operator ==(DominoeTile a, DominoeTile b)
        {
            return
            (
                ((a.TopNumber == b.TopNumber) && (a.BottomNumber == b.BottomNumber))
                ||
                ((a.BottomNumber == b.TopNumber) && (a.TopNumber == b.BottomNumber))
            );
        }
        public static bool operator !=(DominoeTile a, DominoeTile b)
        {
            return
            (
                ((a.TopNumber != b.TopNumber) && (a.BottomNumber != b.BottomNumber))
                ||
                ((a.BottomNumber != b.TopNumber) && (a.TopNumber != b.BottomNumber))
            );
        }

        #endregion




    }
}
