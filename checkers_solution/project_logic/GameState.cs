namespace project_logic
{
    public class GameState
    {
        private BoardField [,] GameBoard;
        private Player CurrentPlayer;
        public GameState()
        {
            CurrentPlayer = Player.White;
            GameBoard = InitBoard();
        }

        private BoardField[,] InitBoard()
        {
            int rows = 8;
            int cols = 8;
            GameBoard = new BoardField[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (r <= 2)
                    {
                        if (r % 2 == 0 && c % 2 != 0)
                        {
                            setBoardField(r, c, FieldContent.Pawn, Player.Black);
                        }
                        else if (r == 1 && c % 2 == 0)
                        {
                            setBoardField(r, c, FieldContent.Pawn, Player.Black);
                        }
                    }
                    else if (r >= 5)
                    {
                        if (r % 2 != 0 && c % 2 == 0)
                        {
                            setBoardField(r, c, FieldContent.Pawn, Player.White);
                        }
                        else if (r == 6 && c % 2 != 0)
                        {
                            setBoardField(r, c, FieldContent.Pawn, Player.White);
                        }
                    }

                    if (GetBoardField(r, c) == null)
                    {
                        setBoardField(r, c, FieldContent.None);
                    }
                }
            }

            return GameBoard;
        }

        public BoardField GetBoardField(int r, int c)
        {
            return GameBoard[r, c];
        }

        public void setBoardField(int r, int c, FieldContent content, Player? player = null)
        {
            GameBoard[r, c] = new BoardField(content, player);
        } 
    }
}
