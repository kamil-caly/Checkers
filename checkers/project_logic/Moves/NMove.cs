namespace project_logic.Moves
{
    public class NMove
    {
        public Position From { get; set; }
        public List<Position> Tos { get; set; }

        public NMove(Position from, List<Position> tos)
        {
            From = from;
            Tos = tos;
        }
    }
}
