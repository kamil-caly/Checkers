﻿using project_logic.Moves;
using System.Drawing;

namespace project_logic
{
    public class GameState
    {
        private const int rows = 8;
        private const int cols = 8;
        private BoardField [,] GameBoard;
        public Player CurrentPlayer { get; private set; }
        public GameState()
        {
            CurrentPlayer = Player.White;
            GameBoard = InitBoard();
        }

        private BoardField[,] InitBoard()
        {
            GameBoard = new BoardField[rows, cols];

            //setBoardField(new Position(6, 3), FieldContent.Lady, Player.White);

            //setBoardField(new Position(2, 3), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(1, 4), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(4, 1), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(1, 6), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(6, 1), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(1, 2), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(4, 5), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(1, 4), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(2, 1), FieldContent.Pawn, Player.Black);
            //setBoardField(new Position(3, 6), FieldContent.Pawn, Player.Black);

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

        public BoardField[,] Copy()
        {
            BoardField[,] copyBoard = new BoardField[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    copyBoard[r, c] = new BoardField(GetBoardField(new Position(r, c)).Content,
                        GetBoardField(new Position(r, c)).Player);
                }
            }

            return copyBoard;
        }

        public void UseBoard(BoardField[,] newBoard)
        {
            GameBoard = newBoard;
        }

        public BoardField GetBoardField(Position pos)
        {
            return GameBoard[pos.row, pos.col];
        }

        public void setBoardField(Position pos, FieldContent content, Player? player = null)
        {
            GameBoard[pos.row, pos.col] = new BoardField(content, player);
        }

        public Player GetNextPlayer()
        {
            return CurrentPlayer == Player.White ? Player.Black : Player.White;
        }

        public Player GetOppositeColor(Player color)
        {
            return color == Player.White ? Player.Black : Player.White;
        }

        public bool IsFieldEmpty(Position pos)
        {
            return IsOnBoard(pos) && GetBoardField(pos).Content == FieldContent.None;
        }

        public bool IsOnBoard(Position pos)
        {
            return pos.row <= 7 && pos.row >= 0 && pos.col <= 7 && pos.col >= 0;
        }

        public bool IsPeaceHere(Position pos)
        {
            return IsOnBoard(pos) && GetBoardField(pos).Content != FieldContent.None;
        }

        public bool IsPeaceHere(Position pos, Player color)
        {
            return !IsFieldEmpty(pos) &&
                GetBoardField(pos).Player == color;
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

        public bool IsPawnHere(Position pos)
        {
            return !IsFieldEmpty(pos) &&
                GetBoardField(pos).Content == FieldContent.Pawn;
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

        public bool IsPlayerOnPromotionLine(Position pos)
        {
            if (CurrentPlayer == Player.White)
            {
                return pos.row == 0;
            }

            return pos.row == 7;
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player.White ? Player.Black : Player.White;
        }

        public bool CanPeaceBeatPeace(Position from, Position peaceToBeat, Player currPlayer, bool LadyBeats = false)
        {
            if (!IsPeaceHere(peaceToBeat))
            {
                return false;
            }

            int fromRow = from.row; //2
            int fromCol = from.col; //1

            int toRow = peaceToBeat.row; //5
            int toCol = peaceToBeat.col; //4

            int vValue = toRow > fromRow ? 1 : -1; //1
            int hValue = toCol > fromCol ? 1 : -1; //1

            if (currPlayer == Player.White ? IsWhiteHere(peaceToBeat) : IsBlackHere(peaceToBeat))
            {
                return false;
            }

            // dodatkowe sprawdzenie gdy damka
            if (LadyBeats) 
            {
                if (IsPeaceHere(new Position(toRow - vValue, toCol - hValue), GetOppositeColor(currPlayer)))
                {
                    return false;
                }
            }

            return IsFieldEmpty(new Position(toRow + vValue, toCol + hValue));
        }

        public bool IsPlayerOnBoard(Player color)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (IsPeaceHere(new Position(r, c), color))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
