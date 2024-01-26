namespace project_logic.Moves
{
    public class Move
    {
        public Position From { get; set; }
        public List<Position> Tos { get; set; }

        public Move(Position from, List<Position> tos)
        {
            From = from;
            Tos = tos;
        }
    }
}
