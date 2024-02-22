using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Snake.Source;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, BitmapSource> r_gridValToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.Snake, Images.Body },
            { GridValue.Food, Images.Food },
        };

        private readonly int r_rows = 16, r_cols = 16;
        private readonly Image[,] r_gridImages;

        private GameState _gameState;

        private bool _gameRunning;

        public MainWindow()
        {
            InitializeComponent();

            r_gridImages = SetUpGrid();

            _gameState = new(r_rows, r_cols);
        }

        private async Task GameLoop()
        {
            while (!_gameState.GameOver)
            {
                // TODO: Make the delay speed up with the the score (lerp)
                await Task.Delay(128);
                _gameState.Move();
                Draw();
            }
        }

        private async Task RunGame()
        {
            Draw();

            await ShowCountDown();

            await GameLoop();

            await ShowGameOver();

            _gameState = new(r_rows, r_cols);
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!_gameRunning)
            {
                _gameRunning = true;
                await RunGame();
                _gameRunning = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_gameState.GameOver)
                return;

            _gameState.ChangeDirection(e.Key switch
            {
                Key.A or Key.Left => Direction.Left,
                Key.D or Key.Right => Direction.Right,
                Key.W or Key.Up => Direction.Up,
                _ or Key.S or Key.Down => Direction.Down
            });
        }


        private Image[,] SetUpGrid()
        {
            var images = new Image[r_rows, r_cols];
            GameGrid.Rows = r_rows;
            GameGrid.Columns = r_cols;
            for (var r = 0; r < r_rows; r++)
                for (var c = 0; c < r_cols; c++)
                {
                    var image = new Image() { Source = Images.Empty };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }

            return images;
        }

        private void DrawGrid()
        {
            for (var r = 0; r < r_rows; r++)
                for (var c = 0;c < r_cols; c++)
                {
                    var gridValue = _gameState.Grid[r, c];
                    r_gridImages[r, c].Source = r_gridValToImage[gridValue];
                }
        }

        private void Draw()
        {
            DrawGrid();
            ScoreText.Text = $"SCORE {_gameState.Score}";
        }

        private async Task ShowCountDown()
        {
            for (var i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }

            Overlay.Visibility = Visibility.Hidden;
        }

        private async Task ShowGameOver()
        {
            await Task.Delay(500);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }
    }
}