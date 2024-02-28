using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Tetris.Source;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int ROWS = 22, COLS = 10;

        private readonly Image[,] r_imageControls;

        private GameState _gameState;

        public MainWindow()
        {
            InitializeComponent();

            _gameState = new(ROWS, COLS);

            // Setup Game Canvas
            r_imageControls = new Image[ROWS, COLS];
            int cellSize = (int)(GameCanvas.Width / COLS); 
            for (var r = 0; r < ROWS; r++)
                for (var c = 0; c < COLS; c++)
                {
                    var imageControl = new Image()
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    // The top 2 rows are invisible/ not visible
                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    r_imageControls[r, c] = imageControl;
                }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_gameState.GameOver)
                return;

            switch (e.Key)
            {
                case Key.Left or Key.A:
                    _gameState.MoveBlockLeft();
                    break;
                case Key.Right or Key.D:
                    _gameState.MoveBlockRight();
                    break;
                case Key.Down or Key.S:
                    _gameState.MoveBlockDown();
                    break;
                case Key.Up or Key.W or Key.E:
                    _gameState.RotateBlockClockWise();
                    break;
                case Key.Z or Key.Q:
                    _gameState.RotateBlockCounterClockWise();
                    break;
                default:
                    return;
            }

            Draw();
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void Playagain_Click(object sender, RoutedEventArgs e)
        {
            _gameState = new(ROWS, COLS);
            GameOverMenu.Visibility = Visibility.Hidden;

            await GameLoop();
        }

        private void Draw()
        {
            // Draw grid
            for (var r = 0; r < ROWS; r++)
                for (var c = 0; c < COLS; c++)
                    r_imageControls[r, c].Source = Images.TileImages[_gameState.GameGrid[r, c]];

            // Draw block
            foreach (var p in _gameState.CurrentBlock.TilePositions())
                r_imageControls[p.Row, p.Col].Source = Images.TileImages[_gameState.CurrentBlock.ID];

            // Draw next block preview
            NextBlockImage.Source = Images.BlockPreviewImages[_gameState.NextBlock.ID];
        }

        private async Task GameLoop()
        {
            Draw();

            while (!_gameState.GameOver)
            {
                // TODO await lerp according to score
                await Task.Delay(500);
                _gameState.MoveBlockDown();
                Draw();
            }

            GameOverMenu.Visibility = Visibility.Visible;
        }
    }
}