using project_logic;
using project_logic.Moves;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        private Image[,] _imgBoard = new Image[_rows, _cols]; 
        public MainWindow()
        { 
            InitializeComponent();
            _gameState = new GameState();
            InitBoard();
            DrawBoard();
            var test = new NormalMove(_gameState).GetAllLegalMoves(Player.White);
        }

        private void InitBoard()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    Image img = new Image();
                    PieceGrid.Children.Add(img);
                    _imgBoard[r, c] = img;
                }
            }
        }

        private void DrawBoard()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    BoardField boardField = _gameState.GetBoardField(new Position(r,c));
                    if (boardField.Player == null)
                    {
                        _imgBoard[r, c].Source = new Image().Source;
                        continue;
                    }
                        
                    ImageSource imgSource = AssetsLoader.GetImage(boardField.Content, (Player)boardField.Player);
                    _imgBoard[r, c].Source = imgSource;
                }
            }
        }
    }
}