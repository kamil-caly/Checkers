using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_logic.Moves
{
    public class BeatedPeace
    {
        public Position pos { get; set; }
        public BoardField boardField { get; set; }
        public BeatedPeace(Position Pos, BoardField BoardField)
        {
            pos = Pos;
            boardField = BoardField;
        }
    }
}
