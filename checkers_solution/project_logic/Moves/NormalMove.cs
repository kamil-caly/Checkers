namespace project_logic.Moves
{
    public class NormalMove
    {
        private readonly GameState gameState;
        private const int rows = 8;
        private const int cols = 8;
        public NormalMove(GameState gameState)
        {
            this.gameState = gameState;
        }

        public IEnumerable<NMove> GetAllLegalMoves(Player color)
        {
            List<NMove> moves = new List<NMove>();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (gameState.IsPawnHere(new Position(r, c), color))
                    {
                        NMove? currPawnMove = GetMovesForPawn(new Position(r, c), color);
                        if(currPawnMove != null)
                        {
                            moves.Add(currPawnMove);
                        }   
                    }
                    else if (gameState.IsLadyHere(new Position(r, c), color))
                    {
                        NMove? currLadyMove = GetMovesForLady(new Position(r, c));
                        if (currLadyMove != null)
                        {
                            moves.Add(currLadyMove);
                        }
                    }
                }
            }

            return moves;
        }

        public void MakeMove(Position from, Position to)
        {
            BoardField movedPiece = gameState.GetBoardField(new Position(from.row, from.col));
            
            gameState.setBoardField(new Position(from.row, from.col), FieldContent.None);

            gameState.setBoardField(new Position(to.row, to.col), movedPiece.Content, movedPiece.Player);

            if (gameState.IsPlayerOnPromotionLine(to) && gameState.IsPawnHere(to))
            {
                ReplaceInLady(to);
            }

            gameState.SwitchPlayer();
        }

        private void ReplaceInLady(Position pos)
        {
            Player color = gameState.IsWhiteHere(pos) ? Player.White : Player.Black;
            gameState.setBoardField(new Position(pos.row, pos.col), FieldContent.Lady, color);
        }

        private NMove? GetMovesForPawn(Position fromPos, Player color) 
        {
            List<Position> toPos = new List<Position>();    

            if (color == Player.White)
            {
                for (int c = -1; c < 2; c += 2)
                {
                    // left-up, right-up
                    if (gameState.IsFieldEmpty(new Position(fromPos.row - 1, fromPos.col + c)))
                    {
                        toPos.Add(new Position(fromPos.row - 1, fromPos.col + c));
                    }
                }
            }
            else
            {
                for (int c = -1; c < 2; c += 2)
                {
                    // left-down, right-down
                    if (gameState.IsFieldEmpty(new Position(fromPos.row + 1, fromPos.col + c)))
                    {
                        toPos.Add(new Position(fromPos.row + 1, fromPos.col + c));
                    }
                }
            }

            return toPos.Count == 0 ? null : new NMove(fromPos, toPos);
        }

        private NMove? GetMovesForLady(Position fromMove)
        {
            List<Position> toMoves =
            [
                // right-up
                .. GetLadyDiagonalPositionsTo(fromMove, -1, 1),
                // left-up
                .. GetLadyDiagonalPositionsTo(fromMove, -1, -1),
                // right-down
                .. GetLadyDiagonalPositionsTo(fromMove, 1, 1),
                // left-down
                .. GetLadyDiagonalPositionsTo(fromMove, 1, -1),
            ];

            return toMoves.Count == 0 ? null : new NMove(fromMove, toMoves);
        }

        private List<Position> GetLadyDiagonalPositionsTo(Position pos, int vertical, int horizontal)
        {
            int r = pos.row;
            int c = pos.col;
            List<Position> toPos = new List<Position>();
            Position newPos;

            while (true)
            {
                newPos = new Position(r += vertical, c += horizontal);

                if (!gameState.IsFieldEmpty(newPos))
                {
                    break;
                }

                toPos.Add(new Position(newPos.row, newPos.col)); 
            }

            return toPos;
        }
    }
}
