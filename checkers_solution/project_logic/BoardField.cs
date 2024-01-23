namespace project_logic
{
    public class BoardField
    {
        public BoardField(FieldContent content, Player? player)
        {
            Content = content;
            if(player != null)
            {
                Player = player;
            }
        }
        public FieldContent Content { get; set; }
        public Player? Player { get; set; }
    }
}
