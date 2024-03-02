using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Snake.Source;
using Colors = System.Windows.Media.Colors;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, BitmapSource> r_gridValToImage = new()
        {
            { GridValue.Food, Images.Empty },
            { GridValue.Empty, Images.Empty },
            { GridValue.Snake, Images.Body }
        };

        private readonly Dictionary<GridCoordinate, int> r_directionToRotation = new()
        {
            { GridCoordinate.Up, 0 },
            { GridCoordinate.Right, 90 },
            { GridCoordinate.Down, 180 },
            { GridCoordinate.Left, 270 }
        };

        private readonly int r_rows = 16, r_cols = 16;
        private readonly Image[,] r_gridImages;

        private GameState _gameState;

        private bool _gameRunning;

        public MainWindow()
        {
            InitializeComponent();

            // Set up grid
            r_gridImages = new Image[r_rows, r_cols];
            GameGrid.Rows = r_rows;
            GameGrid.Columns = r_cols;

            GameGrid.Width = GameGrid.Height * (r_cols / (double)r_rows);

            for (var r = 0; r < r_rows; r++)
                for (var c = 0; c < r_cols; c++)
                {
                    var image = new Image()
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new(0.5, 0.5),
                    };
                    GameGrid.Children.Add(image);
                    r_gridImages[r, c] = image;
                }

            string[] names = ["Easy", "Medium", "Hard"];
            SolidColorBrush[] colors = [
                new(Colors.LightGreen), 
                new(new Color() { R = 190, G = 177, B = 58, A = 255 }), 
                new(Colors.IndianRed)];
            for (var i = 0; i < 3; i++)
            {
                var b = new Button()
                {
                    Name = $"Name_{i}",
                    Content = names[i],
                    FontWeight = FontWeights.DemiBold,
                    FontSize = 18,
                    Margin = new(32.0, 16.0, 32.0, 16.0),
                    BorderThickness = new(4.0),
                    BorderBrush = new SolidColorBrush(new Color{ R = 51, G = 31, B = 80, A = 255}),
                    Width = 96.0,
                    Height = 48.0,
                    Foreground = colors[i],
                    Background = new SolidColorBrush(new Color() { R = 25, G = 18, B = 64, A = 127}),
                };
                var id = i;
                b.MouseEnter += (o, m) =>
                { 
                    b.Foreground = new SolidColorBrush(Colors.Black);
                    b.Background = colors[id];
                };
                b.MouseLeave += (o, m) =>
                {
                    b.Foreground = colors[id];
                    b.Background = new SolidColorBrush(new Color() { R = 25, G = 18, B = 64, A = 127});
                };
                b.Click += async (o, _) => await GameLoop(b.Name[^1] - '0');
                StartButtons.Children.Add(b);
            }

            _gameState = new(r_rows, r_cols);
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_gameRunning)
                return;

            int difficulty;
            switch (e.Key)
            {
                case Key.D1:
                    difficulty = 0;
                    break;
                case Key.D2:
                    difficulty = 1;
                    break;
                case Key.D3:
                    difficulty = 2;
                    break;
                default:
                    return;
            }

            if (Overlay.Visibility == Visibility.Visible)
                e.Handled = true;

            await GameLoop(difficulty);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_gameState.GameOver)
                return;

            _gameState.ChangeDirection(e.Key switch
            {
                Key.A or Key.Left => GridCoordinate.Left,
                Key.D or Key.Right => GridCoordinate.Right,
                Key.W or Key.Up => GridCoordinate.Up,
                _ or Key.S or Key.Down => GridCoordinate.Down
            });
        }

        private async Task GameLoop(int difficultyIndex)
        {
            _gameRunning = true;

            Difficulty difficulty = difficultyIndex switch
            {
                0 => new(64, 192f, 1536f),
                1 => new(32, 128f, 1024f),
                _ => new(16, 64f, 512f)
            };

            StartButtons.Visibility = Visibility.Hidden;
            // Run game

            Draw();

            // Count down
            for (var i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }

            Overlay.Visibility = Visibility.Hidden;

            // Game Loop
            while (!_gameState.GameOver)
            {
                await Task.Delay(difficulty.Delay(_gameState.Score));

                _gameState.Move();

                Draw();
            }

            // Show game Over

            // Draw Snake dead body
            var positions = _gameState.SnakePositions().ToArray();

            await PlacePart(positions[0], Images.HeadDead);
            for (var i = 1; i < positions.Length; i++)
                await PlacePart(positions[i], Images.BodyDead);

            // Replay
            await Task.Delay(333);
            OverlayText.Text = "CHOOSE A LEVEL";
            Overlay.Visibility = Visibility.Visible;
            StartButtons.Visibility = Visibility.Visible;

            // New Game
            _gameState = new(r_rows, r_cols);

            _gameRunning = false;

            void Draw()
            {
                // Draw Grid
                for (var r = 0; r < r_rows; r++)
                    for (var c = 0; c < r_cols; c++)
                    {
                        var gridValue = _gameState.Grid[r, c];
                        r_gridImages[r, c].Source = r_gridValToImage[gridValue];
                        r_gridImages[r, c].RenderTransform = Transform.Identity;
                    }

                // Draw Snake Head
                var snakeHeadPos = _gameState.HeadPosition();
                var snakeHeadImage = r_gridImages[snakeHeadPos.Row, snakeHeadPos.Col];
                snakeHeadImage.Source = Images.Head;

                // Rotate snake head
                var degrees = r_directionToRotation[_gameState.SnakeDirection];
                snakeHeadImage.RenderTransform = new RotateTransform(degrees);

                // Draw Food
                var foodPos = _gameState.Food.Position;
                var foodImage = r_gridImages[foodPos.Row, foodPos.Col];
                foodImage.Source = _gameState.Food.Image;

                ScoreText.Text = $"SCORE {_gameState.Score}";
            }

            async Task PlacePart(GridCoordinate pos, BitmapImage source)
            {
                r_gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }

        private readonly struct Difficulty(int minDelay, float maxDelay, float scoreToMin)
        {
            public readonly int Delay(int score)
            {
                var delay = (int)(maxDelay + (minDelay - maxDelay) * (score / scoreToMin));
                if (delay < minDelay)
                    return minDelay;

                return delay;
            }
        };
    }
}