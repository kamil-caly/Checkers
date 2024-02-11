using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_logic.Moves
{
    public class BMove
    {
        public Position From { get; set; }
        public Position To { get; set; }
        public List<Position> BeatingPeacesPos { get; set; }

        public BMove(Position from, Position to, List<Position> beatingPeacesPos)
        {
            From = from;
            To = to;
            BeatingPeacesPos = beatingPeacesPos;
        }
    }
}
