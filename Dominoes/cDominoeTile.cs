using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominoes
{
    class DominoeTile
    {
        public readonly int TopNumber;
        public readonly int BottomNumber;

        public DominoeTile(int TopN, int BotN)
        {
            TopNumber = TopN;
            BottomNumber = BotN;
        }
        string GetDominoeString( bool Separator = false)
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

    }
}
