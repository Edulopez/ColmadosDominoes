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

        public int Points 
        {
            get
            {
                return TopNumber + BottomNumber;
            }
        }

        public DominoeTile(int TopN, int BotN)
        {
            TopNumber = TopN;
            BottomNumber = BotN;
        }

        public string GetDominoString( bool Separator = false)
        {
            string sep = "";
            if (Separator) sep = "|";
            return TopNumber.ToString() + sep + BottomNumber.ToString();
        }

        string GetDominoString(char SeparatorChar)
        {
            string sep = SeparatorChar.ToString();
            return TopNumber.ToString() + sep + BottomNumber.ToString();
        }

        public DominoeTile SwipedDomino()
        {
            return new DominoeTile(BottomNumber, TopNumber);
        }

        public DominoeTile GetDominoInPosition(DominoeTile D, DominoBoardSide side)
        {
            if (D == null) return null;

            if (side == DominoBoardSide.RigthSide)
            {
                if (D.BottomNumber == this.TopNumber)
                    return this;
                if (D.BottomNumber == this.BottomNumber)
                    return this.SwipedDomino();
            }
            else //DominoBoardSide.LeftSide
            {
                if (D.TopNumber == this.TopNumber)
                    return this.SwipedDomino();
                if (D.TopNumber == this.BottomNumber)
                    return this;
            }
            return null;
        }

        public bool Match(DominoeTile b)
        {
            if (b == null) return false;
            return (
                    TopNumber == b.TopNumber
                || TopNumber == b.BottomNumber
                || BottomNumber == b.TopNumber
                || BottomNumber == b.BottomNumber
                );
        }

        public bool Contains(int number)
        {
            return (number == TopNumber || number == BottomNumber);
        }


        #region "Operators"

        //public static bool operator ==(DominoeTile a, int b)
        //{
        //    return
        //    (
        //        (a.TopNumber == b) || (a.BottomNumber == b)
        //    );
        //}
        //public static bool operator !=(DominoeTile a, int b)
        //{
        //    return
        //    (
        //        (a.TopNumber != b) && (a.BottomNumber != b)
        //    );
        //}
        //public static bool operator ==(DominoeTile a, DominoeTile b)
        //{

        //    return
        //    (
        //        ((a.TopNumber == b.TopNumber) && (a.BottomNumber == b.BottomNumber))
        //        ||
        //        ((a.BottomNumber == b.TopNumber) && (a.TopNumber == b.BottomNumber))
        //    );
        //}
        //public static bool operator !=(DominoeTile a, DominoeTile b)
        //{
        //    return
        //    (
        //        ((a.TopNumber != b.TopNumber) && (a.BottomNumber != b.BottomNumber))
        //        ||
        //        ((a.BottomNumber != b.TopNumber) && (a.TopNumber != b.BottomNumber))
        //    );
        //}

        #endregion




    }
}
