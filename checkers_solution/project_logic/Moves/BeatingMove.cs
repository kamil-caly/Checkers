using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_logic.Moves
{
    public class BeatingMove
    {
        private readonly GameState gameState;
        private const int rows = 8;
        private const int cols = 8;
        private List<BMove>? tempBMoves = new List<BMove>();

        public BeatingMove(GameState gameState)
        {
            this.gameState = gameState;
        }

        public IEnumerable<BMove>? GetAllLegalMoves(Player color)
        {
            List<BMove> moves = new List<BMove>();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (gameState.IsPawnHere(new Position(r, c), color))
                    {
                        List<BMove>? currPawnMoves = GetMovesForPawn(new Position(r, c), color);
                        if (currPawnMoves != null)
                        {
                            moves.AddRange(currPawnMoves);
                        }
                    }

                    //else if (gameState.IsLadyHere(new Position(r, c), color))
                    //{
                    //    BMove? currLadyMove = GetMovesForLady(new Position(r, c));
                    //    if (currLadyMove != null)
                    //    {
                    //        moves.Add(currLadyMove);
                    //    }
                    //}
                }
            }

            return moves;
        }

        private List<BMove>? GetMovesForPawn(Position pos, Player color)
        {
            List<BMove> bMoves = new List<BMove>();

            tempBMoves = new List<BMove>();

            FindMovesForPawnAlgorithm(pos, pos, new List<BeatedPeace>());

            if (tempBMoves != null)
            {
                bMoves.AddRange(tempBMoves);
                tempBMoves.Clear();

                // wybieramy tylko te ruchy, które biją najwięcej pionów
                bMoves = bMoves.OrderByDescending(b => b.BeatingPeacesPos.Count()).ToList();
                int bestOption = bMoves.First().BeatingPeacesPos.Count();
                bMoves = bMoves.Where(b => b.BeatingPeacesPos.Count() == bestOption).ToList();
            }

            return bMoves;
        }

        private void FindMovesForPawnAlgorithm(Position fromPos, Position toPos, List<BeatedPeace> beatedPieces)
        {
            // right up
            if (gameState.CanPawnBeatPeace(toPos, new Position(toPos.row - 1, toPos.col + 1)))
            {
                BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row - 1, toPos.col + 1));
                gameState.setBoardField(new Position(toPos.row - 1, toPos.col + 1), FieldContent.None);
                beatedPieces.Add(new BeatedPeace(new Position(toPos.row - 1, toPos.col + 1), beatedPeaceField));
                FindMovesForPawnAlgorithm(fromPos, new Position(toPos.row - 2, toPos.col + 2), beatedPieces);
            }

            // right down
            if (gameState.CanPawnBeatPeace(toPos, new Position(toPos.row + 1, toPos.col + 1)))
            {
                BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + 1, toPos.col + 1));
                gameState.setBoardField(new Position(toPos.row + 1, toPos.col + 1), FieldContent.None);
                beatedPieces.Add(new BeatedPeace(new Position(toPos.row + 1, toPos.col + 1), beatedPeaceField));
                FindMovesForPawnAlgorithm(fromPos, new Position(toPos.row + 2, toPos.col + 2), beatedPieces);
            }

            // left up
            if (gameState.CanPawnBeatPeace(toPos, new Position(toPos.row - 1, toPos.col - 1)))
            {
                BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row - 1, toPos.col - 1));
                gameState.setBoardField(new Position(toPos.row - 1, toPos.col - 1), FieldContent.None);
                beatedPieces.Add(new BeatedPeace(new Position(toPos.row - 1, toPos.col - 1), beatedPeaceField));
                FindMovesForPawnAlgorithm(fromPos, new Position(toPos.row - 2, toPos.col - 2), beatedPieces);
            }

            // left down
            if (gameState.CanPawnBeatPeace(toPos, new Position(toPos.row + 1, toPos.col - 1)))
            {
                BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + 1, toPos.col - 1));
                gameState.setBoardField(new Position(toPos.row + 1, toPos.col - 1), FieldContent.None);
                beatedPieces.Add(new BeatedPeace(new Position(toPos.row + 1, toPos.col - 1), beatedPeaceField));
                FindMovesForPawnAlgorithm(fromPos, new Position(toPos.row + 2, toPos.col - 2), beatedPieces);
            }

            // adding bMove to private variable
            if (beatedPieces.Count > 0)
            {
                BMove bMove = new BMove(fromPos, toPos, beatedPieces.Select(b => b.pos).ToList());

                gameState.setBoardField(
                    new Position(beatedPieces.Last().pos.row, beatedPieces.Last().pos.col),
                    beatedPieces.Last().boardField.Content, 
                    beatedPieces.Last().boardField.Player);

                beatedPieces.RemoveAt(beatedPieces.Count() - 1);

                tempBMoves!.Add(bMove);
            }
            
            return;
        }

    }
}
