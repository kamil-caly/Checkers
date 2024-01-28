﻿using System;
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
                    //if (gameState.IsPawnHere(new Position(r, c), color))
                    //{
                    //    List<BMove>? currPawnMoves = GetMovesForPawn(new Position(r, c));
                    //    if (currPawnMoves != null)
                    //    {
                    //        moves.AddRange(currPawnMoves);
                    //    }
                    //}

                    if (gameState.IsLadyHere(new Position(r, c), color))
                    {
                        List<BMove>? currLadyMove = GetMovesForLady(new Position(r, c));
                        if (currLadyMove != null)
                        {
                            moves.AddRange(currLadyMove);
                        }
                    }
                }
            }

            return moves;
        }

        private List<BMove>? GetMovesForPawn(Position pos)
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
            if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row - 1, toPos.col + 1)))
            {
                BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row - 1, toPos.col + 1));
                gameState.setBoardField(new Position(toPos.row - 1, toPos.col + 1), FieldContent.None);
                beatedPieces.Add(new BeatedPeace(new Position(toPos.row - 1, toPos.col + 1), beatedPeaceField));
                FindMovesForPawnAlgorithm(fromPos, new Position(toPos.row - 2, toPos.col + 2), beatedPieces);
            }

            // right down
            if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row + 1, toPos.col + 1)))
            {
                BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + 1, toPos.col + 1));
                gameState.setBoardField(new Position(toPos.row + 1, toPos.col + 1), FieldContent.None);
                beatedPieces.Add(new BeatedPeace(new Position(toPos.row + 1, toPos.col + 1), beatedPeaceField));
                FindMovesForPawnAlgorithm(fromPos, new Position(toPos.row + 2, toPos.col + 2), beatedPieces);
            }

            // left up
            if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row - 1, toPos.col - 1)))
            {
                BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row - 1, toPos.col - 1));
                gameState.setBoardField(new Position(toPos.row - 1, toPos.col - 1), FieldContent.None);
                beatedPieces.Add(new BeatedPeace(new Position(toPos.row - 1, toPos.col - 1), beatedPeaceField));
                FindMovesForPawnAlgorithm(fromPos, new Position(toPos.row - 2, toPos.col - 2), beatedPieces);
            }

            // left down
            if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row + 1, toPos.col - 1)))
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

        private List<BMove>? GetMovesForLady(Position pos)
        {
            List<BMove> bMoves = new List<BMove>();

            tempBMoves = new List<BMove>();

            FindMovesForLadyAlgorithm(pos, pos, new List<BeatedPeace>());

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

        private void FindMovesForLadyAlgorithm(Position fromPos, Position toPos, List<BeatedPeace> beatedPieces)
        {
            bool isDiagonalLineChecked = false;


            // right up
            int r = -1;
            int c = 1;
            while (true)
            {
                if (!gameState.IsOnBoard(new Position(toPos.row + r, toPos.col + c)))
                {
                    break;
                }

                if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row + r, toPos.col + c)))
                {
                    BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + r, toPos.col + c));
                    gameState.setBoardField(new Position(toPos.row + r, toPos.col + c), FieldContent.None);
                    beatedPieces.Add(new BeatedPeace(new Position(toPos.row + r, toPos.col + c), beatedPeaceField));

                    int rr = -1;
                    int cc = 1;
                    while (true)
                    {
                        if (!gameState.IsOnBoard(new Position(toPos.row + r + rr, toPos.col + c + cc)) ||
                            gameState.IsPeaceHere(new Position(toPos.row + r + rr, toPos.col + c + cc)))
                        {
                            isDiagonalLineChecked = true;
                            break;
                        }

                        FindMovesForLadyAlgorithm(fromPos, new Position(toPos.row + r + rr, toPos.col + c + cc), beatedPieces);

                        rr --;
                        cc ++;
                    }
                }
                
                r --;
                c ++;
            }


            // right down
            r = 1;
            c = 1;
            while (true)
            {
                if (!gameState.IsOnBoard(new Position(toPos.row + r, toPos.col + c)))
                {
                    break;
                }

                if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row + r, toPos.col + c)))
                {
                    BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + r, toPos.col + c));
                    gameState.setBoardField(new Position(toPos.row + r, toPos.col + c), FieldContent.None);
                    beatedPieces.Add(new BeatedPeace(new Position(toPos.row + r, toPos.col + c), beatedPeaceField));

                    int rr = 1;
                    int cc = 1;
                    while (true)
                    {
                        if (!gameState.IsOnBoard(new Position(toPos.row + r + rr, toPos.col + c + cc)) ||
                            gameState.IsPeaceHere(new Position(toPos.row + r + rr, toPos.col + c + cc)))
                        {
                            isDiagonalLineChecked = true;
                            break;
                        }

                        FindMovesForLadyAlgorithm(fromPos, new Position(toPos.row + r + rr, toPos.col + c + cc), beatedPieces);

                        rr++;
                        cc++;
                    }
                }

                r++;
                c++;
            }


            // left up
            r = -1;
            c = -1;
            while (true)
            {
                if (!gameState.IsOnBoard(new Position(toPos.row + r, toPos.col + c)))
                {
                    break;
                }

                if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row + r, toPos.col + c)))
                {
                    BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + r, toPos.col + c));
                    gameState.setBoardField(new Position(toPos.row + r, toPos.col + c), FieldContent.None);
                    beatedPieces.Add(new BeatedPeace(new Position(toPos.row + r, toPos.col + c), beatedPeaceField));

                    int rr = -1;
                    int cc = -1;
                    while (true)
                    {
                        if (!gameState.IsOnBoard(new Position(toPos.row + r + rr, toPos.col + c + cc)) ||
                            gameState.IsPeaceHere(new Position(toPos.row + r + rr, toPos.col + c + cc)))
                        {
                            isDiagonalLineChecked = true;
                            break;
                        }

                        FindMovesForLadyAlgorithm(fromPos, new Position(toPos.row + r + rr, toPos.col + c + cc), beatedPieces);

                        rr--;
                        cc--;
                    }
                }

                r--;
                c--;
            }


            // left down
            r = 1;
            c = -1;
            while (true)
            {
                if (!gameState.IsOnBoard(new Position(toPos.row + r, toPos.col + c)))
                {
                    break;
                }

                if (gameState.CanPeaceBeatPeace(toPos, new Position(toPos.row + r, toPos.col + c)))
                {
                    BoardField beatedPeaceField = gameState.GetBoardField(new Position(toPos.row + r, toPos.col + c));
                    gameState.setBoardField(new Position(toPos.row + r, toPos.col + c), FieldContent.None);
                    beatedPieces.Add(new BeatedPeace(new Position(toPos.row + r, toPos.col + c), beatedPeaceField));

                    int rr = 1;
                    int cc = -1;
                    while (true)
                    {
                        if (!gameState.IsOnBoard(new Position(toPos.row + r + rr, toPos.col + c + cc)) ||
                            gameState.IsPeaceHere(new Position(toPos.row + r + rr, toPos.col + c + cc)))
                        {
                            isDiagonalLineChecked = true;
                            break;
                        }

                        FindMovesForLadyAlgorithm(fromPos, new Position(toPos.row + r + rr, toPos.col + c + cc), beatedPieces);

                        rr++;
                        cc--;
                    }
                }

                r++;
                c--;
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