using project_logic;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int _rows = 8;
        private const int _cols = 8;
        private readonly GameState _gameState;
        public MainWindow()
        { 
            InitializeComponent();
            _gameState = new GameState();
            DrawBoard();
        }

        private void DrawBoard()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    Image img = new Image();
                    PieceGrid.Children.Add(img);

                    BoardField boardField = _gameState.GetBoardField(r, c);
                    if (boardField.Player == null)
                        continue;

                    ImageSource imgSource = AssetsLoader.GetImage(boardField.Content, (Player)boardField.Player);
                    img.Source = imgSource;
                }
            }
        }
    }
}