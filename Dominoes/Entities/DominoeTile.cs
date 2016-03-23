using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominoes
{
    public class DominoeTile
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly int TopNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly int BottomNumber;

        /// <summary>
        /// Points of the Tile. Points are the sum of the two numbres of the Dominoe Tile
        /// </summary>
        public int Points 
        {
            get
            {
                return TopNumber + BottomNumber;
            }
        }

        /// <summary>
        /// Constructor of the Tile
        /// </summary>
        /// <param name="TopN">Top number</param>
        /// <param name="BotN">Bottom Number</param>
        public DominoeTile(int TopN, int BotN)
        {
            TopNumber = TopN;
            BottomNumber = BotN;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public string GetDominoString( bool Separator = false)
        {
            string sep = "";
            if (Separator) sep = "|";
            return TopNumber.ToString() + sep + BottomNumber.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SeparatorChar"></param>
        /// <returns></returns>
        string GetDominoString(char SeparatorChar)
        {
            string sep = SeparatorChar.ToString();
            return TopNumber.ToString() + sep + BottomNumber.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DominoeTile SwipedDomino()
        {
            return new DominoeTile(BottomNumber, TopNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="D"></param>
        /// <param name="side"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
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
