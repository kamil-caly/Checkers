using project_logic;
using project_logic.Moves;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private readonly Color _greenTp = Color.FromArgb(150, 125, 255, 125);
        private readonly Color _yellowTp = Color.FromArgb(150, 255, 255, 0);
        private readonly GameState _gameState;
        private Image[,] _imgBoard = new Image[_rows, _cols];
        private Rectangle[,] _cacheBoard = new Rectangle[_rows, _cols];
        private readonly NormalMove _normalMove;
        private Position? _prevClickedPiece = null;
        public MainWindow()
        { 
            InitializeComponent();
            _gameState = new GameState();
            _normalMove = new NormalMove(_gameState);
            InitBoards();
            DrawBoard();
            DrawCacheBoard(new List<Position>());

            //var test = new NormalMove(_gameState).GetAllLegalMoves(Player.White);
            var test2 = new BeatingMove(_gameState).GetAllLegalMoves(Player.White);
        }

        private void InitBoards()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    Image img = new Image();
                    Rectangle rec = new Rectangle();
                    PieceGrid.Children.Add(img);
                    CacheGrid.Children.Add(rec);
                    _imgBoard[r, c] = img;
                    _cacheBoard[r, c] = rec;
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

        private void DrawCacheBoard(List<Position>? pos = null)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (pos != null && pos.Any(p => p.row == r && p.col == c))
                    {
                        _cacheBoard[r, c].Fill = new SolidColorBrush(_greenTp);
                    }
                    else
                    {
                        _cacheBoard[r, c].Fill = null;
                    }
                }
            }
        }

        private void BoardGrid_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double x = e.GetPosition(BoardGrid).X;
            double y = e.GetPosition(BoardGrid).Y;

            int clickedRow = (int)(y / (BoardGrid.ActualHeight / _rows));
            int clickedCol = (int)(x / (BoardGrid.ActualWidth / _cols));

            // wyświetlenie możliwości ruchu dla danego pionka
            if (_gameState.CurrentPlayer == Player.White && _gameState.IsWhiteHere(new Position(clickedRow, clickedCol))
                || _gameState.CurrentPlayer == Player.Black && _gameState.IsBlackHere(new Position(clickedRow, clickedCol)))
            {
                ShowMovesForClickedPiece(new Position(clickedRow, clickedCol));
            }

            // TODO: Zareagować na kliknięcie pola na którym jest zaznaczenie z moveCache
            if (_cacheBoard[clickedRow, clickedCol].Fill != null)
            {
                ExecuteMove(new Position(clickedRow, clickedCol));
            }
        }

        private void ShowMovesForClickedPiece(Position clickedField)
        {
            List<NMove>? moves = _normalMove.GetAllLegalMoves(_gameState.CurrentPlayer)?.ToList();

            if(moves != null)
            {
                var tos = moves.FirstOrDefault(m => m.From.row == clickedField.row && m.From.col == clickedField.col)?.Tos;

                if (tos != null)
                {
                    DrawCacheBoard(tos);
                    _prevClickedPiece = new Position(clickedField.row, clickedField.col);
                }
            }
        }

        private void ExecuteMove(Position clickedField)
        {
            if (_prevClickedPiece != null) 
            {
                DrawCacheBoard();
                _normalMove.MakeMove(_prevClickedPiece, clickedField);
                DrawBoard();
            }
        }
    }
}