﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tic_Tac_Toe.Source;
using Tic_Tac_Toe.Source.Enums;

namespace Tic_Tac_Toe;

public partial class MainWindow : Window
{
    private static readonly Dictionary<Player, SolidColorBrush> sr_playerToColour = new(2);

    private static readonly DoubleAnimation sr_fadeOutAnimation = new(1.0, 0.0, TimeSpan.FromSeconds(0.33));
    private static readonly DoubleAnimation sr_fadeInAnimation = new(0.0, 1.0, TimeSpan.FromSeconds(0.33));

    private readonly Image[,] r_imageControls = new Image[3, 3];

    private readonly GameState r_gameState = new();

    public MainWindow()
    {
        InitializeComponent();

        var app = Application.Current as App;
        app.InitializeComponent();

        sr_playerToColour.Add(Player.X, app.Resources["XColour"] as SolidColorBrush);
        sr_playerToColour.Add(Player.O, app.Resources["OColour"] as SolidColorBrush);

        r_gameState.MoveMade += OnMoveMade;
        r_gameState.GameEnded += OnGameEnded;
        r_gameState.GameRestarted += OnGameRestarted;

        // Set up GameGrid
        var thickness = new Thickness(18.0);
        for (var r = 0; r < 3; r++)
            for (var c = 0; c < 3; c++)
            {
                var playerImage = new Image()
                { 
                    Margin = thickness,
                };

                r_imageControls[r, c] = playerImage;
                GameGrid.Children.Add(playerImage);

                GameGridSquare.Children.Add(new Image()
                {
                    Margin = thickness,
                    Source = Images.Square,
                    IsHitTestVisible = false,
                });
            }
    }

    #region GameState Events
    private void OnMoveMade(SquareCoordinate square)
    {
        var prevPlayer = r_gameState.GameGrid[square.Row, square.Col];
        r_imageControls[square.Row, square.Col]
            .BeginAnimation(Image.SourceProperty, Images.PlayerAnimations[prevPlayer]);

        SetPlayerPanel(PlayerText, 
            PlayerImage,
            Images.PlayerCompleteImages[r_gameState.CurrentPlayer],
            r_gameState.CurrentPlayer, 
            prevPlayer);
    }
    
    private async void OnGameEnded(GameResult gameResult)
    {
        PlayAgainButton.Foreground = sr_playerToColour[r_gameState.CurrentPlayer];
        PlayAgainButton.BorderBrush = sr_playerToColour[r_gameState.OppositePlayer];

        await Task.Delay(500);

        // Trasition to end screen
        if (gameResult.Winner != Player.None)
        {
            // Draw LineStreak stroke
            var squareSize = GameGrid.Width / 3.0;
            var margin = squareSize * .5;
            const double GRID_OFFSET = 42;
            var left = 0.0 + GRID_OFFSET;
            var right = GameGrid.Width - GRID_OFFSET;
            var top = 0.0 + GRID_OFFSET;
            var bot = GameGrid.Height - GRID_OFFSET;
            double endPointX, endPointY;
            // Find LineStreak points
            switch (gameResult.WinInfo.WinType)
            {
                case WinType.Row:
                    var y = gameResult.WinInfo.Number * squareSize + margin;
                    LineStreak.X1 = left;
                    LineStreak.Y1 = y;
                    endPointX = right;
                    endPointY = y;
                    break;
                case WinType.Column:
                    var x = gameResult.WinInfo.Number * squareSize + margin;
                    LineStreak.X1 = x;
                    LineStreak.Y1 = top;
                    endPointX = x;
                    endPointY = bot;
                    break;
                case WinType.MainDiagonal:
                    LineStreak.X1 = left;
                    LineStreak.Y1 = top;
                    endPointX = right;
                    endPointY = bot;
                    break;
                default:
                    LineStreak.X1 = right;
                    LineStreak.Y1 = top;
                    endPointX = left;
                    endPointY = bot;
                    break;
            }

            var animEndX = new DoubleAnimation(LineStreak.X1, endPointX, TimeSpan.FromSeconds(.33));
            var animEndY = new DoubleAnimation(LineStreak.Y1, endPointY, TimeSpan.FromSeconds(.33));

            LineStreak.BeginAnimation(Line.X2Property, animEndX);
            LineStreak.BeginAnimation(Line.Y2Property, animEndY);

            LineStreak.Visibility = Visibility.Visible;

            await Task.Delay(animEndX.Duration.TimeSpan);
            await Task.Delay(500);

            ResultText.Text = "Winner: ";
            SetPlayerPanel(ResultText, 
                WinnerImage, 
                Images.PlayerCompleteImages[gameResult.Winner], 
                gameResult.Winner, 
                r_gameState.CurrentPlayer);
        }
        else
        {
            ResultText.Text = "It\'s a tie";
            SetPlayerPanel(ResultText,
                WinnerImage,
                null,
                r_gameState.OppositePlayer,
                r_gameState.CurrentPlayer);
        }

        await Task.WhenAll(FadeOut(TurnPanel), FadeOut(GameCanvas));

        await FadeIn(EndScreen);
    }

    private async void OnGameRestarted()
    {
        // Reset board
        for (var r = 0; r < 3; r++)
            for (var c = 0; c < 3; c++)
                r_imageControls[r, c].BeginAnimation(Image.SourceProperty, null);

        // Transion to game screen
        SetPlayerPanel(
            PlayerText, 
            PlayerImage, 
            Images.PlayerCompleteImages[r_gameState.CurrentPlayer],
            r_gameState.CurrentPlayer,
            r_gameState.OppositePlayer);

        await FadeOut(EndScreen);
        LineStreak.Visibility = Visibility.Hidden;
        await Task.WhenAll(FadeIn(TurnPanel), FadeIn(GameCanvas));
    }
    #endregion

    #region WindowEvents
    private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var squareSize = GameGrid.Width / 3.0;
        var clickPosition = e.GetPosition(GameGrid);
        r_gameState.MakeMove(new(row: (int)(clickPosition.Y / squareSize),
            col: (int)(clickPosition.X / squareSize)));
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (!r_gameState.GameOver)
            return;

        r_gameState.Reset();
    }
    #endregion

    private static void SetPlayerPanel(TextBlock text,
        Image image,
        BitmapImage source, 
        Player currentPlayer,
        Player oppositePlayer)
    {
        text.Foreground = sr_playerToColour[currentPlayer];
        (text.Effect as DropShadowEffect).Color = sr_playerToColour[oppositePlayer].Color;
        image.Source = source;
    }

    private static async Task FadeOut(UIElement element)
    {
        element.BeginAnimation(OpacityProperty, sr_fadeOutAnimation);
        await Task.Delay(sr_fadeInAnimation.Duration.TimeSpan);
        element.Visibility = Visibility.Hidden;
    }

    private static async Task FadeIn(UIElement element)
    {
        element.Visibility= Visibility.Visible;
        element.BeginAnimation(OpacityProperty, sr_fadeInAnimation);
        await Task.Delay(sr_fadeInAnimation.Duration.TimeSpan);
    }
}