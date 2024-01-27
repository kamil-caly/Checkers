using project_logic.Moves;
using System.Drawing;

namespace project_logic
{
    public class GameState
    {
        private BoardField [,] GameBoard;
        public Player CurrentPlayer { get; private set; }
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
                            setBoardField(new Position(r, c), FieldContent.Pawn, Player.Black);
                        }
                        else if (r == 1 && c % 2 == 0)
                        {
                            setBoardField(new Position(r, c), FieldContent.Pawn, Player.Black);
                        }
                    }
                    else if (r >= 5)
                    {
                        if (r % 2 != 0 && c % 2 == 0)
                        {
                            setBoardField(new Position(r, c), FieldContent.Pawn, Player.White);
                        }
                        else if (r == 6 && c % 2 != 0)
                        {
                            setBoardField(new Position(r, c), FieldContent.Pawn, Player.White);
                        }
                    }

                    if (GetBoardField(new Position(r, c)) == null)
                    {
                        setBoardField(new Position(r, c), FieldContent.None);
                    }
                }
            }

            return GameBoard;
        }

        public BoardField GetBoardField(Position pos)
        {
            return GameBoard[pos.row, pos.col];
        }

        public void setBoardField(Position pos, FieldContent content, Player? player = null)
        {
            GameBoard[pos.row, pos.col] = new BoardField(content, player);
        }

        public bool IsFieldEmpty(Position pos)
        {
            return IsOnBoard(pos) && GameBoard[pos.row, pos.col].Content == FieldContent.None;
        }

        public bool IsOnBoard(Position pos)
        {
            return pos.row <= 7 && pos.row >= 0 && pos.col <= 7 && pos.col >= 0;
        }

        public bool IsWhiteHere(Position pos)
        {
            return !IsFieldEmpty(pos) &&
                GetBoardField(pos).Player == Player.White;
        }

        public bool IsBlackHere(Position pos)
        {
            return !IsFieldEmpty(pos) &&
                GetBoardField(pos).Player == Player.Black;
        }

        public bool IsPawnHere(Position pos, Player color)
        {
            return !IsFieldEmpty(pos) && 
                GetBoardField(pos).Player == color && 
                GetBoardField(pos).Content == FieldContent.Pawn;
        }

        public bool IsLadyHere(Position pos, Player color)
        {
            return !IsFieldEmpty(pos) &&
                GetBoardField(pos).Player == color &&
                GetBoardField(pos).Content == FieldContent.Lady;
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player.White ? Player.Black : Player.White;
        }
    }
}
