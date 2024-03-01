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

        private bool _paused;

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

                    // The top 2 rows are invisible/ not to be seen
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
                    _gameState.MoveBlockLeftInput();
                    break;
                case Key.Right or Key.D:
                    _gameState.MoveBlockRightInput();
                    break;
                case Key.Down or Key.S:
                    _gameState.MoveBlockDownInput();
                    break;
                case Key.Up or Key.W or Key.E:
                    _gameState.RotateBlockClockWiseInput();
                    break;
                case Key.Z or Key.Q:
                    _gameState.RotateBlockCounterClockWiseInput();
                    break;
                case Key.C or Key.F:
                    _gameState.HoldBlockInput();
                    break;
                case Key.Space:
                    _gameState.HardDropInput();
                    break;
                case Key.Escape or Key.P:
                    _paused = !_paused;
                    PauseOverlay.Visibility = _paused ? Visibility.Visible : Visibility.Hidden;
                    break;
                default:
                    return;
            }

            Draw();
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e) 
            => await GameLoop();

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
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
                {
                    var image = r_imageControls[r, c];
                    image.Source = Images.TileImages[_gameState.GameGrid[r, c]];
                    image.Opacity = 1.0;
                }

            var dropDistance = _gameState.HardDropDistance();
            foreach (var p in _gameState.CurrentBlock.TilePositions())
            {
                var tileImage = Images.TileImages[_gameState.CurrentBlock.ID];

                // Draw ghost block
                var ghostBlockImage = r_imageControls[p.Row + dropDistance, p.Col];
                ghostBlockImage.Opacity = .25;
                ghostBlockImage.Source = tileImage;

                // Draw current block
                var currentBlockImage = r_imageControls[p.Row, p.Col];
                currentBlockImage.Opacity = 1.0;
                currentBlockImage.Source = tileImage;
            }

            // Draw next block preview
            NextBlockImage.Source = Images.BlockPreviewImages[_gameState.NextBlock.ID];

            // Draw held block
            HeldBlockImage.Source = Images.BlockPreviewImages[
                _gameState.HeldBlock == null ? 0 : _gameState.HeldBlock.ID];

            ScoreText.Text = $"Score: {_gameState.Score}";

            LevelText.Text = $"Level: {_gameState.Level + 1}";

            ComboText.Text = _gameState.ComboChainCount > 1 ? $"Combo x{_gameState.ComboChainCount}" : "";
        }

        private async Task GameLoop()
        {
            Draw();

            while (!_gameState.GameOver)
            {
                while(_paused)
                    await Task.Delay(100);

                // 16.66 = 1000(1 sec) / 60 FPS
                await Task.Delay((int)(16.66f / GameState.SpeedCurve[_gameState.Level]));
                _gameState.MoveBlockDown();
                Draw();
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScore.Text = $"Score: {_gameState.Score}";
        }
    }
}