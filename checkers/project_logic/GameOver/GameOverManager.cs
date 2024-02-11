using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_logic.GameOver
{
    public static class GameOverManager
    {
        private static int WhiteLadyAnotherMoveInARow { get; set; } = 0;
        private static int DarkLadyAnotherMoveInARow { get; set; } = 0;
        public static bool IsGameOver(GameState gameState)
        {
            return !gameState.IsPlayerOnBoard(gameState.CurrentPlayer);
        }

        public static bool IsGameOverFMR()
        {
            return WhiteLadyAnotherMoveInARow >= 15 && DarkLadyAnotherMoveInARow >= 15;
        }

        public static void ResetLadyMoves(Player color)
        {
            if (color == Player.White)
            {
                WhiteLadyAnotherMoveInARow = 0;
                return;
            }

            DarkLadyAnotherMoveInARow = 0;
        }

        public static void IncrementLadyMoves(Player color)
        {
            if (color == Player.White)
            {
                WhiteLadyAnotherMoveInARow += 1;
                return;
            }

            DarkLadyAnotherMoveInARow += 1;
        }
    }
}
