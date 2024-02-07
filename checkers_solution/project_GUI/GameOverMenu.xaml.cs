using project_logic;
using project_logic.GameOver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project_GUI
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public Action<GameOverAction>? ClickedAction; 
        public GameOverMenu(GameOverReason reason, Player? winner)
        {
            InitializeComponent();
            ReasonText.Text = GetReasonText(reason, winner);
        }

        private string GetReasonText(GameOverReason reason, Player? winner) =>
            reason switch
            {
                GameOverReason.CapturedPieces => $"{winner.ToString()} win! All pieces have been captured.",
                GameOverReason.CannotMovePieces => 
                    $"{winner.ToString()} win! " +
                    $"{(winner == Player.Black ? Player.White.ToString() : Player.Black.ToString())} " +
                    "cannot move.",
                GameOverReason.FifteenLadiesMoves => "It is a draw! Neither player has made a capture for 15 moves.",
                _ => ""
            };

        private void Restart_Btn_Click(object sender, RoutedEventArgs e)
        {
            ClickedAction?.Invoke(GameOverAction.Restart);
        }

        private void Quit_Btn_Click(object sender, RoutedEventArgs e)
        {
            ClickedAction?.Invoke(GameOverAction.Quit);
        }
    }
}
