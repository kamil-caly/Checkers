using project_logic;
using project_logic.GameOver;
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
        private readonly Color _greenTp = Color.FromArgb(150, 125, 255, 125); // normal move
        private readonly Color _greenTpLight = Color.FromArgb(100, 125, 255, 125); // normal move mark
        private readonly Color _yellowTp = Color.FromArgb(150, 255, 255, 0); // beating move
        private readonly Color _yellowTpLight = Color.FromArgb(100, 255, 255, 0); // beating move mark
        private GameState _gameState;
        private Image[,] _imgBoard = new Image[_rows, _cols];
        private Rectangle[,] _cacheBoard = new Rectangle[_rows, _cols];
        private NormalMove _normalMove;
        private BeatingMove _beatingMove;
        private Position? _prevClickedPiece = null;
        public MainWindow()
        { 
            InitializeComponent();
            _gameState = new GameState();
            _normalMove = new NormalMove(_gameState);
            _beatingMove = new BeatingMove(_gameState);
            InitBoards();
            DrawBoard();
            DrawCacheBoard(new List<Position>());
            SetCursor();
        }

        private void InitBoards()
        {
            PieceGrid.Children.Clear();
            CacheGrid.Children.Clear();

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

        private void DrawCacheBoard(List<Position>? tos = null, Position? from = null, bool isBeatingMove = false)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (tos != null && tos.Any(p => p.row == r && p.col == c))
                    {
                        _cacheBoard[r, c].Fill = new SolidColorBrush(isBeatingMove ? _yellowTp : _greenTp);
                    }
                    else if (from != null && from.row == r && from.col == c)
                    {
                        _cacheBoard[r, c].Fill = new SolidColorBrush(isBeatingMove ? _yellowTpLight : _greenTpLight);
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
            if (Menu.Content != null)
            {
                return;
            }

            double x = e.GetPosition(BoardGrid).X;
            double y = e.GetPosition(BoardGrid).Y;

            int clickedRow = (int)(y / (BoardGrid.ActualHeight / _rows));
            int clickedCol = (int)(x / (BoardGrid.ActualWidth / _cols));

            // wyświetlenie możliwości ruchu dla danego pionka
            if (_gameState.CurrentPlayer == Player.White && _gameState.IsWhiteHere(new Position(clickedRow, clickedCol))
                || _gameState.CurrentPlayer == Player.Black && _gameState.IsBlackHere(new Position(clickedRow, clickedCol)))
            {
                ShowMovesForClickedPiece(new Position(clickedRow, clickedCol));
                return;
            }

            if (_cacheBoard[clickedRow, clickedCol].Fill != null)
            {
                BMove? beatingMove = null;

                if (!_gameState.IsFieldEmpty(new Position(_prevClickedPiece!.row, _prevClickedPiece.col)))
                {
                    beatingMove = _beatingMove.GetLegalMoveForPeace(
                    new Position(_prevClickedPiece!.row, _prevClickedPiece.col),
                    new Position(clickedRow, clickedCol),
                    (Player)_gameState.GetBoardField(new Position(_prevClickedPiece!.row, _prevClickedPiece.col)).Player!);
                }

                if (beatingMove != null)
                {
                    ExecuteBMove(beatingMove);
                }
                else
                {
                    ExecuteNMove(new Position(clickedRow, clickedCol));
                }

                if (!IsAnyMoveForPlayer(_gameState.GetNextPlayer()))
                {
                    ShowGameOverMenu(GameOverReason.CannotMovePieces, _gameState.CurrentPlayer);
                }
                else if (!IsAnyMoveForPlayer(_gameState.CurrentPlayer))
                {
                    ShowGameOverMenu(GameOverReason.CannotMovePieces, _gameState.GetNextPlayer());
                }
                else
                {
                    SetCursor();
                }
            }
        }

        private bool IsAnyMoveForPlayer(Player player)
        {
            // Podczas pobierania ruchów z jakiegoś powodu znika pion, który
            // stoi na diagonali damki, ale tylko w określonych ustawieniu figur.
            // Jako naprawę przed sprawdzeniem zapisujemy kopię planszy i przywracamy
            // po sprawdzeniu.

            BoardField[,] copyBoard = _gameState.Copy();

            List<BMove> bMoves = _beatingMove.GetAllLegalMoves(player).ToList();
            List<NMove> nMoves = _normalMove.GetAllLegalMoves(player).ToList();

            _gameState.UseBoard(copyBoard);

            return bMoves.Count() > 0 || nMoves.Count() > 0;
        }

        private void ShowMovesForClickedPiece(Position clickedField)
        {
            List<BMove> bMoves = _beatingMove.GetAllLegalMoves(_gameState.CurrentPlayer).ToList();

            if (bMoves.Count > 0)
            {
                var tos = bMoves
                    .Where(m => m.From.row == clickedField.row && m.From.col == clickedField.col)
                    .Select(b => b.To)
                    .ToList();


                if (tos.Count > 0)
                {
                    DrawCacheBoard(tos, new Position(clickedField.row, clickedField.col), true);
                    _prevClickedPiece = new Position(clickedField.row, clickedField.col);
                }

                return;
            }

            List<NMove> moves = _normalMove.GetAllLegalMoves(_gameState.CurrentPlayer).ToList();

            if (moves.Count() > 0)
            {
                var tos = moves
                    .FirstOrDefault(m => m.From.row == clickedField.row && m.From.col == clickedField.col)?
                    .Tos;

                if (tos?.Count > 0)
                {
                    DrawCacheBoard(tos, new Position(clickedField.row, clickedField.col));
                    _prevClickedPiece = new Position(clickedField.row, clickedField.col);
                }
            }
        }

        private void ExecuteNMove(Position clickedField)
        {
            if (_prevClickedPiece != null) 
            {
                DrawCacheBoard();
                _normalMove.MakeMove(_prevClickedPiece, clickedField);
                DrawBoard();
                if (GameOverManager.IsGameOverFMR())
                {
                    ShowGameOverMenu(GameOverReason.FifteenLadiesMoves, null);
                }
            }
        }

        private void ExecuteBMove(BMove bMove)
        {
            if (_prevClickedPiece != null)
            {
                DrawCacheBoard();
                _beatingMove.MakeMove(bMove);
                DrawBoard();
                if (GameOverManager.IsGameOver(_gameState))
                {
                    ShowGameOverMenu(GameOverReason.CapturedPieces, _gameState.GetNextPlayer());
                }
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                if (Menu.Content != null)
                {
                    Menu.Content = null;
                }
                else
                {
                    PauseMenu pauseMenu = new PauseMenu();
                    Menu.Content = pauseMenu;

                    pauseMenu.ClickedAction += option =>
                    {
                        if (option == PauseAction.Continue)
                        {
                            PauseMenu_ContinueClicked();
                            return;
                        }

                        QuitClicked();
                    };
                }
            }
        }

        private void PauseMenu_ContinueClicked()
        {
            if (Menu.Content != null)
            {
                Menu.Content = null;
            }
        }

        private void QuitClicked()
        {
            Application.Current.Shutdown();
        }

        private void ShowGameOverMenu(GameOverReason reason, Player? winner)
        {   
            if (Menu.Content != null)
            {
                return;
            }

            GameOverMenu gameOverMenu = new GameOverMenu(reason, winner);
            Menu.Content = gameOverMenu;

            gameOverMenu.ClickedAction += option =>
            {
                if (option == GameOverAction.Restart)
                {
                    RestartGame();
                    return;
                }

                QuitClicked();
            };
        }

        private void RestartGame()
        {
            Menu.Content = null;
            _gameState = new GameState();
            _normalMove = new NormalMove(_gameState);
            _beatingMove = new BeatingMove(_gameState);
            InitBoards();
            DrawBoard();
            DrawCacheBoard(new List<Position>());
            SetCursor();
        }

        private void SetCursor()
        {
            Cursor = MouseCursor.GetCursor(_gameState.CurrentPlayer);
        }
    }
}