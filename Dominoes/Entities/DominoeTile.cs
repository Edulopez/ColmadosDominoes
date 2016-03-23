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

        public DominoeTile(int _TopNumber, int _BottomNumber)
        {
            TopNumber = _TopNumber;
            BottomNumber = _BottomNumber;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tile"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public DominoeTile GetDominoInPosition(DominoeTile Tile, DominoBoardSides side)
        {
            if (Tile == null) return null;

            if (side == DominoBoardSides.Rigth)
            {
                if (Tile.BottomNumber == this.TopNumber)
                    return this;
                if (Tile.BottomNumber == this.BottomNumber)
                    return this.SwipedDomino();
            }
            else //DominoBoardSide.LeftSide
            {
                if (Tile.TopNumber == this.TopNumber)
                    return this.SwipedDomino();
                if (Tile.TopNumber == this.BottomNumber)
                    return this;
            }
            return null;
        }

        /// <summary>
        ///  Check if two tiles match
        /// </summary>
        /// <param name="Tile"></param>
        /// <returns>true if they match</returns>
        public bool Match(DominoeTile Tile)
        {
            if (Tile == null) return false;
            return (
                    TopNumber == Tile.TopNumber
                || TopNumber == Tile.BottomNumber
                || BottomNumber == Tile.TopNumber
                || BottomNumber == Tile.BottomNumber
                );
        }

        public bool Contains(int number)
        {
            return (number == TopNumber || number == BottomNumber);
        }


   




    }
}
