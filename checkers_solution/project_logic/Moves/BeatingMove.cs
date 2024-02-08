using project_logic.GameOver;
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

        public IEnumerable<BMove> GetAllLegalMoves(Player color)
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
                    else if (gameState.IsLadyHere(new Position(r, c), color))
                    {
                        List<BMove>? currLadyMove = GetMovesForLady(new Position(r, c), color);
                        if (currLadyMove != null)
                        {
                            moves.AddRange(currLadyMove);
                        }
                    }
                }
            }

            return moves;
        }

        public BMove? GetLegalMoveForPeace(Position from, Position to, Player color)
        {
            if (gameState.IsPawnHere(new Position(from.row, from.col), color))
            {
                List<BMove>? currPawnMoves = GetMovesForPawn(new Position(from.row, from.col), color);
                if (currPawnMoves != null)
                {
                    return currPawnMoves.Where(b => b.To.row == to.row && b.To.col == to.col).FirstOrDefault();
                }
            }
            else if (gameState.IsLadyHere(new Position(from.row, from.col), color))
            {
                List<BMove>? currLadyMove = GetMovesForLady(new Position(from.row, from.col), color);
                if (currLadyMove != null)
                {
                    return currLadyMove.Where(b => b.To.row == to.row && b.To.col == to.col).FirstOrDefault();
                }
            }

            return null;
        }

        public void MakeMove(BMove bMove)
        {
            BoardField movedPiece = gameState.GetBoardField(new Position(bMove.From.row, bMove.From.col));

            if (movedPiece.Content == FieldContent.Lady)
            {
                GameOverManager.ResetLadyMoves((Player)movedPiece.Player!);
            }

            gameState.setBoardField(new Position(bMove.From.row, bMove.From.col), FieldContent.None);

            foreach (var peace in bMove.BeatingPeacesPos)
            {
                gameState.setBoardField(new Position(peace.row, peace.col), FieldContent.None);
            }

            gameState.setBoardField(new Position(bMove.To.row, bMove.To.col), movedPiece.Content, movedPiece.Player);

            if (gameState.IsPlayerOnPromotionLine(bMove.To) && gameState.IsPawnHere(bMove.To))
            {
                ReplaceInLady(bMove.To);
            }

            gameState.SwitchPlayer();
        }

        private void ReplaceInLady(Position pos)
        {
            Player color = gameState.IsWhiteHere(pos) ? Player.White : Player.Black;
            gameState.setBoardField(new Position(pos.row, pos.col), FieldContent.Lady, color);
        }


        private List<BMove>? GetMovesForPawn(Position pos, Player color)
        {
            List<BMove> bMoves = new List<BMove>();

            tempBMoves = new List<BMove>();

            FindMovesForPawnAlgorithm(pos, pos, new List<BeatedPeace>(), color);

            if (tempBMoves.Count() > 0)
            {
                bMoves.AddRange(tempBMoves);
                tempBMoves.Clear();

                // wybieramy tylko te ruchy, które biją najwięcej pionów
                bMoves = bMoves.OrderByDescending(b => b.BeatingPeacesPos.Count()).ToList();
                int bestOption = bMoves.First().BeatingPeacesPos.Count();
                bMoves = bMoves.Where(b => b.BeatingPeacesPos.Count() == bestOption).ToList();

                // w przypadku ruchu na to samo pole co najmniej dwiema różnymi drogami, wybierana jest losowo jedna z nich
                Random rand = new Random();
                bMoves = bMoves.OrderBy(x => rand.Next()).ToList();
                bMoves = bMoves.GroupBy(b => new { b.To.row, b.To.col }).Select(g => g.First()).ToList();

            }

            return bMoves;
        }

        private void FindMovesForPawnAlgorithm(Position fromPos, Position toPos, List<BeatedPeace> beatedPieces, Player color)
        {
            // right up ->   -1, 1
            // right down -> 1, 1
            // left up ->    -1, -1
            // left down ->  1, -1
            for (int v = -1; v < 2; v += 2)
            {
                for (int h = -1; h < 2; h += 2)
                {
                    if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row + v, toPos.col + h), color))
                    {
                        BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + v, toPos.col + h));
                        gameState.setBoardField(new Position(toPos.row + v, toPos.col + h), FieldContent.None);
                        beatedPieces.Add(new BeatedPeace(new Position(toPos.row + v, toPos.col + h), beatedPeaceField));
                        FindMovesForPawnAlgorithm(fromPos, new Position(toPos.row + (2 * v), toPos.col + (2 * h)), beatedPieces, color);
                    }
                }
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

        private List<BMove>? GetMovesForLady(Position pos, Player color)
        {
            List<BMove> bMoves = new List<BMove>();

            tempBMoves = new List<BMove>();

            FindMovesForLadyAlgorithm(pos, pos, new List<BeatedPeace>(), color);

            if (tempBMoves.Count() > 0)
            {
                bMoves.AddRange(tempBMoves);
                tempBMoves.Clear();

                // wybieramy tylko te ruchy, które biją najwięcej pionów
                bMoves = bMoves.OrderByDescending(b => b.BeatingPeacesPos.Count()).ToList();
                int bestOption = bMoves.First().BeatingPeacesPos.Count();
                bMoves = bMoves.Where(b => b.BeatingPeacesPos.Count() == bestOption).ToList();

                // w przypadku ruchu na to samo pole co najmniej dwiema różnymi drogami, wybierana jest losowo jedna z nich
                Random rand = new Random();
                bMoves = bMoves.OrderBy(x => rand.Next()).ToList();
                bMoves = bMoves.GroupBy(b => new { b.To.row, b.To.col }).Select(g => g.First()).ToList();
            }

            return bMoves;
        }

        private void FindMovesForLadyAlgorithm(Position fromPos, Position toPos, List<BeatedPeace> beatedPieces, Player color)
        {
            bool isDiagonalLineChecked = false;

            // right up ->   -1, 1
            // right down -> 1, 1
            // left up ->    -1, -1
            // left down ->  1, -1
            for (int v = -1; v < 2; v += 2)
            {
                for (int h = -1; h < 2; h += 2)
                {
                    int r = v;
                    int c = h;
                    while (true)
                    {
                        if (!gameState.IsOnBoard(new Position(toPos.row + r, toPos.col + c)))
                        {
                            break;
                        }

                        if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row + r, toPos.col + c), color))
                        {
                            BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + r, toPos.col + c));
                            gameState.setBoardField(new Position(toPos.row + r, toPos.col + c), FieldContent.None);
                            beatedPieces.Add(new BeatedPeace(new Position(toPos.row + r, toPos.col + c), beatedPeaceField));

                            int rr = v;
                            int cc = h;
                            while (true)
                            {
                                if (!gameState.IsOnBoard(new Position(toPos.row + r + rr, toPos.col + c + cc)) ||
                                    gameState.IsPeaceHere(new Position(toPos.row + r + rr, toPos.col + c + cc)))
                                {
                                    isDiagonalLineChecked = true;
                                    break;
                                }

                                FindMovesForLadyAlgorithm(fromPos, new Position(toPos.row + r + rr, toPos.col + c + cc), beatedPieces, color);

                                rr += v;
                                cc += h;
                            }
                        }

                        r += v;
                        c += h;
                    }
                }
            }


            if (beatedPieces.Count > 0)
            {
                if (isDiagonalLineChecked) 
                {
                    gameState.setBoardField(
                    new Position(beatedPieces.Last().pos.row, beatedPieces.Last().pos.col),
                    beatedPieces.Last().boardField.Content,
                    beatedPieces.Last().boardField.Player);

                    beatedPieces.RemoveAt(beatedPieces.Count() - 1);
                }
                else
                {
                    // adding bMove to private variable
                    BMove bMove = new BMove(fromPos, toPos, beatedPieces.Select(b => b.pos).ToList());
                    tempBMoves!.Add(bMove);
                }
            }

            return;
        }

    }
}
