﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            { GridValue.Food, Images.Apple },
        };

        private readonly Dictionary<GridCoordinate, int> r_directionToRotation = new()
        {
            { GridCoordinate.Up, 0 },
            { GridCoordinate.Right, 90 },
            { GridCoordinate.Down, 180 },
            { GridCoordinate.Left, 270 },
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
                    var delay = (int)(128f + (32f - 128f) * (_gameState.Score / 64f));
                    if (delay < 32)
                        delay = 32;
                    await Task.Delay(delay);
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
                Overlay.Visibility = Visibility.Visible;
                OverlayText.Text = "PRESS ANY KEY TO START";

                // New Game
                _gameState = new(r_rows, r_cols);

                _gameRunning = false;
            }

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
                var image = r_gridImages[snakeHeadPos.Y, snakeHeadPos.X];
                image.Source = Images.Head;

                var degrees = r_directionToRotation[_gameState.SnakeDirection];
                image.RenderTransform = new RotateTransform(degrees);

                ScoreText.Text = $"SCORE {_gameState.Score}";
            }

            async Task PlacePart(GridCoordinate pos, BitmapImage source)
            {
                r_gridImages[pos.Y, pos.X].Source = source;
                await Task.Delay(50);
            }
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
    }
}